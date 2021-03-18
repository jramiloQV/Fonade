<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Protagonista.Clientes" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>www.fondoemprender.com - Clientes</title>
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
    <% Page.DataBind(); %>
    <form id="formClientes" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="ADICIONAR CLIENTE"></asp:Label>
            </div>
            <asp:Panel ID="pnlClientes" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="100%">
                <blockquote>
                    <table style="width: 530px">
                        <tr>
                            <td style="width: 110px">Nombre Cliente: (100 max.)
                            </td>
                            <td>
                                <asp:TextBox ID="txtNombreCliente" runat="server" Width="250px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredNombre" runat="server" ControlToValidate="txtNombreCliente" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Nombre") %>' Font-Bold="True" ForeColor="Red" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="float: left; width: 90px;">Perfil: (1000 max.)</label></td>
                            <td>
                                <asp:TextBox ID="txtPerfil" runat="server" Height="70px" TextMode="MultiLine" Width="300px" MaxLength="1000"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredPerfil" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Perfil") %>' Font-Bold="True" ForeColor="Red" ControlToValidate="txtPerfil" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="float: left; width: 90px;">Localización: (1000 max.)</label></td>
                            <td>
                                <asp:TextBox ID="txtLocalizacion" runat="server" Height="70px" MaxLength="1000" TextMode="MultiLine" Width="300px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredLocalizacion" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Localización") %>' Font-Bold="True" ForeColor="Red" ControlToValidate="txtLocalizacion" SetFocusOnError="True" ToolTip="Requerido" Display="None"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="float: left; width: 90px;">Justificación: (1000 max.)</label></td>
                            <td>
                                <asp:TextBox ID="txtJustificacion" runat="server" Height="70px" TextMode="MultiLine" Width="300px" MaxLength="1000"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredJustificacion" runat="server" ControlToValidate="txtJustificacion" Display="None" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Justificación") %>' Font-Bold="True" ForeColor="Red" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>

                    <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="Advertencia:" Font-Italic="True" ForeColor="Red" />

                </blockquote>
                <div style="text-align: center; height: 40px;">
                    <asp:Button ID="btnGuardarCliente" runat="server" Text="Guardar" OnClick="btnGuardarCliente_Click" />
                </div>
                <br />
                <br />
            </asp:Panel>
        </div>
    </form>
</body>
</html>
