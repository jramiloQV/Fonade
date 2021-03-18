<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionIngresosEf.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionIngresosEf" %>
<asp:Panel ID="pnltabIngresos" runat="server">
    <div class="ImpresionSeccion">
        <label>Ingresos</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Fuentes de Financiación:</label>
        <br />
    </div>
    <div class="divgen ImpresionSubSeccion">
        <div class="divleft">
            <br />
            <label class="tamlabel1">Recursos solicitados al fondo emprender en (smlv)</label>
            &nbsp;<asp:Label ID="lblRecursosSolicitados" runat="server" Font-Size="Medium"></asp:Label>
            <br />
            <br />
        </div>
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Aporte de los Emprendedores</label>
    </div>
    <div>
        <asp:GridView ID="gw_AporteEmprendedores" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" DataKeyNames="Id_Aporte" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <HeaderStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <label><%# Eval("nombre") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <label>
                            <%# Eval("valor")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalVlrAportesEmp" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalVlrAportesEmp"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="detalle" HeaderText="Detalle" ItemStyle-Width="35%" />
            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Recursos de Capital</label>
    </div>
    <div>
        <asp:GridView ID="gw_RecursosCapital" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" DataKeyNames="Id_Recurso" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <HeaderStyle HorizontalAlign="Center" />
            <Columns>
                <asp:TemplateField ItemStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("aux") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cuantía" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <label><%# Eval("cuantia") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalRecCapital" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalRecCapital"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plazo" ItemStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("plazo")%></label>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:BoundField DataField="formaPago" HeaderText="Forma de Pago" ItemStyle-Width="15%" />
                <asp:BoundField DataField="intereses" HeaderText="Interes (Nominal/Anual)" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="destinacion" HeaderText="Destinación" ItemStyle-Width="30%" />

            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Proyeccion de Ingresos por Ventas</label>
    </div>
    <div>
        <asp:GridView ID="gw_IngresosVentas" runat="server" CssClass="Grilla" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'
            OnRowDataBound="gw_IngresosVentas_RowDataBound"><HeaderStyle HorizontalAlign="Center" />
        </asp:GridView>
        <br />
        <br />
    </div>
</asp:Panel>
