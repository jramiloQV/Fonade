<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionSolucion.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionSolucion" %>
<asp:Panel ID="pnltabSolucion" runat="server">
    <div class="ImpresionSeccion">
        <label>1 - Solución</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>5. Describa la alternativa o solución que usted propone para satisfacer las necesidades señaladas en la pregunta 2:</label>
        <br />
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Concepto del negocio:</label>
    </div>
    <div>
        <asp:Label ID="lblConceptoNegocio" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Componente Innovador:</label>
        <br />
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Concepto del negocio:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblConceptoNegocio2" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Producto o servicio:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblProductoServicio" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Proceso:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblProceso" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>6. ¿Cómo validó la aceptación en el mercado de su proyecto (metodología y resultados)?</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblComoValido" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>7. Describa el avance logrado a la fecha para la puesta en marcha de su proyecto, en los aspectos: técnico - productivo, comercial y legal.</label>
        <br />
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Técnico - productivo:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblTecnicoproductivo" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Comercial:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblComercial" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Legal:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblLegal" runat="server"></asp:Label>
        <br />
    </div>
</asp:Panel>
