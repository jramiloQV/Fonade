<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeConsolidadoProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.InformeConsolidadoProyecto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                                        <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                            <tbody>
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <asp:GridView ID="gv_InformesConsolidados" runat="server" Width="98%" AutoGenerateColumns="false"
                                            CssClass="Grilla" AllowPaging="True" OnPageIndexChanging="gv_InformesConsolidados_PageIndexChanging"
                                            OnSorting="gv_InformesConsolidados_Sorting" ShowHeaderWhenEmpty="true" GridLines="None"
                                            CellSpacing="1" CellPadding="4" OnRowCommand="gv_InformesConsolidados_RowCommand"
                                            RowStyle-VerticalAlign="Top" RowStyle-HorizontalAlign="Left" PageSize="10">
                                            <Columns>
                                                <asp:BoundField HeaderStyle-Width="3%" />
                                                <asp:TemplateField HeaderText="Nombre del Informe" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_VerInformeConsolidado" runat="server" CausesValidation="false"
                                                            CommandName="VerInformeConsolidado" ForeColor="Black" Text='<%# Eval("NomInterventorInformeFinal") %>'
                                                            CommandArgument='<%# Eval("Id_InterventorInformeFinal") + ";" + Eval("CodEmpresa")+ ";" + Eval("CodProyecto") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="FechaInforme" HeaderText="Fecha del Informe" HtmlEncode="false"
                                                    ConvertEmptyStringToNull="true" DataFormatString="{0:dd/MM/yyyy}" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Center" />
                                                <asp:TemplateField HeaderText="Estado" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Estado" runat="server" ForeColor="Black" Text='<%# Eval("NombreEstado") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="CodEmpresa" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table width="95%" border="0" cellspacing="0" cellpadding="0">
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
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
