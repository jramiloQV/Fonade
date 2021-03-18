<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="VerObservacionesEvaluacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.VerObservacionesEvaluacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
        .clasemas
        {
            color: #00468f;
        }
        .fondo
        {
            background-color: #00468f;
            color: white;
        }
        .panelmeses
        {
            margin: 0px auto;
            text-align: left;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <table class="auto-style1">
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div style="text-align: center;">
                        <asp:Panel ID="P_Observaciones" runat="server">
                            <asp:Table ID="T_Observaciones" runat="server" CssClass="panelmeses">
                            </asp:Table>
                        </asp:Panel>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
