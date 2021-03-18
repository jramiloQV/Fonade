<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ProyectoAcreditacion.aspx.cs" Inherits="Fonade.FONADE.Administracion.RATProyAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="divCuerpoPrograma" runat="server">
        <h1>
            <label>
                ACREDITAR PLAN DE NEGOCIO</label>
        </h1>
        <table>
            <tr>
                <td colspan="2">
                    <asp:Label ID="mNomConvocatoria" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">UNIDAD:&nbsp;
                    <asp:Label ID="mNomUnidadEmprendimiento" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>ASESOR LIDER:&nbsp;
                    <asp:LinkButton ID="mAsesorLider" runat="server" Text="" OnClick="mAsesorLider_Click"></asp:LinkButton>
                </td>
                <td>ASESOR:&nbsp;
                    <asp:LinkButton ID="mAsesor" runat="server" Text="" OnClick="mAsesor_Click"></asp:LinkButton>
                </td>
            </tr>
        </table>
        <br />
        <hr />
        <br />
        <table>
            <tr>
                <td colspan="4">
                    <h2>DATOS DEL PROYECTO</h2>
                </td>
            </tr>
            <tr>
                <td>Plan de Negocio:
                </td>
                <td>
                    <asp:Label ID="lblmNomproyecto" runat="server" Text=""></asp:Label>
                </td>
                <td>Lugar ejecución:
                </td>
                <td>
                    <asp:Label ID="lblmNomCiudad" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha de aval:(dd/mm/aaaa)
                </td>
                <td>
                    <asp:Label ID="lblmFechaAval" runat="server" Text=""></asp:Label>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:RadioButton ID="rbAsignado" runat="server" Text="Asignado" Enabled="false" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rbPendiente" runat="server" Text="Pendiente" Enabled="false" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rbSubsanado" runat="server" Text="Subsanado" Enabled="false" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rbAcreditado" runat="server" Text="Acreditado" Enabled="false" />&nbsp;&nbsp;
                    <asp:RadioButton ID="rbNoAcreditado" runat="server" Text="No Acreditado" Enabled="false" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Table ID="tproyectoacreditacion" runat="server" class="Grilla">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell RowSpan="3">Nombre Emprendedores</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="3">Pendiente</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="3">Subsanado</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="3">Acreditado</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="3">No Acreditado</asp:TableHeaderCell>
                <asp:TableHeaderCell ColumnSpan="8">Documentos Anexos</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableHeaderRow>
                <asp:TableHeaderCell ColumnSpan="4">Anexos</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="2">Certificaciones</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="2">Diplomas</asp:TableHeaderCell>
                <asp:TableHeaderCell RowSpan="2">Actas</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableHeaderRow>
                <asp:TableHeaderCell>1</asp:TableHeaderCell>
                <asp:TableHeaderCell>2</asp:TableHeaderCell>
                <asp:TableHeaderCell>3</asp:TableHeaderCell>
                <asp:TableHeaderCell>DI</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="12"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="lnknotificaciones" runat="server" Text="Ver Notificaciones Enviadas"
                        OnClick="lnknotificaciones_Click"></asp:LinkButton>
                </td>
                <td style="text-align: right;"># Radicación CRIF
                </td>
                <td>
                    <asp:TextBox ID="txtradificacion" runat="server" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:LinkButton ID="lnkvertodos" runat="server" Text="Ver todos" OnClick="lnkvertodos_Click"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: right;">
                    <asp:Button ID="btnguardar" runat="server" Text="Guardar" OnClick="btnguardar_Click"
                        OnClientClick="return confirm('¿Desea guardar la información?')" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <asp:Panel ID="pnlestadoproyec" runat="server" Visible="false">
            <table>
                <tr>
                    <td>Observaciones
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center;">
                        <asp:TextBox ID="txtobservacionproyecto" runat="server" Text="" TextMode="MultiLine"
                            Width="80%" Height="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Button ID="btnfinalizar" runat="server" Text="Finalizar Acreditación" OnClick="btnfinalizar_Click"
                            OnClientClick="return confirm('¿Desea finalizar la acreditación?')" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
