using System;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace Fonade.FONADE.evaluacion
{

    /// <summary>
    /// comportamiento master page
    /// </summary>
    public partial class EvaluacionMaster : Negocio.Base_Master
    {
        public string codProyecto;
        public string codConvocatoria;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session["CodProyecto"] != null)
            {
                codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                if (HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()))
                {
                    codConvocatoria = HttpContext.Current.Session["CodConvocatoria"].ToString();
                }

                if (codConvocatoria != null && codConvocatoria != "")
                {
                    string convocatoria = consultas.Db.Convocatoria.FirstOrDefault(t => t.Id_Convocatoria == Convert.ToInt32(codConvocatoria)).NomConvocatoria;
                    if (!string.IsNullOrEmpty(convocatoria))
                    {
                        lbl_convocatoria.Text = convocatoria + " - ";
                    }
                }


                var query = (from p in consultas.Db.Proyecto
                             from i in consultas.Db.Institucions
                             from s in consultas.Db.SubSector
                             from c in consultas.Db.Ciudad
                             from d in consultas.Db.departamento
                             where p.Id_Proyecto == Convert.ToInt32(codProyecto)
                                 && s.Id_SubSector == p.CodSubSector
                                 && i.Id_Institucion == p.CodInstitucion
                                 && p.CodCiudad == c.Id_Ciudad
                                 && c.CodDepartamento == d.Id_Departamento
                             select new
                             {
                                 p.Id_Proyecto,
                                 p.NomProyecto,
                                 s.NomSubSector,
                                 i.NomUnidad,
                                 c.NomCiudad,
                                 d.NomDepartamento,
                                 d.Id_Departamento,
                                 i.NomInstitucion,
                                 p.CodEstado
                             });

                foreach (var obj in query)
                {
                    lbl_title.Text = obj.Id_Proyecto
                        + " - " + obj.NomProyecto
                        + " - " + obj.NomUnidad
                        + " (" + obj.NomInstitucion + ")";
                    img_lt.Src = "~/Images/ImgLT" + obj.CodEstado + ".jpg";
                    img_map.Src = "~/Images/Mapas/" + remplazarTilde(obj.NomDepartamento) + "Pq.gif";
                    img_map.Alt = obj.NomCiudad + "(" + obj.NomDepartamento + ")";
                    link_map.HRef = "~/Mapas/Mapas.aspx?ver=1&pc=" + obj.Id_Departamento + "&pid=" + obj.Id_Proyecto;
                    lbl_convocatoria.Text = lbl_convocatoria.Text + obj.NomSubSector;
                    break;
                }

            }
            else
            {
                Response.Redirect("../MiPerfil/Home.aspx");
            }
        }

        private string remplazarTilde(string texto)
        {
            string result = texto.Replace("á", "a").Replace("é", "e").Replace("í", "i").Replace("ó", "o").Replace("ú", "u");
            return result;
        }

        protected void LoginStatus_LoggedOut(Object sender, System.EventArgs e)
        {
            MemoryCache.Default.Dispose();
            Session.Abandon();
        }
    }
}