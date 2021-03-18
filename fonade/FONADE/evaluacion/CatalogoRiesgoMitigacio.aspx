<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoRiesgoMitigacio.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoRiesgoMitigacio" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table class="style1" style="width: 500px;">
            <tr>
                <td colspan="2">
                    <asp:Label ID="L_Riesgo" runat="server" Text="Riesgo"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="TB_Riesgo" runat="server" Width="100%" Height="50px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RFV_Riesgo" runat="server" 
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="TB_Riesgo" 
                        ValidationGroup="crear" Text="Campo Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
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
                    <asp:RequiredFieldValidator ID="RFV_Mitigacion" runat="server" 
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="TB_Mitigacion" 
                        ValidationGroup="crear" Text="Campo Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align:center;"><asp:Button ID="B_Crear" runat="server" Text="Crear" 
                        CssClass="boton_Link_Grid" onclick="B_Crear_Click" ValidationGroup="crear" /></td>
                <td></td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
