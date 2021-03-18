<%@ Page Title="Reintegros de pagos" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Reintegros.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.Reintegros" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Reintegro de pagos"></asp:Label>          
    <br />
    <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">           
            <asp:ListItem Text="Código de proyecto" Value="proyecto" Selected="True" />
            <asp:ListItem Text="Código de pago" Value="pago" />
    </asp:DropDownList>
    
    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Listado de pagos"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="false"   AutoGenerateColumns="False" 
        DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" 
        CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  
        OnRowCommand="gvMain_RowCommand" >
        <Columns>            
            <asp:BoundField HeaderText="Código de pago" DataField="IdPago" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />
            <asp:BoundField HeaderText="Valor del pago" DataField="ValorPagoWithFormat" HtmlEncode="false" />
            <asp:BoundField HeaderText="Valor del ultimo reintegro" DataField="ValorUltimoReintegroWithFormat" HtmlEncode="false" />
            <asp:BoundField HeaderText="Proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("IdPago") %>'
                        CommandName="Ver"  CausesValidation="False" Text='Gestionar reintegros' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Reintegros.Reintegros"
                    SelectMethod="Get" >
                <SelectParameters> 
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue="" />
                    <asp:ControlParameter ControlID="cmbTipo" Name="tipo" PropertyName="SelectedValue" Type="String" DefaultValue="" />
                </SelectParameters>  
        </asp:ObjectDataSource> 
</asp:Content>