<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="ObservacionAcreditacion.aspx.cs" Inherits="Fonade.Formulario_web1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
            height: auto;
        }

        #Control {
            display: inline-block;
            margin: 0px auto;
            float: right;
        }
        #contenido {
            width:650px;
            height:auto;
        }
    </style>
    <script type="text/javascript">
        function cerrar() {
            window.opener.location.reload();
            //window.location.reload();
            window.close();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div id="contenido">
        <div id="titulo">
            <h1>ACREDITAR PLAN DE NEGOCIO</h1>
        </div>
        <table>
            <tr>
                <td>Emprendedor:</td>
                <td>
                    <asp:Label ID="lblEmprendedor" runat="server" Text="" Width="300px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Estado de Acreditación:</td>
                <td>
                    <asp:Label ID="lblEstado" runat="server" Text="" Width="300px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Fecha compromiso:</td>
                <td>
                    <asp:TextBox ID="txtFecha" runat="server" BackColor="White" Enabled="False" Text="" Width="300px"></asp:TextBox>
                    <asp:Image ID="imgFecha" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                    <asp:CalendarExtender ID="cldFecha" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFecha" TargetControlID="txtFecha" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblEstadoR" runat="server" Text="" Width="300px"></asp:Label></td>
                <td>
                    <asp:CheckBox ID="ckkEstado" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Asunto:</td>
                <td>
                    <asp:TextBox ID="txtAsunto" runat="server" Text="" Width="300px" MaxLength="150"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Observación:</td>
                <td>
                    <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Width="300px" Height="100px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div id="Control">
            <asp:Button ID="btnGuardarEnviar" runat="server" Text="Guardar y Enviar" OnClick="btnGuardarEnviar_Click" />
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClientClick="cerrar()" />
        </div>
    </div>
</asp:Content>
