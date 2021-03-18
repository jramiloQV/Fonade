using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.Archivos
{
    public partial class Formulacion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvFormulacion.DataBind();
        }

        public List<Archivo> GetFormulacion(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.Documentos
                         .Where(
                                selector => 
                                selector.CodProyecto.Equals(codigoProyecto) 
                                && selector.Borrado.Equals(false) 
                                && selector.CodEstado.Equals(1))
                         .Select(
                                filter => new Archivo 
                                {
                                    Id = filter.Id_Documento,
                                    Nombre = filter.NomDocumento,
                                    Url = filter.URL,
                                    Fecha = filter.Fecha,
                                    CodigoProyecto = filter.CodProyecto,
                                    CodigoTipoDocumento = filter.CodDocumentoFormato,
                                    CodigoContacto = filter.CodContacto,
                                    Borrado = filter.Borrado                                       
                                }).ToList();
            }
        }
            
        protected void linkAdd_Click(object sender, EventArgs e)
        {

        }

        protected void gvFormulacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
            //    if (e.CommandName.Equals("Eliminar"))
            //    {
            //        if (e.CommandArgument != null)
            //        {
            //            var codigoActividad = Convert.ToInt32(e.CommandArgument.ToString());

            //            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            //            {
            //                var entity = db.ProyectoActividadPOInterventors.Single(filter => filter.Id_Actividad.Equals(codigoActividad));

            //                db.ProyectoActividadPOInterventors.DeleteOnSubmit(entity);
            //                db.SubmitChanges();
            //                gvActividadesDuplicadas.DataBind();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    lblError.Visible = true;
            //    lblError.Text = "Sucedio un error detalle :" + ex.Message;
            //}
        }
    }
    public class Archivo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
        public DateTime Fecha { get; set; }
        public int CodigoProyecto { get; set; }
        public int CodigoTipoDocumento { get; set; }
        public int CodigoContacto { get; set; }
        public bool Borrado { get; set; }        
    }
}