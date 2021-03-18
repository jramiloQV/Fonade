<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Encabezado.ascx.cs" Inherits="Fonade.Controles.Encabezado" %>
<div id="divEncabezado">
    <table width="700px">
        <tbody>
            <tr>
                <td style="width: 10%; text-align: right;">ÚLTIMA ACTUALIZACIÓN: </td>
                <td style="width: 54%; text-align: left;">
                    <asp:Label ID="lblUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" />
                    &nbsp;&nbsp;
                    <asp:Label ID="lblFechaUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" /></td>
                <td style="width: 26%; text-align: right;">
                    <asp:CheckBox ID="chkEsRealizado" runat="server" TextAlign="Left" Text="MARCAR COMO REALIZADO:" />
                </td>
                <td style="width: 10%; text-align: left;">
                    <asp:Button ID="btnUpdateTab" Text="Guardar" runat="server" ToolTip="Guardar" OnClick="btnUpdateTab_Click" Visible="false" /></td>
            </tr>
        </tbody>
    </table>
    <table id="tabla_docs" runat="server" width="700px" border="0" cellspacing="0"
        cellpadding="0">
        <tr>
            <td style="text-align: right;">
                <table style="width: 52px; border-collapse: separate; border-spacing: 0px; border-collapse: collapse; border-spacing: 0;" align="right">
                    <tr style="text-align: center;">
                        <td style="width: 50px;">
                            <asp:ImageButton ID="BtnNuevoDocumento" ImageUrl="~/Images/icoClip.gif" runat="server" ToolTip="Nuevo Documento" OnClick="BtnNuevoDocumento_Click" CausesValidation="False" />
                        </td>
                        <td style="width: 138px;">
                            <asp:ImageButton ID="BtnVerDocumentos" ImageUrl="~/Images/icoClip2.gif" runat="server" ToolTip="Ver Documentos" OnClick="BtnVerDocumentos_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>

