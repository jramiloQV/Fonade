using Datos;
using Fonade.Account;
using Fonade.Negocio;
using Fonade.Negocio.Mensajes;
using Fonade.PlanDeNegocioV2.Formulacion.Utilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan
{
    public partial class ImpresionPlanCompras : System.Web.UI.UserControl
    {
        public int CodigoProyecto { get; set; }

        private int CantidadRegistros
        {
            get
            {
                return ViewState["CantidadRegistros"] != null
                    && !string.IsNullOrEmpty(ViewState["CantidadRegistros"].ToString())
                    ? int.Parse(ViewState["CantidadRegistros"].ToString()) : 0;
            }
            set { ViewState["CantidadRegistros"] = value; }
        }

        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (CodigoProyecto != 0)
                {
                    GenerarTablaInsumos();
                }
                else
                {
                    TableHeaderRow Headerfila = new TableHeaderRow();

                    Headerfila.Cells.Add(CeldaTitulo("Materia Prima, Insumo o Requerimiento"));
                    Headerfila.Cells.Add(CeldaTitulo("Unidad"));
                    Headerfila.Cells.Add(CeldaTitulo("Cantidad", 160));
                    Headerfila.Cells.Add(CeldaTitulo("Presentacion", 160));
                    Headerfila.Cells.Add(CeldaTitulo("Margen de Desperdicio (%)", 160));

                    tbl.Rows.Add(Headerfila);

                    TableHeaderRow fila = new TableHeaderRow();
                    TableCell celda = new TableCell();
                    celda.ColumnSpan = 5;
                    celda.Text = Mensajes.GetMensaje(101);

                    fila.Cells.Add(celda);
                    
                    tbl.Rows.Add(fila);
                }
            }
            catch (Exception ex)
            {
                Utilidades.PresentarMsj(Mensajes.GetMensaje(7), this, "Alert");
            }
        }

        private void GenerarTablaInsumos()
        {
            using (Datos.FonadeDBDataContext consultas = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                if (CodigoProyecto != 0)
                {
                    var proyectoProducto = (from pp in consultas.ProyectoProductos
                                            orderby pp.NomProducto
                                            where pp.CodProyecto == CodigoProyecto
                                            select new
                                            {
                                                pp.Id_Producto,
                                                pp.NomProducto,
                                                pp.UnidadMedida
                                            });

                    foreach (var pp in proyectoProducto)
                    {

                        string codT_Insumo = pp.Id_Producto.ToString();
                        TableRow fila = new TableRow();

                        #region adicionar insumo

                        fila = new TableRow();
                        fila.Cells.Add(celda(pp.NomProducto + " - " + pp.UnidadMedida, 6));
                        fila.Attributes.Add("class", "tituloCelda");
                        tbl.Rows.Add(fila);


                        #endregion

                        TableHeaderRow Headerfila = new TableHeaderRow();

                        Headerfila.Cells.Add(CeldaTitulo("Materia Prima, Insumo o Requerimiento"));
                        Headerfila.Cells.Add(CeldaTitulo("Unidad"));
                        Headerfila.Cells.Add(CeldaTitulo("Cantidad", 160));
                        Headerfila.Cells.Add(CeldaTitulo("Presentacion", 160));
                        Headerfila.Cells.Add(CeldaTitulo("Margen de Desperdicio (%)", 160));

                        tbl.Rows.Add(Headerfila);

                        var ProyectoProductoYTipoInsumo = (from ppi in consultas.ProyectoProductoInsumos
                                                           from pi in consultas.ProyectoInsumos
                                                           from pti in consultas.TipoInsumos
                                                           orderby pti.NomTipoInsumo, pi.nomInsumo
                                                           where pi.codTipoInsumo == pti.Id_TipoInsumo
                                                           && ppi.CodInsumo == pi.Id_Insumo
                                                           && ppi.CodProducto == int.Parse(codT_Insumo)
                                                           select new
                                                           {
                                                               ppi,
                                                               pi.Id_Insumo,
                                                               pi.Unidad,
                                                               pi.nomInsumo,
                                                               pti
                                                           });

                        CantidadRegistros = ProyectoProductoYTipoInsumo.Count();

                        byte tipoInsumo = 0;

                        foreach (var ppti in ProyectoProductoYTipoInsumo)
                        {
                            if (tipoInsumo != ppti.pti.Id_TipoInsumo)
                            {
                                tipoInsumo = ppti.pti.Id_TipoInsumo;
                                fila = new TableRow();
                                fila.Cells.Add(celda(ppti.pti.NomTipoInsumo, 5));
                                fila.Attributes.Add("class", "tituloCelda");
                                tbl.Rows.Add(fila);
                            }
                            fila = new TableRow();
                            fila.Cells.Add(celda(ppti.nomInsumo));
                            fila.Cells.Add(celda(ppti.Unidad));
                            var cant = Convert.ToDecimal(ppti.ppi.Cantidad);
                            fila.Cells.Add(celda(cant.ToString()));
                            fila.Attributes.Add("class", "alineacion");
                            fila.Cells.Add(celda(ppti.ppi.Presentacion));
                            fila.Attributes.Add("class", "alineacion");
                            fila.Cells.Add(celda(ppti.ppi.Desperdicio.ToString()));
                            fila.Attributes.Add("class", "alineacion");


                            tbl.Rows.Add(fila);
                        }

                        TableRow filaesp = new TableRow();
                        tbl.Rows.Add(filaesp);
                    }
                }
            }
        }

        /// <summary>
        /// devuelve un TableCell (<td></td>)
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        private TableCell celda(string texto = "", int colspan = 1, int rowspan = 1, Control control = null, int width = -1)
        {
            TableCell celda = new TableCell();

            if (control != null)
                celda.Controls.Add(control);

            if (!string.IsNullOrEmpty(texto))
                celda.Text = texto;

            if (width != -1)
                celda.Width = width;

            celda.ColumnSpan = colspan;
            celda.RowSpan = rowspan;

            return celda;
        }

        /// <summary>
        /// devuelve un TableHeaderCell (<th></th>)
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        private TableHeaderCell CeldaTitulo(string texto = "", int width = -1)
        {
            TableHeaderCell HeaderCelda = new TableHeaderCell();

            var textoSplit = texto.Split(',');

            if (textoSplit.Count() == 0)
            {
                if (!string.IsNullOrEmpty(texto))
                    HeaderCelda.Text = texto;

                if (width != -1)
                    HeaderCelda.Width = width;
            }
            else
            {
                if (!string.IsNullOrEmpty(textoSplit[0]))
                    HeaderCelda.ColumnSpan = 1;
                HeaderCelda.Text = textoSplit[0];

                if (width != -1)
                    HeaderCelda.Width = width;
            }

            return HeaderCelda;
        }

    }
}