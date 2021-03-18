<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="VerPerfilContacto.aspx.cs" Inherits="Fonade.FONADE.evaluacion.VerPerfilContacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="95%" border="0" cellspacing="0" cellpadding="2">
        <tr>
            <td width="175" align="center" valign="baseline" bgcolor="#3D5A87" style="color: White;">
                PERFIL USUARIO
            </td>
            <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="1">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNombre" runat="server" />
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_tiempo" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:Label ID="lbl_perfil" runat="server" />
    <br />
    <asp:Panel ID="pnl_Cerrar" runat="server" Width="95%" border="0" cellspacing="0"
        cellpadding="2" HorizontalAlign="Right">
        <asp:Button ID="btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="window.close();" />
    </asp:Panel>
</asp:Content>
