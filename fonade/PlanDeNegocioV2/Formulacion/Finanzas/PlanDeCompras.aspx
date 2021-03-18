<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlanDeCompras.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.PlanDeCompras" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head runat="server">
    <title>FONDO EMPRENDER - Costos de Insumos</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js"></script>
    <style type="text/css">
        #tc_proyectos_body {
            height : 100% !important;
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
            //Realizado();
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
        <table runat="server" visible="false">
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
        
         <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
        <div style="position: relative; left: 720px; width: 160px;">            
            <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="PlanCompras"/>                
        </div>

        <br />
         <div style="text-align: center">
            <h1> VIII - Estructura financiera </h1>                            
        </div>
        <table style="width: 95%">
            <tbody>
                <tr>
                    <td colspan="4">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 80%">
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Consumos por Unidad de Producto', texto: 'CostosInsumos'});">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento" />
                                            Consumos por Unidad de Producto:
                                        </div>
                                    </div>
                                </td>
                                <td>
                                 
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
            </tbody>
        </table>
        <br />
        <br />
         <input id="hidInsumo" name="hidInsumo" type="hidden" value="1" />
    </form>
</body>
</html>
