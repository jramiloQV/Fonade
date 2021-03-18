<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" 
    CodeBehind="ProyectosAsignados.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.ProyectosAsignados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Gestión de proyectos y actas de interventoria"></asp:Label>
    <br />
    <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">           
            <asp:ListItem Text="Código de proyecto" Value="id" Selected="True" />
            <asp:ListItem Text="Nombre proyecto" Value="name" />
    </asp:DropDownList>
    
    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Proyectos"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="false"   AutoGenerateColumns="False" 
        DataSourceID="data" EmptyDataText="No se encontro información." 
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" 
        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvMain_RowCommand"  >
        <Columns>            
            <asp:BoundField HeaderText="Código de proyecto" DataField="Id" HtmlEncode="false" />            
            <asp:BoundField HeaderText="Nombre del proyecto" DataField="Nombre" HtmlEncode="false" />
            <asp:BoundField HeaderText="Interventor" DataField="Interventor" HtmlEncode="false"  />                                                            
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>                    
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id")%>'
                        CommandName="Ver"  CausesValidation="False" Text='Gestionar actas' runat="server" Enabled='<%# (bool)Eval("IsOwner") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.ProyectosAsignados"
                    SelectMethod="Get" >
                <SelectParameters> 
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue="" />
                    <asp:ControlParameter ControlID="cmbTipo" Name="tipo" PropertyName="SelectedValue" Type="String" DefaultValue="" />
                </SelectParameters>  
        </asp:ObjectDataSource>    
</asp:Content>
