using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Utilidad
{
    public class IndicadorEvaluacion
    {
        public static Datos.IndicadorGestionEvaluacion GetIndicadores(int codigoProyecto, int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entity = (from indicadores in db.IndicadorGestionEvaluacions                              
                              where indicadores.IdProyecto == codigoProyecto
                                    && indicadores.IdConvocatoria == codigoConvocatoria
                              select indicadores).FirstOrDefault();

                return entity;
            }
        }

        public static void InsertOrUpdate(Datos.IndicadorGestionEvaluacion entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = (from indicadores in db.IndicadorGestionEvaluacions
                                     where indicadores.IdProyecto == entity.IdProyecto
                                     && indicadores.IdConvocatoria == entity.IdConvocatoria
                                     select indicadores
                                     ).FirstOrDefault();
                if (currentEntity != null) {
                    currentEntity.PeriodoImproductivo = entity.PeriodoImproductivo;
                    currentEntity.RecursosAportadosEmprendedor = entity.RecursosAportadosEmprendedor;
                    currentEntity.Ventas = entity.Ventas;
                    currentEntity.ProductoMasRepresentativo = entity.ProductoMasRepresentativo;
                }
                else
                {
                    db.IndicadorGestionEvaluacions.InsertOnSubmit(entity);
                }

                db.SubmitChanges();
            }
        }

        public static void InsertOrUpdateProduccion(Datos.IndicadorProductoEvaluacion entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = (from indicadores in db.IndicadorProductoEvaluacions
                                     where indicadores.IdProyecto == entity.IdProyecto
                                     && indicadores.IdConvocatoria == entity.IdConvocatoria
                                     && indicadores.IdProducto == entity.IdProducto
                                     select indicadores
                                     ).FirstOrDefault();
                if (currentEntity != null)
                {
                    currentEntity.Unidades = entity.Unidades;
                }
                else
                {
                    db.IndicadorProductoEvaluacions.InsertOnSubmit(entity);
                }

                db.SubmitChanges();
            }
        }

        public static void InsertOrUpdateMercadeo(Datos.IndicadorMercadeoEvaluacion entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = (from indicadores in db.IndicadorMercadeoEvaluacions
                                     where indicadores.IdProyecto == entity.IdProyecto
                                     && indicadores.IdConvocatoria == entity.IdConvocatoria
                                     && indicadores.IdActividadMercadeo == entity.IdActividadMercadeo
                                     select indicadores
                                     ).FirstOrDefault();
                if (currentEntity != null)
                {
                    currentEntity.Unidades = entity.Unidades;
                }
                else
                {
                    db.IndicadorMercadeoEvaluacions.InsertOnSubmit(entity);
                }

                db.SubmitChanges();
            }
        }

        public static void InsertOrUpdateCargo(Datos.IndicadorCargoEvaluacion entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = (from indicadores in db.IndicadorCargoEvaluacions
                                     where indicadores.IdProyecto == entity.IdProyecto
                                     && indicadores.IdConvocatoria == entity.IdConvocatoria
                                     && indicadores.IdCargo == entity.IdCargo
                                     select indicadores
                                     ).FirstOrDefault();
                if (currentEntity != null)
                {
                    currentEntity.Unidades = entity.Unidades;
                }
                else
                {
                    db.IndicadorCargoEvaluacions.InsertOnSubmit(entity);
                }

                db.SubmitChanges();
            }
        }

        public static List<ProduccionDTO> GetProductos(int codigoProyecto,int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var productos = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(codigoProyecto);
                var entResumen = Negocio.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.Resumen.Get(codigoProyecto);
                var codigoProductoSeleccionado = entResumen != null ? entResumen.ProductoMasRepresentativo.GetValueOrDefault(0) : 0;
                var indicadoresEvaluacion = db.IndicadorGestionEvaluacions.FirstOrDefault(filter => filter.IdProyecto == codigoProyecto && filter.IdConvocatoria == codigoConvocatoria);
                var codigoProductoSeleccionadoEvaluacion = 0;

                if (indicadoresEvaluacion != null)
                {
                    if (indicadoresEvaluacion.ProductoMasRepresentativo != null)
                        codigoProductoSeleccionadoEvaluacion = indicadoresEvaluacion.ProductoMasRepresentativo.GetValueOrDefault(0);
                }
                
                var produccion = productos.Select(filter => new ProduccionDTO
                {
                    IdProducto = filter.Id_Producto,
                    NombreProducto = filter.NomProducto,
                    Unidades = (int)db.ProyectoProductoUnidadesVentas
                                            .Where(filter2 => filter2.CodProducto.Equals(filter.Id_Producto))
                                            .Sum(sumatory => sumatory.Unidades),
                    unidadDeMedida = filter.UnidadMedida,
                    Selected = filter.Id_Producto == codigoProductoSeleccionado,
                    SelectedEvaluacion = filter.Id_Producto == codigoProductoSeleccionadoEvaluacion,
                    ProductoEvaluacion = db.IndicadorProductoEvaluacions
                                          .FirstOrDefault(filter2 => filter2.IdProducto == filter.Id_Producto 
                                                                     && filter2.IdConvocatoria == codigoConvocatoria
                                                                     && filter2.IdProyecto == codigoProyecto)
                }).ToList();

                return produccion;
            }
        }

        public static int CountProductos(int codigoProyecto, int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from productos in db.IndicadorProductoEvaluacions
                                where productos.IdProyecto == codigoProyecto
                                      && productos.IdConvocatoria == codigoConvocatoria
                                select productos.Unidades
                               ).Sum();

                return entities;
            }
        }

        public static int CountProductoRepresentativo(int codigoProyecto, int codigoConvocatoria)
        {

            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from productos in db.ActaSeguimGestionProduccionEvaluacion
                                where productos.codProyecto == codigoProyecto && productos.ocultar == false
                                && productos.productoRepresentativo == true
                                select productos.unidades
                               ).Sum();

                return entities;
            }

            //    using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            //    {
            //        var entities = (from productos in db.IndicadorProductoEvaluacions
            //                        join v in db.IndicadorGestionEvaluacions
            //                        on productos.IdProducto equals v.ProductoMasRepresentativo
            //                        where productos.IdProyecto == codigoProyecto
            //                              && productos.IdConvocatoria == codigoConvocatoria                                      
            //                        select productos.Unidades
            //                       ).Sum();

            //        return entities;
            //    }
        }

        public static List<CargoDTO> GetCargos(int codigoProyecto,int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from datos in db.ProyectoGastosPersonals
                        where datos.CodProyecto == codigoProyecto
                        select datos).ToList();

                return entities.Select(filter => new CargoDTO {
                    IdCargo = filter.Id_Cargo,
                    Nombre  = filter.Cargo,
                    CargoEvaluacion  = db.IndicadorCargoEvaluacions
                                          .FirstOrDefault(filter2 => filter2.IdCargo == filter.Id_Cargo
                                                                     && filter2.IdConvocatoria == codigoConvocatoria
                                                                     && filter2.IdProyecto == codigoProyecto)
                }).ToList();                
            }
        }

        public static int CountCargos(int codigoProyecto, int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from cargos in db.IndicadorCargoEvaluacions
                                where cargos.IdProyecto == codigoProyecto
                                      && cargos.IdConvocatoria == codigoConvocatoria
                                select cargos.Unidades
                               ).Sum();

                return entities;
            }
        }
       
        public static List<MercadeoDTO> GetMercadeo(int codigoProyecto, int codigoConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from datos in db.ProyectoEstrategiaActividades
                                where datos.IdProyecto == codigoProyecto
                                select datos).ToList();

                return entities.Select(filter => new MercadeoDTO
                {
                    IdActividad = filter.IdActividad,
                    Nombre = filter.Actividad,
                    MercadeoEvaluacion = db.IndicadorMercadeoEvaluacions
                                          .FirstOrDefault(filter2 => filter2.IdActividadMercadeo == filter.IdActividad
                                                                     && filter2.IdConvocatoria == codigoConvocatoria
                                                                     && filter2.IdProyecto == codigoProyecto)
                }).ToList();
            }
        }

        public static int CountMercadeo(int codigoProyecto, int codigoConvocatoria)
        {
            using (Datos.FonadeDBLightDataContext db = new Datos.FonadeDBLightDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entities = (from mercadeo in db.ActaSeguimGestionMercadeoEvaluacion
                                where mercadeo.codProyecto == codigoProyecto && mercadeo.ocultar == false
                                select mercadeo.unidades
                               ).Sum();

                return entities;
            }
        }
    }
    public class CargoDTO
    {
        public int IdCargo { get; set; }
        public string Nombre { get; set; }                
        public int? UnidadesEvaluador
        {
            get
            {
                return CargoEvaluacion != null ? CargoEvaluacion.Unidades : (int?)null;
            }
            set { }
        }
        public Datos.IndicadorCargoEvaluacion CargoEvaluacion { get; set; }
    }

    public class MercadeoDTO
    {
        public int IdActividad { get; set; }
        public string Nombre { get; set; }
        public int? UnidadesEvaluador
        {
            get
            {
                return MercadeoEvaluacion != null ? MercadeoEvaluacion.Unidades : (int?)null;
            }
            set { }
        }
        public Datos.IndicadorMercadeoEvaluacion MercadeoEvaluacion { get; set; }
    }
}
