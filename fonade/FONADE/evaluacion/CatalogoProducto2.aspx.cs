using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Datos;
using System.Data;
using System.Data.SqlClient;
using Fonade.Negocio;
using Newtonsoft.Json.Linq;

namespace Fonade.FONADE.evaluacion
{
    public partial class CatalogoProducto2 : Negocio.Base_Page // System.Web.UI.Page
    {
        Consultas consultas = new Consultas();

        int idProyecto, anios, periodo,filas,idProducto;
        string txtGrv, accion;
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            idProyecto = int.Parse(Session["codProyecto"].ToString());
            idProducto = (Request.QueryString["Id_Producto"] != null) ? int.Parse(Request.QueryString["Id_Producto"]) : 0;
            if(!Page.IsPostBack)
            {
                CrearGrid();
            }
        }

        protected void btnGrabar_Click(object sender, EventArgs e)
        {
            SalvarRegistro();
        }
        #endregion

        #region Metodos
        private void CrearGrid()
        {
            var prjProyecVentas = (from pv in consultas.Db.ProyectoMercadoProyeccionVentas
                                   where pv.CodProyecto == idProyecto
                                   select pv).FirstOrDefault();
            switch(prjProyecVentas.CodPeriodo)
            {
                case Constantes.CONST_Bimestre:
                    filas = 6 + 1;
                    txtGrv = "Bimestre ";
                    break;
                case Constantes.CONST_Trimestre:
                    filas = 4 + 1;
                    txtGrv = "Trimestre ";
                    break;
                case Constantes.CONST_Semestre:
                    filas = 2 + 1;
                    txtGrv = "Semestre ";
                    break;
                case Constantes.CONST_Mes:
                    filas = 12 + 1;
                    txtGrv = "Mes ";
                    break;
            }

            tblProyecionVentas.Style["align-text"] = "right";

            var rowHeade = new TableRow();
            for(var i = 0; i<= prjProyecVentas.TiempoProyeccion; i++)
            {
                var cell = new TableCell();
                if(i == 0)
                {
                    cell.Text = "PERIODOS";
                }
                else
                {
                    cell.Text = "Año " + i;
                }
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.Font.Bold = true;
                cell.Font.Size = 10;
                cell.ForeColor = System.Drawing.Color.Black;
                rowHeade.Cells.Add(cell);
                rowHeade.Width = 50;
                rowHeade.TableSection = TableRowSection.TableHeader;
            }
            tblProyecionVentas.Rows.Add(rowHeade);

            for(var i = 0; i<= filas; i++)
            {
                var tblRow = new TableRow();
                for (var j = 0; j <= prjProyecVentas.TiempoProyeccion; j++)
                {
                    var cell = new TableCell();
                    if(j==0)
                    {
                        var lbl = new Label();
                        if(i >= 0 && i <= (filas -2))
                        {
                            {
                                lbl.Text = "Cant. " + txtGrv + (i + 1);
                                lbl.Font.Bold = true;
                                lbl.Font.Italic = true;
                            };
                        }
                        else
                        {
                            if(i == (filas -1))
                            {
                                lbl.Text = "Precio";
                                lbl.Font.Bold = true;
                                lbl.ForeColor = System.Drawing.Color.Black;
                                lbl.Font.Size = 10;
                            }

                            if(i == filas)
                            {
                                lbl.Text = "Ventas Esperadas";
                                lbl.Font.Bold = true;
                                lbl.ForeColor = System.Drawing.Color.Black;
                                lbl.Font.Size = 10;
                            }
                            cell.CssClass = "AlineadoDerecha";
                        }
                        cell.Controls.Add(lbl);
                        
                    }
                    else
                    {
                        if(i != filas)
                        {
                            var txt = new TextBox();
                            if(i != (filas -1))
                            {
                                
                                txt.ID = "txt_Anio" + (j) + "_Periodo" + (i + 1);
                                txt.Width = 80;
                                txt.Text = "0";
                                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                txt.CssClass = "txt AlineadoDerecha ";
                            }
                            else
                            {
                                txt.ID = "txtPrecioAnio" + (j);
                                txt.Width = 80;
                                txt.Text = "0";
                                txt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                                txt.CssClass = "txtTol AlineadoDerecha ";
                            }
                            
                            cell.Controls.Add(txt);
                        }
                        else
                        {
                            var lblt = new Label();
                            lblt.ID = "lblAnio" + j;
                            lblt.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                            lblt.CssClass = "AlineadoDerecha";
                            lblt.Font.Size = 9;
                            lblt.Font.Bold = true;
                            lblt.ForeColor = System.Drawing.Color.Black;
                            lblt.Text = "0";
                            cell.Controls.Add(lblt);
                            cell.CssClass = "totalCol";
                        };
                    }
                    tblRow.Cells.Add(cell);
                    if(i == filas)
                    {
                        tblRow.TableSection = TableRowSection.TableFooter;
                    }
                }
                tblProyecionVentas.Rows.Add(tblRow);
            }

            if(idProducto > 0)
            {
                var producto = (from p in consultas.Db.ProyectoProductos
                    where p.Id_Producto == idProducto
                    select p).FirstOrDefault();
                NombreProductoServicio.Value = producto.NomProducto;
                PrecioLanzamiento.Value = Convert.ToInt32(producto.PrecioLanzamiento).ToString();
                Iva.Value = producto.PorcentajeIva.ToString();
                Retencion.Value = producto.PorcentajeRetencion.ToString();
                VentasCredito.Value = producto.PorcentajeVentasPlazo.ToString();
                var txtSql = "Select distinct PosicionArancelaria + ' '+ Descripcion Descripcion from PosicionArancelaria ";
                txtSql += "where LTRIM(RTRIM(PosicionArancelaria)) = '"+ producto.PosicionArancelaria +"'";
                var dt = consultas.ObtenerDataTable(txtSql, "text");
                txtArancelPosicion.Text = dt.Rows.Count > 0 ? dt.Rows[0].ItemArray[0].ToString() : string.Empty;

                btnGrabar.Text = "Actualizar";
                var lstppu = ObtenerDatosMeses(); //Carga datos meses

                var cout = 1;
                var periodo = 1;
                foreach (var p in lstppu)
                {
                    var nombre = "txt_Anio" + p.Ano + "_Periodo" + periodo;
                    var txt = (TextBox)tblProyecionVentas.FindControl(nombre);
                    if (txt != null)
                    {
                        txt.Text = decimal.Parse(p.Unidades.ToString()).ToString().Replace(',','.');
                    }
                    if (cout == prjProyecVentas.TiempoProyeccion)
                    {
                        cout = 1;
                        periodo++;
                    }
                    else
                    {
                        cout++;
                    }
                }

                var ppp = ObtenerDatosPrecio();  // CArga datos precio

                foreach (var p in ppp)
                {
                    var nombre = "txtPrecioAnio" + p.Periodo;
                    var txt = (TextBox)tblProyecionVentas.FindControl(nombre);
                    if (txt != null)
                    {
                        txt.Text = p.Precio.Trim().Replace(',','.');
                    }
                }


                //Calcula totales
                foreach (var p in ppp)
                {
                    var suma =  (from pu in lstppu
                                where pu.Ano == p.Periodo
                                select (decimal?)pu.Unidades).Sum() ?? 0;

                    var total = suma * decimal.Parse(p.Precio.Trim());
                    var nombre = "lblAnio" + p.Periodo;
                    var lbl = (Label)tblProyecionVentas.FindControl(nombre);
                    if (lbl != null)
                    {
                        lbl.Text = total.ToString();
                    }
                }
            }
            else
            {
                btnGrabar.Text = "Crear";
            }
            
        }

        private IList<Datos.ProyectoProductoUnidadesVenta> ObtenerDatosMeses()
        {
            var list = (from pp in consultas.Db.ProyectoProductoUnidadesVentas
                        where pp.CodProducto == idProducto
                        orderby pp.Mes
                        select pp).ToList();

            return list;
        }

        private IList<Datos.ProyectoProductoPrecio> ObtenerDatosPrecio()
        {
            var list = (from ppp in consultas.Db.ProyectoProductoPrecios 
                        where ppp.CodProducto == idProducto
                        orderby ppp.Periodo
                        select ppp).ToList();
            return list;
        }

        private void SalvarRegistro()
        {
            var prjProyecVentas = (from pv in consultas.Db.ProyectoMercadoProyeccionVentas
                                   where pv.CodProyecto == idProyecto
                                   select pv).FirstOrDefault();

            switch (prjProyecVentas.CodPeriodo)
            {
                case Constantes.CONST_Bimestre:
                    filas = 6;
                    txtGrv = "Bimestre ";
                    break;
                case Constantes.CONST_Trimestre:
                    filas = 4;
                    txtGrv = "Trimestre ";
                    break;
                case Constantes.CONST_Semestre:
                    filas = 2;
                    txtGrv = "Semestre ";
                    break;
                case Constantes.CONST_Mes:
                    filas = 12;
                    txtGrv = "Mes ";
                    break;
            }

            var posicion = txtArancelPosicion.Text.Split(' ');
            var proyectoProducto = new ProyectoProducto
            {
                Id_Producto = idProducto,
                CodProyecto = idProyecto,
                NomProducto = NombreProductoServicio.Value.Trim(),
                PorcentajeIva = int.Parse(Iva.Value.Trim()),
                PorcentajeRetencion = double.Parse(Retencion.Value.Trim()),
                PorcentajeVentasPlazo = int.Parse(VentasCredito.Value.Trim()),
                PorcentajeVentasContado = (100 - int.Parse(VentasCredito.Value.Trim())),
                PosicionArancelaria = posicion[0].Trim(),
                PrecioLanzamiento = decimal.Parse(PrecioLanzamiento.Value.Trim())
            };

            GrabarProducto(proyectoProducto);

            //Inserta las unidades de ventas x periodo x año
            var txtSql = "delete from proyectoproductounidadesventas where CodProducto= " + idProducto;
            ejecutaReader(txtSql, 2);

            for (var i = 0; i <= filas; i++) // periodo
            {

                for (var j = 0; j <= prjProyecVentas.TiempoProyeccion; j++) // años
                {
                    var nombre = "txt_Anio" + (j + 1) + "_Periodo" + (i + 1);
                    var unidades = Request.Form.Get(nombre); // (Label)tblProyecionVentas.FindControl(nombre);
                    if (unidades != null)
                    {
                        GrabarProductoProyeccion(unidades, (i + 1).ToString(), (j + 1).ToString());
                    }
                }
            }

            //Insertar los precios de venta del producto por año
            txtSql = "delete from ProyectoProductoPrecio where CodProducto=" + idProducto;
            ejecutaReader(txtSql, 2);

            for (var i = 0; i <= prjProyecVentas.TiempoProyeccion; i++)
            {
                var nombre = "txtPrecioAnio" + (i + 1);
                var precioAnio = Request.Form.Get(nombre); // (TextBox)tblProyecionVentas.FindControl(nombre);
                if (precioAnio != null)
                {
                    GrabarPrecioProductoAnio((i + 1).ToString(), precioAnio);
                }
            }

            //Retonarmos a la pagina anterior
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "retorno", "location.href = '../Proyecto/PProyectoMercadoProyecciones.aspx'", true);
        }

        private void GrabarProducto(ProyectoProducto obj)
        {
            if (obj.Id_Producto == 0)
            {
                var producto = (from p in consultas.Db.ProyectoProductos
                                where p.NomProducto == obj.NomProducto && p.CodProyecto == obj.CodProyecto
                                select p).FirstOrDefault();
                if (producto == null)
                {
                    consultas.Db.ProyectoProductos.InsertOnSubmit(obj);
                    consultas.Db.SubmitChanges();
                    idProducto = obj.Id_Producto;
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Ya existe un producto registrado con el nombre de " + obj.NomProducto + ".')", true);
                    return;
                }
            }
            else
            {
                var producto = (from p in consultas.Db.ProyectoProductos
                                where p.Id_Producto == obj.Id_Producto
                                select p).FirstOrDefault();
                producto.NomProducto = obj.NomProducto;
                producto.PorcentajeIva = obj.PorcentajeIva;
                producto.PorcentajeRetencion = obj.PorcentajeRetencion;
                producto.PorcentajeVentasContado = obj.PorcentajeVentasContado;
                producto.PorcentajeVentasPlazo = obj.PorcentajeVentasPlazo;
                producto.PosicionArancelaria = obj.PosicionArancelaria;
                producto.PrecioLanzamiento = obj.PrecioLanzamiento;
                consultas.Db.SubmitChanges();
            }
        }

        private void GrabarProductoProyeccion(string unidades, string mes, string anio)
        {
            var txtSql = "Insert proyectoproductounidadesventas Values(" + idProducto + "," + unidades + "," + mes +
                             "," + anio + ")";
            ejecutaReader(txtSql, 2);
        }

        private void GrabarPrecioProductoAnio(string periodo, string precio)
        {
            var txtSql = "INSERT INTO ProyectoProductoPrecio Values(" + idProducto + "," + periodo + "," + precio + ")";
            ejecutaReader(txtSql, 2);
        }

        #endregion

        #region WebMethods

        [WebMethod]
        public static List<string> BuscarPosicionAlancelaria(string criterio)
        {
            var lstNames = new List<string>();
            var txtSql = "Exec GetPosicionArancelaria '" + criterio + "'";
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            var cmdDa = new SqlDataAdapter(txtSql, con);
            var dt = new DataTable();
            cmdDa.Fill(dt);

            lstNames.AddRange(from DataRow row in dt.Rows select Convert.ToString(row["Descripcion"]));

            return lstNames;
        }
        #endregion
    }


}