using Fonade.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.interventoria
{
    public partial class PlanesaAcreditar : Negocio.Base_Page
    {
        ValidacionCuenta validacionCuenta = new ValidacionCuenta();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Recuperar la url
                string pathRuta = HttpContext.Current.Request.Url.AbsolutePath;

                if (!validacionCuenta.Acreditador(usuario.IdContacto)) //Menu Master Page
                {
                    Response.Redirect(validacionCuenta.rutaHome(), true);
                }
                else
                {
                    string txtSQL = @"SELECT DISTINCT P.ID_PROYECTO, P.NOMPROYECTO,PC.FECHAINICIO 'FECHAASIGNACION', DATEDIFF(DD,PC.FECHAINICIO,GETDATE()) 'DIAS', MAX(PC.CODCONVOCATORIA) 'CODCONVOCATORIA' ,E.NOMESTADO 'ESTADO' FROM PROYECTO P JOIN PROYECTOCONTACTO PC ON (PC.INACTIVO=0 AND PC.CODPROYECTO=P.ID_PROYECTO AND P.CODESTADO IN (10,11,12,13,14,15,16)) JOIN ESTADO E ON (E.ID_ESTADO = P.CODESTADO) INNER JOIN (SELECT DISTINCT P.ID_PROYECTO, MAX(PC.CODCONVOCATORIA) 'CODCONVOCATORIA' FROM PROYECTO P JOIN PROYECTOCONTACTO PC ON (PC.INACTIVO=0 AND PC.CODPROYECTO=P.ID_PROYECTO AND P.CODESTADO IN (10,11,12,13,14,15,16)) JOIN ESTADO E ON (E.ID_ESTADO = P.CODESTADO)  WHERE PC.ACREDITADOR = 1 AND PC.CODCONVOCATORIA IS NOT NULL AND PC.CODCONTACTO = " + usuario.IdContacto +
                       " GROUP BY P.ID_PROYECTO) t on (t.ID_PROYECTO=p.ID_PROYECTO and t.CODCONVOCATORIA=pc.CODCONVOCATORIA) WHERE PC.ACREDITADOR = 1 AND PC.CODCONVOCATORIA IS NOT NULL AND PC.CODCONTACTO = " + usuario.IdContacto +
                       " GROUP BY P.ID_PROYECTO,P.NOMPROYECTO,PC.FECHAINICIO,E.NOMESTADO";

                    var result = consultas.ObtenerDataTable(txtSQL, "text");

                    if (result.Rows.Count > 0)
                    {
                        gvplanesaacreditar.DataSource = result;
                        gvplanesaacreditar.DataBind();
                    }
                }
            }
        }

        protected void gvplanesaacreditar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("proyectoFrame"))
            {
                string param = e.CommandArgument.ToString();

                HttpContext.Current.Session["CodProyecto"] = param;

                //NO ES UNA VENTANA EMERGENTE!
                //Redirect(null, "ProyectoFrameSet.aspx", "_Blank", "width=730,height=585");
                Response.Redirect("../Proyecto/ProyectoFrameSet.aspx");
            }
            else
            {
                string[] param = e.CommandArgument.ToString().Split(';');

                HttpContext.Current.Session["CodProyecto"] = param[0];
                HttpContext.Current.Session["CodConvocatoria"] = param[1];

                Response.Redirect("ProyectoAcreditacion.aspx");
            }
        }
    }
}