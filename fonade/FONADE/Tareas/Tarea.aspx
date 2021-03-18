<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="Tarea.aspx.cs" Inherits="Fonade.FONADE.Tareas.Tarea" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label runat="server" ID="lblTitulo" Text="Revisar tarea" />
    </h1>

    <asp:Panel ID="PnlTarea" runat="server">
        <table>
            <tr>
                <td>
                    De:
                </td>
                <td>
                    <asp:TextBox ID="txtRemitente" runat="server" Enabled="false" Width="500px" Height="16px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Para:
                </td>
                <td>
                    <asp:TextBox ID="txtDestinatario" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Actividad:
                </td>
                <td>
                    <asp:TextBox ID="txtTipo" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr id="pnlPlanDeNegocio" runat="server">
                <td>
                    Plan de Negocio:
                </td>
                <td>
                    <asp:TextBox ID="txtProyecto" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Tarea:
                </td>
                <td>
                    <asp:TextBox ID="txtAsunto" runat="server" Enabled="false" Width="500px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                    <asp:LinkButton ID="lnkSolicitud" runat="server" Visible="false" Font-Bold="true" CausesValidation="false" Text="Ver riesgo" OnClick="lnkSolicitud_Click"/>
                    <br />
                    <asp:LinkButton ID="lnkVerTareasEspeciales" runat="server" Visible="false" Font-Bold="true" CausesValidation="false" Text="Ver tareas especial de interventoria" OnClick="lnkVerTareas_Click"/>
                </td>
            </tr>
            <tr>
                <td>
                    Descripción:
                </td>
                <td>
                    <asp:TextBox ID="txtDescripcion" runat="server" Enabled="false" Width="500px" Height="50px"
                        TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha:
                </td>
                <td>
                    <asp:TextBox ID="txtFecha" runat="server" Enabled="false" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Urgencia:
                </td>
                <td>
                    <asp:Image ID="imgUrgencia" ImageUrl="../../Images/Tareas/Urgencia1.gif" runat="server" />
                    &nbsp;<asp:Label ID="lblUrgencia" Text="Muy Alta" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblObservacion" Text="Observaciones: " runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" Width="500px" Height="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Marcar como finalizada:
                </td>
                <td>
                    <asp:CheckBox ID="chkFinalizada" runat="server" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Label ID="lblError" runat="server" Font-Bold="True" ForeColor="#CC0000" Visible="false" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Grabar" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Atras"  PostBackUrl="~/FONADE/MiPerfil/Home.aspx" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
