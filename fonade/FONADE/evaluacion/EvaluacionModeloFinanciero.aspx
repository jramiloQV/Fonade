<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionModeloFinanciero.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionModeloFinanciero" %>

<%@ Register Src="../../Controles/Alert.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc3" %>--%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
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
</head>
<body>
    <form id="form1" runat="server">
    <asp:UpdatePanel ID="PanelGrilla" runat="server" Width="100%" CssClass="myGridClass"
        UpdateMode="Conditional">
        <ContentTemplate>
            <div style="background-color: #fff;">
                <uc2:Alert ID="Alert1" runat="server" />
                <%--<table border="0" width="100%" style="background-color: White">
                <tr>
                    <td class="auto-style5">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 50%">                                    
                                    <uc3:CtrlCheckedProyecto ID="CtrlCheckedProyecto1" runat="server" />                                    
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>--%>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                ULTIMA ACTUALIZACIÓN:&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                            </td>
                            <td style="width: 20px;">
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                                    runat="server" TextAlign="Left" />
                                &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                                    ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="False" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <br />
                <table border="0" width="100%" style="background-color: White">
                    <tr>
                        <td class="auto-style5">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Modelo Financiero', texto: 'ModeloFinanciero'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                                            </div>
                                            <div>
                                                &nbsp; <strong>Modelo Financiero:</strong>
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="div_Post_It1" runat="server" visible="false">
                                            <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="ModeloFinanciero" _txtTab="1" _mostrarPost="false" />
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
                                                        <%--<br />
                                                        &nbsp;</a><asp:HyperLink ID="btn_descargarmodelo" runat="server" Text="Bajar el Formato del Modelo Financiero"
                                                            Width="264px" Target="_blank" NavigateUrl="~/FONADE/Plantillas/modelofinanciero.xls" />--%>
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
                                                            <%--<asp:Button ID="btn_vermodelo" runat="server" Text="Ver Modelo Financiero" CssClass="boton_Link"
                                                            Width="160px" OnClick="Button1_Click" />
                                                            <br />--%>
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
    </form>
</body>
</html>

  
