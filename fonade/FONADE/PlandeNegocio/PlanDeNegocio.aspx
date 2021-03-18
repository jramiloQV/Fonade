<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="PlanDeNegocio.aspx.cs" Inherits="Fonade.FONADE.PlandeNegocio.PlanDeNegocio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label Text="Planes de negocio" runat="server" ID="lbl_Titulo" />
    </h1>
    <br />
    <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/icoAdicionarUsuario.gif" PostBackUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx?IdVersionProyecto=2" runat="server" AlternateText="image" />
    <asp:HyperLink ID="BtnAgregarProyectoNuevo" NavigateUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx?IdVersionProyecto=2" runat="server" Text="Adicionar Plan de Negocio Estructura Nueva" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

    <asp:ImageButton ID="imgAgregarProyecto" ImageUrl="~/Images/icoAdicionarUsuario.gif" PostBackUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx?IdVersionProyecto=1" runat="server" Visible="false" AlternateText="image" />
    <asp:HyperLink ID="btnAgregarProyecto" NavigateUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx?IdVersionProyecto=1" runat="server" Text="Adicionar Plan de Negocio Actual" Visible="false" />
    <br />
    <br />
    <asp:GridView ID="gvPlanesDeNegocio" runat="server"
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" AllowPaging="true"
        PageSize="100" AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataSourceID="dataPlanesDeNegocio" EmptyDataText="No se encontraron datos." OnRowCommand="gvPlanesDeNegocio_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Código">
                <ItemTemplate>
                    <asp:Label ID="lblCodigoPlan" Text='<%# Eval("Id") %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nombre del plan de negocio">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkmostrar" CommandArgument='<%# Eval("id") %>' IdVersionProyecto='<%# Eval("idVersionProyecto") %>'
                        CommandName="verPlanDeNegocio" CausesValidation="False" Text='<%#Eval("nombre") %>'
                        runat="server" Font-Bold="true" ForeColor="Black" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Ciudad">
                <ItemTemplate>
                    <asp:Label ID="lblCiudad" Text='<%# Eval("ciudad") + " (" +Eval("departamento") + ")" %>' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
    <asp:ObjectDataSource ID="dataPlanesDeNegocio" runat="server" EnablePaging="true" SelectMethod="getPlanesDeNegocio"
        SelectCountMethod="getPlanesDeNegocioCount" TypeName="Fonade.FONADE.PlandeNegocio.PlanDeNegocio" MaximumRowsParameterName="maxRows"
        StartRowIndexParameterName="startIndex"></asp:ObjectDataSource>

</asp:Content>
