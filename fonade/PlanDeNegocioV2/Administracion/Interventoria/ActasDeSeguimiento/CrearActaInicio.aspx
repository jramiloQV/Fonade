<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CrearActaInicio.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.CrearActaInicio" %>
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
            return confirm('¿ Esta seguro de crear esta acta de inicio ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true" ></asp:ToolkitScriptManager>    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" >               
        <ContentTemplate>
            <h1>
                <asp:Label Text="Crear acta de inicio" runat="server" ID="lblMainTitle" Visible="true" />
            </h1>
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Contrato N°"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblNumeroContrato" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Tipo de contrato"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblTipoDeContrato" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Objeto"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblObjeto" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Valor"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblValor" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Contratista (s)"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblContratistas" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" >
                        <asp:Label ID="Label6" Font-Bold="true" runat="server" Text="Plazo"></asp:Label>                        
                    </td>
                    <td class="auto-style3" >
                        <asp:Label ID="lblPlazo" Font-Bold="true" runat="server" Text="N/A"></asp:Label>                        
                    </td>
                </tr>
                
                <tr>
                    <td >
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" ></asp:Button>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Guardar acta de inicio" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>