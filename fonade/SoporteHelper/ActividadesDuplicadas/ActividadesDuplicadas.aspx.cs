using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.ActividadesDuplicadas
{
    public partial class ActividadesDuplicadas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvActividadesDuplicadas.DataBind();
        }

        public List<ActividadDuplicada> GetActividadesDuplicadas(int codigoProyecto) {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ActividadesDuplicadas(codigoProyecto).Select(filter => new ActividadDuplicada
                {
                    Id = filter.Id_Actividad,
                    Nombre = filter.NomActividad,
                    Item = filter.item,
                    Metas = filter.Metas,
                    NumeroAvances = filter.AvancePorActividad.GetValueOrDefault(0),
                    CodigoProyecto = filter.CodProyecto
                }).ToList();
            }            
        }

        protected void gvActividadesDuplicadas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoActividad = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var entity = db.ProyectoActividadPOInterventors.Single(filter => filter.Id_Actividad.Equals(codigoActividad));

                            db.ProyectoActividadPOInterventors.DeleteOnSubmit(entity);
                            db.SubmitChanges();
                            gvActividadesDuplicadas.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }

    public class ActividadDuplicada {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Item { get; set; }
        public string Metas { get; set; }
        public int NumeroAvances { get; set; }
        public bool AllowDelete {
            get {
                return NumeroAvances.Equals(0);
            }
            set { }
        }
        public int CodigoProyecto { get; set; }
    }
}