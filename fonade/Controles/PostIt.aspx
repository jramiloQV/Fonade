<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostIt.aspx.cs" Inherits="Fonade.Controles.PostIt" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/jquery-ui-1.10.3.min.js" rel="stylesheet" type="text/css" />
    
    <script src="../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>

    <style type="text/css">
        .auto-style1 {
            width: 730px;
            height: 585px;
            padding-top:0;
            margin-top:0;
        }
        .para {
            height:200px;
            width:300px;
            overflow:scroll;
        }
        .auto-style2 {
            width: 153px;
        }
        .Grilla {}
    </style>
    <script type="text/javascript">
        function closeWindow() {
            window.parent.opener.focus();
            window.close();
        }
    </script>
</head>
<body class="auto-style1">
    <form id="form1" runat="server">
    <div class="auto-style1">
        <asp:Panel ID="P_PostIt" runat="server" CssClass="auto-style1">
            <table class="auto-style1">
                 <thead>
                    <tr style="width:100%;">
                        <th colspan="3" style="background-color:#00468f; text-align:left; padding-left:50px; height:40px;">
                            <asp:Label ID="L_PostIt" runat="server" ForeColor="White" Text="AGENDAR POST IT" Width="260px"></asp:Label>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td colspan="3"  style="width:100%; text-align:right; height:30px;">
                        <asp:Label ID="L_Nombreusuario" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"  style="width:100%; height:10px;"><br /></td>
                </tr>
                <tr>
                    <td style="width:50%;">
                        
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="L_Tarea" runat="server" Text="Tarea:"></asp:Label>
                    </td>
                    <td style="width:50%;">
                        <asp:TextBox ID="TB_Tarea" runat="server" Width="200px" ValidationGroup="Grabar"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TB_Tarea" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="Grabar">Campo Requerido</asp:RequiredFieldValidator>
                        <br />
                    </td>
                </tr>
                <tr style="width:50%;">
                    <td rowspan="3" style="width:50%;">
                        <asp:Label ID="L_Para" runat="server" Text="Para:"></asp:Label>
                        <br />
                        <asp:ListBox ID="LB_Para" runat="server" DataTextField="Usuario" DataValueField="Id_Contacto" CssClass="para" SelectionMode="Multiple" ValidationGroup="Grabar"></asp:ListBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="LB_Para" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="Grabar">Seleccione Al Menos Un usuario</asp:RequiredFieldValidator>
                    </td>
                    <td class="auto-style2">
                        <asp:Label ID="L_Descripcion" runat="server" Text="Descripción:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TB_Descripcion" runat="server" TextMode="MultiLine" Width="200px" Height="50px" ValidationGroup="Grabar"></asp:TextBox>
                        <br />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="L_Fecha" runat="server" Text="Fecha:"></asp:Label>
                    </td>
                    <td>
                        <asp:Calendar ID="C_Fecha" runat="server" CssClass="Grilla" Height="114px" Width="202px">
                        </asp:Calendar>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">
                        <asp:Label ID="L_Email" runat="server" Text="Avisar por e-mail:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DD_Email" runat="server" ValidationGroup="Grabar">
                            <asp:ListItem Value="0" Selected="True">NO</asp:ListItem>
                            <asp:ListItem Value="1">SI</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width:100%;"><br /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td class="auto-style2">
                        <asp:Button ID="B_Grabar" runat="server" Text="Grabar" OnClick="B_Grabar_Click" ValidationGroup="Grabar" />
                    </td>
                    <td>
                        <asp:Button ID="C_Cerrar" runat="server" Text="Cerrar" OnClientClick="closeWindow();" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
