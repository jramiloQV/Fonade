#region
#endregion

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
    public partial class AdicionarInformeBimensualInter : Negocio.Base_Page
    {
        string idInformeBimensual;
        string idEmpresa;
        string periodo;
        //
        string txtSQL;

        string txtNomEmpresa;
        string CodProyecto;
        string NomPeriodo;
        string fecha;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Cargar el nombre del interventor en sesión.
            //L_TituloNombre.Text = "Interventor " + usuario.Nombres + " " + usuario.Apellidos;

            //Cargar el nombre del coordinador interventor, "usuario creador del usuario en sesión".
            lblCoordinador.Text = CargarCoordinadorDelInterventor();
            //lblFecha.Visible = false;
            //lblPeriodo.Visible = false;

            if (!IsPostBack)
            {
                recogerDatos();
                llenarTabla();

                guardaBimestrl();

                comleeeGrid();
            }
        }

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


                txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre  " +
                         " FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor " +
                         " WHERE (CodContacto = " + usuario.IdContacto + ")))";

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

        private void recogerDatos()
        {
            idInformeBimensual = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
            periodo = HttpContext.Current.Session["PeriodoBimensual"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["PeriodoBimensual"].ToString()) ? HttpContext.Current.Session["PeriodoBimensual"].ToString() : "0";
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
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual =" + idInformeBimensual + ")))";
            else
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto=" + usuario.IdContacto;

            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");
            if (infoTanlaAux.Rows.Count > 0)
                L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0]["Nombre"].ToString();


            //if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            //{
            //    txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual =" + idInformeBimensual + ")";

            //    infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            //    if (infoTanlaAux.Rows.Count > 0)
            //        //    L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();

            //        txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto IN (Select CodCoordinador FROM Interventor WHERE CodContacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual =" + idInformeBimensual + "))";

            //    infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            //    if (infoTanlaAux.Rows.Count > 0)
            //        //Cargar el nombre del interventor en sesión.
            //        L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();
            //        //lblCoordinador.Text = infoTanlaAux.Rows[0][0].ToString();
            //}

            if (!string.IsNullOrEmpty(idInformeBimensual))
            {
                txtSQL = "SELECT * FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    switch (Convert.ToInt32(infoTanlaAux.Rows[0]["Periodo"].ToString()))
                    {
                        case 1: lblPeriodo.Text = "Enero-Febrero 01"; break;
                        case 2: lblPeriodo.Text = "Marzo-Abril 01"; break;
                        case 3: lblPeriodo.Text = "Mayo-Junio 01"; break;
                        case 4: lblPeriodo.Text = "Julio-Agosto 01"; break;
                        case 5: lblPeriodo.Text = "Septiembre-Octubre 01"; break;
                        case 6: lblPeriodo.Text = "Noviembre-Diciembre 01"; break;
                        case 7: lblPeriodo.Text = "Enero-Febrero 02"; break;
                        case 8: lblPeriodo.Text = "Marzo-Abril 02"; break;
                        case 9: lblPeriodo.Text = "Mayo-Junio 02"; break;
                        case 10: lblPeriodo.Text = "Julio-Agosto 02"; break;
                        case 11: lblPeriodo.Text = "Septiembre-Octubre 02"; break;
                        case 12: lblPeriodo.Text = "Noviembre-Diciembre 02"; break;
                        case 13: lblPeriodo.Text = "Enero-Febrero 03"; break;
                        case 14: lblPeriodo.Text = "Marzo-Abril 03"; break;
                        case 15: lblPeriodo.Text = "Mayo-Junio 03"; break;
                        case 16: lblPeriodo.Text = "Julio-Agosto 03"; break;
                        case 17: lblPeriodo.Text = "Septiembre-Octubre 03"; break;
                        case 18: lblPeriodo.Text = "Noviembre-Diciembre 03"; break;
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

            txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + idEmpresa + ")";
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

        private void comleeeGrid()
        {
            var infoTanlaAux = new DataTable();

            if (Convert.ToInt32(periodo) != 0)
            {
                txtSQL = "SELECT * FROM TipoVariable";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    foreach (DataRow fil in infoTanlaAux.Rows)
                    {
                        TableHeaderRow filatablava = new TableHeaderRow();
                        filatablava.Cells.Add(crearceladtitulo(fil["NomTipoVariable"].ToString(), 9, 1, ""));
                        t_variable.Rows.Add(filatablava);

                        txtSQL = "SELECT * FROM Variable WHERE CodTipoVariable=" + fil["Id_TipoVariable"].ToString();
                        var laAux = consultas.ObtenerDataTable(txtSQL, "text");

                        if (laAux.Rows.Count > 0)
                        {
                            foreach (DataRow compl in laAux.Rows)
                            {
                                TableRow filaNeA = new TableRow();

                                filaNeA.Cells.Add(celdaNormal(compl["id_Variable"].ToString(), 1, 1, ""));
                                filaNeA.Cells.Add(celdaNormal(compl["NomVariable"].ToString(), 1, 1, ""));
                                filaNeA.Cells.Add(celdaNormal("", 7, 1, ""));

                                t_variable.Rows.Add(filaNeA);

                                txtSQL = @"SELECT * FROM VariableDetalle WHERE CodInforme IN (SELECT id_InformeBimensual FROM InformeBimensual" +
                                        " WHERE codempresa = " + idEmpresa + " AND id_InformeBimensual <>" + idInformeBimensual + " AND periodo<" + periodo + ") AND CodVariable = " + compl["id_Variable"].ToString() + " AND Seguimiento = 1";

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

                                        filaNeA2.Cells.Add(celdaNormal(compl2["IndicadorAsociado"].ToString(), 1, 1, ""));

                                        if (Convert.ToBoolean(compl2["Seguimiento"].ToString()))
                                            filaNeA2.Cells.Add(celdaNormal("SI", 1, 1, ""));
                                        else
                                            filaNeA2.Cells.Add(celdaNormal("NO", 1, 1, ""));

                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                                        filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));

                                        t_variable.Rows.Add(filaNeA2);
                                    }
                                }

                                txtSQL = "SELECT * FROM VariableDetalle WHERE CodVariable=" + compl["id_Variable"].ToString() + " AND CodInforme=" + idInformeBimensual;

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

                                        filaNeA2.Cells.Add(celdaNormal(compl2["IndicadorAsociado"].ToString(), 1, 1, ""));

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

        private void guardaBimestrl()
        {
            var infoTanlaAux = new DataTable();

            NomPeriodo = lblPeriodo.Text;
            fecha = lblFecha.Text;

            txtSQL = "SELECT Periodo,codempresa FROM InformeBimensual WHERE Periodo=" + periodo + " AND codempresa=" + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                txtSQL = @"Insert into InformeBimensual (NomInformeBimensual,codinterventor,codempresa,Estado,Periodo,Fecha) " +
                    "values ('" + lblEmpresa.Text + " " + NomPeriodo + "'," + usuario.IdContacto + "," + idEmpresa + ",0," + periodo + ",'" + fecha + "')";

                ejecutaReader(txtSQL, 2);

                txtSQL = "SELECT id_InformeBimensual FROM InformeBimensual ORDER BY id_InformeBimensual DESC";
                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                {
                    HttpContext.Current.Session["CodInforme"] = idEmpresa;
                }
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

        /// <summary>
        /// Mauricio Arias Olave.
        /// 16/05/2014.
        /// Grabar informe bimensual.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_grabar_Click(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == Constantes.CONST_Interventor)
            { /*No puede ni debe guardar informes bimensuales.*/ }
            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                //Llamado a método que contiene código para guardar el informe bimensual.
                guardaBimestrl();
            }
        }
    }
}