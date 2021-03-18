#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>10 - 06 - 2014</Fecha>
// <Archivo>ProyectoEmpresa.cs</Archivo>

#endregion

using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Security.Policy;

namespace Fonade.FONADE.interventoria
{
    public partial class ProyectoEmpresa : Negocio.Base_Page
    {

        #region Variables globales.

        string CodProyecto;

        bool Deshabilitar;

        Datos.Empresa empresa; //objeto vempresa
        Datos.Proyecto proyecto; //objeto proyecto

        //delegado para el control
        //de funciones dentro de un
        //procedimiento linq
        delegate bool del(int x);

        /// <summary>
        /// Contiene las consultas SQL.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Conteo de datos traídos a la grilla "gvsocios".
        /// </summary>
        Int32 Cont;

        Int32 txtTab = Constantes.CONST_SubResumenEjecutivo;
        Int32 CodEstado;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 10 - 06 - 2014
        /// Motodo Principal
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 63	ERROR JEF-UNID – 51
        /// Perfil: Jefe de Unidad-> Proyecto en Ejecución	Abierto	Los Radio Button no deben de estar seleccionados por defecto.
        /// asignación de controles radio button inhabilitada.
        /// </remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region desabilitarControles

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor ||
                    usuario.CodGrupo == Constantes.CONST_AdministradorSistema ||
                    usuario.CodGrupo == Constantes.CONST_Interventor)
                    Deshabilitar = true;
                else
                    Deshabilitar = false;
                #endregion
                foreach (var uns in form1.Controls.OfType<System.Web.UI.WebControls.Image>())
                {
                    uns.Visible = Deshabilitar;
                }

                #region informacion de proyecto y empresa

                CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";


                if (CodProyecto == "0" || CodProyecto == null)
                {
                    CodProyecto = Convert.ToString(Request.QueryString["codproyecto"]);
                }


                //proyecto = (from p in consultas.Db.Proyectos
                //            where p.Id_Proyecto == Convert.ToInt32(CodProyecto)
                //            select p).FirstOrDefault();

                string NombreProyecto = getNombreProyecto();

                empresa = (from em in consultas.Db.Empresas
                           where em.codproyecto == Convert.ToInt32(CodProyecto)
                           select em).FirstOrDefault();



                #endregion

                //if (!Negocio.PlanDeNegocioV2.Ejecucion.Empresa.Empresa.IsDataCompleteOnRegistroMercantil(proyecto.Id_Proyecto) && usuario.CodGrupo.Equals(Constantes.CONST_Emprendedor))
                //{
                //    PlanDeNegocioV2.Formulacion.Utilidad.Utilidades.PresentarMsj("Debe llenar por completo la información de la pestaña de registro mercantil.", this, "Alert");
                //}

                CargarCombos();

                if (empresa != null)
                {

                    //lblplannegocio.Text = proyecto.NomProyecto;
                    lblplannegocio.Text = NombreProyecto;

                    txtrazonsocial.Text = empresa.razonsocial;
                    txtrazonsocial.Enabled = Deshabilitar;
                    
                    if (!string.IsNullOrEmpty(empresa.codigoCIIU))
                    {
                        txtCodigoCIIU.Text = empresa.codigoCIIU;
                    }
                    
                    #region desplegar datos de la empresa

                    txtobjetosocial.Text = empresa.ObjetoSocial;
                    txtobjetosocial.Enabled = Deshabilitar;
                    txtcapitalsocial.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                    txtcapitalsocial.Text = empresa.CapitalSocial.ToString();
                    txtcapitalsocial.Enabled = Deshabilitar;
                    if (!string.IsNullOrEmpty(empresa.CodTipoSociedad.ToString()))
                    {
                        ddltiposociedad.SelectedValue = empresa.CodTipoSociedad.ToString();
                    }
                    //if (empresa.CodTipoSociedad.ToString() == "") { } else {
                    //    ddltiposociedad.SelectedValue = empresa.CodTipoSociedad.ToString();
                    //}
                    ddltiposociedad.Enabled = Deshabilitar;
                    txtescriturapublica.Text = empresa.NumEscrituraPublica;
                    txtescriturapublica.Enabled = Deshabilitar;
                    txtdomicilioempresa.Text = empresa.DomicilioEmpresa;
                    txtdomicilioempresa.Enabled = Deshabilitar;
                    //gvsocios.Enabled = false;
                    try
                    {
                        ddldepartamneto.DataBind();
                        ddldepartamneto.SelectedValue = (from c in consultas.Db.Ciudad
                                                         where c.Id_Ciudad == empresa.CodCiudad
                                                         select c.CodDepartamento).FirstOrDefault().ToString();
                    }
                    catch (ArgumentOutOfRangeException) { }
                    ddldepartamneto.Enabled = Deshabilitar;

                    try
                    {
                        llenarCiudad();
                        ddlciudades.SelectedValue = empresa.CodCiudad.ToString();
                    }
                    catch (ArgumentOutOfRangeException) { }
                    ddlciudades.Enabled = Deshabilitar;

                    txttelefono.Text = empresa.Telefono;
                    txttelefono.Enabled = Deshabilitar;
                    txtcorreo.Text = empresa.Email;
                    txtcorreo.Enabled = Deshabilitar;

                    #endregion

                    #region desplegar Información Tributaria

                    //txtnit.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
                    txtnit.Text = empresa.Nit;
                    txtnit.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.RegimenEspecial))
                        rblregimenespecial.SelectedValue = "1";
                    else
                        rblregimenespecial.SelectedValue = "0";
                    rblregimenespecial.Enabled = Deshabilitar;

                    txtrenorma.Text = empresa.RENorma;
                    txtrenorma.Enabled = Deshabilitar;
                    txtrenormafecha.Text = empresa.REFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy");
                    txtrenormafecha.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.Contribuyente))
                        rbl_contribuyente.SelectedValue = "1";
                    else
                        rbl_contribuyente.SelectedValue = "0";
                    rbl_contribuyente.Enabled = Deshabilitar;

                    txtcenorma.Text = empresa.CNorma;
                    txtcenorma.Enabled = Deshabilitar;
                    txtcenormafecha.Text = empresa.CFechaNorma.HasValue ?
                                                empresa.CFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy")
                                                : DateTime.Now.ToShortDateString();
                    txtcenormafecha.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.AutoRetenedor))
                        rbl_Autoretenedor.SelectedValue = "1";
                    else
                        rbl_Autoretenedor.SelectedValue = "0";
                    rbl_Autoretenedor.Enabled = Deshabilitar;

                    txtarnorma.Text = empresa.ARNorma;
                    txtarnorma.Enabled = Deshabilitar;
                    txtarnormafecha.Text = empresa.ARFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy");
                    txtarnormafecha.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.Declarante))
                        rbl_Declarante.SelectedValue = "1";
                    else
                        rbl_Declarante.SelectedValue = "NO";
                    rbl_Declarante.Enabled = Deshabilitar;

                    txtdnorma.Text = empresa.DNorma;
                    txtdnorma.Enabled = Deshabilitar;
                    txtdnormafecha.Text = empresa.DFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy");
                    txtdnormafecha.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.ExentoRetefuente))
                        rbl_Retencion.SelectedValue = "1";
                    else
                        rbl_Retencion.SelectedValue = "0";
                    rbl_Retencion.Enabled = Deshabilitar;

                    txterfnorma.Text = empresa.ERFNorma;
                    txterfnorma.Enabled = Deshabilitar;
                    txterfnormafecha.Text = empresa.ERFFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy");
                    txterfnormafecha.Enabled = Deshabilitar;

                    if (Convert.ToBoolean(empresa.GranContribuyente))
                        rbl_GranContribuyente.SelectedValue = "1";
                    else
                        rbl_GranContribuyente.SelectedValue = "0";
                    rbl_GranContribuyente.Enabled = Deshabilitar;

                    txtgcnorma.Text = empresa.GCNorma;
                    txtgcnorma.Enabled = Deshabilitar;
                    txtgcnormafecha.Text = empresa.GCFechaNorma.GetValueOrDefault().ToString("dd/MM/yyyy");
                    txtgcnormafecha.Enabled = Deshabilitar;
                    CargarGridSocios();

                    #endregion

                    //Consultar el "Estado" del proyecto.
                    CodEstado = CodEstado_Proyecto(txtTab.ToString(), CodProyecto, ""); //CodConvocatoria
                }
                CargarGridSocios();

                if (usuario.CodGrupo == Constantes.CONST_Emprendedor)
                {
                    btnguardar.Enabled = true;
                    btnguardar.Visible = true;
                }
            }


        }

        private string getNombreProyecto()
        {
            try
            {
                string nombre = "";

                nombre = (from p in consultas.Db.Proyecto
                          where p.Id_Proyecto == Convert.ToInt32(getCodProyecto())
                          select p.NomProyecto).FirstOrDefault();

                if (nombre == null || nombre == "")
                {
                    nombre = (from p in consultas.Db.Proyecto
                              where p.Id_Proyecto == getCodProyectoMASTER()
                              select p.NomProyecto).FirstOrDefault();
                }


                return nombre;
            }
            catch (Exception)
            {
                return "No se logró obtener el nombre del proyecto, Por favor cierre sesión y vuelva a ingresar.";
            }
            
        }

        private int getCodProyectoMASTER()
        {
            int codigo = 0;
            
            string titulo = Session["TituloProyectoMaster"].ToString();

            string [] titulos = titulo.Split(' ');

            foreach (string line in titulos)
            {
                codigo = Convert.ToInt32(line);
                break;
            }

            return codigo;
        }

        private string getCodProyecto()
        {
            string codigoProyecto = "";
            codigoProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

            if (codigoProyecto == "0" || codigoProyecto == null)
            {
                codigoProyecto = Convert.ToString(Request.QueryString["codproyecto"]);
            }

            return codigoProyecto;
        }

        private void CargarGridSocios()
        {
            var query = "Select pc.codContacto, 0 Activo, c.Nombres+ ' '+c.Apellidos Nombre, c.Identificacion,Cast(ISNULL(ec.representantelegal, 0) as bit) representantelegal, ";
            query += "Cast(ISNULL(ec.Suplente, 0) as bit) Suplente, c.Direccion, c.email, ec.participacion, c.telefono,c.codciudad  from ProyectoContacto pc ";
            query += "Left join Contacto c on c.id_Contacto = pc.codContacto ";
            query += "Left join EmpresaContacto ec on ec.codcontacto = pc.codContacto ";
            query += "where pc.Inactivo = 0 and pc.codrol = " + Constantes.CONST_RolEmprendedor + " and pc.codProyecto = " + CodProyecto + " and ec.codempresa = 7247";

            var dt = consultas.ObtenerDataTable(query, "text");
            if (dt.Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[4].ToString()) || dt.Rows[0].ItemArray[4].ToString() == "0")
                {
                    dt.Rows[0].ItemArray[4] = false;
                }
                else
                {
                    dt.Rows[0].ItemArray[4] = true;
                }

                if (string.IsNullOrEmpty(dt.Rows[0].ItemArray[5].ToString()) || dt.Rows[0].ItemArray[5].ToString() == "0")
                {
                    dt.Rows[0].ItemArray[5] = false;
                }
                else
                {
                    dt.Rows[0].ItemArray[5] = true;
                }
                gvsocios.DataSource = dt;
                gvsocios.DataBind();
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 10 - 06 - 2014
        /// relaciona el tipo de sociedad al que pertenece la empresa
        /// actualmente seleccionada
        /// o en sesion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_tiposociedad_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from s in consultas.Db.TipoSociedads
                          orderby s.NomTipoSociedad
                          select new
                          {
                              s.Id_TipoSociedad,
                              s.NomTipoSociedad
                          });

            e.Result = result.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 11 - 06 - 2014
        /// metodo que retorna una lista de ciudades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_ciudad_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from c in consultas.Db.Ciudad
                          orderby c.NomCiudad
                          select new
                          {
                              c.Id_Ciudad,
                              c.NomCiudad
                          });

            e.Result = result.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 10 - 06 - 2014
        /// trae el nombre de los departamentos con su respectivo id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_departamento_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from c in consultas.Db.departamento
                          orderby c.NomDepartamento
                          select new
                          {
                              c.Id_Departamento,
                              c.NomDepartamento
                          });

            e.Result = result.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 10 - 06 - 2014
        /// redirecciona al metodo llenarCiudad(sender, e, id) con el fin
        /// de llenar el dropdownlist de ciudades deacuerdo al departamento seleccionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddldepartamneto_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCiudad();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 10 - 06 - 2014
        /// trae el nombre de las ciudades con su respectivo id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="coddepartamento"></param>
        private void llenarCiudad()
        {
            if (!string.IsNullOrEmpty(ddldepartamneto.SelectedValue) && !ddldepartamneto.Equals("-1"))
            {
                var result = (from c in consultas.Db.Ciudad
                              where c.CodDepartamento == Convert.ToInt32(ddldepartamneto.SelectedValue)
                              orderby c.NomCiudad
                              select new
                              {
                                  c.Id_Ciudad,
                                  c.NomCiudad
                              });

                ddlciudades.DataSource = result;
                ddlciudades.DataBind();
            }
        }

        private bool ValidarCargaArchivo()
        {
            bool cargado = false;

            if (CodProyecto == null)
            {
                CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

                if (CodProyecto == "0" || CodProyecto == null)
                {
                    CodProyecto = Convert.ToString(Request.QueryString["codproyecto"]);
                }
            }

            string filename = "RUT_" + CodProyecto + ".pdf";
            int idProyecto = Convert.ToInt32(CodProyecto);

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                var query = (from C in db.ContratosArchivosAnexos
                             where C.CodProyecto == idProyecto && C.NombreArchivo.Contains(filename)
                             select C.IdContratoArchivoAnexo
                             ).FirstOrDefault();

                if (query == 0)// si no se encuentra
                {
                    cargado = false;
                }
                else //si se encientra
                {
                    cargado = true;
                }
            }


            return cargado;
        }


        /// <summary>
        /// Diego Quiñonez
        /// 11 - 06 - 2014
        /// se crea una nueva empresa
        /// o en caso contrario se actualiza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnguardar_Click(object sender, EventArgs e)
        {


            //Nueva versión.
            string msg = "";
            msg = Validar();

            //Si pasa la validación, ejecuta el método.
            if (msg == "")
            {
                //Se asigna el resultado del método a la misma variable.
                msg = Ejecutar_RegistroMercantil();

                //Si hay datos "es decir, si hay errores", muestra el mensaje, si no, todo ha salido bien.
                //y las pruebas locales asílo demostraron.
                if (msg != "")
                { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + msg + "')", true); }
            }
            else
            {
                //De lo contrario, muestra el error obtenido en la validación de datos.
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + msg + "')", true);
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 18/09/2014.
        /// Guardar o Actualizar el registro mercantil.
        /// </summary>
        /// <returns>string.</returns>
        private string Ejecutar_RegistroMercantil()
        {
            if (CodProyecto == null || CodProyecto == "0")
            {
                CodProyecto = getCodProyecto();
            }

            //Inicializar variables.
            string msg = "";
            decimal Capital_Social = 0;
            decimal SMLV = 589500; //Se deja el valor del año 2013 "solo por si llega a pasar un error "no debería pasar".
            #region Obtener el salario mínimo.

            try
            {
                var smlv_sql = consultas.ObtenerDataTable("SELECT SalarioMinimo FROM SalariosMinimos WHERE [AñoSalario] = " + DateTime.Today.Year, "text");
                if (smlv_sql.Rows.Count > 0) { SMLV = decimal.Parse(smlv_sql.Rows[0]["SalarioMinimo"].ToString()); }
            }
            catch
            { SMLV = 589500; }

            #endregion
            DataTable RS = new DataTable();
            DateTime REFechaNorma;
            DateTime CFechaNorma;
            DateTime ARFechaNorma;
            DateTime DFechaNorma;
            DateTime ERFFechaNorma;
            DateTime GCFechaNorma;
            String txtCodContacto = "";
            String txtCodEmpresa = "";
            Int32 rRegimenEspecial = 0;
            Int32 rContribuyente = 0;
            Int32 rAutoretenedor = 0;
            Int32 rDeclarante = 0;
            Int32 rRetencion = 0;
            Int32 rGranContribuyente = 0;
            Int32 RepresentanteLegal = 0;
            Int32 Suplente = 0;

            try
            {
                //Alejandro Garzon R. Julio 25 2005
                //Se cambia la validación del capital social mayor a los recursos aprobados, se deja que sea mayor o igual a 3 SMLV.
                if (txtcapitalsocial.Text.Trim() != "")
                    Capital_Social = decimal.Parse(txtcapitalsocial.Text.Trim());
                else
                    Capital_Social = 0;

                if (Capital_Social < (1 * (SMLV)))
                {
                    msg = "!!!!! El capital social debe ser mayor a 1 SMLV !!!!!";
                }
                else
                {
                    //Si no hay mensajes, puede continuar.
                    if (msg == "")
                    {
                        txtSQL = "SELECT COUNT(*) AS Conteo FROM Empresa WHERE CodProyecto=" + CodProyecto;
                        RS = consultas.ObtenerDataTable(txtSQL, "text");

                        #region Depurar valores obtenidos en el formulario de registro mercantil.

                        REFechaNorma = Convert.ToDateTime(txtrenormafecha.Text);
                        CFechaNorma = Convert.ToDateTime(txtcenormafecha.Text);
                        ARFechaNorma = Convert.ToDateTime(txtarnormafecha.Text);
                        DFechaNorma = Convert.ToDateTime(txtdnormafecha.Text);
                        ERFFechaNorma = Convert.ToDateTime(txterfnormafecha.Text);
                        GCFechaNorma = Convert.ToDateTime(txtgcnormafecha.Text);

                        #endregion

                        if (RS.Rows.Count > 0)
                        {
                            if (RS.Rows[0]["Conteo"].ToString() == "0")
                            {
                                #region Crear registro mercantil.

                                #region Obtener los valores seleccionados de las listas desplegables.

                                if (!string.IsNullOrEmpty(rblregimenespecial.SelectedValue))
                                    rRegimenEspecial = Convert.ToInt32(rblregimenespecial.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_contribuyente.SelectedValue))
                                    rContribuyente = Convert.ToInt32(rbl_contribuyente.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Autoretenedor.SelectedValue))
                                    rAutoretenedor = Convert.ToInt32(rbl_Autoretenedor.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Declarante.SelectedValue))
                                    rDeclarante = Convert.ToInt32(rbl_Declarante.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Retencion.SelectedValue))
                                    rRetencion = Convert.ToInt32(rbl_Retencion.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_GranContribuyente.SelectedValue))
                                    rGranContribuyente = Convert.ToInt32(rbl_GranContribuyente.SelectedValue);

                                #endregion

                                #region Inserción.
                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {
                                    //NEW RESULTS:

                                    SqlCommand cmd = new SqlCommand(txtSQL, con);

                                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    cmd.CommandText = "MD_AdministrarRegistrosMercantiles";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@CASO", "Nuevo");
                                    cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
                                    cmd.Parameters.AddWithValue("@RazonSocial", txtrazonsocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ObjetoSocial", txtobjetosocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CapitalSocial", txtcapitalsocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CodTipoSociedad", ddltiposociedad.SelectedValue);
                                    cmd.Parameters.AddWithValue("@NumEscrituraPublica", txtescriturapublica.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DomicilioEmpresa", txtdomicilioempresa.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CodCiudad", ddlciudades.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Telefono", txttelefono.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Email", txtcorreo.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Nit", txtnit.Text.Trim());
                                    cmd.Parameters.AddWithValue("@RegimenEspecial", rRegimenEspecial);
                                    cmd.Parameters.AddWithValue("@RENorma", txtrenorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@REFechaNorma", REFechaNorma);
                                    cmd.Parameters.AddWithValue("@Contribuyente", rContribuyente);
                                    cmd.Parameters.AddWithValue("@CNorma", txtcenorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CFechaNorma", CFechaNorma);
                                    cmd.Parameters.AddWithValue("@AutoRetenedor", rAutoretenedor);
                                    cmd.Parameters.AddWithValue("@ARNorma", txtarnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ARFechaNorma", ARFechaNorma);
                                    cmd.Parameters.AddWithValue("@Declarante", rDeclarante);
                                    cmd.Parameters.AddWithValue("@DNorma", txtdnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DFechaNorma", DFechaNorma);
                                    cmd.Parameters.AddWithValue("@ExentoRetefuente", rRetencion);
                                    cmd.Parameters.AddWithValue("@ERFNorma", txterfnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ERFFechaNorma", ERFFechaNorma);
                                    cmd.Parameters.AddWithValue("@TipoRegimen", txttiporegimen.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GranContribuyente", rGranContribuyente);
                                    cmd.Parameters.AddWithValue("@GCNorma", txtgcnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GCFechaNorma", GCFechaNorma);
                                    cmd.Parameters.AddWithValue("@codigoCIIU", txtCodigoCIIU.Text.Trim());
                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                    //con.Dispose();
                                    cmd.Dispose();
                                }
                                catch (Exception ex) { string abc = ex.Message; msg = "Error en la inserción."; }
                                finally
                                {

                                    con.Close();
                                    con.Dispose();
                                }
                                #endregion

                                //Si la inserción anterior se hizo correctamente, puede continuar.
                                if (msg == "")
                                {
                                    #region Consulta 1.

                                    txtSQL = "SELECT * FROM ProyectoContacto WHERE CodRol=" + Constantes.CONST_RolEmprendedor + " AND CodProyecto = " + CodProyecto;
                                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (RS.Rows.Count > 0) { txtCodContacto = RS.Rows[0]["CodContacto"].ToString(); }

                                    #endregion

                                    #region Consulta 2.

                                    txtSQL = "SELECT id_empresa FROM empresa WHERE CodProyecto = " + CodProyecto;
                                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (RS.Rows.Count > 0) { txtCodEmpresa = RS.Rows[0]["id_empresa"].ToString(); }

                                    #endregion

                                    if (RS.Rows.Count > 0)
                                    {
                                        //Ingresa los socios de la empresa
                                        //RECORRER LA TABLA PARA OBTENER LOS VALORES REQUERIDOS PARA LA INSERCIÓN.
                                        foreach (GridViewRow row in gvsocios.Rows)
                                        {
                                            #region Obtener valores.

                                            var rb = row.FindControl("rb_representante") as RadioButton;
                                            var hdf = row.FindControl("hdf_rb_codigocontacto") as HiddenField;
                                            var rbSupl = row.FindControl("rb_suplente") as RadioButton;
                                            var Participacion = row.FindControl("txtParticipacion") as TextBox;

                                            #endregion

                                            if (!rb.Checked)
                                                RepresentanteLegal = 0;
                                            else
                                                RepresentanteLegal = 1;

                                            if (!rbSupl.Checked)
                                                Suplente = 1;
                                            else
                                                Suplente = 0;

                                            txtSQL = " INSERT INTO EmpresaContacto (CodEmpresa, CodContacto, Participacion, RepresentanteLegal, Suplente)" +
                                                     " VALUES (" + txtCodEmpresa + ", " + hdf.Value + ", " + Participacion.Text.Trim() + ", " + RepresentanteLegal + ", " + Suplente + ")";
                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }
                                #endregion
                            }
                            else
                            {
                                #region Actualizar registro mercantil.

                                //Obtener valores.
                                rRegimenEspecial = 0;
                                rContribuyente = 0;
                                rAutoretenedor = 0;
                                rDeclarante = 0;
                                rRetencion = 0;
                                rGranContribuyente = 0;

                                #region Obtener los valores seleccionados de las listas desplegables.

                                if (!string.IsNullOrEmpty(rblregimenespecial.SelectedValue))
                                    rRegimenEspecial = Convert.ToInt32(rblregimenespecial.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_contribuyente.SelectedValue))
                                    rContribuyente = Convert.ToInt32(rbl_contribuyente.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Autoretenedor.SelectedValue))
                                    rAutoretenedor = Convert.ToInt32(rbl_Autoretenedor.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Declarante.SelectedValue))
                                    rDeclarante = Convert.ToInt32(rbl_Declarante.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_Retencion.SelectedValue))
                                    rRetencion = Convert.ToInt32(rbl_Retencion.SelectedValue);

                                if (!string.IsNullOrEmpty(rbl_GranContribuyente.SelectedValue))
                                    rGranContribuyente = Convert.ToInt32(rbl_GranContribuyente.SelectedValue);

                                #endregion

                                #region Actualiza los datos de la empresa.
                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {
                                    //NEW RESULTS:

                                    SqlCommand cmd = new SqlCommand(txtSQL, con);

                                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    cmd.CommandText = "MD_AdministrarRegistrosMercantiles";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@CASO", "Actualizar");
                                    cmd.Parameters.AddWithValue("@CodProyecto", CodProyecto);
                                    cmd.Parameters.AddWithValue("@RazonSocial", txtrazonsocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ObjetoSocial", txtobjetosocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CapitalSocial", txtcapitalsocial.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CodTipoSociedad", ddltiposociedad.SelectedValue);
                                    cmd.Parameters.AddWithValue("@NumEscrituraPublica", txtescriturapublica.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DomicilioEmpresa", txtdomicilioempresa.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CodCiudad", ddlciudades.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Telefono", txttelefono.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Email", txtcorreo.Text.Trim());
                                    cmd.Parameters.AddWithValue("@Nit", txtnit.Text.Trim());
                                    cmd.Parameters.AddWithValue("@RegimenEspecial", rRegimenEspecial);
                                    cmd.Parameters.AddWithValue("@RENorma", txtrenorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@REFechaNorma", Convert.ToDateTime(REFechaNorma));
                                    cmd.Parameters.AddWithValue("@Contribuyente", rContribuyente);
                                    cmd.Parameters.AddWithValue("@CNorma", txtcenorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@CFechaNorma", Convert.ToDateTime(CFechaNorma));
                                    cmd.Parameters.AddWithValue("@AutoRetenedor", rAutoretenedor);
                                    cmd.Parameters.AddWithValue("@ARNorma", txtarnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ARFechaNorma", Convert.ToDateTime(ARFechaNorma));
                                    cmd.Parameters.AddWithValue("@Declarante", rDeclarante);
                                    cmd.Parameters.AddWithValue("@DNorma", txtdnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@DFechaNorma", Convert.ToDateTime(DFechaNorma));
                                    cmd.Parameters.AddWithValue("@ExentoRetefuente", rRetencion);
                                    cmd.Parameters.AddWithValue("@ERFNorma", txterfnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@ERFFechaNorma", Convert.ToDateTime(ERFFechaNorma));
                                    cmd.Parameters.AddWithValue("@TipoRegimen", txttiporegimen.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GranContribuyente", rGranContribuyente);
                                    cmd.Parameters.AddWithValue("@GCNorma", txtgcnorma.Text.Trim());
                                    cmd.Parameters.AddWithValue("@GCFechaNorma", Convert.ToDateTime(GCFechaNorma));
                                    cmd.Parameters.AddWithValue("@codigoCIIU", txtCodigoCIIU.Text.Trim());
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    con.Dispose();
                                    cmd.Dispose();
                                }
                                catch (Exception ex) { string abc = ex.Message; msg = "Error en la actualización."; }
                                finally
                                {
                                    con.Close();
                                    con.Dispose();

                                }
                                #endregion

                                if (msg == "")
                                {
                                    txtSQL = "SELECT Id_Empresa FROM Empresa WHERE CodProyecto = " + CodProyecto;
                                    RS = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (RS.Rows.Count > 0) { txtCodEmpresa = RS.Rows[0]["Id_Empresa"].ToString(); }

                                    if (RS.Rows.Count > 0)
                                    {
                                        foreach (GridViewRow row in gvsocios.Rows)
                                        {
                                            #region Obtener valores.

                                            var rb = row.FindControl("rb_representante") as RadioButton;
                                            var hdf = row.FindControl("hdf_rb_codigocontacto") as HiddenField;
                                            var rbSupl = row.FindControl("rb_suplente") as RadioButton;
                                            var Participacion = row.FindControl("txtParticipacion") as TextBox;
                                            var Direcc = row.FindControl("txtdireccion") as TextBox;
                                            var CodCiu = row.FindControl("ddlciudad") as DropDownList;

                                            #endregion

                                            if (!rb.Checked)
                                                RepresentanteLegal = 0;
                                            else
                                                RepresentanteLegal = 1;

                                            if (rbSupl.Checked)
                                                Suplente = 1;
                                            else
                                                Suplente = 0;

                                            //Aquí se realizan las actualizaciones de los datos del contacto.
                                            txtSQL = " UPDATE contacto SET " +
                                                     " Direccion = '" + Direcc.Text.Trim() + "'," +
                                                     " CodCiudad = " + CodCiu.SelectedValue +
                                                     " WHERE Id_Contacto = " + hdf.Value;
                                            ejecutaReader(txtSQL, 2);

                                            //Se valida si el contacto existe en EmpresaContacto
                                            txtSQL = " SELECT CodContacto FROM EmpresaContacto " +
                                                     " WHERE CodContacto = " + hdf.Value +
                                                     " AND CodEmpresa = " + txtCodEmpresa;
                                            RS = consultas.ObtenerDataTable(txtSQL, "text");

                                            if (RS.Rows.Count > 0)
                                            {
                                                //Se actualiza tambien la condicion de representante legal o suplente y la participacion accionaria.
                                                txtSQL = " UPDATE empresacontacto SET " +
                                                         " participacion = " + Participacion.Text.Trim() + ", " +
                                                         " RepresentanteLegal = " + RepresentanteLegal + ", " +
                                                         " Suplente = " + Suplente + " " +
                                                         " WHERE codempresa=" + txtCodEmpresa +
                                                         " AND CodContacto = " + hdf.Value;
                                            }
                                            else
                                            {
                                                //Se ingresa el nuevo contacto de la empresa.
                                                if (Participacion.Text.Trim() == "")
                                                {
                                                    txtSQL = " INSERT INTO EmpresaContacto (CodEmpresa,CodContacto,Participacion,RepresentanteLegal,Suplente)" +
                                                             " VALUES (" + txtCodEmpresa + "," + hdf.Value + ",0," +
                                                             " " + RepresentanteLegal + "," + Suplente + ")";
                                                }
                                                else
                                                {
                                                    txtSQL = " INSERT INTO EmpresaContacto (CodEmpresa,CodContacto,Participacion,RepresentanteLegal,Suplente)" +
                                                             " VALUES (" + txtCodEmpresa + "," + hdf.Value + "," + Participacion.Text.Trim() + "," +
                                                             " " + RepresentanteLegal + "," + Suplente + ")";
                                                }
                                            }

                                            //Ejecutar sentencia SQL obtenida de ls condiciones anteriores.
                                            ejecutaReader(txtSQL, 2);
                                        }
                                    }
                                }

                                #endregion
                            }
                        }
                        //Actualizar fecha modificación del tab.
                        prActualizarTab(txtTab.ToString(), CodProyecto);
                    }
                }

                //Retornar mensaje.
                return msg;
            }
            catch (Exception ex)
            { msg = "Error: " + ex.Message; return msg; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/09/2014.
        /// A diferencia de FONADE clásico, se emplea este método para validar todos los campos
        /// del formulario de registro mercantil "en lugar de crear varios métodos para ello".
        /// </summary>
        /// <returns>string.</returns>
        private string Validar()
        {
            //Inicializar variables.
            String msg = "";
            Int64 NIT = 0;
            Int64 CapitalSocial = 0;

            try
            {
                if (txtCodigoCIIU.Text.Trim() == "")
                {
                    msg = "Debe digitar el código CIIU para poder guardar el formulario.";
                    lblCodigoCIIU.Visible = true;
                    lblCodigoCIIU.Focus();
                }

                if (txtCodigoCIIU.Text.Length > 4)
                {
                    msg = "El código CIIU no debe superar los 4 caracteres.";
                    lblCodigoCIIU.Text = "El código CIIU no debe superar los 4 caracteres.";
                    lblCodigoCIIU.Visible = true;
                    lblCodigoCIIU.Focus();
                }

                if (ValidarCargaArchivo() == false)
                {
                    msg = "Debe cargar el RUT para poder guardar el formulario.";
                    lblMensajeRut.Visible = true;
                    lblMensajeRut.Focus();
                }

                if (!Int64.TryParse(txtnit.Text.Trim(), out NIT))
                { msg = "El valor ''" + txtnit.Text.Trim() + "'' no es un número."; }

                if (txtrazonsocial.Text.Trim() == "")
                { msg = "Debe digitar la razón social."; }

                //Recorrer la tabla para validar la información.
                foreach (GridViewRow row in gvsocios.Rows)
                {
                    #region Obtener valores.

                    var rb = row.FindControl("rb_representante") as RadioButton;
                    var hdf = row.FindControl("hdf_rb_codigocontacto") as HiddenField;
                    var rbSupl = row.FindControl("rb_suplente") as RadioButton;
                    var Participacion = row.FindControl("txtParticipacion") as TextBox;

                    #endregion

                    if (Participacion.Text.Trim() == "")
                    { msg = "Debe llenar la participación por cada emprendedor."; break; }
                }

                if (txtobjetosocial.Text.Trim() == "")
                { msg = "Debe digitar el objeto social."; }

                if (txtcapitalsocial.Text.Trim() == "")
                { msg = "Debe digitar el capital social."; }
                else
                {
                    if (!Int64.TryParse(txtcapitalsocial.Text.Trim(), out CapitalSocial))
                    { msg = "Solo Datos numéricos"; }
                }

                if (ddltiposociedad.SelectedValue == "" || ddltiposociedad.SelectedValue == "0")
                { msg = "Debe seleccinar el tipo de sociedad."; }

                if (CodEstado >= Constantes.CONST_LegalizacionContrato)
                {
                    if (txtescriturapublica.Text.Trim() == "")
                    { msg = "Debe digitar el número de la escritura pública."; }
                }

                if (txtdomicilioempresa.Text.Trim() == "")
                { msg = "Debe digitar el domicilio de la empresa."; }

                if (ddldepartamneto.SelectedValue == "" || ddldepartamneto.SelectedValue == "0")
                { msg = "Debe seleccionar algún departamento."; }

                if (txttelefono.Text.Trim() == "")
                { msg = "Debe digitar el teléfono."; }

                if (txtcorreo.Text.Trim() == "")
                {
                    msg = Texto("TXT_EMAIL_REQ");
                }
                else
                {
                    if (!txtcorreo.Text.Trim().Contains("@") || !txtcorreo.Text.Trim().Contains("."))
                        msg = Texto("TXT_EMAIL_INV");
                }

                if (CodEstado >= Constantes.CONST_LegalizacionContrato)
                {
                    if (txtnit.Text.Trim() == "")
                    { msg = "Debe digitar el Nit."; }

                    if (!rblregimenespecial.Items[0].Selected && !rblregimenespecial.Items[1].Selected)
                    { msg = "Debe seleccionar si es régimen especial."; }

                    if (!rbl_contribuyente.Items[0].Selected && !rbl_contribuyente.Items[1].Selected)
                    { msg = "Debe seleccionar si es contribuyente."; }

                    if (!rbl_Autoretenedor.Items[0].Selected && !rbl_Autoretenedor.Items[1].Selected)
                    { msg = "Debe seleccionar si es autoretenedor."; }

                    if (!rbl_Declarante.Items[0].Selected && !rbl_Declarante.Items[1].Selected)
                    { msg = "Debe seleccionar si es declarante."; }

                    if (!rbl_Retencion.Items[0].Selected && !rbl_Retencion.Items[1].Selected)
                    { msg = "Debe seleccionar si es exento de retención en la fuente."; }

                    if (!rbl_GranContribuyente.Items[0].Selected && !rbl_GranContribuyente.Items[1].Selected)
                    { msg = "Debe seleccionar si es  gran contribuyente."; }
                }

                //Retornar valor.
                return msg;
            }
            catch (Exception ex)
            { msg = "Error en la validación de la información. Error: " + ex.Message; return msg; }
        }

        private void CargarCombos()
        {
            var tipoSociedad = (from ts in consultas.Db.TipoSociedads
                                select ts).ToList();
            ddltiposociedad.DataSource = tipoSociedad;
            ddltiposociedad.DataTextField = "NomTipoSociedad";
            ddltiposociedad.DataValueField = "Id_TipoSociedad";
            ddltiposociedad.DataBind();
            ddltiposociedad.Items.Insert(0, new ListItem("Seleccione", "0"));
        }

        protected void btn_Cargar_Click(object sender, EventArgs e)
        {
            if (fu_archivo.HasFile)
            {

                if (fu_archivo.FileName.Contains(".pdf") || fu_archivo.FileName.Contains(".PDF"))
                {
                    //string ruta = ConfigurationManager.AppSettings.Get("RutaDocumentosProyecto") + @"Proyecto_" + CodProyecto + @"\";
                    //string ruta = @"\" + Math.Abs(Convert.ToInt32(CodProyecto) / 2000) + @"\Proyecto_" + CodProyecto + @"\";
                    if (CodProyecto == null)
                    {
                        CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";

                        if (CodProyecto == "0" || CodProyecto == null)
                        {
                            CodProyecto = Convert.ToString(Request.QueryString["codproyecto"]);
                        }
                    }

                    string ip = ConfigurationManager.AppSettings.Get("RutaIP");
                    string ruta = "Documentos/Proyecto/Proyecto_" + CodProyecto + "/";

                    string nombrearchivo = "RUT_" + CodProyecto + ".pdf";

                    string mensaje = "";
                    Boolean fileOK = false;
                    //String path = (ruta);
                    if (fu_archivo.HasFile)
                    {
                        String fileExtension =
                            System.IO.Path.GetExtension(fu_archivo.FileName).ToLower();
                        String[] allowedExtensions =
                            {".pdf", ".PDF"}; //Validar extension
                        for (int i = 0; i < allowedExtensions.Length; i++)
                        {
                            if (fileExtension == allowedExtensions[i])
                            {
                                fileOK = true;
                            }
                        }
                    }

                    if (fileOK)
                    {
                        try
                        {

                            if (!Directory.Exists(ip + ruta))
                            {
                                // This path is a directory
                                Directory.CreateDirectory(ip + ruta);
                            }

                            //fu_archivo.FileName

                            string filename = Path.GetFileName(nombrearchivo);
                            if (File.Exists(ip + ruta + filename))
                            {
                                File.Delete(ip + ruta + filename);
                                //esto lo hago porque lo necesito, necesito //saber si me toco reemplazarlo
                            }

                            fu_archivo.SaveAs(ip + ruta + filename);
                            string rutaCompleta = ruta + filename;

                            bool actualizado = false;

                            int idProyecto = Convert.ToInt32(CodProyecto);
                            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                            {
                                var query = (from C in db.ContratosArchivosAnexos
                                             where C.CodProyecto == idProyecto && C.NombreArchivo.Contains(filename)
                                             select C.IdContratoArchivoAnexo
                                             ).FirstOrDefault();

                                if (query == 0)// si no se encuentra se agrega 
                                {
                                    //Create new ContactoArchivosAnexos object
                                    ContratosArchivosAnexo ca = new ContratosArchivosAnexo()
                                    {
                                        CodProyecto = idProyecto,
                                        NombreArchivo = filename,
                                        ruta = rutaCompleta,
                                        CodContacto = usuario.IdContacto,
                                        FechaIngreso = DateTime.Now
                                    };
                                    //Add to memory
                                    db.ContratosArchivosAnexos.InsertOnSubmit(ca);
                                    //Save to database
                                    db.SubmitChanges();

                                    actualizado = true;
                                }
                                else //si se encientra se actualiza
                                {
                                    ContratosArchivosAnexo ca = (from C in db.ContratosArchivosAnexos
                                                                 where C.IdContratoArchivoAnexo.Equals(query)
                                                                 select C).First();

                                    ca.NombreArchivo = filename;
                                    ca.FechaIngreso = DateTime.Now;
                                    ca.ruta = rutaCompleta;
                                    ca.CodContacto = usuario.IdContacto;
                                    db.SubmitChanges();

                                    actualizado = true;
                                }
                            }

                            if (actualizado)
                            {
                                mensaje = "Se cargó el archivo correctamente!";
                                lblAvisoCargaArchivo.Text = mensaje;
                                lblAvisoCargaArchivo.Visible = true;
                                fu_archivo.Visible = false;
                                btn_Cargar.Visible = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            mensaje = "No se cargó el archivo: " + ex.Message;
                        }
                    }
                    else
                    {
                        mensaje = "Los archivos permitidos son .PDF";
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + mensaje + "');", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Seleccione un archivo en formato PDF!');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aún no ha seleccionado un archivo!');", true);
                return;
            }
        }

        protected void btnVerArchivo_Click(object sender, EventArgs e)
        {
            if (ValidarCargaArchivo() == false)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Aún no ha cargado el RUT!');", true);
                return;
            }
            else
            {
                string ip = ConfigurationManager.AppSettings.Get("RutaIP");
                string ruta = "Documentos/Proyecto/Proyecto_" + CodProyecto + "/";

                string nombrearchivo = "RUT_" + CodProyecto + ".pdf";

                // Limpiamos la salida
                Response.Clear();
                // Con esto le decimos al browser que la salida sera descargable
                Response.ContentType = "application/octet-stream";
                // esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
                Response.AddHeader("Content-Disposition", "attachment; filename=" + nombrearchivo);
                // Escribimos el fichero a enviar 
                Response.WriteFile(ip + ruta + nombrearchivo);
                // volcamos el stream 
                Response.Flush();
                // Enviamos todo el encabezado ahora
                Response.End();
            }
        }
    }
}
