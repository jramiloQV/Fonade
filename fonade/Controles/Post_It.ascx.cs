#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>Post_It.cs</Archivo>

#endregion

using Datos;
using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.Controles
{
    /// <summary>
    /// Post_It
    /// </summary>
    public partial class Post_It : System.Web.UI.UserControl
    {
        private String codProyecto;
        private String codConvocatoria;
        private String sql;
        String correo = String.Empty;
        private DataTable resulData;
        private Consultas cons;
        int usuContacto = 0;

        /// <summary>
        /// Diego QUiñonez
        /// Metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_mostrarPost)
                {
                    if (!String.IsNullOrEmpty(txtCampo))
                    {
                        if (HttpContext.Current.Session["codProyecto"] != null)
                        {
                            codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                            codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "";
                        }

                        realizarConsulta();
                    }
                }               
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// metodo que devuelve cantidad de tareas
        /// visibles por el usuario
        /// ademas valida de acuerdo a las tareas si se ve o no el post_it
        /// </summary>
        private void realizarConsulta()
        {
            correo = HttpContext.Current.User.Identity.Name;
            DataTable dt = new DataTable();
            sql = "SELECT Id_Contacto FROM CONTACTO WHERE EMAIL = '" + correo + "'";
            try
            {
                cons = new Consultas();
                dt = cons.ObtenerDataTable(sql, "text");
                usuContacto = Convert.ToInt32(dt.Rows[0]["Id_Contacto"].ToString());
            }
            catch (Exception e) { 
                throw new Exception("No pudo ser consultado el codigo del usuario", e); 
            }

            String icodProyecto = string.IsNullOrEmpty((codProyecto)) ? "null" : codProyecto;

            sql = "Select count(T.Id_TareaUsuario) as cuantos" +
                     " From tareausuariorepeticion tr with(nolock) INNER JOIN tareausuario t with(nolock) ON T.Id_TareaUsuario = TR.CodTareaUsuario" +
                                                       " INNER JOIN Contacto c with(nolock) ON c.id_contacto = t.codcontactoagendo" +
                                                       " INNER JOIN Contacto c2 with(nolock) ON c2.id_contacto = t.codcontacto" +
                                                      ",tareaprograma tp with(nolock)" +
                     " Where tr.fechacierre is null and tp.Id_TareaPrograma = 5 and  t.CodTareaPrograma = 5 and t.CodProyecto = " + icodProyecto + " and (T.CodContacto = " + usuContacto + "Or t.codcontactoagendo =" + usuContacto + ")";
            try { 
                resulData = cons.ObtenerDataTable(sql, "text");
            }
            catch (Exception e) { 
                throw new Exception("No pudo ser consultado el listado de tareas", e); 
            }
                                    
            if (resulData.Rows.Count == 0)
            {
                LB_Listar.Visible = false;
                LB_Listar.Enabled = false;
            }
            else
            {
                LB_Listar.Visible = true;
                LB_Listar.Enabled = true;
            }

            if (resulData.Rows.Count == 0)
            {
                LB_Listar.Text = "";
            }
            else
            {
                LB_Listar.Text = "" + resulData.Rows[0][0].ToString();
            }

            LB_Listar.CssClass = "encima";
        }

        /// <summary>
        /// Diego Quiñonez
        /// permite abrir paginas emergentes
        /// </summary>
        /// <param name="response"></param>
        /// <param name="url"></param>
        /// <param name="target"></param>
        /// <param name="windowFeatures"></param>
        public static void Redirect(HttpResponse response, string url, string target, string windowFeatures)
        {
            if ((string.IsNullOrEmpty(target) || target.Equals("_self", StringComparison.OrdinalIgnoreCase)) && string.IsNullOrEmpty(windowFeatures))
            {
                response.Redirect(url);
            }
            else
            {
                Page page = (Page)HttpContext.Current.Handler;

                if (page == null)
                {
                    throw new InvalidOperationException("Cannot redirect to new window outside Page context.");
                }
                url = page.ResolveClientUrl(url);

                string script;
                if (!String.IsNullOrEmpty(windowFeatures))
                {
                    script = @"window.open(""{0}"", ""{1}"", ""{2}"");";
                }
                else
                {
                    script = @"window.open(""{0}"", ""{1}"");";
                }
                script = String.Format(script, url, target, windowFeatures);
                ScriptManager.RegisterStartupScript(page, typeof(Page), "Redirect", script, true);
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// metodo que llama a listar todas las tareas
        /// creadas y pendientes de acuerdo al id del usuario logeado
        /// </summary>        
        protected void LB_Listar_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["EvalCodProyectoPOst"] = codProyecto;
            HttpContext.Current.Session["EvalCodUsuario"] = codUsuario;
            HttpContext.Current.Session["tabEval"] = txtTab;
            HttpContext.Current.Session["EvalConsPOST"] = Constantes.CONST_PostIt;
            HttpContext.Current.Session["Campo"] = txtCampo;

            Redirect(null, "~/Controles/ListarPostIt.aspx", "_Blank", "width=730,height=470");
        }

        /// <summary>
        /// Diego Quiñonez
        /// carga una ventana emergente que permite agendar una nueva tarea
        /// </summary>        
        protected void I_POs_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["EvalCodProyectoPOst"] = codProyecto;
            HttpContext.Current.Session["EvalCodUsuario"] = codUsuario;
            HttpContext.Current.Session["tabEval"] = txtTab;
            HttpContext.Current.Session["EvalConsPOST"] = Constantes.CONST_PostIt;
            HttpContext.Current.Session["Campo"] = txtCampo;
            HttpContext.Current.Session["EvalAccion"] = Accion;

            cons = new Consultas();

            Redirect(null, "~/Controles/PostIt.aspx", "_Blank", "width=730,height=585");
        }

        /// <summary>
        /// Diego Quiñonez
        /// parametros que permiten construir
        /// el objeto post_it
        /// </summary>
        /// 
        private Boolean mostrarPost;
        /// <summary>
        /// Gets or sets a value indicating whether [mostrar post].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [mostrar post]; otherwise, <c>false</c>.
        /// </value>
        public Boolean _mostrarPost
        {
            get { return mostrarPost; }
            set { mostrarPost = value; }
        }

        private Int32 txtTab;

        /// <summary>
        /// Gets or sets the text tab.
        /// </summary>
        /// <value>
        /// The text tab.
        /// </value>
        public Int32 _txtTab
        {
            get { return txtTab; }
            set { txtTab = value; }
        }

        private String txtCampo;


        /// <summary>
        /// Gets or sets the text campo.
        /// </summary>
        /// <value>
        /// The text campo.
        /// </value>
        public String _txtCampo
        {
            get { return txtCampo; }
            set { txtCampo = value; }
        }

        private String codUsuario;

        /// <summary>
        /// Gets or sets the cod usuario.
        /// </summary>
        /// <value>
        /// The cod usuario.
        /// </value>
        public String _codUsuario
        {
            get { return codUsuario; }
            set { codUsuario = value; }
        }

        private String Accion;

        /// <summary>
        /// Gets or sets the accion.
        /// </summary>
        /// <value>
        /// The accion.
        /// </value>
        public String _accion
        {
            get { return Accion; }
            set { Accion = value; }
        }        
    }
}
