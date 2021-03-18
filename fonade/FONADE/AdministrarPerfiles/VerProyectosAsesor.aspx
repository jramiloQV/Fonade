<%@ Page Language="C#"  AutoEventWireup="true" CodeBehind="VerProyectosAsesor.aspx.cs" 
    Inherits="Fonade.FONADE.AdministrarPerfiles.VerProyectosAsesor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

<%--    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />--%>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>

    <script type="text/javascript">
        function closeWindow() {
            window.parent.opener.location.reload();
            window.parent.opener.focus();
            window.parent.close();
        }
    </script>
    <style>
    
        .Grilla table {
            border: none!important;
        }

        .Grilla td {
            border: none!important;
            padding: 5px;
            font-size: 11px!important;
        }

        .Grilla th {
            background-color: #00468f;
            color: #f2f2f2;
            font-weight: bold;
            border: 1px solid #00468f;
            padding: 5px 2px;
            text-align: left;
        }
        .Grilla tr:nth-child(2n+1) {
            background-color: #F1F1F2;
            font-size: 11px;
            border: 1px solid #F1F1F2;
            font-family: "Trebuchet MS", "Lucida Sans Unicode", "Lucida Grande", "Lucida Sans", Arial, sans-serif;
        }

        .Grilla tr:nth-child(2n) {
            background-color: #FFFFFF;
            font-size: 11px;
            border: 1px solid #fff;
            font-family: "Trebuchet MS", "Lucida Sans Unicode", "Lucida Grande", "Lucida Sans", Arial, sans-serif;
        }

    </style>
</head>
<body style="width:900px; height:650px">
    <form id="form1" runat="server" style="width:900px; height:650px">
    <asp:GridView ID="gw_Asesores" runat="server" Width="100%" AutoGenerateColumns="False"
                DataKeyNames="" CssClass="Grilla" AllowSorting="True">
                <Columns>
                    <asp:TemplateField HeaderText="Plan de Negocio">
                        <ItemTemplate>
                                <asp:Label ID="lbl_Proyecto" runat="server" Text='<%# Eval("NomProyecto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rol">
                        <ItemTemplate>
                                <asp:Label ID="lbl_ciudad" runat="server" Text='<%# Eval("Nombre") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Desde">
                        <ItemTemplate>
                                <asp:Label ID="lbl_ciudad" runat="server" Text='<%# Eval("FechaInicio") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <td><asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClientClick="return closeWindow()" /></td>
</form>
</body>
</html>