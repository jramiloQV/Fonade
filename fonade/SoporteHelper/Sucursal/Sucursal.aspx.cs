using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;

namespace Fonade.SoporteHelper.Sucursal
{
    public partial class Sucursal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public List<PagoBanco> GetBancos()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.PagoBancos.OrderBy(order => order.NomBanco).ToList();
            }
        }
     
        public List<PagoSucursal> GetSucursales(int idBanco)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.PagoSucursals.Where(filter => filter.CodPagoBanco.Equals(idBanco)).OrderBy(order => order.NomPagoSucursal).ToList();
            }
        }

        protected void gvSucursales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EliminarSucursal"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoSucursal = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var entity = db.PagoSucursals.Single(filter => filter.Id.Equals(codigoSucursal));

                            db.PagoSucursals.DeleteOnSubmit(entity);
                            db.SubmitChanges();

                            gvSucursales.DataBind();
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

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {                                
                FieldValidate.ValidateNumeric("Codigo sucursal",txtCodigoSucursal.Text,true);
                FieldValidate.ValidateString("Nombre sucursal", txtSucursal.Text, true);

                Int32 codigoBanco = Convert.ToInt32(cmbBancos.SelectedValue);
                Int32 codigoSucursal = Convert.ToInt32(txtCodigoSucursal.Text);                
                string nombreSucursal = txtSucursal.Text;

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    if (db.PagoSucursals.Any(exist => exist.CodPagoBanco.Equals(codigoBanco) && exist.Id_PagoSucursal.Equals(codigoSucursal)))
                        throw new ApplicationException("Existe una sucursal con ese codigo");

                    var sucursal = new PagoSucursal
                    {
                        CodPagoBanco = codigoBanco,
                        Id_PagoSucursal = codigoSucursal,
                        NomPagoSucursal = nombreSucursal,
                        CodPagoSucursal = txtCodigoSucursal.Text
                    };

                    db.PagoSucursals.InsertOnSubmit(sucursal);
                    db.SubmitChanges();
                    gvSucursales.DataBind();
                    txtCodigoSucursal.Text = string.Empty;
                    txtSucursal.Text = string.Empty;
                }
            }
            catch (ApplicationException ex) {
                lblError.Visible = true;
                lblError.Text = "Advertencia, detalle :" + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error, detalle :" + ex.Message;
            }
        }
    }
}