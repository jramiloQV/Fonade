<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerImpresion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.ImpresionPlanNegocio.VerImpresion" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionProtagonista.ascx" TagPrefix="uc1" TagName="ImpresionProtagonista" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionOportunidad.ascx" TagPrefix="uc2" TagName="ImpresionOportunidad" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPpalSolucion.ascx" TagPrefix="uc3" TagName="ImpresionPpalSolucion" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPPalDesarrollo.ascx" TagPrefix="uc4" TagName="ImpresionPPalDesarrollo" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPpalFuturo.ascx" TagPrefix="uc5" TagName="ImpresionPpalFuturo" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionRiesgos.ascx" TagPrefix="uc6" TagName="ImpresionRiesgos" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionResumenEjec.ascx" TagPrefix="uc7" TagName="ImpresionResumenEjec" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/ImpresionPlan/ImpresionPpalEstructura.ascx" TagPrefix="uc9" TagName="ImpresionPpalEstructura" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">

        function imprimir() {

            var divToPrint = document.getElementById('contentPrincipal');
            var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 1000);
        }

    </script>
</head>
<body onload="imprimir()">
    <form id="form1" runat="server">
        <div id="contentPrincipal">
            <h1>
                <asp:Label ID="lblimpresion" runat="server" Text=""></asp:Label>
            </h1>
            <br />
            <br />
            <asp:Panel ID="pnlinfoproyecto" runat="server" CssClass="Grilla">
                <table>
                    <thead>
                        <tr>
                            <th colspan="2">PROYECTO
                            </th>
                        </tr>
                    </thead>
                    <tr>
                        <td>Nombre :
                        </td>
                        <td>
                            <asp:Label ID="lblnombreproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Institucion :
                        </td>
                        <td>
                            <asp:Label ID="lblinstitucionproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Subsector :
                        </td>
                        <td>
                            <asp:Label ID="lblsubsectorproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Ciudad :
                        </td>
                        <td>
                            <asp:Label ID="lblciudadproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Recursos solicitados al fondo (SMLV):
                        </td>
                        <td>
                            <asp:Label ID="lblrecursosproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Fecha de Creación :
                        </td>
                        <td>
                            <asp:Label ID="lblfechacreacionproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Sumario :
                        </td>
                        <td>
                            <asp:Label ID="lblsumarioproyecto" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <br />
            <br />
            <asp:Panel ID="pnlImpresionTab" runat="server" CssClass="Grilla">
                <uc1:ImpresionProtagonista runat="server" ID="ImpresionProtagonista" Visible="false" />
                <uc2:ImpresionOportunidad runat="server" ID="ImpresionOportunidad" Visible="false"/>
                <uc3:ImpresionPpalSolucion runat="server" ID="ImpresionPPalSolucion" Visible="false" />
                <uc4:ImpresionPPalDesarrollo runat="server" ID="ImpresionPpalDesarrollo" Visible="false"/>
                <uc5:ImpresionPpalFuturo runat="server" ID="ImpresionPPalFuturo" Visible="false"/>
                <uc6:ImpresionRiesgos runat="server" ID="ImpresionRiesgo" Visible="false"/>
                <uc7:ImpresionResumenEjec runat="server" ID="ImpresionResumen" Visible="false"/>
                <uc9:ImpresionPpalEstructura runat="server" ID="ImpresionPPalEstructura" Visible="false"/>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
