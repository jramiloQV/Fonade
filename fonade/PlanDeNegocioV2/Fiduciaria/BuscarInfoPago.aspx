<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="BuscarInfoPago.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Fiduciaria.BuscarInfoPago" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Label ID="lblTitulo" runat="server" Text="Información por pago" Font-Size="Large"></asp:Label>
    <hr />
    <asp:Label ID="lblCodPago" runat="server" Text="Código: "></asp:Label>

    <asp:TextBox ID="txtCodPago" runat="server"></asp:TextBox>
    <asp:Button ID="btnBuscarPago" runat="server" Text="Buscar"
        OnClick="btnBuscarPago_Click" />

    <br />
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1"
        ControlToValidate="txtCodPago" runat="server"
        ErrorMessage="Solo se permiten numeros."
        ValidationExpression="\d+">
    </asp:RegularExpressionValidator>

    <asp:Label ID="lblMensaje" runat="server" Text="No se encontraron registros."
        Visible="false"></asp:Label>

    <div id="panelBuscqueda" runat="server" visible="false">
        <table>
            <tr>
                <td>
                    <asp:Label ID="codPago" runat="server" Text="Codigo Pago:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCodPagoBusqueda" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblActividad" runat="server" Text="Actividad:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtActividad" runat="server" Enabled="false"
                        Width="650px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblCodProyecto" runat="server" Text="Proyecto:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtcodProyecto" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNomProyecto" runat="server" Text="Nombre Proyecto:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNomProyecto" runat="server" Enabled="false"
                        Width="650px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                    <asp:Label ID="lblBeneficiario" runat="server"
                        Text="--BENEFICIARIO--"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblTipoIDBeneficiario" runat="server"
                        Text="Tipo Identificacion:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTipoIDBeneficiario" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblIDBeneficiario" runat="server"
                        Text="Identificacion:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtIDBeneficiario" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNombreBeneficiario" runat="server"
                        Text="Nombres:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNombresBen" runat="server" Enabled="false"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblRazonSocial" runat="server"
                        Text="Razon Social:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtRazonSocial" runat="server" Enabled="false"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblBancoBeneficiario" runat="server"
                        Text="Banco:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtBancoBeneficiario" runat="server" Enabled="false"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblNumCuentaBen" runat="server"
                        Text="Numero Cuenta:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumCuenta" runat="server" Enabled="false"
                        Width="350px"></asp:TextBox>
                </td>
            </tr>
        </table>

    </div>


    <!--
    <asp:GridView ID="gvInfoPago" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id" 
                EmptyDataText="No se encontró información."
                ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />                    
                </Columns>
            </asp:GridView>-->

</asp:Content>
