<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameProduccionInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.FrameProduccionInter" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Producción</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function borrar() {
            return confirm('¿Esta seguro que desea borrar el avance seleccionado?');
        }
    </script>
    <style type="text/css">
        table#t_anexos tr:nth-child(3)
        {
            display: none !important;
        }
        #t_anexos tbody tr 
        {
            white-space:nowrap;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <div class="help_container">
                        <div>
                            <a onclick="textoAyuda({titulo: 'Producción', texto: 'Produccion'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" />
                                Producción: </a>
                        </div>
                    </div>
                </td>
                <td align="left" colspan="3">
                    <%--Campo para el botón--%>
                    <asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click">Imprimir</asp:LinkButton>
                </td>
                <td align="left" colspan="3" style="height: 26px; width: 70px"></td>
                <td align="left" colspan="3">
                    <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="Produccion" 
                        _txtTab="1" _mostrarPost="true"/>
                </td>
                <td align="left" colspan="3" style="height: 26px; width: 70px">
                </td>
                <td align="left" colspan="3">
                    <asp:Label runat="server" ID="lblpuestosPendientesConteo" Text="Productos Pendientes de Aprobar: 0" />
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 50%">
            <tr>
                <td style="width: 50%">
                    <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                    <asp:ImageButton ID="Adicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                        Style="cursor: pointer;" OnClick="Adicionar_Click" />
                    &nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Adicionar Producto al Plan Operativo</asp:LinkButton>
                </td>
            </tr>
        </table>
        <table width='100%' style="text-align: center;" border='0' cellpadding='0' cellspacing='0'>
            <tr>
                <td align='left' valign='top' width='98%'>
                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                    </table>
                    <table cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td valign="top">
                                <div style="width: 350px; overflow: scroll; overflow-x: auto !important; border-right: silver 1px solid; height: auto !important;">
                                    <asp:GridView ID="gv_productos" runat="server" Width="400px" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnPageIndexChanging="gv_productos_PageIndexChanging"
                                        OnRowCommand="gv_productos_RowCommand" OnRowDataBound="gv_productos_RowDataBound">
                                        <PagerStyle CssClass="Paginador" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkborrar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("id_produccion")+ ";" + Eval("NomProducto") %>'
                                                        CommandName="eliminar" CausesValidation="false">
                                                        <asp:Label runat="server" ID="lblactividaPOI" Visible="False" Text='<%# Eval("id_produccion") %>' />
                                                        <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                                              commandName="borrar" CommandArgument='<%# Eval("id_produccion")  %>' ></asp:Image>
                                                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Producto o Servicio">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_produccion")+ ";" + Eval("NomProducto")  %>'
                                                        CommandName="mostrar" Text="Mostrar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="" Visible="false" DataField="id_produccion" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="ProduccionID" />
                                            <%--<asp:BoundField HeaderText="" Visible="false" DataField="NomProducto" />--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbl_nombreProducto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_produccion") %>'
                                                        CommandName="editar" Text='<%# Eval("NomProducto") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnk_insumo" Text="Insumos" runat="server" CausesValidation="False"
                                                        CommandArgument='<%# Eval("id_produccion")+ ";" + Eval("NomProducto")+ ";" + Eval("CodProyecto")  %>'
                                                        CommandName="insumos" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="Paginador" />
                                    </asp:GridView>
                                    <br />
                                    <%--Cargos en Aprobación "PageIndexChanging" y "RowCommand"--%>
                                    <asp:Panel ID="pnl_Actividades" runat="server">
                                        <asp:GridView ID="GrvActividadesNoAprobadas" runat="server" Width="400px" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="4" AllowPaging="false" ShowHeaderWhenEmpty="true" OnRowCommand="GrvActividadesNoAprobadas_RowCommand">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Productos en Aprobación" HeaderStyle-Width="160px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkmostrar_aprobacion" runat="server" CausesValidation="False"
                                                            CommandArgument='<%# Eval("id_produccion") %>' CommandName="mostrar1" Text="Mostrar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbl_nombreProductoAprobado" runat="server" CausesValidation="False"
                                                            CommandArgument='<%# Eval("id_produccion") %>' CommandName="editar" Text='<%# Eval("NomProducto") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="" Visible="false" DataField="id_produccion" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="ChequeoCoordinador" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="Tarea" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="ChequeoGerente" />
                                            </Columns>
                                            <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                                            <RowStyle BackColor="White" ForeColor="#330099" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                            <SortedAscendingCellStyle BackColor="#FEFCEB" />
                                            <SortedAscendingHeaderStyle BackColor="#AF0101" />
                                            <SortedDescendingCellStyle BackColor="#F6F0C0" />
                                            <SortedDescendingHeaderStyle BackColor="#7E0000" />
                                            <PagerStyle CssClass="Paginador" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="width: 500px; overflow: auto; height: auto !important;">
                                    <asp:Table ID="t_anexos" runat="server" Width="100%" BorderWidth="0" CellPadding="4"
                                        CellSpacing="1" CssClass="Grilla">
                                    </asp:Table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
