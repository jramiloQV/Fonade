<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoIndicadorGestion.ascx.cs"
    Inherits="Fonade.Controles.CatalogoIndicadorGestion" %>
<html>
<head>
    <title>Crear Indicador de Gestion - Administrar Indicador de Gestion</title>
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
</head>
<body>
    <table width='565' border='0' align='center' cellspacing='0' cellpadding='0'>
        <tr>
            <td colspan='2' align='left'>
                <img src='g/gifTransparente.gif' width='10' height='10'>
            </td>
        </tr>
        <tr>
            <td width='100%' align='center' valign='top'>
                <form name='FrmIndicador' action='CatalogoIndicadorGestion.asp' method='Post' onsubmit='return validar();'>
                <table width='95%' border='0' cellspacing='0' cellpadding='2'>
                    <tr>
                        <td width='175' align='center' valign='baseline' bgcolor='#3D5A87' class='Blanca'>
                            INDICADOR
                        </td>
                        <td>
                            <table width='100%' border='0' cellspacing='0' cellpadding='1'>
                                <tr>
                                    <td align='right' class='tituloDestacados'>
                                        <asp:Literal ID="lt_nombre_session" runat="server"></asp:Literal>
                                    </td>
                                    <td align='right' class='titulosCentro'>
                                        <%= DateTime.Now.ToShortDateString() %>
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
                                <tr>
                                    <td valign='top' class='TitDestacado' align='Right'>
                                        <b>Aspecto:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <textarea class='Boxes' name='Aspecto' cols='50' rows='5' runat="server" id="txt_aspecto"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='TitDestacado' align='Right'>
                                        <b>Fecha de Seguimiento:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <input class='Boxes' name='Fecha' id="txt_fecha" runat="server" maxlength='60' size='30' />
                                    </td>
                                </tr>
                                <tr>
                                    <td class='TitDestacado' align='Right'>
                                        <b>Tipo Indicador:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <asp:DropDownList runat="server" ID="dpl_tipoindicador">
                                            <asp:ListItem Value="1" Text="Gestion" />
                                            <asp:ListItem Value="2" Text="Cualitativo y de Cumplimiento" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign='middle' class='TitDestacado' align='Right'>
                                        <b>Indicador:</b>
                                    </td>
                                    <td align='left' colspan='3' class='TitDestacado'>
                                        <table border='0'>
                                            <tr>
                                                <td>
                                                    <input class='Boxes' name='Numerador' id="txt_numerador" runat="server" maxlength='100'
                                                        size='50' />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td bgcolor='#000000'>
                                                    <img name="imagen" src='g/gifTransparente.gif' height='1' />
                                                </td>
                                                <tr>
                                                    <td>
                                                        <input class='Boxes' name='Denominador' type="text" runat="server" id="txt_denominador"
                                                            maxlength='100' size='50' />
                                                    </td>
                                                </tr>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign='top' class='TitDestacado' align='Right'>
                                        <b>Descripción del indicador:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <textarea class='Boxes' name='Descripcion' cols='50' rows='5' runat="server" id="txt_descripcion"></textarea>
                                    </td>
                                </tr>
                                <tr>
                                    <td class='TitDestacado' align='Right'>
                                        <b>Rango Aceptable:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <input class='Boxes' name='RangoAceptable' id="txt_rangoaceptable" runat="server"
                                            maxlength='3' size='5' /><span>%</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <img src='g/gifTransparente.gif' height='10' />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" align='center'>
                                        <asp:Button ID="btn_accioncatalogo" CssClass="boton" runat="server" OnClick="btn_accioncatalogo_Click" />
                                        <input type='button' class='boton' value='Cerrar' onclick='closeWindow();' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width='95%' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td bgcolor='#3D5A87'>
                            <img src='g/gifTransparente.gif' width='7' height='7' />
                        </td>
                    </tr>
                </table>
                </form>
            </td>
        </tr>
    </table>
    <br>
</body>
</html>
