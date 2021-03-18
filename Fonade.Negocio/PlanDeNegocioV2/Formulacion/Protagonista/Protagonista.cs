using Datos;
using Datos.DataType;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.Protagonista
{
    public class Protagonista
    {
        #region CLIENTES
        /// <summary>
        /// Insertar cliente en modulo protagonista
        /// </summary>
        /// <param name="entCliente">ProyectoProtagonistaCliente</param>
        /// <param name="msg">string</param>
        /// <returns>bool</returns>
        public static bool InsertarClientes(ProyectoProtagonistaCliente entCliente, out string msg)
        {
            try
            {
                if (entCliente.Perfil.Length > 1000)
                {
                    msg = Mensajes.Mensajes.GetMensaje(1);
                    return false;
                }
                if (entCliente.Localizacion.Length > 1000)
                {
                    msg = Mensajes.Mensajes.GetMensaje(2);
                    return false;
                }
                if (entCliente.Justificacion.Length > 1000)
                {
                    msg = Mensajes.Mensajes.GetMensaje(3);
                    return false;
                }
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    if ((from row in db.ProyectoProtagonistaClientes where row.IdProyecto == entCliente.IdProyecto select row).Count() > 98)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(4);
                        return false;
                    }
                    if ((from row in db.ProyectoProtagonistaClientes
                         where row.Nombre == entCliente.Nombre
                         where row.IdProyecto == entCliente.IdProyecto
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }

                    db.ProyectoProtagonistaClientes.InsertOnSubmit(entCliente);
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(6);



                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }


        }

        public static bool EditarClientes(ProyectoProtagonistaCliente entCliente, out string msg)
        {
            try
            {                
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {

                    if ((from row in db.ProyectoProtagonistaClientes
                         where row.Nombre == entCliente.Nombre
                         && row.IdCliente != entCliente.IdCliente
                         && row.IdProyecto == entCliente.IdProyecto
                         select row).Count() > 0)
                    {
                        msg = Mensajes.Mensajes.GetMensaje(5);
                        return false;
                    }

                    var entClientedb = (from row in db.ProyectoProtagonistaClientes
                                        where row.IdCliente == entCliente.IdCliente
                                        select row).First();

                    entClientedb.Nombre = entCliente.Nombre;
                    entClientedb.Perfil = entCliente.Perfil;
                    entClientedb.Justificacion = entCliente.Justificacion;
                    entClientedb.Localizacion = entCliente.Localizacion;
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }


        }

        public static List<ProyectoProtagonistaCliente> GetClientes(int IdProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listClientes = (from row in db.ProyectoProtagonistaClientes
                                    where row.IdProyecto == IdProyecto
                                    select row);

                return listClientes.ToList();

            }

        }

        public static List<CondicionesCliente> GetClientesCondiciones(int IdProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listClientes = (from row in db.ProyectoProtagonistaClientes
                                    where row.IdProyecto == IdProyecto
                                    select row);

                return listClientes.Select(reg => new
                {
                    Cliente = reg.Nombre,
                    IdCliente = reg.IdCliente,
                    Condicion = reg.ProyectoCondicionesComerciales.Where(x => x.IdCliente == reg.IdCliente).SingleOrDefault()
                }).Select(regfinal =>
                        new CondicionesCliente()
                        {
                            IdCliente = regfinal.IdCliente,
                            Cliente = regfinal.Cliente,
                            FrecuenciaCompra = regfinal.Condicion.FrecuenciaCompra,
                            CaracteristicasCompra = regfinal.Condicion.CaracteristicasCompra,
                            SitioCompra = regfinal.Condicion.SitioCompra,
                            FormaPago = regfinal.Condicion.FormaPago,
                            Precio = regfinal.Condicion != null ? regfinal.Condicion.Precio : 0,
                            RequisitosPostVenta = regfinal.Condicion.RequisitosPostVenta,
                            Garantias = regfinal.Condicion.Garantias,
                            Margen = regfinal.Condicion.Margen
                        }).ToList();

            }

        }

        public static ProyectoProtagonistaCliente GetCliente(int IdCliente)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var listClientes = (from row in db.ProyectoProtagonistaClientes
                                    where row.IdCliente == IdCliente
                                    select row);

                return listClientes.First();

            }

        }

        public static CondicionesCliente GetClienteCondiciones(int IdCliente)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                ProyectoProtagonistaCliente regfinal = (from row in db.ProyectoProtagonistaClientes
                                                        where row.IdCliente == IdCliente
                                                        select row).SingleOrDefault();

                var condicion = regfinal.ProyectoCondicionesComerciales.SingleOrDefault();

                return new CondicionesCliente()
                {
                    IdCliente = regfinal.IdCliente,
                    Cliente = regfinal.Nombre,
                    FrecuenciaCompra = condicion != null ? condicion.FrecuenciaCompra : "",
                    CaracteristicasCompra = condicion != null ? condicion.CaracteristicasCompra : "",
                    SitioCompra = condicion != null ? condicion.SitioCompra : "",
                    FormaPago = condicion != null ? condicion.FormaPago : "",
                    Precio = condicion != null ? condicion.Precio : 0,
                    RequisitosPostVenta = condicion != null ? condicion.RequisitosPostVenta : "",
                    Garantias = condicion != null ? condicion.Garantias : "",
                    Margen = condicion != null ? condicion.Margen : ""

                };

            }

        }

        public static bool EliminarClientes(int IdCliente, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entClientedb = (from row in db.ProyectoProtagonistaClientes
                                        where row.IdCliente == IdCliente
                                        select row).First();

                    db.ProyectoProtagonistaClientes.DeleteOnSubmit(entClientedb);
                    db.SubmitChanges();
                    msg = Mensajes.Mensajes.GetMensaje(9);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }


        }
        #endregion

        #region PROTAGONISTA
        public static bool InsertarProtagonista(ProyectoProtagonista entProtagonista, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entProtagonistadb = (from row in db.ProyectoProtagonistas
                                             where row.IdProyecto == entProtagonista.IdProyecto
                                             select row).FirstOrDefault();

                    //insert-update
                    if (entProtagonistadb == null)
                        db.ProyectoProtagonistas.InsertOnSubmit(entProtagonista);
                    else
                    {
                        entProtagonistadb.NecesidadesPotencialesClientes = entProtagonista.NecesidadesPotencialesClientes;
                        entProtagonistadb.NecesidadesPotencialesConsumidores = entProtagonista.NecesidadesPotencialesConsumidores;
                        entProtagonistadb.PerfilConsumidor = entProtagonista.PerfilConsumidor;
                        entProtagonistadb.PerfilesDiferentes = entProtagonista.PerfilesDiferentes;
                    }
                    db.SubmitChanges();

                    if (!entProtagonista.PerfilesDiferentes) setDatosFormulario(entProtagonista.IdProyecto);

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }
        }

        public static ProyectoProtagonista GetProtagonista(int IdProyecto)
        {

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {

                var entProtagonistadb = (from row in db.ProyectoProtagonistas
                                         where row.IdProyecto == IdProyecto
                                         select row).FirstOrDefault();

                return entProtagonistadb;
            }
        }

        /// <summary>
        /// Limpia los campos cuando cliente tambien es consumidor
        /// </summary>
        /// <param name="idProyecto">int</param>
        /// <returns>bool</returns>
        public static bool setDatosFormulario(int idProyecto)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var objActual = (from datos in db.ProyectoDesarrolloSolucions
                                     where datos.IdProyecto == idProyecto
                                     select datos).FirstOrDefault();
                    if (objActual != null)
                    {
                        objActual.CaracteristicasCompra = null;
                        objActual.DondeCompra = null;
                        objActual.FrecuenciaCompra = null;
                        objActual.Precio = null;

                        db.SubmitChanges();
                    }

                }
            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }
        #endregion

    }
}
