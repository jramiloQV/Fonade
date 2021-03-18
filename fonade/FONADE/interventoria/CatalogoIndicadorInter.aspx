<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoIndicadorInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoIndicadorInter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <script type="text/javascript">
        window.onunload = function (e) {
            opener.recargar();
        }
    </script>
    <form id="form1" runat="server">
    <div>
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <table width="90%" border="0" cellspacing="0" cellpadding="3">
            <tbody>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="L_NUEVOINDICADOR" runat="server" Text="Nuevo Indicador" Font-Bold="true" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="L_Aspecto" runat="server" Text="Aspecto:" Font-Bold="true" />
                    </td>
                    <td width="167" align="left" colspan="3">
                        <asp:TextBox ID="TB_Aspecto" runat="server" TextMode="MultiLine" Height="80px" Width="300px"
                            ValidationGroup="Actualizargrilla" MaxLength="300" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="L_FechaSeguimiento" runat="server" Text="Fecha de Seguimiento:" Font-Bold="true" />
                    </td>
                    <td width="167" align="left" colspan="3">
                        <asp:TextBox ID="TB_fechaSeguimiento" runat="server" Width="300px" ValidationGroup="Actualizargrilla"
                            MaxLength="60" />
                    </td>
                </tr>
                <tr>
                    <td align="Right">
                        <asp:Label ID="L_TipoIndicador" runat="server" Text="Tipo Indicador:" Font-Bold="true" />
                    </td>
                    <td width="167" align="left" colspan="3">
                        <asp:DropDownList ID="DD_TipoIndicador" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DD_TipoIndicador_SelectedIndexChanged">
                            <asp:ListItem Value="1">Gestión</asp:ListItem>
                            <asp:ListItem Value="2">Cualitativos y de Cumplimiento</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="middle" align="right">
                        <asp:Label ID="L_Indicador" runat="server" Text="Indicador:" Font-Bold="true" />
                    </td>
                    <td align="left" colspan="3">
                        <table border="0">
                            <tbody>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TB_Numerador" runat="server" Width="300px" ValidationGroup="Actualizargrilla"
                                            MaxLength="100" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <hr style="color: #000000;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TB_Denominador" runat="server" Width="300px" MaxLength="100" Visible="False" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="right">
                        <asp:Label ID="L_Descripcionindicador" runat="server" Text="Descripción del indicador:"
                            Font-Bold="true" />
                    </td>
                    <td width="167" align="left" colspan="3">
                        <asp:TextBox ID="TB_Descripcion" runat="server" Height="80px" TextMode="MultiLine"
                            Width="300px" ValidationGroup="Actualizargrilla" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="L_RangoAceptable" runat="server" Text="Rango Aceptable:" Font-Bold="true" />
                    </td>
                    <td width="167" align="left" colspan="3">
                        <asp:TextBox ID="TB_rango" runat="server" Width="20%" ValidationGroup="Actualizargrilla"
                            MaxLength="3" />
                        <asp:Label ID="L_RangoAceptablePorcentaje" runat="server" Text="%" Font-Bold="true" />
                        &nbsp;
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red"
                            ControlToValidate="TB_rango" ValidationGroup="Actualizargrilla" ValidationExpression="^100$|^\d{0,2}(\.\d{1,7})?$"
                            ErrorMessage="% invalido" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="l_Observacion" runat="server" Text="Observacion:" Font-Bold="true" />
                    </td>
                    <td width="167">
                        <asp:TextBox ID="TB_Observacion" runat="server" TextMode="MultiLine" Height="80px"
                            Width="300px" ValidationGroup="Actualizargrilla" />
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="Label1" runat="server" Text="Aprobar:" Font-Bold="True" Visible="False" />
                        <asp:CheckBox ID="checkAprobar" runat="server" Text="" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <asp:Button ID="B_Crear" runat="server" OnClick="B_Crear_Click" Text="Crear" ValidationGroup="Actualizargrilla" />
                    </td>
                    <td>
                        <asp:Button ID="B_Cancelar" runat="server" Text="Cerrar" OnClientClick="javascript:window.close();" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
