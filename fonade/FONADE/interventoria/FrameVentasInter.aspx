<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameVentasInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.FrameVentasInter" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ventas</title>
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
        .thisIsMyClass div
        {
            height: auto !important;
        }
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
<body style="overflow-x: hidden; height: 580px;">
    <form id="form1" runat="server">
    <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <div class="help_container">
                        <div>
                            <a onclick="textoAyuda({titulo: 'Ventas', texto: 'Ventas'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" />
                                Ventas: </a>
                        </div>
                    </div>
                </td>
                <td align="left" colspan="3">
                </td>
                <td align="right">
                    &nbsp;</td>
                <td align="left" colspan="3">
                    <%--Campo para el botón--%>
                    <asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click">Imprimir</asp:LinkButton>
                </td>
                <td align="left" colspan="3" style="height: 26px; width: 100px">
                    <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="Ventas" 
                        _txtTab="1" _mostrarPost="true"/>
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
                        Style="cursor: pointer;" OnClick="LinkButton1_Click" />
                    &nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server"  OnClick="LinkButton1_Click">Adicionar Producto al Plan Operativo</asp:LinkButton>
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
                            <td valign="top" rowspan="2">
                                <div style="width: 418px; overflow: scroll; overflow-x: hidden; overflow-y: hidden;
                                    border-right: silver 1px solid; height: 90%;" class="thisIsMyClass">
                                    <asp:GridView ID="gv_productos" runat="server" Width="400px" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnPageIndexChanging="gv_productos_PageIndexChanging"
                                        OnRowCommand="gv_productos_RowCommand" OnRowDataBound="gv_productos_RowDataBound" >
                                        <PagerStyle CssClass="Paginador" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkborrar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("id_ventas")+ ";" + Eval("NomProducto") %>'
                                                        CommandName="eliminar" CausesValidation="false">
                                                            <asp:Label runat="server" ID="lblactividaPOI" Visible="False" Text='<%# Eval("id_ventas") %>' />
                                                            <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                                                    venta='<%# Eval("id_ventas")%>' producto='<%= NomProducto %>' ></asp:Image>
                                                        </asp:LinkButton ></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Producto o Servicio">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_ventas")+ ";" + Eval("NomProducto")  %>'
                                                        CommandName="mostrar" Text="Mostrar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="" Visible="false" DataField="id_ventas" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                            <%--<asp:BoundField HeaderText="" Visible="false" DataField="NomProducto" />--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbl_nombreProducto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_ventas") %>'
                                                        CommandName="editar" Text='<%# Eval("NomProducto") %>' />
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
                                            CellPadding="4" AllowPaging="false" ShowHeaderWhenEmpty="true" OnRowCommand="GrvActividadesNoAprobadas_RowCommand" ><Columns>
                                                <asp:TemplateField HeaderText="Ventas en Aprobación" HeaderStyle-Width="160px">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkmostrar_aprobacion" runat="server" CausesValidation="False"
                                                            CommandArgument='<%# Eval("id_ventas") %>' CommandName="mostrar1" Text="Mostrar" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbl_nombreProductoAprobado" runat="server" CausesValidation="False"
                                                            CommandArgument='<%# Eval("id_ventas") %>' CommandName="editar" Text='<%# Eval("NomProducto") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="" Visible="false" DataField="id_ventas" />
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
                                <div style="width: 500px; overflow: auto">
                                    <%--Tabla de meses--%>
                                    <%--<table id="tabla_meses" runat="server" class="Grilla" cellpadding="0" cellspacing="0"
                                        width="3380px" border="0" visible="false">
                                        <tr>
                                            <th style="width: 260px; text-align: center">
                                                Mes 1
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 2
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 3
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 4
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 5
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 6
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 7
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 8
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 9
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 10
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 11
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 12
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 13
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 14
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Costo Total
                                            </th>
                                        </tr>
                                    </table>--%>
                                    <%--Grilla con datos de ventas e ingresos.--%>
                                    <asp:GridView ID="gv_Detallesproductos" runat="server" Width="3380px" AutoGenerateColumns="False"
                                        CssClass="Grilla" RowStyle-Height="35px" ShowFooter="True" Visible="false" OnRowDataBound="gv_Detallesproductos_RowDataBound"
                                        OnRowCommand="gv_Detallesproductos_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ventas" ItemStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="fondo1" Text='<%# Eval("fondo1") %>' />
                                                    <asp:Label runat="server" ID="fondo1F" Text='<%# Eval("fondo1") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance1" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad1" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="1" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("emprendedor1") %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("fondo2") %>' />
                                                    <asp:Label ID="fondo2F" runat="server" Text='<%# Bind("fondo2") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance2" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad2" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="2" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("emprendedor2") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("fondo3") %>'></asp:Label><asp:Label ID="fondo3F" runat="server" Text='<%# Bind("fondo3") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance3" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad3" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="3" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("emprendedor3") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("fondo4") %>'></asp:Label><asp:Label ID="fondo4F" runat="server" Text='<%# Bind("fondo4") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance4" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad4" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="4" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("emprendedor4") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label8" runat="server" Text='<%# Bind("fondo5") %>'></asp:Label><asp:Label ID="fondo5F" runat="server" Text='<%# Bind("fondo5") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance5" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad5" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="5" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label9" runat="server" Text='<%# Bind("emprendedor5") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label10" runat="server" Text='<%# Bind("fondo6") %>' />
                                                    <asp:Label ID="fondo6F" runat="server" Text='<%# Bind("fondo6") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance6" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad6" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="6" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("emprendedor6") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label11" runat="server" Text='<%# Bind("emprendedor6") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label12" runat="server" Text='<%# Bind("fondo7") %>'></asp:Label><asp:Label ID="fondo7F" runat="server" Text='<%# Bind("fondo7") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance7" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad7" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="7" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label13" runat="server" Text='<%# Bind("emprendedor7") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label14" runat="server" Text='<%# Bind("fondo8") %>'></asp:Label><asp:Label ID="fondo8F" runat="server" Text='<%# Bind("fondo8") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance8" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad8" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="8" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label15" runat="server" Text='<%# Bind("emprendedor8") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label16" runat="server" Text='<%# Bind("fondo9") %>'></asp:Label><asp:Label ID="fondo9F" runat="server" Text='<%# Bind("fondo9") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance9" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad9" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="9" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label17" runat="server" Text='<%# Bind("emprendedor9") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label18" runat="server" Text='<%# Bind("fondo10") %>'></asp:Label><asp:Label ID="fondo10F" runat="server" Text='<%# Bind("fondo10") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance10" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad10" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="10" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label19" runat="server" Text='<%# Bind("emprendedor10") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label20" runat="server" Text='<%# Bind("fondo11") %>'></asp:Label><asp:Label ID="fondo11F" runat="server" Text='<%# Bind("fondo11") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance11" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad11" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="11" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label21" runat="server" Text='<%# Bind("emprendedor11") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label22" runat="server" Text='<%# Bind("fondo12") %>'></asp:Label><asp:Label ID="fondo12F" runat="server" Text='<%# Bind("fondo12") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance12" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad12" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="12" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox23" runat="server" Text='<%# Bind("emprendedor12") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label23" runat="server" Text='<%# Bind("emprendedor12") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <%--Columnas 13 y 14 adicionadas según FONADE Clásico.--%>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label24" runat="server" Text='<%# Bind("fondo13") %>'></asp:Label><asp:Label ID="fondo13F" runat="server" Text='<%# Bind("fondo13") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance13" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad13" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="13" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox24" runat="server" Text='<%# Bind("emprendedor13") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label25" runat="server" Text='<%# Bind("emprendedor13") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label26" runat="server" Text='<%# Bind("fondo14") %>'></asp:Label><asp:Label ID="fondo14F" runat="server" Text='<%# Bind("fondo14") %>' Visible="False" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton ID="imgAvance14" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:LinkButton ID="lnkactividad14" runat="server" CausesValidation="False" CommandName="editar"
                                                        Text='Ver Avance' CommandArgument="14" />
                                                </FooterTemplate>
                                                <ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox25" runat="server" Text='<%# Bind("emprendedor14") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label27" runat="server" Text='<%# Bind("emprendedor14") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ventas">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox28" runat="server" Text='<%# Bind("fondoTotal") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label28" runat="server" Text='<%# Bind("fondoTotal") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ingreso">
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TextBox29" runat="server" Text='<%# Bind("emprendedorTotal") %>'></asp:TextBox></EditItemTemplate><ItemTemplate>
                                                    <asp:Label ID="Label29" runat="server" Text='<%# Bind("emprendedorTotal") %>'></asp:Label></ItemTemplate><ItemStyle Width="130px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <RowStyle Height="35px" />
                                    </asp:GridView>
                                    <br />
                                    <asp:Table ID="t_anexos" runat="server" Width="100%" BorderWidth="0" CellPadding="4"
                                        CellSpacing="1" CssClass="Grilla">
                                    </asp:Table>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 290px;">
                            <td style="height: 290px;">
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
