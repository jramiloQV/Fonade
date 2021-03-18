using Datos;
using Datos.DataType;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    /// <summary>
    /// Clase encargada de las operaciones CRUD del capítulo IV pestaña 4
    /// </summary>
    public static class RequerimientosNegocio
    {
        #region Variables

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene la Lista de infraestructura de un plan de negocio
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static List<RequerimientosNeg> getRequerimientos(int codigoproyecto, int idversion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registros =  (  from infra in db.ProyectoInfraestructuras
                                    join tipoinfra in db.TipoInfraestructuras
                                    on infra.CodTipoInfraestructura equals tipoinfra.Id_TipoInfraestructura into tmp
                                    where infra.codProyecto == codigoproyecto
                                    from datos in tmp
                                    join fuente in db.FuenteFinanciacions
                                    on infra.IdFuente equals fuente.IdFuente into tmp2
                                    where datos.IdVersion == idversion
                                    from datos2 in tmp2
                                    select new RequerimientosNeg
                                    {
                                        Cantidad = infra.Cantidad,
                                        CodTipoInfraestructura = datos.Id_TipoInfraestructura,
                                        IdProyectoInfraestructura = infra.Id_ProyectoInfraestructura,
                                        NomInfraestructura = infra.NomInfraestructura,
                                        FuenteFinanciacion = datos2.DescFuente,
                                        IdFuente = datos2.IdFuente,
                                        RequisitosTecnicos = infra.RequisitosTecnicos,
                                        ValorUnidad = infra.ValorUnidad
                            
                                    }).ToList();

                return registros != null ? registros : new List<RequerimientosNeg>();

            }
        }

        /// <summary>
        /// Obtiene los datos ingresados anteriormente de la pestaña 4 del capítulo IV
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static ProyectoRequerimiento getFormulario(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.ProyectoRequerimientos
                               where datos.IdProyecto == codigoproyecto
                               select datos;

                return registro.FirstOrDefault();
            }
        }

        public static ProyectoInfraestructura getRequerimiento(int idproyectoinfra)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return (from datos in db.ProyectoInfraestructuras
                           where datos.Id_ProyectoInfraestructura == idproyectoinfra
                           select datos).SingleOrDefault();
            }
        }

        /// <summary>
        /// Inserta / actualiza los datos de la pestaña 4 del capítulo IV
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosFormulario(ProyectoRequerimiento obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    

                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        
   
                        db.ProyectoRequerimientos.InsertOnSubmit(obj);
                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoRequerimientos
                                         where datos.IdProyecto == obj.IdProyecto
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.LugarFisico = obj.LugarFisico;
                            objActual.TieneLugarFisico = obj.TieneLugarFisico;
                        }

                    }

                    db.SubmitChanges();
                }
            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Inserta / actualiza un requerimiento de un plan de negocio
        /// </summary>
        /// <param name="item">Objeto a insertar / actualizar</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setRequerimiento(ProyectoInfraestructura item, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoInfraestructuras.InsertOnSubmit(item);
                    }
                    else
                    {
                        var reg = (from datos in db.ProyectoInfraestructuras
                                   where datos.Id_ProyectoInfraestructura == item.Id_ProyectoInfraestructura
                                   select datos).SingleOrDefault();

                        if (reg != null)
                        {
                            reg.Cantidad = item.Cantidad;
                            reg.IdFuente = item.IdFuente;
                            reg.NomInfraestructura = item.NomInfraestructura;
                            reg.RequisitosTecnicos = item.RequisitosTecnicos;
                            reg.ValorUnidad = item.ValorUnidad;
                            reg.CodTipoInfraestructura = item.CodTipoInfraestructura;
                        }
                    }

                    db.SubmitChanges();

                    //Se calculan los registros de la tabla ProyectoInversion
                    CalcularProyeccionesInversion(item.codProyecto);
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Borra un requerimiento de un plan de negocio
        /// </summary>
        /// <param name="idRequerimiento">Identificador primario del requerimiento</param>
        /// <returns>Verdadero si la operación de borrado es exitosa. Falso en otro caso</returns>
        public static bool delRequerimiento(int idRequerimiento)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    
                    var reg = (from datos in db.ProyectoInfraestructuras
                               where datos.Id_ProyectoInfraestructura == idRequerimiento
                               select datos).SingleOrDefault();

                    db.ProyectoInfraestructuras.DeleteOnSubmit(reg);

                    db.SubmitChanges();

                    //Se calculan los registros de la tabla ProyectoInversion
                    CalcularProyeccionesInversion(reg.codProyecto);
                }

            }
            catch
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        private static void CalcularProyeccionesInversion(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                List<ProyectoInversion> lstBorrar = new List<ProyectoInversion>();
                List<ProyectoInversion> lstNuevos = new List<ProyectoInversion>();

                string[] reg2 = db.ProyectoInfraestructuras
                    .Where(x => x.codProyecto == codigoproyecto)
                    .Select(y => y.TipoInfraestructura.NomTipoInfraestructura+"|"+ y.FuenteFinanciacion.DescFuente)
                    .Distinct().ToArray();

                //Se consultan los requerimientos del plan de negocio
                var lst = db.ProyectoInfraestructuras
                    .Where(x => x.codProyecto == codigoproyecto)
                    .GroupBy(grupo => new
                    {
                        grupo.CodTipoInfraestructura,
                        grupo.IdFuente,
                        grupo.codProyecto
                    }
                    ).Select(x => new
                    {
                        CodProyecto = x.Key.codProyecto,
                        Concepto = db.TipoInfraestructuras.Where(y => y.Id_TipoInfraestructura == x.Key.CodTipoInfraestructura).SingleOrDefault().NomTipoInfraestructura,
                        Valor = x.Sum(y => (float)(y.ValorUnidad.Value) * (int)(y.Cantidad.Value)),
                        AportadoPor = db.FuenteFinanciacions.Where(y => y.IdFuente == x.Key.IdFuente).SingleOrDefault().DescFuente,
                        IdFuenteFinanciacion = x.FirstOrDefault() != null ? x.First().IdFuente : null 

                    }).ToList();

                foreach (var item in lst)
                {
                    //Se busca los ProyectoInversion asociados
                    var reg1 = (from datos1 in db.ProyectoInversions
                                where datos1.CodProyecto == item.CodProyecto
                                && datos1.Concepto.Equals(item.Concepto)
                                && datos1.AportadoPor.Equals(item.AportadoPor)
                                select datos1).SingleOrDefault();

                    //Si no existe se genera uno nuevo
                    if (reg1 == null)
                    {
                        ProyectoInversion nuevo = new ProyectoInversion()
                        {
                            CodProyecto = item.CodProyecto,
                            Concepto = item.Concepto,
                            Valor = Convert.ToDecimal(item.Valor),
                            AportadoPor = item.AportadoPor,
                            Semanas = 0,
                            TipoInversion = "Fija",
                            IdFuenteFinanciacion = item.IdFuenteFinanciacion
                        };

                        //Se almacena en el listado de nuevos registros
                        lstNuevos.Add(nuevo);
                    }
                    else
                    {
                        //Se actualizan los valores
                        reg1.AportadoPor = item.AportadoPor;
                        reg1.Concepto = item.Concepto;
                        reg1.Valor = Convert.ToDecimal(item.Valor);
                        reg1.IdFuenteFinanciacion = item.IdFuenteFinanciacion;
                    }
                }

                lstBorrar = db.ProyectoInversions.Where(x => !reg2.Contains(x.Concepto + "|" + x.AportadoPor) && x.CodProyecto == codigoproyecto).ToList();
                            

                db.ProyectoInversions.DeleteAllOnSubmit(lstBorrar);
                db.ProyectoInversions.InsertAllOnSubmit(lstNuevos);

                db.SubmitChanges();
            }
        }

        #endregion
    }
}
