<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ObservacionesDeAcreditacion.aspx.cs" Inherits="Fonade.SoporteHelper.ObservacionesDeAcreditacion.ObservacionesDeAcreditacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div>
        <h2>Observaciones de Acreditación</h2>
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Listado de convocatorias"></asp:Label>
    
        <br />
        <asp:DropDownList ID="cmbConvocatorias" runat="server"  DataSourceID="dataConvocatorias" AutoPostBack="false" DataValueField="Id_Convocatoria" DataTextField="NomConvocatoria">
        </asp:DropDownList>
        <br />
        <asp:Button ID="btnAddConvocatoria" runat="server" OnClick="btnAddConvocatoria_Click" Text="Agregar Convocatoria a observación" />
        <br />              
        
                   
        <br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
        <br />
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Listado de convocatorias en acreditación"></asp:Label>        
            <asp:GridView ID="gvConvocatoriasEnObservacion" runat="server" AllowPaging="false" 
            OnRowCommand="gvConvocatoriasEnObservacion_RowCommand"
            AutoGenerateColumns="False" DataSourceID="dataConvocatoriasEnAcreditacion" EmptyDataText="No hay convocatiras en observación."
            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
                >
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="Eliminar" CommandArgument='<%# Eval("Id_LovObjetoSEAcreditacion") %>'
                        CommandName="EliminarConvocatoria" CausesValidation="False" Text='Eliminar' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Convocatoria" DataField="Descripcion" HtmlEncode="false" />
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>
        <asp:ObjectDataSource 
                ID="dataConvocatorias"
                runat="server"
                TypeName="Fonade.SoporteHelper.ObservacionesDeAcreditacion.ObservacionesDeAcreditacion"
                SelectMethod="GetConvocatorias"
                >
        </asp:ObjectDataSource>
        <asp:ObjectDataSource 
                ID="dataConvocatoriasEnAcreditacion"
                runat="server"
                TypeName="Fonade.SoporteHelper.ObservacionesDeAcreditacion.ObservacionesDeAcreditacion"
                SelectMethod="GetConvocatoriasEnAcreditacion"
                >
       </asp:ObjectDataSource>
</asp:Content>
