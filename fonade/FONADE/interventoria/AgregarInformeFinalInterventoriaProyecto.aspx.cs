using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Fonade.FONADE.interventoria
{
    public partial class AgregarInformeFinalInterventoriaProyecto : Negocio.Base_Page
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
                    //guardaBimestrl();
                    //comleeeGrid();
                }
            }
        }

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

                if (RS.Rows.Count > 0) { lblinforme.Text = "Interventor: " + RS.Rows[0]["Nombre"].ToString(); }

                //Coordinador Interventoría.
                //txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InformePresupuestal WHERE id_InformePresupuestal =" + CodInforme + "))))";
                //RS = consultas.ObtenerDataTable(txtSQL, "text");

                //if (RS.Rows.Count > 0) { NombreCoordinador.Text = RS.Rows[0]["Nombre"].ToString(); }

                //Número del contrato.
                txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0) { lblnumContrato.Text = RS.Rows[0]["NumeroContrato"].ToString(); }

                //Varios datos de la empresa.
                txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + CodEmpresa;
                RS = consultas.ObtenerDataTable(txtSQL, "text");

                if (RS.Rows.Count > 0)
                {
                    lblEmpresa.Text = RS.Rows[0]["RazonSocial"].ToString();
                    lblTelefono.Text = RS.Rows[0]["Telefono"].ToString();
                    lblDireccion.Text = RS.Rows[0]["DomicilioEmpresa"].ToString();
                    //lblCiudad.Text = RS.Rows[0]["NomCiudad"].ToString();
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
    }
}