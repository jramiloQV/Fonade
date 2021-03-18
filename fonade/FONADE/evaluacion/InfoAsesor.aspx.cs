    #region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 03 - 2014</Fecha>
// <Archivo>InfoAsesor.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data.SqlClient;
using Fonade.Negocio;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class InfoAsesor : Base_Page
    {
        public int Codproyecto
        {
            get { return (int)ViewState["codproyecto"]; }
            set
            {
                ViewState["codproyecto"] = value;
            }
        }

        public int CodConvocatoria
        {
            get { return (int)ViewState["CodConvocatoria"]; }
            set
            {
                ViewState["CodConvocatoria"] = value;
            }
        }

        public int tipo
        {
            get { return (int)ViewState["tipo"]; }
            set
            {
                ViewState["tipo"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarinformacionAsesor();
            }
        }

        private void CargarinformacionAsesor()
        {
           l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            var requestinfo = CreateQueryStringUrl("a");
            
          

            try
            {
                if (requestinfo["tipoasesor"] != null)
                {
                    tipo = Convert.ToInt32(requestinfo["tipoasesor"]);
                }
                else
                {
                    tipo = 0;
                }


                if (requestinfo["proyectoAsesor"] != null)
                {
                    Codproyecto = Convert.ToInt32(requestinfo["proyectoAsesor"]);
                }
                else
                {
                    Codproyecto = 0;
                }

                if (requestinfo["codconvocatoriaAsesor"] != null)
                {
                    CodConvocatoria = Convert.ToInt32(requestinfo["codconvocatoriaAsesor"]);
                }
                else
                {
                    CodConvocatoria = 0;
                }

                if (Codproyecto != 0 && CodConvocatoria != 0)
                {
                    consultas.Parameters = new[]
                                               {
                                                   new SqlParameter
                                                       {
                                                           ParameterName = "@tipo",
                                                           Value = tipo

                                                       }, new SqlParameter
                                                              {
                                                                  ParameterName = "@proyecto",
                                                                  Value = Codproyecto
                                                              },
                                                   new SqlParameter
                                                       {
                                                           ParameterName = "@convocatoria",
                                                           Value = CodConvocatoria
                                                       }
                                               };

                    var dtAsesor = consultas.ObtenerDataTable("MD_ObtenerAsesor");

                    if (dtAsesor.Rows.Count != 0)
                    {
                        nombre.Text = dtAsesor.Rows[0]["Nombres"].ToString();
                        email.Text = dtAsesor.Rows[0]["EMAIL"].ToString();
                        telefono.Text = dtAsesor.Rows[0]["TELEFONO"].ToString();
                        consultas.Parameters = null;
                    }
                }


            }
            catch (Exception ex)
            {
                consultas.Parameters = null;
                throw new Exception(ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RedirectPage(false,"","cerrar");
            consultas.Parameters = null;
        }
    }
}