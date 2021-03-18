<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="modificarusuario.aspx.cs" Inherits="Fonade.FONADE.Administracion.modificarusuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="MODIFICAR USUARIO" />
    </h1>
    <%--<p>
        <asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
            Width="35%" Text="MODIFICAR USUARIO" />
        <asp:Label ID="lbl_Interventor" runat="server" Width="40%" />
        <asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
    </p>--%>
    <asp:Panel ID="pnlPrincipal" runat="server">
        <table>
            <tbody>
                <tr>
                    <td valign="middle" width="19%">
                        <b>Nombre(s):</b>
                    </td>
                    <td width="26%">
                        <asp:TextBox ID="txt_Nombres" runat="server" MaxLength="80" />
                    </td>
                    <td width="20%">
                        <b>Apellidos(s):</b>
                    </td>
                    <td valign="middle" width="32%">
                        <asp:TextBox ID="txt_Apellidos" runat="server" MaxLength="80" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" width="19%">
                        <b>Email:</b>
                    </td>
                    <td width="26%">
                        <asp:TextBox ID="txt_Email" runat="server" MaxLength="80" />
                    </td>
                    <td width="20%">
                        <b>Identificación:</b>
                    </td>
                    <td valign="middle" width="32%">
                        <asp:TextBox ID="txt_Identificacion" runat="server" MaxLength="80" onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" width="19%">
                        <b>Dirección:</b>
                    </td>
                    <td width="26%">
                        <asp:TextBox ID="txt_Direccion" runat="server" MaxLength="200" />
                    </td>
                    <td width="20%">
                        <b>Teléfono:</b>
                    </td>
                    <td valign="middle" width="32%">
                        <asp:TextBox ID="txt_Telefono" runat="server" MaxLength="80" onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
                    </td>
                </tr>
                <tr>
                    <td valign="middle" width="19%">
                        <b>Inactivo:</b>
                    </td>
                    <td width="26%">
                        <asp:DropDownList ID="dd_EstaInactivo" runat="server" >
                            <asp:ListItem Text="SI" Value="1" />
                            <asp:ListItem Text="NO" Value="0" />
                        </asp:DropDownList>
                    </td>
                    <td width="20%">
                        <b>Bloqueado</b>
                    </td>
                    <td valign="middle" width="32%">
                        <asp:DropDownList ID="dd_EstaBloqueado" runat="server">
                            <asp:ListItem Text="SI" Value="1" />
                            <asp:ListItem Text="NO" Value="0" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="100%" border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chk_1" Text="Acreditador de planes de negocio?" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_2" Text="Generar actas parciales?" runat="server" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chk_3" Text="Generar reporte final?" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="width:100%">
           <div style="text-align: center;">
              <asp:Button ID="btn_Modificar" Text="Modificar" runat="server" OnClick="btn_Modificar_Click" />
           </div>
        </div>
    </asp:Panel>
</asp:Content>
