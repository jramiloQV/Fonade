<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReiniciarAceptarTerminosyCondiciones.aspx.cs" Inherits="Fonade.SoporteHelper.Usuarios.ReiniciarAceptarTerminosyCondiciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de reiniciar los términos y condiciones?');
        }
    </script>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>Reiniciar Terminos y Condiciones</h1>

    <div>
        <asp:DropDownList ID="cmbTipo" runat="server" AutoPostBack="false">
            <asp:ListItem Text="Código de proyecto" Value="id" Selected="True" />
        </asp:DropDownList>

        <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>

        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
    </div>
    <div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="gvMain" runat="server" AllowPaging="false" AutoGenerateColumns="False"
            EmptyDataText="No se encontro información."
            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla"
            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvMain_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Código de proyecto" DataField="Id_Proyecto" HtmlEncode="false" />
                <asp:BoundField HeaderText="Nombre del proyecto" DataField="NomProyecto" HtmlEncode="false" />
                <asp:BoundField HeaderText="Estado" DataField="NomEstado" HtmlEncode="false" />
                <asp:BoundField HeaderText="Nombres" DataField="Nombres" HtmlEncode="false" />
                <asp:BoundField HeaderText="Apellidos" DataField="Apellidos" HtmlEncode="false" />
                <asp:BoundField HeaderText="Identificacion" DataField="Identificacion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Email" DataField="Email" HtmlEncode="false" />
                <asp:TemplateField HeaderText="Acción">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("Id_Contacto")%>'
                            CommandName="Restablecer" CausesValidation="False" OnClientClick="return alerta();"
                            Text="Reiniciar Terminos y Condiciones" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>
</asp:Content>
