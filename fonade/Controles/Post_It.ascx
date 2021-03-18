<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Post_It.ascx.cs" Inherits="Fonade.Controles.Post_It" %>

<%--diseño del postit--%>
<style type="text/css">
    .fonTab {
        background-image: url('/Images/bgrAnotacion2.gif');
    }

    .encima {
        background-image: url('/Images/bgrIcoPos2.gif');
        background-repeat: no-repeat;
        background-position: center;
        text-align: center;
        color: black;
        text-decoration: none;
    }
</style>

<%--panel principal del control--%>
<asp:Panel ID="P_PostIt" runat="server" CssClass="fonTab" Width="160px" Height="30px">
    <%--tabla de control--%>
    <table style="width: 160px">
        <tr>
            <td style="width: 68px; text-align: center;">
                <asp:Label ID="L_Nota" runat="server" Text="NOTAS:"></asp:Label>
            </td>
            <td style="width: 46px; text-align: center;">
                <%--icono que permite agregar una nueva tarea--%>
                <asp:ImageButton ID="I_POs" runat="server" ImageUrl="~/Images/icoNuevo.gif" OnClick="I_POs_Click" />
            </td>
            <td style="text-align: center; width: 46px">
                <%--icono que muestra las tareas listadas--%>
                <asp:LinkButton ID="LB_Listar" runat="server" Text="0" CssClass="encima" Width="100%" OnClick="LB_Listar_Click" Visible="false"></asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Panel>
