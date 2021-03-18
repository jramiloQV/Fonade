﻿using Datos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo
{
    public partial class Ventas : Negocio.Base_Page
    {
        public int CodigoProyecto { get { return Convert.ToInt32(Request.QueryString["codproyecto"]); } set { } }
        public int CodigoConvocatoria
        {
            get
            {
                return Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(CodigoProyecto, HttpContext.Current.Session["HistorialEvaluacion"] != null ? Convert.ToInt32(HttpContext.Current.Session["HistorialEvaluacion"]) : 0).GetValueOrDefault();
            }
            set { }
        }
        public int CodigoTab { get { return Constantes.Const_ventasV2; } set { } }
        private ProyectoMercadoProyeccionVenta pm;
        public Boolean esMiembro;
        /// <summary>
        /// Determina si "está" o "no" realizado...
        /// </summary>
        public Boolean bRealizado;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                EncabezadoEval.IdProyecto = CodigoProyecto;
                EncabezadoEval.IdConvocatoria = CodigoConvocatoria;
                EncabezadoEval.IdTabEvaluacion = CodigoTab;
                                
                esMiembro = fnMiembroProyecto(usuario.IdContacto, CodigoProyecto.ToString());

                bRealizado = esRealizado(CodigoTab, CodigoProyecto, CodigoConvocatoria);

                if (esMiembro && !bRealizado) { this.div_Post_It2.Visible = true; Post_It1._mostrarPost = true; }

                if (!IsPostBack)
                {
                    llenarGrid();
                    UpdateTab();
                    frameDerecho();
                }
            }
            catch (Exception) { }
        }

        protected void I_AyudaProVentas_Click(object sender, ImageClickEventArgs e)
        {
            HttpContext.Current.Session["mensaje"] = "6";
            ClientScriptManager cm = this.ClientScript;
            cm.RegisterClientScriptBlock(this.GetType(), "", "<script type='text/javascript'>open('../Ayuda/Mensaje.aspx', 'Ventas', 'width=500,height=400');</script>");
        }

        private void llenarGrid()
        {
            DataTable datatable = new DataTable();

            datatable.Columns.Add("Id_Producto");
            datatable.Columns.Add("NomProducto");

            String sql;
            sql = "SELECT [Id_Producto], [NomProducto] FROM [ProyectoProducto] WHERE [CodProyecto] = " + CodigoProyecto + " ORDER BY [Id_Producto]";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    DataRow fila = datatable.NewRow();
                    fila["Id_Producto"] = reader["Id_Producto"].ToString();
                    fila["NomProducto"] = reader["NomProducto"].ToString();
                    datatable.Rows.Add(fila);
                }
                GV_ProyectoProducto.DataSource = datatable;
                GV_ProyectoProducto.DataBind();
                reader.Close();
            }
            catch (SqlException se)
            {
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private void frameDerecho()
        {
            DataTable[] datatable = new DataTable[12];
            DataTable costoTotal = new DataTable();

            for (int i = 0; i < 12; i++)
            {
                datatable[i] = new DataTable();

                datatable[i].Columns.Add("Id_Producto");
                datatable[i].Columns.Add("Unidades");
                datatable[i].Columns.Add("PRECIO");
            }

            costoTotal.Columns.Add("Id_Producto");
            costoTotal.Columns.Add("Unidades");
            costoTotal.Columns.Add("PRECIO");

            String sql;
            sql = @"SELECT [Id_Producto], [Unidades], [Mes], [Precio]*unidades as precio
                    FROM [ProyectoProducto]
                    LEFT OUTER JOIN [ProyectoProductoUnidadesVentas] AS U ON [Id_Producto] = U.[CodProducto]
                    LEFT OUTER JOIN [ProyectoProductoPrecio] AS P ON [Id_Producto] = P.[CodProducto]
                    WHERE [Periodo] = 1 AND [Ano] = 1 AND [CodProyecto] = " + CodigoProyecto + @"
                    ORDER BY [Id_Producto], [Mes]";


            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString());
            SqlCommand cmd = new SqlCommand(sql, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                int unidades = 0;
                Decimal precio = 0;

                while (reader.Read())
                {
                    DataRow fila = datatable[Int32.Parse(reader["Mes"].ToString()) - 1].NewRow();

                    fila["Id_Producto"] = reader["Id_Producto"].ToString();
                    fila["Unidades"] = reader["Unidades"].ToString();
                    fila["PRECIO"] = Convert.ToDecimal(reader["Precio"]).ToString("0,0.00", CultureInfo.InvariantCulture);

                    datatable[Int32.Parse(reader["Mes"].ToString()) - 1].Rows.Add(fila);

                    unidades += Int32.Parse(reader["Unidades"].ToString());
                    precio += Convert.ToDecimal(reader["PRECIO"]);

                    if (Int32.Parse(reader["Mes"].ToString()) == 12)
                    {
                        DataRow filatotal = costoTotal.NewRow();
                        filatotal["Id_Producto"] = reader["Id_Producto"].ToString();
                        filatotal["Unidades"] = unidades;
                        filatotal["Precio"] = precio.ToString("0,0.00", CultureInfo.InvariantCulture);
                        costoTotal.Rows.Add(filatotal);

                        unidades = 0;
                        precio = 0;
                    }
                }

                for (int i = 0; i < 12; i++)
                {
                    String objeto = "GV_Mes" + (i + 1);
                    GridView gridview = (GridView)this.FindControl(objeto);
                    gridview.DataSource = datatable[i];
                    gridview.DataBind();
                }

                GV_costoTotal.DataSource = costoTotal;
                GV_costoTotal.DataBind();

                reader.Close();
            }
            catch (SqlException) { }
            catch (NullReferenceException) { }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        private void UpdateTab()
        {
            TabEvaluacionProyecto tabEvaluacion = new TabEvaluacionProyecto()
            {
                CodProyecto = CodigoProyecto,
                CodConvocatoria = CodigoConvocatoria,
                CodTabEvaluacion = (Int16)CodigoTab,
                CodContacto = usuario.IdContacto,
                FechaModificacion = DateTime.Now,
                Realizado = false
            };

            string messageResult;
            Negocio.PlanDeNegocioV2.Utilidad.TabEvaluacion.SetUltimaActualizacion(tabEvaluacion, out messageResult);            
            EncabezadoEval.GetUltimaActualizacion();
        }

    }
}