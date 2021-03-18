<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionIngresos.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionIngresos" %>
<asp:Panel ID="pnltabIngresos" runat="server">
    <div class="ImpresionSeccion">
        <label>1 - Ingreso y condiciones comerciales</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>9. ¿Cómo obtendrá ingresos? Describa la estrategia de generación de ingresos para su proyecto</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblPregunta9" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>10. Describa las condiciones comerciales que aplican para el portafolio de sus productos:</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gw_pregunta10" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Cliente">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("Cliente") %></label> 
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Volúmenes y Frecuencia">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("FrecuenciaCompra") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Características Compra">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("CaracteristicasCompra") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sitio de Compra">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("SitioCompra") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Forma de Pago">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("FormaPago") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Precio" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:150px">
                            <label><%# Eval("PrecioCadena") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Post-Venta">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("RequisitosPostVenta") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Garantías">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("Garantias") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Margen de Comercialización">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width:100px">
                            <label><%# Eval("Margen") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />

    <asp:Panel ID="pnlPtasConsumidor" runat="server" Visible="false">

        <div class="ImpresionSubSeccion">
            <label>Consumidor</label>
            <br />
            <br />
        </div>
        <div class="ImpresionSubSeccion">
            <label>¿Dónde Compra?</label>
            <br />
        </div>
        <div>
            <asp:Label ID="lblPtaConsumidor1" runat="server"></asp:Label>
            <br />
        </div>

        <div class="ImpresionSubSeccion">
            <label>¿Qué características se exigen para la compra (Ej: calidades, presentación - empaque)?</label>
            <br />
        </div>
        <div>
            <asp:Label ID="lblPtaConsumidor2" runat="server"></asp:Label>
            <br />
        </div>

        <div class="ImpresionSubSeccion">
            <label>¿Cuál es la frecuencia de compra?</label>
            <br />
        </div>
        <div>
            <asp:Label ID="lblPtaConsumidor3" runat="server"></asp:Label>
            <br />
        </div>

        <div class="ImpresionSubSeccion">
            <label>Precio</label>
            <br />
        </div>
        <div>
            <asp:Label ID="lblPtaConsumidor4" runat="server"></asp:Label>
            <br />
        </div>

    </asp:Panel>
</asp:Panel>
