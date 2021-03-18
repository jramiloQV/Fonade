<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Entidades.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Entidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Text="Entidades de interventoria"></asp:Label>
    <br />        
    <br />
    <asp:LinkButton ID="lnkSeleccionarPago" Visible="false" Text="Crear entidad de interventoria" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/NewEntidad.aspx" runat="server"></asp:LinkButton>
    <br />
    &nbsp;          
    <asp:TextBox ID="txtCodigo" runat="server" Width="200px"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Listado de entidades"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="true" AutoGenerateColumns="False" 
        DataSourceID="data" EmptyDataText="No se encontro información." 
        Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" 
        HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvMain_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre corto" DataField="NombreCorto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Persona a cargo" DataField="PersonaACargo" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id") %>'
                        CommandName="Ver"  CausesValidation="False" Text='Editar' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Gestionar contratos">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkContrato" CommandArgument='<%# Eval("Id") %>'
                        CommandName="Contrato"  CausesValidation="False" Text='Gestionar contratos' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Operador" DataField="Operador" HtmlEncode="false" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Entidades"
                    SelectMethod="Get"
                    EnablePaging="true"
                    SelectCountMethod="Count"
                    MaximumRowsParameterName="maxRows"
                    StartRowIndexParameterName="startIndex">
                <SelectParameters>
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigo" PropertyName="Text" Type="String" DefaultValue=""  ConvertEmptyStringToNull="false"/>                                        
                </SelectParameters>
        </asp:ObjectDataSource>      
</asp:Content>