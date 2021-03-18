<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoCumpliInformeInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoCumpliInformeInter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONADE - Crear Documento</title>
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
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="ContentInfo">
        <table>
            <tbody>
                <tr valign="top">
                    <td colspan="2">
                        <span>NUEVO CUMPLIMIENTO</span>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <b>Nombre:</b>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="NomItem" runat="server" Width="269px" MaxLength="256" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4" align="right">
                        <asp:HiddenField ID="hdf_CodInforme" runat="server" />
                        <asp:HiddenField ID="hdf_CodItem" runat="server" />
                        <asp:HiddenField ID="hdf_CodEmpresa" runat="server" />
                        <asp:Button ID="btn_Accion" Text="Crear" runat="server" OnClick="btn_Accion_Click" />
                        <asp:Button ID="btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="javascript:window.close()"
                            OnClick="btn_Cerrar_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    </form>
</body>
</html>
