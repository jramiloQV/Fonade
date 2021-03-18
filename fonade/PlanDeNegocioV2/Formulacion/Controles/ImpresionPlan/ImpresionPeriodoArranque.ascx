<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPeriodoArranque.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPeriodoArranque" %>
<asp:Panel ID="pnltabPeriodoArranque" runat="server">
    <div class="ImpresionSeccion">
        <label>Período de arranque e improductivo</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>19. ¿Cuál es el período de arranque del proyecto (meses)?</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblArranque" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>20. ¿Cuál es el período improductivo (meses) que exige el primer ciclo de producción?</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblImproductivo" runat="server"></asp:Label>
        <br />
    </div>
</asp:Panel>
                
                      
                        

