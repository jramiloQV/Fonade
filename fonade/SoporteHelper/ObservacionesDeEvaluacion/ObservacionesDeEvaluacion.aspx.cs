using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;

namespace Fonade.SoporteHelper.ObservacionesDeEvaluacion
{
    public partial class ObservacionesDeEvaluacion : Negocio.Base_Page //System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!(usuario.CodGrupo == Datos.Constantes.CONST_AdministradorSistema))
                {
                    Response.Redirect("~/FONADE/evaluacion/AccesoDenegado.aspx");
                }
            }
        }

        public List<Convocatoria> GetConvocatorias()
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                return db.Convocatoria.OrderByDescending(order => order.FechaInicio).ToList();
            }
        }

        protected void btnAddConvocatoria_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 codigoConvocatoria = Convert.ToInt32(cmbConvocatorias.SelectedValue);
                using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
                {
                    var entity = db.Convocatoria.Single(filter => filter.Id_Convocatoria.Equals(codigoConvocatoria));
                    if (entity == null)
                        throw new ApplicationException("No se logro obtener la información de la convocatoria.");

                    using (Datos.ObservacionEvaluacionDataContext db2 = new Datos.ObservacionEvaluacionDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["SubComponent"].ConnectionString))
                    {
                        var observacion = new LOVObjetoSE
                        {
                            NomLovObjetoSE = "Convocatorias",
                            Valor = entity.Id_Convocatoria.ToString(),
                            Descripcion = entity.NomConvocatoria
                        };

                        db2.LOVObjetoSEs.InsertOnSubmit(observacion);
                        db2.SubmitChanges();

                        gvConvocatoriasEnObservacion.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error, detalle :" + ex.Message;
            }
        }

        protected void gvConvocatoriasEnObservacion_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("EliminarConvocatoria"))
                {
                    if (e.CommandArgument != null)
                    {
                        var codigoConvocatoria = Convert.ToInt32(e.CommandArgument.ToString());

                        using (Datos.ObservacionEvaluacionDataContext db = new Datos.ObservacionEvaluacionDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["SubComponent"].ConnectionString))
                        {
                            var entity = db.LOVObjetoSEs.Single(filter => filter.Id_LovObjetoSE.Equals(codigoConvocatoria));

                            db.LOVObjetoSEs.DeleteOnSubmit(entity);
                            db.SubmitChanges();

                            gvConvocatoriasEnObservacion.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }

        public List<LOVObjetoSE> GetConvocatoriasEnEvaluacion()
        {
            using (Datos.ObservacionEvaluacionDataContext db = new Datos.ObservacionEvaluacionDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["SubComponent"].ConnectionString))
            {
                return db.LOVObjetoSEs.ToList();
            }
        }
    }
}