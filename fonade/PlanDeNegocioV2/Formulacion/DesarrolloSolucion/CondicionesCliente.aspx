<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CondicionesCliente.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.CondicionesCliente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../../Scripts/jquery.number.min.js"></script>
    <script>
        $(function () {
            $('.money').number(true, 2);
        });
    </script>
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
                <asp:Label ID="LabelTitulo" runat="server" Text="Cliente"></asp:Label>
            </div>
            <asp:Panel ID="pnlClientes" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="700px">
                <br />
                <label for="txtNombreCliente" class="tamlabel">&nbsp;Cliente:</label>
                &nbsp;<asp:TextBox ID="txtNombreCliente" runat="server" Enabled="false"></asp:TextBox>
                <br />
                <br />
                <label for="txtVolumenFrecuencia" class="tamlabel">&nbsp;¿Cuáles son los volúmenes y su frecuencia de compra?:</label>
                &nbsp;<asp:TextBox ID="txtVolumenFrecuencia" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_VolumenFrecuencia" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Volumen y frecuencia") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVolumenFrecuencia" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtCaracteristicasCompra" class="tamlabel">&nbsp;¿Qué características se exigen para la compra (Ej.: calidades, presentación - empaque)?:</label>
                &nbsp;<asp:TextBox ID="txtCaracteristicasCompra" runat="server" CssClass="tamCtrl" Height="50px" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_CaracteristicasCompra" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Características compra") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtCaracteristicasCompra" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtSitioCompra" class="tamlabel">&nbsp;Sitio de compra:</label>
                &nbsp;<asp:TextBox ID="txtSitioCompra" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_SitioCompra" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Sitio compra") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtSitioCompra" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtFormaPago" class="tamlabel">&nbsp;Forma de pago:</label>
                &nbsp;<asp:TextBox ID="txtFormaPago" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_FormaPago" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Forma de pago") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtFormaPago" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtPrecio" class="tamlabel">&nbsp;Precio:</label>
                &nbsp;<asp:TextBox ID="txtPrecio" runat="server" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_Precio" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Precio") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtPrecio" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtReqPostVenta" class="tamlabel">&nbsp;Requisitos post-venta:</label>
                &nbsp;<asp:TextBox ID="txtReqPostVenta" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_ReqPostVenta" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Requisitos post-venta") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtReqPostVenta" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtGarantias" class="tamlabel">&nbsp;Garantías:</label>
                &nbsp;<asp:TextBox ID="txtGarantias" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_Garantias" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Garantías") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtGarantias" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtMargen" class="tamlabel">&nbsp;Margen de comercialización:</label>
                &nbsp;<asp:TextBox ID="txtMargen" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_Margen" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Margen comercialización") %>' Text="*" Display="None" ValidationGroup="grupo1" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtMargen" SetFocusOnError="True" ToolTip="Requerido"></asp:RequiredFieldValidator>
                <br />
                <br />
                <div>
                    <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1"
                            Height="180px"/>
                        <br />
                    <div style="text-align: center">
                        <asp:Button ID="btnGuardarCondicionCliente" runat="server" Text="Guardar" OnClick="btnGuardarCondicionCliente_Click" ValidationGroup="grupo1" />
                    </div>
                </div>

            </asp:Panel>
        </div>

    </form>
</body>
</html>
