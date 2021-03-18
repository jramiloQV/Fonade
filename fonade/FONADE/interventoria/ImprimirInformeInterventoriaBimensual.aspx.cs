using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using System.Globalization;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Web;

namespace Fonade.FONADE.interventoria
{
    public partial class ImprimirInformeInterventoriaBimensual : Negocio.Base_Page
    {
        String CodEmpresa;
        String idInformeBimensual;
        String txtSQL;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        

        protected void Page_Load(object sender, EventArgs e)
        {
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null ? CodEmpresa = HttpContext.Current.Session["CodEmpresa"].ToString() : "0";
            idInformeBimensual = HttpContext.Current.Session["INF_idInformeBimensual"] != null ? idInformeBimensual = HttpContext.Current.Session["INF_idInformeBimensual"].ToString() : "0";

            if (!IsPostBack)
            {
                //Obtener el valor enviado desde la página "AdicionarInformeBimensual.aspx".
                if (idInformeBimensual == "0")
                { idInformeBimensual = HttpContext.Current.Session["idInformeBimensual"] != null ? idInformeBimensual = HttpContext.Current.Session["idInformeBimensual"].ToString() : "0"; }

                if (CodEmpresa != "0" && idInformeBimensual != "0")
                { GenerarInforme_Impresion(); }
                else
                {
                    //Cerrar ventana.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
                }
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/07/2014.
        /// Generar HTML imprimible del informe bimensual.
        /// </summary>
        private void GenerarInforme_Impresion()
        {

            //var watch = Stopwatch.StartNew();
            // the code that you want to measure comes here
            
            //Inicializar variables.
            lbl_informe.Text = "";
            String NombreInterventor = "";
            String NombreCoordinador = "";
            String NumeroContrato = "";
            String Empresa = "";
            String Telefono = "";
            String Direccion = "";
            String Ciudad = "";
            String NomTipoVariable = "";
            DataTable RSAux = new DataTable();
            String CodTipoVariable = "";
            DataTable RSAux2 = new DataTable();
            DataTable RSAux3 = new DataTable();
            DataTable RSPeriodo = new DataTable();
            String Periodo = "";
            String txtNomEmpresa = "";
            String CodProyecto = "";
            Boolean Chequeo;
            DataTable RSEstado = new DataTable();
            Boolean Estado;
            Int32 numAncho = 0;
            DateTime Fecha = new DateTime();
            DataTable Rs = new DataTable();
            DataTable RS = new DataTable();

            try
            {
                #region Obtener información de la empresa.

                if (CodEmpresa != "")
                {
                    txtSQL = "select RazonSocial, CodProyecto from Empresa where id_empresa = " + CodEmpresa;
                    Rs = consultas.ObtenerDataTable(txtSQL, "text");
                    if (Rs.Rows.Count > 0)
                    {
                        txtNomEmpresa = Rs.Rows[0]["RazonSocial"].ToString();
                        CodProyecto = Rs.Rows[0]["CodProyecto"].ToString();
                    }
                }

                #endregion

                #region Para traer el nombre del interventor.

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual = " + idInformeBimensual + "))) "; }
                else
                { txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE id_contacto = " + usuario.IdContacto; }

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreInterventor = RS.Rows[0]["Nombre"].ToString(); }

                #endregion

                #region Inicio del informe.

                lbl_informe.Text = lbl_informe.Text + "<a name='FullHead'></a>";
                lbl_informe.Text = lbl_informe.Text + "<table width='640'  border='0' cellspacing='0' cellpadding='0'>" +
                                                              "<tr>" +
                                                              "  <td width='100%' align='left' valign='top'>" +
                                                              "    <table width='100%' border='0' cellspacing='0' cellpadding='2'>" +
                                                              "        	<tr>" +
                                                              "             <td width='50%' align='center' valign='baseline' bgcolor='#000000' class='Blanca'><strong style='color:White'>INFORME BIMENSUAL DE INTERVENTORIA</strong></td>" +
                                                              "          <td width='30%' align='right' class='titulo'>&nbsp;</td>" +
                                                              "          <td width='20%' align='right' class='titulo'>&nbsp;</td>" +
                                                              "       </tr>" +
                                                              "       <tr bgcolor='#000000'><td colspan='3'>&nbsp;</td></tr>" +
                                                              "       <tr bgcolor='#CCCCCC'><td colspan='3'>&nbsp;</td></tr>" +
                                                              "  	</table>" +
                                                              "    <table width='100%' border='0'>" +
                                                              "<tr> " +
                                                              "  <td colspan='7'><div align='center' class='Titulo'>FORMATO 01</div></td>" +
                                                              "</tr>" +
                                                              "<tr> " +
                                                              "  <td colspan='7'><div align='center' class='Titulo'>INFORME BIMENSUAL DE SEGUIMIENTO DE LA INTERVENTORIA</div></td>" +
                                                              "</tr>" +
                                                              "<tr> " +
                                                              "  <td colspan='7'><div align='center' class='tituloDestacados'><span class='Titulo'>Interventor " +
                                                              "          </span>" + NombreInterventor + "</div></td>" +
                                                              "</tr>";

                #endregion

                #region Coordinador Interventoría.

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual + "))))"; }
                else { txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + ")))"; }

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreCoordinador = RS.Rows[0]["Nombre"].ToString(); }

                lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                     "   <td colspan='2' class='Titulo'>&nbsp;</td>" +
                                                     " </tr>		" +
                                                     " <tr> " +
                                                     "   <td colspan='3' bgcolor='#CCCCCC' class='Titulo'>Coordinador</td>" +
                                                     "   <td colspan='4' bgcolor='#CCCCCC'>" + NombreCoordinador + "</td>" +
                                                     " </tr>" +
                                                     " <tr> " +
                                                     "   <td colspan='3' class='Titulo'>Periodo</td>" +
                                                     "   <td colspan='4'>";

                #endregion

                #region Periodo y Número del contrato.

                txtSQL = " SELECT * FROM InformeBimensual WHERE id_InformeBimensual = " + idInformeBimensual;
                RSPeriodo = consultas.ObtenerDataTable(txtSQL, "text");
                if (RSPeriodo.Rows.Count > 0)
                {
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "1") { Periodo = "Enero-Febrero"; lbl_informe.Text = lbl_informe.Text + "Enero-Febrero"; }
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "2") { Periodo = "Marzo-Abril"; lbl_informe.Text = lbl_informe.Text + "Marzo-Abril"; }
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "3") { Periodo = "Mayo-Junio"; lbl_informe.Text = lbl_informe.Text + "Mayo-Junio"; }
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "4") { Periodo = "Julio-Agosto"; lbl_informe.Text = lbl_informe.Text + "Julio-Agosto"; }
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "5") { Periodo = "Septiembre-Octubre"; lbl_informe.Text = lbl_informe.Text + "Septiembre-Octubre"; }
                    if (RSPeriodo.Rows[0]["Periodo"].ToString() == "6") { Periodo = "Noviembre-Diciembre"; lbl_informe.Text = lbl_informe.Text + "Noviembre-Diciembre"; }
                    try { 
                        Fecha = DateTime.Parse(RSPeriodo.Rows[0]["Fecha"].ToString()); 
                    }
                    catch 
                    { 
                        Fecha = new DateTime();    
                    }
                    //La asignacion de la fecha estaba dentro del catch

                    lbl_informe.Text = lbl_informe.Text + Fecha; 

                    Periodo = RSPeriodo.Rows[0]["Periodo"].ToString();
                }

                lbl_informe.Text = lbl_informe.Text + " </td>";
                lbl_informe.Text = lbl_informe.Text + "</tr>";

                txtSQL = " SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NumeroContrato = RS.Rows[0]["NumeroContrato"].ToString(); }

                #endregion

                #region Fecha.

                lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                              "    <td colspan='3' bgcolor='#CCCCCC' class='Titulo'>Contrato</td>" +
                                                              "    <td colspan='4' bgcolor='#CCCCCC'>" + NumeroContrato + "</td>" +
                                                              "  </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan='3' class='Titulo'>Fecha</td>" +
                                                              "		<td colspan='4'>";
                lbl_informe.Text = lbl_informe.Text + Fecha.ToString("dd/MM/yyyy");

                lbl_informe.Text = lbl_informe.Text + " </td>" +
                                                      "</tr>";

                #endregion

                #region Razón social y otros datos.

                txtSQL = " SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa = " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    Empresa = RS.Rows[0]["RazonSocial"].ToString();
                    Telefono = RS.Rows[0]["Telefono"].ToString();
                    Direccion = RS.Rows[0]["DomicilioEmpresa"].ToString();
                    Ciudad = RS.Rows[0]["NomCiudad"].ToString();
                }

                #endregion

                #region Socios.

                lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                              "    <td colspan='3' bgcolor='#CCCCCC' class='Titulo'>Empresa</td>" +
                                                              "    <td colspan='4' bgcolor='#CCCCCC'>" + Empresa + "</td>" +
                                                              "  </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan='3' class='Titulo'>Teléfono</td>" +
                                                              "    <td colspan='4'>" + Telefono + "</td>" +
                                                              "  </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan='3' bgcolor='#CCCCCC' class='Titulo'>Direcci&oacute;n</td>" +
                                                              "    <td colspan='4' bgcolor='#CCCCCC'>" + Direccion + "</td>" +
                                                              "  </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan='3' class='Titulo'>Ciudad</td>" +
                                                              "    <td colspan='4'>" + Ciudad + "</td>" +
                                                              "  </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan='3' bgcolor='#CCCCCC' class='Titulo'>Socios</td>" +
                                                              "    <td colspan='4' bgcolor='#CCCCCC'>";

                txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + CodEmpresa + ")) ";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                { lbl_informe.Text = lbl_informe.Text + "<b>" + row["Nombre"].ToString() + "</b> Identificación: <b>" + row["Identificacion"].ToString() + "</b><br/>"; }

                #endregion

                #region Líneas finales de la tabla principal "Datos previos al Informe Bimensual".

                lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                      "  </tr>" +
                                                      "  <tr> " +
                                                      "    <td colspan='2' align='center'>&nbsp;" +
                                                      "    </td>" +
                                                      "  </tr>   " +
                                                      "</TABLE>";

                #endregion

                /******************************************************************************************/

                #region Inicio de la tabla del informe bimensual.

                lbl_informe.Text = lbl_informe.Text + "<table width='100%' border='1' bordercolor='#000000'>" +
                                                     " <tr> " +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Codigo</td>" +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Ambito</td>" +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Cumplimiento a verificar</td>" +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Observacion Interventor</td>" +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Cumple</td>" +
                                                     "       <td bgcolor='#CCCCCC' class='titulo'>Indicador Asociado</td>" +
                                                     "   <td bgcolor='#CCCCCC' class='titulo'>Hacer Seguimiento</td>" +
                                                     " </tr>" +
                                                     " <tr> " +
                                                     "   <td colspan=7 class='tituloDestacados'>&nbsp;</td>" +
                                                     " </tr> ";

                #endregion

                txtSQL = " SELECT * FROM TipoVariable ";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row_RS in RS.Rows)
                {
                    CodTipoVariable = row_RS["Id_TipoVariable"].ToString();
                    NomTipoVariable = row_RS["NomTipoVariable"].ToString();

                    lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                          "  <td colspan='7'>&nbsp;" +
                                                          "  </td>" +
                                                          "</tr>	" +
                                                          "  <tr> " +
                                                          "    <td colspan='7' bgcolor='#B0B0B0' class='tituloTabla'>" + NomTipoVariable + "</td>" +
                                                          "  </tr>";

                    txtSQL = "SELECT * FROM Variable WHERE CodTipoVariable = " + CodTipoVariable;
                    RSAux = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row_RSAux in RSAux.Rows)
                    {
                        #region Ciclo inicial.

                        lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                                          "    <td align='center' bgcolor='#CCCCCC'>" + row_RSAux["id_Variable"].ToString() + "</td>" +
                                                                          "    <td bgcolor='#CCCCCC' class='Titulo'>" + row_RSAux["NomVariable"].ToString() + "</td>" +
                                                                          "<td colspan=7 valign='middle' bgcolor='#CCCCCC'> " +
                                                                          "		&nbsp;" +
                                                                          "</td>";

                        //Cumplimientos con Seguimiento.
                        txtSQL = " SELECT * FROM VariableDetalle WHERE (CodInforme IN (SELECT id_InformeBimensual FROM InformeBimensual " +
                                 " WHERE codempresa = " + CodEmpresa + " AND id_InformeBimensual <>" + idInformeBimensual + " AND periodo<" + Periodo + "))" +
                                 " AND (CodVariable = " + row_RSAux["id_Variable"].ToString() + ") AND (Seguimiento = 1) ";
                        RSAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                        foreach (DataRow row_RSAux2 in RSAux2.Rows)
                        {
                            #region Ciclo (2).

                            lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                                                  "<td align='center'>&nbsp;</td>" +
                                                                                  "<td>En Seguimiento</td>		" +
                                                                                  "<td>" + row_RSAux2["Cumplimiento"].ToString() + "</td>" +
                                                                                  "<td>" + row_RSAux2["Observacion"].ToString() + "</td>" +
                                                                                  "<td align='center'>";

                            try { 
                                if (Boolean.Parse(row_RSAux2["Cumple"].ToString())) 
                                { 
                                    lbl_informe.Text = lbl_informe.Text + "Si"; 
                                } 
                                else { 
                                    lbl_informe.Text = lbl_informe.Text + "No"; 
                                } 
                            }
                            catch { 
                                lbl_informe.Text = lbl_informe.Text + "No"; 
                            }

                            lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                                  "<td>" + row_RSAux2["IndicadorAsociado"].ToString() + "</td>  " +
                                                                  "<td align='center'>";

                            try { 
                                if (Boolean.Parse(row_RSAux2["Seguimiento"].ToString())) 
                                { 
                                    lbl_informe.Text = lbl_informe.Text + "Si"; 
                                } 
                                else { 
                                    lbl_informe.Text = lbl_informe.Text + "No"; 
                                } 
                            }
                            catch { lbl_informe.Text = lbl_informe.Text + "No"; }

                            lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                                  "</tr>";

                            #endregion
                        }

                        //Nuevos Cumplimientos.
                        txtSQL = " SELECT * FROM VariableDetalle WHERE CodVariable = " + row_RSAux["id_Variable"].ToString() + " AND CodInforme = " + idInformeBimensual;
                        RSAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                        foreach (DataRow row_RSAux2 in RSAux2.Rows)
                        {
                            #region Ciclo (3).

                            lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                                                  "<td align='center'>&nbsp;</td>" +
                                                                                  "<td>&nbsp;</td>		" +
                                                                                  "<td>" + row_RSAux2["Cumplimiento"].ToString() + "</td>" +
                                                                                  "<td>" + row_RSAux2["Observacion"].ToString() + "</td>" +
                                                                                  "<td align='center'>";

                            try { if (Boolean.Parse(row_RSAux2["Cumple"].ToString())) { lbl_informe.Text = lbl_informe.Text + "Si"; } else { lbl_informe.Text = lbl_informe.Text + "No"; } }
                            catch { lbl_informe.Text = lbl_informe.Text + "No"; }

                            lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                                  "<td>" + row_RSAux2["IndicadorAsociado"].ToString() + "</td>  " +
                                                                  "<td align='center'>";

                            try { if (Boolean.Parse(row_RSAux2["Seguimiento"].ToString())) { lbl_informe.Text = lbl_informe.Text + "Si"; } else { lbl_informe.Text = lbl_informe.Text + "No"; } }
                            catch { lbl_informe.Text = lbl_informe.Text + "No"; }

                            lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                                  "</tr>";

                            #endregion
                        }

                        lbl_informe.Text = lbl_informe.Text + "</tr>";

                        #endregion
                    }
                }

                lbl_informe.Text = lbl_informe.Text + "</table>";

                /******************************************************************************************/

                //Firmas.
                lbl_informe.Text = lbl_informe.Text + "            <table width='100%' border='0' cellspacing='2' cellpadding='0'>";
                lbl_informe.Text = lbl_informe.Text + "			  		<tr bgcolor='#C0C0C0'><td colspan='5'>&nbsp;</td></tr>";
                lbl_informe.Text = lbl_informe.Text + "			      <tr><td colspan='5'>&nbsp;</td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr class='Titulo'><td colspan=2>Dadas las condiciones en que el Contratista se viene cumpliendo o incumpliendo, con las obligaciones del contrato, el INTERVENTOR recomienda FONADE: " + "" + "<br/><br/><br/><br/></td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr class='Titulo'><td colspan=2>Para constancia firman:<br/><br/><br/><br/></td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr class='Titulo'><td><br/><br/><br/><br/></td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr class='Titulo'>";
                lbl_informe.Text = lbl_informe.Text + "                <td width='50%'>_____________________________________</td>";
                lbl_informe.Text = lbl_informe.Text + "                <td>_____________________________________</td>";
                lbl_informe.Text = lbl_informe.Text + "              </tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr class='Titulo'>";
                lbl_informe.Text = lbl_informe.Text + "                <td valign=top>Interventor</td>";
                lbl_informe.Text = lbl_informe.Text + "                <td valign=top>Contratista</td>";
                lbl_informe.Text = lbl_informe.Text + "              </tr>";
                lbl_informe.Text = lbl_informe.Text + "            </table>";

 //               watch.Stop();
 //               var elapsedMs = watch.ElapsedMilliseconds;
 //               lbl_informe.Text = lbl_informe.Text + " Tiempo: transcurrido: " + elapsedMs;
            }
            catch (Exception ex)
            { lbl_informe.Text = lbl_informe.Text + "<strong style='color:#CC0000;'>Error: " + ex.Message + "</strong>"; }
        }
    }
}