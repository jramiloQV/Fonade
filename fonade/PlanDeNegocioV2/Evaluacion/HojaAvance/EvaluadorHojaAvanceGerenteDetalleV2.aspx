<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="EvaluadorHojaAvanceGerenteDetalleV2.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.HojaAvance.EvaluadorHojaAvanceGerenteDetalleV2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:LinkButton ID="LB_Volver" runat="server" Text="Regresar a la Lista de Coordinadores" PostBackUrl="~/FONADE/evaluacion/EvaluadorHojaAvanceGerente.aspx"></asp:LinkButton>
    <br />
    <br />
    <div style="overflow: scroll; padding-left: 20px; padding-right: 20px; padding-bottom: 20px; padding-top: 20px; margin: 20px; width: 650px;">
        <asp:DataList ID="DtSeguimiento" runat="server" ShowFooter="False" Style="font-weight: 700; text-align: center;" Width="88%" border="1" CellSpacing="0" CellPadding="2" BorderColor="666633">
            <HeaderTemplate>
                <thead align="center" bgcolor="#D1D8E2">
                    <tr>
                        <td>&nbsp;</td>
                        <td>No.</td>
                        <td>Plan de Negocio</td>
                        <td>Evaluador</td>
                        <td>Lectura del Plan de Negocio</td>
                        <td>Solicitud de Información al Emprendedor</td>
                        <td colspan="18">Tabla de Evaluación</td>
                        <td>Modelo Financiero</td>
                        <td>Indices de Rentabilidad </td>
                        <td colspan="2">Concepto y Recomendaciones</td>
                        <td>Plan Operativo</td>
                        <td>Informe de Evaluación</td>
                    </tr>
                    <tr bgcolor="#EDEFF3">
                        <td>&nbsp;</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td colspan="2">¿Quién es el Protagonista?</td>
                        <td colspan="3">¿Existe Oportunidad en el Mercado?</td>
                        <td colspan="3">¿Cuál es mi Solución?</td>
                        <td colspan="4">¿Cómo Desarrollo mi Solución?</td>
                        <td colspan="3">¿Cuál es el futuro de mi Negocio?
                        </td>
                        <td>¿Qué Riesgos Enfrento?
                        </td>
                        <td colspan="2">Resumen Ejecuivo</td>
                        <td></td>
                        <td></td>
                        <td>Viabilidad</td>
                        <td>Indicadores de Gestión</td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td>Identificación del Mercado Objetivo</td>
                        <td>Necesidad de los Clientes</td>
                        <td>Fuerza del Mercado</td>
                        <td>Tendencias del Mercado</td>
                        <td>Competencia</td>
                        <td>Propuesta de Valor</td>
                        <td>Validación del Mercado</td>
                        <td>Antecedentes</td>
                        <td>Condiciones de Comercialización</td>
                        <td>Normatividad</td>
                        <td>Operación del Negocio</td>
                        <td>Equipo de Trabajo</td>
                        <td>Estrategias de Comercialización</td>
                        <td>Periodo Improductivo</td>
                        <td>Sostenibilidad</td>
                        <td>Riesgos</td>
                        <td>Plan Operativo</td>
                        <td>Indicadores de Seguimiento</td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </thead>
            </HeaderTemplate>
            <ItemTemplate>
                <td>
                    <asp:Label ID="lproyecto" runat="server" Text='<%# Eval("IdProyecto") %>' /></td>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("NomProyecto") %>' /></td>
                <td>
                    <asp:Label runat="server" Text='<%# Eval("Evaluador") %>' /></td>
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
            </ItemTemplate>
        </asp:DataList>
        <p align="center">
            <asp:Label ID="lblmensaje" runat="server" Font-Size="XX-Large" Visible="False"
                Text="No se encontraron  Datos" />
        </p>
    </div>


</asp:Content>
