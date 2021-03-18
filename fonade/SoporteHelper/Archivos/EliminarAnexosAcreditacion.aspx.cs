using Datos;
using Fonade.Clases;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.Archivos
{
    public partial class EliminarAnexosAcreditacion : Negocio.Base_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {

                FieldValidate.ValidateNumeric("Código de proyecto", txtCodigo.Text, true);

                lblError.Visible = false;
                cargarGrilla(Convert.ToInt32(txtCodigo.Text));
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
        string _conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private void cargarGrilla(int _codProyecto)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(_conexion))
            {
                var query = (from caa in db.ContactoArchivosAnexo1s
                             where caa.CodProyecto == _codProyecto
                             select new
                             {
                                 Id_documento = caa.Id_ContactoArchivosAnexos,
                                 CodigoProyecto = caa.CodProyecto,
                                 Nombre = caa.NombreArchivo,
                                 Descripcion = caa.Descripcion,
                                 Ruta = caa.ruta,
                                 NombreEnPlataforma = ""
                             }).ToList();

                var query2 = Negocio.PlanDeNegocioV2.Formulacion.Anexos.Anexos.getDocumentosAcreditacion(_codProyecto).ToList();

                var queryfinal = (from q1 in query
                                  join q2 in query2 on q1.Id_documento equals q2.Id_documento
                                  select new
                                  {
                                      Id_documento = q1.Id_documento,
                                      CodigoProyecto = q1.CodigoProyecto,
                                      Nombre = q1.Nombre,
                                      Descripcion = q1.Descripcion,
                                      Ruta = q1.Ruta,
                                      NombreEnPlataforma = q2.Descripcion
                                  });

                gvAnexos.DataSource = queryfinal;
                gvAnexos.DataBind();
            }


        }

        private void Alert(string mensaje)
        {
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "msg", "alert('" + mensaje + "');", true);
        }

        protected void gvAnexos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            AccionGrid(e.CommandName.ToString(), e.CommandArgument.ToString());
        }

        protected void AccionGrid(string accion, string argumento)
        {
            try
            {
                switch (accion)
                {
                    case "VerDocumento":
                        string url = ConfigurationManager.AppSettings.Get("RutaIP") + argumento;
                        Utilidades.DescargarArchivo(url);
                        break;

                    case "Eliminar":
                        eliminarDocumento(Convert.ToInt32(argumento));
                        cargarGrilla(Convert.ToInt32(txtCodigo.Text));
                        Alert("Se elimino el archivo correctamente");
                        break;
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message);
            }
        }

        private void eliminarDocumento(int _codDococumento)
        {
            var idArchivoContacto = _codDococumento;
            string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(conexion))
            {
                var entity = db.ContactoArchivosAnexo1s.Single(filter => filter.Id_ContactoArchivosAnexos.Equals(idArchivoContacto));
               
                using (FonadeDBLightDataContext db2 = new FonadeDBLightDataContext(conexion))
                {
                    LogSoporte log = new LogSoporte
                    {
                        Accion = "Eliminar",
                        codContacto = usuario.IdContacto,
                        Detalle = DateTime.Now.ToString() + "Se eliminó el archivo de anexos -> " + entity.CodProyecto + "-" + entity.Descripcion

                    };

                    db2.LogSoporte.InsertOnSubmit(log);
                    db2.SubmitChanges();
                }

                db.ContactoArchivosAnexo1s.DeleteOnSubmit(entity);
                db.SubmitChanges();

            }
        }
    }
}