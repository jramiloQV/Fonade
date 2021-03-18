<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgregarInformeFinalInterventoriaProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.AgregarInformeFinalInterventoriaProyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>INFORME PRESUPUESTAL</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <AjaxControlToolkit:ToolkitScriptManager ID="tlkn_fnl" runat="server">
        </AjaxControlToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <h1>
                    <asp:Label ID="L_titulo" runat="server" Text="INFORMES DE INTERVENTORÍA">

                    </asp:Label>
                </h1>
                <br />
                <br />
                <div id="contenido">
                    <table>
                        <thead>
                            <tr>
                                <th colspan="4">
                                    <asp:Label ID="lblinforme" runat="server" Text="Interventor " Visible="false"></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <th colspan="4">
                                    <asp:Label ID="L_TituloNombre" runat="server" Text="Interventor " Visible="false"></asp:Label>
                                </th>
                            </tr>
                            <tr>
                                <th colspan="4">
                                    <asp:Label ID="lblnomcoordinador" runat="server" Text="Interventor " Visible="false"></asp:Label>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="4">
                                    <br />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Número Contrato:
                                </td>
                                <td>
                                    <asp:Label ID="lblnumContrato" runat="server" Text=""></asp:Label>
                                </td>
                                <td>
                                    Fecha Informe:
                                </td>
                                <td>
                                    <asp:Label ID="lblfechainforme" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Empresa
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Teléfono
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Dirección
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Socios
                                </td>
                                <td colspan="2">
                                    <asp:Panel ID="pSocios" runat="server">
                                        <asp:Table ID="t_table" runat="server">
                                        </asp:Table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />
                    <br />
                    <asp:Panel ID="p_iB" runat="server" Width="100%">
                        <asp:Table ID="t_variable" runat="server" class="Grilla">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>CRITERIO</asp:TableHeaderCell>
                                <asp:TableHeaderCell>CUMPLIMIENTO A VERIFICAR</asp:TableHeaderCell>
                                <asp:TableHeaderCell>OBSERVACIÓN INTERVENTOR</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="3"></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:Panel>
                    <br />
                    <br />
                    <br />
                    <div id="dvianexos">
                        <asp:Panel ID="p_Anexos" runat="server">
                            <asp:Table ID="t_anexos" runat="server" class="Grilla">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell ColumnSpan="2">ANEXOS</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="2"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                        <br />
                        <br />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
