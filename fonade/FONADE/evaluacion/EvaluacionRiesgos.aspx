<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionRiesgos.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionRiesgos" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <script type="text/ecmascript">
        function url() {
            open("agregarRiesgo.aspx", "Agregar Riesgo", "width=800,height=600");
        }
    </script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla != 13);
        }
    </script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .sinlinea
        {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--<table border="0" width="100%" style="background-color: White">
        <tr>
            <td class="auto-style5">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <uc2:CtrlCheckedProyecto ID="CtrlCheckedProyecto1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>--%>
    <%--<ajaxToolkit:ToolkitScriptManager ID="tln_1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="updt_cond" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <table>
        <tbody>
            <tr>
                <td>
                    ULTIMA ACTUALIZACIÓN:&nbsp;
                </td>
                <td>
                    <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                </td>
                <td style="width: 20px;">
                </td>
                <td>
                    <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                        runat="server" TextAlign="Left" />
                    &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                        ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="False" />
                </td>
            </tr>
        </tbody>
    </table>
    <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="chk_realizado" EventName="CheckedChanged" />
            <asp:AsyncPostBackTrigger ControlID="btn_guardar_ultima_actualizacion" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <br />
    <table border="0" width="100%" style="background-color: White">
        <tr>
            <td class="auto-style5">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <div class="help_container">
                                <div onclick="textoAyuda({titulo: 'Riesgos Identificados y Mitigación', texto: 'RiesgoMitigacion'});">
                                    <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                    &nbsp; <strong>Riesgos Identificados y Mitigación</strong>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="div_Post_It1" runat="server" visible="false">
                                <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="RiesgoMitigacion" _txtTab="1" _mostrarPost="false"/>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                &nbsp;&nbsp;
                <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                    OnClick="IB_AgregarIndicador_Click" />
                &nbsp;&nbsp;
                <asp:LinkButton ID="btn_agregar" runat="server" Text="Agregar Riesgo" OnClick="btn_agregar_Click1"
                     />
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                    DataSourceID="ObjectDataSource1" DataKeyNames="Id_Riesgo" OnRowDataBound="GridView1_RowDataBound"
                    AllowPaging="True" ForeColor="#666666" Width="100%" OnRowCommand="GridView1_RowCommand" EmptyDataText="No hay datos para mostrar.">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkBLButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="" OnClientClick="return confirm('desea eliminar el riesgo de la lista?')"
                                    CssClass="sinlinea">
                                    <asp:Image ID="LB_eliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CssClass="sinlinea" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" HeaderText="Riesgo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkeditarRiesgo" runat="server" CausesValidation="False" CommandName="editar"
                                    Text='<%# Eval("Riesgo") %>' CommandArgument='<%# Eval("Id_Riesgo") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Mitigacion" HeaderText="Mitigación" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="resultado"
                    TypeName="Fonade.FONADE.evaluacion.EvaluacionRiesgos" DeleteMethod="eliminar">
                    <DeleteParameters>
                        <asp:Parameter Name="Id_Riesgo" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
