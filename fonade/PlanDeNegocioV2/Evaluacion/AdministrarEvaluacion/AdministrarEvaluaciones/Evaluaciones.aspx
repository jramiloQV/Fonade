<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Evaluaciones.aspx.cs" EnableViewState="true" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluaciones.Evaluaciones" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>       
    
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Mis evaluaciones"></asp:Label>          
    <br />
    <asp:TextBox ID="txtCodigoProyecto" runat="server" AutoPostBack="false"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click"/>
    <br />     
    <br />   
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Planes de negocio"></asp:Label>
    <br />     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="form-group center-block">
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <label class="control-label"><b>Actualizando información </b></label>
                                            <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                        </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
    <asp:GridView ID="gvMain" runat="server" AllowPaging="true" AutoGenerateColumns="False" DataSourceID="data" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvMain_RowCommand" PageSize="20" >
        <Columns>             
            <asp:BoundField HeaderText="Codigo Proyecto" DataField="CodigoProyecto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Nombre" SortExpression="NombreProyecto">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkCodigoProyecto" runat="server" CommandName="VerProyecto" CommandArgument='<%# Eval("CodigoProyecto") %>'
                        Text='<%# Eval("NombreProyecto") %>' ForeColor="Black" />
                </ItemTemplate>
            </asp:TemplateField>                                   
            <asp:TemplateField HeaderText="Ciudad">
                    <ItemTemplate>
                        <asp:Label ID="lblCiudad" runat="server" Text='<%# Eval("Ciudad") + " (" + Eval("Departamento") + ")" %>' />
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Convocatoria" DataField="NombreConvocatoria" HtmlEncode="false" />
            <asp:BoundField HeaderText="Evaluador" DataField="Evaluador.NombreCompleto" HtmlEncode="false" />
            <asp:TemplateField HeaderText="Avalado">
                <ItemTemplate>
                    <asp:Image runat="server" ImageUrl="~/Images/chulo.gif" Visible='<%# Eval("Avalado") %>' />
                </ItemTemplate>
            </asp:TemplateField>            
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
                TypeName="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluaciones.Evaluaciones"                
                SelectMethod="Get"
                SelectCountMethod="Count"
                MaximumRowsParameterName="maxRows"
                StartRowIndexParameterName="startIndex"
                EnablePaging="true"                
            >
            <SelectParameters> 
                <asp:ControlParameter ControlID="txtCodigoProyecto" Name="codigoProyecto" PropertyName="Text" Type="Int32" DefaultValue="0" />
            </SelectParameters>  
        </asp:ObjectDataSource>
                </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
