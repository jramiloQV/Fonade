<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoActividadPOTMP.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoActividadPOTMP" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FONDO EMPRENDER - Actividad</title>
    <style type="text/css">
        body,html{
            background-color:#fff !important;
            background-image:none !important;
        }
        .panelmeses
        {
            margin: 0px auto;
            text-align: center;
        }
        .auto-style1 {
            width: 1170px;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" style="font-size: 9px">
    <div style="overflow:scroll; Width:100% ;height:100%"  >
        <table>
            <tr>
                <td class="style50" colspan="2">
                    <h1 style="text-align: center;">
                        <asp:Label ID="lbl_titulo_PO" runat="server" Text="CONSULTAR" Width="50%" Style="text-align: center; font-weight: 700;" />
                    </h1>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="L_Item" runat="server" Text="Item:"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="TB_item" runat="server" ValidationGroup="accionar" Width="50px"
                        MaxLength="3"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_item"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="L_Actividad" runat="server" Text="Actividad:"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="TB_Actividad" runat="server" ValidationGroup="accionar" Width="250px"
                        MaxLength="150"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TB_Actividad"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="L_Metas" runat="server" Text="Metas:"></asp:Label>
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="TB_metas" runat="server" TextMode="MultiLine" ValidationGroup="accionar"
                        Columns="60" Rows="7"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TB_metas"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_inv_aprobar" Text="Aprobar:" runat="server" Visible="false" />
                </td>
                <td class="auto-style1">
                    <asp:DropDownList ID="dd_inv_aprobar" runat="server" AutoPostBack="true" Visible="false">
                        <asp:ListItem Text="Si" Value="1" />
                        <asp:ListItem Text="No" Value="0" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_inv_obvservaciones" Text="Observaciones:" runat="server" Visible="false" />
                </td>
                <td class="auto-style1">
                    <asp:TextBox ID="txt_inv_observaciones" runat="server" TextMode="MultiLine" Columns="25"
                        Rows="5" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style50">
                    <h1>
                        <asp:Label ID="Label1" runat="server" Text="REQUERIMIENTOS DE RECURSOS POR MES" Width="100%" Style="text-align: center;"></asp:Label>
                    </h1>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="P_Meses" runat="server" Width="100%">
                        <asp:Table ID="T_Meses" runat="server" CssClass="panelmeses">
                        </asp:Table>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClick="B_Acion_Click"
                        Text="Crear" />
                </td>
                <td class="auto-style1">
                    <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>