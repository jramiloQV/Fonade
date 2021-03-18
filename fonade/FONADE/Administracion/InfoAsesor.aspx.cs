#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>InfoAsesor.cs</Archivo>

#endregion

#region using 

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

namespace Fonade.FONADE.Administracion
{
    /// <summary>
    /// InfoAsesor
    /// </summary>    
    public partial class InfoAsesor : Negocio.Base_Page
    {
        #region variables globales

        string ID_PROYECTOAcreditar;
        string CODCONVOCATORIAAcreditar;
        string mTipoConsulta;

        //vaiable que contiene las sentencias SQL que se ejecutan en la BD
        string txtSQL;

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //SqlDataReader reader;//lectura de la BD

            //recoge los datos de session utilizados para la consulta
            ID_PROYECTOAcreditar = HttpContext.Current.Session["ID_PROYECTOAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString()) ? HttpContext.Current.Session["ID_PROYECTOAcreditar"].ToString() : "0";
            CODCONVOCATORIAAcreditar = HttpContext.Current.Session["CODCONVOCATORIAAcreditar"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString()) ? HttpContext.Current.Session["CODCONVOCATORIAAcreditar"].ToString() : "0";
            mTipoConsulta = HttpContext.Current.Session["mTipoConsulta"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["mTipoConsulta"].ToString()) ? HttpContext.Current.Session["mTipoConsulta"].ToString() : "0";

            //valida que tipo de consulta realiza de acuerdo al rol del usuario
            switch (mTipoConsulta)
            {
                case "1":
                    txtSQL = "SELECT DISTINCT (C.NOMBRES + ' ' + C.APELLIDOS) 'NOMBRE',C.EMAIL, C.TELEFONO  FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=1 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO) WHERE PC.CODPROYECTO =" + ID_PROYECTOAcreditar;
                    break;
                case "2":
                    txtSQL = "SELECT DISTINCT (C.NOMBRES + ' ' + C.APELLIDOS) 'NOMBRE',C.EMAIL, C.TELEFONO  FROM CONTACTO C JOIN PROYECTOCONTACTO PC ON (PC.CODROL=2 AND PC.INACTIVO=0 AND PC.CODCONTACTO=C.ID_CONTACTO AND (PC.ACREDITADOR IS NULL OR PC.ACREDITADOR=0  )) WHERE PC.CODPROYECTO =" + ID_PROYECTOAcreditar;
                    break;
            }

            if (!string.IsNullOrEmpty(txtSQL))
            {
                //se trae la informacion
                //reader = ejecutaReader(txtSQL, 1);
                var dt = consultas.ObtenerDataTable(txtSQL, "text");

                //si trae la informacion correcta
                //la muestra en los label
                if (dt.Rows.Count > 0)
                {
                    lblnombre.Text = dt.Rows[0].ItemArray[0].ToString();
                    lblcorreo.Text = dt.Rows[0].ItemArray[1].ToString();
                    lblnumero.Text = dt.Rows[0].ItemArray[2].ToString();
                }
            }
        }
    }
}