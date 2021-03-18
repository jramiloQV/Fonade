using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Globalization;
using Fonade.Clases;

namespace Fonade.PlanDeNegocioV2.Formulacion.Finanzas
{
    public partial class CapitalDeTrabajo : Negocio.Base_Page
    {
        public int CodigoProyecto
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["codproyecto"]);
            }
            set { }
        }
        public int txtTab = Constantes.CONST_CapitalDeTrabajoV2;
        public int idCapitalEdita;
        public string accion;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si está o no "realizado"...
        /// </summary>
        public Boolean bRealizado;
        public Boolean PostitVisible
        {
            get
            {
                return esMiembro && !bRealizado;
            }
            set { }
        }
        public bool vldt { get { if (usuario.CodGrupo == Constantes.CONST_Evaluador) { return false; } else { return new Clases.genericQueries().ValidateUserCode(usuario.IdContacto, CodigoProyecto); } } }

        public bool ejecucion
        {
            get
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador) { return (usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador && !vldt); }
                return
                new Clases.genericQueries().getItemsProyectoMercadoProyeccionVentas(CodigoProyecto).Count > 0
                && usuario.CodGrupo != Constantes.CONST_Emprendedor && usuario.CodGrupo != Constantes.CONST_Evaluador;
            }
        }

        public bool visibleGuardar { get; set; }

        public List<FuenteFinanciacion> GetFuenteFinanciacion()
        {
            return Negocio.PlanDeNegocioV2.Utilidad.General.getFuentes();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Encabezado.CodigoProyecto = CodigoProyecto;
            Encabezado.CodigoTab = txtTab;

            SetPostIt();

            txtValor.Attributes.Add("onkeypress", "javascript:return validarNro(event)");
            txtValor.Attributes.Add("onchange", "MoneyFormat(this)");

            accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : string.Empty;
            idCapitalEdita = HttpContext.Current.Session["idCapital"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["idCapital"].ToString()) ? int.Parse(HttpContext.Current.Session["idCapital"].ToString()) : 0;


            //if (Request.QueryString["Accion"] == "Editar")
            //{
            //    accion = Request.QueryString["Accion"].ToString();
            //    idCapitalEdita = Request.QueryString["idCapital"].ToString();
            //}

            inicioEncabezado(CodigoProyecto.ToString(), "", txtTab);

            //Consultar si es miembro.
            esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

            //Consultar si está "realizado".
            bRealizado = esRealizado(txtTab.ToString(), CodigoProyecto.ToString(), "");

            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_AdministradorFonade)
            if (esMiembro && !bRealizado)//!chk_realizado.Checked)
            {
                //this.div_Post_It_2.Visible = true;

            }

            if (!IsPostBack)
            {
                ObtenerDatosUltimaActualizacion();

                if (accion == "Editar")
                {
                    CargarControlesEditarCapital(CodigoProyecto, idCapitalEdita);
                }
                CargarGridCapitalTrabajo();
            }

            if (miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false)
            {
                pnlAdicionar.Visible = true;
                //ImageButton1.Visible = true;
                //ImageButton2.Visible = true;
            }
            else
            {
                pnlAdicionar.Visible = false;
                //ImageButton1.Visible = false;
                //ImageButton2.Visible = false;
            }

            if (!chk_realizado.Checked && esMiembro)
            {
                // div_Post_It_2.Visible = true;
            }
        }

        void SetPostIt()
        {
            Session["CodProyecto"] = Encabezado.CodigoProyecto;
            Post_It._codUsuario = usuario.IdContacto.ToString();
            Post_It._txtTab = Constantes.CONST_CapitalDeTrabajoV2;
        }

        protected void CargarGridCapitalTrabajo()
        {
            var query = (from p in consultas.Db.ProyectoCapitals
                         join fuente in consultas.Db.FuenteFinanciacions on p.codFuenteFinanciacion equals fuente.IdFuente
                         where p.CodProyecto == CodigoProyecto
                         orderby p.Componente
                         select new { p.Id_Capital, p.Componente, p.Valor, p.Observacion, fuente.DescFuente });

            DataTable datos = new DataTable();
            datos.Columns.Add("CodProyecto");
            datos.Columns.Add("id_Capital");
            datos.Columns.Add("componente");
            datos.Columns.Add("valor");
            datos.Columns.Add("FuenteFinanciacion");
            datos.Columns.Add("observacion");

            double total = 0;
            foreach (var item in query)
            {

                DataRow dr = datos.NewRow();
                dr["CodProyecto"] = CodigoProyecto;
                dr["id_Capital"] = item.Id_Capital;
                dr["componente"] = item.Componente;
                dr["valor"] = item.Valor.ToString("0,0.00", CultureInfo.InvariantCulture); ;
                dr["FuenteFinanciacion"] = item.DescFuente;
                dr["observacion"] = item.Observacion;
                total += Convert.ToDouble(item.Valor);
                datos.Rows.Add(dr);
            }

            DataRow drTotal = datos.NewRow();
            drTotal["componente"] = "Total";
            drTotal["valor"] = total.ToString("0,0.00", CultureInfo.InvariantCulture);
            datos.Rows.Add(drTotal);

            gw_CapitalTrabajo.DataSource = datos;
            gw_CapitalTrabajo.DataBind();
        }

        protected void gw_CapitalTrabajo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Borrar":
                    idCapitalEdita = int.Parse(e.CommandArgument.ToString());
                    consultas.Db.ExecuteCommand("Delete from ProyectoCapital where Id_Capital={0}", idCapitalEdita);
                    break;
                case "Editar":
                    accion = "Editar";
                    idCapitalEdita = int.Parse(e.CommandArgument.ToString());

                    HttpContext.Current.Session["Accion"] = accion;
                    HttpContext.Current.Session["idCapital"] = idCapitalEdita;
                    CargarControlesEditarCapital(CodigoProyecto, idCapitalEdita);
                    break;
            }

            ObtenerDatosUltimaActualizacion();
            CargarGridCapitalTrabajo();
        }

        protected void btnAdicionarCapitalTrabajo_Click(object sender, EventArgs e)
        {
            txtComponente.Text = "";
            txtValor.Text = "";
            txtObservacioin.Text = "";
            pnlCapitalTrabajo.Visible = false;
            pnlCrearCapitalTrabajo.Visible = true;
            HttpContext.Current.Session["Accion"] = "Crear";
            HttpContext.Current.Session["idCapital"] = 0;
            btnCrearCapitalTrabajo.Text = "Crear";
        }

        protected void btnCancelarNuevoCapital_Click(object sender, EventArgs e)
        {
            Cancelar();
        }

        private void Cancelar()
        {
            txtComponente.Text = "";
            txtComponente.Text = "";
            txtObservacioin.Text = "";
            HttpContext.Current.Session["Accion"] = string.Empty;
            HttpContext.Current.Session["idCapital"] = 0;
            CargarGridCapitalTrabajo();
            pnlCapitalTrabajo.Visible = true;
            pnlCrearCapitalTrabajo.Visible = false;
        }

        protected void btnCrearCapitalTrabajo_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();
                if (accion == "Editar")
                {

                    var query = (from p in consultas.Db.ProyectoCapitals
                                 where p.CodProyecto == CodigoProyecto &&
                                 p.Id_Capital == idCapitalEdita
                                 select p
                          ).First();

                    query.Componente = txtComponente.Text;
                    query.Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ","));
                    query.codFuenteFinanciacion = Convert.ToInt32(cmbFuenteFinanciacion.SelectedValue);
                    query.Observacion = txtObservacioin.Text;
                    consultas.Db.SubmitChanges();
                }
                else
                {
                    Datos.ProyectoCapital datosNuevos = new ProyectoCapital()
                    {
                        CodProyecto = Convert.ToInt32(CodigoProyecto),
                        Componente = txtComponente.Text,
                        Valor = Convert.ToDecimal(txtValor.Text.Replace(",", "").Replace(".", ",")),
                        Observacion = txtObservacioin.Text,
                        codFuenteFinanciacion = Convert.ToInt32(cmbFuenteFinanciacion.SelectedValue)
                    };
                    consultas.Db.ProyectoCapitals.InsertOnSubmit(datosNuevos);

                    consultas.Db.SubmitChanges();
                }

                prActualizarTab(txtTab.ToString(), CodigoProyecto.ToString());

                txtComponente.Text = "";
                txtValor.Text = "";
                txtObservacioin.Text = "";

                ObtenerDatosUltimaActualizacion();
                Cancelar();

            }
            catch (ApplicationException ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia : " + ex.Message + " ');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        private void CargarControlesEditarCapital(int codProyecto, int idInversionEdita)
        {

            pnlCapitalTrabajo.Visible = false;
            pnlCrearCapitalTrabajo.Visible = true;

            var query = (from p in consultas.Db.ProyectoCapitals
                         where p.CodProyecto == Convert.ToInt32(codProyecto) &&
                         p.Id_Capital == Convert.ToInt32(idCapitalEdita)
                         select p
                             ).First();

            txtComponente.Text = query.Componente;
            txtValor.Text = query.Valor.ToString("0,0.00", CultureInfo.InvariantCulture);
            txtObservacioin.Text = query.Observacion.ToString();
            btnCrearCapitalTrabajo.Text = "Actualizar";
            cmbFuenteFinanciacion.SelectedValue = query.codFuenteFinanciacion != null ? query.codFuenteFinanciacion.ToString() : null;
        }

        #region Métodos de .

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
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

        /// 06/06/2014.
        /// Obtener la información acerca de la última actualización realizada, ási como la habilitación del 
        /// CheckBox para el usuario dependiendo de su grupo / rol.
        /// </summary>
        private void ObtenerDatosUltimaActualizacion()
        {
            //Inicializar variables.
            String txtSQL;
            DateTime fecha = new DateTime();
            DataTable tabla = new DataTable();
            bool bRealizado = false;
            bool EsMiembro = false;
            Int32 numPostIt = 0;
            Int32 CodigoEstado = 0;

            try
            {
                //Consultar si es miembro.
                EsMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

                //Obtener número "numPostIt".
                numPostIt = Obtener_numPostIt();

                //Consultar el "Estado" del proyecto.
                CodigoEstado = CodEstado_Proyecto(txtTab.ToString(), CodigoProyecto.ToString(), "");//CodConvocatoria

                //Consultar los datos a mostrar en los campos correspondientes a la actualización.
                var usuActualizo = consultas.RetornarInformacionActualizaPPagina(CodigoProyecto, txtTab).FirstOrDefault();

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
                    if (hora.Contains("am")) { hora = hora.Replace("am", "a.m"); }
                    if (hora.Contains("pm")) { hora = hora.Replace("pm", "p.m"); }
                    //Formatear la fecha según manejo de FONADE clásico. "Ej: Nov 19 de 2013 07:36:26 p.m.".
                    lbl_fecha_formateada.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year + " " + hora + ".";

                    //Realizado
                    bRealizado = usuActualizo.realizado;
                }

                //Asignar check de acuerdo al valor obtenido en "bRealizado".
                chk_realizado.Checked = bRealizado;

                //Determinar si el usuario actual puede o no "chequear" la actualización.
                //if (!(EsMiembro && numPostIt == 0 && ((usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && usuario.CodGrupo == Constantes.CONST_RolEvaluador && es_bNuevo(codProyecto)))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                if (!(EsMiembro && numPostIt == 0 && ((HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (CodigoEstado == Constantes.CONST_Evaluacion && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && es_bNuevo(CodigoProyecto.ToString())))) || lbl_nombre_user_ult_act.Text.Trim() == "")
                { chk_realizado.Enabled = false; }

                //Mostrar el botón de guardar.
                //if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (usuario.CodGrupo == Constantes.CONST_RolAsesorLider && CodigoEstado == Constantes.CONST_Inscripcion) || (usuario.CodGrupo == Constantes.CONST_RolEvaluador && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(codProyecto)))
                if (EsMiembro && numPostIt == 0 && lbl_nombre_user_ult_act.Text != "" && (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolAsesorLider.ToString() && CodigoEstado == Constantes.CONST_Inscripcion) || (HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolEvaluador.ToString() && CodigoEstado == Constantes.CONST_Evaluacion && es_bNuevo(CodigoProyecto.ToString())))
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
                visibleGuardar = visibleGuardar || Constantes.CONST_CoordinadorEvaluador == usuario.CodGrupo;
                //Destruir variables.
                tabla = null;
                txtSQL = null;
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true);
                //Destruir variables.
                //tabla = null;
                //txtSQL = null;
                //return;
            }
        }

        /// <summary>

        /// 06/06/2014.
        /// Obtener el número "numPostIt" usado en la condicional de "obtener última actualización".
        /// El código se encuentra en "Base_Page" línea "116", método "inicioEncabezado".
        /// Ya se le están enviado por parámetro en el método el código del proyecto y la constante "CONST_PostIt".
        /// </summary>
        /// <returns>numPostIt.</returns>
        private int Obtener_numPostIt()
        {
            Int32 numPosIt = 0;

            //Hallar numero de post it por tab
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(CodigoProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            numPosIt = query.Count();

            return numPosIt;
        }

        /// <summary>

        /// 24/06/2014.
        /// Guardar la información "Ultima Actualización".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_guardar_ultima_actualizacion_Click(object sender, EventArgs e)
        {
            Marcar(txtTab.ToString(), CodigoProyecto.ToString(), "", chk_realizado.Checked);
            ObtenerDatosUltimaActualizacion();
            Response.Redirect(Request.RawUrl);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasCapiTrabajo";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Nuevo";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["TabInvoca"] = "FinanzasCapiTrabajo";
            HttpContext.Current.Session["CodProyecto"] = CodigoProyecto;
            HttpContext.Current.Session["txtTab"] = txtTab.ToString();
            HttpContext.Current.Session["Accion"] = "Vista";
            Redirect(null, "CatalogoDocumento.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        #endregion

        protected void gw_CapitalTrabajo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                var imgBtn = (ImageButton)e.Row.Cells[0].FindControl("btn_Borrar");
                var lnk = (LinkButton)e.Row.Cells[1].FindControl("lnkComponente");

                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    imgBtn.Enabled = false;
                    imgBtn.Visible = false;
                    lnk.Enabled = false;
                    //((ImageButton)e.Row.Cells[0].FindControl("btn_Borrar")).Visible = false;
                }

                if (lnk.Text.Equals("Total"))
                {
                    imgBtn.Enabled = false;
                    imgBtn.Visible = false;
                    lnk.Enabled = false;
                }

                esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());
                bRealizado = esRealizado(txtTab.ToString(), CodigoProyecto.ToString(), "");

                if (!(miembro == true && usuario.CodGrupo == Constantes.CONST_Emprendedor && realizado == false))
                {
                    imgBtn.Enabled = false;
                    imgBtn.Visible = false;
                    lnk.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Valida los campos de los formularios
        /// </summary>
        /// <returns> ApplicationException si encuentra error. </returns>
        private void ValidarCampos()
        {
            FieldValidate.ValidateString("Componente", txtComponente.Text, true, 255);

            FieldValidate.ValidateNumeric("Valor", txtValor.Text, true);

            FieldValidate.ValidateString("Observaciones", txtObservacioin.Text, true, 255);
        }
    }
}