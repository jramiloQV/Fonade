<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionModeloFinanciero.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.EvaluacionModeloFinanciero" %>

<%@ Register Src="~/Controles/Alert.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1
        {
            color: #FFFFFF;
            height: 30px;
            width: 630px;
        }
        
        .auto-style2
        {
            height: 47px;
        }
        
        .auto-style3
        {
            height: 47px;
            width: 50%;
        }
        
        .auto-style4
        {
            width: 630px;
        }
        
        .auto-style5
        {
            height: 22px;
        }
        .myGridClass
        {
            width: 100% !important;
        }
        .ContentInfo
        {
            width: 100% !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">

    <asp:UpdatePanel ID="PanelGrilla" runat="server" Width="100%" CssClass="myGridClass"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div style="background-color: #fff;">
                <uc2:Alert ID="Alert1" runat="server" />
                <uc1:EncabezadoEval runat="server" id="EncabezadoEval" /> 
                <br />           
                <table border="0" width="100%" style="background-color: White">
                    <tr>
                        <td class="auto-style5">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Modelo Financiero', texto: 'ModeloFinanciero'});">
                                                <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                                            </div>
                                            <div>
                                                &nbsp; <strong>Modelo Financiero:</strong>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="div_Post_It1" runat="server" visible="false">
                                            <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="ModeloFinanciero" _txtTab="1" _mostrarPost="false"/>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" style="height: 260px" bgcolor="White">
                    <tr>
                        <td valign="middle" align="center">
                            <table width="100%" style="border: medium solid #00468F; width: 640px;">
                                <tr>
                                    <td style="text-align: center; background-color: #00468F;" class="auto-style1">
                                        <strong style="font-size: 12pt">DILIGENCIAMIENTO DEL FORMATO</strong>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style4" valign="middle" align="center">
                                        <asp:Panel ID="PanelInfo" runat="server">
                                            Modelo Financiero
                                        </asp:Panel>
                                        <asp:Panel ID="PanelModelo" runat="server" Visible="False">
                                            <table width="100%">
                                                <tr>
                                                    <td valign="middle" align="center" class="auto-style3">
                                                        <asp:HyperLink ID="HyperLink1" Text="Bajar el Formato del Modelo Financiero" runat="server" Width="264px"
                                                            Target="_blank" NavigateUrl="~/FONADE/Plantillas/modelofinanciero.xls" />                                                        
                                                    </td>
                                                    <td valign="middle" align="center" class="auto-style2" style="width: 50%">
                                                        <asp:LinkButton ID="ImageButton2" runat="server" OnClick="ImageButton2_Click" Width="264px">Subir el modelo financiero</asp:LinkButton>                                                                                                           
                                                        <br />
                                                        <br />
                                                        <asp:LinkButton ID="btnLinkVerModeloFinanciero" runat="server" OnClick="btnLinkVerModeloFinanciero_Click" Visible="false" Width="264px">Ver modelo financiero cargado</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <asp:Panel ID="PanelModeloPropio" runat="server" Visible="False">
                                                        <td valign="middle" align="center" class="auto-style3">
                                                            <asp:HyperLink ID="HyperLink2" runat="server" Text="Guía para llenar Modelo Financiero"
                                                            Width="264px" Target="_blank" NavigateUrl="~/FONADE/Plantillas/GuiaModelo.doc" />
                                                        </td>
                                                        <td valign="middle" align="center" class="auto-style2" style="width: 50%">                                                            
                                                            <asp:LinkButton runat="server" ID="btn_bajar" CssClass="boton_Link" OnClick="Button1_Click">Ver Modelo Financiero</asp:LinkButton>
                                                        </td>
                                                    </asp:Panel>
                                                </tr>
                                            </table>
                                        </asp:Panel>
     
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

        
</asp:Content>
