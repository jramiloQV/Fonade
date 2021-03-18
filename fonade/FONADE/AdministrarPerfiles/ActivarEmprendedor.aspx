<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivarEmprendedor.aspx.cs"
    Inherits="Fonade.FONADE.AdministrarPerfiles.ActivarEmprendedor" MasterPageFile="~/Master.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="panelInactivo" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellspacing="0" cellpadding="2" width="95%" border="0" id="table1">
                <tbody>
                    <tr>
                        <td valign="baseline" align="middle" width="175">
                            <h1>
                                <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700" /></h1>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" colspan="4" id="table6">
                <tbody>
                    <tr valign="top">
                        <td width="19%">
                        </td>
                    </tr>
                    <tr>
                        <td valing="middle" width="19%">
                            <b>Nombre(s):</b>
                        </td>
                        <td width="26%">
                            <asp:TextBox ID="txnombres" runat="server" MaxLength="80" size="20" />
                        </td>
                        <td width="20%">
                            <b>Apellidos(s):</b>
                        </td>
                        <td valing="middle" width="32%">
                            <asp:TextBox ID="txapellidos" runat="server" MaxLength="80" size="20" />
                        </td>
                    </tr>
                    <tr>
                        <td valing="middle" width="19%">
                            <b>Email:</b>
                        </td>
                        <td width="26%">
                            <asp:TextBox ID="txemail" runat="server" MaxLength="80" size="20" />
                        </td>
                        <td width="20%">
                            <b>Identificación:</b>
                        </td>
                        <td valing="middle" width="32%">
                            <asp:TextBox ID="txidentificacion" runat="server" MaxLength="30" size="20" language="JScript"
                                onkeypress="if ((event.keyCode < 48) || (event.keyCode > 57)) event.returnValue = false;" />
                        </td>
                    </tr>
                    <tr>
                        <td valing="middle" width="19%">
                            <b>Dirección:</b>
                        </td>
                        <td width="26%">
                            <asp:TextBox ID="txdireccion" runat="server" MaxLength="200" size="20" />
                        </td>
                        <td width="20%">
                            <b>Teléfono:</b>
                        </td>
                        <td valing="middle" width="32%">
                            <asp:TextBox ID="txtelefono" runat="server" MaxLength="80" size="20" />
                        </td>
                    </tr>
                    <tr>
                        <td align="middle" colspan="4">
                            <br />
                            <asp:Button ID="BtnReactivar" runat="server" Text="Re-Activar" OnClick="BtnReactivar_Click" />
                        </td>
                    </tr>
                    <input type="hidden" value="ConsultarPlanNeg" name="Accion">
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
</asp:Content>
