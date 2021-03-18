using Datos;
using Fonade.Account;
using Fonade.Clases;
using Fonade.Negocio.Proyecto;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Ejecucion.TerminosSCD
{
    public partial class TerminosSCD : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, false); } set { } }

        string NombreArchivo = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Emprendedor && validarTerminosSCD(usuario.IdContacto))
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");

            NombreArchivo = usuario.IdContacto + "_TerminosYCondicionesSCD.pdf";
        }

        ProyectoController proyectoController = new ProyectoController();
        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private void ActualizarTerminosSCDEmprendedor(int _codUsuario)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var emp = (from e in db.Contacto
                           where e.Id_Contacto == _codUsuario
                           select e).FirstOrDefault();

                emp.AceptoTerminosSCD = true;

                db.SubmitChanges();
            }
        }

        protected void btnAceptarTerminos_Click(object sender, EventArgs e)
        {
            try
            {
                if (!chkTerminos.Checked)
                    throw new ApplicationException("Debe aceptar los términos y condiciones para poder continuar.");

                var codigoProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                //int codProyecto = proyectoController.codProyectoXEmprendedor(usuario.IdContacto);

                var virtualDirectory = CreateDirectory(codigoProyecto);
                var htmlTerminosSCD = File.ReadAllText(HttpContext.Current.Server.MapPath("TerminosSCD.html")).Replace("[FECHAYHORA]", DateTime.Now.getFechaConFormato(true)).Replace("[EMPRENDEDOR]", usuario.Nombres + " " + usuario.Apellidos).Replace("[CEDULA]", usuario.Identificacion.ToString());

                HtmlToPdf(htmlTerminosSCD, baseDirectory + virtualDirectory, PageSize.LEGAL, 10, 10, 30, 65, false);

                //Actualizar TerminosSCD del emprendedor
                ActualizarTerminosSCDEmprendedor(usuario.IdContacto);

                //Ingresar en la tabla Contrato
                IngresarInfoContrato(usuario.IdContacto, codigoProyecto
                                    , ConfigurationManager.AppSettings.Get("DirVirtual") + virtualDirectory);

                HttpContext.Current.Session["usuarioLogged"] = usuario;
                Response.Redirect("~/FONADE/MiPerfil/Home.aspx");
            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentalo de nuevo. Detalle : " + ex.Message;
            }
        }

        private void IngresarInfoContrato(int codigoContacto, int codigoProyecto, string rutaArchivo)
        {
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var currentEntity = db.ContratosArchivosAnexos
                                        .FirstOrDefault(filter => filter.CodContacto.Equals(codigoContacto) 
                                                        && filter.NombreArchivo.Contains("TerminosYCondicionesSCD"));

                string nombreArchivo = NombreArchivo;

                if (currentEntity == null)
                {
                    var documento = new ContratosArchivosAnexo
                    {
                        CodProyecto = codigoProyecto,
                        ruta = rutaArchivo,
                        NombreArchivo = nombreArchivo,
                        CodContacto = codigoContacto,
                        FechaIngreso = DateTime.Now
                    };

                    db.ContratosArchivosAnexos.InsertOnSubmit(documento);
                }
                else
                {
                    currentEntity.CodProyecto = codigoProyecto;
                    currentEntity.ruta = rutaArchivo;
                }

                db.SubmitChanges();
            }
        }

        void HtmlToPdf(string htmlAnexo, string rutaDestinoAnexo, Rectangle tamanoPagina, int margenDerecho, int margenIzquierdo, int margenTop, int margenBotton, Boolean rotate)
        {
            Byte[] bytes;
            PdfWriter writer = null;
            HTMLWorker htmlWorker = null;

            try
            {
                using (var ms = new MemoryStream())
                {
                    using (var doc = new Document(tamanoPagina, margenDerecho, margenIzquierdo, margenTop, margenBotton))
                    {
                        if (rotate)
                            doc.SetPageSize(PageSize.LEGAL.Rotate());

                        writer = PdfWriter.GetInstance(doc, ms);

                        doc.Open();

                        var example_html = htmlAnexo;
                        htmlWorker = new HTMLWorker(doc);

                        using (var sr = new StringReader(example_html))
                        {
                            htmlWorker.Parse(sr);
                        }
                        doc.Close();
                    }
                    bytes = ms.ToArray();
                }

                System.IO.File.WriteAllBytes(rutaDestinoAnexo, bytes);
            }
            finally
            {
                if (writer != null) writer.Dispose();
                if (htmlWorker != null) htmlWorker.Dispose();
            }
        }

        public string baseDirectory = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual");
        
        public string CreateDirectory(int codigoProyecto)
        {
            string fileName = NombreArchivo;
            string nombreFinal = codigoProyecto + "_" + fileName;
            var partialDirectory = "Proyecto\\Proyecto_"+codigoProyecto+"\\"; ;
            var finalDirectory = baseDirectory + partialDirectory;
            var virtualDirectory = partialDirectory + nombreFinal;

            if (!Directory.Exists(finalDirectory))
                Directory.CreateDirectory(finalDirectory);

            if (File.Exists(finalDirectory + nombreFinal))
                File.Delete(finalDirectory + nombreFinal);

            return virtualDirectory;
        }

        private bool validarTerminosSCD(int _codEmprendedor)
        {
            ProyectoController proyecto = new ProyectoController();
            return proyecto.validarTerminosSCDXEmprendedor(_codEmprendedor);
        }
    }
}