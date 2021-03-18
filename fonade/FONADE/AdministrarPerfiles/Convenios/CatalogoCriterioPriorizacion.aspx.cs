using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Account;
using System.Data;
using Fonade.Clases;

namespace Fonade.FONADE.AdministrarPerfiles.Convenios
{
    /// <summary>
    /// CatalogoCriterioPriorizacion
    /// </summary>    
    public partial class CatalogoCriterioPriorizacion : Negocio.Base_Page
    {
        #region Variables globales.

        //string crearAdmin = "Crear Administrador";
        //string crearGerenteEvaluador = "Crear GerenteEvaluador";
        //string crearCallCenter = "Crear Call Center";
        //string crearGerenteInterventor = "Crear Gerente Interventor";
        //string crearPerfilFiduciario = "Crear Perfil Fiduciario";
        string[] codgrupousuario;
        //String grupos1;        
        /// <summary>
        /// The grupos
        /// </summary>
        public String[] grupos = { "0" };
        int idusuario;

        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        #endregion

        /// <summary>
        /// Voids the modificar datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="Grupocontacto">The grupocontacto.</param>
        protected void void_ModificarDatos(string[] codgrupo, int Grupocontacto)
        {
            if (Request.QueryString["CodCriterio"] != null)
            {
                int codigoContacto = Int32.Parse(Request.QueryString["CodCriterio"]);

                var query = (from c in consultas.Db.CriterioPriorizacions
                             where c.Id_Criterio == codigoContacto
                             select c);
                foreach (CriterioPriorizacion c in query)
                {
                    c.NomCriterio = tb_criterio.Text;
                    c.Sigla = tb_sigla.Text;
                    c.CodFactor = Byte.Parse(ddl_Factores.SelectedValue);
                    c.Componente = tb_componente.Text;
                    c.Indicador = tb_indicador.Text;
                    c.ValorBase = tb_valor_base.Text;
                    c.Formulacion = tb_formulacion.Text;
                    c.Query = tb_query.Text;
                }
                try
                {
                    consultas.Db.Connection.Open();
                    consultas.Db.ExecuteCommand(UsuarioActual());
                    consultas.Db.SubmitChanges();
                    //consultas.Db.Connection.Dispose();
                    consultas.Db.Connection.Close();
                    void_show("La información del usuario ha sido actualizado correctamente ", true);

                }
                catch (Exception ex) { void_show("Error: " + ex.Message, true); }
            }
        }

        /// <summary>
        /// Voids the crear datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_CrearDatos(string[] codgrupo, int codusuario)
        {
            CriterioPriorizacion Criterio = new CriterioPriorizacion()
            {
                NomCriterio = tb_criterio.Text,
                Sigla = tb_sigla.Text,
                CodFactor = Byte.Parse(ddl_Factores.SelectedValue),
                Componente = tb_componente.Text,
                Indicador = tb_indicador.Text,
                ValorBase = tb_valor_base.Text,
                Formulacion = tb_formulacion.Text,
                Query = tb_query.Text,
            };
            try
            {
                var vaidarcriterio = (from c1 in consultas.Db.CriterioPriorizacions
                                      where c1.NomCriterio == tb_criterio.Text
                                      select c1).FirstOrDefault();
                if (vaidarcriterio == null)
                {
                    consultas.Db.CriterioPriorizacions.InsertOnSubmit(Criterio);
                    consultas.Db.Connection.Open();
                    consultas.Db.ExecuteCommand(UsuarioActual());
                    void_show("el Criterio " + tb_criterio.Text + " ha sido agregado", true);
                    consultas.Db.Connection.Close();
                }
                else
                {
                    Lbl_Resultados.Visible = true;
                    void_show("el Criterio " + tb_criterio.Text + "ya existe intente nuevamente", true);
                }
            }
            catch (Exception ex) { void_show("Error: " + ex.Message, true); }
        }

        /// <summary>
        /// Voids the hide controls.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="codgrupo">The codgrupo.</param>
        protected void void_HideControls(string accion, string[] codgrupo)
        {
            switch (accion)
            {
                case "Crear":
                    btn_crearActualizar.Text = "Crear";
                    break;
                case "Editar":
                    btn_crearActualizar.Text = "Actualizar";
                    break;
                default: break;
            }
        }

        /// <summary>
        /// Voids the traer datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_traerDatos(string[] codgrupo, int codusuario)
        {
            var criterio = (from c in consultas.Db.CriterioPriorizacions
                            where c.Id_Criterio == codusuario
                            select c).FirstOrDefault();
            tb_criterio.Text = criterio.NomCriterio;
            tb_sigla.Text = criterio.Sigla;
            ddl_Factores.SelectedValue = criterio.CodFactor.ToString();
            tb_componente.Text = criterio.Componente;
            tb_indicador.Text = criterio.Indicador;
            tb_valor_base.Text = criterio.ValorBase;
            tb_formulacion.Text = criterio.Formulacion;
            tb_query.Text = criterio.Query;
        }

        /// <summary>
        /// Voids the obtener parametros.
        /// </summary>
        protected void void_ObtenerParametros()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
            {
                pnl_Criterios.Visible = false;
                pnl_crearEditar.Visible = true;
                AgregarCriterio.Visible = false;
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    if (Request.QueryString["Accion"].ToString() == "Editar")
                    {
                        AgregarCriterio.Text = "Actualizar";
                        idusuario = Int32.Parse(Request.QueryString["CodCriterio"]);
                        void_traerDatos(codgrupousuario, idusuario);//trae la información segun el usuario y el grupo al cual 
                    }
                    if (Request.QueryString["Accion"].ToString() == "Crear")
                    {
                        AgregarCriterio.Text = "Crear";
                    }
                }
            }
        }

        /// <summary>
        /// Voids the show.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <param name="mostrar">if set to <c>true</c> [mostrar].</param>
        protected void void_show(string texto, bool mostrar)
        {
            //lbl_popup.Visible = mostrar;
            //lbl_popup.Text = texto;
            //mpe1.Enabled = mostrar;
            //mpe1.Show();

            //string queryRedir = Request.Url.Query;

            //queryRedir = queryRedir.Substring(0, queryRedir.IndexOf('&'));

            //string redirePagina = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath, queryRedir);

            //ClientScriptManager cm = this.ClientScript;
            //cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('" + texto + "');location.href='" + redirePagina + "'</script>");
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string accion = "";
                int codigoContacto = 0;
                var factor = from f in consultas.Db.Factors
                             select f;
                ddl_Factores.DataSource = factor;
                ddl_Factores.DataTextField = "NomFactor";
                ddl_Factores.DataValueField = "Id_Factor";
                ddl_Factores.DataBind();
                if (!String.IsNullOrEmpty(Request.QueryString["CodCriterio"]))
                {
                    codigoContacto = Int32.Parse(Request.QueryString["CodCriterio"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    accion = Request.QueryString["Accion"].ToString();
                    void_HideControls(accion, grupos);
                };
                pnl_crearEditar.Visible = false;
                lbl_Titulo.Text = void_establecerTitulo(grupos, accion, "CRITERIOS DE PRIORIZACIÓN");
                if (lbl_Titulo.Text == "NUEVO CRITERIOS DE PRIORIZACIÓN") { lbl_Titulo.Text = "NUEVO CRITERIO DE PRIORIZACIÓN"; }
                void_ObtenerParametros();

                CargarCriteriosDePriorizacion();
            }
        }

        /// <summary>
        /// Handles the DataBound event of the gv_Criterios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_Criterios_DataBound(object sender, EventArgs e)
        { gv_Criterios.Columns[2].Visible = false; }

        /// <summary>
        /// Handles the RowDataBound event of the gv_Criterios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Criterios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Conteo;
            Conteo = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "cuantos"));
            ImageButton imbtton = new ImageButton();
            imbtton = ((ImageButton)e.Row.Cells[0].FindControl("btn_Inactivar"));
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Conteo == "0") imbtton.Visible = true;
                else
                    imbtton.Visible = false;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 11/09/2014.
        /// Paginación de la grilla de criterios de priorización.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_Criterios_PageIndexChanged(object sender, GridViewPageEventArgs e)
        {
            var dt = HttpContext.Current.Session["tablaCriterios"] as DataTable;

            if (dt != null)
            {
                gv_Criterios.PageIndex = e.NewPageIndex;
                gv_Criterios.DataSource = dt;
                gv_Criterios.DataBind();
                dt = null;
            }
        }

        /// <summary>
        /// Handles the click event of the btn_Inactivar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void btn_Inactivar_click(object sender, CommandEventArgs e)
        {
            var criterio = (from c in consultas.Db.CriterioPriorizacions
                            where c.Id_Criterio == short.Parse(e.CommandArgument.ToString())
                            select c).FirstOrDefault();
            consultas.Db.CriterioPriorizacions.DeleteOnSubmit(criterio);
            consultas.Db.SubmitChanges();
            RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator3.Enabled = false;
            RequiredFieldValidator4.Enabled = false;
            RequiredFieldValidator5.Enabled = false;
            RequiredFieldValidator6.Enabled = false;
            RequiredFieldValidator7.Enabled = false;
            RequiredFieldValidator10.Enabled = false;
            gv_Criterios.DataBind();
        }

        /// <summary>
        /// Crear o actualizar un criterio de priorización.
        /// NOTA: Es problable que NO se muestre el mensaje, debido a la redirección, pero
        /// la funcionalidad está asegurada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_crearActualizar_onclick(object sender, EventArgs e)
        {
            #region Código anterior.
            //string codigoGrupo;
            //int codigoContacto = 0;
            //string accion = Request.QueryString["Accion"].ToString();
            //switch (accion)
            //{
            //    case "Crear":
            //        void_CrearDatos(grupos, codigoContacto);
            //        break;
            //    case "Editar":
            //        void_ModificarDatos(grupos, codigoContacto);
            //        break;
            //    default: break;
            //} 
            #endregion

            //Inicializar variables.
            DataTable RsCriterio = new DataTable();

            #region Versión mejorada por Mauricio Arias Olave.

            if (btn_crearActualizar.Text == "Crear")
            {
                #region Crear.

                try
                {
                    FieldValidate.ValidateString("Indicador", tb_indicador.Text, true, 255);
                    FieldValidate.ValidateString("Valor base", tb_valor_base.Text, true, 255);
                    FieldValidate.ValidateString("Formulación", tb_formulacion.Text, true, 255);

                    txtSQL = "select *  from CriterioPriorizacion where nomCriterio = '" + tb_criterio.Text + "' ";
                    RsCriterio = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RsCriterio.Rows.Count == 0)
                    {
                        txtSQL = " Insert into CriterioPriorizacion(NomCriterio,Sigla,CodFactor,Componente,Indicador,ValorBase,Formulacion,Query) " +
                                 " values ('" + tb_criterio.Text + "','" + tb_sigla.Text + "'," + ddl_Factores.SelectedValue +
                                 " ,'" + tb_componente.Text + "','" + tb_indicador.Text + "','" + tb_valor_base.Text + "'" +
                                 " ,'" + tb_formulacion.Text + "','" + tb_query.Text + "')";

                        //Ejecutar consulta.
                        ejecutaReader(txtSQL, 2);

                        RsCriterio = new DataTable();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Criterio creado correctamente.');window.location.replace('CatalogoCriterioPriorizacion.aspx');", true);
                    }
                    else
                    {
                        RsCriterio = new DataTable();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El criterio ya existe.')", true);
                        return;
                    }
                }
                catch(ApplicationException ex)
                {
                    RsCriterio = new DataTable();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + " ')", true);
                    return;
                }
                catch (Exception)
                {
                    RsCriterio = new DataTable();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo crear el criterio.')", true);
                    return;
                }

                #endregion
            }
            else if (btn_crearActualizar.Text == "Actualizar")
            {
                #region Actualizar.

                try
                {

                    FieldValidate.ValidateString("Indicador", tb_indicador.Text, true, 255);
                    FieldValidate.ValidateString("Valor base", tb_valor_base.Text, true, 255);
                    FieldValidate.ValidateString("Formulación", tb_formulacion.Text, true, 255);

                    //Código del criterio seleccionado...
                    int CodCriterio = Int32.Parse(Request.QueryString["CodCriterio"]);

                    //txtSQL = " SELECT Id_Criterio FROM CriterioPriorizacion WHERE NomCriterio='" + tb_criterio.Text + "'" +
                    //" AND Id_Criterio<>" + CodCriterio;

                    //RsCriterio = consultas.ObtenerDataTable(txtSQL, "text");

                    //if (RsCriterio.Rows.Count == 0)
                    //{
                        txtSQL = " Update CriterioPriorizacion set NomCriterio ='" + tb_criterio.Text + "'," +
                                                   " Sigla = '" + tb_sigla.Text + "', " +
                                                   " CodFactor = " + ddl_Factores.SelectedValue + ", " +
                                                   " Componente = '" + tb_componente.Text + "', " +
                                                   " Indicador = '" + tb_indicador.Text + "', " +
                                                   " ValorBase = '" + tb_valor_base.Text + "', " +
                                                   " Formulacion = '" + tb_formulacion.Text + "', " +
                                                   " Query = '" + tb_query.Text + "' " +
                                                   " WHERE Id_Criterio = " + CodCriterio;

                        //Ejecutar consulta.
                        ejecutaReader(txtSQL, 2);

                        RsCriterio = new DataTable();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Criterio actualizado correctamente.');window.location.replace('CatalogoCriterioPriorizacion.aspx');", true);
                    //}
                    //else
                    //{
                    //    RsCriterio = new DataTable();
                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El criterio ya existe.')", true);
                    //    return;
                    //}
                }
                catch (ApplicationException ex)
                {
                    RsCriterio = new DataTable();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Advertencia, detalle : " + ex.Message + " ')", true);
                    return;
                }
                catch
                {
                    RsCriterio = new DataTable();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo actualizar el criterio.')", true);
                    return;
                }

                #endregion
            }

            #endregion
        }

        /// <summary>
        /// Handles the RowCreated event of the gv_Criterios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Criterios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null && gv_Criterios.SortExpression == lnk.CommandArgument)
                        {
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_Criterios.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
                            tc.Controls.Add(new LiteralControl(" "));
                            tc.Controls.Add(img);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// RowCommand.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_Criterios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "eliminar":
                    #region Eliminar.
                    try
                    {
                        var criterio = (from c in consultas.Db.CriterioPriorizacions
                                        where c.Id_Criterio == short.Parse(e.CommandArgument.ToString())
                                        select c).FirstOrDefault();
                        consultas.Db.CriterioPriorizacions.DeleteOnSubmit(criterio);
                        consultas.Db.SubmitChanges();
                        RequiredFieldValidator1.Enabled = false;
                        RequiredFieldValidator2.Enabled = false;
                        RequiredFieldValidator3.Enabled = false;
                        RequiredFieldValidator4.Enabled = false;
                        RequiredFieldValidator5.Enabled = false;
                        RequiredFieldValidator6.Enabled = false;
                        RequiredFieldValidator7.Enabled = false;
                        RequiredFieldValidator10.Enabled = false;
                        CargarCriteriosDePriorizacion();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Criterio eliminado correctamente.')", true);
                    }
                    catch
                    { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo eliminar el criterio seleccionado.')", true); }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 11/09/2014.
        /// Cargar la grilla de criterios de priorización.
        /// </summary>
        private void CargarCriteriosDePriorizacion()
        {
            try
            {
                txtSQL = " select id_criterio,nomcriterio,count(codcriteriopriorizacion) as cuantos " +
                         " from criteriopriorizacion left outer join convocatoriacriteriopriorizacion " +
                         "  on  id_criterio=codcriteriopriorizacion " +
                         " group by id_criterio,nomcriterio " +
                         " order by nomcriterio";

                var rs = consultas.ObtenerDataTable(txtSQL, "text");

                HttpContext.Current.Session["tablaCriterios"] = rs;
                gv_Criterios.DataSource = rs;
                gv_Criterios.DataBind();
                rs = null;
            }
            catch { }
        }
    }
}