using Datos;
using Datos.DataType;
using Fonade.Negocio.PlanDeNegocioV2.Formulacion.Protagonista;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.DesarrolloSolucion
{
    /// <summary>
    /// Clase encargada de las operaciones CRUD del capítulo IV pestaña 1
    /// </summary>
    public static class IngresosYCondicionesComercio
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
        /// Determina si el cliente maneja el perfil consumidor
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>True si no existe o es consumidor. Falso en otro caso</returns>
        public static bool esConsumidor(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {

                var consulta = from registro in db.ProyectoProtagonistas
                               where registro.IdProyecto == codigoproyecto
                               select registro;

                return consulta.SingleOrDefault() != null ? consulta.SingleOrDefault().PerfilesDiferentes : true;
                       
            }
        }

        /// <summary>
        /// Obtiene las condiciones de los clientes de un plan de proyecto
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Listado de condiciones consultadas</returns>
        public static List<CondicionesCliente> getCondicionesClientes(int codigoproyecto)
        {
            List<CondicionesCliente> lstClientes = Protagonista.GetClientesCondiciones(codigoproyecto);

            return lstClientes != null ? lstClientes : new List<CondicionesCliente>();
        }

        /// <summary>
        /// Obtiene la condición de un cliente en un plan de proyecto
        /// </summary>
        ///<param name="idcliente">Identificador primario del cliente</param>
        /// <returns>Condición consultada</returns>
        public static CondicionesCliente getCondicionCliente(int idcliente)
        {
            return Protagonista.GetClienteCondiciones(idcliente);
        }

        /// <summary>
        /// Obtiene los datos ingresados anteriormente de la pestaña 1 del capítulo IV
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Información consultada</returns>
        public static ProyectoDesarrolloSolucion getFormulario(int codigoproyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var registro = from datos in db.ProyectoDesarrolloSolucions
                               where datos.IdProyecto == codigoproyecto
                               select datos;

                return registro.FirstOrDefault();
            }
        }

        /// <summary>
        /// Inserta / actualiza los datos de la pestaña 1 del capítulo IV
        /// </summary>
        /// <param name="obj">Objeto </param>
        /// <param name="esNuevo">Indica si el registro es nuevo</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setDatosFormulario(ProyectoDesarrolloSolucion obj, bool esNuevo)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza
                    if (esNuevo)
                    {
                        db.ProyectoDesarrolloSolucions.InsertOnSubmit(obj);
                    }
                    else
                    {
                        var objActual = (from datos in db.ProyectoDesarrolloSolucions
                                         where datos.IdDesarrolloSolucion == obj.IdDesarrolloSolucion
                                         select datos).FirstOrDefault();

                        if (objActual != null)
                        {
                            objActual.CaracteristicasCompra = obj.CaracteristicasCompra;
                            objActual.DondeCompra = obj.DondeCompra;
                            objActual.FrecuenciaCompra = obj.FrecuenciaCompra;
                            objActual.Ingresos = obj.Ingresos;
                            objActual.Precio = obj.Precio;

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
        /// Inserta / actualiza los datos de las condiciones de los clientes de un plan de negocio
        /// </summary>
        /// <param name="item">Objeto a insertar / actualizar</param>
        /// <returns>Verdadero si la operación de inserción / actualización es exitosa. Falso en otro caso</returns>
        public static bool setCondicionesCliente(ProyectoCondicionesComerciale item)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {
                    //Si es nuevo registro se inserta, si no se actualiza

                    var reg = (from datos in db.ProyectoCondicionesComerciales
                               where datos.IdCliente == item.IdCliente
                               select datos).SingleOrDefault();

                    if (reg != null)
                    {
                        reg.CaracteristicasCompra = item.CaracteristicasCompra;
                        reg.FormaPago = item.FormaPago;
                        reg.FrecuenciaCompra = item.FrecuenciaCompra;
                        reg.Garantias = item.Garantias;
                        reg.Margen = item.Margen;
                        reg.Precio = item.Precio;
                        reg.RequisitosPostVenta = item.RequisitosPostVenta;
                        reg.SitioCompra = item.SitioCompra;
                    }
                    else
                    {
                        db.ProyectoCondicionesComerciales.InsertOnSubmit(item);
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
        /// Limpia las preguntas asociada a clientes con perfiles diferentes
        /// </summary>
        /// <param name="codigoproyecto">Código del proyecto</param>
        /// <returns>Verdadero si la actualización es exitosa. Falso en otro caso</returns>
        public static bool limpiarDatosConsumidor(int codigoproyecto)
        {
            bool operacionOk = true;

            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
                {

                    var objActual = (from datos in db.ProyectoDesarrolloSolucions
                                     where datos.IdProyecto == codigoproyecto
                                     select datos).FirstOrDefault();

                    if (objActual != null)
                    {
                        objActual.DondeCompra = "";
                        objActual.FrecuenciaCompra = "";
                        objActual.Ingresos = "";
                        objActual.Precio = "";

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
