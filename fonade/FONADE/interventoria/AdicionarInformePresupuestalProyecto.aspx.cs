using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Datos;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarInformePresupuestalProyecto : Negocio.Base_Page
    {
        #region Variables globales.

        String CodEmpresa;
        String Empresa;
        String Telefono;
        String Direccion;
        String Ciudad;
        String NomTipoVariable;
        String RsAux;
        String CodTipoVariable;
        DataTable RSAux2;
        DataTable RSAux3;
        DataTable RSPeriodo;
        String txtNomEmpresa;
        String CodProyecto;
        String CodInforme;
        String Chequeo;
        DataTable RSEstado;
        String Estado;
        String txtScript;
        String txtPeriodoInformeBi;
        String ObjTarea;
        String CodUsuario;
        String txtTarea;
        String txtSQL;
        /// <summary>
        /// Usada para validar la fecha obtenida de cla consulta y procesarla debidamente.
        /// </summary>
        DateTime fecha;
        /// <summary>
        /// Periodo obtenido de la variable de sesión equivalente.
        /// </summary>
        String Periodo;

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //Obtener los valores de las variables de sesión.
            CodProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? HttpContext.Current.Session["CodProyecto"].ToString() : "0";
            CodInforme = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";
            Periodo = HttpContext.Current.Session["Periodo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Periodo"].ToString()) ? HttpContext.Current.Session["Periodo"].ToString() : "0";

            if (!IsPostBack)
            {
                if (CodEmpresa != "0" && CodInforme != "0")
                {
                    CargarDatosInforme();
                    guardaBimestrl();
                    comleeeGrid();
                }
            }
        }

        #region Métodos de geenrar tabla de informe.

        private void guardaBimestrl()
        {
            var infoTanlaAux = new DataTable();

            string NomPeriodo = lblPeriodo.Text;
            var fechaStr = lblFecha.Text.Split('/');
            fecha = DateTime.Parse(lblFecha.Text);

            txtSQL = "SELECT Periodo,codempresa FROM InformePresupuestal WHERE Periodo=" + Periodo + " AND codempresa=" + CodEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                txtSQL = @"Insert into InformePresupuestal (NomInformePresupuestal,codinterventor,codempresa,Estado,Periodo,Fecha) " +
                    "values ('" + lblEmpresa.Text + " " + NomPeriodo + "'," + usuario.IdContacto + "," + CodEmpresa + ",0," + Periodo + ", Cast('" + fechaStr[2]+"-" + fechaStr[1] + "-" + fechaStr[0] + "' as date))";

                ejecutaReader(txtSQL, 2);

                txtSQL = "SELECT id_InformePresupuestal FROM InformePresupuestal ORDER BY id_InformePresupuestal DESC";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0) { HttpContext.Current.Session["CodInforme"] = CodEmpresa; }
            }
        }

        private void comleeeGrid()
        {
            var infoTanlaAux = new DataTable();

            if (Convert.ToInt32(Periodo) != 0)
            {
                txtSQL = "SELECT * FROM TipoAmbito";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    foreach (DataRow fil in infoTanlaAux.Rows)
                    {
                        TableHeaderRow filatablava = new TableHeaderRow();
                        filatablava.Cells.Add(crearceladtitulo(fil["NomTipoAmbito"].ToString(), 9, 1, ""));
                        t_variable.Rows.Add(filatablava);

                        txtSQL = "SELECT * FROM Ambito WHERE CodTipoAmbito=" + fil["Id_TipoAmbito"].ToString();
                        var laAux = consultas.ObtenerDataTable(txtSQL, "text");

                        if (laAux.Rows.Count > 0)
                        {
                            foreach (DataRow compl in laAux.Rows)
                            {
                                TableRow filaNeA = new TableRow();

                                filaNeA.Cells.Add(celdaNormal(compl["id_Ambito"].ToString(), 1, 1, ""));
                                filaNeA.Cells.Add(celdaNormal(compl["NomAmbito"].ToString(), 1, 1, ""));
                                filaNeA.Cells.Add(celdaNormal("", 6, 1, ""));

                                t_variable.Rows.Add(filaNeA);

                                txtSQL = @"SELECT * FROM AmbitoDetalle WHERE (CodInforme IN (SELECT id_informepresupuestal FROM informepresupuestal" +
                                        " WHERE codempresa = " + CodEmpresa + " AND id_informepresupuestal <>" + CodInforme + " AND periodo<" + Periodo + ")) AND (CodAmbito = " + compl["id_Ambito"].ToString() + ") AND (Seguimiento = 1)";

                                var laAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                                if (laAux2.Rows.Count > 0)
                                {
                                    foreach (DataRow compl2 in laAux2.Rows)
                                    {
                                        TableRow filaNeA2 = new TableRow();

                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal("En Seguimiento", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal(compl2["Cumplimiento"].ToString(), 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal(compl2["Observacion"].ToString(), 1, 1, ""));

                                        if (Convert.ToBoolean(compl2["Cumple"].ToString()))
                                            filaNeA2.Cells.Add(celdaNormal("SI", 1, 1, ""));
                                        else
                                            filaNeA2.Cells.Add(celdaNormal("NO", 1, 1, ""));

                                        if (Convert.ToBoolean(compl2["Seguimiento"].ToString()))
                                            filaNeA2.Cells.Add(celdaNormal("SI", 1, 1, ""));
                                        else
                                            filaNeA2.Cells.Add(celdaNormal("NO", 1, 1, ""));

                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));

                                        t_variable.Rows.Add(filaNeA2);
                                    }
                                }

                                txtSQL = "SELECT * FROM AmbitoDetalle WHERE CodAmbito=" + compl["id_Ambito"].ToString() + " AND CodInforme=" + CodInforme;

                                laAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                                if (laAux2.Rows.Count > 0)
                                {
                                    foreach (DataRow compl2 in laAux2.Rows)
                                    {
                                        TableRow filaNeA2 = new TableRow();

                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal(compl2["Cumplimiento"].ToString(), 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal(compl2["Observacion"].ToString(), 1, 1, ""));

                                        if (Convert.ToBoolean(compl2["Cumple"].ToString()))
                                            filaNeA2.Cells.Add(celdaNormal("SI", 1, 1, ""));
                                        else
                                            filaNeA2.Cells.Add(celdaNormal("NO", 1, 1, ""));

                                        if (Convert.ToBoolean(compl2["Seguimiento"].ToString()))
                                            filaNeA2.Cells.Add(celdaNormal("SI", 1, 1, ""));
                                        else
                                            filaNeA2.Cells.Add(celdaNormal("NO", 1, 1, ""));

                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));

                                        t_variable.Rows.Add(filaNeA2);
                                    }
                                }
                            }
                        }
                    }
                }

                t_variable.DataBind();
            }
            else
            {
                p_iB.Visible = false;
                p_iB.Enabled = false;
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

        #endregion

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/09/2014.
        /// Mostrar la información del informe seleccionado en "InformeEjecucionProyecto.aspx".
        /// </summary>
        private void CargarDatosInforme()
        {
            //Inicializar variables
            DataTable RS = new DataTable();

            try
            {
                //Cargar la información de la empresa.
                txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa=" + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    txtNomEmpresa = RS.Rows[0]["RazonSocial"].ToString();
                    CodProyecto = RS.Rows[0]["CodProyecto"].ToString();
                }

                //Determina el estado del informe.
                txtSQL = "SELECT Estado FROM InformePresupuestal WHERE id_InformePresupuestal = " + CodInforme;
                RSEstado = consultas.ObtenerDataTable(txtSQL, "text");

                if (RSEstado.Rows.Count > 0)
                { Estado = RSEstado.Rows[0]["Estado"].ToString(); }

                //Nombre del interventor.
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformePresupuestal WHERE (id_InformePresupuestal =" + CodInforme + ")))";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreInterventor.Text = "Interventor " + RS.Rows[0]["Nombre"].ToString(); }

                //Coordinador Interventoría.
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_InformePresupuestal =" + CodInforme + "))))";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreCoordinador.Text = RS.Rows[0]["Nombre"].ToString(); }

                #region Periodo "y fecha".

                if (CodInforme != "0")
                {
                    txtSQL = "SELECT * FROM InformePresupuestal WHERE id_InformePresupuestal = " + CodInforme;
                    RSPeriodo = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RSPeriodo.Rows.Count > 0)
                    {
                        switch (RSPeriodo.Rows[0]["Periodo"].ToString())
                        {
                            case "1":
                                lblPeriodo.Text = "Enero-Febrero";
                                break;
                            case "2":
                                lblPeriodo.Text = "Marzo-Abril";
                                break;
                            case "3":
                                lblPeriodo.Text = "Mayo-Junio";
                                break;
                            case "4":
                                lblPeriodo.Text = "Julio-Agosto";
                                break;
                            case "5":
                                lblPeriodo.Text = "Septiembre-Octubre";
                                break;
                            case "6":
                                lblPeriodo.Text = "Noviembre-Diciembre";
                                break;
                            default:
                                break;
                        }

                        //Fecha: 
                        if (DateTime.TryParse(RSPeriodo.Rows[0]["Periodo"].ToString(), out fecha))
                            lblFecha.Text = DateTime.Parse(RSPeriodo.Rows[0]["Periodo"].ToString()).ToString("dd/MM/yyyy");
                        else
                            lblFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");
                    }
                }

                #endregion

                //Número del contrato.
                txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NumeroContrato.Text = RS.Rows[0]["NumeroContrato"].ToString(); }

                //Varios datos de la empresa.
                txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    lblEmpresa.Text = RS.Rows[0]["RazonSocial"].ToString();
                    lblTelefono.Text = RS.Rows[0]["Telefono"].ToString();
                    lblDireccion.Text = RS.Rows[0]["DomicilioEmpresa"].ToString();
                    lblCiudad.Text = RS.Rows[0]["NomCiudad"].ToString();
                }

                #region Socios.

                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + CodEmpresa + "))"; //CodEmpresa
                var infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow fila in infoTanlaAux.Rows)
                {
                    TableRow filat = new TableRow();
                    TableCell celdat = new TableCell();
                    celdat.Text = "" + fila["Nombre"].ToString() + " Identificación: " + fila["Identificacion"].ToString();
                    filat.Cells.Add(celdat);
                    t_table.Rows.Add(filat);
                    t_table.DataBind();
                }

                infoTanlaAux = null;

                #endregion
            }
            catch { }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/09/2014.
        /// Imprimir informe de ejecución "llamado a pantalla que contiene los datos".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            //Nueva ventana emergente.
            HttpContext.Current.Session["CodInforme"] = CodInforme; //idInformePresupuestal
            HttpContext.Current.Session["CodEmpresa"] = CodEmpresa;
            HttpContext.Current.Session["Periodo"] = dd_Periodo.SelectedValue;
            Redirect(null, "ImprimirInformeInterventoriaPresup.aspx", "_blank", "width=640,height=480,scrollbars=yes,resizable=no");
        }
    }
}