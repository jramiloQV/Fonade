using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimientoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public bool InsertActaSeguimiento(ref ActaSeguimientoModel acta, ref string Error)
        {
            bool insertado = false;
            try
            {
                int idProye = acta.idProyecto;
                int idtipoAct = acta.idTipoActa;
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(_cadena))
                {
                    ActaSeguimientoInterventoria actaI = new ActaSeguimientoInterventoria
                    {
                        Nombre = acta.Nombre,
                        IdTipoActa = acta.idTipoActa,
                        FechaCreacion = acta.FechaCreacion,
                        IdUsuarioCreacion = acta.idUsuarioCreacion,
                        Publicado = acta.Publicado,
                        IdProyecto = acta.idProyecto,
                        NumeroActa = acta.NumeroActa,
                        FechaPublicacion = acta.FechaPublicacion,
                        FechaFinalVisita = acta.FechaFinalVisita
                    };

                    db.ActaSeguimientoInterventoria.InsertOnSubmit(actaI);
                    db.SubmitChanges();
                    acta.idActa = actaI.Id;
                    //acta.FechaCreacion = actaI.FechaCreacion;
                    insertado = true;
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                insertado = false;
            }
            return insertado;
        }
    }
}
