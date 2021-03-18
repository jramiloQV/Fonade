<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="AdicionarInformeBimensualDetalle.aspx.cs" Inherits="Fonade.FONADE.interventoria.AdicionarInformeBimensualDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>FONDO EMPRENDER</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.4.min.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="100%">
        <tr>
            <td width="33.3%">
                <asp:Label ID="lbl_enunciado" runat="server" Text="ADICIONAR" />
            </td>
            <td width="33.3%">
                <asp:Label ID="l_usuariolog" runat="server"></asp:Label>
            </td>
            <td width="33.3%">
                <asp:Label ID="l_fechaActual" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <div style="width: 100%;">
        <%--Adicionar informe bimensual detalle--%>
        <asp:Panel ID="pnlPrincipal" runat="server" Visible="true" Width="100%">
            <table cellpadding="2">
                <tbody>
                    <tr>
                        <td>
                            Cumplimiento a verificar:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCumplimiento" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Observación del interventor
                        </td>
                        <td>
                            <asp:TextBox ID="txtObservacionInterventor" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Indicador asociado
                        </td>
                        <td>
                            <asp:TextBox ID="txtIndicador" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cumple
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbCumple" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1" Text="Si" />
                                <asp:ListItem Value="0" Text="No" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Hacer seguimiento
                        </td>
                        <td>
                            <asp:CheckBox ID="cbHacerSeguimiento" runat="server" />
                        </td>
                    </tr>
                    <tr style="text-align: center;">
                        <td>
                            <asp:Button ID="btnActualizar" runat="server" Text="Enviar" OnClick="btnActualizar_Click1" />
                        </td>
                        <td>
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False"
                                OnClick="btnCancelar_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <%--Adicionar informe bimensual detalle.--%>
    </div>
</asp:Content>
