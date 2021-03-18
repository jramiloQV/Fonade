<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionRequerimientos.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionRequerimientos" %>
<asp:Panel ID="pnltabRequerimiento" runat="server">
    <div class="ImpresionSeccion">
        <label>4 - Requerimientos</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>14. Defina los requerimientos en: Infraestructura - adecuaciones, maquinaria y equipos, muebles y enseres, y demás activos</label>
        <br />
    </div>
    <div class="divgen ImpresionSubSeccion">
        <div class="divleft">
            <br />
            <label class="tamlabel1">14.1. ¿Para el funcionamiento del negocio, es necesario un lugar físico de operación? (SI / NO, justificación)</label>
            &nbsp;<asp:Label ID="lblSel141" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
        </div>
    </div>
    <div>
        <asp:Label ID="lblPregunta141" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>14.2. Identifique los requerimientos de inversión</label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>1. Infraestructura - Adecuaciones</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1421" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1421" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1421"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>2. Maquinaria y Equipo</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1422" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1422" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1422"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>3. Equipo de Comunicación y Computación</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1423" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1423" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1423"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>

        </asp:GridView>
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>4. Muebles y Enseres y Otros</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1424" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1424" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1424"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>5. Otros (incluído herrramientas)</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1425" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1425" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1425"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>6. Gastos Preoperativos</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwPregunta1426" runat="server" Width="100%" AutoGenerateColumns="False" ShowFooter="true"
            CssClass="Grilla" ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <RowStyle HorizontalAlign="Left" />
            <Columns>
                <asp:TemplateField HeaderText="Descripción">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("NomInfraestructura") %></label>
                        </div>
                    </ItemTemplate>
                     <FooterTemplate>
                        <div style="text-align: left;">
                            <label><b>Total:</b></label>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Requisitos Técnicos">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("RequisitosTecnicos") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fuente de Financiación">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("FuenteFinanciacion") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <div style="word-wrap: break-word">
                            <label><%# Eval("Cantidad") %></label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor Unitario" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                    <ItemTemplate>
                        <label><%# Eval("ValorUnidadCadena")%></label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <asp:Label ID="lblTotal1426" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalG1426"))%>' />
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
</asp:Panel>
