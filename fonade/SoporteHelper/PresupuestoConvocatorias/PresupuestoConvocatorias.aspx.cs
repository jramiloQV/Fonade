using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Account;
using Fonade.Negocio.PlanDeNegocioV2.Utilidad;

namespace Fonade.SoporteHelper.PresupuestoConvocatorias
{
    public partial class PresupuestoConvocatorias : System.Web.UI.Page
    {
        protected FonadeUser usuario { get { return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true); } set { } }
        protected void Page_Load(object sender, EventArgs e)
        {            
        }
        public List<Datos.sp_presupuestoDisponiblePorConvocatoriaResult> GetPresupuestos(int codigoConvocatoria)
        {
            return Convocatoria.GetPresupuestoPorConvocatoria(codigoConvocatoria);
        }

        public List<Datos.Convocatoria> GetConvocatorias() {
            var convocatorias = new List<Datos.Convocatoria>(){ };

            convocatorias.Add(
                new Datos.Convocatoria {
                        Id_Convocatoria = 0,
                        NomConvocatoria = "Todas",                        
                        Descripcion = "N/A",
                        FechaInicio = DateTime.Now,                   
                        FechaFin = DateTime.Now,
                        Presupuesto = 0,
                        MinimoPorPlan = 0 ,
                        Publicado = true,
                        CodContacto = 0,
                        encargofiduciario = "0",
                        CodConvenio = 0,
                        IdVersionProyecto = 2,
                        TopeConvocatoria = 0,
                });

            convocatorias.AddRange(Convocatoria.GetConvocatorias(usuario.CodOperador));

            return convocatorias;
        }
    }
}