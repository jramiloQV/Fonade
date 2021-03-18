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
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoInterventor : Negocio.Base_Page
    {
        protected int CodContacto;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Ajustar el texto de la columna de la grilla.
            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            { gvcGerenteInterventor.Columns[3].HeaderText = "<center>interventores</center>"; }
            else { gvcGerenteInterventor.Columns[3].HeaderText = "<center>Empresas</center>"; }

            if (usuario.CodGrupo != Constantes.CONST_GerenteInterventor && usuario.CodGrupo != Constantes.CONST_CoordinadorInterventor)
            {
                Response.Redirect(@"\FONADE\MiPerfil\Home.aspx");
            }

            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                lbl_Titulo.Text = "USUARIO COORDINADOR DE INTERVENTORÍA";
                lbtn_crearInterv.Text = "Adicionar Usuario Coordinador de Interventoria";
                btn_asignar.Text = "Asignar Coordinador a Interventores";
            }
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                lbl_Titulo.Text = "USUARIO INTERVENTORES";
                lbtn_crearInterv.Text = "Adicionar Usuario Interventores";
                btn_asignar.Text = "Asignar interventor a Empresas";
                btnAsignarV2.Visible = true;
            }
            if (!IsPostBack)
            { CargarInterventoresAdministrables(""); }
        }

        #region Métodos generales.

        /// <summary>
        /// Se debe enviar la información de la tabla en una variable se sesión
        /// para poder sortearlo.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private string GetSortDirection(string column)
        {
            string sortDirection = "ASC";
            var sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;

                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        #endregion

        protected void lds_listaInterventors_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var SQL = from G in consultas.VerGerenteInterv()
                          select G;

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {

                }

                switch (gvcGerenteInterventor.SortExpression)
                {
                    case "nombre":
                        if (gvcGerenteInterventor.SortDirection == SortDirection.Ascending)
                            SQL = SQL.OrderBy(r => r.nombre);

                        else
                            SQL.OrderByDescending(r => r.nombre);
                        break;
                    case "email":
                        if (gvcGerenteInterventor.SortDirection == SortDirection.Ascending)
                            SQL = SQL.OrderBy(r => r.email);

                        else
                            SQL.OrderByDescending(r => r.email);
                        break;
                    case "Cantidad":
                        if (gvcGerenteInterventor.SortDirection == SortDirection.Ascending)
                            SQL = SQL.OrderBy(r => r.Cuantos);
                        else
                            SQL = SQL.OrderByDescending(r => r.Cuantos);
                        break;
                    case "inactividad":
                        if (gvcGerenteInterventor.SortDirection == SortDirection.Ascending)
                            SQL = SQL.OrderBy(r => r.inactividad);
                        else
                            SQL = SQL.OrderByDescending(r => r.inactividad);

                        break;
                    default:
                        SQL = SQL.OrderBy(r => r.nombre);
                        break;

                }
                e.Arguments.TotalRowCount = SQL.Count();
                e.Result = SQL;
            }


            catch (Exception exc)
            {
                throw new Exception("Error al seleccionar los datos", exc);

            }

        }

        /// <summary>
        /// Reactivar el perfil Interventor.
        /// </summary>
        /// <param name="interventor">Código del contacto.</param>
        protected void reactivarPerfilInterventor(int interventor)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                SqlCommand cmd = new SqlCommand("MD_ActivarInterventores", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodContacto", interventor);
                SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
                con.Open();
                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd2.Dispose();
                cmd.Dispose();
                Response.Redirect(Request.RawUrl);
            }
            finally {
                con.Close();
                con.Dispose();
            }
        }

        protected void btn_asignar_Click(object sender, EventArgs e)
        {
            if (btn_asignar.Text == "Asignar interventor a Empresas")
            {
                Redirect(null, "FrameInterventorEmpresas.aspx", "_blank", "menubar=0,scrollbars=1,width=820,height=545,top=100");
            }
            else
            {
                Response.Redirect("AsignacionCoordinacionInterventores.aspx");
            }
        }

        protected void ibtn_crearGerentInterv_Click(object sender, ImageClickEventArgs e)
        {

        }

        #region Interventor.

        /// <summary>
        /// Crear Interventor.
        /// </summary>
        private void CrearInterventor()
        {
            HttpContext.Current.Session["ContactoInterventor"] = "0";
            Response.Redirect("Interventor.aspx");
        }

        /// <summary>
        /// Llamada al método "Crear Interventor".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ibtn_crearCoorE_Click(object sender, ImageClickEventArgs e)
        {
            CrearInterventor();
        }

        /// <summary>
        /// Llamada al método "Crear Interventor".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtn_crearInterv_Click(object sender, EventArgs e)
        {
            CrearInterventor();
        }

        #endregion

        #region Coordinador Interventor.

        #endregion


        private void CargarInterventoresAdministrables(String sOrder)
        {
            //Inicializar variables.
            String txtSQL = "";

            if (sOrder.Trim() == "") { sOrder = "c.Nombres, c.Apellidos"; }



            if (usuario.CodGrupo != Constantes.CONST_GerenteInterventor)
            {
                    //CASO COORD INT

                txtSQL = "SELECT id_contacto,nombres + ' ' +apellidos as nombre, email, count(distinct codproyecto) as Cuantos, c.inactivo, " +
                         " count(distinct ec.numcontrato) as Inhabilitado " +
                         "FROM contacto c " +
                         " LEFT JOIN interventor e ON id_contacto=e.codcontacto " +
                         " left join interventorcontrato ec on id_contacto=ec.codcontacto " +
                         " INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = " + Constantes.CONST_Interventor +
                         " LEFT JOIN proyectocontacto pc  ON id_contacto=pc.codcontacto and pc.inactivo=0 and codrol=" + Constantes.CONST_RolInterventor +
                         " WHERE e.CodCoordinador = " + usuario.IdContacto +
                         " and c.codOperador = " + usuario.CodOperador +
                         " GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo " +
                         " ORDER BY " + sOrder;
            }else{
                //CASO GERENTE INT
                txtSQL =   "SELECT id_contacto,nombres + ' ' +apellidos as nombre, email, count(e.codcontacto) as Cuantos, c.inactivo, " +
                           " count(distinct ec.numcontrato) as Inhabilitado " +
                           "FROM contacto c " +
                           " LEFT JOIN interventor e ON id_contacto=e.codcoordinador " +
                           " left join interventorcontrato ec on id_contacto=ec.codcontacto AND ec.Motivo IS NOT NULL AND ec.FechaInicio > GETDATE() AND ec.FechaExpiracion < GETDATE()" +
                           " INNER JOIN grupocontacto gc ON id_contacto=gc.codcontacto and codgrupo = " + Constantes.CONST_CoordinadorInterventor +
                           " where c.codOperador = " + usuario.CodOperador +
                           " GROUP BY id_contacto,Nombres,apellidos,email,c.inactivo " +                           
                           " ORDER BY " + sOrder;

            }



            #region Asignar resultados a la consulta y procesar la información pra bindearla en la grilla.

            //Asignar resultados a la consulta.
            var tabla_sql = consultas.ObtenerDataTable(txtSQL, "text");
            //Asignar la información de la tabla a la variable de sesión.
            HttpContext.Current.Session["dtEmpresas"] = tabla_sql;

            //Bindear la información.
            gvcGerenteInterventor.DataSource = tabla_sql;
            gvcGerenteInterventor.DataBind();

            foreach (GridViewRow grd_Row in this.gvcGerenteInterventor.Rows)
            {
                try
                {
                    LinkButton lnk = (LinkButton)grd_Row.FindControl("lnkbtn_Estado");
                    var img = (ImageButton)grd_Row.FindControl("imgActivarOdesactivarinterventor");

                    if (lnk.Text.Equals("Inactivo"))
                    {
                        img.Visible = true;
                        img.CommandName = "reactivarcoorEval";
                        img.ImageUrl = "~/Images/icoActivar.gif";
                        img.OnClientClick = "return confirm('Esta seguro que desea Activar el interventor seleccionado?');";
                    }
                    else
                    {
                        img.Visible = true;
                        img.CommandName = "inactivarcoorEval";
                        img.ImageUrl = "~/Images/icoBorrar.gif";
                    }
                }
                catch (FormatException) { }
            }

            //Ajustar el texto de la columna de la grilla.
            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            { gvcGerenteInterventor.Columns[3].HeaderText = "<center>interventores</center>"; }
            else { gvcGerenteInterventor.Columns[3].HeaderText = "<center>Empresas</center>"; }

            #endregion
        }

        #region Eventos de la grilla "gvcGerenteInterventor".

        protected void gvcGerenteInterventor_Load(object sender, EventArgs e)
        {


        }

        /// <summary>
        /// RowCommand de la grilla "gvcGerenteInterventor".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcGerenteInterventor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "vercuantos":
                    HttpContext.Current.Session["ContactoInterventor"] = e.CommandArgument.ToString();
                    //Redirect(null, "VerInterventoresCoordinador.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                        Redirect(null, "VerProyectosInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=420,height=465,top=100");
                    else
                        Redirect(null, "VerinterventoresCoordinador.aspx", "_blank", "menubar=0,scrollbars=1,width=420,height=465,top=100");
                    
                    break;
                case "reactivarcoorEval":
                    reactivarPerfilInterventor(Convert.ToInt32(e.CommandArgument));
                    break;

                case "verestador":
                    string enviarsesion = e.CommandArgument.ToString() + "," + Constantes.CONST_Interventor.ToString() + ",Ver";
                    HttpContext.Current.Session["ContactoInterventor"] = enviarsesion;
                    Redirect(null, "DesactivaInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    break;

                case "inactivarcoorEval":
                    string enviarsesion2 = e.CommandArgument.ToString() + "," + Constantes.CONST_Interventor.ToString() + ",Desactivar";
                    HttpContext.Current.Session["ContactoInterventor"] = enviarsesion2;
                    Redirect(null, "DesactivaInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    break;

                case "editacontacto":

                    #region Conjunto de líneas de código.

                    //Instancia del botón seleccionado.
                    LinkButton lnkbtn2 = e.CommandSource as LinkButton;
                    
                    //Usada en "Interventor.aspx".
                    HttpContext.Current.Session["ContactoInterventor"] = e.CommandArgument.ToString();

                    //Crear variables para ser usadas también en "CatalogoContratoInterventor.aspx".
                    HttpContext.Current.Session["NombreContactoSeleccionado"] = lnkbtn2.Text;
                    HttpContext.Current.Session["CodContacto_Seleccionado"] = e.CommandArgument.ToString();

                    #region Establecer valor de la variable a usar en "CatalogoContratoInterventor.aspx".

                    String sql_interno = " SELECT COUNT(*) AS Cuantos " +
                                         " FROM InterventorContrato " +
                                         " WHERE CodContacto = " + e.CommandArgument.ToString() +
                                         " AND FechaInicio < GETDATE() " +
                                         " AND FechaExpiracion > GETDATE()";

                    //Asignar valor de la consulta anterior.
                    var interno_2 = consultas.ObtenerDataTable(sql_interno, "text");

                    //Establecer valor de la variable a usar en "CatalogoContratoInterventor.aspx".
                    if (interno_2.Rows[0]["Cuantos"].ToString() == "0")
                    { HttpContext.Current.Session["Accion_Interventor"] = "Nuevo"; }
                    else
                    { HttpContext.Current.Session["Accion_Interventor"] = "vacio"; /*Quiere decir que es para crear datos...*/ }

                    //Destruir variables.
                    sql_interno = null;
                    interno_2 = null;

                    #endregion

                    //Redirigir al usuario.
                    Response.Redirect("Interventor.aspx");
                    break;

                    #endregion

                case "verhabilitado":

                    #region Habilitado / No habilitado.

                    //Instancia del botón seleccionado.
                    LinkButton lnkbtn = e.CommandSource as LinkButton;

                    //Establecer las variables.
                    string[] palabras = lnkbtn.CommandArgument.Split(';');
                    HttpContext.Current.Session["CodContacto_Seleccionado"] = palabras[0];
                    HttpContext.Current.Session["NombreContactoSeleccionado"] = palabras[1];

                    if (lnkbtn.Text == "Inhabilitado")
                    {
                        HttpContext.Current.Session["Accion_Interventor"] = "Nuevo";
                        Redirect(null, "CatalogoContratoInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=420,height=470,top=100");
                    }
                    if (lnkbtn.Text == "Habilitado")
                    {
                        /*Ver línea 115 de "CatalogoContratoInterventor.aspx", se dirige a esta página, pero no hace nada...*/
                        HttpContext.Current.Session["Accion_Interventor"] = "vacio"; //Quiere decir que es para crear datos...
                        Redirect(null, "CatalogoContratoInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=420,height=470,top=100");
                    }

                    break;

                    #endregion

                case "Estado_Activo_Inactivo":
                    string enviarsesion23 = e.CommandArgument.ToString() + "," + Constantes.CONST_Interventor.ToString() + ",Ver";
                    HttpContext.Current.Session["ContactoInterventor"] = enviarsesion23;
                    Redirect(null, "DesactivaInterventor.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mostrar cierta información...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcGerenteInterventor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Inicializar variables e instancias de controles.
                var hdf_Inactivo = e.Row.FindControl("Hiddeninactivo") as HiddenField; //"Inactivo".
                var hdf_Cuantos = e.Row.FindControl("HiddenNumevals") as HiddenField; //"Cuantos".
                var hdf_Inhabilitado = e.Row.FindControl("Inhabilitado") as HiddenField; //"Inhabilitado".
                //Controles de activar.
                var img_reactivar = e.Row.FindControl("ibtnreactivar") as ImageButton; //Activar.
                var img_desactivar = e.Row.FindControl("ibtninactivar") as ImageButton; //Desactivar.
                //Controles de "Cuantos" = Conteo real.
                var lnkbtn_cuantos = e.Row.FindControl("hlcuantos") as LinkButton;//Conteo real.
                String sql_interno = ""; //Para consultar el conteo de datos.
                //Control que muestra el texto "Inhabilitado" o "Hablitado" según el resultado de la consulta.
                var btn_inhabilitado = e.Row.FindControl("hinhabilitado") as LinkButton;
                //Control que muestra "Activo" o "Inactivo" dependiendo de si está o no inactivo.
                var lnk_estado = e.Row.FindControl("lnkbtn_Estado") as LinkButton;

                //Validar que los campos SI hayan sido detectados a tiempo.
                if (hdf_Inactivo != null && hdf_Cuantos != null && hdf_Inhabilitado != null && lnkbtn_cuantos != null && btn_inhabilitado != null && lnk_estado != null)
                {
                    //Si el valor Inactivo" es igual a 0, muestra los campos para habilitar usuarios.
                    if (hdf_Inactivo.Value == "1") //1
                    {
                        //Mostrar los campos para activar usuario.
                        img_reactivar.Visible = true;
                        img_reactivar.Attributes.Add("onclick", "return AlertaActivar()");
                        img_desactivar.Visible = false;
                    }
                    else
                    {
                        #region Evaluar el valor "Cuantos" inicial, luego se carga el verdadero valor "Cuantos".
                        try
                        {
                            int valorCuantos = 0;
                            valorCuantos = Convert.ToInt32(hdf_Cuantos.Value);

                            if (valorCuantos > 0)
                            {
                                /*No debe mostrar nada en ese campo.*/
                                img_reactivar.Style.Add("display", "none");
                                img_desactivar.Style.Add("display", "none");
                            }
                            else
                            {
                                #region Mostrar campos para "Desactivar usuario" y mostrar el conteo de empresas correcto.
                                //img_desactivar.Visible = true;
                                //img_reactivar.Visible = false;

                                //Ejecutar consulta para mostrar en el valor indicado el conteo de empresas.
                                sql_interno = " SELECT COUNT(*) AS Cuantos " +
                                              " FROM EmpresaInterventor " +
                                              " WHERE Rol IN (6,8) " +
                                              " AND Inactivo = 0 " +
                                              " AND CodContacto = " + lnkbtn_cuantos.CommandArgument.ToString();

                                //Asignar resultado de la consulta a variable DataTable. 
                                var interno = consultas.ObtenerDataTable(sql_interno, "text");

                                //Establecer texto en el botón.
                                lnkbtn_cuantos.Text = interno.Rows[0]["Cuantos"].ToString();
                                if (lnkbtn_cuantos.Text == "0")
                                {
                                    lnkbtn_cuantos.Enabled = false;

                                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                                    {
                                        img_desactivar.Visible = false;
                                    }
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex) { string em = ex.Message; }
                        #endregion
                    }

                    #region Se trae el numero de contratos activos que tiene el Interventor o el Coordinador de interventoria.
                    try
                    {
                        String[] split_data = btn_inhabilitado.CommandArgument.Split(';');
                        sql_interno = " SELECT COUNT(*) AS Cuantos " +
                                                  " FROM InterventorContrato " +
                                                  " WHERE CodContacto = " + split_data[0] +
                                                  " AND FechaInicio < GETDATE() " +
                                                  " AND FechaExpiracion > GETDATE()";

                        //Asignar valor de la consulta anterior.
                        var interno_2 = consultas.ObtenerDataTable(sql_interno, "text");
                        //Establecer texto del control "btn_inhabilitado".
                        if (interno_2.Rows[0]["Cuantos"].ToString() == "0")
                        { btn_inhabilitado.Text = "Inhabilitado"; }
                        else { btn_inhabilitado.Text = "Habilitado"; }
                    }
                    catch (Exception ex) { string em = ex.Message; }
                    #endregion

                    #region Establecer el texto del campo "lnk_estado" (instanciado se llama "lnk_estado") en "Activo" o "Inactivo".

                    try
                    {
                        if (lnk_estado.Text == "False")//"0"
                        {
                            lnk_estado.Enabled = false;
                            lnk_estado.Text = "Activo";
                        }
                        else
                        {
                            //Además se debe hacer algo para que opere según FONADE clásico.
                            //<a href=""javascript:createWindow2('Desactivarinterventor.asp?Accion=Traer&Codinterventor="&RS("Id_Contacto")&"')"" class=underline title='Motivo Inactivación'>Inactivo</a>"
                            lnk_estado.Enabled = true;
                            lnk_estado.ToolTip = "Motivo Inactivación";
                            lnk_estado.Text = "Inactivo";
                        }
                    }
                    catch (Exception ex) { string err = ex.Message;  }


                    #endregion
                }
            }
        }

        /// <summary>
        /// Paginación de la grilla "gvcGerenteInterventor".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcGerenteInterventor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvcGerenteInterventor.PageIndex = e.NewPageIndex;
            CargarInterventoresAdministrables("");
        }

        /// <summary>
        /// Sortear la grilla por "Nombre" o "Email".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvcGerenteInterventor_Sorting(object sender, GridViewSortEventArgs e)
        {
            var dt = HttpContext.Current.Session["dtEmpresas"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvcGerenteInterventor.DataSource = HttpContext.Current.Session["dtEmpresas"];
                gvcGerenteInterventor.DataBind();
            }
        }

        #endregion
    }
}