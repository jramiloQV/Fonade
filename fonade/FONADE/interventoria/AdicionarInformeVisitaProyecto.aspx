<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdicionarInformeVisitaProyecto.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.AdicionarInformeVisitaProyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxControlToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function borrar() {
            return confirm('Está seguro de ELIMINAR este informe???');
        }
    </script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <AjaxControlToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </AjaxControlToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF"
                    align="center">
                    <tbody>
                        <tr>
                            <td align="center" valign="top" width="98%">
                                <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table width="98%" border="0" cellspacing="1" cellpadding="4">
                                    <tbody>
                                        <tr>
                                            <td class="TituloDestacados" width="25%">
                                                Informe de Visita de Interventoría
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <%--<form name="frmDatos" method="POST" action="AdicionarInformeVisitaProyecto.asp" onsubmit="return ValidaFormulario();">
                                </form>--%>
                                        <tr>
                                            <td colspan="2">
                                                <table border="0" cellspacing="0" cellpadding="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td width="57%">
                                                                &nbsp;Nombre Informe<br />
                                                                &nbsp;
                                                                <asp:TextBox ID="NombreInforme" runat="server" size="40" MaxLength="255" Enabled="false" />
                                                            </td>
                                                            <td>
                                                                Nombre Empresa<br />
                                                                <asp:HiddenField ID="Empresa" runat="server" />
                                                                <asp:TextBox ID="NomEmpresa" runat="server" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Origen
                                                <table>
                                                    <tbody>
                                                        <tr valign="top">
                                                            <td>
                                                                <b>Departamento:</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--Colocar código para cambio de Ciudad!--%>
                                                                <asp:DropDownList ID="SelDpto2" runat="server" Enabled="false" OnSelectedIndexChanged="SelDpto2_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td>
                                                                <b>Ciudad:</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="25" align="left">
                                                                <asp:DropDownList ID="SelMun" runat="server" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                            <td>
                                                Destino
                                                <table>
                                                    <tbody>
                                                        <tr valign="top">
                                                            <td>
                                                                <b>Departamento:</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--Colocar código para cambio de Ciudad!--%>
                                                                <asp:DropDownList ID="SelDpto1" runat="server" Enabled="false" OnSelectedIndexChanged="SelDpto1_SelectedIndexChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td>
                                                                <b>Ciudad:</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="25" align="left">
                                                                <asp:DropDownList ID="SelMun1" runat="server" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <b>Costos Medios de Transporte:</b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="25" align="left" colspan="2">
                                                <table cellspacing="0" cellpadding="0" width="100%">
                                                    <tbody>
                                                        <tr>
                                                            <td width="2%" valign="center" align="right">
                                                                Otro&nbsp;&nbsp;
                                                            </td>
                                                            <td height="25" align="left" width="20%">
                                                                <asp:TextBox ID="Valor1" runat="server" Enabled="false" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="2%" valign="center" align="right">
                                                                Avión&nbsp;&nbsp;
                                                            </td>
                                                            <td height="25" align="left" width="20%">
                                                                <asp:TextBox ID="Valor2" runat="server" Enabled="false" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="2%" valign="center" align="right">
                                                                Bus&nbsp;&nbsp;
                                                            </td>
                                                            <td height="25" align="left" width="20%">
                                                                <asp:TextBox ID="Valor3" runat="server" Enabled="false" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td width="2%" valign="center" align="right">
                                                                Barco&nbsp;&nbsp;
                                                            </td>
                                                            <td height="25" align="left" width="20%">
                                                                <asp:TextBox ID="Valor4" runat="server" Enabled="false" MaxLength="10" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <b>Ingrese el costo del medio de transporte empleado en la visita.</b>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <b>Fecha Salida:</b>
                                            </td>
                                            <td height="25" align="left" width="20%">
                                                <asp:TextBox ID="FechaSalida" runat="server" ReadOnly="true" />
                                                <asp:ImageButton ID="imgPopup" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                                                    runat="server" />
                                                <ajaxToolkit:CalendarExtender ID="c_FechaSalida" PopupButtonID="imgPopup" runat="server"
                                                    TargetControlID="FechaSalida" Format="dd/MM/yyyy" />
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td>
                                                <b>Fecha Regreso:</b>
                                            </td>
                                            <td height="25" align="left" width="20%">
                                                <asp:TextBox ID="FechaRegreso" runat="server" ReadOnly="true" />
                                                <asp:ImageButton ID="imgRegreso" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                                                    runat="server" />
                                                <ajaxToolkit:CalendarExtender ID="c_FechaRegreso" PopupButtonID="imgRegreso" runat="server"
                                                    TargetControlID="FechaRegreso" Format="dd/MM/yyyy" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tituloDestacados">
                                                Contenido Informe Visita
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <b>Información Técnica:</b><br />
                                                <asp:TextBox ID="InformacionTecnica" runat="server" TextMode="MultiLine" Columns="60"
                                                    Rows="10" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <b>Información Financiera:</b><br />
                                                <asp:TextBox ID="InformacionFinanciera" runat="server" TextMode="MultiLine" Columns="60"
                                                    Rows="10" Enabled="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" align="right">
                                                <asp:HiddenField ID="Accion" runat="server" Value="Modificar" />
                                                <asp:HiddenField ID="AccionB" runat="server" />
                                                <asp:HiddenField ID="AccionIngresar" runat="server" Value="Ingresar" />
                                                <asp:Button ID="btn_IngresarInforme" Text="Ingresar Informe" runat="server" Visible="false"
                                                    OnClick="btn_IngresarInforme_Click" />
                                            </td>
                                        </tr>
                                        <tr align="left" valign="top">
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_IngresarInforme" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
