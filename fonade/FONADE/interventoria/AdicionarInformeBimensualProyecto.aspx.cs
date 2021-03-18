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
    public partial class AdicionarInformeBimensualProyecto : Negocio.Base_Page
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

            if (!IsPostBack)
            {
                if (CodEmpresa != "0" && CodInforme != "0")
                    CargarDatosInforme();
            }
        }

        /// <summary>
        /// Mauricio Arias Olave.
        /// 17/09/2014.
        /// Mostrar la información del informe seleccionado en "InformeBimensualProyecto.aspx".
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
                txtSQL = "SELECT Estado FROM InformeBimensual WHERE id_InformeBimensual = " + CodInforme;
                RSEstado = consultas.ObtenerDataTable(txtSQL, "text");

                if (RSEstado.Rows.Count > 0)
                { Estado = RSEstado.Rows[0]["Estado"].ToString(); }

                //Nombre del interventor.
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InformeBimensual WHERE (id_InformeBimensual =" + CodInforme + ")))";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreInterventor.Text = "Interventor " + RS.Rows[0]["Nombre"].ToString(); }

                //Coordinador Interventoría.
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformeBimensual WHERE id_InformeBimensual =" + CodInforme + "))))";
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NombreCoordinador.Text = RS.Rows[0]["Nombre"].ToString(); }

                #region Periodo.

                if (CodInforme != "0")
                {
                    txtSQL = "SELECT * FROM InformeBimensual WHERE id_InformeBimensual = " + CodInforme;
                    RSPeriodo = consultas.ObtenerDataTable(txtSQL, "text");

                    if (RSPeriodo.Rows.Count > 0)
                    {
                        switch (RSPeriodo.Rows[0]["Periodo"].ToString())
                        {
                            case "1":
                                lblPeriodo.Text = "Enero-Febrero 01";
                                break;
                            case "2":
                                lblPeriodo.Text = "Marzo-Abril 01";
                                break;
                            case "3":
                                lblPeriodo.Text = "Mayo-Junio 01";
                                break;
                            case "4":
                                lblPeriodo.Text = "Julio-Agosto 01";
                                break;
                            case "5":
                                lblPeriodo.Text = "Septiembre-Octubre 01";
                                break;
                            case "6":
                                lblPeriodo.Text = "Noviembre-Diciembre 01";
                                break;
                            case "7":
                                lblPeriodo.Text = "Enero-Febrero 02";
                                break;
                            case "8":
                                lblPeriodo.Text = "Marzo-Abril 02";
                                break;
                            case "9":
                                lblPeriodo.Text = "Mayo-Junio 02";
                                break;
                            case "10":
                                lblPeriodo.Text = "Julio-Agosto 02";
                                break;
                            case "11":
                                lblPeriodo.Text = "Septiembre-Octubre 02";
                                break;
                            case "12":
                                lblPeriodo.Text = "Noviembre-Diciembre 02";
                                break;
                            case "13":
                                lblPeriodo.Text = "Enero-Febrero 03";
                                break;
                            case "14":
                                lblPeriodo.Text = "Marzo-Abril 03";
                                break;
                            case "15":
                                lblPeriodo.Text = "Mayo-Junio 03";
                                break;
                            case "16":
                                lblPeriodo.Text = "Julio-Agosto 03";
                                break;
                            case "17":
                                lblPeriodo.Text = "Septiembre-Octubre 03";
                                break;
                            case "18":
                                lblPeriodo.Text = "Noviembre-Diciembre 03";
                                break;
                            default:
                                break;
                        }
                    }
                }

                #endregion

                //Número del contrato.
                txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { NumeroContrato.Text = RS.Rows[0]["NumeroContrato"].ToString(); }

                //Fecha: 
                lblFecha.Text = DateTime.Today.ToString("dd/MM/yyyy");

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

                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + CodEmpresa + "))"; //idEmpresa
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
        /// Imprimir informe bimensual "llamado a pantalla que contiene los datos".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            //Nueva ventana emergente.
            HttpContext.Current.Session["idInformeBimensual"] = CodInforme;
            HttpContext.Current.Session["CodEmpresa"] = CodEmpresa;
            //Redirect(null, "ImprimirInformeInterventoriaBimensual.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
            Redirect(null, "ImprimirInformeInterventoriaBimensual.aspx", "_blank", "width=640,height=480,scrollbars=yes,resizable=no");
        }
    }
}