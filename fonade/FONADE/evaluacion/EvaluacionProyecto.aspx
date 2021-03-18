<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionProyecto.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionProyecto" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body style="color: #696969;">
    <form id="form1" runat="server">
    <div>
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
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td>
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
                    <table style="width: 100%">
                        <%--<tr>
                                <td style="width: 50%">
                                    <uc2:CtrlCheckedProyecto ID="CtrlCheckedProyecto1" runat="server" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td class="auto-style5">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 50%">
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Evaluación del Proyecto', texto: 'EvaluacionProyecto'});">
                                        <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                        &nbsp; <strong>Evaluación del Proyecto:</strong>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div id="div_Post_It1" runat="server" visible="false">
                                    <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="EvaluacionProyecto" _txtTab="1" _mostrarPost="false"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="auto-style1">
            <tr>
                <td>
                    <table class="auto-style1">
                        <tr>
                            <td style="width: 25%">
                                <asp:Label ID="L_TiempoProyeccion" runat="server" Text="Tiempo de Proyección"></asp:Label>
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="DD_TiempoProyeccion" runat="server">
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 50%;">
                                <asp:Button ID="B_Guardar" runat="server" Text="Guardar" OnClick="B_Guardar_Click"
                                    Visible="false" OnClientClick="return confirm('Si cambia el tiempo de proyección se borraran todos los valores proyectados.  Esta seguro de realizar este cambio?')" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Table ID="T_Supuestos" runat="server" Width="100%">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="B_ActualizarSupuesto" runat="server" Text="Actualizar Supuesto" OnClick="B_ActualizarSupuesto_Click"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="L_IndicadoresFinancierosProyectados" runat="server" Font-Bold="True"
                        Text="Indicadores Financieros Proyectados"></asp:Label>
                    <br />
                    <asp:Table ID="T_Indicadores" runat="server" Width="100%">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="B_ActualizarIndicador" runat="server" Text="Actualizar Indicador"
                        OnClick="B_ActualizarIndicador_Click" Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
