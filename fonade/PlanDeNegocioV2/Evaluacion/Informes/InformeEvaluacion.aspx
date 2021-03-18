<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InformeEvaluacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.Informes.InformeEvaluacion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!--<link href="../../Styles/siteProyecto.css" rel="stylesheet" />-->
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
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
    <style type="text/css">
        table {
            width: 100%;
        }

        .celdaest {
            text-align: center;
        }

        .lectura {
            font-size: 8px;
            font-family: Arial;
        }

        .zebra tr:nth-child(odd) {
            background-color: #ccc;
        }

        .mainContainer {
            font-size: 8px !important;
        }
    </style>
</head>
<body onload="imprimir()" style="font-size: 8px; font-family: Tahoma">
    <div id="contentPrincipal">
        <%--Epílogo: "Información de la ventana".--%>
        <table border="0" cellspacing="0" cellpadding="2" width="100%" style="font-size: 8px">
            <tbody>
                <tr>
                    <td width="175" align="center" valign="baseline" bgcolor="#000000">
                        <asp:Label ID="Label1" Text="INFORME DE EVALUACIÓN" runat="server" ForeColor="White" />
                    </td>
                    <td>
                        <table border="0" cellspacing="0" cellpadding="1">
                            <tbody>
                                <tr>
                                    <td align="right" class="titulo">
                                        <asp:Label ID="lusuario" runat="server" ForeColor="Black" BackColor="White" Width="200px" />
                                    </td>
                                    <td align="right" class="titulo">
                                        <asp:Label ID="lblfecha" runat="server" ForeColor="Black" BackColor="White" Width="200px" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
                <tr bgcolor="#000000">
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
                <tr bgcolor="#CCCCCC">
                    <td colspan="2">&nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
        <%--Tabla dibujada.--%>
        <asp:Label ID="lbl_tr_emprendedores" Text="" runat="server" CssClass="lectura" />
    </div>
</body>
</html>
