using Datos;
using DocumentFormat.OpenXml.InkML;
using Fonade.Account;
using Fonade.Error;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Fonade.Negocio.Proyecto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    public partial class ExportarInformacionIA : Negocio.Base_Page
    {
        OperadorController operadorController = new OperadorController();

        public string avisoVisible { get { return ViewState["AvisoVisible"].ToString(); } }
        public string colorDiv { get { return ViewState["ColorFondo"].ToString(); } }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                ValidacionCuenta validacionCuenta = new ValidacionCuenta();

                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    cargarDllOperador(usuario.CodOperador);
                    CargarDropDown_Convocatorias();
                }               
            }
        }

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDropDown_Convocatorias();
        }

        private void cargarDllOperador(int? _codOperador)
        {
            ddlOperador.DataSource = operadorController.cargaDLLOperador(_codOperador);
            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataBind();
        }

        private void CargarDropDown_Convocatorias()
        {
            //Inicializar variables.
            String txtSQL;
            DataTable tabla = new DataTable();

            try
            {
                string txtCondicionOperador = "";

                txtCondicionOperador = " where IdVersionProyecto = 2 and codOperador = " + ddlOperador.SelectedValue;

                txtSQL = " SELECT Id_Convocatoria, NomConvocatoria FROM Convocatoria " + txtCondicionOperador;

                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                ddlconvocatoria.Items.Clear();

                ListItem item_default = new ListItem();
                item_default.Value = "";
                item_default.Text = "Seleccione";
                ddlconvocatoria.Items.Add(item_default);
                item_default = null;

                foreach (DataRow row in tabla.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["Id_Convocatoria"].ToString();
                    item.Text = row["NomConvocatoria"].ToString();
                    ddlconvocatoria.Items.Add(item);
                    item = null;
                }

                txtSQL = null;
                tabla = null;
            }
            catch { }
        }

        protected void btnExportar_Click(object sender, EventArgs e)
        {
            try
            { 

                pnlAvisoOK.Visible = true;
                lblAvisoOK.Text = "Procesado Datos...";

                int codConvocatoria = Convert.ToInt32(ddlconvocatoria.SelectedValue.ToString());

                if (codConvocatoria > 0)
                {
                    string fechaArchivo = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                                   "_" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                    string rutaConvocatoria = "";
                    string nombreArchivoPlano = "";
                    int cantidadDeProyectosProcesados = 0;
                    if (crearArchivoPlano(codConvocatoria, ddlconvocatoria.SelectedItem.Text
                        , fechaArchivo, ref rutaConvocatoria, ref nombreArchivoPlano, ref cantidadDeProyectosProcesados))
                    {

                        //Copiar carpeta a SFTP
                        if (copiarArchivosGeneradosSFTP(rutaConvocatoria, ddlconvocatoria.SelectedItem.Text, fechaArchivo))
                        {
                            //eliminararchivosTemporales
                            if(eliminarArchivosTemporales(ddlconvocatoria.SelectedItem.Text))
                            {
                                Alert("Se creó el archivo plano de manera correta con nombre:" + nombreArchivoPlano
                                + " y se procesaron: " + cantidadDeProyectosProcesados + " proyectos.", true);
                            }
                            else
                            {
                                Alert("Se creó el archivo plano de manera correta con nombre:" + nombreArchivoPlano
                                + " y se procesaron: " + cantidadDeProyectosProcesados + " proyectos, pero no se lograron" +
                                "eliminar los archivos temporales.", true);
                            }
                            
                        }
                        else
                        {
                            Alert("Se creó el archivo plano, pero no se logró copiar al SFTP.", false);
                        }
                    }
                    else
                    {
                        Alert("No se logró crear el archivo plano.", false);
                    }
                }
                else
                {
                    Alert("Por favor seleccione una convocatoria válida.", false);
                }
            }
            catch (Exception ex)
            {
                Alert("Sucedió un error al crear el archivo plano. " + ex.Message, false);
            }

            //upExportar.Update();
        }

        private bool eliminarArchivosTemporales(string _nomConvocatoria)
        {
            bool eliminados = true;

            var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                        + @"ExportarArchivosIA\" + _nomConvocatoria + @"\";

            try
            {
                Directory.Delete(basePath, true);
            }
            catch (Exception ex)
            {
                eliminados = false;
            }

            return eliminados;
        }

        private void Alert(string mensaje, bool correcto)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);

            lblAvisoOK.Text = mensaje;
            pnlAvisoOK.Visible = correcto;
            pnlAvisoError.Visible = !correcto;
            pnlInfo.Visible = false;
            
            //upboton.Update();
        }

        private bool copiarArchivosGeneradosSFTP(string rutaOriginal, string nomConvocatoria, string fechaArchivo)
        {
            bool copiado = true;

            try
            {
                //Archivo txt
                string rutaSFTP = ConfigurationManager.AppSettings.Get("RutaSFTPSena") + @"\" + nomConvocatoria + @"\";

                if (!Directory.Exists(rutaSFTP))
                    Directory.CreateDirectory(rutaSFTP);

                if (Directory.Exists(rutaOriginal))
                {
                    string[] files = Directory.GetFiles(rutaOriginal);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = System.IO.Path.GetFileName(s);
                        if (fileName.Contains(fechaArchivo))
                        {
                            string destFile = System.IO.Path.Combine(rutaSFTP, fileName);
                            System.IO.File.Copy(s, destFile, true);
                        }

                    }
                }

                //Archivos Formatos Financieros

                rutaSFTP = rutaSFTP + @"FormatoFinancieros_" + fechaArchivo + @"\";
                rutaOriginal = rutaOriginal + @"FormatoFinancieros_" + fechaArchivo + @"\";

                if (!Directory.Exists(rutaSFTP))
                    Directory.CreateDirectory(rutaSFTP);

                if (Directory.Exists(rutaOriginal))
                {
                    string[] files = Directory.GetFiles(rutaOriginal);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.                        
                        string fileName = System.IO.Path.GetFileName(s);
                        string destFile = System.IO.Path.Combine(rutaSFTP, fileName);
                        System.IO.File.Copy(s, destFile, true);
                    }
                }
            }
            catch (Exception ex)
            {
                copiado = false;

                string urlEx = "SFTP";

                string mensajeEx = ex.Message.ToString();
                string dataEx = ex.Data.ToString();
                string stackTraceEx = ex.StackTrace.ToString();
                string innerExceptionEx = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                ErrHandler.WriteError(mensajeEx, urlEx, dataEx, stackTraceEx, innerExceptionEx, usuario.Email, usuario.IdContacto.ToString());
            }

            return copiado;
        }

        private bool crearArchivoPlano(int _codConvocatoria, string _nomConvocatoria
            , string fechaArchivo, ref string rutaConvocatoria, ref string nombreArchivoPlano, ref int cantidadProyectos)
        {
            bool creado = false;

            string nombreConv = _nomConvocatoria.Trim();

            nombreConv = nombreConv.Replace(" ", "").Replace(".", "");

            try
            {

                //Crear archivo plano
                var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                        + @"ExportarArchivosIA\" + _nomConvocatoria + @"\";
                string nombreArchivo = nombreConv + "_" + fechaArchivo + ".txt";

                nombreArchivoPlano = nombreArchivo;

                if (!Directory.Exists(basePath))
                    Directory.CreateDirectory(basePath);

                string finalPath = basePath + nombreArchivo;

                var listadoProyectos = ListadoProyectos(_codConvocatoria);

                cantidadProyectos = listadoProyectos.Count();

                if (!File.Exists((finalPath)))
                {
                    File.Create((finalPath)).Close();
                }
                using (StreamWriter w = File.AppendText((finalPath)))
                {
                    w.WriteLine(titulos());

                    foreach (var p in listadoProyectos)
                    {
                        //-----------------------------General-------------------------------------------------
                        var infogeneral = proyectoController.infoGeneralProyecto(p.Id_proyecto);

                        //string contenido = infogeneral.Id_proyecto + "|" + infogeneral.NomProyecto + "|" + infogeneral.CodInstitucion
                        //            + "|" + infogeneral.NomInstitucion + "|" + infogeneral.NomUnidad
                        //            + "|" + infogeneral.CodigoDane + "|" + infogeneral.NomCiudad;

                        //contenido = contenido + "|";
                        //w.Write(contenido);
                        if (infogeneral!=null)
                        {
                            w.Write(infogeneral.Id_proyecto + "|" + infogeneral.NomProyecto
                                     + "|" + infogeneral.CodigoCIIU + "|" + infogeneral.Sector + "|" + infogeneral.SubSector
                                   + "|" + infogeneral.CodInstitucion
                                   + "|" + infogeneral.NomInstitucion + "|" + infogeneral.NomUnidad
                                   + "|" + infogeneral.CodigoDane + "|" + infogeneral.NomCiudad + "|");
                        }
                        else
                        {
                            w.Write( "|" + "|" +  "|" + "|" + "|" + "|" +  "|"  + "|" + "|" + "|");
                        }
                       

                        //------------------------------Protagonista Cliente-----------------------------------
                        //contenido = "";

                        var protagonistaCliente = proyectoController.infoProtagonistaCliente(p.Id_proyecto);
                        if (protagonistaCliente!=null)
                        {
                            if (protagonistaCliente.Count > 0)
                            {
                                foreach (var pc in protagonistaCliente)
                                {
                                    //contenido = contenido + pc.Perfil + "¦" + pc.Localizacion + "¦" + pc.Justificacion + "§";
                                    w.Write(pc.Perfil + "¦" + pc.Localizacion + "¦" + pc.Justificacion);

                                    //Ultimo Registro
                                    if (!(protagonistaCliente.IndexOf(pc) == protagonistaCliente.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        
                        //contenido = contenido.TrimEnd('§');
                        //contenido = contenido + "|";
                        //w.Write(contenido);

                        //-----------------------------Protagonista----------------------------------
                        //contenido = "";

                        var infoProtagonista = proyectoController.infoProtagonista(p.Id_proyecto);

                        //contenido = infoProtagonista.PerfilConsumidor + "|" + infoProtagonista.Cliente 
                        //            + "|" + infoProtagonista.Consumidores;

                        //contenido = contenido + "|";
                        //w.Write(contenido);
                        if (infoProtagonista!=null)
                        {
                            w.Write(infoProtagonista.PerfilConsumidor + "|" + infoProtagonista.Cliente
                                   + "|" + infoProtagonista.Consumidores + "|");
                        }
                        else
                        {
                            w.Write("|" +  "|" + "|");
                        }

                        //--------------------Oportunidad de Mercado-----------------------
                        //contenido = "";

                        var infoOportunidadMercado = proyectoController.infoOportunidadMercado(p.Id_proyecto);

                        //contenido = infoOportunidadMercado.OportunidadDeMercado + "|";
                        //w.Write(contenido);
                        if (infoOportunidadMercado!=null)
                        {
                            w.Write(infoOportunidadMercado.OportunidadDeMercado + "|");
                        }
                        else
                        {
                            w.Write( "|");
                        }
                       

                        //-------------------------Oportunidad de Mercado - Competidores--------------------------------

                        var OportunidadMercadoCompetidores = proyectoController.infoOportunidadMercadoCompetidores(p.Id_proyecto);
                        if (OportunidadMercadoCompetidores!=null)
                        {
                            if (OportunidadMercadoCompetidores.Count > 0)
                            {
                                foreach (var mc in OportunidadMercadoCompetidores)
                                {
                                    w.Write(mc.Localizacion + "¦" + mc.ProductosServicios + "¦" + mc.Precios + "¦" + mc.LogisticaDistribucion + "¦" + mc.OtroCual);

                                    //Ultimo Registro
                                    if (!(OportunidadMercadoCompetidores.IndexOf(mc) == OportunidadMercadoCompetidores.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }

                        //---------------------------------Solucion - Solucion-----------------------------------
                        var infoSolucionSOlucion = proyectoController.infoSolucionSolucion(p.Id_proyecto);

                        if (infoSolucionSOlucion!=null)
                        {
                            w.Write(infoSolucionSOlucion.ConceptoNegocio + "|" + infoSolucionSOlucion.ComponenteInnovador
                              + "|" + infoSolucionSOlucion.ProductoServicio + "|" + infoSolucionSOlucion.Proceso
                              + "|" + infoSolucionSOlucion.AceptacionMercado + "|" + infoSolucionSOlucion.AspectoTecnicoProductivo
                              + "|" + infoSolucionSOlucion.AspectoComercial + "|" + infoSolucionSOlucion.AspectoLegal
                              + "|");
                        }
                        else
                        {
                            w.Write( "|" +  "|" +  "|" +  "|" + "|" + "|" +  "|" +  "|");
                        }
                       

                        //-------------------------------Solucion - Ficha Tecnica----------------------------------

                        var infoSolucionFichaTecnica = proyectoController.infoSolucionFichaTecnica(p.Id_proyecto);

                        if (infoSolucionFichaTecnica!=null)
                        {
                            if (infoSolucionFichaTecnica.Count > 0)
                            {
                                foreach (var ft in infoSolucionFichaTecnica)
                                {
                                    w.Write(ft.ProductoEspecifico + "¦" + ft.NombreComercial + "¦" + ft.UnidadDeMedida
                                        + "¦" + ft.DescripcionGeneral + "¦" + ft.CondicionesEspeciales + "¦" + ft.Composicion + "¦" + ft.Otros
                                        + "¦" + ft.ProductoMasRrepresentativo);

                                    //Ultimo Registro
                                    if (!(infoSolucionFichaTecnica.IndexOf(ft) == infoSolucionFichaTecnica.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                       

                        //----------------------------Desarrollo de Solucion - Ingresos

                        var infoDesarrolloSolucionIngresos = proyectoController.infoDesarrolloSolucionIngresos(p.Id_proyecto);

                        if (infoDesarrolloSolucionIngresos!=null)
                        {
                            w.Write(infoDesarrolloSolucionIngresos.EstrategiaIngresos + "|");
                        }
                        else
                        {
                            w.Write( "|");
                        }
                       

                        //---------------------------Desarrollo Solucion Ingresos - Condiciones Comerciales ---------------------

                        var infoDesarrolloSolucionCondicionComercial = proyectoController.infoDesarrolloSolucionCondicionComercial(p.Id_proyecto);

                        if (infoDesarrolloSolucionCondicionComercial!=null)
                        {
                            if (infoDesarrolloSolucionCondicionComercial.Count > 0)
                            {
                                foreach (var cc in infoDesarrolloSolucionCondicionComercial)
                                {
                                    w.Write(cc.VolumenesFrecuencia + "¦" + cc.CaracteristicasCompra + "¦" + cc.SitioCcompra
                                        + "¦" + cc.FormaPago + "¦" + cc.Precio + "¦" + cc.RequisitosPostVenta + "¦" + cc.Garantias
                                        + "¦" + cc.MargenComercializacion);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionCondicionComercial.IndexOf(cc) == infoDesarrolloSolucionCondicionComercial.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }



                        //----------------------------Desarrollo de Solucion - Ingresos

                        if (infoDesarrolloSolucionIngresos!=null)
                        {
                            w.Write(infoDesarrolloSolucionIngresos.DondeCompra
                                                          + "|" + infoDesarrolloSolucionIngresos.CaracteristicasParaCompra + "|" + infoDesarrolloSolucionIngresos.FrecuenciaCompra
                                                          + "|" + infoDesarrolloSolucionIngresos.Precio
                                                          + "|");
                        }
                        else
                        {
                            w.Write("|" + "|" + "|" +  "|");
                        }
                       

                        //-------------------------------Desarrollo de Solucion - Proyeccion
                        var proyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(p.Id_proyecto);

                        int _proyeccion = proyeccion.TiempoProyeccion.HasValue ?
                                            Convert.ToInt32(proyeccion.TiempoProyeccion.Value) : 0;

                        var infoIngresoPorVenta = proyectoController.infoIngresosPorVentas(p.Id_proyecto);
                        if (infoIngresoPorVenta!=null)
                        {
                            if (infoIngresoPorVenta.Count > 0)
                            {
                                foreach (var iv in infoIngresoPorVenta)
                                {
                                    w.Write(iv.Periodo);

                                    if (_proyeccion >= 1)
                                        w.Write("¦" + iv.Year1);
                                    if (_proyeccion >= 2)
                                        w.Write("¦" + iv.Year2);
                                    if (_proyeccion >= 3)
                                        w.Write("¦" + iv.Year3);
                                    if (_proyeccion >= 4)
                                        w.Write("¦" + iv.Year4);
                                    if (_proyeccion >= 5)
                                        w.Write("¦" + iv.Year5);
                                    if (_proyeccion >= 6)
                                        w.Write("¦" + iv.Year6);
                                    if (_proyeccion >= 7)
                                        w.Write("¦" + iv.Year7);
                                    if (_proyeccion >= 8)
                                        w.Write("¦" + iv.Year8);
                                    if (_proyeccion >= 9)
                                        w.Write("¦" + iv.Year9);
                                    if (_proyeccion >= 10)
                                        w.Write("¦" + iv.Year10);

                                    //Ultimo Registro
                                    if (!(infoIngresoPorVenta.IndexOf(iv) == infoIngresoPorVenta.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                       
                        //-----------------------------Desarrollo Solucion - Normatividad

                        var infoDesarrolloSolucionNormatividad = proyectoController.infoDesarrolloSolucionNormatividad(p.Id_proyecto);

                        if (infoDesarrolloSolucionNormatividad!=null)
                        {
                            w.Write(infoDesarrolloSolucionNormatividad.NormatividadEmpresarial + "|" + infoDesarrolloSolucionNormatividad.NormatividadTributaria
                               + "|" + infoDesarrolloSolucionNormatividad.NormatividadTecnica + "|" + infoDesarrolloSolucionNormatividad.NormatividadLaboral
                               + "|" + infoDesarrolloSolucionNormatividad.NormatividadAmbiental + "|" + infoDesarrolloSolucionNormatividad.RegistroMarca
                               + "|" + infoDesarrolloSolucionNormatividad.CondicionesTecnicasOperacionNegocio
                               + "|");
                        }
                        else
                        {
                            w.Write( "|" +  "|" + "|" + "|" + "|" +  "|" +  "|");
                        }
                        

                        //-----------------------------Desarrollo Solucion - Requerimientos

                        var infoDesarrolloSolucionRequerimiento = proyectoController.infoDesarrolloSolucionRequerimiento(p.Id_proyecto);

                        if (infoDesarrolloSolucionRequerimiento!=null)
                        {
                            w.Write(infoDesarrolloSolucionRequerimiento.NecesarioLugarFisico + "|" + infoDesarrolloSolucionRequerimiento.JustificacionLugarFisico
                              + "|");
                        }
                        else
                        {
                            w.Write( "|" + "|");
                        }
                       

                        //-----------------------------Desarrollo Solucion Infraestructura

                        var infoDesarrolloSolucionReqInfraestructura = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_Infraestructura_Adecuaciones);

                        if (infoDesarrolloSolucionReqInfraestructura!=null)
                        {
                            if (infoDesarrolloSolucionReqInfraestructura.Count > 0)
                            {
                                foreach (var dsi in infoDesarrolloSolucionReqInfraestructura)
                                {
                                    w.Write(dsi.Descripcion + "¦" + dsi.Cantidad
                                        + "¦" + dsi.ValorUnitario + "¦" + dsi.FuenteFinanciacion + "¦" + dsi.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqInfraestructura.IndexOf(dsi) == infoDesarrolloSolucionReqInfraestructura.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                       

                        //-----------------------------Desarrollo Solucion Maquinaria y Equipo

                        var infoDesarrolloSolucionReqMaquinariaYEquipo = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_MaquinariayEquipo);

                        if (infoDesarrolloSolucionReqMaquinariaYEquipo!=null)
                        {
                            if (infoDesarrolloSolucionReqMaquinariaYEquipo.Count > 0)
                            {
                                foreach (var dsme in infoDesarrolloSolucionReqMaquinariaYEquipo)
                                {
                                    w.Write(dsme.Descripcion + "¦" + dsme.Cantidad
                                        + "¦" + dsme.ValorUnitario + "¦" + dsme.FuenteFinanciacion + "¦" + dsme.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqMaquinariaYEquipo.IndexOf(dsme) == infoDesarrolloSolucionReqMaquinariaYEquipo.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-----------------------------Desarrollo Solucion Equipo de Computo

                        var infoDesarrolloSolucionReqEquipoComputo = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_EquiposComuniCompu);

                        if (infoDesarrolloSolucionReqEquipoComputo!=null)
                        {
                            if (infoDesarrolloSolucionReqEquipoComputo.Count > 0)
                            {
                                foreach (var dsec in infoDesarrolloSolucionReqEquipoComputo)
                                {
                                    w.Write(dsec.Descripcion + "¦" + dsec.Cantidad
                                        + "¦" + dsec.ValorUnitario + "¦" + dsec.FuenteFinanciacion + "¦" + dsec.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqEquipoComputo.IndexOf(dsec) == infoDesarrolloSolucionReqEquipoComputo.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }

                        }
                        else
                        {
                            w.Write("|");
                        }

                        //-----------------------------Desarrollo Solucion Muebles y Enseres

                        var infoDesarrolloSolucionReqMueblesEnseres = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_MueblesEnseresOtros);
                        if (infoDesarrolloSolucionReqMueblesEnseres!=null)
                        {
                            if (infoDesarrolloSolucionReqMueblesEnseres.Count > 0)
                            {
                                foreach (var dsme in infoDesarrolloSolucionReqMueblesEnseres)
                                {
                                    w.Write(dsme.Descripcion + "¦" + dsme.Cantidad
                                        + "¦" + dsme.ValorUnitario + "¦" + dsme.FuenteFinanciacion + "¦" + dsme.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqMueblesEnseres.IndexOf(dsme) == infoDesarrolloSolucionReqMueblesEnseres.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-----------------------------Desarrollo Solucion Otros

                        var infoDesarrolloSolucionReqOtros = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_Otros);
                        if (infoDesarrolloSolucionReqOtros!=null)
                        {
                            if (infoDesarrolloSolucionReqOtros.Count > 0)
                            {
                                foreach (var dso in infoDesarrolloSolucionReqOtros)
                                {
                                    w.Write(dso.Descripcion + "¦" + dso.Cantidad
                                        + "¦" + dso.ValorUnitario + "¦" + dso.FuenteFinanciacion + "¦" + dso.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqOtros.IndexOf(dso) == infoDesarrolloSolucionReqOtros.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                       

                        //-----------------------------Desarrollo Solucion Gastos Operativos

                        var infoDesarrolloSolucionReqGastosOperativos = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_GastoPreoperativos);
                        if (infoDesarrolloSolucionReqGastosOperativos!=null)
                        {
                            if (infoDesarrolloSolucionReqGastosOperativos.Count > 0)
                            {
                                foreach (var dsgo in infoDesarrolloSolucionReqGastosOperativos)
                                {
                                    w.Write(dsgo.Descripcion + "¦" + dsgo.Cantidad
                                        + "¦" + dsgo.ValorUnitario + "¦" + dsgo.FuenteFinanciacion + "¦" + dsgo.RequisitosTecnicos);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionReqGastosOperativos.IndexOf(dsgo) == infoDesarrolloSolucionReqGastosOperativos.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-----------------------------Desarrollo Solucion - Produccion

                        var infoDesarrolloSolucionProduccion = proyectoController.infoDesarrolloSolucionProduccion(p.Id_proyecto);
                        if (infoDesarrolloSolucionProduccion!=null)
                        {
                            w.Write(infoDesarrolloSolucionProduccion.DetalleCondicionesTecnicas + "|" + infoDesarrolloSolucionProduccion.ContemplaImportacion
                                                          + "|" + infoDesarrolloSolucionProduccion.JustificacionContemplaImportacion + "|" + infoDesarrolloSolucionProduccion.DetalleActivos
                                                          + "|" + infoDesarrolloSolucionProduccion.FinanciacionMayorValor
                                                           + "|");
                        }
                        else
                        {
                            w.Write( "|" +  "|" + "|" +  "|" +  "|");
                        }
                        

                        //-----------------------------Desarrollo Solucion ProcesoProduccion

                        var infoDesarrolloSolucionProcesoProduccion = proyectoController.infoDesarrolloSolucionProcesoProduccion(p.Id_proyecto);
                        if (infoDesarrolloSolucionProcesoProduccion!=null)
                        {
                            if (infoDesarrolloSolucionProcesoProduccion.Count > 0)
                            {
                                foreach (var dspp in infoDesarrolloSolucionProcesoProduccion)
                                {
                                    w.Write(dspp.NombreProducto + "¦" + dspp.ProcesoProduccionProducto);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionProcesoProduccion.IndexOf(dspp) == infoDesarrolloSolucionProcesoProduccion.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-----------------------------Desarrollo Solucion - Productividad

                        var infoDesarrolloSolucionProductividad = proyectoController.infoDesarrolloSolucionProductividad(p.Id_proyecto);
                        if (infoDesarrolloSolucionProductividad!=null)
                        {
                            w.Write(infoDesarrolloSolucionProductividad.CapacidadProductivaEmpresa
                                                           + "|");
                        }
                        else
                        {
                            w.Write( "|");
                        }
                        

                        //-----------------------------Desarrollo Solucion Perfil Emprendedor

                        var infoDesarrolloSolucionPerfilEmprendedor = proyectoController.infoDesarrolloSolucionPerfilEmprendedor(p.Id_proyecto);
                        if (infoDesarrolloSolucionPerfilEmprendedor!=null)
                        {
                            if (infoDesarrolloSolucionPerfilEmprendedor.Count > 0)
                            {
                                foreach (var sdpf in infoDesarrolloSolucionPerfilEmprendedor)
                                {
                                    w.Write(sdpf.Perfil + "¦" + sdpf.Rol);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionPerfilEmprendedor.IndexOf(sdpf) == infoDesarrolloSolucionPerfilEmprendedor.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-----------------------------Desarrollo Solucion Cargos Operacion

                        var infoDesarrolloSolucionCargosOperacion = proyectoController.infoDesarrolloSolucionCargosOperacion(p.Id_proyecto);
                        if (infoDesarrolloSolucionCargosOperacion!=null)
                        {
                            if (infoDesarrolloSolucionCargosOperacion.Count > 0)
                            {
                                foreach (var dsco in infoDesarrolloSolucionCargosOperacion)
                                {
                                    w.Write(dsco.NombreCargo + "¦" + dsco.FuncionesPrincipales
                                         + "¦" + dsco.Perfil + "¦" + dsco.ExperienciaGeneral
                                          + "¦" + dsco.ExperienciaEspecifica + "¦" + dsco.TipoContratacion
                                           + "¦" + dsco.DedicacionTiempo + "¦" + dsco.UnidadMedidaTiempo
                                            + "¦" + dsco.tiempoVinculacion + "¦" + dsco.valorRemuneracion
                                             + "¦" + dsco.otrosGastos + "¦" + dsco.valorConPrestaciones
                                              + "¦" + dsco.remuneracionPrimerAno + "¦" + dsco.valorSolicitadoFondoEmprender
                                               + "¦" + dsco.aportesEmprendedores + "¦" + dsco.ingresosPorVentas);

                                    //Ultimo Registro
                                    if (!(infoDesarrolloSolucionCargosOperacion.IndexOf(dsco) == infoDesarrolloSolucionCargosOperacion.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                       

                        //-----------------------------Futuro Negocio ----------------------------------

                        //-------------------------------Promocion

                        var infoFuturoNegocioEstrategia = proyectoController.infoFuturoNegocioEstrategia(p.Id_proyecto);
                        if (infoFuturoNegocioEstrategia!=null)
                        {
                            w.Write(infoFuturoNegocioEstrategia.NombreEstrategiaPromocion + "|" + infoFuturoNegocioEstrategia.PropositoPromocion
                               + "|");
                        }
                        else
                        {
                            w.Write( "|" + "|");
                        }
                        

                        //Actividades de promocion

                        var infoFuturoNegocioActividadesPromocion = proyectoController.infoFuturoNegocioActividadesPromocion(p.Id_proyecto);
                        if (infoFuturoNegocioActividadesPromocion!=null)
                        {
                            if (infoFuturoNegocioActividadesPromocion.Count > 0)
                            {
                                foreach (var fnap in infoFuturoNegocioActividadesPromocion)
                                {
                                    w.Write(fnap.Actividad + "¦" + fnap.RecursoRequerido
                                        + "¦" + fnap.MesDeEjecucion + "¦" + fnap.costo
                                        + "¦" + fnap.Responsable);

                                    //Ultimo Registro
                                    if (!(infoFuturoNegocioActividadesPromocion.IndexOf(fnap) == infoFuturoNegocioActividadesPromocion.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }


                        //-------------------------------Comunicacion
                        if (infoFuturoNegocioEstrategia!=null)
                        {
                            w.Write(infoFuturoNegocioEstrategia.NombreEstrategiaComunicacion + "|" + infoFuturoNegocioEstrategia.PropositoComunicacion
                               + "|");
                        }
                        else
                        {
                            w.Write( "|" + "|");
                        }
                        

                        //Actividades de Comunicacion

                        var infoFuturoNegocioActividadesComunicacion = proyectoController.infoFuturoNegocioActividadesComunicacion(p.Id_proyecto);
                        if (infoFuturoNegocioActividadesComunicacion!=null)
                        {
                            if (infoFuturoNegocioActividadesComunicacion.Count > 0)
                            {
                                foreach (var fnac in infoFuturoNegocioActividadesComunicacion)
                                {
                                    w.Write(fnac.Actividad + "¦" + fnac.RecursoRequerido
                                        + "¦" + fnac.MesDeEjecucion + "¦" + fnac.costo
                                        + "¦" + fnac.Responsable);

                                    //Ultimo Registro
                                    if (!(infoFuturoNegocioActividadesComunicacion.IndexOf(fnac) == infoFuturoNegocioActividadesComunicacion.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }


                        //--------------------------------Distribucion
                        if (infoFuturoNegocioEstrategia!=null)
                        {
                            w.Write(infoFuturoNegocioEstrategia.NombreEstrategiaDistribucion + "|" + infoFuturoNegocioEstrategia.PropositoDistribucion
                               + "|");
                        }
                        else
                        {
                            w.Write("|" + "|");
                        }
                        

                        //Actividades de Distribucion

                        var infoFuturoNegocioActividadesDistribucion = proyectoController.infoFuturoNegocioActividadesDistribucion(p.Id_proyecto);
                        if (infoFuturoNegocioActividadesDistribucion!=null)
                        {
                            if (infoFuturoNegocioActividadesDistribucion.Count > 0)
                            {
                                foreach (var fnad in infoFuturoNegocioActividadesDistribucion)
                                {
                                    w.Write(fnad.Actividad + "¦" + fnad.RecursoRequerido
                                        + "¦" + fnad.MesDeEjecucion + "¦" + fnad.costo
                                        + "¦" + fnad.Responsable);

                                    //Ultimo Registro
                                    if (!(infoFuturoNegocioActividadesDistribucion.IndexOf(fnad) == infoFuturoNegocioActividadesDistribucion.Count - 1))
                                    {
                                        w.Write("§");
                                    }
                                    else
                                    {
                                        w.Write("|");
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|");
                            }
                        }
                        else
                        {
                            w.Write("|");
                        }
                        

                        //-------------------------------Futuro del Negocio - Periodo de Arranque

                        var infoFuturoNegocioPeriodoArranque = proyectoController.infoFuturoNegocioPeriodoArranque(p.Id_proyecto);
                        if (infoFuturoNegocioPeriodoArranque!=null)
                        {
                            w.Write(infoFuturoNegocioPeriodoArranque.PeriodoArranque + "|" + infoFuturoNegocioPeriodoArranque.PeriodoImproductivo
                              + "|");
                        }
                        else
                        {
                            w.Write( "|" +  "|");
                        }
                       

                        //-------------------------------Riesgos

                        var infoRiesgos = proyectoController.infoRiesgos(p.Id_proyecto);

                        if (infoRiesgos!=null)
                        {
                            w.Write(infoRiesgos.ActoresExternosCriticos + "|" + infoRiesgos.FactoresExternosAfectanOperacion
                                                           + "|");
                        }
                        else
                        {
                            w.Write( "|" + "|");
                        }
                        

                        //-------------------------------Resumen Ejecutivo

                        var infoResumenEjecutivo = proyectoController.infoResumenEjecutivo(p.Id_proyecto);
                        if (infoResumenEjecutivo!=null)
                        {
                            w.Write(infoResumenEjecutivo.ConceptoNegocio + "|" + infoResumenEjecutivo.empleos
                                                        + "|" + infoResumenEjecutivo.contrapartidas + "|" + infoResumenEjecutivo.ejecucionPresupuestal
                                                        + "|" + infoResumenEjecutivo.ventas + "|" + infoResumenEjecutivo.mercadeo
                                                        + "|" + infoResumenEjecutivo.periodoImproductivo + "|" + infoResumenEjecutivo.IDH
                                                        + "|" + infoResumenEjecutivo.RecursosAportadosEmprendedor + "|" + infoResumenEjecutivo.VideoEmprendedor
                                                           );
                        }
                        else
                        {
                            w.Write( "|" + "|" + "|" + "|" + "|" +  "|" + "|" + "|" +  "|" );
                        }
                        

                        //contenido = contenido + System.Environment.NewLine;
                        w.Write(System.Environment.NewLine);
                    }

                    w.Flush();
                    w.Close();
                }
                creado = true;

                rutaConvocatoria = basePath;

                //Copiar los formatos financieros
                copiarFormatosFinancieros(listadoProyectos, basePath, fechaArchivo);

            }
            catch (Exception ex)
            {
                creado = false;

                string urlEx = "SFTP";

                string mensajeEx = ex.Message.ToString();
                string dataEx = ex.Data.ToString();
                string stackTraceEx = ex.StackTrace.ToString();
                string innerExceptionEx = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                ErrHandler.WriteError(mensajeEx, urlEx, dataEx, stackTraceEx, innerExceptionEx, usuario.Email, usuario.IdContacto.ToString());
            }

            return creado;
        }

        private void copiarFormatosFinancieros(List<InfoGeneralProyecto> proyectos, string rutaBase, string fecha)
        {
            string rutaFinal = rutaBase + @"FormatoFinancieros_" + fecha + @"\";

            if (!Directory.Exists(rutaFinal))
                Directory.CreateDirectory(rutaFinal);

            foreach (var p in proyectos)
            {
                string rutaOrigen = "";
                if (File.Exists(ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                    + @"Proyecto\" + Math.Abs(Convert.ToInt32(p.Id_proyecto) / 2000) + @"\Proyecto_"
                            + p.Id_proyecto + @"\FORMATOSFINANCIEROS" + p.Id_proyecto + ".xls"))
                {
                    rutaOrigen = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                                + @"Proyecto\" + Math.Abs(Convert.ToInt32(p.Id_proyecto) / 2000) + @"\Proyecto_"
                                + p.Id_proyecto + @"\";

                    if (Directory.Exists(rutaOrigen))
                    {
                        string fileName = "FORMATOSFINANCIEROS" + p.Id_proyecto + ".xls";
                        string destFile = Path.Combine(rutaFinal, fileName);
                        string origFile = Path.Combine(rutaOrigen, fileName);
                        System.IO.File.Copy(origFile, destFile, true);
                    }
                }

            }
        }


        ProyectoController proyectoController = new ProyectoController();

        private List<InfoGeneralProyecto> ListadoProyectos(int _codConvocatoria)
        {
            return proyectoController.ListadoProyectosPorConvocatoria(_codConvocatoria);
        }

        private string titulos()
        {
            return "IdProyecto|Nombre proyecto|Codigo CIIU|Sector|Subsector|IdInstitucion|Nombre de institución" +
                "|Nombre de unidad de emprendimiento|IdMunicipio|Nombre Municipio|Cliente|Perfil Consumidor|Cliente|Consumidores" +
                "|Oportunidad de mercado|Competidores|Concepto del negocio|Componente innovador|Producto o servicio|Proceso" +
                "|Aceptación en el mercado|Aspecto técnico productivo|Aspecto comercial|Aspecto Legal|Producto|Estrategia de ingresos" +
                "|Condiciones comerciales|Dónde compra|Características para la compra|Cual es la frecuencia de compra" +
                "|Precio" +
                "|Ingresos por venta|Normatividad empresarial|Normatividad tributaria|Normatividad técnica|Normatividad Laboral" +
                "|Normatividad ambiental|Registro de marca|Condiciones técnicas para operación del negocio" +
                "|Es necesario lugar físico|Justificación lugar físico" +
                "|Infraestructura adecuaciones|Maquinaria y equipos|Equipo de comunicación y computación|Muebles y enseres y otros" +
                "|Otros|Gastos preoperativos|Detalle de las condiciones técnicas|Contempla importación" +
                "|Justificación contempla importación" +
                "|Detalle de los activos|Financiación mayor valor|Proceso de producción|Capacidad productiva de la empresa" +
                "|Perfil emprendedor|Cargos Operación|Nombre estrategia promoción|Propósito promoción|Actividades de promoción" +
                "|Nombre estrategia comunicación|Propósito estrategia de comunicación|Actividades de comunicación|Nombre estrategia distribución" +
                "|Propósito estrategia de distribución|Actividades de distribución|Periodo de arranque|Periodo improductivo|Actores externos críticos" +
                "|Factores externos que afectan la operación|Concepto del negocio|Empleos|Contrapartidas|Ejecución presupuestal|Ventas" +
                "|Mercadeo|Periodo improductivo|IDH|Recursos aportados emprendedor|Video del emprendedor";
        }

        protected void btnMostrarConfirmacion_Click(object sender, EventArgs e)
        {
            int codConvocatoria = 0;
            if (ddlconvocatoria.SelectedValue.ToString() != "")
            {
                codConvocatoria = Convert.ToInt32(ddlconvocatoria.SelectedValue.ToString());
            }
            
            if (codConvocatoria>0)
            {
                lblPanelInfo.Text = "¿Está seguro de exportar la informacion de la convocatoria " + ddlconvocatoria.SelectedItem.Text + "?" +
               ", este proceso puede tardar varios minutos, por favor no cierre la ventana.";

                pnlInfo.Visible = true;

                pnlAvisoError.Visible = false;
                pnlAvisoOK.Visible = false;

                upExportar.Update();
            }
            else
            {
                Alert("Por favor seleccione una convocatoria.",false);
            }
        }
    }
}