<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionCostosAdmin.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionCostosAdmin" %>
<asp:Panel ID="pnltabCostosAdmin" runat="server">
    <div class="ImpresionSeccion">
        <label>Costos Administrativos</label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Gastos de Puesta en Marcha:</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gw_GastosPuestaMarca" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" DataKeyNames="protegido" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <label><%# Eval("Descripcion") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lblvlrIngresosVentas" runat="server" Text='<%# Eval("Valor")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalGastosPuestaMarca" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalGastosPuestaMarca"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>Gastos Anuales de Administracion:</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gw_GastosAnuales" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" DataKeyNames="protegido" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <label><%# Eval("Descripcion") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lblvlrIngresosVentas" runat="server" Text='<%# Eval("Valor")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalGastosAnuales" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalGastosAnuales"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <br />
    </div>
</asp:Panel>


