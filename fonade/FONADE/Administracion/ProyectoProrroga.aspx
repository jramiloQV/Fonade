<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ProyectoProrroga.aspx.cs" Inherits="Fonade.FONADE.Administracion.ProyectoProrroga" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
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
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <h1>
        <label>PRORROGA DE PROYECTOS</label>
    </h1>
    <%Page.DataBind(); %>
    
    <asp:Panel ID="pnlproyectos" runat="server">
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="imgagregar" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" PostBackUrl="~/FONADE/Administracion/AdicionarProrroga.aspx" />
                    &nbsp;&nbsp;
                    <asp:LinkButton ID="lnkagregar" runat="server" Text="Adicionar Proyecto prorroga" PostBackUrl="~/FONADE/Administracion/AdicionarProrroga.aspx"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                        Buscar por código de proyecto:&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtBuscarProyecto" Text="" AutoPostBack="false" runat="server"></asp:TextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnBuscarProyecto" runat="server" Text="Buscar" />  
                    </td>
                </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    <asp:GridView ID="gv_proyectosProrroga" runat="server" CssClass="Grilla" Width="100%"
                        AutoGenerateColumns="false" AllowPaging="True" PageSize="50" 
                        OnPageIndexChanging="gv_proyectosProrroga_PageIndexChanging">
                        <Columns>
                            <asp:BoundField HeaderText="Id" DataField="Id_proyecto" />
                            <asp:BoundField HeaderText="Proyecto" DataField="nomproyecto" />
                            <asp:BoundField HeaderText="Prorroga" DataField="Prorroga" />
                            <asp:BoundField HeaderText="Operador" DataField="NombreOperador" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
