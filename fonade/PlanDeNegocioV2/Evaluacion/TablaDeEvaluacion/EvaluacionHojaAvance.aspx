<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionHojaAvance.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.TablaDeEvaluacion.EvaluacionHojaAvance" %>

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

        .auto-style2 {
            width: 91px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%; height: auto; overflow-x: scroll;">
            <table class="Grilla">
                <tr style="background-color: #00468F; color: #FFFFFF; font-weight: bold; text-align: center;">
                    <td>Lectura del plan de negocio</td>
                    <td>Solicitud de información al emprendedor</td>
                    <td colspan="18">Tabla de Evaluación
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
                    <td></td>
                    <td></td>
                    <td colspan="2">¿Quién es el protagonista?</td>
                    <td colspan="3">¿Existe oportunidad en el mercado?</td>
                    <td colspan="3">¿Cuál es mi solución?</td>
                    <td colspan="4">¿Cómo desarrollo mi solución?</td>
                    <td colspan="3">¿Cuál es el futuro de mi negocio?</td>
                    <td>¿Qué riesgos enfrento?</td>
                    <td colspan="2">Resumen ejecutivo</td>
                    <td></td>
                    <td></td>
                    <td>Viabilidad</td>
                    <td>Indicadores de Gestión</td>
                    <td>&nbsp;</td>
                    <td></td>
                </tr>
                <tr style="text-align: center;">
                    <td></td>
                    <td></td>
                    <td>Identificación del mercado objetivo</td>
                    <td>Necesidad de los clientes</td>
                    <td>Fuerza del mercado</td>
                    <td>Tendencias del mercado</td>
                    <td>Competencia</td>
                    <td>Propuesta de valor</td>
                    <td>Validación del mercado</td>
                    <td>Antecedentes</td>
                    <td>Condiciones de comercialización</td>
                    <td>Normatividad</td>
                    <td class="auto-style2">Operación del negocio</td>
                    <td>Equipo de trabajo</td>
                    <td>Estrategias de comercialización</td>
                    <td>Periodo improductivo</td>
                    <td>Sostenibilidad</td>
                    <td>Riesgos</td>
                    <td>Plan operativo</td>
                    <td>Indicadores de seguimiento</td>
                    <td></td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                    <td>&nbsp;</td>
                    <td></td>
                </tr>
                <tr style="text-align: center;">
                    <td>
                        <asp:CheckBox ID="CheckLectura" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckSolicitud" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckIdentificacion" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckNecesidad" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckFuerza" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckTendencias" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckCompetencia" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckPropuesta" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckValidacion" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckAntecedente" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckCondicion" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckNormatividad" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckOperacion" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckEquipo" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckEstrategia" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckPeriodo" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckSostenibilidad" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckRiesgos" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckPlan" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckIndiSegui" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckModelo" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckIndiRenta" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckViabilidad" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckIndiGestion" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckPlanOperativo" runat="server" />
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckInforme" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div style="text-align: center; width: 100%;">
            <asp:Button ID="btnEnviar" runat="server" Text="Enviar" OnClick="btnEnviar_Click" />&nbsp;<asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" OnClick="btnLimpiar_Click" />
        </div>
    </form>
</body>
</html>
