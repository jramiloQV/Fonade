using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class MetasEmpleoControllerDTO
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<MetasEmpleoModelDTO> ListMetasEmpleo(int _codProyecto, int _codConvocatoria, ref int MetaTotalEmpleos)
        {
            List<MetasEmpleoModelDTO> listMetas = new List<MetasEmpleoModelDTO>();

            int totalempleos = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from p in db.ProyectoGastosPersonals
                             join e in db.ProyectoEmpleoCargos on p.Id_Cargo equals e.CodCargo
                             join i in db.IndicadorCargoEvaluacions on p.Id_Cargo equals i.IdCargo
                             where p.CodProyecto == _codProyecto 
                             && i.IdConvocatoria == _codConvocatoria
                             && i.Unidades > 0
                             select new
                             {
                                 Unidades = i.Unidades,
                                 Cargo = p.Cargo,
                                 codCargo = e.CodCargo,
                                 Joven = e.Joven,
                                 Desmovilizado = e.Desmovilizado,
                                 Desplazado = e.Desplazado,
                                 Desvinculado = e.Desvinculado,
                                 Discapacitado = e.Discapacitado,
                                 Madre = e.Madre,
                                 Minoria = e.Minoria,
                                 Recluido = e.Recluido
                             }).ToList();

                foreach (var item in query)
                {
                    MetasEmpleoModelDTO meta = new MetasEmpleoModelDTO();
                    string condicion = "";
                    meta.Unidades = item.Unidades;
                    meta.Cargo = item.Cargo;
                    meta.codCargo = item.codCargo;
                    if (item.Joven)
                    {
                        condicion += " Jovenes,";
                    }
                    if (item.Desplazado)
                    {
                        condicion += " Desplazado por la violencia,";
                    }
                    if (item.Madre)
                    {
                        condicion += " Madre Cabeza de Familia,";
                    }
                    if (item.Minoria)
                    {
                        condicion += " Minoría Etnica (Indigena o Negritud),";
                    }
                    if (item.Recluido)
                    {
                        condicion += " Recluido Carceles INPEC,";
                    }
                    if (item.Desmovilizado)
                    {
                        condicion += " Desmovilizado o Reinsertado,";
                    }
                    if (item.Discapacitado)
                    {
                        condicion += " Discapacitado,";
                    }
                    if (item.Desvinculado)
                    {
                        condicion += " Desvinculado de Entidades del Estado,";
                    }

                    if (condicion.Equals(""))
                    {
                        condicion = "Sin Condición,";
                    }

                    condicion = condicion.TrimEnd(',');

                    meta.Condicion = condicion;

                    //sumamos la cantidad de empleos a generar para devolver el valor por referencia
                    totalempleos += meta.Unidades;

                    listMetas.Add(meta);

                }
            }

            MetaTotalEmpleos = totalempleos;

            return listMetas;
        }
    }
}
