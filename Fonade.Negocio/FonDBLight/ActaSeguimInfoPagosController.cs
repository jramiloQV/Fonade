using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimInfoPagosController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private const int constEstadoPago = 4; // Aprobado

        public List<ActaSeguimInfoPagosModel> getInfoPagos(int _codProyecto, int _codConvocatoria, ref decimal pagosAnteriores)
        {
            List<ActaSeguimInfoPagosModel> listInfoPago = new List<ActaSeguimInfoPagosModel>();
            pagosAnteriores = 0;
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listInfoPago = (from l in db.ActaSeguimInfoPagos
                                where l.codProyecto == _codProyecto
                                && l.codConvocatoria == _codConvocatoria
                                select new ActaSeguimInfoPagosModel
                                {
                                    Actividad = l.Actividad,
                                    codConvocatoria = l.codConvocatoria,
                                    codigoPago = l.codigoPago,
                                    codProyecto = l.codProyecto,
                                    Concepto = l.Concepto,
                                    id = l.idActaInfoPago,
                                    idPagoActividad = l.idPagoActividad,
                                    numActa = l.numActa,
                                    Observacion = l.Observacion,
                                    Valor = l.Valor,
                                    verificoActivosEstado = l.verificoActivosEstado,
                                    verificoDocumentos = l.verificoDocumentos,
                                    visita = l.visita
                                }).ToList();
            }

            pagosAnteriores = listInfoPago.Sum(x => x.Valor);

            List<ActaSeguimInfoPagosModel> lstLeftJoin = new List<ActaSeguimInfoPagosModel>();
                        
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = db.GetPagoActSinRegistrarActa(_codProyecto).ToList();

                foreach (var i in query)
                {
                    ActaSeguimInfoPagosModel acta = new ActaSeguimInfoPagosModel();
                    acta.idPagoActividad = i.Id_PagoActividad;
                    acta.Actividad = i.NomPagoActividad;
                    acta.codigoPago = i.codigopago;
                    acta.Valor = i.CantidadDinero.HasValue ? i.CantidadDinero.Value : 0;
                    acta.Concepto = i.NomPagoConcepto;

                    lstLeftJoin.Add(acta);
                }

            }
            return lstLeftJoin;
        }

        public bool insertPagoInfo(ActaSeguimInfoPagosModel infoPago)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                ActaSeguimInfoPagos gesInfoPago = new ActaSeguimInfoPagos
                {
                    Actividad = infoPago.Actividad,
                    codConvocatoria = infoPago.codConvocatoria,
                    codigoPago = infoPago.codigoPago,
                    codProyecto = infoPago.codProyecto,
                    Concepto = infoPago.Concepto,
                    fechaModificado = DateTime.Now,
                    idPagoActividad = infoPago.idPagoActividad,
                    numActa = infoPago.numActa,
                    Observacion = infoPago.Observacion,
                    Valor = infoPago.Valor,
                    verificoActivosEstado = infoPago.verificoActivosEstado,
                    verificoDocumentos = infoPago.verificoDocumentos,
                    visita = infoPago.visita
                };

                db.ActaSeguimInfoPagos.InsertOnSubmit(gesInfoPago);

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

        public List<ActaSeguimInfoPagosModel> getHistoricoEjecucion(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimInfoPagosModel> list = new List<ActaSeguimInfoPagosModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from a in db.ActaSeguimInfoPagos
                        where a.codProyecto == _codProyecto
                        && a.codConvocatoria == _codConvocatoria
                        orderby a.idActaInfoPago
                        select new ActaSeguimInfoPagosModel
                        {
                            Actividad = a.Actividad,
                            codConvocatoria = a.codConvocatoria,
                            codigoPago = a.codigoPago,
                            codProyecto = a.codProyecto,
                            Concepto = a.Concepto,
                            fechaModificado = a.fechaModificado,
                            id = a.idActaInfoPago,
                            idPagoActividad = a.idPagoActividad,
                            numActa = a.numActa,
                            Observacion = a.Observacion,
                            Valor = a.Valor,
                            verificoActivosEstado = a.verificoActivosEstado,
                            verificoDocumentos = a.verificoDocumentos,
                            visita = a.visita
                        }).ToList();
            }

            return list;
        }

        public List<ActaSeguimInventarioContratoModel> getInventarioContrato(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimInventarioContratoModel> list = new List<ActaSeguimInventarioContratoModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from a in db.ActaSeguimInventarioContrato
                        where a.codProyecto == _codProyecto
                        && a.codConvocatoria == _codConvocatoria
                        orderby a.idActaInventContrato
                        select new ActaSeguimInventarioContratoModel
                        {                            
                            codConvocatoria = a.codConvocatoria,                         
                            codProyecto = a.codProyecto,                            
                            fechaModificado = a.fechaModificado,
                            id = a.idActaInventContrato,                            
                            numActa = a.numActa,                            
                            visita = a.visita,
                            descripcionRecursos = a.descripcionRecursos,
                            fechaCargaAnexo = a.fechaCargaAnexo,
                            valorActivos = a.valorActivos
                        }).ToList();
            }

            return list;
        }

        public bool InsertOrUpdateInventario(ActaSeguimInventarioContratoModel _inventarioModel)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaInventario = (from g in db.ActaSeguimInventarioContrato
                                         where g.codConvocatoria == _inventarioModel.codConvocatoria
                                         && g.codProyecto == _inventarioModel.codProyecto
                                         && g.numActa == _inventarioModel.numActa
                                         select g).FirstOrDefault();

                if (actaInventario != null)//Actualizar
                {
                    actaInventario.descripcionRecursos = _inventarioModel.descripcionRecursos;
                    actaInventario.fechaCargaAnexo = _inventarioModel.fechaCargaAnexo;
                    actaInventario.valorActivos = _inventarioModel.valorActivos;
                    actaInventario.fechaModificado = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimInventarioContrato gesInventario = new ActaSeguimInventarioContrato
                    {
                        codConvocatoria = _inventarioModel.codConvocatoria,
                        codProyecto = _inventarioModel.codProyecto,
                        numActa = _inventarioModel.numActa,
                        fechaModificado = DateTime.Now,
                        descripcionRecursos = _inventarioModel.descripcionRecursos,
                        visita = _inventarioModel.visita,
                        fechaCargaAnexo = _inventarioModel.fechaCargaAnexo,
                        valorActivos = _inventarioModel.valorActivos
                    };

                    db.ActaSeguimInventarioContrato.InsertOnSubmit(gesInventario);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }

        public string AporteEmpRecomendado(int _codProyecto, int _codConvocatoria)
        {
            string Aporte = "";
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var entity = (from indicadores in db.IndicadorGestionEvaluacions
                              where indicadores.IdProyecto == _codProyecto
                                    && indicadores.IdConvocatoria == _codConvocatoria
                              select indicadores).FirstOrDefault();

                Aporte = entity.RecursosAportadosEmprendedor;
            }

            return Aporte;
        }

        public List<ActaSeguimAporteEmprendedorModel> getAporteEmp(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimAporteEmprendedorModel> list = new List<ActaSeguimAporteEmprendedorModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                list = (from a in db.ActaSeguimAporteEmprendedor
                        where a.codProyecto == _codProyecto
                        && a.codConvocatoria == _codConvocatoria
                        orderby a.idActaAporteEmp
                        select new ActaSeguimAporteEmprendedorModel
                        {
                            codConvocatoria = a.codConvocatoria,
                            codProyecto = a.codProyecto,
                            fechaModificado = a.fechaModificado,
                            id = a.idActaAporteEmp,
                            numActa = a.numActa,
                            visita = a.visita,
                            descripcion = a.descripcion
                        }).ToList();
            }

            return list;
        }

        public bool InsertOrUpdateAporteEmp(ActaSeguimAporteEmprendedorModel _Model)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaAporte = (from g in db.ActaSeguimAporteEmprendedor
                                      where g.codConvocatoria == _Model.codConvocatoria
                                      && g.codProyecto == _Model.codProyecto
                                      && g.numActa == _Model.numActa
                                      select g).FirstOrDefault();

                if (actaAporte != null)//Actualizar
                {
                    actaAporte.descripcion = _Model.descripcion;
                    actaAporte.metaEmprendedor = _Model.metaEmprendedor;
                    actaAporte.fechaModificado = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimAporteEmprendedor gesAporte = new ActaSeguimAporteEmprendedor
                    {
                        codConvocatoria = _Model.codConvocatoria,
                        codProyecto = _Model.codProyecto,
                        numActa = _Model.numActa,
                        fechaModificado = DateTime.Now,                       
                        visita = _Model.visita,
                        descripcion = _Model.descripcion,
                        metaEmprendedor = _Model.metaEmprendedor
                    };

                    db.ActaSeguimAporteEmprendedor.InsertOnSubmit(gesAporte);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }
    }
}
