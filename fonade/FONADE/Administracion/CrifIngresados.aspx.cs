#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>CrifIngresados.cs</Archivo>

#endregion

#region using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// CrifIngresados
    /// </summary>    
    public partial class CrifIngresados : Negocio.Base_Page
    {

        #region variables globales

        string ID_PROYECTOAcreditar;
        string CODCONVOCATORIAAcreditar;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //recoge variables de session del proyecto y de la convocatoria
            //para enlazarlos a la informacion de la grilla
            ID_PROYECTOAcreditar = HttpContext.Current.Session["ID_PROYECTOAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString()) ? HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString() : "0";
            CODCONVOCATORIAAcreditar = HttpContext.Current.Session["CODCONVOCATORIAAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString()) ? HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString() : "0";
        }

        /// <summary>
        /// Diego Quiñonez
        /// enlace de datos con linq
        /// carga en la grilla informacion de los documentos crif
        /// con su respectiva fecha
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lds_crifingresados_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //consulta en la BD por linq los documentos CRIFS de acuerdo al id del proyecto y al id de la convocatoria
            var crif = (from pa in consultas.Db.ProyectoAcreditaciondocumentosCRIFs
                        orderby pa.Fecha descending
                        where pa.CodProyecto == Convert.ToInt32(ID_PROYECTOAcreditar)
                        && pa.CodConvocatoria == Convert.ToInt32(CODCONVOCATORIAAcreditar)
                        select new
                        {
                            pa.Crif,
                            pa.Fecha
                        });

            //devuelve la consulta en tipo lista
            e.Result = crif.ToList();
        }
    }
}