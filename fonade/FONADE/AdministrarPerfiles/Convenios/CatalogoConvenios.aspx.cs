using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using System.Globalization;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.AdministrarPerfiles.Convenios
{
    /// <summary>
    /// CatalogoConvenios
    /// </summary>    
    public partial class CatalogoConvenios : Negocio.Base_Page
    {
        //string crearAdmin = "Crear Administrador";
        //string crearGerenteEvaluador = "Crear GerenteEvaluador";
        //string crearCallCenter = "Crear Call Center";
        //string crearGerenteInterventor = "Crear Gerente Interventor";
        //string crearPerfilFiduciario = "Crear Perfil Fiduciario";
        //String grupos1;
        int idusuario;//id del usuario seleccionado en la grilla
        public String[] grupos = { "0" };
        string[] codgrupousuario;

        /// <summary>
        /// Voids the show.
        /// </summary>
        /// <param name="texto">The texto.</param>
        /// <param name="mostrar">if set to <c>true</c> [mostrar].</param>
        protected void void_show(string texto, bool mostrar)
        {
            lbl_popup.Visible = mostrar;
            lbl_popup.Text = texto;
            mpe1.Enabled = mostrar;
            mpe1.Show();
        }

        /// <summary>
        /// Voids the modificar datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="Grupocontacto">The grupocontacto.</param>
        protected void void_ModificarDatos(string[] codgrupo, int Grupocontacto)
        {
            if (Request.QueryString["CodCriterio"] != null)
            {
                int codigoContacto = Int32.Parse(Request.QueryString["CodCriterio"]);
                var query = (from c in consultas.Db.Convenios
                             where c.Id_Convenio == codigoContacto
                             select c).FirstOrDefault();
                query.Nomconvenio = tb_Convenio.Text;
                //query.Fechainicio = Convert.ToDateTime(tb_fechaInicio.Text);
                query.Fechainicio = DateTime.ParseExact(tb_fechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                //query.FechaFin = Convert.ToDateTime(tb_fechaFin.Text);
                query.FechaFin = DateTime.ParseExact(tb_fechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query.Descripcion = tb_Descripcion.Text;
                query.CodcontactoFiduciaria = int.Parse(ddl_fiduciaria.SelectedValue);

                consultas.Db.SubmitChanges();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Convenio actualizado satisfactoriamente!' );", true);
                //Response.Redirect("CatalogoConvenios.aspx");
            }
        }

        /// <summary>
        /// Voids the crear datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_CrearDatos(string[] codgrupo, int codusuario)
        {
            Convenio Criterio = new Convenio()
            {
                Nomconvenio = tb_Convenio.Text,
                Fechainicio = DateTime.ParseExact(tb_fechaInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), //Convert.ToDateTime(tb_fechaInicio.Text),
                FechaFin = DateTime.ParseExact(tb_fechaFin.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), //Convert.ToDateTime(tb_fechaFin.Text),
                Descripcion = tb_Descripcion.Text,
                CodcontactoFiduciaria = Int32.Parse(ddl_fiduciaria.SelectedValue)
            };

            consultas.Db.Convenios.InsertOnSubmit(Criterio);
            consultas.Db.SubmitChanges();
            Lbl_Resultados.Visible = true;
            Lbl_Resultados.Text = "el Criterio " + tb_Convenio.Text + " ha sido agregado";
            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Convenio creado satisfactoriamente!' );", true);
        }

        /// <summary>
        /// Voids the hide controls.
        /// </summary>
        /// <param name="accion">The accion.</param>
        /// <param name="codgrupo">The codgrupo.</param>
        protected void void_HideControls(string accion, string[] codgrupo)
        {
            switch (accion)
            {
                case "Crear":
                    calExtender2.SelectedDate = DateTime.Today;
                    Calendarextender1.SelectedDate = DateTime.Today;
                    btn_crearActualizar.Text = "Crear";
                    break;
                case "Editar":
                    btn_crearActualizar.Text = "Actualizar";
                    break;
                default: break;
            }

        }

        /// <summary>
        /// Voids the traer datos.
        /// </summary>
        /// <param name="codgrupo">The codgrupo.</param>
        /// <param name="codusuario">The codusuario.</param>
        protected void void_traerDatos(string[] codgrupo, int codusuario)
        {
            if (Request.QueryString["CodCriterio"] != null)
            {
                int codigoContacto = Int32.Parse(Request.QueryString["CodCriterio"]);
                var query = (from c in consultas.Db.Convenios
                             where c.Id_Convenio == codigoContacto
                             select c).FirstOrDefault();

                calExtender2.SelectedDate = query.FechaFin;
                Calendarextender1.SelectedDate = query.Fechainicio;
                tb_Descripcion.Text = query.Descripcion;
                tb_Convenio.Text = query.Nomconvenio;
                ddl_fiduciaria.SelectedValue = query.CodcontactoFiduciaria.ToString();
            }
        }

        /// <summary>
        /// Voids the obtener parametros.
        /// </summary>
        protected void void_ObtenerParametros()
        {
            if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
            {
                pnl_Convenios.Visible = false;
                pnl_crearEditar.Visible = true;
                AgregarConvenio.Visible = false;
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    if (Request.QueryString["Accion"].ToString() == "Editar")
                    {
                        AgregarConvenio.Text = "Actualizar";
                        idusuario = Int32.Parse(Request.QueryString["CodCriterio"]);
                        void_traerDatos(codgrupousuario, idusuario);//trae la información segun el usuario y el grupo al cual 
                    }
                    if (Request.QueryString["Accion"].ToString() == "Crear")
                    {
                        AgregarConvenio.Text = "Crear";
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {


                string accion = "";

                int codigoConvenio = 0;
                if (!String.IsNullOrEmpty(Request.QueryString["CodCriterio"]))
                {
                    codigoConvenio = Int32.Parse(Request.QueryString["CodCriterio"]);
                    int operador = operadorDeFiducia(codigoConvenio);
                    cargarDllOperador(operador);
                    llenarFiducias(operador);
                }
                else
                {
                    cargarDllOperador(null);
                }
                if (!String.IsNullOrEmpty(Request.QueryString["Accion"]))
                {
                    accion = Request.QueryString["Accion"].ToString();
                    void_HideControls(accion, grupos);
                };

                pnl_crearEditar.Visible = false;
                lbl_Titulo.Text = void_establecerTitulo(grupos, accion, "CONVENIO");
                void_ObtenerParametros();
            }
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDllOperador(int? _codOperador)
        {
            ddlOperador.DataSource = operadorController.cargaDLLOperador(_codOperador);
            ddlOperador.DataTextField = "NombreOperador";
            ddlOperador.DataValueField = "idOperador";
            ddlOperador.DataBind();
        }

        private int operadorDeFiducia(int _codConvenio)
        {
            int _codOperador = 0;

            _codOperador = (from c in consultas.Db.Contacto
                            join co in consultas.Db.Convenios
                            on c.Id_Contacto equals co.CodcontactoFiduciaria
                            where co.Id_Convenio == _codConvenio
                            select c.codOperador
                            ).FirstOrDefault() ?? 0;

            return _codOperador;
        }

        /// <summary>
        /// Handles the DataBound event of the gv_Convenios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void gv_Convenios_DataBound(object sender, EventArgs e)
        { }

        /// <summary>
        /// Handles the RowDataBound event of the gv_Convenios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Convenios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string Conteo = "";
            ImageButton imbtton = new ImageButton();
            imbtton = ((ImageButton)e.Row.Cells[0].FindControl("btn_Inactivar"));
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Conteo == "0")
                {
                    imbtton.Visible = true;
                }
                else
                {
                    imbtton.Visible = false;
                }
            }
        }

        /// <summary>
        /// Handles the PageIndexChanged event of the gv_Convenios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewPageEventArgs"/> instance containing the event data.</param>
        protected void gv_Convenios_PageIndexChanged(object sender, GridViewPageEventArgs e)
        { }

        /// <summary>
        /// Handles the Selecting event of the lds_Convenios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="LinqDataSourceSelectEventArgs"/> instance containing the event data.</param>
        protected void lds_Convenios_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var convenios = (from cv in consultas.Db.Convenios
                             select cv into convenio
                             from c in consultas.Db.Contacto
                             where c.Id_Contacto == convenio.CodcontactoFiduciaria
                             select new convenioModel
                             {
                                 Id_convenio = convenio.Id_Convenio,
                                 nomconvenio = convenio.Nomconvenio,
                                 FechaInicio = convenio.Fechainicio,
                                 FechaFin = convenio.FechaFin,
                                 EmailFiduciaria = c.Email,
                                 idOperador = c.codOperador
                             }).ToList();

            foreach (var c in convenios)
            {
                if(c.idOperador!=null)
                c.operador = operadorController.getOperador(c.idOperador).NombreOperador;
            }

            e.Arguments.TotalRowCount = convenios.Count();
            if (e.Arguments.TotalRowCount == 0)
            {
                Lbl_Resultados.Visible = true;
                Lbl_Resultados.Text = "No tiene actividades  para este rango de fechas";
            }
            else
            {
                Lbl_Resultados.Visible = false;
            }

            e.Result = convenios.ToList();
        }

        /// <summary>
        /// Handles the click event of the btn_Inactivar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="CommandEventArgs"/> instance containing the event data.</param>
        protected void btn_Inactivar_click(object sender, CommandEventArgs e)
        {

            var criterio = (from c in consultas.Db.CriterioPriorizacions
                            where c.Id_Criterio == short.Parse(e.CommandArgument.ToString())
                            select c).FirstOrDefault();
            consultas.Db.CriterioPriorizacions.DeleteOnSubmit(criterio);
            consultas.Db.SubmitChanges();
            // deshabilitar Validadores
            RequiredFieldValidator1.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            RequiredFieldValidator6.Enabled = false;
            //bindear dataview            
            gv_Convenios.DataBind();
        }

        /// <summary>
        /// Handles the onclick event of the btn_crearActualizar control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btn_crearActualizar_onclick(object sender, EventArgs e)
        {
            int codigoContacto = 0;
            if (!String.IsNullOrEmpty(Request.QueryString["CodCriterio"]))
            {
                codigoContacto = Int32.Parse(Request.QueryString["CodCriterio"]);
            }
            string accion = Request.QueryString["Accion"].ToString();
            switch (accion)
            {
                case "Crear":
                    void_CrearDatos(grupos, codigoContacto);
                    break;
                case "Editar":
                    void_ModificarDatos(grupos, codigoContacto);
                    break;
            }

            Response.Redirect("CatalogoConvenios.aspx");
        }

        /// <summary>
        /// Handles the RowCreated event of the gv_Convenios control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="GridViewRowEventArgs"/> instance containing the event data.</param>
        protected void gv_Convenios_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        // buscar el link del header
                        LinkButton lnk = (LinkButton)tc.Controls[0];
                        if (lnk != null && gv_Convenios.SortExpression == lnk.CommandArgument)
                        {
                            // inicializar nueva imagen
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            // url de la imagen dinamicamente
                            img.ImageUrl = "/Images/ImgFlechaOrden" + (gv_Convenios.SortDirection == SortDirection.Ascending ? "Up" : "Down") + ".gif";
                            // a ñadir el espacio de la imagen
                            tc.Controls.Add(new LiteralControl(" "));
                            tc.Controls.Add(img);

                        }
                    }
                }
            }
        }

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarFiducias(Convert.ToInt32(ddlOperador.SelectedValue));
        }

        private void llenarFiducias(int? _codOperador)
        {
            var factor = from c in consultas.Db.Contacto
                         from gc in consultas.Db.GrupoContactos
                         where c.Id_Contacto == gc.CodContacto
                         & gc.CodGrupo == Constantes.CONST_Perfil_Fiduciario
                         & c.Inactivo == false
                         & c.codOperador == _codOperador
                         select c;
            ddl_fiduciaria.DataSource = factor;
            ddl_fiduciaria.DataTextField = "Email";
            ddl_fiduciaria.DataValueField = "id_contacto";
            ddl_fiduciaria.DataBind();
        }
    }

    public class convenioModel
    {
        public int Id_convenio { get; set; }
        public string nomconvenio { get; set; }
        public DateTime? FechaInicio  { get; set; }
        public DateTime? FechaFin { get; set; }
        public string EmailFiduciaria { get; set; }
        public int? idOperador { get; set; }
        public string operador { get; set; }
    }
}