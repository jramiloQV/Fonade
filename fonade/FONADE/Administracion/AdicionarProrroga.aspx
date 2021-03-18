<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdicionarProrroga.aspx.cs" Inherits="Fonade.FONADE.Administracion.AdicionarProrroga" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <style type="text/css">
        table
        {
            width: 70%;
        }
        td
        {
            vertical-align: top;
        }
    </style>
    <script type="text/javascript">
        var ijn = function () {
            var uhb = confirm('¿Actualizar el plazo de prorroga?');
            if (!uhb) { return false; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <label>PRORROGA DE PROYECTOS</label>
    </h1>    
        <table>
            <tr>
                <td>
                    Proyecto
                </td>
                <td>
                    <asp:TextBox ID="txtNombreProyecto" runat="server" Width="200px" ValidationGroup="adicionar"
                        Enabled="false" Text='<%# DataBinder.GetPropertyValue(this, "nmbrpryct", "{0}")??string.Empty %>'></asp:TextBox>
                    <asp:ImageButton ID="btnBuscarProyecto" runat="server" ImageUrl="~/Images/BotBinocular.gif" OnClick="btnBuscarProyectoEvent" ToolTip="Buscar Proyecto" />                    
                </td>
            </tr>
            <tr>
                <td>
                    Meses Prorroga
                </td>
                <td>
                    <asp:TextBox ID="txtMesesProrroga" runat="server" Width="200px" ValidationGroup="adicionar"></asp:TextBox>
                    <br />                
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnAdicionarProrroga" runat="server" Text="Adicionar" ValidationGroup="adicionar" OnClick="btnAdicionarProrrogaEvent"
                         />
                    <asp:Button ID="btnCancelar" runat="server" Text="Volver" OnClick="btnVolverEvent" />
                </td>
            </tr>
        </table>           
</asp:Content>
