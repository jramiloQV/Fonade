using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Data;

namespace Fonade.FONADE.Proyecto
{
    public partial class ProyectoFrameSet : Negocio.Base_Page
    {
        public string codProyecto;
        public string codConvocatoria;
        /// <summary>
        /// NEW! según "SeguimientoEmpresas.aspx, este valor también viaja por variable de sesión.
        /// </summary>
        public string codEmpresa;
        public int codigoEstado;

        protected void Page_Load(object sender, EventArgs e)

        {
            if(usuario != null)
            {
                txtIdGrupoUser.Text = usuario.CodGrupo.ToString();
            }
            #region Comentarios.
            //Session["codProyecto"] = string.Empty;
            //Session["codConvocatoria"] = string.Empty;

            //if (Request.QueryString["codProyecto"] != null && Request.QueryString["codProyecto"] != "")
            //{
            //    codProyecto = Request.QueryString["codProyecto"].ToString();
            //    HttpContext.Current.Session["codProyecto"] = Request.QueryString["codProyecto"].ToString();
            //}
            //else
            //{
            //    Response.Redirect("~/Default.aspx");
            //}

            //if (Request.QueryString["codConvocatoria"] != null && Request.QueryString["codConvocatoria"] != "")
            //{
            //    codConvocatoria = Request.QueryString["codConvocatoria"].ToString();
            //    HttpContext.Current.Session["codConvocatoria"] = Request.QueryString["codConvocatoria"].ToString();
            //}

            //setTabsStatus(); 
            #endregion

            #region Nueva versión.

            if (!IsPostBack)
            {
                codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

                //Verifica si el proyecto es nuevo y lo envía al mainMenu
                if (!codProyecto.Equals("0") && VerificarVersionProyecto(int.Parse(codProyecto)).Equals(2))
                    Response.Redirect("../../PlanDeNegocioV2/Formulacion/Master/MainMenu.aspx?codproyecto="+codProyecto);

                codConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? HttpContext.Current.Session["CodConvocatoria"].ToString() : "0";
                //codEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

                if (codProyecto == "0")// && codEmpresa == "0")
                {
                    // 2014/10/24 Se adicioona la pregunta para que NO de error
                    if (Request["CodProyecto"] != null)
                        codProyecto = Request["CodProyecto"].ToString();
                }
            }

            setTabsStatus();

            #endregion
        }

        int VerificarVersionProyecto(int IdProyecto) {
            var proyecto = (from p in consultas.Db.Proyecto1s
                             where p.Id_Proyecto == Convert.ToInt32(codProyecto)
                             select p).FirstOrDefault();
           
            return proyecto.IdVersionProyecto;
        }

        private void setTabsStatus()
        {
            var codEstado = (from p in consultas.Db.Proyecto
                             where p.Id_Proyecto == Convert.ToInt32(codProyecto)
                             select p).FirstOrDefault();

            if (codEstado != null)
            { codigoEstado = codEstado.CodEstado; }


            if (codigoEstado >= Constantes.CONST_LegalizacionContrato && codigoEstado <= Constantes.CONST_Condonacion)
            {
                tc_empresa.Visible = true;
                tc_seguimiento.Visible = true;
                tc_contrato.Visible = true;
            }
        }
    
        protected string setTab(int idPestana, String txtNomOpcion)
        {
            #region LINQ ajustado al desarrollo llevado hasta el momento...

            string css_class = "";
            List<short> idtabs = consultas.Db.Tabs.Where(t => t.CodTab == idPestana).Select(t => t.Id_Tab).ToList();
            var query = from tur in consultas.Db.TareaUsuarioRepeticions
                        from tu in consultas.Db.TareaUsuarios
                        from tp in consultas.Db.TareaProgramas
                        where tp.Id_TareaPrograma == tu.CodTareaPrograma
                        && tu.Id_TareaUsuario == tur.CodTareaUsuario
                        && tu.CodProyecto == Convert.ToInt32(codProyecto)
                        && tp.Id_TareaPrograma == Constantes.CONST_PostIt
                        && tur.FechaCierre == null
                        select tur;

            var predicate = PredicateBuilder.False<Datos.TareaUsuarioRepeticion>();

            if (idtabs.Count > 0)
            {
                foreach (short idtab in idtabs)
                {
                    predicate = predicate.Or(t => SqlMethods.Like(t.Parametros, "Codproyecto=" + codProyecto + "&tab=" + idtab + "&Campo=%"));
                }
            }
            else
                predicate = predicate.Or(t => SqlMethods.Like(t.Parametros, "%tab=" + idPestana + "%"));

            query = query.Where(predicate);
            int cuantos = query.Count();

            if (cuantos > 0)
            {

                css_class = "tab_advertencia";
            }
            else
            {
                var RS = consultas.ObtenerDataTable("select realizado from tabproyecto where CodTab=" + idPestana + " and codproyecto=" + Convert.ToInt32(codProyecto ?? "0"), "text");
                if (RS.Rows.Count > 0) { if (Boolean.Parse(RS.Rows[0]["realizado"].ToString())) { css_class = "tab_aprobado"; } }
            }
            /*Revision 18-02-015 verificacion de icono elementos tab*/
            return css_class;

            #endregion

            ///Se usa como parámetro el nombre de "CodOpcion" en lugar de "idPestana".
        }
    }

    public class MyTemplate : ITemplate
    {
        ListItemType _type;
        string _css_class;
        string _text;

        public MyTemplate(ListItemType type)
        {
            _type = type;
        }

        public MyTemplate(ListItemType type, string css_class, string text)
        {
            _css_class = css_class;
            _text = text;
        }

        public void InstantiateIn(Control control)
        {
            if (_type == ListItemType.Header)
            {
                control.Controls.Add(new LiteralControl("<span class='" + _css_class + "'>" + _text + "</span>"));
            }
        }
    }
}