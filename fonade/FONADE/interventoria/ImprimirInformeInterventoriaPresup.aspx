<%@ Page Title="FONDO EMPRENDER" Language="C#" AutoEventWireup="true" CodeBehind="ImprimirInformeInterventoriaPresup.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.ImprimirInformeInterventoriaPresup" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>FONDO EMPRENDER</title>
    <%--<link href="../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>
    <script src="../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.printPage.js"></script>--%>
    <script type="text/javascript">

        function imprimir() {

            var divToPrint = document.getElementById('contentPrincipal');
            var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 1000);
        }

    </script>
</head>
<body onload="imprimir()">
    <form id="form1" runat="server">
    <div id="contentPrincipal">
        <asp:Label ID="lbl_informe" Text="" runat="server" />
    </div>
    </form>
</body>
</html>
