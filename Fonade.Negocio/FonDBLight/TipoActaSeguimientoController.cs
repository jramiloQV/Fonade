using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class TipoActaSeguimientoController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public TipoActaSeguimientoModel GetTipoActaByID(int _idTipoActa)
        {
            TipoActaSeguimientoModel tipoActa;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                tipoActa = (from a in db.TipoActaSeguimientos
                            where a.Id == _idTipoActa
                            select new TipoActaSeguimientoModel
                            {
                               idActa = a.Id,
                               Codigo = a.Codigo,
                               Tipo = a.Tipo,
                               Version = a.Version,
                               Vigencia = a.Vigencia
                            }).FirstOrDefault();
            }

            return tipoActa;
        }
    }
}
