<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Sucursal.aspx.cs" Inherits="Fonade.SoporteHelper.Sucursal.Sucursal" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de borrar esta sucursal ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div>
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Listado de bancos"></asp:Label>
    
        <br />
        <asp:DropDownList ID="cmbBancos" runat="server"  DataSourceID="dataBancos" AutoPostBack="true" DataValueField="Id_Banco" DataTextField="NomBanco">
        </asp:DropDownList>
        <br />
        <asp:Label ID="Label3" runat="server" Text="Nombre sucursal"></asp:Label>
        <asp:TextBox ID="txtSucursal" runat="server"></asp:TextBox>
        <br />
        <asp:Label ID="Label4" runat="server" Text="Codigo sucursal"></asp:Label>
        <asp:TextBox ID="txtCodigoSucursal" runat="server"></asp:TextBox>        
        <br /> 
        <asp:Button ID="btnAdd" runat="server"  Text="Agregar Sucursal al banco" OnClick="btnAdd_Click" />
        <br />              
        
                   
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Listado de sucursales"></asp:Label>        
            <asp:GridView ID="gvSucursales" runat="server" AllowPaging="false" 
            OnRowCommand="gvSucursales_RowCommand"
            AutoGenerateColumns="False" DataSourceID="dataSucursales" EmptyDataText="No hay sucursales."
            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
                >
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="Eliminar" CommandArgument='<%# Eval("Id") %>'
                        CommandName="EliminarSucursal" CausesValidation="False" Text='Eliminar' runat="server" OnClientClick="return alerta();"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Sucursal" DataField="NomPagoSucursal" HtmlEncode="false" />
            <asp:BoundField HeaderText="Codigo" DataField="Id_PagoSucursal" HtmlEncode="false" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>
        <asp:ObjectDataSource 
                ID="dataBancos"
                runat="server"
                TypeName="Fonade.SoporteHelper.Sucursal.Sucursal"
                SelectMethod="GetBancos"
                >
        </asp:ObjectDataSource>
        <asp:ObjectDataSource 
                ID="dataSucursales"
                runat="server"
                TypeName="Fonade.SoporteHelper.Sucursal.Sucursal"
                SelectMethod="GetSucursales"
                >
            <SelectParameters> 
                <asp:ControlParameter ControlID="cmbBancos" Name="idBanco" PropertyName="SelectedValue" Type="Int32" DefaultValue="" />
            </SelectParameters>  
       </asp:ObjectDataSource>
</asp:Content>

