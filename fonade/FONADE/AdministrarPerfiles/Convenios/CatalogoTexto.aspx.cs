using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.AdministrarPerfiles.Convenios
{
    /// <summary>
    /// CatalogoTexto
    /// </summary>    
    public partial class CatalogoTexto :   Negocio.Base_Page
    {

        //string crearAdmin = "Crear Administrador";
        //string crearGerenteEvaluador = "Crear GerenteEvaluador";
        //string crearCallCenter = "Crear Call Center";
        //string crearGerenteInterventor = "Crear Gerente Interventor";
        //string crearPerfilFiduciario = "Crear Perfil Fiduciario";
        //String grupos1;
        //int idusuario;
        //string[] codgrupousuario;

        /// <summary>
        /// The grupos
        /// </summary>
        public String[] grupos = { "0" };

        /// <summary>
        /// Voids the modificar datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="Grupocontacto">The grupocontacto.</param>
        protected void void_ModificarDatos(string[] codgrupo, int Grupocontacto)
        {
            if (Request.QueryString["CodAnexo"] != null)
            {
                int codigocontacto = Int32.Parse(Request.QueryString["CodAnexo"]);
                var query = (from t in consultas.Db.Textos
                             where t.Id_Texto == codigocontacto
                             select t).FirstOrDefault();
                query.Texto1 = tb_Descripción.Text;
                try
                {
                    //void_show("el texto ha sido actualizado correctamente", true);
                    consultas.Db.SubmitChanges();
                }
                catch ( Exception)
                {}
            }
        }

        /// <summary>
        /// Voids the traer datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_traerDatos(string[] codgrupo, int codusuario)
        {
            if (Request.QueryString["CodAnexo"] != null)
            {
            int codigoContacto = Int32.Parse(Request.QueryString["CodAnexo"]);
            var query = (from c in consultas.Db.Textos
                         where c.Id_Texto == codusuario
                         select c).FirstOrDefault();
            lbl_tituloanexo.Text=query.NomTexto;
            tb_Descripción.Text= query.Texto1;
            }
        }

        /// <summary>
        /// Voids the obtener parametros.
        /// </summary>
        protected void void_ObtenerParametros()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
            {
                pnl_Anexos.Visible = false;
                pnl_crearEditar.Visible = true;
                pnl_Anexos.Visible = false;
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

            string queryRedir = Request.Url.Query;

            queryRedir = queryRedir.Substring(0, queryRedir.IndexOf('&'));

            string redirePagina = string.Format("{0}://{1}{2}{3}", Request.Url.Scheme, Request.Url.Authority, Request.Url.AbsolutePath, queryRedir);

            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>alert('" + texto + "');location.href='" + redirePagina + "'</script>");
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
                string accion="";
                int codigoContacto = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["CodAnexo"]))
                {
                    codigoContacto = Int32.Parse(Request.QueryString["CodAnexo"]);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    accion = Request.QueryString["Accion"].ToString();

                };
                void_traerDatos(grupos, codigoContacto);
                pnl_crearEditar.Visible = false;
                void_ObtenerParametros();
                lbl_Titulo.Text= void_establecerTitulo(grupos, accion, "Texto");
            }
        }

        /// <summary>
        /// Handles the DataBound event of the gv_Anexos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_Anexos_DataBound(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the RowDataBound event of the gv_Anexos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Anexos_RowDataBound(object sender, GridViewRowEventArgs e)
        {}

        /// <summary>
        /// Handles the PageIndexChanged event of the gv_Anexos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gv_Anexos_PageIndexChanged(object sender, GridViewPageEventArgs e)
        {

        }
        /// <summary>
        /// Handles the Selecting event of the lds_Anexos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_Anexos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var anexos = (from tx in consultas.Db.Textos  
                          orderby  tx.NomTexto                           
                             select new
                             {
                                 Id_anexo = tx.Id_Texto,
                                 nomtexto = tx.NomTexto,                                 
                             });
            e.Arguments.TotalRowCount = anexos.Count();
            if (e.Arguments.TotalRowCount == 0)
            {
                Lbl_Resultados.Visible = true;
                Lbl_Resultados.Text = "No tiene actividades  para este rango de fechas";
            }
            else
            {
                Lbl_Resultados.Visible = false;
            }
            e.Result = anexos.ToList();
        }

        /// <summary>
        /// Handles the click event of the btn_Inactivar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void btn_Inactivar_click(object sender, CommandEventArgs e)
        {}

        /// <summary>
        /// Handles the onclick event of the btn_crearActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crearActualizar_onclick(object sender, EventArgs e)
        {
            int codigoContacto = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CodAnexo"]))
            {
                codigoContacto = Int32.Parse(Request.QueryString["CodAnexo"]);
            }
            string accion = Request.QueryString["Accion"].ToString();
            switch (accion)
            {
                case "Editar":
                    void_ModificarDatos(grupos, codigoContacto);
                    break;
                default: break;
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tfc", "document.location = 'CatalogoTexto.aspx';", true);
        }

        /// <summary>
        /// Handles the RowCreated event of the gv_Anexos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Anexos_RowCreated(object sender, GridViewRowEventArgs e)
        {
           
        }
    }
}