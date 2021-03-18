<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="EvaluacionBloqueoEditar.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionBloqueoEditar" %>

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
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="BLOQUEO DE EVALUACION POR PROYECTO" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr style="text-align:center;">
            <td>
                <br />
                <br />
                <asp:Label ID="TB_Nuevo" runat="server" Text="Proyecto No "></asp:Label>
                <br />
                <asp:CheckBox ID="CB_Bloqueado" runat="server" Text="Bloqueado:" TextAlign="Left" />
            </td>
        </tr>
        <tr style="text-align:center;">
            <td>
                <br />
                <br />
                <asp:Button ID="B_Nuevo" runat="server" Text="Actualizar" OnClick="B_Nuevo_Click" ValidationGroup="Actualizar" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" PostBackUrl="~/FONADE/evaluacion/EvaluacionBloqueo.aspx" />
            </td>
        </tr>
    </table>

</asp:Content>