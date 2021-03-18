<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="PresupuestoConvocatorias.aspx.cs" Inherits="Fonade.SoporteHelper.PresupuestoConvocatorias.PresupuestoConvocatorias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar usuarios por codigo de proyecto"></asp:Label>          
    <br />
    <asp:DropDownList runat="server" ID="cmbConvocatorias" DataSourceID="dataConvocatorias" DataValueField="Id_Convocatoria" DataTextField="NomConvocatoria" AutoPostBack="true" ></asp:DropDownList>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Usuarios por proyecto"></asp:Label>
    <br />     
    <br />     
    <asp:GridView ID="gvUsuarios" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="dataPresupuesto" EmptyDataText="No hay usuarios." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  >
        <Columns>            
            <asp:BoundField HeaderText="Convocatoria" DataField="Convocatoria" HtmlEncode="false" />                                    
            <asp:BoundField HeaderText="Presupuesto" DataField="PresupuestoDinero" HtmlEncode="false" />  
            <asp:BoundField HeaderText="Tope %" DataField="Tope" HtmlEncode="false" />  
            <asp:BoundField HeaderText="Presupuesto total en salarios" DataField="PresupuestoTotalConvocatoriaSalarios" HtmlEncode="false" />                                    
            <asp:BoundField HeaderText="Presupuesto ejecutado" DataField="PresupuestoEjecutadoConvocatoriaSalarios" HtmlEncode="false" />                                    
            <asp:BoundField HeaderText="Presupuesto disponible" DataField="PresupuestoDisponibleConvocatoriaSalarios" HtmlEncode="false" />                                    
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    
        <asp:ObjectDataSource
            ID="dataConvocatorias"
            runat="server"
            TypeName="Fonade.SoporteHelper.PresupuestoConvocatorias.PresupuestoConvocatorias"
            SelectMethod="GetConvocatorias"                                    
            EnablePaging="false">            
        </asp:ObjectDataSource>

        <asp:ObjectDataSource
            ID="dataPresupuesto"
            runat="server"
            TypeName="Fonade.SoporteHelper.PresupuestoConvocatorias.PresupuestoConvocatorias"
            SelectMethod="GetPresupuestos"                                    
            EnablePaging="false">
            <SelectParameters>                
                <asp:ControlParameter Name="codigoConvocatoria" ControlID="cmbConvocatorias" Type="Int32" PropertyName="SelectedValue" DefaultValue="0" />                
            </SelectParameters>
        </asp:ObjectDataSource>
</asp:Content>
