<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoVentasTMP.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.CatalogoVentasTMP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ventas</title>
    <style type="text/css">
    body,html{
            background-color:#fff !important;
            background-image:none !important;
        }
        .auto-style1
        {
            width: 90%;
            margin: 0px auto;
            text-align: center;
        }
        .panelmeses
        {
            margin: 0px auto;
            text-align: center;
        }
        .auto-style2
        {
            height: 23px;
        }
        .panelmeses tr:nth-child(n+2) td:nth-child(14){
            text-align:right;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
    <script language="javascript">
        window.onload = zoom;
        function zoom()
        {
          //self.moveTo(50,50);
          //self.resizeTo(850, 400);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="auto-style1">
            <tr>
                <td colspan="2" class="style50" style="text-align: left;font-size:16px;background-color:#000066;color:#fff;">
                    <asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White" Text="" Width="40%" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center;"><br>
                    <asp:Label ID="L_Item" runat="server" Text="Nombre:"></asp:Label>
                </td>
                <td style="text-align: left;"><br>
                    <asp:TextBox ID="TB_Item" runat="server" ValidationGroup="accionar" Width="250px"
                        MaxLength="150"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TB_Item"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">El nombre es requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_inv_aprobar" Text="Aprobar:" runat="server" Visible="false" />
                </td>
                <td style="text-align: left">
                    <asp:DropDownList ID="dd_inv_aprobar" runat="server" Visible="false" >
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_inv_obvservaciones" Text="Observaciones:" runat="server" Visible="false" />
                </td>
                <td style="text-align: left">
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
                <td colspan="2" class="auto-style2">
                    <asp:Label ID="Label1" runat="server" BackColor="#000066" ForeColor="White" Text="REQUERIMIENTOS DE RECURSOS POR MES"
                        Width="100%"></asp:Label>
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
            <tr style="text-align: right;">
                <td colspan="2"><br>
                    <asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClick="B_Acion_Click" />
                    <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
