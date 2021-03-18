using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;


namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.Solucion
{
    public partial class Solucion
    {
        #region SOLUCION
        public static bool Insertar(ProyectoSolucion entSolucion, out string msg)
        {
            try
            {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entSoluciondb = (from row in db.ProyectoSolucions
                                         where row.IdProyecto == entSolucion.IdProyecto
                                         select row).FirstOrDefault();

                    //insert-update
                    if (entSoluciondb == null)
                        db.ProyectoSolucions.InsertOnSubmit(entSolucion);
                    else
                    {
                        entSoluciondb.AceptacionProyecto = entSolucion.AceptacionProyecto;
                        entSoluciondb.Comercial = entSolucion.Comercial;
                        entSoluciondb.ConceptoNegocio = entSolucion.ConceptoNegocio;
                        entSoluciondb.InnovadorConceptoNegocio = entSolucion.InnovadorConceptoNegocio;
                        entSoluciondb.Legal = entSolucion.Legal;
                        entSoluciondb.Proceso = entSolucion.Proceso;
                        entSoluciondb.ProductoServicio = entSolucion.ProductoServicio;
                        entSoluciondb.TecnicoProductivo = entSolucion.TecnicoProductivo;
                    }
                    db.SubmitChanges();

                    msg = Mensajes.Mensajes.GetMensaje(8);
                    return true;

                }
            }
            catch (Exception ex)
            {
                //todo guardar log
                msg = Mensajes.Mensajes.GetMensaje(7);
                return false;
            }
        }

        public static ProyectoSolucion Get(int IdProyecto)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var entSoluciondb = (from row in db.ProyectoSolucions
                                     where row.IdProyecto == IdProyecto
                                     select row).FirstOrDefault();

                return entSoluciondb;
            }
        }

        #endregion




    }
}
