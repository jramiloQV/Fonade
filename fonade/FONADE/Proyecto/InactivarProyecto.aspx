<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="InactivarProyecto.aspx.cs" Inherits="Fonade.FONADE.Proyecto.InactivarProyecto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
        <tbody>
            <tr>
                <td align="center" valign="top" width="98%">
                    <table width="98%" border="0" cellspacing="0" cellpadding="0">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table width="98%" border="0" cellspacing="0" cellpadding="3">
                        <tbody>
                            <tr valign="top">
                                <td align="left">
                                    <b>Motivo de Inactivación:</b>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td width="167" align="left">
                                    <asp:TextBox ID="MotivoInactivacion" runat="server" MaxLength="300" TextMode="MultiLine"
                                        Columns="60" Rows="5" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td colspan="4" align="right">
                                    <asp:Button ID="btnInactivar" Text="Inactivar" runat="server" Visible="false" 
                                        onclick="btnInactivar_Click" />
                                    <asp:Button ID="btnCerrar" Text="Cerrar" runat="server" OnClick="btnCerrar_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
