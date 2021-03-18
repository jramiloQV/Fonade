<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ObligarCargarAnexosActualizacionDatos.aspx.cs" Inherits="Fonade.FONADE.Administracion.ObligarCargarAnexosActualizacionDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="ACTUALIZACIÓN DE INFORMACIÓN" />
    </h1>
    <asp:Panel ID="pnlPrincipal" runat="server">
        <table width="95%" border="1" cellpadding="0" cellspacing="0" style="border-color: #4E77AF;">
            <tbody>
                <tr>
                    <td align="center" valign="top" width="98%">
                        <table width="95%" border="0" align="center" cellspacing="0" cellpadding="3">
                            <tbody>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="left">
                                        <b>Actualizacion de datos:</b>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="left">
                                        <asp:CheckBox ID="chk_actualizarInfo" Text="Obligar carga Anexos:" runat="server"
                                            TextAlign="Left" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" class="TitDestacado">
                                        <asp:Button ID="btn_Actualizar" Text="Actualizar" runat="server" ToolTip="Actualizar"
                                            OnClick="btn_Actualizar_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
