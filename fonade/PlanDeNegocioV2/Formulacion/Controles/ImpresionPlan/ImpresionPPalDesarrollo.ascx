<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPPalDesarrollo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPPalDesarrollo" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionIngresos.ascx" TagPrefix="uc1" TagName="ImpresionIngresos" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionProyeccion.ascx" TagPrefix="uc2" TagName="ImpresionProyeccion" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionNormatividad.ascx" TagPrefix="uc3" TagName="ImpresionNormatividad" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionRequerimientos.ascx" TagPrefix="uc4" TagName="ImpresionRequerimientos" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionProduccion.ascx" TagPrefix="uc5" TagName="ImpresionProduccion" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionProductividad.ascx" TagPrefix="uc6" TagName="ImpresionProductividad" %>






<div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
    <label>IV. ¿Cómo desarrollo mi solución?</label>
</div>
<div>
    <uc1:ImpresionIngresos runat="server" id="ImpresionIngreso" Visible="false" />
    <uc2:ImpresionProyeccion runat="server" id="ImpresionProyecciones" Visible="false" />
    <uc3:ImpresionNormatividad runat="server" ID="ImpresionNormatividades" Visible="false" />
    <uc4:ImpresionRequerimientos runat="server" ID="ImpresionRequerimientosNeg" Visible="false" />
    <uc5:impresionproduccion runat="server" id="ImpresionProduccions" Visible="false" />
    <uc6:ImpresionProductividad runat="server" id="ImpresionProducti" Visible="false" />
</div>