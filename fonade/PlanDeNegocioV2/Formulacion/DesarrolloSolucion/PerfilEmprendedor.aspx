<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PerfilEmprendedor.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.PerfilEmprendedor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <style>
        .tamlabel {
            float: left;
            width: 30%;
        }

        .tamCtrl {
            width: 60%;
        }

        .panelPopup {
            display: block;
            background: white;
            height: auto;
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
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="Emprendedor"></asp:Label>
            </div>
            <asp:Panel ID="pnlEmprendedor" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="700px">
                <br />
                <label for="txtNombreEmprendedor" class="tamlabel">&nbsp;Emprendedor:</label>
                &nbsp;<asp:TextBox ID="txtNombreEmprendedor" Width="250px" runat="server" Enabled="false"></asp:TextBox>
                <br />
                <br />
                <label for="txtPerfil" class="tamlabel">&nbsp;Perfil:</label>
                &nbsp;<asp:TextBox ID="txtPerfil" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvPerfil" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(126)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtPerfil" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtRol" class="tamlabel">&nbsp;Rol:</label>
                &nbsp;<asp:TextBox ID="txtRol" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvRol" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(127)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtRol" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <div>
                    <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1" Height="180px"/>
                        <br />
                    <div style="text-align: center">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="grupo1" />
                    </div>
                </div>
                <br />
                <br />
            </asp:Panel>
        </div>

    </form>
</body>
</html>
