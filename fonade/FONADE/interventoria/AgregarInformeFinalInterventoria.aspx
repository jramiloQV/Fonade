<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="AgregarInformeFinalInterventoria.aspx.cs" Inherits="Fonade.FONADE.interventoria.AgregarInformeFinalInterventoria" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
        td
        {
            vertical-align: top;
        }
    </style>
    <script type="text/javascript">

        function imprimir() {
            document.getElementById("oculto").style.visibility = "visible";
            document.getElementById("ocl").style.display = "none";

            var divToPrint = document.getElementById('contentPrincipal');
            var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            //setTimeout(function () { newWin.close(); }, 1000);
            document.getElementById("oculto").style.visibility = "hidden";
            document.getElementById("ocl").style.display = "block";
        }

        function alerta() {
            return confirm('Esta seguro que desea borrar el cumplimiento seleccionado?');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <AjaxControlToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </AjaxControlToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="contentPrincipal">
                <asp:Panel ID="principal" runat="server">
                    <h1>
                        <asp:Label ID="L_titulo" runat="server" Text="INFORMES DE INTERVENTORÍA" Visible="False" />
                    </h1>
                    <br />
                    <br />
                    <div id="contenido">
                        <table>
                            <thead>
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="lblinforme" runat="server" Text="Interventor "></asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="L_TituloNombre" runat="server" Text="Interventor "></asp:Label>
                                    </th>
                                </tr>
                                <tr>
                                    <th colspan="4">
                                        <asp:Label ID="lblnomcoordinador" runat="server" Text="Interventor "></asp:Label>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="4">
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Número Contrato:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblnumContrato" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td>
                                        Fecha Informe:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblfechainforme" runat="server" Text=""></asp:Label>
                                        <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" />
                                        <asp:ImageButton ID="imgPopup" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                                            runat="server" />
                                        <ajaxToolkit:CalendarExtender ID="c_fecha_s" PopupButtonID="imgPopup" runat="server"
                                            TargetControlID="txtDate" Format="dd/MM/yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Empresa
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Teléfono
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Dirección
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        Socios
                                    </td>
                                    <td colspan="2">
                                        <asp:Panel ID="pSocios" runat="server">
                                            <asp:Table ID="t_table" runat="server">
                                            </asp:Table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <br />
                        <asp:Panel ID="p_iB" runat="server" Width="100%">
                            <asp:Table ID="t_variable" runat="server" class="Grilla">
                                <asp:TableHeaderRow>
                                    <asp:TableHeaderCell>CRITERIO</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>CUMPLIMIENTO A VERIFICAR</asp:TableHeaderCell>
                                    <asp:TableHeaderCell>OBSERVACIÓN INTERVENTOR</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3"></asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Panel ID="pnl_options" runat="server" Visible="false">
                            <asp:HiddenField ID="hdf_cod_proyecto" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_CodInforme" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_FechaInforme" runat="server" Visible="false" />
                            <asp:HiddenField ID="hdf_Estado" runat="server" Visible="false" />
                        </asp:Panel>
                        <div id="dvianexos">
                            <asp:Panel ID="p_Anexos" runat="server">
                                <asp:Table ID="t_anexos" runat="server" class="Grilla">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell ColumnSpan="2">ANEXOS</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                    <asp:TableRow>
                                        <asp:TableCell ColumnSpan="2"></asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                            <br />
                            <br />
                        </div>
                    </div>
                    <%--<div id="imprimir" style="text-align: center; width: 100%;">--%>
                    <div id="ocl" style="text-align: center;">
                        <asp:Button ID="btn_guardar" runat="server" Text="Guardar" OnClick="btn_guardar_Click"
                            Visible="False" />
                        <asp:Button ID="btn_borrar" runat="server" Text="Borrar" OnClick="btn_borrar_Click"
                            Visible="False" />
                        <asp:Button ID="btn_imprimir" runat="server" Text="Imprimir" OnClientClick="imprimir()"
                            Visible="False" />
                        <asp:Label ID="espacio_aprobar" Text="<br/>" runat="server" Visible="false" />
                        <center>
                            <asp:Label ID="lbl_texto_aprobar" Text="Aprobar: " runat="server" Visible="false"
                                Font-Bold="true" />
                            <asp:RadioButtonList ID="Aprobar" runat="server" Visible="false" RepeatDirection="Horizontal"
                                Width="100px">
                                <asp:ListItem Text="SI" Value="True" />
                                <asp:ListItem Text="NO" Value="False" />
                            </asp:RadioButtonList>
                        </center>
                        <asp:Button ID="btn_enviar" Text="Enviar" runat="server" OnClick="btn_enviar_Click" />
                    </div>
                    <%--</div>--%>
                    <div id="oculto" style="visibility: hidden;">
                        <p>
                            Dadas las condiciones en que el Contratista se viene cumpliendo o incumpliendo,
                            con las obligaciones del contrato, el INTERVENTOR recomienda FONADE:</p>
                        <br />
                        <br />
                        <p>
                            Para constancia firman:</p>
                        <br />
                        <br />
                        ________________________________&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;________________________________
                        <br />
                        Interventor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        Contratista
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_guardar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_borrar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_imprimir" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
