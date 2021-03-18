<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="TerminosYCondiciones.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.TerminosYCondiciones.TerminosYCondiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
        <ContentTemplate>
                        
            <asp:Image ID="imgLogoFondoEmprender" ImageAlign="Left" runat="server" ImageUrl="~/Images/Img/LogoFE.png" Width="288" Height="92" />            
            
            <br style="clear: both;" />

            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <h1 >
                    <asp:Label Text="Certifico" runat="server" ID="lblTitle" Visible="true" />                
                </h1>
            </div>
   
            <p>
                Yo <asp:Label Text="Nombre del emprendedor" runat="server" ID="lblNombreEmprendedor" Font-Bold="true" /> 
                , identificado con la CC No. <asp:Label Text="123456789" runat="server" ID="lblCedulaEmprendedor" Font-Bold="true" /> 
                acepto la política de datos personales y confidencialidad de la información personal, comercial e industrial, 
                que suministro a través del presente portal, así como lo que sea entregada en el marco del proceso de postulación,
                evaluación, acreditación, asignación de recursos, y seguimiento a éstos.
                Declaro que acepto y conozco la reglamentación vigente que enmarca el Fondo Emprender y que me comprometo a cumplir 
                y a someterme a las condiciones establecidas por ésta y los términos de referencia de las convocatorias a que a título 
                personal o en colectivo me postule.
                Declaro bajo la gravedad de juramento que a la fecha no me encuentro incurso en algún régimen de inhabilidades e incompatibilidad 
                para contratar o recibir recursos del estado y me comprometo a notificar de manera inmediata tanto al SENA, de la ocurrencia de alguna de estas circunstancias con posterioridad a la presente declaración, 
                so pena de la terminación de la relación contractual o de la terminación del proceso por parte de alguna de estas entidades. 
                Acepto que en caso de presentarse alguna de las causales de inhabilidad e incompatibilidad para contratar con el estado o cualquier 
                otra inhabilidad por vía administrativa o judicial, el SENA puede dar por terminada la relación establecida, 
                el giro de los recursos y ordenar el reintegro de ellos, así como finalizar el proceso de postulación en las convocatorias del Fondo Emprender.
            </p>

            <br />            
            <asp:updateprogress id="UpdateProgress1" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true" >
                                <progresstemplate>
                                    <div class="form-group center-block">                                                                 
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <label class="control-label"> 
                                                <b>Procesando información</b> </label><img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                        </div>
                                    </div>
                                </progresstemplate>

            </asp:updateprogress>            
            <asp:CheckBox ID="chkTerminos"  runat="server" Text="Autorizo al SENA para que verifique mis datos personales, antecedentes y situación personal en las bases de datos, centrales de riesgos, boletines públicos y privados que estimen pertinentes."/>
            <br />           
            <br /> 
                <asp:Label ID="lblError" Text="Sucedio un error" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Medium" Visible="false"></asp:Label>
            <br />  
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Button ID="btnAceptarTerminos" runat="server" Text="Aceptar" Visible="true" OnClick="btnAceptarTerminos_Click" />
                <asp:Button ID="btnCancelarTerminos" runat="server" Text="Cancelar" Visible="true" PostBackUrl="~/Account/Login.aspx" />   
                <asp:Button ID="btnCancelarTerminosFormalizar" runat="server" Text="Cancelar" Visible="true" PostBackUrl="~/FONADE/Proyecto/ProyectoFormalizar.aspx" /> 
            </div>                     
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>