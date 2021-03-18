using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.ActividadesDuplicadas
{
    public partial class ActividadesVentas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvActividad.DataBind();
        }

        public List<ActividadVentas> GetActividades(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return db.InterventorVentas.Where(filter => filter.CodProyecto.Equals(codigoProyecto)).Select(selector => new ActividadVentas
                    {
                        Id = selector.id_ventas,
                        Nombre = selector.NomProducto,
                        NumeroAvances = db.AvanceVentasPOMes
                                        .Where(filter => filter.CodProducto.Equals(selector.id_ventas))
                                        .GroupBy(group => group.Mes)
                                        .Select(selector2 => selector2.OrderByDescending(p => p.Id).FirstOrDefault())
                                        .Count(),
                        CodigoProyecto = selector.CodProyecto
                    }).ToList();
                }
                else
                {
                    return new List<ActividadVentas>();
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
                            var entity = db.InterventorVentas.Single(filter => filter.id_ventas.Equals(codigoActividad));

                            db.InterventorVentas.DeleteOnSubmit(entity);
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

    public class ActividadVentas
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
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