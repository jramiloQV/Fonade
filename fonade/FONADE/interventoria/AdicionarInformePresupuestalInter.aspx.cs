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

namespace Fonade.FONADE.interventoria
{
    public partial class AdicionarInformePresupuestalInter : Negocio.Base_Page
    {
        string idInformePresupuestal;
        string idEmpresa;
        string periodo;

        string txtSQL;

        string txtNomEmpresa;
        string CodProyecto;
        string NomPeriodo;
        string fecha;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                recogerDatos();
                llenarTabla();
                guardaBimestrl();
                comleeeGrid();
            }
        }

        private void recogerDatos()
        {
            idInformePresupuestal = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            periodo = HttpContext.Current.Session["Periodo"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Periodo"].ToString()) ? HttpContext.Current.Session["Periodo"].ToString() : "0";
            idEmpresa = HttpContext.Current.Session["CodEmpresa"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodEmpresa"].ToString()) ? HttpContext.Current.Session["CodEmpresa"].ToString() : "0";

            if (!string.IsNullOrEmpty(idEmpresa))
            {
                txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa=" + idEmpresa;

                var infoEmoresa = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoEmoresa.Rows.Count > 0)
                {
                    txtNomEmpresa = infoEmoresa.Rows[0]["RazonSocial"].ToString();
                    CodProyecto = infoEmoresa.Rows[0]["CodProyecto"].ToString();
                }
            }
        }

        private void llenarTabla()
        {
            var infoTanlaAux = new DataTable();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformePresupuestal WHERE (id_informepresupuestal =" + idInformePresupuestal + ")))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();

                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_informepresupuestal =" + idInformePresupuestal + "))))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblCoordinador.Text = infoTanlaAux.Rows[0][0].ToString();
            }

            if (!string.IsNullOrEmpty(idInformePresupuestal))
            {
                txtSQL = "SELECT * FROM InformePresupuestal WHERE id_informepresupuestal = " + idInformePresupuestal;
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    switch (Convert.ToInt32(infoTanlaAux.Rows[0]["Periodo"].ToString()))
                    {
                        case 1: lblPeriodo.Text = "Enero-Febrero"; break;
                        case 2: lblPeriodo.Text = "Marzo-Abril"; break;
                        case 3: lblPeriodo.Text = "Mayo-Junio"; break;
                        case 4: lblPeriodo.Text = "Julio-Agosto"; break;
                        case 5: lblPeriodo.Text = "Septiembre-Octubre"; break;
                        case 6: lblPeriodo.Text = "Noviembre-Diciembre"; break;
                    }

                    lblFecha.Text = Convert.ToDateTime(infoTanlaAux.Rows[0]["Fecha"].ToString()).ToString("dd/MM/yyyy");
                }
            }

            txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");
            if (infoTanlaAux.Rows.Count > 0)
                lblContrato.Text = infoTanlaAux.Rows[0][0].ToString();

            txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                lblEmpresa.Text = infoTanlaAux.Rows[0]["RazonSocial"].ToString();
                lblTelefono.Text = infoTanlaAux.Rows[0]["Telefono"].ToString();
                lblDireccion.Text = infoTanlaAux.Rows[0]["DomicilioEmpresa"].ToString();
                lblCiudad.Text = infoTanlaAux.Rows[0]["NomCiudad"].ToString();
            }

            txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + idEmpresa + "))";
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow fila in infoTanlaAux.Rows)
            {
                TableRow filat = new TableRow();
                TableCell celdat = new TableCell();
                celdat.Text = "" + string.Format("<b>{0}</b>",fila["Nombre"].ToString()) + " Identificación: " + string.Format("<b>{0}</b>",fila["Identificacion"].ToString());

                filat.Cells.Add(celdat);
                t_table.Rows.Add(filat);
                t_table.DataBind();
            }
        }

        private void guardaBimestrl()
        {
            var infoTanlaAux = new DataTable();

            NomPeriodo = lblPeriodo.Text;
            fecha = lblFecha.Text;

            txtSQL = "SELECT Periodo,codempresa FROM InformePresupuestal WHERE Periodo=" + periodo + " AND codempresa=" + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                txtSQL = @"Insert into InformePresupuestal (NomInformePresupuestal,codinterventor,codempresa,Estado,Periodo,Fecha) " +
                    "values ('" + lblEmpresa.Text + " " + NomPeriodo + "'," + usuario.IdContacto + "," + idEmpresa + ",0," + periodo + ",'" + fecha + "')";

                ejecutaReader(txtSQL, 2);

                txtSQL = "SELECT id_InformePresupuestal FROM InformePresupuestal ORDER BY id_InformePresupuestal DESC";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0) { HttpContext.Current.Session["CodInforme"] = idEmpresa; }
            }
        }

        private void comleeeGrid()
        {
            var infoTanlaAux = new DataTable();

            if (Convert.ToInt32(periodo) != 0)
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
                                        " WHERE codempresa = " + idEmpresa + " AND id_informepresupuestal <>" + idInformePresupuestal + " AND periodo<" + periodo + ")) AND (CodAmbito = " + compl["id_Ambito"].ToString() + ") AND (Seguimiento = 1)";

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

                                txtSQL = "SELECT * FROM AmbitoDetalle WHERE CodAmbito=" + compl["id_Ambito"].ToString() + " AND CodInforme=" + idInformePresupuestal;

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

        private SqlDataReader ejecutaReader(String sql, int obj)
        {
            SqlDataReader reader = null;

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                if (reader != null)
                {
                    if (!reader.IsClosed)
                        reader.Close();
                }

                if (conn != null)
                    conn.Close();

                conn.Open();

                if (obj == 1)
                    reader = cmd.ExecuteReader();
                else
                    cmd.ExecuteReader();
            }
            catch (SqlException se)
            {
                if (conn != null)
                    conn.Close();
                return null;
            }
            finally {
                conn.Close();
                conn.Dispose();
            }

            return reader;
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
    }
}