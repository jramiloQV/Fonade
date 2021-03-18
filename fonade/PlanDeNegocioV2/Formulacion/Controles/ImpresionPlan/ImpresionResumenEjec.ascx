<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionResumenEjec.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionResumenEjec" %>
<asp:Panel ID="pnltabResumen" runat="server">
    <div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
        <label>VII. Resumen Ejecutivo</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Emprendedor(es):</label>
        <br />
        <br />
    </div>
    <div>
        <asp:GridView ID="gwEmprendedores" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%"
            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
            <Columns>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <label><%# Eval("Nombre") %></label>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("Email") %>  </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rol">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;"><%# Eval("Rol") %></div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Concepto del Negocio:</label>
        <br />
    </div>
    <div>
        <br />
        <asp:Label ID="lblConcepto" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Metas</label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Indicador Empleos:</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1"> Meta para el primer año:</label>
            &nbsp;<asp:Label ID="lblEmpleo" runat="server"></asp:Label>
            <br />
        </div>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Indicador Ventas:</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1"> Meta para el primer año:</label>
            &nbsp;<asp:Label ID="lblVentas" runat="server"></asp:Label>
            <br />
        </div>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Indicador Mercadeo (eventos):</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1"> Meta para el primer año:</label>
            &nbsp;<asp:Label ID="lblMercadeo" runat="server"></asp:Label>
            <br />
        </div>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Indicador Contrapartida SENA:</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1"> Meta para el primer año:</label>
            &nbsp;<asp:Label ID="lblSena" runat="server"></asp:Label>
            <br />
        </div>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Indicador Empleos Indirectos:</label>
        <br />
    </div>
    <div class="divgen">
        <div class="divleft">
            <br />
            <label class="tamlabel1"> Meta para el primer año:</label>
            &nbsp;<asp:Label ID="lblIndirectos" runat="server"></asp:Label>
            <br />
        </div>
    </div>
    <br />
</asp:Panel>
