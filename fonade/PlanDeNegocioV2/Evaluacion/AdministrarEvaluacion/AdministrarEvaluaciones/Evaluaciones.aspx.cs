using Fonade.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using Fonade.Account;

namespace Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluaciones
{
    public partial class Evaluaciones : Base_Page
    {
        string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!validacionCuenta.validarPermiso(usuario.IdContacto, pathRuta))
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                FieldValidate.ValidateNumeric("código", txtCodigoProyecto.Text, false);

                gvMain.DataBind();
            }
            catch (Exception ex)
            {                
            }
        }

        public List<Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.EvaluacionProyecto> Get(int codigoProyecto, int startIndex, int maxRows)
        {
            try
            {
                if (!codigoProyecto.Equals(0))
                {
                    return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Get(usuario.IdContacto, usuario.CodGrupo, 0, 100, usuario.CodOperador, codigoProyecto);
                }
                else
                {
                    return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Get(usuario.IdContacto, usuario.CodGrupo, startIndex, maxRows, usuario.CodOperador, null);
                }
            }
            catch (Exception)
            {
                return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Get(usuario.IdContacto, usuario.CodGrupo, startIndex, maxRows, null);
            }                           
        }

        public int Count(int codigoProyecto)
        {   
            try
            {
                if (!codigoProyecto.Equals(0))
                {
                    return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Count(usuario.IdContacto, usuario.CodGrupo, usuario.CodOperador, codigoProyecto);
                }
                else
                {
                    return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Count(usuario.IdContacto, usuario.CodGrupo, usuario.CodOperador, null);
                }
            }
            catch (Exception)
            {
                return Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluaciones.Evaluaciones.Count(usuario.IdContacto, usuario.CodGrupo, null);
            }
        }

        protected void gvMain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Ver"))
                {
                    if (e.CommandArgument != null)
                    {
                        string[] data = e.CommandArgument.ToString().Split(';');
                        var codigoProyecto = Convert.ToInt32(data[0]);
                        var codigoConvocatoria = Convert.ToInt32(data[1]);

                        HttpContext.Current.Session["CodProyecto"] = codigoProyecto;
                        HttpContext.Current.Session["CodConvocatoria"] = codigoConvocatoria;

                        FieldValidate.Redirect(null, "~/FONADE/evaluacion/EvaluacionFrameSet.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=600,top=50");
                    }
                } else if (e.CommandName.Equals("VerProyecto")) {
                    HttpContext.Current.Session["CodProyecto"] = e.CommandArgument;
                    FieldValidate.Redirect(null, "~/FONADE/Proyecto/ProyectoFrameSet.aspx", "_blank", "menubar=0,scrollbars=1,width=1000,height=600,top=50");
                }
            }
            catch (Exception ex)
            {
                lblError.Visible = true;
                lblError.Text = "Sucedio un error detalle :" + ex.Message;
            }
        }
    }    
}