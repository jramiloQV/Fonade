<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoInformeFinalEditarItem.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoInformeFinalEditarItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONADE - Editar Item Informe Final</title>
    <style type="text/css">
        .auto-style1
        {
            width: 80%;
            margin: 0px auto;
            text-align: center;
        }
        .panelmeses
        {
            margin: 0px auto;
            text-align: center;
        }
        .auto-style2
        {
            height: 23px;
        }
        #form1 > div.ContentInfo > table > tbody > tr:nth-child(4) 
        {
            height:200px;
        }
        #form1 > div.ContentInfo > table > tbody > tr:nth-child(5) > td 
        {
            padding:10px;
        }
        #form1 > div.ContentInfo > table > tbody > tr:nth-child(1) > td > p 
        {
            font-size:14pt;
        }
        #form1 > div.ContentInfo > table > tbody > tr:nth-child(2) > td
        {
            display:none;
        }
        #lbl_cumplimientoSeleccionado 
        {
            font-size:14px;
        }
        #form1 > div.ContentInfo > table 
        {
            width:100%;
        }
        #TextoItem 
        {
            width:99%;
            height:100%;
        }
        #form1 > div.ContentInfo 
        {
            margin: 10px;
            width: 99% !important;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="ContentInfo">
        <table border="0" cellspacing="0" cellpadding="3">
            <tr>
                <td colspan="2">
                    <!-- class="auto-style2"-->
                    <p style="text-align: left;">
                        <asp:Label ID="lbl_enunciado" runat="server" Text="Editar Item Informe Final" Width="40%" />
                    </p>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr valign="top">
                <td align="left">
                    <b>
                        <asp:Label ID="lbl_cumplimientoSeleccionado" Text="" runat="server" /></b>
                </td>
            </tr>
            <tr valign="top">
                <td align="left">
                    <asp:TextBox ID="TextoItem" runat="server" TextMode="MultiLine" Columns="60" Rows="5" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="4" align="right">
                    <asp:HiddenField ID="hdf_CodInforme" runat="server" />
                    <asp:HiddenField ID="hdf_CodItem" runat="server" />
                    <asp:HiddenField ID="hdf_CodEmpresa" runat="server" />
                    <asp:Button ID="btn_Accion" Text="Actualizar" runat="server" OnClick="btn_Accion_Click" />
                    <asp:Button ID="btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="javascript:window.close()"
                        OnClick="btn_Cerrar_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
