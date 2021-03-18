#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 07 - 2014</Fecha>
// <Archivo>Convocatoria.cs</Archivo>

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Fonade.Account;
using LinqKit;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Globalization;
using Fonade.Negocio;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.Convocatoria
{
    /// <summary>
    /// Convocatoria
    /// </summary>    
    public partial class Convocatoria : Negocio.Base_Page
    {
        /// <summary>
        /// The identifier convocatoria
        /// </summary>
        public int IdConvocatoria;
        /// <summary>
        /// publicado
        /// </summary>
        public bool publicado;
        HttpPostedFile[] qaz;
        delegate string del(string x, string y, int z);
        /// <summary>
        /// The index
        /// </summary>
        protected static int idx;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { idx = 10; }
            
            IdConvocatoria = Convert.ToInt32(HttpContext.Current.Session["IdConvocatoria"]);
            IdConvocatoriaHiddenField.Value = IdConvocatoria.ToString();
            if (!IsPostBack)
            {
                lbl_Titulo.Text = void_establecerTitulo("CONVOCATORIA");
                
                cargarDllOperador();

                llenarConvenios(Convert.ToInt32(ddlOperador.SelectedValue));
                if (IdConvocatoria == 0)
                {
                    panelBotonesCrear.Visible = true;
                    pnl_confirmacion.Visible = false;
                    pnlcriteriosselecion.Visible = false;                    
                    ConvocatoriaReglaSalariosPanel.Visible = 
                    string.IsNullOrEmpty(HttpContext.Current.Session["IdConvocatoria"].ToString()) ? int.Parse(HttpContext.Current.Session["IdConvocatoria"].ToString()) > 0 : false;
                }
                else
                {
                    pnl_confirmacion.Visible = true;
                    pnlcriteriosselecion.Visible = true;
                    llenarInfoConvocatoria(IdConvocatoria);
                    PanelUpdate.Visible = true;
                    PanelBotonesactualizar.Visible = true;
                    panelApertura.Visible = true;

                }
            }
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDllOperadorSeleccionado(int? _codOperador)
        {
            ddlOperador.DataSource = operadorController.cargaDLLOperador(_codOperador);
            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataBind();
        }

        private void cargarDllOperador()
        {
            ddlOperador.DataSource = operadorController.getAllOperador();
            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataBind();
        }

        /// <summary>
        /// Llenar los convenios.
        /// </summary>
        protected void llenarConvenios(int? _codOperador)
        {
            var query = from c in consultas.Db.Convenios
                        join f in consultas.Db.Contacto 
                        on c.CodcontactoFiduciaria equals f.Id_Contacto
                        where f.codOperador == _codOperador
                        select new
                        {
                            idConvenio = c.Id_Convenio,
                            nomConvenio = c.Nomconvenio,
                        };
            ddl_convenios.DataSource = query.ToList();
            ddl_convenios.DataTextField = "nomConvenio";
            ddl_convenios.DataValueField = "idConvenio";
            ddl_convenios.DataBind();
        }


        /// <summary>
        /// Handles the Click event of the btn_CrearConv control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_CrearConv_Click(object sender, EventArgs e)
        {
            var fechaInicio = DateTime.ParseExact(txt_frechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var fechaFin = DateTime.ParseExact(txt_fechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int topeConvocatoria = Convert.ToInt32(txtTopeConvocatoria.Text);

            fechaFin = fechaFin.AddHours(int.Parse(ddlhora.SelectedValue));
            fechaFin = fechaFin.AddMinutes(int.Parse(ddlminuto.SelectedValue));

            if (fechaInicio > fechaFin)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('La fecha de inicio de convocatoria no puede ser mayor a la de finalización!')", true);
            }
            else
            {
                int codOperador = Convert.ToInt32(ddlOperador.SelectedValue);
                insert_update(fechaInicio, fechaFin, txt_Presupuesto.Text, txt_valorminimo.Text, 0
                    , Convert.ToInt32(ddl_convenios.SelectedValue), IdConvocatoria, "Create"
                    , topeConvocatoria, codOperador);
                Response.Redirect("Convocatoria.aspx");
            }
        }

        /// <summary>
        /// llenarInfoConvocatoria
        /// carga la informacion de session de la convocatoria solicitada
        /// </summary>
        /// <param name="IdConvoct"></param>
        protected void llenarInfoConvocatoria(int IdConvoct)
        {
            var query = (from conv in consultas.Db.Convocatoria
                         where conv.Id_Convocatoria == IdConvoct
                         select new
                         {
                             conv
                         }).FirstOrDefault();

            txt_nombre.Text = query.conv.NomConvocatoria;
            txt_descripcion.Text = query.conv.Descripcion;
            txt_encargo.Text = query.conv.encargofiduciario;
            txt_fechaFin.Text = query.conv.FechaFin.ToString("dd/MM/yyyy");
            txt_frechaInicio.Text = query.conv.FechaInicio.ToString("dd/MM/yyyy");
            string hora = query.conv.FechaFin.ToString("HH:mm tt");
            ddlhora.SelectedValue = int.Parse(hora.Substring(0, 2)).ToString();
            ddlminuto.SelectedValue = int.Parse(hora.Substring(3, 2)).ToString();
            txt_Presupuesto.Text = query.conv.Presupuesto.ToString();
            txt_valorminimo.Text = query.conv.MinimoPorPlan.ToString();
            txtTopeConvocatoria.Text = query.conv.TopeConvocatoria.GetValueOrDefault(0).ToString();

            cargarDllOperadorSeleccionado(query.conv.codOperador);
            llenarConvenios(query.conv.codOperador);

            ddl_convenios.SelectedValue = query.conv.CodConvenio.ToString();
            Ch_publicado.Checked = (bool)query.conv.Publicado;
            

            #region desabilitar o activar ambitos segun publicacion
            lnk_adicionarconfirmacion.Visible = !(bool)query.conv.Publicado;
            lnkcriteriosseleccion.Visible = !(bool)query.conv.Publicado;
            foreach (GridViewRow gvr in gvr_confirmacion.Rows)
            {
                ((LinkButton)gvr.FindControl("lnkeditar")).Enabled = !(bool)query.conv.Publicado;
                ((LinkButton)gvr.FindControl("lnkeliminar")).Visible = !(bool)query.conv.Publicado;
            }
            foreach (GridViewRow gvr in gvr_criteriosseleccion.Rows)
            {
                ((LinkButton)gvr.FindControl("lnkeditar")).Enabled = !(bool)query.conv.Publicado;
                ((LinkButton)gvr.FindControl("lnkeliminar")).Visible = !(bool)query.conv.Publicado;
            }
            #endregion

            l_numeroapertura.Text = query.conv.Id_Convocatoria.ToString("D6");
            if ((bool)query.conv.Publicado == true)
            {
                txt_nombre.ReadOnly = true;
                txt_descripcion.ReadOnly = true;
                btnDate1.Visible = false;
                //
                btnDate2.Visible = false;
                txt_valorminimo.ReadOnly = true;
                Ch_publicado.Enabled = false;
                btn_Proyectos.Enabled = true;
                publicado = true;
            }
            else
            {
                publicado = false;
            }
        }

        /// <summary>
        /// permite la insercion y/o la actualizacion
        /// de una convocatoria
        /// </summary>
        /// <param name="FechaInicioV"></param>
        /// <param name="FechaFinV"></param>
        /// <param name="PresupuestoV"></param>
        /// <param name="MinimoPorPlanV"></param>
        /// <param name="PublicadoV"></param>
        /// <param name="CodConvenioV"></param>
        /// <param name="idConvocatoriaV"></param>
        /// <param name="caso"></param>
        /// <param name="topeConvocatoria"></param>
        protected void insert_update(DateTime FechaInicioV, DateTime FechaFinV, string PresupuestoV
            , string MinimoPorPlanV, int PublicadoV, int CodConvenioV, int idConvocatoriaV
            , string caso, int topeConvocatoria, int _codOperador)
        {
            Int64 presupuesto = Convert.ToInt64(PresupuestoV);
            Int64 valorminimo = Convert.ToInt64(MinimoPorPlanV);

            var fechaInicio = FechaInicioV;
            var fechaInicioFinal = fechaInicio.Month + "/" + fechaInicio.Day + "/" + fechaInicio.Year + " " + fechaInicio.Hour + ":" + fechaInicio.Minute + ":00";
            var fechaFin = FechaFinV;
            var fechaFinFInal = fechaFin.Month + "/" + fechaFin.Day + "/" + fechaFin.Year + " " + fechaFin.Hour + ":" + fechaFin.Minute + ":00";

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_Insert_Update_Convocatorias", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NomConvocatoriaV", txt_nombre.Text);
            cmd.Parameters.AddWithValue("@DescripcionV", txt_descripcion.Text);
            cmd.Parameters.AddWithValue("@FechaInicioV", fechaInicioFinal); 
            cmd.Parameters.AddWithValue("@FechaFinV", fechaFinFInal); 
            cmd.Parameters.AddWithValue("@PresupuestoV", presupuesto);
            cmd.Parameters.AddWithValue("@MinimoPorPlanV", valorminimo);
            cmd.Parameters.AddWithValue("@PublicadoV", PublicadoV);
            cmd.Parameters.AddWithValue("@codContactoV", usuario.IdContacto);
            cmd.Parameters.AddWithValue("@EncargoFiduciarioV", txt_encargo.Text);
            cmd.Parameters.AddWithValue("@CodConvenioV", CodConvenioV);
            cmd.Parameters.AddWithValue("@idConvocatoriaV", idConvocatoriaV);
            cmd.Parameters.AddWithValue("@caso", caso);
            cmd.Parameters.AddWithValue("@codOperador", _codOperador);
            con.Open();
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();
            if (caso == "Create")
            {
                var query = (from conv in consultas.Db.Convocatoria
                             select new
                             {
                                 conv
                             }).Max(x => x.conv.Id_Convocatoria);
                int IdConvoca = query;
                HttpContext.Current.Session["IdConvocatoria"] = IdConvoca;
                Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.UpdateTopeConvocatoria(IdConvoca, topeConvocatoria);

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.document.location.reload()", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Actualizado exitosamente!')", true);
                HttpContext.Current.Session["IdConvocatoria"] = idConvocatoriaV;
                Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.UpdateTopeConvocatoria(idConvocatoriaV, topeConvocatoria);

                Response.Redirect("Convocatoria.aspx");
            }
        }

        /// <summary>
        /// Parses the military time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Invalid hour
        /// or
        /// Invalid minute
        /// </exception>
        public static DateTime ParseMilitaryTime(string time, int year, int month, int day)
        {
            //
            // Convert hour part of string to integer.
            //
            string hour = time.Substring(0, 2);
            int hourInt = int.Parse(hour);
            if (hourInt >= 24)
            {
                throw new ArgumentOutOfRangeException("Invalid hour");
            }
            //
            // Convert minute part of string to integer.
            //
            string minute = time.Substring(2, 2);
            int minuteInt = int.Parse(minute);
            if (minuteInt >= 60)
            {
                throw new ArgumentOutOfRangeException("Invalid minute");
            }
            //
            // Return the DateTime.
            //
            return new DateTime(year, month, day, hourInt, minuteInt, 0);
        }

        /// <summary>
        /// Handles the Click event of the btn_Proyectos control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_Proyectos_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Id_ProyPorConvoct"] = IdConvocatoria;
            Response.Redirect("ProyectosPorConvocatoria.aspx");
        }

        /// <summary>
        /// Handles the Click event of the btn_actualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_actualizar_Click(object sender, EventArgs e)
        {
            var fechaInicio = DateTime.ParseExact(txt_frechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var fechaFin = DateTime.ParseExact(txt_fechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            fechaFin = fechaFin.AddHours(int.Parse(ddlhora.SelectedValue));
            fechaFin = fechaFin.AddMinutes(int.Parse(ddlminuto.SelectedValue));
            int topeConvocatoria = Convert.ToInt32(txtTopeConvocatoria.Text);           

            string caso = "";
            if (publicado)
            {
                caso = "Update2";
            }
            else
            {
                caso = "Update1";
            }

            int publicar = 0;
            if (Ch_publicado.Checked == true)
            {
                publicar = 1;
            }

            insert_update(fechaInicio, fechaFin, txt_Presupuesto.Text, txt_valorminimo.Text
                , publicar, Convert.ToInt32(ddl_convenios.SelectedValue), IdConvocatoria, caso
                , topeConvocatoria, Convert.ToInt32(ddlOperador.SelectedValue));
        }

        /// <summary>
        /// Handles the Click event of the btn_Criterios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_Criterios_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Id_ConvocatCriterios"] = IdConvocatoria;
            Response.Redirect("CatalogoConvocatoriaCriterioPriorizacion.aspx");
        }

        /// <summary>
        /// Handles the RowCommand event of the gvreglasalarios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvreglasalarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString() == "Select")
            {
                
                int NumCondicion = Convert.ToInt32(e.CommandArgument.ToString()??"0");
                var ujm = ((HiddenField)gvreglasalarios.Rows[NumCondicion].FindControl("hiddenNumero")??new HiddenField(){Value="0"}).Value;
                NumCondicion = Convert.ToInt32(ujm);
                HttpContext.Current.Session["IdConvocatoriaRegla"] = IdConvocatoria;
                HttpContext.Current.Session["condicionRegla"] = ((GridView)e.CommandSource).SelectedValue;
                string scriptBock = string.Format("<script type='text/javascript'>uhb({0});</script>", NumCondicion);
                base.ClientScript.RegisterClientScriptBlock(this.GetType(),"popup", scriptBock);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", scriptBock, false);
                base.RegisterClientScriptBlock("Mensaje", string.Format("uhb({0})", NumCondicion));
                ImageButton5_ModalPopupExtender.Show();
            }
        }

        /// <summary>
        /// Handles the Load event of the gvreglasalarios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gvreglasalarios_Load(object sender, EventArgs e)
        {
            foreach (GridViewRow grd_Row in this.gvreglasalarios.Rows)
            {
                if (((HiddenField)grd_Row.FindControl("hiddenNumero")).Value == gvreglasalarios.Rows.Count.ToString())
                {
                    
                }
            }
        }

        /// <summary>
        /// Handles the Selecting event of the lds_regla control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_regla_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            try
            {
                var query = from P in consultas.VerReglasConvocatoria(IdConvocatoria)
                            select P;
                var lst = query.Select(p => new { p.condicion, p.CodConvocatoria, p.ExpresionLogica, p.EmpleosGenerados1, p.EmpleosGenerados2, p.SalariosAPrestar, p.NoRegla, itmid = ++idx }).ToList();
                e.Result = lst;
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Handles the Click event of the lbtn_adicionarRegla control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lbtn_adicionarRegla_Click(object sender, EventArgs e)
        {
            if (gvreglasalarios.Rows.Count < 6)
            {
                HttpContext.Current.Session["IdConvocatoriaRegla"] = IdConvocatoria;
                HttpContext.Current.Session["condicionRegla"] = "0";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "window.open('ConvocatoriaReglaSalarios.aspx','_blank','width=685,height=157,toolbar=no, scrollbars=no, resizable=no');", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('El máximo de condiciones para esta regla es de 6!')", true);
            }
        }

        #region cofinanciacion y criterios de seleccion

        /// <summary>
        /// Handles the Selecting event of the ldsconfirmacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void ldsconfirmacion_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var result = (from k in consultas.Db.ConvocatoriaCofinanciacions
                          from c in consultas.Db.Ciudad
                          from d in consultas.Db.departamento
                          where k.CodCiudad == c.Id_Ciudad
                          && c.CodDepartamento == d.Id_Departamento
                          && k.CodConvocatoria == IdConvocatoria
                          select new
                          {
                              k.CodCiudad,
                              nomCiudad = c.NomCiudad + " (" + d.NomDepartamento + ") ",
                              k.Cofinanciacion
                          });

            e.Result = result.ToList();
        }

        /// <summary>
        /// Handles the RowCommand event of the gvr_confirmacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvr_confirmacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToString().Equals("Borrar"))
            {
                ConvocatoriaCofinanciacion Cofinanciacion = (from cc in consultas.Db.ConvocatoriaCofinanciacions
                                                             where cc.CodConvocatoria == IdConvocatoria
                                                             && cc.CodCiudad == Convert.ToInt32(e.CommandArgument.ToString())
                                                             select cc).FirstOrDefault();

                consultas.Db.ConvocatoriaCofinanciacions.DeleteOnSubmit(Cofinanciacion);

                try
                {
                    consultas.Db.SubmitChanges();
                }
                catch (Exception) { }
                finally
                {
                    gvr_confirmacion.DataBind();
                }
            }

            if (e.CommandName.ToString().Equals("Editar"))
            {
                HttpContext.Current.Session["CodCiudad"] = e.CommandArgument.ToString();
                HttpContext.Current.Session["Accion"] = "Editar";
                HttpContext.Current.Session["codConvocatoria"] = IdConvocatoria;

                Redirect(null, "CatalogoCofinanciacion.aspx", "_Blank", "width=600,height=220");
            }
        }

        /// <summary>
        /// Handles the Click event of the lnk_adicionarconfirmacion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnk_adicionarconfirmacion_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "Nuevo";
            HttpContext.Current.Session["codConvocatoria"] = IdConvocatoria;

            Redirect(null, "CatalogoCofinanciacion.aspx", "_Blank", "width=600,height=220");
        }

        /// <summary>
        /// Handles the Selecting event of the lds_criterioseleccion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_criterioseleccion_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var criterios = (from cc in consultas.Db.ConvocatoriaCriterios
                             orderby cc.NomCriterio
                             where cc.CodConvocatoria == IdConvocatoria
                             select new
                             {
                                 cc.Id_Criterio,
                                 cc.NomCriterio
                             });

            e.Result = criterios.ToList();
        }

        /// <summary>
        /// Handles the RowCreated event of the gvr_criteriosseleccion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gvr_criteriosseleccion_RowCreated(object sender, GridViewRowEventArgs e)
        {
            del delegado = (string x, string y, int z) =>
            {
                string valor = string.Empty;

                if (z == 1)
                {
                    if (string.IsNullOrEmpty(x))
                        valor += " (Todo el pais) ";
                    else
                        valor += " " + x + " ";

                    if (string.IsNullOrEmpty(y))
                        valor += " (Todos los Municipios) ";
                    else
                        valor += " " + y + " ";
                }
                else
                {
                    if (string.IsNullOrEmpty(x))
                        valor += " (Todos los Sectores) ";
                    else
                        valor += " " + x + " ";
                }
                return valor;
            };

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idcriterio = Convert.ToInt32(gvr_criteriosseleccion.DataKeys[e.Row.RowIndex].Value.ToString());

                var geografico = (from k in consultas.Db.ConvocatoriaCriterioCiudads
                                  join d in consultas.Db.departamento on k.CodDepartamento equals d.Id_Departamento into r1
                                  from c1 in r1.DefaultIfEmpty()
                                  join c in consultas.Db.Ciudad on k.CodCiudad equals c.Id_Ciudad into r2
                                  from c2 in r2.DefaultIfEmpty()
                                  orderby c1.NomDepartamento, c2.NomCiudad
                                  where k.CodCriterio == idcriterio
                                  select new
                                  {
                                      Ciudad = delegado(c1.NomDepartamento, c2.NomCiudad, 1)
                                  });

                ((GridView)e.Row.FindControl("gvr_ambitos")).DataSource = geografico;
                ((GridView)e.Row.FindControl("gvr_ambitos")).DataBind();

                var economico = (from k in consultas.Db.ConvocatoriaCriterioSectors
                                 join s in consultas.Db.Sector on k.CodSector equals s.Id_Sector into r1
                                 from c1 in r1.DefaultIfEmpty()
                                 orderby c1.NomSector
                                 where k.CodCriterio == idcriterio
                                 select new
                                 {
                                     nombreSector = delegado(c1.NomSector, "", 2)
                                 });

                ((GridView)e.Row.FindControl("gvr_ambitos1")).DataSource = economico;
                ((GridView)e.Row.FindControl("gvr_ambitos1")).DataBind();
            }
        }

        /// <summary>
        /// Handles the RowCommand event of the gvr_criteriosseleccion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewCommandEventArgs"/> instance containing the event data.</param>
        protected void gvr_criteriosseleccion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToString())
            {
                case "Borrar":

                    try
                    {
                        var convocatoriacc = (from ccc in consultas.Db.ConvocatoriaCriterioCiudads
                                              where ccc.CodCriterio == Convert.ToInt32(e.CommandArgument.ToString())
                                              select ccc);

                        consultas.Db.ConvocatoriaCriterioCiudads.DeleteAllOnSubmit(convocatoriacc);
                        consultas.Db.SubmitChanges();

                        var convocatoriacs = (from ccs in consultas.Db.ConvocatoriaCriterioSectors
                                              where ccs.CodCriterio == Convert.ToInt32(e.CommandArgument.ToString())
                                              select ccs);

                        consultas.Db.ConvocatoriaCriterioSectors.DeleteAllOnSubmit(convocatoriacs);
                        consultas.Db.SubmitChanges();

                        var convocatoriac = (from cc in consultas.Db.ConvocatoriaCriterios
                                             where cc.Id_Criterio == Convert.ToInt32(e.CommandArgument.ToString())
                                             select cc).First();

                        consultas.Db.ConvocatoriaCriterios.DeleteOnSubmit(convocatoriac);
                        consultas.Db.SubmitChanges();
                    }
                    catch (Exception) { }
                    finally
                    {
                        gvr_criteriosseleccion.DataBind();
                    }

                    break;

                case "Editar":

                    HttpContext.Current.Session["codCriterio"] = e.CommandArgument.ToString();
                    HttpContext.Current.Session["Accion"] = "Editar";
                    HttpContext.Current.Session["codConvocatoria"] = IdConvocatoria;

                    Redirect(null, "CatalogoCriterio.aspx", "_Blank", "width=860,height=550");

                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkcriteriosseleccion control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void lnkcriteriosseleccion_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["Accion"] = "Nuevo";
            HttpContext.Current.Session["codConvocatoria"] = IdConvocatoria;

            Redirect(null, "CatalogoCriterio.aspx", "_Blank", "width=938,height=751");
        }

        #endregion


        /// <summary>
        /// Handles the Click event of the btnModificar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void btnModificar_Click(object sender, ImageClickEventArgs e){
            
        }

        /// <summary>
        /// Handles the Click event of the ImageButton5 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
        {

        }

        /// <summary>
        /// Handles the RowDeleting event of the gvreglasalarios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewDeleteEventArgs"/> instance containing the event data.</param>
        protected void gvreglasalarios_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string NumCondicion = string.Empty;
            IdConvocatoria = Convert.ToInt32(IdConvocatoriaHiddenField.Value);
            NumCondicion = (e.Keys[0].ToString());
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString);
            SqlCommand cmd = new SqlCommand("MD_convocatoria_regla_salarios", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CodConvocatoriaR", IdConvocatoria);
            cmd.Parameters.AddWithValue("@ExpresionLogicaR", "");
            cmd.Parameters.AddWithValue("@EmpleosGenerados1R", 0);
            cmd.Parameters.AddWithValue("@EmpleosGenerados2R", DBNull.Value);
            cmd.Parameters.AddWithValue("@SalariosAPrestarR", 0);
            cmd.Parameters.AddWithValue("@NoReglaR", Convert.ToInt32(NumCondicion));
            cmd.Parameters.AddWithValue("@caso", "Delete");
            SqlCommand cmd2 = new SqlCommand(UsuarioActual(), con);
            con.Open();
            cmd2.ExecuteNonQuery();
            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            cmd2.Dispose();
            cmd.Dispose();
            e.Cancel=true;
            Response.Redirect(Request.UrlReferrer.AbsoluteUri);
        }

        /// <summary>
        /// Handles the Click event of the ImageButton2 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
        {
            Session["CodConvoca"] = IdConvocatoria;
            Session["Accion"] = "NuevoDocumento";
            Redirect(null, "CatalogoActa.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        /// <summary>
        /// Handles the Click event of the ImageButton1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Session["CodConvoca"] = IdConvocatoria;
            Session["Accion"] = "Lista";
            Redirect(null, "CatalogoActa.aspx", "_blank", "menubar=0,scrollbars=1,width=710,height=400,top=100");
        }

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarConvenios(Convert.ToInt32(ddlOperador.SelectedValue));
        }
    }
}