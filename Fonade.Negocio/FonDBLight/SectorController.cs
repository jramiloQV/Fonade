using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class SectorController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public SectorModel GetSectorByProyecto(int _codProyecto)
        {
            SectorModel sector;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                sector = (from p in db.Proyecto1s
                          join ss in db.SubSector on p.CodSubSector equals ss.Id_SubSector
                          join s in db.Sector on ss.CodSector equals s.Id_Sector
                            where p.Id_Proyecto == _codProyecto
                            orderby p.Id_Proyecto descending
                            select new SectorModel
                            {
                                idSector = s.Id_Sector,
                                Codigo = s.Codigo.ToString(),
                                Icono = s.Icono,
                                NomSector = s.NomSector,
                                NomSubSector = ss.NomSubSector
                            }).FirstOrDefault();
            }

            return sector;
        }
    }
}
