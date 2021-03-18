<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.Controles.WebForm1" %>

<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />
    </div>
        <asp:Button ID="btnGuardar" runat="server" OnClick="btnGuardar_Click" Text="Guardar" />
    </form>
</body>
</html>
