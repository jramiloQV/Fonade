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
    /// Listado de pagos descargados
    /// Author : marztres@gmail.com
    /// </summary>
    public partial class SolicitudesDePagoDescargadas : Negocio.Base_Page
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
        public List<SolicitudPagoFiduciaria> getSolicitudesDePago(string numeroActa, int startIndex, int maxRows)
        {
            Int32 numeroActaFonade;
            Int32.TryParse(numeroActa,out numeroActaFonade);

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener las solicitudes de pago
                var solicitudesDePago = (from pagoActaSolicitud in db.PagosActaSolicitudes                                         
                                         where 
                                               pagoActaSolicitud.Tipo.ToLower().Equals("fiduciaria") &&
                                               pagoActaSolicitud.CodActaFonade != null &&
                                               pagoActaSolicitud.CodContactoFiduciaria == codigoUsuarioContacto &&
                                               (numeroActaFonade == 0 || (numeroActaFonade!=0 && pagoActaSolicitud.CodActaFonade == numeroActaFonade))
                                         select new SolicitudPagoFiduciaria
                                         {
                                             codigoActaPago = pagoActaSolicitud.Id_Acta,
                                             fechaSolicitudPagoConFormato = pagoActaSolicitud.Fecha.Value != null ? FieldValidate.getFechaConFormato(pagoActaSolicitud.Fecha.Value,false) : "Fecha no disponible",
                                             fechaSolicitudPago = pagoActaSolicitud.Fecha,
                                             firma = pagoActaSolicitud.DatosFirma.Substring(0, 500),
                                             numeroSolicitudesPago = pagoActaSolicitud.NumSolicitudes.GetValueOrDefault(0),
                                             descargado = pagoActaSolicitud.DescargadoFA.Value ? "Descargado" : "No descargada",
                                             codigoActaFiduciaria = pagoActaSolicitud.CodActaFonade.Value,                                            
                                         }
                                         ).GroupBy(solicitudesPago => new { solicitudesPago.codigoActaPago, solicitudesPago.fechaSolicitudPago, solicitudesPago.firma, solicitudesPago.codigoActaFiduciaria, solicitudesPago.descargado }).Select(solicitudPago => solicitudPago.First()).OrderByDescending(solicitudPago => solicitudPago.fechaSolicitudPago).Skip(startIndex).Take(maxRows).ToList();

                return solicitudesDePago;
            }
        }
        /// <summary>
        ///     Listado de solicitudes de datos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void detalleSolicitud_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("verDetalleSolicitud"))
            {
                if (e.CommandArgument != null)
                {
                    string codigoActaPago = e.CommandArgument.ToString();

                    Session["CodActaFonade"] = codigoActaPago;
                    Response.Redirect("descargarPagos.aspx");
                }
            }
        }
        /// <summary>
        ///     Contar cuantos registros se encuentran retornara la consulta.
        /// </summary>
        /// <returns> Numero de registros de la consulta </returns>
        public int getSolicitudesDePagoCount(string numeroActa)
        {
            Int32 numeroActaFonade;
            Int32.TryParse(numeroActa, out numeroActaFonade);

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener las solicitudes de pago
                var solicitudesDePago = (from pagoActaSolicitud in db.PagosActaSolicitudes
                                         where
                                               pagoActaSolicitud.Tipo.ToLower().Equals("fiduciaria") &&
                                               pagoActaSolicitud.CodActaFonade != null &&
                                               pagoActaSolicitud.CodContactoFiduciaria == codigoUsuarioContacto &&
                                               (numeroActaFonade == 0 || (numeroActaFonade != 0 && pagoActaSolicitud.CodActaFonade == numeroActaFonade))
                                         select new SolicitudPagoFiduciaria
                                         {
                                             codigoActaPago = pagoActaSolicitud.Id_Acta,
                                             fechaSolicitudPagoConFormato = pagoActaSolicitud.Fecha.Value != null ? FieldValidate.getFechaConFormato(pagoActaSolicitud.Fecha.Value,false) : "Fecha no disponible",
                                             fechaSolicitudPago = pagoActaSolicitud.Fecha,
                                             firma = pagoActaSolicitud.DatosFirma.Substring(0, 500),
                                             numeroSolicitudesPago = pagoActaSolicitud.NumSolicitudes.GetValueOrDefault(0),
                                             descargado = pagoActaSolicitud.DescargadoFA.Value ? "Descargado" : "No descargada",
                                             codigoActaFiduciaria = pagoActaSolicitud.CodActaFonade.Value
                                         }
                                         ).GroupBy(solicitudesPago => new { solicitudesPago.codigoActaPago, solicitudesPago.fechaSolicitudPago, solicitudesPago.firma, solicitudesPago.codigoActaFiduciaria, solicitudesPago.descargado }).Select(solicitudPago => solicitudPago.First()).OrderByDescending(solicitudPago => solicitudPago.fechaSolicitudPago).Count();
              
                return solicitudesDePago;
            }
        }

        protected void btnBuscarActaPorNumero_Click(object sender, EventArgs e)
        {
            gvSolicitudesDePagoDescargadas.DataBind();
        }
    
    }

}