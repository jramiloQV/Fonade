<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ActaInicio.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.ActaInicio" %>

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

        function dateselect(ev) {
            //var calendarBehavior1 = $find("bodyContentPlace_calendarFechaInicio");
            //var d = calendarBehavior1._selectedDate;
            //var now = new Date();
            //calendarBehavior1.get_element().value = d.getDate() + 1 + "/"
            //                                        + (d.getMonth() + 1) + "/"
            //                                        + d.getFullYear() + " "
            //                                        + now.getHours() + ":" + now.getMinutes();            
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <h1>
                <asp:Label Text="Acta de inicio" runat="server" ID="lblMainTitle" Visible="true" />
            </h1>
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label7" runat="server" Font-Bold="true" Text="Fecha y Hora de Inicio del Acta:"></asp:Label>
                       </td>
                    <td class="auto-style3">
                       
                        <asp:TextBox ID="txtFechaInicio" runat="server" autocomplete="off"  required="true"
                            pattern="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"></asp:TextBox>
                        <asp:CalendarExtender ID="calendarFechaInicio" runat="server"
                            Format="dd/MM/yyyy hh:mm tt"                            
                            TargetControlID="txtFechaInicio" ClearTime="True" 
                            EnabledOnClient="True" TodaysDateFormat="" >
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label1" runat="server" Font-Bold="true" Text="Contrato N°"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblNumeroContrato" runat="server" Font-Bold="true" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Tipo de contrato"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblTipoDeContrato" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Objeto"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblObjeto" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Valor"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblValor" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Contratista (s)"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblContratistas" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label6" Font-Bold="true" runat="server" Text="Plazo"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblPlazo" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar - Dejar Borrador" OnClick="btnCancelar_Click"></asp:Button>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Publicar" OnClientClick="return alerta();" OnClick="btnAdd_Click"></asp:Button>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
