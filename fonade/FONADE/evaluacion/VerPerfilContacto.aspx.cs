using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;

namespace Fonade.FONADE.evaluacion
{
    public partial class VerPerfilContacto : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Contacto seleccionado en "EvaluacionActa.aspx".
        /// </summary>
        String codcontacto;

        /// <summary>
        /// Código del rol "según "EvaluacionActa.aspx" SIEMPRE tiene el valor de la constante.
        /// </summary>
        Int32 CodRol = Constantes.CONST_RolEvaluador;

        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Fecha para datos internos.
        /// </summary>
        DateTime fecha;

        /// <summary>
        /// Mes en mayúscula.
        /// </summary>
        String sMes;

        /// <summary>
        /// Fecha formateada para mostrar internamente 
        /// en el perfil del contacto seleccionado.
        /// </summary>
        String fecha_mostrar;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                codcontacto = HttpContext.Current.Session["codcontacto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["codcontacto"].ToString()) ? HttpContext.Current.Session["codcontacto"].ToString() : "0";
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            //Se valida que si tenga datos "válidos".
            if (codcontacto == "0") { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true); }

            //Establecer datos iniciales.
            lblNombre.Text = usuario.Nombres + " " + usuario.Apellidos;
            fecha = DateTime.Now;
            sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
            lbl_tiempo.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

            if (!IsPostBack)
            { CargarPerfil(); }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 12/08/2014.
        /// Cargar la información del contacto seleccionado en "EvaluacionActa.aspx".
        /// </summary>
        private void CargarPerfil()
        {
            //Inicializar variables.
            DataTable RsContacto = new DataTable();
            lbl_perfil.Text = "";

            try
            {
                lbl_perfil.Text = lbl_perfil.Text + "<table width='95%' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF' class='Grilla' >" + //aaa
                                                     "        <tr>" +
                                                     "          <td align='center' valign='top' width='98%'>" +
                                                     "            <table width='95%' border='0' cellspacing='0' cellpadding='0'>" +
                                                     "              <tr>" +
                                                     "                " +
                                                     "              </tr>" +
                                                     "            </table>" +
                                                     "            <table width='95%' border='0' cellspacing='0' cellpadding='2' >" +
                                                     "                <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>";

                if (CodRol == Constantes.CONST_RolAsesor || CodRol == Constantes.CONST_RolAsesorLider)
                {
                    #region Si es Rol Asesor o Asesor Líder.

                    txtSQL = " SELECT nombres +' '+apellidos as nombre, email,  " +
                             " experiencia, dedicacion, Hojavida, intereses " +
                             " FROM Contacto " +
                             " WHERE  Id_Contacto =" + codcontacto;

                    RsContacto = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RsContacto.Rows.Count > 0)
                    {
                        //Nombre
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD width='35%'><b>Nombre:</b></TD>" +
                                                            "                  <TD>" + RsContacto.Rows[0]["Nombre"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Email
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Email:</b></TD>" +
                                                            "                  <TD>" + RsContacto.Rows[0]["Email"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Dedicacion
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Dedicación a la Unidad:</b></TD>" +
                                                            "                  <TD>";

                        if (RsContacto.Rows[0]["Dedicacion"].ToString() == "0") { lbl_perfil.Text = lbl_perfil.Text + "Completa"; }
                        else { lbl_perfil.Text = lbl_perfil.Text + "Parcial"; }

                        lbl_perfil.Text = lbl_perfil.Text + "</TD>" +
                                                            "                </TR>" +
                                                            "                <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>";

                        //Experiencia
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2><b>Experiencia Docente:</b></TD>" +
                                                             "                </TR>" +
                                                             "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2>" + RsContacto.Rows[0]["Experiencia"].ToString() + "</TD>" +
                                                             "                </TR>" +
                                                             "                <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>";

                        //Hoja de Vida
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2><b>Resumen Hoja de Vida:</b></TD>" +
                                                             "                </TR>" +
                                                             "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2>" + RsContacto.Rows[0]["HojaVida"].ToString() + "</TD>" +
                                                             "                </TR>" +
                                                             "                <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>";

                        //Intereses
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2><b>Experiencia e Intereses:</b></TD>" +
                                                             "                </TR>" +
                                                             "                <TR vAlign=top> " +
                                                             "                  <TD colspan=2>" + RsContacto.Rows[0]["Intereses"].ToString() + "</TD>" +
                                                             "                </TR>";

                    }
                    #endregion
                }
                else
                {
                    #region Siguiente grupo de código fuente.

                    txtSQL = " SELECT nombres +' '+apellidos as nombre, email, FechaNacimiento, " +
                                         " NomCiudad, NomDepartamento, Telefono " +
                                         " FROM Contacto " +
                                         " Left join Ciudad on id_ciudad=codciudad " +
                                         " left join Departamento on id_departamento=coddepartamento " +
                                         " WHERE  Id_Contacto =" + codcontacto;

                    RsContacto = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RsContacto.Rows.Count > 0)
                    {
                        //Nombre
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD width='35%'><b>Nombre:</b></TD>" +
                                                            "                  <TD>" + RsContacto.Rows[0]["Nombre"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Email
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Email:</b></TD>" +
                                                            "                  <TD>" + RsContacto.Rows[0]["Email"].ToString() + "</TD>" +
                                                            "                </TR>";
                        //Fecha de Nacimiento.
                        try
                        {
                            if (!String.IsNullOrEmpty(RsContacto.Rows[0]["FechaNacimiento"].ToString()))
                            {
                                fecha = DateTime.Parse(RsContacto.Rows[0]["FechaNacimiento"].ToString());
                                sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                                fecha_mostrar = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

                                lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                                     "                  <TD><b>Fecha de Nacimiento:</b></TD>" +
                                                                     "                  <TD>" + fecha_mostrar + "</TD>" +
                                                                     "                </TR>";
                            }
                        }
                        catch { }

                        //Lugar de Nacimiento.
                        try
                        {
                            if (!String.IsNullOrEmpty(RsContacto.Rows[0]["NomCiudad"].ToString()))
                            {
                                lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                                     "                  <TD><b>Lugar de Nacimiento:</b></TD>" +
                                                                     "                  <TD>" + RsContacto.Rows[0]["NomCiudad"].ToString() + " (" + RsContacto.Rows[0]["NomDepartamento"].ToString() + ") " + "</TD>" +
                                                                     "                </TR>";
                            }
                        }
                        catch { }
                    }

                    #endregion
                }

                //Información Académica
                txtSQL = " select tituloobtenido, anotitulo, nomnivelestudio, " +
                         " institucion, nomciudad,nomdepartamento " +
                         " from contactoestudio, nivelestudio, ciudad, departamento " +
                         " where id_nivelestudio=codnivelestudio and id_ciudad=codciudad " +
                         " and id_departamento=coddepartamento and codcontacto =" + codcontacto;

                RsContacto = consultas.ObtenerDataTable(txtSQL, "text");

                if (RsContacto.Rows.Count > 0)
                {
                    #region Ciclo para generar datos...

                    lbl_perfil.Text = lbl_perfil.Text + "<TR vAlign=top><TD colSpan=2 class='theme-title'>Información Académica</TD></TR>";

                    foreach (DataRow row in RsContacto.Rows)
                    {
                        //Nivel de Estudio
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Nivel de Estudio:</b></TD>" +
                                                            "                  <TD>" + row["NomNivelEstudio"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Titulo Obtenido
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>TituloObtenido:</b></TD>" +
                                                            "                  <TD>" + row["tituloobtenido"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Año
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Año:</b></TD>" +
                                                            "                  <TD>" + row["anotitulo"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Institucion
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Institucion:</b></TD>" +
                                                            "                  <TD>" + row["institucion"].ToString() + "</TD>" +
                                                            "                </TR>";

                        //Ciudad
                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top> " +
                                                            "                  <TD><b>Ciudad:</b></TD>" +
                                                            "                  <TD>" + row["NomCiudad"].ToString() + " (" + row["NomDepartamento"].ToString() + ")</TD>" +
                                                            "                </TR>";

                        lbl_perfil.Text = lbl_perfil.Text + "                <TR vAlign=top><TD colSpan=2>&nbsp;</TD></TR>";
                    }

                    #endregion
                }

                RsContacto = new DataTable();

                if (CodRol == Constantes.CONST_RolEvaluador)
                {
                    lbl_perfil.Text = lbl_perfil.Text + "<TR vAlign=top><TD colSpan=2 class='theme-title'><strong>Sectores a los que aplica</strong></TD></TR>";
                                                        

                    txtSQL = " SELECT orden = case Experiencia WHEN 'P' then 1 when 'S' then 2 else 3 end, Experiencia, E.ExperienciaPrincipal, " +
                             " E.ExperienciaSecundaria, Persona, S.nomSector " +
                             " FROM Evaluador E " +
                             " INNER JOIN EvaluadorSector ES ON E.codContacto = ES.codContacto " +
                             " INNER JOIN Sector S ON ES.codSector = S.id_Sector " +
                             " WHERE E.codContacto = " + codcontacto + " " +
                             " ORDER BY Orden";

                    RsContacto = consultas.ObtenerDataTable(txtSQL, "text");

                    bool pasa = true;
                    foreach (DataRow row in RsContacto.Rows)
                    {
                        if (pasa)
                        {
                            switch (row["Orden"].ToString())
                            {
                                case "1":
                                    lbl_perfil.Text = lbl_perfil.Text + "				<TR vAlign=top><TD><strong>Sector Principal:</strong></TD><td>" + row["nomSector"].ToString() + "</td></TR>" +
                                                                        "				<TR vAlign=top><TD><strong>Experiencia:</strong></TD><td>" + row["ExperienciaPrincipal"].ToString() + "</td></TR>";
                                    break;
                                case "2":
                                    lbl_perfil.Text = lbl_perfil.Text + "				<TR vAlign=top><TD><strong>Sector Secundario:</strong></TD><td>" + row["nomSector"].ToString() + "</td></TR>" +
                                                                        "				<TR vAlign=top><TD><strong>Experiencia:</strong></TD><td>" + row["ExperienciaSecundaria"].ToString() + "</td></TR>";
                                    break;
                                case "3":
                                    lbl_perfil.Text = lbl_perfil.Text + "				<TR vAlign=top><TD colspan='2' class='theme-title'><strong>Otros Sectores</strong></TD></TR>";
                                    lbl_perfil.Text = lbl_perfil.Text + "               <TR vAlign=top><td colspan='2'>" + row["nomSector"].ToString() + "</td></TR>";
                                    pasa = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            lbl_perfil.Text = lbl_perfil.Text + "				<tr vAlign=top><td colspan='2'>" + row["nomSector"].ToString() + "</td></TR>";
                        }
                    }

                    //Final.
                    lbl_perfil.Text = lbl_perfil.Text + "            </table>   " +
                                                        "          </td>" +
                                                        "        </tr>" +
                                                        "      </table>";
                }
            }
            catch { }
        }

        /// <summary>
        /// Establecer el primer valor en mayúscula, retornando un string con la primera en maýsucula.
        /// </summary>
        /// <param name="s">String a procesar</param>
        /// <returns>String procesado.</returns>
        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}