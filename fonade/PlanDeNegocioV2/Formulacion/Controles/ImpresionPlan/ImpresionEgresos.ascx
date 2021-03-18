<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionEgresos.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionEgresos" %>
<asp:Panel ID="pnltabIngresos" runat="server">
    <div class="ImpresionSeccion">
        <label>Egresos</label>
        <br />
    </div>
    <div class="divgen ImpresionSubSeccion">
        <div class="divleft">
            <br />
            <label class="tamlabel1">Índice de Actualización monetaria</label>
            &nbsp;<asp:Label ID="lblActualizacionMonetaria" runat="server" Width="30px"></asp:Label>
            <br />
        </div>
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <br />
        <label>Inversiones Fijas y Diferidas</label>

    </div>
    <div>
        <asp:GridView ID="gw_InversionesFijas" runat="server" Width="100%" AutoGenerateColumns="False" HeaderStyle-HorizontalAlign="Center"
            CssClass="Grilla" ShowFooter="true" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Concepto" HeaderStyle-Width="42%" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <label><%# Eval("concepto") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="20%">
                    <ItemTemplate>
                        <div style="text-align: right;">
                            <label><%# string.Format("{0:$ #.00}", Convert.ToDecimal(Eval("valor"))) %></label>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalInvFijas" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalInvFijas"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="mes" HeaderText="Mes" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Fuente de financiación" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30%">
                    <ItemTemplate>
                        <label><%# Eval("tipoFuente")%></label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Costos de Puesta en Marcha</label>
    </div>
    <div>
        <asp:GridView ID="gw_CostosPuestaMarcha" runat="server" AutoGenerateColumns="true"
            CssClass="Grilla" Width="100%" CellSpacing="1" CellPadding="4" OnRowDataBound="gw_CostosPuestaMarcha_RowDataBound"
            HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' />
        <br />
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Costos Anualizados Administrativos</label>
    </div>
    <div>
        <asp:GridView ID="gw_CostosAnualizados" runat="server" AutoGenerateColumns="True"
            CssClass="Grilla" Width="100%" CellSpacing="1" CellPadding="4" OnRowDataBound="gw_CostosAnualizados_RowDataBound"
            HeaderStyle-HorizontalAlign="Center" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' />
        <br />
    </div>
    <div class="ImpresionSeccion" style="text-align: center">
        <label>Gastos de Personal</label>
    </div>
    <div>
        <asp:GridView ID="gw_GastosPersonales" runat="server" AutoGenerateColumns="True" HeaderStyle-HorizontalAlign="Center" OnRowDataBound="gw_GastosPersonales_RowDataBound"
            CssClass="Grilla" Width="100%" CellSpacing="1" CellPadding="4" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' />
        <br />
    </div>
</asp:Panel>
