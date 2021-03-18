using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ConvenioController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public ConvenioModel GetConvenioByProyecto(int _codProyecto)
        {
            ConvenioModel convenio;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                convenio = (from CP in db.ConvocatoriaProyectos
                            join C in db.Convocatoria on CP.CodConvocatoria equals C.Id_Convocatoria
                            join CN in db.Convenios on C.CodConvenio equals CN.Id_Convenio
                            where CP.CodProyecto == _codProyecto
                            orderby C.Id_Convocatoria descending
                            select new ConvenioModel
                            {
                               idConvenio = CN.Id_Convenio,
                               NomConveio = CN.Nomconvenio,
                               FechaInicio = CN.Fechainicio.Value,
                               Fechafin = CN.FechaFin.Value,
                               Descripcion = CN.Descripcion,
                               codContactoFiduciaria = CN.CodcontactoFiduciaria.Value
                            }).FirstOrDefault();
            }

            return convenio;
        }
    }
}
