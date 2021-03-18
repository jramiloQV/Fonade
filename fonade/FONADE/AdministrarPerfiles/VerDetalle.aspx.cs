using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.FONADE.AdministrarPerfiles
{
    /// <summary>
    /// VerDetalle
    /// </summary>    
    public partial class VerDetalle : Negocio.Base_Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["CodContacto"] != null)
                {

                    //"SELECT Id_TipoIdentificacion, NomTipoIdentificacion From TipoIdentificacion ORDER BY NomTipoIdentificacion"
                    // llenar opciondes del dropdown de t.ipo de identificación
                    var query3 = (from ti in consultas.Db.TipoIdentificacions
                                  select new
                                  {
                                      Id_TipoIdentificacion = ti.Id_TipoIdentificacion,
                                      NomTipoIdentificacion = ti.NomTipoIdentificacion,

                                  }
                                 );



                    //recibir los datos de los conttactos en cada control de asp mapeado

                    int codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
                    var query = (from c in consultas.Db.Contacto
                                 where c.Id_Contacto == codigoContacto
                                 select new
                                 {
                                     CodTipoIdentificacion = c.CodTipoIdentificacion,
                                     Identificacion = c.Identificacion,
                                     Nombres = c.Nombres,
                                     Apellidos = c.Apellidos,
                                     Email = c.Email,
                                     Experiencia = c.Experiencia,
                                     Dedicacion = c.Dedicacion,
                                     HojaVida = c.HojaVida,
                                     Intereses = c.Intereses
                                 }).FirstOrDefault();


                    tb_email.Text = query.Email;
                    tb_ExperienciaDocente.Text = query.Experiencia;
                    tb_experienciaIntereses.Text = query.Intereses;
                    tb_Resumenhv.Text = query.HojaVida;
                    if (query.Dedicacion != null)
                    {
                      if (query.Dedicacion.Trim() == "0")
                         tb_Dedicacion.Text = "Completa";
                    }
                    

                    var query2 = (from ce in consultas.Db.ContactoEstudios
                                  from cd in consultas.Db.Ciudad
                                  from ne in consultas.Db.NivelEstudios
                                  where ce.CodCiudad == cd.Id_Ciudad & ce.CodNivelEstudio == ne.Id_NivelEstudio &
                                         ce.CodContacto == codigoContacto
                                  select new
                                  {
                                      TituloObtenido = ce.TituloObtenido,
                                      FechaTitulo = ce.AnoTitulo,
                                      Institucion = ce.Institucion,
                                      Ciudad = cd.NomCiudad,
                                      NivelEstudio = ne.NomNivelEstudio

                                  }
                                     );

                    gw_InformacionAcademica.DataSource = query2;
                    gw_InformacionAcademica.DataBind();




                }


            }
        }
    }
}