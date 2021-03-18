#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 03 - 2014</Fecha>
// <Archivo>InfoEmprendedor.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Data.SqlClient;
using Fonade.Negocio;
using System.Web;

#endregion

namespace Fonade.FONADE.evaluacion
{
    public partial class InfoEmprendedor : Base_Page
    {
        int infoEmprendedorCodContacto;
        protected void Page_Load(object sender, EventArgs e)
        {
            infoEmprendedorCodContacto = HttpContext.Current.Session["infoEmprendedorCodContacto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["infoEmprendedorCodContacto"].ToString()) ? int.Parse(HttpContext.Current.Session["infoEmprendedorCodContacto"].ToString()) : 0;

            if (infoEmprendedorCodContacto == 0)
                Response.Redirect("./MiPerfil/Home.aspx");
            else
            {
                if (!IsPostBack)
                {
                    CargarinforacionEmprendedor();
                }
            }
        }

        private void CargarinforacionEmprendedor()
        {
            int codContacto = infoEmprendedorCodContacto;

            l_fechaActual.Text = DateTime.Now.ToString("dd 'de' MMMM 'de' yyyy");

            try
            {
                if (codContacto != 0)
                {
                    consultas.Parameters = new[]
                                               {
                                                   new SqlParameter
                                                       {
                                                           ParameterName = "@CodContacto",
                                                           Value = codContacto

                                                       }
                                               };

                    var dtEmprendedor = consultas.ObtenerDataSet("MD_ObtenerInformacionEmprendedor");

                    if (dtEmprendedor.Tables.Count != 0)
                    {
                        nombre.Text = dtEmprendedor.Tables[0].Rows[0]["nombres"].ToString();
                        apellidos.Text = dtEmprendedor.Tables[0].Rows[0]["apellidos"].ToString();
                        tipo.Text = dtEmprendedor.Tables[0].Rows[0]["NOMTIPOIDENTIFICACION"].ToString();
                        cedula.Text = dtEmprendedor.Tables[0].Rows[0]["identificacion"].ToString();
                        email.Text = dtEmprendedor.Tables[0].Rows[0]["email"].ToString();
                        aprendiz.Text = dtEmprendedor.Tables[0].Rows[0]["NOMTIPOAPRENDIZ"].ToString();
                        fechan.Text = formatearfecha(dtEmprendedor.Tables[0].Rows[0]["FechaNacimiento"].ToString());
                        ciuddn.Text = dtEmprendedor.Tables[0].Rows[0]["NomCiudad"].ToString() + " - " + dtEmprendedor.Tables[0].Rows[0]["NomDepartamento"].ToString();
                        numerot.Text = dtEmprendedor.Tables[0].Rows[0]["Telefono"].ToString();
                        expedicion.Text = dtEmprendedor.Tables[0].Rows[0]["Expedicion"].ToString();
                        genero.Text = dtEmprendedor.Tables[0].Rows[0]["Genero"].ToString();

                        // estudios
                        if (dtEmprendedor.Tables[1].Rows.Count != 0)
                        {
                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["TITULOOBTENIDO"].ToString()))
                            {
                                programR.Text = dtEmprendedor.Tables[1].Rows[0]["TITULOOBTENIDO"].ToString();
                            }
                            else
                            {
                                tr_programa.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["NOMNIVELESTUDIO"].ToString()))
                            {
                                nivelE.Text = dtEmprendedor.Tables[1].Rows[0]["NOMNIVELESTUDIO"].ToString();
                            }
                            else
                            {
                                tr_nivel.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["INSTITUCION"].ToString()))
                            {
                                institucion.Text = dtEmprendedor.Tables[1].Rows[0]["INSTITUCION"].ToString();
                            }
                            else
                            {
                                tr_institucion.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["NOMCIUDAD"].ToString()))
                            {
                                ciudadinst.Text = dtEmprendedor.Tables[1].Rows[0]["NOMCIUDAD"].ToString();
                            }
                            else
                            {
                                tr_ciudadi.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["Finalizado"].ToString()))
                            {
                                estado.Text = dtEmprendedor.Tables[1].Rows[0]["Finalizado"].ToString();
                            }
                            else
                            {
                                tr_estado.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["FECHAINICIO"].ToString()))
                            {
                                fechaini.Text = formatearfecha(dtEmprendedor.Tables[1].Rows[0]["FECHAINICIO"].ToString());
                            }
                            else
                            {
                                tr_fechai.Visible = false;
                            }
                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["FECHAGRADO"].ToString()))
                            {
                                fechag.Text = formatearfecha(dtEmprendedor.Tables[1].Rows[0]["FECHAGRADO"].ToString());
                            }
                            else
                            {
                                tr_fechagrado.Visible = false;
                            }


                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["FECHAGRADO"].ToString()))
                            {
                                fechag.Text = formatearfecha(dtEmprendedor.Tables[1].Rows[0]["FECHAGRADO"].ToString());
                            }
                            else
                            {
                                tr_fechagrado.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["FECHAULTIMOCORTE"].ToString()))
                            {
                                fechafinalizacionc.Text = formatearfecha(dtEmprendedor.Tables[1].Rows[0]["FECHAULTIMOCORTE"].ToString());
                            }
                            else
                            {
                                tr_fecgafubakuzacionc.Visible = false;
                            }

                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["FECHAFINMATERIAS"].ToString()))
                            {
                                fechafm.Text = formatearfecha(dtEmprendedor.Tables[1].Rows[0]["FECHAFINMATERIAS"].ToString());
                            }
                            else
                            {
                                tr_fechafinalizacionM.Visible = false;
                            }


                            if (!string.IsNullOrEmpty(dtEmprendedor.Tables[1].Rows[0]["SEMESTRESCURSADOS"].ToString()))
                            {
                                horas.Text = dtEmprendedor.Tables[1].Rows[0]["SEMESTRESCURSADOS"].ToString();
                            }
                            else
                            {
                                tr_horas.Visible = false;
                            }
                        }
                        else
                        {
                                tr_programa.Visible = false;
                                tr_nivel.Visible = false;
                                tr_institucion.Visible = false;
                                tr_ciudadi.Visible = false;
                                tr_estado.Visible = false;
                                tr_fechai.Visible = false;
                                tr_fechagrado.Visible = false;
                                tr_fechagrado.Visible = false;
                                tr_fecgafubakuzacionc.Visible = false;
                                tr_fechafinalizacionM.Visible = false;
                                tr_horas.Visible = false;
                        }

                    }
                    consultas.Parameters = null;

                }


            }
            catch (Exception ex)
            {
                consultas.Parameters = null;
                throw new Exception(ex.Message);
            }

        }

        protected void BtnCerrar_Click(object sender, EventArgs e)
        {
            RedirectPage(false, "", "cerrar");
            consultas.Parameters = null;
        }

        string formatearfecha(string valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                DateTime fecha = Convert.ToDateTime(valor);
                valor = string.Format("{0:dd/MM/yyyy}", fecha);
            }
            return valor;
        }
    }
}