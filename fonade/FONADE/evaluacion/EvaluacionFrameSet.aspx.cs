#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>28 - 02 - 2014</Fecha>
// <Archivo>EvaluacionFrameSet.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using LinqKit;
using System.Web;
using System.Data;
using System.Data.SqlClient;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class EvaluacionFrameSet : Negocio.Base_Page
    {
        public string codConvocatoria;
        public string codProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            /*
             * Modificado de acuerdo a especificaciones REVISIÓN 3 – GERENTE INTERVENTOR VALIDACIÓN FINAL DE PLAN DE NEGOCIO GERINT21
             * Jorge Martinez
             */
            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor){
                if (ScriptManager.GetCurrent(this).GetRegisteredClientScriptBlocks().Where(s => s.Key == "acs").ToList().Count == 0){
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "acs", "alert('Restriccion de acceso \n El perfil no cuenta con la funcionalidad solicitada.')", true);
                    Response.Redirect(Request.RequestContext.HttpContext.Request.UrlReferrer.AbsoluteUri);
                }
            }
            if (!(usuario.CodGrupo == Constantes.CONST_CoordinadorEvaluador || usuario.CodGrupo == Constantes.CONST_GerenteEvaluador))
            {
                TabPanel1.Visible = false;
                TabPanel1.Enabled = false;
            }

            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()))
                {
                    codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();

                    if (!Negocio.PlanDeNegocioV2.Utilidad.ProyectoGeneral.VerificarVersionProyecto(Convert.ToInt32(codProyecto), Constantes.CONST_PlanV1))
                    {
                        Response.Redirect("~/PlanDeNegocioV2/Evaluacion/Master/MainMenu.aspx?codproyecto=" + codProyecto);                        
                        Context.ApplicationInstance.CompleteRequest();                       
                    }
                }
                else
                {
                    Response.Redirect("../MiPerfil/Home.aspx");
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()))
                {
                    codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();
                }
            }
            catch (NullReferenceException) { return; }
            setTabsStatus();
            
        }

        private void setTabsStatus()
        {
            var codEstado = (from p in consultas.Db.Proyecto
                             where p.Id_Proyecto == Convert.ToInt32(codProyecto)
                             select p).FirstOrDefault();

            int codigoEstado = 0;
            if (codEstado != null)
            {
                codigoEstado = codEstado.CodEstado;
            }

            
        }

        protected string setTab(int idPestana)
        {
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

            var predicate = PredicateBuilder.False<TareaUsuarioRepeticion>();

            if (idtabs.Count > 0)
            {
                foreach (short idtab in idtabs)
                {
                    predicate =
                        predicate.Or(
                            t =>
                            SqlMethods.Like(t.Parametros, "Codproyecto=" + codProyecto + "&tab=" + idtab + "&Campo=%"));
                }
            }
            else
                predicate = predicate.Or(t => SqlMethods.Like(t.Parametros, "%tab=" + idPestana + "%"));
      
            String Sql = "SELECT realizado FROM tabEvaluacionProyecto WHERE CodTabEvaluacion= " + idPestana + " AND codproyecto= " + codProyecto + " AND codConvocatoria= " + codConvocatoria;

            DataTable estado = new DataTable();
            estado = consultas.ObtenerDataTable(Sql, "text");

            if (estado.Rows.Count > 0)
            {
                var estatus = Convert.ToBoolean(estado.Rows[0]["realizado"].ToString());

                if (estatus == true)
                {
                    css_class = "tab_aprobado";
                }
            }

            return css_class;
        }
    }

    
}public class MyTemplate : ITemplate
    {
        private readonly string _css_class;
        private readonly string _text;
        private readonly ListItemType _type;

        public MyTemplate(ListItemType type)
        {
            _type = type;
        }

        public MyTemplate(ListItemType type, string css_class, string text)
        {
            _css_class = css_class;
            _text = text;
        }

        #region ITemplate Members

        public void InstantiateIn(Control control)
        {
            if (_type == ListItemType.Header)
            {
                control.Controls.Add(new LiteralControl("<span class='" + _css_class + "'>" + _text + "</span>"));
            }
        }

        #endregion
    }

