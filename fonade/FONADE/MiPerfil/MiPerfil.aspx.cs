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
using System.Activities.Expressions;

namespace Fonade.FONADE.MiPerfil
{
    public partial class MiPerfil : Negocio.Base_Page
    {
        String CodTipoInstitucion;
        int CodProyecto;
        String txtSQL = String.Empty;
        String RutaArchivo = String.Empty;



        protected void Page_Load(object sender, EventArgs e)
        {
            CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? 0);

            if (!IsPostBack)
            {
                Cargarddl_dedicacion2();
                lbl_Titulo.Text = void_establecerTitulo("MI PERFIL");
                switch (usuario.CodGrupo)
                {
                    case 1:
                        //gerente Admin
                        ingresarGerenteAdmin();
                        PanelGerenteAdmin.Visible = true;

                        break;
                    case 8:
                    case 20:
                        pnlCallCenter.Visible = true;
                        var clave = (from c in consultas.Db.Clave
                                     where c.codContacto == usuario.IdContacto
                                     select c).ToList();

                        if (clave != null)
                        {
                            if (clave[clave.Count - 1].YaAvisoExpiracion == 1)
                            {
                                if (!IsPostBack)
                                    Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
                            }
                        }
                        else
                        {
                            if (!IsPostBack)
                                Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
                        }
                        break;
                    case 2:
                        if (!IsPostBack)
                            Admonfon.Visible = true;
                        break;
                    case 4:
                        //Jefe de Unidad
                        ingresarDatosJefeUnidad();
                        PanelJefeUnidad.Visible = true;
                        string txtsql = "SELECT CodTipoInstitucion, NomInstitucion, Nit, RegistroIcfes, FechaRegistro, CodCiudad, Direccion, Telefono, Fax, Website FROM Institucion WHERE Id_Institucion = " + usuario.CodInstitucion;
                        var dt = consultas.ObtenerDataTable(txtsql, "text");
                        if (dt.Rows.Count > 0) { CodTipoInstitucion = dt.Rows[0]["CodTipoInstitucion"].ToString(); }
                        dt = null;
                        txtsql = null;
                        if (CodTipoInstitucion == "33") // si es 33, NO se muestran los campos "ICFES, etc".
                        {
                            tr_ICFES.Visible = false;
                            tr_FECHA_REGISTRO.Visible = false;
                        }

                        break;
                    case 5:
                        //asesor
                        ingresarDatosAsesor();
                        PanelAsesor.Visible = true;
                        break;
                    case 6:
                        //Emprendedor
                        ingresarDatosEmprendedor();
                        //validar el estado del proyecto 
                        if (codEstadoProyecto(CodProyecto) > 1)//Si ya paso el estado de Registro y Asesoria
                        {
                            h_addInfoacademica4.Visible = false;
                            Ibtn_adicionarIA4.Visible = false;
                        }

                        PanelEmprendedor.Visible = true;
                        break;
                    case 9:
                        ingresardatosUs_GruposSelectos();
                        PanelGeneral.Visible = true;
                        break;
                    case 10:
                        ingresardatosUs_GruposSelectos();
                        PanelGeneral.Visible = true;
                        break;
                    case 11:
                        ingresarDatosEvaluador();
                        PanelEvaluador.Visible = true;
                        break;
                    case 12:
                        ingresardatosUs_GruposSelectos();
                        PanelGeneral.Visible = true;
                        break;
                    case 13:
                        ingresardatosUs_GruposSelectos();
                        PanelGeneral.Visible = true;
                        break;
                    case 14:

                        tr_cedula.Visible = true;
                        tr_numIdentificacion.Visible = false;
                        tr_cargo.Visible = false;
                        lblCargo.Visible = false;
                        tx_cargo1.Visible = false;
                        tr_direccion.Visible = true;
                        tr_Celular.Visible = true;
                        tr_Departamento.Visible = true;
                        tr_Ciudad.Visible = true;
                        pnl_exp_interventor.Visible = true;

                        CargarEstudiosInterventor();
                        ingresardatosUsGeneral();
                        PanelGeneral.Visible = true;

                        break;
                    case 15:
                        var clave2 = (from c in consultas.Db.Clave
                                      where c.codContacto == usuario.IdContacto
                                      select c).ToList();

                        if (clave2 != null)
                        {
                            if (clave2[clave2.Count - 1].YaAvisoExpiracion == 1)
                            {
                                if (!IsPostBack)
                                    Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
                            }
                        }
                        else
                        {
                            if (!IsPostBack)
                                Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
                        }
                        //if (!IsPostBack)
                        //    Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
                        break;
                    case 16:
                        cargarDatosLiderRegional();
                        PanelAsesor.Visible = true;
                        break;

                }
            }

            string txtSQL = "SELECT Count(id_ContactoArchivosAnexos) FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto;

            var reader = consultas.ObtenerDataTable(txtSQL, "text");


            if (int.Parse(reader.Rows[0].ItemArray[0].ToString()) > 0)
            {
                verDocAdjunto.Visible = true;
            }

        }

        private void Cargarddl_dedicacion2()
        {
            ddl_decalracion2.Items.Insert(0, new ListItem("Completa", "0"));
            ddl_decalracion2.Items.Insert(0, new ListItem("Parcial", "1"));
        }

        protected void ingresarDatosAsesor()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                int CodigoCiudad = 111;

                SqlCommand cmd = new SqlCommand("MD_cargarAsesorMiPerfil", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();
                l_nombre2.Text = Convert.ToString(r["Nombres"]);
                l_apellido2.Text = Convert.ToString(r["Apellidos"]);
                l_identificacion2.Text = Convert.ToString(r["Identificacion"]);
                l_email2.Text = Convert.ToString(r["Email"]);
                tx_experiencia2.Text = Convert.ToString(r["Experiencia"]);
                ddl_decalracion2.SelectedValue = Convert.ToString(r["Dedicacion"]);
                tx_hojadevida2.Text = Convert.ToString(r["HojaVida"]);
                tx_interes2.Text = Convert.ToString(r["Intereses"]);
                try
                {
                    CodigoCiudad = Convert.ToInt32(r["LugarExpedicionDI"]);
                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                cmd.Dispose();


                var query = from dept in consultas.Db.departamento
                            select new
                            {
                                Id_dpto = dept.Id_Departamento,
                                Nombre_dpto = dept.NomDepartamento,
                            };
                ddl_departamento2.DataSource = query.ToList();
                ddl_departamento2.DataTextField = "Nombre_dpto";
                ddl_departamento2.DataValueField = "Id_dpto";
                ddl_departamento2.DataBind();
                seleccionarDepartamento(ddl_departamento2, CodigoCiudad);
                llenarCiudad(ddl_ciudad2, ddl_departamento2);
                ddl_ciudad2.SelectedValue = CodigoCiudad.ToString();

            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
            finally
            {

                con.Close();
                con.Dispose();
            }
        }

        protected void ingresarDatosEvaluador()
        {
            string banco = "";
            string tipocuenta = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {


                SqlCommand cmd = new SqlCommand("MD_cargarMiPerfilEvaluador", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();
                l_nombre6.Text = Convert.ToString(r["Nombres"]);
                l_apellidos6.Text = Convert.ToString(r["Apellidos"]);
                l_identificacion6.Text = Convert.ToString(r["identificacion"]);
                l_persona6.Text = Convert.ToString(r["Persona"]);
                l_email6.Text = Convert.ToString(r["email"]);
                txt_direccion6.Text = Convert.ToString(r["Direccion"]);
                txt_telefono6.Text = Convert.ToString(r["Telefono"]);
                txt_fax6.Text = Convert.ToString(r["fax"]);
                txt_numcuenta6.Text = Convert.ToString(r["Cuenta"]);
                txt_planes6.Text = Convert.ToString(r["MaximoPlanes"]);
                txt_expgeneral6.Text = Convert.ToString(r["Experiencia"]);
                txt_expintereses6.Text = Convert.ToString(r["Intereses"]);
                txt_secprincipal6.Text = Convert.ToString(r["ExperienciaPrincipal"]);
                txt_secsecundario6.Text = Convert.ToString(r["ExperienciaSecundaria"]);
                txt_hojadevida6.Text = Convert.ToString(r["HojaVida"]);
                banco = Convert.ToString(r["codBanco"]);
                tipocuenta = Convert.ToString(r["CodTipoCuenta"]);
                cmd.Dispose();
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
            finally
            {

                con.Close();
                con.Dispose();

            }
            llenarbancos(ddl_banco6);
            ddl_banco6.SelectedValue = banco;
            llenartipocuenta(ddl_tipocuenta6);
            ddl_tipocuenta6.SelectedValue = tipocuenta;
            llenarsectores(ddl_secprincipal6);
            ddl_secprincipal6.SelectedValue = llenarddlsector("P");
            llenarsectores(ddl_secsecundario6);
            ddl_secsecundario6.SelectedValue = llenarddlsector("S");

        }

        protected void llenarbancos(DropDownList lista)
        {
            var query = from bn in consultas.Db.Bancos
                        select new
                        {
                            Id_banco = bn.Id_Banco,
                            Nombre_banco = bn.nomBanco,
                        };
            lista.DataSource = query.ToList();
            lista.DataTextField = "Nombre_banco";
            lista.DataValueField = "Id_banco";
            lista.DataBind();
        }

        protected string llenarddlsector(string tiposector)
        {
            string retorno = "11";
            try
            {
                var query = (from x in consultas.Db.EvaluadorSectors
                             where x.CodContacto == usuario.IdContacto
                             && x.Experiencia == tiposector
                             select new
                             {
                                 x.CodSector,
                             }).FirstOrDefault();
                retorno = query.CodSector.ToString();
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }

            return retorno;
        }

        protected void llenartipocuenta(DropDownList lista)
        {
            var query = from tc in consultas.Db.TipoCuentas
                        select new
                        {
                            Id_tc = tc.Id_TipoCuenta,
                            Nombre_tc = tc.NomTipoCuenta,
                        };
            lista.DataSource = query.ToList();
            lista.DataTextField = "Nombre_tc";
            lista.DataValueField = "Id_tc";
            lista.DataBind();
        }

        protected void llenarsectores(DropDownList lista)
        {
            var query = (from sc in consultas.Db.Sector
                         select new
                         {
                             Id_sec = sc.Id_Sector,
                             Nombre_sec = sc.NomSector,
                         }).OrderBy(e => e.Nombre_sec);

            lista.DataSource = query.ToList();
            lista.DataTextField = "Nombre_sec";
            lista.DataValueField = "Id_sec";
            lista.DataBind();
        }

        protected void ingresarDatosJefeUnidad()
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {

                SqlCommand cmd = new SqlCommand("MD_CargarJefeUnidadMiPerfil", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
                con.Open();
                SqlDataReader r = cmd.ExecuteReader();
                r.Read();
                int CodigoCiudadUnidad = 111;
                int CodigoCiudadEmprendedor = 111;
                try
                {
                    l_nomcentro3.Text = Convert.ToString(r["NomInstitucionI"]);
                    l_nit3.Text = Convert.ToString(r["NitI"]);
                    tx_icfes3.Text = Convert.ToString(r["RegistroIcfesI"]);
                    tx_fregistro2.Text = Convert.ToDateTime(r["FechaRegistroI"]).ToString("dd/MM/yyyy");
                    l_correspondencia3.Text = Convert.ToString(r["DireccionI"]);
                    tx_telunidad3.Text = Convert.ToString(r["TelefonoI"]);
                    tx_faxunidad3.Text = Convert.ToString(r["FaxI"]);
                    tx_sitioweb3.Text = Convert.ToString(r["WebsiteI"]);
                    l_nombre3.Text = Convert.ToString(r["NombresC"]);
                    l_apellido3.Text = Convert.ToString(r["Apellidos"]);
                    l_identificacion3.Text = Convert.ToString(r["IdentificacionC"]);
                    l_email3.Text = Convert.ToString(r["EmailC"]);
                    tx_cargo3.Text = Convert.ToString(r["CargoC"]);
                    tx_telefono3.Text = Convert.ToString(r["TelefonoC"]);
                    tx_fax3.Text = Convert.ToString(r["FaxC"]);
                    try
                    {
                        CodigoCiudadUnidad = Convert.ToInt32(r["CodCiudadI"]);
                    }
                    catch (Exception ex) { errorMessageDetail = ex.Message; }
                    try
                    {
                        CodigoCiudadEmprendedor = Convert.ToInt32(r["LugarExpedicionDIC"]);
                    }
                    catch (Exception ex) { errorMessageDetail = ex.Message; }

                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                cmd.Dispose();

                var query = from dept in consultas.Db.departamento
                            select new
                            {
                                Id_dpto1 = dept.Id_Departamento,
                                Nombre_dpto1 = dept.NomDepartamento,
                            };
                ddl_deparunidad3.DataSource = query.ToList();
                ddl_deparunidad3.DataTextField = "Nombre_dpto1";
                ddl_deparunidad3.DataValueField = "Id_dpto1";
                ddl_deparunidad3.DataBind();
                seleccionarDepartamento(ddl_deparunidad3, CodigoCiudadUnidad);
                llenarCiudad(ddl_ciudadunidad3, ddl_deparunidad3);
                ddl_ciudadunidad3.SelectedValue = CodigoCiudadUnidad.ToString();

                var query2 = from dept2 in consultas.Db.departamento
                             select new
                             {
                                 Id_dpto2 = dept2.Id_Departamento,
                                 Nombre_dpto2 = dept2.NomDepartamento,
                             };
                ddl_departamento3.DataSource = query2.ToList();
                ddl_departamento3.DataTextField = "Nombre_dpto2";
                ddl_departamento3.DataValueField = "Id_dpto2";
                ddl_departamento3.DataBind();
                seleccionarDepartamento(ddl_departamento3, CodigoCiudadEmprendedor);
                llenarCiudad(ddl_ciudad3, ddl_departamento3);
                ddl_ciudad3.SelectedValue = CodigoCiudadEmprendedor.ToString();

            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
            finally
            {
                con.Close();
                con.Dispose();
            }
        }

        protected void ingresarGerenteAdmin()
        {
            l_nombres5.Text = usuario.Nombres;
            l_apellidos5.Text = usuario.Apellidos;
            l_email5.Text = usuario.Email;
            l_identificacion5.Text = usuario.Identificacion.ToString();
        }

        protected void ingresardatosUsGeneral()
        {
            var query = (from c in consultas.Db.Contacto
                         where c.Id_Contacto == usuario.IdContacto
                         select new
                         {
                             c,
                         }).FirstOrDefault();

            l_nombre1.Text = usuario.Nombres;
            l_apellido1.Text = usuario.Apellidos;
            l_email1.Visible = true;
            l_email1.Text = usuario.Email;
            l_identificacion1.Text = usuario.Identificacion.ToString();
            tx_cargo1.Text = query.c.Cargo;
            tx_telefono1.Text = query.c.Telefono;
            tx_fax1.Text = query.c.Fax;

            lbl_Cedula.Text = query.c.Identificacion.ToString();
            lbl_Email.Visible = true;
            lbl_Email.Text = "Email:";
            txtDireccion.Text = query.c.Direccion;

            var queryDepartamentos = (from departamentos in consultas.Db.departamento
                                      select new
                                      {
                                          Id_dpto1 = departamentos.Id_Departamento,
                                          Nombre_dpto1 = departamentos.NomDepartamento,
                                      }).ToList();

            dd_deptos.DataSource = queryDepartamentos;
            dd_deptos.DataTextField = "Nombre_dpto1";
            dd_deptos.DataValueField = "Id_dpto1";
            dd_deptos.DataBind();

            if (query.c.CodCiudad != null)
            {
                dd_deptos.Items.FindByValue(query.c.Ciudad.CodDepartamento.ToString()).Selected = true;
            }
            llenarCiudad(dd_ciudad, dd_deptos);

            if (query.c.Ciudad != null)
            {
                //seleccionarDepartamento(dd_ciudad, query.c.Ciudad.Id_Ciudad);

                dd_ciudad.Items.FindByValue(query.c.Ciudad != null ? query.c.Ciudad.Id_Ciudad.ToString() : "").Selected = true;
            }

            var rstContacto = consultas.ObtenerDataTable("SELECT * FROM Interventor WHERE codContacto = " + usuario.IdContacto, "text");

            if (rstContacto.Rows.Count > 0)
            {
                try
                {
                    txtCelular.Text = rstContacto.Rows[0]["Celular"].ToString();
                    txt_exp_sector_principal.Text = rstContacto.Rows[0]["ExperienciaPrincipal"].ToString();
                    txt_exp_sector_secundario.Text = rstContacto.Rows[0]["ExperienciaSecundaria"].ToString();
                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
            }

            rstContacto = null;

            txt_exp_int_profesional.Text = query.c.Experiencia;
            txt_int_res_HV.Text = query.c.HojaVida;
            txt_exp_int_experi_intere.Text = query.c.Intereses;

            llenarsectores(dd_sector_princ_int);
            llenarsectores(dd_sector_second_int);

            var a = consultas.ObtenerDataTable("SELECT codSector FROM InterventorSector WHERE codContacto='" + usuario.IdContacto + "' and Experiencia='P'", "text");
            if (a.Rows.Count > 0) { dd_sector_princ_int.SelectedValue = a.Rows[0]["codSector"].ToString(); }
            a = null;
            var b = consultas.ObtenerDataTable("SELECT codSector FROM InterventorSector WHERE codContacto='" + usuario.IdContacto + "' and Experiencia='S'", "text");
            if (b.Rows.Count > 0) { dd_sector_second_int.SelectedValue = b.Rows[0]["codSector"].ToString(); }
            b = null;
        }

        private void cargarDatosLiderRegional()
        {
            var lider = (from c in consultas.Db.Contacto
                         where c.Id_Contacto == usuario.IdContacto
                         select c).FirstOrDefault();

            l_nombre2.Text = lider.Nombres;
            l_apellido2.Text = lider.Apellidos;
            l_identificacion2.Text = lider.Identificacion.ToString();
            l_email2.Text = lider.Email;
            tx_experiencia2.Text = lider.Experiencia;
            ddl_decalracion2.SelectedValue = lider.Dedicacion;
            tx_hojadevida2.Text = lider.HojaVida;
            tx_interes2.Text = lider.Intereses;
            if (!string.IsNullOrEmpty(lider.Dedicacion))
            {
                ddl_decalracion2.SelectedValue = lider.Dedicacion;
            }

            var query = from dept in consultas.Db.departamento
                        select new
                        {
                            Id_dpto = dept.Id_Departamento,
                            Nombre_dpto = dept.NomDepartamento,
                        };
            ddl_departamento2.DataSource = query.ToList();
            ddl_departamento2.DataTextField = "Nombre_dpto";
            ddl_departamento2.DataValueField = "Id_dpto";
            ddl_departamento2.DataBind();
            ddl_departamento2.Items.Insert(0, new ListItem("Seleccione", "0"));

            ddlRegioanles.DataSource = query.ToList();
            ddlRegioanles.DataTextField = "Nombre_dpto";
            ddlRegioanles.DataValueField = "Id_dpto";
            ddlRegioanles.DataBind();
            ddlRegioanles.SelectedValue = lider.CodTipoAprendiz.ToString();
            rowRegional.Visible = true;

            var ciudad = (from c in consultas.Db.Ciudad
                          where c.Id_Ciudad == lider.CodCiudad
                          select c).FirstOrDefault();
            if (ciudad != null)
            {
                var dpto = (from d in consultas.Db.departamento
                            where d.Id_Departamento == ciudad.CodDepartamento
                            select d).FirstOrDefault();

                var ciudades = (from c in consultas.Db.Ciudad
                                where c.CodDepartamento == dpto.Id_Departamento
                                select c).ToList();
                ddl_ciudad2.DataSource = ciudades;
                ddl_ciudad2.DataTextField = "NomCiudad";
                ddl_ciudad2.DataValueField = "Id_Ciudad";
                ddl_ciudad2.DataBind();

                ddl_departamento2.SelectedValue = dpto.Id_Departamento.ToString();

                ddl_ciudad2.SelectedValue = ciudad.Id_Ciudad.ToString();
            }
        }

        protected void ingresarDatosEmprendedor()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    int CodigoCiudadNacimiento = 111;
                    int CodigoCiudadexped = 111;
                    int CodigoCiudadDomicilio = 0;

                    SqlCommand cmd = new SqlCommand("MD_cargarMiPerfilEmprendedor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
                    con.Open();
                    SqlDataReader r = cmd.ExecuteReader();
                    r.Read();
                    l_nombre4.Text = Convert.ToString(r["Nombres"]);
                    l_apellidos4.Text = Convert.ToString(r["Apellidos"]);
                    l_identificacion4.Text = Convert.ToString(r["Identificacion"]);
                    l_email4.Text = r["Email"].ToString(); // Convert.ToString();
                    if (!string.IsNullOrEmpty(r["FechaNacimiento"].ToString()))
                    {
                        tx_fechanacimiento4.Text = Convert.ToDateTime(r["FechaNacimiento"]).ToString("dd/MM/yyyy");
                    }
                    tx_telefono4.Text = Convert.ToString(r["Telefono"]);
                    txtDireccionEmprendedor.Text = Convert.ToString(r["Direccion"]);
                    ddl_genero4.SelectedValue = Convert.ToString(r["Genero"]);
                    CodigoCiudadNacimiento = Convert.ToInt32(r["CodCiudad"]);
                    CodigoCiudadexped = Convert.ToInt32(r["LugarExpedicionDI"]);
                    CodigoCiudadDomicilio = Convert.ToInt32(r["CodCiudadResidencia"]);
                    cmd.Dispose();

                    var query = from dept in consultas.Db.departamento
                                select new
                                {
                                    Id_dpto1 = dept.Id_Departamento,
                                    Nombre_dpto1 = dept.NomDepartamento,
                                };
                    ddl_depexped4.DataSource = query.ToList();
                    ddl_depexped4.DataTextField = "Nombre_dpto1";
                    ddl_depexped4.DataValueField = "Id_dpto1";
                    ddl_depexped4.DataBind();
                    seleccionarDepartamento(ddl_depexped4, CodigoCiudadexped);
                    llenarCiudad(dd_ciuexp4, ddl_depexped4);
                    dd_ciuexp4.SelectedValue = CodigoCiudadexped.ToString();

                    var query2 = from dept2 in consultas.Db.departamento
                                 select new
                                 {
                                     Id_dpto2 = dept2.Id_Departamento,
                                     Nombre_dpto2 = dept2.NomDepartamento,
                                 };
                    ddl_departamento4.DataSource = query2.ToList();
                    ddl_departamento4.DataTextField = "Nombre_dpto2";
                    ddl_departamento4.DataValueField = "Id_dpto2";
                    ddl_departamento4.DataBind();
                    seleccionarDepartamento(ddl_departamento4, CodigoCiudadNacimiento);
                    llenarCiudad(ddl_ciudad4, ddl_departamento4);
                    ddl_ciudad4.SelectedValue = CodigoCiudadNacimiento.ToString();

                    //cargar Departamento y ciudad de domicilio
                    var queryDptoDomicilio = (from dept2 in consultas.Db.departamento
                                              select new
                                              {
                                                  Id_dptoDomicilio = dept2.Id_Departamento,
                                                  Nombre_dptoDomicilio = dept2.NomDepartamento
                                              }).ToList();

                    queryDptoDomicilio.Add(new { Id_dptoDomicilio = 0, Nombre_dptoDomicilio = "Seleccione..." });

                    ddl_DepartamentoDomicilio.DataSource = queryDptoDomicilio;
                    ddl_DepartamentoDomicilio.DataTextField = "Nombre_dptoDomicilio";
                    ddl_DepartamentoDomicilio.DataValueField = "Id_dptoDomicilio";
                    ddl_DepartamentoDomicilio.DataBind();
                    seleccionarDepartamento(ddl_DepartamentoDomicilio, CodigoCiudadDomicilio);
                    llenarCiudad(ddl_CiudadDomicilio, ddl_DepartamentoDomicilio);
                    ddl_CiudadDomicilio.SelectedValue = CodigoCiudadDomicilio.ToString();

                }
                catch (SqlException ex) { errorMessageDetail = ex.Message; }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }
        }

        /// <summary>
        ///Este método se aplica a los siguientes grupos:
        /// 9 = Gerente Evaluador
        /// 10 = Coordinador de Evaluadores
        /// 12 = Gerente Interventor
        /// 13 = Coordinar de Interventor
        /// </summary>
        protected void ingresardatosUs_GruposSelectos()
        {
            tb_PanelAsesor.Visible = false;
            tb_PanelJefeUnidad.Visible = false;
            tb_PanelEmprendedor.Visible = false;
            tb_PanelGerenteAdmin.Visible = false;
            tb_PanelEvaluador.Visible = false;

            var query = (from c in consultas.Db.Contacto
                         where c.Id_Contacto == usuario.IdContacto
                         select new
                         {
                             c,
                         }).FirstOrDefault();

            l_nombre1.Text = usuario.Nombres;
            l_apellido1.Text = usuario.Apellidos;
            l_identificacion1.Text = usuario.Identificacion.ToString();
            l_email1.Text = usuario.Email;
            tx_cargo1.Text = query.c.Cargo;
            tx_telefono1.Text = query.c.Telefono;
            tx_fax1.Text = query.c.Fax;

        }

        /// <summary>
        /// 9 = Gerente Evaluador
        /// 10 = Coordinador de Evaluadores
        /// 12 = Gerente Interventor
        /// 13 = Coordinar de Interventor
        /// </summary>
        protected void ingresarUsuarioFiduciaria()
        {
            //Ocultar los demás campos.
            tb_PanelAsesor.Visible = false;
            tb_PanelJefeUnidad.Visible = false;
            tb_PanelEmprendedor.Visible = false;
            tb_PanelGerenteAdmin.Visible = false;
            tb_PanelEvaluador.Visible = false;

            //Consulta.
            var query = (from c in consultas.Db.Contacto
                         where c.Id_Contacto == usuario.IdContacto
                         select new
                         {
                             c,
                         }).FirstOrDefault();

            l_nombre1.Text = usuario.Nombres;
            l_apellido1.Text = usuario.Apellidos;
            l_identificacion1.Text = usuario.Identificacion.ToString();
            l_email1.Text = usuario.Email;
            tx_cargo1.Text = query.c.Cargo;
            tx_telefono1.Text = query.c.Telefono;
            tx_fax1.Text = query.c.Fax;

        }

        protected void ddl_departamento2_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(ddl_ciudad2, ddl_departamento2);
        }

        protected void seleccionarDepartamento(DropDownList dtoaSeleccionar, int codigoCiudad)
        {
            try
            {
                if (codigoCiudad != 0)
                {
                    var query = (from CIud in consultas.Db.Ciudad
                                 where CIud.Id_Ciudad == codigoCiudad
                                 select new
                                 {
                                     Id_departamento = CIud.CodDepartamento,
                                 }).FirstOrDefault();
                    dtoaSeleccionar.SelectedValue = Convert.ToString(query.Id_departamento);
                }
                else
                {
                    dtoaSeleccionar.SelectedValue = "0";
                }

            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }


        protected void llenarCiudad(DropDownList listaciudad, DropDownList departamentolist)
        {
            try
            {
                List<ListCiudades> queryCiudades = new List<ListCiudades>();

                if (Convert.ToInt32(departamentolist.SelectedValue) != 0)
                {
                    queryCiudades = (from ciudades in consultas.Db.Ciudad
                                     where ciudades.CodDepartamento == Convert.ToInt32(departamentolist.SelectedValue)
                                     select new ListCiudades
                                     {
                                         codCiudad = ciudades.Id_Ciudad,
                                         nomCiudad = ciudades.NomCiudad
                                     }).ToList();
                }
                else
                {
                    queryCiudades.Add(new ListCiudades { codCiudad = 0, nomCiudad = "Seleccione..." });
                }

                listaciudad.DataSource = queryCiudades;
                listaciudad.DataTextField = "nomCiudad";
                listaciudad.DataValueField = "codCiudad";
                listaciudad.DataBind();
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        protected void lds_estudios_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.Db.MD_VerEstudiosAsesor(usuario.IdContacto)
                            select P;

                var query2 = (from q in query
                              select new
                              {
                                  q.AnoTitulo,
                                  q.CodCiudad,
                                  q.codprogramaAcademico,
                                  q.Finalizado,
                                  q.FlagIngresadoAsesor,
                                  q.Id_ContactoEstudio,
                                  q.Institucion,
                                  q.NomCiudad,
                                  q.NomDepartamento,
                                  q.NomNivelEstudio,
                                  q.TituloObtenido,
                                  q.URL,
                                  Habilitado = habilitadoProyecto()
                              }).ToList();

                e.Result = query2;
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        private bool habilitadoProyecto()
        {
            bool habilitado = true;

            if (codEstadoProyecto(CodProyecto) > 1) //Registro y Asesoria - 1
            {
                habilitado = false;
            }

            return habilitado;
        }

        /// <summary>
        /// Cargar el GridView "gv_infoAcademic_Interventor" con los estudios del inteventor.
        /// </summary>
        private void CargarEstudiosInterventor()
        {
            String txtSQL;
            DataTable RS = new DataTable();

            try
            {
                txtSQL = " SELECT CE.Id_ContactoEstudio, CE.TituloObtenido, CE.AnoTitulo,CE.Finalizado, CE.Institucion, CE.CodCiudad, C.NomCiudad, D.NomDepartamento, NE.NomNivelEstudio" +
                         " FROM ContactoEstudio CE, Ciudad C, Departamento D, NivelEstudio NE" +
                         " WHERE CE.CodCiudad = C.ID_Ciudad" +
                         " AND C.CodDepartamento = D.Id_Departamento" +
                         " AND CE.CodNivelEstudio = NE.Id_NivelEstudio" +
                         " AND codcontacto = " + usuario.IdContacto +
                         " ORDER BY CE.Finalizado,CE.AnoTitulo Desc";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                this.gv_infoAcademic_Interventor.DataSource = RS;
                this.gv_infoAcademic_Interventor.DataBind();
            }
            catch { }
        }

        protected void btn_actualizar2_Click(object sender, EventArgs e)
        {
            updateAsesor();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
        }

        protected void Eliminar_Estudios_Realizados(object sender, CommandEventArgs e)
        {
            try
            {
                int codigo_estudio = Convert.ToInt32(e.CommandArgument);
                var query = (from Estudio in consultas.Db.ContactoEstudios
                             where Estudio.FlagIngresadoAsesor == 1
                             & Estudio.Id_ContactoEstudio == codigo_estudio
                             select Estudio.FlagIngresadoAsesor).Count();

                if (query == 0)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("MD_EliminarEstudioRealizado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_estudio", codigo_estudio);
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            cmd2.Dispose();
                            cmd.Dispose();
                            this.gvestudiosrealizadosasesor.DataBind();
                        }
                        catch (SqlException ex) { errorMessageDetail = ex.Message; }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }


                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No ha sido posible eliminar esta información académica')", true);
                }
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        protected void Eliminar_Estudios_Emprendedor(object sender, CommandEventArgs e)
        {
            try
            {
                int codigo_estudio = Convert.ToInt32(e.CommandArgument);
                var query = (from Estudio in consultas.Db.ContactoEstudios
                             where Estudio.FlagIngresadoAsesor == 1
                             & Estudio.Id_ContactoEstudio == codigo_estudio
                             select Estudio.FlagIngresadoAsesor).Count();

                if (query == 0)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("MD_EliminarEstudioRealizado", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@id_estudio", codigo_estudio);
                            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                            con.Open();
                            cmd2.ExecuteNonQuery();
                            cmd.ExecuteNonQuery();
                            cmd2.Dispose();
                            cmd.Dispose();
                            this.gvestudiosemprendedor.DataBind();
                        }
                        finally
                        {
                            con.Close();
                            con.Dispose();
                        }
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No ha sido posible eliminar esta información académica')", true);
                }
            }
            catch (Exception ex) { errorMessageDetail = ex.Message; }
        }

        protected void eliminararchivos()
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Funciona!')", true);
        }

        protected void btn_actualizar4_Click(object sender, EventArgs e)
        {
            if (validarCamposEmprendedor())
            {
                updateEmprendedor();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
            }
        }

        private bool validarCamposEmprendedor()
        {
            bool valido = true;

            if (txtDireccionEmprendedor.Text == "")
            {
                valido = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar la direccion de domicilio!'); document.location=('MiPerfil.aspx');", true);
            }

            if (Convert.ToInt32(ddl_CiudadDomicilio.SelectedValue) == 0)
            {
                valido = false;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar una ciudad!');", true);
            }

            return valido;
        }

        protected void ddl_departamento3_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(ddl_ciudad3, ddl_departamento3);
        }

        protected void ddl_deparunidad3_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(ddl_ciudadunidad3, ddl_deparunidad3);
        }

        protected void ddl_depexped4_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(dd_ciuexp4, ddl_depexped4);
        }

        protected void dd_deptos_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(dd_ciudad, dd_deptos);
        }

        protected void ddl_departamento4_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(ddl_ciudad4, ddl_departamento4);
        }

        public string _cadenaConex = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        private int codEstadoProyecto(int codProyecto)
        {
            int codEstado = 0;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {

                codEstado = (from p in db.Proyecto1s
                             where p.Id_Proyecto == codProyecto
                             select p.CodEstado).FirstOrDefault();
            }

            return codEstado;
        }

        protected void Image1_Click(object sender, ImageClickEventArgs e)
        {
            int codRol = 0;
            int codProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
            {
                codRol = (from pc in db.ProyectoContactos
                          where pc.CodContacto == usuario.IdContacto
                          select pc.CodRol).FirstOrDefault();
            }

            if (codRol == 3) //3 Rol Emprendedor
            {
                if (codEstadoProyecto(codProyecto) == 1) // 1 Estado Registro y Asesoria
                {
                    var user = usuario.IdContacto;
                    HttpContext.Current.Session["USER"] = user;
                    HttpContext.Current.Session["CodProyecto"] = codProyecto;
                    HttpContext.Current.Session["TipoDocumento"] = "FotocopiaDocumento";
                    HttpContext.Current.Session["idCertificacionEstudios"] = 0;
                    Session["Cedula"] = "CC_" + usuario.Identificacion;
                    Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
                }
                else //Si ya el proyecto NO esta en Registro y Asesoria
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje"
                        , "alert('Este proyecto ya no se encuentra en Registro y Asesoría, por lo tanto no está habilitado para actualizar el documento de identificacion!'); ", true);
                }

            }
            else //Si es diferente a un emprendedor
            {
                var user = usuario.IdContacto;
                HttpContext.Current.Session["USER"] = user;
                HttpContext.Current.Session["CodProyecto"] = codProyecto;
                HttpContext.Current.Session["TipoDocumento"] = "FotocopiaDocumento";
                HttpContext.Current.Session["idCertificacionEstudios"] = 0;
                Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
            }
        }


        protected void btn_actualizar3_Click(object sender, EventArgs e)
        {
            updateJefeUnidad();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
        }

        protected void updateJefeUnidad()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("MD_Update_JefeUnidad", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@icfes", tx_icfes3.Text);
                    cmd.Parameters.AddWithValue("@fechaderegistro", Convert.ToDateTime(tx_fregistro2.Text).ToShortDateString());
                    cmd.Parameters.AddWithValue("@telefonoInst", tx_telunidad3.Text);
                    cmd.Parameters.AddWithValue("@faxInst", tx_faxunidad3.Text);
                    cmd.Parameters.AddWithValue("@web", tx_sitioweb3.Text);
                    cmd.Parameters.AddWithValue("@ciudadInst", Convert.ToInt32(ddl_ciudadunidad3.SelectedValue));
                    cmd.Parameters.AddWithValue("@idInstitucion", usuario.CodInstitucion);
                    cmd.Parameters.AddWithValue("@cargo", tx_cargo3.Text);
                    cmd.Parameters.AddWithValue("@telefono", tx_telefono3.Text);
                    cmd.Parameters.AddWithValue("@fax", tx_fax3.Text);
                    cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad3.SelectedValue));
                    cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();

                }
                catch (Exception ex)
                { throw; }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
            }

        }

        protected void updateEmprendedor()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("MD_Update_Emprendedor", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ciudadexpedicion", Convert.ToInt32(dd_ciuexp4.SelectedValue));
                    cmd.Parameters.AddWithValue("@telefono", tx_telefono4.Text);
                    cmd.Parameters.AddWithValue("@direccion", txtDireccionEmprendedor.Text);
                    cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad4.SelectedValue));
                    cmd.Parameters.AddWithValue("@ciudadResidencia", Convert.ToInt32(ddl_CiudadDomicilio.SelectedValue));
                    cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
                    cmd.Parameters.AddWithValue("@genero", ddl_genero4.SelectedValue);
                    cmd.Parameters.AddWithValue("@fechanacimiento", Convert.ToDateTime(tx_fechanacimiento4.Text).ToString("yyyy-MM-dd"));
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();

                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }

        protected void updateAsesor()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    var valida = true;
                    if (usuario.CodGrupo == 16)
                    {
                        valida = ValidarCombos();
                    }
                    if (valida)
                    {
                        SqlCommand cmd = new SqlCommand("MD_Update_Asesor", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@dedicacion", ddl_decalracion2.SelectedValue);
                        cmd.Parameters.AddWithValue("@experiencia", tx_experiencia2.Text);
                        cmd.Parameters.AddWithValue("@hojadevida", tx_hojadevida2.Text);
                        cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
                        cmd.Parameters.AddWithValue("@intereses", tx_interes2.Text);
                        cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad2.SelectedValue));
                        SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                        con.Open();
                        cmd2.ExecuteNonQuery();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        cmd2.Dispose();
                        cmd.Dispose();

                        if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
                        {
                            var lider = (from c in consultas.Db.Contacto
                                         where c.Id_Contacto == usuario.IdContacto
                                         select c).FirstOrDefault();
                            lider.CodCiudad = int.Parse(ddl_ciudad2.SelectedValue);
                            lider.Dedicacion = ddl_decalracion2.SelectedValue.Trim();
                            consultas.Db.SubmitChanges();
                        }
                    }

                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                finally
                {
                    con.Close();
                    con.Dispose();

                }
            }

        }

        private bool ValidarCombos()
        {
            var resp = false;
            if (ddl_departamento2.SelectedIndex != 0)
            {
                resp = true;
            }
            else
            {
                resp = false;
                ddl_departamento2.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar el departamento de expedición del documento!'); ", true);
            }

            return resp;
        }

        protected void updateUsGeneral()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {

                    SqlCommand cmd = new SqlCommand("MD_Update_UsGeneral", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@cargo", tx_cargo1.Text);
                    cmd.Parameters.AddWithValue("@telefono", tx_telefono1.Text);
                    cmd.Parameters.AddWithValue("@fax", tx_fax1.Text);
                    cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();

                    var queryInterventor = (from c in consultas.Db.Contacto
                                            where c.Id_Contacto == usuario.IdContacto
                                            select c).FirstOrDefault();

                    if (queryInterventor != null)
                    {
                        queryInterventor.CodCiudad = Convert.ToInt32(dd_ciudad.SelectedValue);
                        consultas.Db.SubmitChanges();
                    }
                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }

        protected void updateEvaluador()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("MD_Update_Evaluador", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@telefono", txt_telefono6.Text);
                    cmd.Parameters.AddWithValue("@direccion", txt_direccion6.Text);
                    cmd.Parameters.AddWithValue("@experiencia", txt_expgeneral6.Text);
                    cmd.Parameters.AddWithValue("@intereses", txt_expintereses6.Text);
                    cmd.Parameters.AddWithValue("@hojavida", txt_hojadevida6.Text);
                    cmd.Parameters.AddWithValue("@fax", txt_fax6.Text);
                    cmd.Parameters.AddWithValue("@codBanco", Convert.ToInt32(ddl_banco6.SelectedValue));
                    cmd.Parameters.AddWithValue("@CodTipocuenta", Convert.ToInt32(ddl_tipocuenta6.SelectedValue));
                    cmd.Parameters.AddWithValue("@txtNumCuenta", txt_numcuenta6.Text);
                    cmd.Parameters.AddWithValue("@MaximoPlanes", Convert.ToInt16(txt_planes6.Text));
                    cmd.Parameters.AddWithValue("@txtExpPrincipal", txt_secprincipal6.Text);
                    cmd.Parameters.AddWithValue("@txtExpSecundaria", txt_secsecundario6.Text);
                    cmd.Parameters.AddWithValue("@codSectorPri", Convert.ToInt32(ddl_secprincipal6.SelectedValue));
                    cmd.Parameters.AddWithValue("@codSectorSec", Convert.ToInt32(ddl_secsecundario6.SelectedValue));
                    cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                    cmd2.Dispose();
                    cmd.Dispose();
                }
                catch (Exception ex) { errorMessageDetail = ex.Message; }
                finally
                {
                    con.Close();
                    con.Dispose();
                }
            }

        }

        protected void btn_actualizar1_Click(object sender, EventArgs e)
        {
            updateUsGeneral();
            if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si es interventor
            {
                updateInterventor(usuario.IdContacto);
            }
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
        }

        private void ActualizarSectorInterventor(DropDownList ddl, string tipo, int _codUsuario)
        {
            //Tabla InterventorSector Se hace de esta manera porque la tabla no cuenta con PrimaryKey
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

            string fechaHora = DateTime.Now.Year + "-"
                                + DateTime.Now.Month + "-"
                                + DateTime.Now.Day + " " + DateTime.Now.ToShortTimeString().Replace(".", "");


            string sqlText = "update InterventorSector " +
                             "set CodSector = " + ddl.SelectedValue + " " +
                             ", fechaActualizacion = cast('" + fechaHora + "' as datetime) " +
                             "where CodContacto = " + _codUsuario.ToString() + " and Experiencia = '" + tipo + "'";

            SqlCommand cmd = new SqlCommand(sqlText, con);
            cmd.CommandType = CommandType.Text;
            con.Open();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
            con.Dispose();
        }

        private void updateInterventor(int codUsuario)
        {

            //Actualizar Sector Principal P
            ActualizarSectorInterventor(dd_sector_princ_int, "P", codUsuario);

            //Actualizar Sector Secundario S
            ActualizarSectorInterventor(dd_sector_second_int, "S", codUsuario);

            //Tabla Contacto
            var queryContacto = (from c in consultas.Db.Contacto
                                 where c.Id_Contacto == codUsuario
                                 select c).FirstOrDefault();

            if (queryContacto != null)
            {
                if (txtDireccion.Text != "")
                    queryContacto.Direccion = txtDireccion.Text;

                if (txt_exp_int_profesional.Text != "")
                    queryContacto.Experiencia = txt_exp_int_profesional.Text;

                if (txt_int_res_HV.Text != "")
                    queryContacto.HojaVida = txt_int_res_HV.Text;

                if (txt_exp_int_experi_intere.Text != "")
                    queryContacto.Intereses = txt_exp_int_experi_intere.Text;

                consultas.Db.SubmitChanges();
            }

            //Tabla interventor
            var queryInterventor = (from c in consultas.Db.Interventor1s
                                    where c.CodContacto == codUsuario
                                    select c).FirstOrDefault();

            if (queryInterventor != null)
            {
                if (txtCelular.Text != "")
                    queryInterventor.Celular = txtCelular.Text;

                if (txt_exp_sector_principal.Text != "")
                    queryInterventor.ExperienciaPrincipal = txt_exp_sector_principal.Text;

                if (txt_exp_sector_secundario.Text != "")
                    queryInterventor.Experienciasecundaria = txt_exp_sector_secundario.Text;

                consultas.Db.SubmitChanges();
            }


        }

        protected void btn_actualizar6_Click(object sender, EventArgs e)
        {
            updateEvaluador();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
        }

        protected void Img_Btn_Nuevo_Doc_interventor_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion_Docs"] = "Crear";
            Redirect(null, "CatalogoAnexarInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void Img_Btn_Ver_Doc_interventor_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["Accion_Docs"] = "Vista";
            Redirect(null, "CatalogoAnexarInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
        }

        protected void verDocAdjunto_Click(object sender, ImageClickEventArgs e)
        {
            String txtSQL = "SELECT NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto + " AND TipoArchivo ='FotocopiaDocumento' AND CodProyecto = " + CodProyecto.ToString();
            DataTable RS = consultas.ObtenerDataTable(txtSQL, "text");

            if (RS.Rows.Count > 0)
            {
                int CodCarpeta = Convert.ToInt32(usuario.IdContacto) / 2000;
                String Filename = RS.Rows[0]["NombreArchivo"].ToString();
                RutaArchivo = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "contactoAnexos/" + CodCarpeta.ToString() + "/ContactoAnexo_" + usuario.IdContacto + "/" + Filename.ToString();
            }
            RS.Dispose();
            Redirect(null, RutaArchivo, "_blank", null);
        }

        protected void AgregaDocumento(object sender, CommandEventArgs e)
        {
            int codProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);

            if (codEstadoProyecto(codProyecto) == 1) // 1 Estado Registro y Asesoria
            {
                var idCertificacion = e.CommandArgument;
                var user = usuario.IdContacto;
                HttpContext.Current.Session["USER"] = user;
                HttpContext.Current.Session["CodProyecto"] = HttpContext.Current.Session["CodProyecto"];
                HttpContext.Current.Session["TipoDocumento"] = "CertificacionEstudios";
                HttpContext.Current.Session["idCertificacionEstudios"] = idCertificacion;
                Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje"
                        , "alert('Este proyecto ya no se encuentra en Registro y Asesoría, por lo tanto no está habilitado para actualizar el documento de identificacion!'); ", true);
            }
        }

        protected void VerCertificado(object sender, CommandEventArgs e)
        {
            var idCertificacion = e.CommandArgument;

            string txtSQL = "SELECT TOP 1 NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto + " AND TipoArchivo ='CertificacionEstudios' AND CodProyecto = " + CodProyecto.ToString() + " AND CodContactoEstudio='" + idCertificacion.ToString() + "'";
            DataTable RS = consultas.ObtenerDataTable(txtSQL, "text");

            if (RS.Rows.Count > 0)
            {
                int CodCarpeta = Convert.ToInt32(usuario.IdContacto) / 2000;
                string Filename = RS.Rows[0]["NombreArchivo"].ToString();
                RutaArchivo = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "contactoAnexos/" + CodCarpeta.ToString() + "/ContactoAnexo_" + usuario.IdContacto + "/" + Filename.ToString();

                Redirect(null, RutaArchivo, "_blank", null);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ningun documento de estudio adjunto a esta información académica ')", true);
            }

            RS.Dispose();
        }

        protected void ddl_DepartamentoDomicilio_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad(ddl_CiudadDomicilio, ddl_DepartamentoDomicilio);
        }
    }

    public class ListCiudades
    {
        public int codCiudad { get; set; }
        public string nomCiudad { get; set; }
    }

    #region AntiguoCODE
    //public partial class MiPerfil : Negocio.Base_Page
    //{
    //    String CodTipoInstitucion;
    //    int CodProyecto;
    //    String txtSQL = String.Empty;
    //    String RutaArchivo = String.Empty;



    //    protected void Page_Load(object sender, EventArgs e)
    //    {
    //        CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"] ?? 0);

    //        if (!IsPostBack)
    //        {
    //            Cargarddl_dedicacion2();
    //            lbl_Titulo.Text = void_establecerTitulo("MI PERFIL");
    //            switch (usuario.CodGrupo)
    //            {
    //                case 1:
    //                    //gerente Admin
    //                    ingresarGerenteAdmin();
    //                    PanelGerenteAdmin.Visible = true;

    //                    break;
    //                case 8:
    //                case 20:
    //                    pnlCallCenter.Visible = true;
    //                    var clave = (from c in consultas.Db.Clave
    //                                 where c.codContacto == usuario.IdContacto
    //                                 select c).ToList();

    //                    if (clave != null)
    //                    {
    //                        if (clave[clave.Count - 1].YaAvisoExpiracion == 1)
    //                        {
    //                            if (!IsPostBack)
    //                                Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (!IsPostBack)
    //                            Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
    //                    }
    //                    break;
    //                case 2:
    //                    if (!IsPostBack)
    //                        Admonfon.Visible = true;
    //                    break;
    //                case 4:
    //                    //Jefe de Unidad
    //                    ingresarDatosJefeUnidad();
    //                    PanelJefeUnidad.Visible = true;
    //                    string txtsql = "SELECT CodTipoInstitucion, NomInstitucion, Nit, RegistroIcfes, FechaRegistro, CodCiudad, Direccion, Telefono, Fax, Website FROM Institucion WHERE Id_Institucion = " + usuario.CodInstitucion;
    //                    var dt = consultas.ObtenerDataTable(txtsql, "text");
    //                    if (dt.Rows.Count > 0) { CodTipoInstitucion = dt.Rows[0]["CodTipoInstitucion"].ToString(); }
    //                    dt = null;
    //                    txtsql = null;
    //                    if (CodTipoInstitucion == "33") // si es 33, NO se muestran los campos "ICFES, etc".
    //                    {
    //                        tr_ICFES.Visible = false;
    //                        tr_FECHA_REGISTRO.Visible = false;
    //                    }

    //                    break;
    //                case 5:
    //                    //asesor
    //                    ingresarDatosAsesor();
    //                    PanelAsesor.Visible = true;
    //                    break;
    //                case 6:
    //                    //Emprendedor
    //                    ingresarDatosEmprendedor();
    //                    //validar el estado del proyecto 
    //                    if (codEstadoProyecto(CodProyecto) > 1)//Si ya paso el estado de Registro y Asesoria
    //                    {
    //                        h_addInfoacademica4.Visible = false;
    //                        Ibtn_adicionarIA4.Visible = false;
    //                    }

    //                    PanelEmprendedor.Visible = true;
    //                    break;
    //                case 9:
    //                    ingresardatosUs_GruposSelectos();
    //                    PanelGeneral.Visible = true;
    //                    break;
    //                case 10:
    //                    ingresardatosUs_GruposSelectos();
    //                    PanelGeneral.Visible = true;
    //                    break;
    //                case 11:
    //                    ingresarDatosEvaluador();
    //                    PanelEvaluador.Visible = true;
    //                    break;
    //                case 12:
    //                    ingresardatosUs_GruposSelectos();
    //                    PanelGeneral.Visible = true;
    //                    break;
    //                case 13:
    //                    ingresardatosUs_GruposSelectos();
    //                    PanelGeneral.Visible = true;
    //                    break;
    //                case 14:

    //                    tr_cedula.Visible = true;
    //                    tr_numIdentificacion.Visible = false;
    //                    tr_cargo.Visible = false;
    //                    lblCargo.Visible = false;
    //                    tx_cargo1.Visible = false;
    //                    tr_direccion.Visible = true;
    //                    tr_Celular.Visible = true;
    //                    tr_Departamento.Visible = true;
    //                    tr_Ciudad.Visible = true;
    //                    pnl_exp_interventor.Visible = true;

    //                    CargarEstudiosInterventor();
    //                    ingresardatosUsGeneral();
    //                    PanelGeneral.Visible = true;

    //                    break;
    //                case 15:
    //                    var clave2 = (from c in consultas.Db.Clave
    //                                  where c.codContacto == usuario.IdContacto
    //                                  select c).ToList();

    //                    if (clave2 != null)
    //                    {
    //                        if (clave2[clave2.Count - 1].YaAvisoExpiracion == 1)
    //                        {
    //                            if (!IsPostBack)
    //                                Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        if (!IsPostBack)
    //                            Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
    //                    }
    //                    //if (!IsPostBack)
    //                    //    Redirect(null, "~/FONADE/MiPerfil/CambiarClave.aspx", "_Blank", "width=580,height=300,toolbar=no, scrollbars=no, resizable=no");
    //                    break;
    //                case 16:
    //                    cargarDatosLiderRegional();
    //                    PanelAsesor.Visible = true;
    //                    break;

    //            }
    //        }

    //        string txtSQL = "SELECT Count(id_ContactoArchivosAnexos) FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto;

    //        var reader = consultas.ObtenerDataTable(txtSQL, "text");


    //        if (int.Parse(reader.Rows[0].ItemArray[0].ToString()) > 0)
    //        {
    //            verDocAdjunto.Visible = true;
    //        }

    //    }

    //    private void Cargarddl_dedicacion2()
    //    {
    //        ddl_decalracion2.Items.Insert(0, new ListItem("Completa", "0"));
    //        ddl_decalracion2.Items.Insert(0, new ListItem("Parcial", "1"));
    //    }

    //    protected void ingresarDatosAsesor()
    //    {
    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
    //        try
    //        {
    //            int CodigoCiudad = 111;

    //            SqlCommand cmd = new SqlCommand("MD_cargarAsesorMiPerfil", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
    //            con.Open();
    //            SqlDataReader r = cmd.ExecuteReader();
    //            r.Read();
    //            l_nombre2.Text = Convert.ToString(r["Nombres"]);
    //            l_apellido2.Text = Convert.ToString(r["Apellidos"]);
    //            l_identificacion2.Text = Convert.ToString(r["Identificacion"]);
    //            l_email2.Text = Convert.ToString(r["Email"]);
    //            tx_experiencia2.Text = Convert.ToString(r["Experiencia"]);
    //            ddl_decalracion2.SelectedValue = Convert.ToString(r["Dedicacion"]);
    //            tx_hojadevida2.Text = Convert.ToString(r["HojaVida"]);
    //            tx_interes2.Text = Convert.ToString(r["Intereses"]);
    //            try
    //            {
    //                CodigoCiudad = Convert.ToInt32(r["LugarExpedicionDI"]);
    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            cmd.Dispose();


    //            var query = from dept in consultas.Db.departamento
    //                        select new
    //                        {
    //                            Id_dpto = dept.Id_Departamento,
    //                            Nombre_dpto = dept.NomDepartamento,
    //                        };
    //            ddl_departamento2.DataSource = query.ToList();
    //            ddl_departamento2.DataTextField = "Nombre_dpto";
    //            ddl_departamento2.DataValueField = "Id_dpto";
    //            ddl_departamento2.DataBind();
    //            seleccionarDepartamento(ddl_departamento2, CodigoCiudad);
    //            llenarCiudad(ddl_ciudad2, ddl_departamento2);
    //            ddl_ciudad2.SelectedValue = CodigoCiudad.ToString();

    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //        finally
    //        {

    //            con.Close();
    //            con.Dispose();
    //        }
    //    }

    //    protected void ingresarDatosEvaluador()
    //    {
    //        string banco = "";
    //        string tipocuenta = "";
    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
    //        try
    //        {


    //            SqlCommand cmd = new SqlCommand("MD_cargarMiPerfilEvaluador", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
    //            con.Open();
    //            SqlDataReader r = cmd.ExecuteReader();
    //            r.Read();
    //            l_nombre6.Text = Convert.ToString(r["Nombres"]);
    //            l_apellidos6.Text = Convert.ToString(r["Apellidos"]);
    //            l_identificacion6.Text = Convert.ToString(r["identificacion"]);
    //            l_persona6.Text = Convert.ToString(r["Persona"]);
    //            l_email6.Text = Convert.ToString(r["email"]);
    //            txt_direccion6.Text = Convert.ToString(r["Direccion"]);
    //            txt_telefono6.Text = Convert.ToString(r["Telefono"]);
    //            txt_fax6.Text = Convert.ToString(r["fax"]);
    //            txt_numcuenta6.Text = Convert.ToString(r["Cuenta"]);
    //            txt_planes6.Text = Convert.ToString(r["MaximoPlanes"]);
    //            txt_expgeneral6.Text = Convert.ToString(r["Experiencia"]);
    //            txt_expintereses6.Text = Convert.ToString(r["Intereses"]);
    //            txt_secprincipal6.Text = Convert.ToString(r["ExperienciaPrincipal"]);
    //            txt_secsecundario6.Text = Convert.ToString(r["ExperienciaSecundaria"]);
    //            txt_hojadevida6.Text = Convert.ToString(r["HojaVida"]);
    //            banco = Convert.ToString(r["codBanco"]);
    //            tipocuenta = Convert.ToString(r["CodTipoCuenta"]);
    //            cmd.Dispose();
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //        finally
    //        {

    //            con.Close();
    //            con.Dispose();

    //        }
    //        llenarbancos(ddl_banco6);
    //        ddl_banco6.SelectedValue = banco;
    //        llenartipocuenta(ddl_tipocuenta6);
    //        ddl_tipocuenta6.SelectedValue = tipocuenta;
    //        llenarsectores(ddl_secprincipal6);
    //        ddl_secprincipal6.SelectedValue = llenarddlsector("P");
    //        llenarsectores(ddl_secsecundario6);
    //        ddl_secsecundario6.SelectedValue = llenarddlsector("S");

    //    }

    //    protected void llenarbancos(DropDownList lista)
    //    {
    //        var query = from bn in consultas.Db.Bancos
    //                    select new
    //                    {
    //                        Id_banco = bn.Id_Banco,
    //                        Nombre_banco = bn.nomBanco,
    //                    };
    //        lista.DataSource = query.ToList();
    //        lista.DataTextField = "Nombre_banco";
    //        lista.DataValueField = "Id_banco";
    //        lista.DataBind();
    //    }

    //    protected string llenarddlsector(string tiposector)
    //    {
    //        string retorno = "11";
    //        try
    //        {
    //            var query = (from x in consultas.Db.EvaluadorSectors
    //                         where x.CodContacto == usuario.IdContacto
    //                         && x.Experiencia == tiposector
    //                         select new
    //                         {
    //                             x.CodSector,
    //                         }).FirstOrDefault();
    //            retorno = query.CodSector.ToString();
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }

    //        return retorno;
    //    }

    //    protected void llenartipocuenta(DropDownList lista)
    //    {
    //        var query = from tc in consultas.Db.TipoCuentas
    //                    select new
    //                    {
    //                        Id_tc = tc.Id_TipoCuenta,
    //                        Nombre_tc = tc.NomTipoCuenta,
    //                    };
    //        lista.DataSource = query.ToList();
    //        lista.DataTextField = "Nombre_tc";
    //        lista.DataValueField = "Id_tc";
    //        lista.DataBind();
    //    }

    //    protected void llenarsectores(DropDownList lista)
    //    {
    //        var query = (from sc in consultas.Db.Sector
    //                     select new
    //                     {
    //                         Id_sec = sc.Id_Sector,
    //                         Nombre_sec = sc.NomSector,
    //                     }).OrderBy(e => e.Nombre_sec);

    //        lista.DataSource = query.ToList();
    //        lista.DataTextField = "Nombre_sec";
    //        lista.DataValueField = "Id_sec";
    //        lista.DataBind();
    //    }

    //    protected void ingresarDatosJefeUnidad()
    //    {
    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
    //        try
    //        {

    //            SqlCommand cmd = new SqlCommand("MD_CargarJefeUnidadMiPerfil", con);
    //            cmd.CommandType = CommandType.StoredProcedure;
    //            cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
    //            con.Open();
    //            SqlDataReader r = cmd.ExecuteReader();
    //            r.Read();
    //            int CodigoCiudadUnidad = 111;
    //            int CodigoCiudadEmprendedor = 111;
    //            try
    //            {
    //                l_nomcentro3.Text = Convert.ToString(r["NomInstitucionI"]);
    //                l_nit3.Text = Convert.ToString(r["NitI"]);
    //                tx_icfes3.Text = Convert.ToString(r["RegistroIcfesI"]);
    //                tx_fregistro2.Text = Convert.ToDateTime(r["FechaRegistroI"]).ToString("dd/MM/yyyy");
    //                l_correspondencia3.Text = Convert.ToString(r["DireccionI"]);
    //                tx_telunidad3.Text = Convert.ToString(r["TelefonoI"]);
    //                tx_faxunidad3.Text = Convert.ToString(r["FaxI"]);
    //                tx_sitioweb3.Text = Convert.ToString(r["WebsiteI"]);
    //                l_nombre3.Text = Convert.ToString(r["NombresC"]);
    //                l_apellido3.Text = Convert.ToString(r["Apellidos"]);
    //                l_identificacion3.Text = Convert.ToString(r["IdentificacionC"]);
    //                l_email3.Text = Convert.ToString(r["EmailC"]);
    //                tx_cargo3.Text = Convert.ToString(r["CargoC"]);
    //                tx_telefono3.Text = Convert.ToString(r["TelefonoC"]);
    //                tx_fax3.Text = Convert.ToString(r["FaxC"]);
    //                try
    //                {
    //                    CodigoCiudadUnidad = Convert.ToInt32(r["CodCiudadI"]);
    //                }
    //                catch (Exception ex) { errorMessageDetail = ex.Message; }
    //                try
    //                {
    //                    CodigoCiudadEmprendedor = Convert.ToInt32(r["LugarExpedicionDIC"]);
    //                }
    //                catch (Exception ex) { errorMessageDetail = ex.Message; }

    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            cmd.Dispose();

    //            var query = from dept in consultas.Db.departamento
    //                        select new
    //                        {
    //                            Id_dpto1 = dept.Id_Departamento,
    //                            Nombre_dpto1 = dept.NomDepartamento,
    //                        };
    //            ddl_deparunidad3.DataSource = query.ToList();
    //            ddl_deparunidad3.DataTextField = "Nombre_dpto1";
    //            ddl_deparunidad3.DataValueField = "Id_dpto1";
    //            ddl_deparunidad3.DataBind();
    //            seleccionarDepartamento(ddl_deparunidad3, CodigoCiudadUnidad);
    //            llenarCiudad(ddl_ciudadunidad3, ddl_deparunidad3);
    //            ddl_ciudadunidad3.SelectedValue = CodigoCiudadUnidad.ToString();

    //            var query2 = from dept2 in consultas.Db.departamento
    //                         select new
    //                         {
    //                             Id_dpto2 = dept2.Id_Departamento,
    //                             Nombre_dpto2 = dept2.NomDepartamento,
    //                         };
    //            ddl_departamento3.DataSource = query2.ToList();
    //            ddl_departamento3.DataTextField = "Nombre_dpto2";
    //            ddl_departamento3.DataValueField = "Id_dpto2";
    //            ddl_departamento3.DataBind();
    //            seleccionarDepartamento(ddl_departamento3, CodigoCiudadEmprendedor);
    //            llenarCiudad(ddl_ciudad3, ddl_departamento3);
    //            ddl_ciudad3.SelectedValue = CodigoCiudadEmprendedor.ToString();

    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //        finally
    //        {
    //            con.Close();
    //            con.Dispose();
    //        }
    //    }

    //    protected void ingresarGerenteAdmin()
    //    {
    //        l_nombres5.Text = usuario.Nombres;
    //        l_apellidos5.Text = usuario.Apellidos;
    //        l_email5.Text = usuario.Email;
    //        l_identificacion5.Text = usuario.Identificacion.ToString();
    //    }

    //    protected void ingresardatosUsGeneral()
    //    {
    //        var query = (from c in consultas.Db.Contacto
    //                     where c.Id_Contacto == usuario.IdContacto
    //                     select new
    //                     {
    //                         c,
    //                     }).FirstOrDefault();

    //        l_nombre1.Text = usuario.Nombres;
    //        l_apellido1.Text = usuario.Apellidos;
    //        l_email1.Visible = true;
    //        l_email1.Text = usuario.Email;
    //        l_identificacion1.Text = usuario.Identificacion.ToString();
    //        tx_cargo1.Text = query.c.Cargo;
    //        tx_telefono1.Text = query.c.Telefono;
    //        tx_fax1.Text = query.c.Fax;

    //        lbl_Cedula.Text = query.c.Identificacion.ToString();
    //        lbl_Email.Visible = true;
    //        lbl_Email.Text = "Email:";
    //        txtDireccion.Text = query.c.Direccion;

    //        var queryDepartamentos = (from departamentos in consultas.Db.departamento
    //                                  select new
    //                                  {
    //                                      Id_dpto1 = departamentos.Id_Departamento,
    //                                      Nombre_dpto1 = departamentos.NomDepartamento,
    //                                  }).ToList();

    //        dd_deptos.DataSource = queryDepartamentos;
    //        dd_deptos.DataTextField = "Nombre_dpto1";
    //        dd_deptos.DataValueField = "Id_dpto1";
    //        dd_deptos.DataBind();

    //        if (query.c.CodCiudad != null)
    //        {
    //            dd_deptos.Items.FindByValue(query.c.Ciudad.CodDepartamento.ToString()).Selected = true;
    //        }
    //        llenarCiudad(dd_ciudad, dd_deptos);

    //        if (query.c.Ciudad != null)
    //        {
    //            //seleccionarDepartamento(dd_ciudad, query.c.Ciudad.Id_Ciudad);

    //            dd_ciudad.Items.FindByValue(query.c.Ciudad != null ? query.c.Ciudad.Id_Ciudad.ToString() : "").Selected = true;
    //        }

    //        var rstContacto = consultas.ObtenerDataTable("SELECT * FROM Interventor WHERE codContacto = " + usuario.IdContacto, "text");

    //        if (rstContacto.Rows.Count > 0)
    //        {
    //            try
    //            {
    //                txtCelular.Text = rstContacto.Rows[0]["Celular"].ToString();
    //                txt_exp_sector_principal.Text = rstContacto.Rows[0]["ExperienciaPrincipal"].ToString();
    //                txt_exp_sector_secundario.Text = rstContacto.Rows[0]["ExperienciaSecundaria"].ToString();
    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //        }

    //        rstContacto = null;

    //        txt_exp_int_profesional.Text = query.c.Experiencia;
    //        txt_int_res_HV.Text = query.c.HojaVida;
    //        txt_exp_int_experi_intere.Text = query.c.Intereses;

    //        llenarsectores(dd_sector_princ_int);
    //        llenarsectores(dd_sector_second_int);

    //        var a = consultas.ObtenerDataTable("SELECT codSector FROM InterventorSector WHERE codContacto='" + usuario.IdContacto + "' and Experiencia='P'", "text");
    //        if (a.Rows.Count > 0) { dd_sector_princ_int.SelectedValue = a.Rows[0]["codSector"].ToString(); }
    //        a = null;
    //        var b = consultas.ObtenerDataTable("SELECT codSector FROM InterventorSector WHERE codContacto='" + usuario.IdContacto + "' and Experiencia='S'", "text");
    //        if (b.Rows.Count > 0) { dd_sector_second_int.SelectedValue = b.Rows[0]["codSector"].ToString(); }
    //        b = null;
    //    }

    //    private void cargarDatosLiderRegional()
    //    {
    //        var lider = (from c in consultas.Db.Contacto
    //                     where c.Id_Contacto == usuario.IdContacto
    //                     select c).FirstOrDefault();

    //        l_nombre2.Text = lider.Nombres;
    //        l_apellido2.Text = lider.Apellidos;
    //        l_identificacion2.Text = lider.Identificacion.ToString();
    //        l_email2.Text = lider.Email;
    //        tx_experiencia2.Text = lider.Experiencia;
    //        ddl_decalracion2.SelectedValue = lider.Dedicacion;
    //        tx_hojadevida2.Text = lider.HojaVida;
    //        tx_interes2.Text = lider.Intereses;
    //        if (!string.IsNullOrEmpty(lider.Dedicacion))
    //        {
    //            ddl_decalracion2.SelectedValue = lider.Dedicacion;
    //        }

    //        var query = from dept in consultas.Db.departamento
    //                    select new
    //                    {
    //                        Id_dpto = dept.Id_Departamento,
    //                        Nombre_dpto = dept.NomDepartamento,
    //                    };
    //        ddl_departamento2.DataSource = query.ToList();
    //        ddl_departamento2.DataTextField = "Nombre_dpto";
    //        ddl_departamento2.DataValueField = "Id_dpto";
    //        ddl_departamento2.DataBind();
    //        ddl_departamento2.Items.Insert(0, new ListItem("Seleccione", "0"));

    //        ddlRegioanles.DataSource = query.ToList();
    //        ddlRegioanles.DataTextField = "Nombre_dpto";
    //        ddlRegioanles.DataValueField = "Id_dpto";
    //        ddlRegioanles.DataBind();
    //        ddlRegioanles.SelectedValue = lider.CodTipoAprendiz.ToString();
    //        rowRegional.Visible = true;

    //        var ciudad = (from c in consultas.Db.Ciudad
    //                      where c.Id_Ciudad == lider.CodCiudad
    //                      select c).FirstOrDefault();
    //        if (ciudad != null)
    //        {
    //            var dpto = (from d in consultas.Db.departamento
    //                        where d.Id_Departamento == ciudad.CodDepartamento
    //                        select d).FirstOrDefault();

    //            var ciudades = (from c in consultas.Db.Ciudad
    //                            where c.CodDepartamento == dpto.Id_Departamento
    //                            select c).ToList();
    //            ddl_ciudad2.DataSource = ciudades;
    //            ddl_ciudad2.DataTextField = "NomCiudad";
    //            ddl_ciudad2.DataValueField = "Id_Ciudad";
    //            ddl_ciudad2.DataBind();

    //            ddl_departamento2.SelectedValue = dpto.Id_Departamento.ToString();

    //            ddl_ciudad2.SelectedValue = ciudad.Id_Ciudad.ToString();
    //        }
    //    }

    //    protected void ingresarDatosEmprendedor()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {
    //                int CodigoCiudadNacimiento = 111;
    //                int CodigoCiudadexped = 111;
    //                int CodigoCiudadDomicilio = 0;

    //                SqlCommand cmd = new SqlCommand("MD_cargarMiPerfilEmprendedor", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@id_contacto", usuario.IdContacto);
    //                con.Open();
    //                SqlDataReader r = cmd.ExecuteReader();
    //                r.Read();
    //                l_nombre4.Text = Convert.ToString(r["Nombres"]);
    //                l_apellidos4.Text = Convert.ToString(r["Apellidos"]);
    //                l_identificacion4.Text = Convert.ToString(r["Identificacion"]);
    //                l_email4.Text = r["Email"].ToString(); // Convert.ToString();
    //                if (!string.IsNullOrEmpty(r["FechaNacimiento"].ToString()))
    //                {
    //                    tx_fechanacimiento4.Text = Convert.ToDateTime(r["FechaNacimiento"]).ToString("dd/MM/yyyy");
    //                }
    //                tx_telefono4.Text = Convert.ToString(r["Telefono"]);
    //                txtDireccionEmprendedor.Text = Convert.ToString(r["Direccion"]);
    //                ddl_genero4.SelectedValue = Convert.ToString(r["Genero"]);
    //                CodigoCiudadNacimiento = Convert.ToInt32(r["CodCiudad"]);
    //                CodigoCiudadexped = Convert.ToInt32(r["LugarExpedicionDI"]);
    //                CodigoCiudadDomicilio = Convert.ToInt32(r["CodCiudadResidencia"]);
    //                cmd.Dispose();

    //                var query = from dept in consultas.Db.departamento
    //                            select new
    //                            {
    //                                Id_dpto1 = dept.Id_Departamento,
    //                                Nombre_dpto1 = dept.NomDepartamento,
    //                            };
    //                ddl_depexped4.DataSource = query.ToList();
    //                ddl_depexped4.DataTextField = "Nombre_dpto1";
    //                ddl_depexped4.DataValueField = "Id_dpto1";
    //                ddl_depexped4.DataBind();
    //                seleccionarDepartamento(ddl_depexped4, CodigoCiudadexped);
    //                llenarCiudad(dd_ciuexp4, ddl_depexped4);
    //                dd_ciuexp4.SelectedValue = CodigoCiudadexped.ToString();

    //                var query2 = from dept2 in consultas.Db.departamento
    //                             select new
    //                             {
    //                                 Id_dpto2 = dept2.Id_Departamento,
    //                                 Nombre_dpto2 = dept2.NomDepartamento,
    //                             };
    //                ddl_departamento4.DataSource = query2.ToList();
    //                ddl_departamento4.DataTextField = "Nombre_dpto2";
    //                ddl_departamento4.DataValueField = "Id_dpto2";
    //                ddl_departamento4.DataBind();
    //                seleccionarDepartamento(ddl_departamento4, CodigoCiudadNacimiento);
    //                llenarCiudad(ddl_ciudad4, ddl_departamento4);
    //                ddl_ciudad4.SelectedValue = CodigoCiudadNacimiento.ToString();

    //                //cargar Departamento y ciudad de domicilio
    //                var queryDptoDomicilio = (from dept2 in consultas.Db.departamento
    //                                          select new
    //                                          {
    //                                              Id_dptoDomicilio = dept2.Id_Departamento,
    //                                              Nombre_dptoDomicilio = dept2.NomDepartamento
    //                                          }).ToList();

    //                queryDptoDomicilio.Add(new { Id_dptoDomicilio = 0, Nombre_dptoDomicilio = "Seleccione..." });

    //                ddl_DepartamentoDomicilio.DataSource = queryDptoDomicilio;
    //                ddl_DepartamentoDomicilio.DataTextField = "Nombre_dptoDomicilio";
    //                ddl_DepartamentoDomicilio.DataValueField = "Id_dptoDomicilio";
    //                ddl_DepartamentoDomicilio.DataBind();
    //                seleccionarDepartamento(ddl_DepartamentoDomicilio, CodigoCiudadDomicilio);
    //                llenarCiudad(ddl_CiudadDomicilio, ddl_DepartamentoDomicilio);
    //                ddl_CiudadDomicilio.SelectedValue = CodigoCiudadDomicilio.ToString();

    //            }
    //            catch (SqlException ex) { errorMessageDetail = ex.Message; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();
    //            }
    //        }
    //    }

    //    /// <summary>
    //    ///Este método se aplica a los siguientes grupos:
    //    /// 9 = Gerente Evaluador
    //    /// 10 = Coordinador de Evaluadores
    //    /// 12 = Gerente Interventor
    //    /// 13 = Coordinar de Interventor
    //    /// </summary>
    //    protected void ingresardatosUs_GruposSelectos()
    //    {
    //        tb_PanelAsesor.Visible = false;
    //        tb_PanelJefeUnidad.Visible = false;
    //        tb_PanelEmprendedor.Visible = false;
    //        tb_PanelGerenteAdmin.Visible = false;
    //        tb_PanelEvaluador.Visible = false;

    //        var query = (from c in consultas.Db.Contacto
    //                     where c.Id_Contacto == usuario.IdContacto
    //                     select new
    //                     {
    //                         c,
    //                     }).FirstOrDefault();

    //        l_nombre1.Text = usuario.Nombres;
    //        l_apellido1.Text = usuario.Apellidos;
    //        l_identificacion1.Text = usuario.Identificacion.ToString();
    //        l_email1.Text = usuario.Email;
    //        tx_cargo1.Text = query.c.Cargo;
    //        tx_telefono1.Text = query.c.Telefono;
    //        tx_fax1.Text = query.c.Fax;

    //    }

    //    /// <summary>
    //    /// 9 = Gerente Evaluador
    //    /// 10 = Coordinador de Evaluadores
    //    /// 12 = Gerente Interventor
    //    /// 13 = Coordinar de Interventor
    //    /// </summary>
    //    protected void ingresarUsuarioFiduciaria()
    //    {
    //        //Ocultar los demás campos.
    //        tb_PanelAsesor.Visible = false;
    //        tb_PanelJefeUnidad.Visible = false;
    //        tb_PanelEmprendedor.Visible = false;
    //        tb_PanelGerenteAdmin.Visible = false;
    //        tb_PanelEvaluador.Visible = false;

    //        //Consulta.
    //        var query = (from c in consultas.Db.Contacto
    //                     where c.Id_Contacto == usuario.IdContacto
    //                     select new
    //                     {
    //                         c,
    //                     }).FirstOrDefault();

    //        l_nombre1.Text = usuario.Nombres;
    //        l_apellido1.Text = usuario.Apellidos;
    //        l_identificacion1.Text = usuario.Identificacion.ToString();
    //        l_email1.Text = usuario.Email;
    //        tx_cargo1.Text = query.c.Cargo;
    //        tx_telefono1.Text = query.c.Telefono;
    //        tx_fax1.Text = query.c.Fax;

    //    }

    //    protected void ddl_departamento2_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(ddl_ciudad2, ddl_departamento2);
    //    }

    //    protected void seleccionarDepartamento(DropDownList dtoaSeleccionar, int codigoCiudad)
    //    {
    //        try
    //        {
    //            if (codigoCiudad!=0)
    //            {
    //                var query = (from CIud in consultas.Db.Ciudad
    //                             where CIud.Id_Ciudad == codigoCiudad
    //                             select new
    //                             {
    //                                 Id_departamento = CIud.CodDepartamento,
    //                             }).FirstOrDefault();
    //                dtoaSeleccionar.SelectedValue = Convert.ToString(query.Id_departamento);
    //            }
    //            else
    //            {
    //                dtoaSeleccionar.SelectedValue = "0";
    //            }

    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //    }


    //    protected void llenarCiudad(DropDownList listaciudad, DropDownList departamentolist)
    //    {
    //        try
    //        {
    //            List<ListCiudades> queryCiudades = new List<ListCiudades>();

    //            if (Convert.ToInt32(departamentolist.SelectedValue)!=0)
    //            {
    //                queryCiudades = (from ciudades in consultas.Db.Ciudad
    //                                    where ciudades.CodDepartamento == Convert.ToInt32(departamentolist.SelectedValue)
    //                                    select new ListCiudades
    //                                    {
    //                                        codCiudad = ciudades.Id_Ciudad,
    //                                        nomCiudad = ciudades.NomCiudad
    //                                    }).ToList();
    //            }
    //            else
    //            {
    //                queryCiudades.Add(new ListCiudades { codCiudad = 0, nomCiudad = "Seleccione..." });
    //            }                

    //            listaciudad.DataSource = queryCiudades;
    //            listaciudad.DataTextField = "nomCiudad";
    //            listaciudad.DataValueField = "codCiudad";
    //            listaciudad.DataBind();
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //    }

    //    protected void lds_estudios_Selecting(object sender, LinqDataSourceSelectEventArgs e)
    //    {
    //        try
    //        {
    //            var query = from P in consultas.Db.MD_VerEstudiosAsesor(usuario.IdContacto)
    //                        select P;

    //            var query2 = (from q in query
    //                         select new {
    //                             q.AnoTitulo,
    //                             q.CodCiudad,
    //                             q.codprogramaAcademico,
    //                             q.Finalizado,
    //                             q.FlagIngresadoAsesor,
    //                             q.Id_ContactoEstudio,
    //                             q.Institucion,
    //                             q.NomCiudad,
    //                             q.NomDepartamento,
    //                             q.NomNivelEstudio,
    //                             q.TituloObtenido,
    //                             q.URL,
    //                             Habilitado = habilitadoProyecto() }).ToList();

    //            e.Result = query2;
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //    }

    //    private bool habilitadoProyecto()
    //    {
    //        bool habilitado = true;

    //        if (codEstadoProyecto(CodProyecto)>1) //Registro y Asesoria - 1
    //        {
    //            habilitado = false;
    //        }

    //        return habilitado;
    //    }

    //    /// <summary>
    //    /// Cargar el GridView "gv_infoAcademic_Interventor" con los estudios del inteventor.
    //    /// </summary>
    //    private void CargarEstudiosInterventor()
    //    {
    //        String txtSQL;
    //        DataTable RS = new DataTable();

    //        try
    //        {
    //            txtSQL = " SELECT CE.Id_ContactoEstudio, CE.TituloObtenido, CE.AnoTitulo,CE.Finalizado, CE.Institucion, CE.CodCiudad, C.NomCiudad, D.NomDepartamento, NE.NomNivelEstudio" +
    //                     " FROM ContactoEstudio CE, Ciudad C, Departamento D, NivelEstudio NE" +
    //                     " WHERE CE.CodCiudad = C.ID_Ciudad" +
    //                     " AND C.CodDepartamento = D.Id_Departamento" +
    //                     " AND CE.CodNivelEstudio = NE.Id_NivelEstudio" +
    //                     " AND codcontacto = " + usuario.IdContacto +
    //                     " ORDER BY CE.Finalizado,CE.AnoTitulo Desc";

    //            RS = consultas.ObtenerDataTable(txtSQL, "text");

    //            this.gv_infoAcademic_Interventor.DataSource = RS;
    //            this.gv_infoAcademic_Interventor.DataBind();
    //        }
    //        catch { }
    //    }

    //    protected void btn_actualizar2_Click(object sender, EventArgs e)
    //    {
    //        updateAsesor();
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
    //    }

    //    protected void Eliminar_Estudios_Realizados(object sender, CommandEventArgs e)
    //    {
    //        try
    //        {
    //            int codigo_estudio = Convert.ToInt32(e.CommandArgument);
    //            var query = (from Estudio in consultas.Db.ContactoEstudios
    //                         where Estudio.FlagIngresadoAsesor == 1
    //                         & Estudio.Id_ContactoEstudio == codigo_estudio
    //                         select Estudio.FlagIngresadoAsesor).Count();

    //            if (query == 0)
    //            {
    //                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //                {
    //                    try
    //                    {
    //                        SqlCommand cmd = new SqlCommand("MD_EliminarEstudioRealizado", con);
    //                        cmd.CommandType = CommandType.StoredProcedure;
    //                        cmd.Parameters.AddWithValue("@id_estudio", codigo_estudio);
    //                        SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                        con.Open();
    //                        cmd2.ExecuteNonQuery();
    //                        cmd.ExecuteNonQuery();
    //                        cmd2.Dispose();
    //                        cmd.Dispose();
    //                        this.gvestudiosrealizadosasesor.DataBind();
    //                    }
    //                    catch (SqlException ex) { errorMessageDetail = ex.Message; }
    //                    finally
    //                    {
    //                        con.Close();
    //                        con.Dispose();
    //                    }
    //                }


    //            }
    //            else
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No ha sido posible eliminar esta información académica')", true);
    //            }
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //    }

    //    protected void Eliminar_Estudios_Emprendedor(object sender, CommandEventArgs e)
    //    {
    //        try
    //        {
    //            int codigo_estudio = Convert.ToInt32(e.CommandArgument);
    //            var query = (from Estudio in consultas.Db.ContactoEstudios
    //                         where Estudio.FlagIngresadoAsesor == 1
    //                         & Estudio.Id_ContactoEstudio == codigo_estudio
    //                         select Estudio.FlagIngresadoAsesor).Count();

    //            if (query == 0)
    //            {
    //                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //                {
    //                    try
    //                    {
    //                        SqlCommand cmd = new SqlCommand("MD_EliminarEstudioRealizado", con);
    //                        cmd.CommandType = CommandType.StoredProcedure;
    //                        cmd.Parameters.AddWithValue("@id_estudio", codigo_estudio);
    //                        SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                        con.Open();
    //                        cmd2.ExecuteNonQuery();
    //                        cmd.ExecuteNonQuery();
    //                        cmd2.Dispose();
    //                        cmd.Dispose();
    //                        this.gvestudiosemprendedor.DataBind();
    //                    }
    //                    finally
    //                    {
    //                        con.Close();
    //                        con.Dispose();
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No ha sido posible eliminar esta información académica')", true);
    //            }
    //        }
    //        catch (Exception ex) { errorMessageDetail = ex.Message; }
    //    }

    //    protected void eliminararchivos()
    //    {
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Funciona!')", true);
    //    }

    //    protected void btn_actualizar4_Click(object sender, EventArgs e)
    //    {
    //        if (validarCamposEmprendedor())
    //        {
    //            updateEmprendedor();
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
    //        }                        
    //    }

    //    private bool validarCamposEmprendedor()
    //    {
    //        bool valido = true;

    //        if (txtDireccionEmprendedor.Text=="")
    //        {
    //            valido = false;
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe ingresar la direccion de domicilio!'); document.location=('MiPerfil.aspx');", true);
    //        }

    //        if (Convert.ToInt32(ddl_CiudadDomicilio.SelectedValue) == 0)
    //        {
    //            valido = false;
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar una ciudad!');", true);
    //        }

    //        return valido;
    //    }

    //    protected void ddl_departamento3_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(ddl_ciudad3, ddl_departamento3);
    //    }

    //    protected void ddl_deparunidad3_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(ddl_ciudadunidad3, ddl_deparunidad3);
    //    }

    //    protected void ddl_depexped4_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(dd_ciuexp4, ddl_depexped4);
    //    }

    //    protected void dd_deptos_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(dd_ciudad, dd_deptos);
    //    }

    //    protected void ddl_departamento4_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(ddl_ciudad4, ddl_departamento4);
    //    }

    //    public string _cadenaConex = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

    //    private int codEstadoProyecto(int codProyecto)
    //    {
    //        int codEstado = 0;

    //        using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
    //        {

    //            codEstado = (from p in db.Proyecto1s
    //                         where p.Id_Proyecto == codProyecto
    //                         select p.CodEstado).FirstOrDefault();
    //        }

    //        return codEstado;
    //    }

    //    protected void Image1_Click(object sender, ImageClickEventArgs e)
    //    {
    //        int codRol = 0;
    //        int codProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
    //        using (FonadeDBDataContext db = new FonadeDBDataContext(_cadenaConex))
    //        {
    //            codRol = (from pc in db.ProyectoContactos
    //                      where pc.CodContacto == usuario.IdContacto
    //                      select pc.CodRol).FirstOrDefault();
    //        }

    //        if (codRol == 3) //3 Rol Emprendedor
    //        {
    //            if (codEstadoProyecto(codProyecto) == 1) // 1 Estado Registro y Asesoria
    //            {
    //                var user = usuario.IdContacto;
    //                HttpContext.Current.Session["USER"] = user;
    //                HttpContext.Current.Session["CodProyecto"] = codProyecto;
    //                HttpContext.Current.Session["TipoDocumento"] = "FotocopiaDocumento";
    //                HttpContext.Current.Session["idCertificacionEstudios"] = 0;
    //                Session["Cedula"] = "CC_" + usuario.Identificacion;
    //                Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
    //            }
    //            else //Si ya el proyecto NO esta en Registro y Asesoria
    //            {
    //                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje"
    //                    , "alert('Este proyecto ya no se encuentra en Registro y Asesoría, por lo tanto no está habilitado para actualizar el documento de identificacion!'); ", true);
    //            }

    //        }
    //        else //Si es diferente a un emprendedor
    //        {
    //            var user = usuario.IdContacto;
    //            HttpContext.Current.Session["USER"] = user;
    //            HttpContext.Current.Session["CodProyecto"] = codProyecto;
    //            HttpContext.Current.Session["TipoDocumento"] = "FotocopiaDocumento";
    //            HttpContext.Current.Session["idCertificacionEstudios"] = 0;
    //            Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
    //        }
    //    }


    //    protected void btn_actualizar3_Click(object sender, EventArgs e)
    //    {
    //        updateJefeUnidad();
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
    //    }

    //    protected void updateJefeUnidad()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {
    //                SqlCommand cmd = new SqlCommand("MD_Update_JefeUnidad", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@icfes", tx_icfes3.Text);
    //                cmd.Parameters.AddWithValue("@fechaderegistro", Convert.ToDateTime(tx_fregistro2.Text).ToShortDateString());
    //                cmd.Parameters.AddWithValue("@telefonoInst", tx_telunidad3.Text);
    //                cmd.Parameters.AddWithValue("@faxInst", tx_faxunidad3.Text);
    //                cmd.Parameters.AddWithValue("@web", tx_sitioweb3.Text);
    //                cmd.Parameters.AddWithValue("@ciudadInst", Convert.ToInt32(ddl_ciudadunidad3.SelectedValue));
    //                cmd.Parameters.AddWithValue("@idInstitucion", usuario.CodInstitucion);
    //                cmd.Parameters.AddWithValue("@cargo", tx_cargo3.Text);
    //                cmd.Parameters.AddWithValue("@telefono", tx_telefono3.Text);
    //                cmd.Parameters.AddWithValue("@fax", tx_fax3.Text);
    //                cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad3.SelectedValue));
    //                cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
    //                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                con.Open();
    //                cmd2.ExecuteNonQuery();
    //                cmd.ExecuteNonQuery();
    //                cmd2.Dispose();
    //                cmd.Dispose();

    //            }
    //            catch (Exception ex)
    //            { throw; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();

    //            }
    //        }

    //    }

    //    protected void updateEmprendedor()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {
    //                SqlCommand cmd = new SqlCommand("MD_Update_Emprendedor", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@ciudadexpedicion", Convert.ToInt32(dd_ciuexp4.SelectedValue));
    //                cmd.Parameters.AddWithValue("@telefono", tx_telefono4.Text);
    //                cmd.Parameters.AddWithValue("@direccion", txtDireccionEmprendedor.Text);
    //                cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad4.SelectedValue));
    //                cmd.Parameters.AddWithValue("@ciudadResidencia", Convert.ToInt32(ddl_CiudadDomicilio.SelectedValue));                    
    //                cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
    //                cmd.Parameters.AddWithValue("@genero", ddl_genero4.SelectedValue);
    //                cmd.Parameters.AddWithValue("@fechanacimiento", Convert.ToDateTime(tx_fechanacimiento4.Text).ToString("yyyy-MM-dd"));
    //                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                con.Open();
    //                cmd2.ExecuteNonQuery();
    //                cmd.ExecuteNonQuery();
    //                cmd2.Dispose();
    //                cmd.Dispose();

    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();
    //            }
    //        }

    //    }

    //    protected void updateAsesor()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {
    //                var valida = true;
    //                if (usuario.CodGrupo == 16)
    //                {
    //                    valida = ValidarCombos();
    //                }
    //                if (valida)
    //                {
    //                    SqlCommand cmd = new SqlCommand("MD_Update_Asesor", con);
    //                    cmd.CommandType = CommandType.StoredProcedure;
    //                    cmd.Parameters.AddWithValue("@dedicacion", ddl_decalracion2.SelectedValue);
    //                    cmd.Parameters.AddWithValue("@experiencia", tx_experiencia2.Text);
    //                    cmd.Parameters.AddWithValue("@hojadevida", tx_hojadevida2.Text);
    //                    cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
    //                    cmd.Parameters.AddWithValue("@intereses", tx_interes2.Text);
    //                    cmd.Parameters.AddWithValue("@ciudad", Convert.ToInt32(ddl_ciudad2.SelectedValue));
    //                    SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                    con.Open();
    //                    cmd2.ExecuteNonQuery();
    //                    cmd.ExecuteNonQuery();
    //                    con.Close();
    //                    con.Dispose();
    //                    cmd2.Dispose();
    //                    cmd.Dispose();

    //                    if (usuario.CodGrupo == Constantes.CONST_LiderRegional)
    //                    {
    //                        var lider = (from c in consultas.Db.Contacto
    //                                     where c.Id_Contacto == usuario.IdContacto
    //                                     select c).FirstOrDefault();
    //                        lider.CodCiudad = int.Parse(ddl_ciudad2.SelectedValue);
    //                        lider.Dedicacion = ddl_decalracion2.SelectedValue.Trim();
    //                        consultas.Db.SubmitChanges();
    //                    }
    //                }

    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();

    //            }
    //        }

    //    }

    //    private bool ValidarCombos()
    //    {
    //        var resp = false;
    //        if (ddl_departamento2.SelectedIndex != 0)
    //        {
    //            resp = true;
    //        }
    //        else
    //        {
    //            resp = false;
    //            ddl_departamento2.Focus();
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar el departamento de expedición del documento!'); ", true);
    //        }

    //        return resp;
    //    }

    //    protected void updateUsGeneral()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {

    //                SqlCommand cmd = new SqlCommand("MD_Update_UsGeneral", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@cargo", tx_cargo1.Text);
    //                cmd.Parameters.AddWithValue("@telefono", tx_telefono1.Text);
    //                cmd.Parameters.AddWithValue("@fax", tx_fax1.Text);
    //                cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
    //                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                con.Open();
    //                cmd2.ExecuteNonQuery();
    //                cmd.ExecuteNonQuery();
    //                cmd2.Dispose();
    //                cmd.Dispose();

    //                var queryInterventor = (from c in consultas.Db.Contacto
    //                                        where c.Id_Contacto == usuario.IdContacto
    //                                        select c).FirstOrDefault();

    //                if (queryInterventor != null)
    //                {
    //                    queryInterventor.CodCiudad = Convert.ToInt32(dd_ciudad.SelectedValue);
    //                    consultas.Db.SubmitChanges();
    //                }
    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();
    //            }
    //        }

    //    }

    //    protected void updateEvaluador()
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
    //        {
    //            try
    //            {
    //                SqlCommand cmd = new SqlCommand("MD_Update_Evaluador", con);
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@telefono", txt_telefono6.Text);
    //                cmd.Parameters.AddWithValue("@direccion", txt_direccion6.Text);
    //                cmd.Parameters.AddWithValue("@experiencia", txt_expgeneral6.Text);
    //                cmd.Parameters.AddWithValue("@intereses", txt_expintereses6.Text);
    //                cmd.Parameters.AddWithValue("@hojavida", txt_hojadevida6.Text);
    //                cmd.Parameters.AddWithValue("@fax", txt_fax6.Text);
    //                cmd.Parameters.AddWithValue("@codBanco", Convert.ToInt32(ddl_banco6.SelectedValue));
    //                cmd.Parameters.AddWithValue("@CodTipocuenta", Convert.ToInt32(ddl_tipocuenta6.SelectedValue));
    //                cmd.Parameters.AddWithValue("@txtNumCuenta", txt_numcuenta6.Text);
    //                cmd.Parameters.AddWithValue("@MaximoPlanes", Convert.ToInt16(txt_planes6.Text));
    //                cmd.Parameters.AddWithValue("@txtExpPrincipal", txt_secprincipal6.Text);
    //                cmd.Parameters.AddWithValue("@txtExpSecundaria", txt_secsecundario6.Text);
    //                cmd.Parameters.AddWithValue("@codSectorPri", Convert.ToInt32(ddl_secprincipal6.SelectedValue));
    //                cmd.Parameters.AddWithValue("@codSectorSec", Convert.ToInt32(ddl_secsecundario6.SelectedValue));
    //                cmd.Parameters.AddWithValue("@idcontacto", usuario.IdContacto);
    //                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
    //                con.Open();
    //                cmd2.ExecuteNonQuery();
    //                cmd.ExecuteNonQuery();
    //                cmd2.Dispose();
    //                cmd.Dispose();
    //            }
    //            catch (Exception ex) { errorMessageDetail = ex.Message; }
    //            finally
    //            {
    //                con.Close();
    //                con.Dispose();
    //            }
    //        }

    //    }

    //    protected void btn_actualizar1_Click(object sender, EventArgs e)
    //    {
    //        updateUsGeneral();
    //        if (usuario.CodGrupo == Constantes.CONST_Interventor) //Si es interventor
    //        {
    //            updateInterventor(usuario.IdContacto);
    //        }
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
    //    }

    //    private void ActualizarSectorInterventor(DropDownList ddl, string tipo, int _codUsuario)
    //    {
    //        //Tabla InterventorSector Se hace de esta manera porque la tabla no cuenta con PrimaryKey
    //        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);

    //        string fechaHora = DateTime.Now.Year + "-"
    //                            + DateTime.Now.Month + "-"
    //                            + DateTime.Now.Day + " " + DateTime.Now.ToShortTimeString().Replace(".", "");


    //        string sqlText = "update InterventorSector " +
    //                         "set CodSector = " + ddl.SelectedValue + " " +
    //                         ", fechaActualizacion = cast('" + fechaHora + "' as datetime) " +
    //                         "where CodContacto = " + _codUsuario.ToString() + " and Experiencia = '" + tipo + "'";

    //        SqlCommand cmd = new SqlCommand(sqlText, con);
    //        cmd.CommandType = CommandType.Text;
    //        con.Open();
    //        cmd.ExecuteNonQuery();
    //        cmd.Dispose();
    //        con.Close();
    //        con.Dispose();
    //    }

    //    private void updateInterventor(int codUsuario)
    //    {

    //        //Actualizar Sector Principal P
    //        ActualizarSectorInterventor(dd_sector_princ_int, "P", codUsuario);

    //        //Actualizar Sector Secundario S
    //        ActualizarSectorInterventor(dd_sector_second_int, "S", codUsuario);

    //        //Tabla Contacto
    //        var queryContacto = (from c in consultas.Db.Contacto
    //                             where c.Id_Contacto == codUsuario
    //                             select c).FirstOrDefault();

    //        if (queryContacto != null)
    //        {
    //            if (txtDireccion.Text != "")
    //                queryContacto.Direccion = txtDireccion.Text;

    //            if (txt_exp_int_profesional.Text != "")
    //                queryContacto.Experiencia = txt_exp_int_profesional.Text;

    //            if (txt_int_res_HV.Text != "")
    //                queryContacto.HojaVida = txt_int_res_HV.Text;

    //            if (txt_exp_int_experi_intere.Text != "")
    //                queryContacto.Intereses = txt_exp_int_experi_intere.Text;

    //            consultas.Db.SubmitChanges();
    //        }

    //        //Tabla interventor
    //        var queryInterventor = (from c in consultas.Db.Interventor1s
    //                                where c.CodContacto == codUsuario
    //                                select c).FirstOrDefault();

    //        if (queryInterventor != null)
    //        {
    //            if (txtCelular.Text != "")
    //                queryInterventor.Celular = txtCelular.Text;

    //            if (txt_exp_sector_principal.Text != "")
    //                queryInterventor.ExperienciaPrincipal = txt_exp_sector_principal.Text;

    //            if (txt_exp_sector_secundario.Text != "")
    //                queryInterventor.Experienciasecundaria = txt_exp_sector_secundario.Text;

    //            consultas.Db.SubmitChanges();
    //        }


    //    }

    //    protected void btn_actualizar6_Click(object sender, EventArgs e)
    //    {
    //        updateEvaluador();
    //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Información actualizada exitosamente!'); document.location=('MiPerfil.aspx');", true);
    //    }

    //    protected void Img_Btn_Nuevo_Doc_interventor_Click(object sender, ImageClickEventArgs e)
    //    {
    //        HttpContext.Current.Session["Accion_Docs"] = "Crear";
    //        Redirect(null, "CatalogoAnexarInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
    //    }

    //    protected void Img_Btn_Ver_Doc_interventor_Click(object sender, ImageClickEventArgs e)
    //    {
    //        HttpContext.Current.Session["Accion_Docs"] = "Vista";
    //        Redirect(null, "CatalogoAnexarInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
    //    }

    //    protected void verDocAdjunto_Click(object sender, ImageClickEventArgs e)
    //    {
    //        String txtSQL = "SELECT NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto + " AND TipoArchivo ='FotocopiaDocumento' AND CodProyecto = " + CodProyecto.ToString();
    //        DataTable RS = consultas.ObtenerDataTable(txtSQL, "text");

    //        if (RS.Rows.Count > 0)
    //        {
    //            int CodCarpeta = Convert.ToInt32(usuario.IdContacto) / 2000;
    //            String Filename = RS.Rows[0]["NombreArchivo"].ToString();
    //            RutaArchivo = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "contactoAnexos/" + CodCarpeta.ToString() + "/ContactoAnexo_" + usuario.IdContacto + "/" + Filename.ToString();
    //        }
    //        RS.Dispose();
    //        Redirect(null, RutaArchivo, "_blank", null);
    //    }

    //    protected void AgregaDocumento(object sender, CommandEventArgs e)
    //    {
    //        int codProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);

    //        if (codEstadoProyecto(codProyecto) == 1) // 1 Estado Registro y Asesoria
    //        {
    //            var idCertificacion = e.CommandArgument;
    //            var user = usuario.IdContacto;
    //            HttpContext.Current.Session["USER"] = user;
    //            HttpContext.Current.Session["CodProyecto"] = HttpContext.Current.Session["CodProyecto"];
    //            HttpContext.Current.Session["TipoDocumento"] = "CertificacionEstudios";
    //            HttpContext.Current.Session["idCertificacionEstudios"] = idCertificacion;
    //            Redirect(null, "SubirArchivoAdjunto_Imagenes.aspx", "_blank", "menubar=0,scrollbars=1,width=663,height=547,top=100");
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje"
    //                    , "alert('Este proyecto ya no se encuentra en Registro y Asesoría, por lo tanto no está habilitado para actualizar el documento de identificacion!'); ", true);
    //        }
    //    }

    //    protected void VerCertificado(object sender, CommandEventArgs e)
    //    {
    //        var idCertificacion = e.CommandArgument;

    //        string txtSQL = "SELECT TOP 1 NombreArchivo FROM ContactoArchivosAnexos WHERE CodContacto= " + usuario.IdContacto + " AND TipoArchivo ='CertificacionEstudios' AND CodProyecto = " + CodProyecto.ToString() + " AND CodContactoEstudio='" + idCertificacion.ToString() + "'";
    //        DataTable RS = consultas.ObtenerDataTable(txtSQL, "text");

    //        if (RS.Rows.Count > 0)
    //        {
    //            int CodCarpeta = Convert.ToInt32(usuario.IdContacto) / 2000;
    //            string Filename = RS.Rows[0]["NombreArchivo"].ToString();
    //            RutaArchivo = ConfigurationManager.AppSettings.Get("RutaWebSite") + ConfigurationManager.AppSettings.Get("DirVirtual2") + "contactoAnexos/" + CodCarpeta.ToString() + "/ContactoAnexo_" + usuario.IdContacto + "/" + Filename.ToString();

    //            Redirect(null, RutaArchivo, "_blank", null);
    //        }
    //        else
    //        {
    //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Ningun documento de estudio adjunto a esta información académica ')", true);
    //        }

    //        RS.Dispose();
    //    }

    //    protected void ddl_DepartamentoDomicilio_SelectedIndexChanged(object sender, EventArgs e)
    //    {
    //        llenarCiudad(ddl_CiudadDomicilio, ddl_DepartamentoDomicilio);
    //    }
    //}

    //public class ListCiudades
    //{
    //    public int codCiudad { get; set; }
    //    public string nomCiudad { get; set; }
    //}
    #endregion
}