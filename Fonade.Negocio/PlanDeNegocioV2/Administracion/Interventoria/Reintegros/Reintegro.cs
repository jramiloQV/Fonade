using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Fonade.Negocio.Utility;
using System.Configuration;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reintegros
{
    public class Reintegro
    {


        public static List<ReintegroDTO> GetPagosByProyecto(int? codOperadorUsuario, int codigoProyecto)
        {
            List<ReintegroDTO> reintegros = new List<ReintegroDTO>();
            //validar el operador del proyecto y usuario
            OperadorController operadorController = new OperadorController();
            if (operadorController.validarOperadorXProyecto(codOperadorUsuario, codigoProyecto))
            {

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    reintegros = (from pagos in db.PagoActividad
                                  where pagos.CodProyecto == codigoProyecto
                                        && pagos.Estado == Constantes.CONST_EstadoAprobadoFA
                                  select new ReintegroDTO
                                  {
                                      IdPago = pagos.Id_PagoActividad,
                                      Nombre = pagos.NomPagoActividad,
                                      ValorPago = pagos.CantidadDinero.GetValueOrDefault(0),
                                      CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(0),
                                      ValorUltimoReintegro = db.Reintegros.Any(filter1 => filter1.CodigoPago == pagos.Id_PagoActividad) ? db.Reintegros.Where(filter => filter.CodigoPago == pagos.Id_PagoActividad).OrderByDescending(FilterOrder => FilterOrder.FechaIngreso).FirstOrDefault().ValorReintegro : 0
                                  }).ToList();
                }
            }
            return reintegros;
        }

        public static List<ReintegroDTO> GetPagosByPagoId(int? codOperadorUsuario, int codigoPago)
        {
            List<ReintegroDTO> reintegros = new List<ReintegroDTO>();

            OperadorController operadorController = new OperadorController();
            //traer el codoperador del proyecto
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int codProyecto = (from pagos in db.PagoActividad
                                   where pagos.Id_PagoActividad == codigoPago
                                         && pagos.Estado == Constantes.CONST_EstadoAprobadoFA
                                   select pagos.CodProyecto).FirstOrDefault() ?? 0;

                if (codProyecto != 0)
                {
                    if (operadorController.validarOperadorXProyecto(codOperadorUsuario, codProyecto))
                    {
                        reintegros = (from pagos in db.PagoActividad
                                        where pagos.Id_PagoActividad == codigoPago
                                              && pagos.Estado == Constantes.CONST_EstadoAprobadoFA
                                        select new ReintegroDTO
                                        {
                                            IdPago = pagos.Id_PagoActividad,
                                            Nombre = pagos.NomPagoActividad,
                                            ValorPago = pagos.CantidadDinero.GetValueOrDefault(0),
                                            CodigoProyecto = pagos.CodProyecto.GetValueOrDefault(0),
                                            ValorUltimoReintegro = db.Reintegros.Any(filter1 => filter1.CodigoPago == pagos.Id_PagoActividad) ? db.Reintegros.Where(filter => filter.CodigoPago == pagos.Id_PagoActividad).OrderByDescending(FilterOrder => FilterOrder.FechaIngreso).FirstOrDefault().ValorReintegro : 0
                                        }).ToList();

                    }
                }
            }

            return reintegros;           
        }

        public static List<HistorialReintegroDTO> GetReintegros(int codigoPago)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from reintegros in db.Reintegros
                                join contacto in db.Contacto on reintegros.codigoContacto equals contacto.Id_Contacto
                                where reintegros.CodigoPago == codigoPago
                                select new HistorialReintegroDTO
                                {
                                    CodigoPago = reintegros.CodigoPago,
                                    CodigoReintegro = reintegros.Consecutivo,
                                    Descripcion = reintegros.Observacion,
                                    ValorReintegro = reintegros.ValorReintegro,
                                    ValorPagoConReintegro = reintegros.ValorPagoConReintegro,
                                    PresupuestoConReintegro = reintegros.PresupuestoPostReintegro,
                                    PresupuestoSinReintegro = reintegros.PresupuestoPreReintegro,
                                    FechaReintegro = reintegros.FechaReintegro,
                                    CodigoContacto = reintegros.codigoContacto,
                                    NombreContacto = contacto.Nombres + " " + contacto.Apellidos,
                                    ArchivoInforme = ConfigurationManager.AppSettings.Get("DirVirtual") + reintegros.archivoInforme
                                }).ToList();

                return entities;
            }
        }

        public static void Insert(Datos.Reintegro entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.Reintegros.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }
    }

    public class ReintegroDTO
    {
        public int IdPago { get; set; }
        public string Nombre { get; set; }
        public decimal ValorPago { get; set; }
        public string ValorPagoWithFormat
        {
            get
            {
                return ValorPago.moneyFormat();
            }
            set { }
        }
        public decimal ValorUltimoReintegro { get; set; }
        public string ValorUltimoReintegroWithFormat
        {
            get
            {
                return ValorUltimoReintegro.moneyFormat();
            }
            set { }
        }
        public int CodigoProyecto { get; set; }
    }

    public class HistorialReintegroDTO
    {
        public int CodigoPago { get; set; }
        public int CodigoReintegro { get; set; }
        public string Descripcion { get; set; }
        public decimal ValorReintegro { get; set; }
        public string ValorReintegroWithFormat
        {
            get
            {
                return ValorReintegro.moneyFormat(false);
            }
            set { }
        }
        public decimal ValorPagoConReintegro { get; set; }
        public string ValorPagoConReintegroWithFormat
        {
            get
            {
                return ValorPagoConReintegro.moneyFormat(false);
            }
            set { }
        }
        public decimal PresupuestoSinReintegro { get; set; }
        public string PresupuestoSinReintegroWithFormat
        {
            get
            {
                return PresupuestoSinReintegro.moneyFormat(false);
            }
            set { }
        }
        public decimal PresupuestoConReintegro { get; set; }
        public string PresupuestoConReintegroWithFormat
        {
            get
            {
                return PresupuestoConReintegro.moneyFormat(false);
            }
            set { }
        }
        public DateTime FechaReintegro { get; set; }
        public string FechaReintegroWithFormat
        {
            get
            {
                return FechaReintegro.getFechaAbreviadaConFormato(false);
            }
            set
            {
            }
        }
        public int CodigoContacto { get; set; }
        public String NombreContacto { get; set; }
        public String ArchivoInforme { get; set; }
    }
}
