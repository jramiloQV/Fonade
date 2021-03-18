using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.FONADE.Fiduciaria
{
    /// <summary>
    /// Listado de pagos sin descargar
    /// Author : marztres@gmail.com
    /// </summary>
    public partial class SolicitudesDePagoSinDescargar : Negocio.Base_Page
    {
        private static Int32 codigoUsuarioContacto = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codigoUsuarioContacto = usuario.IdContacto;                                 
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// Listado de solicitudes de pago.
        /// </summary>
        /// <param name="codigoActa"></param>
        /// <returns></returns>
        public List<SolicitudPagoFiduciaria> getSolicitudesDePago(int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener las solicitudes de pago
                var solicitudesDePago = (from pagoActaSolicitud in db.PagosActaSolicitudes
                                         join pagoActaSolicitudPago in db.PagosActaSolicitudPagos on pagoActaSolicitud.Id_Acta equals pagoActaSolicitudPago.CodPagosActaSolicitudes
                                         where pagoActaSolicitud.DescargadoFA == false &&
                                               pagoActaSolicitud.CodRechazoFirmaDigital.Equals(null) &&
                                               pagoActaSolicitud.Tipo.ToLower().Equals("fonade") &&
                                               pagoActaSolicitudPago.Aprobado == true &&
                                               pagoActaSolicitud.CodContactoFiduciaria == codigoUsuarioContacto
                                         select new SolicitudPagoFiduciaria
                                         {
                                             codigoActaPago = pagoActaSolicitud.Id_Acta,
                                             fechaSolicitudPagoConFormato = pagoActaSolicitud.Fecha.Value != null ? FieldValidate.getFechaConFormato(pagoActaSolicitud.Fecha.Value,false) : "Fecha no disponible",
                                             fechaSolicitudPago = pagoActaSolicitud.Fecha,
                                             firma = pagoActaSolicitud.DatosFirma.Substring(0, 500),
                                             numeroSolicitudesPago = db.PagosActaSolicitudPagos.Count(pagos => pagos.Aprobado == true && pagos.CodPagosActaSolicitudes == pagoActaSolicitud.Id_Acta)
                                         }
                                         ).GroupBy(solicitudesPago => new { solicitudesPago.codigoActaPago, solicitudesPago.fechaSolicitudPago, solicitudesPago.firma }).Select(solicitudPago => solicitudPago.First()).Skip(startIndex).Take(maxRows).ToList();

                return solicitudesDePago;
            }            
        }
        /// <summary>
        ///     Listado de solicitudes de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void detalleSolicitud_RowCommand(object sender, GridViewCommandEventArgs e){
            if (e.CommandName.Equals("verDetalleSolicitud"))
            {
                if (e.CommandArgument != null)
                {
                    string codigoActaPago = e.CommandArgument.ToString();
                    //CodigoActaPago = e.CommandArgument.ToString();

                    Session["CodActaFonade"] = codigoActaPago;
                    Response.Redirect("descargarPagos.aspx");
                }
            }
        }

        public string CodigoActaPago{ get; set; }
        /// <summary>
        ///     Contar cuantos registros se encuentran retornara la consulta.
        /// </summary>
        /// <returns> Numero de registros de la consulta </returns>
        public int getSolicitudesDePagoCount()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener las solicitudes de pago
                var solicitudesDePago = (from pagoActaSolicitud in db.PagosActaSolicitudes
                                         join pagoActaSolicitudPago in db.PagosActaSolicitudPagos on pagoActaSolicitud.Id_Acta equals pagoActaSolicitudPago.CodPagosActaSolicitudes
                                         where pagoActaSolicitud.DescargadoFA == false &&
                                               pagoActaSolicitud.CodRechazoFirmaDigital.Equals(null) &&
                                               pagoActaSolicitud.Tipo.ToLower().Equals("fonade") &&
                                               pagoActaSolicitudPago.Aprobado == true &&
                                               pagoActaSolicitud.CodContactoFiduciaria == codigoUsuarioContacto
                                         select new SolicitudPagoFiduciaria
                                         {
                                             codigoActaPago = pagoActaSolicitud.Id_Acta,
                                             fechaSolicitudPagoConFormato = pagoActaSolicitud.Fecha.Value != null ? FieldValidate.getFechaConFormato(pagoActaSolicitud.Fecha.Value,false) : "Fecha no disponible",
                                             fechaSolicitudPago = pagoActaSolicitud.Fecha,
                                             firma = pagoActaSolicitud.DatosFirma.Substring(0, 500),
                                             numeroSolicitudesPago = db.PagosActaSolicitudPagos.Count(pagos => pagos.Aprobado == true && pagos.CodPagosActaSolicitudes == pagoActaSolicitud.Id_Acta)
                                         }
                                         ).GroupBy(solicitudesPago => new { solicitudesPago.codigoActaPago, solicitudesPago.fechaSolicitudPago, solicitudesPago.firma }).Select(solicitudPago => solicitudPago.First()).Count();

                return solicitudesDePago;
            }
        }
    }
 
   public class SolicitudPagoFiduciaria {
        public Int32 codigoActaPago {get; set;}
        public string fechaSolicitudPagoConFormato { get; set; }
        public string firma { get; set; }
        public Int32 numeroSolicitudesPago { get; set; }
        public DateTime? fechaSolicitudPago { get; set; }
        public string descargado { get; set; }

        public Int32 codigoActaFiduciaria { get; set; }
   }
}