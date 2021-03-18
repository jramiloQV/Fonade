<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Convocatorias.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.Convocatorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
    <h1> <asp:Label Text="Convocatorias" runat="server" ID="lbl_Titulo" /> </h1>
    <br />

    <asp:ImageButton ID="imgAgregarProyecto" ImageUrl="../../../Images/icoAdicionarUsuario.gif" PostBackUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx" runat="server" AlternateText="image" />
    
    <asp:HyperLink ID="btnAgregarProyecto" NavigateUrl="~/FONADE/PlandeNegocio/CrearPlanDeNegocio.aspx" runat="server" Text="Adicionar Plan de Negocio" />
    <br />
    <br />

    <asp:GridView ID="gvConvocatorias"  CssClass="Grilla" runat="server"  AllowSorting="True" PageSize="20"
                    AutoGenerateColumns="false" DataSourceID="dataConvocatorias" AllowPaging="true"
                    EmptyDataText="No hay convocatorias." Width="100%" >
        <Columns>
            <asp:TemplateField HeaderText="Nombre" SortExpression="Nombre" >
                <ItemTemplate>
                    <asp:LinkButton ID="lnkMostrarConvocatoria" CommandArgument='<%# Eval("id") %>'
                        CommandName="verConvocatoria" CausesValidation="False" Text='<%#Eval("nombre") %>'
                        runat="server" Font-Bold="true" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha Inicio" SortExpression="FechaInicio">
                <ItemTemplate>
                    <asp:Label ID="fechaInicio" runat="server" Text='<%# Eval("fechaInicio") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha Fin" SortExpression="FechaFin">
                <ItemTemplate>
                    <asp:Label ID="fechaFin" runat="server" Text='<%# Eval("fechaFin") %>'></asp:Label>
                </ItemTemplate> 
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Publicado" SortExpression="Publicado" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>                    
                    <asp:ImageButton ID="btnPublicado" CommandArgument='<%# Eval("id_convocatoria") %>'  Visible='<%# !((int)Eval("Publicado") == 0) %>' CommandName="VerProyectosConvatoria" runat="server" ImageUrl="~/Images/check.png" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Aspectos Evaluados" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Button ID="btnAspectos" runat="server" Text="Ver" CommandArgument='<%# Eval("id_convocatoria") %>' CommandName="VerEvalConvatoria" CssClass="boton_Link_Grid" />
                </ItemTemplate> 
            </asp:TemplateField>
        </Columns>
   </asp:GridView>

     <asp:ObjectDataSource ID="dataConvocatorias" runat="server" EnablePaging="true" SelectMethod="getConvocatorias"
        SelectCountMethod="getConvocatoriasCount" TypeName="Fonade.FONADE.Convocatoria.Convocatorias" MaximumRowsParameterName="maxRows"
        StartRowIndexParameterName="startIndex" > 
    </asp:ObjectDataSource>

</asp:Content>
