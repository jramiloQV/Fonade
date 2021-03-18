using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ContratoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public ContratoModel GetContratoByEmpresa(int _idEmpresa)
        {
            ContratoModel contrato;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                contrato = (from CE in db.ContratoEntidads
                            join EI in db.EmpresaInterventors on CE.Id equals EI.IdContratoInterventoria
                            where EI.CodEmpresa == _idEmpresa
                            orderby CE.Id descending
                            select new ContratoModel
                            {
                               NumeroContrato = CE.NumeroContrato
                            }).FirstOrDefault();
            }

            return contrato;
        }
    }
}
