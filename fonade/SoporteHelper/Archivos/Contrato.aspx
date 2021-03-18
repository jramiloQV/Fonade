<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Contrato.aspx.cs" Inherits="Fonade.SoporteHelper.Archivos.Contrato" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de borrar este archivo ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar archivos por proyecto"></asp:Label>          
    <br />
    <asp:TextBox ID="txtCodigoProyecto" runat="server"></asp:TextBox>        
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar Archivos" OnClick="Button1_Click" />
    <br />     
    <br />   
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <br />    
    <br />    
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Documentos de contratos"></asp:Label>
    <br />            
    <asp:GridView ID="gvContratos" runat="server" AllowPaging="false"   AutoGenerateColumns="False" DataSourceID="dataContrato" EmptyDataText="No hay archivos." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvFormulacion_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="Eliminar" CommandArgument='<%# Eval("Id") %>'
                        CommandName="Eliminar" CausesValidation="False" Text='Eliminar' runat="server"                                                
                        />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />  
            <asp:BoundField HeaderText="Codigo de proyecto" DataField="CodigoProyecto" HtmlEncode="false" />                        
            <asp:TemplateField HeaderText="Ver archivo">
                <ItemTemplate>
                    <asp:LinkButton ID="VerArchivo" CommandArgument='<%#  Eval("Nombre") + ";" + Eval("Url") %>'
                        CommandName="VerArchivo" CausesValidation="False" Text='Ver archivo' runat="server"                                                
                        />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
       
        <asp:ObjectDataSource
                ID="dataContrato"
                runat="server"
                TypeName="Fonade.SoporteHelper.Archivos.Contrato"
                SelectMethod="GetArchivos"
                >
            <SelectParameters> 
                <asp:ControlParameter ControlID="txtCodigoProyecto" Name="codigoProyecto" PropertyName="Text" Type="Int32" DefaultValue="" />
            </SelectParameters>  
        </asp:ObjectDataSource>     
</asp:Content>