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

namespace Fonade.FONADE.evaluacion
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
            get { return (bool) ViewState["lectura"]; } 
            set { ViewState["lectura"] = value; }
        }

        public string Observacionfinal
        {
            get { return ViewState["observacionfinal"].ToString(); }
            set { ViewState["observacionfinal"] = value; }
        }
   
        
        #endregion

        #region page load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Obtenervariables();
                ObtenerInformacionGeneral();
                int id = usuario.IdContacto;
            }
        }

        #endregion

        #region Metodos


        void Obtenervariables()
        {
            if (HttpContext.Current.Session["CodProyecto"] != null && HttpContext.Current.Session["CodConvocatoria"] != null)
            {
                Codproyecto = (int) HttpContext.Current.Session["CodProyecto"];
                CodConvocatoria = (int) HttpContext.Current.Session["CodConvocatoria"];
            }
            else
            {
                var querystringSeguro = CreateQueryStringUrl("info");
                Codproyecto = Convert.ToInt32(querystringSeguro["CodProyecto"]);
                CodConvocatoria = Convert.ToInt32(querystringSeguro["CodConvocatoria"]);
            }

            EsSoloLectura();
        }

        void EsSoloLectura()
        {
            Lectura = true;

            int codigo = GetEstadoProyectoFromProyecto();

            if ((codigo == 10) || (codigo == 11) || (codigo == 12) || (codigo == 15) || (codigo == 16))
            {
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

                if (lPaginas!=null)
                {

                    foreach (var s in result)
                    {
                         if(s == url){

                                Lectura = true;
                                return;
                            }
                    }
                   

                }
                else
                {
                    Lectura = false;
                }
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
                                                   ParameterName = "@codProyecto",
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

                if (dtsInformacion.Tables.Count!=0)
                {
                    
                    if (dtsInformacion.Tables[0].Rows.Count != 0)
                    {
                        Observacionfinal = dtsInformacion.Tables[0].Rows[0]["OBSERVACIONFINAL"].ToString();
                    }
                    else
                    {
                        Observacionfinal = string.Empty;
                    }
                    if (dtsInformacion.Tables[1].Rows.Count != 0)
                    {
                        CodEstado = Convert.ToInt32(dtsInformacion.Tables[1].Rows[0]["CODESTADO"].ToString());
                    }
                    else
                    {
                        CodEstado = 0;
                    }

                    if (dtsInformacion.Tables[3].Rows.Count!=0)
                    {
                        lblnomconvocatoria.Text = ObtenerNombreconvocatoria();
                        lblnomproyecto.Text = Codproyecto.ToString() + " - " + "\r\n" +dtsInformacion.Tables[3].Rows[0]["NOMPROYECTO"].ToString();
                        lblunidad.Text = dtsInformacion.Tables[3].Rows[0]["UnidadEmprendimiento"].ToString();
                        lblnomciudad.Text = dtsInformacion.Tables[3].Rows[0]["NOMCIUDAD"].ToString();
                    }


                    if (dtsInformacion.Tables[6].Rows.Count != 0)
                    {
                        txtcrif.Text = dtsInformacion.Tables[6].Rows[0]["CRIF"].ToString();
                     }
                    else
                    {
                        txtcrif.Text = string.Empty;
                    }

                    if (dtsInformacion.Tables[4].Rows.Count != 0)
                    {
                        hplAsesorLider.Text = dtsInformacion.Tables[4].Rows[0]["NombreLider"].ToString();
                     }
                    else
                    {
                        hplAsesorLider.Visible = false;
                    }
                    
                    if (dtsInformacion.Tables[5].Rows.Count != 0)
                    {
                        hplAsesor.Text = dtsInformacion.Tables[5].Rows[0]["NombreAsesor"].ToString();
                    }
                    else
                    {
                        hplAsesor.Visible = false;
                    }

                    // obtengo todos los crif 

                    if (dtsInformacion.Tables[7].Rows.Count != 0)
                    {
                        HttpContext.Current.Session["dtcrif"] = dtsInformacion.Tables[7];
                    }
                    else
                    {
                        HttpContext.Current.Session["dtcrif"] = null;
                    }


                    GetFechaAval();

                    ValidarPrecondiciones();

                    ValidaBotones();

                    if (Lectura)
                    {
                        txtcrif.Enabled = false;
                    }

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

              if (count!=0)
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
                   bool bandera =   consultas.InsertarDataTable("MD_InsertProyectoAcreditacionDocumento");

                   InsertarProyectoAcreditacion(Constantes.CONST_Asignado_para_acreditacion,string.Empty);

                   consultas.Parameters = null;
                  }
              }
              else
              {
                  Response.Redirect("PlanesaAcreditar.aspx");
              }
          }

         void  InsertarProyectoAcreditacion(int estado, string observaciones)
         {
             var proyectoAcreditacion = new Datos.ProyectoAcreditacion();

             try
             {
                 observaciones = observaciones.Replace("'", "");
              
                 if (Codproyecto!= 0 && CodConvocatoria != 0  )
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
                case  "asesor" :
                    para = 2;
                    break;

                case  "lider" :
                    para = 1;
                    break;
            }

            var querystringSeguro = CreateQueryStringUrl();
            querystringSeguro["proyectoAsesor"] = Codproyecto.ToString();
            querystringSeguro["codconvocatoriaAsesor"] = CodConvocatoria.ToString();
            querystringSeguro["tipoasesor"] = para.ToString();
            Redirect(null, "InfoAsesor.aspx?a=" +  HttpUtility.UrlEncode(querystringSeguro.ToString()), "_blank", "menubar=0,scrollbars=1,width=510,height=200,top=50");


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

                    if (dtResultConvocatorias.Rows.Count!=0)
                    {
                        foreach (DataRow dtrow in dtResultConvocatorias.Rows)
                        {
                            if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONACREDITADO"].ToString()))
                            {
                                txtObservaciones.Text = txtObservaciones.Text +
                                                        dtResultConvocatorias.Rows[0]["OBSERVACIONACREDITADO"].ToString();
                            }
                            else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONSUBSANADO"].ToString()))
                            {
                                txtObservaciones.Text = txtObservaciones.Text +
                                                        dtResultConvocatorias.Rows[0]["OBSERVACIONSUBSANADO"].ToString();
                            }
                            else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONPENDIENTE"].ToString()))
                            {
                                txtObservaciones.Text = txtObservaciones.Text +
                                                        dtResultConvocatorias.Rows[0]["OBSERVACIONPENDIENTE"].ToString();
                            }
                            else if (!string.IsNullOrEmpty(dtResultConvocatorias.Rows[0]["OBSERVACIONNOACREDITADO"].ToString()))
                            {

                                txtObservaciones.Text = txtObservaciones.Text +
                                                        dtResultConvocatorias.Rows[0]["OBSERVACIONNOACREDITADO"].ToString();
                            }
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

        public static string  Guardar()
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
           Redirect(null, "NotificacionesEnviadas.aspx", "_blank", "menubar=0,scrollbars=1,width=610,height=280,top=70");
        }

        protected void lbvertodos_Click(object sender, EventArgs e)
        {
           Redirect(null, "CrifIngresados.aspx", "_blank", "menubar=0,scrollbars=1,width=580,height=200,top=70");
        }


        protected void Btnguardar_Click(object sender, EventArgs e)
        {
            guardar();
        }
       
        protected void btnfinalizar_Click(object sender, EventArgs e)
        {
            Actualizar();
        }

        #endregion

        #region Eventos Datalis

        protected void DtlProyectoConvocatoriaItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region checkbox 
                
                //  ********************* ***************************************//
                var chPendiente = e.Item.FindControl("chPendiente") as Label;
                var lblidproyecto = e.Item.FindControl("lblidproyecto") as Label;
                var chsubsanado = e.Item.FindControl("chsubsanado") as Label;
                var chAcreditado = e.Item.FindControl("chAcreditado") as Label;
                var chNoAcreditado = e.Item.FindControl("chNoAcreditado") as Label;
                var chCertificaion = e.Item.FindControl("chCertificaion") as CheckBox;
                var chDiplomas = e.Item.FindControl("chDiplomas") as CheckBox;
                var chActas = e.Item.FindControl("chActas") as CheckBox;

            //  ********************* ***************************************//
                var ch1 = e.Item.FindControl("ch1") as CheckBox;
                var ch2 = e.Item.FindControl("ch2") as CheckBox;
                var ch3 = e.Item.FindControl("ch3") as CheckBox;
                var chdi = e.Item.FindControl("chdi") as CheckBox;

                // validaciones pendientes //
                //if (chsubsanado != null) chsubsanado.Enabled = false;
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

                // Acreditado //

                if (chAcreditado.Text == "True")
                {
                    chAcreditado.Text = "<img src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                    rdb.SelectedValue = "13";
                }
                else
                {
                    chAcreditado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                }
                // No Acreditado

                if (chNoAcreditado.Text == "True")
                {
                    chNoAcreditado.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                    rdb.SelectedValue = "14";
                }
                else
                {
                    chNoAcreditado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                }

               // Pendiente //

                if (chPendiente.Text == "True")
                {
                    rdb.SelectedValue = "15";
                    chPendiente.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                }
                else
                {
                    chPendiente.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                }
                
                // subsanando //

                if (chsubsanado.Text == "True")
                {
                    rdb.SelectedValue = "16";
                    chsubsanado.Text = "<img id='img" + lblidproyecto.Text + "' src='../../Images/chulo.gif' border='0' style='cursor:pointer'/>";
                }
                else
                {
                    chsubsanado.Text = "<img src='../../Images/icoSinObservacion.gif' border='0' style='cursor:pointer'/>";
                }

                //*********************************///

                

                #endregion

            }
        }

        protected void DtlProyectoRowcommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "emprendedor")
            {
                var querystringSeguro = CreateQueryStringUrl();
                querystringSeguro["codcontacto"] = e.CommandArgument.ToString();
                HttpContext.Current.Session["infoEmprendedorCodContacto"] = e.CommandArgument.ToString();
                Redirect(null, "InfoEmprendedor.aspx?cosws=" + HttpUtility.UrlEncode(querystringSeguro.ToString()), "_blank", "menubar=0,scrollbars=1,width=510,height=600,top=50");
            }
        }

        #endregion

        #region Validaciones


        void ValidaBotones()
        {
            if (CodEstado == Constantes.CONST_Acreditado || CodEstado == Constantes.CONST_No_acreditado)
            {
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
                    btnfinalizar.Visible = true;
                    Btnguardar.Visible = true;
                }

            }
        }
        
        void guardar()
        {
            foreach (DataListItem listItem in DtlProyectoConvocatoria.Items)
            {
                if (listItem.ItemType  == ListItemType.Item || listItem.ItemType == ListItemType.AlternatingItem)
                {
                    var proyectoId = listItem.FindControl("lblidproyecto") as Label;
                    var pendiente = listItem.FindControl("lchPendiente") as Label;
                    var subsanado = listItem.FindControl("lchsubsanado") as Label;
                    var acredidato = listItem.FindControl("lchAcreditado") as Label;
                    var noacreditado = listItem.FindControl("lchNoAcreditado") as Label;

                    //

                    var certificacion = listItem.FindControl("chCertificaion") as CheckBox;
                    var diplomas = listItem.FindControl("chDiplomas") as CheckBox;
                    var actas = listItem.FindControl("chActas") as CheckBox;

                    var ch1 = listItem.FindControl("ch1") as CheckBox;
                    var ch2 = listItem.FindControl("ch2") as CheckBox;
                    var ch3 = listItem.FindControl("ch3") as CheckBox;
                    var chdi = listItem.FindControl("chdi") as CheckBox;

                    // busco la entidad para modificarla

                    var proyectoDoc = 
                        consultas.Db.ProyectoAcreditacionDocumentos.FirstOrDefault(pd => pd.Id_ProyectoAcreditacionDocumento == Convert.ToInt32(proyectoId.Text));

                    if (proyectoDoc!=null)
                    {

                        // 
                        if (pendiente != null)
                        {
                            if (pendiente.Text == "True")
                            {
                                proyectoDoc.Pendiente = true;
                            }
                            else
                            {
                                proyectoDoc.Pendiente = false;
                                proyectoDoc.AsuntoPendiente = null;
                                proyectoDoc.FechaPendiente = null;
                                proyectoDoc.ObservacionPendiente = null;
                            }
                        }

                        if (subsanado != null)
                        {
                            if (subsanado.Text == "True")
                            {
                                proyectoDoc.Subsanado = true;
                            }
                            else
                            {
                                proyectoDoc.Subsanado = false;
                                proyectoDoc.AsuntoSubsanado = null;
                                proyectoDoc.FechaSubsanado = null;
                                proyectoDoc.ObservacionSubsanado = null;
                            }
                        }

                        if (acredidato != null)
                        {
                            if (acredidato.Text == "True")
                            {
                                proyectoDoc.Acreditado = true;
                            }
                            else
                            {
                                proyectoDoc.Pendiente = false;
                                proyectoDoc.AsuntoPendiente = null;
                                proyectoDoc.FechaPendiente = null;
                                proyectoDoc.ObservacionPendiente = null;
                            }
                        }

                        if (noacreditado != null)
                        {
                            if (noacreditado.Text == "True")
                            {
                                proyectoDoc.NoAcreditado = true;
                            }
                            else
                            {
                                proyectoDoc.NoAcreditado = false;
                                proyectoDoc.AsuntoNoAcreditado = null;
                                proyectoDoc.FechaNoAcreditado = null;
                                proyectoDoc.ObservacionNoAcreditado = null;
                            }
                        }


                        if (certificacion != null)
                        {
                            if (certificacion.Checked)
                            {
                                proyectoDoc.FlagCertificaciones = true;
                            }
                            else
                            {
                                proyectoDoc.FlagCertificaciones = false;
                                
                            }
                        }

                        if (diplomas != null)
                        {
                            if (diplomas.Checked)
                            {
                                proyectoDoc.FlagDiploma = true;
                            }
                            else
                            {
                                proyectoDoc.FlagDiploma = false;

                            }
                        }

                        if (actas != null)
                        {
                            if (actas.Checked)
                            {
                                proyectoDoc.FlagActa = true;
                            }
                            else
                            {
                                proyectoDoc.FlagActa = false;

                            }
                        }

                        if (ch1 != null)
                        {
                            if (ch1.Checked)
                            {
                                proyectoDoc.FlagAnexo1 = true;
                            }
                            else
                            {
                                proyectoDoc.FlagAnexo1 = false;

                            }
                        }

                        if (ch2 != null)
                        {
                            if (ch2.Checked)
                            {
                                proyectoDoc.FlagAnexo2 = true;
                            }
                            else
                            {
                                proyectoDoc.FlagAnexo2 = false;

                            }
                        }

                        if (ch3 != null)
                        {
                            if (ch3.Checked)
                            {
                                proyectoDoc.FlagAnexo3 = true;
                            }
                            else
                            {
                                proyectoDoc.FlagAnexo3 = false;

                            }
                        }

                        if (chdi != null)
                        {
                            if (chdi.Checked)
                            {
                                proyectoDoc.FlagDI = true;
                            }
                            else
                            {
                                proyectoDoc.FlagDI = false;

                            }
                        }

                        consultas.Db.SubmitChanges();

                        if (!string.IsNullOrEmpty(txtcrif.Text))
                        {
                            var crif =
                                consultas.Db.ProyectoAcreditaciondocumentosCRIFs.FirstOrDefault(
                                    pc => pc.Crif == txtcrif.Text
                                          && pc.CodProyecto == Codproyecto);
                            if (crif== null)
                            {
                                var crifNew = new ProyectoAcreditaciondocumentosCRIF();

                                crifNew.CodProyecto = Codproyecto;
                                crifNew.CodConvocatoria = CodConvocatoria;
                                crifNew.Crif = txtcrif.Text;
                                crifNew.Fecha = DateTime.Now;
                                consultas.Db.ProyectoAcreditaciondocumentosCRIFs.InsertOnSubmit(crifNew);
                                consultas.Db.SubmitChanges();
                            }
                        }

                    }
                    

                   
                    
                }
            }


            //Actualizo el estado del proyecto

            if (CodEstado != CodEstado)
            {
                InsertarProyectoAcreditacion(codEstado,string.Empty);

                 var proyecto = consultas.Db.Proyecto.FirstOrDefault(p => p.Id_Proyecto == Codproyecto);

                if (CodEstado == Constantes.CONST_Acreditado)
                {
                   if(proyecto!=null)
                    {
                        proyecto.CodEstado = Constantes.CONST_Aprobacion_Acreditacion;
                    }
                }else if (CodEstado == Constantes.CONST_No_acreditado)
                {
                    proyecto.CodEstado = Constantes.CONST_Aprobacion_No_Acreditacion;
                }
                else
                {
                    proyecto.CodEstado = (byte) CodEstado;
                    EliminarProyectoDeActa();
                }
            }

           
        }

        void Actualizar()
        {
            try
            {
                var proyecto = consultas.Db.Proyecto.FirstOrDefault(p => p.Id_Proyecto == Codproyecto);

                if (proyecto!= null)
                {
                    
                    if (CodEstado == Constantes.CONST_Acreditado)
                    {
                        proyecto.CodEstado = Constantes.CONST_Aprobacion_Acreditacion;

                    }
                    else if (CodEstado == Constantes.CONST_No_acreditado)
                    {
                        proyecto.CodEstado = Constantes.CONST_Aprobacion_No_Acreditacion;
                    }

                    consultas.Db.SubmitChanges();

                    InsertarProyectoAcreditacion(CodEstado,txtObservaciones.Text);
                }

                Response.Redirect("PlanesAcreditar.aspx");
            }
            catch (Exception exception)
            {
                throw  new Exception(exception.Message);
                
            }
        }

        #endregion


    }

   
}