<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CargarArchivos.ascx.cs"
    Inherits="Fonade.Controles.CargarArchivos" %>
    
<asp:Panel ID="pnlCargue" runat="server" Visible="false">
    Carga Archivo
    <table>
        <tr>
            <td>
                Seleccione el archivo:
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblErrorDocumento" runat="server" CssClass="failureNotification"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="LEFT">
                <asp:FileUpload ID="fuArchivo" runat="server" Width="200px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnSubirDocumento" runat="server" Text="Cargar Archivo" OnClick="btnSubirDocumento_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
            </td>
        </tr>
        <tr>
            <td>
                Solo se debe subir el archivo de Formatos Financieros descargado desde esta pagina.
            </td>
        </tr>
    </table>
</asp:Panel>
