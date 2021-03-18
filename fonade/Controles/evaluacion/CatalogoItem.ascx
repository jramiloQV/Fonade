<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoItem.ascx.cs"
    Inherits="Fonade.Controles.evaluacion.CatalogoItem" %>
<html>
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <meta http-equiv="CACHE-CONTROL" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="-1" />
    <style type="text/css">
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
        .Boton
        {
            height: 26px;
        }
    </style>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    
    </script>
</head>
<body>
    <form runat="server" onsubmit='return validar()'>
    <table width="565" border="0" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td colspan="2" align="left">
                <img src="g/gifTransparente.gif" width="10" height="10">
            </td>
        </tr>
        <tr>
            <td width="100%" align="center" valign="top">
                <table width='95%' border='0' cellspacing='0' cellpadding='2'>
                    <tr>
                        <td width='175' align='center' valign='baseline' bgcolor='#3D5A87' class='Blanca'>
                            <asp:Literal runat="server" ID="lt_item" Visible="false"></asp:Literal>
                        </td>
                        <td>
                            <table width='100%' border='0' cellspacing='0' cellpadding='1'>
                                <tr>
                                    <td align='right' class='tituloDestacados'>
                                    </td>
                                    <td align='right' class='titulosCentro'>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr bgcolor='#3D5A87'>
                        <td colspan='2'>
                            <img src='g/gifTransparente.gif' width='2' height='2'>
                        </td>
                    </tr>
                    <tr bgcolor='#CC0000'>
                        <td colspan='2'>
                            <img src='g/gifTransparente.gif' width='1' height='1'>
                        </td>
                    </tr>
                </table>
                <table width='95%' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF'>
                    <tr>
                        <td align='center' valign='top' width='98%'>
                            <table width='95%' border='0' align="center" cellspacing='0' cellpadding='3'>
                                <tr valign="top">
                                    <td class="TitDestacado">
                                        <b>Item:</b>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado">
                                        <input name="Item" id="txt_nombreitem" runat="server" class="boxes" size="80" maxlength="255" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado">
                                        <b>Escala:</b>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td>
                                        <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                            <tr class='Titulo' bgcolor='#3D5A87'>
                                                <td align='left' class='tituloTabla'>
                                                    Texto
                                                </td>
                                                <td align='left' width='15%' class='tituloTabla'>
                                                    Puntaje
                                                </td>
                                            </tr>
                                            <tr class='Titulo'>
                                                <td align='left'>
                                                    <input class="boxes" id="txt_texto" runat="server" size="60" maxlength="255" />
                                                </td>
                                                <td align='left'>
                                                    <input class="boxes" id="txt_puntaje" runat="server" size="3" maxlength="3" onkeypress="return isNumberKey(event)" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="4" align="right" class="TitDestacado">
                                        <asp:Button ID="btn_crear" CssClass="Boton" Text="Crear" runat="server" OnClick="btn_crear_Click" />
                                        <input type="button" name="Cerrar" value='Cerrar' class='Boton' onclick='javascript:closeWindow()' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td bgcolor="#3D5A87">
                            <img src="g/gifTransparente.gif" width="7" height="7">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
