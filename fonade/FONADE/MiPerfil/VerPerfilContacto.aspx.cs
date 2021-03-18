using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using System.Globalization;

namespace Fonade.FONADE.MiPerfil
{
    public partial class VerPerfilContacto : Negocio.Base_Page
    {
        public int codigo;
        private String codProyecto;
        /// <summary>
        /// Variable que contiene el Id del contacto seleccionado en "AcreditacionActa.aspx".
        /// </summary>
        public int IdAcreditador_Session;

        /// <summary>
        /// Variable que contiene el rol del contacto seleccionado en "AcreditacionActa.aspx".
        /// </summary>
        public int CodRol_ActaAcreditacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            IdAcreditador_Session = HttpContext.Current.Session["IdAcreditador_Session"] != null ? IdAcreditador_Session = Convert.ToInt32(HttpContext.Current.Session["IdAcreditador_Session"].ToString()) : 0;
            CodRol_ActaAcreditacion = HttpContext.Current.Session["CodRol_ActaAcreditacion"] != null ? CodRol_ActaAcreditacion = Convert.ToInt32(HttpContext.Current.Session["CodRol_ActaAcreditacion"].ToString()) : 0;
            if (IdAcreditador_Session > 0 && CodRol_ActaAcreditacion > 0)
            {
                lbl_Titulo.Text = "PERFIL USUARIO";
                DateTime fecha = DateTime.Today;
                string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                l_fechaActual.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;
                this.PanelTitilo.Visible = true;

                PanelAsesores.Visible = false;
                P_PanelAsesores.Visible = false;

                PanelDemasRoles.Visible = false;
                P_PanelDemasRoles.Visible = false;

                PanelEstudios.Visible = false;
                P_PanelEstudios.Visible = false;

                PanelCerrar.Visible = false;
                P_PanelCerrar.Visible = false;

                pnl_especial.Visible = true;
                updt_especial.Visible = true;
                CargarInformacionContacto(IdAcreditador_Session, CodRol_ActaAcreditacion);
            }
            else
            {
                PanelAsesores.Visible = true;
                P_PanelAsesores.Visible = true;

                PanelDemasRoles.Visible = true;
                P_PanelDemasRoles.Visible = true;

                PanelEstudios.Visible = true;
                P_PanelEstudios.Visible = true;

                PanelCerrar.Visible = true;
                P_PanelCerrar.Visible = true;

                pnl_especial.Visible = false;
                updt_especial.Visible = false;

                codigo = Convert.ToInt32(Request.QueryString["LoadCode"]);
                l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");
                lbl_Titulo.Text = void_establecerTitulo("RESUMEN DEL PERFIL");
                if (!IsPostBack)
                {
                    var query = (from roles in consultas.Db.ProyectoContactos
                                 where roles.CodContacto == codigo
                                 select new
                                 {
                                     rol = roles.CodRol,
                                 }).FirstOrDefault();
                    int rol = query.rol;

                    if (rol == 1 || rol == 2)
                    {
                        PanelAsesores.Visible = true;
                        PanelDemasRoles.Visible = false;
                        llenarRoles("Asesores");
                    }
                    else
                    {
                        codProyecto = HttpContext.Current.Session["CodProyecto"].ToString();
                        var proyectoEstado = "SELECT CodEstado FROM Proyecto where Id_Proyecto="+codProyecto;
                        var dt = consultas.ObtenerDataTable(proyectoEstado, "text");
                        var estadoActual=0;
                        if (dt.Rows.Count > 0)
                            estadoActual = Convert.ToInt32(dt.Rows[0]["CodEstado"].ToString());
                        else
                            estadoActual = 0;

                        if( estadoActual != 1)
                        {
                            PanelDemasRoles.Visible = true;
                            PanelAsesores.Visible = false;
                            llenarRoles("Otros");
                        }
                        else
                        {                           
                            PanelDemasRoles.Visible = true;
                            lbl_FechaDeNacimiento.Visible = true;
                            l_fechanac.Visible = true;
                            lbl_LugarDeNacimiento.Visible = true;
                            l_lugarnac.Visible = true;
                            PanelAsesores.Visible = false;
                            P_PanelEstudios.Visible = false;
                            llenarRoles("Otros");
                        }
                    }
                }
            }
        }

        protected void llenarRoles(string caso)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_Ver_Perfil_Contacto", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdContacto", codigo);
                cmd.Parameters.AddWithValue("@caso", caso);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();

                String sMes;
                switch (caso)
                {
                    case "Asesores":
                        l_dedicacionAs.Text = Convert.ToString(r["dedicacion"]);
                        l_nombresAs.Text = Convert.ToString(r["nombre"]);
                        l_emailAs.Text = Convert.ToString(r["email"]);
                        txt_expintAs.Text = Convert.ToString(r["intereses"]);
                        txt_Sectores.Text = Convert.ToString(r["experiencia"]);
                        txt_hojavidaAs.Text = Convert.ToString(r["Hojavida"]);
                        break;

                    case "Otros":
                        DateTime fecha = Convert.ToDateTime(r["FechaNacimiento1"]);
                        l_nombre.Text = Convert.ToString(r["nombre1"]);
                        l_email.Text = Convert.ToString(r["email1"]);
                        sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
                        l_fechanac.Text = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year; Convert.ToString(r["FechaNacimiento1"]);
                        l_lugarnac.Text = Convert.ToString(r["NomCiudad1"]);
                        break;
                }

                cmd.Dispose();
            }
            finally {

                con.Close();
                con.Dispose();
            }
        }

        protected void lds_estudios_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.Db.MD_VerEstudiosAsesor(codigo)
                            select P;
                e.Result = query;
            }
            catch (Exception ex) { errorMessageDetail = ex.Message;}
        }

        protected void btn_Cerrar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "window.close();", true);
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

        /// <summary>
        /// Cargar la información del contacto seleccionado en "AcreditacionActa.aspx".
        /// </summary>
        /// <param name="codigo_contacto">Código del contacto seleccionado.</param>
        /// <param name="rol_contacto">Rol del contacto seleccionado.</param>
        private void CargarInformacionContacto(Int32 codigo_contacto, Int32 rol_contacto)
        {
            //Inicializar variables.
            String sqlConsulta = "";
            pnl_especial.Visible = true;
            updt_especial.Visible = true;

            try
            {
                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                          "    <td colspan=\"2\">" +
                                          "        &nbsp;" +
                                          "    </td>" +
                                          "</tr>";
                if (rol_contacto == Constantes.CONST_RolAsesor || rol_contacto == Constantes.CONST_RolAsesorLider)
                {

                    sqlConsulta = " SELECT Nombres + ' ' + Apellidos AS Nombre, Email, Experiencia, Dedicacion, " +
                                  " HojaVida, Intereses FROM Contacto WHERE  Id_Contacto = " + codigo_contacto;

                    var DT_1 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (DT_1.Rows.Count > 0)
                    {
                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_1.Rows[0]["Nombre"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Nombre:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_1.Rows[0]["Nombre"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                      "    </td>" +
                                                      "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_1.Rows[0]["Nombre"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Nombre:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + DT_1.Rows[0]["Nombre"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                          "    </td>" +
                                                          "</tr>";
                            }
                        }

                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_1.Rows[0]["Email"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Email:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_1.Rows[0]["Email"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                      "    </td>" +
                                                      "</tr>";
                            }


                        }
                        else
                        {
                            if (DT_1.Rows[0]["Email"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                         "    <td class=\"TitDestacado\">" +
                                                         "        <b>" +
                                                         "            <span>Email:</span></b>" +
                                                         "    </td>" +
                                                         "    <td class=\"TitDestacado\">" +
                                                         "        <span>" + DT_1.Rows[0]["Email"].ToString() + "</span>" +//lbl_Email.Text = DT_1.Rows[0]["Email"].ToString();
                                                         "    </td>" +
                                                         "</tr>";
                            }
                        }

                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_1.Rows[0]["Experiencia"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <b>" +
                                                                                      "            <span>Experiencia Docente:</span></b>" +
                                                                                      "    </td>" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <span>" + DT_1.Rows[0]["Experiencia"].ToString() + "</span>" +//lbl_experiencia.Text = DT_1.Rows[0]["Experiencia"].ToString();
                                                                                      "    </td>" +
                                                                                      "</tr>";
                            }

                        }
                        else
                        {
                            if (DT_1.Rows[0]["Experiencia"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Experiencia Docente:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + DT_1.Rows[0]["Experiencia"].ToString() + "</span>" +//lbl_experiencia.Text = DT_1.Rows[0]["Experiencia"].ToString();
                                                          "    </td>" +
                                                          "</tr>";
                            }
                        }

                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_1.Rows[0]["HojaVida"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <b>" +
                                                                                      "            <span>Resumen Hoja de Vida:</span></b>" +
                                                                                      "    </td>" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <span>" + DT_1.Rows[0]["HojaVida"].ToString() + "</span>" +//lbl_HV.Text = DT_1.Rows[0]["HojaVida"].ToString();
                                                                                      "    </td>" +
                                                                                      "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_1.Rows[0]["HojaVida"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Resumen Hoja de Vida:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_1.Rows[0]["HojaVida"].ToString() + "</span>" +//lbl_HV.Text = DT_1.Rows[0]["HojaVida"].ToString();
                                                      "    </td>" +
                                                      "</tr>";
                            }
                        }

                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_1.Rows[0]["Intereses"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <b>" +
                                                                                      "            <span>Experiencia e Intereses:</span></b>" +
                                                                                      "    </td>" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <span>" + DT_1.Rows[0]["Intereses"].ToString() + "</span>" +//lbl_exp_int.Text = DT_1.Rows[0]["Intereses"].ToString();
                                                                                      "    </td>" +
                                                                                      "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_1.Rows[0]["Intereses"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Experiencia e Intereses:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_1.Rows[0]["Intereses"].ToString() + "</span>" +//lbl_exp_int.Text = DT_1.Rows[0]["Intereses"].ToString();
                                                      "    </td>" +
                                                      "</tr>";
                            }
                        }

                        lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text +
                                                "<tr valign=\"top\">" +
                                                "    <td colspan=\"2\">" +
                                                "        &nbsp;" +
                                                "    </td>" +
                                                "</tr>" +
                                                "<tr valign=\"top\">" +
                                                "    <td colspan=\"2\" class=\"tituloDestacados\">Sectores a los que aplica</td>" +
                                                "</tr>" +
                                                "<tr valign=\"top\">" +
                                                "    <td colspan=\"2\">" +
                                                "        &nbsp;" +
                                                "    </td>" +
                                                "</tr>" +
                                                "<tr valign=\"top\">" +
                                                "    <td colspan=\"2\">" +
                                                "        &nbsp;" +
                                                "    </td>" +
                                                "</tr>";
                        return;
                    }
                }
                if (rol_contacto == Constantes.CONST_RolEvaluador)
                {
                    lbl_tabla_dibujada.Text = "";

                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>";

                    sqlConsulta = " SELECT orden = case Experiencia WHEN 'P' THEN 1 WHEN 'S' THEN 2 ELSE 3 END, " +
                                  " Experiencia, E.ExperienciaPrincipal, E.ExperienciaSecundaria, Persona, S.NomSector " +
                                  " FROM Evaluador E INNER JOIN EvaluadorSector ES " +
                                  " ON E.codContacto = ES.codContacto INNER JOIN Sector S " +
                                  " ON ES.codSector = S.id_Sector " +
                                  " WHERE E.codContacto = " + codigo_contacto + " " +
                                  " ORDER BY Orden";

                    var DT_4 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (DT_4.Rows.Count > 0)
                    {
                        bool pasa = true;
                        lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text +
                                                "<tr valign=\"top\">" +
                                                "    <td colspan=\"2\" class=\"tituloDestacados\">Sectores a los que aplica</td>" +
                                                "</tr>";
                        
                        for (int j = 0; j < DT_4.Rows.Count; j++)
                        {
                            String vl_Orden = DT_4.Rows[j]["orden"].ToString();

                            if (pasa)
                            {
                                switch (vl_Orden)
                                {
                                    case "1":
                                        if (lbl_tabla_dibujada.Text.Trim() == "")
                                        {
                                            if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "" && DT_4.Rows[j]["ExperienciaSecundaria"].ToString().Trim() != "")
                                            {
                                                lbl_tabla_dibujada.Text = " <TR><TD class='titulo'>Sector Principal:</TD><td>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Experiencia:</TD><td>" + DT_4.Rows[j]["ExperienciaSecundaria"].ToString() + "</td></TR>" + "";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>" + "";
                                            }
                                        }
                                        else
                                        {
                                            if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "" && DT_4.Rows[j]["ExperienciaSecundaria"].ToString().Trim() != "")
                                            {
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Sector Principal:</TD><td>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Experiencia:</TD><td>" + DT_4.Rows[j]["ExperienciaSecundaria"].ToString() + "</td></TR>" + "";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>" + "";
                                            }
                                        }
                                        break;

                                    case "2":
                                        if (lbl_tabla_dibujada.Text.Trim() == "")
                                        {
                                            if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "" && DT_4.Rows[j]["ExperienciaSecundaria"].ToString().Trim() != "")
                                            {
                                                lbl_tabla_dibujada.Text = " <TR><TD class='titulo'>Sector Secundario:</TD><td>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Experiencia:</TD><td>" + DT_4.Rows[j]["ExperienciaSecundaria"].ToString() + "</td></TR>" + "";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>" + "";
                                            }
                                        }
                                        else
                                        {
                                            if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "" && DT_4.Rows[j]["ExperienciaSecundaria"].ToString().Trim() != "")
                                            {
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Sector Secundario:</TD><td>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD class='titulo'>Experiencia:</TD><td>" + DT_4.Rows[j]["ExperienciaSecundaria"].ToString() + "</td></TR>" + "";
                                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD><img src='g/giftransparente.gif' height='8'></td></TR>" + "";
                                            }
                                        }
                                        break;

                                    case "3":
                                        if (lbl_tabla_dibujada.Text.Trim() == "")
                                        {
                                            lbl_tabla_dibujada.Text = " <TR><TD colspan='2' class='titulo'>Otros Sectores</TD></TR>";
                                            pasa = false;
                                        }
                                        else
                                        {
                                            lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + " <TR><TD colspan='2' class='titulo'>Otros Sectores</TD></TR>";
                                            pasa = false;
                                        }
                                        break;

                                    default:
                                        break;
                                }
                            }
                            if (!pasa)
                            {
                                if (lbl_tabla_dibujada.Text.Trim() == "")
                                {
                                    if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "")
                                    {
                                        lbl_tabla_dibujada.Text = "<TR><td colspan='2'>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                    }
                                }
                                else
                                {
                                    if (DT_4.Rows[j]["NomSector"].ToString().Trim() != "")
                                    {
                                        lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<TR><td colspan='2'>" + DT_4.Rows[j]["NomSector"].ToString() + "</td></TR>";
                                    }
                                }
                            }
                        }
                    }
                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>" +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>" +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>";
                    return;
                }
                else
                {
                    lbl_tabla_dibujada.Text = "";

                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>";

                    sqlConsulta = " SELECT Nombres + ' ' + Apellidos AS Nombre, Email, FechaNacimiento, " +
                                  " NomCiudad, NomDepartamento, Telefono " +
                                  " FROM Contacto LEFT JOIN Ciudad " +
                                  " ON Id_Ciudad = CodCiudad LEFT JOIN Departamento " +
                                  " ON Id_Departamento = CodDepartamento " +
                                  " WHERE Id_Contacto = " + codigo_contacto;

                    var DT_2 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (DT_2.Rows.Count > 0)
                    {
                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_2.Rows[0]["Nombre"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <b>" +
                                                                                      "            <span>Nombre:</span></b>" +
                                                                                      "    </td>" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <span>" + DT_2.Rows[0]["Nombre"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                                                      "    </td>" +
                                                                                      "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_2.Rows[0]["Nombre"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Nombre:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + DT_2.Rows[0]["Nombre"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                          "    </td>" +
                                                          "</tr>";
                            }
                        }

                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_2.Rows[0]["Email"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <b>" +
                                                                                      "            <span>Email:</span></b>" +
                                                                                      "    </td>" +
                                                                                      "    <td class=\"TitDestacado\">" +
                                                                                      "        <span>" + DT_2.Rows[0]["Email"].ToString() + "</span>" +//lbl_Nombre.Text = DT_1.Rows[0]["Nombre"].ToString();
                                                                                      "    </td>" +
                                                                                      "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_2.Rows[0]["Email"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Email:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_2.Rows[0]["Email"].ToString() + "</span>" +//lbl_Email.Text = DT_1.Rows[0]["Email"].ToString();
                                                      "    </td>" +
                                                      "</tr>";
                            }

                        }
                        string badInput = DT_2.Rows[0]["FechaNacimiento"].ToString();
                        DateTime fecha = new DateTime();

                        if (DateTime.TryParse(badInput, out fecha))
                        {                            

                            fecha = Convert.ToDateTime(DT_2.Rows[0]["FechaNacimiento"].ToString());
                            string sMes = fecha.ToString("MMM", CultureInfo.CreateSpecificCulture("es-CO"));
                            String MostrarFechaDepurada = "";
                            MostrarFechaDepurada = UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Fecha de Nacimiento:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + MostrarFechaDepurada + "</span>" +
                                                          "    </td>" +
                                                          "</tr>";
                            }
                            else
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Fecha de Nacimiento:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + MostrarFechaDepurada + "</span>" +
                                                          "    </td>" +
                                                          "</tr>";
                            }
                        }
                        
                        if (lbl_tabla_dibujada.Text.Trim() == "")
                        {
                            if (DT_2.Rows[0]["NomCiudad"].ToString().Trim() != "" && DT_2.Rows[0]["NomDepartamento"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Lugar de Nacimiento:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + DT_2.Rows[0]["NomCiudad"].ToString() + " (" + DT_2.Rows[0]["NomDepartamento"].ToString() + ")" + "</span>" +
                                                          "    </td>" +
                                                          "</tr>";
                            }
                        }
                        else
                        {
                            if (DT_2.Rows[0]["NomCiudad"].ToString().Trim() != "" && DT_2.Rows[0]["NomDepartamento"].ToString().Trim() != "")
                            {
                                lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <b>" +
                                                      "            <span>Lugar de Nacimiento:</span></b>" +
                                                      "    </td>" +
                                                      "    <td class=\"TitDestacado\">" +
                                                      "        <span>" + DT_2.Rows[0]["NomCiudad"].ToString() + " (" + DT_2.Rows[0]["NomDepartamento"].ToString() + ")" + "</span>" +
                                                      "    </td>" +
                                                      "</tr>";
                            }
                        }
                    }
                    sqlConsulta = " SELECT TituloObtenido, AnoTitulo, NomNivelEstudio, Institucion, NomCiudad, NomDepartamento " +
                                  " FROM ContactoEstudio, NivelEstudio, Ciudad, departamento " +
                                  " WHERE Id_NivelEstudio = CodNivelEstudio AND Id_Ciudad = CodCiudad " +
                                  " AND Id_Departamento = CodDepartamento AND CodContacto = " + codigo_contacto;

                    var DT_3 = consultas.ObtenerDataTable(sqlConsulta, "text");

                    if (DT_3.Rows.Count > 0)
                    {
                        lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                 "    <td colspan=\"2\">" +
                                                 "        &nbsp;" +
                                                 "    </td>" +
                                                 "</tr>" +
                                                 "<tr valign=\"top\">" +
                                                 "    <td colspan=\"2\" class=\"tituloDestacados\">" +
                                                 "        &nbsp;Información Académica" +
                                                 "    </td>" +
                                                 "</tr>";
                        for (int i = 0; i < DT_3.Rows.Count; i++)
                        {
                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                if (DT_3.Rows[0]["NomNivelEstudio"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <b>" +
                                                                                              "            <span>Nivel de Estudio:</span></b>" +
                                                                                              "    </td>" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <span>" + DT_3.Rows[0]["NomNivelEstudio"].ToString() + "</span>" +//lbl_NivelEstudio.Text = DT_3.Rows[0]["NomNivelEstudio"].ToString();
                                                                                              "    </td>" +
                                                                                              "</tr>";
                                }
                            }
                            else
                            {
                                if (DT_3.Rows[0]["NomNivelEstudio"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <b>" +
                                                              "            <span>Nivel de Estudio:</span></b>" +
                                                              "    </td>" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <span>" + DT_3.Rows[0]["NomNivelEstudio"].ToString() + "</span>" +//lbl_NivelEstudio.Text = DT_3.Rows[0]["NomNivelEstudio"].ToString();
                                                              "    </td>" +
                                                              "</tr>";
                                }
                            }

                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                if (DT_3.Rows[0]["TituloObtenido"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <b>" +
                                                                                              "            <span>Título Obtenido:</span></b>" +
                                                                                              "    </td>" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <span>" + DT_3.Rows[0]["TituloObtenido"].ToString() + "</span>" +//lbl_TituloObtenido.Text = DT_3.Rows[0]["TituloObtenido"].ToString();
                                                                                              "    </td>" +
                                                                                              "</tr>";
                                }
                            }
                            else
                            {
                                if (DT_3.Rows[0]["TituloObtenido"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <b>" +
                                                              "            <span>Título Obtenido:</span></b>" +
                                                              "    </td>" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <span>" + DT_3.Rows[0]["TituloObtenido"].ToString() + "</span>" +//lbl_TituloObtenido.Text = DT_3.Rows[0]["TituloObtenido"].ToString();
                                                              "    </td>" +
                                                              "</tr>";
                                }
                            }

                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                if (DT_3.Rows[0]["AnoTitulo"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <b>" +
                                                                                              "            <span>Año:</span></b>" +
                                                                                              "    </td>" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <span>" + DT_3.Rows[0]["AnoTitulo"].ToString() + "</span>" +//lbl_YearTitle.Text = DT_3.Rows[0]["AnoTitulo"].ToString();
                                                                                              "    </td>" +
                                                                                              "</tr>";
                                }
                            }
                            else
                            {
                                if (DT_3.Rows[0]["AnoTitulo"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <b>" +
                                                              "            <span>Año:</span></b>" +
                                                              "    </td>" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <span>" + DT_3.Rows[0]["AnoTitulo"].ToString() + "</span>" +//lbl_YearTitle.Text = DT_3.Rows[0]["AnoTitulo"].ToString();
                                                              "    </td>" +
                                                              "</tr>";
                                }
                            }

                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                if (DT_3.Rows[0]["Institucion"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <b>" +
                                                          "            <span>Institución:</span></b>" +
                                                          "    </td>" +
                                                          "    <td class=\"TitDestacado\">" +
                                                          "        <span>" + DT_3.Rows[0]["Institucion"].ToString() + "</span>" +//lbl_NmbInstitucion.Text = DT_3.Rows[0]["Institucion"].ToString();
                                                          "    </td>" +
                                                          "</tr>";
                                }
                            }
                            else
                            {
                                if (DT_3.Rows[0]["Institucion"].ToString().Trim() != "")
                                {
                                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <b>" +
                                                              "            <span>Institución:</span></b>" +
                                                              "    </td>" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <span>" + DT_3.Rows[0]["Institucion"].ToString() + "</span>" +//lbl_NmbInstitucion.Text = DT_3.Rows[0]["Institucion"].ToString();
                                                              "    </td>" +
                                                              "</tr>";
                                }
                            }

                            if (lbl_tabla_dibujada.Text.Trim() == "")
                            {
                                if (DT_3.Rows[0]["NomCiudad"].ToString().Trim() != "" && DT_3.Rows[0]["NomDepartamento"].ToString() != "")
                                {
                                    lbl_tabla_dibujada.Text = "<tr valign=\"top\">" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <b>" +
                                                                                              "            <span>Ciudad:</span></b>" +
                                                                                              "    </td>" +
                                                                                              "    <td class=\"TitDestacado\">" +
                                                                                              "        <span>" + DT_3.Rows[0]["NomCiudad"].ToString() + "(" + DT_3.Rows[0]["NomDepartamento"].ToString() + ")" + "</span>" +
                                                                                              "    </td>" +
                                                                                              "</tr>";
                                }
                            }
                            else
                            {
                                if (DT_3.Rows[0]["NomCiudad"].ToString().Trim() != "" && DT_3.Rows[0]["NomDepartamento"].ToString() != "")
                                {
                                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text + "<tr valign=\"top\">" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <b>" +
                                                              "            <span>Ciudad:</span></b>" +
                                                              "    </td>" +
                                                              "    <td class=\"TitDestacado\">" +
                                                              "        <span>" + DT_3.Rows[0]["NomCiudad"].ToString() + "(" + DT_3.Rows[0]["NomDepartamento"].ToString() + ")" + "</span>" +
                                                              "    </td>" +
                                                              "</tr>";
                                }
                            }
                        }
                    }

                    lbl_tabla_dibujada.Text = lbl_tabla_dibujada.Text +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>" +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\" class=\"tituloDestacados\">Sectores a los que aplica</td>" +
                                            "</tr>" +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>" +
                                            "<tr valign=\"top\">" +
                                            "    <td colspan=\"2\">" +
                                            "        &nbsp;" +
                                            "    </td>" +
                                            "</tr>";
                    PanelAsesores.Visible = false;
                    P_PanelAsesores.Visible = false;

                    return;
                }
            }
            catch (Exception ar)
            { string arr = ar.Message; }
        }

    }

}