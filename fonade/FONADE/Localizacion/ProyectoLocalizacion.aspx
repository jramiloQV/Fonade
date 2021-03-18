<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoLocalizacion.aspx.cs" Inherits="Fonade.FONADE.Localizacion.ProyectoLocalizacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/jquery-ui-1.10.3.min.js" rel="stylesheet" type="text/css" />

    <script src="../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../Scripts/common.js" type="text/javascript"></script>

    <style type="text/css">
        body {
            text-align: center;
            vertical-align: middle;
        }
    </style>

    <script type="text/javascript">
        function coordenadas() {
            x = event.clientX;
            y = event.clientY;

            confirm('El proyecto ha sido localizado en las coordenadas\n(' + x + ', ' + y + ')');
        }
    </script>

</head>
<body style="width: 600px; height: 480px;">
    <form id="form1" runat="server" style="width: 600px; height: 480px;">
        <div style="width: 600px; height: 480px;">
            <asp:ImageButton ID="imgbtn_mapa" runat="server" OnClick="imgbtn_mapa_Click" Width="500px" Height="400px"
                OnClientClick="return coordenadas()" />
        </div>
    </form>
</body>
</html>
