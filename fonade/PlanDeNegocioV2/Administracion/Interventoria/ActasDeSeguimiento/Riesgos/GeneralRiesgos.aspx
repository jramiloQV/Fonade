<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GeneralRiesgos.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Riesgos.GeneralRiesgos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <script src="../../../../../Scripts/jquery-1.7.2.min.js"></script>
    <script src="../../../../../Scripts/jquery-ui.js"></script>
    <link href="../../../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />

    <title>General/Riesgos</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function mostrarHistorico() {
            $("#dialog").dialog({
                title: "Historial Riesgo",
                buttons: {
                    Ok: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true,
                width: 600
            });
            return false;
        };
    </script>

    <script type="text/javascript">
        function alerta() {
            return confirm("Está seguro que desea eliminar el riesgo seleccionado?");
        }
    </script>

    <script type="text/javascript">
        function mostrar(elemento, accion) {
            if (document.getElementById(elemento).style.display == 'inherit') {
                document.getElementById(elemento).style.display = 'none'
            }
            else {
                document.getElementById(elemento).style.display = 'inherit'
            }

        };
    </script>
</head>
<body>
    <%-- <% Page.DataBind(); %>--%>
    <form id="form1" runat="server">
        <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server" Text="2. RIESGOS IDENTIFICADOS EN EVALUACION"></asp:Label>
            </h1>
        </div>
        <div>
            <%--OnRowDataBound="GridView1_RowDataBound"--%>
            <%--OnRowCommand="GridView1_RowCommand"--%>
            <asp:GridView ID="gvRiesgos" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="idRiesgoInterventoria"
                OnPageIndexChanging="gvRiesgos_PageIndexChanging"
                AllowPaging="True" ForeColor="#666666" Width="100%" OnRowEditing="gvRiesgos_RowEditing"
                OnRowCancelingEdit="gvRiesgos_RowCancelingEdit" OnRowUpdating="gvRiesgos_RowUpdating"
                EmptyDataText="No hay datos para mostrar." OnRowCommand="gvRiesgos_RowCommand">
                <Columns>
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" />

                    <asp:TemplateField ShowHeader="False" HeaderText="Riesgo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCodConvocatoria" runat="server"
                                Text='<%# Eval("codConvocatoria") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="Riesgo" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCodProyecto" runat="server"
                                Text='<%# Eval("codProyecto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Descripción del Riesgo">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtRiesgoEditar" runat="server" Text='<%# Bind("Riesgo") %>' TextMode="MultiLine"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRiesgo" runat="server" Text='<%# Bind("Riesgo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Mitigación">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMitigacionEditar" runat="server" Text='<%# Bind("Mitigacion") %>' TextMode="MultiLine"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblMitigacion" runat="server" Text='<%# Bind("Mitigacion") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ShowHeader="False" HeaderText="Riesgo">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkHistorial" runat="server" CausesValidation="False"
                                CommandName="Historial"
                                Text="Historial"
                                CommandArgument='<%# Eval("idRiesgoInterventoria")+"|"
                                                    +(Container.DataItemIndex + 1)
                                                    +"|"+Eval("Riesgo") %>'>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="Gestión del Emprendedor">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGestionRiesgo" runat="server"
                                TextMode="MultiLine" MaxLength="10000"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>

                            <asp:ImageButton ID="btn_Borrar" CommandArgument='<%# Eval("idRiesgoInterventoria")%>' runat="server"
                                ImageUrl="/Images/icoBorrar.gif" Visible="true" CausesValidation="false"
                                CommandName="Eliminar"
                                OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

        <div style="text-align: center;">

            <input type="button" onclick="mostrar('panelAgregarNuevoRiesgo')"
                value="Agregar Riesgo" class="btnCertif" />

        </div>

        <div id="panelAgregarNuevoRiesgo" style="display: none;">
            <h1 style="text-align: center;">
                <asp:Label Text="Agregar Riesgo"
                    runat="server" ID="lblTituloRiesgo" Visible="true" />
            </h1>
            <hr />
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">Descripción del Riesgo:</td>
                    <td>
                        <asp:TextBox ID="txtDescripcionRiesgo" runat="server" MaxLength="500"
                            Width="97%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">Mitigación:</td>
                    <td>
                        <asp:TextBox ID="txtMitigacion" runat="server" MaxLength="500"
                            Width="97%" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <!--OnClick="btnGuardarProducto_Click"-->
                    <td colspan="2" style="text-align: center" class="auto-style2">
                        <asp:Button ID="btnAgregarRiesgo" runat="server" Height="29px"
                            Text="Guardar Riesgo"
                            OnClick="btnAgregarRiesgo_Click" />

                    </td>
                </tr>
            </table>
        </div>

        <hr />
        <div style="text-align: center">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
        </div>

        <div id="dialog" style="display: none">
            <div>
                <asp:Label ID="lblRiesgo" runat="server" Text="Riesgo: " Font-Bold="True" ForeColor="#00468f"></asp:Label>
            </div>
            <hr />
            <div>
                <asp:GridView ID="gvGestionEmprendedor" runat="server"
                    AutoGenerateColumns="False" CssClass="Grilla"
                    DataKeyNames="CodRiesgo"
                    ForeColor="#666666" Width="100%"
                    EmptyDataText="No hay datos para mostrar.">
                    <Columns>
                        <asp:TemplateField ShowHeader="False" HeaderText="Visita">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="GestionRiesgo" HeaderText="Gestión del Emprendedor" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

    </form>
</body>
</html>
