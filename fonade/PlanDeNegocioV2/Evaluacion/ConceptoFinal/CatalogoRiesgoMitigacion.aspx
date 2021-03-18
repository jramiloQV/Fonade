<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="CatalogoRiesgoMitigacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.CatalogoRiesgoMitigacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
<div>
    <table class="style1" style="width: 500px;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="L_Riesgo" runat="server" Text="Riesgo"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TB_Riesgo" runat="server" Width="100%" Height="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFV_Riesgo" runat="server" 
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="TB_Riesgo" 
                        ValidationGroup="crear" Text="Campo Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="L_Mitigacion" runat="server" Text="Mitigacion"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TB_Mitigacion" runat="server" Width="100%" Height="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFV_Mitigacion" runat="server" 
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="TB_Mitigacion" 
                        ValidationGroup="crear" Text="Campo Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align:center;"><asp:Button ID="B_Crear" runat="server" Text="Crear" 
                        CssClass="boton_Link_Grid" onclick="B_Crear_Click" ValidationGroup="crear" /></td>
                <td></td>
            </tr>
        </table>
    </div>
</asp:Content>
