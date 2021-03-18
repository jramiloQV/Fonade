<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActasDeValidacionEvaluacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ActasDeValidacionEvaluacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm('¿ Está seguro que desea eliminar esta acta final de validación ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1> <asp:Label Text="Actas de validación final" runat="server" ID="lbl_Titulo" /> </h1>
    <br />
    <asp:ImageButton ID="imgAgregarProyecto" ImageUrl="../../../Images/icoAdicionarUsuario.gif" PostBackUrl="~/FONADE/evaluacion/CrearActa.aspx?a=a" runat="server" AlternateText="image" />
    
    <asp:HyperLink ID="btnAgregarActa" NavigateUrl="~/FONADE/evaluacion/CrearActa.aspx?a=a" runat="server" Text="Adicionar acta de validación" />
    <br />
    <br />
    <asp:GridView ID="gvActasValidacion" runat="server" Width="100%" BorderWidth="0" CellSpacing="1" CellPadding="4" 
                  AllowPaging="true" PageSize="30" AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
                  DataSourceID="dataActaValidacion" EmptyDataText="No se encontraron datos." OnRowCommand="gvActasValidacion_RowCommand">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="imgborrar" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="eliminar" CausesValidation="false" Visible='<%# Eval("SePuedeEliminar") %>' OnClientClick="return alerta();">
                        <asp:Image ID="btnBorrar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;" ToolTip="Eliminar acta" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Numero" SortExpression="NumActa">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkNumeroActa" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id") + ";" + Eval("Publicado") %>' CommandName="actualizar" Text='<%# Eval("Numero") %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="NomActa" />
            <asp:BoundField DataField="Convocatoria" HeaderText="Convocatoria" SortExpression="NomConvocatoria" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>

    <asp:ObjectDataSource ID="dataActaValidacion" runat="server" EnablePaging="true" SelectMethod="getActasValidacion"
                          SelectCountMethod="getActaValidacionCount" TypeName="Fonade.FONADE.evaluacion.ActasDeValidacionEvaluacion"
                          MaximumRowsParameterName="maxRows" StartRowIndexParameterName="startIndex">                
    </asp:ObjectDataSource>
</asp:Content>