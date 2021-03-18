<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="EliminarAnexosAcreditacion.aspx.cs" Inherits="Fonade.SoporteHelper.Archivos.EliminarAnexosAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de eliminar el archivo?');
        }
    </script>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Buscar archivos acreditación por proyecto"></asp:Label>
    <br />
    <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
    <asp:Button ID="btnBuscar" runat="server" Text="Buscar Archivos Anexos Acreditación" OnClick="btnBuscar_Click" />
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Documentos de Anexos Acreditación"></asp:Label>
    <br />
    <br />
    <asp:GridView ID="gvAnexos" runat="server" AllowPaging="false" AutoGenerateColumns="False"
        EmptyDataText="No hay archivos." Width="98%" BorderWidth="0"
        CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
        OnRowCommand="gvAnexos_RowCommand">
        <Columns>
            <asp:TemplateField HeaderText="Eliminar">
                <ItemTemplate>
                    <asp:LinkButton ID="Eliminar" CommandArgument='<%# Eval("Id_documento") %>' OnClientClick="return alerta();"
                        CommandName="Eliminar" CausesValidation="False" Text='Eliminar' runat="server" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Codigo de proyecto" DataField="CodigoProyecto" HtmlEncode="false" />           
            <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />
            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
             <asp:BoundField DataField="NombreEnPlataforma" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
            <asp:TemplateField HeaderText="Descargar Archivo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento"
                        CommandArgument='<%# ConfigurationManager.AppSettings.Get("DirVirtual") + Eval("Ruta") %>'>
                        <asp:Image ID="img_Url" runat="server" ImageUrl="~/Images/IcoDocNormal.gif" />
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
    </asp:GridView>
</asp:Content>
