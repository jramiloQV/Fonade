using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Datos.DataType
{
    public class UsuarioFonade
    {
        public UsuarioFonade(string nombres, string apellidos, string email, int codGrupo, int idContacto,
                             int codInstitucion, double identificacion, DateTime fechaCreacion, DateTime fechaCambioClave, string clave
                                , Boolean aceptoTerminosYCondiciones, int? _codOperador)
        {
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.email = email;
            this.codGrupo = codGrupo;
            this.idContacto = idContacto;
            this.codInstitucion = codInstitucion;
            this.identificacion = identificacion;
            this.fechaCreacion = fechaCreacion;
            this.fechaCambioClave = fechaCambioClave;
            this.clave = clave;
            this.AceptoTerminosYCondiciones = aceptoTerminosYCondiciones;
            this.CodOperador = _codOperador;
        }

        private int? codOperador;

        public int? CodOperador
        {
            get { return codOperador; }
            set { codOperador = value; }
        }

        private string nombres;

        public string Nombres
        {
            get { return nombres; }
            set { nombres = value; }
        }

        private string apellidos;

        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string clave;

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }
        
        private int codGrupo;

        public int CodGrupo
        {
            get { return codGrupo; }
            set { codGrupo = value; }
        }

        private int idContacto;

        public int IdContacto
        {
            get { return idContacto; }
            set { idContacto = value; }
        }

        private int codInstitucion;
    
        public int CodInstitucion
        {
            get { return codInstitucion; }
            set { codInstitucion = value; }
        }

        private double identificacion;

        public double Identificacion
        {
            get { return identificacion; }
            set { identificacion = value; }
        }

        private DateTime fechaCreacion;

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        private DateTime fechaCambioClave;

        public DateTime FechaCambioClave
        {
            get { return fechaCambioClave; }
            set { fechaCambioClave = value; }
        }

        private Boolean aceptoTerminosYCondiciones;
        public Boolean AceptoTerminosYCondiciones
        {
            get
            {
                return aceptoTerminosYCondiciones;
            }
            set
            {
                aceptoTerminosYCondiciones = value;
            }
        }
    }
}
