<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionConclusion.aspx.cs"  
    Inherits="Fonade.FONADE.evaluacion.EvaluacionConclusion" ValidateRequest="false" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        #div_observaciones
        {
            cursor: pointer;
        }
        .auto-style1 {
            width: 775px;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/ScriptsGenerales.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td style="text-align: right;" colspan="2">
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
                </td>
            </tr>
        </table>
        <div align="center" style="margin: 15px;">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="L_Viable" runat="server" Text="Viable:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Si</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        &nbsp;&nbsp;&nbsp;
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="2">
                    <div id="div_observaciones">
                        <a id="btnRedirect" idproducto='<%# Eval("Id_Producto") %>' style="text-decoration: none;
                            color: blue;">Ver Observaciones</a>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Conceptos de Justificación', texto: 'ConceptosJustificacion'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            &nbsp; <strong>Conceptos de Justificación:</strong>
                        </div>
                    </div>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="DDL_Conceptos" runat="server" Width="50%">
                    </asp:DropDownList>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;&nbsp;&nbsp;
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Justificación', texto: 'JustificacionEval'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            &nbsp; <strong>Justificación:</strong>
                        </div>
                    </div>
                    <br />
                </td>
                <td>
                    <div id="div_Post_It1" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="JustificacionEval" _txtTab="1" _mostrarPost="false"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="__TB_Justificacion" runat="server" Height="100px" TextMode="MultiLine"
                        Width="99%" ValidationGroup="guardar" />
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="__TB_Justificacion"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="guardar">Este Campo Es Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: Center;">
                    &nbsp;
                </td>
            </tr>
            <td class="auto-style1">
                <tr>
                    <td class="auto-style1">Empleos detectados en evaluación: 
                        <asp:TextBox ID="txtEmpleosDetectados" runat="server" Text="" EnableViewState="true"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </td>
            <tr>
                <td class="auto-style1">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="B_Guardar" runat="server" OnClick="B_Guardar_Click" Text="Guardar"
                        ValidationGroup="guardar" CausesValidation="True" CommandName="Update" Visible="False" />
                </td>
            </tr>
        </table>
    </div>
    </form>
    <script type="text/javascript">
        $("#div_observaciones").click(function (event) {
            event.preventDefault();
            $.fn.windowopen('../evaluacion/VerObservacionesEvaluacion.aspx', 735, 735, 'no', 1, 'no');
        });
    </script>
</body>
</html>
