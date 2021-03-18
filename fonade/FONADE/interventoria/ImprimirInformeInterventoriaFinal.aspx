<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirInformeInterventoriaFinal.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.ImprimirInformeInterventoriaFinal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resultado de Impresión</title>
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
