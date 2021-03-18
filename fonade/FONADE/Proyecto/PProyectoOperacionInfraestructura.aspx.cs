using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;
using Datos;
using Newtonsoft.Json;
using System.Web;
using System.Web.UI;
using System.Data;
using Fonade.Negocio;

namespace Fonade.FONADE.Proyecto
{
    public partial class PProyectoOperacionInfraestructura : Negocio.Base_Page
    {
        public string codProyecto;
        public int txtTab = Constantes.CONST_Infraestructura;
        public string codConvocatoria;
        public ProyectoOperacionInfraestructura poi;
        public static Consultas Varconsulta;
        //double Total = 0;
        public bool bandera;
        String txtSQL;
        Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        Boolean bRealizado;

        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, HttpContext.Current.Session["CodProyecto"]); } } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                else
                    return
                    new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(Convert.ToInt32(HttpContext.Current.Session["CodProyecto"].ToString())).Count > 0
                    && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

            //Consultar si está "realizado".
            //bRealizado = esRealizado(txtTab.ToString(), codProyecto, codConvocatoria);
            bRealizado = esRealizado(txtTab.ToString(), codProyecto, "");

            //if (!IsPostBack)
            //{
                #region Determinar ocultar algunos campos.

                //Mostrar control Post It.
                if (esMiembro && !bRealizado)
                {
                    div_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;

                    div_Post_It2.Visible = true;
                Post_It2._mostrarPost = true;
                }

                //Mostrar controles de adición.
                if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                {
                    divcrear.Visible = true;
                    imgBtn_Adicionar.Visible = true;
                    lnk_Adicionar.Visible = true;
                    //Mostrar botón "Guardar".
                    btm_guardarCambios.Visible = true;
                    div_Post_It1.Visible = true;
                Post_It1._mostrarPost = true;
                    div_Post_It2.Visible = true;
                Post_It2._mostrarPost = true;
                }
                //else
                //{
                //    Post_It1.Visible = false;
                //    Post_It2.Visible = false;
                //}

                #endregion

                // 2014/11/27 RAlvaradoT 
                // Se comenta esto porque para ambos hace lo mismo, la unica diferencia es que div le cambia el style
                //bandera = habilitarGuardadoEval(codProyecto, codConvocatoria, txtTab, usuario.CodGrupo);
                //if (!bandera)
                //{
                //    btm_guardarCambios.Visible = false;
                //    txt_parametrosT.Enabled = false;
                //    divcrear.Style.Add("display", "none");
                //}
                //else
                //{
                //    btm_guardarCambios.Visible = false;
                //    txt_parametrosT.Enabled = false;
                //    divcrear.Style.Add("display", "block");
                //}

                //Comentado "genera datos en el GridView".
                GenerarTablaInfraestructura();
                
                if (!IsPostBack)
                {
                    ObtenerDatosUltimaActualizacion();
                    txt_parametrosT.Text = CargarTexto_ParametrosTecnicosEspeciales();
                }
                
                
                //Determinar la visualización de código HTML.
                //if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                if (btm_guardarCambios.Visible)
                {
                    div_param.Visible = false;
                    txt_parametrosT.Visible = true;
                    //txt_parametrosT.Text = CargarTexto_ParametrosTecnicosEspeciales();
                }
                else
                {
                    // 2014/12/05 RalvaradoT
                    MostarDivConsultaInfo();
                }

                // 2014/12/05 RalvaradoT
                if(chk_realizado.Checked)
                {
                    MostarDivConsultaInfo();
                }
            //}
        }

        /// <summary> Metodo que muestra la informacion en el div
        /// </summary>
        /// <remarks>2014/12/05 RalvaradoT</remarks>
        private void MostarDivConsultaInfo()
        {
            div_param.Visible = true;
            txt_parametrosT.Visible = false;
            div_param.InnerHtml = CargarTexto_ParametrosTecnicosEspeciales();
        }

        #region Metodos Infraestructura

        #region ObtenerInfraestructura

        //public void ObtenerInfraestructura()
        //{
        //    string proyecto = !string.IsNullOrEmpty(codProyecto) ? codProyecto : "0";
        //    List<MD_ObtenerInfraestructuraResult> listInfraestructura = consultas.ObtenerInfraestructura(proyecto);
        //    if (listInfraestructura.Any())
        //    {
        //        Grv.DataSource = listInfraestructura;
        //        Grv.DataBind();
        //        divtotal.Style.Add("display", "block");
        //    }
        //    else divtotal.Style.Add("display", "none");

        //    var query = from p in consultas.Db.ProyectoOperacionInfraestructuras
        //                where p.CodProyecto == Convert.ToInt32(codProyecto)
        //                select p;

        //    if (query.Any())
        //    {
        //        txt_parametrosT.Text = query.First().ParametrosTecnicos;
        //    }
        //}

        //[WebMethod]
        public static string Eliminar(string codigo)
        {
            string mensajeDeError = string.Empty;



            Varconsulta = new Consultas();
            var entity = Varconsulta.Db.ProyectoInfraestructuras.Single(
                p => p.Id_ProyectoInfraestructura == Convert.ToInt64(codigo));



            if (entity.Id_ProyectoInfraestructura != 0)
            {
                Varconsulta.Db.ProyectoInfraestructuras.DeleteOnSubmit(entity);
                Varconsulta.Db.SubmitChanges();

                mensajeDeError = "ok";
            }
            else
            {
                mensajeDeError = "El registro no se puede eliminar";
            }





            return JsonConvert.SerializeObject(new
                                                   {
                                                       mensaje = mensajeDeError
                                                   });
        }

        //[WebMethod]
        public static string Editar(ProyectoInfraestructura entidad)
        {
            string mensajeDeError = string.Empty;

            Varconsulta = new Consultas();
            var entity = Varconsulta.Db.ProyectoInfraestructuras.Single(
                p => p.Id_ProyectoInfraestructura == Convert.ToInt64(entidad.Id_ProyectoInfraestructura));

            if (entity.Id_ProyectoInfraestructura != 0)
            {

                entity.CodTipoInfraestructura = entidad.CodTipoInfraestructura;
                entity.codProyecto = entidad.codProyecto;
                entity.NomInfraestructura = entidad.NomInfraestructura;

                Varconsulta.Db.ProyectoInfraestructuras.DeleteOnSubmit(entity);

                Varconsulta.Db.SubmitChanges();

                mensajeDeError = "ok";
            }
            else
            {
                mensajeDeError = "El registro no se puede eliminar";
            }





            return JsonConvert.SerializeObject(new
            {
                mensaje = mensajeDeError
            });
        }

        #endregion

        #endregion

        //private ProyectoOperacionInfraestructura getProyectoOperacionInfraestructura()
        //{
        //    var query = from tp in consultas.Db.ProyectoOperacionInfraestructuras
        //                 where tp.CodProyecto == Convert.ToInt32(codProyecto)
        //                 select tp;

        //    return query.FirstOrDefault();

        //}

        //private void definirCampos()
        //{
        //    if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
        //    {
        //        btm_guardarCambios.Visible = true;
        //        //btn_limpiarCampos.Visible = true;
        //    }
        //    else
        //    {
        //        btm_guardarCambios.Visible = false;
        //        //btn_limpiarCampos.Visible = false;
        //    }

        //    if (poi != null)
        //    {
        //        procesarCampo(ref txt_parametrosT, ref txt_parametrosT_HtmlEditorExtender, ref panel_parametrosT, poi.ParametrosTecnicos, true, false, "");

        //        double Total = 0;
        //        string table = "";

        //        table += "<table width='780' border='0' cellspacing='0' cellpadding='0'>";
        //        table += "<tr><td width='19'>&nbsp;</td><td width='761'>";
        //        //Validar si se pinta el postit If Session('Miembro')=1  and not bRealizado Then table += 			fnPintarPostIt('Infraestructura')
        //        table += "<table width='95%' align=Center border='0' cellpadding='0' cellspacing='0'>";
        //        table += "<tr><td align='left' valign='top' width='98%'>";

        //        if (codProyecto != "")
        //        {
        //            List<Datos.TipoInfraestructura> tipos = (from ti in consultas.Db.TipoInfraestructuras
        //                         orderby ti.Id_TipoInfraestructura
        //                         select ti).ToList();

        //            table += "<table width='98%' border='0' cellspacing='1' cellpadding='4'>";
        //            if (miembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado)
        //            {
        //                table += "<tr>";
        //                table += "<td colspan=9 align='left'><a href=\"javascript:createWindow('CatalogoInfraestructura.asp?Accion=Nuevo&CodProyecto=" + codProyecto + "')\" class='TituloDestacados'><img src='../../Images/icoAdicionarUsuario.gif' width='14' height='14' border=0></a>";
        //                table += "<a href=\"javascript:createWindow('CatalogoInfraestructura.asp?Accion=Nuevo&CodProyecto='" + codProyecto + "')\" class='TituloDestacados'>Adicionar Infraestructura</a></td>";
        //                table += "</tr>";
        //            }

        //            table += "<tr class='Titulo' bgcolor='#3D5A87'>";
        //            table += "<td width='3%' align='left'>&nbsp;</td>";
        //            table += "<td align='left'  class='tituloTabla'>Nombre</td>";
        //            table += "<td align='center' width='14%' class='tituloTabla'>Fecha de Compra</td>";
        //            table += "<td align='center' width='13%' class='tituloTabla'>Periodos de Amortización</td>";
        //            table += "<td align='center' width='10%' class='tituloTabla'>Sis. de Depreciación y/o Agotamiento</td>";
        //            table += "<td align='center' width='10%' class='tituloTabla'>% Crédito</td>";
        //            table += "<td align='center' width= '8%' class='tituloTabla'>Unidad</td>";
        //            table += "<td align='center' width='10%' class='tituloTabla'>Cantidad</td>";
        //            table += "<td align='center' width='12%' class='tituloTabla'>Precio/Unidad</td>";
        //            table += "</tr>";

        //            foreach (TipoInfraestructura txt in tipos)
        //            {
        //                List<ProyectoInfraestructura> datac = (from pi in consultas.Db.ProyectoInfraestructuras
        //                             join tip in consultas.Db.TipoInfraestructuras
        //                             on pi.CodTipoInfraestructura equals tip.Id_TipoInfraestructura
        //                             where pi.codProyecto == Convert.ToInt32(codProyecto)
        //                             && pi.CodTipoInfraestructura == txt.Id_TipoInfraestructura
        //                             select pi).ToList();

        //                if (datac.Count() > 0)
        //                {


        //                    table += "<tr class='Titulo'>";
        //                    table += "<td width='3%' align='left'>&nbsp;</td>";
        //                    table += "<td colspan=8 class='tituloDestacados'><b>" + txt.NomTipoInfraestructura + "</b></td>";
        //                    table += "</tr>";
        //                }
        //                foreach (ProyectoInfraestructura litem in datac)
        //                {
        //                    table += "<tr>";
        //                    if (miembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado)
        //                    {
        //                        table += "<td align='center'><a href=\"javascript:borrarInfraestructura(" + codProyecto + "," + txt.Id_TipoInfraestructura + ");\"><img src='../../Images/icoBorrar.gif' width='15' height='14'  Alt='Eliminar la Infraestructura del proyecto' border=0></a></td>";
        //                        table += "<td align='left'><a href=\"javascript:createWindow('CatalogoInfraestructura.asp?Accion=Editar&CodProyecto=" + codProyecto + "&CodInfraestructura=" + litem.Id_ProyectoInfraestructura + "')\" class='underline'>" + litem.NomInfraestructura + "</a></td>";
        //                    }
        //                    else
        //                    {
        //                        table += "<td align='center'>&nbsp;</td>";
        //                        table += "<td align='left' >" + litem.NomInfraestructura + "</td>";
        //                    }
        //                    table += "<td align='center'>" + litem.FechaCompra.ToShortDateString() + "</td>";
        //                    table += "<td align='center'>" + litem.PeriodosAmortizacion + "</td>";
        //                    table += "<td align='center'>" + litem.SistemaDepreciacion + "</td>";
        //                    table += "<td align='center'>" + litem.ValorCredito + "%</td>";
        //                    table += "<td align='center'>" + litem.Unidad + "</td>";
        //                    table += "<td align='center'>" + litem.Cantidad + "</td>";
        //                    table += "<td align='right' >" + litem.ValorUnidad + "</td>";
        //                    table += "</tr>";
        //                    Total = Total + ((litem.ValorUnidad.HasValue ? (double)litem.ValorUnidad.Value : 0) * (litem.Cantidad.HasValue ? litem.Cantidad.Value : 0));

        //                }
        //            }
        //        }
        //        table += "</td></tr>";
        //        table += "<tr><td colspan=6><td><b>Total</b></td><td colspan=2 align=Right><b>" + Total + "</b></td></tr>";
        //        table += "</table>";

        //        pnl_tabla.InnerHtml = table;

        //    }


        //}

        /// <summary>
        /// Guardar cambios.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btm_guardarCambios_Click(object sender, EventArgs e)
        {
            //Miramos si ya esxist el registro
            var query = from p in consultas.Db.ProyectoOperacionInfraestructuras
                        where p.CodProyecto == Convert.ToInt32(codProyecto)
                        select p;

            string MensajeDerror = string.Empty;

            if (query.Any())
            { consultas.CrudProyectoInfraestructura(1, Convert.ToInt32(codProyecto), txt_parametrosT.Text, ref MensajeDerror); }
            else { consultas.CrudProyectoInfraestructura(0, Convert.ToInt32(codProyecto), txt_parametrosT.Text, ref MensajeDerror); }

            //Actualizar fecha modificación del tab.
            prActualizarTab(txtTab.ToString(), codProyecto);
            ObtenerDatosUltimaActualizacion();
        }

        #region Comentados eventos del GridView.
        //protected void Grv_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        Label lvalorU = e.Row.FindControl("lvalorunidad") as Label;
        //        Label lcantidad = e.Row.FindControl("lcantidad") as Label;

        //        double dcantidad = !string.IsNullOrEmpty(lcantidad.Text) ? Convert.ToDouble(lcantidad.Text) : 0;
        //        double dvalor = !string.IsNullOrEmpty(lvalorU.Text) ? Convert.ToDouble(lvalorU.Text) : 0;

        //        if (dcantidad != 0 && dvalor != 0)
        //        {
        //            Total += dcantidad * dvalor;
        //            LblTotal.Text = string.Format("{0:N2}", Total);
        //        }
        //    }
        //}

        //protected void Grv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    Grv.PageIndex = e.NewPageIndex;
        //    ObtenerInfraestructura();
        //} 
        #endregion

        #region Métodos de .



        /// <summary>
        
        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            //String txtSQL;
            //DateTime fecha = new DateTime();
            //DataTable tabla = new DataTable();
            List<String> tabla = new List<String>();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;
            OperacionesRoleNegocio opRoleNeg = new OperacionesRoleNegocio();


            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, codProyecto);

                //Obtener número "numPostIt".
                //numPostIt = Obtener_numPostIt();
                //numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto));
                numPostIt = opRoleNeg.Obtener_numPostIt(Convert.ToInt32(codProyecto), Convert.ToInt32(usuario.IdContacto));

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), codProyecto, codConvocatoria);

                #region Obtener el rol.

                String codRole = String.Empty;

                var avk = opRoleNeg.ObtenerRolUsuario(codProyecto, usuario.IdContacto) ?? string.Empty;
                codRole = avk.ToString();
                HttpContext.Current.Session["CodRol"] = codRole;

                #endregion

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                //tabla = opRoleNeg.UltimaModificacion(codProyecto, txtTab);

                ////Si tiene datos "y debe tenerlos" ejecuta el siguiente código.
                //if (tabla.Count > 0)
                //{
                //    //Nombre del usuario quien hizo la actualización.
                //    lbl_nombre_user_ult_act.Text = tabla[0].ToUpperInvariant();

                //    #region Formatear la fecha.

                //    lbl_fecha_formateada.Text = tabla[1];

                //    #endregion

                //    //Valor "bRealziado".
                //    bRealizado = Convert.ToBoolean(tabla[2]);
                //}
                DateTime fecha = new DateTime();

                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(int.Parse(codProyecto), txtTab).FirstOrDefault();

                if (usuActualizo != null)
                {
                    lbl_nombre_user_ult_act.Text = usuActualizo.nombres.ToUpper();

                    //Convertir fecha.
                    try { fecha = Convert.ToDateTime(usuActualizo.fecha); }
                    catch { fecha = DateTime.Today; }
                    //Obtener el nombre del mes (las primeras tres letras).
                    string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                    //Obtener la hora en minúscula.
                    string hora = fecha.ToString("hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture).ToLowerInvariant();
                    //Reemplazar el valor "am" o "pm" por "a.m" o "p.m" respectivamente.
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); } if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.realizado;
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((codRole == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && codRole == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { /*chk_realizado.Enabled = false; */ }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (codRole == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (codRole == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                {
                    if (usuario.CodGrupo == Constantes.CONST_Evaluador)
                    {
                        visibleGuardar = false;
                    }
                    else
                    {
                        visibleGuardar = true;
                    }
                }

                //Mostrar los enlaces para adjuntar documentos.
                if (EsMiembro && codRole == Constantes.CONST_RolEmprendedor.ToString() && !bRealizado)
                {
                    tabla_docs.Visible = true;
                }
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;

                if (usuario.CodGrupo == Constantes.CONST_Asesor)
                {
                    visibleGuardar = true;
                }
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }



        /// <summary>
        
        /// 11/06/2014.
        /// Generar tabla "Infraestructura".
        /// </summary>
        private void GenerarTablaInfraestructura()
        {
            //Inicializar variables.
            Table Tabla_infraestructura = new Table();
            Tabla_infraestructura.CssClass = "Grilla";
            Tabla_infraestructura.Attributes.Add("width", "98%");
            Tabla_infraestructura.Attributes.Add("cellspacing", "1");
            Tabla_infraestructura.Attributes.Add("cellpadding", "4");
            DataTable rsTipo = new DataTable();
            DataTable rsInfraestructura = new DataTable();
            TableHeaderRow fila1 = new TableHeaderRow();
            TableRow fila = new TableRow();
            TableCell celdaDatosda = new TableCell();
            TableHeaderCell celdaDatos = new TableHeaderCell();
            TableCell celdaEspecial = new TableCell();
            TableHeaderCell celdaEncabezado = new TableHeaderCell();
            String[] arr_titulos = { "&nbsp", "Nombre", "Fecha de compra", "Periodos de Amortización",
                                     "Sis. de Depreciación y/o Agotamiento", "% Crédito", "Unidad",
                                      "Cantidad", "Precio/Unidad"};
            String[] arr_width = { "", "3%", "14%", "13%", "10%", "10%", "'8%", "10%", "12%" };
            Double Total = 0;

            try
            {
                #region Generar encabezados.

                //Inicializar la fila.
                fila1 = new TableHeaderRow();
                //Recorrer el arreglo de títulos.
                for (int i = 0; i < arr_titulos.Count(); i++)
                {
                    //Agregar atributos a la fila.
                    fila1.Attributes.Add("bgcolor", "#3D5A87");
                    //Inicializar la celda de encabezado.
                    celdaEncabezado = new TableHeaderCell();
                    celdaEncabezado.Attributes.Add("color", "white");
                    //Si es cero "es la columna sin nombre de columna".
                    if (i != 0)
                    {
                        //Si es 1 "es la columna (Nombre)".
                        if (i == 1)
                        { celdaEncabezado.Style.Add("text-align", "left"); }
                        else
                        {
                            //De lo contrario, son las demás columnas.
                            celdaEncabezado.Style.Add("text-align", "center");
                            celdaEncabezado.Attributes.Add("width", arr_width[i].ToString());
                        }
                    }
                    else //De lo contrario, son las demás columnas.
                    { celdaEncabezado.Style.Add("text-align", "left"); }
                    //Agregar el nombre de la conlumna a la celda del encabezado.
                    celdaEncabezado.Text = arr_titulos[i].ToString();
                    //Agregar celda del encabezado a la fila.
                    fila1.Cells.Add(celdaEncabezado);
                }
                //Agregar la fila con las celdas de encabezado generadas a la tabla.
                Tabla_infraestructura.Rows.Add(fila1);

                #endregion

                #region Ejecutar consulta y asignar resultados a variable "rsTipo".

                //Consulta #1.
                txtSQL = " SELECT id_TipoInfraestructura, nomTipoInfraestructura FROM tipoInfraestructura ORDER BY 1 ";

                //Asignar los resultados de la consulta en variable DataTable.
                rsTipo = consultas.ObtenerDataTable(txtSQL, "text");

                #endregion

                #region Recorrer filas de la tabla para generar la información.

                foreach (DataRow row in rsTipo.Rows)
                {
                    #region Consultar los detalles de infraestructura de acuerdo al ID de la consulta #1.
                    txtSQL = "";
                    txtSQL = " SELECT Id_ProyectoInfraestructura, NomInfraestructura, CodTipoInfraestructura, " +
                             " Unidad, ValorUnidad,  " +
                             " Cantidad, FechaCompra, ValorCredito, PeriodosAmortizacion, SistemaDepreciacion,  " +
                             " T.nomTipoInfraestructura " +
                             " FROM proyectoInfraestructura P, TipoInfraestructura T  " +
                             " WHERE T.id_TipoInfraestructura = P.codTipoInfraestructura " +
                             " AND codProyecto = " + codProyecto +
                             " AND CodTipoInfraestructura = " + row["id_TipoInfraestructura"].ToString();

                    //Asignar los resultados de la consulta #2 a variable DataTable.
                    rsInfraestructura = consultas.ObtenerDataTable(txtSQL, "text");
                    #endregion

                    #region Generar fila única "nombre de la infraestructura".

                    //Si tiene detalles, genera la fila única "y luego sus detalles".
                    if (rsInfraestructura.Rows.Count > 0)
                    {
                        //Inicializar la nueva fila.
                        fila = new TableRow();
                        //Generar celda vacía.
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = "&nbsp;";
                        fila.Cells.Add(celdaEspecial);
                        //Inicializar nueva celda.
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("colspan", "8");
                        celdaEspecial.Text = "<strong style=\"color:#174680;\">" + row["nomTipoInfraestructura"].ToString() + "</strong>";
                        //Añadir la celda a la fila de datos obtenidos.
                        fila.Cells.Add(celdaEspecial);
                        Tabla_infraestructura.Rows.Add(fila);
                    }

                    #endregion

                    #region Recorrer los detalles para generar la información.

                    foreach (DataRow row_detalles in rsInfraestructura.Rows)
                    {
                        #region Generar celdas "aquí se agrega l ainformación y dependiendo del rol, el control de eliminación.

                        //Inicializar la fila.
                        fila = new TableRow();
                        //if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                        //{
                        //    ////GENERAR IMAGEBUTTON Y LINKBUTTON CON FUNCIONALIDAD DE ELIMINACIÓN.
                        //    ////Inicializar la fila.
                        //    //fila = new TableRow();
                        //    ////Generar celda vacía.
                        //    //celdaEspecial = new TableCell();
                        //    //celdaEspecial.Attributes.Add("width", "3%");
                        //    //fila.Cells.Add(celdaEspecial);

                        //    ////Celda con botón de eliminación.
                        //}
                        //else
                        //{
                        //Generar celda vacía.

                        #region Diego Quiñonez - 16 de Diciembre de 2014

                        if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                        {
                            celdaEspecial = new TableCell();
                            celdaEspecial.Attributes.Add("align", "left");

                            ImageButton imgBtn = new ImageButton();
                            imgBtn.ImageUrl = "../../Images/icoBorrar.gif";
                            imgBtn.OnClientClick = "return confirm('Esta seguro que desea borrar la infraestructura seleccionada?');";
                            imgBtn.Click += new ImageClickEventHandler(imgBtn_Click);
                            imgBtn.CommandName = "Eliminar";
                            imgBtn.CommandArgument = row_detalles["Id_ProyectoInfraestructura"].ToString();

                            celdaEspecial.Controls.Add(imgBtn);

                            fila.Cells.Add(celdaEspecial);
                        }
                        else
                        {
                            celdaEspecial = new TableCell();
                            celdaEspecial.Attributes.Add("width", "3%");
                            fila.Cells.Add(celdaEspecial);
                        }

                        //Nombre:
                        if (esMiembro && usuario.CodGrupo == Constantes.CONST_Emprendedor && !bRealizado)
                        {
                            celdaEspecial = new TableCell();
                            celdaEspecial.Attributes.Add("align", "left");
                            
                            LinkButton lnk = new LinkButton();
                            lnk.Text = row_detalles["nomInfraestructura"].ToString();
                            lnk.Click += new EventHandler(lnk_Click);
                            lnk.CommandName = "Editar";
                            lnk.CommandArgument = row_detalles["Id_ProyectoInfraestructura"].ToString();

                            celdaEspecial.Controls.Add(lnk);

                            fila.Cells.Add(celdaEspecial);
                        }
                        else
                        {
                            celdaEspecial = new TableCell();
                            celdaEspecial.Attributes.Add("align", "left");
                            celdaEspecial.Text = row_detalles["nomInfraestructura"].ToString();
                            fila.Cells.Add(celdaEspecial);
                        }

                        #endregion

                        //Fecha de Compra:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = FormatearFecha(row_detalles["FechaCompra"].ToString());
                        fila.Cells.Add(celdaEspecial);

                        //Periodos de amortización:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = row_detalles["PeriodosAmortizacion"].ToString();
                        fila.Cells.Add(celdaEspecial);

                        //Sistema de depreciación:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = row_detalles["SistemaDepreciacion"].ToString();
                        fila.Cells.Add(celdaEspecial);

                        //Sistema de depreciación:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = row_detalles["ValorCredito"].ToString() + " %";
                        fila.Cells.Add(celdaEspecial);

                        //Unidad:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = row_detalles["Unidad"].ToString();
                        fila.Cells.Add(celdaEspecial);

                        //Cantidad:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "center");
                        celdaEspecial.Text = row_detalles["Cantidad"].ToString();
                        fila.Cells.Add(celdaEspecial);

                        //Valor de la unidad:
                        celdaEspecial = new TableCell();
                        celdaEspecial.Attributes.Add("align", "right");
                        celdaEspecial.Text = Double.Parse(row_detalles["ValorUnidad"].ToString()).ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        fila.Cells.Add(celdaEspecial);
                        //}

                        //Agregar la fila a la tabla.
                        Tabla_infraestructura.Rows.Add(fila);

                        //Generar total.
                        Total = Total + Double.Parse(row_detalles["ValorUnidad"].ToString()) * Double.Parse(row_detalles["Cantidad"].ToString());

                        #endregion
                    }

                    #endregion
                }

                #endregion

                #region Generar nueva fila de "Total".

                //Inicializar la nueva fila.
                fila = new TableRow();
                //Inicializar nueva celda.
                celdaEspecial = new TableCell();
                celdaEspecial.Attributes.Add("colspan", "6");
                //Añadir la celda a la fila de datos obtenidos.
                fila.Cells.Add(celdaEspecial);
                //Generar nueva celda con texto "Total".
                celdaEspecial = new TableCell();
                celdaEspecial.Text = "<b>Total</b>";
                fila.Cells.Add(celdaEspecial);
                //Generar nueva celda con el Total formateado.
                celdaEspecial = new TableCell();
                celdaEspecial.Attributes.Add("colspan", "2");
                celdaEspecial.Attributes.Add("align", "right");
                celdaEspecial.Text = "<b>" + Total.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</b>";
                fila.Cells.Add(celdaEspecial);
                //Agregar la fila a la tabla.
                Tabla_infraestructura.Rows.Add(fila);

                #endregion

                //Bindear la grilla.
                pnl_tabla_infraestructura.Controls.Add(Tabla_infraestructura);
                Tabla_infraestructura.DataBind();

                #region Destruir variables.

                Tabla_infraestructura = null;
                rsTipo = null;
                rsInfraestructura = null;
                fila1 = null;
                fila = null;
                celdaDatosda = null;
                celdaDatos = null;
                celdaEspecial = null;
                celdaEncabezado = null;
                arr_titulos = null;
                Total = 0;

                #endregion
            }
            catch (Exception ex)
            {
                #region Destruir variables.

                Tabla_infraestructura = null;
                rsTipo = null;
                rsInfraestructura = null;
                fila1 = null;
                fila = null;
                celdaDatosda = null;
                celdaDatos = null;
                celdaEspecial = null;
                celdaEncabezado = null;
                arr_titulos = null;
                Total = 0;

                #endregion
            }
        }

        #region Diego Quiñonez - 16 de Diciembre de 2014

        protected void lnk_Click(object sender, EventArgs e)
        {
            var lnk = (LinkButton)sender;

            //if (lnk.CommandName.Equals("Editar"))
            if  (lnk.CommandName.Equals("Editar"))
            {
                HttpContext.Current.Session["Accion"] = "Editar";
                HttpContext.Current.Session["Id_ProyectoInfraestructura"] = lnk.CommandArgument;
                Redirect(null, "../evaluacion/CatalogoInfraestructura.aspx?Accion=Editar", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
            }
        }

        protected void imgBtn_Click(object sender, ImageClickEventArgs e)
        {
            var imgBtn = (ImageButton)sender;

            if (imgBtn.CommandName.Equals("Eliminar"))
            {
                string txtSQL = "DELETE FROM ProyectoInfraestructura WHERE Id_ProyectoInfraestructura = " + imgBtn.CommandArgument;

                ejecutaReader(txtSQL, 2);

                txtSQL = "select isnull(sum(valorunidad*cantidad),0) AS valor, nomtipoinfraestructura from tipoinfraestructura T left outer join proyectoinfraestructura pf  on id_tipoinfraestructura=codtipoinfraestructura where CodProyecto=" + codProyecto + " group by nomtipoinfraestructura";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                foreach(DataRow dr in dt.Rows)
                {
                    int valor = int.Parse(dr["valor"].ToString());

                    if (valor > 0)
                    {
                        txtSQL = "select * from proyectoinversion  where codproyecto=" + codProyecto + " and concepto = '" + dr["NomTipoInfraestructura"].ToString() + "' and TipoInversion='Fija'";

                        var dt1 = consultas.ObtenerDataTable(txtSQL, "text");

                        if (dt1.Rows.Count > 0)
                        {
                            txtSQL = "Update proyectoinversion  set valor = " + dr["valor"].ToString() +
                            " where CodProyecto=" + codProyecto + " and concepto='" + dr["NomTipoInfraestructura"].ToString() + "' and TipoInversion='Fija'";
                        }
                        else
                        {
                            txtSQL = "insert into proyectoinversion (CodProyecto,Concepto,Valor,AportadoPor,TipoInversion) " +
                            "values (" + codProyecto + ",'" + dr["NomTipoInfraestructura"].ToString() + "'," + dr["valor"].ToString() + ",'','Fija')";
                        }

                        ejecutaReader(txtSQL, 2);
                    }
                    else
                    {
                        txtSQL = "delete from proyectoinversion where codproyecto=" + codProyecto + " and concepto = '" + dr["NomTipoInfraestructura"].ToString() + "' and TipoInversion='Fija' ";

                        ejecutaReader(txtSQL, 2);
                    }
                }
                Response.Redirect(Request.RawUrl);
                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.location.reload();", true);
            }
        }

        #endregion

        /// <summary>
        
        /// 11/06/2014.
        /// Generar fecha formateada "Ej: Oct 5 de 2011".
        /// </summary>
        /// <param name="fecha">fecha obtenida durante la lectura de datos.</param>
        /// <returns>fecha formateada = conversión correcta // fecha sin formato = error.</returns>
        private string FormatearFecha(string fecha)
        {
            //Inicializar variables.
            DateTime fecha_convertida = new DateTime();

            string sMes = "";

            try
            {
                fecha_convertida = Convert.ToDateTime(fecha, System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                sMes = OperacionesRoleNegocio.UppercaseFirst(fecha_convertida.ToString("MMM"));
                return fecha = fecha_convertida.ToString("'" + sMes + "' dd 'de' yyyy");
            }
            catch
            {
                return fecha;
            }
        }

        /// <summary>
        
        /// 11/06/2014.
        /// Obtener el texto "Parámetros Técnicos Especiales".
        /// </summary>
        /// <returns>String.</returns>
        private string CargarTexto_ParametrosTecnicosEspeciales()
        {
            try
            {
                txtSQL = " select ParametrosTecnicos from ProyectoOperacionInfraestructura where codproyecto = " + codProyecto;
                string valor = "";
                var dt = consultas.ObtenerDataTable(txtSQL, "text");
                if (dt.Rows.Count > 0) { valor = dt.Rows[0]["ParametrosTecnicos"].ToString(); }
                return valor;
            }
            catch { return ""; }
        }

        /// <summary>
        
        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {            
            int flag = 0;
            prActualizarTab(txtTab.ToString(), codProyecto.ToString());
            flag = Marcar(txtTab.ToString(), codProyecto, "", chk_realizado.Checked);
            Response.Redirect(Request.RawUrl);
            ObtenerDatosUltimaActualizacion();
            GenerarTablaInfraestructura();
         
            if (flag == 1)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "refreshParent", "window.top.location.reload();", true);
            }
            else
            {
                Response.Redirect(Request.RawUrl);
            }   
        }

        #endregion

        #region Controles adicionados y faltantes de programación.

        protected void imgBtn_Adicionar_Click(object sender, ImageClickEventArgs e)
        {
            Redirect(null, "../evaluacion/CatalogoInfraestructura.aspx?Accion=nuevo", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void lnk_Adicionar_Click(object sender, EventArgs e)
        {
            Redirect(null, "../evaluacion/CatalogoInfraestructura.aspx?Accion=nuevo", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        // INICIO WAFS 12-OCT-2014
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperInfraestructura";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "OperInfraestructura";
            HttpContext.Current.Session["CodProyecto"] = codProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        // FIN WAFS 12-OCT-2014
        #endregion
    }
}