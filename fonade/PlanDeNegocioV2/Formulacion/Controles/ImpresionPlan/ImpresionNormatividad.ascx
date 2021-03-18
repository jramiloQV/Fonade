<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionNormatividad.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionNormatividad" %>
<asp:Panel ID="pnltabNormatividad" runat="server">
    <div class="ImpresionSeccion">
        <label>3 - Normatividad y condiciones técnicas</label>
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>12. Describa la normatividad que debe cumplirse para el portafolio definido anteriormente: Identificación de la norma, procesos, costos y tiempos asociados al cumplimiento de la normatividad.</label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <br />
        <label>Normatividad empresarial (constitución empresa):</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblNormEmpresa" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Normatividad tributaria:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblNormTribu" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Normatividad técnica (Permisos, licencias de funcionamiento, registros, reglamentos):</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblNormTecnica" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Normatividad laboral:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblNormLaboral" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Normatividad ambiental:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblNormAmbiental" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>Registro de marca – Propiedad intelectual:</label>
        <br />
    </div>
    <div>
        <asp:Label ID="lblMarcaProp" runat="server"></asp:Label>
        <br />
    </div>
    <div class="ImpresionSubSeccion">
        <label>13. Describa las condiciones técnicas más importantes que se requieren para la operación del negocio.</label>
        <br />
    </div>
   <div>
        <asp:Label ID="lblPregunta13" runat="server"></asp:Label>
        <br />
    </div>
    <br />
</asp:Panel>

