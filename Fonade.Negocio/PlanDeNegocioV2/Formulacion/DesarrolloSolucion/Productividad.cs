using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    /// <summary>
    /// Clase encargada de las operaciones CRUD del capítulo IV pestaña 6
    /// </summary>
    public class Productividad
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
        /// Obtiene los datos ingresados anteriormente de la pestaña 6 del capítulo IV
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static ProyectoProductividad getFormulario(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.ProyectoProductividads
                               where datos.IdProyecto == codigoproyecto
                               select datos;

                return registro.FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene los datos de un cargo en un plan de negocio
        /// </summary>
        /// <param name="idcargo">Identificador primario del cargo</param>
        /// <returns>Información consultada</returns>
        public static ProyectoGastosPersonal getCargo(int idcargo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.ProyectoGastosPersonals
                               where datos.Id_Cargo == idcargo
                               select datos;

                return registro.FirstOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el listado de cargos en un plan de negocio
        /// </summary>
        /// <param name="codproyecto">Identificador primario del cargo</param>
        /// <returns>Información consultada</returns>
        public static List<ProyectoGastosPersonal> getCargos(int codproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = (from datos in db.ProyectoGastosPersonals
                               where datos.CodProyecto == codproyecto
                               select datos).ToList();

                return registro != null ? registro : new List<ProyectoGastosPersonal>();
            }
        }

        /// <summary>
        /// Inserta / actualiza los datos de la pestaña 6 del capítulo IV (pregunta 17.1)
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosPerfil(ProyectoEmprendedorPerfil obj)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza

                    var objActual = (from datos in db.ProyectoEmprendedorPerfils
                                     where datos.IdEmprendedorPerfil == obj.IdEmprendedorPerfil
                                     select datos).SingleOrDefault();

                    if (objActual != null)
                    {
                        objActual.Perfil = obj.Perfil;
                        objActual.Rol = obj.Rol;
                    }
                    else
                    {
                        db.ProyectoEmprendedorPerfils.InsertOnSubmit(obj);
                    };

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
        /// Inserta / actualiza los datos de la pestaña 6 del capítulo IV (Pregunta 16)
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosFormulario(ProyectoProductividad obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoProductividads.InsertOnSubmit(obj);
                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoProductividads
                                         where datos.IdProductividad == obj.IdProductividad
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.CapacidadProductiva = obj.CapacidadProductiva;
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
        /// Inserta / actualiza los datos de la pestaña 6 del capítulo IV (Pregunta 17.2)
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setCargo(ProyectoGastosPersonal obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Se realizan los cálculos para plan operativo
                    obj.ValorMensual = obj.UnidadTiempo.Value == Constantes.CONST_UniTiempoDias ?
                            obj.ValorRemuneracion.Value * 30 :
                            obj.ValorRemuneracion.Value;

                    obj.ValorAnual = obj.ValorMensual * 12;

                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoGastosPersonals.InsertOnSubmit(obj);
                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoGastosPersonals
                                         where datos.Id_Cargo == obj.Id_Cargo
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.Cargo = obj.Cargo;
                            objActual.AportesEmprendedor = obj.AportesEmprendedor;
                            objActual.Dedicacion = obj.Dedicacion;
                            objActual.ExperienciaEspecifica = obj.ExperienciaEspecifica;
                            objActual.ExperienciaGeneral = obj.ExperienciaGeneral;
                            objActual.Formacion = obj.Formacion;
                            objActual.Funciones = obj.Funciones;
                            objActual.IngresosVentas = obj.IngresosVentas;
                            objActual.OtrosGastos = obj.OtrosGastos;
                            objActual.TiempoVinculacion = obj.TiempoVinculacion;
                            objActual.TipoContratacion = obj.TipoContratacion;
                            objActual.UnidadTiempo = obj.UnidadTiempo;
                            objActual.ValorFondoEmprender = obj.ValorFondoEmprender;
                            objActual.ValorRemuneracion = obj.ValorRemuneracion;
                            objActual.ValorMensual = obj.ValorMensual;
                            objActual.ValorAnual = obj.ValorAnual;
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
        /// Elimina un carga de un plan de negocio
        /// </summary>
        /// <param name="idCargo">Identificador primario del cargo</param>
        /// <returns>Verdadero si la operación de borrado es exitosa. Falso en otro caso</returns>
        public static bool delCargo(int idCargo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Se consulta el registro padre a borrar
                    var reg = (from datos in db.ProyectoGastosPersonals
                               where datos.Id_Cargo == idCargo
                               select datos).SingleOrDefault();


                    //Se consulta el registro asociado al cargo de la tabla ProyectoEmpleoCargo
                    var reg1 = (from datos in db.ProyectoEmpleoCargos
                               where datos.CodCargo == idCargo
                               select datos).ToList();

                    //Se consulta si tiene datos en la tabla de evaluacion cargo
                    var evalCargo = (from cargo in db.IndicadorCargoEvaluacions
                                     where cargo.IdCargo == idCargo
                                     select cargo).FirstOrDefault();

                    //Se realizo el borrado de los registros hijos
                    if(reg1 != null)
                    {
                        db.ProyectoEmpleoCargos.DeleteAllOnSubmit(reg1);
                    }

                    //Eliminar cargo de la fase de evaluacion
                    if (evalCargo != null)
                    {
                        db.IndicadorCargoEvaluacions.DeleteOnSubmit(evalCargo);
                    }

                    //Se realiza el registro del padre principal
                    db.ProyectoGastosPersonals.DeleteOnSubmit(reg);

                    db.SubmitChanges();

                }

            }
            catch(Exception ex)
            {
                operacionOk = false;
            }

            return operacionOk;
        }

        /// <summary>
        /// Indica si el cargo ya existe en el plan de negocio
        /// </summary>
        /// <param name="nomcargo">Nombre del Cargo</param>
        /// <param name="codproyecto">Código del proyecto</param>
        /// <returns>Verdadero si existe, falso en otro caso</returns>
        public static bool existeCargo(int idcargo, string nomcargo, int codproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                return (from datos in db.ProyectoGastosPersonals
                                where datos.CodProyecto == codproyecto
                                && datos.Cargo.Equals(nomcargo)
                                && datos.Id_Cargo != idcargo
                                select datos).Count() > 0;
            }
        }

        #endregion
    }
}
