using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class MetasVentasControllerDTO
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<MetasVentasModelDTO> ListMetasVentas(int _codProyecto, int _codConvocatoria)
        {
            List<MetasVentasModelDTO> listVentas = new List<MetasVentasModelDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                listVentas = (from r in db.IndicadorGestionEvaluacions
                                  where r.IdProyecto == _codProyecto
                                  && r.IdConvocatoria == _codConvocatoria
                                  select new MetasVentasModelDTO
                                  {
                                      id = r.Id,
                                      idConvocatoria = r.IdConvocatoria,
                                      idProyecto = r.IdProyecto,
                                      periodoImproductivo = r.PeriodoImproductivo,
                                      recursosAprobadosEmprendedor = r.RecursosAportadosEmprendedor,
                                      ventas = r.Ventas
                                  }).ToList();
            }

            return listVentas;
        }
    }
}
