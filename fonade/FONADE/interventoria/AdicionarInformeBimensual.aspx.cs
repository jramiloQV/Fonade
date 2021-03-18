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
    public partial class AdicionarInformeBimensual : Negocio.Base_Page
    {
        #region Variables.

        String txtSQL;
        DataTable tabla;
        String idInformeBimensual;
        String CodEmpresa;
        String periodo;
        String Estado;
        String NuevoInforme;
        String[] listaEncabezados = { "Código", "Ámbito", "Cumplimiento a verificar", 
                           "Observación Interventor", "Cumple", "Indicador Asociado", 
                           "Hacer Seguimiento", "Eliminar", "Modificar" };

        String NombreInterventor;
        Boolean Miembro;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //Establecer título.
            this.Page.Title = "ADICIONAR INFORME PRESUPUESTAL";

            idInformeBimensual = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            periodo = HttpContext.Current.Session["PeriodoBimensual"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["PeriodoBimensual"].ToString()) ? HttpContext.Current.Session["PeriodoBimensual"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";
            NuevoInforme = HttpContext.Current.Session["NEW_INFORME"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NEW_INFORME"].ToString()) ? HttpContext.Current.Session["NEW_INFORME"].ToString() : "";
            Estado = EstablecerEstado();

           

            if (!Page.IsPostBack)
            {
                #region Establecer fecha actual en los campos correspondientes.
                DateTime fecha = DateTime.Today;
                c_fecha_s.SelectedDate = fecha;
                if (txtDate.Text.Trim() == "") { txtDate.Text = fecha.ToString("dd/MM/yyyy"); }
                #endregion

                #region Cargar el nombre del interventor en sesión.

                //Para traer el nombre del interventor
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual =" + idInformeBimensual + ")))"; }
                else { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto=" + usuario.IdContacto; }

                var dt = consultas.ObtenerDataTable(txtSQL, "text");
                if (dt.Rows.Count > 0) { NombreInterventor = dt.Rows[0]["Nombre"].ToString(); }

                L_TituloNombre.Text = "Interventor " + NombreInterventor;

                dt = null;
                txtSQL = "";

                #endregion

                //Cargar el nombre del coordinador interventor, "usuario creador del usuario en sesión".
                lblCoordinador.Text = CargarCoordinadorDelInterventor();

                if (NuevoInforme == "")
                {
                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                    {
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
                        {
                            #region Coordinador Interventor.

                            txtSQL = "SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual=" + idInformeBimensual;
                            var RS = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RS.Rows.Count > 0)
                            {
                                if (RS.Rows[0]["Estado"].ToString() == "2")
                                {
                                    
                                    HttpContext.Current.Session["CodInforme"] = null;
                                    HttpContext.Current.Session["PeriodoBimensual"] = null;
                                    HttpContext.Current.Session["CodEmpresa"] = null;
                                    HttpContext.Current.Session["NEW_INFORME"] = null;
                                    //Response.Redirect("InformeBimensualInter.aspx");
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.close();", true);
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            #region Gerente Interventor.

                            txtSQL = "SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual=" + idInformeBimensual;
                            var RS = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RS.Rows.Count > 0)
                            {
                                if (Estado.Equals("3"))
                                {
                                    chk_aprobar.Checked = true;
                                }
                                else
                                {
                                    chk_aprobar.Checked = false;
                                }
                                if (RS.Rows[0]["Estado"].ToString() == "3" && chk_aprobar.Checked)
                                {
                                    
                                    HttpContext.Current.Session["CodInforme"] = null;
                                    HttpContext.Current.Session["PeriodoBimensual"] = null;
                                    HttpContext.Current.Session["CodEmpresa"] = null;
                                    HttpContext.Current.Session["NEW_INFORME"] = null;
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Tarea ya aprobada.');window.close();", true);
                                    //Response.Redirect("InformeBimensualInter.aspx");
                                }
                                
                            }

                            #endregion
                        }
                    }

                    #region Mostrar los campos "cuando NO es un nuevo informe".

                    #region Mostrar campos de comentarios (y botón "Enviar" si es Interventor o Interventor Líder).

                    txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa=" + CodEmpresa;

                    var infoEmoresa = consultas.ObtenerDataTable(txtSQL, "text");

                    if (infoEmoresa.Rows.Count > 0)
                    {
                        hdf_cod_proyecto.Value = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                        Miembro = fnMiembroProyecto(usuario.IdContacto, hdf_cod_proyecto.Value);
                    }

                    infoEmoresa = null;

                    if (idInformeBimensual != "0")
                    {
                        if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                        {
                            lblComentarios.Visible = true;
                            chk_aprobar.Visible = true;
                            //chk_aprobar.Checked = true;
                            txt_ComentariosAprobacion.Visible = true;
                            btn_grabar.Visible = true;
                            btn_grabar.Text = "Enviar";
                        }
                        if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())//usuario.CodGrupo
                        { btn_grabar.Visible = true; btn_grabar.Text = "Enviar"; /*t_variable.Visible = true;*/ }
                    }
                    else
                    {
                        btn_grabar.Visible = true;
                        btn_grabar.Text = "Grabar";
                    }

                    #endregion

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

                //VerificarTareaAprobada();

                recogerDatos();
                llenarTabla();
                if (NuevoInforme == "") { GenerarTabla_InformeBimensual(); }
            }
            else
            {
                recogerDatos();
                llenarTabla(); if (NuevoInforme == "") { GenerarTabla_InformeBimensual(); }
            }
        }

        #region Métodos generales.

        private string CargarCoordinadorDelInterventor()
        {
            try
            {
                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM Informebimensual WHERE id_informebimensual =" + idInformeBimensual + "))))"; }
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

        private string EstablecerEstado()
        {
            try
            {
                //Consultar estado.
                txtSQL = " SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;

                //Establecer resultados de la consulta a variable DataTable.
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                //Si tiene datos...
                if (dt.Rows.Count > 0)
                    return dt.Rows[0]["Estado"].ToString();
                else
                    return "0";
            }
            catch { return "0"; }
        }

        #endregion

        #region Métodos para generar la grilla.

        /// <summary>
        /// Llenar tabla.
        /// </summary>
        private void llenarTabla()
        {
            if (NuevoInforme == "")
            {
                #region Mostrar los campos "cuando NO es un nuevo informe".

                //btn_grabar.Text = "Grabar";
                //btn_grabar.Visible = false;
                txtDate.Visible = false;
                imgPopup.Visible = false;
                dd_periodos.Visible = false;
                lblFecha.Visible = true;
                lblPeriodo.Visible = true;
                tabla_datos.Visible = true;

                #endregion
            }

            var infoTanlaAux = new DataTable();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual =" + idInformeBimensual + ")))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    //    L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();

                    txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual =" + idInformeBimensual + "))))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblCoordinador.Text = infoTanlaAux.Rows[0][0].ToString();
            }

            if (!string.IsNullOrEmpty(idInformeBimensual))
            {
                txtSQL = "SELECT * FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    switch (Convert.ToInt32(infoTanlaAux.Rows[0]["Periodo"].ToString()))
                    {
                        case 1: lblPeriodo.Text = "Enero-Febrero 01"; dd_periodos.SelectedValue = "1"; break;
                        case 2: lblPeriodo.Text = "Marzo-Abril 01"; dd_periodos.SelectedValue = "2"; break;
                        case 3: lblPeriodo.Text = "Mayo-Junio 01"; dd_periodos.SelectedValue = "3"; break;
                        case 4: lblPeriodo.Text = "Julio-Agosto 01"; dd_periodos.SelectedValue = "4"; break;
                        case 5: lblPeriodo.Text = "Septiembre-Octubre 01"; dd_periodos.SelectedValue = "5"; break;
                        case 6: lblPeriodo.Text = "Noviembre-Diciembre 01"; dd_periodos.SelectedValue = "6"; break;
                        case 7: lblPeriodo.Text = "Enero-Febrero 02"; dd_periodos.SelectedValue = "7"; break;
                        case 8: lblPeriodo.Text = "Marzo-Abril 02"; dd_periodos.SelectedValue = "8"; break;
                        case 9: lblPeriodo.Text = "Mayo-Junio 02"; dd_periodos.SelectedValue = "9"; break;
                        case 10: lblPeriodo.Text = "Julio-Agosto 02"; dd_periodos.SelectedValue = "10"; break;
                        case 11: lblPeriodo.Text = "Septiembre-Octubre 02"; dd_periodos.SelectedValue = "11"; break;
                        case 12: lblPeriodo.Text = "Noviembre-Diciembre 02"; dd_periodos.SelectedValue = "12"; break;
                        case 13: lblPeriodo.Text = "Enero-Febrero 03"; dd_periodos.SelectedValue = "13"; break;
                        case 14: lblPeriodo.Text = "Marzo-Abril 03"; dd_periodos.SelectedValue = "14"; break;
                        case 15: lblPeriodo.Text = "Mayo-Junio 03"; dd_periodos.SelectedValue = "15"; break;
                        case 16: lblPeriodo.Text = "Julio-Agosto 03"; dd_periodos.SelectedValue = "16"; break;
                        case 17: lblPeriodo.Text = "Septiembre-Octubre 03"; dd_periodos.SelectedValue = "17"; break;
                        case 18: lblPeriodo.Text = "Noviembre-Diciembre 03"; dd_periodos.SelectedValue = "18"; break;
                    }

                    try { lblFecha.Text = Convert.ToDateTime(infoTanlaAux.Rows[0]["Fecha"].ToString()).ToString("dd/MM/yyyy"); }
                    catch { lblFecha.Text = infoTanlaAux.Rows[0]["Fecha"].ToString(); }
                }
            }
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

        private TableCell celdaNormalCONTROL(Control mensaje, Control mensaje2, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo }; ;
            celda1.Controls.Add(mensaje);
            if (mensaje2 != null) { celda1.Controls.Add(mensaje2); }
            return celda1;
        }

        private void recogerDatos()
        {
            if (!string.IsNullOrEmpty(CodEmpresa))
            {
                txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa=" + CodEmpresa;

                var infoEmoresa = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoEmoresa.Rows.Count > 0)
                {
                    //txtNomEmpresa = infoEmoresa.Rows[0]["RazonSocial"].ToString();
                    //CodProyecto = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                    lblEmpresa.Text = infoEmoresa.Rows[0]["RazonSocial"].ToString();
                    hdf_cod_proyecto.Value = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                    Miembro = fnMiembroProyecto(usuario.IdContacto, hdf_cod_proyecto.Value);
                }
            }
        }

        #endregion

        private void GenerarTabla_InformeBimensual()
        {
            //Inicializar variables.
            TableRow fila = new TableRow();
            TableHeaderCell celda_Encabezado = new TableHeaderCell();
            TableCell celda = new TableCell();
            DataTable RS = new DataTable();
            String CodTipoVariable;
            String NomTipoVariable;
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
                txtSQL = " SELECT Estado FROM InformeBimensual WHERE id_informeBimensual = " + idInformeBimensual;
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

                txtSQL = " SELECT * FROM TipoVariable ";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row_RS in RS.Rows)
                {
                    //Asignar valores.
                    CodTipoVariable = row_RS["Id_TipoVariable"].ToString();
                    NomTipoVariable = row_RS["NomTipoVariable"].ToString();

                    fila = new TableRow();
                    //Generar celda de encabezado
                    celda = new TableHeaderCell();
                    celda.ColumnSpan = 9;
                    celda.Text = NomTipoVariable;
                    fila.Cells.Add(celda);
                    tabla_datos.Rows.Add(fila);

                    txtSQL = " SELECT * FROM Variable WHERE CodTipoVariable = " + CodTipoVariable;
                    RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row_RSAux in RSAux.Rows)
                    {
                        //Inicializar la fila.
                        fila = new TableRow();

                        //Celda con el Id de la variable.
                        celda = new TableCell();
                        celda.Style.Add("text-align", "center");
                        celda.Text = row_RSAux["id_Variable"].ToString();
                        fila.Cells.Add(celda);

                        //Celda con el Nombre de la variable.
                        celda = new TableCell();
                        celda.Text = row_RSAux["NomVariable"].ToString();
                        fila.Cells.Add(celda);

                        //Celda con el control "si lo posee"...
                        celda = new TableCell();
                        celda.ColumnSpan = 7;
                        celda.VerticalAlign = VerticalAlign.Middle;

                        if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor || usuario.CodGrupo == Constantes.CONST_RolInterventorLider)
                        {
                            #region Se añade el control de "Adicionar".

                            #region ImageButton Adicionar.

                            ImageButton img = new ImageButton();
                            img.ID = "img_NSP" + incr.ToString();
                            incr++;
                            img.ImageUrl = "../../Images/icoAdicionar.gif";
                            img.CommandArgument = row_RSAux["id_Variable"].ToString() + ";" + idInformeBimensual + ";" + "Adicionar";
                            img.Command += new CommandEventHandler(this.DynamicCommand_3);
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
                        txtSQL = " SELECT * FROM VariableDetalle WHERE (CodInforme IN (SELECT id_InformeBimensual FROM InformeBimensual " +
                                 " WHERE codempresa = " + CodEmpresa + " AND id_InformeBimensual <>" + idInformeBimensual + " AND periodo<" + periodo + "))" +
                                 " AND (CodVariable = " + row_RSAux["id_Variable"].ToString() + ") AND (Seguimiento = 1) ";

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

                            celda = new TableCell();
                            celda.Text = row_RSAux2["IndicadorAsociado"].ToString();
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
                                img.ID = "imgElim_NSP" + incr.ToString();
                                incr++;
                                img.ImageUrl = "../../Images/icoEliminar.gif";
                                img.AlternateText = "Eliminar Cumplimiento";
                                img.OnClientClick = "return borrar2();";
                                img.CommandArgument = row_RSAux2["id_VariableDetalle"].ToString();// +";" + idInformeBimensual + ";" + "Borrar2";
                                img.Command += new CommandEventHandler(DynamicCommand_4);
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
                                img2.CommandArgument = row_RSAux2["id_VariableDetalle"].ToString() + ";" + idInformeBimensual + ";" + "Editar";
                                img2.Command += new CommandEventHandler(this.DynamicCommand_3);
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
                        txtSQL = " SELECT * FROM VariableDetalle WHERE CodVariable = " + row_RSAux["id_Variable"].ToString() + " AND CodInforme = " + idInformeBimensual;
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

                            #region Celda con el valor cargado en "IndicadorAsociado".

                            celda = new TableCell();
                            celda.Text = row_RSAux2["IndicadorAsociado"].ToString();
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

                            if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor || HttpContext.Current.Session["Codrol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())//usuario.CodGrupo 
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
                                img.CommandArgument = row_RSAux2["id_VariableDetalle"].ToString();// +";" + idInformeBimensual + ";" + "Borrar2";
                                img.Command += new CommandEventHandler(DynamicCommand_4);
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
                                img2.CommandArgument = row_RSAux2["id_VariableDetalle"].ToString() + ";" + idInformeBimensual + ";" + "Editar";
                                img2.Command += new CommandEventHandler(this.DynamicCommand_3);
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
            catch (Exception ex) { System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error: " + ex.Message + "')", true); }
        }

        protected void btn_grabar_Click(object sender, EventArgs e)
        {
            #region
            //Ajuste realizado por Alex Flautero
            // Nov 20 de 2014 .El cambio permite utilizar el boton con la acción Enviar sin que solicite el periodo 
            // No es necesario verificart periodo porque ya lo grabaron y se visualiza en el label
            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            //{
            //    EfectuarAccion(btn_grabar.Text);

            //    // Alex Flautero Diciembre 2 de 2014
            //    // 1. Para la persona que comentó el código que coloqué el 20 de Diciembre, por favor verifique bien
            //    // porque para el proceso de de enviar la acción se valida la informacióncon con el label lblPeriodo no con el objeto dd_periodos
            //    // que es el que utilizan para la creación
            //    // 2. Los cambios que realicen en el código debe estar identado, y deben colocar el nombre de quien lo hizo y por qué
            //}
            //else
            //{
            //        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Acción no permitida al usuario.')", true);

            //        /* if (dd_periodos.SelectedValue != "")
            //         {
            //             EfectuarAccion(btn_grabar.Text);
            //         }
            //         else
            //         {
            //             System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Seleccione un periodo.')", true);
            //             return;
            //         }*/
            //}
            
            #endregion
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


        
        private bool EfectuarAccion(string accion)
        {
            //Inicializar variables.
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand();
            //REEMPLAZAR VALORES CUANDO SE APAREZCA ESTA PANTALLA!
            TextBox txt_comentariosAprobacion = new TextBox();
            CheckBox chk_Aprobar = new CheckBox();
            //Se agrega la linea para que el objeto que crearon contenga el dato correcto que esta en el objeto.
            // Nov 21 de 2014 Alex Flautero
            chk_Aprobar.Checked = chk_aprobar.Checked;

            bool correcto = false;
            bool pasoFinal = false;

            try
            {
                if (usuario.CodGrupo == Constantes.CONST_Interventor)
                {
                    #region Interactuar según el grupo Interventor.

                    switch (accion)
                    {
                        case "Enviar":
                            #region Enviar informe bimensual como Interventor.

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
                                txtSQL = " UPDATE InformeBimensual SET Estado = 1 " +
                                         " WHERE id_InformeBimensual = " + idInformeBimensual;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                //Si es correcto, genera una nueva tarea.
                                if (correcto == true)
                                {
                                    //prTareaAsignarCoordinadorInfBimensual
                                    AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Bimensual",
                                        "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 26, "0", false, 1,
                                        true, false, usuario.IdContacto,
                                        "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Coordinador Inf Bimensual");
                                    tarea.Agendar();

                                    //Aquí seguiría ocultar el botón "Enviar".
                                    btn_grabar.Visible = false;
                                    btn_grabar.Enabled = false;
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Bimensual.')", true);
                                    pasoFinal = false;
                                }
                            }

                            #endregion
                            break;
                        case "Grabar":
                            #region Crear el informe bimensual.

                            //Se debe colocar así para que el CalendarExtender si ponga la fecha en el campo de texto:
                            string[] tempsplit = Request.Form[txtDate.UniqueID].Split('/');
                            string joinstring = "/";
                            string newdate = tempsplit[2] + joinstring + tempsplit[1] + joinstring + tempsplit[0];
                            DateTime fecha_seleccionada = DateTime.Parse(newdate);

                            //DateTime a = Convert.ToDateTime(fecha_oculta.Value);
                            //if (fecha_seleccionada == DateTime.Today) { return; }

                            //Inicializar variables.
                            DataTable RS = new DataTable();
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
                            //txtSQL = " SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE RazonSocial=" + lblEmpresa.Text + " and DomicilioEmpresa=" + lblDireccion.Text + " and  CodCiudad = id_ciudad";
                            txtSQL = " SELECT id_informeBimensual, Periodo, codempresa FROM InformeBimensual WHERE Periodo = " + dd_periodos.SelectedValue + " AND codempresa = " + CodEmpresa;
                            RS = consultas.ObtenerDataTable(txtSQL, "text");

                            if (RS.Rows.Count == 0)
                            {
                                #region Inserción del nuevo informe bimensual.
                                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
                                try
                                {
                                    //NEW RESULTS:

                                    cmd = new SqlCommand("MD_Create_InformeBimensual", con);

                                    if (con != null) { if (con.State != ConnectionState.Open) { con.Open(); } }
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@NombreEmpresa", Empresa + " " + dd_periodos.SelectedItem.Text);
                                    cmd.Parameters.AddWithValue("@codinterventor", usuario.IdContacto);
                                    cmd.Parameters.AddWithValue("@codempresa", CodEmpresa);
                                    cmd.Parameters.AddWithValue("@Periodo", dd_periodos.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Fecha", fecha_seleccionada);
                                    //cmd.Parameters.AddWithValue("@Fecha", Convert.ToDateTime(txtDate.Text));

                                    cmd.ExecuteNonQuery();
                                    //con.Close();
                                    //con.Dispose();
                                    cmd.Dispose();
                                    pasoFinal = true;

                                    #region El siguiente paso es mostrar el informe creado.

                                    string dato = Empresa + " " + dd_periodos.SelectedItem.Text;
                                    txtSQL = " SELECT id_informeBimensual FROM InformeBimensual WHERE NomInformeBimensual = '" + dato + "' AND codinterventor = " + usuario.IdContacto;
                                    DataTable dt = new DataTable();
                                    dt = consultas.ObtenerDataTable(txtSQL, "text");
                                    idInformeBimensual = dt.Rows[0]["id_informeBimensual"].ToString();

                                    #region Mostrar campos de comentarios (y botón "Enviar" si es Interventor o Interventor Líder).

                                    if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                                    {
                                        lblComentarios.Visible = true;
                                        chk_aprobar.Visible = true;
                                        //chk_aprobar.Checked = true;
                                        txt_ComentariosAprobacion.Visible = true;
                                        btn_grabar.Visible = true;
                                        btn_grabar.Text = "Enviar";
                                    }
                                    if (Estado == "0" && usuario.CodGrupo == Constantes.CONST_Interventor && HttpContext.Current.Session["CodRol"].ToString() == Constantes.CONST_RolInterventorLider.ToString())//usuario.CodGrupo
                                    { btn_grabar.Visible = true; btn_grabar.Text = "Enviar"; /*t_variable.Visible = true;*/ }

                                    #endregion

                                    HttpContext.Current.Session["INF_idInformeBimensual"] = dt.Rows[0]["id_informeBimensual"].ToString();
                                    string a = HttpContext.Current.Session["INF_idInformeBimensual"].ToString();
                                    dato = null;
                                    dt = null;
                                    t_table.Rows.Clear();
                                    HttpContext.Current.Session["NEW_INFORME"] = null;
                                    txtDate.Visible = false;
                                    imgPopup.Visible = false;
                                    dd_periodos.Visible = false;
                                    lblFecha.Visible = true;
                                    lblPeriodo.Visible = true;
                                    tabla_datos.Visible = true;

                                    //Campos y grilla.
                                    recogerDatos();
                                    llenarTabla();
                                    //Vuelve a generar los resultados.
                                    tabla_datos.Rows.Clear();
                                    GenerarTabla_InformeBimensual();

                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al generar el informe bimensual.\n Error:" + ex.Message + ".')", true);
                                    pasoFinal = false;
                                }
                                finally {
                                    con.Close();
                                    con.Dispose();
                                }
                                #endregion
                            }
                            else
                            {
                                #region Se supone que hace una especie de PostBack, reasignando la variable "CodInforme"...
                                t_table.Rows.Clear();
                                idInformeBimensual = RS.Rows[0]["id_informeBimensual"].ToString();
                                HttpContext.Current.Session["INF_idInformeBimensual"] = RS.Rows[0]["id_informeBimensual"].ToString();
                                string a = HttpContext.Current.Session["INF_idInformeBimensual"].ToString();
                                HttpContext.Current.Session["NEW_INFORME"] = null;
                                btn_grabar.Text = "Grabar";
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
                                GenerarTabla_InformeBimensual();
                                #endregion
                            }

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
                            #region Enviar informe bimensual como Coordinador Interventor.

                            //Inicializar variables.
                            tabla = null;

                            //Verifica si el interventor tiene un coordinador asignado
                            txtSQL = " UPDATE InformeBimensual SET Estado = 2 , " +
                                     " observacionescoordinador = '" + txt_comentariosAprobacion.Text.Trim() + "' " +
                                     " WHERE id_InformeBimensual = " + idInformeBimensual;

                            //Asignar SqlCommand para su ejecución.
                            cmd = new SqlCommand(txtSQL, conn);

                            //Ejecutar SQL.
                            correcto = EjecutarSQL(conn, cmd);

                            //Si es correcto, genera una nueva tarea.
                            if (correcto == true)
                            {
                                #region Continuar.

                                if (chk_Aprobar.Checked == true)
                                {
                                    #region Si está aprobado...
                                    //Consulta:
                                    txtSQL = " SELECT Id_grupo FROM Grupo WHERE NomGrupo = 'Gerente Interventor' ";

                                    //Asignar resultados de la consulta a variable temporal.
                                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Consulta #2:
                                    txtSQL = " SELECT CodContacto FROM GrupoContacto WHERE CodGrupo =" + dt.Rows[0]["Id_grupo"].ToString();

                                    dt = null;
                                    dt = consultas.ObtenerDataTable(txtSQL, "text");

                                    if (dt.Rows.Count > 0)
                                    {
                                        //prTareaAsignarCoordinadorInfBimensual.
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Bimensual",
                                            "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 27, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Gerente Inf Bimensual");
                                        tarea.Agendar();

                                    }

                                    /*
                                     * MODIFICACION SEGUN REQUERIMIENTO REV COORDINT02
                                     * ASIGNAR TAREA GERENTE INTERVENTOR
                                     */

                                    var nhy = new Clases.genericQueries().getGerenteInterventorProyecto(Convert.ToInt32(hdf_cod_proyecto.Value), usuario.CodOperador);
                                    var vfr = new Clases.AgendarTarea(nhy.Id_Contacto, "Revisión del Informe Bimensual", "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 27, "0", false, 1,true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Gerente Inf Bimensual");
                                    vfr.Agendar();

                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
                                    #endregion
                                }
                                else
                                {
                                    #region Si NO está aprobado...

                                    //El coordinador negó la tarea, le devuelve el informe al interventor, explicando porqué...
                                    txtSQL = " UPDATE InformeBimensual SET Estado = 0 WHERE id_InformeBimensual = " + idInformeBimensual;

                                    //Consulta a "ejecutar"...
                                    txtSQL = "SELECT Periodo, CodInterventor FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;

                                    //Asignar resultados a variable DataTable.
                                    var dt = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Si tiene datos...
                                    if (dt.Rows.Count > 0)
                                    {
                                        //Asignar valores encontrados en variables temporales.
                                        string txtPeriodoInformeBi = dt.Rows[0]["Periodo"].ToString();
                                        string CodUsuario = dt.Rows[0]["CodInterventor"].ToString();

                                        //Se genera tarea pendiente para el interventor para que vuelva a ingresar el informe.
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisión del Informe Bimensual",
                                            "El coordinador de Interventoria no aprueba el informe bimensual por los siguientes motivos: " + txt_comentariosAprobacion.Text.Trim() + lblEmpresa.Text, hdf_cod_proyecto.Value, 26, "0", false, 1,
                                            true, false, usuario.IdContacto,
                                            "CodInforme=" + idInformeBimensual + "&Periodo=" + txtPeriodoInformeBi + "&CodEmpresa=" + CodEmpresa, "", "Informe Bimensual");
                                        tarea.Agendar();

                                        var nhy = new Clases.genericQueries().getInterventorProyecto(Convert.ToInt32(hdf_cod_proyecto.Value));
                                        var vfr = new Clases.AgendarTarea(nhy.Id_Contacto, "Revisión del Informe Bimensual", "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 27, "0", false, 1, true, false, usuario.IdContacto,
                                                "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Gerente Inf Bimensual");
                                        vfr.Agendar();

                                        //Como que esta ventana se invoca en ventana emergente, este código refresca 
                                        //la página, lo redirigirá a la página de "".
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Informe se ha devuelto al Interventor!!!')", true);//;window.opener.location.reload();window.close();
                                        Response.Redirect("InformeBimensualInter.aspx");
                                    }

                                    #endregion
                                }

                                #endregion
                            }
                            else
                            {
                                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Bimensual.')", true);
                                pasoFinal = false;
                            }

                            #endregion
                            break;
                        case "Grabar":
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
                            #region Enviar informe bimensual como Gerente Interventor.

                            //Si está aprobado.
                            if (chk_Aprobar.Checked == true)
                            {
                                #region Si está aprobado...

                                //Consulta.
                                txtSQL = " UPDATE InformeBimensual SET Estado = 3 WHERE id_InformeBimensual = " + idInformeBimensual;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                //Si es correcto, genera una nueva tarea.
                                if (correcto == true)
                                {
                                    var nhy = new Clases.genericQueries().getGerenteInterventorProyecto(Convert.ToInt32(hdf_cod_proyecto.Value), usuario.CodOperador);
                                    var vfr = new Clases.AgendarTarea(nhy.Id_Contacto, "Revisión del Informe Bimensual", "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 27, "0", false, 1, true, false, usuario.IdContacto,
                                            "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Gerente Inf Bimensual");
                                    vfr.Agendar();
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Informe Bimensual procesado correctamente.'); window.close();", true);
                                    //Response.Redirect("~/Fonade/Tareas/TareasAgendar.aspx");
                                    pasoFinal = true;
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Bimensual.'); window.close();", true);
                                    //Response.Redirect("~/Fonade/Tareas/TareasAgendar.aspx");
                                    pasoFinal = false;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Si NO está aprobado...

                                //El gerente interventor niega la tarea, se la devuelve al coordinador, explicando porqué...
                                txtSQL = "UPDATE InformeBimensual SET Estado = 1 WHERE id_InformeBimensual = " + idInformeBimensual;

                                //Asignar SqlCommand para su ejecución.
                                cmd = new SqlCommand(txtSQL, conn);

                                //Ejecutar SQL.
                                correcto = EjecutarSQL(conn, cmd);

                                //Si es correcto, continúa el flujo.
                                if (correcto == true)
                                {
                                    //Consulta.
                                    txtSQL = " SELECT Periodo, CodInterventor, CodCoordinador FROM InformeBimensual, Interventor " +
                                             " WHERE CodContacto = CodInterventor AND id_InformeBimensual = " + idInformeBimensual;

                                    //Asignar resultados de la consulta anterior a variable DataTable.
                                    var rsAux = consultas.ObtenerDataTable(txtSQL, "text");

                                    //Si tiene datos, continúa el flujo...
                                    if (rsAux.Rows.Count > 0)
                                    {
                                        string txtPeriodoInformeBi = rsAux.Rows[0]["Periodo"].ToString();
                                        string CodUsuario = rsAux.Rows[0]["CodCoordinador"].ToString();

                                        //Se genera tarea pendiente para el Coordinador de interventor para que le devuelva el informe al interventor.
                                        AgendarTarea tarea = new AgendarTarea(usuario.IdContacto, "Revisar Informe Bimensual",
                                            "El Gerente de Interventoria no aprueba el informe bimensual por los siguientes motivos: " + txt_comentariosAprobacion.Text.Trim(),
                                            hdf_cod_proyecto.Value, 26, "0", false, 1, true, false, usuario.IdContacto,
                                            "Accion=Editar&CodInforme=" + idInformeBimensual + "&Periodo=" + txtPeriodoInformeBi + "&CodEmpresa=" + CodEmpresa, "",
                                            "Informe Bimensual");
                                        tarea.Agendar();

                                        var nhy = new Clases.genericQueries().getInterventorProyecto(Convert.ToInt32(hdf_cod_proyecto.Value));
                                        var vfr = new Clases.AgendarTarea(nhy.Id_Contacto, "Revisión del Informe Bimensual", "Revisión del Informe Bimensual " + lblEmpresa.Text, hdf_cod_proyecto.Value, 27, "0", false, 1, true, false, usuario.IdContacto,
                                                "Accion=Editar&CodEmpresa=" + CodEmpresa + "&CodInforme=" + idInformeBimensual + "&Periodo=" + dd_periodos.SelectedValue, "", "Asignar Gerente Inf Bimensual");
                                        vfr.Agendar();

                                        //Como que esta ventana se invoca en ventana emergente, este código refresca 
                                        //la página, lo redirigirá a la página de "".
                                        System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El Informe se ha devuelto al Coordinador de Interventoria!!!')", true);//;window.opener.location.reload();window.close();
                                        Response.Redirect("InformeBimensualInter.aspx");
                                    }
                                }
                                else
                                {
                                    System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('No se pudo enviar el Informe Bimensual.')", true);
                                    pasoFinal = false;
                                }

                                #endregion
                            }

                            #endregion
                            break;
                        case "Grabar":
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

        private void VerificarTareaAprobada()
        {
            //Cambiar este control cuando se invoque con el usuario correcto!!
            CheckBox chk_Aprobar = new CheckBox();
            //Se agrega la linea para que el objeto que crearon contenga el dato correcto que esta en el objeto.
            // Nov 21 de 2014 Alex Flautero
            chk_Aprobar.Checked = chk_aprobar.Checked;

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor)
            {
                //Consulta.
                txtSQL = " SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;

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
                txtSQL = " SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;

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
        //SANTIAGO SANCHEZ
        private void ValidarPeriodo(DropDownList Periodo,String mensaje)
        {
            if (!Periodo.SelectedValue.Equals("") && !string.IsNullOrEmpty(Periodo.SelectedValue))
                EfectuarAccion(btn_grabar.Text);
            else
            {
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('"+ mensaje +"')", true);
                return;
            }
        }

        #region Métodos dinámicos.

        /// <summary>
        /// Mauricio Arias Olave.
        /// 21/05/2014.
        /// Dependiendo del CommandName "si es Eliminar o Crear", eliminará o detalle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DynamicCommand_3(Object sender, CommandEventArgs e)
        {
            //Separar valores.
            string[] valores = e.CommandArgument.ToString().Split(';');

            HttpContext.Current.Session["CodVariable"] = valores[0];
            HttpContext.Current.Session["CodInforme"] = valores[1];
            HttpContext.Current.Session["Accion"] = e.CommandName;
            Redirect(null, "AdicionarInformeBimensualDetalle.aspx", "_blank",
                         "menubar=0,scrollbars=1,width=500,height=400,top=100");
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
                GenerarTabla_InformeBimensual();
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Error al eliminar el detalle seleccionado.'); window.opener.location.reload(); window.close();", true);
                return;
            }
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
            HttpContext.Current.Session["idInformeBimensual"] = idInformeBimensual;
            HttpContext.Current.Session["CodEmpresa"] = CodEmpresa;
            Redirect(null, "ImprimirInformeInterventoriaBimensual.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        protected void chk_aprobar_CheckedChanged(object sender, EventArgs e)
        {
          /*  if (chk_aprobar.Checked == true)
            {
                //Inicializar variables.
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
                txtSQL = " UPDATE InformeBimensual  SET Estado = 3 WHERE id_InformeBimensual = " + idInformeBimensual;
                SqlCommand cmd = new SqlCommand(txtSQL, conn);
                bool correcto = false;

                //Ejecutar SQL.
                //Enviar el cambio de estado a 3
                correcto = EjecutarSQL(conn, cmd);

                //Si es correcto, genera una nueva tarea.
                if (correcto == true)
                { 
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Se ha aprobado el informe.'); window.opener.location.reload(); window.close();", true);
                    Response.Redirect("TareasAgendar.aspx");   
                }
            }*/
        }
    }
}