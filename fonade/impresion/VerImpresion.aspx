<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerImpresion.aspx.cs" Inherits="Fonade.impresion.VerImpresion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.printPage.js"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>
    <script src="../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
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
        <h1>
            <asp:Label ID="lblimpresion" runat="server" Text=""></asp:Label>
        </h1>
        <br />
        <br />
        <asp:Panel ID="pnlinfoproyecto" runat="server" CssClass="Grilla">
            <table>
                <thead>
                    <tr>
                        <th colspan="2">
                            PROYECTO
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td>
                        Nombre :
                    </td>
                    <td>
                        <asp:Label ID="lblnombreproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Institucion :
                    </td>
                    <td>
                        <asp:Label ID="lblinstitucionproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Subsector :
                    </td>
                    <td>
                        <asp:Label ID="lblsubsectorproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ciudad :
                    </td>
                    <td>
                        <asp:Label ID="lblciudadproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Recursos solicitados al fondo:
                    </td>
                    <td>
                        <asp:Label ID="lblrecursosproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha de Creación :
                    </td>
                    <td>
                        <asp:Label ID="lblfechacreacionproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Sumario :
                    </td>
                    <td>
                        <asp:Label ID="lblsumarioproyecto" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="pnltab" runat="server">
            <asp:Table ID="tbltab" runat="server" CssClass="Grilla">
            </asp:Table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
