<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionEstrategia.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionEstrategia" %>
<asp:Panel ID="pnltabEstrategia" runat="server">
    <div class="ImpresionSeccion">
        <label>Estrategias</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>18. ¿Qué estrategias utilizará para lograr la meta de ventas y cuál es su presupuesto?</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1">Estrategia de promoción (nombre):</label>
            &nbsp;<asp:Label ID="lblPromocion" runat="server"></asp:Label>
            <br />
            <label class="tamlabel1">Propósito:</label>
            &nbsp;<asp:Label ID="lblProposito" runat="server"></asp:Label>
            <br />
            <br />
        </div>
    </div>
    <div>
        <asp:GridView ID="gwPromocion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%" ShowFooter="true"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Actividad" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Actividad") %> </label>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Costo Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recurso Requerido" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("RecursosRequeridos") %>  </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mes de Ejecución" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word; width: 180px;"><%# Eval("MesEjecucion") %></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Costo" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <%# decimal.Parse(Eval("Costo").ToString()).ToString("$ 0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="LabelCosto" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalCosto"))%>' />
                        </div>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<div style='word-wrap: break-word;'>Responsable<br/>(nombre del cargo lider del proceso)</div>" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("Responsable") %></div>
                    </ItemTemplate>
                    
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1">Estrategia de Comunicación (nombre):</label>
            &nbsp;<asp:Label ID="lblComunicacion" runat="server"></asp:Label>
            <br />
            <label class="tamlabel1">Propósito:</label>
            &nbsp;<asp:Label ID="lblPropositoCom" runat="server"></asp:Label>
            <br />
            <br />
        </div>
    </div>
    <div>
        <asp:GridView ID="gwComunicacion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%" ShowFooter="true"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Actividad" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Actividad") %> </label>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Costo Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recurso Requerido" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("RecursosRequeridos") %>  </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mes de Ejecución" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("MesEjecucion") %></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Costo" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <%# decimal.Parse(Eval("Costo").ToString()).ToString("$ 0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="LabelCostoCom" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalCostoCom"))%>' />
                        </div>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<div style='word-wrap: break-word;'>Responsable<br/>(nombre del cargo lider del proceso)</div>" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("Responsable") %></div>
                    </ItemTemplate>
                    
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1">Estrategia de Distribución (nombre):</label>
            &nbsp;<asp:Label ID="lblDistribucion" runat="server"></asp:Label>
            <br />
            <label class="tamlabel1">Propósito:</label>
            &nbsp;<asp:Label ID="lblPropositoDis" runat="server"></asp:Label>
            <br />
            <br />
        </div>
    </div>
    <div>
        <asp:GridView ID="gwDistribucion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%" ShowFooter="true"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Actividad" ItemStyle-Width="20%" >
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Actividad") %> </label>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <label><b>Costo Total:</b></label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Recurso Requerido" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("RecursosRequeridos") %>  </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Mes de Ejecución" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("MesEjecucion") %></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Costo" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <%# decimal.Parse(Eval("Costo").ToString()).ToString("$ 0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="LabelCostoDis" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalCostoDis"))%>' />
                        </div>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<div style='word-wrap: break-word;'>Responsable<br/>(nombre del cargo lider del proceso)</div>" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("Responsable") %></div>
                    </ItemTemplate>
                    
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
</asp:Panel>
