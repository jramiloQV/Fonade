using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fonade.FONADE.Administracion
{
    /// <summary>    
    /// InfoEmprendedor
    /// </summary>    
    public partial class InfoEmprendedor : Negocio.Base_Page
    {
        string CodContactoEval;
        string txtSQL;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CodContactoEval = HttpContext.Current.Session["CodContactoEval"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodContactoEval"].ToString()) ? HttpContext.Current.Session["CodContactoEval"].ToString() : "0";

            if (!CodContactoEval.Equals("0"))
            {
                consultas.Parameters = new[]
                {
                    new SqlParameter{
                        ParameterName = "@CodContacto",
                        Value = Convert.ToInt32(CodContactoEval)
                    }
                };

                var dt = consultas.ObtenerDataTable("MD_InfoContactoEmprendedor");

                if (dt.Rows.Count > 0)
                {
                    lblnombre.Text = dt.Rows[0]["Nombres"].ToString();
                    lblapellidos.Text = dt.Rows[0]["Apellidos"].ToString();
                    lbltipoidentificacion.Text = dt.Rows[0]["NomTipoIdentificacion"].ToString();
                    lblidentificacion.Text = dt.Rows[0]["Identificacion"].ToString();
                    lblciudadexpedicion.Text = dt.Rows[0]["NomCiudadExpedicion"].ToString() + " - " + dt.Rows[0]["NomDepartamentoExpedicion"].ToString();
                    lblcorreo.Text = dt.Rows[0]["Email"].ToString();

                    if (dt.Rows[0]["Nombres"].ToString().Equals("M"))
                        lblgenero.Text = "Masculino";
                    if (dt.Rows[0]["Nombres"].ToString().Equals("F"))
                        lblgenero.Text = "Femenino";

                    lblfecha.Text = dt.Rows[0]["FechaNacimiento"].ToString();
                    lblciudadnacimiento.Text = dt.Rows[0]["NomCiudadNacimiento"].ToString() + " - " + dt.Rows[0]["NomDepartamentoNacimiento"].ToString();
                    lblnumero.Text = dt.Rows[0]["Telefono"].ToString();
                    lbltipoaprendiz.Text = dt.Rows[0]["NomTipoAprendiz"].ToString();
                }

                txtSQL = "SELECT CE.TITULOOBTENIDO,CE.INSTITUCION,CE.CODCIUDAD,NE.NOMNIVELESTUDIO,CE.CODPROGRAMAACADEMICO,CE.FINALIZADO,CE.FECHAINICIO,CE.FECHAFINMATERIAS,CE.FECHAGRADO,CE.FECHAULTIMOCORTE,CE.SEMESTRESCURSADOS,C.NOMCIUDAD,PA.NOMPROGRAMAACADEMICO FROM CONTACTOESTUDIO CE JOIN CIUDAD C ON (C.ID_CIUDAD = CE.CODCIUDAD) JOIN PROGRAMAACADEMICO PA ON (PA.ID_PROGRAMAACADEMICO = CE.CODPROGRAMAACADEMICO) JOIN NIVELESTUDIO NE ON (NE.ID_NIVELESTUDIO = CE.CODNIVELESTUDIO) WHERE CODCONTACTO =" + CodContactoEval + " ORDER BY FLAGINGRESADOASESOR DESC, FINALIZADO DESC, FECHAINICIO ASC";

                dt = consultas.ObtenerDataTable(txtSQL,"text");

                if (dt.Rows.Count > 0)
                {
                    lblnivelestudio.Text = dt.Rows[0]["NOMNIVELESTUDIO"].ToString();
                    lblprogramarealizado.Text = dt.Rows[0]["TITULOOBTENIDO"].ToString();
                    lblinstitucion.Text = dt.Rows[0]["INSTITUCION"].ToString();
                    lblciudadinstitucion.Text = dt.Rows[0]["NOMCIUDAD"].ToString();

                    if (dt.Rows[0]["FINALIZADO"].ToString().Equals("1"))
                        lblestado.Text = "Finalizado";
                    else
                        lblestado.Text = "Actualmente en curso";

                    lblfechainicion.Text = dt.Rows[0]["FECHAINICIO"].ToString();
                    lblfechagrado.Text = dt.Rows[0]["FECHAGRADO"].ToString();
                    lblfechafinalizacionmaterias.Text = dt.Rows[0]["FECHAFINMATERIAS"].ToString();
                    lblfechafinalizacionetapa.Text = dt.Rows[0]["FECHAULTIMOCORTE"].ToString();
                    lbltotalsemestres.Text = dt.Rows[0]["SEMESTRESCURSADOS"].ToString();
                }
            }
        }
    }
}