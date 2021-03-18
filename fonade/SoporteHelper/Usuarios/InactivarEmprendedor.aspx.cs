using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Fonade.Clases;
using Datos;

namespace Fonade.SoporteHelper.Usuarios
{
    public partial class InactivarEmprendedor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbTipo.SelectedValue.Equals("email")) {
                    FieldValidate.ValidateString("Email", txtCodigo.Text, true, 500, true);
                }
                else {
                    FieldValidate.ValidateNumeric("Identificación", txtCodigo.Text,true);
                }
                                
                gvUsuarios.DataBind();                                        
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<Emprendedor> Get(string codigo, string tipo)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    if (tipo.Equals("email"))
                    {
                        FieldValidate.ValidateString("Email", codigo, true, 500, true);
                    }
                    else
                    {
                        FieldValidate.ValidateNumeric("Identificación", codigo, true);
                    }

                    if (tipo.Equals("email"))
                    {
                        return (from emprendedores in db.Contacto
                                        join grupos in db.GrupoContactos on emprendedores.Id_Contacto equals grupos.CodContacto
                                        where emprendedores.Email.Equals(codigo) 
                                              && grupos.CodGrupo.Equals(Constantes.CONST_Emprendedor)
                                        select new Emprendedor
                                        {
                                            Id = emprendedores.Id_Contacto,
                                            Nombre = emprendedores.Nombres,
                                            Apellido = emprendedores.Apellidos,
                                            Email = emprendedores.Email,
                                            Identificacion = emprendedores.Identificacion,
                                            Clave = emprendedores.Clave,
                                            Activo = !emprendedores.Inactivo
                                        }
                                        ).ToList();                                                                                              
                    }
                    else
                    {

                        return (from emprendedores in db.Contacto
                                join grupos in db.GrupoContactos on emprendedores.Id_Contacto equals grupos.CodContacto
                                where emprendedores.Identificacion.Equals(Convert.ToInt32(codigo))
                                      && grupos.CodGrupo.Equals(Constantes.CONST_Emprendedor)
                                select new Emprendedor
                                {
                                    Id = emprendedores.Id_Contacto,
                                    Nombre = emprendedores.Nombres,
                                    Apellido = emprendedores.Apellidos,
                                    Email = emprendedores.Email,
                                    Identificacion = emprendedores.Identificacion,
                                    Clave = emprendedores.Clave,
                                    Activo = !emprendedores.Inactivo
                                }
                                       ).ToList();
                    }
                }
                catch (ApplicationException)
                {
                    return new List<Emprendedor>();
                }
                catch (Exception)
                {
                    throw;                   
                }                
            }
        }

        protected void gvActividad_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Eliminar"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoContacto = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                        {
                            var entity = db.Contacto.SingleOrDefault(filter => filter.Id_Contacto.Equals(codigoContacto));
                            if (entity != null)
                                entity.Inactivo = true;

                            var entity2 = db.ProyectoContactos.SingleOrDefault(filter => filter.CodContacto.Equals(codigoContacto));
                            if(entity2 != null)
                            {
                                entity2.FechaFin = DateTime.Now;
                                entity2.Inactivo = true;
                            }

                            db.SubmitChanges();
                            gvUsuarios.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }
    public class Emprendedor
    {
        public int Id { get; set; }
        public double Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NombreCompleto {
            get {
                return Nombre + " " + Apellido;
            }
            set { }
        }
        public string Email { get; set; }
        public string Clave { get; set; }
        public bool Activo { get; set; }
        public int? CodigoProyecto {
            get {
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entity = db.ProyectoContactos.SingleOrDefault(unique => 
                                                                      unique.CodContacto.Equals(Id) 
                                                                      && unique.FechaFin == null 
                                                                      && unique.Inactivo.Equals(false)
                                                                      && unique.CodRol.Equals(Constantes.CONST_RolEmprendedor)
                                                                      );
                    if (entity == null)
                        return null;
                    else
                        return entity.CodProyecto;
                }
            }
            private set {
            }
        }
        public bool ActivoProyecto {
            get {
                return CodigoProyecto != null;
            }
            set {}
        }
        public bool AllowInactivar {
            get {
                return Activo || ActivoProyecto;
            }
            set {}
        }
    }
}