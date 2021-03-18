<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="InactivarEmprendedor.aspx.cs" Inherits="Fonade.SoporteHelper.Usuarios.InactivarEmprendedor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de inactivar este emprendedor ?');
        }
    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar emprendedor por email o numero de identificación"></asp:Label>          
    <br />
    <br />
    <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">           
            <asp:ListItem Text="Identificación" Value="identificacion" Selected="True" />
            <asp:ListItem Text="Email" Value="email" />
    </asp:DropDownList>

    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar Emprendedor" OnClick="Button1_Click"/>
    <br />     
    <br />   
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <br />
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Listado de emprendedores"></asp:Label>
    <br />     
    <br />     
    <asp:GridView ID="gvUsuarios" runat="server" AllowPaging="false" AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No hay usuarios." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvActividad_RowCommand" ShowHeaderWhenEmpty="true">
        <Columns>                   
            <asp:BoundField HeaderText="Identificación" DataField="Identificacion" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombres" DataField="NombreCompleto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Email" DataField="Email" HtmlEncode="false" />
            <asp:BoundField HeaderText="Clave" DataField="Clave" HtmlEncode="false" />            
            <asp:TemplateField HeaderText="Estado del usuario">
                <ItemTemplate>
                    <asp:Label ID="lblActivoUsuario" Text='<%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>' runat="server" ></asp:Label>                                       
                </ItemTemplate>
            </asp:TemplateField>                      
            <asp:BoundField HeaderText="Codigo Proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Estado en proyecto">
                <ItemTemplate>
                    <asp:Label ID="lblActivoProyecto" Text='<%# (bool)Eval("ActivoProyecto") ? "Activo" : "Inactivo" %>' runat="server" ></asp:Label>                                       
                </ItemTemplate>
            </asp:TemplateField>                      
            <asp:TemplateField HeaderText="Inactivar emprendedor">
                <ItemTemplate>
                    <asp:LinkButton ID="EliminarConAvances" CommandArgument='<%# Eval("Id") %>' Enabled ='<%# (bool)Eval("AllowInactivar") %>'
                        CommandName="Eliminar" CausesValidation="False" Text='Inactivar' runat="server" OnClientClick="return alerta();"
                        />
                </ItemTemplate>
            </asp:TemplateField>          
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
       
        <asp:ObjectDataSource
                ID="data"
                runat="server"
                TypeName="Fonade.SoporteHelper.Usuarios.InactivarEmprendedor"
                SelectMethod="Get" >
            <SelectParameters> 
                <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue="" />
                <asp:ControlParameter ControlID="cmbTipo" Name="tipo" PropertyName="SelectedValue" Type="String" DefaultValue="" />
            </SelectParameters>  
        </asp:ObjectDataSource>   
</asp:Content>
