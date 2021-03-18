using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.ImpresionPlanNeg
{
    public static class ImpresionPlanNeg
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
        /// Consulta la información general del proyecto
        /// </summary>
        /// <param name="codigoProyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static MD_InformacionProyectoResult getInfoProyecto(int codigoProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.MD_InformacionProyecto(codigoProyecto)
                               select datos;

                return registro.SingleOrDefault();
            }
        }

        /// <summary>
        /// Obtiene el valor del salario mínimo
        /// </summary>
        /// <param name="anio">Año a consultar</param>
        /// <returns>Valor salario según año</returns>
        public static long getSalarioMinimo(int anio)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.SalariosMinimos
                               where datos.AñoSalario == anio
                               select datos;

                return registro != null ? registro.SingleOrDefault().SalarioMinimo : -1;
            }
        }

        #endregion

    }
}
