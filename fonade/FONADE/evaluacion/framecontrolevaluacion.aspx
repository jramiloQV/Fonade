<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="framecontrolevaluacion.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.framecontrolevaluacion" %>

<%@ Register Src="~/Controles/evaluacion/CatalogoAporteEvaluacion.ascx" TagName="Aporteevaluacion"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:Aporteevaluacion ID="Aporteevaluacion" runat="server" />
    </div>
    </form>
</body>
</html>
