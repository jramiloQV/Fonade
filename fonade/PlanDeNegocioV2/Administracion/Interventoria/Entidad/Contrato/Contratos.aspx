<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Contratos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.Contratos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="lblMainTitle" runat="server" Font-Bold="True" Text="Contratos de entidad"></asp:Label>            
    <br />
     <br />
    <asp:LinkButton ID="lnkSeleccionarPago" Visible="true" Text="Crear nuevo contrato" OnClick="lnkSeleccionarPago_Click" runat="server"></asp:LinkButton>
    <br /> 
    <asp:TextBox ID="txtCodigo" runat="server" Width="200px"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>    
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="true" AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvMain_RowCommand" >
        <Columns>
            <asp:BoundField HeaderText="Numero de contrato" DataField="NumeroContrato" HtmlEncode="false" />                              
            <asp:TemplateField HeaderText="Acción">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id") + ";" + Eval("IdEntidad") %>'
                        CommandName="Ver"  CausesValidation="False" Text='Editar' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>

        <br />
        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx" ></asp:Button>            

        <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.Contratos"
                    SelectMethod="Get"
                    EnablePaging="true"
                    SelectCountMethod="Count"
                    MaximumRowsParameterName="maxRows"
                    StartRowIndexParameterName="startIndex">
                <SelectParameters> 
                    <asp:QueryStringParameter Name="codigo" Type="String" DefaultValue="0" QueryStringField="codigo" />
                    <asp:ControlParameter ControlID="txtCodigo" Name="numeroContrato" PropertyName="Text" Type="String" DefaultValue=""  ConvertEmptyStringToNull="false"/>
                </SelectParameters>
        </asp:ObjectDataSource>
</asp:Content>