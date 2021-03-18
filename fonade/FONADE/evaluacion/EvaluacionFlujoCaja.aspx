<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionFlujoCaja.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionFlujoCaja" ValidateRequest="false" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
        #Label1{
            font-weight:bold;
            font-size:22px;
        }
    </style>
</head>
<body style="color: #696969; width: 100%;">
    <form id="form1" runat="server">
    <div>
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
                        <tbody>
                            <tr>
                                <td style="width: 50%">
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Flujo de Caja', texto: 'FlujoCaja'});">
                                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            &nbsp; <strong>Flujo de Caja:</strong>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div id="div_Post_It1" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="FlujoCaja" _txtTab="1" _mostrarPost="false" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="L_des" runat="server" Text="Flujo de Caja Proyectado. Cifras en Miles de Pesos"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Panel ID="P_FlujoCaja" runat="server">
                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Text="Conclusiones" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 99%">
            <tbody>
                <tr>
                    <td>
                        <asp:TextBox ID="TB_Conclusiones" runat="server" TextMode="MultiLine" Width="99%"
                            Height="150px" Visible="false" Style="margin: 5px;" MaxLength="1000" />
                        <div id="div_conclusiones" runat="server" style="width: 99%; height: 150px;" visible="false">
                        </div>
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="99%">
            <tr>
                <td>
                    <table style="width: 99%">
                        <tbody>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <asp:Button ID="B_Registar" runat="server" Text="Actualizar" OnClick="B_Registar_Click"
                                        Visible="false" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
