using Datos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace Fonade.FONADE.interventoria
{
    public partial class AgregarInformeFinalInterventoriaInter : Negocio.Base_Page
    {
        string InformeIdInformeFinal;
        string idEmpresa;

        string txtSQL;

        string txtNomEmpresa;
        string CodProyecto;

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
                recogerDatos();
                llenarTabla();

                comleeeGrid();

                anexos();
                L_titulo.Visible = true;
            //}
        }

        private void recogerDatos()
        {
            InformeIdInformeFinal = HttpContext.Current.Session["CodInforme"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodInforme"].ToString()) ? HttpContext.Current.Session["CodInforme"].ToString() : "0";
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
            else
            {  Response.Redirect("InformeConsolidadoInter.aspx");  }
        }

        private void llenarTabla()
        {
            var infoTanlaAux = new DataTable();

            txtSQL = "SELECT RazonSocial, Telefono, DomicilioEmpresa, NomCiudad FROM Empresa, Ciudad WHERE CodCiudad = id_ciudad and id_empresa= " + idEmpresa;
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                lblEmpresa.Text = infoTanlaAux.Rows[0]["RazonSocial"].ToString();
                lblTelefono.Text = infoTanlaAux.Rows[0]["Telefono"].ToString();
                lblDireccion.Text = infoTanlaAux.Rows[0]["DomicilioEmpresa"].ToString() + "-" + infoTanlaAux.Rows[0]["NomCiudad"].ToString();
                //lblCiudad.Text = infoTanlaAux.Rows[0]["NomCiudad"].ToString();
            }

            lblinforme.Text = "Informe Interventoría Final - " + infoTanlaAux.Rows[0]["RazonSocial"].ToString();

            if (usuario.CodGrupo == Constantes.CONST_CoordinadorInterventor || usuario.CodGrupo == Constantes.CONST_GerenteInterventor)
            {
                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (SELECT codinterventor FROM InterventorInformeFinal WHERE (id_InterventorInformeFinal =" + InformeIdInformeFinal + ")))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    L_TituloNombre.Text = "Interventor " + infoTanlaAux.Rows[0][0].ToString();

                txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre FROM contacto WHERE (id_contacto IN (Select CodCoordinador FROM Interventor WHERE (CodContacto IN (SELECT codinterventor FROM InterventorInformeFinal WHERE id_InterventorInformeFinal =" + InformeIdInformeFinal + "))))";

                infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

                if (infoTanlaAux.Rows.Count > 0)
                    lblnomcoordinador.Text = "Coordinador: " + infoTanlaAux.Rows[0][0].ToString();
            }

            txtSQL = "SELECT NumeroContrato FROM ContratoEmpresa WHERE CodEmpresa = " + idEmpresa;

            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
                lblnumContrato.Text = infoTanlaAux.Rows[0][0].ToString();

            txtSQL = "SELECT Id_InterventorInformeFinal, FechaInforme, Estado FROM InterventorInformeFinal WHERE  Id_InterventorInformeFinal= " + InformeIdInformeFinal;

            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
                lblfechainforme.Text = infoTanlaAux.Rows[0]["FechaInforme"].ToString();

            txtSQL = "SELECT Nombres +' '+ Apellidos as Nombre, Identificacion FROM Contacto WHERE (Id_Contacto IN (SELECT codcontacto FROM EmpresaContacto WHERE codempresa = " + idEmpresa + "))";
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            foreach (DataRow fila in infoTanlaAux.Rows)
            {
                TableRow filat = new TableRow();
                TableCell celdat = new TableCell();
                celdat.Text = "" + String.Format("<b>{0}</b>",fila["Nombre"].ToString()) + " Identificación: " + string.Format("<b>{0}</b>",fila["Identificacion"].ToString());
                filat.Cells.Add(celdat);
                t_table.Rows.Add(filat);
                t_table.DataBind();
            }
        }

        private void comleeeGrid()
        {
            var infoTanlaAux = new DataTable();

            txtSQL = "SELECT Id_InterventorInformeFinalCriterio, NomInterventorInformeFinalCriterio FROM InterventorInformeFinalCriterio WHERE CodEmpresa IN (0," + idEmpresa + ") ORDER BY CodEmpresa, Id_InterventorInformeFinalCriterio";
            infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

            if (infoTanlaAux.Rows.Count > 0)
            {
                foreach (DataRow fil in infoTanlaAux.Rows)
                {
                    TableHeaderRow filatablava = new TableHeaderRow();
                    filatablava.Cells.Add(crearceladtitulo(fil["NomInterventorInformeFinalCriterio"].ToString(), 3, 1, ""));
                    t_variable.Rows.Add(filatablava);

                    txtSQL = "SELECT Id_InterventorInformeFinalItem, NomInterventorInformeFinalItem, CodEmpresa FROM InterventorInformeFinalItem WHERE CodEmpresa IN (0," + idEmpresa + ") AND CodInformeFinalCriterio = " + fil["Id_InterventorInformeFinalCriterio"].ToString() + " ORDER BY CodEmpresa, Id_InterventorInformeFinalItem";
                    var laAux = consultas.ObtenerDataTable(txtSQL, "text");

                    if (laAux.Rows.Count > 0)
                    {
                        foreach (DataRow compl in laAux.Rows)
                        {
                            txtSQL = "SELECT Observaciones FROM InterventorInformeFinalCumplimiento WHERE CodInformeFinal = " + InformeIdInformeFinal + " AND CodInformeFinalItem = " + compl["Id_InterventorInformeFinalItem"].ToString() + " AND CodEmpresa = " + idEmpresa;

                            var laAux2 = consultas.ObtenerDataTable(txtSQL, "text");

                            string txtObservacion = "";
                            if (laAux2.Rows.Count > 0)
                            {
                                txtObservacion = laAux2.Rows[0]["Observaciones"].ToString();
                            }
                            TableRow filaNeA2 = new TableRow();

                            filaNeA2.Cells.Add(celdaNormal("", 1, 1, ""));
                            filaNeA2.Cells.Add(celdaNormal(compl["NomInterventorInformeFinalItem"].ToString(), 1, 1, ""));
                            filaNeA2.Cells.Add(celdaNormal(txtObservacion, 1, 1, ""));

                            t_variable.Rows.Add(filaNeA2);
                        }
                    }
                }
            }

            t_variable.DataBind();
        }

        private void anexos()
        {
            var infoTanlaAux = new DataTable();

            txtSQL = @"SELECT InterventorInformeFinalAnexos.Id_InterventorInformeFinalAnexos, " +
                     " InterventorInformeFinalAnexos.NomInterventorInformeFinalAnexos, InterventorInformeFinalAnexos.RutaArchivo, " +
                     " DocumentoFormato.Icono FROM InterventorInformeFinalAnexos INNER JOIN DocumentoFormato " +
                     " ON InterventorInformeFinalAnexos.CodDocumentoFormato = DocumentoFormato.Id_DocumentoFormato " +
                     " WHERE (InterventorInformeFinalAnexos.Borrado = 0) AND (InterventorInformeFinalAnexos.CodInformeFinal = " + InformeIdInformeFinal + ")";

             infoTanlaAux = consultas.ObtenerDataTable(txtSQL, "text");

             if (infoTanlaAux.Rows.Count > 0)
             {
                 foreach (DataRow fil in infoTanlaAux.Rows)
                 {
                     TableRow filaNeA2 = new TableRow();

                     filaNeA2.Cells.Add(celdaNormal((new Image {
                                                                ImageUrl = "~/Images/" + fil["Icono"].ToString()
                                                    }), 1, 1, ""));
                     filaNeA2.Cells.Add(celdaNormal((new HyperLink {
                                                                Text = "" + fil["NomInterventorInformeFinalAnexos"].ToString(),
                                                                Target="_blank",
                                                                NavigateUrl= ConfigurationManager.AppSettings.Get("RutaWebSite") + fil["RutaArchivo"].ToString() }),
                                                    1, 1, ""));

                     t_anexos.Rows.Add(filaNeA2);
                 }
                 t_anexos.DataBind();
             }
             else
             {
                 p_Anexos.Visible = false;
                 p_Anexos.Enabled = false;
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

        private TableCell celdaNormal(Control mensaje, Int32 colspan, Int32 rowspan, String cssestilo)
        {
            var celda1 = new TableCell { ColumnSpan = colspan, RowSpan = rowspan, CssClass = cssestilo };
            celda1.Controls.Add(mensaje);
            return celda1;
        }

        protected void btn_imprimir_Click(object sender, EventArgs e)
        {
            L_titulo.Visible = false;
        }

        //protected void btn_imprimir_Click(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Session["ImprimiridEmpresa"] = idEmpresa;
        //    HttpContext.Current.Session["ImprimirInformeIdInformeFinal"] = InformeIdInformeFinal;

        //    //Response.Redirect("ImprimirInformeInterventoriaFinal.aspx");
        //}
    }
}