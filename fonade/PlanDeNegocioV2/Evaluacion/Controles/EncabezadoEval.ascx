<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EncabezadoEval.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.Controles.EncabezadoEval" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<table style="width: 100%">
    <tbody>
        <tr>
            <td style="width: 145px;">ÚLTIMA ACTUALIZACIÓN:&nbsp;
            </td>
            <td style="text-align: left">
                <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
            </td>
            <td style="text-align: right">
                <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                    runat="server" TextAlign="Left" />
                &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                    ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="false" />
            </td>
        </tr>
    </tbody>
</table>

