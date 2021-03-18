using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.SoporteHelper.Usuarios
{
    public partial class UsuariosPorProyecto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvUsuarios.DataBind();
        }

        public List<UsuarioPorProyecto> GetUsuarios(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return db.UsuariosPorProyecto(codigoProyecto).Select(selector => new UsuarioPorProyecto
                    {
                        CodigoProyecto = selector.CodigoProyecto,
                        NombreProyecto = selector.NombreProyecto,
                        EstadoProyecto = selector.EstadoProyecto,
                        CodigoContacto = selector.CodigoContacto,
                        NombresContacto = selector.NombresContacto,
                        EmailContacto = selector.EmailContacto,
                        ClaveContacto = selector.ClaveContacto,
                        EstadoContacto = selector.EstadoContacto,
                        GrupoContacto = selector.GrupoContacto,
                        RolContacto = selector.RolContacto,
                    }).ToList();
                }
                else
                {
                    return new List<UsuarioPorProyecto>();
                }
            }
        }        
    }

    public class UsuarioPorProyecto {
        public int CodigoProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public string EstadoProyecto { get; set; }
        public int CodigoContacto { get; set; }
        public string NombresContacto { get; set; }
        public string EmailContacto { get; set; }
        public string ClaveContacto { get; set; }
        public string EstadoContacto { get; set; }
        public string GrupoContacto { get; set; }
        public string RolContacto { get; set; }
    }

}