using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Fonade.Negocio.Utility;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    public static class Proyeccion
    {
        public static ProyectoMercadoProyeccionVenta GetTiempoProyeccion(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.ProyectoMercadoProyeccionVentas
                         .Where(filter => filter.CodProyecto.Equals(codigoProyecto))
                         .SingleOrDefault();
            }
        }

        public static void Insert(ProyectoMercadoProyeccionVenta proyeccion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ProyectoMercadoProyeccionVentas.InsertOnSubmit(proyeccion);
                db.SubmitChanges();
            }
        }
         
        public static void Update(ProyectoMercadoProyeccionVenta proyeccion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentProducto = db.ProyectoMercadoProyeccionVentas.Single(filter => filter.Id_ProyectoProyeccionVentas.Equals(proyeccion.Id_ProyectoProyeccionVentas));

                currentProducto.TiempoProyeccion = proyeccion.TiempoProyeccion;

                db.SubmitChanges();
            }
        }

        public static List<ProyeccionDeVentasPorMes> GetVentasPorMes(int codigoProducto) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var ProyeccionesPorMes = new List<ProyeccionDeVentasPorMes>();                                    
                
                var monthCounter = 1;
                for (int i = 0; i <= 11; i++)
                {
                    var proyecciones = new ProyeccionDeVentasPorMes();
                    proyecciones.Mes = monthCounter;

                    var yearCounter = 1;
                    for (int j = 0; j <= 9; j++)
                    {
                        var proyeccion = db.ProyectoProductoUnidadesVentas
                                           .FirstOrDefault(
                                                filter =>
                                                    filter.CodProducto.Equals(codigoProducto)
                                                    && filter.Ano.Equals(yearCounter)
                                                    && filter.Mes.Equals(monthCounter)
                                            );

                        if (proyeccion == null) {
                            proyeccion = new ProyectoProductoUnidadesVenta()
                            {
                                CodProducto = codigoProducto,
                                Mes = (Int16)monthCounter,
                                Ano = (Int16)yearCounter,
                                Unidades = 0
                            };

                            InsertProyeccionMes(proyeccion);
                        }

                        switch (yearCounter)
                        {
                            case 1:
                                proyecciones.Year1 = proyeccion;
                                break;
                            case 2:
                                proyecciones.Year2 = proyeccion;
                                break;
                            case 3:
                                proyecciones.Year3 = proyeccion;
                                break;
                            case 4:
                                proyecciones.Year4 = proyeccion;
                                break;
                            case 5:
                                proyecciones.Year5 = proyeccion;
                                break;
                            case 6:
                                proyecciones.Year6 = proyeccion;
                                break;
                            case 7:
                                proyecciones.Year7 = proyeccion;
                                break;
                            case 8:
                                proyecciones.Year8 = proyeccion;
                                break;
                            case 9:
                                proyecciones.Year9 = proyeccion;
                                break;
                            case 10:
                                proyecciones.Year10 = proyeccion;
                                break;
                        }

                        yearCounter += 1;
                    }

                    ProyeccionesPorMes.Add(proyecciones);
                    monthCounter += 1;
                }
                //Precios
                var totalPrecioProducto = new ProyeccionDeVentasPorMes();
                totalPrecioProducto.Mes = 13;

                var precioProducto1 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(1));
                if (precioProducto1 == null)
                {
                    precioProducto1 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 1,                        
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto1);                    
                }
                totalPrecioProducto.Year1 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 1,
                    Unidades = (double)precioProducto1.Valor
                };

                var precioProducto2 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(2));
                if (precioProducto2 == null)
                {
                    precioProducto2 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 2,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto2);
                }
                totalPrecioProducto.Year2 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 2,
                    Unidades = (double)precioProducto2.Valor
                };

                var precioProducto3 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(3));
                if (precioProducto3 == null)
                {
                    precioProducto3 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 3,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto3);
                }
                totalPrecioProducto.Year3 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 3,
                    Unidades = (double)precioProducto3.Valor
                };

                var precioProducto4 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(4));
                if (precioProducto4 == null)
                {
                    precioProducto4 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 4,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto4);
                }
                totalPrecioProducto.Year4 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 4,
                    Unidades = (double)precioProducto4.Valor
                };

                var precioProducto5 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(5));
                if (precioProducto5 == null)
                {
                    precioProducto5 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 5,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto5);
                }
                totalPrecioProducto.Year5 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 5,
                    Unidades = (double)precioProducto5.Valor
                };

                var precioProducto6 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(6));
                if (precioProducto6 == null)
                {
                    precioProducto6 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 6,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto6);
                }
                totalPrecioProducto.Year6 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 6,
                    Unidades = (double)precioProducto6.Valor
                };

                var precioProducto7 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(7));
                if (precioProducto7 == null)
                {
                    precioProducto7 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 7,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto7);
                }
                totalPrecioProducto.Year7 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 7,
                    Unidades = (double)precioProducto7.Valor
                };

                var precioProducto8 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(8));
                if (precioProducto8 == null)
                {
                    precioProducto8 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 8,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto8);
                }
                totalPrecioProducto.Year8 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 8,
                    Unidades = (double)precioProducto8.Valor
                };

                var precioProducto9 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(9));
                if (precioProducto9 == null)
                {
                    precioProducto9 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 9,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto9);
                }
                totalPrecioProducto.Year9 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 9,
                    Unidades = (double)precioProducto9.Valor
                };

                var precioProducto10 = db.ProyectoProductoPrecios.FirstOrDefault(filter => filter.CodProducto.Equals(codigoProducto) && filter.Periodo.Equals(10));
                if (precioProducto10 == null)
                {
                    precioProducto10 = new ProyectoProductoPrecio()
                    {
                        CodProducto = codigoProducto,
                        Periodo = 10,
                        Valor = 0
                    };

                    InsertProductoPrecio(precioProducto10);
                }
                totalPrecioProducto.Year10 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 13,
                    Ano = 10,
                    Unidades = (double)precioProducto10.Valor
                };

                

                //Totales
                var totalProyeccion = new ProyeccionDeVentasPorMes();

                totalProyeccion.Mes = 14;
                totalProyeccion.Year1 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 1,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year1.Unidades) * totalPrecioProducto.Year1.Unidades
                };
                totalProyeccion.Year2 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 2,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year2.Unidades) * totalPrecioProducto.Year2.Unidades
                };
                totalProyeccion.Year3 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 3,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year3.Unidades) * totalPrecioProducto.Year3.Unidades
                };
                totalProyeccion.Year4 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 4,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year4.Unidades) * totalPrecioProducto.Year4.Unidades
                };
                totalProyeccion.Year5 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 5,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year5.Unidades) * totalPrecioProducto.Year5.Unidades
                };
                totalProyeccion.Year6 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 6,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year6.Unidades) * totalPrecioProducto.Year6.Unidades
                };
                totalProyeccion.Year7 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 7,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year7.Unidades) * totalPrecioProducto.Year7.Unidades
                };
                totalProyeccion.Year8 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 8,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year8.Unidades) * totalPrecioProducto.Year8.Unidades
                };
                totalProyeccion.Year9 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 9,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year9.Unidades) * totalPrecioProducto.Year9.Unidades
                };
                totalProyeccion.Year10 = new ProyectoProductoUnidadesVenta()
                {
                    CodProducto = codigoProducto,
                    Mes = 14,
                    Ano = 10,
                    Unidades = ProyeccionesPorMes.Sum(operation => operation.Year10.Unidades) * totalPrecioProducto.Year10.Unidades
                };

                ProyeccionesPorMes.Add(totalPrecioProducto);
                ProyeccionesPorMes.Add(totalProyeccion);
                                          
                return ProyeccionesPorMes;
            }
        }

        public static void InsertProyeccionMes(ProyectoProductoUnidadesVenta proyeccion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ProyectoProductoUnidadesVentas.InsertOnSubmit(proyeccion);
                db.SubmitChanges();
            }
        }

        public static void UpdateProyeccionMes(ProyectoProductoUnidadesVenta proyeccion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentProducto = db.ProyectoProductoUnidadesVentas
                                        .Single(
                                                filter => filter.Ano.Equals(proyeccion.Ano) 
                                                          && filter.Mes.Equals(proyeccion.Mes)
                                                          && filter.CodProducto.Equals(proyeccion.CodProducto));

                currentProducto.Unidades = proyeccion.Unidades;                

                db.SubmitChanges();
            }
        }

        public static void InsertProductoPrecio(ProyectoProductoPrecio entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                db.ProyectoProductoPrecios.InsertOnSubmit(entity);
                db.SubmitChanges();
            }
        }

        public static void UpdateProductoPrecio(ProyectoProductoPrecio entity)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentProducto = db.ProyectoProductoPrecios
                                       .Single(
                                               filter => filter.Periodo.Equals(entity.Periodo)                                                         
                                                         && filter.CodProducto.Equals(entity.CodProducto));
                currentProducto.Valor = entity.Valor;
                currentProducto.Precio = entity.Valor.ToString().Replace(",",string.Empty);

                db.SubmitChanges();
            }
        }

        public static List<IngresosPorVentas> GetIngresosPorVentas(int codigoProyecto ) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                int tiempo = 3;
                var tiempoProyeccion = Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion.GetTiempoProyeccion(codigoProyecto);
                if (tiempoProyeccion != null)
                    tiempo = (int)tiempoProyeccion.TiempoProyeccion;

                var productos =  Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductosByProyecto(codigoProyecto);
                var ingresosPorProductos = new List<IngresosPorVentas>();
                foreach (var producto in productos)
                {                                      
                        var ingreso = IngresosPorVentasPorProducto(producto.Id_Producto, tiempo);

                    ingresosPorProductos.Add(ingreso);
                }

                var iva = new  IngresosPorVentas();

                iva.Tipo = 1;
                iva.Iva  = 0;
                iva.Year1 = ingresosPorProductos.Sum(sumatory => (sumatory.Year1 * sumatory.Iva/100));
                iva.Year2 = ingresosPorProductos.Sum(sumatory => (sumatory.Year2 * sumatory.Iva / 100));
                iva.Year3 = ingresosPorProductos.Sum(sumatory => (sumatory.Year3 * sumatory.Iva / 100));
                iva.Year4 = ingresosPorProductos.Sum(sumatory => (sumatory.Year4 * sumatory.Iva / 100));
                iva.Year5 = ingresosPorProductos.Sum(sumatory => (sumatory.Year5 * sumatory.Iva / 100));
                iva.Year6 = ingresosPorProductos.Sum(sumatory => (sumatory.Year6 * sumatory.Iva / 100));
                iva.Year7 = ingresosPorProductos.Sum(sumatory => (sumatory.Year7 * sumatory.Iva / 100));
                iva.Year8 = ingresosPorProductos.Sum(sumatory => (sumatory.Year8 * sumatory.Iva / 100));
                iva.Year9 = ingresosPorProductos.Sum(sumatory => (sumatory.Year9 * sumatory.Iva / 100));
                iva.Year10 = ingresosPorProductos.Sum(sumatory => (sumatory.Year10 * sumatory.Iva / 100));

                var total = new IngresosPorVentas();

                total.Tipo = 2;
                total.Iva = 0;
                total.Year1 = ingresosPorProductos.Sum(sumatory => (sumatory.Year1 ));
                total.Year2 = ingresosPorProductos.Sum(sumatory => (sumatory.Year2 ));
                total.Year3 = ingresosPorProductos.Sum(sumatory => (sumatory.Year3 ));
                total.Year4 = ingresosPorProductos.Sum(sumatory => (sumatory.Year4 ));
                total.Year5 = ingresosPorProductos.Sum(sumatory => (sumatory.Year5 ));
                total.Year6 = ingresosPorProductos.Sum(sumatory => (sumatory.Year6 ));
                total.Year7 = ingresosPorProductos.Sum(sumatory => (sumatory.Year7 ));
                total.Year8 = ingresosPorProductos.Sum(sumatory => (sumatory.Year8 ));
                total.Year9 = ingresosPorProductos.Sum(sumatory => (sumatory.Year9 ));
                total.Year10 = ingresosPorProductos.Sum(sumatory => (sumatory.Year10 ));

                var totalMasIva = new IngresosPorVentas();

                totalMasIva.Tipo = 0;
                totalMasIva.Iva = 0;
                totalMasIva.Year1 = ingresosPorProductos.Sum(sumatory => sumatory.Year1 + (sumatory.Year1 * sumatory.Iva / 100));
                totalMasIva.Year2 = ingresosPorProductos.Sum(sumatory => sumatory.Year2 +(sumatory.Year2 * sumatory.Iva / 100));
                totalMasIva.Year3 = ingresosPorProductos.Sum(sumatory => sumatory.Year3 +(sumatory.Year3 * sumatory.Iva / 100));
                totalMasIva.Year4 = ingresosPorProductos.Sum(sumatory => sumatory.Year4 +(sumatory.Year4 * sumatory.Iva / 100));
                totalMasIva.Year5 = ingresosPorProductos.Sum(sumatory => sumatory.Year5 +(sumatory.Year5 * sumatory.Iva / 100));
                totalMasIva.Year6 = ingresosPorProductos.Sum(sumatory => sumatory.Year6 +(sumatory.Year6 * sumatory.Iva / 100));
                totalMasIva.Year7 = ingresosPorProductos.Sum(sumatory => sumatory.Year7 +(sumatory.Year7 * sumatory.Iva / 100));
                totalMasIva.Year8 = ingresosPorProductos.Sum(sumatory => sumatory.Year8 +(sumatory.Year8 * sumatory.Iva / 100));
                totalMasIva.Year9 = ingresosPorProductos.Sum(sumatory => sumatory.Year9 +(sumatory.Year9 * sumatory.Iva / 100));
                totalMasIva.Year10 = ingresosPorProductos.Sum(sumatory => sumatory.Year10 +(sumatory.Year10 * sumatory.Iva / 100));

                ingresosPorProductos.Add(total);
                ingresosPorProductos.Add(iva);
                ingresosPorProductos.Add(totalMasIva);

                return ingresosPorProductos;
            }
        }

        public static IngresosPorVentas IngresosPorVentasPorProducto(int codigoProducto, int tiempoProyeccion) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var ingreso = new IngresosPorVentas();

                for (int i = 1; i <= 10; i++)
                {                   
                    switch (i)
                    {
                        case 1:
                            if (i <= tiempoProyeccion)
                                ingreso.Year1 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year1 = 0;
                            break;
                        case 2:
                            if (i <= tiempoProyeccion)
                                ingreso.Year2 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year2 = 0;
                            break;
                        case 3:
                            if (i <= tiempoProyeccion)
                                ingreso.Year3 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year3 = 0;
                            break;
                        case 4:
                            if (i <= tiempoProyeccion)
                                ingreso.Year4 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year4 = 0;
                            break;
                        case 5:
                            if (i <= tiempoProyeccion)
                                ingreso.Year5 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year5 = 0;
                            break;
                        case 6:
                            if (i <= tiempoProyeccion)
                                ingreso.Year6 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year6 = 0;
                            break;
                        case 7:
                            if (i <= tiempoProyeccion)
                                ingreso.Year7 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year7 = 0;
                            break;
                        case 8:
                            if (i <= tiempoProyeccion)
                                ingreso.Year8 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year8 = 0;
                            break;
                        case 9:
                            if (i <= tiempoProyeccion)
                                ingreso.Year9 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year9 = 0;
                            break;
                        case 10:
                            if (i <= tiempoProyeccion)
                                ingreso.Year10 = VentasPorPeriodo(codigoProducto, i);
                            else
                                ingreso.Year10 = 0;
                            break;
                    }                    
                }

                ingreso.Tipo = 3;
                var producto = Negocio.PlanDeNegocioV2.Formulacion.Solucion.Producto.GetProductoById(codigoProducto);
                ingreso.Iva = producto.Iva.GetValueOrDefault(0);
                ingreso.NombreProducto = producto.NomProducto;

                return ingreso;
            }
        }

        public static decimal VentasPorPeriodo(int codigoProducto, int year) {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var unidades = db.ProyectoProductoUnidadesVentas
                                 .Where(
                                        filter =>
                                        filter.CodProducto.Equals(codigoProducto)
                                        && filter.Ano.Equals(year)).Sum(sumatory => sumatory.Unidades);                                                   

                var precio = db.ProyectoProductoPrecios
                                .Where(
                                        filter => 
                                        filter.CodProducto.Equals(codigoProducto) 
                                        && filter.Periodo.Equals(year))
                                .Sum(sumatory => sumatory.Valor);

                return Convert.ToDecimal(unidades.GetValueOrDefault(0)) * precio.GetValueOrDefault(0);
            }
        }

    }

    public class ProyeccionDeVentasPorMes {
        public string Periodo { get {
                if (Mes.Equals(13))
                    return "Precio";
                if (Mes.Equals(14))
                    return "Total";
                                
                return  "Mes " + Mes.ToString() ;
            }
            set { }
        }
        public int Mes { get; set; }
        public ProyectoProductoUnidadesVenta Year1 { get; set; }
        public ProyectoProductoUnidadesVenta Year2 { get; set; }
        public ProyectoProductoUnidadesVenta Year3 { get; set; }
        public ProyectoProductoUnidadesVenta Year4 { get; set; }
        public ProyectoProductoUnidadesVenta Year5 { get; set; }
        public ProyectoProductoUnidadesVenta Year6 { get; set; }
        public ProyectoProductoUnidadesVenta Year7 { get; set; }
        public ProyectoProductoUnidadesVenta Year8 { get; set; }
        public ProyectoProductoUnidadesVenta Year9 { get; set; }
        public ProyectoProductoUnidadesVenta Year10 { get; set; }
        public Boolean UnLock {
                get {
                    return !Mes.Equals(14);
            }
            set { }
        }
        public string CssClass
        {
            get
            {
                if (Mes.Equals(13))
                    return "YearPrice";
                if (Mes.Equals(14))
                    return "YearTotal";

                return "Year";
            }
            set { }
        }
    }

    public class IngresosPorVentas
    {
        public string Periodo
        {
            get
            {
                if (Tipo.Equals(2))
                    return "Total";
                if (Tipo.Equals(1))
                    return "Iva";
                if (Tipo.Equals(0))
                    return "Total mas Iva";

                return NombreProducto;
            }
            set { }
        }
        public string NombreProducto { get; set; }
        public int Tipo { get; set; }
        public Decimal Year1 { get; set; }
        public string Year1Formatted { get { return Year1.moneyFormat(); } set { } }
        public Decimal Year2 { get; set; }
        public string Year2Formatted { get { return Year2.moneyFormat(); } set { } }
        public Decimal Year3 { get; set; }
        public string Year3Formatted { get { return Year3.moneyFormat(); } set { } }
        public Decimal Year4 { get; set; }
        public string Year4Formatted { get { return Year4.moneyFormat(); } set { } }
        public Decimal Year5 { get; set; }
        public string Year5Formatted { get { return Year5.moneyFormat(); } set { } }
        public Decimal Year6 { get; set; }
        public string Year6Formatted { get { return Year6.moneyFormat(); } set { } }
        public Decimal Year7 { get; set; }
        public string Year7Formatted { get { return Year7.moneyFormat(); } set { } }
        public Decimal Year8 { get; set; }
        public string Year8Formatted { get { return Year8.moneyFormat(); } set { } }
        public Decimal Year9 { get; set; }
        public string Year9Formatted { get { return Year9.moneyFormat(); } set { } }
        public Decimal Year10 { get; set; }
        public string Year10Formatted { get { return Year10.moneyFormat(); } set { } }
        public int Iva { get; set; }
    }
}
