using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Datos;
using System.Text.RegularExpressions;
using Fonade.Clases;
using Fonade.Negocio.Proyecto;

namespace Fonade.FONADE.interventoria
{
    public partial class CatalogoDocumentoPagos : Negocio.Base_Page
    {
        //string CodPagoActividad;
        string txtSQL;
           
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) {                
                if (Session["CodPagoActividad"] != null)
                {
                    hdCodigoPago.Value = Session["CodPagoActividad"].ToString();
                }
                else
                {
                    hdCodigoPago.Value = "0";
                }
            }
                        
            //CodPagoActividad = HttpContext.Current.Session["CodPagoActividad"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodPagoActividad"].ToString()) ? HttpContext.Current.Session["CodPagoActividad"].ToString() : "0";
            var accion = Request.QueryString["Accion"].ToString();
            

            if (!Page.IsPostBack)
            {
                LlenarGrilla(hdCodigoPago.Value);
                LlenarCombo();
            }

            if(accion == "Nuevo")
            {
                panelEditar.Visible = true;
                panelEditar.Enabled = true;
                lblTitulo.Text = "NUEVO DOCUMENTO";
                contPri.Visible = false;
                btnActualizar.Text = "Crear";
            }

            if (accion == "Lista")
            {
                panelEditar.Visible = false;
                panelEditar.Enabled = false;
                lblTitulo.Text = "EDITAR DOCUMENTO";
                contPri.Visible = true;
                btnActualizar.Text = "Actualizar";
            }

            if(usuario.CodGrupo != 6)
            {
                lblSubir.Visible = false;
                fArchivo.Visible = false;
            }
        }

        private void LlenarCombo()
        {
            var sql = "SELECT Id_PagoTipoArchivo, NomPagoTipoArchivo FROM PagoTipoArchivo";
            var result = consultas.ObtenerDataTable(sql, "text");
            ddltipodocumento.DataSource = result;
            ddltipodocumento.DataValueField = "Id_PagoTipoArchivo";
            ddltipodocumento.DataTextField = "NomPagoTipoArchivo";
            ddltipodocumento.DataBind();
            ddltipodocumento.Items.Insert(0, new ListItem("Seleccione", "0"));
        }
        private void LlenarGrilla(string CodPagoActividad)
        {
            

            if (!string.IsNullOrEmpty(CodPagoActividad))
            {
                txtSQL = "SELECT PagoActividadarchivo.*, PagoActividad.Estado  FROM PagoActividadarchivo INNER JOIN PagoActividad ON PagoActividadarchivo.CodPagoActividad = PagoActividad.Id_PagoActividad  WHERE (PagoActividadarchivo.CodPagoActividad = " + CodPagoActividad + ")";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if(dt.Rows.Count > 0)
                {
                    gvpresupuesto.DataSource = dt;
                    gvpresupuesto.DataBind();
                }
                else
                {
                    contPri.Visible = false;
                }
                
            }
        }
        
        protected void gvpresupuesto_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("VerArchivo"))
            {
                if (e.CommandArgument != null)
                {
                    string[] parametros;
                    parametros = e.CommandArgument.ToString().Split(';');

                    var nombreArchivo = parametros[0];
                    var urlArchivo = parametros[1];

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + nombreArchivo);
                    Response.TransmitFile(urlArchivo);
                    Response.End();
                }
            }

            if (e.CommandName.Equals("DocEditar"))
            {
                panelEditar.Visible = true;
                panelEditar.Enabled = true;

                contPri.Visible = false;
                contPri.Enabled = false;

                string[] pram = e.CommandArgument.ToString().Split(';');

                

                ActualizaDoc(pram);
            }
            if(e.CommandName.Equals("ElimarArchivo"))
            {
                var idPagoActividadArchivo = e.CommandArgument.ToString();
                var obj = (from pad in consultas.Db.PagoActividadarchivo
                           where pad.Id_PagoActividadArchivo == Convert.ToInt32(idPagoActividadArchivo)                               
                           select pad).FirstOrDefault();
                consultas.Db.PagoActividadarchivo.DeleteOnSubmit(obj);
                consultas.Db.SubmitChanges();

                Response.Redirect("CatalogoDocumentoPagos.aspx?Accion=Lista");
            }
        }

        private void ActualizaDoc(string []param)
        {
            

            txtSQL = "SELECT * FROM PagoActividadArchivo WHERE codPagoactividad=" + param[0] + 
                                " AND NomPagoActividadArchivo = '" + param[1] + "'";

            var dt = consultas.ObtenerDataTable(txtSQL,"text");

            string CodTipoArchivo = "";

            if (dt.Rows.Count > 0)
            {
                CodTipoArchivo = dt.Rows[0]["CodTipoArchivo"].ToString();
                txtNomDocumento.Text = dt.Rows[0]["NomPagoActividadarchivo"].ToString();
                txtNomDochide.Text = dt.Rows[0]["NomPagoActividadarchivo"].ToString();

                 // 2014710/24 Se cambia el campo que se envia por la session porque estaba errado RAlvaradoT
               // HttpContext.Current.Session["paramDoc"] = CodTipoArchivo + ";" + param[1];
                HttpContext.Current.Session["paramDoc"] = dt.Rows[0]["codPagoactividad"].ToString() + ";" + param[1];
            }

            //txtSQL = "SELECT Id_PagoTipoArchivo, NomPagoTipoArchivo FROM PagoTipoArchivo";

            //dt = consultas.ObtenerDataTable(txtSQL, "text");

            //ddltipodocumento.DataSource = dt;
            //ddltipodocumento.DataTextField = "NomPagoTipoArchivo";
            //ddltipodocumento.DataValueField = "Id_PagoTipoArchivo";

            //ddltipodocumento.DataBind();

            ddltipodocumento.SelectedValue = CodTipoArchivo;
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            //contPri.Visible = true;
            //contPri.Enabled = true;

            //panelEditar.Visible = false;
            //panelEditar.Enabled = false;
            if(usuario.CodGrupo == 6)
            {
                Response.Redirect("PagosActividad.aspx");
            }
            else
            {
                Response.Redirect("PagosActividad.aspx");
            }
            
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            string[] param = HttpContext.Current.Session["paramDoc"].ToString().Split(';');
            var ruta = string.Empty;
            var flag = true;
            
            if(!string.IsNullOrEmpty(txtNomDocumento.Text))
            {
                if(ddltipodocumento.SelectedIndex != 0)
                {
                    var sql = "Select * from PagoActividadArchivo where CodPagoActividad = " + hdCodigoPago.Value + " And NomPagoActividadArchivo = '" + txtNomDochide.Text.TrimEnd(' ').TrimStart(' ') + "'";
                    var dt = consultas.ObtenerDataTable(sql, "text");
                    if (dt.Rows.Count > 0)
                    {
                        txtSQL = @" UPDATE PagoActividadArchivo SET NomPagoActividadArchivo ='" + txtNomDocumento.Text + "', CodTipoArchivo=" + ddltipodocumento.SelectedValue +
                            " WHERE CodPagoActividad =" + hdCodigoPago.Value + " AND NomPagoActividadArchivo='" + txtNomDochide.Text + "'";
                    }
                    else
                    {
                        if (fArchivo.HasFile)
                        {
                            var nameFile = DateTime.Now.Millisecond + fArchivo.FileName.RemoveAccent();
                            ruta = ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "PagosFonade\\Anexos\\Proyecto_" + Session["CodProyecto"].ToString() + "\\IdActividad_" + hdCodigoPago.Value + "\\";
                            if (!Directory.Exists(ruta))
                            {
                                Directory.CreateDirectory(ruta);
                            }
                            if (File.Exists(ruta + nameFile))
                            {
                                File.Delete(ruta + nameFile);
                            }
                            fArchivo.SaveAs(ruta + nameFile);

                            ruta = ruta.Replace((ConfigurationManager.AppSettings.Get("RutaIP") + ConfigurationManager.AppSettings.Get("DirVirtual") + "PagosFonade\\"), ConfigurationManager.AppSettings.Get("RutaDocumentosPagos"));

                            var ext = Path.GetExtension(ruta + nameFile);
                            var tipoDoc = (from tp in consultas.Db.DocumentoFormatos
                                           where tp.Extension == ext
                                           select tp).FirstOrDefault();
                            var fecha = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString();
                            txtSQL = "Insert PagoActividadArchivo(NomPagoActividadarchivo,CodPagoActividad,RutaArchivo,Icono,URL,Fecha,CodTipoArchivo,CodDocumentoFormato) Values('" + txtNomDocumento.Text.TrimEnd(' ').TrimStart(' ') +
                                "'," + hdCodigoPago.Value + ",'" + ruta.Replace(@"Documentos\", "") + nameFile + "','" + tipoDoc.Icono + "','Documentos/PagosFonade/Anexos/Proyecto_" + Session["CodProyecto"].ToString() + "/" + "/IdActividad_" + hdCodigoPago.Value + "/" + nameFile +
                                "', Getdate()," + ddltipodocumento.SelectedValue + "," + tipoDoc.Id_DocumentoFormato + ")";
                        }
                        else
                        {
                            flag = false;
                            fArchivo.Focus();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar un archivo a cargar!')", true);
                        }

                    }

                    if (flag)
                    {
                        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ToString()))
                        {
                            try
                            {
                                var cmd = new SqlCommand(txtSQL, conn);
                                conn.Open();
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException se)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('" + se.Message + "')", true);
                            }
                            finally
                            {
                                conn.Close();
                                conn.Dispose();
                            }
                        }
                    }


                    contPri.Visible = true;
                    contPri.Enabled = true;

                    panelEditar.Visible = false;
                    panelEditar.Enabled = false;
                                       
                    LlenarGrilla(hdCodigoPago.Value);
                }
                else
                {
                    ddltipodocumento.Focus();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe seleccionar la categoria del documento!')", true);
                }
            }
            else
            {
                txtNomDocumento.Focus();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Debe el nombre del documento!')", true);
            }
            
            
        }

        protected void gvpresupuesto_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvpresupuesto_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                var lnkBorrar = (LinkButton)e.Row.FindControl("lnkEliminar");
                var lnk = (HyperLink)e.Row.FindControl("hlarchivo");
                lnk.NavigateUrl = ConfigurationManager.AppSettings.Get("RutaWebSite") + lnk.NavigateUrl;
                if (usuario.CodGrupo != Constantes.CONST_Emprendedor)
                {
                    lnkBorrar.Visible = false;
                }
                else
                {
                    if (Session["IdEstadoPago"] != null)
                    {
                        if(Session["IdEstadoPago"].ToString() != "0")
                        {
                            lnkBorrar.Visible = false;
                        }
                    }
                    else //Consultar el estado del pago en caso de que la sesion este nula
                    {
                        if(estadoPago(hdCodigoPago.Value) != Constantes.CONST_EstadoEdicion)
                            lnkBorrar.Visible = false;
                    }                    
                }
            }
        }

        private int estadoPago (string codPago)
        {
            ProyectoController proyectoController = new ProyectoController();

            return proyectoController.codEstadoPago(Convert.ToInt32(codPago));
        }

        protected void btnregresar_Click(object sender, EventArgs e)
        {
            if (usuario.CodGrupo == 6)
            {
                Response.Redirect("PagosActividad.aspx");
            }
            else
            {
                Response.Redirect("SeguimientoPptal.aspx");
            }
        }

    }
}