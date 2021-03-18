<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActaSeguimientoPDF.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates.ActaSeguimientoPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 131px;
        }
        .auto-style2 {
            width: 577px;
        }
        .auto-style3 {
            width: 120px;
            height: 105px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%;">
                <tr>
                    <td class="auto-style1" rowspan="2" style="border-style: solid; border-width: 1px">
                        <img alt="" class="auto-style3" src="../../../../../Images/Img/logoFonade.png" /></td>
                    <td class="auto-style2" style="border-style: solid; border-width: thin">
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td rowspan="2" style="border-style: solid; border-width: thin">
                        <img alt="" class="auto-style3" src="../../../../../Images/Img/logoFonade.png" /></td>
                </tr>
                <tr>
                    <td class="auto-style2" style="border-style: solid; border-width: thin">&nbsp;</td>
                </tr>                
            </table>
        </div>
    </form>
</body>
</html>
<script language="javascript">
    window.print();
</script>