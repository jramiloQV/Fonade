using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimEstadoEmpresaController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public List<ActaSeguimEstadoEmpresaModel> getListEstadoEmpresa(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimEstadoEmpresaModel> listEstado = new List<ActaSeguimEstadoEmpresaModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listEstado = (from e in db.ActaSeguimEstadoEmpresa
                                     where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                     orderby e.numActa
                                     select new ActaSeguimEstadoEmpresaModel
                                     {
                                         id = e.idActaEstadoEmpresa,
                                         codProyecto = e.codProyecto,
                                         codConvocatoria = e.codConvocatoria,
                                         numActa = e.numActa,
                                         visita = e.visita,
                                         Descripcion = e.Descripcion
                                     }).ToList();
            }

            return listEstado;
        }

        public bool InsertOrUpdateEstadoEmpresa(ActaSeguimEstadoEmpresaModel _estadoEmpresa)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                var actaEstadoEmpresa = (from g in db.ActaSeguimEstadoEmpresa
                                  where g.codConvocatoria == _estadoEmpresa.codConvocatoria
                                  && g.codProyecto == _estadoEmpresa.codProyecto
                                  && g.numActa == _estadoEmpresa.numActa
                                  select g).FirstOrDefault();

                if (actaEstadoEmpresa != null)//Actualizar
                {
                    actaEstadoEmpresa.Descripcion = _estadoEmpresa.Descripcion;
                    actaEstadoEmpresa.NBI = _estadoEmpresa.NBI;
                    actaEstadoEmpresa.FechaIngresado = DateTime.Now;
                }
                else//Insertar
                {
                    ActaSeguimEstadoEmpresa gesEstadoEmpresa = new ActaSeguimEstadoEmpresa
                    {                        
                        codConvocatoria = _estadoEmpresa.codConvocatoria,
                        codProyecto = _estadoEmpresa.codProyecto,                        
                        numActa = _estadoEmpresa.numActa,                       
                        FechaIngresado = DateTime.Now,
                        Descripcion = _estadoEmpresa.Descripcion,
                        visita = _estadoEmpresa.visita,
                        NBI = _estadoEmpresa.NBI 
                    };

                    db.ActaSeguimEstadoEmpresa.InsertOnSubmit(gesEstadoEmpresa);
                }

                db.SubmitChanges();

                insertado = true;
            }

            return insertado;
        }
    }
}
