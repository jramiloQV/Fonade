using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Clases;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// Clase para adicionar prorroga a proyectos.
    /// Author : Marcel Solera
    /// Ultima modificación : Optimización de codigo.
    /// </summary>
    public partial class AdicionarProrroga : Negocio.Base_Page
    {
        Int64? CodigoProyecto;
        /// <summary>
        /// The nombre proyecto
        /// </summary>
        public string NombreProyecto;

        /// <summary>
        /// Pages the load.
        /// </summary>
        protected void Page_Load()
        {
            try
            {
                CodigoProyecto = FieldValidate.GetSessionInt("CodigoProyecto");
                NombreProyecto = FieldValidate.GetSessionString("NombreProyecto");

                //Si existen las variables de sessión mostramos el nombre del proyecto
                //en la caja correspondiente.
                if (CodigoProyecto.Equals(null) || NombreProyecto.Equals(String.Empty))
                {
                    txtNombreProyecto.Text = string.Empty;
                }
                else
                {
                    txtNombreProyecto.Text = NombreProyecto;    
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, intentelo de nuevo. detalle : " + ex.Message + " ');", true);
            }
        }

        /// <summary>
        /// BTNs the adicionar prorroga event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnAdicionarProrrogaEvent(object sender, EventArgs e)
        {
            try
            {
                //Validadores
                FieldValidate.ValidateString("Nombre de proyecto", txtNombreProyecto.Text, true);
                FieldValidate.ValidateNumeric("CodigoProyecto", CodigoProyecto.ToString(), true);
                FieldValidate.ValidateNumeric("Meses de prorroga", txtMesesProrroga.Text, true);

                Int64 MesesProrroga = Convert.ToInt64(txtMesesProrroga.Text);

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    Datos.ProyectoProrroga prorroga = db.ProyectoProrrogas.FirstOrDefault(_prorroga => _prorroga.CodProyecto == CodigoProyecto);

                    if( prorroga == null ) 
                    {
                        Datos.ProyectoProrroga nuevaProrroga = new  Datos.ProyectoProrroga()
                                                                        { 
                                                                            CodProyecto = (Int32)CodigoProyecto,
                                                                            Prorroga = (Int32)MesesProrroga
                                                                        };
                        db.ProyectoProrrogas.InsertOnSubmit(nuevaProrroga);
                        db.SubmitChanges();
                    }
                    else
                    {
                        prorroga.Prorroga = prorroga.Prorroga + (Int32)MesesProrroga;

                        db.SubmitChanges();
                    }

                    btnVolverEvent(null, null);
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error, detalle : " + ex.Message + " ');", true);
            }

        }

        /// <summary>
        /// Evento Volver y limpiar variables de sessión.
        /// </summary>
        protected void btnVolverEvent(object sender, EventArgs e)
        {
            HttpContext.Current.Session["CodigoProyecto"] = null;
            HttpContext.Current.Session["NombreProyecto"] = null;

            Response.Redirect("~/FONADE/Administracion/ProyectoProrroga.aspx",true);
        }

        /// <summary>
        /// Evento Buscar proyectos
        /// </summary>
        protected void btnBuscarProyectoEvent(object sender, ImageClickEventArgs e)
        {
            Redirect(null, "BuscarProyecto.aspx", "_Blank", "width=730,height=585");
        }
    
    }
}