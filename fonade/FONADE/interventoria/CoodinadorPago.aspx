<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CoodinadorPago.aspx.cs" Inherits="Fonade.FONADE.interventoria.CoodinadorPago" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
     <style type="text/css">
         table {
             width: 100%;
         }
     </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
    <body>
        <form id="form1" runat="server">
    <h1>
        <label>
        SOLICITUD DE PAGO</label> </h1>
    <br />
    <br />
    <table class="Grilla">
        <tr>
            <td>Número Solicitud</td>
            <td><asp:Label ID="lblnumsolicitud" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Número Proyecto</td>
            <td><asp:Label ID="lblnumproyecto" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Nombre Proyecto</td>
            <td><asp:Label ID="lblnomproyecto" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Valor Solicitud</td>
            <td><asp:Label ID="lblvalorsolicitud" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Concepto Solicitud</td>
            <td><asp:Label ID="lblconceptosolicitud" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Fecha Solicitud</td>
            <td><asp:Label ID="lblfechasolicitud" runat="server" Text=""></asp:Label></td>
        </tr>
        <tr>
            <td>Archivos Adjuntos</td>
            <td><asp:Panel ID="pSocios" runat="server">
                                <asp:Table ID="t_table" runat="server">

                                </asp:Table>
                            </asp:Panel></td>
        </tr>
    </table>
            </form>
    </body>
</html>