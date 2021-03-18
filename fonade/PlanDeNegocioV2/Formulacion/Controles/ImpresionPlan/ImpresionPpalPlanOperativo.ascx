<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPpalPlanOperativo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPpalPlanOperativo" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPlanOperativo.ascx" TagPrefix="uc1" TagName="ImpresionPlanOperativo" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionMetasSociales.ascx" TagPrefix="uc2" TagName="ImpresionMetasSociales" %>


<div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
    <label>IX - Plan operativo</label>
</div>
<div>
    <uc1:ImpresionPlanOperativo runat="server" ID="ImpresionPlanOper" Visible="false" />
    <uc2:ImpresionMetasSociales runat="server" ID="ImpresionMetaSocial" Visible="false" />
</div>


