<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Actividades.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Actividades" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>www.fondoemprender.com - Actividades</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <style type="text/css">
        .panelPopup {
            display: block;
            background: white;
            width: 100%;
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
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery.number.min.js"></script>
    <script type="text/javascript">
        $(function () {
            // Set up the number formatting.
            $('.money').number(true, 2);
        });
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="formActividades" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="ADICIONAR ACTIVIDAD"></asp:Label>
            </div>
            <asp:Panel ID="pnlClientes" runat="server" CssClass="panelPopup" Width="98.5%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="100%">
                <br />
                <asp:Label ID="LabelNombreEstrategia" runat="server" Style="float: left; width: 300px; padding-left: 5px;" Font-Bold="True"></asp:Label>
                <br />
                <br />
                <label for="txtNombreCompetidor" style="float: left; width: 150px; padding-left: 5px;">Actividad:</label>
                <asp:TextBox ID="txtActividad" runat="server" Width="400px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredActividad" runat="server" ControlToValidate="txtActividad" SetFocusOnError="True" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Actividad") %>' Display="None"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px; padding-left: 5px;">Recursos Requeridos:</label>
                <asp:TextBox ID="txtRecursos" runat="server" Height="70px" TextMode="MultiLine" Width="400px" MaxLength="250"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredRecursos" runat="server" ControlToValidate="txtRecursos" SetFocusOnError="True" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Recursos Requeridos") %>'></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px; padding-left: 5px;">Mes de Ejecución:</label>
                <asp:TextBox ID="txtMes" runat="server" Width="400px" MaxLength="250" Height="70px" TextMode="MultiLine"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredMes" runat="server" ControlToValidate="txtMes" SetFocusOnError="True" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Mes de Ejecución") %>'></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px; padding-left: 5px;">Costo:</label>
                <asp:TextBox runat="server" type="text" ID="txtCosto" name="number" MaxLength="16" class="money" Text="0"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredCosto" runat="server" ControlToValidate="txtCosto" SetFocusOnError="True" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Costo") %>'></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txt" style="float: left; width: 150px; padding-left: 5px;">Responsable (Nombre del cargo líder del proceso):</label>
                <asp:TextBox ID="txtResponsable" runat="server" Width="400px" MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredResponsable" runat="server" ControlToValidate="txtResponsable" SetFocusOnError="True" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Responsable...") %>'></asp:RequiredFieldValidator>
                <br />
                <blockquote>
                    <asp:ValidationSummary ID="ValidationSummary" runat="server" DisplayMode="List" ForeColor="Red" HeaderText="Advertencia:" Font-Italic="True" />
                </blockquote>
                <div style="text-align: center; height: 40px;">
                    &nbsp;<asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
