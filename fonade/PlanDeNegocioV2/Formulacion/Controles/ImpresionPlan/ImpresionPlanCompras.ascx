<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPlanCompras.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPlanCompras" %>
<style>
    table {
        border: solid #000 !important;
        border-width: 1px 0 0 1px !important;
    }

    th, td {
        border: solid #000 !important;
        border-width: 0 1px 1px 0 !important;
    }

    .alineacion td:nth-child(n+4) {
        text-align: right;
    }

    .tituloCelda {
        font-weight: bold;
    }
</style>

<asp:Panel ID="pnltabPlanCompras" runat="server">
    <div class="ImpresionSeccion">
        <label>Plan de Compras</label>
        <br />
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Consumos por Unidad de Producto:</label>
        <br />
    </div>
    <div>
        <br />
        <asp:Table ID="tbl" runat="server" CssClass="Grilla" Width="100%">
        </asp:Table>
        <br />
    </div>
</asp:Panel>

