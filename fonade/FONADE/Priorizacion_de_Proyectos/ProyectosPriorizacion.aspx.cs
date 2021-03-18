using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;
using System.Globalization;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Administracion.Operador;

namespace Fonade.FONADE.Priorizacion_deProyectos
{

    public partial class ProyectosPriorizacion : Negocio.Base_Page//System.Web.UI.Page
    {
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Recuperar la url
            string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

            if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
            {
                Response.Redirect(validacionCuenta.rutaHome(), true);
            }

            if (!Page.IsPostBack)
            {
                CargarDrops();
                MostrarPanel();
            }
           
        }

        private void MostrarPanel()
        {
            if (ddlOperador.SelectedItem.Value == "0")
            {
                UpdatePanel1.Visible = false;
            }
            else
            {
                UpdatePanel1.Visible = true;
                cargarGrid(Convert.ToInt32(ddlOperador.SelectedItem.Value));
            }
        }

        private void cargarGrid(int _codOperador)
        {
            var lista = getProyectosPorPriorizar(_codOperador);

            gvProyectosAPriorizar.DataSource = lista;
            gvProyectosAPriorizar.DataBind();
        }


        private void CargarDrops()
        {
            //operador
            cargarDDL(ddlOperador, usuario.CodOperador);
        }

        OperadorController operadorController = new OperadorController();

        private void cargarDDL(DropDownList ddl, int? _codOperador)
        {
            List<OperadorModel> opciones = new List<OperadorModel>();

            opciones = operadorController.cargaDLLOperador(_codOperador);

            ddl.DataSource = opciones;
            ddl.DataTextField = "NombreOperador"; // FieldName of Table in DataBase
            ddl.DataValueField = "idOperador";
            ddl.DataBind();
        }

        /// <summary>
        /// Listado de proyectos por priorizar
        /// </summary>                
        public List<ProyectoPorPriorizar> getProyectosPorPriorizar(int _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener los proyectos por priorizar
                var proyectosPorPriorizar = db.MD_MostrarProyectosaPriorizar(Constantes.CONST_AsignacionRecursos, _codOperador)
                                            .Select(
                                                    filter => new ProyectoPorPriorizar
                                                    {
                                                        Codigo = filter.codigoproyecto,
                                                        Nombre = filter.nombreproyecto,
                                                        CodigoConvocatoria = filter.codigoconvocatoria.GetValueOrDefault(0),
                                                        NombreConvocatoria = filter.nombreconvocatoria,
                                                        ValorRecomendado = filter.valorrecomendado,
                                                        Anio = filter.anio.GetValueOrDefault(DateTime.Now.Year),
                                                        Total = filter.total,
                                                        Operador = filter.operador
                                                    }).ToList();

                return proyectosPorPriorizar;
            }
        }

        protected void chkSeleccionarProyecto_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                decimal totalRecursos = 0;
                foreach (GridViewRow proyecto in gvProyectosAPriorizar.Rows)
                {
                    var radioViable = proyecto.FindControl("chkSeleccionarProyecto") as CheckBox;
                    if (radioViable.Checked)
                    {
                        //The field resourses comes from the database in this string format "$0,0.00",
                        //It was necesary remove the commas, the money character and ".00" tail for the successfull convertion to decimal. By @marztres
                        var recursos = Convert.ToDecimal((proyecto.FindControl("ValorRecomendado") as Label).Text.Remove(0, 1).Replace(",", string.Empty).Replace(".00", string.Empty));
                        totalRecursos += recursos;
                    }
                }
                lblTotalRecursos.Text = FieldValidate.moneyFormat(totalRecursos, true) + ".00";
            }
            catch (Exception)
            {
                lblTotalRecursos.Text = "$0.00";
            }
        }

        /// <summary>
        /// Listado de convocatorias activas
        /// </summary>                
        public List<ConvocatoriaActiva> getConvocatorias(int _codOperador)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                //Consulta para obtener los proyectos por priorizar
                var entities = db.Convocatoria
                                 .Where(filter => filter.Publicado == true 
                                                && filter.codOperador == _codOperador)
                                 .OrderBy(order => order.NomConvocatoria)
                                 .Select(filterSelection => new ConvocatoriaActiva
                                 {
                                     Codigo = filterSelection.Id_Convocatoria,
                                     Nombre = filterSelection.NomConvocatoria
                                 }).ToList();
                return entities;
            }
        }

        protected void btn_asignarRecursos_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Visible = false;
                lnkRutaActa.Visible = false;

                FieldValidate.ValidateString("Numero de acta", txtNumero.Text, true, 20);
                FieldValidate.ValidateString("Nombre de acta", txtNombre.Text, true, 250);
                FieldValidate.ValidateString("Fecha de acta", txtFecha.Text, true, 250);
                FieldValidate.ValidateString("Observaciones", txtObservaciones.Text, true, 1500);
                FieldValidate.ValidateString("Convocatoria", cmbConvocatoria.SelectedValue, true, 1500);

                DateTime fechaActa = DateTime.ParseExact(txtFecha.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (fechaActa > DateTime.Now.Date)
                    throw new ApplicationException("La fecha del acta no puede ser mayor a la fecha de hoy");

                Datos.AsignacionActa actaDePriorizacion = new Datos.AsignacionActa();

                actaDePriorizacion.NumActa = txtNumero.Text;
                actaDePriorizacion.NomActa = txtNombre.Text;
                actaDePriorizacion.FechaActa = fechaActa;
                actaDePriorizacion.Observaciones = txtObservaciones.Text;
                actaDePriorizacion.CodConvocatoria = Convert.ToInt32(cmbConvocatoria.SelectedValue);
                actaDePriorizacion.Publicado = true;

                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    db.AsignacionActas.InsertOnSubmit(actaDePriorizacion);
                    db.SubmitChanges();
                }

                List<ProyectoPorPriorizar> proyectosParaPriorizar = getProyectosParaPriorizar();

                priorizarProyectos(proyectosParaPriorizar, actaDePriorizacion);

                showLinkActa(actaDePriorizacion);

            }
            catch (ApplicationException ex)
            {
                lblError.Visible = true;
                lblError.Text = "Advertencia : " + ex.Message;
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error inesperado, intentelo de nuevo. detalle : " + ex.Message;
            }
        }

        private void showLinkActa(Datos.AsignacionActa actaDePriorizacion)
        {
            gvProyectosAPriorizar.DataBind();
            txtNumero.Text = "";
            txtNombre.Text = "";
            txtFecha.Text = "";
            txtObservaciones.Text = "";

            lnkRutaActa.Visible = true;
            lnkRutaActa.NavigateUrl = "VerActaAsignacionRecursos.aspx";
            lnkRutaActa.Text = "Se ha generado el acta número " + actaDePriorizacion.NumActa + " de Asignación de recursos.  Click aquí para verla.";
            lnkRutaActa.Focus();
            Session["Id_Acta"] = actaDePriorizacion.Id_Acta;

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Nueva acta creada: " + actaDePriorizacion.NumActa + " satisfactoriamente!');", true);
        }

        private List<ProyectoPorPriorizar> getProyectosParaPriorizar()
        {
            List<ProyectoPorPriorizar> proyectosPorPriorizar = new List<ProyectoPorPriorizar>();

            foreach (GridViewRow proyecto in gvProyectosAPriorizar.Rows)
            {
                var proyectoParaPriorizar = new ProyectoPorPriorizar();

                proyectoParaPriorizar.Codigo = Convert.ToInt32((proyecto.FindControl("lblCodigoProyecto") as Label).Text);
                proyectoParaPriorizar.Priorizado = (proyecto.FindControl("chkSeleccionarProyecto") as CheckBox).Checked;

                proyectosPorPriorizar.Add(proyectoParaPriorizar);
            }

            return proyectosPorPriorizar;
        }

        private void priorizarProyectos(List<ProyectoPorPriorizar> proyectos, Datos.AsignacionActa actaDePriorizacion)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                foreach (var proyectoParaPriorizar in proyectos)
                {
                    var proyecto = (from p in db.Proyecto where p.Id_Proyecto == proyectoParaPriorizar.Codigo select p).FirstOrDefault();

                    if (proyectoParaPriorizar.Priorizado)
                        proyecto.CodEstado = Constantes.CONST_LegalizacionContrato;

                    var asignacionActaProyecto = new Datos.AsignacionActaProyecto
                    {
                        CodActa = actaDePriorizacion.Id_Acta,
                        CodProyecto = proyectoParaPriorizar.Codigo,
                        Asignado = proyectoParaPriorizar.Priorizado
                    };

                    db.AsignacionActaProyecto.InsertOnSubmit(asignacionActaProyecto);
                    db.SubmitChanges();
                }
            }
        }

        protected void ddlOperador_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarPanel();
            cargarDllConvocatorias();
        }

        private void cargarDllConvocatorias()
        {
            cmbConvocatoria.DataSource = getConvocatorias(Convert.ToInt32(ddlOperador.SelectedValue));
            cmbConvocatoria.DataBind();
        }
    }

    public class ProyectoPorPriorizar
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int CodigoConvocatoria { get; set; }
        public string NombreConvocatoria { get; set; }
        public string ValorRecomendado { get; set; }
        public string Operador { get; set; }
        public int Anio { get; set; }
        public double Total { get; set; }
        public Boolean Priorizado { get; set; }
    }

    public class ConvocatoriaActiva
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
    }

}