<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Emergente.Master" CodeBehind="UploadModeloFinancieroEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.UploadModeloFinancieroEvaluador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 113px;
        }
        .auto-style2 {
            width: 253px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="Panel1" runat="server">
            

            <br />
    </asp:Panel>
    
    <asp:Panel ID="Panel2" runat="server" Height="165px" >
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Subir Modelo Financiero evaluador. (Solo se permite subir el archivo el modelo descargado previamente con el mismo nombre.)"></asp:Label>
        <br />
        <asp:Label ID="Error" Visible="False" runat="server" Text="Error" ForeColor="Red"></asp:Label>
        <br />
        <table style="width:50%;">
            <tr  valign="top">
                <td class="auto-style1" align="right">Subir Archivo:</td>
                <td class="auto-style2" colspan="3" align="left">
                    <asp:FileUpload ID="Archivo" runat="server" Width="422px" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2" align="right">
                    <asp:Button ID="SubirArchivo" runat="server" Text="Enviar" OnClick="SubirArchivo_Click" CommandName="CertificacionEstudios"/>
                    <asp:Button ID="Cancelar" runat="server" Text="Cancelar" OnClick="CancelarEvent" />
                </td>
            </tr>
        </table>                
    </asp:Panel>
</asp:Content>
