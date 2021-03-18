using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.Administracion
{
    class BuscarProyectos
    {
        public int Id_Proyecto { get; set; }
        public string NomProyecto { get; set; }
        public bool Inactivo { get; set; }
        public int CodCiudad { get; set; }
    }

    /// <summary>
    /// LegalizacionEmpresas
    /// </summary>    
    public partial class LegalizacionEmpresas : Negocio.Base_Page
    {
        string txtSQL;
        //DataTable datatable;
        string[] arr_meses = { "", "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dic" };
        DateTime fecha_hoy = DateTime.Today;

        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Recuperar la url
            string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

            if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
            {
                Response.Redirect(validacionCuenta.rutaHome(), true);
            }
            else
            {
                gvplanesnegocio.Attributes.CssStyle.Add("font-size", "9px");
                if (!IsPostBack)
                {
                    cargarDllOperador(usuario.CodOperador);
                    mostrarGrilla();
                    llenarGrilla();
                    GenerarFecha_Year();
                    CargarDropDown_Convocatorias();
                    DropDown_Fecha_Actual();
                }

                txtnummemorando.Attributes.Add("onkeypress", "javascript: return ValidNum(event);");
            }
        }

        private void mostrarGrilla()
        {
            if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema 
                || Convert.ToInt32(ddlOperador.SelectedValue) != 0)
            {
                GrillaPlanes.Visible = true;
                GrillaActaLegalizacion.Visible = true;
            }
            else
            {
                GrillaPlanes.Visible = false;
                GrillaActaLegalizacion.Visible = false;
            }
        }

        /// <summary>
        /// Cargar el GridView.
        /// </summary>
        private void llenarGrilla()
        {
            List<BuscarProyectos> proyectos = new List<BuscarProyectos>();

            if (usuario.CodGrupo == Constantes.CONST_AdministradorSistema)
            {
                proyectos = (from p in consultas.Db.Proyecto
                             where p.Inactivo == false && p.CodEstado == Constantes.CONST_LegalizacionContrato
                             && p.codOperador == Convert.ToInt32(ddlOperador.SelectedValue)
                             select new BuscarProyectos
                             {
                                 Id_Proyecto = p.Id_Proyecto,
                                 NomProyecto = p.NomProyecto.ToUpper(),
                                 Inactivo = p.Inactivo,
                                 CodCiudad = p.CodCiudad
                             }).ToList();
            }
            else
            {
                proyectos = (from p in consultas.Db.Proyecto
                             where p.Inactivo == false && p.CodEstado == Constantes.CONST_LegalizacionContrato
                             && p.codOperador == usuario.CodOperador
                             select new BuscarProyectos
                             {
                                 Id_Proyecto = p.Id_Proyecto,
                                 NomProyecto = p.NomProyecto.ToUpper(),
                                 Inactivo = p.Inactivo,
                                 CodCiudad = p.CodCiudad
                             }).ToList();
            }

            foreach (var proyecto in proyectos)
            {
                var objEmp = (from emp in consultas.Db.Empresas
                              where emp.codproyecto == proyecto.Id_Proyecto
                              select emp).FirstOrDefault();
                if (objEmp == null)
                {
                    var objEmpresa = new Empresa
                    {
                        razonsocial = proyecto.NomProyecto,
                        codproyecto = proyecto.Id_Proyecto,
                        CodCiudad = proyecto.CodCiudad,
                        REFechaNorma = DateTime.Now,
                        CFechaNorma = DateTime.Now,
                        DFechaNorma = DateTime.Now,
                        ERFFechaNorma = DateTime.Now,
                        GCFechaNorma = DateTime.Now
                    };
                    consultas.Db.Empresas.InsertOnSubmit(objEmpresa);
                    consultas.Db.SubmitChanges();
                }
            }

            string txtCondicionOperador = "";

            if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema)
            {
                txtCondicionOperador = " and p.codOperador = " + usuario.CodOperador;
            }

            txtSQL = " Select p.Id_Proyecto, Upper(e.razonsocial) NomProyecto, ISNULL(lap.Garantia, 0) Garantia" +
                " , ISNULL(lap.Pagare, 0) Pagare, ISNULL(lap.Contrato, 0) Contrato" +
                " , ISNULL(lap.PlanOperativo, 0) PlanOperativo, ISNULL(lap.Legalizado, 0) Legalizado" +
                " , o.NombreOperador";
            txtSQL += " from Proyecto p Inner Join Empresa e on e.CodProyecto = p.Id_Proyecto ";
            txtSQL += " Left join operador o on p.codOperador = o.idOperador ";
            txtSQL += " Left Join LegalizacionActaProyecto lap on lap.CodProyecto = p.Id_Proyecto " +
                " Where p.Inactivo = 0 " + txtCondicionOperador +
                " and p.CodEstado = " + Constantes.CONST_LegalizacionContrato + " order by 2";


            var mostrarProyectos = consultas.ObtenerDataTable(txtSQL, "text");

            if (mostrarProyectos.Rows.Count > 0)
            {
                gvplanesnegocio.DataSource = mostrarProyectos;
                gvplanesnegocio.DataBind();
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvplanesnegocio control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvplanesnegocio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("proyectoframeset"))
            {
                HttpContext.Current.Session["codProyecto"] = e.CommandArgument.ToString();

                Response.Redirect("~/FONADE/Proyecto/ProyectoFrameSet.aspx");
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(3000);
        }

        /// <summary>
        /// Actualizar legalización de empresas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnactualizar_Click(object sender, EventArgs e)
        {

            //Inicializar variables.
            string validado = "";
            validado = ValidarCampos();

            //Si pasa la validación, continúa con el flujo.
            if (validado == "")
            {
                #region Continuar con el flujo del sistema, generar / actualizar detalles del memorando.

                //string fecha = cldmemorando.SelectedDate.Year + "-" + cldmemorando.SelectedDate.Month + "-" + cldmemorando.SelectedDate.Day + " " + cldmemorando.SelectedDate.Hour + ":" + cldmemorando.SelectedDate.Minute + ":" + cldmemorando.SelectedDate.Second;

                //Obtener la fecha.
                DateTime fecha_now = DateTime.Now;
                string fecha = dd_fecha_year_Memorando.SelectedValue + "-" + dd_fecha_mes_Memorando.SelectedValue + "-" + dd_fecha_dias_Memorando.SelectedValue + " " + fecha_now.Hour + ":" + fecha_now.Minute + ":" + fecha_now.Second;

                #region Inserción.

                //Inicializar variables
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                var objLegaliza = new LegalizacionActa
                {
                    NomActa = txtnommemorando.Text,
                    NumActa = txtnummemorando.Text,
                    FechaActa = Convert.ToDateTime(fecha),
                    Observaciones = txtobservaciones.Text,
                    Publicado = true,
                    CodConvocatoria = int.Parse(ddlconvocatoria.SelectedValue)
                };

                consultas.Db.LegalizacionActas.InsertOnSubmit(objLegaliza);
                consultas.Db.SubmitChanges();

                #endregion

                foreach (GridViewRow gvfila in gvplanesnegocio.Rows)
                {
                    if (((CheckBox)gvfila.FindControl("rbtnlegalizadosi")).Checked || ((CheckBox)gvfila.FindControl("rbtnlegalizadono")).Checked)
                    {
                        Int32 id_proyecto = Convert.ToInt32(gvplanesnegocio.DataKeys[gvfila.RowIndex].Value.ToString());

                        int cbxgarantia = Convert.ToInt32(((CheckBox)gvfila.FindControl("cbxgarantia")).Checked);
                        int cbxpagare = Convert.ToInt32(((CheckBox)gvfila.FindControl("cbxpagare")).Checked);
                        int cbxcontrato = Convert.ToInt32(((CheckBox)gvfila.FindControl("cbxcontrato")).Checked);
                        int cbxplanoperativo = Convert.ToInt32(((CheckBox)gvfila.FindControl("cbxplanoperativo")).Checked);
                        int lealizado = Convert.ToInt32(((RadioButton)gvfila.FindControl("rbtnlegalizadosi")).Checked);

                        txtSQL = " INSERT INTO LegalizacionActaProyecto (CodActa, CodProyecto, garantia, Pagare, Contrato, PlanOperativo, Legalizado) " +
                                 " VALUES(" + objLegaliza.Id_Acta + "," + id_proyecto + "," + cbxgarantia + "," + cbxpagare + "," + cbxcontrato + "," + cbxplanoperativo + "," + lealizado + ")";

                        ejecutaReader(txtSQL, 2);

                        if (((CheckBox)gvfila.FindControl("rbtnlegalizadosi")).Checked)
                        {
                            //Ejecución de procedimientos almacenados
                            //PARA ARREGLAR
                            if (!validarProyectoActividadPOInterventor(id_proyecto))
                            {
                                txtSQL = "pr_LegalizacionPaso1 " + id_proyecto;
                                ejecutaReader(txtSQL, 2);

                                txtSQL = "pr_ActividadDefault " + id_proyecto;
                                ejecutaReader(txtSQL, 2);
                            }

                            if (!validarInterventorNomina(id_proyecto))
                            {
                                txtSQL = "pr_LegalizacionPaso2 " + id_proyecto;
                                ejecutaReader(txtSQL, 2);

                                txtSQL = "pr_LegalizacionPaso3 " + id_proyecto;
                            }

                            if (!validarInterventorProduccion(id_proyecto))
                            {
                                txtSQL = "pr_LegalizacionPaso4 " + id_proyecto;
                                ejecutaReader(txtSQL, 2);
                            }

                            if (!validarInterventorVentas(id_proyecto))
                            {
                                txtSQL = "pr_LegalizacionPaso5 " + id_proyecto;
                                ejecutaReader(txtSQL, 2);
                            }

                            if (!validarInterventorRiesgo(id_proyecto))
                            {
                                txtSQL = "pr_LegalizacionPaso6 " + id_proyecto;
                                ejecutaReader(txtSQL, 2);
                            }

                            Empresa nuevaEmpresa;

                            nuevaEmpresa = (from emp in consultas.Db.Empresas
                                            where emp.codproyecto == id_proyecto
                                            select emp).FirstOrDefault();
                            if (nuevaEmpresa == null)
                            {
                                var proyectoEmpresa = (from proyecto in consultas.Db.Proyecto
                                                       where proyecto.Id_Proyecto == id_proyecto
                                                       select proyecto
                                              ).FirstOrDefault();

                                nuevaEmpresa = new Empresa()
                                {
                                    razonsocial = proyectoEmpresa.NomProyecto,
                                    codproyecto = proyectoEmpresa.Id_Proyecto,
                                    CodCiudad = proyectoEmpresa.CodCiudad,
                                    REFechaNorma = DateTime.Now,
                                    CFechaNorma = DateTime.Now,
                                    ARFechaNorma = DateTime.Now,
                                    DFechaNorma = DateTime.Now,
                                    ERFFechaNorma = DateTime.Now,
                                    GCFechaNorma = DateTime.Now
                                };

                                consultas.Db.Empresas.InsertOnSubmit(nuevaEmpresa);
                                consultas.Db.SubmitChanges();
                            }

                            txtSQL = " insert into indicadorgenerico (CodEmpresa, Nombreindicador, descripcion, numerador, denominador, evaluacion, observacion) " +
                                    " select " + nuevaEmpresa.id_empresa + ", Nombreindicador, descripcion, numerador, denominador, evaluacion, observacion from IndicadoresGenericosModelo ";
                            ejecutaReader(txtSQL, 2);

                            txtSQL = " UPDATE Proyecto SET CodEstado = " + Constantes.CONST_Ejecucion + " WHERE Id_proyecto = " + id_proyecto;
                            ejecutaReader(txtSQL, 2);
                        }
                        if (((CheckBox)gvfila.FindControl("rbtnlegalizadono")).Checked)
                        {
                            txtSQL = "pr_LegalizacionPaso7 " + id_proyecto + "," + Constantes.CONST_Inscripcion + "," + Constantes.CONST_RolEmprendedor;

                            ejecutaReader(txtSQL, 2);
                        }
                    }
                }

                crearGriDVIew(objLegaliza.Id_Acta, objLegaliza.NumActa);

                #region Vaciar campos y establecer fecha actual.

                txtnommemorando.Text = "";
                txtnummemorando.Text = "";
                DropDown_Fecha_Actual();
                ddlconvocatoria.Items[0].Selected = true;
                txtobservaciones.Text = "";

                #endregion

                // Response.Redirect("InterIndicadoresGenericos.aspx");
                #endregion
            }
            else
            {
                //Muestra mensaje de error y/o falta completar el campo.
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + "')", true);
                return;
            }
        }

        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        private bool validarProyectoActividadPOInterventor(int _codproyecto)
        {
            bool insertado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from PO in db.ProyectoActividadPOInterventors
                             where PO.CodProyecto == _codproyecto
                             select PO).Count();

                if (query > 0)
                {
                    insertado = true;
                }
            }

            return insertado;
        }

        private bool validarInterventorNomina(int _codproyecto)
        {
            bool insertado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from N in db.InterventorNominas
                             where N.CodProyecto == _codproyecto
                             select N).Count();

                if (query > 0)
                {
                    insertado = true;
                }
            }

            return insertado;
        }

        private bool validarInterventorProduccion(int _codproyecto)
        {
            bool insertado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from N in db.InterventorProduccions
                             where N.CodProyecto == _codproyecto
                             select N).Count();

                if (query > 0)
                {
                    insertado = true;
                }
            }

            return insertado;
        }

        private bool validarInterventorVentas(int _codproyecto)
        {
            bool insertado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from N in db.InterventorVentas
                             where N.CodProyecto == _codproyecto
                             select N).Count();

                if (query > 0)
                {
                    insertado = true;
                }
            }

            return insertado;
        }

        private bool validarInterventorRiesgo(int _codproyecto)
        {
            bool insertado = false;

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var query = (from N in db.InterventorRiesgo
                             where N.CodProyecto == _codproyecto
                             select N).Count();

                if (query > 0)
                {
                    insertado = true;
                }
            }

            return insertado;
        }

        private void crearGriDVIew(int IdActa, string NumActa)
        {
            DataTable data = new DataTable();

            data.Columns.Add("campo01");
            data.Columns.Add("campo02");
            data.Columns.Add("campo03");
            data.Columns.Add("campo04");
            data.Columns.Add("campo05");
            data.Columns.Add("campo06");
            data.Columns.Add("campo07");
            data.Columns.Add("campo08");
            data.Columns.Add("campo09");
            data.Columns.Add("campo10");

            txtSQL = "SELECT p.Id_Proyecto FROM dbo.Proyecto p INNER JOIN dbo.Ciudad ON p.CodCiudad = dbo.Ciudad.Id_Ciudad " +
            "INNER JOIN dbo.departamento ON dbo.Ciudad.CodDepartamento = dbo.departamento.Id_Departamento " +
            "INNER JOIN LegalizacionActaProyecto ON p.Id_Proyecto = LegalizacionActaProyecto.CodProyecto " +
            "WHERE (p.Inactivo = 0)  AND (p.CodEstado >= " + Constantes.CONST_Ejecucion + ") AND (LegalizacionActaProyecto.CodActa = " + IdActa + ")";

            var dt = consultas.ObtenerDataTable(txtSQL, "text");

            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                i += 1;
                txtSQL = "select p.id_proyecto, p.nomproyecto, e.razonsocial, e.DomicilioEmpresa, e.CodCiudad, e.Telefono, e.Email, c.identificacion" +
                                    " from proyecto p LEFT JOIN (empresa e INNER JOIN (empresacontacto ec INNER JOIN contacto c on ec.codcontacto=c.id_contacto ) ON e.id_empresa = ec.Codempresa) ON" +
                                    " p.id_proyecto=e.codproyecto Where p.id_proyecto = " + dr["ID_Proyecto"].ToString();

                var dtas = consultas.ObtenerDataTable(txtSQL, "text");

                if (dtas.Rows.Count > 0)
                {
                    DataRow fila = data.NewRow();

                    fila["campo01"] = "e";
                    fila["campo02"] = "" + i;
                    fila["campo03"] = dr["ID_Proyecto"].ToString();
                    fila["campo04"] = dtas.Rows[0].ItemArray[1].ToString();
                    fila["campo05"] = dtas.Rows[0].ItemArray[2].ToString();
                    fila["campo06"] = dtas.Rows[0].ItemArray[3].ToString();
                    fila["campo07"] = dtas.Rows[0].ItemArray[4].ToString();
                    fila["campo08"] = dtas.Rows[0].ItemArray[5].ToString();
                    fila["campo09"] = dtas.Rows[0].ItemArray[6].ToString();
                    fila["campo10"] = dtas.Rows[0].ItemArray[7].ToString();

                    data.Rows.Add(fila);
                }

                txtSQL = "select c.id_contacto, c.nombres, c.apellidos, c.identificacion, c.direccion, c.email, pc.participacion" +
                                    " from contacto c, proyectocontacto pc where c.id_contacto = pc.codcontacto and pc.codrol = 3 and pc.inactivo = 0 and pc.codproyecto = " + dr["ID_Proyecto"].ToString();

                dtas = consultas.ObtenerDataTable(txtSQL, "text");

                if (dtas.Rows.Count > 0)
                {
                    DataRow fila = data.NewRow();

                    fila["campo01"] = "s";
                    fila["campo02"] = "" + i;
                    fila["campo03"] = dtas.Rows[0].ItemArray[1].ToString();
                    fila["campo04"] = dtas.Rows[0].ItemArray[2].ToString();
                    fila["campo05"] = dtas.Rows[0].ItemArray[3].ToString();
                    fila["campo06"] = dtas.Rows[0].ItemArray[4].ToString();
                    fila["campo07"] = dtas.Rows[0].ItemArray[5].ToString();
                    fila["campo08"] = dtas.Rows[0].ItemArray[6].ToString();
                    fila["campo09"] = "";
                    fila["campo10"] = "";

                    data.Rows.Add(fila);
                }
            }

            ExportToExcel(data, NumActa);
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Creado exitosamente.')", true);
            llenarGrilla();
            CargarDropDown_Convocatorias();
            DropDown_Fecha_Actual();
        }

        /// <summary>
        /// Exports to excel.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <param name="NumActa">The number acta.</param>
        public void ExportToExcel(DataTable dt, string NumActa)
        {
            if (dt.Rows.Count > 0)
            {
                string saveLocation = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "Confecamaras\\CargueConfecamaras" + NumActa + ".csv";

                var wr = new StreamWriter(saveLocation, false, Encoding.Unicode);
                try
                {
                    //write rows to excel file
                    for (int i = 0; i < (dt.Rows.Count); i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j] != null)
                            {
                                wr.Write("=\"" + Convert.ToString(dt.Rows[i][j]) + "\"" + "\t");
                            }
                            else
                            {
                                wr.Write("\t");
                            }
                        }
                        //go to next line
                        wr.WriteLine();
                    }
                    //close file
                    wr.Close();
                }
                catch (Exception)
                {

                }
            }
        }


        private void CargarDropDown_Convocatorias()
        {
            //Inicializar variables.
            String txtSQL;
            DataTable tabla = new DataTable();

            try
            {
                string txtCondicionOperador = "";
                if (usuario.CodGrupo != Constantes.CONST_AdministradorSistema)
                {
                    txtCondicionOperador = " where codOperador = " + usuario.CodOperador;
                }
                else
                {
                    txtCondicionOperador = " where codOperador = " + ddlOperador.SelectedValue;
                }

                txtSQL = " SELECT Id_Convocatoria, NomConvocatoria FROM Convocatoria " + txtCondicionOperador;

                tabla = consultas.ObtenerDataTable(txtSQL, "text");

                ddlconvocatoria.Items.Clear();

                ListItem item_default = new ListItem();
                item_default.Value = "";
                item_default.Text = "Seleccione";
                ddlconvocatoria.Items.Add(item_default);
                item_default = null;

                foreach (DataRow row in tabla.Rows)
                {
                    ListItem item = new ListItem();
                    item.Value = row["Id_Convocatoria"].ToString();
                    item.Text = row["NomConvocatoria"].ToString();
                    ddlconvocatoria.Items.Add(item);
                    item = null;
                }

                txtSQL = null;
                tabla = null;
            }
            catch { }
        }

        private void GenerarFecha_Year()
        {
            try
            {
                int currentYear = DateTime.Today.AddYears(-11).Year;
                int futureYear = DateTime.Today.AddYears(5).Year;

                for (int i = currentYear; i < futureYear; i++)
                {
                    ListItem item = new ListItem();
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    dd_fecha_year_Memorando.Items.Add(item);
                }
            }
            catch { }
        }

        private void DropDown_Fecha_Actual()
        {
            dd_fecha_dias_Memorando.SelectedValue = fecha_hoy.Day.ToString();
            dd_fecha_mes_Memorando.SelectedValue = fecha_hoy.Month.ToString();
            dd_fecha_year_Memorando.SelectedValue = fecha_hoy.Year.ToString();
        }

        private string ValidarCampos()
        {
            //Inicializar variables.
            string resultado = "";

            if (txtnummemorando.Text.Trim() == "")
            { resultado = "Debe Ingresar un valor de Numero Memorando"; return resultado; }
            if (txtnommemorando.Text.Trim() == "")
            { resultado = "Debe Ingresar un valor de Nombre Memorando"; return resultado; }
            if (ddlconvocatoria.SelectedValue == "")
            { resultado = "Debe Selecionar una convocatoria"; return resultado; }
            if (txtobservaciones.Text.Trim() == "")
            { resultado = "Debe Ingresar comentarios"; return resultado; }

            return resultado;
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDllOperador(int? _codOperador)
        {
            ddlOperador.DataSource = operadorController.cargaDLLOperador(_codOperador);
            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataBind();
        }

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            mostrarGrilla();
            llenarGrilla();            
            CargarDropDown_Convocatorias();
        }
    }
}