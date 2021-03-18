using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class EmpresaController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public EmpresaModel GetEmpresaByProyecto(int _codProyecto)
        {
            EmpresaModel proyecto;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                proyecto = (from e in db.Empresas
                            where e.codproyecto == _codProyecto
                            orderby e.id_empresa descending
                            select new EmpresaModel
                            {
                                idEmpresa = e.id_empresa,
                                codProyecto = e.codproyecto.Value,
                                nit = e.Nit,
                                razonSocial = e.razonsocial,
                                Direccion = e.DomicilioEmpresa
                            }).FirstOrDefault();
            }

            return proyecto;
        }
    }
}
