using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class UsuarioController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public int GetIdContactoByEmail(string _Email)
        {
            int id = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                id = (from u in db.Contacto
                            where u.Email == _Email && u.Inactivo == false                            
                            orderby u.Id_Contacto descending
                            select u.Id_Contacto
                            ).FirstOrDefault();
            }

            return id;
        }
    }
}
