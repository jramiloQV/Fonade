#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>24 - 03 - 2014</Fecha>
// <Archivo>SeguimientoFrameset.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Linq;
using Datos;
using Fonade.Negocio;
using LinqKit;
using System.Web;

#endregion

namespace Fonade.FONADE.interventoria
{
    public partial class SeguimientoFrameset : Base_Page
    {
        public string codConvocatoria;
        public string codProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()))
                {
                    codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                }
                else
                {
                    Response.Redirect("../MiPerfil/Home.aspx");
                }
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()))
                {
                    codConvocatoria = HttpContext.Current.Session["CodEmpresa"].ToString();
                }
            }
            catch (Exception)
            {
                Response.Redirect("../MiPerfil/Home.aspx");
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

            query = query.Where(predicate);
            int cuantos = query.Count();

            if (cuantos > 0)
            {
                css_class = "tab_advertencia";
            }
            else
            {
                var tbp =
                    consultas.Db.TabProyectos.FirstOrDefault(
                        t => t.CodProyecto == Convert.ToInt32(codProyecto) && t.CodTab == idPestana);
                if (tbp != null && tbp.Realizado)
                {
                    css_class = "tab_aprobado";
                }
            }
            return css_class;
        }
    }
}