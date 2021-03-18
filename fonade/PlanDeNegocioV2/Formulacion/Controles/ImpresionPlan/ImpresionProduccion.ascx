<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionProduccion.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionProduccion" %>
<asp:Panel ID="pnltabProduccion" runat="server">
    <div class="ImpresionSeccion">
        <label>5 - Producción</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>14.3. Detalle las condiciones técnicas de infraestructura: áreas requeridas y distribución de espacios. (Anexar mapa y /o plano)</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblPregunta143" runat="server"></asp:Label>
        <br />
    </div>
    <div class="divgen ImpresionSubSeccion">
        <div class="divleft">
            <br />
            <label class="tamlabel1">14.4. ¿Para la adquisición de algún activo, se tiene contemplado realizar importación? (SI / NO, justificación)</label>
            &nbsp;<asp:Label ID="lblSel144" runat="server" Font-Bold="true" Font-Size="Medium"></asp:Label>
            <br />
        </div>
    </div>
    <div>
        <asp:Label ID="lblPregunta144" runat="server"></asp:Label>
        <br />
    </div>

    <div class="ImpresionSubSeccion">
        <br />
        <label>14.4.1. Detalle los activos, países proveedores y tiempos estimados:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblPregunta1441" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>14.4.2. En caso de presentarse incremento en el valor del activo por factores como: tasa de cambio, reformas tributarias, etc. ¿Cómo financiará éste mayor valor?</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblPregunta1442" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>15. ¿Cuál es el proceso que se debe seguir para la producción del bien o prestación del servicio?</label>
        <br />
    </div>
    <div>
        <br />
        <asp:Panel ID="pnProcesosProducto" runat="server">
        </asp:Panel>
    </div>
    <br />
</asp:Panel>
