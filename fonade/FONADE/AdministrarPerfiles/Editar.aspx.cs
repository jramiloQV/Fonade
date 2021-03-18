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
    /// Editar
    /// </summary>    
    public partial class Editar : Negocio.Base_Page
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
                    ddl_TipoIdentificacion.DataTextField = "NomTipoIdentificacion";
                    ddl_TipoIdentificacion.DataValueField = "Id_TipoIdentificacion";

                    ddl_TipoIdentificacion.DataSource = query3;
                    ddl_TipoIdentificacion.DataBind();


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
                    tb_EmailAsesor.Text = query.Email;
                    tb_ExperienciaDocente.Text = query.Experiencia;
                    tb_ExperienciaIntereses.Text = query.Experiencia;
                    tb_NombreAsesor.Text = query.Nombres;
                    tb_ApellidosAsesor.Text = query.Apellidos;
                    tb_NumeroIdentificacion.Text = query.Identificacion.ToString();
                    tb_ResumenHojaVida.Text = query.HojaVida;
                    ddl_TipoIdentificacion.Text = query.CodTipoIdentificacion.ToString();

                    string obcDedi = query.Dedicacion;
                    try { if (obcDedi != "" || obcDedi != null) { obcDedi = obcDedi.Split(' ')[0]; } }
                    catch { obcDedi = ""; }

                    ddl_DedicacionUnidad.SelectedValue = obcDedi;

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

                    //       "SELECT CE.TituloObtenido, CE.AnoTitulo, CE.Institucion, C.NomCiudad, NE.NomNivelEstudio" & _
                    //" FROM ContactoEstudio CE, Ciudad C, NivelEstudio NE" & _
                    //" WHERE CE.CodCiudad = C.ID_Ciudad" & _
                    //" AND CE.CodNivelEstudio = NE.Id_NivelEstudio" & _
                    //" AND codcontacto = " & CodContacto
                }
                //   SELECT CodTipoIdentificacion, Identificacion, Nombres, Apellidos, Email, Experiencia, Dedicacion, HojaVida, Intereses FROM Contacto " & _
                //" WHERE Id_contacto = " & CodContacto & _
                //" AND CodInstitucion = " & Session("CodInstitucion")


            }


        }

        /// <summary>
        /// Handles the onclick event of the btn_modificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_modificar_onclick(object sender, EventArgs e)
        {
            if (Request.QueryString["CodContacto"] != null)
            {
                int codigoContacto = Int32.Parse(Request.QueryString["CodContacto"]);
                var query = (from c in consultas.Db.Contacto
                             where c.Id_Contacto == codigoContacto
                             select c);
                foreach (Contacto c in query)
                {
                    c.Email = tb_EmailAsesor.Text;
                    c.Experiencia = tb_ExperienciaDocente.Text;
                    c.Experiencia = tb_ExperienciaIntereses.Text;
                    c.Nombres = tb_NombreAsesor.Text;
                    c.Apellidos = tb_ApellidosAsesor.Text;
                    c.Identificacion = Int32.Parse(tb_NumeroIdentificacion.Text);
                    c.HojaVida = tb_ResumenHojaVida.Text;
                    c.CodTipoIdentificacion = short.Parse(ddl_TipoIdentificacion.Text);
                    c.Dedicacion = ddl_DedicacionUnidad.SelectedValue;
                }

                // Submit the changes to the database. 
                try
                {
                    consultas.Db.SubmitChanges();
                }
                catch (Exception)
                {
                    //Console.WriteLine(e);
                    // Provide for exceptions.
                }

            }

            //UPDATE Contacto SET " & _
            //     " Nombres = '"& txtNombres &"'," & _
            //     " Apellidos = '"& txtApellidos &"'," & _
            //     " CodTipoIdentificacion = " & CodTipoIdentificacion & "," & _
            //     " Identificacion = " & numIdentificacion & "," & _
            //     " Email = '"& txtEmail &"'" & _
            //     " WHERE Id_Contacto = " & CodContacto

            Response.Redirect("AdministrarAsesores.aspx");
        }



    }
}