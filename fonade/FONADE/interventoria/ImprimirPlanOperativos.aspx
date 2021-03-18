<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirPlanOperativos.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.ImprimirPlanOperativos" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Resultado de impresión</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" />
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
    <div id="contentPrincipal">
        <table style="width:100%; border:0" >
            <tbody>
                <tr>
                    <td style="width:50%; text-align:center; vertical-align:baseline; background-color:#000000; color: White;">
                        <b>PLAN OPERATIVO</b>
                    </td>
                    <td style="width:10%; text-align:right">
                        &nbsp;
                    </td>
                    <td style= "width:40%; text-align:left">
                        <asp:Label ID="L_Fecha" runat="server" />
                    </td>
                </tr>
                <tr style="background-color:#000000">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr style="background-color:#CCCCCC">
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Label ID="lbl_cuerpo" runat="server" />
    </div>
</body>
</html>
