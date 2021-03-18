using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Datos.DataType;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.Solucion
{
    public static class Producto
    {
        public static List<ProyectoProducto> GetProductosByProyecto(int codigoProyecto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return db.ProyectoProductos.Where(filter => filter.CodProyecto.Equals(codigoProyecto)).ToList();
                }
                else
                {

                    return new List<ProyectoProducto>();
                }
            }
        }

        public static List<ProyectoProducto> GetProductosByProyecto(int codigoProyecto, int startIndex, int maxRows)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return db.ProyectoProductos.Where(filter => filter.CodProyecto.Equals(codigoProyecto)).Skip(startIndex).Take(maxRows).ToList();
                }
                else
                {

                    return new List<ProyectoProducto>();
                }
            }
        }

        public static List<ProductoProceso> GetProductosProceso(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                if (!codigoProyecto.Equals(0))
                {
                    return (db.ProyectoProductos.Where(filter => filter.CodProyecto.Equals(codigoProyecto)).OrderBy(y => y.NomProducto)
                        .Select(reg => new ProductoProceso()
                        {
                            Id_Proceso = reg.ProyectoDetalleProcesos.Where(x => x.IdProducto == reg.Id_Producto).SingleOrDefault().IdDetalleProceso,
                            Id_Producto = reg.Id_Producto,
                            NomProducto = reg.NomProducto,
                            DescProceso = reg.ProyectoDetalleProcesos.Where(x => x.IdProducto == reg.Id_Producto).SingleOrDefault().DescripcionProceso,
                            Unidad = reg.UnidadMedida
                        }

                        )).ToList();
                }
                else
                {

                    return new List<ProductoProceso>();
                }
            }
        }

        public static ProyectoProducto GetProductoById(int codigoProducto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoProductos
                         .Where(filter => filter.Id_Producto.Equals(codigoProducto))
                         .SingleOrDefault();             
            }
        }

        /// <summary>
        /// Verifica si el producto existe
        /// </summary>
        /// <param name="codigoProducto"> Codigo del producto</param>
        /// <returns>Verifica si el producto existe</returns>
        public static Boolean ProductoExist(int codigoProducto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoProductos.Any(filter => filter.Id_Producto.Equals(codigoProducto));
            }
        }

        /// <summary>
        /// Valida que no exista un producto con la misma unidad de medida al insertar un nuevo producto
        /// </summary>
        public static Boolean ExistProductoByName(string nombreProducto, string unidadMedida, int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoProductos.Any(filter => filter.CodProyecto.Equals(codigoProyecto)
                                                          && filter.NomProducto.ToLower().Trim().Equals(nombreProducto.ToLower().Trim())
                                                          && filter.UnidadMedida.ToLower().Trim().Equals(unidadMedida.ToLower().Trim())
                                                );
            }
        }

        /// <summary>
        /// Valida que no exista un producto con la misma unidad de medida diferente al producto actual
        /// </summary>
        public static Boolean ExistProductoById(string nombreProducto, string unidadMedida, int codigoProducto, int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoProductos.Any(filter => !filter.Id_Producto.Equals(codigoProducto) 
                                                          && filter.CodProyecto.Equals(codigoProyecto)
                                                          && filter.NomProducto.ToLower().Trim().Equals(nombreProducto.ToLower().Trim())
                                                          && filter.UnidadMedida.ToLower().Trim().Equals(unidadMedida.ToLower().Trim())
                                                );
            }
        }

        public static int CountProductos(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoProductos
                         .Count(filter => filter.CodProyecto.Equals(codigoProyecto));
            }
        }
        
        public static void Insert(ProyectoProducto producto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ProyectoProductos.InsertOnSubmit(producto);
                db.SubmitChanges();
            }
        }

        public static void Update(ProyectoProducto producto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentProducto = db.ProyectoProductos.Single(filter => filter.Id_Producto.Equals(producto.Id_Producto));

                currentProducto.NomProducto = producto.NomProducto;
                currentProducto.NombreComercial = producto.NombreComercial;
                currentProducto.UnidadMedida = producto.UnidadMedida;
                currentProducto.DescripcionGeneral = producto.DescripcionGeneral;
                currentProducto.CondicionesEspeciales = producto.CondicionesEspeciales;
                currentProducto.Composicion = producto.Composicion;
                currentProducto.Otros = producto.Otros;
                currentProducto.Justificacion = producto.Justificacion;
                currentProducto.FormaDePago = producto.FormaDePago;
                currentProducto.Iva = producto.Iva;
                currentProducto.PorcentajeIva = producto.PorcentajeIva;

                db.SubmitChanges();
            }
        }

        public static void Delete(int codigoProducto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentProducto = db.ProyectoProductos.Single(filter => filter.Id_Producto.Equals(codigoProducto));

                DeleteDependencies(codigoProducto);

                db.ProyectoProductos.DeleteOnSubmit(currentProducto);
                db.SubmitChanges();
            }
        }

        public static void DeleteDependencies(int codigoProducto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var cantidadesVentas = db.ProyectoProductoUnidadesVentas.Where(filter => filter.CodProducto.Equals(codigoProducto)).ToList();
                var precioVentas =  db.ProyectoProductoPrecios.Where(filter => filter.CodProducto.Equals(codigoProducto)).ToList();
                var insumos = db.ProyectoProductoInsumos.Where(filter => filter.CodProducto.Equals(codigoProducto)).ToList();
                var procesoProducto = db.ProyectoDetalleProcesos.Where(filter => filter.IdProducto.Equals(codigoProducto)).ToList();

                var indicadorEvaluacion = db.IndicadorProductoEvaluacions.Where(filter => filter.IdProducto.Equals(codigoProducto)).ToList();
                db.IndicadorProductoEvaluacions.DeleteAllOnSubmit(indicadorEvaluacion);

                db.ProyectoProductoUnidadesVentas.DeleteAllOnSubmit(cantidadesVentas);
                db.ProyectoProductoPrecios.DeleteAllOnSubmit(precioVentas);
                db.ProyectoProductoInsumos.DeleteAllOnSubmit(insumos);
                db.ProyectoDetalleProcesos.DeleteAllOnSubmit(procesoProducto);
                db.SubmitChanges();
            }
        }

        public static Boolean VerificarProyeccionesCompletas(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {                
                return !db.ProyectoProductos.Any(filter => filter.CodProyecto.Equals(codigoProyecto) && filter.FormaDePago == null); 
            }
        }

    }
}
