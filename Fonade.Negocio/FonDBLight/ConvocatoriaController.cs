using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ConvocatoriaController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public ConvocatoriaModel GetConvocatoriaByProyecto(int _codProyecto)
        {
            ConvocatoriaModel convocatoria;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                convocatoria = (from CP in db.ConvocatoriaProyectos
                            join C in db.Convocatoria on CP.CodConvocatoria equals C.Id_Convocatoria
                            where CP.CodProyecto == _codProyecto
                            orderby C.Id_Convocatoria descending
                            select new ConvocatoriaModel
                            {
                                idConvocatoria = C.Id_Convocatoria,
                                Convocatoria = C.NomConvocatoria
                            }).FirstOrDefault();
            }

            return convocatoria;
        }
    }
}
