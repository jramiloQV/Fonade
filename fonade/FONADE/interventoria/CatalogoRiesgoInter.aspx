<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoRiesgoInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoRiesgoInter" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style2 {
            width: 96px;
        }
    </style>
</head>
<body style="width: 500px;">
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlRevisaRiesgo" runat="server" Height="65px">
            <table style="width:100%;">
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">Tarea cerrada</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2" style="text-align: center;">
                        <asp:Button ID="btnCerrar1" runat="server" OnClick="btnCerrar1_Click" Text="Cerrar" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlRiesgo" runat="server" Height="126px">
            <table class="style1" style="width: 500px;">
                <tr>
                    <td colspan="2">
                        <asp:Label ID="L_Riesgo" runat="server" Text="Riesgo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="TB_Riesgo" runat="server" Width="100%" Height="50px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV_Riesgo" runat="server" ErrorMessage="RequiredFieldValidator"
                        ControlToValidate="TB_Riesgo" ValidationGroup="crear" Text="Campo Requerido"
                        ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="L_Mitigacion" runat="server" Text="Mitigacion"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="TB_Mitigacion" runat="server" Width="100%" Height="50px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RFV_Mitigacion" runat="server" ErrorMessage="RequiredFieldValidator"
                        ControlToValidate="TB_Mitigacion" ValidationGroup="crear" Text="Campo Requerido"
                        ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text=" Eje Funcional:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlejefuncional" runat="server" Width="250px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblobservacion" runat="server" Text="Observación:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtobservacion" runat="server" Width="100%" Height="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tr_aprobar" runat="server" visible="false">
                    <td colspan="2">
                        <asp:CheckBox ID="Aprobar" Text="Aprobar:" runat="server" TextAlign="Left" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:Button ID="B_Crear" runat="server" Text="Crear" CssClass="boton_Link_Grid" OnClick="B_Crear_Click"
                        ValidationGroup="crear" />
                        &nbsp;<asp:Button ID="btnCerrar" Text="Cerrar" runat="server" OnClick="btnCerrar_Click" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />

    </div>
    </form>
</body>
</html>
