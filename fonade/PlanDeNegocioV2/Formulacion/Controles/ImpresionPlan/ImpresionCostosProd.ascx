<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionCostosProd.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionCostosProd" %>
<style>
    table {
        border: solid #000 !important;
        border-width: 1px 0 0 1px !important;
    }

    th, td {
        border: solid #000 !important;
        border-width: 0 1px 1px 0 !important;
    }

    .tituloCelda {
        font-weight: bold;
    }
</style>

<asp:Panel ID="pnltabCostosProd" runat="server">
    <div class="ImpresionSeccion">
        <br />
        <label>Costos de Producción</label>
        <br />
    </div>
    <div>
        <br />
        <asp:Table ID="Tabla_Costos" runat="server" CssClass="Grilla" Width="100%">
        </asp:Table>
        <br />
        <asp:Table ID="Tabla_Proyecciones_1" runat="server" CssClass="Grilla" Width="100%">
        </asp:Table>
        <br />
        <asp:Table ID="Tabla_Proyecciones_2" runat="server" CssClass="Grilla" Width="100%">
        </asp:Table>
        <br />
        <br />
    </div>
</asp:Panel>
