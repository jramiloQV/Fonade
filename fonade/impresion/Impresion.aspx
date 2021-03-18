<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Impresion.aspx.cs"
    Inherits="Fonade.impresion.Impresion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function postBackByObject() {
            var o = window.event.srcElement;
            if (o.tagName == "INPUT" && o.type == "checkbox") {
                __doPostBack("", "");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--<asp:ObjectDataSource ID="ODS_Proyectos" runat="server" SelectMethod="enlaceproyecto"
        TypeName="Fonade.impresion.Impresion">
        <SelectParameters>
            <asp:ControlParameter ControlID="txt_codProyecto" Name="param" PropertyName="Text"
                Type="String" ConvertEmptyStringToNull="False" />
        </SelectParameters>
    </asp:ObjectDataSource>--%>
    <h1>
        <asp:Label ID="lbl_Titulo" runat="server" Text="IMPRESIÓN" />
    </h1>
    <br />
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblCodigoProyecto" runat="server" Text="Código Proyecto" />
                :
                <asp:TextBox ID="txt_codProyecto" runat="server" Width="17%" />
                <asp:LinkButton ID="lnk_buscarProyectos" Text="Buscar..." runat="server" ForeColor="#CC0000"
                    Style="text-decoration: none;" OnClick="lnk_buscarProyectos_Click" ToolTip="Digite el código del proyecto y presione el botón Buscar." />
            </td>
            <td>
                <asp:DropDownList ID="DDL_proyecto" runat="server" Width="90%" OnSelectedIndexChanged="DDL_proyecto_SelectedIndexChanged" AutoPostBack="true" Visible="False" />
                <br />
            </td>
        </tr>
        <tr>
            <td>
                <br />
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label1" runat="server" Text="Plan de Negocio: " />
                <asp:Label ID="lblNombrePlanNeg" runat="server" Text="."></asp:Label>
                <asp:Label ID="lblBusquedaPlanNegocio" runat="server" Text="ResultSearch" Visible="False"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TreeView ID="tv_tab" runat="server" OnTreeNodeCheckChanged="tv_tab_TreeNodeCheckChanged" ExpandDepth="0">
                </asp:TreeView>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <br />
                <br />
                <asp:Button ID="btnimpresion" runat="server" Text="Ver..." OnClick="btnimpresion_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
