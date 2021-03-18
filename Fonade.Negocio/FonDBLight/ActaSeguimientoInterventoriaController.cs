using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimientoInterventoriaController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public ActaSeguimientoInterventoria GetActaInterventoria(int _codProyecto, int _numActa)
        {
             ActaSeguimientoInterventoria acta = new ActaSeguimientoInterventoria();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {                
                acta = (from a in db.ActaSeguimientoInterventoria
                        where a.IdProyecto == _codProyecto && a.NumeroActa == _numActa
                        select a
                        ).FirstOrDefault();
            }

                return acta;
        }
    }
}
