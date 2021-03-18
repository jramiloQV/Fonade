using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimCompromisosController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public bool InsertCompromisos(ActaSeguimCompromisosModel compromiso)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                ActaSeguimCompromisos gesCompromiso = new ActaSeguimCompromisos
                {
                    codConvocatoria = compromiso.codConvocatoria,
                    codProyecto = compromiso.codProyecto,
                    numActa = compromiso.numActa,
                    visita = compromiso.visita,
                    fechaModificado = compromiso.fechaModificado,
                    compromiso = compromiso.compromiso,
                    estado = compromiso.estado,
                    fechaCumpliCompromiso = compromiso.fechaCumpliCompromiso,
                    fechaPropuestaEjec = compromiso.fechaPropuestaEjec,
                    observacion = compromiso.observacion
                };

                db.ActaSeguimCompromisos.InsertOnSubmit(gesCompromiso);

                db.SubmitChanges();

                insertado = true;

                ActaSeguimCompromisosHistoricoModel historicoModel = new ActaSeguimCompromisosHistoricoModel()
                {
                    idCompromiso = gesCompromiso.idActaCompromiso,
                    codConvocatoria = gesCompromiso.codConvocatoria,
                    codProyecto = gesCompromiso.codProyecto,
                    compromiso = gesCompromiso.compromiso,
                    estado = gesCompromiso.estado,
                    fechaCumpliCompromiso = gesCompromiso.fechaCumpliCompromiso,
                    fechaModificado = gesCompromiso.fechaModificado,
                    fechaPropuestaEjec = gesCompromiso.fechaPropuestaEjec,
                    numActa = gesCompromiso.numActa,
                    observacion = gesCompromiso.observacion,
                    visita = gesCompromiso.visita
                };

                //Insertar Historico
                if (insertado)
                {
                    insertado = InsertCompromisosHistorico(historicoModel);
                }

            }

            return insertado;
        }

        public bool UpdateCompromisos(ActaSeguimCompromisosModel compromiso)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaCompromiso = (from g in db.ActaSeguimCompromisos
                                      where g.idActaCompromiso == compromiso.id
                                      select g).FirstOrDefault();

                if (compromiso.estado == "CERRADO")
                {
                    compromiso.fechaPropuestaEjec = actaCompromiso.fechaPropuestaEjec;
                }

                actaCompromiso.estado = compromiso.estado;
                actaCompromiso.fechaCumpliCompromiso = compromiso.fechaCumpliCompromiso;
                actaCompromiso.fechaModificado = compromiso.fechaModificado;
                actaCompromiso.fechaPropuestaEjec = compromiso.fechaPropuestaEjec;
                actaCompromiso.observacion = compromiso.observacion;
                actaCompromiso.numActa = compromiso.numActa;
                actaCompromiso.visita = compromiso.visita;

                db.SubmitChanges();

                ActaSeguimCompromisosHistoricoModel historicoModel = new ActaSeguimCompromisosHistoricoModel()
                {
                    idCompromiso = actaCompromiso.idActaCompromiso,
                    codConvocatoria = actaCompromiso.codConvocatoria,
                    codProyecto = actaCompromiso.codProyecto,
                    compromiso = actaCompromiso.compromiso,
                    estado = actaCompromiso.estado,
                    fechaCumpliCompromiso = actaCompromiso.fechaCumpliCompromiso,
                    fechaModificado = actaCompromiso.fechaModificado,
                    fechaPropuestaEjec = actaCompromiso.fechaPropuestaEjec,
                    numActa = actaCompromiso.numActa,
                    observacion = actaCompromiso.observacion,
                    visita = actaCompromiso.visita
                };

                insertado = InsertCompromisosHistorico(historicoModel);

            }

            return insertado;
        }

        private bool InsertCompromisosHistorico(ActaSeguimCompromisosHistoricoModel compromisoHis)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                ActaSeguimCompromisosHistorico gesCompromiso = new ActaSeguimCompromisosHistorico
                {
                    codConvocatoria = compromisoHis.codConvocatoria,
                    codProyecto = compromisoHis.codProyecto,
                    numActa = compromisoHis.numActa,
                    visita = compromisoHis.visita,
                    fechaModificado = compromisoHis.fechaModificado,
                    compromiso = compromisoHis.compromiso,
                    estado = compromisoHis.estado,
                    fechaCumpliCompromiso = compromisoHis.fechaCumpliCompromiso,
                    fechaPropuestaEjec = compromisoHis.fechaPropuestaEjec,
                    observacion = compromisoHis.observacion,
                    idCompromiso = compromisoHis.idCompromiso
                };

                db.ActaSeguimCompromisosHistorico.InsertOnSubmit(gesCompromiso);

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

        public List<ActaSeguimCompromisosHistoricoModel> getCompromisosHist(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimCompromisosHistoricoModel> list = new List<ActaSeguimCompromisosHistoricoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from e in db.ActaSeguimCompromisosHistorico
                        where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                        orderby e.idActaCompromisoHist
                        select new ActaSeguimCompromisosHistoricoModel
                        {
                            id = e.idActaCompromisoHist,
                            codProyecto = e.codProyecto,
                            codConvocatoria = e.codConvocatoria,
                            numActa = e.numActa,
                            visita = e.visita,
                            compromiso = e.compromiso,
                            estado = e.estado,
                            fechaCumpliCompromiso = e.fechaCumpliCompromiso,
                            fechaModificado = e.fechaModificado,
                            fechaPropuestaEjec = e.fechaPropuestaEjec,
                            idCompromiso = e.idCompromiso,
                            observacion = e.observacion
                        }).ToList();
            }

            return list;
        }

        public List<ActaSeguimCompromisosModel> getCompromisos(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimCompromisosModel> list = new List<ActaSeguimCompromisosModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from e in db.ActaSeguimCompromisos
                        where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                        && (e.estado.Equals("PENDIENTE") || e.estado.Contains("REPROGRAMADO"))
                        orderby e.idActaCompromiso
                        select new ActaSeguimCompromisosModel
                        {
                            id = e.idActaCompromiso,
                            codProyecto = e.codProyecto,
                            codConvocatoria = e.codConvocatoria,
                            numActa = e.numActa,
                            visita = e.visita,
                            compromiso = e.compromiso,
                            estado = e.estado,
                            fechaCumpliCompromiso = e.fechaCumpliCompromiso,
                            fechaModificado = e.fechaModificado,
                            fechaPropuestaEjec = e.fechaPropuestaEjec,
                            observacion = e.observacion
                        }).ToList();
            }

            return list;
        }

        public List<ActaSeguimCompromisosModel> getAllCompromisos(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimCompromisosModel> list = new List<ActaSeguimCompromisosModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from e in db.ActaSeguimCompromisos
                        where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria                       
                        orderby e.visita
                        select new ActaSeguimCompromisosModel
                        {
                            id = e.idActaCompromiso,
                            codProyecto = e.codProyecto,
                            codConvocatoria = e.codConvocatoria,
                            numActa = e.numActa,
                            visita = e.visita,
                            compromiso = e.compromiso,
                            estado = e.estado,
                            fechaCumpliCompromiso = e.fechaCumpliCompromiso,
                            fechaModificado = e.fechaModificado,
                            fechaPropuestaEjec = e.fechaPropuestaEjec,
                            observacion = e.observacion
                        }).ToList();
            }

            return list;
        }

        public ActaSeguimCompromisosModel getCompromisoByID(Int64 _idCompromiso)
        {
            ActaSeguimCompromisosModel list = new ActaSeguimCompromisosModel();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from e in db.ActaSeguimCompromisos
                        where e.idActaCompromiso == _idCompromiso
                        orderby e.idActaCompromiso
                        select new ActaSeguimCompromisosModel
                        {
                            id = e.idActaCompromiso,
                            codProyecto = e.codProyecto,
                            codConvocatoria = e.codConvocatoria,
                            numActa = e.numActa,
                            visita = e.visita,
                            compromiso = e.compromiso,
                            estado = e.estado,
                            fechaCumpliCompromiso = e.fechaCumpliCompromiso,
                            fechaModificado = e.fechaModificado,
                            fechaPropuestaEjec = e.fechaPropuestaEjec,
                            observacion = e.observacion
                        }).FirstOrDefault();
            }

            return list;
        }

        public bool EliminarCompromiso(Int64 _idCompromiso)
        {
            bool eliminado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var Compromiso =
                           from comp in db.ActaSeguimCompromisos
                           where comp.idActaCompromiso == _idCompromiso
                           select comp;

                foreach (var c in Compromiso)
                {
                    db.ActaSeguimCompromisos.DeleteOnSubmit(c);
                }

                try
                {
                    db.SubmitChanges();
                    eliminado = true;
                }
                catch (Exception)
                {
                    eliminado = false;
                }

            }
            return eliminado;
        }
    }
}
