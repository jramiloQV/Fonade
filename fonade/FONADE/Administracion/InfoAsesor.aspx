<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InfoAsesor.aspx.cs" Inherits="Fonade.FONADE.Administracion.InfoAsesor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .auto-style1 {
            width: 125px;
        }
    </style>
</head>
<body style="width:550px; height:250px;">
    <form id="form1" runat="server">
    <div style="width:550px; height:250px;">
        <h1>
            <label>INFORMACIÓN DE ASESOR</label>
        </h1>
        <br />
        <br />
        <table class="Grilla" style="width:100%;">
            <tr>
                <td class="auto-style1">Nombre:</td>
                <td><asp:Label ID="lblnombre" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style1">Correo Electrónico:</td>
                <td><asp:Label ID="lblcorreo" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style1">Número Telefónico</td>
                <td><asp:Label ID="lblnumero" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td colspan="2" style="text-align:center;">
                    <asp:Button ID="btncerrar" runat="server" Text="Cerrar" OnClientClick="window.close();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
