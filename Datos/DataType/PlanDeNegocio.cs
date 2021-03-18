using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    public class PlanDeNegocio
    {
        int idProyecto;

        public int IdProyecto
        {
            get { return idProyecto; }
            set { idProyecto = value; }
        }

        string nombreProyecto;

        public string NombreProyecto
        {
            get { return nombreProyecto; }
            set { nombreProyecto = value; }
        }

        int codigoInstitucion;

        public int CodigoInstitucion
        {
            get { return codigoInstitucion; }
            set { codigoInstitucion = value; }
        }

        int codigoEstado;

        public int CodigoEstado
        {
            get { return codigoEstado; }
            set { codigoEstado = value; }
        }

        string nombreUnidad;

        public string NombreUnidad
        {
            get { return nombreUnidad; }
            set { nombreUnidad = value; }
        }

        string nombreInstitucion;

        public string NombreInstitucion
        {
            get { return nombreInstitucion; }
            set { nombreInstitucion = value; }
        }

        string nombreCiudad;

        public string NombreCiudad
        {
            get { return nombreCiudad; }
            set { nombreCiudad = value; }
        }

        string nombreDepartamento;

        public string NombreDepartamento
        {
            get { return nombreDepartamento; }
            set { nombreDepartamento = value; }
        }

        bool inactivo;

        public bool Inactivo
        {
            get { return inactivo; }
            set { inactivo = value; }
        }
    }
}
