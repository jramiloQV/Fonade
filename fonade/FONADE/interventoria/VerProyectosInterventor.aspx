<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerProyectosInterventor.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.VerProyectosInterventor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONDO EMPRENDER</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="ContentInfo">
        <asp:Panel ID="pnlprincipal" runat="server">
        <asp:Label ID="lbl_enunciado" Text="INTERVENTOR" runat="server" />
        <table>
            <tr align="center">
                <td>
                    <h5>
                        EMPRESAS PARA</h5>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gv_Empresas" runat="server" AutoGenerateColumns="false" OnRowDataBound="gv_Empresas_RowDataBound"
                        GridLines="None" RowStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                        CssClass="Grilla">
                        <Columns>
                            <asp:BoundField HeaderText="Empresa" DataField="razonsocial" />
                            <asp:BoundField HeaderText="Rol" DataField="Rol" />
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_fecha_formateada" Text='<%# Eval("FechaInicio") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr align="center">
                <td>
                    <asp:LinkButton ID="lnk_btn_Cerrar" Text="Cerrar" runat="server" OnClick="lnk_btn_Cerrar_Click"
                        ForeColor="Red" Style="text-decoration: none;" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    </div>
        </form>
</body>
</html>
