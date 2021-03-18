using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public class IndicadorFormulacion
    {
        public static double GetIDH(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from proyectos in db.Proyecto
                              join ciudades in db.Ciudad on proyectos.CodCiudad equals ciudades.Id_Ciudad
                              where proyectos.Id_Proyecto == codigoProyecto
                              select ciudades.IDH).SingleOrDefault();

                return entity != null ? entity.GetValueOrDefault() : 0;
            }
        }

        public static double GetNBI(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from proyectos in db.Proyecto
                              join ciudades in db.Ciudad on proyectos.CodCiudad equals ciudades.Id_Ciudad
                              where proyectos.Id_Proyecto == codigoProyecto
                              select ciudades.NBI).SingleOrDefault();

                return entity != null ? entity.GetValueOrDefault() : 0;
            }
        }

        public static int GetCargos(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return (from datos in db.ProyectoGastosPersonals
                                where datos.CodProyecto == codigoProyecto
                                select datos).Count();                
            }
        }

        public static int GetPresupuesto(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from finanzas in db.ProyectoFinanzasIngresos
                              where finanzas.CodProyecto == codigoProyecto
                              select finanzas).SingleOrDefault();

                return entity != null ? (int)entity.Recursos : 0;
            }
        }

        public static int GetMercadeo(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from estrategias in db.ProyectoEstrategiaActividades
                              where estrategias.IdProyecto == codigoProyecto
                              select estrategias).Count();

                return entity;
            }
        }

        public static int GetContrapartidas(int codigoProyecto) {
            var contrapartidas = Math.Round(((GetPresupuesto(codigoProyecto) * 1.4) / 100) / 0.25,0);

            return (int)contrapartidas;
        }

        public static  decimal GetVentas(int codigoProyecto) {
            var ventas = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetIngresosPorVentas(codigoProyecto);
            var ventasPrimerPeriodo = ventas.Last();

            return ventasPrimerPeriodo.Year1;
        }

        public static List<ProduccionDTO> GetProductos(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var productos = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(codigoProyecto);
                var entResumen = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.Get(codigoProyecto);
                var codigoProductoSeleccionado = entResumen != null ? entResumen.ProductoMasRepresentativo.GetValueOrDefault(0) : 0;

                var produccion = productos.Select(filter => new ProduccionDTO
                {
                    IdProducto = filter.Id_Producto,
                    NombreProducto = filter.NomProducto,
                    Unidades = (int)db.ProyectoProductoUnidadesVentas
                                            .Where(filter2 => filter2.CodProducto.Equals(filter.Id_Producto))
                                            .Sum(sumatory => sumatory.Unidades),
                    unidadDeMedida = filter.UnidadMedida,
                    Selected = filter.Id_Producto == codigoProductoSeleccionado
                }).ToList();

                return produccion;
            }
        }
    }

    public class ProduccionDTO {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Unidades { get; set; }
        public string unidadDeMedida { get; set; }
        public bool Selected { get; set; }
        public bool SelectedEvaluacion { get; set; }
        public int? UnidadesEvaluador {
            get {
                return ProductoEvaluacion != null ? ProductoEvaluacion.Unidades : (int?)0;
            } set { } }
        public Datos.IndicadorProductoEvaluacion ProductoEvaluacion { get; set; }
    }
}