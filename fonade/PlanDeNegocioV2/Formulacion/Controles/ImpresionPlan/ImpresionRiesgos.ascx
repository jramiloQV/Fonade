<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionRiesgos.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionRiesgos" %>
<asp:Panel ID="pnltabRiesgos" runat="server">
    <div class="ImpresionSeccion ImpresionTitulo" style="text-align: center">
        <label>VI. ¿Qué Riesgos Enfrento?</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>¿Qué actores externos son críticos para la ejecución del negocio? Indique el nombre y su rol en la ejecución.</label>
    </div>
    <div>
        <br />
        <asp:Label ID="lblActores" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>¿Qué factores externos pueden afectar la operación del negocio, y cuál es el plan de acción para mitigar estos riesgos?</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblFactores" runat="server"></asp:Label>
        <br />
    </div>
    <br />
</asp:Panel>