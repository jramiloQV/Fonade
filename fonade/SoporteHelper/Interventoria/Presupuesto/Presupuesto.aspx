<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Presupuesto.aspx.cs" Inherits="Fonade.SoporteHelper.Interventoria.Presupuesto.Presupuesto" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .auto-style3 {
            width: 197px;
        }
    </style>
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro de actualizar este presupuesto ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Modificación de presupuesto"></asp:Label>
            <br />
            <asp:TextBox ID="txtCodigoProyecto" runat="server"></asp:TextBox>        
            <asp:Button ID="btnBuscarProyecto" runat="server" Text="Buscar Proyecto" OnClick="Button1_Click"/>
            <br />     
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
            <br />
            <br />
            <table id="gvPrespuesto" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3"> <asp:Label ID="lblTitleSalariosRecomendados" Font-Bold="true" runat="server" Text="Salarios Minimos Recomendados"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitlePresupuestoTotal" Font-Bold="true" runat="server" Text="PresupuestoTotal"></asp:Label></td>
                    <td class="auto-style3"> <asp:Label ID="lblTitleSalarioMinimoVigente" Font-Bold="true" runat="server" Text="Salario minimo vigente"></asp:Label></td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblValorRecomendadoActual" runat="server" Text="0"></asp:Label>
                    </td>
                    <td><asp:Label ID="lblPresupuestoTotalActual" runat="server" Text="0"></asp:Label></td>
                    <td><asp:Label ID="lblSalarioMinimoVigente" runat="server" Text="0"></asp:Label></td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtValorRecomendadoActual" AutoPostBack="true" OnTextChanged="txtValorRecomendadoActual_TextChanged" Text="" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td><asp:Label ID="lblPresupuestoTotalNuevo" runat="server" Text="0"></asp:Label></td>
                    <td><asp:Button ID="btnActualizarPresupuesto" runat="server" Text="Actualizar presupuesto" OnClientClick="return alerta();" OnClick="btnActualizarPresupuesto_Click"></asp:Button></td>
                </tr>
            </table>
            <br />   
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
