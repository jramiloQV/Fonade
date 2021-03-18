<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DetalleEmprendedor.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.DetalleEmprendedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>www.fondoemprender.com - Detalle Emprendedor</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <style type="text/css">
        .panelPopup {
            display: block;
            background: white;
            vertical-align: middle;
        }

        .DivCaption {
            background-color: #00468F;
            color: #ffffff;
            font-weight: bold;
            height: 23px;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="formDetalle" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="RESUMEN DEL PERFIL"></asp:Label>
            </div>
            <asp:Panel ID="pnlDetalle" runat="server" CssClass="panelPopup" Width="97.5%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px">
                <blockquote>
                    <asp:Label ID="Label1" runat="server" Width="120" Text="Nombres:"></asp:Label>
                    <asp:Label ID="LabelNombre" runat="server"></asp:Label><br />
                    <br />
                    <asp:Label ID="Label2" runat="server" Width="120" Text="Correo Electrónico:"></asp:Label>
                    <asp:Label ID="LabelEmail" runat="server"></asp:Label><br />
                    <br />
                    <asp:Label ID="Label4" runat="server" Width="120" Text="Fecha Nacimiento:"></asp:Label>
                    <asp:Label ID="LabelFechaNac" runat="server"></asp:Label><br />
                    <br />
                    <asp:Label ID="Label6" runat="server" Width="120" Text="Lugar Nacimiento:"></asp:Label>
                    <asp:Label ID="LabelLugarNac" runat="server"></asp:Label>
                    <br />
                    <br />
                </blockquote>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
