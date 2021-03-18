<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionProyeccion.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionProyeccion" %>
<div class="ImpresionSeccion">
    <label>2 - Proyección</label>
</div>
<div class="ImpresionSubSeccion">
    <br />
    <label>11. Realice la proyección de cantidades y precios de venta (mensual). Justifique los resultados y señala la forma de pago:</label>
    <br />
    <br />
</div>

<div class="ImpresionSeccion" style="text-align:center">
    <label>Listado de productos</label>
    <br />
    <br />
</div>

<div>
    <asp:GridView ID="gvProductos" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla"
        ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
        <Columns>
            <asp:BoundField HeaderText="Nombre de Producto" DataField="NomProducto" HtmlEncode="false"/>
            <asp:BoundField HeaderText="Unidad de medida" DataField="UnidadMedida" HtmlEncode="false" />
            <asp:BoundField HeaderText="Forma de pago" DataField="FormaDePago" HtmlEncode="false" />
            <asp:BoundField HeaderText="Justificacion" DataField="Justificacion" HtmlEncode="false" />
            <asp:BoundField HeaderText="Iva" DataField="Iva" HtmlEncode="false" ItemStyle-HorizontalAlign="Right" />
        </Columns>
    </asp:GridView>
    <br />
    <br />
</div>
<div class="ImpresionSeccion">
    <label>Ingresos por ventas</label>
    <br />
    <br />
</div>
<div>
    <asp:GridView ID="gvIngresosPorVentas" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla"
        ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
        <Columns>
            <asp:TemplateField HeaderText="Periodo">
                <ItemTemplate>
                    <asp:Label ID="lblPeriodo" runat="server" Text='<%# Eval("Periodo") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 1" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear1" runat="server" Text='<%# Eval("Year1Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 2" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear2" runat="server" Text='<%# Eval("Year2Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 3" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear3" runat="server" Text='<%# Eval("Year3Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 4" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear4" runat="server" Text='<%# Eval("Year4Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 5" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear5" runat="server" Text='<%# Eval("Year5Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 6" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear6" runat="server" Text='<%# Eval("Year6Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 7" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear7" runat="server" Text='<%# Eval("Year7Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 8" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear8" runat="server" Text='<%# Eval("Year8Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 9" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear9" runat="server" Text='<%# Eval("Year9Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 10" Visible="true" ItemStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblYear10" runat="server" Text='<%# Eval("Year10Formatted") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
</div>
