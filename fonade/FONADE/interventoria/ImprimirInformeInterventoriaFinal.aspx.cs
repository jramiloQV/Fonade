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
using System.Web;

namespace Fonade.FONADE.interventoria
{
    public partial class ImprimirInformeInterventoriaFinal : Negocio.Base_Page
    {
        String CodEmpresa;
        String CodInforme;
        String txtSQL;

        /// <summary>
        /// Page_Load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodEmpresa = HttpContext.Current.Session["CodEmpresa"] != null ? CodEmpresa = HttpContext.Current.Session["CodEmpresa"].ToString() : "0";
            CodInforme = HttpContext.Current.Session["CodInforme"] != null ? CodInforme = HttpContext.Current.Session["CodInforme"].ToString() : "0";

            if (CodEmpresa != "0" && CodInforme != "0")
            { GenerarInforme_Impresion(); }
            else
            {
                //Cerrar ventana.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.close();", true);
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/07/2014.
        /// Generar HTML imprimible del informe final.
        /// </summary>
        private void GenerarInforme_Impresion()
        {
            //Inicializar variables.
            lbl_informe.Text = "";
            String NombreInterventor = "";
            String NombreCoordinador = "";
            String NumeroContrato = "";
            String Empresa = "";
            String Telefono = "";
            String Direccion = "";
            String Ciudad = "";
            DataTable RSAux = new DataTable();
            DataTable RSAux2 = new DataTable();
            DataTable RSAux3 = new DataTable();
            DataTable RSPeriodo = new DataTable();
            String txtNomEmpresa = "";
            String CodProyecto = "";
            DataTable RSEstado = new DataTable();
            //Boolean Estado;
            DateTime Fecha = new DateTime();
            DataTable Rs = new DataTable();
            DataTable RS = new DataTable();
            String txtObservacion = "";

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
                { txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual = " + CodInforme + "))) "; }
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
                                                      "          <tr>" +
                                                      "           <td width='50%' align='center' valign='baseline' bgcolor='#000000'><b>INFORME FINAL DE INTERVENTORIA</b></td>" +
                                                      "          <td width='30%' align='right' >&nbsp;</td>" +
                                                      "          <td width='20%' align='right' >&nbsp;</td>" +
                                                      "       </tr>" +
                                                      "       <tr bgcolor='#000000'><td colspan='3'>&nbsp;</td></tr>" +
                                                      "       <tr bgcolor='#CCCCCC'><td colspan='3'>&nbsp;</td></tr>" +
                                                      "      </table>" +
                                                      "          <table width='98%' align=center border='1' cellpadding='0' cellspacing='0' bordercolor='#CCCCCC'>" +
                                                      "              <tr>" +
                                                      "                  <td valign='top' width='98%'><br/>" +
                                                      "                      <TABLE width='98%' align=center border='0' cellpadding='0' cellspacing='0'>" +
                                                      "                          <TR>" +
                                                      "                              <TD align='center' COLSPAN='2'>" +
                                                      "                                  <B>Informe Interventoría Final - " + txtNomEmpresa + "</B>" +
                                                      "                              </TD>" +
                                                      "                          </TR>" +
                                                      "                          <TR>" +
                                                      "                              <TD align='center' COLSPAN='2'>" +
                                                      "                                  <B>Interventor: </B>" + usuario.Nombres + " " + usuario.Apellidos +
                                                      "                              </TD>" +
                                                      "                          </TR>";

                #endregion

                #region Coordinador Interventoría.

                if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
                { txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual = " + CodInforme + "))))"; }
                else { txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre FROM Contacto WHERE (Id_Contacto IN (SELECT codcoordinador FROM Interventor WHERE (CodContacto = " + usuario.IdContacto + ")))"; }

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreCoordinador = RS.Rows[0]["Nombre"].ToString(); }

                lbl_informe.Text = lbl_informe.Text + "<TR>" +
                                                      "     <TD align='center' COLSPAN='2'>" +
                                                      "         <b>Coordinador: </b>" + NombreCoordinador +
                                                      "     </TD>" +
                                                      " </TR>";

                #endregion

                /*************************/

                #region Número del contrato, fecha empresa y socios.

                lbl_informe.Text = lbl_informe.Text + "<TR>" +
                                                      "    <TD align='center' bgcolor='#CCCCCC'>" +
                                                      "        <b>Número Contrato: </b>" + NumeroContrato +
                                                      "    </TD>" +
                                                      "    <TD align='center' bgcolor='#CCCCCC'>" +
                                                      "        <b>Fecha Informe: </b>" + Fecha.ToString("dd/MM/yyyy") +
                                                      "    </TD>" +
                                                      "</TR>" +
                                                      "<TR>" +
                                                      "    <TD ALIGN='center'>" +
                                                      "        <b>Empresa: </b>" + Empresa +
                                                      "    </TD>" +
                                                      "    <TD ALIGN='center'><b>Socios:</b>";

                txtSQL = " SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + CodEmpresa + ")) ";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                { lbl_informe.Text = lbl_informe.Text + "<b>" + row["Nombre"].ToString() + "</b> Identificación: <b>" + row["Identificacion"].ToString() + "</b><br/>"; }

                #endregion

                #region Teléfono, Dirección e inicio de la grilla.

                lbl_informe.Text = lbl_informe.Text + "</TD>" +
                                                      " </TR>" +
                                                      " <TR bgcolor='#CCCCCC'>" +
                                                      "     <TD align='center'>" +
                                                      "         <b>Teléfono: </b>" + Telefono +
                                                      "     </TD>" +
                                                      "     <TD align='center'>" +
                                                      "         <b>Dirección: </b>" + Direccion + " - " + Ciudad +
                                                      "     </TD>" +
                                                      " </TR>" +
                                                      " <TR>" +
                                                      "     <TD align='center' colspan=2>" +
                                                      "         <TABLE width='98%' align=center border='0' cellpadding='0' cellspacing='0'>" +
                                                      "   <tr> " +
                                                      "     <td colspan=8 ><p>&nbsp;</p></td>" +
                                                      "   </tr>									" +
                                                      "   <tr> " +
                                                      "     <td colspan=8 ><p>&nbsp;</p></td>" +
                                                      "   </tr>			" +
                                                      "             <tr>" +
                                                      "                 <td bgcolor='#B0b0b0' align=center><B>CRITERIO</B></td>" +
                                                      "                 <td bgcolor='#B0b0b0' align=center><B>CUMPLIMIENTO A VERIFICAR</B></td>" +
                                                      "                 <td bgcolor='#B0b0b0' align=center><B>OBSERVACION INTERVENTOR</B></td>" +
                                                      "             </tr>";

                #endregion

                #region Grilla de informe final.

                txtSQL = "SELECT Id_InterventorInformeFinalCriterio, NomInterventorInformeFinalCriterio FROM InterventorInformeFinalCriterio WHERE CodEmpresa IN (0," + CodEmpresa + ") ORDER BY CodEmpresa, Id_InterventorInformeFinalCriterio";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                #region RS y RSInforme.

                foreach (DataRow row in RS.Rows)
                {
                    lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                          "  <td colspan=48 ><p>&nbsp;</p></td>" +
                                                          "</tr>" +
                                                          "<tr> " +
                                                          "  <td colspan=48 ><p>&nbsp;</p></td>" +
                                                          "</tr>" +
                                                          "<tr> " +
                                                          "  <td colspan=8 bgcolor='#EDEFF3' >" + row["NomInterventorInformeFinalCriterio"].ToString() + "</td>" +
                                                          "</tr>";

                    txtSQL = "SELECT Id_InterventorInformeFinalItem, NomInterventorInformeFinalItem FROM InterventorInformeFinalItem WHERE CodEmpresa IN (0," + CodEmpresa + ") AND CodInformeFinalCriterio = " + row["Id_InterventorInformeFinalCriterio"].ToString() + " ORDER BY CodEmpresa, Id_InterventorInformeFinalItem";
                    var RSInforme = consultas.ObtenerDataTable(txtSQL, "text");

                    foreach (DataRow row_RSInforme in RSInforme.Rows)
                    {
                        txtSQL = "SELECT Observaciones FROM InterventorInformeFinalCumplimiento WHERE CodInformeFinal = " + CodInforme + " AND CodInformeFinalItem = " + row_RSInforme["Id_InterventorInformeFinalItem"].ToString() + " AND CodEmpresa = " + CodEmpresa;
                        var RSObser = consultas.ObtenerDataTable(txtSQL, "text");

                        txtObservacion = "";
                        if (RSObser.Rows.Count > 0) { txtObservacion = RSObser.Rows[0]["Observaciones"].ToString(); }
                        RSObser = null;

                        lbl_informe.Text = lbl_informe.Text + "<tr>" +
                                                              "     <td>&nbsp;</td>" +
                                                              "     <td >" + row_RSInforme["NomInterventorInformeFinalItem"].ToString() + "</td>" +
                                                              "     <td>" + txtObservacion + "     </td>" +
                                                              " </tr>";
                    }

                    RSInforme = null;
                }

                #endregion

                #region Enlace de anexos.

                lbl_informe.Text = lbl_informe.Text + "<tr> " +
                                                              "    <td colspan=8 ><p>&nbsp;</p></td>" +
                                                              "  </tr>										" +
                                                              "            <tr>" +
                                                              "                <td COLSPAN=3 bgcolor='#EDEFF3' ><BR>ANEXOS</td>" +
                                                              "            </tr>" +
                                                              "  <tr> " +
                                                              "    <td colspan=8 ><p>&nbsp;</p></td>" +
                                                              "  </tr>									" +
                                                              "  <tr> " +
                                                              "    <td colspan=8 ><p>&nbsp;</p></td>" +
                                                              "  </tr>";

                txtSQL = " SELECT InterventorInformeFinalAnexos.Id_InterventorInformeFinalAnexos, " +
                         " InterventorInformeFinalAnexos.NomInterventorInformeFinalAnexos, InterventorInformeFinalAnexos.RutaArchivo, " +
                         " DocumentoFormato.Icono FROM InterventorInformeFinalAnexos INNER JOIN DocumentoFormato " +
                         " ON InterventorInformeFinalAnexos.CodDocumentoFormato = DocumentoFormato.Id_DocumentoFormato " +
                         " WHERE (InterventorInformeFinalAnexos.Borrado = 0) AND (InterventorInformeFinalAnexos.CodInformeFinal = " + CodInforme + ")";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                foreach (DataRow row in RS.Rows)
                {
                    lbl_informe.Text = lbl_informe.Text + "<tr>" +
                                                          "     <td>&nbsp;</td>" +
                                                          "     <td COLSPAN=8 valign='center'>" +
                                                          "         <img src='../../Images/" + row["Icono"].ToString() + "'> - <a href='" + row["RutaArchivo"].ToString() + "'  target='_blank'>" + row["NomInterventorInformeFinalAnexos"].ToString() + "</a>" +
                                                          "     </td>" +
                                                          " </tr>";
                }

                #endregion

                #endregion

                #region Cierra los elementos principales generados hasta el momento.

                lbl_informe.Text = lbl_informe.Text + "</table>" +
                                    "					</td>" +
                                    "				</tr>" +
                                    "			</table>" +
                                    "		</td>" +
                                    "	</tr>" +
                                    "</table>";
                #endregion

                #region Firmas.

                lbl_informe.Text = lbl_informe.Text + "            <table width='100%' border='0' cellspacing='2' cellpadding='0'>";
                lbl_informe.Text = lbl_informe.Text + "			  		<tr bgcolor='#C0C0C0'><td colspan='5'>&nbsp;</td></tr>";
                lbl_informe.Text = lbl_informe.Text + "			      <tr><td colspan='5'>&nbsp;</td></tr>";
                try { lbl_informe.Text = lbl_informe.Text + "              <tr ><td colspan=2>Dadas las condiciones en que el Contratista se viene cumpliendo o incumpliendo, con las obligaciones del contrato, el INTERVENTOR recomienda FONADE: " + "" + "<br/><br/><br/><br/></td></tr>"; }
                catch { lbl_informe.Text = lbl_informe.Text + "              <tr ><td colspan=2>Dadas las condiciones en que el Contratista se viene cumpliendo o incumpliendo, con las obligaciones del contrato, el INTERVENTOR recomienda FONADE: " + "" + "<br/><br/><br/><br/></td></tr>"; }
                lbl_informe.Text = lbl_informe.Text + "              <tr ><td colspan=2>Para constancia firman:<br/><br/><br/><br/></td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr ><td><br/><br/><br/><br/></td></tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr >";
                lbl_informe.Text = lbl_informe.Text + "                <td width='50%'>_____________________________________</td>";
                lbl_informe.Text = lbl_informe.Text + "                <td>_____________________________________</td>";
                lbl_informe.Text = lbl_informe.Text + "              </tr>";
                lbl_informe.Text = lbl_informe.Text + "              <tr >";
                lbl_informe.Text = lbl_informe.Text + "                <td valign=top>Interventor</td>";
                lbl_informe.Text = lbl_informe.Text + "                <td valign=top>Contratista</td>";
                lbl_informe.Text = lbl_informe.Text + "              </tr>";
                lbl_informe.Text = lbl_informe.Text + "            </table>";

                #endregion

                //Final.
                lbl_informe.Text = lbl_informe.Text + "</td>" +
                                                "	</tr>" +
                                                "</table>";

            }
            catch (Exception ex)
            { lbl_informe.Text = lbl_informe.Text + "<strong style='color:#CC0000;'>Error: " + ex.Message + "</strong>"; }
        }
    }
}