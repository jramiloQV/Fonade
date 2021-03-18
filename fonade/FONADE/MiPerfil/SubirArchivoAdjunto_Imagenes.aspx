<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="SubirArchivoAdjunto_Imagenes.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.SubirArchivoAdjunto_Imagenes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 113px;
        }
        .auto-style2 {
            width: 253px;
        }
        .auto-style3 {
            height: 40px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="Panel1" runat="server">
        <br />
    </asp:Panel>
    <center>
        <asp:Panel ID="Panel2" runat="server" Height="165px" BackColor="White" Width="95%">
            <br />
            <br />
            <asp:Label ID="Label1" runat="server" Text="NUEVO DOCUMENTO"></asp:Label>
            <br />
            <table style="width: 60%;">
                <!-- <tr valign="top">
                <td align="right">Nombre:</td>
                <td align="left">
                    <asp:TextBox ID="NomDocumento" runat="server" MaxLength="256" Width="322px" EnableTheming="False" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblInformacion" runat="server"></asp:Label>
                </td>
            </tr>-->
                <tr valign="top">
                    <td class="auto-style1" align="right">Subir Archivo:</td>
                    <td class="auto-style2" colspan="3" align="left">
                        <asp:FileUpload ID="Archivo" runat="server" Width="422px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2" align="right" class="auto-style3">
                        <asp:Button ID="SubirArchivo" runat="server" Text="Enviar" OnClick="SubirArchivo_Click" CommandName="CertificacionEstudios" />
                        <asp:Button ID="Cancelar" runat="server" Text="Cancelar" OnClick="Cancelar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </center>
</asp:Content>
