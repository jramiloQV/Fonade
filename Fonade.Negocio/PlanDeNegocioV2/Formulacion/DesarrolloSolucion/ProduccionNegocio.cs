using Datos;
using Datos.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    /// <summary>
    /// Clase encargada de las operaciones CRUD del capítulo IV pestaña 5
    /// </summary>
    public class ProduccionNegocio
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
        /// Obtiene los datos ingresados anteriormente de la pestaña 5 del capítulo IV
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static ProyectoProduccion getFormulario(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.ProyectoProduccions
                               where datos.IdProyecto == codigoproyecto
                               select datos;

                return registro.FirstOrDefault();
            }
        }

        /// <summary>
        /// Inserta / actualiza los datos de la pestaña 5 del capítulo IV
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosFormulario(ProyectoProduccion obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoProduccions.InsertOnSubmit(obj);

                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoProduccions
                                         where datos.IdProyecto == obj.IdProyecto
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.IncrementoValor = obj.IncrementoValor;
                            objActual.ActivosProveedores = obj.ActivosProveedores;
                            objActual.CondicionesTecnicas = obj.CondicionesTecnicas;
                            objActual.RealizaImportacion = obj.RealizaImportacion;
                            objActual.Justificacion = obj.Justificacion;
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
        /// Inserta / actualiza los procesos de los productos en la pestaña 5 del capítulo IV
        /// </summary>
        /// <param name="lst">Objetos detalle de procesos del  </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDetalleProceso(List<ProyectoDetalleProceso> lst)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    foreach (var item in lst)
                    {
                        var objproceso = (from datos in db.ProyectoDetalleProcesos
                                          where datos.IdProducto == item.IdProducto
                                          select datos).SingleOrDefault();

                        if (objproceso != null)
                        {
                            objproceso.DescripcionProceso = item.DescripcionProceso;
                        }
                        else
                        {
                            db.ProyectoDetalleProcesos.InsertOnSubmit(item);
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

        #endregion
    }
}
