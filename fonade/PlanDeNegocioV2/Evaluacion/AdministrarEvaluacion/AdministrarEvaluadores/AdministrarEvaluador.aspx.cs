using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluador;
using Fonade.Clases;
using System.Net.Mail;
using System.Configuration;
using Fonade.Account;
using System.Web.Security;
using Fonade.Negocio.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinador;


namespace Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluadores
{
    public partial class AdministrarEvaluador : System.Web.UI.Page
    {
        protected FonadeUser Usuario
        {
            get
            {
                return HttpContext.Current.Session["usuarioLogged"] != null ? (FonadeUser)HttpContext.Current.Session["usuarioLogged"] : (FonadeUser)Membership.GetUser(HttpContext.Current.User.Identity.Name, true);
            }
            set
            {
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gvProyectos.DataSource = null;
                gvProyectos.DataBind();
            }
        }

        public List<EvaluadorProyecto> GetEvaluadores() {
            return AsignarEvaluador.GetEvaluadores(Usuario.CodOperador);
        }

        protected void gvEvaluadores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var codigoEvaluador = Convert.ToInt32(e.CommandArgument);
            hdCodigoEvaluador.Value = codigoEvaluador.ToString();

            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            LinkButton evaluadorSeleccionado = (LinkButton)row.Cells[0].FindControl("verEvaluador");

            btnAsignarEvaluador.Text ="Asignar planes de negocio a evaluador " + evaluadorSeleccionado.Text;            
            btnAsignarEvaluador.Visible = true;

            gvProyectos.DataSource = GetProyectosPorEvaluador(codigoEvaluador);            
            gvProyectos.DataBind();
        }
                
        public List<ProyectoEvaluacion> GetProyectosPorEvaluador(int codigoEvaluador) {
            return AsignarEvaluador.GetProyectosPorEvaluador(codigoEvaluador, Usuario.CodOperador);
        }

        protected void gvProyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var codigoSector = Convert.ToInt32(e.CommandArgument);
            var codigoEvaluador = Convert.ToInt32(hdCodigoEvaluador.Value);            

            if(codigoEvaluador != 0)
            {
                AsignarEvaluador.AddSectorEvaluador(codigoEvaluador, codigoSector);


                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                
                CheckBox checkOwner = (CheckBox)row.Cells[0].FindControl("chkProyecto");
                Label lblSector = (Label)row.Cells[1].FindControl("lblSectorDiferente");
                LinkButton linkAsociarSector = (LinkButton)row.Cells[1].FindControl("lnkAsociarSector");

                checkOwner.Enabled = true;
                lblSector.Visible = false;
                linkAsociarSector.Visible = false;                
            }            
        }

        protected void btnAsignarEvaluador_Click(object sender, EventArgs e)
        {
            try
            {
                var codigoEvaluador = Convert.ToInt32(hdCodigoEvaluador.Value);

                foreach (GridViewRow currentRow in gvProyectos.Rows)
                {
                    CheckBox checkProyecto = (CheckBox)currentRow.FindControl("chkProyecto");
                    CheckBox isCurrentOwner = (CheckBox)currentRow.FindControl("chkIsCurrentOwner"); //isCurrentOwner es para saber si estaba asignado al inicio, el usuario lo desmarco y se debe desasignar
                    HiddenField codigoProyecto = (HiddenField)currentRow.FindControl("hdCodigoProyecto");

                    if (checkProyecto.Checked || isCurrentOwner.Checked) {                        
                        if (!checkProyecto.Checked && isCurrentOwner.Checked)
                        {
                            AsignarEvaluador.AsignarProyectoEvaluador(codigoEvaluador, Convert.ToInt32(codigoProyecto.Value), true);
                        }
                                                                       
                        if (checkProyecto.Checked && !isCurrentOwner.Checked) {
                            AsignarEvaluador.AsignarProyectoEvaluador(codigoEvaluador, Convert.ToInt32(codigoProyecto.Value));
                            //Quitar Para Produccion el comentario de abajo
                            EnviarNotificacion(Convert.ToInt32(codigoProyecto.Value));
                        }                                                    
                    }
                }

                gvProyectos.DataSource = GetProyectosPorEvaluador(codigoEvaluador);
                gvProyectos.DataBind();

                Formulacion.Utilidad.Utilidades.PresentarMsj("Información guardada con exito.", this, "Alert");
            }
            catch (Exception ex)
            {
                Formulacion.Utilidad.Utilidades.PresentarMsj("Sucedio un error : " + ex.Message, this, "Alert");
            }
        }

        private void EnviarNotificacion(int codigoProyecto) {

            var proyecto = Negocio.PlanDeNegocio2017.Utilidad.ProyectoGeneral.GetProjectInfo(codigoProyecto);
            var evaluador = AsignarEvaluador.GetEvaluadorProyecto(codigoProyecto);
            var coordinador = AsignarCoordinador.GetCoordinadorByEvaluador(evaluador.Id);
            var codigoConvocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaByProyecto(codigoProyecto, 0);
            var convocatoria = Negocio.PlanDeNegocioV2.Utilidad.Convocatoria.GetConvocatoriaDetails(codigoConvocatoria.GetValueOrDefault());

            enviarEmail(Usuario.Email, evaluador.Email, " "+ proyecto.Id_Proyecto + " " + proyecto.NomProyecto + " ", " " + convocatoria.NomConvocatoria + " " ," " + evaluador.NombreCompleto + " "," " + coordinador.NombreCompleto + " ", DateTime.Now);
        }

        private bool enviarEmail(string emailRemitente, string emailDestinatario, string proyecto,string convocatoria, string evaluador, string coordinador, DateTime fechaAsignacion)
        {
            bool errorMessage = false;

			#region Estrucutura Correo antigua
			//string bodyTemplate = 
			//	"<!doctype html><html xmlns=\"http://www.w3.org/1999/xhtml\" "
			//	+"xmlns:v=\"urn:schemas-microsoft-com:vml\" "
			//	+"xmlns:o=\"urn:schemas-microsoft-com:office:office\">"
			//	+"<head><!--[if gte mso 15]><xml><o:OfficeDocumentSettings><o:AllowPNG/>"
			//	+"<o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml>"
			//	+"<![endif]--><meta charset=\"UTF-8\"> "
			//	+"<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> "
			//	+"<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"
			//	+"<title>*|MC:SUBJECT|*</title> "
			//	+"<style type=\"text/css\">"
			//	+"p{margin:10px 0;padding:0;}"
			//	+"table{border-collapse:collapse;}"
			//	+"h1,h2,h3,h4,h5,h6{display:block;margin:0;padding:0;}"
			//	+"img,a img{border:0;height:auto;outline:none;text-decoration:none;}"
			//	+"body,#bodyTable,#bodyCell{height:100%;margin:0;padding:0;width:100%;}"
			//	+".mcnPreviewText{display:none !important;}#outlook a{padding:0;}"
			//	+"img{-ms-interpolation-mode:bicubic;}table{mso-table-lspace:0pt;mso-table-rspace:0pt;}"
			//	+".ReadMsgBody{width:100%;}.ExternalClass{width:100%;}"
			//	+"p,a,li,td,blockquote{mso-line-height-rule:exactly;}"
			//	+"a[href^=tel],a[href^=sms]{color:inherit;cursor:default;text-decoration:none;}"
			//	+"p,a,li,td,body,table,blockquote{-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;}"
			//	+".ExternalClass,.ExternalClass p,.ExternalClass td,.ExternalClass div,.ExternalClass span,"
			//	+".ExternalClass font{line-height:100%;}"
			//	+"a[x-apple-data-detectors]"
			//	+"{color:inherit !important;text-decoration:none !important;"
			//	+"font-size:inherit !important;font-family:inherit !important;font-weight:inherit !important;"
			//	+"line-height:inherit !important;}.templateContainer{max-width:600px !important;}"
			//	+"a.mcnButton{display:block;}.mcnImage,.mcnRetinaImage{vertical-align:bottom;}"
			//	+".mcnTextContent{word-break:break-word;}.mcnTextContent img{height:auto !important;}"
			//	+".mcnDividerBlock{table-layout:fixed !important;}"
			//	+"/*@tab Page@section Heading 1@style heading 1*/"
			//	+"h1{/*@editable*/color:#222222;/*@editable*/font-family:Helvetica;"
			//	+"/*@editable*/font-size:40px;/*@editable*/font-style:normal;/*@editable*/font-weight:bold;"
			//	+"/*@editable*/line-height:150%;/*@editable*/letter-spacing:normal;/*@editable*/text-align:center;}"
			//	+"/*@tab Page@section Heading 2@style heading 2*/h2{/*@editable*/color:#222222;"
			//	+"/*@editable*/font-family:Helvetica;/*@editable*/font-size:34px;/*@editable*/font-style:normal;"
			//	+"/*@editable*/font-weight:bold;/*@editable*/line-height:150%;/*@editable*/letter-spacing:normal;"
			//	+"/*@editable*/text-align:left;}/*@tab Page@section Heading 3@style heading 3*/"
			//	+"h3{/*@editable*/color:#444444;/*@editable*/font-family:Helvetica;/*@editable*/font-size:22px;"
			//	+"/*@editable*/font-style:normal;/*@editable*/font-weight:bold;/*@editable*/line-height:150%;"
			//	+"/*@editable*/letter-spacing:normal;/*@editable*/text-align:left;}"
			//	+"/*@tab Page@section Heading 4@style heading 4*/h4{/*@editable*/color:#999999;"
			//	+"/*@editable*/font-family:Georgia;/*@editable*/font-size:20px;/*@editable*/font-style:italic;"
			//	+"/*@editable*/font-weight:normal;/*@editable*/line-height:125%;/*@editable*/letter-spacing:normal;"
			//	+"/*@editable*/text-align:left;}"
			//	+"/*@tab Header@section Header Container Style*/#templateHeader{/*@editable*/background-color:#f7f7f7;"
			//	+"/*@editable*/background-image:none;/*@editable*/background-repeat:no-repeat;"
			//	+"/*@editable*/background-position:50% 50%;/*@editable*/background-size:cover;"
			//	+"/*@editable*/border-top:0;/*@editable*/border-bottom:0;/*@editable*/padding-top:0px;"
			//	+"/*@editable*/padding-bottom:0px;}/*@tab Header@section Header Interior Style*/"
			//	+".headerContainer{/*@editable*/background-color:#transparent;/*@editable*/background-image:none;"
			//	+"/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
			//	+"/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
			//	+"/*@editable*/padding-top:0;/*@editable*/padding-bottom:0;}/*@tab Header@section Header Text*/"
			//	+".headerContainer .mcnTextContent,.headerContainer .mcnTextContent p{/*@editable*/color:#808080;"
			//	+"/*@editable*/font-family:Helvetica;/*@editable*/font-size:16px;/*@editable*/line-height:150%;"
			//	+"/*@editable*/text-align:left;}/*@tab Header@section Header Link*/"
			//	+".headerContainer .mcnTextContent a,.headerContainer "
			//	+".mcnTextContent p a{/*@editable*/color:#00ADD8;/*@editable*/font-weight:normal;"
			//	+"/*@editable*/text-decoration:underline;}/*@tab Body@section Body Container Style*/"
			//	+"#templateBody{/*@editable*/background-color:#FFFFFF;/*@editable*/background-image:none;"
			//	+"/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
			//	+"/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
			//	+"/*@editable*/padding-top:119px;/*@editable*/padding-bottom:119px;}"
			//	+"/*@tab Body@section Body Interior Style*/.bodyContainer{/*@editable*/background-color:transparent;"
			//	+"/*@editable*/background-image:none;/*@editable*/background-repeat:no-repeat;"
			//	+"/*@editable*/background-position:center;/*@editable*/background-size:cover;"
			//	+"/*@editable*/border-top:0;/*@editable*/border-bottom:0;/*@editable*/padding-top:0;"
			//	+"/*@editable*/padding-bottom:0;}/*@tab Body@section Body Text*/.bodyContainer .mcnTextContent,"
			//	+".bodyContainer .mcnTextContent p{/*@editable*/color:#808080;/*@editable*/font-family:Helvetica;"
			//	+"/*@editable*/font-size:16px;/*@editable*/line-height:150%;/*@editable*/text-align:left;}"
			//	+"/*@tab Body@section Body Link*/.bodyContainer .mcnTextContent a,.bodyContainer "
			//	+".mcnTextContent p a{/*@editable*/color:#00ADD8;/*@editable*/font-weight:normal;"
			//	+"/*@editable*/text-decoration:underline;}/*@tab Footer@section Footer Style*/"
			//	+"#templateFooter{/*@editable*/background-color:#ffffff;/*@editable*/background-image:none;"
			//	+"/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
			//	+"/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
			//	+"/*@editable*/padding-top:0px;/*@editable*/padding-bottom:0px;}"
			//	+"/*@tab Footer@section Footer Interior Style*/"
			//	+".footerContainer{/*@editable*/background-color:transparent;/*@editable*/background-image:none;"
			//	+"/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
			//	+"/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
			//	+"/*@editable*/padding-top:0;/*@editable*/padding-bottom:0;}/*@tab Footer@section Footer Text*/"
			//	+".footerContainer .mcnTextContent,.footerContainer .mcnTextContent p{/*@editable*/color:#FFFFFF;"
			//	+"/*@editable*/font-family:Helvetica;/*@editable*/font-size:12px;/*@editable*/line-height:150%;"
			//	+"/*@editable*/text-align:center;}/*@tab Footer@section Footer Link*/.footerContainer "
			//	+".mcnTextContent a,.footerContainer .mcnTextContent p a{/*@editable*/color:#FFFFFF;"
			//	+"/*@editable*/font-weight:normal;/*@editable*/text-decoration:underline;}"
			//	+"@media only screen and (min-width:768px){.templateContainer{width:600px !important;}}"
			//	+"@media only screen and (max-width: 480px)"
			//	+"{body,table,td,p,a,li,blockquote{-webkit-text-size-adjust:none !important;}}"
			//	+"@media only screen and (max-width: 480px){body{width:100% !important;min-width:100% !important;}}"
			//	+"@media only screen and (max-width: 480px){.mcnRetinaImage{max-width:100% !important;}}"
			//	+"@media only screen and (max-width: 480px){.mcnImage{width:100% !important;}}@media only screen and (max-width: 480px){.mcnCartContainer,.mcnCaptionTopContent,.mcnRecContentContainer,.mcnCaptionBottomContent,.mcnTextContentContainer,.mcnBoxedTextContentContainer,.mcnImageGroupContentContainer,.mcnCaptionLeftTextContentContainer,.mcnCaptionRightTextContentContainer,.mcnCaptionLeftImageContentContainer,.mcnCaptionRightImageContentContainer,.mcnImageCardLeftTextContentContainer,.mcnImageCardRightTextContentContainer,.mcnImageCardLeftImageContentContainer,.mcnImageCardRightImageContentContainer{max-width:100% !important;width:100% !important;}}@media only screen and (max-width: 480px){.mcnBoxedTextContentContainer{min-width:100% !important;}}@media only screen and (max-width: 480px){.mcnImageGroupContent{padding:9px !important;}}@media only screen and (max-width: 480px){.mcnCaptionLeftContentOuter .mcnTextContent,.mcnCaptionRightContentOuter .mcnTextContent{padding-top:9px !important;}}@media only screen and (max-width: 480px){.mcnImageCardTopImageContent,.mcnCaptionBottomContent:last-child .mcnCaptionBottomImageContent,.mcnCaptionBlockInner .mcnCaptionTopContent:last-child .mcnTextContent{padding-top:18px !important;}}@media only screen and (max-width: 480px){.mcnImageCardBottomImageContent{padding-bottom:9px !important;}}@media only screen and (max-width: 480px){.mcnImageGroupBlockInner{padding-top:0 !important;padding-bottom:0 !important;}}@media only screen and (max-width: 480px){.mcnImageGroupBlockOuter{padding-top:9px !important;padding-bottom:9px !important;}}@media only screen and (max-width: 480px){.mcnTextContent,.mcnBoxedTextContentColumn{padding-right:18px !important;padding-left:18px !important;}}@media only screen and (max-width: 480px){.mcnImageCardLeftImageContent,.mcnImageCardRightImageContent{padding-right:18px !important;padding-bottom:0 !important;padding-left:18px !important;}}@media only screen and (max-width: 480px){.mcpreview-image-uploader{display:none !important;width:100% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 1@tip Make the first-level headings larger in size for better readability on small screens.*/h1{/*@editable*/font-size:30px !important;/*@editable*/line-height:125% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 2@tip Make the second-level headings larger in size for better readability on small screens.*/h2{/*@editable*/font-size:26px !important;/*@editable*/line-height:125% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 3@tip Make the third-level headings larger in size for better readability on small screens.*/h3{/*@editable*/font-size:20px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 4@tip Make the fourth-level headings larger in size for better readability on small screens.*/h4{/*@editable*/font-size:18px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Boxed Text@tip Make the boxed text larger in size for better readability on small screens. We recommend a font size of at least 16px.*/.mcnBoxedTextContentContainer .mcnTextContent,.mcnBoxedTextContentContainer .mcnTextContent p{/*@editable*/font-size:14px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Header Text@tip Make the header text larger in size for better readability on small screens.*/.headerContainer .mcnTextContent,.headerContainer .mcnTextContent p{/*@editable*/font-size:16px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Body Text@tip Make the body text larger in size for better readability on small screens. We recommend a font size of at least 16px.*/.bodyContainer .mcnTextContent,.bodyContainer .mcnTextContent p{/*@editable*/font-size:16px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Footer Text@tip Make the footer content text larger in size for better readability on small screens.*/.footerContainer .mcnTextContent,.footerContainer .mcnTextContent p{/*@editable*/font-size:14px !important;/*@editable*/line-height:150% !important;}}</style></head> <body><span class=\"mcnPreviewText\" style=\"display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;\">*|MC_PREVIEW_TEXT|*</span> <center> <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\" id=\"bodyTable\"> <tr> <td align=\"center\" valign=\"top\" id=\"bodyCell\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"center\" valign=\"top\" id=\"templateHeader\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"headerContainer\"></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr><tr><td align=\"center\" valign=\"top\" id=\"templateBody\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"bodyContainer\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnImageBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnImageBlockOuter\"> <tr> <td valign=\"top\" style=\"padding:9px\" class=\"mcnImageBlockInner\"> <table align=\"left\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnImageContentContainer\" style=\"min-width:100%;\"> <tbody><tr> <td class=\"mcnImageContent\" valign=\"top\" style=\"padding-right: 9px; padding-left: 9px; padding-top: 0; padding-bottom: 0; text-align:center;\"> <img align=\"center\" alt=\"\" src=\"https://gallery.mailchimp.com/f97337609833a5aabaf671140/images/58236e6e-52ba-4e21-b4b8-96fcbf746549.png\" width=\"288\" style=\"max-width:288px; padding-bottom: 0; display: inline !important; vertical-align: bottom;\" class=\"mcnImage\"> </td></tr></tbody></table> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnTextBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnTextBlockOuter\"> <tr> <td valign=\"top\" class=\"mcnTextBlockInner\" style=\"padding-top:9px;\"><!--[if mso]><table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100%;\"><tr><![endif]--><!--[if mso]><td valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:100%; min-width:100%;\" width=\"100%\" class=\"mcnTextContentContainer\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\" style=\"padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;\"> <h1><span style=\"font-size:14px\">Asignación del plan de negocio{{proyecto}}&nbsp;&nbsp;</span></h1><p style=\"text-align:center !important;\">Se le asigno el proyecto{{proyecto}}para realizar la evaluación, favor tener en cuenta la siguiente información:</p><ol><li style=\"text-align: left;\">Proyecto{{proyecto}}&nbsp;</li><li style=\"text-align: left;\">Convocatoria{{convocatoria}}</li><li style=\"text-align: left;\">Evaluador{{evaluador}}</li><li style=\"text-align: left;\">Coordinador de evaluación{{coordinador}}</li><li style=\"text-align: left;\">Fecha de asignación{{fechaasignacion}}</li></ol> </td></tr></tbody></table><!--[if mso]></td><![endif]--><!--[if mso]></tr></table><![endif]--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width:100%; padding:18px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width: 100%;border-top: 2px solid #EAEAEA;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnTextBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnTextBlockOuter\"> <tr> <td valign=\"top\" class=\"mcnTextBlockInner\" style=\"padding-top:9px;\"><!--[if mso]><table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100%;\"><tr><![endif]--><!--[if mso]><td valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:100%; min-width:100%;\" width=\"100%\" class=\"mcnTextContentContainer\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\" style=\"padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;\"> <span style=\"color:#000000\"><span style=\"font-size:22px\"><strong>Fechas de evaluación</strong></span></span> </td></tr></tbody></table><!--[if mso]></td><![endif]--><!--[if mso]></tr></table><![endif]--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 18px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 18px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width:100%; padding:18px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnCaptionBlock\"> <tbody class=\"mcnCaptionBlockOuter\"> <tr> <td class=\"mcnCaptionBlockInner\" valign=\"top\" style=\"padding:9px;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionRightContentOuter\" width=\"100%\"> <tbody><tr> <td valign=\"top\" class=\"mcnCaptionRightContentInner\" style=\"padding:0 9px ;\"> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionRightImageContentContainer\" width=\"264\"> <tbody><tr> <td class=\"mcnCaptionRightImageContent\" align=\"center\" valign=\"top\"> <img alt=\"\" src=\"https://gallery.mailchimp.com/f97337609833a5aabaf671140/images/aaccc3eb-1ca1-40fe-ba02-4312cc341203.jpg\" width=\"264\" style=\"max-width:268px;\" class=\"mcnImage\"> </td></tr></tbody></table> <table class=\"mcnCaptionRightTextContentContainer\" align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"264\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\"> <h3><span style=\"font-size:14px\">Agendamiento de preguntas de evaluación al emprendedor</span></h3><p><span style=\"font-size:14px\">Los tres días disponibles para realizar la evaluación son:</span></p><ul><li>{{evalday1}}</li><li><span style=\"font-size:14px\">{{evalday2}}</span></li><li><span style=\"font-size:14px\">{{evalday3}}</span></li></ul><h3><span style=\"font-size:14px\">Respuesta de emprendedores</span></h3><p><span style=\"font-size:14px\">Los tres días disponibles para que el emprendedor envié las respuestas a evaluación&nbsp;son:</span></p><ul><li>{{empday1}}</li><li><span style=\"font-size:14px\">{{empday2}}</span></li><li><span style=\"font-size:14px\">{{empday3}}</span></li></ul><h3><span style=\"font-size:14px\">Informe del evaluador</span></h3><p><span style=\"font-size:14px\">Los cuatro&nbsp;días disponibles para que el evaluador envié&nbsp; el informe de evaluación son:</span></p><ul><li>{{infoday1}}</li><li><span style=\"font-size:14px\">{{infoday2}}</span></li><li><span style=\"font-size:14px\">{{infoday3}}</span></li><li><span style=\"font-size:14px\">{{infoday4}}</span></li></ul> </td></tr></tbody></table> </td></tr></tbody></table> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 27px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 27px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr><tr><td align=\"center\" valign=\"top\" id=\"templateFooter\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"footerContainer\"></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr></table> </td></tr></table> </center> </body></html>";
			#endregion

			string bodyTemplate = "<!doctype html><html xmlns=\"http://www.w3.org/1999/xhtml\" "
															   + "xmlns:v=\"urn:schemas-microsoft-com:vml\" "
															   + "xmlns:o=\"urn:schemas-microsoft-com:office:office\">"
															   + "<head><!--[if gte mso 15]><xml><o:OfficeDocumentSettings><o:AllowPNG/>"
															   + "<o:PixelsPerInch>96</o:PixelsPerInch></o:OfficeDocumentSettings></xml>"
															   + "<![endif]--><meta charset=\"UTF-8\"> "
															   + "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> "
															   + "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">"
															   + "<title>*|MC:SUBJECT|*</title> "
															   + "<style type=\"text/css\">"
															   + "p{margin:10px 0;padding:0;}"
															   + "table{border-collapse:collapse;}"
															   + "h1,h2,h3,h4,h5,h6{display:block;margin:0;padding:0;}"
															   + "img,a img{border:0;height:auto;outline:none;text-decoration:none;}"
															   + "body,#bodyTable,#bodyCell{height:100%;margin:0;padding:0;width:100%;}"
															   + ".mcnPreviewText{display:none !important;}#outlook a{padding:0;}"
															   + "img{-ms-interpolation-mode:bicubic;}table{mso-table-lspace:0pt;mso-table-rspace:0pt;}"
															   + ".ReadMsgBody{width:100%;}.ExternalClass{width:100%;}"
															   + "p,a,li,td,blockquote{mso-line-height-rule:exactly;}"
															   + "a[href^=tel],a[href^=sms]{color:inherit;cursor:default;text-decoration:none;}"
															   + "p,a,li,td,body,table,blockquote{-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;}"
															   + ".ExternalClass,.ExternalClass p,.ExternalClass td,.ExternalClass div,.ExternalClass span,"
															   + ".ExternalClass font{line-height:100%;}"
															   + "a[x-apple-data-detectors]"
															   + "{color:inherit !important;text-decoration:none !important;"
															   + "font-size:inherit !important;font-family:inherit !important;font-weight:inherit !important;"
															   + "line-height:inherit !important;}.templateContainer{max-width:600px !important;}"
															   + "a.mcnButton{display:block;}.mcnImage,.mcnRetinaImage{vertical-align:bottom;}"
															   + ".mcnTextContent{word-break:break-word;}.mcnTextContent img{height:auto !important;}"
															   + ".mcnDividerBlock{table-layout:fixed !important;}"
															   + "/*@tab Page@section Heading 1@style heading 1*/"
															   + "h1{/*@editable*/color:#222222;/*@editable*/font-family:Helvetica;"
															   + "/*@editable*/font-size:40px;/*@editable*/font-style:normal;/*@editable*/font-weight:bold;"
															   + "/*@editable*/line-height:150%;/*@editable*/letter-spacing:normal;/*@editable*/text-align:center;}"
															   + "/*@tab Page@section Heading 2@style heading 2*/h2{/*@editable*/color:#222222;"
															   + "/*@editable*/font-family:Helvetica;/*@editable*/font-size:34px;/*@editable*/font-style:normal;"
															   + "/*@editable*/font-weight:bold;/*@editable*/line-height:150%;/*@editable*/letter-spacing:normal;"
															   + "/*@editable*/text-align:left;}/*@tab Page@section Heading 3@style heading 3*/"
															   + "h3{/*@editable*/color:#444444;/*@editable*/font-family:Helvetica;/*@editable*/font-size:22px;"
															   + "/*@editable*/font-style:normal;/*@editable*/font-weight:bold;/*@editable*/line-height:150%;"
															   + "/*@editable*/letter-spacing:normal;/*@editable*/text-align:left;}"
															   + "/*@tab Page@section Heading 4@style heading 4*/h4{/*@editable*/color:#999999;"
															   + "/*@editable*/font-family:Georgia;/*@editable*/font-size:20px;/*@editable*/font-style:italic;"
															   + "/*@editable*/font-weight:normal;/*@editable*/line-height:125%;/*@editable*/letter-spacing:normal;"
															   + "/*@editable*/text-align:left;}"
															   + "/*@tab Header@section Header Container Style*/#templateHeader{/*@editable*/background-color:#f7f7f7;"
															   + "/*@editable*/background-image:none;/*@editable*/background-repeat:no-repeat;"
															   + "/*@editable*/background-position:50% 50%;/*@editable*/background-size:cover;"
															   + "/*@editable*/border-top:0;/*@editable*/border-bottom:0;/*@editable*/padding-top:0px;"
															   + "/*@editable*/padding-bottom:0px;}/*@tab Header@section Header Interior Style*/"
															   + ".headerContainer{/*@editable*/background-color:#transparent;/*@editable*/background-image:none;"
															   + "/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
															   + "/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
															   + "/*@editable*/padding-top:0;/*@editable*/padding-bottom:0;}/*@tab Header@section Header Text*/"
															   + ".headerContainer .mcnTextContent,.headerContainer .mcnTextContent p{/*@editable*/color:#808080;"
															   + "/*@editable*/font-family:Helvetica;/*@editable*/font-size:16px;/*@editable*/line-height:150%;"
															   + "/*@editable*/text-align:left;}/*@tab Header@section Header Link*/"
															   + ".headerContainer .mcnTextContent a,.headerContainer "
															   + ".mcnTextContent p a{/*@editable*/color:#00ADD8;/*@editable*/font-weight:normal;"
															   + "/*@editable*/text-decoration:underline;}/*@tab Body@section Body Container Style*/"
															   + "#templateBody{/*@editable*/background-color:#FFFFFF;/*@editable*/background-image:none;"
															   + "/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
															   + "/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
															   + "/*@editable*/padding-top:119px;/*@editable*/padding-bottom:119px;}"
															   + "/*@tab Body@section Body Interior Style*/.bodyContainer{/*@editable*/background-color:transparent;"
															   + "/*@editable*/background-image:none;/*@editable*/background-repeat:no-repeat;"
															   + "/*@editable*/background-position:center;/*@editable*/background-size:cover;"
															   + "/*@editable*/border-top:0;/*@editable*/border-bottom:0;/*@editable*/padding-top:0;"
															   + "/*@editable*/padding-bottom:0;}/*@tab Body@section Body Text*/.bodyContainer .mcnTextContent,"
															   + ".bodyContainer .mcnTextContent p{/*@editable*/color:#808080;/*@editable*/font-family:Helvetica;"
															   + "/*@editable*/font-size:16px;/*@editable*/line-height:150%;/*@editable*/text-align:left;}"
															   + "/*@tab Body@section Body Link*/.bodyContainer .mcnTextContent a,.bodyContainer "
															   + ".mcnTextContent p a{/*@editable*/color:#00ADD8;/*@editable*/font-weight:normal;"
															   + "/*@editable*/text-decoration:underline;}/*@tab Footer@section Footer Style*/"
															   + "#templateFooter{/*@editable*/background-color:#ffffff;/*@editable*/background-image:none;"
															   + "/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
															   + "/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
															   + "/*@editable*/padding-top:0px;/*@editable*/padding-bottom:0px;}"
															   + "/*@tab Footer@section Footer Interior Style*/"
															   + ".footerContainer{/*@editable*/background-color:transparent;/*@editable*/background-image:none;"
															   + "/*@editable*/background-repeat:no-repeat;/*@editable*/background-position:center;"
															   + "/*@editable*/background-size:cover;/*@editable*/border-top:0;/*@editable*/border-bottom:0;"
															   + "/*@editable*/padding-top:0;/*@editable*/padding-bottom:0;}/*@tab Footer@section Footer Text*/"
															   + ".footerContainer .mcnTextContent,.footerContainer .mcnTextContent p{/*@editable*/color:#FFFFFF;"
															   + "/*@editable*/font-family:Helvetica;/*@editable*/font-size:12px;/*@editable*/line-height:150%;"
															   + "/*@editable*/text-align:center;}/*@tab Footer@section Footer Link*/.footerContainer "
															   + ".mcnTextContent a,.footerContainer .mcnTextContent p a{/*@editable*/color:#FFFFFF;"
															   + "/*@editable*/font-weight:normal;/*@editable*/text-decoration:underline;}"
															   + "@media only screen and (min-width:768px){.templateContainer{width:600px !important;}}"
															   + "@media only screen and (max-width: 480px)"
															   + "{body,table,td,p,a,li,blockquote{-webkit-text-size-adjust:none !important;}}"
															   + "@media only screen and (max-width: 480px){body{width:100% !important;min-width:100% !important;}}"
															   + "@media only screen and (max-width: 480px){.mcnRetinaImage{max-width:100% !important;}}"
															   + "@media only screen and (max-width: 480px){.mcnImage{width:100% !important;}}@media only screen and (max-width: 480px){.mcnCartContainer,.mcnCaptionTopContent,.mcnRecContentContainer,.mcnCaptionBottomContent,.mcnTextContentContainer,.mcnBoxedTextContentContainer,.mcnImageGroupContentContainer,.mcnCaptionLeftTextContentContainer,.mcnCaptionRightTextContentContainer,.mcnCaptionLeftImageContentContainer,.mcnCaptionRightImageContentContainer,.mcnImageCardLeftTextContentContainer,.mcnImageCardRightTextContentContainer,.mcnImageCardLeftImageContentContainer,.mcnImageCardRightImageContentContainer{max-width:100% !important;width:100% !important;}}@media only screen and (max-width: 480px){.mcnBoxedTextContentContainer{min-width:100% !important;}}@media only screen and (max-width: 480px){.mcnImageGroupContent{padding:9px !important;}}@media only screen and (max-width: 480px){.mcnCaptionLeftContentOuter .mcnTextContent,.mcnCaptionRightContentOuter .mcnTextContent{padding-top:9px !important;}}@media only screen and (max-width: 480px){.mcnImageCardTopImageContent,.mcnCaptionBottomContent:last-child .mcnCaptionBottomImageContent,.mcnCaptionBlockInner .mcnCaptionTopContent:last-child .mcnTextContent{padding-top:18px !important;}}@media only screen and (max-width: 480px){.mcnImageCardBottomImageContent{padding-bottom:9px !important;}}@media only screen and (max-width: 480px){.mcnImageGroupBlockInner{padding-top:0 !important;padding-bottom:0 !important;}}@media only screen and (max-width: 480px){.mcnImageGroupBlockOuter{padding-top:9px !important;padding-bottom:9px !important;}}@media only screen and (max-width: 480px){.mcnTextContent,.mcnBoxedTextContentColumn{padding-right:18px !important;padding-left:18px !important;}}@media only screen and (max-width: 480px){.mcnImageCardLeftImageContent,.mcnImageCardRightImageContent{padding-right:18px !important;padding-bottom:0 !important;padding-left:18px !important;}}@media only screen and (max-width: 480px){.mcpreview-image-uploader{display:none !important;width:100% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 1@tip Make the first-level headings larger in size for better readability on small screens.*/h1{/*@editable*/font-size:30px !important;/*@editable*/line-height:125% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 2@tip Make the second-level headings larger in size for better readability on small screens.*/h2{/*@editable*/font-size:26px !important;/*@editable*/line-height:125% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 3@tip Make the third-level headings larger in size for better readability on small screens.*/h3{/*@editable*/font-size:20px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Heading 4@tip Make the fourth-level headings larger in size for better readability on small screens.*/h4{/*@editable*/font-size:18px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Boxed Text@tip Make the boxed text larger in size for better readability on small screens. We recommend a font size of at least 16px.*/.mcnBoxedTextContentContainer .mcnTextContent,.mcnBoxedTextContentContainer .mcnTextContent p{/*@editable*/font-size:14px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Header Text@tip Make the header text larger in size for better readability on small screens.*/.headerContainer .mcnTextContent,.headerContainer .mcnTextContent p{/*@editable*/font-size:16px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Body Text@tip Make the body text larger in size for better readability on small screens. We recommend a font size of at least 16px.*/.bodyContainer .mcnTextContent,.bodyContainer .mcnTextContent p{/*@editable*/font-size:16px !important;/*@editable*/line-height:150% !important;}}@media only screen and (max-width: 480px){/*@tab Mobile Styles@section Footer Text@tip Make the footer content text larger in size for better readability on small screens.*/.footerContainer .mcnTextContent,.footerContainer .mcnTextContent p{/*@editable*/font-size:14px !important;/*@editable*/line-height:150% !important;}}</style></head> <body><span class=\"mcnPreviewText\" style=\"display:none; font-size:0px; line-height:0px; max-height:0px; max-width:0px; opacity:0; overflow:hidden; visibility:hidden; mso-hide:all;\">*|MC_PREVIEW_TEXT|*</span> <center> <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" height=\"100%\" width=\"100%\" id=\"bodyTable\"> <tr> <td align=\"center\" valign=\"top\" id=\"bodyCell\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\"><tr><td align=\"center\" valign=\"top\" id=\"templateHeader\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"headerContainer\"></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr><tr><td align=\"center\" valign=\"top\" id=\"templateBody\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"bodyContainer\"><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnImageBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnImageBlockOuter\"> <tr> <td valign=\"top\" style=\"padding:9px\" class=\"mcnImageBlockInner\"> <table align=\"left\" width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnImageContentContainer\" style=\"min-width:100%;\"> <tbody><tr> <td class=\"mcnImageContent\" valign=\"top\" style=\"padding-right: 9px; padding-left: 9px; padding-top: 0; padding-bottom: 0; text-align:center;\"> <img align=\"center\" alt=\"\" src=\"https://gallery.mailchimp.com/f97337609833a5aabaf671140/images/58236e6e-52ba-4e21-b4b8-96fcbf746549.png\" width=\"288\" style=\"max-width:288px; padding-bottom: 0; display: inline !important; vertical-align: bottom;\" class=\"mcnImage\"> </td></tr></tbody></table> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnTextBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnTextBlockOuter\"> <tr> <td valign=\"top\" class=\"mcnTextBlockInner\" style=\"padding-top:9px;\"><!--[if mso]><table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100%;\"><tr><![endif]--><!--[if mso]><td valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:100%; min-width:100%;\" width=\"100%\" class=\"mcnTextContentContainer\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\" style=\"padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;\"> <h1><span style=\"font-size:14px\">Asignación del plan de negocio{{proyecto}}&nbsp;&nbsp;</span></h1><p style=\"text-align:center !important;\">Se le asigno el proyecto{{proyecto}}para realizar la evaluación, favor tener en cuenta la siguiente información:</p><ol><li style=\"text-align: left;\">Proyecto{{proyecto}}&nbsp;</li><li style=\"text-align: left;\">Convocatoria{{convocatoria}}</li><li style=\"text-align: left;\">Evaluador{{evaluador}}</li><li style=\"text-align: left;\">Coordinador de evaluación{{coordinador}}</li><li style=\"text-align: left;\">Fecha de asignación{{fechaasignacion}}</li></ol> </td></tr></tbody></table><!--[if mso]></td><![endif]--><!--[if mso]></tr></table><![endif]--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width:100%; padding:18px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width: 100%;border-top: 2px solid #EAEAEA;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnTextBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnTextBlockOuter\"> <tr> <td valign=\"top\" class=\"mcnTextBlockInner\" style=\"padding-top:9px;\"><!--[if mso]><table align=\"left\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" style=\"width:100%;\"><tr><![endif]--><!--[if mso]><td valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"max-width:100%; min-width:100%;\" width=\"100%\" class=\"mcnTextContentContainer\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\" style=\"padding-top:0; padding-right:18px; padding-bottom:9px; padding-left:18px;\"> <span style=\"color:#000000\"><span style=\"font-size:22px\"><strong>Fechas de evaluación</strong></span></span> </td></tr></tbody></table><!--[if mso]></td><![endif]--><!--[if mso]></tr></table><![endif]--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 18px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 18px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width:100%; padding:18px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnCaptionBlock\"> <tbody class=\"mcnCaptionBlockOuter\"> <tr> <td class=\"mcnCaptionBlockInner\" valign=\"top\" style=\"padding:9px;\"> <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionRightContentOuter\" width=\"100%\"> <tbody><tr> <td valign=\"top\" class=\"mcnCaptionRightContentInner\" style=\"padding:0 9px ;\"> <table align=\"left\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" class=\"mcnCaptionRightImageContentContainer\" width=\"264\"> <tbody><tr> <td class=\"mcnCaptionRightImageContent\" align=\"center\" valign=\"top\"> <img alt=\"\" src=\"https://gallery.mailchimp.com/f97337609833a5aabaf671140/images/aaccc3eb-1ca1-40fe-ba02-4312cc341203.jpg\" width=\"264\" style=\"max-width:268px;\" class=\"mcnImage\"> </td></tr></tbody></table> <table class=\"mcnCaptionRightTextContentContainer\" align=\"right\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"264\"> <tbody><tr> <td valign=\"top\" class=\"mcnTextContent\"> <h3><span style=\"font-size:14px\">Agendamiento de preguntas de evaluación al emprendedor</span></h3><ul><li><span style=\"font-size:14px\">{{evalday3}}</span></li></ul><h3><span style=\"font-size:14px\">Respuesta de emprendedores</span></h3><ul><li><span style=\"font-size:14px\">{{empday3}}</span></li></ul><h3><span style=\"font-size:14px\">Informe del evaluador</span></h3><ul><li><span style=\"font-size:14px\">{{infoday4}}</span></li></ul> </td></tr></tbody></table> </td></tr></tbody></table> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 27px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table><table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"mcnDividerBlock\" style=\"min-width:100%;\"> <tbody class=\"mcnDividerBlockOuter\"> <tr> <td class=\"mcnDividerBlockInner\" style=\"min-width: 100%; padding: 27px 18px 0px;\"> <table class=\"mcnDividerContent\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"min-width:100%;\"> <tbody><tr> <td> <span></span> </td></tr></tbody></table><!-- <td class=\"mcnDividerBlockInner\" style=\"padding: 18px;\"> <hr class=\"mcnDividerContent\" style=\"border-bottom-color:none; border-left-color:none; border-right-color:none; border-bottom-width:0; border-left-width:0; border-right-width:0; margin-top:0; margin-right:0; margin-bottom:0; margin-left:0;\"/>--> </td></tr></tbody></table></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr><tr><td align=\"center\" valign=\"top\" id=\"templateFooter\" data-template-container><!--[if (gte mso 9)|(IE)]><table align=\"center\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" width=\"600\" style=\"width:600px;\"><tr><td align=\"center\" valign=\"top\" width=\"600\" style=\"width:600px;\"><![endif]--><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"templateContainer\"><tr> <td valign=\"top\" class=\"footerContainer\"></td></tr></table><!--[if (gte mso 9)|(IE)]></td></tr></table><![endif]--></td></tr></table> </td></tr></table> </center> </body></html>";

			MailMessage mail;
            mail = new MailMessage();
            mail.To.Add(new MailAddress(emailDestinatario));
            mail.From = new MailAddress(emailRemitente);
            mail.Subject = "Asignación de plan de negocios " + proyecto;

			#region Reemplazo estructura antigua correo
			//mail.Body = bodyTemplate
			//    .ReplaceWord("{{proyecto}}", proyecto)
			//    .ReplaceWord("{{convocatoria}}", convocatoria)
			//    .ReplaceWord("{{evaluador}}", evaluador)
			//    .ReplaceWord("{{coordinador}}", coordinador)
			//    .ReplaceWord("{{fechaasignacion}}", " " + fechaAsignacion.getFechaConFormato())
			//    .ReplaceWord("{{evalday1}}", fechaAsignacion.AddDays(1).getFechaConFormato())
			//    .ReplaceWord("{{evalday2}}", fechaAsignacion.AddDays(2).getFechaConFormato())
			//    .ReplaceWord("{{evalday3}}", fechaAsignacion.AddDays(3).getFechaConFormato())
			//    .ReplaceWord("{{empday1}}", fechaAsignacion.AddDays(4).getFechaConFormato())
			//    .ReplaceWord("{{empday2}}", fechaAsignacion.AddDays(5).getFechaConFormato())
			//    .ReplaceWord("{{empday3}}", fechaAsignacion.AddDays(6).getFechaConFormato())
			//    .ReplaceWord("{{infoday1}}", fechaAsignacion.AddDays(7).getFechaConFormato())
			//    .ReplaceWord("{{infoday2}}", fechaAsignacion.AddDays(8).getFechaConFormato())
			//    .ReplaceWord("{{infoday3}}", fechaAsignacion.AddDays(9).getFechaConFormato())
			//    .ReplaceWord("{{infoday4}}", fechaAsignacion.AddDays(10).getFechaConFormato());
			#endregion

			mail.Body = bodyTemplate
			   .ReplaceWord("{{proyecto}}", proyecto)
			   .ReplaceWord("{{convocatoria}}", convocatoria)
			   .ReplaceWord("{{evaluador}}", evaluador)
			   .ReplaceWord("{{coordinador}}", coordinador)
			   .ReplaceWord("{{fechaasignacion}}", " " + fechaAsignacion.getFechaConFormato())
			   .ReplaceWord("{{evalday1}}", "")
			   .ReplaceWord("{{evalday2}}", "")
			   .ReplaceWord("{{evalday3}}", fechaAsignacion.AddDays(3).getFechaConFormato())
			   .ReplaceWord("{{empday1}}", "")
			   .ReplaceWord("{{empday2}}", "")
			   .ReplaceWord("{{empday3}}", fechaAsignacion.AddDays(6).getFechaConFormato())
			   .ReplaceWord("{{infoday1}}", "")
			   .ReplaceWord("{{infoday2}}", "")
			   .ReplaceWord("{{infoday3}}", "")
			   .ReplaceWord("{{infoday4}}", fechaAsignacion.AddDays(10).getFechaConFormato());

			mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;

            var smtp = ConfigurationManager.AppSettings.Get("SMTP");
            var port = int.Parse(ConfigurationManager.AppSettings.Get("SMTP_UsedPort"));
            SmtpClient client = new SmtpClient(smtp, port);
            using (client)
            {
                var usuarioEmail = ConfigurationManager.AppSettings.Get("SMTPUsuario");
                var passwordEmail = ConfigurationManager.AppSettings.Get("SMTPPassword");
                client.Credentials = new System.Net.NetworkCredential(usuarioEmail, passwordEmail);
                client.EnableSsl = false;
                //client.
                client.Send(mail);
                errorMessage = true;
            }
            return errorMessage;
        }
    }
}