<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CatalogoEvaluacionConceptosAdicionar.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoEvaluacionConceptosAdicionar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
    .sinlinea {
            border:none;
            border-collapse:collapse;
            border-bottom-color:none;
        }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="NUEVO" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr style="text-align:center;">
            <td>
                <br />
                <br />
                <asp:TextBox ID="TB_Nuevo" runat="server" Width="300px" ValidationGroup="nuevo"></asp:TextBox>
                <br />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="RequiredFieldValidator" ControlToValidate="TB_Nuevo" ValidationGroup="nuevo" ForeColor="Red">Campo Requerido</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr style="text-align:center;">
            <td>
                <br />
                <br />
                <asp:Button ID="B_Nuevo" runat="server" Text="Adicionar" OnClick="B_Nuevo_Click" ValidationGroup="nuevo" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" PostBackUrl="~/FONADE/evaluacion/CatalogoEvaluacionConceptos.aspx" />
            </td>
        </tr>
    </table>

</asp:Content>