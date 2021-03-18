using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Datos;
using Fonade.Negocio;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Fonade.Clases;
using System.Text;
using Fonade.Negocio.Interventoria;
using Fonade.Negocio.Entidades;

namespace Fonade.FONADE.interventoria
{
    public partial class ProyectoAcreditacion : Base_Page
    {
        #region propiedades

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

        public int CodEstado
        {
            get { return (int)ViewState["CodEstado"]; }
            set
            {
                ViewState["CodEstado"] = value;
            }
        }

        public bool Lectura
        {
            get { return (bool)ViewState["lectura"]; }
            set { ViewState["lectura"] = value; }
        }

        public string Observacionfinal
        {
            get { return ViewState["observacionfinal"].ToString(); }
            set { ViewState["observacionfinal"] = value; }
        }


        #endregion


        #region cambio de estado
        //Diego Quiñonez - 12 de Diciembre de 2014

        int indeceFilaDataList;
        int Id_Emprendedor;
        string NomEstado;
        bool EstPendiente;
        bool Estsubsanado;
        bool EstAcreditado;
        bool EstNoAcreditado;

        delegate string del(string x);

        #endregion

        #region page load

        protected void Page_Load(object sender, EventArgs e)
        {
            Obtenervariables();

            if (!IsPostBack)
            {
                ObtenerInformacionGeneral();
                int id = usuario.IdContacto;
            }
        }

        #endregion

        #region Metodos


        void Obtenervariables()
        {
            //Diego Quiñonez - Valida y recoge parametros de session
            Codproyecto = HttpContext.Current.Session["CodProyecto"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodProyecto"].ToString()) ? int.Parse(HttpContext.Current.Session["CodProyecto"].ToString()) : 0;
            CodConvocatoria = HttpContext.Current.Session["CodConvocatoria"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["CodConvocatoria"].ToString()) ? int.Parse(HttpContext.Current.Session["CodConvocatoria"].ToString()) : 0;

            //var querystringSeguro = CreateQueryStringUrl("info");

            //if (querystringSeguro != null)
            //{
            //    if (Codproyecto == 0)
            //        Codproyecto = int.Parse(querystringSeguro["CodProyecto"].ToString());
            //    if (CodConvocatoria == 0)
            //        CodConvocatoria = int.Parse(querystringSeguro["CodConvocatoria"].ToString());
            //}

            EsSoloLectura();
        }

        void EsSoloLectura()
        {
            Lectura = true;

            int codigo = GetEstadoProyectoFromProyecto();

            if ((codigo == 10) || (codigo == 11) || (codigo == 12) || (codigo == 15) || (codigo == 16))
            {
                switch (codigo)
                {
                    case 11:
                        rdb.SelectedValue = "13";
                        break;
                    case 12:
                        rdb.SelectedValue = "14";
                        break;
                    default:
                        rdb.SelectedValue = codigo.ToString();
                        break;
                }

                Lectura = false;
            }
            else
            {
                //Obtengo el valor de las páginas desde las cuales se puede ver el formulario
                const string url = "ProyectoAcreditacion.aspx";
                //Request.ServerVariables["HTTP_REFERER"];
                var lPaginas = (string)consultas.RetornarEscalar("SELECT VALOR FROM PARAMETRO WHERE NOMPARAMETRO='PaginaPermitidaProyectoAcreditacion'", "text");
                lPaginas = lPaginas.Replace("?", "");
                string[] result = lPaginas.Split(';');

                if (lPaginas != null)
                {
                    foreach (var s in result)
                    {
                        if (s == url)
                        {
                            Lectura = true;
                            return;
                        }
                    }
                }
                else
                    Lectura = false;
            }
            return;
        }

        int GetEstadoProyectoFromProyecto()
        {
            var estado = consultas.Db.Proyecto.FirstOrDefault(p => p.Id_Proyecto == Codproyecto);

            if (estado != null && estado.Id_Proyecto != 0)
            {
                return estado.CodEstado;
            }
            else
            {
                return 0;
            }
        }

        void EliminarProyectoDeActa()
        {
            consultas.Parameters = null;

            if (CodConvocatoria != 0 && Codproyecto != 0)
            {
                consultas.Parameters = new[]
                                       {
                                           new SqlParameter
                                               {
                                                   ParameterName = "@codProyecto",
                                                   Value = Codproyecto
                                               },
                                           new SqlParameter
                                               {
                                                   ParameterName = "@codConvocatoria",
                                                   Value = CodConvocatoria
                                               }
                                       };

                bool bandera = consultas.InsertarDataTable("MD_EliminarProyectoDeActa");
                consultas.Parameters = null;

            }
        }

        void ObtenerInformacionGeneral()
        {
            try
            {
                #region parameters

                consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                       ParameterName = "@codproyecto",Value = Codproyecto
                                                   },
                                                    new SqlParameter
                                                   {
                                                         ParameterName = "@codconvocatoria",Value = CodConvocatoria
                                                   },
                                                    new SqlParameter
                                                   {
                                                         ParameterName = "@codusuario",Value = usuario.CodGrupo
                                                   }

                                           };

                #endregion

                var dtsInformacion = consultas.ObtenerDataSet("MD_ObtenerInformacionProyectoAcreditacion");

                if (dtsInformacion.Tables.Count != 0)
                {

                    if (dtsInformacion.Tables[0].Rows.Count != 0)
                    {
                        Observacionfinal = dtsInformacion.Tables[0].Rows[0]["OBSERVACIONFINAL"].ToString();
                        //txtObservaciones.Text = Observacionfinal;
                    }
                    else
                        Observacionfinal = string.Empty;

                    if (dtsInformacion.Tables[1].Rows.Count != 0)
                        CodEstado = Convert.ToInt32(dtsInformacion.Tables[1].Rows[0]["CODESTADO"].ToString());
                    else
                        CodEstado = 0;

                    if (dtsInformacion.Tables[3].Rows.Count != 0)
                    {
                        lblnomconvocatoria.Text = ObtenerNombreconvocatoria();
                        lblnomproyecto.Text = Codproyecto.ToString() + " - " + "\r\n" + dtsInformacion.Tables[3].Rows[0]["NOMPROYECTO"].ToString();
                        lblunidad.Text = dtsInformacion.Tables[3].Rows[0]["UnidadEmprendimiento"].ToString();
                        lblnomciudad.Text = dtsInformacion.Tables[3].Rows[0]["NOMCIUDAD"].ToString();
                    }


                    if (dtsInformacion.Tables[6].Rows.Count != 0)
                        txtcrif.Text = dtsInformacion.Tables[6].Rows[0]["CRIF"].ToString();
                    else
                        txtcrif.Text = string.Empty;

                    if (dtsInformacion.Tables[4].Rows.Count != 0)
                        hplAsesorLider.Text = dtsInformacion.Tables[4].Rows[0]["NombreLider"].ToString();
                    else
                        hplAsesorLider.Visible = false;

                    if (dtsInformacion.Tables[5].Rows.Count != 0)
                        hplAsesor.Text = dtsInformacion.Tables[5].Rows[0]["NombreAsesor"].ToString();
                    else
                        hplAsesor.Visible = false;

                    // obtengo todos los crif 

                    if (dtsInformacion.Tables[7].Rows.Count != 0)
                        HttpContext.Current.Session["dtcrif"] = dtsInformacion.Tables[7];
                    else
                        HttpContext.Current.Session["dtcrif"] = null;

                    GetFechaAval();
                    ValidarPrecondiciones();
                    ValidaBotones();

                    if (Lectura)
                        txtcrif.Enabled = false;
                }

                consultas.Parameters = null;
                LlenarDtlistProyectoAcreditacion();
            }
            catch (Exception)
            {
                consultas.Parameters = null;
                throw;
            }
        }

        void ValidarPrecondiciones()
        {
            var count =
                consultas.Db.ConvocatoriaProyectos.Count(
                    c => c.CodProyecto == Codproyecto && c.CodConvocatoria == CodConvocatoria);

            if (count != 0)
            {
                var countd =
                    consultas.Db.ProyectoAcreditacionDocumentos.Count(
                        c => c.CodProyecto == Codproyecto && c.CodConvocatoria == CodConvocatoria);

                if (countd == 0)
                {
                    consultas.Parameters = null;
                    consultas.Parameters = new[]
                                           {
                                               new SqlParameter
                                                   {
                                                         ParameterName = "@proyectoId",Value = Codproyecto
                                                   },
                                                    new SqlParameter
                                                   {
                                                         ParameterName = "@convocatoriaId",Value = CodConvocatoria
                                                   }

                                           };
                    bool bandera = consultas.InsertarDataTable("MD_InsertProyectoAcreditacionDocumento");

                    InsertarProyectoAcreditacion(Constantes.CONST_Asignado_para_acreditacion, string.Empty);

                    consultas.Parameters = null;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Redirect", "alert('Existen registros correspondientes al proyecto y/o a la convocatoria');location.href=\"PlanesaAcreditar.aspx\"", true);

                //Response.Redirect("PlanesaAcreditar.aspx");
            }
        }

        void InsertarProyectoAcreditacion(int estado, string observaciones)
        {
            var proyectoAcreditacion = new Datos.ProyectoAcreditacion();

            try
            {
                observaciones = observaciones.Replace("'", "");

                if (Codproyecto != 0 && CodConvocatoria != 0)
                {
                    proyectoAcreditacion.CodConvocatoria = CodConvocatoria;
                    proyectoAcreditacion.CodProyecto = Codproyecto;

                    if (observaciones.Length > 1000)
                    {
                        observaciones = observaciones.Substring(0, 1000);
                    }

                    proyectoAcreditacion.ObservacionFinal = observaciones;

                    proyectoAcreditacion.Fecha = DateTime.Now;
                    proyectoAcreditacion.CodEstado = estado;
                    consultas.Db.ProyectoAcreditacions.InsertOnSubmit(proyectoAcreditacion);
                    consultas.Db.SubmitChanges();
                }


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        void GetFechaAval()
        {
            var firstOrDefault =
                consultas.Db.ProyectoFormalizacions.FirstOrDefault(
                    pf => pf.codProyecto == Codproyecto && pf.CodConvocatoria == CodConvocatoria);

            if (firstOrDefault != null)
            {
                lblfechaEval.Text = firstOrDefault.Fecha.ToString();
            }
        }

        string ObtenerNombreconvocatoria()
        {

            var convocatoria = consultas.Db.Convocatoria.FirstOrDefault(c => c.Id_Convocatoria == CodConvocatoria);

            if (convocatoria != null && convocatoria.Id_Convocatoria != 0)
            {
                return convocatoria.NomConvocatoria;
            }
            return "";
        }

        string ObtenerNombreProyecto()
        {

            var proyecto = consultas.Db.Proyecto.FirstOrDefault(c => c.Id_Proyecto == Codproyecto);

            if (proyecto != null && proyecto.Id_Proyecto != 0)
            {
                return proyecto.NomProyecto;
            }
            return "";
        }


        void VerInformacionAseso(string tipo)
        {
            int para = 0;
            switch (tipo)
            {
                case "asesor":
                    para = 2;
                    break;

                case "lider":
                    para = 1;
                    break;
            }

            var querystringSeguro = CreateQueryStringUrl();
            querystringSeguro["proyectoAsesor"] = Codproyecto.ToString();
            querystringSeguro["codconvocatoriaAsesor"] = CodConvocatoria.ToString();
            querystringSeguro["tipoasesor"] = para.ToString();
            Redirect(null, "../evaluacion/InfoAsesor.aspx?a=" + HttpUtility.UrlEncode(querystringSeguro.ToString()), "_blank", "menubar=0,scrollbars=1,width=510,height=200,top=50");


        }

        void LlenarDtlistProyectoAcreditacion()
        {
            try
            {
                consultas.Parameters = null;

                if (Codproyecto != 0 && CodConvocatoria != 0)
                {
                    consultas.Parameters = new[]
                                           {

                                               new SqlParameter
                                                   {
                                                       ParameterName = "@codproyecto",Value = Codproyecto

                                                   },new SqlParameter
                                                         {
                                                             ParameterName = "@codConvocatoria",Value = CodConvocatoria
                                                         }
                                           };

                    var dtResultConvocatorias =
                        consultas.ObtenerDataTable("MD_ObtenerInformacionProyectoAcreditacion_");

                    if (dtResultConvocatorias.Rows.Count != 0)
                    {
                        foreach (DataRow dtrow in dtResultConvocatorias.Rows)
                        {
                            //if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONACREDITADO"].ToString()))
                            //{
                            //    txtObservaciones.Text = txtObservaciones.Text +
                            //                            dtResultConvocatorias.Rows[0]["OBSERVACIONACREDITADO"].ToString();
                            //}
                            //else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONSUBSANADO"].ToString()))
                            //{
                            //    txtObservaciones.Text = txtObservaciones.Text +
                            //                            dtResultConvocatorias.Rows[0]["OBSERVACIONSUBSANADO"].ToString();
                            //}
                            //else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONPENDIENTE"].ToString()))
                            //{
                            //    txtObservaciones.Text = txtObservaciones.Text +
                            //                            dtResultConvocatorias.Rows[0]["OBSERVACIONPENDIENTE"].ToString();
                            //}
                            //else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONNOACREDITADO"].ToString()))
                            //{

                            //    txtObservaciones.Text = txtObservaciones.Text +
                            //                            dtResultConvocatorias.Rows[0]["OBSERVACIONNOACREDITADO"].ToString();
                            //}
                        }

                        HttpContext.Current.Session["dtResultConvocatoria"] = dtResultConvocatorias;
                        DtlProyectoConvocatoria.DataSource = dtResultConvocatorias;
                        DtlProyectoConvocatoria.DataBind();
                    }
                    else
                    {
                        DtlProyectoConvocatoria.DataSource = dtResultConvocatorias;
                        DtlProyectoConvocatoria.DataBind();
                    }


                }

            }
            catch (Exception exception)
            {

                throw new Exception(exception.Message);
            }
        }

        public static string Guardar()
        {

            return "";
        }

        #endregion

        #region Event botonera

        protected void hplAsesorLider_Click(object sender, EventArgs e)
        {
            VerInformacionAseso("lider");
        }

        protected void hplAsesor_Click(object sender, EventArgs e)
        {
            VerInformacionAseso("asesor");
        }

        protected void hplnotificaciones_Click(object sender, EventArgs e)
        {
            HttpContext.Current.Session["codpNotificacion"] = Codproyecto;
            HttpContext.Current.Session["codcNotificacion"] = CodConvocatoria;
            Redirect(null, "../evaluacion/NotificacionesEnviadas.aspx", "_blank", "menubar=0,scrollbars=1,width=610,height=280,top=70");
        }

        protected void lbvertodos_Click(object sender, EventArgs e)
        {
            Redirect(null, "../evaluacion/CrifIngresados.aspx", "_blank", "menubar=0,scrollbars=1,width=580,height=200,top=70");
        }


        protected void Btnguardar_Click(object sender, EventArgs e)
        {
            #region Diego Quiñonez 12 de Diciembre de 2014
            if (modificarEstado())
            {
                guardar();
                MostrarObservacion();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", " alert('La información de los emprendedores está incompleta.');", true);
                return;
            }
            #endregion
        }

        protected void btnfinalizar_Click(object sender, EventArgs e)
        {

            #region Diego Quiñonez 12 de Diciembre de 2014
            if (!string.IsNullOrEmpty(txtObservaciones.Text))
                Actualizar();
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", " alert('Debe especificar una observación');", true);
                return;
            }
            #endregion
        }

        #endregion

        #region Eventos Datalis

        protected void DtlProyectoConvocatoriaItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                #region Diego Quiñonez

                var imgPendiente = (ImageButton)e.Item.FindControl("imgPendiente");
                var imgsubsanado = (ImageButton)e.Item.FindControl("imgsubsanado");
                var imgAcreditado = (ImageButton)e.Item.FindControl("imgAcreditado");
                var imgNoAcreditado = (ImageButton)e.Item.FindControl("imgNoAcreditado");

                var ch1 = (CheckBox)e.Item.FindControl("ch1");
                var ch2 = (CheckBox)e.Item.FindControl("ch2");
                var ch3 = (CheckBox)e.Item.FindControl("ch3");
                var chdi = (CheckBox)e.Item.FindControl("chdi");

                var chCertificaion = (CheckBox)e.Item.FindControl("chCertificaion");
                var chDiplomas = (CheckBox)e.Item.FindControl("chDiplomas");
                var chActas = (CheckBox)e.Item.FindControl("chActas");

                if (Lectura)
                {
                    if (chCertificaion != null) chCertificaion.Enabled = false;
                    if (chDiplomas != null) chDiplomas.Enabled = false;
                    if (chActas != null) chActas.Enabled = false;
                    if (ch1 != null) ch1.Enabled = false;
                    if (ch2 != null) ch2.Enabled = false;
                    if (ch3 != null) ch3.Enabled = false;
                    if (chdi != null) chdi.Enabled = false;
                }

                if (bool.Parse(imgAcreditado.CommandArgument))
                {
                    //rdb.SelectedValue = "13";
                    imgAcreditado.ImageUrl = "../../Images/chulo.gif";
                }
                else
                    imgAcreditado.ImageUrl = "../../Images/icoSinObservacion.gif";

                if (bool.Parse(imgNoAcreditado.CommandArgument))
                {
                    //rdb.SelectedValue = "14";
                    imgNoAcreditado.ImageUrl = "../../Images/chulo.gif";
                }
                else
                    imgNoAcreditado.ImageUrl = "../../Images/icoSinObservacion.gif";

                if (bool.Parse(imgPendiente.CommandArgument))
                {
                    //rdb.SelectedValue = "15";
                    imgPendiente.ImageUrl = "../../Images/chulo.gif";
                }
                else
                    imgPendiente.ImageUrl = "../../Images/icoSinObservacion.gif";

                if (bool.Parse(imgsubsanado.CommandArgument))
                {
                    //rdb.SelectedValue = "16";
                    imgsubsanado.ImageUrl = "../../Images/chulo.gif";
                }
                else
                    imgsubsanado.ImageUrl = "../../Images/icoSinObservacion.gif";

                #endregion

                #region checkbox

                //  ********************* ***************************************//
                //var chPendiente = e.Item.FindControl("chPendiente") as Label;
                //var lblidproyecto = e.Item.FindControl("lblidproyecto") as Label;
                //var chsubsanado = e.Item.FindControl("chsubsanado") as Label;
                //var chAcreditado = e.Item.FindControl("chAcreditado") as Label;
                //var chNoAcreditado = e.Item.FindControl("chNoAcreditado") as Label;
                //var chCertificaion = e.Item.FindControl("chCertificaion") as CheckBox;
                //var chDiplomas = e.Item.FindControl("chDiplomas") as CheckBox;
                //var chActas = e.Item.FindControl("chActas") as CheckBox;

                //  ********************* ***************************************//
                //var ch1 = e.Item.FindControl("ch1") as CheckBox;
                //var ch2 = e.Item.FindControl("ch2") as CheckBox;
                //var ch3 = e.Item.FindControl("ch3") as CheckBox;
                //var chdi = e.Item.FindControl("chdi") as CheckBox;

                // validaciones pendientes //
                //if (chsubsanado != null) chsubsanado.Enabled = false;
                //if (Lectura)
                //{
                //    if (chCertificaion != null) chCertificaion.Enabled = false;
                //    if (chDiplomas != null) chDiplomas.Enabled = false;
                //    if (chActas != null) chActas.Enabled = false;
                //    if (ch1 != null) ch1.Enabled = false;
                //    if (ch2 != null) ch2.Enabled = false;
                //    if (ch3 != null) ch3.Enabled = false;
                //    if (chdi != null) chdi.Enabled = false;
                //}

                // Acreditado //

                //if (chAcreditado.Text == "True")
                //{
                //    chAcreditado.Text = "<img src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                //    rdb.SelectedValue = "13";
                //}
                //else
                //{
                //    chAcreditado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                //}

                // No Acreditado

                //if (chNoAcreditado.Text == "True")
                //{
                //    chNoAcreditado.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                //    rdb.SelectedValue = "14";
                //}
                //else
                //{
                //    chNoAcreditado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                //}

                // Pendiente //

                //if (chPendiente.Text == "True")
                //{
                //    rdb.SelectedValue = "15";
                //    chPendiente.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                //}
                //else
                //{
                //    chPendiente.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                //}

                // subsanando //

                //if (chsubsanado.Text == "True")
                //{
                //    rdb.SelectedValue = "16";
                //    chsubsanado.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                //}
                //else
                //{
                //    chsubsanado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                //}

                //*********************************///



                #endregion

            }
        }

        protected void DtlProyectoRowcommand(object source, DataListCommandEventArgs e)
        {
            //if (e.CommandName == "emprendedor")
            //{
            //    var querystringSeguro = CreateQueryStringUrl();
            //    querystringSeguro["codcontacto"] = e.CommandArgument.ToString();
            //    Redirect(null, "../evaluacion/InfoEmprendedor.aspx?cosws=" + HttpUtility.UrlEncode(querystringSeguro.ToString()), "_blank", "menubar=0,scrollbars=1,width=510,height=600,top=50");
            //}

            #region Diego Quiñonez

            var imgPendiente = (ImageButton)e.Item.FindControl("imgPendiente");
            var imgsubsanado = (ImageButton)e.Item.FindControl("imgsubsanado");
            var imgAcreditado = (ImageButton)e.Item.FindControl("imgAcreditado");
            var imgNoAcreditado = (ImageButton)e.Item.FindControl("imgNoAcreditado");

            var ch1 = (CheckBox)e.Item.FindControl("ch1");
            var ch2 = (CheckBox)e.Item.FindControl("ch2");
            var ch3 = (CheckBox)e.Item.FindControl("ch3");
            var chdi = (CheckBox)e.Item.FindControl("chdi");

            var chCertificaion = (CheckBox)e.Item.FindControl("chCertificaion");
            var chDiplomas = (CheckBox)e.Item.FindControl("chDiplomas");
            var chActas = (CheckBox)e.Item.FindControl("chActas");

            var lnkEmprendedor = (LinkButton)e.Item.FindControl("lnbEmprendedor");

            int idContacto = int.Parse(lnkEmprendedor.CommandArgument);
            HttpContext.Current.Session["indeceFilaDataList"] = e.Item.ItemIndex;
            HttpContext.Current.Session["Id_Emprendedor"] = idContacto;
            HttpContext.Current.Session["NomEstado"] = e.CommandName;
            HttpContext.Current.Session["imgPendiente"] = imgPendiente.CommandArgument;
            HttpContext.Current.Session["imgsubsanado"] = imgsubsanado.CommandArgument;
            HttpContext.Current.Session["imgAcreditado"] = imgAcreditado.CommandArgument;
            HttpContext.Current.Session["imgNoAcreditado"] = imgNoAcreditado.CommandArgument;

            switch (e.CommandName)
            {
                case "emprendedor":
                    HttpContext.Current.Session["infoEmprendedorCodContacto"] = e.CommandArgument;
                    Redirect(null, "../evaluacion/InfoEmprendedor.aspx", "_blank", "menubar=0,scrollbars=1,width=510,height=600,top=50");
                    break;
                default:
                    if (e.CommandArgument != null && !string.IsNullOrEmpty(e.CommandArgument.ToString()))
                    {
                        switch (e.CommandName)
                        {
                            case "Pendiente":
                                if (bool.Parse(imgAcreditado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Pendiente\" si el estado del emprendedor es \"Acreditado\"');", true);
                                    return;
                                }
                                else if (bool.Parse(imgNoAcreditado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Pendiente\" si el estado del emprendedor es \"No Acreditado\"');", true);
                                    return;
                                }
                                break;
                            case "Subsanado":
                                if (bool.Parse(imgNoAcreditado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Subsanado\" si el estado del emprendedor es \"No Acreditado\"');", true);
                                    return;
                                }
                                break;
                            case "Acreditado":
                                if (bool.Parse(imgNoAcreditado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Acreditado\" si el estado del emprendedor es \"No Acreditado\"');", true);
                                    return;
                                }
                                else if (bool.Parse(imgPendiente.CommandArgument) && !bool.Parse(imgsubsanado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Acreditado\" si el estado del emprendedor es \"Pendiente\"');", true);
                                    return;
                                }
                                else if (!ch1.Checked || !ch2.Checked || !ch3.Checked || !chdi.Checked)
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"Acreditado\" si los anexos no están completos (1,2,3 y DI)');", true);
                                    return;
                                }
                                break;
                            case "NoAcreditado":
                                if (bool.Parse(imgAcreditado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"No Acreditado\" si el estado del emprendedor es \"Acreditado\"');", true);
                                    return;
                                }
                                else if (bool.Parse(imgsubsanado.CommandArgument))
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede activar el estado \"No Acreditado\" si el estado del emprendedor es \"Subsanado\"');", true);
                                    return;
                                }
                                break;
                        }
                        //Redirect(null, "ObservacionAcreditacion.aspx", "_blank", "width=650,height=350");
                        validarDatosParaCambioEstado();

                        pnlContenidoAcreditacion.Visible = true;
                        pnlContenidoAcreditacion.Enabled = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Error en [mostrarContenido] Exception: undefined');", true);
                    }
                    break;
            }

            #endregion
        }

        #endregion

        #region Validaciones


        void ValidaBotones()
        {
            if (CodEstado == Constantes.CONST_Acreditado || CodEstado == Constantes.CONST_No_acreditado)
            {
                txtObservaciones.Text = Observacionfinal;

                if (CodEstado == Constantes.CONST_No_acreditado && string.IsNullOrEmpty(Observacionfinal))
                {
                    txtObservaciones.Text = Constantes.CONST_TextoObsNoAcreditado;
                }
                else if (CodEstado == Constantes.CONST_Acreditado && string.IsNullOrEmpty(Observacionfinal))
                {
                    txtObservaciones.Text = Constantes.CONST_TextoObsAcreditado;
                }
                if (!Lectura)
                {
                    MostrarObservacion();
                }
            }
            else
            {
                btnfinalizar.Visible = false;
                txtObservaciones.Visible = false;
                lblObservaciones.Visible = false;
            }
        }

        private void MostrarObservacion()
        {
            btnfinalizar.Visible = true;
            Btnguardar.Visible = true;
            txtObservaciones.Visible = true;//muestra el campo observaciones cuando el proyecto fue aprobado o no aprobado
            lblObservaciones.Visible = true;
        }

        void guardar()
        {

            #region Diego Quiñonez - 12 de Diciembre de 2014

            foreach (DataListItem dtl in DtlProyectoConvocatoria.Items)
            {
                var proyectoId = (Label)dtl.FindControl("lblidproyecto");
                var imgPendiente = (ImageButton)dtl.FindControl("imgPendiente");
                var imgsubsanado = (ImageButton)dtl.FindControl("imgsubsanado");
                var imgAcreditado = (ImageButton)dtl.FindControl("imgAcreditado");
                var imgNoAcreditado = (ImageButton)dtl.FindControl("imgNoAcreditado");

                var txtPendiente = (TextBox)dtl.FindControl("txtParametrosPendiente");
                var txtubsanado = (TextBox)dtl.FindControl("txtParametrossubsanado");
                var txtAcreditado = (TextBox)dtl.FindControl("txtParametrosAcreditado");
                var txtNoAcreditado = (TextBox)dtl.FindControl("txtParametrosNoAcreditado");

                var ch1 = (CheckBox)dtl.FindControl("ch1");
                var ch2 = (CheckBox)dtl.FindControl("ch2");
                var ch3 = (CheckBox)dtl.FindControl("ch3");
                var chdi = (CheckBox)dtl.FindControl("chdi");

                var chCertificaion = (CheckBox)dtl.FindControl("chCertificaion");
                var chDiplomas = (CheckBox)dtl.FindControl("chDiplomas");
                var chActas = (CheckBox)dtl.FindControl("chActas");

                ProyectoAcreditacionDocumento padProyecto = (from pad in consultas.Db.ProyectoAcreditacionDocumentos
                                                             where pad.Id_ProyectoAcreditacionDocumento == int.Parse(proyectoId.Text)
                                                             select pad).First();

                if (padProyecto != null)
                {
                    padProyecto.Pendiente = bool.Parse(imgPendiente.CommandArgument);
                    padProyecto.Subsanado = bool.Parse(imgsubsanado.CommandArgument);
                    padProyecto.Acreditado = bool.Parse(imgAcreditado.CommandArgument);
                    padProyecto.NoAcreditado = bool.Parse(imgNoAcreditado.CommandArgument);

                    padProyecto.FlagActa = chActas.Checked;
                    padProyecto.FlagDiploma = chDiplomas.Checked;
                    padProyecto.FlagCertificaciones = chCertificaion.Checked;
                    padProyecto.FlagAnexo1 = ch1.Checked;
                    padProyecto.FlagAnexo2 = ch2.Checked;
                    padProyecto.FlagAnexo3 = ch3.Checked;
                    padProyecto.FlagDI = chdi.Checked;

                    string[] paramterosEstado;

                    if (string.IsNullOrEmpty(txtPendiente.Text))
                    {
                        padProyecto.AsuntoPendiente = null;
                        padProyecto.FechaPendiente = null;
                        padProyecto.ObservacionPendiente = null;
                    }
                    else
                    {
                        paramterosEstado = txtPendiente.Text.Split(';');
                        padProyecto.ObservacionPendiente = !string.IsNullOrEmpty(paramterosEstado[0]) ? paramterosEstado[0] : null;
                        padProyecto.AsuntoPendiente = !string.IsNullOrEmpty(paramterosEstado[1]) ? paramterosEstado[1] : null;

                        if (!string.IsNullOrEmpty(paramterosEstado[2]))
                            padProyecto.FechaPendiente = DateTime.Parse(paramterosEstado[2]);
                        else
                            padProyecto.FechaPendiente = null;
                    }

                    if (string.IsNullOrEmpty(txtubsanado.Text))
                    {
                        padProyecto.AsuntoSubsanado = null;
                        padProyecto.FechaSubsanado = null;
                        padProyecto.ObservacionSubsanado = null;
                    }
                    else
                    {
                        paramterosEstado = txtubsanado.Text.Split(';');
                        padProyecto.ObservacionSubsanado = !string.IsNullOrEmpty(paramterosEstado[0]) ? paramterosEstado[0] : null;
                        padProyecto.AsuntoSubsanado = !string.IsNullOrEmpty(paramterosEstado[1]) ? paramterosEstado[1] : null;

                        if (!string.IsNullOrEmpty(paramterosEstado[2]))
                            padProyecto.FechaSubsanado = DateTime.Parse(paramterosEstado[2]);
                        else
                            padProyecto.FechaSubsanado = null;
                    }

                    if (string.IsNullOrEmpty(txtAcreditado.Text))
                    {
                        padProyecto.AsuntoAcreditado = null;
                        padProyecto.FechaAcreditado = null;
                        padProyecto.ObservacionAcreditado = null;
                    }
                    else
                    {
                        paramterosEstado = txtAcreditado.Text.Split(';');
                        padProyecto.ObservacionAcreditado = !string.IsNullOrEmpty(paramterosEstado[0]) ? paramterosEstado[0] : null;
                        padProyecto.AsuntoAcreditado = !string.IsNullOrEmpty(paramterosEstado[1]) ? paramterosEstado[1] : null;

                        if (!string.IsNullOrEmpty(paramterosEstado[2]))
                            padProyecto.FechaAcreditado = DateTime.Parse(paramterosEstado[2]);
                        else
                            padProyecto.FechaAcreditado = null;
                    }

                    if (string.IsNullOrEmpty(txtNoAcreditado.Text))
                    {
                        padProyecto.AsuntoNoAcreditado = null;
                        padProyecto.FechaNoAcreditado = null;
                        padProyecto.ObservacionNoAcreditado = null;
                    }
                    else
                    {
                        paramterosEstado = txtNoAcreditado.Text.Split(';');
                        padProyecto.ObservacionNoAcreditado = !string.IsNullOrEmpty(paramterosEstado[0]) ? paramterosEstado[0] : null;
                        padProyecto.AsuntoNoAcreditado = !string.IsNullOrEmpty(paramterosEstado[1]) ? paramterosEstado[1] : null;


                        if (!string.IsNullOrEmpty(paramterosEstado[2]))
                            padProyecto.FechaNoAcreditado = DateTime.Parse(paramterosEstado[2]);
                        else
                            padProyecto.FechaNoAcreditado = null;
                    }

                    //consultas.Db.SubmitChanges();
                }
            }

            if (!string.IsNullOrEmpty(txtcrif.Text))
            {
                ProyectoAcreditaciondocumentosCRIF rCirf;

                try
                {
                    rCirf = (from padc in consultas.Db.ProyectoAcreditaciondocumentosCRIFs
                             where padc.CodProyecto == Codproyecto
                             && padc.Crif == txtcrif.Text
                             select padc).First();
                }
                catch (InvalidOperationException)
                {
                    //si entra al cath deja el objeto nulo para poder crear uno nuevo
                    rCirf = null;
                }

                if (rCirf == null)
                {
                    ProyectoAcreditaciondocumentosCRIF padc = new ProyectoAcreditaciondocumentosCRIF();

                    padc.CodProyecto = Codproyecto;
                    padc.CodConvocatoria = CodConvocatoria;
                    padc.Crif = txtcrif.Text;
                    padc.Fecha = DateTime.Now;
                    consultas.Db.ProyectoAcreditaciondocumentosCRIFs.InsertOnSubmit(padc);

                    consultas.Db.SubmitChanges();
                }
            }

            if (int.Parse(rdb.SelectedValue) != codEstado)
            {
                //InsertarProyectoAcreditacion(int.Parse(rdb.SelectedValue), string.Empty);
                // 2015/06/13 Roberto Alvarado, se adiciona la Observacion pues siempre se envia vacia
                InsertarProyectoAcreditacion(int.Parse(rdb.SelectedValue), txtObservacion.Text);

                Datos.Proyecto pr = (from p in consultas.Db.Proyecto
                                     where p.Id_Proyecto == Codproyecto
                                     select p).First();

                if (pr != null)
                {
                    switch (int.Parse(rdb.SelectedValue))
                    {
                        case Constantes.CONST_Acreditado:
                            pr.CodEstado = Constantes.CONST_Aprobacion_Acreditacion;
                            break;
                        case Constantes.CONST_No_acreditado:
                            pr.CodEstado = Constantes.CONST_Aprobacion_No_Acreditacion;
                            break;
                        default:
                            pr.CodEstado = byte.Parse(rdb.SelectedValue);
                            EliminarProyectoDeActa();
                            break;
                    }
                }
            }

            consultas.Db.SubmitChanges();
            #endregion

            #region anterior


            //foreach (DataListItem listItem in DtlProyectoConvocatoria.Items)
            //{
            //    if (listItem.ItemType == ListItemType.Item || listItem.ItemType == ListItemType.AlternatingItem)
            //    {



            //        var proyectoId = listItem.FindControl("lblidproyecto") as Label;
            //        var pendiente = listItem.FindControl("lchPendiente") as Label;
            //        var subsanado = listItem.FindControl("lchsubsanado") as Label;
            //        var acredidato = listItem.FindControl("lchAcreditado") as Label;
            //        var noacreditado = listItem.FindControl("lchNoAcreditado") as Label;

            //        //

            //        var certificacion = listItem.FindControl("chCertificaion") as CheckBox;
            //        var diplomas = listItem.FindControl("chDiplomas") as CheckBox;
            //        var actas = listItem.FindControl("chActas") as CheckBox;

            //        var ch1 = listItem.FindControl("ch1") as CheckBox;
            //        var ch2 = listItem.FindControl("ch2") as CheckBox;
            //        var ch3 = listItem.FindControl("ch3") as CheckBox;
            //        var chdi = listItem.FindControl("chdi") as CheckBox;

            //        // busco la entidad para modificarla

            //        var proyectoDoc =
            //            consultas.Db.ProyectoAcreditacionDocumentos.FirstOrDefault(pd => pd.Id_ProyectoAcreditacionDocumento == Convert.ToInt32(proyectoId.Text));

            //        if (proyectoDoc != null)
            //        {

            //            // 
            //            if (pendiente != null)
            //            {
            //                if (pendiente.Text == "True")
            //                {
            //                    proyectoDoc.Pendiente = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.Pendiente = false;
            //                    proyectoDoc.AsuntoPendiente = null;
            //                    proyectoDoc.FechaPendiente = null;
            //                    proyectoDoc.ObservacionPendiente = null;
            //                }
            //            }

            //            if (subsanado != null)
            //            {
            //                if (subsanado.Text == "True")
            //                {
            //                    proyectoDoc.Subsanado = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.Subsanado = false;
            //                    proyectoDoc.AsuntoSubsanado = null;
            //                    proyectoDoc.FechaSubsanado = null;
            //                    proyectoDoc.ObservacionSubsanado = null;
            //                }
            //            }

            //            if (acredidato != null)
            //            {
            //                if (acredidato.Text == "True")
            //                {
            //                    proyectoDoc.Acreditado = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.Pendiente = false;
            //                    proyectoDoc.AsuntoPendiente = null;
            //                    proyectoDoc.FechaPendiente = null;
            //                    proyectoDoc.ObservacionPendiente = null;
            //                }
            //            }

            //            if (noacreditado != null)
            //            {
            //                if (noacreditado.Text == "True")
            //                {
            //                    proyectoDoc.NoAcreditado = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.NoAcreditado = false;
            //                    proyectoDoc.AsuntoNoAcreditado = null;
            //                    proyectoDoc.FechaNoAcreditado = null;
            //                    proyectoDoc.ObservacionNoAcreditado = null;
            //                }
            //            }


            //            if (certificacion != null)
            //            {
            //                if (certificacion.Checked)
            //                {
            //                    proyectoDoc.FlagCertificaciones = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagCertificaciones = false;

            //                }
            //            }

            //            if (diplomas != null)
            //            {
            //                if (diplomas.Checked)
            //                {
            //                    proyectoDoc.FlagDiploma = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagDiploma = false;

            //                }
            //            }

            //            if (actas != null)
            //            {
            //                if (actas.Checked)
            //                {
            //                    proyectoDoc.FlagActa = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagActa = false;

            //                }
            //            }

            //            if (ch1 != null)
            //            {
            //                if (ch1.Checked)
            //                {
            //                    proyectoDoc.FlagAnexo1 = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagAnexo1 = false;

            //                }
            //            }

            //            if (ch2 != null)
            //            {
            //                if (ch2.Checked)
            //                {
            //                    proyectoDoc.FlagAnexo2 = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagAnexo2 = false;

            //                }
            //            }

            //            if (ch3 != null)
            //            {
            //                if (ch3.Checked)
            //                {
            //                    proyectoDoc.FlagAnexo3 = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagAnexo3 = false;

            //                }
            //            }

            //            if (chdi != null)
            //            {
            //                if (chdi.Checked)
            //                {
            //                    proyectoDoc.FlagDI = true;
            //                }
            //                else
            //                {
            //                    proyectoDoc.FlagDI = false;

            //                }
            //            }

            //            consultas.Db.SubmitChanges();

            //            if (!string.IsNullOrEmpty(txtcrif.Text))
            //            {
            //                var crif =
            //                    consultas.Db.ProyectoAcreditaciondocumentosCRIFs.FirstOrDefault(
            //                        pc => pc.Crif == txtcrif.Text
            //                              && pc.CodProyecto == Codproyecto);
            //                if (crif == null)
            //                {
            //                    var crifNew = new ProyectoAcreditaciondocumentosCRIF();

            //                    crifNew.CodProyecto = Codproyecto;
            //                    crifNew.CodConvocatoria = CodConvocatoria;
            //                    crifNew.Crif = txtcrif.Text;
            //                    crifNew.Fecha = DateTime.Now;
            //                    consultas.Db.ProyectoAcreditaciondocumentosCRIFs.InsertOnSubmit(crifNew);
            //                    consultas.Db.SubmitChanges();
            //                }
            //            }

            //        }




            //    }
            //}

            #endregion


            #region anterior
            //if (CodEstado != CodEstado)
            //{
            //    InsertarProyectoAcreditacion(codEstado, string.Empty);

            //    var proyecto = consultas.Db.Proyectos.FirstOrDefault(p => p.Id_Proyecto == Codproyecto);

            //    if (CodEstado == Constantes.CONST_Acreditado)
            //    {
            //        if (proyecto != null)
            //        {
            //            proyecto.CodEstado = Constantes.CONST_Aprobacion_Acreditacion;
            //        }
            //    }
            //    else if (CodEstado == Constantes.CONST_No_acreditado)
            //    {
            //        proyecto.CodEstado = Constantes.CONST_Aprobacion_No_Acreditacion;
            //    }
            //    else
            //    {
            //        proyecto.CodEstado = byte.Parse(rdb.SelectedValue);
            //        EliminarProyectoDeActa();
            //    }
            //}
            #endregion


        }

        void Actualizar()
        {
            try
            {
                var proyecto = consultas.Db.Proyecto.FirstOrDefault(p => p.Id_Proyecto == Codproyecto);

                if (proyecto != null)
                {

                    if (int.Parse(rdb.SelectedValue) == Constantes.CONST_Acreditado)
                    {
                        proyecto.CodEstado = Constantes.CONST_Aprobacion_Acreditacion;

                    }
                    else if (int.Parse(rdb.SelectedValue) == Constantes.CONST_No_acreditado)
                    {
                        proyecto.CodEstado = Constantes.CONST_Aprobacion_No_Acreditacion;
                    }

                    consultas.Db.SubmitChanges();

                    InsertarProyectoAcreditacion(int.Parse(rdb.SelectedValue), txtObservaciones.Text);
                }

                Response.Redirect("../interventoria/PlanesaAcreditar.aspx");
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);

            }
        }

        #endregion


        #region Diego Quiñonez - 12 de Diciembre de 2014

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            cargarSesion();

            modificarContenido();

            Btnguardar_Click(sender, e);
        }

        protected void btnGuardarEnviar_Click(object sender, EventArgs e)
        {
            cargarSesion();

            // 2015/06/11 Roberto Alvarado
            // Se comenta esta pregunta para que siempre haga los pasos de grabacion
            //if (validarEnviarContenido() && validarModificarContenido())
            //{
            pnlContenidoAcreditacion.Visible = false;
            pnlContenidoAcreditacion.Enabled = false;

            dtxtVistaEmail.InnerHtml = getTextoEmail();

            pnlCOntenidoEmail.Visible = true;
            pnlCOntenidoEmail.Enabled = true;

            GrabarAcreditacionEstado();

            Btnguardar_Click(sender, e);

            //}
        }

        /// <summary> Metodo que Guarda en la tabla HistoricoEmailAcreditacion los datos del compromiso
        /// </summary>
        /// <remarks>Roberto Alvarado 2015/05/28</remarks>
        private void GrabarAcreditacionEstado()
        {
            // Lleno el Objeto
            HistoricoEmailAcreditacionEntity oAcreditacion = new HistoricoEmailAcreditacionEntity();
            ProyectoAcreditacionNegocio acredNeg = new ProyectoAcreditacionNegocio();
            try
            {
                oAcreditacion.CodContacto = Id_Emprendedor;
                oAcreditacion.CodContactoEnvia = usuario.IdContacto;
                oAcreditacion.CodConvocatoria = Convert.ToInt32(HttpContext.Current.Session["CodConvocatoria"]);

                if (lblEstadoR.Text == "Pendiente")
                    oAcreditacion.CodEstadoAcreditacion = Constantes.CONST_Pendiente;
                else if (lblEstadoR.Text == "Subsanado")
                    oAcreditacion.CodEstadoAcreditacion = Constantes.CONST_Subsanado;
                else if (lblEstadoR.Text == "Acreditado")
                    oAcreditacion.CodEstadoAcreditacion = Constantes.CONST_Acreditado;
                else if (lblEstadoR.Text == "NoAcreditado")
                    oAcreditacion.CodEstadoAcreditacion = Constantes.CONST_No_acreditado;

                oAcreditacion.CodProyecto = Convert.ToInt32(HttpContext.Current.Session["CodProyecto"]);
                oAcreditacion.Email = txtObservacion.Text;
                oAcreditacion.Fecha = DateTime.Now;

                acredNeg.Agregar(oAcreditacion);

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Destriir();
        }

        protected void btnCancelarEmail_Click(object sender, EventArgs e)
        {
            pnlCOntenidoEmail.Visible = false;
            pnlCOntenidoEmail.Enabled = false;

            pnlContenidoAcreditacion.Visible = true;
            pnlContenidoAcreditacion.Enabled = true;
        }

        protected void EnviarEmail_Click(object sender, EventArgs e)
        {
            cargarSesion();

            if (validarEnviarContenido() && validarModificarContenido())
            {
                enviarEmail();
                modificarContenido();
                pnlCOntenidoEmail.Visible = false;
                pnlCOntenidoEmail.Enabled = false;
                // 2015/06/22 Roberto Alvarado Para guardar los datos de manera corecta
                btnGuardar_Click(sender, e);
            }
        }

        private void enviarEmail()
        {
            // Enviar email a Asesor Lider, Asesor, Emprendedor y Acreditador
            // By Marcel Solera @marztres
            // 23 de 07 de 2015
            try
            {
                //Abreviaturas de los roles a enviar correo.
                string[] rolesAsociados = { "L", "A", "E", "ACR" };

                var ContactosProyecto = (from proyectocontacto in consultas.Db.ProyectoContactos
                                         join contacto in consultas.Db.Contacto on proyectocontacto.CodContacto equals contacto.Id_Contacto
                                         join rolcontacto in consultas.Db.Rols on proyectocontacto.CodRol equals rolcontacto.Id_Rol
                                         where proyectocontacto.Inactivo == false && rolesAsociados.Contains(rolcontacto.Abreviacion)
                                               && proyectocontacto.CodProyecto == Codproyecto
                                         select contacto).ToList();

                foreach (var contacto in ContactosProyecto)
                {
                    Correo correo = new Correo(
                                                usuario.Email,
                                                usuario.Nombres + " " + usuario.Apellidos,
                                                contacto.Email,
                                                lblEmprendedor.Text,
                                                txtAsunto.Text,
                                                dtxtVistaEmail.InnerText);
                    correo.Enviar();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Mensaje", "alert('Sucedio un error al enviar correos, detalle : " + ex.Message + " ');", true);
            }

        }

        private void modificarContenido()
        {
            if (validarModificarContenido())
            {
                TextBox txt = null;
                ImageButton imgbtn = null;

                if (ckkEstado.Checked)
                {
                    switch (NomEstado)
                    {
                        case "Pendiente":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosPendiente");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgPendiente"));
                            break;
                        case "Subsanado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrossubsanado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgsubsanado"));
                            break;
                        case "Acreditado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosAcreditado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgAcreditado"));
                            break;
                        case "NoAcreditado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosNoAcreditado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgNoAcreditado"));
                            break;
                    }

                    if (txt != null)
                        txt.Text = txtObservacion.Text + ";" + txtAsunto.Text + ";" + txtFecha.Text;

                    if (imgbtn != null)
                    {
                        imgbtn.ImageUrl = "../../Images/chulo.gif";
                        imgbtn.CommandArgument = "true";
                    }

                }
                else
                {
                    switch (NomEstado)
                    {
                        case "Pendiente":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosPendiente");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgPendiente"));
                            break;
                        case "Subsanado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrossubsanado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgsubsanado"));
                            break;
                        case "Acreditado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosAcreditado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgAcreditado"));
                            break;
                        case "NoAcreditado":
                            txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosNoAcreditado");
                            imgbtn = ((ImageButton)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("imgNoAcreditado"));
                            break;
                    }

                    if (txt != null)
                        txt.Text = string.Empty;

                    if (imgbtn != null)
                    {
                        imgbtn.ImageUrl = "../../Images/icoSinObservacion.gif";
                        imgbtn.CommandArgument = "false";
                    }
                }
                Destriir();
            }
        }

        private bool validarModificarContenido()
        {
            bool result = true;

            if (ckkEstado.Checked && string.IsNullOrEmpty(txtObservacion.Text))
            {
                result = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Debe especificar una observación');", true);
                txtObservacion.Focus();
            }
            else if (!ckkEstado.Checked)
            {
                result = puedeDesactivarse();
            }

            var observacionSinCaracteresEspeciales = txtObservacion.Text.Replace(";", "");

            txtObservacion.Text = observacionSinCaracteresEspeciales;

            return result;
        }

        private bool puedeDesactivarse()
        {
            bool result = true;

            switch (NomEstado)
            {
                case "Subsanado":
                    if (EstPendiente && EstAcreditado)
                    {
                        result = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('No puede Desactivar el estado \"Subsanado\" si se encuentran activados los estados \"Pendiente\" y \"Acreditado\"');", true);
                    }
                    break;
            }

            return result;
        }

        private void validarDatosParaCambioEstado()
        {
            cargarSesion();

            TextBox txt = null;

            lblEmprendedor.Text = (from c in consultas.Db.Contacto where c.Id_Contacto == Id_Emprendedor select (c.Nombres + " " + c.Apellidos)).First();
            lblEstado.Text = NomEstado;
            lblEstadoR.Text = NomEstado;

            switch (NomEstado)
            {
                case "Pendiente":
                    txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosPendiente");
                    //ckkEstado.Checked = EstPendiente;
                    break;
                case "Subsanado":
                    txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrossubsanado");
                    //ckkEstado.Checked = Estsubsanado;
                    break;
                case "Acreditado":
                    txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosAcreditado");
                    //ckkEstado.Checked = EstAcreditado;
                    break;
                case "NoAcreditado":
                    txt = (TextBox)DtlProyectoConvocatoria.Items[indeceFilaDataList].FindControl("txtParametrosNoAcreditado");
                    //ckkEstado.Checked = EstNoAcreditado;
                    break;
            }

            string[] parametros;

            if (txt == null || string.IsNullOrEmpty(txt.Text))
            {
                txtFecha.Text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
                txtAsunto.Text = "Plan de Negocios # " + Codproyecto;
            }
            else
            {
                parametros = txt.Text.Split(';');
                txtObservacion.Text = parametros[0];
                txtAsunto.Text = !string.IsNullOrEmpty(parametros[1]) ? parametros[1] : "Plan de Negocios # " + Codproyecto;
                txtFecha.Text = !string.IsNullOrEmpty(parametros[2]) ? parametros[2] : DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            }
        }

        private bool validarEnviarContenido()
        {
            bool result = true;

            if (!ckkEstado.Checked)
            {
                result = false;
                ckkEstado.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Debe activar la casilla de verificación');", true);
                return result;
            }

            if (string.IsNullOrEmpty(txtAsunto.Text))
            {
                result = false;
                txtAsunto.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Debe especificar el asunto');", true);
                return result;
            }

            if (string.IsNullOrEmpty(txtObservacion.Text))
            {
                result = false;
                txtObservacion.Focus();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Error", "alert('Debe especificar una observación');", true);
                return result;
            }

            return result;
        }

        private string getTextoEmail()
        {
            StringBuilder result = new StringBuilder();

            del myDelegate = (x) =>
            {
                if (string.IsNullOrEmpty(x))
                    return "Fonade";
                else
                    return x;
            };

            var rs = (from c in consultas.Db.Contacto
                      join i in consultas.Db.Institucions on c.CodInstitucion equals i.Id_Institucion into inst
                      from ins in inst.DefaultIfEmpty()
                      where c.Id_Contacto == usuario.IdContacto
                      select new
                      {
                          c.Nombres,
                          c.Apellidos,
                          c.Email,
                          c.Cargo,
                          c.Telefono,
                          c.Direccion,
                          NomInstitucion = myDelegate(ins.NomInstitucion),
                          c.codOperador
                      }).FirstOrDefault();

            string _cadena = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

            string correoAcreditacion = "";
            string nomOperador = "";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                correoAcreditacion = (from e in db.Operador
                                      where e.IdOperador == usuario.CodOperador
                                      select e.EmailObservacionAcreditacion).FirstOrDefault();

                nomOperador = (from e in db.Operador
                               where e.IdOperador == usuario.CodOperador
                               select e.NombreOperador).FirstOrDefault();
            }



            if (rs != null)
            {
                result.AppendLine("<html>");
                result.AppendLine("<body>");
                result.AppendLine("<div id=\"cabeza\">");
                result.AppendLine("<h1 style=\"float:left);\">" + lblnomproyecto.Text + "</h1>");
                //result.AppendLine( "<h4 style=\"float:right);\">" + txtFecha.Text + "</h4>");
                result.AppendLine("</div>");
                result.AppendLine("<br>");
                result.AppendLine("<p style=\"clear:both); font-size:18px);\">" + txtObservacion.Text + "</p>");
                result.AppendLine("<br>");
                result.AppendLine("<div id=\"pie\" style=\"font-size:12px);\">");
                result.AppendLine("<span>" + rs.Nombres + " " + rs.Apellidos + "</span><br>");
                result.AppendLine("<span>" + correoAcreditacion + "</span><br>");
                //result.AppendLine("<span>" + rs.Cargo + "</span><br>");
                //result.AppendLine("<span>" + rs.Direccion + "</span><br>");
                result.AppendLine("<span>" + nomOperador + "</span><br>");
                //result.AppendLine("<span>" + rs.Telefono + "</span><br>");
                result.AppendLine("<br/>");
                //result.AppendLine("<h1><b>Observación realizada al Plan de Negocio Se debe solucionar antes del " + txtFecha.Text + "<b/></h1>");
                result.AppendLine("</div>");
                result.AppendLine("</body>");
                result.AppendLine("</html>");
            }

            return result.ToString();
        }

        private void cargarSesion()
        {
            indeceFilaDataList = HttpContext.Current.Session["indeceFilaDataList"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["indeceFilaDataList"].ToString()) ? int.Parse(HttpContext.Current.Session["indeceFilaDataList"].ToString()) : 0;
            Id_Emprendedor = HttpContext.Current.Session["Id_Emprendedor"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["Id_Emprendedor"].ToString()) ? int.Parse(HttpContext.Current.Session["Id_Emprendedor"].ToString()) : 0;
            NomEstado = HttpContext.Current.Session["NomEstado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["NomEstado"].ToString()) ? HttpContext.Current.Session["NomEstado"].ToString() : string.Empty;
            EstPendiente = HttpContext.Current.Session["imgPendiente"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgPendiente"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgPendiente"].ToString()) : false;
            Estsubsanado = HttpContext.Current.Session["imgsubsanado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgsubsanado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgsubsanado"].ToString()) : false;
            EstAcreditado = HttpContext.Current.Session["imgAcreditado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgAcreditado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgAcreditado"].ToString()) : false;
            EstNoAcreditado = HttpContext.Current.Session["imgNoAcreditado"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["imgNoAcreditado"].ToString()) ? bool.Parse(HttpContext.Current.Session["imgNoAcreditado"].ToString()) : false;
        }

        private void Destriir()
        {
            lblEmprendedor.Text = "";
            lblEstado.Text = "";
            txtFecha.Text = "";
            //ckkEstado.Checked = false;
            txtAsunto.Text = "";
            txtObservacion.Text = "";
            dtxtVistaEmail.InnerText = "";

            HttpContext.Current.Session["Id_Emprendedor"] = null;
            HttpContext.Current.Session["NomEstado"] = null;
            HttpContext.Current.Session["imgPendiente"] = null;
            HttpContext.Current.Session["imgsubsanado"] = null;
            HttpContext.Current.Session["imgAcreditado"] = null;
            HttpContext.Current.Session["imgNoAcreditado"] = null;

            pnlContenidoAcreditacion.Visible = false;
            pnlContenidoAcreditacion.Enabled = false;
        }

        #endregion


        #region modificar estado
        //Diego Quiñonez
        //12 de Diciembre de 2014

        private bool modificarEstado()
        {
            if (todosPoseenEstado("Acreditado;NoAcreditado"))
            {
                if (todosPoseenEstado("Acreditado"))
                {
                    rdb.SelectedValue = "13";
                    return true;
                }
                else
                {
                    rdb.SelectedValue = "14";
                    return true;
                }
            }

            if (todosPoseenEstado("subsanado"))
            {
                rdb.SelectedValue = "16";
                return true;
            }

            if (algunoPoseeEstado("Pendiente"))
            {
                rdb.SelectedValue = "15";
                return true;
            }

            if (!algunoPoseeEstado("Pendiente") && !algunoPoseeEstado("subsanado") && !algunoPoseeEstado("Acreditado") && !algunoPoseeEstado("NoAcreditado"))
            {
                rdb.SelectedValue = "10";
                return true;
            }

            return false;
        }

        private bool todosPoseenEstado(string nomEstado)
        {
            bool result = true;
            int lExaminados = 0;

            foreach (DataListItem dli in DtlProyectoConvocatoria.Items)
            {
                bool lResTmp = false;
                string[] Estados = nomEstado.Split(';');

                foreach (string est in Estados)
                {
                    var imgbtn = ((ImageButton)dli.FindControl("img" + est));

                    if (imgbtn != null)
                        lExaminados++;

                    if (imgbtn != null && bool.Parse(imgbtn.CommandArgument))
                    {
                        lResTmp = true;
                        break;
                    }
                }
                if (!lResTmp)
                {
                    result = false;
                    break;
                }
            }

            if (lExaminados == 0)
                result = false;

            return result;
        }

        private bool algunoPoseeEstado(string nomEstado)
        {
            bool result = false;

            foreach (DataListItem dli in DtlProyectoConvocatoria.Items)
            {
                var imgbtn = ((ImageButton)dli.FindControl("img" + nomEstado));

                if (bool.Parse(imgbtn.CommandArgument))
                {
                    result = true;
                    return result;
                }
            }

            return result;
        }

        #endregion

        protected void lnkDocumentosAcreditacion_Click(object sender, EventArgs e)
        {
            Redirect(null, "~/PlanDeNegocioV2/Formulacion/Anexos/DocumentosAcreditacion.aspx?codproyecto=" + Codproyecto, "_blank", "menubar=0,scrollbars=1,width=580,height=200,top=70");
        }
    }

}