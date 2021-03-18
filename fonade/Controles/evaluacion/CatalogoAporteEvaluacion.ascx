<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogoAporteEvaluacion.ascx.cs"
    Inherits="Fonade.Controles.evaluacion.CatalogoAporteEvaluacion" %>
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
    </style>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css">
    <script type="text/javascript">

        function validate_registry() {

            if (document.getElementById("<%=txt_nombre.ClientID%>").value == "" || document.getElementById("<%=txt_nombre.ClientID%>").value == "Ingrese su nombre") {
                document.getElementById("<%=txt_nombre.ClientID%>").value = "Ingrese su nombre";
                document.getElementById("<%=txt_nombre.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txt_detalle.ClientID%>").value == "" || document.getElementById("<%=txt_detalle.ClientID%>").value == "Ingrese su Descripción") {
                document.getElementById("<%=txt_detalle.ClientID%>").value = "Ingrese su Descripción";
                document.getElementById("<%=txt_detalle.ClientID%>").focus();
                return false;
            }

            if (document.getElementById("<%=txt_recomendado.ClientID%>").value == "" || document.getElementById("<%=txt_recomendado.ClientID%>").value == "Ingrese su número de solicitud") {
                document.getElementById("<%=txt_recomendado.ClientID%>").value = "Ingrese su solicitud";
                document.getElementById("<%=txt_recomendado.ClientID%>").focus();
                return false;
            }



            return true;
        }

    </script>
    <script language="Javascript">
          <!--
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        //-->
    </script>
</head>
<body>
    <form runat="server" method="Post">
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
                            APORTE
                        </td>
                        <td>
                            <table width='100%' border='0' cellspacing='0' cellpadding='1'>
                                <tr>
                                    <td align='right' class='tituloDestacados'>
                                        <asp:Literal ID="lt_session" runat="server"></asp:Literal>
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
                            <table width='98%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td>
                                        <img src='g/gifTransparente.gif' width='8' height='8'>
                                    </td>
                                </tr>
                            </table>
                            <table width='98%' border='0' cellspacing='0' cellpadding='3'>
                                <tr valign="top">
                                    <td colspan="2">
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="Right">
                                        <b>Nombre:</b>
                                    </td>
                                    <td class="TitDestacado" colspan="3">
                                        <input class='boxes' name='Nombre' size="70" maxlength='255' runat="server" id="txt_nombre" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="Right">
                                        <b>Detalle:</b>
                                    </td>
                                    <td class="TitDestacado" colspan="3">
                                        <textarea class='Boxes' name='Detalle' rows='6' cols='50' runat="server" id="txt_detalle"></textarea>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="Right">
                                        <b>Solicitado:</b>
                                    </td>
                                    <td width='167' align="left" colspan="3" class="TitDestacado">
                                        <input class='soloLectura' name='Solicitado' id="txt_solicitado" size='15' maxlength='20'
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="Right">
                                        <b>Recomendado:</b>
                                    </td>
                                    <td width='167' align="left" colspan="3" class="TitDestacado">
                                        <input class='Boxes' name='Recomendado' runat="server" id="txt_recomendado" size='15'
                                            maxlength='20' onkeypress="return isNumberKey(event)" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td class="TitDestacado" align="Right">
                                        <b>Tipo de Aporte:</b>
                                    </td>
                                    <td width='167' align='left' colspan='3' class='TitDestacado'>
                                        <asp:DropDownList ID="dpl_tipo" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan='4' align='center' class='TitDestacado'>
                                        <asp:Button runat="server" ID="btn_crearaporte" OnClick="btn_crearaporte_Click" OnClientClick="if(!validate_registry(this)) return false;" />
                                        <input type="button" class='Boton' name="Cerrar" value='Cerrar' onclick='javascript:closeWindow()' />
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
