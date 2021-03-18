<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="HistoriaEvaluacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.HistorialEvaluacion.HistoriaEvaluacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Historial de evaluación por proyecto"></asp:Label>          
    <br />
    <asp:TextBox ID="txtCodigoProyecto" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Historial de evaluación"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvMain_RowCommand" >
        <Columns>            
            <asp:BoundField HeaderText="Codigo Proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Nombre Proyecto" DataField="NombreProyecto" HtmlEncode="false" />
            <asp:BoundField HeaderText="Convocatoria" DataField="NombreConvocatoria" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Ver evaluación">
                <ItemTemplate>
                    <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("CodigoProyecto") + ";" + Eval("CodigoConvocatoria") %>'
                        CommandName="Ver"  CausesValidation="False" Text='Ver evaluación' runat="server"/>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
       
        <asp:ObjectDataSource
                ID="data"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.HistorialEvaluacion.HistoriaEvaluacion"
                SelectMethod="Get">
            <SelectParameters> 
                <asp:ControlParameter ControlID="txtCodigoProyecto" Name="codigoProyecto" PropertyName="Text" Type="Int32" DefaultValue="" />
            </SelectParameters>  
        </asp:ObjectDataSource>
</asp:Content>
