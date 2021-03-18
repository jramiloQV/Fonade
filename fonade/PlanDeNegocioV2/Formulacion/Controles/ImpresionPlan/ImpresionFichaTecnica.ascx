<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionFichaTecnica.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionFichaTecnica" %>
<asp:Panel ID="pnltabFichaTec" runat="server">
    <div class="ImpresionSeccion">
        <label>2 - Ficha técnica</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>8. Elabore la ficha técnica para cada uno de los productos(Bienes o servicios) que componen su portafolio :</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gvProductos" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Nombre especifico">
                    <ItemTemplate>
                        <label><%# Eval("NomProducto") %></label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Nombre comercial" DataField="NombreComercial" HtmlEncode="false" />
                <asp:BoundField HeaderText="Unidad de medida" DataField="UnidadMedida" HtmlEncode="false" />
                <asp:BoundField HeaderText="Descripción" DataField="DescripcionGeneral" HtmlEncode="false" />
                <asp:BoundField HeaderText="Condiciones especiales" DataField="CondicionesEspeciales" HtmlEncode="false" />
                <asp:BoundField HeaderText="Composición" DataField="Composicion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Otros" DataField="Otros" HtmlEncode="false" />
            </Columns>
        </asp:GridView>
        <br />
    </div>
</asp:Panel>
