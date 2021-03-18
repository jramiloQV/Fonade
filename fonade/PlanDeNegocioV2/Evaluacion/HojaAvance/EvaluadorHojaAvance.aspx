<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluadorHojaAvance.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance.EvaluadorHojaAvance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Hoja de Evaluación por Coordinador</title>
    <link href="../../../Styles/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" style="background-color: #FFFFFF">
        <div style="background-color: #FFFFFF; width: 100%;">
            <asp:DataList ID="DtSeguimiento" runat="server" Width="100%" BackColor="White">
                <HeaderTemplate>
                    <table border="0" width="100%" style="border: 1px solid #C0C0C0">
                        <tr align="center" bgcolor="#D1D8E2" class="Titulo">
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">No.</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Plan de Negocio</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Evaluador</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Lectura del Plan de Negocio</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Solicitud de Información al Emprendedor</td>
                            <td colspan="18" style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Tabla de Evaluación</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Modelo Financiero</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Indices de Rentabilidad </td>
                            <td colspan="2" style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Concepto y Recomendaciones</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Plan Operativo</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Informe de Evaluación</td>
                        </tr>
                        <tr align="center" class="Titulo">
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td colspan="2" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">¿Quién es el Protagonista?
                            </td>
                            <td colspan="3" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">Comerciales
                            </td>
                            <td colspan="3" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">¿Cuál es mi Solución?</td>
                            <td colspan="4" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">¿Cómo Desarrollo mi Solución?</td>
                            <td colspan="3" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">¿Cuál es el futuro de mi Negocio?
                            </td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">¿Qué Riesgos Enfrento?
                            </td>
                            <td colspan="2" style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">Resumen Ejecuivo</td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">Viabilidad</td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;">Indicadores de Gestión</td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #D1D8E2; padding: 5px; font-weight: bold;"></td>
                        </tr>
                        <tr align="center" bgcolor="#EDEFF3" class="Titulo">
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Identificación del Mercado Objetivo</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Necesidad de los Clientes</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Fuerza del Mercado</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Tendencias del Mercado</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Competencia</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Propuesta de Valor</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Validación del Mercado</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Antecedentes</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Condiciones de Comercialización</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Normatividad</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Operación del Negocio</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Equipo de Trabajo</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Estrategias de Comercialización</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Periodo Improductivo</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Sostenibilidad</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Riesgos</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Plan Operativo</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;">Indicadores de Seguimiento</td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                            <td style="border: 1px solid #FFFFFF; padding: 5px; font-weight: bold;"></td>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="text-align: center; font-size: x-small;">
                        <td>
                            <asp:Label ID="lproyecto" runat="server" Text='<%# Eval("IdProyecto") %>' /></td>
                        <td style="text-align: left; padding-left: 5px;">
                            <asp:Label runat="server" Text='<%# Eval("NomProyecto") %>' Width="200" /></td>
                        <td style="text-align: left; padding-left: 5px;">
                            <asp:Label runat="server" Text='<%# Eval("Evaluador") %>' Width="200" /></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("LecturaPlan")%>' Enabled="False" /></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("SolicitudInformacion")%>' Enabled="False" /></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("IdentificacionMercado")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("NecesidadClientes")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("FuerzaMercado")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("TendenciasMercado")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Competencia")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("PropuestaValor")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("ValidacionMercado")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Antecedentes")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("CondicionesComercializacion")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Normatividad")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("OperacionNegocio")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("EquipoTrabajo")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("EstrategiasComercializacion")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("PeriodoImproductivo")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Sostenibilidad")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Riesgos")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("PlanOperativo")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("IndicadoresSeguimiento")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Modelo")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("IndiceRentabilidad")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("Viabilidad")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("IndicadoresGestion")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("PlanOperativo2")%>' Enabled="False"></asp:CheckBox></td>
                        <td>
                            <asp:CheckBox runat="server" Checked='<%#Eval("InformeEvaluacion")%>' Enabled="False"></asp:CheckBox></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <p align="center">
                <asp:Label ID="lblmensaje" runat="server" Font-Size="XX-Large" Visible="False"
                    Text="No se encontraron  Datos" />
            </p>
        </div>
    </form>
</body>
</html>
