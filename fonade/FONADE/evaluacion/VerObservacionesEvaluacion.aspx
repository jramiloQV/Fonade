<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerObservacionesEvaluacion.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.VerObservacionesEvaluacion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
        .clasemas
        {
            color: #00468f;
        }
        .fondo
        {
            background-color: #00468f;
            color: white;
        }
        .panelmeses
        {
            margin: 0px auto;
            text-align: left;
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
    <div>
        <table class="auto-style1">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center;">
                        <asp:Panel ID="P_Observaciones" runat="server">
                            <asp:Table ID="T_Observaciones" runat="server" CssClass="panelmeses">
                            </asp:Table>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
