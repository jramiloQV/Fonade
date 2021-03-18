using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoIndicadorInter : Negocio.Base_Page
    {
        #region Variables globales.

        string Accion;//EDITAR
        string CodProyecto;
        string CodIndicador;
        string txtSQL;
        Int32 txtTab;
        Int32 CodTipo;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodIndicador = string.Empty;
            Accion = HttpContext.Current.Session["Accion"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Accion"].ToString()) ? HttpContext.Current.Session["Accion"].ToString() : "0";
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodIndicador = HttpContext.Current.Session["id_indicadorinter"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["id_indicadorinter"].ToString()) ? HttpContext.Current.Session["id_indicadorinter"].ToString() : "0";

            //CREAR
            if (!IsPostBack)
            {
                CargarDatos(CodIndicador);
                L_NUEVOINDICADOR.Text = Accion + " Indicador";
                B_Crear.Text = Accion;

                //Mostrar control de "Aprobar".
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    Label1.Visible = true;
                    checkAprobar.Visible = true;
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 03/07/2014.
        /// Cargar la información del indicador seleccionado.
        /// </summary>
        /// <param name="Cod_Indicador">Código del indicador seleccionado.</param>
        private void CargarDatos(String Cod_Indicador)
        {
            //Inicializar variables.
            DataTable rs = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        //Según FONADE clásico, si es Coordinador o Gerente Interventor, los controles deben estar bloqueados.
                        TB_Aspecto.Enabled = false;
                        TB_fechaSeguimiento.Enabled = false;
                        TB_Numerador.Enabled = false;
                        TB_Denominador.Enabled = false;
                        TB_Descripcion.Enabled = false;
                        TB_rango.Enabled = false;
                        TB_Observacion.Enabled = false;
                        DD_TipoIndicador.Enabled = false;

                        //Consulta SQL.
                        txtSQL = " SELECT * FROM InterventorIndicadorTMP WHERE id_indicadorInter = " + Cod_Indicador;
                    }
                    else
                    { txtSQL = " SELECT Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable,Observacion FROM InterventorIndicador WHERE id_indicadorInter = " + Cod_Indicador; }
                }

                if (txtSQL != "")
                {
                    rs = consultas.ObtenerDataTable(txtSQL, "text");

                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor || usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            #region Cargar la información según el Coordinador o el Gerente Interventor.

                            if (rs.Rows.Count > 0)
                            {
                                Accion = "Actualizar";
                                //'txtAccion=rs.Rows[0]["Tarea"].ToString();
                                TB_Aspecto.Text = rs.Rows[0]["Aspecto"].ToString();
                                TB_fechaSeguimiento.Text = rs.Rows[0]["FechaSeguimiento"].ToString();
                                TB_Numerador.Text = rs.Rows[0]["Numerador"].ToString();

                                if (rs.Rows[0]["Denominador"].ToString().Trim() == "") { TB_Denominador.Visible = true; }
                                else { TB_Denominador.Visible = true; TB_Denominador.Text = rs.Rows[0]["Denominador"].ToString(); }

                                TB_Descripcion.Text = rs.Rows[0]["Descripcion"].ToString();
                                TB_rango.Text = rs.Rows[0]["RangoAceptable"].ToString();
                                TB_Observacion.Text = rs.Rows[0]["Observacion"].ToString();
                                //'Session("Tarea") = rs.Rows["Tarea"].ToString();

                                //Chequear el CheckBox si es Gerente Interventor y de acuerdo al valor de la variable.
                                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrEmpty(rs.Rows[0]["ChequeoGerente"].ToString()))
                                        { checkAprobar.Checked = Boolean.Parse(rs.Rows[0]["ChequeoGerente"].ToString()); }
                                    }
                                    catch { checkAprobar.Checked = false; }
                                }

                                //Chequear el CheckBox si es Coordinador Interventor y de acuerdo al valor de la variable.
                                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrEmpty(rs.Rows[0]["ChequeoCoordinador"].ToString()))
                                        { checkAprobar.Checked = Boolean.Parse(rs.Rows[0]["ChequeoCoordinador"].ToString()); }
                                    }
                                    catch { checkAprobar.Checked = false; }
                                }

                                //Destruir la variable.
                                rs = null;
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');this.window.close();", true);
                                CodIndicador = string.Empty;
                                return;
                            }

                            #endregion
                        }
                        else
                        {
                            #region Cargar la información según el Interventor.

                            if (rs.Rows.Count > 0)
                            {
                                Accion = "Modificar";
                                TB_Aspecto.Text = rs.Rows[0]["Aspecto"].ToString();
                                TB_fechaSeguimiento.Text = rs.Rows[0]["FechaSeguimiento"].ToString();
                                TB_Numerador.Text = rs.Rows[0]["Numerador"].ToString();

                                if (rs.Rows[0]["Denominador"].ToString().Trim() == "") { TB_Denominador.Visible = false; }
                                else { TB_Denominador.Text = rs.Rows[0]["Denominador"].ToString(); }

                                TB_Descripcion.Text = rs.Rows[0]["Descripcion"].ToString();
                                TB_rango.Text = rs.Rows[0]["RangoAceptable"].ToString();
                                TB_Observacion.Text = rs.Rows[0]["Observacion"].ToString();
                                rs = null;
                            }

                            #endregion
                        }
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Crear, Actualizar, Eliminar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void B_Crear_Click(object sender, EventArgs e)
        {
            //Inicializar variables.             
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            DataTable RsActividad = new DataTable();
            bool bRepetido = false;
            string correcto = "";
            DataTable Rs = new DataTable();
            String ActividadTmp = "";
            String validado = "";

            //Establecer el valor de la variable "CodTipo" de acuerdo a la selección del ítem en el DropDownList.
            if (DD_TipoIndicador.SelectedValue == "1") { CodTipo = Constantes.CONST_IndicadoresGen; }
            else { CodTipo = Constantes.CONST_IndicadoresEsp; }

            validado = ValidarCampos();

            if (validado.Trim() == "")
            {
                if (B_Crear.Text.Equals("EDITAR") || B_Crear.Text == "Actualizar" || B_Crear.Text == "Modificar")
                {
                    #region Editar.

                    //Si es interventor actualiza los registros en tablas temporales para la aprobación del coordinador 
                    //y el gerente INTERVENTOR.

                    #region Procesar si es Interventor.

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        //Verifica si el interventor tiene un coordinador asignado.
                        txtSQL = "select CodCoordinador from interventor where codcontacto = " + usuario.IdContacto;

                        var dt = consultas.ObtenerDataTable(txtSQL, "text");

                        if (dt.Rows.Count > 0)
                        {
                            #region Asigna la tarea al coordinador.

                            #region COMENTARIOS NO BORRAR!
                            //txtSQL = " Insert into InterventorIndicadorTMP (Id_indicadorinter, CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion, Tarea) " +
                            //         " values (" + CodIndicador + "," + CodProyecto + ",'" + TB_Aspecto.Text.Trim() + "','" + TB_fechaSeguimiento.Text.Trim() + "', '" + TB_Numerador.Text.Trim() + "', '" + TB_Denominador.Text.Trim() + "', '" + TB_Descripcion.Text.Trim() + "', " + TB_rango.Text.Trim() + "," + CodTipo + ",'" + TB_Observacion.Text.Trim() + "','Modificar')";

                            ////Ejecutar consulta.
                            //cmd = new SqlCommand(txtSQL, conn);
                            //correcto = String_EjecutarSQL(conn, cmd);

                            //if (correcto != "") { }
                            ////EN FONADE Clásico, este código está comentado.
                            ////prTareaAsignarCoordinadorIndicadores Rs("CodCoordinador"),Session("CodUsuario"),CodProyecto,txtNomProyecto,CodIndicador  
                            #endregion

                            txtSQL = @" Insert into InterventorIndicadorTMP (Id_indicadorinter, CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion, Tarea) " +
                                     " values (" + CodIndicador + "," + CodProyecto + ",'" + TB_Aspecto.Text.Trim() + "','" + TB_fechaSeguimiento.Text.Trim() + "', '" + TB_Numerador.Text.Trim() + "', '" + TB_Denominador.Text.Trim() + "', '" + TB_Descripcion.Text.Trim() + "', " + TB_rango.Text.Trim() + "," + CodTipo + ",'" + TB_Observacion.Text.Trim() + "','Modificar')";

                            //Ejecutar setencia.
                            ejecutaReader(txtSQL, 2);

                            #endregion
                        }
                        else
                        { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true); }
                    }

                    #endregion

                    #region Coordinador de Interventoría.

                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                    {
                        if (checkAprobar.Checked)
                        {
                            #region Actualización.

                            txtSQL = " UPDATE InterventorIndicadorTMP SET ChequeoCoordinador = 1 " +
                                     " where CodProyecto = " + CodProyecto + " and Id_indicadorinter = " + CodIndicador;

                            //Ejecutar consulta.
                            cmd = new SqlCommand(txtSQL, conn);
                            correcto = String_EjecutarSQL(conn, cmd);

                            if (correcto != "") { }

                            #endregion

                            #region MyRegion (1).

                            txtSQL = " select Id_grupo from Grupo where NomGrupo = 'Gerente Interventor' ";
                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region MyRegion (2).

                            txtSQL = " select CodContacto from GrupoContacto where CodGrupo = " + RsActividad.Rows[0]["Id_Grupo"].ToString();
                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region Comentado en el clásico.

                            //'prTareaAsignarGerenteIndicadores RsActividad("CodContacto"),Session("CodUsuario"),CodProyecto,txtNomProyecto,CodIndicador

                            /*
                             * Actualización requerimiento REV COORDINT14 checklist-final-diciembre 19-02-015
                             * Envío de tareas al Gerente administrador
                             */

                            var gerInterv = new Clases.genericQueries()
                                                .getGerenteInterventorProyecto(Convert.ToInt32(CodProyecto??"0"), usuario.CodOperador);
                            var agendar = 
                            new Clases.AgendarTarea(gerInterv.Id_Contacto, "Verificacion indicador de gestion", "Confirmar actualización del indicador en cuestion", CodProyecto, 13, string.Empty, true, 1, false, false, usuario.IdContacto, string.Empty, string.Empty, string.Empty);
                            agendar.Agendar();
                            #endregion

                            RsActividad = null;
                            txtSQL = null;
                        }
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);        
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
                    }

                    #endregion

                    #region Gerente de interventoría.

                    if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (checkAprobar.Checked)
                        {
                            #region TRAE LOS REGISTROS DE LA TABLA TEMPORAL.

                            txtSQL = " select * from InterventorIndicadorTMP where CodProyecto = " + CodProyecto +
                                     " and Id_indicadorinter = " + CodIndicador;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            #endregion

                            #region INSERTA LOS NUEVOS REGISTROS EN LA TABLA DEFINITIVA.

                            string aspecto = "";
                            string descripcion = "";
                            string observacion = "";

                            try { aspecto = RsActividad.Rows[0]["Aspecto"].ToString().Replace("'", ""); }
                            catch { aspecto = RsActividad.Rows[0]["Aspecto"].ToString(); }

                            try { descripcion = RsActividad.Rows[0]["Descripcion"].ToString().Replace("'", ""); }
                            catch { descripcion = RsActividad.Rows[0]["Descripcion"].ToString(); }

                            try { observacion = RsActividad.Rows[0]["Observacion"].ToString().Replace("'", ""); }
                            catch { observacion = RsActividad.Rows[0]["Observacion"].ToString(); }

                            txtSQL = " INSERT INTO InterventorIndicador(CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion) " +
                                     " VALUES (" + CodProyecto + ", '" + aspecto + "', '" + RsActividad.Rows[0]["FechaSeguimiento"].ToString() + "', '" + RsActividad.Rows[0]["Numerador"].ToString().Replace("'",".") + "', '" + RsActividad.Rows[0]["Denominador"].ToString() + "', '" + descripcion + "', " + RsActividad.Rows[0]["RangoAceptable"].ToString() + "," + RsActividad.Rows[0]["CodTipoIndicadorInter"].ToString() + ", '" + observacion + "')";

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);
                            //cmd = new SqlCommand(txtSQL, conn);
                            //correcto = String_EjecutarSQL(conn, cmd);

                            //if (correcto != "") { }

                            #endregion

                            //Actualizar fecha modificación del tab.
                            prActualizarTabInter(txtTab.ToString(), CodProyecto);
                            bRepetido = false;

                            #region BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL.

                            txtSQL = " DELETE FROM InterventorIndicadorTMP where CodProyecto = " + CodProyecto + " and Id_indicadorinter = " + CodIndicador;

                            //Ejecutar consulta.
                            ejecutaReader(txtSQL, 2);
                            //cmd = new SqlCommand(txtSQL, conn);
                            //correcto = String_EjecutarSQL(conn, cmd);

                            //if (correcto != "") { }

                            #endregion
                        }

                        #region El siguiente código se ha dejado comentado porque no es claro de dónde se crea la variable "Session("Tarea")".

                        #region Actualizar.

                        //        If Session("CodGrupo") = CONST_GerenteInterventor and Session("Tarea") = "Modificar" Then
                        //    If request("AprobarGerente")=1 Then
                        //        'TRAE LOS REGISTROS DE LA TABLA TEMPORAL
                        //        txtSQL = "select * from InterventorIndicadorTMP "&_
                        //                    "where CodProyecto="&CodProyecto&" and Id_indicadorinter="&CodIndicador
                        //        Set RsActividad = Conn.Execute(txtSQL)

                        //        'ACTUALIZA LOS REGISTROS EN LA TABLA DEFINITIVA
                        //        txtSQL=	"UPDATE InterventorIndicador "&_
                        //                "SET Aspecto = '"&replace(RsActividad("Aspecto"),"'","")&"', "&_
                        //                    "FechaSeguimiento = '"&RsActividad("FechaSeguimiento")&"', "&_
                        //                    "Numerador = '"&RsActividad("Numerador")&"', "&_
                        //                    "Denominador = '"&RsActividad("Denominador")&"', "&_
                        //                    "Descripcion = '"&replace(RsActividad("Descripcion"),"'","")&"', "&_
                        //                    "RangoAceptable = "&RsActividad("RangoAceptable")&", "&_
                        //                    "CodTipoIndicadorInter = "&RsActividad("CodTipoIndicadorInter")&", "&_
                        //                    "Observacion = '"&replace(RsActividad("Observacion"),"'","")&"' "&_
                        //                "WHERE Id_IndicadorInter=" & CodIndicador
                        //        Conn.Execute txtSQL

                        //        'Actualizar fecha modificación del tab
                        //        prActualizarTabInter txtTab, CodProyecto
                        //        bRepetido = false

                        //        'BORRAR EL REGISTRO YA ACTUALIZADO DE LA TABLA TEMPORAL
                        //        txtSQL = "DELETE FROM InterventorIndicadorTMP "&_
                        //                        "where CodProyecto="&CodProyecto&" and Id_indicadorinter="&CodIndicador
                        //        Conn.Execute(txtSQL)
                        //    End If
                        //    txtScript = "document.location.reload();" & vbCrLf & "window.close();"
                        //End If

                        //If Session("CodGrupo") = CONST_GerenteInterventor and Session("Tarea") = "Borrar" Then
                        //    If request("AprobarGerente")=1 Then
                        //        'BORRA LOS REGISTROS EN LA TABLA DEFINITIVA
                        //        txtSQL = "DELETE FROM InterventorIndicador WHERE Id_IndicadorInter="&CodIndicador
                        //        Conn.execute(txtSQL)

                        //        'Actualizar fecha modificación del tab
                        //        prActualizarTabInter txtTab, CodProyecto

                        //        'BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL
                        //        txtSQL = "DELETE FROM InterventorIndicadorTMP "&_
                        //                        "where CodProyecto="&CodProyecto&" and Id_indicadorinter="&CodIndicador
                        //        Conn.Execute(txtSQL)
                        //    End If
                        //    txtScript = "document.location.reload();" & vbCrLf & "window.close();"
                        //End If 

                        #endregion

                        #region Eliminar.

                        //            If Session("CodGrupo") = CONST_GerenteInterventor and Session("Tarea") = "Borrar" Then
                        //    If request("AprobarGerente")=1 Then
                        //        'BORRA LOS REGISTROS EN LA TABLA DEFINITIVA
                        //        txtSQL = "DELETE FROM InterventorIndicador WHERE Id_IndicadorInter="&CodIndicador
                        //        Conn.execute(txtSQL)

                        //        'Actualizar fecha modificación del tab
                        //        prActualizarTabInter txtTab, CodProyecto

                        //        'BORRAR EL REGISTRO YA INSERTADO DE LA TABLA TEMPORAL
                        //        txtSQL = "DELETE FROM InterventorIndicadorTMP "&_
                        //                        "where CodProyecto="&CodProyecto&" and Id_indicadorinter="&CodIndicador
                        //        Conn.Execute(txtSQL)
                        //    End If
                        //    txtScript = "document.location.reload();" & vbCrLf & "window.close();"
                        //End If

                        #endregion

                        #endregion

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);

                    }

                    #endregion

                    #endregion
                }
                if (B_Crear.Text.Equals("Modificar"))
                {
                    ClientScriptManager cm = this.ClientScript;

                    txtSQL = " UPDATE InterventorIndicador SET " +
                        " Aspecto = '" + TB_Aspecto.Text + "', " +
                        " FechaSeguimiento = '" + TB_fechaSeguimiento.Text + "', " +
                        " Numerador = '" + TB_Numerador.Text + "', " +
                        " Denominador = '" + TB_Denominador.Text + "', " +
                        " Descripcion = '" + TB_Descripcion.Text + "', " +
                        " RangoAceptable = '" + TB_rango.Text + "', " +
                        " Observacion = '" + TB_Observacion.Text +"'"+
                        " WHERE id_indicadorInter = " + CodIndicador;
                    ejecutaReader(txtSQL, 2);
                    cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>window.close();</script>");
                }
                if (B_Crear.Text.Equals("CREAR"))
                {
                    #region Crear "Nueva versión".

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        if (string.IsNullOrEmpty(TB_Aspecto.Text) ||
                                string.IsNullOrEmpty(TB_Numerador.Text) ||
                                string.IsNullOrEmpty(TB_Descripcion.Text) ||
                                string.IsNullOrEmpty(TB_fechaSeguimiento.Text) ||
                                string.IsNullOrEmpty(TB_rango.Text) ||
                                string.IsNullOrEmpty(DD_TipoIndicador.Text))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hay algunos campos requeridos')", true);
                            return;
                        }
                        else
                        {
                            if (DD_TipoIndicador.SelectedValue.Equals("1"))
                            {
                                if (TB_Denominador.Visible == true)
                                {
                                    if (String.IsNullOrEmpty(TB_Denominador.Text))
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Campo Denominador es requerido')", true);
                                        return;
                                    }
                                }
                            }

                            //Verifica si el interventor tiene un coordinador asignado
                            txtSQL = " select CodCoordinador from interventor where codcontacto = " + usuario.IdContacto;
                            Rs = consultas.ObtenerDataTable(txtSQL, "text");

                            if (Rs.Rows.Count > 0)
                            {
                                ClientScriptManager cm = this.ClientScript;
                                //Asigna la tarea al coordinador
                                //Variable para capturar el número temporal de la actividad
                                String txtSQL2;
                                txtSQL2 = "SELECT max(Id_indicadorinter)+1 FROM InterventorIndicadorTMP";
                                var Num = consultas.ObtenerDataTable(txtSQL2, "text");
                                int numMax = int.Parse(Num.Rows[0][0].ToString());

                                if (numMax > 0)
                                { ActividadTmp = numMax++.ToString(); }
                                else { ActividadTmp = "1"; }

                                #region Inserción (versión 1 COMENTADA).

                                //txtSQL = " Insert into InterventorIndicadorTMP (Id_indicadorinter, CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion) " +
                                //         " values (" + ActividadTmp + "," + CodProyecto + ",'" + TB_Aspecto.Text + "','" + TB_fechaSeguimiento.Text + "', '" + TB_Numerador.Text + "', '" + TB_Denominador.Text + "', '" + TB_Descripcion.Text + "', " + TB_rango.Text + "," + CodTipo + ",'" + TB_Observacion.Text + "')";
                                //try
                                //{
                                //    //NEW RESULTS:
                                //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                //    cmd = new SqlCommand(txtSQL, con);

                                //    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                //    cmd.CommandType = CommandType.Text;
                                //    cmd.ExecuteNonQuery();
                                //    con.Close();
                                //    con.Dispose();
                                //    cmd.Dispose();
                                //}
                                //catch { }

                                ////prTareaAsignarCoordinadorIndicadores Rs("CodCoordinador"),Session("CodUsuario"),CodProyecto,txtNomProyecto,ActividadTmp 

                                #endregion

                                #region Inserción (versión 2).

                                //txtSQL = @" Insert into InterventorIndicadorTMP (Id_indicadorinter, CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion) " +
                                //         " values (" + ActividadTmp + "," + CodProyecto + ",'" + TB_Aspecto.Text + "','" + TB_fechaSeguimiento.Text + "', '" + TB_Numerador.Text + "', '" + TB_Denominador.Text + "', '" + TB_Descripcion.Text + "', " + TB_rango.Text + "," + CodTipo + ",'" + TB_Observacion.Text + "')";

                                //Se actualiza la consulta de la insercion
                                txtSQL = @" Insert into InterventorIndicadorTMP (Id_indicadorinter, CodProyecto, Aspecto, FechaSeguimiento, Numerador, Denominador, Descripcion, RangoAceptable, CodTipoIndicadorInter, Observacion) " +
                                " values (" + ActividadTmp + "," + CodProyecto + ",'" + TB_Aspecto.Text + "','" + TB_fechaSeguimiento.Text + "', '" + TB_Numerador.Text + "', '" + TB_Denominador.Text + "', '" + TB_Descripcion.Text + "', " + TB_rango.Text + "," + CodTipo + ",'" + TB_Observacion.Text + "')";

                                
                                //Ejecutar setencia.
                                ejecutaReader(txtSQL, 2);
                                
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);
                                #endregion
                            }
                            else
                            { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true); }
                        }
                    }


                    #endregion
                }
                if (B_Crear.Text.Equals("ELIMINAR"))
                {
                    #region Eliminar.

                    if (usuario.CodGrupo == Constantes.CONST_Interventor)
                    {
                        //Verifica si el interventor tiene un coordinador asignado.
                        txtSQL = " select CodCoordinador from interventor where codcontacto = " + usuario.IdContacto;

                        Rs = consultas.ObtenerDataTable(txtSQL, "text");

                        if (Rs.Rows.Count > 0)
                        {
                            #region Asigna la tarea al coordinador (inserción).
                            txtSQL = "SELECT * FROM InterventorIndicador WHERE CodIndicador = " + CodIndicador;

                            RsActividad = consultas.ObtenerDataTable(txtSQL, "text");

                            txtSQL = "Insert into InterventorIndicadorTMP (CodIndicador,CodProyecto,Aspecto,FechaSeguimiento,Numerador,Denominador,Descripcion,RangoAceptable,CodTipoIndicadorInter,Observacion,Tarea) " +
                                     " values (" + CodIndicador + "," + CodProyecto + ",'" + RsActividad.Rows[0]["Aspecto"].ToString() + "','" + RsActividad.Rows[0]["FechaSeguimiento"].ToString() + "', '" + RsActividad.Rows[0]["Numerador"].ToString() + "', '" + RsActividad.Rows[0]["Denominador"].ToString() + "', '" + RsActividad.Rows[0]["Descripcion"].ToString() + "', " + RsActividad.Rows[0]["RangoAceptable"].ToString() + "," + RsActividad.Rows[0]["CodTipoIndicadorInter"].ToString() + ",'" + RsActividad.Rows[0]["Observacion"].ToString() + "','Borrar')";
                            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                            try
                            {
                                //NEW RESULTS:

                                cmd = new SqlCommand("MD_Update_EmpresaInterventor", con);

                                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                cmd.CommandType = CommandType.Text;
                                cmd.ExecuteNonQuery();
                                //con.Close();
                                //con.Dispose();
                                cmd.Dispose();
                            }
                            catch { }
                            finally {
                                con.Close();
                                con.Dispose();
                            }
                            #endregion
                        }
                        else { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No tiene ningún coordinador asignado.')", true); }

                        RsActividad = null;
                        Rs = null;
                        txtSQL = null;
                    }

                    //Recargar la página padre y cerrar la actual "emergente".
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);        

                    #endregion
                }
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);        
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + validado + ".')", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.reload();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Salida por solicitud del usuario", "window.close();", true);        
                return;
            }
        }

        /// <summary>
        /// Desactivar controles...
        /// </summary>
        private void desactivarControles()
        {
            TB_Aspecto.Enabled = false;
            TB_Denominador.Enabled = false;
            TB_Descripcion.Enabled = false;
            TB_fechaSeguimiento.Enabled = false;
            TB_Numerador.Enabled = false;
            TB_Observacion.Enabled = false;
            TB_rango.Enabled = false;
            DD_TipoIndicador.Enabled = false;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 09/07/2014.
        /// Validar los campos.
        /// </summary>
        /// <returns>string sin datos = Puede continuar. // string con datos = Error.</returns>
        private string ValidarCampos()
        {
            string valor = "";
            int dato = 0;

            if (TB_Aspecto.Text.Trim() == "")
            {
                valor = Texto("TXT_ASPECTO_REQ");
            }
            if (TB_Aspecto.Text.Trim().Length > 300)
            {
                valor = "El texto no debe tener mas de 300 caracteres";
            }
            if (TB_fechaSeguimiento.Text.Trim() == "")
            {
                valor = Texto("TXT_FECHA_REQ");
            }
            if (TB_Numerador.Text.Trim() == "")
            {
                valor = "Debe Completar la formula del indicador";
            }
            if (TB_Denominador.Visible == true)
            {
                //if (TB_Denominador.Text.Trim() == "")
                //{ valor = "Debe Completar la formula del indicador"; }
            }
            if (TB_Descripcion.Text.Trim() == "")
            {
                valor = Texto("TXT_DESCRIP_REQ");
            }
            if (TB_Descripcion.Text.Trim().Length > 300)
            {
                valor = "El texto no debe tener mas de 300 caracteres";
            }
            try
            {
                dato = Convert.ToInt32(TB_rango.Text.Trim());

                if (dato > 100)
                { valor = Texto("TXT_PORCENTAJE_NOVALIDO"); }
            }
            catch { valor = Texto("TXT_PORCENTAJE_NUM"); }

            return valor;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 09/07/2014.
        /// Mostrar el campo "Denominado" sólo cuando el valor seleccionado de la lista desplegable sea
        /// "Gestión", es decir, el SelectedValue (1).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DD_TipoIndicador_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DD_TipoIndicador.SelectedValue == "1")//Gestión
            { TB_Denominador.Visible = true; }
            else { TB_Denominador.Visible = false; }
        }
    }
}