using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// NotificacionesEnviadas
    /// </summary>    
    public partial class NotificacionesEnviadas : Negocio.Base_Page
    {
        string ID_PROYECTOAcreditar;
        string CODCONVOCATORIAAcreditar;

        string txtSQL;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            ID_PROYECTOAcreditar = HttpContext.Current.Session["ID_PROYECTOAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString()) ? HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString() : "0";
            CODCONVOCATORIAAcreditar = HttpContext.Current.Session["CODCONVOCATORIAAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString()) ? HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString() : "0";


            if (!IsPostBack)
            {
                txtSQL = "SELECT DISTINCT HEA.ID_HISTORICOEMAILACREDITACION, C.NOMBRES + ' ' + C.APELLIDOS 'NOMBRE', HEA.EMAIL,HEA.FECHA, C1.NOMBRES + ' ' + C1.APELLIDOS 'NOMCC', (SELECT TOP 1 R.NOMBRE FROM ROL R JOIN PROYECTOCONTACTO PC ON (R.ID_ROL = PC.CODROL) WHERE CODROL IN (1,2) AND CODCONTACTO=C1.ID_CONTACTO AND CODPROYECTO= HEA.CODPROYECTO) 'ROL',E.NOMESTADO FROM CONTACTO C JOIN HISTORICOEMAILACREDITACION HEA ON (HEA.CODCONTACTO = C.ID_CONTACTO)  JOIN ESTADO E ON (E.ID_ESTADO = HEA.CODESTADOACREDITACION) LEFT JOIN CONTACTOHISTORICOEMAILACREDITACION CHEA ON  (CHEA.CODHISTORICOEMAILACREDITACION=HEA.ID_HISTORICOEMAILACREDITACION) LEFT JOIN CONTACTO C1 ON (C1.ID_CONTACTO = CHEA.CODCONTACTO) WHERE HEA.CODPROYECTO=" + ID_PROYECTOAcreditar + " AND HEA.CODCONVOCATORIA=" + CODCONVOCATORIAAcreditar + " ORDER BY HEA.FECHA DESC ";

                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                if (dt.Rows.Count > 0)
                {
                    gvnotificaciones.DataSource = dt;
                    gvnotificaciones.DataBind();
                }
            }
        }
    }
}