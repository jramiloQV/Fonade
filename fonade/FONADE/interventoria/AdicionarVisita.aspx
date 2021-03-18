<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdicionarVisita.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.AdicionarVisita" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxControlToolkit" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Agendar Visita</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        table {
            width: 95%;
        }
    </style>
    <script type="text/javascript">
        function SetDate(paramtro) {
            var fechas = paramtro.split(';');

            $('#txtFechaInicio').val(fechas[0]);
            $('#txtFechaFin').val(fechas[1]);
        }

        function refreshwindow() {
            window.opener.location.href = window.opener.location.href;
            if (window.opener.progressWindow) {
                window.opener.progressWindow.close()
            }
            window.close();
        }

        function confirmation() {
            if (confirm("Desea eliminar esta visita?"))
                return true;
            else return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="ContentInfo" style="width: 1000px; height: auto;">
            <ajaxToolkit:ToolkitScriptManager ID="tk_1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            <asp:UpdatePanel ID="updt_add_visita" runat="server">
                <ContentTemplate>
                    <!-- Pedro V. Carreño  Al mirar una visita ya creada No tienen los botones de Borrar y cerrar ERROR INT-80 - 14/11/2014 - INICIO-->
                    <br />
                    <table>
                        <tr>
                            <td>
                                <asp:Panel ID="pnl_tarea_a_crear" runat="server">
                                    <div style="float: left">
                                        <h1>
                                            <label id="lblTitulo" runat="server">AGENDAR VISITA</label>
                                        </h1>
                                    </div>
                                    <table>
                                        <tr>
                                            <td>Nombre:<br />
                                                <asp:DropDownList ID="DD_Empresas" runat="server" AutoPostBack="true" Width="450px"
                                                    OnSelectedIndexChanged="DD_Empresas_SelectedIndexChanged" />
                                            </td>
                                            <td style="width: 5px"></td>
                                            <td>
                                                <div style="float: left">
                                                    Fecha Inicio:<br />
                                                    <asp:TextBox runat="server" ID="txtFechaInicio" type="date" ClientIDMode="Static" />
                                                </div>
                                                <div style="float: left">
                                                    &nbsp;&nbsp;Fecha Fin:<br />
                                                    &nbsp;&nbsp;<asp:TextBox runat="server" ID="txtFechaFin" type="date" ClientIDMode="Static" />
                                                </div>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Nit:<br />
                                                <asp:TextBox ID="TXT_nit" runat="server" Enabled="false" Width="200" />
                                            </td>
                                            <td></td>
                                            <td>Ciudad:<br />
                                                <asp:TextBox ID="TXT_ciudad" runat="server" Enabled="false" Width="200" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">Objeto:<br />
                                                <asp:TextBox ID="TXT_objeto" runat="server" Width="830px" TextMode="MultiLine" MaxLength="254" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" style="text-align:center">
                                                <asp:Button ID="btn_agendar" Text="Agendar" runat="server" OnClick="btn_agendar_Click"  />&nbsp;
                                                <asp:Button ID="btnBorra" Text="Borrar" runat="server" Visible="false" OnClick="btnBorra_Click" OnClientClick="return confirm('Desea eliminar esta visita?');" />&nbsp;
                                                <asp:Button ID="Button2" Text="Cerrar" runat="server" OnClientClick="window.close();" />
                                            </td>
                                        </tr>
                                    </table>

                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
    <script type="text/javascript">
        window.onload = function () {
            SetDateNow();
        }

        function SetDateNow() {
            var fecha = document.getElementById('txtFechaInicio').value;
            if (fecha == "") {
                var now = new Date();

                var day = ("0" + now.getDate()).slice(-2);
                var month = ("0" + (now.getMonth() + 1)).slice(-2);

                var today = now.getFullYear() + "-" + (month) + "-" + (day);

                $('#txtFechaInicio').val(today);
                $('#txtFechaFin').val(today);
            }
            
        }
    </script>
</body>
</html>
