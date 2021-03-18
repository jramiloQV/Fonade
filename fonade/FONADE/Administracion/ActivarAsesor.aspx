<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ActivarAsesor.aspx.cs" Inherits="Fonade.FONADE.Administracion.ActivarAsesor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" Text="ASESORES INACTIVOS - REACTIVAR" />
    </h1>
    <%--<p>
        <asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
            Width="35%" Text="MODIFICAR USUARIO" />
        <asp:Label ID="lbl_Interventor" runat="server" Width="40%" />
        <asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
    </p>--%>
    
        <table width="100%" border="0" id="table6">
            <tbody>
                <tr valign="top">
                    <td class="tituloDestacados" width="19%">
                    </td>
                </tr>
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
                        <asp:TextBox ID="txt_Identificacion" runat="server" MaxLength="80" language="JScript"
                            onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
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
                        <asp:TextBox ID="txt_Telefono" runat="server" MaxLength="80" />
                    </td>
                </tr>
                <tr>
                    <td align="middle" colspan="4">
                        <asp:Button ID="btnEnviar" Text="Re-Activar" runat="server" 
                            onclick="btnEnviar_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
</asp:Content>
