using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public class AvancesInterventoriaReg
    {
        static string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public static List<HistoricoAvanceModel> GetHistoricoAvancesPlanOperativo(int _codActividad, int _Mes)
        {
            List<HistoricoAvanceModel> historicos = new List<HistoricoAvanceModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                historicos = (from h in db.HistorialAvancesPlanOperativo
                              where h.codActividad == _codActividad && h.Mes == _Mes
                              orderby h.fechaRegistro
                              select new HistoricoAvanceModel
                              {
                                  Aprobada = h.Aprobada,
                                  codActividad = h.codActividad,
                                  codContacto = h.codContacto,
                                  codProyecto = h.codProyecto,
                                  FechaAvanceEmprendedor = h.FechaAvanceEmprendedor,
                                  FechaAvanceInterventor = h.FechaAvanceInterventor,
                                  fechaRegistro = h.fechaRegistro,
                                  idHistorico = h.idHistoricoAvance,
                                  Mes = h.Mes,
                                  ObservacionEmprendedor = h.ObservacionEmprendedor,
                                  ObservacionInterventor = h.ObservacionInterventor,
                                  ValorAporteEmprendedor = h.ValorAporteEmprendedor,
                                  ValorFondoEmprender = h.ValorFondoEmprender,
                                  nombres = nombreContacto(h.codContacto)
                              }).ToList();


            }

            return historicos;
        }

        public static List<HistoricoAvanceModel> GetHistoricoAvancesNomina(int _codActividad, int _Mes)
        {
            List<HistoricoAvanceModel> historicos = new List<HistoricoAvanceModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                historicos = (from h in db.HistorialAvancesNomina
                              where h.codNomina == _codActividad && h.Mes == _Mes
                              orderby h.fechaRegistro
                              select new HistoricoAvanceModel
                              {
                                  Aprobada = h.Aprobada,
                                  codActividad = h.codNomina,
                                  codContacto = h.codContacto,
                                  codProyecto = h.codProyecto,
                                  FechaAvanceEmprendedor = h.FechaAvanceEmprendedor,
                                  FechaAvanceInterventor = h.FechaAvanceInterventor,
                                  fechaRegistro = h.fechaRegistro,
                                  idHistorico = h.idHistoricoAvaNomina,
                                  Mes = h.Mes,
                                  ObservacionEmprendedor = h.ObservacionEmprendedor,
                                  ObservacionInterventor = h.ObservacionInterventor,
                                  ValorPrestaciones = h.valorPrestaciones,
                                  ValorSueldo = h.valorSueldo,
                                  nombres = nombreContacto(h.codContacto)
                              }).ToList();


            }

            return historicos;
        }

        public static List<HistoricoAvanceModel> GetHistoricoAvancesProduccion(int _codActividad, int _Mes)
        {
            List<HistoricoAvanceModel> historicos = new List<HistoricoAvanceModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                historicos = (from h in db.HistorialAvancesProduccion
                              where h.CodProduccion == _codActividad && h.Mes == _Mes
                              orderby h.fechaRegistro
                              select new HistoricoAvanceModel
                              {
                                  Aprobada = h.Aprobada,
                                  codActividad = h.CodProduccion,
                                  codContacto = h.codContacto,
                                  codProyecto = h.codProyecto,
                                  FechaAvanceEmprendedor = h.FechaAvanceEmprendedor,
                                  FechaAvanceInterventor = h.FechaAvanceInterventor,
                                  fechaRegistro = h.fechaRegistro,
                                  idHistorico = h.idHistorialAvaProduccion,
                                  Mes = h.Mes,
                                  ObservacionEmprendedor = h.ObservacionEmprendedor,
                                  ObservacionInterventor = h.ObservacionInterventor,
                                  Costo = h.Costo,
                                  Cantidad = h.Cantidad,
                                  nombres = nombreContacto(h.codContacto)
                              }).ToList();


            }

            return historicos;
        }

        public static List<HistoricoAvanceModel> GetHistoricoAvancesVentas(int _codActividad, int _Mes)
        {
            List<HistoricoAvanceModel> historicos = new List<HistoricoAvanceModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                historicos = (from h in db.HistorialAvancesVentas
                              where h.CodVenta == _codActividad && h.Mes == _Mes
                              orderby h.fechaRegistro
                              select new HistoricoAvanceModel
                              {
                                  Aprobada = h.Aprobada,
                                  codActividad = h.CodVenta,
                                  codContacto = h.codContacto,
                                  codProyecto = h.codProyecto,
                                  FechaAvanceEmprendedor = h.FechaAvanceEmprendedor,
                                  FechaAvanceInterventor = h.FechaAvanceInterventor,
                                  fechaRegistro = h.fechaRegistro,
                                  idHistorico = h.idHistoricoAvaVentas,
                                  Mes = h.Mes,
                                  ObservacionEmprendedor = h.ObservacionEmprendedor,
                                  ObservacionInterventor = h.ObservacionInterventor,
                                  Ventas = h.Ventas,
                                  Ingreso = h.Ingreso,
                                  nombres = nombreContacto(h.codContacto)
                              }).ToList();


            }

            return historicos;
        }

        private static string nombreContacto(int _codContacto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from c in db.Contacto
                          where c.Id_Contacto == _codContacto
                          select c.Nombres + " " + c.Apellidos).FirstOrDefault();
            }

                return nombre;
        }

        public static bool insertarHistoricoPlanOperativo(HistoricoAvanceModel histAvance)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                HistorialAvancesPlanOperativo historials = new HistorialAvancesPlanOperativo
                { 
                    Aprobada = histAvance.Aprobada,
                    codActividad = histAvance.codActividad,
                    codContacto = histAvance.codContacto,
                    FechaAvanceEmprendedor = histAvance.FechaAvanceEmprendedor,
                    FechaAvanceInterventor = histAvance.FechaAvanceInterventor,
                    fechaRegistro = DateTime.Now,
                    Mes = histAvance.Mes,
                    ObservacionEmprendedor = histAvance.ObservacionEmprendedor,
                    ObservacionInterventor = histAvance.ObservacionInterventor,
                    ValorAporteEmprendedor = histAvance.ValorAporteEmprendedor,
                    ValorFondoEmprender = histAvance.ValorFondoEmprender,
                    codProyecto = histAvance.codProyecto
                };

                db.HistorialAvancesPlanOperativo.InsertOnSubmit(historials);
                db.SubmitChanges();
                insertado = true;
            }

            return insertado;
        }

        public static bool insertarHistoricoNomina(HistoricoAvanceModel histAvance)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                HistorialAvancesNomina historials = new HistorialAvancesNomina
                {
                    Aprobada = histAvance.Aprobada,
                    codNomina = histAvance.codActividad,
                    codContacto = histAvance.codContacto,
                    FechaAvanceEmprendedor = histAvance.FechaAvanceEmprendedor,
                    FechaAvanceInterventor = histAvance.FechaAvanceInterventor,
                    fechaRegistro = DateTime.Now,
                    Mes = histAvance.Mes,
                    ObservacionEmprendedor = histAvance.ObservacionEmprendedor,
                    ObservacionInterventor = histAvance.ObservacionInterventor,
                    valorPrestaciones = histAvance.ValorPrestaciones,
                    valorSueldo = histAvance.ValorSueldo,
                    codProyecto = histAvance.codProyecto
                };

                db.HistorialAvancesNomina.InsertOnSubmit(historials);
                db.SubmitChanges();
                insertado = true;
            }

            return insertado;
        }

        public static bool insertarHistoricoProduccion(HistoricoAvanceModel histAvance)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                HistorialAvancesProduccion historials = new HistorialAvancesProduccion
                {
                    Aprobada = histAvance.Aprobada,
                    CodProduccion = histAvance.codActividad,
                    codContacto = histAvance.codContacto,
                    FechaAvanceEmprendedor = histAvance.FechaAvanceEmprendedor,
                    FechaAvanceInterventor = histAvance.FechaAvanceInterventor,
                    fechaRegistro = DateTime.Now,
                    Mes = histAvance.Mes,
                    ObservacionEmprendedor = histAvance.ObservacionEmprendedor,
                    ObservacionInterventor = histAvance.ObservacionInterventor,
                    Costo = histAvance.Costo,
                    Cantidad = histAvance.Cantidad,
                    codProyecto = histAvance.codProyecto
                };

                db.HistorialAvancesProduccion.InsertOnSubmit(historials);
                db.SubmitChanges();
                insertado = true;
            }

            return insertado;
        }

        public static bool insertarHistoricoVentas(HistoricoAvanceModel histAvance)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                HistorialAvancesVentas historials = new HistorialAvancesVentas
                {
                    Aprobada = histAvance.Aprobada,
                    CodVenta = histAvance.codActividad,
                    codContacto = histAvance.codContacto,
                    FechaAvanceEmprendedor = histAvance.FechaAvanceEmprendedor,
                    FechaAvanceInterventor = histAvance.FechaAvanceInterventor,
                    fechaRegistro = DateTime.Now,
                    Mes = histAvance.Mes,
                    ObservacionEmprendedor = histAvance.ObservacionEmprendedor,
                    ObservacionInterventor = histAvance.ObservacionInterventor,
                    Ventas = histAvance.Ventas,
                    Ingreso = histAvance.Ingreso,
                    codProyecto = histAvance.codProyecto
                };

                db.HistorialAvancesVentas.InsertOnSubmit(historials);
                db.SubmitChanges();
                insertado = true;
            }

            return insertado;
        }
    }

    public class HistoricoAvanceModel
    {
        public long idHistorico { get; set; }
        public int codActividad { get; set; }
        public int Mes { get; set; }
        public DateTime? FechaAvanceEmprendedor { get; set; }
        public string ObservacionEmprendedor { get; set; }
        public DateTime? FechaAvanceInterventor { get; set; }
        public string ObservacionInterventor { get; set; }
        public bool? Aprobada { get; set; }
        public string avanceAprobado { 
            get {
                if (Aprobada ?? false) return "SI"; else return "NO";
            } 
        }
        public decimal? ValorFondoEmprender { get; set; }
        public decimal? ValorAporteEmprendedor { get; set; }
        public decimal? ValorSueldo { get; set; }
        public decimal? ValorPrestaciones { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? Costo { get; set; }
        public decimal? Ventas { get; set; }
        public decimal? Ingreso { get; set; }
        public int codContacto { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int? codProyecto { get; set; }
        public string nombres { get; set; }
    }
}
