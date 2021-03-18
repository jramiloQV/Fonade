using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.DesarrolloSolucion
{
    /// <summary>
    /// Clase encargada de las operaciones CRUD del capítulo IV pestaña 3
    /// </summary>
    public static class NormatividadYCondicionTech
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
        /// Obtiene los datos ingresados anteriormente de la pestaña 1 del capítulo IV
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static ProyectoNormatividad getFormulario(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = (from datos in db.ProyectoNormatividads
                               where datos.IdProyecto == codigoproyecto
                               select datos).FirstOrDefault();

                return registro;
            }
        }

        /// <summary>
        /// Inserta / actualiza los datos de la pestaña 3 del capítulo IV
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosFormulario(ProyectoNormatividad obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {


                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoNormatividads.InsertOnSubmit(obj);
                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoNormatividads
                                         where datos.IdProyecto == obj.IdProyecto
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.Ambiental = obj.Ambiental;
                            objActual.CondicionesTecnicas = obj.CondicionesTecnicas;
                            objActual.Empresarial = obj.Empresarial;
                            objActual.Laboral = obj.Laboral;
                            objActual.RegistroMarca = obj.RegistroMarca;
                            objActual.Tecnica = obj.Tecnica;
                            objActual.Tributaria = obj.Tributaria;
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
