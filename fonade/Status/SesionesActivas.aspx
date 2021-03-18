<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SesionesActivas.aspx.cs" Inherits="Fonade.Status.SesionesActivas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="refresh" content="2" />


    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        Server start time:
    <asp:Label ID="lblFechaHoraServerStart" runat="server"></asp:Label><br />
        Total number of users since the Web server started:
    <asp:Label ID="lblTotalNumberOfUsers" runat="server"></asp:Label><br />
        Current number of users browsing the site:
    <asp:Label ID="lblCurrentNumberOfUsers" runat="server"></asp:Label><br />
    </form>
</body>
</html>
