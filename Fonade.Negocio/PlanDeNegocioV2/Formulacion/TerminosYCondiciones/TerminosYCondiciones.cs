using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Datos;
using Datos.DataType;

namespace Fonade.Negocio.PlanDeNegocioV2.Formulacion.TerminosYCondiciones
{
    public static class TerminosYCondiciones
    {
        public static void Update(int CodigoContacto, bool aceptoTerminosYCondiciones)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = db.Contacto.Single(filter => filter.Id_Contacto.Equals(CodigoContacto));

                currentEntity.AceptoTerminosYCondiciones = aceptoTerminosYCondiciones;

                db.SubmitChanges();
            }
        }

        public static void InsertOrUpdate(int codigoContacto,int codigoProyecto, string rutaArchivo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var currentEntity = db.ContactoArchivosAnexos.FirstOrDefault(filter => filter.CodContacto.Equals(codigoContacto) && filter.TipoArchivo.Equals("TerminosYCondiciones"));

                if (currentEntity == null) {
                    var documento = new ContactoArchivosAnexos
                    {
                        CodContacto = codigoContacto,
                        CodProyecto = codigoProyecto,
                        NombreArchivo = "TerminosYCondiciones.pdf",
                        TipoArchivo = "TerminosYCondiciones",
                        ruta = rutaArchivo,
                        CodContactoEstudio = null                        
                    };

                    db.ContactoArchivosAnexos.InsertOnSubmit(documento);
                }
                else
                {
                    currentEntity.CodProyecto = codigoProyecto;
                    currentEntity.ruta = rutaArchivo; 
                }
                                
                db.SubmitChanges();
            }
        }
    }
}
