using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.SoporteHelper.Archivos
{
    public partial class Contrato : System.Web.UI.Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {        
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            gvContratos.DataBind();
        }

        public List<ArchivoContrato> GetArchivos(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    int? CodOperador;

                    CodOperador = (from p in db.Proyecto
                                   where p.Id_Proyecto == codigoProyecto
                                   select p.codOperador).FirstOrDefault();
                                       

                    if (CodOperador == Usuario.CodOperador || Usuario.CodOperador == null) {

                        return db.ContratosArchivosAnexos
                        .Where(
                               selector =>
                               selector.CodProyecto.Equals(codigoProyecto))
                        .Select(
                               filter => new ArchivoContrato
                               {
                                   Id = filter.IdContratoArchivoAnexo,
                                   Nombre = filter.NombreArchivo,
                                   CodigoProyecto = filter.CodProyecto.GetValueOrDefault(0),
                                   Url = filter.ruta
                               }).ToList();
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert(' “No hay Archivos, Usted no está asociado al operador del proyecto ');", true);
                        return new List<ArchivoContrato>();
                    }

                }
                else
                {
                    return new List<ArchivoContrato>();
                }
            }
        }

        protected void linkAdd_Click(object sender, EventArgs e)
        {

        }

        protected void gvFormulacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var idArchivoContrato = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var entity = db.ContratosArchivosAnexos.Single(filter => filter.IdContratoArchivoAnexo.Equals(idArchivoContrato));

                            db.ContratosArchivosAnexos.DeleteOnSubmit(entity);
                            db.SubmitChanges();
                            gvContratos.DataBind();
                        }
                    }
                }
                if (e.CommandName.Equals("VerArchivo"))
                {
                    if (e.CommandArgument != null)
                    {
                        string[] parametros;
                        parametros = e.CommandArgument.ToString().Split(';');

                        var nombreArchivo = parametros[0];                        
                        var urlArchivo = parametros[1];

                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
                        Response.TransmitFile(urlArchivo);
                        Response.End();
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

    public class ArchivoContrato
    {
        public int Id { get; set; }
        public string Nombre { get; set; }               
        public int CodigoProyecto { get; set; }
        private string url;
        public string Url { get {
                return ConfigurationManager.AppSettings.Get("RutaIP") + url;
            } set {
                url = value;
            } }
    }
}