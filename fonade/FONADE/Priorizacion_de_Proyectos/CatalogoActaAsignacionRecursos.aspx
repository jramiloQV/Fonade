<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoActaAsignacionRecursos.aspx.cs" Inherits="Fonade.FONADE.Priorizacion_de_Proyectos.CatalogoActaAsignacionRecursos" MasterPageFile="~/Master.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_listadoActas" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
        onselecting="lds_listadoActas_Selecting" >
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelInfo" runat="server" Visible="true" Width="98%" UpdateMode="Conditional">
    <ContentTemplate>

        <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
        
        <asp:GridView ID="GridViewActas"  CssClass="Grilla" PageSize="50" runat="server"  
            AllowSorting="True"
            AutoGenerateColumns="false" DataSourceID="lds_listadoActas" AllowPaging="true"
            EmptyDataText="No hay información disponible."  onrowcommand="GridViewProyectos_RowCommand"
             Width="100%" >
        
            <Columns>
                <asp:TemplateField HeaderText="Número de Acta"  SortExpression="numacta">
                    <ItemTemplate>
                        <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("id_acta") %>' />
                        <asp:Button ID="hl_numacta" Text='<%# Eval("numacta") %>' CommandArgument='<%# Eval("id_acta") %>' CommandName="VerActa" CssClass="boton_Link_Grid" runat="server"  />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" SortExpression="nomacta">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_nombre" runat="server" Text='<%# Eval("nomacta") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Convocatoria" SortExpression="nomconvocatoria">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_convocatoria" runat="server" Text='<%# Eval("nomconvocatoria") %>'></asp:HyperLink>
                    </ItemTemplate> 
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Operador" SortExpression="NombreOperador">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Operador" runat="server" Text='<%# Eval("NombreOperador") %>'></asp:HyperLink>
                    </ItemTemplate> 
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>