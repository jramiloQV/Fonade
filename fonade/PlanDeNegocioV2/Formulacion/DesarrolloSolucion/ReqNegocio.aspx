<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReqNegocio.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.ReqNegocio" %>

<!DOCTYPE html>

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
                <asp:Label ID="LabelTitulo" runat="server" Text="Adicionar Descripción"></asp:Label>
            </div>
            <asp:Panel ID="pnlRequerimiento" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="700px">
                <br />
                <label for="txtDescripcion" class="tamlabel">&nbsp;Descripción:</label>
                &nbsp;<asp:TextBox ID="txtDescripcion" runat="server" MaxLength="100" CssClass="tamCtrl"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvDescripcion" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(119)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtDescripcion" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtCantidad" class="tamlabel">&nbsp;Cantidad:</label>
                &nbsp;<asp:TextBox ID="txtCantidad" runat="server" MaxLength="10"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvCantidad" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(120)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtCantidad" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                &nbsp;<asp:RegularExpressionValidator ID="ravCantidad" ControlToValidate="txtCantidad" runat="server" ErrorMessage='<%#Fonade.Negocio.Mensajes.Mensajes.GetMensaje(118)%>' Display="None" ValidationExpression="\d+" ValidationGroup="grupo1"></asp:RegularExpressionValidator>
                <asp:RangeValidator ID="Range1"
                    ControlToValidate="txtCantidad"
                    MinimumValue="1"
                    MaximumValue="2147483647"
                    Type="Integer" Display="None"
                    ErrorMessage='<%#Fonade.Negocio.Mensajes.Mensajes.GetMensaje(124)%>'
                    runat="server" ValidationGroup="grupo1"/>

                <br />
                <br />
                <label for="txtVlrUnitario" class="tamlabel">&nbsp;Valor Unitario:</label>
                &nbsp;<asp:TextBox ID="txtVlrUnitario" runat="server" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rv_Precio" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(121)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrUnitario" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <blockquote style="font-style: italic">Nota: El valor debe incluír todos los impuestos y costos asociados a su compra y ubicación en el lugar donde aparecerá el negocio</blockquote>
                <br />
                <br />
                <label for="ddlFuenteFinan" class="tamlabel">&nbsp;Fuente de Financiación:</label>
                &nbsp;<asp:DropDownList ID="ddlFuenteFinan" runat="server" AutoPostBack="True" CausesValidation="True" ClientIDMode="Static">
                </asp:DropDownList>
                &nbsp;<asp:RequiredFieldValidator ID="rvFuenteFinan" runat="server" InitialValue="0" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(122)) %>' Text="" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlFuenteFinan" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtReqTecnico" class="tamlabel">&nbsp;Requisitos Técnicos:</label>
                &nbsp;<asp:TextBox ID="txtReqTecnico" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvReqTecnico" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(123)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtReqTecnico" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <div>
                    <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1" Height="180px"/>
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
