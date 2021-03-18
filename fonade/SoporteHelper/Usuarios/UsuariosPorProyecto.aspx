<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="UsuariosPorProyecto.aspx.cs" Inherits="Fonade.SoporteHelper.Usuarios.UsuariosPorProyecto" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar usuarios por codigo de proyecto"></asp:Label>          
    <br />
    <asp:TextBox ID="txtCodigoProyecto" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar usuarios" OnClick="Button1_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Usuarios por proyecto"></asp:Label>
    <br />     
    <br />     
    <asp:GridView ID="gvUsuarios" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="dataUsuarios" EmptyDataText="No hay usuarios." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  >
        <Columns>            
            <asp:BoundField HeaderText="Codigo Proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre Proyecto" DataField="NombreProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Estado Proyecto" DataField="EstadoProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Codigo Contacto" DataField="CodigoContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombres Contacto" DataField="NombresContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Email Contacto" DataField="EmailContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Clave Contacto" DataField="ClaveContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Estado Contacto" DataField="EstadoContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Grupo Contacto" DataField="GrupoContacto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Rol Contacto" DataField="RolContacto" HtmlEncode="false" />                  
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                ID="dataUsuarios"
                runat="server"
                TypeName="Fonade.SoporteHelper.Usuarios.UsuariosPorProyecto"
                SelectMethod="GetUsuarios">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtCodigoProyecto" Name="codigoProyecto" PropertyName="Text" Type="Int32" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
</asp:Content>

