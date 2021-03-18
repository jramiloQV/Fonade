using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ddlCumplimientoController
    {
        public List<ddlCumplimientoModel> ddlSelEstadoActivo()
        {
            List<ddlCumplimientoModel> ddlList = new List<ddlCumplimientoModel>();

            ddlCumplimientoModel ddl0 = new ddlCumplimientoModel();
            ddl0.id = 0;
            ddl0.valor = "Seleccione";

            ddlCumplimientoModel ddl1 = new ddlCumplimientoModel();
            ddl1.id = 1;
            ddl1.valor = "SI/Mal Estado";

            ddlCumplimientoModel ddl2 = new ddlCumplimientoModel();
            ddl2.id = 2;
            ddl2.valor = "SI/Buen Estado";

            ddlCumplimientoModel ddl3 = new ddlCumplimientoModel();
            ddl3.id = 3;
            ddl3.valor = "NO";

            ddlCumplimientoModel ddl4 = new ddlCumplimientoModel();
            ddl4.id = 4;
            ddl4.valor = "No Aplica";

            ddlList.Add(ddl0);
            ddlList.Add(ddl1);
            ddlList.Add(ddl2);
            ddlList.Add(ddl3);
            ddlList.Add(ddl4);

            return ddlList;
        }

        public List<ddlCumplimientoModel> ddlSeleccionSINO()
        {
            List<ddlCumplimientoModel> ddlList = new List<ddlCumplimientoModel>();

            ddlCumplimientoModel ddl0 = new ddlCumplimientoModel();
            ddl0.id = 0;
            ddl0.valor = "Seleccione";

            ddlCumplimientoModel ddl1 = new ddlCumplimientoModel();
            ddl1.id = 1;
            ddl1.valor = "SI";

            ddlCumplimientoModel ddl2 = new ddlCumplimientoModel();
            ddl2.id = 2;
            ddl2.valor = "NO";

            ddlList.Add(ddl0);
            ddlList.Add(ddl1);
            ddlList.Add(ddl2);

            return ddlList;
        }

        public List<ddlCumplimientoModel> ddlDatosCumplimiento()
        {
            List<ddlCumplimientoModel> ddlList = new List<ddlCumplimientoModel>();

            ddlCumplimientoModel ddl0 = new ddlCumplimientoModel
            {
                id = 0,
                valor = "Seleccione"
            };

            ddlCumplimientoModel ddl1 = new ddlCumplimientoModel
            {
                id = 1,
                valor = "Cumple"
            };

            ddlCumplimientoModel ddl2 = new ddlCumplimientoModel
            {
                id = 2,
                valor = "No Cumple"
            };

            ddlCumplimientoModel ddl3 = new ddlCumplimientoModel
            {
                id = 3,
                valor = "No Aplica"
            };
            
            ddlList.Add(ddl0);
            ddlList.Add(ddl1);
            ddlList.Add(ddl2);
            ddlList.Add(ddl3);

            return ddlList;
        }

        public List<ddlCumplimientoModel> opcionesCompromiso()
        {
            List<ddlCumplimientoModel> ddlList = new List<ddlCumplimientoModel>();

            ddlCumplimientoModel ddl0 = new ddlCumplimientoModel();
            ddl0.id = 0;
            ddl0.valor = "Seleccione";

            ddlCumplimientoModel ddl1 = new ddlCumplimientoModel();
            ddl1.id = 1;
            ddl1.valor = "CERRADO";

            ddlCumplimientoModel ddl2 = new ddlCumplimientoModel();
            ddl2.id = 2;
            ddl2.valor = "REPROGRAMADO POR INCUMPLIMIENTO";

            ddlList.Add(ddl0);
            ddlList.Add(ddl1);
            ddlList.Add(ddl2);

            return ddlList;
        }

        public List<ddlCumplimientoModel> ddlDatosOtrosAspectos()
        {
            List<ddlCumplimientoModel> ddlList = new List<ddlCumplimientoModel>();

            ddlCumplimientoModel ddl0 = new ddlCumplimientoModel();
            ddl0.id = 0;
            ddl0.valor = "Seleccione";

            ddlCumplimientoModel ddl1 = new ddlCumplimientoModel();
            ddl1.id = 1;
            ddl1.valor = "SI";

            ddlCumplimientoModel ddl2 = new ddlCumplimientoModel();
            ddl2.id = 2;
            ddl2.valor = "NO";

            ddlCumplimientoModel ddl3 = new ddlCumplimientoModel();
            ddl3.id = 3;
            ddl3.valor = "PARCIAL";

            ddlList.Add(ddl0);
            ddlList.Add(ddl1);
            ddlList.Add(ddl2);
            ddlList.Add(ddl3);

            return ddlList;
        }
    }
}
