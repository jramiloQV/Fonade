<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPpalEstructura.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPpalEstructura" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPlanCompras.ascx" TagPrefix="uc1" TagName="ImpresionPlanCompras" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionCostosProd.ascx" TagPrefix="uc2" TagName="ImpresionCostosProd" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionCostosAdmin.ascx" TagPrefix="uc3" TagName="ImpresionCostosAdmin" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionIngresosEf.ascx" TagPrefix="uc4" TagName="ImpresionIngresosEf" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionEgresos.ascx" TagPrefix="uc5" TagName="ImpresionEgresos" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionCapitalTrabajo.ascx" TagPrefix="uc6" TagName="ImpresionCapitalTrabajo" %>

<div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
    <label>VIII - Estructura financiera</label>
</div>
<div>
    <uc1:ImpresionPlanCompras runat="server" ID="ImpresionPlanCompra" Visible="false"/>
    <uc2:ImpresionCostosProd runat="server" ID="ImpresionCostosProduccion" Visible ="false" />
    <uc3:ImpresionCostosAdmin runat="server" ID="ImpresionCostosAdministrativos" Visible="false" />
    <uc4:ImpresionIngresosEf runat="server" ID="ImpresionIngresosEstrucFin" Visible="false" />
    <uc5:ImpresionEgresos runat="server" ID="ImpresionEgresosEstrucFin" Visible="false" />
    <uc6:ImpresionCapitalTrabajo runat="server" ID="ImpresionCapitalTrab" Visible="false" />
</div>