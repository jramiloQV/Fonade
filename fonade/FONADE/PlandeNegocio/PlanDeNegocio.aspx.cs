using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.FONADE.PlandeNegocio
{
    public partial class PlanDeNegocio : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["codigoPlanDeNegocio"] = null;
        }

        /// <summary>
        /// Obtiene el listado de planes de negocio de ese usuario.
        /// </summary>
        /// <returns>Lista de planes de negocio</returns>
        public List<PlanDeNegocioDetalle> getPlanesDeNegocio(int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var planesDeNegocio = (from planes in db.Proyecto
                                       join ciudad in db.Ciudad on planes.CodCiudad equals ciudad.Id_Ciudad
                                       join departamento in db.departamento on ciudad.CodDepartamento equals departamento.Id_Departamento
                                       where planes.CodContacto == usuario.IdContacto
                                             && planes.CodInstitucion == usuario.CodInstitucion
                                             && planes.Inactivo == false
                                       select new PlanDeNegocioDetalle
                                       {
                                           Id = planes.Id_Proyecto,
                                           Nombre = planes.NomProyecto,
                                           Ciudad = ciudad.NomCiudad,
                                           Departamento = departamento.NomDepartamento,
                                           IdVersionProyecto = planes.IdVersionProyecto
                                       }).OrderBy(plan => plan.Nombre).Skip(startIndex).Take(maxRows).ToList();

                return planesDeNegocio;
            }
        }

        /// <summary>
        /// Obtiene el numero de planes de negocio de ese usuario.
        /// </summary>
        /// <returns>Numero de planes de negocio</returns>
        public int getPlanesDeNegocioCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener listado de planes de negocio
                var planesDeNegocio = (from planes in db.Proyecto
                                       join ciudad in db.Ciudad on planes.CodCiudad equals ciudad.Id_Ciudad
                                       join departamento in db.departamento on ciudad.CodDepartamento equals departamento.Id_Departamento
                                       where planes.CodContacto == usuario.IdContacto
                                             && planes.CodInstitucion == usuario.CodInstitucion
                                             && planes.Inactivo == false
                                       select new PlanDeNegocioDetalle
                                       {
                                           Id = planes.Id_Proyecto,
                                           Nombre = planes.NomProyecto,
                                           Ciudad = ciudad.NomCiudad,
                                           Departamento = departamento.NomDepartamento
                                       }).Count();

                return planesDeNegocio;
            }
        }

        protected void gvPlanesDeNegocio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verPlanDeNegocio"))
            {
                if (e.CommandArgument != null)
                {
                    int codigoPlanDeNegocio = Convert.ToInt32(e.CommandArgument);

                    Session["codigoPlanDeNegocio"] = codigoPlanDeNegocio;
                    Response.Redirect("CrearPlanDeNegocio.aspx?IdVersionProyecto="+((LinkButton)e.CommandSource).Attributes["IdVersionProyecto"]);
                }
            }
        }
    }

    public class PlanDeNegocioDetalle
    {
        public int Id { get; set; }
        private string _nombre;
        public string Nombre
        {
            get
            {
                return this._nombre.htmlDecode();
            }
            set
            {
                _nombre = value;
            }
        }
        public string Descripcion { get; set; }
        public int CodigoCiudad { get; set; }
        public string Ciudad { get; set; }
        public int CodigoDepartamento { get; set; }
        public string Departamento { get; set; }
        public int CodigoSector { get; set; }
        public int CodigoSubSector { get; set; }
        public int Estado { get; set; }
        public int? IdVersionProyecto { get; set; }
    }
}