<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionProtagonista.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionProtagonista" %>
<asp:Panel ID="pnltabProtagonista" runat="server">
    <div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
        <label>I. ¿Quién es el Protagonista?</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>1. Describa el perfil de su cliente, junto a su localización. Justifique las razones de su elección:</label>
        <br />
        <br />
    </div>
    <asp:GridView ID="gwClientes" runat="server" CssClass="Grilla" RowStyle-Height="35px" CellSpacing="1" CellPadding="4" Width="100%" AutoGenerateColumns="False"
        ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
        <Columns>
            <asp:TemplateField HeaderText="Cliente">
                <ItemTemplate>
                    <div style="word-wrap: break-word;">
                        <label><%# Eval("Nombre") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Perfil">
                <ItemTemplate>
                    <div style="word-wrap: break-word;">
                        <label><%# Eval("Perfil") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Localización">
                <ItemTemplate>
                    <div style="word-wrap: break-word;">
                        <label><%# Eval("Localizacion") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Justificación">

                <ItemTemplate>
                    <div style="word-wrap: break-word;">
                        <label><%# Eval("Justificacion") %></label>
                    </div>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
        <RowStyle Height="35px"></RowStyle>
    </asp:GridView>
    <br />
    <div class="divgen ImpresionSubSeccion">
        <div class="divleft">
            <label class="tamlabel1">¿Su proyecto tiene perfiles diferentes de clientes y consumidores?</label>
            &nbsp;<asp:Label ID="lblPerDiferent" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
            <br />
        </div>
    </div>
    <div class="ImpresionSubSeccion">
        <label id="lbPerConsumidor" runat="server">Perfil Consumidor:</label>
    </div>
    <div>
        <asp:Label ID="lblPerConsumidor" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>2. ¿Cuáles son las necesidades que usted espera satisfacer de sus potenciales clientes / consumidores?</label>
        <br />
        <br />
        <label>Cliente:</label>
    </div>
    <div>
        <asp:Label ID="lblNecCliente" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label id="lbNecConsumidor" runat="server">Consumidores:</label>
    </div>
    <div>
        <asp:Label ID="lblNecConsumidor" runat="server"></asp:Label>
        <br />
    </div>
</asp:Panel>
