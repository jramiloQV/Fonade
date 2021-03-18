<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoAnexosV2.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Anexos.CatalogoAnexosV2" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/CtrlAnexo.ascx" TagPrefix="uc1" TagName="CtrlAnexo" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html, body {
            background-color: #fff !important;
            background-image: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc1:CtrlAnexo runat="server" id="CtrlAnexo" />
        </div>
    </form>
</body>
</html>
