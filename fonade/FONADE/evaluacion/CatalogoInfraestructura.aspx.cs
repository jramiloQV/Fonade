using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;

namespace Fonade.FONADE.evaluacion
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    public partial class CatalogoInfraestructura : System.Web.UI.Page
    {
        Consultas oConsultas = new Consultas();
        private ProyectoInfraestructura entity;

        int codProyecto;
        int Id_ProyectoInfraestructura;
        string Accion;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            TxtValorU.Attributes.Add("onkeypress", "javascript:return validarNro(event)");

            TxtValorU.Attributes.Add("onchange", "MoneyFormat(this)");
            codProyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? int.Parse(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            Id_ProyectoInfraestructura = HttpContext.Current.Session["Id_ProyectoInfraestructura"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_ProyectoInfraestructura"].ToString()) ? int.Parse(HttpContext.Current.Session["Id_ProyectoInfraestructura"].ToString()) : 0;
            Accion = Request.QueryString["Accion"];
            if (codProyecto == 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.close();", true);
            }

            if (!IsPostBack)
            {
                lblfecha.Text = DateTime.Now.ToShortDateString();
                CargarTipoInfraestructura();

                if (Request.QueryString["Accion"] == "Editar")
                {
                    lblfecha.Text = DateTime.Now.ToShortDateString();
                    ObtenerById(Id_ProyectoInfraestructura, Accion);
                    BtnCrear.Text = "Editar";

                }

            }
        }

        // Metodos 
        /// <summary>
        /// cargar tipo de infraestructura
        /// </summary>

        public void CargarTipoInfraestructura()
        {
            List<TipoInfraestructura> lstInfraestructura = oConsultas.Db.TipoInfraestructuras.Where(x => x.IdVersion == Constantes.CONST_PlanV1).ToList();
            if (lstInfraestructura.Any())
            {

                DdlTpoInfraestructura.DataTextField = "NomTipoInfraestructura";
                DdlTpoInfraestructura.DataValueField = "Id_TipoInfraestructura";
                DdlTpoInfraestructura.DataSource = lstInfraestructura;
                DdlTpoInfraestructura.DataBind();
                DdlTpoInfraestructura.Items.Insert(0, new ListItem("Seleccione", "0"));
            }

        }
        /// <summary>
        /// actualizar
        /// </summary>
        public void Insertar()
        {
            try
            {


                entity = new ProyectoInfraestructura
                {
                    codProyecto = codProyecto,
                    NomInfraestructura = TxtNombre.Text.Trim(),
                    CodTipoInfraestructura = Convert.ToByte(DdlTpoInfraestructura.SelectedValue),
                    FechaCompra = Convert.ToDateTime(TxtFecha.Text),
                    Unidad = !string.IsNullOrEmpty(TxtUnidadTipo.Text) ? TxtUnidadTipo.Text.Trim() : string.Empty,
                    ValorCredito = !string.IsNullOrEmpty(TxtPorcentaje.Text) ? Convert.ToInt32(TxtPorcentaje.Text.Trim()) : 0,
                    ValorUnidad = (!string.IsNullOrEmpty(TxtValorU.Text) ? Convert.ToDecimal(TxtValorU.Text.Replace(",", "").Replace(".", ",")) : 0),
                    Cantidad = (!string.IsNullOrEmpty(TxtCantidad.Text) ? Convert.ToDouble(TxtCantidad.Text.Trim()) : 0),
                    PeriodosAmortizacion = Convert.ToByte(DdlPeriodo.SelectedValue),
                    SistemaDepreciacion = !string.IsNullOrEmpty(Txtsistema.Text) ? Txtsistema.Text.Trim() : string.Empty,

                };

                oConsultas.Db.ProyectoInfraestructuras.InsertOnSubmit(entity);
                oConsultas.Db.SubmitChanges();

                var oListItem = DdlTpoInfraestructura.Items.FindByValue(DdlTpoInfraestructura.SelectedValue);
                var objProyectInverBusqueda = (from pinver in oConsultas.Db.ProyectoInversions
                                               where pinver.CodProyecto == codProyecto && pinver.Concepto == oListItem.Text
                                               select pinver).FirstOrDefault();

                if (objProyectInverBusqueda == null)
                {
                    var valorU = TxtValorU.Text.Split('.');
                    var objProyectInversion = new ProyectoInversion
                    {
                        CodProyecto = codProyecto,
                        Concepto = oListItem.Text,
                        Valor = int.Parse(valorU[0].Replace(",", "")) * int.Parse(TxtCantidad.Text),
                        AportadoPor = "",
                        Semanas = 0,
                        TipoInversion = "Fija"
                    };

                    oConsultas.Db.ProyectoInversions.InsertOnSubmit(objProyectInversion);
                    oConsultas.Db.SubmitChanges();
                }
                else
                {
                    var objConsulta = oConsultas.Db.ProyectoInversions.Single(
                        p => p.CodProyecto == codProyecto && p.Concepto == oListItem.Text);
                    var valorU = TxtValorU.Text.Split('.');
                    objConsulta.Valor += int.Parse(valorU[0].Replace(",", "")) * int.Parse(TxtCantidad.Text);
                    oConsultas.Db.SubmitChanges();
                }

                //ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('Registro Creado Exitosamente!');", true);
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.parent.location.reload();window.close();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;", true);
                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }


        }
        /// <summary>
        /// modificar tabla
        /// </summary>
        public void Update()
        {
            try
            {
                var consult = oConsultas.Db.ProyectoInfraestructuras.Single(
                               p => p.Id_ProyectoInfraestructura == Id_ProyectoInfraestructura);
                if (consult != null)
                {

                    consult.NomInfraestructura = TxtNombre.Text.Trim();
                    consult.CodTipoInfraestructura = Convert.ToByte(DdlTpoInfraestructura.SelectedValue);
                    consult.FechaCompra = Convert.ToDateTime(TxtFecha.Text);
                    consult.Unidad = !string.IsNullOrEmpty(TxtUnidadTipo.Text) ? TxtUnidadTipo.Text.Trim() : string.Empty;
                    consult.ValorCredito = !string.IsNullOrEmpty(TxtPorcentaje.Text) ? Convert.ToInt32(TxtPorcentaje.Text.Trim()) : 0;
                    consult.ValorUnidad = (!string.IsNullOrEmpty(TxtValorU.Text) ? Convert.ToDecimal(TxtValorU.Text.Replace(",", "").Replace(".", ",")) : 0);
                    consult.Cantidad = (!string.IsNullOrEmpty(TxtCantidad.Text) ? Convert.ToDouble(TxtCantidad.Text.Trim()) : 0);
                    consult.PeriodosAmortizacion = Convert.ToByte(DdlPeriodo.SelectedValue);
                    consult.SistemaDepreciacion = !string.IsNullOrEmpty(Txtsistema.Text) ? Txtsistema.Text.Trim() : string.Empty;
                    oConsultas.Db.SubmitChanges();
                    ScriptManager.RegisterClientScriptBlock(this, GetType(), "Mensaje", "alert('Registro Actualizado Exitosamente!');", true);
                    //this.ClientScript.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;", true);
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.opener.location.href = window.opener.location.href;window.close();", true);

                    //                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "window.opener.location.href = window.opener.location.href;", true);
                    //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);

                }

            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }


        }

        /// <summary>
        /// OObtener por id
        /// </summary>
        /// <param name="codigo">The codigo.</param>
        /// <param name="accion">The accion.</param>
        public void ObtenerById(int codigo, string accion)
        {
            try
            {
                if (codigo != 0 && accion == "Editar")
                {
                    var proyecto =
                        oConsultas.Db.ProyectoInfraestructuras.Single(
                            p => p.Id_ProyectoInfraestructura == codigo);
                    if (proyecto != null)
                    {
                        TxtNombre.Text = proyecto.NomInfraestructura;
                        TxtFecha.Text = proyecto.FechaCompra.ToShortDateString();
                        TxtCantidad.Text = proyecto.Cantidad.ToString();
                        TxtUnidadTipo.Text = proyecto.Unidad;
                        DdlTpoInfraestructura.SelectedValue = Convert.ToString(proyecto.CodTipoInfraestructura);
                        DdlPeriodo.SelectedValue = Convert.ToString(proyecto.PeriodosAmortizacion);
                        //WPLAZAS 
                        TxtValorU.Text = proyecto.ValorUnidad.Value.ToString("0,0.00", CultureInfo.InvariantCulture);
                        TxtPorcentaje.Text = Convert.ToString(proyecto.ValorCredito);
                        Txtsistema.Text = proyecto.SistemaDepreciacion;
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = ex.Message;
            }
            
        }


        /// <summary>
        /// Handles the Click event of the BtnCrear control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void BtnCrear_Click(object sender, EventArgs e)
        {
            if (Accion != "Editar")
            {
                Insertar();
            }
            else Update();

        }

        /// <summary>
        /// Handles the Click event of the BtnCancelar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "", "window.close();", true);
            if (Page.PreviousPage != null) Response.Redirect(Page.PreviousPage.Request.Url.PathAndQuery);
        }
    }
}