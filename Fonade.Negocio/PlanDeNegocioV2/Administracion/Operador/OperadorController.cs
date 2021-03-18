using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador
{
    public class OperadorController
    {

        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public OperadorModel getOperador(int? _codOperador)
        {
            OperadorModel operador = new OperadorModel();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                operador = (from o in db.Operador
                            where o.IdOperador == _codOperador
                            select new OperadorModel
                            {
                                DireccionOperador = o.DireccionOperador,
                                EmailObservacionAcreditacion = o.EmailObservacionAcreditacion,
                                EmailOperador = o.EmailOperador,
                                EmailRepresentante = o.EmailRepresentante,
                                FechaCreacion = o.FechaCreacion,
                                FechaModificacion = o.FechaModificacion,
                                idOperador = o.IdOperador,
                                NitOperador = o.NitOperador,
                                NombreOperador = o.NombreOperador,
                                NombreRepresentante = o.NombreRepresentante,
                                TelefonoOperador =o.TelefonoOperador,
                                TelefonoRepresentante = o.TelefonoRepresentante,
                                Rutalogo = o.Rutalogo
                            }).FirstOrDefault();
            }

                return operador;
        }

        public List<OperadorModel> getAllOperador()
        {
            List<OperadorModel> operador = new List<OperadorModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                operador = (from o in db.Operador
                            where o.Activo.Equals(true)
                            orderby o.NombreOperador                            
                            select new OperadorModel
                            {
                                DireccionOperador = o.DireccionOperador,
                                EmailObservacionAcreditacion = o.EmailObservacionAcreditacion,
                                EmailOperador = o.EmailOperador,
                                EmailRepresentante = o.EmailRepresentante,
                                FechaCreacion = o.FechaCreacion,
                                FechaModificacion = o.FechaModificacion,
                                idOperador = o.IdOperador,
                                NitOperador = o.NitOperador,
                                NombreOperador = o.NombreOperador,
                                NombreRepresentante = o.NombreRepresentante,
                                TelefonoOperador = o.TelefonoOperador,
                                TelefonoRepresentante = o.TelefonoRepresentante
                            }).ToList();
            }

            return operador;
        }

        public List<OperadorModel> cargaDLLOperador(int? _codOperador)
        {
            List<OperadorModel> opciones = new List<OperadorModel>();

            OperadorModel opcionUnica = new OperadorModel();

            if (_codOperador == null)
            {
                opcionUnica.idOperador = 0;
                opcionUnica.NombreOperador = "Seleccione...";

                opciones = getAllOperador();

                opciones.Add(opcionUnica);

                opciones = opciones.OrderBy(x => x.idOperador).ToList();
            }
            else
            {
                opcionUnica = getOperador(_codOperador);
                opciones.Add(opcionUnica);
            }

            return opciones;
        }

        /// <summary>
        /// Actualizar el operador seleccionado
        /// </summary>
        /// <param name="operadorModel">modelo del operador</param>
        /// <param name="mensaje">mensaje que se devuelve en caso de error</param>
        /// <returns>retorna true si se actualiza correctamente o false si no lo hace </returns>
        public bool actualizarOperador(OperadorModel operadorModel, ref string mensaje)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var operador = (from o in db.Operador
                                where o.IdOperador == operadorModel.idOperador
                                select o).FirstOrDefault();

                var operadorList = (from o in db.Operador
                                where o.IdOperador != operadorModel.idOperador
                                select o).ToList();

                var cant = operadorList.Where(x => x.NitOperador == operadorModel.NitOperador
                                    || x.NombreOperador.ToUpper() == operadorModel.NombreOperador.ToUpper()).Count();

                if (cant > 0)
                {
                    mensaje = "No es posible actualizar el operador, ya existe el nit o nombre para otro operador.";
                    actualizado = false;
                    return actualizado;
                }
                
                if (operador == null)
                {
                    mensaje = "No se encontró el operador a actualizar.";
                    actualizado = false;
                }
                else
                {
                    operador.DireccionOperador = operadorModel.DireccionOperador;
                    operador.EmailObservacionAcreditacion = operadorModel.EmailObservacionAcreditacion;
                    operador.EmailOperador = operadorModel.EmailOperador;
                    operador.EmailRepresentante = operadorModel.EmailRepresentante;
                    operador.FechaModificacion = DateTime.Now;
                    operador.NitOperador = operadorModel.NitOperador;
                    operador.NombreOperador = operadorModel.NombreOperador;
                    operador.NombreRepresentante = operadorModel.NombreRepresentante;
                    operador.TelefonoOperador = operadorModel.TelefonoOperador;
                    operador.TelefonoRepresentante = operadorModel.TelefonoRepresentante;

                    if (operadorModel.Rutalogo != string.Empty)
                        operador.Rutalogo = operadorModel.Rutalogo;

                    db.SubmitChanges();

                    actualizado = true;
                }
            }

            return actualizado;
        }

        public bool InsertarOperador(OperadorModel operadorModel, ref string mensaje)
        {
            bool insert = false;
             
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var cant = (from o in db.Operador
                            where o.NitOperador.Equals(operadorModel.NitOperador) ||
                            o.NombreOperador.ToUpper().Equals(operadorModel.NombreOperador.ToUpper())
                            select o.IdOperador).Count();

                if (cant > 0)
                {
                    mensaje = "No se pudo guardar el operador, ya existe un operador con el mismo nit o nombre.";
                    insert = false;
                }
                else
                {
                    Datos.Operador entity = new Datos.Operador { 
                        DireccionOperador = operadorModel.DireccionOperador,
                        EmailObservacionAcreditacion = operadorModel.EmailObservacionAcreditacion,
                        EmailOperador = operadorModel.EmailOperador,
                        EmailRepresentante = operadorModel.EmailRepresentante,
                        FechaCreacion = DateTime.Now,
                        FechaModificacion = DateTime.Now,
                        NitOperador = operadorModel.NitOperador,
                        NombreOperador = operadorModel.NombreOperador,
                        NombreRepresentante = operadorModel.NombreRepresentante,
                        Rutalogo = operadorModel.Rutalogo,
                        TelefonoOperador = operadorModel.TelefonoOperador,
                        TelefonoRepresentante = operadorModel.TelefonoRepresentante
                    };

                    db.Operador.InsertOnSubmit(entity);
                    db.SubmitChanges();

                    insert = true;
                }

            }

            return insert;
        }

        public bool desactivarOperador(int _codOperador, ref string mensaje)
        {
            bool desactivado = false;

            //validar que no este asociado a una convocatoria o convenio
            if (validarAsociacionConvocatoriaOConvenio(_codOperador))
            {
                mensaje = "El operador no se puede eliminar, ya que se encuentra asociado a una o mas convocatorias";
                desactivado = false;
            }
            else
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    var operador = (from o in db.Operador
                                    where o.IdOperador == _codOperador
                                    select o).FirstOrDefault();

                    operador.Activo = false;
                    operador.FechaModificacion = DateTime.Now;
                    db.SubmitChanges();

                    desactivado = true;
                }
            }
            return desactivado;
        }

        private bool validarAsociacionConvocatoriaOConvenio(int _codOperador)
        {
            bool validar;
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                var cant = (from c in db.Convocatoria
                            where c.codOperador == _codOperador
                            select c.Id_Convocatoria).Count();

                validar = cant > 0 ? true : false;                            
            }

            return validar;
        }

        public bool validarOperadorXProyecto(int? _codOperadorUsuario, int _codProyecto)
        {
            bool valido = false; ;

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                int codEstado = (from p in db.Proyecto
                                 where p.Id_Proyecto == _codProyecto
                                 select p.CodEstado).FirstOrDefault();

                int? codOperadorProyecto = (from p in db.Proyecto
                                 where p.Id_Proyecto == _codProyecto
                                 select p.codOperador).FirstOrDefault();

                if (codEstado < Constantes.CONST_Convocatoria //Si el estado es menor a convocatoria es valido
                    || _codOperadorUsuario == null //Si el usuario no tiene operador asociado es valido
                    || _codOperadorUsuario == codOperadorProyecto) //Si los operadores son iguales es valido
                {
                    valido = true;
                }                
            }

                return valido;
        }

        public string nombreOperadorXProyecto(int _codProyecto)
        {
            string nombreOperador = "";

            int codOperador = 0;

            //hallamos el id del operador
            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                codOperador = (from p in db.Proyecto
                               where p.Id_Proyecto == _codProyecto
                               select p.codOperador).FirstOrDefault()??0;
            }

            if (codOperador>0)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    nombreOperador = (from o in db.Operador
                                      where o.IdOperador == codOperador
                                      select o.NombreOperador).FirstOrDefault();
                }
            }
            
            return nombreOperador;
        }

    }
}
