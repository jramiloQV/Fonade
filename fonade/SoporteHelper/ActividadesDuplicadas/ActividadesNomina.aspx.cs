using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.ActividadesDuplicadas
{
    public partial class ActividadesNomina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvActividad.DataBind();
        }

        public List<ActividadNomina> GetActividades(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return db.InterventorNominas.Where(filter => filter.CodProyecto.Equals(codigoProyecto)).Select(selector => new ActividadNomina
                    {
                        Id = selector.Id_Nomina,
                        Nombre = selector.Cargo,
                        Tipo = selector.Tipo,                                                
                        NumeroAvances = db.AvanceCargoPOMes
                                        .Where(filter => filter.CodCargo.Equals(selector.Id_Nomina))
                                        .GroupBy(group => group.Mes)
                                        .Select( selector2 => selector2.OrderByDescending(p => p.Id).FirstOrDefault())
                                        .Count(),
                        CodigoProyecto = selector.CodProyecto
                    }).ToList();
                }
                else
                {
                    return new List<ActividadNomina>();
                }
            }
        }

        protected void gvActividad_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            var entity = db.InterventorNominas.Single(filter => filter.Id_Nomina.Equals(codigoActividad));

                            db.InterventorNominas.DeleteOnSubmit(entity);
                            db.SubmitChanges();
                            gvActividad.DataBind();
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

    public class ActividadNomina
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }        
        public int NumeroAvances { get; set; }
        public bool AllowDelete
        {
            get
            {
                return NumeroAvances.Equals(0);
            }
            set { }
        }
        public int CodigoProyecto { get; set; }
    }
}