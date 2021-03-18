<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostosDeProduccion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.CostosDeProduccion" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head id="Head1" runat="server">
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <title>Fondo emprender</title>
</head>
<body>
    <script type="text/javascript">
        window.onload = function () {            
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

    <% Page.DataBind(); %>
    <form id="form1" runat="server">      
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
                    <td style="width: 100px;"></td>
                    <td>
                        <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left"
                            Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                        &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                            ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click"
                            Visible='False' />
                    </td>
                </tr>
            </tbody>
        </table>
        <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
        <div style="position: relative; left: 705px; width: 160px;">
            <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="CostosProduccion" />
        </div>


        <div style="text-align: center">
            <h1>VIII - Estructura financiera </h1>
        </div>
        <div style="margin-top: 10px;">
            <div class="help_container">
                <div onclick="textoAyuda({titulo: 'Costos de Producción',texto: 'CostosProduccion'});">
                    <img src="../../../Images/imgAyuda.gif" border="0" alt="help_CostosProduccion" alt="img" />
                </div>
                <div>
                    Costos de Producción:
                </div>
            </div>
            <br />
            <br />
            <asp:Panel ID="pnl_tablas_costos" runat="server" Width="100%" Height="100%" />
            <br />
            <br />
            <asp:Button ID="btm_guardarCambios" runat="server" Text="Guardar" OnClick="btm_guardarCambios_Click" />
            <br />
            <br />
            <br />
            <br />
        </div>
    </form>
</body>
</html>
