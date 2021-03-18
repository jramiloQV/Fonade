<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionProductividad.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionProductividad" %>

<asp:Panel ID="pnltabProductividad" runat="server">
    <div class="ImpresionSeccion">
        <label>6 - Productividad y equipo de trabajo</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>16. ¿Cuál es la capacidad productiva de la empresa? (cantidad de bien o servicio por unidad de tiempo)</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblPregunta16" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>17. Equipo de trabajo</label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>17.1. ¿Cuál es el perfil del emprendedor, el rol que tendría dentro de la empresa y su dedicación?</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gw_pregunta171" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla">
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Nombre Emprendedor">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NombreEmprendedor") %></label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Perfil">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Perfil") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rol">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Rol") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>17.2. ¿Qué cargos requiere la empresa para su operación (primer año)?</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta172A" runat="server" Width="100%" AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>

                <asp:TemplateField HeaderText="Descripción"  ItemStyle-Width="20%">
                    <ItemTemplate>
                        <label><%# Eval("Cargo") %></label>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="ValorRemunCadena" HeaderText="Remuneración Unitario" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%" />
                <asp:BoundField DataField="OtrosGastosCadena" HeaderText="Otros Gastos" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="24%"  />
                <asp:BoundField DataField="ValorPrestacionesCadena" HeaderText="Valor con Prestaciones" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="23%"   />
                <asp:BoundField DataField="UnidadTiempo" HeaderText="Unidad" ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%" />
                <asp:BoundField DataField="TiempoVinculacion" HeaderText="Tiempo Vinculación Primer Año" ItemStyle-HorizontalAlign="center" ItemStyle-Width="5%"  />
            </Columns>

        </asp:GridView>
        <br />
        <br />
        <asp:GridView ID="gwPregunta172B" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción" ItemStyle-Width="20%" >
                    <ItemTemplate>
                        <label><%# Eval("Cargo") %></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total Solicitado:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ValorRemunPrimerAnioCadena" HeaderText="Remuneración Primer Año" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%" />
                <asp:TemplateField HeaderText="Valor Solicitado Fondo" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblvlrFondoEmprender" runat="server" Text='<%# Eval("ValorFondoEmprenderCadena")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalFondoEmprender" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalFondoEmprender"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Aportes Emprendedores" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblvlrAportesEmprendedor" runat="server" Text='<%# Eval("AportesEmprendedorCadena")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalAportesEmprendedor" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalAportesEmprendedor"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ingresos por Ventas" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="20%">
                    <ItemTemplate>
                        <asp:Label ID="lblvlrIngresosVentas" runat="server" Text='<%# Eval("IngresosVentasCadena")%>' />
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotalIngresosVentas" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalIngresosVentas"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>

    </div>
    <br />
</asp:Panel>
