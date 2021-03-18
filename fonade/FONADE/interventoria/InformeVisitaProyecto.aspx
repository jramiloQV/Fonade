<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeVisitaProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.InformeVisitaProyecto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONDO EMPRENDER - </title>
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
        <table width="780" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td colspan="3" align="left">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="215" align="center" valign="top">
                    </td>
                    <td width="565" align="left" valign="top">
                        <table width="95%" border="0" cellspacing="0" cellpadding="2">
                            <tbody>
                                <tr bgcolor="#00468f">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
                            <tbody>
                                <tr>
                                    <td align="center" valign="top" width="98%">
                                       <%-- <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>--%>
                                        <asp:GridView ID="gv_InformesVisita" runat="server" Width="98%" AutoGenerateColumns="false"
                                            CssClass="Grilla" AllowPaging="True" OnPageIndexChanging="gv_InformesVisita_PageIndexChanging"
                                            OnSorting="gv_InformesVisita_Sorting" ShowHeaderWhenEmpty="false" EmptyDataText="No se encontraron coincidencias."
                                            GridLines="None" CellSpacing="1" CellPadding="4" OnRowCommand="gv_InformesVisita_RowCommand"
                                            RowStyle-VerticalAlign="Top" PageSize="10">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Numregistro" runat="server" ForeColor="Black" Text='<%# Eval("NUM") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre del Informe" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_VerInformeVisita" runat="server" CausesValidation="false"
                                                            CommandName="VerInformeVisita" ForeColor="Black" Text='<%# Eval("NombreInforme") %>'
                                                            CommandArgument='<%# Eval("Id_Informe") + ";" + Eval("NombreInforme") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="CodEmpresa" runat="server" />
                                        <br />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                       <%-- <table width="95%" border="0" cellspacing="0" cellpadding="0">
                            <tbody>
                                <tr>
                                    <td bgcolor="#00468f">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#00468f">
                                        &nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>--%>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
