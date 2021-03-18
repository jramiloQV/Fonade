using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Text;
using Fonade.Negocio;
using Fonade.Negocio.Interventoria;
using Fonade.Negocio.Entidades;

namespace Fonade.FONADE.interventoria
{
    public partial class ImprimirPlanOperativos : Negocio.Base_Page
    {
        #region Variables globales.

        /// <summary>
        /// Código del Proyecto.
        /// </summary>
        int CodProyecto;

        /// <summary>
        /// Contiene las consultas de SQL.
        /// </summary>
        String txtSQL;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    #region Obtener el código del proyecto.

                    CodProyecto = (int)
                                (!string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString())
                                     ? Convert.ToInt64(HttpContext.Current.Session["CodProyecto"])
                                     : 0);

                    #endregion

                    CargarInformeCompleto();
                }
            }
            catch (Exception)
            {
                Response.Redirect("FramePlanOperativoInterventoria.aspx");
            }

            #region Establecer fecha.

            //Establecer fecha.
            DateTime fecha = DateTime.Now;
            string sMes = fecha.ToString("MMM", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"));
            L_Fecha.Text = OperacionesRoleNegocio.UppercaseFirst(sMes) + " " + fecha.Day + " de " + fecha.Year;

            #endregion

            #region Comentarios NO BORRAR!.
            //datosGenerales();

            //CrearTabla();

            //llennarTabla(); 
            #endregion
        }


        /// <summary>
        /// Mauricio Arias Olave.
        /// 10/07/2014.
        /// Cargar el informe completo.
        /// </summary>
        private void CargarInformeCompleto()
        {
            #region Inicializar variables.

            DataTable RS = new DataTable();
            String CodEmpresa = "";
            String Empresa = "";
            String Telefono = "";
            String Direccion = "";
            String Ciudad = "";
            String CodContacto = "";
            String nomInterventor = "";
            String apeInterventor = "";
            String NumeroContrato = "";
            String NombreCoordinador = "";
            DataTable rsActividades = new DataTable();
            String CodActividad = "";
            String NomActividad = "";
            Double TotalFE = 0;
            Double TotalEmp = 0;
            int filas = 0;
            int z = 0;
            Double TotalTipo1 = 0;
            Double TotalTipo2 = 0;
            DataTable rsTipo1 = new DataTable();
            DataTable rsTipo2 = new DataTable();
            String Tipo1 = "";
            String Tipo2 = "";
            DataTable rsActividad = new DataTable();
            #region Obtener el valor de la prórroga para sumarla a la constante de meses generar la tabla.
            int CONST_Meses = 12 + ObtenerProrroga(CodProyecto.ToString());
            #endregion
            DataTable rsCargo = new DataTable();
            int i = 0;
            Double TotalTipo1A = 0;
            Double TotalTipo2A = 0;
            DataTable rsCargoss = new DataTable();
            DataTable rsCargos = new DataTable();
            DataTable rsProducto = new DataTable();
            DataTable rsProductos = new DataTable();
            //String txtProducto = "";

            // 2014/11/19 RAlvaradoT Se crea un String BUilder para almacenar las cadenas temporales ya que esto es mas optimo
            StringBuilder sbCuerpo = new StringBuilder();
            #endregion

            try
            {
                #region Trae datos de la empresa.

                txtSQL = " SELECT Id_Empresa, RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and CodProyecto = " + CodProyecto;

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    CodEmpresa = RS.Rows[0]["Id_Empresa"].ToString();
                    Empresa = RS.Rows[0]["RazonSocial"].ToString();
                    Telefono = RS.Rows[0]["Telefono"].ToString();
                    Direccion = RS.Rows[0]["DomicilioEmpresa"].ToString();
                    Ciudad = RS.Rows[0]["NomCiudad"].ToString();
                }

                #endregion

                #region Se trae los datos del interventor.

                txtSQL = " SELECT Contacto.Id_Contacto, Contacto.Nombres, Contacto.Apellidos " +
                         " FROM EmpresaInterventor INNER JOIN Contacto " +
                         " ON EmpresaInterventor.CodContacto = Contacto.Id_Contacto " +
                         " WHERE (EmpresaInterventor.CodEmpresa = " + CodEmpresa + ") " +
                         " AND (EmpresaInterventor.Rol = " + Constantes.CONST_RolInterventorLider + ")";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    CodContacto = RS.Rows[0]["Id_Contacto"].ToString();
                    nomInterventor = RS.Rows[0]["Nombres"].ToString();
                    apeInterventor = RS.Rows[0]["Apellidos"].ToString();
                }

                #endregion

                #region Información del interventor.

                // Esta forma de armar dinamicamente textos es muy lenta, hay que utilizar un StringBUilder para mejorar el performance

                sbCuerpo.AppendLine("<table width='100%' align=center border='1' cellpadding='0' cellspacing='0' bordercolor='#CCCCCC'>");
                sbCuerpo.AppendLine(" <tr>");
                sbCuerpo.AppendLine(" 	 <td valign='top' width='100%'><br/>");
                sbCuerpo.AppendLine(" 	 	<table width='100%' align=center border='0' cellpadding='0' cellspacing='0'>");
                sbCuerpo.AppendLine(" 	 		<tr>");
                sbCuerpo.AppendLine(" 	 			<td align='center' COLSPAN='2'>");
                sbCuerpo.AppendLine(" 	 				<B></B>");
                sbCuerpo.AppendLine(" 	 			</td>");
                sbCuerpo.AppendLine(" 	 		</tr>");
                sbCuerpo.AppendLine(" 	 		<tr>");
                sbCuerpo.AppendLine(" 	 			<td align='center' COLSPAN='2'>");
                sbCuerpo.AppendLine(" 	 				<B>Interventor: </B>" + nomInterventor + " " + apeInterventor);
                sbCuerpo.AppendLine(" 	 			</td>");
                sbCuerpo.AppendLine(" 	 		</tr>");


                #endregion

                #region Se trae el Coordinador de Interventoría.

                txtSQL = " SELECT nombres, apellidos FROM contacto " +
                         " WHERE id_contacto In (SELECT CodCoordinador FROM interventor " +
                         " WHERE codcontacto = " + CodContacto + ")";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    NombreCoordinador = RS.Rows[0]["Nombres"].ToString() + " " + RS.Rows[0]["Apellidos"].ToString();
                }

                #endregion

                #region Se trae el número de contrato asociado a la empresa.

                txtSQL = "SELECT ContratoEmpresa.NumeroContrato FROM ContratoEmpresa " +
                         " INNER JOIN Empresa ON ContratoEmpresa.CodEmpresa = Empresa.id_empresa" +
                         " WHERE (Empresa.codproyecto = " + CodProyecto + ")";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NumeroContrato = RS.Rows[0]["NumeroContrato"].ToString(); }

                #endregion

                #region Dibujar Coordinador, Número del contrato y Empresa.

                sbCuerpo.AppendLine("<tr>");
                sbCuerpo.AppendLine("		<td align='center' COLSPAN='2'>");
                sbCuerpo.AppendLine("			<b>Coordinador: </b>" + NombreCoordinador);
                sbCuerpo.AppendLine("		</td>");
                sbCuerpo.AppendLine("	</tr>");
                sbCuerpo.AppendLine("	<tr>");
                sbCuerpo.AppendLine("		<td align='center' bgcolor='#CCCCCC'>");
                sbCuerpo.AppendLine("			<b>Número Contrato: </b>" + NumeroContrato);
                sbCuerpo.AppendLine("		</td>");
                sbCuerpo.AppendLine("		<td align='center' bgcolor='#CCCCCC'>");
                sbCuerpo.AppendLine("		</td>");
                sbCuerpo.AppendLine("	</tr>");
                sbCuerpo.AppendLine("	<tr>");
                sbCuerpo.AppendLine("		<td align='center'>");
                sbCuerpo.AppendLine("			<b>Empresa: </b>" + Empresa);
                sbCuerpo.AppendLine("		</td>");
                sbCuerpo.AppendLine("		<td align='center'><b>Socios:</b>");

                #endregion

                #region Trae los socios de la empresa.

                txtSQL = " SELECT Nombres + ' ' + Apellidos AS Nombre, Identificacion FROM Contacto " +
                         " WHERE (Id_Contacto IN " +
                         " (SELECT EmpresaContacto.codcontacto " +
                         " FROM EmpresaContacto INNER JOIN " +
                         " Empresa ON EmpresaContacto.codempresa = Empresa.id_empresa " +
                         " WHERE (Empresa.codproyecto = " + CodProyecto + "))) ";

                RS = consultas.ObtenerDataTable(txtSQL, "text");

                for (int ii = 0; ii < RS.Rows.Count; ii++)
                //for(DataRow row in RS.Rows)
                {
                    sbCuerpo.AppendLine("<br><b>" + RS.Rows[ii]["Nombre"].ToString() +
                        "</b> Identificación: <b>" + RS.Rows[ii]["Identificacion"].ToString() + "</b>");
                }

                #endregion

                #region Dibujar Teléfono, Dirección y la Ciudad "INICIO DE PLAN OPERATIVO".

                sbCuerpo.AppendLine("</td>");
                sbCuerpo.AppendLine("</tr>");
                sbCuerpo.AppendLine(" <tr bgcolor='#CCCCCC'>");
                sbCuerpo.AppendLine(" 		<td align='center' Class='TitDestacado'>");
                sbCuerpo.AppendLine(" 			<b>Teléfono: </b>" + Telefono);
                sbCuerpo.AppendLine(" 		</td>");
                sbCuerpo.AppendLine(" 		<td align='center' Class='TitDestacado'>");
                sbCuerpo.AppendLine(" 			<b>Dirección: </b>" + Direccion + " - " + Ciudad);
                sbCuerpo.AppendLine(" 		</td>");
                sbCuerpo.AppendLine(" 	</tr>");
                sbCuerpo.AppendLine("<tr>");
                sbCuerpo.AppendLine("	<td align='center' colspan=2>");
                sbCuerpo.AppendLine("		<table width=\"100%\" align=\"center\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\">");
                sbCuerpo.AppendLine("  <tr> ");
                sbCuerpo.AppendLine("    <td><p>&nbsp;</p></td>");
                sbCuerpo.AppendLine("  </tr>	");
                sbCuerpo.AppendLine("  <tr> ");
                sbCuerpo.AppendLine("    <td><p>&nbsp;</p></td>");
                sbCuerpo.AppendLine("  </tr>			");
                sbCuerpo.AppendLine("			<tr>");
                sbCuerpo.AppendLine("				<td bgcolor='#CCCCCC' align=center><B>PLAN OPERATIVO</B></td>");
                sbCuerpo.AppendLine("			</tr>");
                sbCuerpo.AppendLine("			<tr>");
                sbCuerpo.AppendLine("				<td align=\"center\">");
                sbCuerpo.AppendLine("					<table width=\"100%\" align=\"center\" border=\"1\" cellpadding=\"0\" cellspacing=\"0\">");
                #endregion

                #region Se traen las actividades del plan operativo.

                if (CodProyecto != 0)
                {

                    ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                    List<PlanOperativoEntity> lstActividades = new List<PlanOperativoEntity>();
                    lstActividades = imPlanOperNeg.TraerActividadesPlanOperativo(CodProyecto);

                    var maxMeses = (from mes in lstActividades
                                    orderby mes.Mes descending
                                    select mes).ToList();
                    int maximoMes = maxMeses[0].Mes;

                    List<String> lstActivides = new List<String>();
                    CodActividad = "";
                    for (int iAc = 0; iAc < lstActividades.Count; iAc++)
                    {
                        if (CodActividad != lstActividades[iAc].Id_Actividad.ToString())
                        {
                            CodActividad = lstActividades[iAc].Id_Actividad.ToString();
                            lstActivides.Add(CodActividad);
                        }
                    }

                    i = 0;
                    int fila = 1;
                    //foreach (DataRow row_actividades in rsActividades.Rows)
                    foreach (String actividad in lstActivides)
                    {
                        #region Ciclo inicial por Actividad

                        var lisActividad = (from act in lstActividades
                                            where act.CodActividad == Convert.ToInt32(actividad)
                                            select act).ToList();

                        CodActividad = "";
                        foreach (PlanOperativoEntity row_actividades in lisActividad)
                        {
                            #region Ciclo...
                            TotalFE = 0;
                            TotalEmp = 0;
                            TotalTipo1 = 0;
                            TotalTipo2 = 0;

                            NomActividad = row_actividades.NomActividad.Trim();


                            if (CodActividad != row_actividades.Id_Actividad.ToString())
                            {
                                #region Nombre de la actividad.

                                sbCuerpo.AppendLine("<tr>");
                                sbCuerpo.AppendLine("	<td colspan=\"12\">Actividad: " + row_actividades.NomActividad + "</td>");
                                sbCuerpo.AppendLine("</tr>");

                                #endregion

                                CodActividad = row_actividades.Id_Actividad.ToString();
                                //lbl_cuerpo.Text = lbl_cuerpo.Text + " <tr align='left' valign='top'>";
                                sbCuerpo.AppendLine(" <tr align=\"left\" valign=\"top\">");
                                i = i + 1;
                                fila = 1;
                            }

                            //Para pintar los datos en dos filas...

                            //if (CONST_Meses % 6 > 0)
                            //    filas = (CONST_Meses / 6) + 1;
                            //else
                            //    filas = (CONST_Meses / 6);
                            if (maximoMes % 6 > 0)
                                filas = (maximoMes / 6) + 1;
                            else
                                filas = (maximoMes / 6);

                            // Armo las caberas de 6 meses
                            for (int iFilas = 1; iFilas <= filas; iFilas++)
                            {

                                sbCuerpo.AppendLine("					<tr>");
                                for (int j = (6 * iFilas) - 5; j <= 6 * iFilas; j++)
                                {
                                    sbCuerpo.AppendLine("					<td colspan=\"2\" align=\"center\">Mes " + j * fila + "</td>");
                                }
                                sbCuerpo.AppendLine("					</tr>");

                                // Armo  las columnas para mes
                                sbCuerpo.AppendLine("					<tr>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">Fondo</td><td style=\"width: 8.3%;text-align:center\">Emprendedor</td>");
                                sbCuerpo.AppendLine("					</tr>");

                                // Fila de los DATOS
                                sbCuerpo.AppendLine("					<tr>");

                                for (int x = (6 * iFilas) - 5; x <= 6 * iFilas; x++)
                                {
                                    var acti = (from act in lisActividad
                                                where act.Mes == x
                                                select act).FirstOrDefault();
                                    if (acti != null)
                                    {
                                        sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\">" +
                                            (acti.CodTipoFinanciacion == 1 ?
                                            acti.Valor.ToString(
                                                "C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"))
                                                : "") + "</td>");
                                        sbCuerpo.AppendLine("<td style=\"width: 8.3%;text-align:center\">" +
                                            (acti.CodTipoFinanciacion == 2 ?
                                            acti.Valor.ToString(
                                                "C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO"))
                                                : "") +
                                            "</td>");

                                        if (acti.CodTipoFinanciacion == 1)
                                            TotalFE += acti.Valor;
                                        else
                                            TotalEmp += acti.Valor;
                                    }
                                    else
                                        sbCuerpo.AppendLine("					<td style=\"width: 8.3%;text-align:center\"></td><td style=\"width: 8.3%;text-align:center\"></td>");


                                    //sbCuerpo.AppendLine("              </tr>");
                                }  // Fin For de Filas por Actividad

                                sbCuerpo.AppendLine("					</tr>");


                                /*************************************************/
                                //Avance de Actividad por MES
                                /*************************************************/
                                // 2014/11/24 RAlvaradoT Se sacan los dos querys que son iguales, solo cambia el tipo de financiacion
                                // Configuration esto se va 1 sola vez a la base de datos y se adiciona una pregunta para 
                                // mejorar el performance de este reporte 

                                //ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                                List<AvanceActividadPOMes> lstPlanes = new List<AvanceActividadPOMes>();
                                lstPlanes = imPlanOperNeg.TraerAvanceActividadPOXMes(CodActividad);

                                sbCuerpo.AppendLine("              <tr>");
                                for (int xQ = (6 * iFilas) - 5; xQ <= 6 * iFilas; xQ++)
                                {
                                    // Tipo 1
                                    var ti1 = (from avanc in lstPlanes
                                               where avanc.Mes == xQ & avanc.CodTipoFinanciacion == 1
                                               select avanc).FirstOrDefault();

                                    if (ti1 != null)
                                    {
                                        Tipo1 = ti1.Valor.ToString();
                                        TotalTipo1 += Convert.ToDouble(ti1.Valor);
                                    }
                                    else
                                    {
                                        Tipo1 = "";
                                    }

                                    sbCuerpo.AppendLine("              <td align=\"right\">");
                                    if (Tipo1 != "")
                                    {
                                        try
                                        {
                                            sbCuerpo.AppendLine(ti1.Valor.ToString("C2",
                                                System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));

                                        }
                                        catch (Exception)
                                        {

                                            throw;
                                        }
                                    }
                                    else
                                    {
                                        sbCuerpo.AppendLine(Tipo1);
                                    }
                                    sbCuerpo.AppendLine("              </td>");

                                    // Tipo 2
                                    var ti2 = (from avanc in lstPlanes
                                               where avanc.Mes == xQ & avanc.CodTipoFinanciacion == 2
                                               select avanc).FirstOrDefault();

                                    if (ti2 != null)
                                    {
                                        Tipo2 = ti2.Valor.ToString();
                                        TotalTipo2 += Double.Parse(ti2.Valor.ToString());
                                    }
                                    else
                                    {
                                        Tipo2 = "";
                                    }

                                    sbCuerpo.AppendLine(@"              <td style=\""text-align:right; color:#CC0000\"">");
                                    if (Tipo2 != "")
                                    {
                                        try
                                        {
                                        sbCuerpo.AppendLine(ti2.Valor.ToString("C2",
                                            System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));

                                        }
                                        catch (Exception)
                                        {
                                            
                                            throw;
                                        }
                                    }
                                    else
                                    {
                                        sbCuerpo.AppendLine(Tipo2);
                                    }
                                    sbCuerpo.AppendLine("              </td>");
                                }

                                #region For (2).

                                //for (int m = 1; m <= filas; m++)
                                //{
                                //    z = m * 6 - 5;
                                //    if (!String.IsNullOrEmpty(row_actividades["Mes"].ToString()))
                                //    {
                                //        #region Se pintan los titulos de los meses.

                                //        //sbCuerpo.AppendLine("					<tr>");

                                //        //// Arma las columnas d elos meses, los titulos en este caso
                                //        //for (int j = z; j <= 6 * m; j++)
                                //        //{
                                //        //    if (j <= CONST_Meses)
                                //        //    {
                                //        //        // 2014/11/21  Se comenta no hace NADA
                                //        //        //if (j > 12) { }//aaa
                                //        //        sbCuerpo.AppendLine("					<td colspan=\"2\" align=\"center\">Mes " + j + "</td>");
                                //        //    }
                                //        //}

                                //        //sbCuerpo.AppendLine("					</tr>");

                                //        //#endregion

                                //        //#region Se pintan los titulos de los tipos de aportes.

                                //        //sbCuerpo.AppendLine("					<tr>");                                
                                //        //// como de el resultado de la operacion
                                //        //for (int j = z; j <= 6 * m; j++)
                                //        //{
                                //        //    if (j <= CONST_Meses)
                                //        //    {
                                //        //        sbCuerpo.AppendLine("					<td align=\"center\">Fondo</td><td align=\"center\">Emprendedor</td>");
                                //        //    }
                                //        //}

                                //        //sbCuerpo.AppendLine("					</tr>");

                                //        //#endregion

                                //        for (int j = z; j <= 6 * m; j++)
                                //        {
                                //    //        if (j <= CONST_Meses)
                                //    //        {
                                //    //            #region Mes y CodTipoFinanciacion (ciclo).

                                //    //            for (int k = 1; k <= Constantes.CONST_Fuentes; k++) //Fuentes de Financiación.
                                //    //            {
                                //    //                if (rsActividades.Rows.Count > 0)
                                //    //                {
                                //    //                    if (j == Int32.Parse(row_actividades["Mes"].ToString()) && k == Int32.Parse(row_actividades["CodTipoFinanciacion"].ToString()))
                                //    //                    {
                                //    //                        #region Colocar el valor.

                                //    //                        sbCuerpo.AppendLine("                <td align=\"right\">" +
                                //    //                                                Double.Parse(row_actividades["Valor"].ToString())
                                //    //                                                .ToString("C2", System.Globalization.CultureInfo
                                //    //                                                .CreateSpecificCulture("es-CO")) + "</td>");
                                //    //                        switch (k)
                                //    //                        {
                                //    //                        case 1:
                                //    //                            TotalFE = TotalFE + Double.Parse(row_actividades["Valor"].ToString());
                                //    //                            break;
                                //    //                        case 2:
                                //    //                            TotalEmp = TotalEmp + Double.Parse(row_actividades["Valor"].ToString());
                                //    //                            break;
                                //    //                        default:
                                //    //                            break;
                                //    //                        }
                                //    //                        //rsActividad.movenext 

                                //    //                        #endregion
                                //    //                    }
                                //    //                    else
                                //    //                    {
                                //    //                        sbCuerpo.AppendLine("                <td align=\"right\">&nbsp;</td>");
                                //    //                    }
                                //    //                }
                                //    //                else
                                //    //                {
                                //    //                    sbCuerpo.AppendLine("                <td align=\"right\">&nbsp;</td>");
                                //    //                }
                                //    //            }

                                //    //            #endregion
                                //    //        }
                                //    //    }
                                //    //    //Costo Total.
                                //    //    //lbl_cuerpo.Text = lbl_cuerpo.Text = "                 <td align='right'>"&formatCurrency(TotalFE,2)&"</td>";
                                //    //    //lbl_cuerpo.Text = lbl_cuerpo.Text = "                <td align='right'>"&formatCurrency(TotalEmp,2)&"</td>";
                                //    //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "              </tr>";
                                //    //    sbCuerpo.AppendLine("              </tr>");
                                //    //}
                                //    //else
                                //    //{
                                //    //    #region CONST_Meses * Constantes.CONST_Fuentes
                                //    //    for (int j = z; j < CONST_Meses * Constantes.CONST_Fuentes; j++)//abc
                                //    //    {
                                //    //        //lbl_cuerpo.Text = lbl_cuerpo.Text + "<td align='right'>&nbsp;</td>";
                                //    //        //lbl_cuerpo.Text = lbl_cuerpo.Text + "              </tr>";

                                //    //        sbCuerpo.AppendLine("                <td align=\"right\">&nbsp;</td>");

                                //    //    }
                                //    //    #endregion

                                //    //    //Costo Total
                                //    //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "                 <td align='right'>"&formatCurrency(TotalFE,2)&"</td>"&VbCrLf
                                //    //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "                <td align='right'>"&formatCurrency(TotalEmp,2)&"</td>"&VbCrLf

                                //    //    sbCuerpo.AppendLine("              </tr>");
                                //    //    //rsActividad.movenext
                                //    //}

                                //    ////Avances reportados.
                                //    //sbCuerpo.AppendLine("              <tr>");
                                //            #endregion

                                //    TotalTipo1 = 0;
                                //    TotalTipo2 = 0;


                                //    // 2014/11/24 RAlvaradoT Se sacan los dos querys que son iguales, solo cambia el tipo de financiacion
                                //    // Configuration esto se va 1 sola vez a la base de datos y se adiciona una pregunta para 
                                //    // mejorar el performance de este reporte 

                                //    //ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                                //    List<AvanceActividadPOMe> lstPlanes = new List<AvanceActividadPOMe>();
                                //    lstPlanes = imPlanOperNeg.TraerAvanceActividadPOXMes(CodActividad);

                                //    for (int j = z; j <= 6 * m; j++)
                                //    {
                                //        if (j <= CONST_Meses)
                                //        {
                                //            #region Tipo 1.

                                //            // Esto se comenta y se saca del For
                                //            //txtSQL = " SELECT * " +
                                //            //         " FROM AvanceActividadPOMes " +
                                //            //         " WHERE codactividad = " + CodActividad +
                                //            //         " AND Mes = " + j + " AND codtipofinanciacion = 1 ";

                                //            //rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");

                                //            var ti1 = (from avanc in lstPlanes
                                //                       where avanc.Mes == j & avanc.CodTipoFinanciacion == 1
                                //                       select avanc).FirstOrDefault();

                                //            //if (rsTipo1.Rows.Count > 0   )
                                //            if (ti1 != null)
                                //            {
                                //                //Tipo1 = Double.Parse(rsTipo1.Rows[0]["valor"].ToString()).ToString();
                                //                //TotalTipo1 = TotalTipo1 + Double.Parse(rsTipo1.Rows[0]["valor"].ToString());
                                //                Tipo1 = ti1.Valor.ToString();
                                //                TotalTipo1 += Convert.ToDouble(ti1.Valor);
                                //            }
                                //            else
                                //            {
                                //                Tipo1 = "";
                                //            }

                                //            sbCuerpo.AppendLine("              <td align=\"right\">");
                                //            if (Tipo1 != "")
                                //            {
                                //                sbCuerpo.AppendLine(ti1.Valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                                //            }
                                //            else
                                //            {
                                //                sbCuerpo.AppendLine(Tipo1);
                                //            }
                                //            sbCuerpo.AppendLine("              </td>");
                                //            #endregion

                                //            #region Tipo 2.

                                //            //txtSQL = " SELECT * " +
                                //            //         " FROM AvanceActividadPOMes " +
                                //            //         " WHERE codactividad=" + CodActividad +
                                //            //         " AND Mes=" + j + " AND codtipofinanciacion = 2 ";

                                //            //rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");
                                //            var ti2 = (from avanc in lstPlanes
                                //                       where avanc.Mes == j & avanc.CodTipoFinanciacion == 2
                                //                       select avanc).FirstOrDefault();

                                //            //if (rsTipo2.Rows.Count > 0)
                                //            if (ti2 != null)
                                //            {
                                //                //Tipo2 = Double.Parse(rsTipo2.Rows[0]["valor"].ToString()).ToString();
                                //                //TotalTipo2 = TotalTipo2 + Double.Parse(rsTipo2.Rows[0]["valor"].ToString());
                                //                Tipo2 = ti2.ToString();
                                //                TotalTipo2 += Double.Parse(ti2.ToString());
                                //            }
                                //            else
                                //            {
                                //                Tipo2 = "";
                                //            }

                                //            sbCuerpo.AppendLine(@"              <td style=\""text-align:right; color:#CC0000\"">");
                                //            if (Tipo2 != "")
                                //            {
                                //                sbCuerpo.AppendLine(ti2.Valor.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                                //            }
                                //            else
                                //            {
                                //                sbCuerpo.AppendLine(Tipo2);
                                //            }
                                //            sbCuerpo.AppendLine("              </td>");
                                //            #endregion
                                //        }
                                //    }

                                //    //Costo Total de Avances reportados
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "              <td align='right'><font color='#CC0000'>"&VbCrLf
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + formatCurrency(TotalTipo1,2)
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "              </font></td>"&VbCrLf
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "              <td align='right'><font color='#CC0000'>"&VbCrLf
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + formatCurrency(TotalTipo2,2)
                                //    //lbl_cuerpo.Text = lbl_cuerpo.Text + "              </font> </td>"&VbCrLf

                                //    sbCuerpo.AppendLine("              </tr>");
                                //}

                                #endregion
                            }

                            #region Líneas...

                            sbCuerpo.AppendLine("							<tr>");
                            sbCuerpo.AppendLine("								<td COLSPAN='2'>Costo Total:</td>");
                            sbCuerpo.AppendLine("							</tr>");
                            sbCuerpo.AppendLine("							<tr>");

                            sbCuerpo.AppendLine("								<td align='Center'>Emprendedor</td>");
                            sbCuerpo.AppendLine("								<td align='Center'>Fondo</td>");
                            sbCuerpo.AppendLine("							</tr>");

                            //Costo Total
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td align='right'>" + TotalEmp.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("                <td align='right'>" + TotalFE.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            //Costo Total de Avances reportados
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("              <td align='right'>");
                            sbCuerpo.AppendLine(TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                            sbCuerpo.AppendLine("              </td>");
                            sbCuerpo.AppendLine("              <td align='right'>");
                            sbCuerpo.AppendLine(TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                            sbCuerpo.AppendLine("              </td>");
                            sbCuerpo.AppendLine("              </tr>");

                            // Salgo del For para la siguiente actividad
                            break;

                            #endregion

                            #endregion
                            //}

                            //rsActividades.MoveNext 

                        #endregion
                        }
                    }

                #endregion

                    #region Nómina.

                    sbCuerpo.AppendLine("</table>");
                    sbCuerpo.AppendLine("		</td>");
                    sbCuerpo.AppendLine("	</tr>");
                    sbCuerpo.AppendLine("	<tr>");
                    sbCuerpo.AppendLine("		<td colspan=12 bgcolor='#CCCCCC' class='TitDestacado' align=center><B>NÓMINA</B></td>");
                    sbCuerpo.AppendLine("	</tr>");
                    sbCuerpo.AppendLine("	<tr>");
                    sbCuerpo.AppendLine("		<td class='TitDestacado' align=center>");
                    sbCuerpo.AppendLine("			<TABLE width='100%' align=center border='1' cellpadding='0' cellspacing='0'>");

                    #region Personal Calificado - Cargos.

                    txtSQL = " select Id_Nomina, Cargo from InterventorNomina where Tipo='Cargo' and codproyecto=" + CodProyecto + " order by id_nomina ";
                    rsCargo = consultas.ObtenerDataTable(txtSQL, "text");

                    i = 0;

                    if (rsCargo.Rows.Count > 0)
                    {
                        sbCuerpo.AppendLine("     		<tr class='Titulo'> ");
                        sbCuerpo.AppendLine("       			<td colspan=12><b>Personal Calificado</b></td>");
                        sbCuerpo.AppendLine("     		</tr>");

                        #region Cargo.

                        foreach (DataRow r_rsCargo in rsCargo.Rows)
                        {
                            sbCuerpo.AppendLine("              <tr align='left' valign='top'>");
                            sbCuerpo.AppendLine("                <td align='left' colspan=12>" + r_rsCargo["Cargo"].ToString() + "</td>");
                            sbCuerpo.AppendLine("              </tr>");
                            i = i + 1;

                            TotalTipo1 = 0;
                            TotalTipo2 = 0;

                            TotalTipo1A = 0;
                            TotalTipo2A = 0;

                            //para pintar los datos en dos filas.
                            for (int m = 1; m <= filas; m++)
                            {
                                #region Interno.

                                z = m * 6 - 5;

                                #region Pinta los titulos de los meses.
                                sbCuerpo.AppendLine("              <tr>");
                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        // 2014/11/20 NO Hace NADA, se comenta
                                        //if (k > 12) { }
                                        sbCuerpo.AppendLine("                <td align='center' colspan=2>Mes " + k + "</td>");
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Pinta los titulos de sueldo/prestaciones.
                                sbCuerpo.AppendLine("              <tr>");
                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        sbCuerpo.AppendLine("                <td align='center'>Sueldo</td><td align='center'>Prestaciones</td>");
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Pinta los valores de nómina.
                                sbCuerpo.AppendLine("              <tr>");


                                // 2014/11/24 RAlvaradoT saco fuera del for la ida a la Base de datos y que vaya 1 sola vez 
                                // y a traves de LinQ hacer el filtro 

                                List<InterventoriaNominaEntity> lstNomina = new List<InterventoriaNominaEntity>();
                                lstNomina = imPlanOperNeg.TraerInterventoriaNominaXMes(CodProyecto);

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Trae valores de tipo sueldo.

                                        //txtSQL = " SELECT * FROM InterventorNomina a, InterventorNominaMes b " +
                                        //         " WHERE a.tipo = 'Cargo' AND id_nomina = codcargo AND b.Tipo = 1 " +
                                        //         " AND codproyecto = " + CodProyecto +
                                        //         " AND mes = " + k +
                                        //         " ORDER BY id_nomina, mes, b.tipo";

                                        //rsCargoss = consultas.ObtenerDataTable(txtSQL, "text");

                                        Tipo1 = "&nbsp;";

                                        var Cargos1 = (from nom in lstNomina
                                                       where nom.Tipo == 1 && nom.Mes == k
                                                       select nom).FirstOrDefault();

                                        //if (rsCargoss.Rows.Count > 0)
                                        if (Cargos1 != null)
                                        {
                                            //Tipo1 = rsCargoss.Rows[0]["Valor"].ToString();
                                            //TotalTipo1 = TotalTipo1 + Double.Parse(rsCargoss.Rows[0]["Valor"].ToString());
                                            Tipo1 = Cargos1.Valor.ToString();
                                            TotalTipo1 += Cargos1.Valor;
                                        }

                                        #endregion

                                        #region Trae valores de tipo prestaciones.

                                        //txtSQL = " SELECT * FROM InterventorNomina a, InterventorNominaMes b " +
                                        //         " WHERE a.tipo = 'Cargo' AND id_nomina=codcargo AND b.Tipo = 2" +
                                        //         " AND codproyecto = " + CodProyecto +
                                        //         " AND mes = " + k +
                                        //         " ORDER BY id_nomina, mes, b.tipo ";
                                        //rsCargoss = consultas.ObtenerDataTable(txtSQL, "text");
                                        var Cargos2 = (from nom in lstNomina
                                                       where nom.Tipo == 2 && nom.Mes == k
                                                       select nom).FirstOrDefault();

                                        Tipo2 = "&nbsp;";

                                        //if (rsCargoss.Rows.Count > 0)
                                        if (Cargos2 != null)
                                        {
                                            //Tipo2 = rsCargoss.Rows[0]["Valor"].ToString();
                                            //TotalTipo2 = TotalTipo2 + Double.Parse(rsCargoss.Rows[0]["Valor"].ToString());
                                            Tipo2 = Cargos2.Valor.ToString();
                                            TotalTipo2 += Cargos2.Valor;
                                        }

                                        #endregion

                                        if (Tipo1 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td>" + Tipo1 + "</td>");
                                        }

                                        if (Tipo2 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td>" + Tipo2 + "</td>");
                                        }
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Pinta los valores reportados por el emprendedor.

                                //ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                                List<AvanceCargoPOMes> lstValEmprendedor = new List<AvanceCargoPOMes>();
                                lstValEmprendedor = imPlanOperNeg.TraerAvanceCargoPOMes(Convert.ToInt32(r_rsCargo["Id_Nomina"]));

                                //for (int k = z; i < m * 6; i++)//mal
                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Tipo 1.

                                        //txtSQL = " select * " +
                                        //         " from AvanceCargoPOMes " +
                                        //         " where CodCargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and Mes = " + k + " and codtipofinanciacion = 1 ";

                                        //rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");

                                        var varTipo1 = (from vaEmp in lstValEmprendedor
                                                        where vaEmp.CodTipoFinanciacion == 1 && vaEmp.Mes == k
                                                        select vaEmp
                                                           ).FirstOrDefault();
                                        //if (rsTipo1.Rows.Count > 0)
                                        if (varTipo1 != null)
                                        {
                                            //Tipo1 = rsTipo1.Rows[0]["valor"].ToString();
                                            //TotalTipo1A = TotalTipo1A + Double.Parse(rsTipo1.Rows[0]["valor"].ToString());
                                            Tipo1 = varTipo1.Valor.ToString();
                                            TotalTipo1A += Convert.ToDouble(varTipo1.Valor);
                                        }
                                        else { Tipo1 = ""; }


                                        sbCuerpo.AppendLine("              <td align='right'>");
                                        if (Tipo1 != "")
                                        {
                                            sbCuerpo.AppendLine(Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine(Tipo1);
                                        }
                                        sbCuerpo.AppendLine("              </td>");

                                        #endregion

                                        #region Tipo 2.

                                        //txtSQL = " select * " +
                                        //         " from AvanceCargoPOMes " +
                                        //         " where CodCargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and Mes=" + k + " and codtipofinanciacion = 2 ";

                                        //rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");
                                        var varTipo2 = (from vaEmp in lstValEmprendedor
                                                        where vaEmp.CodTipoFinanciacion == 2 && vaEmp.Mes == k
                                                        select vaEmp
                                                        ).FirstOrDefault();

                                        //if (rsTipo2.Rows.Count > 0)
                                        if (varTipo2 != null)
                                        {
                                            //Tipo2 = rsTipo2.Rows[0]["valor"].ToString();
                                            //TotalTipo2A = TotalTipo2A + Double.Parse(rsTipo2.Rows[0]["valor"].ToString());
                                            Tipo2 = varTipo2.Valor.ToString();
                                            TotalTipo2A += Convert.ToDouble(varTipo2.Valor);
                                        }
                                        else
                                        { Tipo2 = ""; }

                                        sbCuerpo.AppendLine("              <td align='right'>");
                                        if (Tipo2 != "")
                                        {
                                            sbCuerpo.AppendLine(Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")));
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine(Tipo2);
                                        }
                                        sbCuerpo.AppendLine("              </td>");

                                        #endregion
                                    }
                                }
                                #endregion

                                #endregion
                            }

                            #region Líneas finales.

                            //Costo Total
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td colspan=2>Costo Total</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("              </tr>");
                            //Costo Total avances reportados
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            //rsCargo.movenext

                            #endregion
                        }

                        #endregion
                    }

                    #endregion

                    #region Personal Calificado - Insumos.

                    txtSQL = " select * from InterventorNomina where Tipo = 'Insumo' and codproyecto = " + CodProyecto + " order by id_nomina";
                    rsCargo = consultas.ObtenerDataTable(txtSQL, "text");
                    i = 0;

                    if (rsCargo.Rows.Count > 0)
                    {
                        sbCuerpo.AppendLine("     		<tr class='Titulo'> ");
                        sbCuerpo.AppendLine("       			<td colspan=12><b>Mano de Obra Directa</b></td>");
                        sbCuerpo.AppendLine("     		</tr>");

                        #region Insumo (ciclo).

                        foreach (DataRow r_rsCargo in rsCargo.Rows)
                        {
                            #region Ciclo inicial de nómina.

                            sbCuerpo.AppendLine("              <tr align='left' valign='top'>");
                            sbCuerpo.AppendLine("                <td colspan=12 align='left'>" + r_rsCargo["Cargo"].ToString() + "</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            TotalTipo1 = 0;
                            TotalTipo2 = 0;

                            TotalTipo1A = 0;
                            TotalTipo2A = 0;

                            for (int m = 1; m <= filas; m++)
                            {
                                #region Insumo.

                                z = m * 6 - 5;

                                #region Pinta los titulos de los meses.
                                sbCuerpo.AppendLine("              <tr>");

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        // 2014/11/20 Se comenta no hace NADA
                                        //if (k > 12) { }
                                        sbCuerpo.AppendLine("                <td align='center' colspan=2>Mes " + k + "</td>");
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Pinta los titulos de sueldo/prestaciones.
                                sbCuerpo.AppendLine("              <tr>");

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        sbCuerpo.AppendLine("                <td align='center'>Sueldo</td><td align='center'>Prestaciones</td>");
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Tipos 1 y 2.

                                sbCuerpo.AppendLine("              <tr>");

                                //ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                                List<InterventoriaNominaEntity> lstInsumos = new List<InterventoriaNominaEntity>();
                                lstInsumos = imPlanOperNeg.TraerInterventoriaInsumoXMes(CodProyecto);


                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Tipo 1.

                                        //txtSQL = " select *  from InterventorNomina a,InterventorNominaMes b " +
                                        //         " where a.tipo='Insumo' and id_nomina=codcargo and codcargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and codproyecto = " + CodProyecto + " and mes = " + k + " and b.Tipo = 1 order by id_nomina, mes, b.tipo ";

                                        //rsCargos = consultas.ObtenerDataTable(txtSQL, "text");
                                        var Cargos1 = (from ins in lstInsumos
                                                       where ins.Id_Nomina == Convert.ToInt32(r_rsCargo["Id_Nomina"])
                                                       && ins.Mes == k && ins.Tipo == 1
                                                       select ins).FirstOrDefault();

                                        Tipo1 = "&nbsp;";

                                        //if (rsCargos.Rows.Count > 0)
                                        if (Cargos1 != null)
                                        {
                                            Tipo1 = Cargos1.Valor.ToString();
                                            TotalTipo1 += Double.Parse(Cargos1.Valor.ToString());
                                        }

                                        #endregion

                                        #region Tipo 2.

                                        //txtSQL = " select *  from InterventorNomina a,InterventorNominaMes b " +
                                        //         " where a.tipo='Insumo' and id_nomina=codcargo and codcargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and codproyecto = " + CodProyecto + " and mes = " + k + " and b.Tipo = 2 order by id_nomina, mes, b.tipo ";

                                        //rsCargos = consultas.ObtenerDataTable(txtSQL, "text");

                                        var Cargos2 = (from ins in lstInsumos
                                                       where ins.Id_Nomina == Convert.ToInt32(r_rsCargo["Id_Nomina"])
                                                       && ins.Mes == k && ins.Tipo == 2
                                                       select ins).FirstOrDefault();

                                        Tipo2 = "&nbsp;";

                                        if (rsCargos.Rows.Count > 0)
                                        {
                                            Tipo2 = Cargos2.Valor.ToString();
                                            TotalTipo2 += Double.Parse(Cargos2.Valor.ToString());
                                        }

                                        #endregion

                                        if (Tipo1 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>");
                                        }

                                        if (Tipo2 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>");
                                        }
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");

                                #endregion

                                #region Avances de tipo 1 y 2.

                                sbCuerpo.AppendLine("              <tr>");
                                // Traigo los datos 1 Sola vez
                                List<AvanceCargoPOMes> lista = new List<AvanceCargoPOMes>();
                                lista = imPlanOperNeg.TraerAvanceCargoPOMes(Convert.ToInt32(r_rsCargo["Id_Nomina"]));

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Tipo 1.

                                        //txtSQL = " select * " +
                                        //         " from AvanceCargoPOMes " +
                                        //         " where CodCargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and Mes = " + k + " and codtipofinanciacion = 1 ";
                                        //rsCargos = consultas.ObtenerDataTable(txtSQL, "text");

                                        var vTipo1 = (from ava in lista
                                                      where ava.Mes == k && ava.CodTipoFinanciacion == 1
                                                      select ava).FirstOrDefault();
                                        Tipo1 = "&nbsp;";

                                        //if (rsCargos.Rows.Count > 0)
                                        if (vTipo1 != null)
                                        {
                                            Tipo1 = vTipo1.Valor.ToString();
                                            TotalTipo1A += Double.Parse(vTipo1.Valor.ToString());
                                        }

                                        #endregion

                                        #region Tipo 2.

                                        //txtSQL = " select * " +
                                        //         " from AvanceCargoPOMes " +
                                        //         " where CodCargo = " + r_rsCargo["Id_Nomina"].ToString() +
                                        //         " and Mes = " + k + " and codtipofinanciacion = 2 ";
                                        //rsCargos = consultas.ObtenerDataTable(txtSQL, "text");

                                        var vTipo2 = (from ava in lista
                                                      where ava.Mes == k && ava.CodTipoFinanciacion == 2
                                                      select ava).FirstOrDefault();

                                        Tipo2 = "&nbsp;";

                                        //if (rsCargos.Rows.Count > 0)
                                        if (vTipo2 != null)
                                        {
                                            Tipo2 = vTipo2.Valor.ToString();
                                            TotalTipo2A += Double.Parse(vTipo2.Valor.ToString());
                                        }

                                        #endregion

                                        if (Tipo1 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>");
                                        }

                                        if (Tipo2 != "&nbsp;")
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                                        }
                                        else
                                        {
                                            sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>");
                                        }
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");

                                #endregion

                                #endregion
                            }

                            //Costo Total
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td colspan=2>Costo Total</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("              </tr>");
                            //Costo Total avances reportados
                            sbCuerpo.AppendLine("              <tr>");
                            sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            i = i + 1;
                            //rsCargo.movenext 

                            #endregion
                        }

                        #endregion
                    }

                    #endregion

                    #endregion

                    #region Producción.

                    sbCuerpo.AppendLine("</table>");
                    sbCuerpo.AppendLine("	</td>");
                    sbCuerpo.AppendLine("</tr>");
                    sbCuerpo.AppendLine("<tr>");
                    sbCuerpo.AppendLine("	<td colspan=12 bgcolor='#CCCCCC' class='TitDestacado' align=center><B>PRODUCCIÓN</B></td>");
                    sbCuerpo.AppendLine("</tr>");
                    sbCuerpo.AppendLine("<tr>");
                    sbCuerpo.AppendLine("<td class='TitDestacado' align=center>");
                    sbCuerpo.AppendLine("<TABLE width='100%' align=center border='1' cellpadding='0' cellspacing='0'>");

                    if (CodProyecto != 0)
                    {
                        txtSQL = " select * from InterventorProduccion " +
                                 " where codproyecto = " + CodProyecto + " order by id_produccion ";
                        rsProducto = consultas.ObtenerDataTable(txtSQL, "text");

                        sbCuerpo.AppendLine("     		<tr class='Titulo'> ");
                        sbCuerpo.AppendLine("       			<td colspan=12><b>Producto o Servicio</b></td>");
                        sbCuerpo.AppendLine("     		</tr>");

                        i = 0;
                        //txtProducto = "";

                        foreach (DataRow r_rsProducto in rsProducto.Rows)
                        {
                            #region Ciclo de producto.

                            sbCuerpo.AppendLine("              <tr align='left' valign='top'>");
                            sbCuerpo.AppendLine("                <td align='left' colspan=12>" + r_rsProducto["NomProducto"].ToString() + "</td>");
                            sbCuerpo.AppendLine("              </tr>");

                            TotalTipo1 = 0;
                            TotalTipo2 = 0;

                            TotalTipo1A = 0;
                            TotalTipo2A = 0;

                            for (int m = 1; m <= filas; m++)
                            {
                                #region Producción.

                                z = m * 6 - 5;

                                #region Pinta los titulos de los meses.
                                sbCuerpo.AppendLine("              <tr>");

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        // 2014/11/20 NO hace NADA se comenta
                                        //if (k > 12) { }
                                        sbCuerpo.AppendLine("                <td align='center' colspan=2>Mes " + k + "</td>");
                                    }
                                }
                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Pinta los titulos de sueldo/prestaciones.
                                sbCuerpo.AppendLine("              <tr>");
                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        sbCuerpo.AppendLine("                <td align='center'>Cantidad</td><td align='center'>Costo</td>");
                                    }
                                }
                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                sbCuerpo.AppendLine("              <tr align='left' valign='top'>");

                                #region Datos registrados y aprobados.
                                // Traigo los datos 1 Sola vez
                                //ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                                List<InterventorProduccionEntity> lista = new List<InterventorProduccionEntity>();
                                lista = imPlanOperNeg.TraerInterventorProduccionXMes(CodProyecto);

                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Tipo 1.

                                        //txtSQL = " select * from InterventorProduccion,InterventorProduccionMes " +
                                        //         " where id_produccion=codproducto and codproyecto = " + CodProyecto +
                                        //         " and mes = " + k + " and tipo = 1 order by id_produccion, mes, tipo";
                                        //rsProductos = consultas.ObtenerDataTable(txtSQL, "text");
                                        var prod1 = (from prod in lista
                                                     where prod.Mes == k && prod.Tipo == 1
                                                     select prod).FirstOrDefault();
                                        Tipo1 = "&nbsp;";
                                        //if (rsProductos.Rows.Count > 0)
                                        if (prod1 != null)
                                        {
                                            Tipo1 = prod1.Valor.ToString();
                                            TotalTipo1 += Double.Parse(prod1.Valor.ToString());
                                        }

                                        #endregion

                                        #region Tipo 2.

                                        //txtSQL = " select * from InterventorProduccion,InterventorProduccionMes " +
                                        //         " where id_produccion=codproducto and codproyecto = " + CodProyecto +
                                        //         " and mes = " + k + " and tipo = 2 order by id_produccion, mes, tipo";
                                        //rsProductos = consultas.ObtenerDataTable(txtSQL, "text");
                                        var prod2 = (from prod in lista
                                                     where prod.Mes == k && prod.Tipo == 2
                                                     select prod).FirstOrDefault();

                                        Tipo2 = "&nbsp;";
                                        //if (rsProductos.Rows.Count > 0)
                                        if (prod2 != null)
                                        {
                                            Tipo2 = prod2.Valor.ToString();
                                            TotalTipo2 += Double.Parse(prod2.Valor.ToString());
                                        }

                                        #endregion

                                        if (Tipo1 == "&nbsp;")
                                        { sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>"); }
                                        else
                                        { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }

                                        if (Tipo2 == "&nbsp;")
                                        { sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>"); }
                                        else
                                        { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion

                                #region Datos avances reportados.
                                sbCuerpo.AppendLine("              <tr align='left' valign='top'>");

                                List<AvanceProduccionPOMes> lstAvnceProd = new List<AvanceProduccionPOMes>();
                                lstAvnceProd = imPlanOperNeg.TraerAvanceProduccionPOMes(Convert.ToInt32(r_rsProducto["Id_produccion"]));

                                //for (int k = z; i < m * 6; i++)//mal
                                for (int k = z; k <= m * 6; k++)
                                {
                                    if (k <= CONST_Meses)
                                    {
                                        #region Tipo 1.
                                        //txtSQL = " select * " +
                                        //         " from AvanceProduccionPOMes " +
                                        //         " where CodProducto=" + r_rsProducto["Id_produccion"].ToString() +
                                        //         " and mes = " + k + " and codtipofinanciacion = 1 ";
                                        //rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");
                                        var prod1 = (from pro in lstAvnceProd
                                                     where pro.CodTipoFinanciacion == 1 && pro.Mes == k
                                                     select pro).FirstOrDefault();
                                        //if (rsTipo1.Rows.Count > 0)
                                        if (prod1 != null)
                                        {
                                            Tipo1 = prod1.Valor.ToString();
                                            TotalTipo1A += Double.Parse(prod1.Valor.ToString());
                                        }
                                        else
                                        {
                                            Tipo1 = "&nbsp;";
                                        }
                                        #endregion

                                        #region Tipo 2.
                                        //txtSQL = " select * " +
                                        //         " from AvanceProduccionPOMes " +
                                        //         " where CodProducto=" + r_rsProducto["Id_produccion"].ToString() +
                                        //         " and mes = " + k + " and codtipofinanciacion = 2 ";
                                        //rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");
                                        var prod2 = (from pro in lstAvnceProd
                                                     where pro.CodTipoFinanciacion == 2 && pro.Mes == k
                                                     select pro).FirstOrDefault();

                                        //if (rsTipo2.Rows.Count > 0)
                                        if (prod2 != null)
                                        {
                                            Tipo2 = prod2.Valor.ToString();
                                            TotalTipo2A += Double.Parse(prod2.Valor.ToString());
                                        }
                                        else
                                        {
                                            Tipo2 = "&nbsp;";
                                        }
                                        #endregion

                                        if (Tipo1 == "&nbsp;")
                                        { sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>"); }
                                        else
                                        { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }

                                        if (Tipo2 == "&nbsp;")
                                        { sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>"); }
                                        else
                                        { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }
                                    }
                                }

                                sbCuerpo.AppendLine("              </tr>");
                                #endregion
                            }
                                #endregion
                        }

                        #region Líneas finales.

                        //Costo Total
                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td colspan=\"2\">Costo Total</td>");
                        sbCuerpo.AppendLine("              </tr>");

                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td align=\"right\">" + TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("                <td align=\"right\">" + TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("              </tr>");
                        //Costo Total avances reportados
                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td align=\"right\">" + TotalTipo1A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("                <td align=\"right\">" + TotalTipo2A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("              </tr>");

                        i = i + 1;
                        //rsProducto.movenext

                        #endregion

                            #endregion
                    }

                    rsProducto = null;
                }

                    #endregion

                #region Ventas.

                sbCuerpo.AppendLine("</table>");
                sbCuerpo.AppendLine("	</td>");
                sbCuerpo.AppendLine("</tr>");
                sbCuerpo.AppendLine("<tr>");
                sbCuerpo.AppendLine("	<td colspan=\"12\" bgcolor='#CCCCCC' class='TitDestacado' align=center><B>VENTAS</B></td>");
                sbCuerpo.AppendLine("</tr>");
                sbCuerpo.AppendLine("<tr>");
                sbCuerpo.AppendLine("<td class='TitDestacado' align=center>");
                sbCuerpo.AppendLine("<TABLE width='100%' align=center border='1' cellpadding='0' cellspacing='0'>");

                sbCuerpo.AppendLine("     		<tr class='Titulo'> ");
                sbCuerpo.AppendLine("       			<td colspan=12><b>Producto o Servicio</b></td>");
                sbCuerpo.AppendLine("     		</tr>");

                if (CodProyecto != 0)
                {
                    txtSQL = " select * from InterventorVentas " +
                             " where codproyecto = " + CodProyecto + " order by id_ventas ";
                    rsProducto = consultas.ObtenerDataTable(txtSQL, "text");
                    i = 0;
                    //txtProducto = "";

                    foreach (DataRow r_rsProducto in rsProducto.Rows)
                    {
                        sbCuerpo.AppendLine("              <tr align='left' valign='top'>");
                        sbCuerpo.AppendLine("                <td align='left' colspan=12>" + r_rsProducto["NomProducto"].ToString() + "</td>");
                        sbCuerpo.AppendLine("              </tr>");

                        TotalTipo1 = 0;
                        TotalTipo2 = 0;

                        TotalTipo1A = 0;
                        TotalTipo2A = 0;

                        for (int m = 1; m <= filas; m++)
                        {
                            #region Ventas.

                            z = m * 6 - 5;

                            #region Pinta los titulos de los meses.
                            sbCuerpo.AppendLine("              <tr>");

                            for (int k = z; k <= m * 6; k++)
                            {
                                if (k <= CONST_Meses)
                                {
                                    // 2014/11/20 Se comenta no hace NADA
                                    //if (k > 12) { }
                                    sbCuerpo.AppendLine("                <td align='center' colspan=2>Mes " + k + "</td>");
                                }
                            }
                            sbCuerpo.AppendLine("              </tr>");
                            #endregion

                            #region Pinta los titulos de Ventas/ingresos.
                            sbCuerpo.AppendLine("              <tr>");
                            for (int k = z; k <= m * 6; k++)
                            {
                                if (k <= CONST_Meses)
                                {
                                    sbCuerpo.AppendLine("                <td align='center'>Ventas</td><td align='center'>Ingreso</td>");
                                }
                            }
                            sbCuerpo.AppendLine("              </tr>");
                            #endregion

                            #region Datos registrados y aprobados.

                            sbCuerpo.AppendLine("              <tr align='left' valign='top'>");

                            //Datos registrados y aprobados.
                            //for (int k = z; i < m * 6; i++)//mal

                            // Traigo los datos 1 Sola vez
                            ImprimirPlanOperativoNegocio imPlanOperNeg = new ImprimirPlanOperativoNegocio();
                            List<InterventorProduccionEntity> lista = new List<InterventorProduccionEntity>();
                            lista = imPlanOperNeg.TraerInterventorVentasXMes(CodProyecto);

                            for (int k = z; k <= m * 6; k++)
                            {
                                if (k <= CONST_Meses)
                                {
                                    #region Tipo 1.

                                    //txtSQL = " select valor " +
                                    //         " from InterventorVentas,InterventorVentasMes " +
                                    //         " where id_ventas=codproducto and codProducto = " + r_rsProducto["Id_Ventas"].ToString() + " and codproyecto = " + CodProyecto +
                                    //         " and mes = " + k + " and tipo = 1 order by id_ventas, mes, tipo ";
                                    //rsProductos = consultas.ObtenerDataTable(txtSQL, "text");

                                    var vta1 = (from vta in lista
                                                where vta.CodProducto == Convert.ToInt32(r_rsProducto["Id_Ventas"])
                                                        && vta.Mes == k && vta.Tipo == 1
                                                select vta).FirstOrDefault();

                                    Tipo1 = "&nbsp;";
                                    //if (rsProductos.Rows.Count > 0)
                                    if (vta1 != null)
                                    {
                                        Tipo1 = vta1.Valor.ToString();
                                        TotalTipo1 += Double.Parse(vta1.Valor.ToString());
                                    }

                                    #endregion

                                    #region Tipo 2.

                                    //txtSQL = " select valor " +
                                    //         " from InterventorVentas,InterventorVentasMes " +
                                    //         " where id_ventas=codproducto and codProducto = " + r_rsProducto["Id_Ventas"].ToString() + " and codproyecto = " + CodProyecto +
                                    //         " and mes = " + k + " and tipo = 2 order by id_ventas, mes, tipo ";
                                    //rsProductos = consultas.ObtenerDataTable(txtSQL, "text");
                                    var vta2 = (from vta in lista
                                                where vta.CodProducto == Convert.ToInt32(r_rsProducto["Id_Ventas"])
                                                        && vta.Mes == k && vta.Tipo == 2
                                                select vta).FirstOrDefault();

                                    Tipo2 = "&nbsp;";
                                    if (vta2 != null)
                                    {
                                        Tipo2 = vta2.Valor.ToString();
                                        TotalTipo2 += Double.Parse(vta2.Valor.ToString());
                                    }

                                    #endregion

                                    if (Tipo1 == "&nbsp;")
                                    { sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>"); }
                                    else
                                    { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }

                                    if (Tipo2 == "&nbsp;")
                                    { sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>"); }
                                    else
                                    { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }
                                }
                            }

                            sbCuerpo.AppendLine("              </tr>");

                            #endregion

                            #region Datos avances reportados.
                            sbCuerpo.AppendLine("              <tr align='left' valign='top'>");

                            List<AvanceVentasPOMes> lstVtas = new List<AvanceVentasPOMes>();
                            lstVtas = imPlanOperNeg.TraerAvanceVentasPOMes(CodProyecto);

                            //for (int k = z; i < m * 6; i++)//mal
                            for (int k = z; k <= m * 6; k++)
                            {
                                if (k <= CONST_Meses)
                                {
                                    #region Tipo 1.
                                    //txtSQL = " select * " +
                                    //         " from AvanceVentasPOMes " +
                                    //         " where CodProducto = " + r_rsProducto["Id_ventas"].ToString() +
                                    //         " and mes = " + k + " and codtipofinanciacion = 1 ";
                                    //rsTipo1 = consultas.ObtenerDataTable(txtSQL, "text");
                                    var vta1 = (from vta in lstVtas
                                                where vta.CodProducto == Convert.ToInt32(r_rsProducto["Id_ventas"])
                                                && vta.Mes == k && vta.CodTipoFinanciacion == 1
                                                select vta).FirstOrDefault();

                                    if (vta1 != null)
                                    {
                                        Tipo1 = vta1.Valor.ToString();
                                        TotalTipo1A += Double.Parse(vta1.Valor.ToString());
                                    }
                                    else
                                    {
                                        Tipo1 = "&nbsp;";
                                    }
                                    #endregion

                                    #region Tipo 2.
                                    //txtSQL = " select * " +
                                    //         " from AvanceVentasPOMes " +
                                    //         " where CodProducto = " + r_rsProducto["Id_ventas"].ToString() +
                                    //         " and mes = " + k + " and codtipofinanciacion = 2 ";
                                    //rsTipo2 = consultas.ObtenerDataTable(txtSQL, "text");
                                    var vta2 = (from vta in lstVtas
                                                where vta.CodProducto == Convert.ToInt32(r_rsProducto["Id_ventas"])
                                                && vta.Mes == k && vta.CodTipoFinanciacion == 2
                                                select vta).FirstOrDefault();

                                    if (vta2 != null)
                                    {
                                        Tipo2 = vta2.Valor.ToString();
                                        TotalTipo2A += Double.Parse(vta2.Valor.ToString());
                                    }
                                    else
                                    {
                                        Tipo2 = "&nbsp;";
                                    }
                                    #endregion

                                    if (Tipo1 == "&nbsp;")
                                    { sbCuerpo.AppendLine("<td align='right'>" + Tipo1 + "</td>"); }
                                    else
                                    { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo1).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }

                                    if (Tipo2 == "&nbsp;")
                                    { sbCuerpo.AppendLine("<td align='right'>" + Tipo2 + "</td>"); }
                                    else
                                    { sbCuerpo.AppendLine("<td align='right'>" + Double.Parse(Tipo2).ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>"); }
                                }
                            }

                            sbCuerpo.AppendLine("              </tr>");
                            #endregion

                            #endregion
                        }

                        #region Líneas finales.

                        //Costo Total
                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td colspan=2>Costo Total</td>");
                        sbCuerpo.AppendLine("              </tr>");

                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("              </tr>");
                        //Costo Total avances reportados
                        sbCuerpo.AppendLine("              <tr>");
                        sbCuerpo.AppendLine("                 <td align='right'>" + TotalTipo1A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("                <td align='right'>" + TotalTipo2A.ToString("C2", System.Globalization.CultureInfo.CreateSpecificCulture("es-CO")) + "</td>");
                        sbCuerpo.AppendLine("              </tr>");

                        i = i + 1;
                        //rsProducto.movenext

                        #endregion
                    }

                    rsProducto = null;
                }

                lbl_cuerpo.Text = sbCuerpo.ToString();

                #endregion
            }
            catch (Exception ex)
            {
                sbCuerpo.AppendLine("<strong style='color:#CC0000;'>" + ex.Message + "</strong>");
                //lbl_cuerpo.Text = lbl_cuerpo.Text + "<strong style='color:#CC0000;'>" + ex.Message + "</strong>";
                lbl_cuerpo.Text = sbCuerpo.ToString();
            }
        }
    }
}