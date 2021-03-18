<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="VisorDeContratos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Abogado.VisorContrato.VisorDeContratos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Visor de contratos"></asp:Label>          
    <br />
    <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">           
            <asp:ListItem Text="Código de proyecto" Value="proyecto" Selected="True" />
            <asp:ListItem Text="Número de contrato" Value="pago" />
    </asp:DropDownList>
    
    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Listado de contratos"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="false"   AutoGenerateColumns="False" 
        DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" 
        BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" 
        HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvMain_RowCommand" >
        <Columns>                     
            <asp:BoundField HeaderText="Operador" DataField="Operador" HtmlEncode="false" />  
            <asp:BoundField HeaderText="Código de proyecto" DataField="CodigoProyecto" HtmlEncode="false" />            
            <asp:BoundField HeaderText="Nombre del proyecto" DataField="NombreProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Número de contrato" DataField="NumeroContrato" HtmlEncode="false" />                                                
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("CodigoProyecto") %>' Enabled='<%# (bool)Eval("HasInfoCompleted") %>'
                        CommandName="Editar"  CausesValidation="False" Text='Editar' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Información completa">
                <ItemTemplate>
                    <asp:Image runat="server" ID="imgHasUpdates" Height="15" Width="14" ImageUrl="~/Images/icoActivar.gif" Visible='<%# (bool)Eval("HasInfoCompleted") %>' />
                    <asp:Image runat="server" ID="imgNoHasUpdates" Height="15" Width="14" ImageUrl="~/Images/icoBorrar.gif" Visible='<%# !(bool)Eval("HasInfoCompleted") %>' />
                </ItemTemplate>
            </asp:TemplateField>      
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Abogado.VisorContrato.VisorDeContratos"
                    SelectMethod="Get" >
                <SelectParameters> 
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue="" />
                    <asp:ControlParameter ControlID="cmbTipo" Name="tipo" PropertyName="SelectedValue" Type="String" DefaultValue="" />
                </SelectParameters>  
        </asp:ObjectDataSource> 
</asp:Content>