<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoItem.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <table class="auto-style1">
            <thead>
                <tr>
                    <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">

                        <asp:Label ID="L_NuevoItem" runat="server" ForeColor="White" Text="NUEVO ITEM"></asp:Label>

                    </th>
                </tr>
            </thead>
            
            <tr>
                <td colspan="2" style="padding-right:50px; text-align:right;">
                    <asp:Label ID="L_Titulo" runat="server" Text="Prueba Gerente Evaluador" ForeColor="#00468F" Font-Bold="True"></asp:Label>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="L_Fecha" runat="server" Text="" ForeColor="#00468F" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="L_Item" runat="server" Font-Bold="True" Text="Item:"></asp:Label>
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="TB_NomItem" runat="server" Width="50%" ValidationGroup="crear"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_NomItem" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">Campo Item Requerido</asp:RequiredFieldValidator>
                    <br />
                    <br />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <table class="Grilla" cellspacing="0" cellpadding="0">
                        <thead >
                            <tr>
                                <th style="text-align:center;"><asp:Label ID="L_TituloTexto" runat="server" Text="Texto"></asp:Label></th>
                                <th style="text-align:center;"><asp:Label ID="L_TituloPuntaje" runat="server" Text="Puntaje"></asp:Label></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Texto1" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Puntaje1" runat="server" Width="50px" ValidationGroup="crear"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Puntaje1" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="[0-9]*" ValidationGroup="crear">Formato Incorrecto</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Texto2" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Puntaje2" runat="server" Width="50px" ValidationGroup="crear"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="Puntaje2" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="[0-9]*" ValidationGroup="crear">Formato Incorrecto</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Texto3" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Puntaje3" runat="server" Width="50px" ValidationGroup="crear"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="Puntaje3" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="[0-9]*" ValidationGroup="crear">Formato Incorrecto</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Texto4" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Puntaje4" runat="server" Width="50px" ValidationGroup="crear"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="Puntaje4" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="[0-9]*" ValidationGroup="crear">Formato Incorrecto</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="Texto5" runat="server" Width="250px"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="Puntaje5" runat="server" Width="50px" ValidationGroup="crear"></asp:TextBox>
                                    <br />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="Puntaje5" ErrorMessage="RegularExpressionValidator" ForeColor="Red" ValidationExpression="[0-9]*" ValidationGroup="crear">Formato Incorrecto</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <br />

                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <table style="width:50%; text-align:center">
                        <tr>
                            <td><asp:Button ID="B_Crear" runat="server" Text="Crear" OnClick="B_Crear_Click" ValidationGroup="crear" /></td>
                            <td><asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click" /></td>
                        </tr>
                    </table>
                    <br />
                </td>
            </tr>
            </table>
        
    </div>
    </form>
</body>
</html>
