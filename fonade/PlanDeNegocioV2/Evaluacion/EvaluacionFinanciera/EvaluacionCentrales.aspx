<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionCentrales.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.EvaluacionCentrales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1
        {
            width: 30px;
        }
        .auto-style4
        {
            width: 140px;
            font-weight: 700;
        }
        .auto-style5
        {
            width: 30px;
            height: 45px;
        }
        .auto-style6
        {
            width: 631px;
            height: 45px;
        }
        .auto-style7
        {
            height: 45px;
        }
        .auto-style10
        {
            width: 631px;
        }
        .myPanelClass
        {
            width: 100% !important;
            padding-right: 15px;
        }

        #CalendarExtender4_container{
            height: auto;
        }

        .ajax__calendar_header{
            height: 0px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <asp:LinqDataSource ID="lds_Integrantes" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_Integrantes_Selecting">
    </asp:LinqDataSource>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelCentrales" runat="server" Width="100%" CssClass="myPanelClass"
        UpdateMode="Conditional">
        <ContentTemplate>                             
            <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />
            <table border="0" width="100%" style="background-color: White">
                <tr>
                    <td>                                      
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 50%">
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Informe de Centrales de Riesgo', texto: 'CentralesRiesgo'});">
                                            <img alt="help_Objetivos" border="0" src="../../../Images/imgAyuda.gif"> </img>
                                        </div>
                                        <div>
                                            &nbsp; <strong>Informe de Centrales de Riesgo:</strong>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div id="div_Post_It1" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="CentralesRiesgo" _txtTab="1" _mostrarPost="false" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <table width="100%" style="background-color: White">
                <tr>
                    <td class="auto-style10" width="100%">
                        <asp:GridView ID="gv_Integrante" CssClass="Grilla" runat="server" AllowPaging="false"
                            AutoGenerateColumns="false" DataSourceID="lds_Integrantes" EmptyDataText="No hay información disponible."
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="Integrante">
                                    <ItemTemplate>
                                        <asp:Label ID="lintegrante" runat="server" Text='<%# Eval("NomCompleto") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Castigo">
                                    <ItemTemplate>
                                        <asp:Label ID="lcastigo" runat="server" Text='<%# Eval("Entidades") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cartera Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lcarteratotal" runat="server" Text='<%# Eval("ValorCartera") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Peor Calificación">
                                    <ItemTemplate>
                                        <asp:Label ID="lpeorcalificacion" runat="server" Text='<%# Eval("PeorCalificacion") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valor Mora Máximo">
                                    <ItemTemplate>
                                        <asp:Label ID="lvalormoramaximo" runat="server" Text='<%# Eval("CuentasCorrientes") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Día Mora Máximo">
                                    <ItemTemplate>
                                        <asp:Label ID="ldiamoramaximo" runat="server" Text='<%# Eval("ValorOtrasCarteras") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        <table width="100%">
                            <tr>
                                <td class="auto-style4" valign="baseline">
                                    Fecha del Reporte:
                                </td>
                                <td valign="baseline">
                                    <asp:TextBox runat="server" ID="txt_fechareporte" Enabled="false" BackColor="White"></asp:TextBox>&nbsp;
                                    <asp:Image runat="server" ID="btnimgcalend4" AlternateText="cal2" ImageUrl="/images/icoModificar.gif"
                                        ImageAlign="AbsMiddle"></asp:Image>
                                    <ajax:CalendarExtender ID="CalendarExtender4" TargetControlID="txt_fechareporte" 
                                        Format="dd/MM/yyyy" runat="server" PopupButtonID="btnimgcalend4" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        <strong>Observaciones:</strong>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        <asp:TextBox ID="txt_observaciones" runat="server" Height="80px" TextMode="MultiLine"
                            Width="99%" BackColor="White" />
                        <br />
                        <div id="div_observaciones" runat="server" style="height: 80px; width: 100%; background-color: White;"
                            visible="false">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style6" align="center">
                        <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" OnClick="btn_actualizar_Click" Visible="false"/>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>            
</asp:Content>
