<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPpalSolucion.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPpalSolucion" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionSolucion.ascx" TagPrefix="uc1" TagName="ImpresionSolucion" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionFichaTecnica.ascx" TagPrefix="uc2" TagName="ImpresionFichaTecnica" %>

<div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
    <label>III. ¿Cual es mi Solución?</label>
</div>
<div>
    <uc1:ImpresionSolucion runat="server" ID="ImpresionSolucion" Visible="false" />
    <uc2:ImpresionFichaTecnica runat="server" ID="ImpresionFichaTec" Visible="false"/>
</div>
