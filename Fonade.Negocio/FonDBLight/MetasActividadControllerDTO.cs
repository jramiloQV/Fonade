using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class MetasActividadControllerDTO
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<MetasActividadModelDTO> ListMetasMercadeo(int _codProyecto
                                                , int _codConvocatoria, ref int MetaTotalActividades)
        {
            List<MetasActividadModelDTO> listMetas = new List<MetasActividadModelDTO>();

            int totalActividades = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                listMetas = (from p in db.ProyectoEstrategiaActividades
                             join i in db.IndicadorMercadeoEvaluacions
                             on p.IdActividad equals i.IdActividadMercadeo
                             where i.IdProyecto == _codProyecto
                             && i.IdConvocatoria == _codConvocatoria
                             && i.Unidades > 0
                             select new MetasActividadModelDTO
                             {
                                 idActividad = i.IdActividadMercadeo,
                                 Unidades = i.Unidades,
                                 Actividad = p.Actividad
                             }).ToList();

                if (listMetas.Count() > 0)
                {
                    totalActividades = listMetas.Sum(x => x.Unidades);
                }
            }

            MetaTotalActividades = totalActividades;

            return listMetas;
        }

        public List<MetasActividadModelDTO> ListMetasMercadeoInterventoria(int _codProyecto
                                               , int _codConvocatoria, ref int MetaTotalActividades)
        {
            List<MetasActividadModelDTO> listMetas = new List<MetasActividadModelDTO>();

            int totalActividades = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listMetas = (from p in db.ActaSeguimGestionMercadeoEvaluacion
                             where p.codProyecto == _codProyecto && p.ocultar == false
                             select new MetasActividadModelDTO
                             {
                                 idActividadInterventoria = p.idActividadInterventoria,
                                 idActividad = p.idActividad,
                                 Unidades = p.unidades,
                                 Actividad = p.actividad
                             }).ToList();

                if (listMetas.Count() > 0)
                {
                    totalActividades = listMetas.Sum(x => x.Unidades);
                }
            }

            MetaTotalActividades = totalActividades;

            return listMetas;
        }

        public bool ocultarMetaMercadeo(int _idMetaMercadeo, int _codContacto)
        {
            bool oculto = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = (from a in db.ActaSeguimGestionMercadeoEvaluacion
                             where a.idActividadInterventoria == _idMetaMercadeo
                             select a).FirstOrDefault();

                query.ocultar = true;
                query.codContacto = _codContacto;
                query.fechaUltimaActualizacion = DateTime.Now;

                db.SubmitChanges();

                oculto = true;
            }

            return oculto;
        }

        public bool actualizarMetaMercadeo(int _idMetaMercadeo, int _codContacto, int _cantidad, string _actividad)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = (from a in db.ActaSeguimGestionMercadeoEvaluacion
                             where a.idActividadInterventoria == _idMetaMercadeo
                             select a).FirstOrDefault();

                query.actividad = _actividad;
                query.unidades = _cantidad;
                query.codContacto = _codContacto;
                query.fechaUltimaActualizacion = DateTime.Now;

                db.SubmitChanges();

                actualizado = true;
            }

            return actualizado;
        }

        public bool insertarMercadeo(ActaSeguimGestionMercadeoEvaluacion mercadeo)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                db.ActaSeguimGestionMercadeoEvaluacion.InsertOnSubmit(mercadeo);
                db.SubmitChanges();

                insertado = true;
            }

                return insertado;
        }

        public void copiarInformacionMercadeo(List<MetasActividadModelDTO> metas, int _codContacto, int _codProyecto)
        {
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                List<ActaSeguimGestionMercadeoEvaluacion> mercadeoEvaluacions = new List<ActaSeguimGestionMercadeoEvaluacion>();

                foreach (MetasActividadModelDTO m in metas)
                {
                    var cant = (from a in db.ActaSeguimGestionMercadeoEvaluacion
                                where a.idActividad == m.idActividad
                                select a).Count();
                    if (cant == 0)
                    {
                        ActaSeguimGestionMercadeoEvaluacion mercadeoEval = new ActaSeguimGestionMercadeoEvaluacion
                        {
                            actividad = m.Actividad,
                            codContacto = _codContacto,
                            codProyecto = _codProyecto,
                            fechaUltimaActualizacion = DateTime.Now,
                            idActividad = m.idActividad,
                            ocultar = false,
                            unidades = m.Unidades
                        };

                        mercadeoEvaluacions.Add(mercadeoEval);
                    }

                }

                db.ActaSeguimGestionMercadeoEvaluacion.InsertAllOnSubmit(mercadeoEvaluacions);
                db.SubmitChanges();
            }
        }
    }
}
