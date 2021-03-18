<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Competidores.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.OportunidadMercado.Competidores" ValidateRequest="false"%>


<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>www.fondoemprender.com - Competidores</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <style type="text/css">
        .panelPopup {
            display: block;
            background: white;
            width: auto;
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
    <form id="formCompetidores" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="ADICIONAR COMPETIDOR"></asp:Label>
            </div>
            <asp:Panel ID="pnlClientes" runat="server" CssClass="panelPopup" Width="98.5%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="100%">
                <br />
                <label for="txtNombreCompetidor" style="float: left; width: 150px;" title="Máximo 100 caracteres">&nbsp;Nombre Competidor:<br />(100 max.)</label>
                <asp:TextBox ID="txtNombreCompetidor" runat="server" Width="400px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredNombre" runat="server" ControlToValidate="txtNombreCompetidor" SetFocusOnError="True" ToolTip="Requerido" ErrorMessage='' Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px;" title="Máximo 250 caracteres">&nbsp;Localización:<br />(1000 max.)</label>
                <asp:TextBox ID="txtLocalizacion" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredLocalizacion" runat="server" ControlToValidate="txtLocalizacion" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px;" title="Máximo 250 caracteres">&nbsp;Productos y Servicios (Atributos):<br />(1000 max.)</label>
                <asp:TextBox ID="txtProductos" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredProductos" runat="server" ControlToValidate="txtProductos" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px;" title="Máximo 250 caracteres">&nbsp;Precios:<br />(1000 max.)</label>
                <asp:TextBox ID="txtPrecios" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredPrecios" runat="server" ControlToValidate="txtPrecios" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px;" title="Máximo 250 caracteres">&nbsp;Logística de Distribución: (1000 max.)</label>
                <asp:TextBox ID="TxtLogistica" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="1000"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredLogistica" runat="server" ControlToValidate="TxtLogistica" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px;" title="Máximo 250 caracteres">&nbsp;Otro, ¿Cuál?<br />(1000 max.)</label>
                <asp:TextBox ID="txtOtroCual" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="1000"></asp:TextBox>
                <br />
                <blockquote>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="List" ForeColor="Red" HeaderText="Advertencia:" />
                </blockquote>
                <div style="text-align: center; height: 40px;">
                    <asp:Button ID="btnGuardarCompetidor" runat="server" Text="Guardar" OnClick="btnGuardarCompetidor_Click" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
