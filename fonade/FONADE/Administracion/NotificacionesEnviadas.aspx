<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NotificacionesEnviadas.aspx.cs" Inherits="Fonade.FONADE.Administracion.NotificacionesEnviadas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        table {
            width:100%;
        }
    </style>
</head>
<body style="width:750px;height:auto;">
    <form id="form1" runat="server">
    <div style="width:750px;height:auto;">
        <h1>
            <label>NOTIFICACIONES ENVIADAS</label>
        </h1>
        <br />
        <br />

        <asp:GridView ID="gvnotificaciones" runat="server" CssClass="Grilla" Width="100%" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField HeaderText="Emprendedor" DataField="Nombre" />
                <asp:BoundField HeaderText="Emprendedores CC" DataField="NOMCC" />
                <asp:BoundField HeaderText="ROL" DataField="ROL" />
                <asp:BoundField HeaderText="Mensaje" DataField="Email" />
                <asp:BoundField HeaderText="Estado" DataField="NOMESTADO" />
                <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
            </Columns>
        </asp:GridView>

        <br />
        <br />

        <table>
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
