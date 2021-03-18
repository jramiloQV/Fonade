<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="AdicionarInformePresupuestal.aspx.cs" Inherits="Fonade.FONADE.interventoria.AdicionarInformePresupuestal" %>

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

        function borrar2() {
            return confirm('Esta seguro que desea borrar el cumplimiento seleccionado?');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <AjaxControlToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </AjaxControlToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="contentPrincipal">
                <asp:Panel ID="principal" runat="server">
                    <h1>
                        <asp:HiddenField ID="Estado" runat="server" />
                        <asp:Label ID="L_titulo" runat="server" Text="ADICIONAR INFORME PRESUPUESTAL" />
                        <asp:HiddenField ID="hdf_cod_proyecto" runat="server" Visible="false" />
                    </h1>
                    <br />
                    <br />
                    <div id="contenido">
                        <table>
                            <thead>
                                <tr>
                                    <th colspan="2">FORMATO 01A
                                    </th>
                                </tr>
                                <tr>
                                    <th colspan="2">INFORME DE SEGUIMIENTO PRESUPUESTAL DE LA INTERVENTORIA
                                    </th>
                                </tr>
                                <tr>
                                    <th colspan="2">
                                        <asp:Label ID="L_TituloNombre" runat="server" Text="Interventor " />
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td colspan="2">&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Coordinador
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCoordinador" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Periodo
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPeriodo" Text="" runat="server" Visible="false" />
                                        <asp:DropDownList ID="dd_periodos" runat="server" AutoPostBack="false" Width="164px"
                                            Height="16px" OnSelectedIndexChanged="dd_periodos_SelectedIndexChanged">
                                            <asp:ListItem Value="" Text="Seleccione el periodo" />
                                            <asp:ListItem Value="1" Text="Enero-Febrero" />
                                            <asp:ListItem Value="2" Text="Marzo-Abril" />
                                            <asp:ListItem Value="3" Text="Mayo-Junio" />
                                            <asp:ListItem Value="4" Text="Julio-Agosto" />
                                            <asp:ListItem Value="5" Text="Septiembre-Octubre" />
                                            <asp:ListItem Value="6" Text="Noviembre-Diciembre" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Contrato
                                    </td>
                                    <td>
                                        <asp:Label ID="lblContrato" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fecha
                                    </td>
                                    <td>
                                        <asp:Label ID="lblFecha" Text="" runat="server" Visible="false" />
                                        <asp:TextBox ID="txtDate" runat="server" ReadOnly="true" />
                                        <asp:ImageButton ID="imgPopup" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                                            runat="server" />
                                        <AjaxControlToolkit:CalendarExtender ID="c_fecha_s" PopupButtonID="imgPopup" runat="server"
                                            TargetControlID="txtDate" Format="dd/MM/yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Empresa
                                    </td>
                                    <td>
                                        <asp:Label ID="lblEmpresa" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Teléfono
                                    </td>
                                    <td>
                                        <asp:Label ID="lblTelefono" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Dirección
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDireccion" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Ciudad
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCiudad" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Socios
                                    </td>
                                    <td>
                                        <asp:Panel ID="pSocios" runat="server">
                                            <asp:Table ID="t_table" runat="server">
                                            </asp:Table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <asp:Panel ID="pnl_buttons" runat="server" HorizontalAlign="Center">
                            <asp:CheckBox ID="chk_aprobar" Text="Aprobar:" runat="server" Visible="false" AutoPostBack="true"
                                TextAlign="Left" />
                            <br />
                            <div id="ocl">
                                <asp:Button ID="btn_grabar" Text="Grabar" runat="server" OnClick="btn_grabar_Click"
                                    Visible="false" />
                                <asp:Button ID="btn_imprimir" Text="Imprimir" runat="server" OnClick="btn_imprimir_Click" />
                            </div>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="p_iB" runat="server" Width="100%">
                            <asp:Table ID="tabla_datos" runat="server" CssClass="Grilla">
                            </asp:Table>
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_grabar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
