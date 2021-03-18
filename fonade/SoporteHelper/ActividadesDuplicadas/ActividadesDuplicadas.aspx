<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActividadesDuplicadas.aspx.cs" Inherits="Fonade.SoporteHelper.ActividadesDuplicadas.ActividadesDuplicadas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de borrar esta actividad con avances ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar actividades duplicadas de plan operativo por codigo de proyecto"></asp:Label>          
    <br />
    <asp:TextBox ID="txtCodigoProyecto" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscarActividadesDuplicadas" runat="server" Text="Buscar actividades" OnClick="Button1_Click"/>
    <br />     
    <br />   
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <br />
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Actividades duplicadas por proyecto"></asp:Label>
    <br />     
    <br />     
    <asp:GridView ID="gvActividadesDuplicadas" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="dataActividadesDuplicadas" EmptyDataText="No hay actividades duplicadas." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvActividadesDuplicadas_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="Eliminar" CommandArgument='<%# Eval("Id") %>' Enabled ='<%# Eval("AllowDelete") %>'
                        CommandName="Eliminar" CausesValidation="False" Text='Eliminar' runat="server"                                                
                        />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Id actividad" DataField="Id" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre actividad" DataField="Nombre" HtmlEncode="false" />
            <asp:BoundField HeaderText="Item" DataField="Item" HtmlEncode="false" />
            <asp:BoundField HeaderText="Metas" DataField="Metas" HtmlEncode="false" />
            <asp:BoundField HeaderText="Numero de avances" DataField="NumeroAvances" HtmlEncode="false" />
            <asp:BoundField HeaderText="Codigo proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Forzar eliminación con avances">
                <ItemTemplate>
                    <asp:LinkButton ID="EliminarConAvances" CommandArgument='<%# Eval("Id") %>' Enabled ='<%# !(bool)Eval("AllowDelete") %>'
                        CommandName="Eliminar" CausesValidation="False" Text='Eliminar con avances' runat="server" OnClientClick="return alerta();"
                        />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
       
        <asp:ObjectDataSource
                ID="dataActividadesDuplicadas"
                runat="server"
                TypeName="Fonade.SoporteHelper.ActividadesDuplicadas.ActividadesDuplicadas"
                SelectMethod="GetActividadesDuplicadas"
                >
            <SelectParameters> 
                <asp:ControlParameter ControlID="txtCodigoProyecto" Name="codigoProyecto" PropertyName="Text" Type="Int32" DefaultValue="" />
            </SelectParameters>  
        </asp:ObjectDataSource>     
</asp:Content>
