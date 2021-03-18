<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionHojaAvance.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance.EvaluacionHojaAvance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <style type="text/css">
        htnl, body {
            background-color: white !important;
            background-image: none !important;
        }

        .TCenter {
            text-align: center;
        }
        .auto-style1 {
            width: 340px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%--<div style="text-align: center; font-weight: bold; color: #FFFFFF; background-color: #00468F; width: 100%;">Tabla de Evaluación</div>--%>
        <div style="width: 100%; height: auto; overflow-x: scroll;">
            <table class="Grilla">
                <tr style="background-color: #00468F; color: #FFFFFF; font-weight: bold; text-align: center;">
                    <td>Lectura del plan de negocio</td>
                    <td>Solicitud de información al emprendedor</td>
                    <td class="auto-style1">Tabla de Evaluación
                    </td>
                    <td>Modelo financiero
                    </td>
                    <td>Índices de rentabilidad
                    </td>
                    <td colspan="2">Concepto y recomendaciones</td>
                    <td>Plan operativo
                    </td>
                    <td>Informe de Evaluación
                    </td>
                </tr>
                <tr style="text-align: center;">
                    <td rowspan="2"></td>
                    <td rowspan="2"></td>
                    <td class="auto-style1" rowspan="2">
                        <table class="Grilla">
                            <tr>
                                <asp:Repeater ID="RepeaterTitulos" runat="server" DataSourceID="SqlDataTitulos">
                                    <ItemTemplate>
                                        <td style="text-align: center" colspan='<%#Eval("Conteo")%>'><%#Eval("Campo")%></td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                            <tr>
                                <asp:Repeater ID="RepeaterAspectos" runat="server" DataSourceID="SqlDataAspectos">
                                    <ItemTemplate>
                                        <td style="text-align: center"><%#Eval("Campo")%></td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                            <tr>
                                <asp:Repeater ID="RepeaterAvance" runat="server" DataSourceID="SqlDataAspectos">
                                    <ItemTemplate>
                                        <td style="text-align: center">
                                            <asp:CheckBox ID="CheckBoxAspecto" runat="server" IdCampo='<%#Eval("id_Campo")%>' Checked='<%#Bind("Avance")%>' Enabled='<%#Enabled(Eval("Avance"))%>' />
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </table>
                    </td>
                    <td rowspan="2"></td>
                    <td rowspan="2"></td>
                    <td>Viabilidad</td>
                    <td>Indicadores de Gestión</td>
                    <td rowspan="2"></td>
                    <td rowspan="2"></td>
                </tr>
                <tr style="text-align: center;">
                    <td></td>
                    <td></td>
                </tr>
            </table>




            <br />

            <asp:SqlDataSource ID="SqlDataTitulos" runat="server" ConnectionString="<%$ ConnectionStrings:FonadeConnectionString %>" SelectCommand="SP_HojaAvance" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter DefaultValue="1" Name="Opcion" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataAspectos" runat="server" ConnectionString="<%$ ConnectionStrings:FonadeConnectionString %>" SelectCommand="SP_HojaAvance" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:Parameter DefaultValue="2" Name="Opcion" Type="Int32" />
                    <asp:Parameter Direction="ReturnValue" Name="RETURN_VALUE" Type="Int32" />
                    <asp:QueryStringParameter DefaultValue="0" Name="IdProyecto" QueryStringField="codproyecto" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
        <br />
        <div style="text-align: center">
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click" />&nbsp;<asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
        </div>
    </form>
</body>

</html>
