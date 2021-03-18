<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoIndicador.ascx.cs"
    Inherits="Fonade.Controles.evaluacion.CatalogoIndicador" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv='Content-Type' content='text/html; charset=iso-8859-1' />
    <meta http-equiv='CACHE-CONTROL' content='no-cache' />
    <meta http-equiv='Pragma' content='no-cache' />
    <meta http-equiv='Expires' content='-1' />
    <style type='text/css'>
        body
        {
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
    </style>
    <link href='StyleSheet.css' rel='stylesheet' type='text/css' />
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
    <form id="form1" runat="server" name='FrmIndicador' action='CatalogoIndicador.asp'
    method='Post' onsubmit='return validar()'>
    <div>
        <table width='565' border='0' align='center' cellspacing='0' cellpadding='0'>
            <tr>
                <td colspan='2' align='left'>
                    <img src='g/gifTransparente.gif' width='10' height='10'>
                </td>
            </tr>
            <tr>
                <td width='100%' align='center' valign='top'>
                    <table width='95%' border='0' cellspacing='0' cellpadding='2'>
                        <tr>
                            <td width='175' align='center' valign='baseline' bgcolor='#3D5A87' class='Blanca'>
                                INDICADOR
                            </td>
                            <td>
                                <table width='100%' border='0' cellspacing='0' cellpadding='1'>
                                    <tr>
                                        <td align='right' class='tituloDestacados'>
                                            <%= HttpContext.Current.Session["nombre"].ToString()%>
                                        </td>
                                        <td align='right' class='titulosCentro'>
                                            <%= DateTime.Now.ToShortDateString()%>
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
                                <table width='95%' border='0' cellspacing='0' cellpadding='0'>
                                    <tr>
                                        <td>
                                            <img src='g/gifTransparente.gif' width='8' height='8'>
                                        </td>
                                    </tr>
                                </table>
                                <table width='95%' border='0' cellspacing='0' cellpadding='3'>
                                    <tr valign='top'>
                                        <td colspan='2'>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr valign='top'>
                                        <td class='TitDestacado' align='Right'>
                                            <b>Descripción:</b>
                                        </td>
                                        <td class='TitDestacado'>
                                            <textarea class='boxes' rows="5" cols="50" name='Descripcion' id="txtDescripcion"
                                                runat="server"></textarea>
                                        </td>
                                    </tr>
                                    <tr valign='top'>
                                        <td class='TitDestacado' align='Right'>
                                            <b>Valor:</b>
                                        </td>
                                        <td width='167' align='left' colspan='3' class='TitDestacado'>
                                            <input class='Boxes' name='Valor' id="txtValor" size='15' maxlength='15' runat="server"
                                                onkeypress="return isNumberKey(event)" />
                                        </td>
                                    </tr>
                                    <tr valign='top'>
                                        <td class='TitDestacado' align='Right'>
                                            <b>Tipo:</b>
                                        </td>
                                        <td width='167' align='left' colspan='3' class='TitDestacado'>
                                            <asp:DropDownList runat="server" ID="dpl_tipo">
                                                <asp:ListItem Value="$" Text="Dinero" />
                                                <asp:ListItem Value="%" Text="Porcentaje" />
                                                <asp:ListItem Value="#" Text="Numérico" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr valign='top'>
                                        <td colspan='4' align='right' class='TitDestacado'>
                                            <input type='button' name='Cerrar' value='Cerrar' class='Boton' onclick='javascript:closeWindow()' />
                                            <input name='Accion' type='submit' class='Boton' value='Crear' runat="server" onclick="btn_crear_Click"
                                                id="btn_crear" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table width='95%' border='0' cellspacing='0' cellpadding='0'>
                        <tr>
                            <td bgcolor='#3D5A87'>
                                <img src='g/gifTransparente.gif' width='7' height='7'>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
    </div>
    </form>
</body>
</html>
