<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CargosProyecto.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.CargosProyecto" %>

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

        .tamlabel1 {
            float: left;
            width: 40%;
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

        .divgen {
            width: auto;
            height: auto;
        }

        .divleft {
            float: left;
            width: 50%;
        }

        .divright {
            float: right;
            width: 50%;
        }
    </style>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <div>
            <div class="DivCaption">
                <hr />
                <asp:Label ID="LabelTitulo" runat="server" Text="Adicionar Cargo"></asp:Label>
            </div>
            <asp:Panel ID="pnlCargo" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" ScrollBars="Vertical" Height="700px">
                <br />
                <label for="txtNomCargo" class="tamlabel">&nbsp;Nombre del Cargo:</label>
                &nbsp;<asp:TextBox ID="txtNomCargo" runat="server" MaxLength="100" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvNomCargo" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(129)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtNomCargo" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtFunciones" class="tamlabel">&nbsp;Funciones Principales:</label>
                &nbsp;<asp:TextBox ID="txtFunciones" runat="server" CssClass="tamCtrl" MaxLength="250" TextMode="MultiLine" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvFunciones" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(130)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtFunciones" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <h1>Perfil Requerido</h1>
                <br />
                <br />
                <label for="txtFormacion" class="tamlabel">&nbsp;Formación:</label>
                &nbsp;<asp:TextBox ID="txtFormacion" runat="server" MaxLength="250" CssClass="tamCtrl" TextMode="MultiLine" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvFormacion" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(131)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtFormacion" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtExpGeneral" class="tamlabel">&nbsp;Experiencia General (Años):</label>
                &nbsp;<asp:TextBox ID="txtExpGeneral" runat="server" MaxLength="100" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvExpGeneral" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(132)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtExpGeneral" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <label for="txtExpEspecifica" class="tamlabel">&nbsp;Experiencia Específica (Años):</label>
                &nbsp;<asp:TextBox ID="txtExpEspecifica" runat="server" MaxLength="100" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="rvExpEspecifica" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(133)) %>' Text="*" Display="None" Font-Bold="True"
                    Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtExpEspecifica" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                <br />
                <br />
                <div class="divgen">
                    <div class="divleft">
                        <label for="ddlTipoContrato" class="tamlabel1">&nbsp;Tipo de Contratación:</label>
                        &nbsp;<asp:DropDownList ID="ddlTipoContrato" runat="server" AutoPostBack="false" CausesValidation="false" ValidationGroup="grupo1" ClientIDMode="Static"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="0">Jornal</asp:ListItem>
                            <asp:ListItem Value="1">Nómina</asp:ListItem>
                            <asp:ListItem Value="2">Prestación de Servicios</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:RequiredFieldValidator ID="rvTipoContrato" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(134)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlTipoContrato" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divright">
                        <label for="ddlDedicacion" class="tamlabel1">&nbsp;Dedicación de Tiempo:</label>
                        &nbsp;<asp:DropDownList ID="ddlDedicacion" runat="server" AutoPostBack="False" CausesValidation="false" ValidationGroup="grupo1" ClientIDMode="Static"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="0">Completo</asp:ListItem>
                            <asp:ListItem Value="1">Parcial</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:RequiredFieldValidator ID="rvDedicacion" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(135)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlDedicacion" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divleft">
                        <label for="ddlUnidadTiempo" class="tamlabel1">&nbsp;Unidad de Medida en Tiempo:</label>
                        &nbsp;<asp:DropDownList ID="ddlUnidadTiempo" runat="server" AutoPostBack="False" CausesValidation="false" ValidationGroup="grupo1" ClientIDMode="Static"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'>
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="1">Días</asp:ListItem>
                            <asp:ListItem Value="2">Mes</asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;<asp:RequiredFieldValidator ID="rvUnidadTiempo" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(136)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="ddlUnidadTiempo" SetFocusOnError="False" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divright">
                        <label for="txtTiempoVinculacion" class="tamlabel1">&nbsp;Tiempo Vinculación Primer Año (meses o días):</label>
                        &nbsp;<asp:TextBox ID="txtTiempoVinculacion" runat="server" MaxLength="3" Width="25px" ValidationGroup="grupo1" AutoPostBack="true" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>' OnTextChanged="txtCalculos_TextChanged"
                             ></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvTiempoVinculacion" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(137)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtTiempoVinculacion" SetFocusOnError="false" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divleft">
                        <label for="txtVlrRemunUnitario" class="tamlabel1">&nbsp;Valor Remuneración *Unitario:</label>
                        &nbsp;<asp:TextBox ID="txtVlrRemunUnitario" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1" AutoPostBack="true" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>' OnTextChanged="txtCalculos_TextChanged" ></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvVlrRemunUnitario" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(138)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrRemunUnitario" SetFocusOnError="false" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divright">
                        <label for="txtVlrOtros" class="tamlabel1">&nbsp;Otros Gastos (Prestaciones / Auxilio Transporte):</label>
                        &nbsp;<asp:TextBox ID="txtVlrOtros" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1" AutoPostBack="true" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>' OnTextChanged="txtCalculos_TextChanged"></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvVlrOtros" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(139)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrOtros" SetFocusOnError="False" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divleft">
                        <label for="txtVlrPrestaciones" class="tamlabel1">&nbsp;Valor con Prestaciones:</label>
                        &nbsp;<asp:TextBox ID="txtVlrPrestaciones" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" Enabled="false"></asp:TextBox>

                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divright">
                        <label for="txtVlrRemunPrimerAnio" class="tamlabel1">&nbsp;Valor Remuneración Primer Año:</label>
                        &nbsp;<asp:TextBox ID="txtVlrRemunPrimerAnio" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" Enabled="false"></asp:TextBox>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divleft">
                        <label for="txtVlrFondoEmprender" class="tamlabel1">&nbsp;Valor Solicitado Fondo Emprender:</label>
                        &nbsp;<asp:TextBox ID="txtVlrFondoEmprender" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvVlrFondoEmprender" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(140)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrFondoEmprender" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div class="divright">
                        <label for="txtVlrAportesEmprendedor" class="tamlabel1">&nbsp;Aportes Emprendedores:</label>
                        &nbsp;<asp:TextBox ID="txtVlrAportesEmprendedor" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvVlrAportesEmprendedor" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(141)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrAportesEmprendedor" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <br />
                    </div>
                    <div style="width:50%">
                        <label for="txtVlrIngresosVentas" class="tamlabel1">&nbsp;Ingresos por Ventas:</label>
                        &nbsp;<asp:TextBox ID="txtVlrIngresosVentas" runat="server" Width="110px" CssClass="money" Text="0" Height="16px" MaxLength="16" ValidationGroup="grupo1" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'></asp:TextBox>
                        &nbsp;<asp:RequiredFieldValidator ID="rvVlrIngresosVentas" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(142)) %>' Text="*" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="txtVlrIngresosVentas" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                    </div>
                </div>
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
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" ValidationGroup="grupo1" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "EsEdicion"))%>'/>
                    </div>
                </div>
                <br />
                <br />


            </asp:Panel>
        </div>

    </form>
</body>
</html>
