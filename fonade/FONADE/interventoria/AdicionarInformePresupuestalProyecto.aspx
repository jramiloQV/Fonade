<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdicionarInformePresupuestalProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.AdicionarInformePresupuestalProyecto" %>

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
        <AjaxControlToolkit:ToolkitScriptManager ID="tlkn_2" runat="server">
        </AjaxControlToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table>
                    <thead>
                        <tr>
                            <th colspan="2">
                                FORMATO 01A
                            </th>
                        </tr>
                        <tr>
                            <th colspan="2">
                                INFORME DE SEGUIMIENTO PRESUPUESTAL DE LA INTERVENTORIA
                            </th>
                        </tr>
                        <tr>
                            <th colspan="2">
                                <asp:Label ID="NombreInterventor" runat="server" Text="Interventor " />
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Coordinador
                            </td>
                            <td>
                                <asp:Label ID="NombreCoordinador" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Periodo
                            </td>
                            <td>
                                <asp:Label ID="lblPeriodo" runat="server" Text="" />
                                <asp:DropDownList ID="dd_Periodo" runat="server">
                                    <asp:ListItem Value="">Seleccione el periodo</asp:ListItem>
                                    <asp:ListItem Value="1">Enero-Febrero</asp:ListItem>
                                    <asp:ListItem Value="2">Marzo-Abril</asp:ListItem>
                                    <asp:ListItem Value="3">Mayo-Junio</asp:ListItem>
                                    <asp:ListItem Value="4">Julio-Agosto</asp:ListItem>
                                    <asp:ListItem Value="5">Septiembre-Octubre</asp:ListItem>
                                    <asp:ListItem Value="6">Noviembre-Diciembre</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Contrato
                            </td>
                            <td>
                                <asp:Label ID="NumeroContrato" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fecha
                            </td>
                            <td>
                                <asp:Label ID="lblFecha" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Empresa
                            </td>
                            <td>
                                <asp:Label ID="lblEmpresa" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Teléfono
                            </td>
                            <td>
                                <asp:Label ID="lblTelefono" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Dirección
                            </td>
                            <td>
                                <asp:Label ID="lblDireccion" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Ciudad
                            </td>
                            <td>
                                <asp:Label ID="lblCiudad" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Socios
                            </td>
                            <td>
                                <asp:Panel ID="pSocios" runat="server">
                                    <asp:Table ID="t_table" runat="server">
                                    </asp:Table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:Button ID="btn_imprimir" Text="Imprimir" runat="server" OnClick="btn_imprimir_Click" />
                <br />
                <br />
                <asp:Panel ID="p_iB" runat="server" Width="100%">
                    <asp:Table ID="t_variable" runat="server" class="Grilla">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Código</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Ámbito</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cumplimiento a verificar</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Observación Interventor</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cumple</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Hacer Seguimiento</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Eliminar</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Modificar</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="8"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </ContentTemplate>
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_IngresarInforme" EventName="Click" />
            </Triggers>--%>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
