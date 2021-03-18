<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="DatosGenerales.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.DatosGenerales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
    
    <% Page.DataBind(); %>    
    <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />           
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" onclick="textoAyuda({titulo: 'Actividades a las que se dedicará la empresa', texto: 'EvaluacionLocalizacion'});">
                                Localización: </img>
                        </div>
                    </div>
                </td>
                <td>
                      <div id="div_post_it_1" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Objetivos" _txtTab="1" _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="txtLocalizacion" runat="server" CssClass="actividades" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../../Images/imgAyuda.gif" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Productos y servicios que ofrecerá', texto: 'EvaluacionSector'});">
                                Sector : </img>
                        </div>
                    </div>
                </td>
                 <td>
                      <div id="div2" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="Objetivos" _txtTab="1" _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                </td>
            </tr>
        </table>       
        <asp:TextBox ID="txtSector" runat="server" CssClass="actividades" TextMode="MultiLine" Height="146px" Width="100%" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../../Images/imgAyuda.gif" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Productos y servicios que ofrecerá', texto: 'ProductosServicios'});">
                                Resumen concepto general - Compromisos y condiciones : </img>
                        </div>
                    </div>
                </td>
                <td>
                      <div id="div1" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="Objetivos" _txtTab="1" _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>' />
                        </div>
                </td>
            </tr>
        </table>       
        <asp:TextBox ID="txtResumenConcepto" runat="server" CssClass="actividades" TextMode="MultiLine" Height="146px" Width="100%" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
    </div>      
    <br />
    <div align="center">
        <asp:Button ID="btnGuardar" runat="server" Text="Actualizar" class="Boton" OnClick="btnGuardar_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' ></asp:Button>
    </div>
    <br />
                </ContentTemplate>
    </asp:UpdatePanel>  
                                    
</asp:Content>
