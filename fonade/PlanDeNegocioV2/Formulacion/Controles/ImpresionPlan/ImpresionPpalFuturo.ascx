<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPpalFuturo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPpalFuturo" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPeriodoArranque.ascx" TagPrefix="uc1" TagName="ImpresionPeriodoArranque" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionEstrategia.ascx" TagPrefix="uc2" TagName="ImpresionEstrategia" %>


<div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
    <label>V. ¿Cuál es el Futuro de mi Negocio?</label>
</div>
<div>
    <uc2:ImpresionEstrategia runat="server" id="ImpresionEstrateg" Visible="false" />
    <uc1:ImpresionPeriodoArranque runat="server" id="ImpresionPeriodo" Visible="false" />
</div>