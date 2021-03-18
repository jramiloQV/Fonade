<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoOperacionCompras.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.ProyectoOperacionCompras" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head runat="server">
    <title>FONDO EMPRENDER - Costos de Insumos</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/ScriptsGenerales.js"></script>
    <style type="text/css">
        body {
            background-color: white;
        }

        .sinlinea {
            border: none;
            border-collapse: collapse;
            border-color: none;
        }
        .FondoCelda{
            background-color: white !important;
            color: #00468f;
            font-weight: bold;
        }
        .FondoCelda td{
            font-size:14px !important;
            padding-top:10px;
        }
        .FondoCelda2{
            /*background-color: white !important;*/
            color: #00468f;
            font-weight: bold;
        }
        .alineacion td:nth-child(n+4)
        {
            text-align:right;
        }
    </style>
    <script type="text/ecmascript">
        function url()
        {
            open("../Ayuda/Mensaje.aspx", "Consumos por Unidad de Producto", "width=500,height=200");
        }
        function OpenPage(strPage) {
            window.open(strPage, null, 'status:false;dialogWidth:900px;dialogHeight:1500px')
        }

        window.onload = function () {
            Realizado();
        };

        function Realizado() {
            var chk = document.getElementById('chk_realizado')
            var rol = document.getElementById('txtIdGrupoUser').value;
            if (rol != '5') {
                if (chk.checked) {
                    chk.disabled = true;
                    document.getElementById('btn_guardar_ultima_actualizacion').setAttribute("hidden", 'true');
                }
            }
        }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <table>
            <tbody>
                <tr>
                    <td>&nbsp;
                    </td>
                    <td>ULTIMA ACTUALIZACIÓN:&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                        &nbsp;
                    <asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server" ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <table id="tabla_docs" runat="server" width="95%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td style="width: 80%"></td>
                <td style="width: 20%">
                    <table style="width: 52px; border: 0">
                        <tr style="text-align: center;">
                            <td style="width: 50px;">
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server" ToolTip="Nuevo Documento" OnClick="ImageButton1_Click" />
                            </td>
                            <td style="width: 138px;">
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../../Images/icoClip2.gif" runat="server" ToolTip="Ver Documentos" OnClick="ImageButton2_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 95%">
            <tbody>
                <tr>
                    <td colspan="4">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 80%">
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Consumos por Unidad de Producto', texto: 'CostosInsumos'});">
                                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento" />
                                            Consumos por Unidad de Producto:
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div id="div_post_it_1" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Consumos por Unidad de Producto"
                                            _txtTab="1" _mostrarPost="false"/>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlContenido" runat="server">
                            <asp:Table ID="tbl" runat="server" CssClass="Grilla">

                            </asp:Table>
                            <br />
                            <div style="width:100%; text-align:right; display:inline-block">
                                <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <%--<tr id="tr_buttons" runat="server" visible="false">
                    <td>
                        <asp:ImageButton ID="img_AddInsumo" ImageUrl="~/Images/icoAdicionarUsuario.gif" runat="server" AlternateText="img" />
                        &nbsp;
                        <asp:LinkButton ID="lnkBtn_AddInsumo" Text="Adicionar Insumo al producto o servicio" runat="server" OnClick="lnkBtn_AddInsumo_Click" target="_Blank" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btm_guardarCambios" Text="Guardar" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnl_tablas" runat="server" Width="98%" border="0" cellspacing="1" cellpadding="4" />
                    </td>
                </tr>--%>
            </tbody>
        </table>
        <br />
        <br />
         <input id="hidInsumo" name="hidInsumo" type="hidden" value="1" />
    </form>
</body>
</html>
