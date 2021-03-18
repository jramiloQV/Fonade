using Datos;
using Fonade.Account;
using Fonade.Error;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;
using Fonade.Negocio.Proyecto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    public partial class ExportarInformacionHIS : Negocio.Base_Page
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

        protected void btnMostrarConfirmacion_Click(object sender, EventArgs e)
        {

            int codConvocatoria = 0;
            if (ddlconvocatoria.SelectedValue.ToString() != "")
            {
                codConvocatoria = Convert.ToInt32(ddlconvocatoria.SelectedValue.ToString());
            }

            if (codConvocatoria > 0)
            {
                lblPanelInfo.Text = "¿Está seguro de exportar la información histórica de la convocatoria "
                    + ddlconvocatoria.SelectedItem.Text + "?" +
               ", este proceso puede tardar varios minutos, por favor no cierre la ventana.";

                pnlInfo.Visible = true;

                pnlAvisoError.Visible = false;
                pnlAvisoOK.Visible = false;

                upExportar.Update();
            }
            else
            {
                Alert("Por favor seleccione una convocatoria.", false);
            }
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
                    string tipoArchivo = ddlEtapa.SelectedValue;
                    if (crearArchivoPlano(codConvocatoria, ddlconvocatoria.SelectedItem.Text
                        , fechaArchivo, tipoArchivo, ref rutaConvocatoria, ref nombreArchivoPlano, ref cantidadDeProyectosProcesados))
                    {

                        //Copiar carpeta a SFTP
                        if (copiarArchivosGeneradosSFTP(rutaConvocatoria, ddlconvocatoria.SelectedItem.Text, fechaArchivo, tipoArchivo))
                        {
                            //eliminararchivosTemporales
                            if (eliminarArchivosTemporales(ddlconvocatoria.SelectedItem.Text))
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
        }

        private bool crearArchivoPlano(int _codConvocatoria, string _nomConvocatoria
           , string fechaArchivo, string tipoArchivo, ref string rutaConvocatoria, ref string nombreArchivoPlano, ref int cantidadProyectos)
        {
            bool creado = false;

            string nombreConv = _nomConvocatoria.Trim();

            nombreConv = nombreConv.Replace(" ", "").Replace(".", "");

            try
            {
                //Archivo de Formulacion
                if (tipoArchivo == "FOR")
                {
                    //Crear archivo plano
                    var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                            + @"ExportarArchivosHIS\" + _nomConvocatoria + @"\";
                    string nombreArchivo = "FORMULACION_" + nombreConv + "_" + fechaArchivo + ".txt";

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
                            if (infogeneral != null)
                            {
                                w.Write(infogeneral.Id_proyecto + "|" + infogeneral.NomProyecto
                                         + "|" + infogeneral.CodigoCIIU + "|" + infogeneral.Sector + "|" + infogeneral.SubSector
                                       + "|" + infogeneral.CodInstitucion
                                       + "|" + infogeneral.NomInstitucion + "|" + infogeneral.NomUnidad
                                       + "|" + infogeneral.CodigoDane + "|" + infogeneral.NomCiudad + "|" + infogeneral.fechaNacimiento
                                       + "|" + infogeneral.genero + "|" + infogeneral.nivelEstudio + "|" + infogeneral.Programa
                                       + "|" + infogeneral.Institucion + "|" + infogeneral.Estado + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                            }


                            //------------------------------Protagonista Cliente-----------------------------------
                            //contenido = "";

                            var protagonistaCliente = proyectoController.infoProtagonistaCliente(p.Id_proyecto);
                            if (protagonistaCliente != null)
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
                            if (infoProtagonista != null)
                            {
                                w.Write(infoProtagonista.PerfilConsumidor + "|" + infoProtagonista.Cliente
                                       + "|" + infoProtagonista.Consumidores + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }

                            //--------------------Oportunidad de Mercado-----------------------
                            //contenido = "";

                            var infoOportunidadMercado = proyectoController.infoOportunidadMercado(p.Id_proyecto);

                            //contenido = infoOportunidadMercado.OportunidadDeMercado + "|";
                            //w.Write(contenido);
                            if (infoOportunidadMercado != null)
                            {
                                w.Write(infoOportunidadMercado.OportunidadDeMercado + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }


                            //-------------------------Oportunidad de Mercado - Competidores--------------------------------

                            var OportunidadMercadoCompetidores = proyectoController.infoOportunidadMercadoCompetidores(p.Id_proyecto);
                            if (OportunidadMercadoCompetidores != null)
                            {
                                if (OportunidadMercadoCompetidores.Count > 0)
                                {
                                    foreach (var mc in OportunidadMercadoCompetidores)
                                    {
                                        w.Write(mc.Nombre + "¦" + mc.Localizacion + "¦" + mc.ProductosServicios + "¦" + mc.Precios
                                            + "¦" + mc.LogisticaDistribucion + "¦" + mc.OtroCual);

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

                            if (infoSolucionSOlucion != null)
                            {
                                w.Write(infoSolucionSOlucion.ConceptoNegocio + "|" + infoSolucionSOlucion.ComponenteInnovador
                                  + "|" + infoSolucionSOlucion.ProductoServicio + "|" + infoSolucionSOlucion.Proceso
                                  + "|" + infoSolucionSOlucion.AceptacionMercado + "|" + infoSolucionSOlucion.AspectoTecnicoProductivo
                                  + "|" + infoSolucionSOlucion.AspectoComercial + "|" + infoSolucionSOlucion.AspectoLegal
                                  + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                            }


                            //-------------------------------Solucion - Ficha Tecnica----------------------------------

                            var infoSolucionFichaTecnica = proyectoController.infoSolucionFichaTecnica(p.Id_proyecto);

                            if (infoSolucionFichaTecnica != null)
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

                            if (infoDesarrolloSolucionIngresos != null)
                            {
                                w.Write(infoDesarrolloSolucionIngresos.EstrategiaIngresos + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }


                            //---------------------------Desarrollo Solucion Ingresos - Condiciones Comerciales ---------------------

                            var infoDesarrolloSolucionCondicionComercial = proyectoController.infoDesarrolloSolucionCondicionComercial(p.Id_proyecto);

                            if (infoDesarrolloSolucionCondicionComercial != null)
                            {
                                if (infoDesarrolloSolucionCondicionComercial.Count > 0)
                                {
                                    foreach (var cc in infoDesarrolloSolucionCondicionComercial)
                                    {
                                        w.Write(cc.Cliente + "¦" + cc.VolumenesFrecuencia + "¦" + cc.CaracteristicasCompra + "¦" + cc.SitioCcompra
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

                            if (infoDesarrolloSolucionIngresos != null)
                            {
                                w.Write(infoDesarrolloSolucionIngresos.DondeCompra
                                                              + "|" + infoDesarrolloSolucionIngresos.CaracteristicasParaCompra + "|" + infoDesarrolloSolucionIngresos.FrecuenciaCompra
                                                              + "|" + infoDesarrolloSolucionIngresos.Precio
                                                              + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|");
                            }


                            //-------------------------------Desarrollo de Solucion - Proyeccion

                            //---------------------------------Listado de productos
                            var infoDesarrolloSolucionlistadoProductos = proyectoController.infoDesarrolloSolucionProyeccionListadoProductos(p.Id_proyecto);

                            if (infoDesarrolloSolucionlistadoProductos != null)
                            {
                                if (infoDesarrolloSolucionlistadoProductos.Count > 0)
                                {
                                    foreach (var cc in infoDesarrolloSolucionlistadoProductos)
                                    {
                                        w.Write(cc.NomProduto + "¦" + cc.UnidadMedida + "¦" + cc.FormaDePago + "¦" + cc.Justificacion
                                            + "¦" + cc.IVA);

                                        //Ultimo Registro
                                        if (!(infoDesarrolloSolucionlistadoProductos.IndexOf(cc) == infoDesarrolloSolucionlistadoProductos.Count - 1))
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

                            //---------------------------------Ingresos por venta
                            var proyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(p.Id_proyecto);

                            int _proyeccion = proyeccion.TiempoProyeccion.HasValue ?
                                                Convert.ToInt32(proyeccion.TiempoProyeccion.Value) : 0;

                            var infoIngresoPorVenta = proyectoController.infoIngresosPorVentas(p.Id_proyecto);
                            if (infoIngresoPorVenta != null)
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

                            if (infoDesarrolloSolucionNormatividad != null)
                            {
                                w.Write(infoDesarrolloSolucionNormatividad.NormatividadEmpresarial + "|" + infoDesarrolloSolucionNormatividad.NormatividadTributaria
                                   + "|" + infoDesarrolloSolucionNormatividad.NormatividadTecnica + "|" + infoDesarrolloSolucionNormatividad.NormatividadLaboral
                                   + "|" + infoDesarrolloSolucionNormatividad.NormatividadAmbiental + "|" + infoDesarrolloSolucionNormatividad.RegistroMarca
                                   + "|" + infoDesarrolloSolucionNormatividad.CondicionesTecnicasOperacionNegocio
                                   + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|");
                            }


                            //-----------------------------Desarrollo Solucion - Requerimientos

                            var infoDesarrolloSolucionRequerimiento = proyectoController.infoDesarrolloSolucionRequerimiento(p.Id_proyecto);

                            if (infoDesarrolloSolucionRequerimiento != null)
                            {
                                w.Write(infoDesarrolloSolucionRequerimiento.NecesarioLugarFisico + "|" + infoDesarrolloSolucionRequerimiento.JustificacionLugarFisico
                                  + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }


                            //-----------------------------Desarrollo Solucion Infraestructura

                            var infoDesarrolloSolucionReqInfraestructura = proyectoController.infoDesarrolloSolucionReqInversion(p.Id_proyecto, Constantes.CONST_Infraestructura_Adecuaciones);

                            if (infoDesarrolloSolucionReqInfraestructura != null)
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

                            if (infoDesarrolloSolucionReqMaquinariaYEquipo != null)
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

                            if (infoDesarrolloSolucionReqEquipoComputo != null)
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
                            if (infoDesarrolloSolucionReqMueblesEnseres != null)
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
                            if (infoDesarrolloSolucionReqOtros != null)
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
                            if (infoDesarrolloSolucionReqGastosOperativos != null)
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
                            if (infoDesarrolloSolucionProduccion != null)
                            {
                                w.Write(infoDesarrolloSolucionProduccion.DetalleCondicionesTecnicas + "|" + infoDesarrolloSolucionProduccion.ContemplaImportacion
                                                              + "|" + infoDesarrolloSolucionProduccion.JustificacionContemplaImportacion + "|" + infoDesarrolloSolucionProduccion.DetalleActivos
                                                              + "|" + infoDesarrolloSolucionProduccion.FinanciacionMayorValor
                                                               + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|");
                            }


                            //-----------------------------Desarrollo Solucion ProcesoProduccion

                            var infoDesarrolloSolucionProcesoProduccion = proyectoController.infoDesarrolloSolucionProcesoProduccion(p.Id_proyecto);
                            if (infoDesarrolloSolucionProcesoProduccion != null)
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
                            if (infoDesarrolloSolucionProductividad != null)
                            {
                                w.Write(infoDesarrolloSolucionProductividad.CapacidadProductivaEmpresa
                                                               + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }


                            //-----------------------------Desarrollo Solucion Perfil Emprendedor

                            var infoDesarrolloSolucionPerfilEmprendedor = proyectoController.infoDesarrolloSolucionPerfilEmprendedor(p.Id_proyecto);
                            if (infoDesarrolloSolucionPerfilEmprendedor != null)
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
                            if (infoDesarrolloSolucionCargosOperacion != null)
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
                            if (infoFuturoNegocioEstrategia != null)
                            {
                                w.Write(infoFuturoNegocioEstrategia.NombreEstrategiaPromocion + "|" + infoFuturoNegocioEstrategia.PropositoPromocion
                                   + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }


                            //Actividades de promocion

                            var infoFuturoNegocioActividadesPromocion = proyectoController.infoFuturoNegocioActividadesPromocion(p.Id_proyecto);
                            if (infoFuturoNegocioActividadesPromocion != null)
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
                            if (infoFuturoNegocioEstrategia != null)
                            {
                                w.Write(infoFuturoNegocioEstrategia.NombreEstrategiaComunicacion + "|" + infoFuturoNegocioEstrategia.PropositoComunicacion
                                   + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }


                            //Actividades de Comunicacion

                            var infoFuturoNegocioActividadesComunicacion = proyectoController.infoFuturoNegocioActividadesComunicacion(p.Id_proyecto);
                            if (infoFuturoNegocioActividadesComunicacion != null)
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
                            if (infoFuturoNegocioEstrategia != null)
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
                            if (infoFuturoNegocioActividadesDistribucion != null)
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
                            if (infoFuturoNegocioPeriodoArranque != null)
                            {
                                w.Write(infoFuturoNegocioPeriodoArranque.PeriodoArranque + "|" + infoFuturoNegocioPeriodoArranque.PeriodoImproductivo
                                  + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }


                            //-------------------------------Riesgos

                            var infoRiesgos = proyectoController.infoRiesgos(p.Id_proyecto);

                            if (infoRiesgos != null)
                            {
                                w.Write(infoRiesgos.ActoresExternosCriticos + "|" + infoRiesgos.FactoresExternosAfectanOperacion
                                                               + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }


                            //-------------------------------Resumen Ejecutivo

                            var infoResumenEjecutivo = proyectoController.infoResumenEjecutivo(p.Id_proyecto);
                            if (infoResumenEjecutivo != null)
                            {
                                w.Write(infoResumenEjecutivo.ConceptoNegocio + "|" + infoResumenEjecutivo.empleos
                                                            + "|" + infoResumenEjecutivo.contrapartidas + "|" + infoResumenEjecutivo.ejecucionPresupuestal
                                                            + "|" + infoResumenEjecutivo.ventas + "|" + infoResumenEjecutivo.mercadeo
                                                            + "|" + infoResumenEjecutivo.periodoImproductivo + "|" + infoResumenEjecutivo.IDH
                                                            + "|" + infoResumenEjecutivo.RecursosAportadosEmprendedor + "|" + infoResumenEjecutivo.VideoEmprendedor
                                                            + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                            }

                            //Estructura financiera - Plan de compras

                            var estructuraFinancieraPlanDeCompras = proyectoController.estructuraFinancieraPlanDeCompra(p.Id_proyecto);
                            if (estructuraFinancieraPlanDeCompras != null)
                            {
                                if (estructuraFinancieraPlanDeCompras.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraPlanDeCompras)
                                    {
                                        w.Write(fnad.TipoMateriaPrima + "¦" + fnad.MateriaPrima
                                            + "¦" + fnad.Unidad + "¦" + fnad.CantidadPresentacion
                                            + "¦" + fnad.MargenDesperdicio);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraPlanDeCompras.IndexOf(fnad) == estructuraFinancieraPlanDeCompras.Count - 1))
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

                            //Estructura financiera - Tabla de costos de producción en pesos

                            var estructuraFinancieraTablaCostrosProduccion = proyectoController.estructuraFinancieraTablaCostosProduccion(p.Id_proyecto);
                            if (estructuraFinancieraTablaCostrosProduccion != null)
                            {
                                if (estructuraFinancieraPlanDeCompras.Count > 0)
                                {


                                    //Seleccionamos los insumos
                                    var listInsumos = estructuraFinancieraTablaCostrosProduccion.Select(x => x.TipoInsumo).Distinct().ToList();

                                    foreach (var insumo in listInsumos)
                                    {
                                        //Seleccionamos los valores por insumo
                                        var valores = estructuraFinancieraTablaCostrosProduccion.Where(x => x.TipoInsumo.Equals(insumo)).ToList();
                                        string nombreInsumo = valores.Select(x => x.TipoInsumo).FirstOrDefault();

                                        int MaxAno = valores.Max(x => x.Ano);

                                        w.Write(nombreInsumo + "¦");

                                        for (int i = 1; i <= MaxAno; i++)
                                        {
                                            var valorInsumo = valores.Where(x => x.Ano == i).FirstOrDefault();

                                            w.Write(valorInsumo.Valor);

                                            if (i < MaxAno)
                                                w.Write("¦");
                                        }

                                        //Ultimo Registro
                                        if (!(listInsumos.IndexOf(insumo) == listInsumos.Count - 1))
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

                            //Estructura financiera - Proyección de Compras (Unidades)

                            var estructuraFinancieraTablaProyeccionComprasUnidades = proyectoController.estructuraFinancieraTablaProyeccionComprasUnidades(p.Id_proyecto);
                            if (estructuraFinancieraTablaProyeccionComprasUnidades != null)
                            {
                                if (estructuraFinancieraTablaProyeccionComprasUnidades.Count > 0)
                                {

                                    //Seleccionamos los insumos
                                    var listInsumos = estructuraFinancieraTablaProyeccionComprasUnidades.Select(x => x.Insumo).Distinct().ToList();

                                    foreach (var insumo in listInsumos)
                                    {
                                        //Seleccionamos los valores por insumo
                                        var valores = estructuraFinancieraTablaProyeccionComprasUnidades.Where(x => x.Insumo.Equals(insumo)).ToList();

                                        string nombreInsumo = valores.Select(x => x.Insumo).FirstOrDefault();
                                        string tipoInsumo = valores.Select(x => x.TipoInsumo).FirstOrDefault();

                                        int MaxAno = valores.Max(x => x.ano);

                                        w.Write(tipoInsumo + "¦");
                                        w.Write(nombreInsumo + "¦");

                                        for (int i = 1; i <= MaxAno; i++)
                                        {
                                            var valorInsumo = valores.Where(x => x.ano == i).FirstOrDefault();

                                            w.Write(valorInsumo.Valor);

                                            if (i < MaxAno)
                                                w.Write("¦");
                                        }

                                        //Ultimo Registro
                                        if (!(listInsumos.IndexOf(insumo) == listInsumos.Count - 1))
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

                            //Estructura financiera - Proyección de Compras (Pesos)

                            var estructuraFinancieraTablaProyeccionComprasPesos = proyectoController.estructuraFinancieraTablaProyeccionComprasPesos(p.Id_proyecto);
                            if (estructuraFinancieraTablaProyeccionComprasPesos != null)
                            {
                                if (estructuraFinancieraTablaProyeccionComprasPesos.Count > 0)
                                {

                                    //Seleccionamos los insumos
                                    var listInsumos = estructuraFinancieraTablaProyeccionComprasPesos.Select(x => x.Insumo).Distinct().ToList();

                                    foreach (var insumo in listInsumos)
                                    {
                                        //Seleccionamos los valores por insumo
                                        var valores = estructuraFinancieraTablaProyeccionComprasPesos.Where(x => x.Insumo.Equals(insumo)).ToList();

                                        string nombreInsumo = valores.Select(x => x.Insumo).FirstOrDefault();
                                        string tipoInsumo = valores.Select(x => x.TipoInsumo).FirstOrDefault();

                                        int MaxAno = valores.Max(x => x.ano);

                                        w.Write(tipoInsumo + "¦");
                                        w.Write(nombreInsumo + "¦");

                                        for (int i = 1; i <= MaxAno; i++)
                                        {
                                            var valorInsumo = valores.Where(x => x.ano == i).FirstOrDefault();

                                            w.Write(valorInsumo.Valor);

                                            if (i < MaxAno)
                                                w.Write("¦");
                                        }

                                        //Ultimo Registro
                                        if (!(listInsumos.IndexOf(insumo) == listInsumos.Count - 1))
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

                            //Estructura financiera - Gastos de Apuesta en Marcha

                            var estructuraFinancieraPuestaMarcha = proyectoController.estructuraFinancieraGastosPuestaMarcha(p.Id_proyecto);
                            if (estructuraFinancieraPuestaMarcha != null)
                            {
                                if (estructuraFinancieraPuestaMarcha.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraPuestaMarcha)
                                    {
                                        w.Write(fnad.Descripcion + "¦" + fnad.Valor);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraPuestaMarcha.IndexOf(fnad) == estructuraFinancieraPuestaMarcha.Count - 1))
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

                            //Estructura financiera - Gastos Anuales de Administración

                            var estructuraFinancieraGastosAnualesAdmin = proyectoController.estructuraFinancieraGastosAnuales(p.Id_proyecto);
                            if (estructuraFinancieraGastosAnualesAdmin != null)
                            {
                                if (estructuraFinancieraGastosAnualesAdmin.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraGastosAnualesAdmin)
                                    {
                                        w.Write(fnad.Descripcion + "¦" + fnad.Valor);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraGastosAnualesAdmin.IndexOf(fnad) == estructuraFinancieraGastosAnualesAdmin.Count - 1))
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

                            //-------------------------------RecursosSolicitado Fondo emprender

                            var infoRecursoSolicitadoFondoEmprender = proyectoController.RecursosSolicitadoFondoEmprender(p.Id_proyecto);
                            if (infoRecursoSolicitadoFondoEmprender != null)
                            {
                                w.Write(infoRecursoSolicitadoFondoEmprender + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //Estructura Financiera - Ingresos - Aporte de los Emprendedores

                            var estructuraFinancieraAporteEmprendedores = proyectoController.estructuraFinancieraAporteEmprendedores(p.Id_proyecto);
                            if (estructuraFinancieraAporteEmprendedores != null)
                            {
                                if (estructuraFinancieraAporteEmprendedores.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraAporteEmprendedores)
                                    {
                                        w.Write(fnad.Nombre + "¦" + fnad.Valor + "¦" + fnad.Descripcion);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraAporteEmprendedores.IndexOf(fnad) == estructuraFinancieraAporteEmprendedores.Count - 1))
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

                            //Estructura Financiera - Ingresos - Recursos de Capital

                            var estructuraFinancieraRecursosCapital = proyectoController.estructuraFinancieraRecursoCapital(p.Id_proyecto);
                            if (estructuraFinancieraRecursosCapital != null)
                            {
                                if (estructuraFinancieraRecursosCapital.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraRecursosCapital)
                                    {
                                        w.Write(fnad.Cuantia + "¦" + fnad.Plazo + "¦" + fnad.FormaPago + "¦" + fnad.Interes
                                            + "¦" + fnad.Destinacion);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraRecursosCapital.IndexOf(fnad) == estructuraFinancieraRecursosCapital.Count - 1))
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

                            //Estructura Financiera - Ingresos - Proyeccion de Ingresos por Ventas

                            var estructuraFinancieraProyeccionIngresoVentas = proyectoController.estructuraFinancieraProyeccionIngresosVentas(p.Id_proyecto);
                            if (estructuraFinancieraProyeccionIngresoVentas != null)
                            {
                                if (estructuraFinancieraProyeccionIngresoVentas.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraProyeccionIngresoVentas)
                                    {
                                        w.Write(fnad.Dato + "¦" + fnad.ano_1 + "¦" + fnad.ano_2 + "¦" + fnad.ano_3
                                            + "¦" + fnad.ano_4 + "¦" + fnad.ano_5 + "¦" + fnad.ano_6 + "¦" + fnad.ano_7
                                            + "¦" + fnad.ano_8 + "¦" + fnad.ano_9 + "¦" + fnad.ano_10);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraProyeccionIngresoVentas.IndexOf(fnad) == estructuraFinancieraProyeccionIngresoVentas.Count - 1))
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

                            //-------------------------------Índice de Actualización monetaria

                            var infoIndiceActuaMonetaria = proyectoController.EstructuraFinancieraIndiceActuaMonetaria(p.Id_proyecto);
                            if (infoIndiceActuaMonetaria != null)
                            {
                                w.Write(infoIndiceActuaMonetaria + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //Estructura Financiera - Egresos - Inversiones Fijas y Diferidadas

                            var estructuraFinancieraInversionesFijasDiferidas = proyectoController.estructuraFinancieraInversionesFijasDiferidas(p.Id_proyecto);
                            if (estructuraFinancieraInversionesFijasDiferidas != null)
                            {
                                if (estructuraFinancieraInversionesFijasDiferidas.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraInversionesFijasDiferidas)
                                    {
                                        w.Write(fnad.Concepto + "¦" + fnad.valor + "¦" + fnad.mes + "¦" + fnad.FuenteFinanciacion);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraInversionesFijasDiferidas.IndexOf(fnad) == estructuraFinancieraInversionesFijasDiferidas.Count - 1))
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

                            //Estructura Financiera - Egresos - Costo de Puesta en Marcha

                            var estructuraFinancieraCostoPuestaMarcha = proyectoController.estructuraFinancieraCostoPuestaEnMarcha(p.Id_proyecto);
                            if (estructuraFinancieraCostoPuestaMarcha != null)
                            {
                                if (estructuraFinancieraCostoPuestaMarcha.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraCostoPuestaMarcha)
                                    {
                                        w.Write(fnad.Descripcion + "¦" + fnad.Valor);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraCostoPuestaMarcha.IndexOf(fnad) == estructuraFinancieraCostoPuestaMarcha.Count - 1))
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

                            //Estructura Financiera - Egresos - Costos Anualizados Administrativos

                            var estructuraFinancieraCostoAnualAdmin = proyectoController.estructuraFinancieraCostosAnualesAdmin(p.Id_proyecto);
                            if (estructuraFinancieraCostoAnualAdmin != null)
                            {
                                if (estructuraFinancieraCostoAnualAdmin.Rows.Count > 0)
                                {
                                    foreach (DataRow row in estructuraFinancieraCostoAnualAdmin.Rows)
                                    {
                                        foreach (DataColumn column in estructuraFinancieraCostoAnualAdmin.Columns)
                                        {
                                            w.Write(row[column]);
                                            if (!(estructuraFinancieraCostoAnualAdmin.Columns.IndexOf(column) == estructuraFinancieraCostoAnualAdmin.Columns.Count - 1))
                                            {
                                                w.Write("¦");
                                            }
                                        }

                                        if (!(estructuraFinancieraCostoAnualAdmin.Rows.IndexOf(row) == estructuraFinancieraCostoAnualAdmin.Rows.Count - 1))
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

                            //Estructura Financiera - Egresos - Gastos de Personal

                            var estructuraFinancieraGastosPersonal = proyectoController.estructuraFinancieraGastosDePersonal(p.Id_proyecto);
                            if (estructuraFinancieraGastosPersonal != null)
                            {
                                if (estructuraFinancieraGastosPersonal.Rows.Count > 0)
                                {
                                    foreach (DataRow row in estructuraFinancieraGastosPersonal.Rows)
                                    {
                                        foreach (DataColumn column in estructuraFinancieraGastosPersonal.Columns)
                                        {
                                            w.Write(row[column]);
                                            if (!(estructuraFinancieraGastosPersonal.Columns.IndexOf(column) == estructuraFinancieraGastosPersonal.Columns.Count - 1))
                                            {
                                                w.Write("¦");
                                            }
                                        }

                                        if (!(estructuraFinancieraGastosPersonal.Rows.IndexOf(row) == estructuraFinancieraGastosPersonal.Rows.Count - 1))
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

                            //Estructura Financiera - Capital de Trabajo

                            var estructuraFinancieraCapitalTrabajo = proyectoController.estructuraFinancieraCapitalTrabajo(p.Id_proyecto);
                            if (estructuraFinancieraCapitalTrabajo != null)
                            {
                                if (estructuraFinancieraCapitalTrabajo.Count > 0)
                                {
                                    foreach (var fnad in estructuraFinancieraCapitalTrabajo)
                                    {
                                        w.Write(fnad.Componente + "¦" + fnad.Valor + "¦"
                                            + fnad.FuenteFinanciacion + "¦" + fnad.Observacion);

                                        //Ultimo Registro
                                        if (!(estructuraFinancieraCapitalTrabajo.IndexOf(fnad) == estructuraFinancieraCapitalTrabajo.Count - 1))
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

                            //Plan Operativo - Plan Operativo

                            var planOperativo = proyectoController.PlanOperativoActividades(p.Id_proyecto);
                            if (planOperativo != null)
                            {
                                if (planOperativo.Count > 0)
                                {
                                    foreach (var fnad in planOperativo)
                                    {
                                        w.Write(fnad.Item + "¦" + fnad.Actividad + "¦"
                                            + fnad.FondoEmprenderMes_1 + "¦" + fnad.AporteEmprendedorMes_1 + "¦"
                                            + fnad.FondoEmprenderMes_2 + "¦" + fnad.AporteEmprendedorMes_2 + "¦"
                                            + fnad.FondoEmprenderMes_3 + "¦" + fnad.AporteEmprendedorMes_3 + "¦"
                                            + fnad.FondoEmprenderMes_4 + "¦" + fnad.AporteEmprendedorMes_4 + "¦"
                                            + fnad.FondoEmprenderMes_5 + "¦" + fnad.AporteEmprendedorMes_5 + "¦"
                                            + fnad.FondoEmprenderMes_6 + "¦" + fnad.AporteEmprendedorMes_6 + "¦"
                                            + fnad.FondoEmprenderMes_7 + "¦" + fnad.AporteEmprendedorMes_7 + "¦"
                                            + fnad.FondoEmprenderMes_8 + "¦" + fnad.AporteEmprendedorMes_8 + "¦"
                                            + fnad.FondoEmprenderMes_9 + "¦" + fnad.AporteEmprendedorMes_9 + "¦"
                                            + fnad.FondoEmprenderMes_10 + "¦" + fnad.AporteEmprendedorMes_10 + "¦"
                                            + fnad.FondoEmprenderMes_11 + "¦" + fnad.AporteEmprendedorMes_11 + "¦"
                                            + fnad.FondoEmprenderMes_12 + "¦" + fnad.AporteEmprendedorMes_12 + "¦"
                                            + fnad.FondoEmprender + "¦" + fnad.AporteEmprendedor
                                            );

                                        //Ultimo Registro
                                        if (!(planOperativo.IndexOf(fnad) == planOperativo.Count - 1))
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

                            //-------Plan Nacional de Desarrollo - Plan Regional de Desarrollo - Cluster o Cadena Productiva

                            var infoPlanNacionalRegionalCluster = proyectoController.PlanNacional_Regional_Cluster(p.Id_proyecto);
                            if (infoPlanNacionalRegionalCluster != null)
                            {
                                w.Write(infoPlanNacionalRegionalCluster.PlanNacional + "|"
                                    + infoPlanNacionalRegionalCluster.PlanRegional + "|"
                                    + infoPlanNacionalRegionalCluster.Cluster + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }

                            //Plan Operativo - Metas Sociales - Empleo

                            var planOperativoEmpleos = proyectoController.PlanOperativoEmpleos(p.Id_proyecto);
                            if (planOperativoEmpleos != null)
                            {
                                if (planOperativoEmpleos.Count > 0)
                                {
                                    foreach (var fnad in planOperativoEmpleos)
                                    {
                                        w.Write(fnad.TipoEmpleo + "¦" + fnad.Cargo + "¦"
                                            + fnad.valorMensual + "¦" + fnad.generadoPrimerAno + "¦"
                                             + fnad.EdadEntre1824Anos + "¦" + fnad.EsDesplazado + "¦"
                                              + fnad.EsMadre + "¦" + fnad.EsMinoria + "¦"
                                               + fnad.EsRecluido + "¦" + fnad.EsDesmovilizado + "¦"
                                                + fnad.EsDiscapacitado + "¦" + fnad.EsDesvinculado
                                            );

                                        //Ultimo Registro
                                        if (!(planOperativoEmpleos.IndexOf(fnad) == planOperativoEmpleos.Count - 1))
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

                            //-------Plan Operativo - Metas Sociales

                            var infoGeneracionEmpleo = proyectoController.PlanOperativoGenerarEmpleos(p.Id_proyecto);
                            if (infoGeneracionEmpleo != null)
                            {
                                w.Write(infoGeneracionEmpleo.GenerarPrimerAno + "|"
                                    + infoGeneracionEmpleo.GenerarTotalProyecto + "|"
                                    + infoGeneracionEmpleo.EmpleosIndirectos + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }

                            //Plan Operativo - Metas Sociales - Empleo

                            var planOperativoEmprendedores = proyectoController.PlanOperativoEmprendedores(p.Id_proyecto);
                            if (planOperativoEmprendedores != null)
                            {
                                if (planOperativoEmprendedores.Count > 0)
                                {
                                    foreach (var fnad in planOperativoEmprendedores)
                                    {
                                        w.Write(fnad.nombre + "¦" + fnad.beneficiario + "¦"
                                            + fnad.participacion);

                                        //Ultimo Registro
                                        if (!(planOperativoEmprendedores.IndexOf(fnad) == planOperativoEmprendedores.Count - 1))
                                        {
                                            w.Write("§");
                                        }
                                        else
                                        {
                                            //w.Write("|");
                                        }
                                    }
                                }
                                else
                                {
                                    //w.Write("|");
                                }
                            }
                            else
                            {
                                //w.Write("|");
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


                //archivo de Evaluacion
                if (tipoArchivo == "EVA")
                {
                    //Crear archivo plano
                    var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                            + @"ExportarArchivosHIS\" + _nomConvocatoria + @"\";
                    string nombreArchivo = "EVALUACION_" + nombreConv + "_" + fechaArchivo + ".txt";

                    nombreArchivoPlano = nombreArchivo;

                    if (!Directory.Exists(basePath))
                        Directory.CreateDirectory(basePath);

                    string finalPath = basePath + nombreArchivo;

                    var listadoProyectos = ListadoProyectosEvaluacion(_codConvocatoria);

                    cantidadProyectos = listadoProyectos.Count();

                    if (!File.Exists((finalPath)))
                    {
                        File.Create((finalPath)).Close();
                    }
                    using (StreamWriter w = File.AppendText((finalPath)))
                    {
                        w.WriteLine(titulosEvaluacion());

                        //contenido del archivo
                        foreach (var p in listadoProyectos)
                        {
                            //id del proyecto
                            w.Write(p.Id_proyecto + "|");

                            //Tabla de evaluacion -Datos generales
                            var infogeneral = proyectoController.infoEvalDatosGenerales(p.Id_proyecto, _codConvocatoria);

                            if (infogeneral != null)
                            {
                                w.Write(infogeneral.Localizacion + "|" + infogeneral.Sector + "|" + infogeneral.ResumenConceptoGeneral + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }


                            //Tabla de evaluacion
                            var infoTabEvaluacion = proyectoController.infoEvalTabEvaluacion(p.Id_proyecto, _codConvocatoria);
                            //Tabla de evaluacion -Protagonista

                            //¿Se tiene claridad del perfil del cliente y/o consumidor a atender, junto con su ubicación geográfica?
                            var claridadCliente = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_ClaridadPerfilCliente).FirstOrDefault();

                            if (claridadCliente != null)
                            {
                                w.Write(claridadCliente.Observacion + "|" + claridadCliente.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Se han identificado las necesidades y/o motivaciones a satisfacer del cliente y/o consumidor?
                            var necesidadesCliente = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_NecesidadesCliente).FirstOrDefault();

                            if (necesidadesCliente != null)
                            {
                                w.Write(necesidadesCliente.Observacion + "|" + necesidadesCliente.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Define los aspectos más importantes del mercado?
                            var aspectosMasImportantes = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_AspectosMasImportantes).FirstOrDefault();

                            if (aspectosMasImportantes != null)
                            {
                                w.Write(aspectosMasImportantes.Observacion + "|" + aspectosMasImportantes.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Establece el comportamiento de cada uno de los segmentos del mercado?
                            var comportamientoSegmento = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_ComportamientoSegmentoMercado).FirstOrDefault();

                            if (comportamientoSegmento != null)
                            {
                                w.Write(comportamientoSegmento.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Relaciona la estrategia y actividades de vinculación del proyecto con el ecosistema local de emprendimiento, en términos de participación y promoción de la industria de apoyo a emprendedores?
                            var relacionaActividadVinculacion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_RelacionaActividadVinculacion).FirstOrDefault();

                            if (relacionaActividadVinculacion != null)
                            {
                                w.Write(relacionaActividadVinculacion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Define las principales tendencias que afectan el mercado?
                            var defineTendenciaMercado = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_DefineTendenciaMercado).FirstOrDefault();

                            if (defineTendenciaMercado != null)
                            {
                                w.Write(defineTendenciaMercado.Observacion + "|" + defineTendenciaMercado.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Tiene claridad sobre como la situación del sector afecta al negocio?
                            var claridadSituacion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_claridadSituacionSector).FirstOrDefault();

                            if (claridadSituacion != null)
                            {
                                w.Write(claridadSituacion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Tiene claridad sobre como la situación del sector afecta al negocio?
                            var estableceParticipacion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estableceParticipacionMercado).FirstOrDefault();

                            if (estableceParticipacion != null)
                            {
                                w.Write(estableceParticipacion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Establece cómo se articula con la Política de desarrollo regional?
                            var politicaDesarrollo = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_politicaDesarrolloRegional).FirstOrDefault();

                            if (politicaDesarrollo != null)
                            {
                                w.Write(politicaDesarrollo.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Define los principales competidores en el mercado?
                            var defineCompetidoresMercado = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_defineCompetidresMercado).FirstOrDefault();

                            if (defineCompetidoresMercado != null)
                            {
                                w.Write(defineCompetidoresMercado.Observacion + "|" + defineCompetidoresMercado.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Establece cómo se articula con la Política de desarrollo regional?
                            var estableceVentajaDesventaja = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estableceVentajasDesventajas).FirstOrDefault();

                            if (estableceVentajaDesventaja != null)
                            {
                                w.Write(estableceVentajaDesventaja.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Establece cómo se articula con la Política de desarrollo regional?
                            var identificaPropuestaValor = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_identificaPropuestaDeValor).FirstOrDefault();

                            if (identificaPropuestaValor != null)
                            {
                                w.Write(identificaPropuestaValor.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Identifica como solucionar los problemas y/o necesidades de los clientes?
                            var identificaSolucionProblema = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_identificaSolucionarProblemasNecesidades).FirstOrDefault();

                            if (identificaSolucionProblema != null)
                            {
                                w.Write(identificaSolucionProblema.Observacion + "|" + identificaSolucionProblema.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Define claramente el concepto del negocio?
                            var defineConceptoNegocio = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_defineConceptoNegocio).FirstOrDefault();

                            if (defineConceptoNegocio != null)
                            {
                                w.Write(defineConceptoNegocio.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿La propuesta de valor está acorde con las necesidades de los clientes?
                            var acordeNecesidadCliente = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_acordeNecesidadCliente).FirstOrDefault();

                            if (acordeNecesidadCliente != null)
                            {
                                w.Write(acordeNecesidadCliente.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Establece el aspecto diferenciador del negocio?
                            var aspoectoDiferenciadorNegocio = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_aspectoDiferenciadordelNegocio).FirstOrDefault();

                            if (aspoectoDiferenciadorNegocio != null)
                            {
                                w.Write(aspoectoDiferenciadorNegocio.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿En el plan de negocios se describen las acciones que favorecen la preservación y sostenibilidad del medio ambiente?
                            var describeAccionMedioAmbiente = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_describeAccionesMedioAmbiente).FirstOrDefault();

                            if (describeAccionMedioAmbiente != null)
                            {
                                w.Write(describeAccionMedioAmbiente.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Explica la metodología utilizada para la validación de mercado?
                            var metodologiaValidaMercado = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_metodologiaUsadaValidarMercado).FirstOrDefault();

                            if (metodologiaValidaMercado != null)
                            {
                                w.Write(metodologiaValidaMercado.Observacion + "|" + metodologiaValidaMercado.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Muestra los resultados obtenidos de la validación de mercado?
                            var muestraResultadosMercado = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_muestraResultadosObtenidos).FirstOrDefault();

                            if (muestraResultadosMercado != null)
                            {
                                w.Write(muestraResultadosMercado.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Muestra los resultados obtenidos de la validación de mercado?
                            var validacionMercadoAcorde = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_validacionMercadoAcorde).FirstOrDefault();

                            if (validacionMercadoAcorde != null)
                            {
                                w.Write(validacionMercadoAcorde.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Evidencia avances técnicos, legales y comerciales en el desarrollo del negocio?
                            var avancesTecnicosLegalesComerciales = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_avancesTecnicosLegalesComerciales).FirstOrDefault();

                            if (avancesTecnicosLegalesComerciales != null)
                            {
                                w.Write(avancesTecnicosLegalesComerciales.Observacion + "|" + avancesTecnicosLegalesComerciales.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Cuenta actualmente con un producto mínimo viable validado en el mercado?
                            var productoMinimoViable = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_productoMinimoViable).FirstOrDefault();

                            if (productoMinimoViable != null)
                            {
                                w.Write(productoMinimoViable.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Menciona su experiencia previa relacionada con el tipo de negocio?
                            var mencionaExperienciaPrevia = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_mencionaExperienciaPrevia).FirstOrDefault();

                            if (mencionaExperienciaPrevia != null)
                            {
                                w.Write(mencionaExperienciaPrevia.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Define claramente como el negocio generará ingresos y la frecuencia de los mismos?
                            var defineIngresosFrecuencia = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_defineNegocioIngresos).FirstOrDefault();

                            if (defineIngresosFrecuencia != null)
                            {
                                w.Write(defineIngresosFrecuencia.Observacion + "|" + defineIngresosFrecuencia.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Establece condiciones de comercialización acorde con los clientes potenciales?
                            var estableceCondicionesComercio = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estableceCondicionesComercializacion).FirstOrDefault();

                            if (estableceCondicionesComercio != null)
                            {
                                w.Write(estableceCondicionesComercio.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se identifica el canal de comercialización a utilizar para la venta de productos y/o servicios?
                            var identificaCanalComercializacion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_identificaCanalComercializacion).FirstOrDefault();

                            if (identificaCanalComercializacion != null)
                            {
                                w.Write(identificaCanalComercializacion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se explica la metodología para la fijación de los precios de venta?
                            var explicaMetodologiaFijaPRecios = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_explicaMetodologiaFijaPrecios).FirstOrDefault();

                            if (explicaMetodologiaFijaPRecios != null)
                            {
                                w.Write(explicaMetodologiaFijaPRecios.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se identifican acercamientos con los clientes potenciales?
                            var identificaAcercamientosClientes = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_identificaAcercamientosClientes).FirstOrDefault();

                            if (identificaAcercamientosClientes != null)
                            {
                                w.Write(identificaAcercamientosClientes.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Describe cómo aplica la normatividad que regula al negocio? (Permiso de uso de suelos, licencias de funcionamiento, registros, manejo ambiental, manejo/licencia de aguas, bomberos, entre otros.)
                            var describeNormatividadRegula = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_describeNormatividadQueRegulaNegocio).FirstOrDefault();

                            if (describeNormatividadRegula != null)
                            {
                                w.Write(describeNormatividadRegula.Observacion + "|" + describeNormatividadRegula.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Establece los costos y trámites necesarios para cumplir con la normatividad descrita incluyendo la normatividad propia de cada región?
                            var estableceCostosTramites = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estableceCostosTramites).FirstOrDefault();

                            if (estableceCostosTramites != null)
                            {
                                w.Write(estableceCostosTramites.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Las condiciones técnicas de la infraestructura y el sitio de operación de negocio están acorde con lo que se requiere según la normatividad?
                            var condicionesTecnicasInfraestructura = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_condicionesInfraestructuraOperacion).FirstOrDefault();

                            if (condicionesTecnicasInfraestructura != null)
                            {
                                w.Write(condicionesTecnicasInfraestructura.Observacion + "|" + condicionesTecnicasInfraestructura.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Se evidencia el cumplimiento de parámetros técnicos conforme al tipo de negocio?
                            var evidenciaCumplimientoParamentros = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_evidenciaCumplimientoParametros).FirstOrDefault();

                            if (evidenciaCumplimientoParamentros != null)
                            {
                                w.Write(evidenciaCumplimientoParamentros.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Las fichas técnicas describen adecuadamente las características y parámetros requeridos?
                            var fichasDescribenTecnicas = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_fichasTecnicasDescripcion).FirstOrDefault();

                            if (fichasDescribenTecnicas != null)
                            {
                                w.Write(fichasDescribenTecnicas.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se describe claramente el proceso de producción, indicando los responsables, tiempos y equipos requeridos para cada una de las actividades?
                            var describeClaramenteProcesoProduccion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_describeClaramenteProcesoProd).FirstOrDefault();

                            if (describeClaramenteProcesoProduccion != null)
                            {
                                w.Write(describeClaramenteProcesoProduccion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿La capacidad de producción está acorde con el tamaño propuesto del negocio, partiendo de las ventas proyectadas?
                            var capacidadProduccionAcorde = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_capacidadProduccionAcorde).FirstOrDefault();

                            if (capacidadProduccionAcorde != null)
                            {
                                w.Write(capacidadProduccionAcorde.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿El programa de producción está acorde con la capacidad de producción y las variables técnicas?
                            var programaProduccionAcorde = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_programaProduccionAcorde).FirstOrDefault();

                            if (programaProduccionAcorde != null)
                            {
                                w.Write(programaProduccionAcorde.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Los requerimientos de materias primas e insumos están acorde con lo que se requiere para la operación del negocio y el plan de producción propuesto?
                            var requerimientosMateriasPrimas = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_requerimientosMateriasPrimas).FirstOrDefault();

                            if (requerimientosMateriasPrimas != null)
                            {
                                w.Write(requerimientosMateriasPrimas.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Existe complementariedad en los perfiles del equipo de trabajo propuesto y guardan relación con el tipo de negocio?
                            var complementoPerfilTrabajo = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_complementoPerfilTrabajos).FirstOrDefault();

                            if (complementoPerfilTrabajo != null)
                            {
                                w.Write(complementoPerfilTrabajo.Observacion + "|" + complementoPerfilTrabajo.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿En el plan de negocios se plantea la contratación de un aprendiz SENA?
                            var contrataAprendizSENA = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_contrataAprendizSENA).FirstOrDefault();

                            if (contrataAprendizSENA != null)
                            {
                                w.Write(contrataAprendizSENA.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿La estructura organizacional propuesta incluyendo cargos y funciones a desempeñar por el personal, es la adecuada para el funcionamiento de la empresa? (tipo de vinculación, proyección en el tiempo de ejecución y sostenibilidad de empleo)
                            var estructuraOrganizacional = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estructuraOrganizacional).FirstOrDefault();

                            if (estructuraOrganizacional != null)
                            {
                                w.Write(estructuraOrganizacional.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //Califique la generación de empleo según el siguiente rango: 0 a 3 empleos (1), 4 a 5 empleo (2) y 6 empleos en adelante (3)
                            var generacionEmpleo = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_generacionEmpleo).FirstOrDefault();

                            if (generacionEmpleo != null)
                            {
                                w.Write(generacionEmpleo.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se tiene la claridad sobre las actividades, responsables y costos relacionados para llevar a cabo las estrategias de comercialización?
                            var claridadActividades = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_claridadActividades).FirstOrDefault();

                            if (claridadActividades != null)
                            {
                                w.Write(claridadActividades.Observacion + "|" + claridadActividades.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Las actividades propuestas dentro de las estrategias de comercialización están acorde con el mercado objetivo?
                            var actividadesPropuestas = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_actividadesPropuestas).FirstOrDefault();

                            if (actividadesPropuestas != null)
                            {
                                w.Write(actividadesPropuestas.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Es claro el tiempo improductivo que se requiere para el arranque de la empresa y para empezar a vender de acuerdo al tipo de negocio?
                            var tiempoImproductivoRequerido = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_tiempoImproductivoRequerido).FirstOrDefault();

                            if (tiempoImproductivoRequerido != null)
                            {
                                w.Write(tiempoImproductivoRequerido.Observacion + "|" + tiempoImproductivoRequerido.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Se definen con claridad las fuentes de financiación para el periodo improductivo?
                            var definenClaridadFinanciacionPerImprod = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_fuentesFinanciacionPeriodoImprod).FirstOrDefault();

                            if (definenClaridadFinanciacionPerImprod != null)
                            {
                                w.Write(definenClaridadFinanciacionPerImprod.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se tiene en cuenta el tiempo, trámites y recursos para la puesta en marcha del negocio conforme al sector productivo?
                            var cuentaTiempoTramiteRecurso = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_cuentaTiempoTramitesRecursos).FirstOrDefault();

                            if (cuentaTiempoTramiteRecurso != null)
                            {
                                w.Write(cuentaTiempoTramiteRecurso.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿El plan de negocios presenta fuente de ingresos sostenibles en el largo plazo?
                            var planNegociosPresentaIngresos = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_planNegociosFuenteIngresos).FirstOrDefault();

                            if (planNegociosPresentaIngresos != null)
                            {
                                w.Write(planNegociosPresentaIngresos.Observacion + "|" + planNegociosPresentaIngresos.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿La estructura de costos se adecua al modelo de negocios propuesto?
                            var estructuraCostosAdecuada = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_estructuraCostosAdecuaModelo).FirstOrDefault();

                            if (estructuraCostosAdecuada != null)
                            {
                                w.Write(estructuraCostosAdecuada.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿La operación del negocio es rentable en el horizonte de tiempo proyectado?
                            var operacionNegocioRentable = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_operacionNegocioRentable).FirstOrDefault();

                            if (operacionNegocioRentable != null)
                            {
                                w.Write(operacionNegocioRentable.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Existe claridad sobre la estimación de los márgenes de ganancia por producto?
                            var claridadEstimacionMargen = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_claridadEstimacionMargen).FirstOrDefault();

                            if (claridadEstimacionMargen != null)
                            {
                                w.Write(claridadEstimacionMargen.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Define y mide de manera coherente los factores externos que pueden afectar la operación del negocio?
                            var defineCoherenteFactoresExternos = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_DefineCoherenteFactoresExternos).FirstOrDefault();

                            if (defineCoherenteFactoresExternos != null)
                            {
                                w.Write(defineCoherenteFactoresExternos.Observacion + "|" + defineCoherenteFactoresExternos.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Define y mide de manera coherente los factores internos que pueden afectar la operación del negocio?
                            var defineCoherenteFactorInterno = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_DefineCoherenteFactoresInternos).FirstOrDefault();

                            if (defineCoherenteFactorInterno != null)
                            {
                                w.Write(defineCoherenteFactorInterno.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Explica la forma en que se pueden mitigar los riesgos identificados?
                            var formaMitigarRiesgos = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_formaMitigarRiesgos).FirstOrDefault();

                            if (formaMitigarRiesgos != null)
                            {
                                w.Write(formaMitigarRiesgos.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se presentan las principales actividades en el plan operativo que llevara a cabo el negocio?
                            var presentaActividadNegocio = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_principalActividadNegocio).FirstOrDefault();

                            if (presentaActividadNegocio != null)
                            {
                                w.Write(presentaActividadNegocio.Observacion + "|" + presentaActividadNegocio.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //¿Para cada una de las actividades se establece su duración y fuente de financiación?
                            var actividadDuracionFinanciacion = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_actividadEstableceFinanciacion).FirstOrDefault();

                            if (actividadDuracionFinanciacion != null)
                            {
                                w.Write(actividadDuracionFinanciacion.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|");
                            }

                            //¿Se establecen indicadores de seguimiento claros y alcanzables para el primer año? Ejemplo: ventas y generación de empleos
                            var indicadorSeguimientoClaro = infoTabEvaluacion.Where(x => x.idCampo == Constantes.const_indicadorSeguimientoClaro).FirstOrDefault();

                            if (indicadorSeguimientoClaro != null)
                            {
                                w.Write(indicadorSeguimientoClaro.Observacion + "|" + indicadorSeguimientoClaro.puntaje + "|");
                            }
                            else
                            {
                                w.Write("|" + "|");
                            }

                            //Evaluacion financiera - Indicadores
                            var evaluacionFinanciera = proyectoController.evaluacionFinancieraIndicadores(p.Id_proyecto, _codConvocatoria);
                            if (evaluacionFinanciera != null)
                            {
                                if (evaluacionFinanciera.Count > 0)
                                {
                                    foreach (var qevalfinan in evaluacionFinanciera)
                                    {
                                        w.Write(qevalfinan.Descripcion + "¦" + qevalfinan.Valor);

                                        //Ultimo Registro
                                        if (!(evaluacionFinanciera.IndexOf(qevalfinan) == evaluacionFinanciera.Count - 1))
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

                            //Inversiones fijas
                            var inversionesFijas = proyectoController.evaluacionInversionesFijas(p.Id_proyecto, _codConvocatoria);
                            if (inversionesFijas != null)
                            {
                                if (inversionesFijas.Count > 0)
                                {
                                    foreach (var ifijas in inversionesFijas)
                                    {
                                        w.Write(ifijas.Nombre + "¦" + ifijas.Detalle + "¦" + ifijas.FuenteFinanciacion + "¦"
                                            + ifijas.TotalSolicitado + "¦" + ifijas.PorcentajeSolicitado + "¦"
                                            + ifijas.TotalRecomendado + "¦" + ifijas.PorcentajeRecomendado);

                                        //Ultimo Registro
                                        if (!(inversionesFijas.IndexOf(ifijas) == inversionesFijas.Count - 1))
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

                            //Capital de trabajo primer año de operacion
                            var capitalTrabajoPrimerAno = proyectoController.evaluacionCapitalTrabajoPrimerAno(p.Id_proyecto, _codConvocatoria);
                            if (capitalTrabajoPrimerAno != null)
                            {
                                if (capitalTrabajoPrimerAno.Count > 0)
                                {
                                    foreach (var capTrabajo in capitalTrabajoPrimerAno)
                                    {
                                        w.Write(capTrabajo.Nombre + "¦" + capTrabajo.Detalle + "¦" + capTrabajo.FuenteFinanciacion + "¦"
                                            + capTrabajo.TotalSolicitado + "¦" + capTrabajo.PorcentajeSolicitado + "¦"
                                            + capTrabajo.TotalRecomendado + "¦" + capTrabajo.PorcentajeRecomendado);

                                        //Ultimo Registro
                                        if (!(capitalTrabajoPrimerAno.IndexOf(capTrabajo) == capitalTrabajoPrimerAno.Count - 1))
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

                            //Inversiones Diferidas
                            var inversionesDiferidas = proyectoController.evaluacionInversionesDiferidas(p.Id_proyecto, _codConvocatoria);
                            if (inversionesDiferidas != null)
                            {
                                if (inversionesDiferidas.Count > 0)
                                {
                                    foreach (var inverDiferida in inversionesDiferidas)
                                    {
                                        w.Write(inverDiferida.Nombre + "¦" + inverDiferida.Detalle + "¦" + inverDiferida.FuenteFinanciacion + "¦"
                                            + inverDiferida.TotalSolicitado + "¦" + inverDiferida.PorcentajeSolicitado + "¦"
                                            + inverDiferida.TotalRecomendado + "¦" + inverDiferida.PorcentajeRecomendado);

                                        //Ultimo Registro
                                        if (!(inversionesDiferidas.IndexOf(inverDiferida) == inversionesDiferidas.Count - 1))
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

                            //Integrantes
                            var integrantesProyecto = proyectoController.evaluacionIntegrantesIniciativa(p.Id_proyecto, _codConvocatoria);
                            if (integrantesProyecto != null)
                            {
                                if (integrantesProyecto.Count > 0)
                                {
                                    foreach (var intProyecto in integrantesProyecto)
                                    {
                                        w.Write(intProyecto.Nombre + "¦" + intProyecto.Emprendedor + "¦" + intProyecto.Otro + "¦"
                                            + intProyecto.AporteTotal + "¦" + intProyecto.AporteDinero + "¦"
                                            + intProyecto.AporteEspecie + "¦" + intProyecto.ClaseEspecie);

                                        //Ultimo Registro
                                        if (!(integrantesProyecto.IndexOf(intProyecto) == integrantesProyecto.Count - 1))
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

                            //Observacion equipo de trabajo - Recursos solicitados - Valor recomendado
                            var evalObsAporte = proyectoController.evaluacionObservacionAportes(p.Id_proyecto, _codConvocatoria);

                            if (evalObsAporte != null)
                            {
                                w.Write(evalObsAporte.EquipoTrabajo + "|" + evalObsAporte.RecursosSolicitados + "|" + evalObsAporte.valorRecomendado + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }

                            //Concepto final - Riesgos
                            var evaluacionRiesg = proyectoController.evaluacionRiesgos(p.Id_proyecto, _codConvocatoria);
                            if (evaluacionRiesg != null)
                            {
                                if (evaluacionRiesg.Count > 0)
                                {
                                    foreach (var evalRiego in evaluacionRiesg)
                                    {
                                        w.Write(evalRiego.Riesgo + "¦" + evalRiego.Mitigacion);

                                        //Ultimo Registro
                                        if (!(evaluacionRiesg.IndexOf(evalRiego) == evaluacionRiesg.Count - 1))
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

                            //Concepto final - Conclusion
                            var evalconcepFinal = proyectoController.evaluacionConceptoFinal(p.Id_proyecto, _codConvocatoria);

                            if (evalconcepFinal != null)
                            {
                                w.Write(evalconcepFinal.Viable + "|" + evalconcepFinal.Conceptos + "|" + evalconcepFinal.Justificacion + "|");
                            }
                            else
                            {
                                w.Write("|" + "|" + "|");
                            }

                            //Indicadores de gestion
                            var evalIndicadorGestion = proyectoController.evaluacionIndicadorGestion(p.Id_proyecto, _codConvocatoria);

                            if (evalIndicadorGestion != null)
                            {
                                w.Write(evalIndicadorGestion.EjecucionPresupuestalEmprendedor + "|" + evalIndicadorGestion.EjecucionPresupuestalEvaluador + "|"
                                    + evalIndicadorGestion.IDHEmprendedor + "|" + evalIndicadorGestion.IDHEvaluador + "|"
                                    + evalIndicadorGestion.ContrapartidaEmprendedor + "|" + evalIndicadorGestion.ContrapartidaEvaluador + "|"
                                    + evalIndicadorGestion.VentasEmprendedor + "|" + evalIndicadorGestion.VentasEvaluador + "|"
                                    + evalIndicadorGestion.PeriodoemproductivoEmprededor + "|" + evalIndicadorGestion.PeriodoemproductivoEvaluador + "|"
                                    + evalIndicadorGestion.AportesEmprendedorEmprededor + "|" + evalIndicadorGestion.AportesEmprendedorEvaluador + "|"
                                    );
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|"
                                   + "|" + "|" + "|" + "|" + "|" + "|");
                            }

                            //Indicador Gestion - Produccion
                            var indiGestionProduccion = proyectoController.evaluacionIndicadorGestionProduccion(p.Id_proyecto, _codConvocatoria);
                            if (indiGestionProduccion != null)
                            {
                                if (indiGestionProduccion.Count > 0)
                                {
                                    foreach (var ingp in indiGestionProduccion)
                                    {
                                        w.Write(ingp.ProductoRepresentativo + "¦" + ingp.ProductoRepresentativoEvaluacion
                                            + "¦" + ingp.Producto + "¦" + ingp.Unidades + "¦" + ingp.DatosEvaluador);

                                        //Ultimo Registro
                                        if (!(indiGestionProduccion.IndexOf(ingp) == indiGestionProduccion.Count - 1))
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

                            //Indicador Gestion - Mercadeo
                            var indiGestionMercadeo = proyectoController.evaluacionIndicadorGestionMercadeo(p.Id_proyecto, _codConvocatoria);
                            if (indiGestionMercadeo != null)
                            {
                                if (indiGestionMercadeo.Count > 0)
                                {
                                    foreach (var ingmer in indiGestionMercadeo)
                                    {
                                        w.Write(ingmer.Cantidad + "¦" + ingmer.Actividad_Cargo
                                            + "¦" + ingmer.DatosEvaluador);

                                        //Ultimo Registro
                                        if (!(indiGestionMercadeo.IndexOf(ingmer) == indiGestionMercadeo.Count - 1))
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

                            //Indicador Gestion - Empleo
                            var indiGestionEmpleo = proyectoController.evaluacionIndicadorGestionEmpleo(p.Id_proyecto, _codConvocatoria);
                            if (indiGestionEmpleo != null)
                            {
                                if (indiGestionEmpleo.Count > 0)
                                {
                                    foreach (var ingEmpleo in indiGestionEmpleo)
                                    {
                                        w.Write(ingEmpleo.Cantidad + "¦" + ingEmpleo.Actividad_Cargo
                                            + "¦" + ingEmpleo.DatosEvaluador);

                                        //Ultimo Registro
                                        if (!(indiGestionEmpleo.IndexOf(ingEmpleo) == indiGestionEmpleo.Count - 1))
                                        {
                                            w.Write("§");
                                        }
                                        else
                                        {
                                            //w.Write("|");
                                        }
                                    }
                                }
                                else
                                {
                                    //w.Write("|");
                                }
                            }
                            else
                            {
                                //w.Write("|");
                            }

                            //Final de linea
                            w.Write(System.Environment.NewLine);
                        }

                        w.Flush();
                        w.Close();
                    }

                    creado = true;

                    rutaConvocatoria = basePath;

                }

                //archivo de Interventoria
                if (tipoArchivo == "INT")
                {
                    //Crear archivo plano
                    var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                            + @"ExportarArchivosHIS\" + _nomConvocatoria + @"\";
                    string nombreArchivo = "INTERVENTORIA_" + nombreConv + "_" + fechaArchivo + ".txt";

                    nombreArchivoPlano = nombreArchivo;

                    if (!Directory.Exists(basePath))
                        Directory.CreateDirectory(basePath);

                    string finalPath = basePath + nombreArchivo;

                    var listadoProyectos = ListadoProyectos(_codConvocatoria);

                    //cantidadProyectos = listadoProyectos.Count();

                    if (!File.Exists((finalPath)))
                    {
                        File.Create((finalPath)).Close();
                    }

                    using (StreamWriter w = File.AppendText((finalPath)))
                    {
                        //-----------------------------Actividades plan operativo-------------------------------------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloActividadesPlanOperativo());
                        cantidadProyectos = 0;
                        int oldProyect = 0;
                        foreach (var p in listadoProyectos)
                        {

                            var actividadesXProyecto = proyectoController.actividadesPlanOperativoXProyecto(p.Id_proyecto);

                            if (actividadesXProyecto != null)
                            {

                                foreach (var actividad in actividadesXProyecto)
                                {
                                    w.Write(actividad.codProyecto + "|" + actividad.Item + "|" + actividad.Actividad);
                                    w.Write(System.Environment.NewLine);

                                    if (oldProyect != actividad.codProyecto)
                                    {
                                        cantidadProyectos = cantidadProyectos + 1;
                                        oldProyect = actividad.codProyecto;
                                    }
                                }
                            }
                            else
                            {
                                w.Write("|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Resumen mes a mes por actividad Plan Operativo------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloResumenMesxMesActividad());

                        foreach (var p in listadoProyectos)
                        {


                            var resumenMesXMEsXActividad = proyectoController.resumenMesXMesActividades(p.Id_proyecto);

                            if (resumenMesXMEsXActividad != null)
                            {

                                foreach (var actividad in resumenMesXMEsXActividad)
                                {
                                    w.Write(p.Id_proyecto + "|" + actividad.Actividad
                                        + "|" + actividad.FondoEmprenderMes_1 + "|" + actividad.AporteEmprendedorMes_1
                                        + "|" + actividad.FondoEmprenderMes_2 + "|" + actividad.AporteEmprendedorMes_2
                                        + "|" + actividad.FondoEmprenderMes_3 + "|" + actividad.AporteEmprendedorMes_3
                                        + "|" + actividad.FondoEmprenderMes_4 + "|" + actividad.AporteEmprendedorMes_4
                                        + "|" + actividad.FondoEmprenderMes_5 + "|" + actividad.AporteEmprendedorMes_5
                                        + "|" + actividad.FondoEmprenderMes_6 + "|" + actividad.AporteEmprendedorMes_6
                                        + "|" + actividad.FondoEmprenderMes_7 + "|" + actividad.AporteEmprendedorMes_7
                                        + "|" + actividad.FondoEmprenderMes_8 + "|" + actividad.AporteEmprendedorMes_8
                                        + "|" + actividad.FondoEmprenderMes_9 + "|" + actividad.AporteEmprendedorMes_9
                                        + "|" + actividad.FondoEmprenderMes_10 + "|" + actividad.AporteEmprendedorMes_10
                                        + "|" + actividad.FondoEmprenderMes_11 + "|" + actividad.AporteEmprendedorMes_11
                                        + "|" + actividad.FondoEmprenderMes_12 + "|" + actividad.AporteEmprendedorMes_12
                                        + "|" + actividad.FondoEmprenderMes_13 + "|" + actividad.AporteEmprendedorMes_13
                                        + "|" + actividad.FondoEmprenderMes_14 + "|" + actividad.AporteEmprendedorMes_14
                                        + "|" + actividad.FondoEmprenderMes_15 + "|" + actividad.AporteEmprendedorMes_15
                                        + "|" + actividad.FondoEmprenderMes_16 + "|" + actividad.AporteEmprendedorMes_16
                                        + "|" + actividad.FondoEmprenderMes_17 + "|" + actividad.AporteEmprendedorMes_17
                                        + "|" + actividad.FondoEmprenderMes_18 + "|" + actividad.AporteEmprendedorMes_18
                                        + "|" + actividad.FondoEmprenderMes_19 + "|" + actividad.AporteEmprendedorMes_19
                                        + "|" + actividad.FondoEmprenderMes_20 + "|" + actividad.AporteEmprendedorMes_20
                                        + "|" + actividad.FondoEmprender + "|" + actividad.AporteEmprendedor);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }


                        }

                        //---------------ver avance mes a mes por actividad Plan Operativo------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloAvancenMesxMesActividad());

                        foreach (var p in listadoProyectos)
                        {


                            var avanceMesXMEsXActividad = proyectoController.avanceMesXMesActividades(p.Id_proyecto);

                            if (avanceMesXMEsXActividad != null)
                            {

                                foreach (var actividad in avanceMesXMEsXActividad)
                                {
                                    w.Write(actividad.idPlan + "|" + actividad.Actividad + "|" + actividad.mes
                                        + "|" + actividad.FechaAvance + "|" + actividad.ObservacionesEmprendedor
                                        + "|" + actividad.FechaAprobacion + "|" + actividad.ObservacionesInterventor
                                        + "|" + actividad.ActividaAprobada);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------ver Historico del avance por actividad Plan Operativo------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloHistoricoAvanceActividad());

                        foreach (var p in listadoProyectos)
                        {


                            var historicoAvanceActividad = proyectoController.historicoAvanceActividades(p.Id_proyecto);

                            if (historicoAvanceActividad != null)
                            {

                                foreach (var actividad in historicoAvanceActividad)
                                {
                                    w.Write(actividad.idPlan + "|" + actividad.Actividad + "|" + actividad.mes
                                        + "|" + actividad.ObservacionesEmprendedor + "|" + actividad.FechaAvance
                                        + "|" + actividad.ObservacionesInterventor + "|" + actividad.FechaAprobacion
                                        + "|" + actividad.ValorFondoEmprender
                                        + "|" + actividad.ValorAporteEmprendedor + "|" + actividad.ActividaAprobada
                                        + "|" + actividad.RegistradoPor);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Nomina Mes a Mes Por Cargo------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(titulosNominaMesxMesxCargo());

                        foreach (var p in listadoProyectos)
                        {
                            var nomicaCargoMesxMes = proyectoController.nominaMesxMesxCargo(p.Id_proyecto);

                            if (nomicaCargoMesxMes != null)
                            {

                                foreach (var n in nomicaCargoMesxMes)
                                {
                                    w.Write(p.Id_proyecto + "|" + n.Cargo + "|" + n.SueldoMes_1 + "|" + n.PrestacionesMes_1
                                        + "|" + n.SueldoMes_2 + "|" + n.PrestacionesMes_2 + "|" + n.SueldoMes_3 + "|" + n.PrestacionesMes_3
                                        + "|" + n.SueldoMes_4 + "|" + n.PrestacionesMes_4 + "|" + n.SueldoMes_5 + "|" + n.PrestacionesMes_5
                                        + "|" + n.SueldoMes_6 + "|" + n.PrestacionesMes_6 + "|" + n.SueldoMes_7 + "|" + n.PrestacionesMes_7
                                        + "|" + n.SueldoMes_8 + "|" + n.PrestacionesMes_8 + "|" + n.SueldoMes_9 + "|" + n.PrestacionesMes_9
                                        + "|" + n.SueldoMes_10 + "|" + n.PrestacionesMes_10 + "|" + n.SueldoMes_11 + "|" + n.PrestacionesMes_11
                                        + "|" + n.SueldoMes_12 + "|" + n.PrestacionesMes_12 + "|" + n.SueldoMes_13 + "|" + n.PrestacionesMes_13
                                        + "|" + n.SueldoMes_14 + "|" + n.PrestacionesMes_14 + "|" + n.SueldoMes_15 + "|" + n.PrestacionesMes_15
                                        + "|" + n.SueldoMes_16 + "|" + n.PrestacionesMes_16 + "|" + n.SueldoMes_17 + "|" + n.PrestacionesMes_17
                                        + "|" + n.SueldoMes_18 + "|" + n.PrestacionesMes_18 + "|" + n.SueldoMes_19 + "|" + n.PrestacionesMes_19
                                        + "|" + n.SueldoMes_20 + "|" + n.PrestacionesMes_20 + "|" + n.SueldoTotal + "|" + n.PrestacionesTotal);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Avance Nomina Mes a Mes Por Cargo------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloAvancenMesxMesCargo());

                        foreach (var p in listadoProyectos)
                        {
                            var nominaAvanceCargoMesxMes = proyectoController.AvanceNominaMesxMes(p.Id_proyecto);

                            if (nominaAvanceCargoMesxMes != null)
                            {

                                foreach (var n in nominaAvanceCargoMesxMes)
                                {
                                    w.Write(n.idPlan + "|" + n.Actividad + "|" + n.mes
                                        + "|" + n.FechaAvance + "|" + n.ObservacionesEmprendedor
                                        + "|" + n.FechaAprobacion + "|" + n.ObservacionesInterventor
                                        + "|" + n.ActividaAprobada);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------ver Historico del avance por Nomina------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloHistoricoAvanceNomina());

                        foreach (var p in listadoProyectos)
                        {


                            var historicoAvanceNomina = proyectoController.historicoAvanceNomina(p.Id_proyecto);

                            if (historicoAvanceNomina != null)
                            {

                                foreach (var actividad in historicoAvanceNomina)
                                {
                                    w.Write(actividad.idPlan + "|" + actividad.Actividad + "|" + actividad.mes
                                        + "|" + actividad.ObservacionesEmprendedor + "|" + actividad.FechaAvance
                                        + "|" + actividad.ObservacionesInterventor + "|" + actividad.FechaAprobacion
                                        + "|" + actividad.ValorAporteEmprendedor //ValorSueldo
                                        + "|" + actividad.ValorFondoEmprender    //ValorPrestaciones
                                        + "|" + actividad.ActividaAprobada
                                        + "|" + actividad.RegistradoPor);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Produccion Mes a Mes------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(titulosProduccionMesxMes());

                        foreach (var p in listadoProyectos)
                        {
                            var produccionMesxMes = proyectoController.ProduccionMesxMes(p.Id_proyecto);

                            if (produccionMesxMes != null)
                            {

                                foreach (var n in produccionMesxMes)
                                {
                                    w.Write(p.Id_proyecto + "|" + n.Producto + "|" + n.CantidadMes_1 + "|" + n.CostoMes_1
                                        + "|" + n.CantidadMes_2 + "|" + n.CostoMes_2 + "|" + n.CantidadMes_3 + "|" + n.CostoMes_3
                                        + "|" + n.CantidadMes_4 + "|" + n.CostoMes_4 + "|" + n.CantidadMes_5 + "|" + n.CostoMes_5
                                        + "|" + n.CantidadMes_6 + "|" + n.CostoMes_6 + "|" + n.CantidadMes_7 + "|" + n.CostoMes_7
                                        + "|" + n.CantidadMes_8 + "|" + n.CostoMes_8 + "|" + n.CantidadMes_9 + "|" + n.CostoMes_9
                                        + "|" + n.CantidadMes_10 + "|" + n.CostoMes_10 + "|" + n.CantidadMes_11 + "|" + n.CostoMes_11
                                        + "|" + n.CantidadMes_12 + "|" + n.CostoMes_12 + "|" + n.CantidadMes_13 + "|" + n.CostoMes_13
                                        + "|" + n.CantidadMes_14 + "|" + n.CostoMes_14 + "|" + n.CantidadMes_15 + "|" + n.CostoMes_15
                                        + "|" + n.CantidadMes_16 + "|" + n.CostoMes_16 + "|" + n.CantidadMes_17 + "|" + n.CostoMes_17
                                        + "|" + n.CantidadMes_18 + "|" + n.CostoMes_18 + "|" + n.CantidadMes_19 + "|" + n.CostoMes_19
                                        + "|" + n.CantidadMes_20 + "|" + n.CostoMes_20 + "|" + n.CantidadTotal + "|" + n.CostoTotal);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Avance Produccion Mes a Mes------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloAvancenMesxMesProducto());

                        foreach (var p in listadoProyectos)
                        {
                            var produccionAvanceMesxMes = proyectoController.AvanceProduccionMesxMes(p.Id_proyecto);

                            if (produccionAvanceMesxMes != null)
                            {

                                foreach (var n in produccionAvanceMesxMes)
                                {
                                    w.Write(n.idPlan + "|" + n.Actividad + "|" + n.mes
                                        + "|" + n.FechaAvance + "|" + n.ObservacionesEmprendedor
                                        + "|" + n.FechaAprobacion + "|" + n.ObservacionesInterventor
                                        + "|" + n.ActividaAprobada);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------ver Historico del avance Produccion------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloHistoricoAvanceProducion());

                        foreach (var p in listadoProyectos)
                        {


                            var historicoAvanceProduccion = proyectoController.historicoAvanceProduccion(p.Id_proyecto);

                            if (historicoAvanceProduccion != null)
                            {

                                foreach (var actividad in historicoAvanceProduccion)
                                {
                                    w.Write(actividad.idPlan + "|" + actividad.Actividad + "|" + actividad.mes
                                        + "|" + actividad.ObservacionesEmprendedor + "|" + actividad.FechaAvance
                                        + "|" + actividad.ObservacionesInterventor + "|" + actividad.FechaAprobacion
                                        + "|" + actividad.ValorAporteEmprendedor //Cantidad
                                        + "|" + actividad.ValorFondoEmprender    //Costo
                                        + "|" + actividad.ActividaAprobada
                                        + "|" + actividad.RegistradoPor);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Ventas Mes a Mes------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(titulosVentasMesxMes());

                        foreach (var p in listadoProyectos)
                        {
                            var ventasMesxMes = proyectoController.VentasMesxMes(p.Id_proyecto);

                            if (ventasMesxMes != null)
                            {

                                foreach (var n in ventasMesxMes)
                                {
                                    w.Write(p.Id_proyecto + "|" + n.Producto + "|" + n.VentasMes_1 + "|" + n.IngresoMes_1
                                        + "|" + n.VentasMes_2 + "|" + n.IngresoMes_2 + "|" + n.VentasMes_3 + "|" + n.IngresoMes_3
                                        + "|" + n.VentasMes_4 + "|" + n.IngresoMes_4 + "|" + n.VentasMes_5 + "|" + n.IngresoMes_5
                                        + "|" + n.VentasMes_6 + "|" + n.IngresoMes_6 + "|" + n.VentasMes_7 + "|" + n.IngresoMes_7
                                        + "|" + n.VentasMes_8 + "|" + n.IngresoMes_8 + "|" + n.VentasMes_9 + "|" + n.IngresoMes_9
                                        + "|" + n.VentasMes_10 + "|" + n.IngresoMes_10 + "|" + n.VentasMes_11 + "|" + n.IngresoMes_11
                                        + "|" + n.VentasMes_12 + "|" + n.IngresoMes_12 + "|" + n.VentasMes_13 + "|" + n.IngresoMes_13
                                        + "|" + n.VentasMes_14 + "|" + n.IngresoMes_14 + "|" + n.VentasMes_15 + "|" + n.IngresoMes_15
                                        + "|" + n.VentasMes_16 + "|" + n.IngresoMes_16 + "|" + n.VentasMes_17 + "|" + n.IngresoMes_17
                                        + "|" + n.VentasMes_18 + "|" + n.IngresoMes_18 + "|" + n.VentasMes_19 + "|" + n.IngresoMes_19
                                        + "|" + n.VentasMes_20 + "|" + n.IngresoMes_20 + "|" + n.VentasTotal + "|" + n.IngresoTotal);
                                    w.Write(System.Environment.NewLine);

                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                        + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Avance Ventas Mes a Mes------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloAvancenMesxMesProducto());

                        foreach (var p in listadoProyectos)
                        {
                            var VentaAvanceMesxMes = proyectoController.AvanceVentasMesxMes(p.Id_proyecto);

                            if (VentaAvanceMesxMes != null)
                            {

                                foreach (var n in VentaAvanceMesxMes)
                                {
                                    w.Write(n.idPlan + "|" + n.Actividad + "|" + n.mes
                                        + "|" + n.FechaAvance + "|" + n.ObservacionesEmprendedor
                                        + "|" + n.FechaAprobacion + "|" + n.ObservacionesInterventor
                                        + "|" + n.ActividaAprobada);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------ver Historico del avance Ventas------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloHistoricoAvanceProducion());

                        foreach (var p in listadoProyectos)
                        {


                            var historicoAvanceVentas = proyectoController.historicoAvanceVentas(p.Id_proyecto);

                            if (historicoAvanceVentas != null)
                            {

                                foreach (var actividad in historicoAvanceVentas)
                                {
                                    w.Write(actividad.idPlan + "|" + actividad.Actividad + "|" + actividad.mes
                                        + "|" + actividad.ObservacionesEmprendedor + "|" + actividad.FechaAvance
                                        + "|" + actividad.ObservacionesInterventor + "|" + actividad.FechaAprobacion
                                        + "|" + actividad.ValorAporteEmprendedor //Ventas
                                        + "|" + actividad.ValorFondoEmprender    //Ingresos
                                        + "|" + actividad.ActividaAprobada
                                        + "|" + actividad.RegistradoPor);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Indicadores genericos------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloIndicadoresGenericos());

                        foreach (var p in listadoProyectos)
                        {
                            var indiGenerico = proyectoController.GetIndicadoresGestionxProyectos(p.Id_proyecto);

                            if (indiGenerico != null)
                            {

                                foreach (var i in indiGenerico)
                                {
                                    w.Write(i.IdPlan + "|" + i.Nombre + "|" + i.Decripcion + "|" + i.Evaluacion + "|" + i.Observacion);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Riesgos Interventor------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloRiegosInterventor());

                        foreach (var p in listadoProyectos)
                        {
                            var riesgoInter = proyectoController.GetRiesgosInterventorxProyecto(p.Id_proyecto);

                            if (riesgoInter != null)
                            {

                                foreach (var i in riesgoInter)
                                {
                                    w.Write(i.IdPlan + "|" + i.EjeFuncional + "|" + i.Riesgo + "|" + i.Mitigacion + "|" + i.Observacion);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write("|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Concepto final  y recomendaciones------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloConceptoFinalRecomendaciones());

                        foreach (var p in listadoProyectos)
                        {
                            var conceptFinalRecom = proyectoController.GetConceptoFinalRecomendaciones(p.Id_proyecto);

                            if (conceptFinalRecom != null)
                            {

                                w.Write(conceptFinalRecom.IdPlan + "|" + conceptFinalRecom.DificultadCentral + "|" + conceptFinalRecom.Observaciones);
                                w.Write(System.Environment.NewLine);

                            }
                            else
                            {
                                w.Write(p.Id_proyecto + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Contrato------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloContrato());

                        foreach (var p in listadoProyectos)
                        {
                            var contratoProyecto = proyectoController.GetContratoInterventoria(p.Id_proyecto);

                            if (contratoProyecto != null)
                            {

                                w.Write(contratoProyecto.IdPlan + "|" + contratoProyecto.NumeroDeContrato
                                    + "|" + contratoProyecto.FechaActaDeInicio + "|" + contratoProyecto.Objeto
                                     + "|" + contratoProyecto.FechaDelAp + "|" + contratoProyecto.NoPolizaDeSeguroDeVida
                                      + "|" + contratoProyecto.ValorInicial + "|" + contratoProyecto.Plazo
                                       + "|" + contratoProyecto.NumeroDelApPresupuestal + "|" + contratoProyecto.FechaFirmaDelContrato
                                        + "|" + contratoProyecto.CompaniaSeguroDeVida);

                                w.Write(System.Environment.NewLine);

                            }
                            else
                            {
                                w.Write(p.Id_proyecto + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Registro Mercantil------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloRegistroMercantil());

                        foreach (var p in listadoProyectos)
                        {
                            var regMercantil = proyectoController.GetRegistroMercantilxProyecto(p.Id_proyecto);

                            if (regMercantil != null)
                            {

                                w.Write(regMercantil.IdPlan + "|" + regMercantil.NombrePlan + "|" + regMercantil.RazonSocial
                                    + "|" + regMercantil.ObjetoSocial + "|" + regMercantil.CapitalSocial + "|" + regMercantil.TipoSociedad
                                    + "|" + regMercantil.CodigoCIIU + "|" + regMercantil.NumeroEscrituraPublica + "|" + regMercantil.DomicilioEmpresa
                                    + "|" + regMercantil.Departamento + "|" + regMercantil.Ciudad + "|" + regMercantil.Telefono
                                    + "|" + regMercantil.Email + "|" + regMercantil.Nit
                                    + "|" + regMercantil.EsRegimenEspecial + "|" + regMercantil.NormaRegimenEspecial + "|" + regMercantil.FechaNormaRegimenEspecial
                                    + "|" + regMercantil.EsContribuyente + "|" + regMercantil.NormaContribuyente + "|" + regMercantil.FechaNormaContribuyente
                                    + "|" + regMercantil.EsAutoretenedor + "|" + regMercantil.NormaAutoretenedor + "|" + regMercantil.FechaNormaAutoretenedor
                                    + "|" + regMercantil.EsDeclarante + "|" + regMercantil.NormaDeclarante + "|" + regMercantil.FechaNormaDeclarante
                                    + "|" + regMercantil.EsExentoDeRetencion + "|" + regMercantil.NormaExentoDeRetencion + "|" + regMercantil.FechaNormaExentoDeRetencion
                                    + "|" + regMercantil.EsGranContribuyente + "|" + regMercantil.NormaGranContribuyente + "|" + regMercantil.FechaNormaGranContribuyente);

                                w.Write(System.Environment.NewLine);

                            }
                            else
                            {
                                w.Write(p.Id_proyecto + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|" + "|"
                                    + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        //---------------Socios------------------
                        w.WriteLine(saltoLineaGuion());
                        w.WriteLine(tituloSocios());

                        foreach (var p in listadoProyectos)
                        {
                            var sociosInfo = proyectoController.GetSociosxProyecto(p.Id_proyecto);

                            if (sociosInfo != null)
                            {

                                foreach (var i in sociosInfo)
                                {
                                    w.Write(i.IdPlan + "|" + i.Nombre + "|" + i.Identificacion + "|" + i.Email + "|" + i.Telefono);
                                    w.Write(System.Environment.NewLine);
                                }
                            }
                            else
                            {
                                w.Write(p.Id_proyecto + "|" + "|" + "|" + "|");
                                w.Write(System.Environment.NewLine);
                            }
                        }

                        w.Flush();
                        w.Close();
                    }

                    creado = true;

                    rutaConvocatoria = basePath;

                    //copiar archivos de interventoria
                    crearArchivoExcelPagos(listadoProyectos, _nomConvocatoria, fechaArchivo);

                    copiarArchivosRut(listadoProyectos, basePath, _nomConvocatoria, fechaArchivo);

                }
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

        private void copiarArchivosRut(List<InfoGeneralProyecto> listadoProyectos, string rutaBase, string nomConvocatoria, string fechaArchivo)
        {
            string rutaFinal = rutaBase + @"RUT_" + fechaArchivo + @"\";

            if (!Directory.Exists(rutaFinal))
                Directory.CreateDirectory(rutaFinal);

            foreach (var p in listadoProyectos)
            {
                if (ValidarCargaArchivo(p.Id_proyecto))
                {
                    string ip = ConfigurationManager.AppSettings.Get("RutaIP");
                    string ruta = "Documentos/Proyecto/Proyecto_" + p.Id_proyecto + "/";

                    string nombrearchivo = "RUT_" + p.Id_proyecto + ".pdf";

                    string rutaOrigenArchivo = ip + ruta + nombrearchivo;
                
                    
                    if (File.Exists(rutaOrigenArchivo))
                    {                        
                        if (Directory.Exists(ip + ruta))
                        {
                            string fileName = "RUT_" + p.Id_proyecto + ".pdf";
                            string destFile = Path.Combine(rutaFinal, fileName);
                            string origFile = Path.Combine((ip + ruta), fileName);
                            System.IO.File.Copy(origFile, destFile, true);
                        }
                    }
                }
            }
        }

        private bool ValidarCargaArchivo(int CodProyecto)
        {
            bool cargado = false;

            string filename = "RUT_" + CodProyecto + ".pdf";
            int idProyecto = Convert.ToInt32(CodProyecto);

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = (from C in db.ContratosArchivosAnexos
                             where C.CodProyecto == idProyecto && C.NombreArchivo.Contains(filename)
                             select C.IdContratoArchivoAnexo
                             ).FirstOrDefault();

                if (query == 0)// si no se encuentra
                {
                    cargado = false;
                }
                else //si se encientra
                {
                    cargado = true;
                }
            }


            return cargado;
        }

        private void crearArchivoExcelPagos(List<InfoGeneralProyecto> listadoProyectos, string nomConvocatoria, string fecha)
        {
            foreach (var p in listadoProyectos)
            {
                int CodProyecto = p.Id_proyecto;

                try
                {
                    List<dataDescargaPagos> dt = (from pi in consultas.Db.MD_PresupuestoInterventor(Convert.ToInt32(CodProyecto))
                                                  select new dataDescargaPagos
                                                  {
                                                      codigo = pi.Id_PagoActividad,
                                                      NombrePago = pi.NomPagoActividad,
                                                      FechaInterventor = pi.FechaInterventor.HasValue ? pi.FechaInterventor.Value.ToString() : "",
                                                      CantidadDinero = pi.CantidadDinero.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                                                      Estado = new Func<string>(() =>
                                                      {
                                                          switch (Convert.ToInt32(pi.Estado))
                                                          {
                                                              case Constantes.CONST_EstadoInterventor:
                                                                  return "Interventor";
                                                              case Constantes.CONST_EstadoCoordinador:
                                                                  return "Coordinador";
                                                              case Constantes.CONST_EstadoFiduciaria:
                                                                  return "Fiduciaria";
                                                              case Constantes.CONST_EstadoAprobadoFA:
                                                                  return "Aprobado";
                                                              case Constantes.CONST_EstadoRechazadoFA:
                                                                  return "Rechazado";
                                                              default:
                                                                  return "Interventor";
                                                          }
                                                      })(),
                                                      TipoIdentificacion = pi.NomTipoIdentificacion,
                                                      Identificacion = pi.NumIdentificacion,
                                                      Nombre = pi.Nombre,
                                                      Apellido = pi.Apellido,
                                                      RazonSocial = pi.RazonSocial,
                                                      FechaRtaFA = pi.FechaRtaFA.HasValue ? pi.FechaRtaFA.Value.ToString() : "",
                                                      ValorReteFuente = pi.ValorReteFuente.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),

                                                      ValorReteIVA = pi.ValorReteIVA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),

                                                      ValorReteICA = pi.ValorReteICA.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),

                                                      OtrosDescuentos = pi.OtrosDescuentos.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),

                                                      ValorPagado = pi.ValorPagado.Value.ToString("c", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")),
                                                      CodigoPago = pi.CodigoPago,
                                                      ObservacionesFA = new Func<string>(() =>
                                                      {
                                                          using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                                                          {
                                                              if (pi.Estado.Equals(Constantes.CONST_EstadoRechazadoFA) && pi.FechaRtaFA == null)
                                                              {
                                                                  var observacion = db.PagosActaSolicitudPagos.SingleOrDefault(filter => filter.CodPagoActividad.Equals(pi.Id_PagoActividad) && filter.Aprobado.Equals(false));
                                                                  if (observacion != null)
                                                                      return "Observación coordinador Interventoria : " + observacion.Observaciones;
                                                                  else
                                                                      return string.Empty;
                                                              }
                                                              else
                                                              {
                                                                  if (consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad))
                                                                      return pi.ObservacionesFA + " - Pago con reintegros.";
                                                                  else
                                                                      return pi.ObservacionesFA;
                                                              }
                                                          }
                                                      })(),
                                                      FechaIngresoPago = pi.FechaIngresoPago.HasValue ? pi.FechaIngresoPago.Value.ToString() : "",
                                                      FechaIngresoInterventor = pi.FechaIngresoInterventor.HasValue ? pi.FechaIngresoInterventor.Value.ToString() : "",
                                                      FechaAprobacionInterventor = pi.FechaAprobacionInterventor.HasValue ? pi.FechaAprobacionInterventor.Value.ToString() : "",
                                                      FechaIngresoCoordinacion = pi.FechaIngresoCoordinador.HasValue ? pi.FechaIngresoCoordinador.Value.ToString() : "",
                                                      FechaAprobacionORechazoCoordinador = pi.FechaAprobacionORechazoCoordinador.HasValue ? pi.FechaAprobacionORechazoCoordinador.ToString() : "",
                                                      FechaRespuestaFiduciaria = pi.FechaRespuestaFiduciaria.HasValue ? pi.FechaRespuestaFiduciaria.Value.ToString() : "",
                                                      UltimoReintegro = consultas.Db.Reintegros.Any(filter1 => filter1.CodigoPago == pi.Id_PagoActividad) ? consultas.Db.Reintegros.Where(filter => filter.CodigoPago == pi.Id_PagoActividad).OrderByDescending(FilterOrder => FilterOrder.FechaIngreso).FirstOrDefault().ValorReintegro : 0
                                                  }).ToList();

                    ExportDataSetToExcel(dt, p.Id_proyecto, nomConvocatoria, fecha);

                }
                catch (Exception ex)
                {
                    string url = Request.Url.ToString();

                    string mensaje = ex.Message.ToString();
                    string data = ex.Data.ToString();
                    string stackTrace = ex.StackTrace.ToString();
                    string innerException = ex.InnerException == null ? "" : ex.InnerException.Message.ToString();

                    // Log the error
                    ErrHandler.WriteError(mensaje, url, data, stackTrace, innerException, usuario.Email, usuario.IdContacto.ToString());
                    //throw;
                }
            }
        }
        private void ExportDataSetToExcel(List<dataDescargaPagos> descargaPagos, int idProyecto, string nomConvocatoria, string fecha)
        {
            // file location
            var ruta = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                            + @"ExportarArchivosHIS\" + nomConvocatoria + @"\";

            var excelFileName = new FileInfo(ruta + @"\Presupuesto_" + fecha + @"\" + idProyecto + "_DescargaPagos.xlsx");

            if (!excelFileName.Directory.Exists)
                Directory.CreateDirectory(excelFileName.Directory.ToString());

            if (excelFileName.Exists)
            {
                excelFileName.Delete();
            }

            //lets create the file
            using (var excelFile = new ExcelPackage(excelFileName))
            {
                // add a worksheet              

                var sheet = excelFile.Workbook.Worksheets.Add("DescargaPagos");
                sheet.Name = "DescargaPagos";

                int col = 1, rowindex = 1;

                sheet.Cells[rowindex, col++].Value = "Codigo";
                sheet.Cells[rowindex, col++].Value = "NombrePago";
                sheet.Cells[rowindex, col++].Value = "FechaInterventor";
                sheet.Cells[rowindex, col++].Value = "CantidadDinero";
                sheet.Cells[rowindex, col++].Value = "Estado";
                sheet.Cells[rowindex, col++].Value = "TipoIdentificacion";
                sheet.Cells[rowindex, col++].Value = "Identificacion";
                sheet.Cells[rowindex, col++].Value = "Nombre";
                sheet.Cells[rowindex, col++].Value = "Apellido";
                sheet.Cells[rowindex, col++].Value = "RazonSocial";
                sheet.Cells[rowindex, col++].Value = "FechaRtaFA";
                sheet.Cells[rowindex, col++].Value = "ValorReteFuente";
                sheet.Cells[rowindex, col++].Value = "ValorReteIVA";
                sheet.Cells[rowindex, col++].Value = "ValorReteICA";
                sheet.Cells[rowindex, col++].Value = "OtrosDescuentos";
                sheet.Cells[rowindex, col++].Value = "ValorPagado";
                sheet.Cells[rowindex, col++].Value = "CodigoPago";
                sheet.Cells[rowindex, col++].Value = "ObservacionesFA";
                sheet.Cells[rowindex, col++].Value = "FechaIngresoPago";
                sheet.Cells[rowindex, col++].Value = "FechaIngresoInterventor";
                sheet.Cells[rowindex, col++].Value = "FechaAprobacionInterventor";
                sheet.Cells[rowindex, col++].Value = "FechaIngresoCoordinacion";
                sheet.Cells[rowindex, col++].Value = "FechaAprobacionORechazoCoordinador";
                sheet.Cells[rowindex, col++].Value = "FechaRespuestaFiduciaria";
                sheet.Cells[rowindex, col++].Value = "UltimoReintegro";

                rowindex = 2;

                foreach (var r in descargaPagos)
                {
                    col = 1;
                    sheet.Cells[rowindex, col++].Value = r.codigo;
                    sheet.Cells[rowindex, col++].Value = r.NombrePago;
                    sheet.Cells[rowindex, col++].Value = r.FechaInterventor;
                    sheet.Cells[rowindex, col++].Value = r.CantidadDinero;
                    sheet.Cells[rowindex, col++].Value = r.Estado;
                    sheet.Cells[rowindex, col++].Value = r.TipoIdentificacion;
                    sheet.Cells[rowindex, col++].Value = r.Identificacion;
                    sheet.Cells[rowindex, col++].Value = r.Nombre;
                    sheet.Cells[rowindex, col++].Value = r.Apellido;
                    sheet.Cells[rowindex, col++].Value = r.RazonSocial;
                    sheet.Cells[rowindex, col++].Value = r.FechaRtaFA;
                    sheet.Cells[rowindex, col++].Value = r.ValorReteFuente;
                    sheet.Cells[rowindex, col++].Value = r.ValorReteIVA;
                    sheet.Cells[rowindex, col++].Value = r.ValorReteICA;
                    sheet.Cells[rowindex, col++].Value = r.OtrosDescuentos;
                    sheet.Cells[rowindex, col++].Value = r.ValorPagado;
                    sheet.Cells[rowindex, col++].Value = r.CodigoPago;
                    sheet.Cells[rowindex, col++].Value = r.ObservacionesFA;
                    sheet.Cells[rowindex, col++].Value = r.FechaIngresoPago;
                    sheet.Cells[rowindex, col++].Value = r.FechaIngresoInterventor;
                    sheet.Cells[rowindex, col++].Value = r.FechaAprobacionInterventor;
                    sheet.Cells[rowindex, col++].Value = r.FechaIngresoCoordinacion;
                    sheet.Cells[rowindex, col++].Value = r.FechaAprobacionORechazoCoordinador;
                    sheet.Cells[rowindex, col++].Value = r.FechaRespuestaFiduciaria;
                    sheet.Cells[rowindex, col++].Value = r.UltimoReintegro;
                    rowindex++;
                }

                sheet.Cells.AutoFitColumns();

                excelFile.Save();
            }
        }
        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data)
        {

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)

                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            foreach (T item in data)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;

                table.Rows.Add(row);
            }

            return table;
        }

        ProyectoController proyectoController = new ProyectoController();

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

        private List<InfoGeneralProyecto> ListadoProyectos(int _codConvocatoria)
        {
            return proyectoController.ListadoProyectosSinTenerEnCuentaEstado(_codConvocatoria);
        }

        private List<InfoGeneralProyecto> ListadoProyectosEvaluacion(int _codConvocatoria)
        {
            return proyectoController.ListadoProyectosEvaluados(_codConvocatoria);
        }

        private string saltoLineaGuion()
        {
            return "-------------------------";
        }

        private string tituloActividadesPlanOperativo()
        {
            return "IdPlan|Item|Actividad";
        }
        private string tituloResumenMesxMesActividad()
        {
            return "IdPlan|Actividad|Mes 1 Fondo Emprender|Mes 1 Emprendedor|Mes 2 Fondo Emprender|Mes 2 Emprendedor" +
                "|Mes 3 Fondo Emprender|Mes 3 Emprendedor|Mes 4 Fondo Emprender|Mes 4 Emprendedor|Mes 5 Fondo Emprender|Mes 5 Emprendedor" +
                "|Mes 6 Fondo Emprender|Mes 6 Emprendedor|Mes 7 Fondo Emprender|Mes 7 Emprendedor|Mes 8 Fondo Emprender|Mes 8 Emprendedor" +
                "|Mes 9 Fondo Emprender|Mes 9 Emprendedor|Mes 10 Fondo Emprender|Mes 10 Emprendedor|Mes 11 Fondo Emprender|Mes 11 Emprendedor" +
                "|Mes 12 Fondo Emprender|Mes 12 Emprendedor|Mes 13 Fondo Emprender|Mes 13 Emprendedor|Mes 14 Fondo Emprender|Mes 14 Emprendedor" +
                "|Mes 15 Fondo Emprender|Mes 15 Emprendedor|Mes 16 Fondo Emprender|Mes 16 Emprendedor|Mes 17 Fondo Emprender|Mes 17 Emprendedor" +
                "|Mes 18 Fondo Emprender|Mes 18 Emprendedor|Mes 19 Fondo Emprender|Mes 19 Emprendedor|Mes 20 Fondo Emprender|Mes 20 Emprendedor" +
                "|Aporte Fondo Emprender|Aporte Emprendedor";
        }

        private string tituloAvancenMesxMesActividad()
        {
            return "IdPlan|Actividad|Mes|Fecha Avance|Observaciones Emprendedor|Fecha Aprobacion|Observacion Interventor" +
                "|Actividad Aprobada";
        }
        private string tituloHistoricoAvanceActividad()
        {
            return "IdPlan|Actividad|Mes|Observacion Emprendedor|Fecha Avance Emprendedor|Observacion Interventor|Fecha Avance Interventor" +
                "|Valor Fondo Emprender|Valor Aporte Emprendedor|Aprobado|Registrado por";
        }
        private string tituloAvancenMesxMesCargo()
        {
            return "IdPlan|Cargo|Mes|Fecha Avance|Observaciones Emprendedor|Fecha Aprobacion|Observacion Interventor" +
                "|Actividad Aprobada";
        }
        private string tituloAvancenMesxMesProducto()
        {
            return "IdPlan|Producto|Mes|Fecha Avance|Observaciones Emprendedor|Fecha Aprobacion|Observacion Interventor" +
                "|Actividad Aprobada";
        }
        private string tituloHistoricoAvanceNomina()
        {
            return "IdPlan|Cargo|Mes|Observacion Emprendedor|Fecha Avance Emprendedor|Observacion Interventor|Fecha Avance Interventor" +
                "|Valor Sueldo|Valor Prestaciones|Aprobado|Registrado por";
        }
        private string tituloHistoricoAvanceProducion()
        {
            return "IdPlan|Producto|Mes|Observacion Emprendedor|Fecha Avance Emprendedor|Observacion Interventor|Fecha Avance Interventor" +
                "|Ventas|Ingresos|Aprobado|Registrado por";
        }
        private string tituloIndicadoresGenericos()
        {
            return "IdPlan|Nombre|Descripcion|Evaluacion|Observacion";
        }
        private string tituloRiegosInterventor()
        {
            return "IdPlan|Eje Funcional|Riesgo|Mitigacion|Observacion";
        }
        private string tituloConceptoFinalRecomendaciones()
        {
            return "IdPlan|Dificultad Central|Observaciones";
        }
        private string tituloContrato()
        {
            return "IdPlan|Numero de Contrato de Colaboracion Empresarial|Fecha Acta de Inicio" +
                "|Objeto|Fecha del AP|No. Poliza de Seguro de Vida|Valor Inicial|Plazo" +
                "|Numero del AP Presupuestal|Fecha Firma del Contrato|Compañia Seguro de Vida";
        }
        private string tituloRegistroMercantil()
        {
            return "IdPlan|Nombre|Identificacion|Email|Telefono" +
                "|Objeto Social|Capital Social|Tipo de Sociedad|Codigo CIIU|Numero de Escritura Publica" +
                "|Domicilio de la Empresa|Departamento|Ciudad|Telefono|Email|Nit" +
                "|Es Regimen Especial|Norma|Fecha de Norma" +
                "|Es Contribuye|Norma|Fecha de Norma" +
                "|Es Autoretenedor|Norma|Fecha de Norma" +
                "|Es Declarante|Norma|Fecha de Norma" +
                "|Es Exento de Retencion|Norma|Fecha de Norma" +
                "|Es Gran Contribuyente|Norma|Fecha de Norma";
        }
        private string tituloSocios()
        {
            return "IdPlan|Nombre|Identificacion|Email|Telefono";
        }
        private string titulosNominaMesxMesxCargo()
        {
            return "IdPlan|Puesto de Trabajo|Mes 1 Sueldo|Mes 1 Prestaciones|Mes 2 Sueldo|Mes 2 Prestaciones" +
                "|Mes 3 Sueldo|Mes 3 Prestaciones|Mes 4 Sueldo|Mes 4 Prestaciones|Mes 5 Sueldo|Mes 5 Prestaciones" +
                "|Mes 6 Sueldo|Mes 6 Prestaciones|Mes 7 Sueldo|Mes 7 Prestaciones|Mes 8 Sueldo|Mes 8 Prestaciones" +
                "|Mes 9 Sueldo|Mes 9 Prestaciones|Mes 10 Sueldo|Mes 10 Prestaciones|Mes 11 Sueldo|Mes 11 Prestaciones" +
                "|Mes 12 Sueldo|Mes 12 Prestaciones|Mes 13 Sueldo|Mes 13 Prestaciones|Mes 14 Sueldo|Mes 14 Prestaciones" +
                "|Mes 15 Sueldo|Mes 15 Prestaciones|Mes 16 Sueldo|Mes 16 Prestaciones|Mes 17 Sueldo|Mes 17 Prestaciones" +
                "|Mes 18 Sueldo|Mes 18 Prestaciones|Mes 19 Sueldo|Mes 19 Prestaciones|Mes 20 Sueldo|Mes 20 Prestaciones" +
                "|Sueldo Total|Prestaciones Total";
        }
        private string titulosProduccionMesxMes()
        {
            return "IdPlan|Nombre del Producto|Mes 1 Cantidad|Mes 1 Costo|Mes 2 Cantidad|Mes 2 Costo" +
                "|Mes 3 Cantidad|Mes 3 Costo|Mes 4 Cantidad|Mes 4 Costo|Mes 5 Cantidad|Mes 5 Costo" +
                "|Mes 6 Cantidad|Mes 6 Costo|Mes 7 Cantidad|Mes 7 Costo|Mes 8 Cantidad|Mes 8 Costo" +
                "|Mes 9 Cantidad|Mes 9 Costo|Mes 10 Cantidad|Mes 10 Costo|Mes 11 Cantidad|Mes 11 Costo" +
                "|Mes 12 Cantidad|Mes 12 Costo|Mes 13 Cantidad|Mes 13 Costo|Mes 14 Cantidad|Mes 14 Costo" +
                "|Mes 15 Cantidad|Mes 15 Costo|Mes 16 Cantidad|Mes 16 Costo|Mes 17 Cantidad|Mes 17 Costo" +
                "|Mes 18 Cantidad|Mes 18 Costo|Mes 19 Cantidad|Mes 19 Costo|Mes 20 Cantidad|Mes 20 Costo" +
                "|Cantidad Total|Costo Total";
        }

        private string titulosVentasMesxMes()
        {
            return "IdPlan|Nombre del Producto|Mes 1 Ventas|Mes 1 Ingreso|Mes 2 Ventas|Mes 2 Ingreso" +
                "|Mes 3 Ventas|Mes 3 Ingreso|Mes 4 Ventas|Mes 4 Ingreso|Mes 5 Ventas|Mes 5 Ingreso" +
                "|Mes 6 Ventas|Mes 6 Ingreso|Mes 7 Ventas|Mes 7 Ingreso|Mes 8 Ventas|Mes 8 Ingreso" +
                "|Mes 9 Ventas|Mes 9 Ingreso|Mes 10 Ventas|Mes 10 Ingreso|Mes 11 Ventas|Mes 11 Ingreso" +
                "|Mes 12 Ventas|Mes 12 Ingreso|Mes 13 Ventas|Mes 13 Ingreso|Mes 14 Ventas|Mes 14 Ingreso" +
                "|Mes 15 Ventas|Mes 15 Ingreso|Mes 16 Ventas|Mes 16 Ingreso|Mes 17 Ventas|Mes 17 Ingreso" +
                "|Mes 18 Ventas|Mes 18 Ingreso|Mes 19 Ventas|Mes 19 Ingreso|Mes 20 Ventas|Mes 20 Ingreso" +
                "|Ventas Total|Ingreso Total";
        }
        private string titulosEvaluacion()
        {
            return "IdProyecto|Localizacion|Sector|Resumen concepto general" +
                "|MercadoO|MercadoP1|ClientesO|ClientesP1" +
                "|FuerzaO|FuerzaP1|FuerzaP2|FuerzaP3" +
                "|TendendenciasO|TendendenciasP1|TendendenciasP2|TendendenciasP3|TendendenciasP4" +
                "|CompetenciaO|CompetenciaP1|CompetenciaP2|CompetenciaP3" +
                "|PValorO|PValorP1|PValorP2|PValorP3|PValorP4|PValorP5" +
                "|ValidacionO|ValidacionP1|ValidacionP2|ValidacionP3" +
                "|AntecedentesO|AntecedentesP1|AntecedentesP2|AntecedentesP3" +
                "|ComercializacionO|ComercializacionP1|ComercializacionP2|ComercializacionP3|ComercializacionP4|ComercializacionP5" +
                "|NormatividadO|NormatividadP1|NormatividadP2" +
                "|OperacionO|OperacionP1|OperacionP2|OperacionP3|OperacionP4|OperacionP5|OperacionP6|OperacionP7" +
                "|EquipoO|EquipoP1|EquipoP2|EquipoP3|EquipoP4" +
                "|EstrategiasO|EstrategiasP1|EstrategiasP2" +
                "|ImproductivoO|ImproductivoP1|ImproductivoP2|ImproductivoP3" +
                "|SostenibilidadO|SostenibilidadP1|SostenibilidadP2|SostenibilidadP3|SostenibilidadP4" +
                "|RiesgosO|RiesgosP1|RiesgosP2|RiesgosP3" +
                "|PlanO|PlanP1|PlanP2" +
                "|IndicadoresO|IndicadoresP1" +
                "|Indicadores§Decripcion§Valor" +
                "|Inversiones fijas§Nombre§Detalle§Fuente de financiacion§Total solicitado§Porcentaje§Total recomendado§Porcentaje" +
                "|Capital de trabajo primer año de operacion§Nombre§Detalle§Fuente de financiacion§Total solicitado§Porcentaje§Total recomendado§Porcentaje" +
                "|Inversiones  diferidas§Nombre§Detalle§Fuente de financiacion§Total solicitado§Porcentaje§Total recomendado§Porcentaje" +
                "|Integrantes§Nombre§Emprendedor§Otro§Aporte total§Aporte en dinero§Aporte en especie§Clase de especie" +
                "|Observacion equipo de trabajo|Recursos solicitados|Valor recomendado" +
                "|Riesgos§Riesgo§Mitigacion" +
                "|Viable|Conceptos|Justificacion" +
                "|ejecucionpresupuestal-emprendedor|ejecucionpresupuestal-evaluador|IDH-emprendedor|IDH-evaluador" +
                "|Contrapartida-emprendedor|Contrapartida-evaluador|Ventas-emprendedor|Ventas-evaluador|Periodoemproductivo -emprededor" +
                "|Periodoemproductivo -evaluador|AportesEmprendedor-emprededor|AportesEmprendedor-evaluador" +
                "|Produccion§Producto representativo§Producto representativo evaluacion§Producto§Unidades§Datos evaluador}" +
                "|Mercadeo§Cantidad§Actividad§Datos evaluador" +
                "|Empleos§Cantidad§Cargo§Datos evaluador";
        }

        private string titulos()
        {
            return "IdProyecto|Nombre proyecto|Codigo CIIU|Sector|Subsector|IdInstitucion|Nombre de institución" +
                "|Nombre de unidad de emprendimiento|IdMunicipio|Nombre Municipio|Fecha de nacimiento|Genero|Nivel de estudio" +
                "|Programa|Institucion|Estado" +
                "|Cliente§Perfil§Localización§Justificación" + //Cliente
                "|Perfil Consumidor|Cliente|Consumidores" +
                "|Oportunidad de mercado" +
                "|Competidores§Nombre§Localización§Productos y servicios§Precios§Logística distribución§Otro Cual" + //Competidores
                "|Concepto del negocio|Componente innovador|Producto o servicio|Proceso" +
                "|Aceptación en el mercado|Aspecto técnico productivo|Aspecto comercial|Aspecto Legal" +
                "|Producto§Producto especifico§Nombre comercial§Unidad de medida§Descripción general§Condiciones especiales§Composición§Otros§Producto más representativo (SI/NO)" + //Producto
                "|Estrategia de ingresos" +
                "|Condiciones Comerciales§Cliente§Volúmenes y frecuencia§Características compra§Sitio de compra§Forma de pago§Precio§Requisitos post venta§Garantías§Margen de comercialización" + //Condiciones Comerciales
                "|Dónde compra|Características para la compra|Cual es la frecuencia de compra" +
                "|Precio" +
                "|Listado de productos§Nombre de producto§Unidad de medida§Forma de pago§Justificacion§IVA" + //Listado de productos
                "|Ingresos por venta§Periodo§Año" + //Ingresos por venta
                "|Normatividad empresarial|Normatividad tributaria|Normatividad técnica|Normatividad Laboral" +
                "|Normatividad ambiental|Registro de marca|Condiciones técnicas para operación del negocio" +
                "|Es necesario lugar físico|Justificación lugar físico" +
                "|Infraestructura adecuaciones§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Infraestructura adecuaciones
                "|Maquinaria y equipos§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Maquinaria y equipos  
                "|Equipo de comunicación y computación§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Equipo de comunicación y computación
                "|Muebles y enseres y otros§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Muebles y enseres y otros
                "|Otros§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Otros
                "|Gastos preoperativos§Descripción§Cantidad§Valor unitario§Fuente de financiación§Requisitos técnicos" + //Gastos preoperativos
                "|Detalle de las condiciones técnicas|Contempla importación" +
                "|Justificación contempla importación" +
                "|Detalle de los activos|Financiación mayor valor" +
                "|Proceso de producción§Nombre de producto§Proceso de producción producto" + //Proceso de producción
                "|Capacidad productiva de la empresa" +
                "|Perfil emprendedor§Perfil§Rol" + //Perfil emprendedor
                "|Cargos operación§Nombre de cargo§Funciones principales§Perfil§Experiencia general§Experiencia específica§Tipo de contratación" + //Cargos operación
                "§Dedicación de tiempo§Unidad de medida en tiempo§Tiempo de vinculación§Valor remuneración§Otros gastos§Valor con prestaciones" + //Cargos operación
                "§Remuneración primer año§Valor solicitado fondo emprender§Aportes emprendedores§Ingresos por ventas" + //Cargos operación
                "|Nombre estrategia promoción|Propósito promoción" +
                "|Actividades de promoción§Actividad§Recurso requerido§Mes de ejecución§Costo§Responsable" + //Actividades de promoción
                "|Nombre estrategia comunicación|Propósito estrategia de comunicación" +
                "|Actividades de comunicación§Actividad§Recurso requerido§Mes de ejecución§Costo§Responsable" + //Actividades de comunicación
                "|Nombre estrategia distribución" +
                "|Propósito estrategia de distribución" +
                "|Actividades de distribución§Actividad§Recurso requerido§Mes de ejecución§Costo§Responsable" + //Actividades de distribución
                "|Periodo de arranque|Periodo improductivo|Actores externos críticos" +
                "|Factores externos que afectan la operación|Concepto del negocio|Empleos|Contrapartidas|Ejecución presupuestal|Ventas" +
                "|Mercadeo|Periodo improductivo|IDH|Recursos aportados emprendedor|Video del emprendedor" +
                "|Consumos por unidad de producto§Tipo de materia prima§Materia prima§Unidad§Canidad Presentación§Margen de Desperdicio (%)" + //Consumos por unidad de producto
                "|Tabla de costos de producción en pesos (incluido IVA)§Tipo de Insumo§Año" + //Tabla de costos de producción en pesos (incluido IVA)
                "|Proyección de Compras (Unidades)§Tipo de Insumo§Insumo§Año" + //Proyección de Compras (Unidades)
                "|Proyección de Compras (Pesos)§Tipo de Insumo§Insumo§Año" + //Proyección de Compras (Pesos)
                "|Gastos de Apuesta en Marcha§Descripción§Valor" + //Gastos de Apuesta en Marcha
                "|Gastos Anuales de Administración§Descripción§Valor" + //Gastos Anuales de Administración
                "|Recursos solicitados al fondo emprender en (smlv)" +
                "|Aporte de los Emprendedores§Nombre§Valor§Detalle" + //Aporte de los Emprendedores
                "|Recursos de Capital§Cuantía§Plazo§Forma de Pago§Interes (Nominal/Anual)§Destinación" + //Recursos de Capital
                "|Proyeccion de Ingresos por Ventas§Producto§Año" + //Proyeccion de Ingresos por Ventas
                "|Índice de Actualización monetaria" +
                "|Inversiones Fijas y Diferidadas§Concepto§Valor§Mes§Fuente de financiación" + //Inversiones Fijas y Diferidadas
                "|Costo de Puesta en Marcha§Descripción§Valor" + //Costo de Puesta en Marcha
                "|Costos Anualizados Administrativos§Descripción§Año" + //Costos Anualizados Administrativos
                "|Gastos de Personal§Cargo§Año" + //Gastos de Personal
                "|Capital de Trabajo§Componente§Valor§Fuente de financiación§Observación" + //Capital de Trabajo
                "|Cronogramas de Actividades§Item§Actividad§Mes§Fondo§Emprendedor" + //Cronogramas de Actividades
                "|Plan Nacional de Desarrollo|Plan Regional de Desarrollo" +
                "|Cluster o Cadena Productiva" +
                "|Empleo§Tipo Empleo directo§Cargo§Sueldo Mes§Generado en el primer año§Edad entre 18 y 24 años§Desplazado por la violencia" +
                "§Madre Cabeza de Familia§Minoría Etnica (Indigena o Negritud)§Recluido Carceles INPEC" +
                "§Desmovilizado o Reinsertado§Discapacitado§Desvinculado de Entidades del Estado" + //Empleo
                "|Empleos a Generar en el Primer Año|Empleos a Generar en la Totalidad del Proyecto" +
                "|Empleos Indirectos" +
                "|Emprendedores§Nombre§Beneficiario del Fondo emprender§Participación Accionaria (%)"; //Emprendedores
        }

        private bool eliminarArchivosTemporales(string _nomConvocatoria)
        {
            bool eliminados = true;

            var basePath = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual")
                        + @"ExportarArchivosHIS\" + _nomConvocatoria + @"\";

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

        private bool copiarArchivosGeneradosSFTP(string rutaOriginal, string nomConvocatoria, string fechaArchivo, string tipoArchivo)
        {
            bool copiado = true;
            string rutaOriginalRUT = rutaOriginal;
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
                if (tipoArchivo == "FOR")
                {
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

                //Copiar Excel de Pagos y RUT
                if (tipoArchivo == "INT")
                {
                    //Excel de pagos
                    rutaSFTP = rutaSFTP + @"Presupuesto_" + fechaArchivo + @"\";
                    rutaOriginal = rutaOriginal + @"Presupuesto_" + fechaArchivo + @"\";

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

                    //RUT
                    rutaSFTP = ConfigurationManager.AppSettings.Get("RutaSFTPSena") + @"\" + nomConvocatoria + @"\";
                    rutaOriginal = rutaOriginalRUT;

                    rutaSFTP = rutaSFTP + @"RUT_" + fechaArchivo + @"\";
                    rutaOriginal = rutaOriginal + @"RUT_" + fechaArchivo + @"\";

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

        private class dataDescargaPagos
        {
            public int codigo { get; set; }
            public string NombrePago { get; set; }
            public string FechaInterventor { get; set; }
            public string CantidadDinero { get; set; }
            public string Estado { get; set; }
            public string TipoIdentificacion { get; set; }
            public string Identificacion { get; set; }
            public string ValorReteFuente { get; set; }
            public string ValorReteIVA { get; set; }
            public string ValorReteICA { get; set; }
            public string OtrosDescuentos { get; set; }
            public string ValorPagado { get; set; }
            public string ObservacionesFA { get; set; }
            public string FechaIngresoPago { get; set; }
            public string FechaIngresoInterventor { get; set; }
            public string FechaAprobacionInterventor { get; set; }
            public string FechaIngresoCoordinacion { get; set; }
            public string FechaAprobacionORechazoCoordinador { get; set; }
            public string FechaRespuestaFiduciaria { get; set; }
            public decimal UltimoReintegro { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public string RazonSocial { get; set; }
            public string FechaRtaFA { get; set; }
            public string CodigoPago { get; set; }
        }
    }
}