<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionCapitalTrabajo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionCapitalTrabajo" %>
<asp:Panel ID="pnltabIngresos" runat="server">
    <div class="ImpresionSeccion">
        <label>Capital de Trabajo</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gw_CapitalTrabajo" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" DataKeyNames="id_Capital" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Componente">
                    <ItemTemplate>
                        <label><%# Eval("componente") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <label><%# Eval("valor")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalCapital" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalCapital"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FuenteFinanciacion" HeaderText="Fuente de financiación" />
                <asp:TemplateField HeaderText="Observación" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <label><%# Eval("observacion")%></label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <br />
    </div>
</asp:Panel>
