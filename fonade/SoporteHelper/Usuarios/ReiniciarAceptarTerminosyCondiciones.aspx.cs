using Datos;
using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.Usuarios
{
    public partial class ReiniciarAceptarTerminosyCondiciones : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Restablecer"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoContacto = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var entity = db.Contacto.SingleOrDefault(filter => filter.Id_Contacto.Equals(codigoContacto));
                            if (entity != null)
                                entity.AceptoTerminosYCondiciones = false;
                            
                            db.SubmitChanges();
                            Alert("Indicar al emprendedor que puede ingresar nuevamente a la plataforma para aceptar términos y condiciones. ");
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

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
    
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipo.SelectedValue.Equals("id"))
                {
                    FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, true);
                }
                else
                {
                    FieldValidate.ValidateString("Nombre de proyecto", txtCodigo.Text, true);
                }
                lblError.Visible = false;
                cargarGrilla();
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        string _conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private void cargarGrilla()
        {
            int codProyecto = Convert.ToInt32(txtCodigo.Text);

            using (FonadeDBDataContext db = new FonadeDBDataContext(_conexion))
            {
                var query = (from pc in db.ProyectoContactos
                             join c in db.Contacto on pc.CodContacto equals c.Id_Contacto
                             join p in db.Proyecto on pc.CodProyecto equals p.Id_Proyecto
                             join e in db.Estados on p.CodEstado equals e.Id_Estado
                             where pc.CodRol == 3 && pc.Inactivo == false && c.Inactivo == false
                             && pc.CodProyecto == codProyecto
                             select new
                             {
                                 Id_Proyecto = pc.CodProyecto,
                                 NomProyecto = p.NomProyecto,
                                 NomEstado = e.NomEstado,
                                 Nombres = c.Nombres,
                                 Apellidos = c.Apellidos,
                                 Email = c.Email,
                                 Id_Contacto = c.Id_Contacto,
                                 Identificacion = c.Identificacion
                             }).ToList();

                gvMain.DataSource = query;
                gvMain.DataBind();
            }
        }
    }
}