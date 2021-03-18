using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;

namespace Fonade.Negocio.PlanDeNegocioV2.Administracion.Interventoria.Reportes
{
    public class EstadisticasPagosBLL
    {
        private string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<EstadisticasPagosModel> GetEstadisticasPagos(int _codCoordinador, int _codInterventor
                                                                        , DateTime? fechaInicial
                                                                        , DateTime? fechaFinal
                                                                        , int? _codOperador)
        {
            List<EstadisticasPagosModel> estadisticasPagos = new List<EstadisticasPagosModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                if (_codOperador == null)
                {
                    estadisticasPagos = (from e in db.Vista_Estadisticas_Pagos
                                         select new EstadisticasPagosModel
                                         {
                                             idProyecto = e.Id_Proyecto,
                                             nomProyecto = e.NomProyecto,
                                             codInterventor = e.codInterventor,
                                             nombreInterventor = e.Interventor,
                                             fechaAprobInterventor = e.FechaAprobacionInterventor,
                                             fechaAprobORechaCoordinador = e.FechaAprobacionORechazoCoord,
                                             fechaRespuestaFiducia = e.FechaRespuestaFiduciaria,
                                             idPagoActividad = e.Id_PagoActividad,
                                             nomPagoActividad = e.NomPagoActividad,
                                             cantidadDinero = e.CantidadDinero,
                                             estado = e.Estado,
                                             observacionFiduciaOCoordinador = e.ObservacionesFiduciariaOCoordinacion,
                                             codCoordinador = e.codCoordinador,
                                             nombreCoordinador = e.coordInterventor,
                                             codOperador = e.codOperador,
                                             Operador = e.NombreOperador
                                         }).ToList();
                }
                else
                {
                    estadisticasPagos = (from e in db.Vista_Estadisticas_Pagos
                                         where e.codOperador == _codOperador
                                         select new EstadisticasPagosModel
                                         {
                                             idProyecto = e.Id_Proyecto,
                                             nomProyecto = e.NomProyecto,
                                             codInterventor = e.codInterventor,
                                             nombreInterventor = e.Interventor,
                                             fechaAprobInterventor = e.FechaAprobacionInterventor,
                                             fechaAprobORechaCoordinador = e.FechaAprobacionORechazoCoord,
                                             fechaRespuestaFiducia = e.FechaRespuestaFiduciaria,
                                             idPagoActividad = e.Id_PagoActividad,
                                             nomPagoActividad = e.NomPagoActividad,
                                             cantidadDinero = e.CantidadDinero,
                                             estado = e.Estado,
                                             observacionFiduciaOCoordinador = e.ObservacionesFiduciariaOCoordinacion,
                                             codCoordinador = e.codCoordinador,
                                             nombreCoordinador = e.coordInterventor,
                                             codOperador = e.codOperador,
                                             Operador = e.NombreOperador
                                         }).ToList();
                }

            }

            if (validarPermisosEspeciales(_codCoordinador, Datos.Constantes.const_EstadisticaPagos))
            {
                //validar si tiene el permiso de ver todos los interventores
                if (!validarPermisoVerTodosInterventores(_codCoordinador))
                {
                    string[] codInterventores = PermisoInterventoresCoord(_codCoordinador);

                    var estadisticasPagosFiltrado = from element in estadisticasPagos
                                                    where (codInterventores
                                                            .Contains(element.codInterventor.ToString()))
                                                    select element;

                    estadisticasPagos = estadisticasPagosFiltrado.ToList();
                }
            }
            else
            {
                if (_codCoordinador != 0)
                {
                    estadisticasPagos = estadisticasPagos.Where(x => x.codCoordinador == _codCoordinador).ToList();
                }
            }

            if (_codInterventor != 0)
            {
                estadisticasPagos = estadisticasPagos.Where(x => x.codInterventor == _codInterventor).ToList();
            }

            if (fechaInicial != null)
            {
                estadisticasPagos = estadisticasPagos.Where(x => x.fechaAprobInterventor.HasValue
                                                                ? x.fechaAprobInterventor.Value.Date >= fechaInicial.Value.Date : false).ToList();
            }

            if (fechaFinal != null)
            {
                estadisticasPagos = estadisticasPagos.Where(x => x.fechaAprobInterventor.HasValue
                                                                ? x.fechaAprobInterventor.Value.Date <= fechaFinal.Value.Date : false).ToList();
            }

            return estadisticasPagos;
        }

        public List<EstadisticasAvancesModel> GetEstadisticasAvances(int _codInterventor)
        {
            List<EstadisticasAvancesModel> estadisticasAvances = new List<EstadisticasAvancesModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                estadisticasAvances = (from e in db.sp_EstadisticaAvanceXInterventor(_codInterventor)
                                       select new EstadisticasAvancesModel
                                       {
                                           idProyecto = e.CodProyecto,
                                           codActividad = e.CodActividad,
                                           nomActividad = e.NomActividad,
                                           item = e.Item,
                                           mes = e.Mes,
                                           fechaAvanceEmprendedor = e.FechaAvance,
                                           observacionesEmprendedor = e.Observaciones,
                                           fechaAprobacionInterventor = e.FechaAprobacion,
                                           observacionesInterventor = e.ObservacionesInterventor,
                                           Aprobada = e.Aprobada,
                                           codInterventor = e.codInterventor,
                                           nomInterventor = e.Interventor,
                                           nomEntidad = e.Entidad,
                                           nomOperador = e.Operador
                                       }).ToList();
            }

            return estadisticasAvances;
        }


        public List<InterventoresActivosModel> getListadoInterventores(int _codCoordinador, int? _codOperador
                                                                        , int _opcEstadistica)
        {
            List<InterventoresActivosModel> interventores = new List<InterventoresActivosModel>();

            List<InterventoresActivosModel> query = new List<InterventoresActivosModel>();

            InterventoresActivosModel todos = new InterventoresActivosModel
            {
                codInterventor = 0,
                nomInterventor = "TODOS",
                codCoordinador = 0
            };

            interventores.Add(todos);

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                if (_codOperador == null)
                {

                    query = (from e in db.Vista_InterventoresActivos
                             select new InterventoresActivosModel
                             {
                                 codInterventor = e.codInterventor,
                                 nomInterventor = e.interventores,
                                 codCoordinador = e.codCoordinador
                             })
                           .OrderBy(x => x.nomInterventor).ToList();
                }
                else
                {

                    query = (from e in db.Vista_InterventoresActivos
                             select new InterventoresActivosModel
                             {
                                 codInterventor = e.codInterventor,
                                 nomInterventor = e.interventores,
                                 codCoordinador = e.codCoordinador,
                                 codOperador = e.codOperador
                             })
                            .Where(x => x.codOperador == _codOperador)
                            .OrderBy(x => x.nomInterventor).ToList();
                }

                query.ForEach(c => interventores.Add(c));
            }

            //validar si se encuentra en la tabla de permisos especiales "PermisosInformesCoordInterv"

            if (validarPermisosEspeciales(_codCoordinador, _opcEstadistica))
            {
                //validar si tiene el permiso de ver todos los interventores
                if (!validarPermisoVerTodosInterventores(_codCoordinador))
                {
                    string[] codInterventores = PermisoInterventoresCoord(_codCoordinador);                    
                    var interventoresFiltrado = from element in interventores
                                                    where (codInterventores
                                                            .Contains(element.codInterventor.ToString()))
                                                    select element;

                    interventores = interventoresFiltrado.ToList();
                }
            }
            else
            {
                if (_codCoordinador != 0)
                {
                    interventores = interventores.Where(x => x.codCoordinador == _codCoordinador
                                                            || x.codCoordinador == 0).ToList();


                }
            }

            return interventores;
        }

        private string [] PermisoInterventoresCoord(int _codCoordinador)
        {
            string[] list;

            List<string> Interventores = new List<string>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                Interventores = (from p in db.PermisosInformesCoordInterv
                             where p.codCoordinadorInterventor == _codCoordinador
                             select p.ListadoInterventores).FirstOrDefault().Split(';').ToList();
            }

            Interventores.Add("0");

            list = Interventores.ToArray();

            return list;
        }

        private bool validarPermisoVerTodosInterventores(int _codCoordinador)
        {
            bool valido = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                valido = (from p in db.PermisosInformesCoordInterv
                          where p.codCoordinadorInterventor == _codCoordinador
                          select p.TodosInterventores).FirstOrDefault();
            }

            return valido;
        }

        private bool validarPermisosEspeciales(int _codCoordinador, int opcEstadistica)
        {
            bool valido = false;

            //Pagos
            if (Constantes.const_EstadisticaPagos == opcEstadistica)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    valido = (from p in db.PermisosInformesCoordInterv
                              where p.codCoordinadorInterventor == _codCoordinador
                              select p.EstadisticasPagos).FirstOrDefault();
                }
            }

            //Avances
            if (Constantes.const_EstadisticaAvance == opcEstadistica)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    valido = (from p in db.PermisosInformesCoordInterv
                              where p.codCoordinadorInterventor == _codCoordinador
                              select p.EstadisticasAvances).FirstOrDefault();
                }
            }

            return valido;
        }


    }
}
