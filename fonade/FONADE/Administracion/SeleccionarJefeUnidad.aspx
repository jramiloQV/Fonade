<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeleccionarJefeUnidad.aspx.cs"
    Inherits="Fonade.FONADE.Administracion.SeleccionarJefeUnidad" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FONDO EMPRENDER</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function MyButtonJava_Click() {
            history.go(-1);
            return false;
        }
    </script>
    <%--<script type="text/javascript">
        function validar() {


            var obj = document.getElementById('<%= lnk_0001.ClientID %>');

            var para = $(this).attr(obj);


            confirm()

        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div>
        <%--Información General--%>
        <h1>
            <asp:Label ID="lbl_enunciado" runat="server" Text="JEFE UNIDAD EMPRENDIMIENTO"></asp:Label>
        </h1>
        <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
            <table cellpadding="2">
                <tbody>
                    <tr>
                        <td class="TituloDestacados" colspan="2">
                            BUSCAR JEFE DE LA UNIDAD
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tipo Documento:
                        </td>
                        <td>
                            <asp:DropDownList ID="dd_listado_TipoIdentificacion" runat="server" AutoPostBack="true"
                                Width="151px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Número Documento:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_numIdentificacion" runat="server" MaxLength="100" Width="215px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_enviar_cambio_jefe_unidad" Text="Enviar" runat="server" OnClick="btn_enviar_cambio_jefe_unidad_Click" />
                            <ajaxToolkit:ConfirmButtonExtender ID="btn_enviar_cambio_jefe_unidad_ConfirmButtonExtender" runat="server" ConfirmText="" Enabled="True" TargetControlID="btn_enviar_cambio_jefe_unidad">
                            </ajaxToolkit:ConfirmButtonExtender>
                            <asp:HiddenField ID="hdf_depto_selected" runat="server" />
                            <asp:HiddenField ID="hdf_municipio_selected" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <%--Generar nuevo usuario.--%>
        <asp:Panel ID="pnl_new_user" runat="server" Visible="false">
            <table cellpadding="2">
                <tbody>
                    <tr>
                        <td class="TituloDestacados" colspan="2">
                            ADICIONAR JEFE DE LA UNIDAD
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Nombres
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Nombres" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Apellidos
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Apellidos" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-Mail:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Email" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" class="tituloDestacados">
                            Información de la Unidad
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Departamento:
                        </td>
                        <td>
                            <asp:DropDownList ID="dd_SelDpto2_Unidad" runat="server" AutoPostBack="true" OnSelectedIndexChanged="dd_SelDpto2_SelectedIndexChanged"
                                Width="179px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Ciudad:
                        </td>
                        <td>
                            <asp:DropDownList ID="dd_Ciudades_Unidad" runat="server" AutoPostBack="true" Width="75px" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Telefono:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Telefono_Unidad" runat="server" MaxLength="20" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Fax:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_fax_Unidad" runat="server" MaxLength="20" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Sitio Web:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_website_Unidad" runat="server" MaxLength="100" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2" class="tituloDestacados">
                            Datos Personales
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Cargo:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_NombreCargo_NEWUSER" runat="server" MaxLength="80" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Telefono:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Telefono_NEWUSER" runat="server" MaxLength="80" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            Fax:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Fax_NEWUSER" runat="server" MaxLength="80" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_crearJefeUnidad" Text="Crear" runat="server" OnClick="btn_crearJefeUnidad_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <%--Resultados de la búsqueda.--%>
        <asp:Panel ID="pnl_Resultado" runat="server" HorizontalAlign="Left" Visible="false">
            <table id="TB_JefeSeleccionable" runat="server" visible="false" cellpadding="2">
                <tbody>
                    <tr>
                        <td class="TituloDestacados" colspan="2">
                            SELECCIONAR EL JEFE DE LA UNIDAD.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnk_jefeSeleccionable" runat="server" OnClick="lnk_jefeSeleccionable_Click" />
                        </td>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_rol_jefe_seleccionable" runat="server" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="TB_Jefe_NO_seleccionable" runat="server" visible="false" cellpadding="2">
                <tbody>
                    <tr>
                        <td class="TituloDestacados" colspan="2">
                            SELECCIONAR EL JEFE DE LA UNIDAD
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnk_jefe_NO_seleccionable" runat="server" ForeColor="Black" />
                        </td>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <span>Rol:</span>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_rol_jefe_NO_seleccionable" runat="server" /><br />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lbl_usuarioEsRol" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                            <asp:LinkButton ID="lnk_btn_otraBusqueda" Text="Realizar otra búsqueda" runat="server"
                                OnClientClick="return MyButtonJava_Click()" Style="text-decoration: none;" ForeColor="Red" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
