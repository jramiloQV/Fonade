using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarInformePresupuestal : Negocio.Base_Page
    {
        #region Variables.

        /// <summary>
        /// Almacena el SQL generado en variable String para luego se ejecutado.
        /// </summary>
        String txtSQL;

        /// <summary>
        /// Tabla usada en el código fuente para almacenar resultados de las consultas.
        /// </summary>
        DataTable tabla;

        /// <summary>
        /// Id del Informe Presupuestal "seleccionado".
        /// </summary>
        String idInformePresupuestal;

        /// <summary>
        /// Código de la empresa seleccionada.
        /// </summary>
        String CodEmpresa;

        /// <summary>
        /// Se envía desde el método "gv_informesinterventoria_RowCommand" de la página
        /// "InformeEjecucionInter.aspx" (línea 160).
        /// </summary>
        String periodo;

        /// <summary>
        /// Valor obtenido de la consulta hecha en el método "EstablecerEstado".
        /// </summary>
        String Estado;

        /// <summary>
        /// Determina si es un valor nuevo o no.
        /// </summary>
        String NUEVO;

        /// <sumary>
        /// Guarda la Razon social
        /// <sumary>
        String RSocial;


        /// <summary>
        /// Lista que contiene los encabezados de la tabla del informe bimensual.
        /// </summary>
        String[] listaEncabezados = { "Código", "Ámbito", "Cumplimiento a verificar", 
                           "Observación Interventor", "Cumple", "Hacer Seguimiento", "Eliminar", "Modificar" };

        /// <summary>
        /// Nombre del interventor.
        /// </summary>
        String NombreInterventor;

        string accionInforme;

        #endregion

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Establecer título.
            this.Page.Title = "ADICIONAR Informe Presupuestal";

            idInformePresupuestal = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            periodo = HttpContext.Current.Session["Periodo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Periodo"].ToString()) ? HttpContext.Current.Session["Periodo"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";
            NUEVO = HttpContext.Current.Session["NUEVO"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NUEVO"].ToString()) ? HttpContext.Current.Session["NUEVO"].ToString() : "";
            accionInforme = HttpContext.Current.Session["accionInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["accionInforme"].ToString()) ? HttpContext.Current.Session["accionInforme"].ToString() : string.Empty;
            Estado = EstablecerEstado();

            if (!Page.IsPostBack)
            {
                #region Establecer fecha actual en los campos correspondientes.
                DateTime fecha = DateTime.Today;
                c_fecha_s.SelectedDate = fecha;
                if (txtDate.Text.Trim() == "") { txtDate.Text = fecha.ToString("dd/MM/yyyy"); }
                #endregion

                #region Validar nombre...

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformePresupuestal WHERE (id_informepresupuestal =" + idInformePresupuestal + ")))"; }
                else { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto=" + usuario.IdContacto; }

                var dt = consultas.ObtenerDataTable(txtSQL, "text");
                if (dt.Rows.Count > 0)
                { NombreInterventor = dt.Rows[0]["Nombre"].ToString(); }

                L_TituloNombre.Text = "Interventor " + NombreInterventor;
                dt = null;
                txtSQL = "";

                #endregion

                #region Coordinador de interventoría.
                ////Cargar el nombre del coordinador interventor, "usuario creador del usuario en sesión".
                //lblCoordinador.Text = CargarCoordinadorDelInterventor();

                //Coordinador Interventoría
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_informepresupuestal =" + idInformePresupuestal + "))))"; }
                else { txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + ")))"; }
                dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0) { lblCoordinador.Text = dt.Rows[0]["Nombre"].ToString(); }
                dt = null;
                #endregion

                #region Mostrar campos de comentarios (y botón "Enviar" si es Interventor o Interventor Líder).

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    chk_aprobar.Visible = true;
                    //chk_aprobar.Checked = true;
                    btn_grabar.Visible = true;
                    btn_grabar.Text = "Enviar";
                }

                if (Estado == "0" && (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider))
                {
                    btn_grabar.Visible = true;
                    btn_grabar.Text = "Enviar";
                }
                else
                {
                    btn_grabar.Visible = false;
                }

                #endregion

                if (NUEVO == "")//NUEVO
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            #region Coordinador Interventor.

                            txtSQL = " SELECT Estado FROM InformePresupuestal WHERE id_informepresupuestal = " + idInformePresupuestal;
                            var RS = consultas.ObtenerDataTable(txtSQL, "text");
                            if (RS.Rows.Count > 0)
                            {
                                if (RS.Rows[0]["Estado"].ToString() == "2")
                                {
                                    
                                    HttpContext.Current.Session["CodInforme"] = null;
                                    HttpContext.Current.Session["Periodo"] = null;
                                    HttpContext.Current.Session["CodEmpresa"] = null;
                                    HttpContext.Current.Session["NUEVO"] = null;
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.close();", true);
                                    //Response.Redirect("InformeEjecucionInter.aspx");
                                }
                            }
                            RS = null;

                            #endregion
                        }
                        else
                        {
                            #region Gerente Interventor.

                            txtSQL = " SELECT Estado FROM InformePresupuestal WHERE id_informepresupuestal=" + idInformePresupuestal;
                            var RS = consultas.ObtenerDataTable(txtSQL, "text");
                            if (RS.Rows.Count > 0)
                            {
                                if (RS.Rows[0]["Estado"].ToString() == "3")
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.')", true);
                                    HttpContext.Current.Session["CodInforme"] = null;
                                    HttpContext.Current.Session["Periodo"] = null;
                                    HttpContext.Current.Session["CodEmpresa"] = null;
                                    HttpContext.Current.Session["NUEVO"] = null;
                                    Response.Redirect("InformeEjecucionInter.aspx");
                                }
                            }
                            RS = null;

                            #endregion
                        }
                    }

                    #region Mostrar los campos "cuando NO es un nuevo informe".

                    btn_grabar.Visible = true;
                    btn_grabar.Text = "Enviar";
                    txtDate.Visible = false;
                    imgPopup.Visible = false;
                    dd_periodos.Visible = false;
                    lblFecha.Visible = true;
                    lblPeriodo.Visible = true;
                    tabla_datos.Visible = true;

                    #endregion
                }
                else
                {
                    #region Mostrar los campos "cuando SI es un nuevo informe".

                    btn_grabar.Text = "Grabar";
                    btn_grabar.Visible = true;
                    txtDate.Visible = true;
                    imgPopup.Visible = true;
                    dd_periodos.Visible = true;
                    lblFecha.Visible = false;
                    lblPeriodo.Visible = false;
                    tabla_datos.Visible = false;

                    #endregion
                }
            }
            // tabla_datos.Visible = true;
            recogerDatos();
            llenarTabla();
            if (NUEVO == "") { GenerarTabla_InformePresupuestal(); }
        }

        #region Métodos generales.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Cargar el nombre del coordinador interventor quien es el "usuario creador del usuario en sesión".
        /// </summary>
        /// <returns></returns>
        private string CargarCoordinadorDelInterventor()
        {
            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_informepresupuestal =" + idInformePresupuestal + "))))"; }
                else
                { txtSQL = "SELECT Nombres + ' ' + Apellidos AS Nombre FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + ")))"; }

                var t = consultas.ObtenerDataTable(txtSQL, "text");

                if (t.Rows.Count > 0)
                {
                    string NombreCoordinador = t.Rows[0]["Nombre"].ToString();
                    t = null;
                    return NombreCoordinador;
                }
                else
                    return "";
            }
            catch { return ""; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/05/2014.
        /// Determina el estado del informe.
        /// </summary>
        /// <returns>String con dato = variable con resultados de la consulta. // "" vacío = Error.</returns>
        private string EstablecerEstado()
        {
            try
            {
                txtSQL = "SELECT LTRIM(RTRIM(RazonSocial)) AS RazonSocial FROM Empresa WHERE id_empresa=  " + CodEmpresa;
                var dta = consultas.ObtenerDataTable(txtSQL, "text");

                RSocial = dta.Rows[0]["RazonSocial"] != null && !string.IsNullOrEmpty(dta.Rows[0]["RazonSocial"].ToString()) ? dta.Rows[0]["RazonSocial"].ToString() : "";

                txtSQL = "SELECT Estado FROM InformePresupuestal ";
                txtSQL = txtSQL + "WHERE id_InformePresupuestal IN(";
                txtSQL = txtSQL + "SELECT distinct(id_informepresupuestal) as id_informepresupuestal FROM InformePresupuestal WHERE NomInformePresupuestal like '%";
                txtSQL = txtSQL + RSocial.ToString();
                txtSQL = txtSQL + "%')";

                //Consultar estado.|
                //txtSQL = "SELECT Estado FROM InformePresupuestal WHERE id_InformePresupuestal = " + idInformePresupuestal;

                //Establecer resultados de la consulta a variable DataTable.
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (dt.Rows.Count > 0)
                {
                    Estado = dt.Rows[0]["Estado"].ToString();
                    return dt.Rows[0]["Estado"].ToString();
                }
                else
                    Estado = "";
                    return "";
            }
            catch { return ""; }
        }

        #endregion

        #region Métodos para generar la grilla.

        /// <summary>
        /// Cargar los socios de la empresa seleccioanda "Teléfono, Periodo, etc".
        /// </summary>
        private void llenarTabla()
        {
            var infoTanlaAux = new DataTable();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformePresupuestal WHERE (id_InformePresupuestal =" + idInformePresupuestal + ")))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_InformePresupuestal =" + idInformePresupuestal + "))))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblCoordinador.Text = infoTanlaAux.Rows[0][0].ToString();
            }

            if (!string.IsNullOrEmpty(idInformePresupuestal))
            {

                txtSQL = "SELECT LTRIM(RTRIM(RazonSocial)) AS RazonSocial FROM Empresa WHERE id_empresa=  " + CodEmpresa;
                var dta = consultas.ObtenerDataTable(txtSQL, "text");

                RSocial = dta.Rows[0]["RazonSocial"] != null && !string.IsNullOrEmpty(dta.Rows[0]["RazonSocial"].ToString()) ? dta.Rows[0]["RazonSocial"].ToString() : "";

                txtSQL = "SELECT * FROM InformePresupuestal ";
                txtSQL = txtSQL + "WHERE id_InformePresupuestal IN(";
                txtSQL = txtSQL + "SELECT distinct(id_informepresupuestal) as id_informepresupuestal FROM InformePresupuestal WHERE NomInformePresupuestal like '%";
                txtSQL = txtSQL + RSocial.ToString();
                txtSQL = txtSQL + "%')";

                dta.Clear();
                //txtSQL = "SELECT * FROM InformePresupuestal WHERE id_informepresupuestal = " + idInformePresupuestal;

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    switch (Convert.ToInt32(infoTanlaAux.Rows[0]["Periodo"].ToString()))
                    {
                            //hace falta ajustar el iteractive change de el selectedvalue porque siempre queda como escogido el primer bimestre.
                        case 1: lblPeriodo.Text = "Enero-Febrero"; dd_periodos.SelectedValue = "1"; break;
                        case 2: lblPeriodo.Text = "Marzo-Abril"; dd_periodos.SelectedValue = "2"; break;
                        case 3: lblPeriodo.Text = "Mayo-Junio"; dd_periodos.SelectedValue = "3"; break;
                        case 4: lblPeriodo.Text = "Julio-Agosto"; dd_periodos.SelectedValue = "4"; break;
                        case 5: lblPeriodo.Text = "Septiembre-Octubre"; dd_periodos.SelectedValue = "5"; break;
                        case 6: lblPeriodo.Text = "Noviembre-Diciembre"; dd_periodos.SelectedValue = "6"; break;
                    }

                    try
                    { lblFecha.Text = DateTime.Parse(infoTanlaAux.Rows[0]["Fecha"].ToString()).ToString("dd/MM/yyyy"); }
                    catch { lblFecha.Text = new DateTime().ToString("dd/MM/yyyy"); }
                }
            }
            //Limpiar la tabla de los datos a mostrar
            t_table.Rows.Clear();
            txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + CodEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");
            if (infoTanlaAux.Rows.Count > 0)
                lblContrato.Text = infoTanlaAux.Rows[0][0].ToString();

            txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + CodEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                lblEmpresa.Text = infoTanlaAux.Rows[0]["RazonSocial"].ToString();
                lblTelefono.Text = infoTanlaAux.Rows[0]["Telefono"].ToString();
                lblDireccion.Text = infoTanlaAux.Rows[0]["DomicilioEmpresa"].ToString();
                lblCiudad.Text = infoTanlaAux.Rows[0]["NomCiudad"].ToString();
            }

            txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + CodEmpresa + "))";
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            string lista = "";

            foreach (DataRow fila in infoTanlaAux.Rows)
            {
                TableRow filat = new TableRow();
                TableCell celdat = new TableCell();
                celdat.Text = "" + fila["Nombre"].ToString() + " Identificación: " + fila["Identificacion"].ToString();
                filat.Cells.Add(celdat);
                t_table.Rows.Add(filat);
                t_table.DataBind();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/07/2014.
        /// Genera tabla "se tuvo que hacer otra porque la tabla anterior no estaba bien hecha.
        /// </summary>
        private void GenerarTabla_InformePresupuestal()
        {
            //Inicializar variables.
            TableRow fila = new TableRow();
            TableHeaderCell celda_Encabezado = new TableHeaderCell();
            TableCell celda = new TableCell();
            DataTable RS = new DataTable();
            String CodTipoAmbito;
            String NomTipoAmbito;
            DataTable RSAux = new DataTable();
            int incr = 0;
            DataTable RSAux2 = new DataTable();
            DataTable RSEstado = new DataTable();

            try
            {
                #region Generar encabezados "NO se debe colocar el ColumnSpan para evitar errores en diseño".

                foreach (string item in listaEncabezados)
                {
                    celda_Encabezado = new TableHeaderCell();
                    //celda_Encabezado.ColumnSpan = 2;
                    celda_Encabezado.Text = item;
                    fila.Cells.Add(celda_Encabezado);
                    tabla_datos.Rows.Add(fila);
                }

                #endregion

                #region Determina el estado del informe.
                txtSQL = " SELECT Estado FROM InformePresupuestal WHERE id_informepresupuestal = " + idInformePresupuestal;
                RSEstado = consultas.ObtenerDataTable(txtSQL, "text");
                if (RSEstado.Rows.Count > 0) { Estado = RSEstado.Rows[0]["Estado"].ToString(); }
                #endregion

                #region Generar una fila vacía (eran dos, pero por diseño, sólo se dejará una).

                fila = new TableRow();
                celda = new TableCell();
                celda.Text = "&nbsp;";
                fila.Cells.Add(celda);
                tabla_datos.Rows.Add(fila);

                #endregion

                txtSQL = " SELECT * FROM TipoAmbito ";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row_RS in RS.Rows)
                {
                    //Asignar valores.
                    CodTipoAmbito = row_RS["Id_TipoAmbito"].ToString();
                    NomTipoAmbito = row_RS["NomTipoAmbito"].ToString();

                    fila = new TableRow();
                    //Generar celda de encabezado
                    celda = new TableHeaderCell();
                    celda.ColumnSpan = 9;
                    celda.Text = NomTipoAmbito;
                    fila.Cells.Add(celda);
                    tabla_datos.Rows.Add(fila);

                    txtSQL = " SELECT * FROM Ambito WHERE CodTipoAmbito = " + CodTipoAmbito;
                    RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row_RSAux in RSAux.Rows)
                    {
                        //Inicializar la fila.
                        fila = new TableRow();

                        //Celda con el Id de la variable.
                        celda = new TableCell();
                        celda.Style.Add("text-align", "center");
                        celda.Text = row_RSAux["id_Ambito"].ToString();
                        fila.Cells.Add(celda);

                        //Celda con el Nombre de la variable.
                        celda = new TableCell();
                        celda.Text = row_RSAux["NomAmbito"].ToString();
                        fila.Cells.Add(celda);

                        //Celda con el control "si lo posee"...
                        celda = new TableCell();
                        celda.ColumnSpan = 7;
                        celda.VerticalAlign = VerticalAlign.Middle;

                        if (Estado == "0" && (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider))
                        {
                            #region Se añade el control de "Adicionar".

                            #region ImageButton Adicionar.

                            ImageButton img = new ImageButton();
                            img.ID = "img_NSP" + incr.ToString();
                            incr++;
                            img.ImageUrl = "../../Images/icoAdicionar.gif";
                            img.CommandArgument = row_RSAux["id_Ambito"].ToString() + ";" + idInformePresupuestal + ";" + "Adicionar";
                            img.Command += new CommandEventHandler(this.DynammicCommand_5);
                            img.CommandName = "Adicionar";
                            img.BackColor = System.Drawing.Color.White;
                            img.CausesValidation = false;

                            Label lbl = new Label();
                            lbl.ID = "lbl_nw_NSP" + incr.ToString();
                            lbl.Text = "&nbsp;Adicionar";

                            #endregion

                            //Añadir los controles "ImageButton" y "Label con texto (Adicionar)".
                            celda.Controls.Add(img);
                            celda.Controls.Add(lbl);

                            //Añadir la celda que tiene el control a la fila.
                            fila.Cells.Add(celda);

                            #endregion
                        }
                        else
                        { fila.Cells.Add(celda); }

                        //Añadir la fila.
                        tabla_datos.Rows.Add(fila);

                        //Cumplimientos con Seguimiento.
                        txtSQL = " SELECT * FROM AmbitoDetalle WHERE (CodInforme IN (SELECT id_informepresupuestal FROM informepresupuestal " +
                                 " WHERE codempresa = " + CodEmpresa + " AND id_informepresupuestal <>" + idInformePresupuestal + " AND periodo<" + periodo + ")) AND (CodAmbito = " + row_RSAux["id_Ambito"].ToString() + ") AND (Seguimiento = 1)";

                        RSAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                        foreach (DataRow row_RSAux2 in RSAux2.Rows)
                        {
                            //Inicializar la siguiente fila.
                            fila = new TableRow();

                            #region Generar celda con espacio "por defecto".

                            celda = new TableCell();
                            celda.Text = "&nbsp;";
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con valor (En Seguiemiento) "por defecto".

                            celda = new TableCell();
                            celda.Text = "En Seguiemiento";
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con valor (Cumplimiento) "traído de base de datos".

                            celda = new TableCell();
                            celda.Text = row_RSAux2["Cumplimiento"].ToString();
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con valor (Observación) "traído de base de datos".

                            celda = new TableCell();
                            celda.Text = row_RSAux2["Observacion"].ToString();
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda "Si / No" (de acuerdo al valor "Cumple").

                            celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            try { if (Boolean.Parse(row_RSAux2["Cumple"].ToString())) { celda.Text = "Si"; } else { celda.Text = "No"; } }
                            catch { celda.Text = "NULL"; }
                            fila.Cells.Add(celda);

                            #endregion

                            #region Celda con el valor cargado en "IndicadorAsociado".

                            //celda = new TableCell();
                            //celda.Text = row_RSAux2["IndicadorAsociado"].ToString();
                            //fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda "Si / No" (de acuerdo al valor "Seguimiento").

                            celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            try { if (Boolean.Parse(row_RSAux2["Seguimiento"].ToString())) { celda.Text = "Si"; } else { celda.Text = "No"; } }
                            catch { celda.Text = "NULL"; }
                            fila.Cells.Add(celda);

                            #endregion

                            #region News Cells "Eliminar / Modificar".

                            if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
                            {
                                #region Celda Eliminar.

                                celda = new TableCell();
                                celda.Style.Add("align", "center");

                                #region ImageButton "Eliminar Cumplimiento".

                                ImageButton img = new ImageButton();
                                img.ID = "imgElim_NSP" + incr.ToString();
                                incr++;
                                img.ImageUrl = "../../Images/icoEliminar.gif";
                                img.AlternateText = "Eliminar Cumplimiento";
                                img.OnClientClick = "return borrar2();";
                                img.CommandArgument = row_RSAux2["id_AmbitoDetalle"].ToString() + ";" + idInformePresupuestal + ";" + "Borrar2";
                                img.Command += new CommandEventHandler(DynammicCommand_6);
                                //img.CommandName = "Borrar2";
                                img.BackColor = System.Drawing.Color.White;
                                img.CausesValidation = false;

                                #endregion

                                //Añadir control "ImageButton (Eliminar Cumplimiento)".
                                celda.Controls.Add(img);

                                fila.Cells.Add(celda);

                                #endregion

                                #region Celda Modificar.

                                celda = new TableCell();
                                celda.Style.Add("align", "center");

                                #region ImageButton "Modificar Cumplimiento".

                                ImageButton img2 = new ImageButton();
                                img2.ID = "imgModif_NSP" + incr.ToString();
                                incr++;
                                img2.ImageUrl = "../../Images/icoModificar1.gif";
                                img2.AlternateText = "Modificar Cumplimiento";
                                img2.CommandArgument = row_RSAux2["id_AmbitoDetalle"].ToString() + ";" + idInformePresupuestal + ";" + "Editar";
                                img2.Command += new CommandEventHandler(DynammicCommand_5);
                                img2.CommandName = "Editar";
                                img2.BackColor = System.Drawing.Color.White;
                                img2.CausesValidation = false;

                                #endregion

                                //Añadir control "ImageButton (Modificar Cumplimiento)".
                                celda.Controls.Add(img2);

                                fila.Cells.Add(celda);

                                #endregion
                            }
                            else
                            {
                                celda = new TableCell();
                                celda.Style.Add("text-align", "center");
                                fila.Cells.Add(celda);

                                celda = new TableCell();
                                celda.Style.Add("text-align", "center");
                                fila.Cells.Add(celda);
                            }

                            #endregion

                            //Agregar la fila.
                            tabla_datos.Rows.Add(fila);
                        }

                        #region ...

                        //Nuevos Cumplimientos
                        txtSQL = "SELECT * FROM AmbitoDetalle WHERE CodAmbito = " + row_RSAux["id_Ambito"].ToString() + " AND CodInforme = " + idInformePresupuestal;
                        RSAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                        foreach (DataRow row_RSAux2 in RSAux2.Rows)
                        {
                            //Inicializar la siguiente fila.
                            fila = new TableRow();

                            #region Generar celda con espacio (alineada al centro) "por defecto".

                            celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            celda.Text = "&nbsp;";
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con espacio "por defecto".

                            celda = new TableCell();
                            celda.Text = "&nbsp;";
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con valor (Cumplimiento) "traído de base de datos".

                            celda = new TableCell();
                            celda.Text = row_RSAux2["Cumplimiento"].ToString();
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda con valor (Observación) "traído de base de datos".

                            celda = new TableCell();
                            celda.Text = row_RSAux2["Observacion"].ToString();
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda "Si / No" (de acuerdo al valor "Cumple").

                            celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            try { if (Boolean.Parse(row_RSAux2["Cumple"].ToString())) { celda.Text = "Si"; } else { celda.Text = "No"; } }
                            catch { celda.Text = "NULL"; }
                            fila.Cells.Add(celda);

                            #endregion

                            #region Generar celda "Si / No" (de acuerdo al valor "Seguimiento").

                            celda = new TableCell();
                            celda.Style.Add("text-align", "center");
                            try { if (Boolean.Parse(row_RSAux2["Seguimiento"].ToString())) { celda.Text = "Si"; } else { celda.Text = "No"; } }
                            catch { celda.Text = "NULL"; }
                            fila.Cells.Add(celda);

                            #endregion

                            #region News Cells "Eliminar / Modificar".

                            if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
                            {
                                #region Celda Eliminar.

                                celda = new TableCell();
                                celda.Style.Add("align", "center");

                                #region ImageButton "Eliminar Cumplimiento".

                                ImageButton img = new ImageButton();
                                img.ID = "imgElim_NSP_Added" + incr.ToString();
                                incr++;
                                img.ImageUrl = "../../Images/icoEliminar.gif";
                                img.AlternateText = "Eliminar Cumplimiento";
                                img.OnClientClick = "return borrar2();";
                                img.CommandArgument = row_RSAux2["id_AmbitoDetalle"].ToString() + ";" + idInformePresupuestal + ";" + "Borrar2";
                                img.Command += new CommandEventHandler(DynammicCommand_6);
                                //img.CommandName = "Borrar2";
                                img.BackColor = System.Drawing.Color.White;
                                img.CausesValidation = false;

                                #endregion

                                //Añadir control "ImageButton (Eliminar Cumplimiento)".
                                celda.Controls.Add(img);

                                fila.Cells.Add(celda);

                                #endregion

                                #region Celda Modificar.

                                celda = new TableCell();
                                celda.Style.Add("align", "center");

                                #region ImageButton "Modificar Cumplimiento".

                                ImageButton img2 = new ImageButton();
                                img2.ID = "imgModif_NSP_Added" + incr.ToString();
                                incr++;
                                img2.ImageUrl = "../../Images/icoModificar1.gif";
                                img2.AlternateText = "Modificar Cumplimiento";
                                img2.CommandArgument = row_RSAux2["id_AmbitoDetalle"].ToString() + ";" + idInformePresupuestal + ";" + "Editar";
                                img2.Command += new CommandEventHandler(DynammicCommand_5);
                                img2.CommandName = "Editar";
                                img2.BackColor = System.Drawing.Color.White;
                                img2.CausesValidation = false;

                                #endregion

                                //Añadir control "ImageButton (Modificar Cumplimiento)".
                                celda.Controls.Add(img2);

                                fila.Cells.Add(celda);

                                #endregion
                            }
                            else
                            {
                                celda = new TableCell();
                                celda.Style.Add("text-align", "center");
                                fila.Cells.Add(celda);

                                celda = new TableCell();
                                celda.Style.Add("text-align", "center");
                                fila.Cells.Add(celda);
                            }

                            #endregion

                            //Agregar la fila.
                            tabla_datos.Rows.Add(fila);
                        }

                        #endregion

                        //Añadir la fila...
                        tabla_datos.Rows.Add(fila);
                    }

                    incr++;
                }

                //Bindear finalmente la grilla.
                tabla_datos.DataBind();
            }
            catch (Exception ex) { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + ".')", true); }
        }

        private TableHeaderCell crearceladtitulo(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableHeaderCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };

            var titulo1 = new Label { Text = mensaje };
            celda1.Controls.Add(titulo1);

            return celda1;
        }

        private TableCell celdaNormal(String mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            var titulo1 = new Label { Text = mensaje };
            celda1.Controls.Add(titulo1);
            return celda1;
        }

        /// <summary>
        /// Cargar los datos de la empresa "Razón Social y Código del Proyecto".
        /// </summary>
        private void recogerDatos()
        {
            if (!string.IsNullOrEmpty(CodEmpresa))
            {
                if (CodEmpresa != "0")
                {
                    txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa = " + CodEmpresa;

                    var infoEmoresa = consultas.ObtenerDataTable(txtSQL, "text");

                    if (infoEmoresa.Rows.Count > 0)
                    {
                        lblEmpresa.Text = infoEmoresa.Rows[0]["RazonSocial"].ToString();
                        hdf_cod_proyecto.Value = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Grabar o enviar Informe Presupuestal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_grabar_Click(object sender, EventArgs e)
        {
            if (dd_periodos.SelectedValue != "")
            {
                if (EfectuarAccion(btn_grabar.Text))
                {
                    HttpContext.Current.Session["NUEVO"] = null;
                    if (btn_grabar.Text == "Grabar")
                    {
                        btn_grabar.Visible = true;
                        btn_grabar.Text = "Enviar";
                    }
                    else
                    { btn_grabar.Visible = false; btn_grabar.Enabled = false; }
                    lblPeriodo.Visible = true;
                    dd_periodos.Visible = false;
                }
            }
            else
            {
                if (dd_periodos.Visible == true)
                {
                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Seleccione un periodo.')", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/05/2014.
        /// Dependiendo del rol "grupo" y la acción a realizar, aplicará el siguiente ciclo.
        /// </summary>
        /// <param name="accion"></param>
        private bool EfectuarAccion(string accion)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            //REEMPLAZAR VALORES CUANDO SE APAREZCA ESTA PANTALLA!
            TextBox txt_comentariosAprobacion = new TextBox();
            //CheckBox chk_Aprobar = new CheckBox();
            bool correcto = false;
            bool pasoFinal = false;
            DataTable RS = new DataTable();

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Interactuar según el grupo Interventor.

                    switch (accion)
                    {
                        case "Enviar":
                            #region Enviar Informe Presupuestal como Interventor.

                            //Inicializar variables.
                            tabla = null;

                            //Verifica si el interventor tiene un coordinador asignado
                            txtSQL = " SELECT CodCoordinador FROM Interventor WHERE CodContacto = " + usuario.IdContacto;

                            //Asignar resultados de la consulta anterior a variable DataTable.
                            tabla = consultas.ObtenerDataTable(txtSQL, "text");

                            //Si hay resultados:
                            if (tabla.Rows.Count > 0)
                            {
                                //Ejecutar sentencia UPDATE:
                                txtSQL = " UPDATE InformePresupuestal SET Estado = 1 " +
                                         " WHERE id_informepresupuestal = " + idInformePresupuestal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                //Si es correcto, genera una nueva tarea.
                                if (correcto == true)
                                {
                                    //prTareaAsignarCoordinadorInfBimensual //Comentado en FONADE clásico.
                                    AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Presupuestal",
                                        "Revisión del Informe Presupuestal " + lblEmpresa.Text, hdf_cod_proyecto.Value, 24, "0", false, 1,
                                        true, false, usuario.IdContacto,
                                        "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformePresupuestal + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Coordinador Inf Presupuestal");
                                    tarea.Agendar();

                                    //Aquí seguiría ocultar el botón "Enviar".
                                    btn_grabar.Visible = false;
                                    btn_grabar.Enabled = false;
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Presupuestal.')", true);
                                    pasoFinal = false;
                                }
                            }

                            #endregion
                            break;
                        case "Grabar":
                            #region Grabar.

                            //Inicializar variables.
                            String Empresa = "";
                            String Telefono = "";
                            String Direccion = "";
                            String Ciudad = "";

                            #region Obtener información inicial.

                            txtSQL = " SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + CodEmpresa;
                            RS = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RS.Rows.Count > 0)
                            {
                                Empresa = RS.Rows[0]["RazonSocial"].ToString();
                                Telefono = RS.Rows[0]["Telefono"].ToString();
                                Direccion = RS.Rows[0]["DomicilioEmpresa"].ToString();
                                Ciudad = RS.Rows[0]["NomCiudad"].ToString();
                            }

                            #endregion

                            //Se debe colocar así para que el CalendarExtender si ponga la fecha en el campo de texto:
                            DateTime fecha_seleccionada = Convert.ToDateTime(Request.Form[txtDate.UniqueID]);
                            //DateTime a = Convert.ToDateTime(fecha_oculta.Value);
                            //if (fecha_seleccionada == DateTime.Today) { pasoFinal = false; return false; }

                            txtSQL = " SELECT Periodo, codempresa, id_informepresupuestal FROM InformePresupuestal WHERE Periodo = " + dd_periodos.SelectedValue +
                                                         " AND codempresa = " + CodEmpresa;

                            RS = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RS.Rows.Count == 0)
                            {
                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {
                                    //NEW RESULTS:
                                    
                                    cmd = new SqlCommand("MD_Create_InformePresupuestal", con);

                                    //if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    if (con.State == ConnectionState.Broken || con.State == ConnectionState.Closed) { con.Open(); }
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@NomInformePresupuestal", lblEmpresa.Text + " " + dd_periodos.SelectedItem.Text);
                                    cmd.Parameters.AddWithValue("@codinterventor", usuario.IdContacto);
                                    cmd.Parameters.AddWithValue("@codempresa", CodEmpresa);
                                    cmd.Parameters.AddWithValue("@Periodo", dd_periodos.SelectedValue);
                                    //cmd.Parameters.AddWithValue("@Fecha", Convert.ToDateTime(txtDate.Text));
                                    cmd.Parameters.AddWithValue("@Fecha", fecha_seleccionada);
                                    cmd.ExecuteNonQuery();

                                    //con.Close();
                                    //con.Dispose();
                                    cmd.Dispose();

                                    pasoFinal = true;

                                    #region El siguiente paso es mostrar el informe creado.

                                    #region Mostrar campos de comentarios (y botón "Enviar" si es Interventor o Interventor Líder).

                                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                                    {
                                        chk_aprobar.Visible = true;
                                        //chk_aprobar.Checked = true;
                                        btn_grabar.Visible = true;
                                        btn_grabar.Text = "Enviar";
                                    }
                                    if (Estado == "0" && (usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider))
                                    { btn_grabar.Text = "Enviar"; /*t_variable.Visible = true;*/                }

                                    #endregion

                                    string dato = Empresa + " " + dd_periodos.SelectedItem.Text;
                                    txtSQL = " SELECT id_informepresupuestal FROM InformePresupuestal WHERE NomInformePresupuestal = '" + dato + "' AND codinterventor = " + usuario.IdContacto;
                                    DataTable dt = new DataTable();
                                    dt = consultas.ObtenerDataTable(txtSQL, "text");
                                    idInformePresupuestal = dt.Rows[0]["id_informepresupuestal"].ToString();
                                    HttpContext.Current.Session["CodInforme"] = dt.Rows[0]["id_informepresupuestal"].ToString();
                                    HttpContext.Current.Session["INF_idInformePresupuestal"] = dt.Rows[0]["id_informepresupuestal"].ToString();
                                    string a = HttpContext.Current.Session["INF_idInformePresupuestal"].ToString();
                                    dato = null;
                                    dt = null;
                                    pSocios.Controls.Clear();
                                    HttpContext.Current.Session["NUEVO"] = null;
                                    btn_grabar.Text = "Enviar";

                                    btn_grabar.Visible = true;
                                    txtDate.Visible = false;
                                    imgPopup.Visible = false;
                                    dd_periodos.Visible = false;
                                    lblFecha.Visible = true;
                                    lblPeriodo.Visible = true;
                                    tabla_datos.Visible = true;

                                    //Campos y grilla.
                                    recogerDatos();
                                    llenarTabla();
                                    GenerarTabla_InformePresupuestal();

                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al generar el informe presupuestal.\n Error:" + ex.Message + ".')", true);
                                }
                                finally {
                                    con.Close();
                                    con.Dispose();
                                }
                                #region Comentarios.
                                //txtSQL = " SELECT id_informepresupuestal FROM InformePresupuestal " +
                                //         " WHERE NomInformePresupuestal = '" + lblEmpresa.Text + " " + dd_periodos.SelectedItem.Text + "'" +
                                //         " AND CodInterventor = " + usuario.IdContacto + " AND CodEmpresa = " + CodEmpresa +
                                //         " AND Periodo = " + dd_periodos.SelectedValue + " AND Estado = 0 ";

                                //RS = new DataTable();
                                //RS = consultas.ObtenerDataTable(txtSQL, "text");

                                //if (RS.Rows.Count > 0)
                                //{
                                //    HttpContext.Current.Session["CodInforme"] = RS.Rows[0]["id_informepresupuestal"].ToString();//idInformePresupuestal; }
                                //    idInformePresupuestal = HttpContext.Current.Session["CodInforme"].ToString();
                                //} 
                                #endregion
                            }
                            else
                            {
                                #region Mas comentarios.
                                //if (idInformePresupuestal != "0")
                                //{ HttpContext.Current.Session["CodInforme"] = idInformePresupuestal; } 
                                #endregion

                                #region Se supone que hace una especie de PostBack, reasignando la variable "CodInforme"...
                                pSocios.Controls.Clear();
                                idInformePresupuestal = RS.Rows[0]["id_informepresupuestal"].ToString();
                                HttpContext.Current.Session["INF_idInformePresupuestal"] = RS.Rows[0]["id_informepresupuestal"].ToString();
                                string a = HttpContext.Current.Session["INF_idInformePresupuestal"].ToString();
                                HttpContext.Current.Session["NUEVO"] = null;
                                btn_grabar.Text = "Grabar"; //Enviar
                                btn_grabar.Visible = false;
                                txtDate.Visible = false;
                                imgPopup.Visible = false;
                                dd_periodos.Visible = false;
                                lblFecha.Visible = true;
                                lblPeriodo.Visible = true;
                                tabla_datos.Visible = true;

                                //Campos y grilla.
                                recogerDatos();
                                llenarTabla();
                                GenerarTabla_InformePresupuestal();
                                #endregion
                            }
                            #region Comentarios.
                            //RS = null;
                            //txtSQL = null;
                            //recogerDatos();
                            //llenarTabla();
                            //GenerarTabla_InformePresupuestal(); 
                            #endregion
                            #endregion
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                {
                    #region Interactuar según el grupo Coordinador Interventor.

                    switch (accion)
                    {
                        case "Enviar":
                            #region Enviar Informe Presupuestal como Coordinador Interventor.

                            if (chk_aprobar.Checked == true)
                            {
                                #region Si está aprobado...

                                //Inicializar variables.
                                tabla = null;

                                //Verifica si el interventor tiene un coordinador asignado
                                txtSQL = " UPDATE InformePresupuestal SET Estado = 2 " +
                                         " WHERE id_informepresupuestal = " + idInformePresupuestal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                if (correcto == true)
                                {
                                    txtSQL = " SELECT Id_grupo FROM Grupo " +
                                             " WHERE NomGrupo = 'Gerente Interventor' ";

                                    tabla = consultas.ObtenerDataTable(txtSQL, "text");

                                    txtSQL = " SELECT CodContacto FROM GrupoContacto " +
                                             " WHERE CodGrupo = " + tabla.Rows[0]["Id_grupo"].ToString();

                                    tabla = null;
                                    tabla = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (tabla.Rows.Count > 0)
                                    {
                                        //Agendar tarea.
                                        //prTareaAsignarGerenteInfPresupuestal
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Presupuestal",
                                            "Revisión del Informe Presupuestal " + lblEmpresa.Text, hdf_cod_proyecto.Value, 25, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformePresupuestal + "&Periodo=" + dd_periodos.SelectedValue, "", ""); //NO hay nombre del programa en FONADE clásico (ver "Bck20100310_DeclaraVariables").
                                        tarea.Agendar();
                                    }
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Presupuestal.')", true);
                                    pasoFinal = false;
                                }
                                #endregion
                            }

                            #endregion
                            break;
                        case "Grabar":
                            pasoFinal = true;
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
                if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                {
                    #region Interactuar según el grupo de Gerente Interventor.

                    switch (accion)
                    {
                        case "Enviar":
                            #region Enviar Informe Presupuestal como Gerente Interventor.

                            //Si está aprobado.
                            if (chk_aprobar.Checked == true)
                            {
                                #region Si está aprobado...

                                //Consulta.
                                txtSQL = " UPDATE InformePresupuestal SET Estado = 3 WHERE id_informepresupuestal = " + idInformePresupuestal;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                //Si es correcto, genera una nueva tarea.
                                if (correcto == true)
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Informe Presupuestal procesado correctamente.')", true);
                                    pasoFinal = true;
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Presupuestal.')", true);
                                    pasoFinal = false;
                                }
                                #endregion
                            }

                            #endregion
                            break;
                        case "Grabar":
                            pasoFinal = true;
                            break;
                        default:
                            break;
                    }

                    #endregion
                }
            }
            catch { }

            return pasoFinal;
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 19/05/2014.
        /// Verificar si la tarea ya fue aprobada.
        /// </summary>
        private void VerificarTareaAprobada()
        {
            //Cambiar este control cuando se invoque con el usuario correcto!!
            CheckBox chk_aprobar = new CheckBox();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                //Consulta.
                txtSQL = " SELECT Estado FROM InformePresupuestal WHERE id_InformePresupuestal = " + idInformePresupuestal;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (RS.Rows.Count > 0)
                {
                    //Si el estado es igual a 2 y está aprobado, la tarea ya fué asignada.
                    if ((RS.Rows[0]["Estado"].ToString() == "2") && (chk_aprobar.Checked == true)) //1
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert(Tarea ya aprobada.');window.opener.location.reload();window.close();", true);
                    }
                }
            }
            if (usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                //Consulta.
                txtSQL = " SELECT Estado FROM InformePresupuestal WHERE id_InformePresupuestal = " + idInformePresupuestal;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (RS.Rows.Count > 0)
                {
                    //Si el estado es igual a 3 y está aprobado, la tarea ya fué asignada.
                    if ((RS.Rows[0]["Estado"].ToString() == "3") && (chk_aprobar.Checked == true)) //1
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert(Tarea ya aprobada.');window.opener.location.reload();window.close();", true);
                    }
                }
            }
        }

        /// <summary>
        /// Consultar el estado y establacer si se ven o no los campos de la tabla.
        /// </summary>
        /// <param name="codInforme"></param>
        /// <returns>FALSE (Estado != 0) = Ocultar campos. // TRUE (Estado = 0) No ocultar campos.</returns>
        private bool OcultarCampos(String codInforme)
        {
            try
            {
                //Trae el id del informe final
                txtSQL = " SELECT Estado " +
                         " FROM InterventorPresupuestal WHERE Id_InterventorPresupuestal = " + codInforme;

                //Asignar resultados de la consulta a variable DataTable.
                var RS = consultas.ObtenerDataTable(txtSQL, "text");

                //Ocultar botones y sólo mostrar el botón "Imprimir".
                if (RS.Rows[0]["Estado"].ToString() != "0")
                {
                    txtSQL = null;
                    RS = null;
                    return false;
                }
                else
                {
                    txtSQL = null;
                    RS = null;
                    return true;
                }
            }
            catch
            {
                txtSQL = null;
                return true;
            }
        }

        #region Métodos dinámicos. "http://stackoverflow.com/questions/4041137/setting-linkbuttons-onclick-event-to-method-in-codebehind".

        /// <summary>
        /// Evento RowCommand para establecer funcionalidad al seleccionar el control ubicado en el título/encabezado/TableHeader.
        /// Invoca ventana emergente para crear cumplimientos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_1(Object sender, CommandEventArgs e)
        {
            try
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[2];
                HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[0];
                HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                Redirect(null, "CatalogoCumpliInformeInter.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch
            { return; }
        }

        /// <summary>
        /// Evento RowCommand para establecer funcionalidad al seleccionar el control ubicado 
        /// en el detalle del ámbito/encabezado/TableCell.
        /// Invoca ventana emergente para crear o actualizar item informe final.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_2(Object sender, CommandEventArgs e)
        {
            try
            {
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');
                HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[0];
                HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[2];
                HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                Redirect(null, "CatalogoInformeFinalEditarItem.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=710,height=400,top=100");
            }
            catch
            { return; }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/05/2014.
        /// Dependiendo del CommandName "si es Eliminar o Crear", eliminará o creará el cumplimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_3(Object sender, CommandEventArgs e)
        {
            try
            {
                //Inicializar variables.
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                SqlCommand cmd = new SqlCommand();
                bool correcto = false;

                //Inicializar variables para obtener por separado la información almacenada en el CommandArgument.
                var valores_command = new string[] { };
                valores_command = e.CommandArgument.ToString().Split(';');

                //Si es "Eliminar"...
                if (e.CommandName.Equals("eliminar"))
                {
                    #region Eliminar cumplimiento.

                    #region Comentarios.
                    //Eliminar cumplimiento..
                    //System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Item: " + valores_command[2] + ", Informe: " + valores_command[0] + " , Empresa: " + valores_command[1] + "');", true);
                    //return; 
                    #endregion

                    #region PASO 1: Borrar de la tabla de InterventorInformeFinalCumplimiento.

                    txtSQL = " DELETE FROM InterventorInformeFinalCumplimiento " +
                             " WHERE CodInformeFinal = " + valores_command[0] +
                             " AND CodInformeFinalItem = " + valores_command[2] +
                             " AND CodEmpresa = " + valores_command[1];

                    //Asignar SqlCommand para su ejecución.
                    cmd = new SqlCommand(txtSQL, conn);

                    //Ejecutar SQL.
                    correcto = EjecutarSQL(conn, cmd);

                    if (correcto == true)
                    {
                        #region PASO 2: Borrar de la tabla de InterventorInformeFinalItem.

                        txtSQL = " DELETE FROM InterventorInformeFinalItem " +
                                 " WHERE Id_InterventorInformeFinalItem = " + valores_command[2];

                        //Asignar SqlCommand para su ejecución.
                        cmd = new SqlCommand(txtSQL, conn);

                        //Ejecutar SQL.
                        correcto = EjecutarSQL(conn, cmd);

                        if (correcto == true)
                        {
                            //Refrescar la ventana para observar los cambios.
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Cumplimiento eliminado correctamente.');window.location.reload(false);", true);
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un error al eliminar el cumplimiento (2).')", true);
                            return;
                        }

                        #endregion
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Hubo un error al eliminar el cumplimiento (1).')", true);
                        return;
                    }

                    #endregion

                    #endregion
                }
                else
                {
                    //Crear Cumplimiento.
                    HttpContext.Current.Session["s_hdf_CodInforme"] = valores_command[2];
                    HttpContext.Current.Session["s_hdf_CodItem"] = valores_command[0];
                    HttpContext.Current.Session["s_hdf_CodEmpresa"] = valores_command[1];
                    Redirect(null, "CatalogoCumpliInformeInter.aspx", "_blank",
                             "menubar=0,scrollbars=1,width=710,height=400,top=100");
                }
            }
            catch { return; }
        }

        /// <summary>
        /// Sólo para eliminar el detalle seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_4(Object sender, CommandEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            txtSQL = " Delete from VariableDetalle where Id_VariableDetalle = " + e.CommandArgument;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                //NEW RESULTS:

                cmd = new SqlCommand(txtSQL, con);

                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd.Dispose();

                //Vuelve a generar los resultados.
                tabla_datos.Rows.Clear();
                GenerarTabla_InformePresupuestal();
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar el detalle seleccionado.'); window.opener.location.reload(); window.close();", true); return; }
            finally {
                con.Close();
                con.Dispose();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 22/05/2014.
        /// Cargar en variable de sesión "Session["cod_rol"]" el rol del usuario, al ejecutar una consulta SQL.
        /// </summary>
        private void CargarRol()
        {
            try
            {
                txtSQL = " SELECT TOP 1 CodContacto,Rol From EmpresaInterventor,Empresa " +
                       " WHERE id_empresa = codempresa AND CodContacto =" + usuario.IdContacto +
                       " AND inactivo = 0 and FechaFin IS NULL ORDER BY rol DESC";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                    HttpContext.Current.Session["cod_rol"] = dt.Rows[0]["Rol"];
                else
                    HttpContext.Current.Session["cod_rol"] = "0";
            }
            catch
            { HttpContext.Current.Session["cod_rol"] = "0"; }
        }

        /// <summary>
        /// Adicionar o editar un ámbito.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynammicCommand_5(Object sender, CommandEventArgs e)
        {
            //Separar valores.
            string[] valores = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["CodAmbito"] = valores[0];
            HttpContext.Current.Session["CodInforme"] = valores[1];
            HttpContext.Current.Session["Accion"] = e.CommandName;
            Redirect(null, "AdicionarInformePresupuestalDetalle.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=500,height=400,top=100");
        }

        /// <summary>
        /// Eliminar ámbito seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynammicCommand_6(Object sender, CommandEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            //Separar valores.
            string[] valores = e.CommandArgument.ToString().Split(';');
            txtSQL = " Delete from AmbitoDetalle where Id_AmbitoDetalle = " + valores[0];
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            try
            {
                //NEW RESULTS:

                cmd = new SqlCommand(txtSQL, con);

                if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                //con.Close();
                //con.Dispose();
                cmd.Dispose();

                //Vuelve a generar los resultados.
                tabla_datos.Rows.Clear();
                GenerarTabla_InformePresupuestal();
            }
            catch { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar el ámbito seleccionado.'); window.opener.location.reload(); window.close();", true); return; }
            finally {
                con.Close();
                con.Dispose();
            }
        }

        #endregion

        /// <summary>
        /// Imprimir.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["idInformePresupuestal"] = idInformePresupuestal;
            HttpContext.Current.Session["CodEmpresa"] = CodEmpresa;
            Redirect(null, "ImprimirInformeInterventoriaPresup.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        protected void dd_periodos_SelectedIndexChanged(object sender, EventArgs e)
        {
 
        }
    }
}