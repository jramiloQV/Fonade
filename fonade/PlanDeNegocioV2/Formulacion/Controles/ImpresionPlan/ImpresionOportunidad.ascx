<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionOportunidad.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionOportunidad" %>
<asp:Panel ID="pnltabOportunidad" runat="server">
    <div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
        <label>II. ¿Existe Oportunidad en el Mercado?</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>3. Describa la tendencia de crecimiento del mercado en el que se encuentra su negocio</label>
    </div>
    <div>
        <asp:Label ID="lblTendencia" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>4. Realice un análisis de la competencia, alrededor de los criterios* más relevantes para su negocio:</label>
        <br />
        <br />
    </div>
    <asp:GridView ID="gwCompetidores" runat="server" CssClass="Grilla" RowStyle-Height="35px" CellSpacing="1" CellPadding="4" Width="100%" AutoGenerateColumns="False"
        ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
        <Columns>
            <asp:TemplateField HeaderText="Nombre">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label><%# Eval("Nombre") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Localización">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label><%# Eval("Localizacion") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Productos y Servicios<br />(Atributos)">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label><%# Eval("ProductosServicios") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Precios">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label>
                            <%# Eval("Precios") %></label>

                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Logistica de Distribución">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label><%# Eval("LogisticaDistribucion") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Otro, ¿Cuál?">
                <ItemTemplate>
                    <div style="word-wrap: break-word; width:100px"">
                        <label><%# Eval("OtroCual") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <RowStyle Height="35px"></RowStyle>
    </asp:GridView>
    <br />
</asp:Panel>
