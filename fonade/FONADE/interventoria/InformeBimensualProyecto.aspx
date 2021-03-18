<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeBimensualProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.InformeBimensualProyecto" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FONDO EMPRENDER - Informe Bimensual Proyecto</title>
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
                                        <asp:GridView ID="gv_InformesBimensuales" runat="server" Width="98%" AutoGenerateColumns="false"
                                            CssClass="Grilla" AllowPaging="True" OnPageIndexChanging="gv_InformesBimensuales_PageIndexChanging"
                                            OnSorting="gv_InformesBimensuales_Sorting" ShowHeaderWhenEmpty="true" GridLines="None"
                                            CellSpacing="1" CellPadding="4" OnRowCommand="gv_InformesBimensuales_RowCommand"
                                            RowStyle-VerticalAlign="Top" PageSize="10">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="3%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Numregistro" runat="server" ForeColor="Black" Text='<%# Eval("Id_InformeBimensual") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre del Informe" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_VerInformeVisita" runat="server" CausesValidation="false"
                                                            CommandName="VerInformeBimensual" ForeColor="Black" Text='<%# Eval("NomInformeBimensual") %>'
                                                            CommandArgument='<%# Eval("Id_InformeBimensual") + ";" + Eval("Periodo")+ ";" + Eval("CodEmpresa")+ ";" + Eval("CodProyecto") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:HiddenField ID="hdf_CodEmpresa" runat="server" />
                                        <br />
                                    </td>
                                </tr>
                                <tr id="tr_adicionar" runat="server" align="center" visible="false">
                                    <td colspan="5">
                                        <asp:DropDownList ID="CodEmpresa" runat="server" AutoPostBack="true" Height="16px"
                                            Width="449px" />
                                        <br />
                                        <asp:Button ID="btn_adicionar" Text="Adicionar" runat="server" />
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
