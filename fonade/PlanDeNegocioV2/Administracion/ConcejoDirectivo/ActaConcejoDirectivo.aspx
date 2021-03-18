<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActaConcejoDirectivo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ActaConcejoDirectivo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de eliminar esta acta ?');
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <% Page.DataBind(); %>  
    <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Actas de concejo directivo"></asp:Label>
        <br />
        <br />
        <asp:ImageButton ID="imgAdd" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClick="imgAdd_Click" />
        <asp:LinkButton ID="lnkAdd" runat="server" Text=" Adicionar acta de concejo directivo" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'  OnClick="lnkAdd_Click" />
        <br />
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false">Sucedio un error inesperado, intentalo de nuevo.</asp:Label>

        <asp:GridView ID="gvActas" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla" AllowPaging="true" AllowSorting="false" EmptyDataText="No existen actas que mostrar." DataSourceID="data" PageSize="25" OnRowCommand="gvActas_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Eliminar">
                    <ItemTemplate>
                        <asp:LinkButton ID="imgborrar" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="Eliminar" CausesValidation="false" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate") && !(bool)Eval("Publicado") ) %>' OnClientClick="return alerta();">
                            <asp:Image ID="btnBorrar" ImageUrl="../../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;" ToolTip="Eliminar acta" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Número">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id")%>' CommandName="actualizar" Text='<%# Eval("Numero") %>' Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />
                <asp:BoundField HeaderText="Fecha de creación" DataField="Fecha" HtmlEncode="false" />
                <asp:BoundField HeaderText="Convocatoria" DataField="Convocatoria" HtmlEncode="false" />
                <asp:TemplateField HeaderText="Publicado" SortExpression="Publicado" >
                    <ItemTemplate>                    
                        <asp:ImageButton ID="btnPublicado" Visible='<%# (bool)Eval("Publicado") %>' CommandName="VerProyectosConvatoria" runat="server" ImageUrl="~/Images/check.png" Enabled ="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <asp:ObjectDataSource
            ID="data"
            runat="server"
            TypeName="Fonade.PlanDeNegocioV2.Administracion.ConcejoDirectivo.ActaConcejoDirectivo"
            SelectMethod="Get"
            SelectCountMethod="Count"
            MaximumRowsParameterName="maxRows"
            StartRowIndexParameterName="startIndex"
            EnablePaging="true">            
        </asp:ObjectDataSource>
</asp:Content>
