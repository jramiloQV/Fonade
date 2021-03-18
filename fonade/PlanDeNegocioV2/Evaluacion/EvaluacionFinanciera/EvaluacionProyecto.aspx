<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionProyecto.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.EvaluacionProyecto" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server" >
    <div>       
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />                
        <br />
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td class="auto-style5">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 50%">
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Evaluación del Proyecto', texto: 'EvaluacionProyecto'});">
                                        <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                        &nbsp; <strong>Evaluación del Proyecto:</strong>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div id="div_Post_It1" runat="server" visible="false">
                                    <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="EvaluacionProyecto" _txtTab="1" _mostrarPost="false"/>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="auto-style1">
            <tr>
                <td>
                    <table class="auto-style1">
                        <tr>
                            <td style="width: 25%">
                                <asp:Label ID="L_TiempoProyeccion" runat="server" Text="Tiempo de Proyección"></asp:Label>
                            </td>
                            <td style="width: 25%">
                                <asp:DropDownList ID="DD_TiempoProyeccion" runat="server">
                                    <asp:ListItem Value="3">3</asp:ListItem>
                                    <asp:ListItem Value="4">4</asp:ListItem>
                                    <asp:ListItem Value="5">5</asp:ListItem>
                                    <asp:ListItem Value="6">6</asp:ListItem>
                                    <asp:ListItem Value="7">7</asp:ListItem>
                                    <asp:ListItem Value="8">8</asp:ListItem>
                                    <asp:ListItem Value="9">9</asp:ListItem>
                                    <asp:ListItem Value="10">10</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 50%;">
                                <asp:Button ID="B_Guardar" runat="server" Text="Guardar" OnClick="B_Guardar_Click"
                                    Visible="false" OnClientClick="return confirm('Si cambia el tiempo de proyección se borraran todos los valores proyectados.  Esta seguro de realizar este cambio?')" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Table ID="T_Supuestos" runat="server" Width="100%">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="B_ActualizarSupuesto" runat="server" Text="Actualizar Supuesto" OnClick="B_ActualizarSupuesto_Click"
                        Visible="False" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Label ID="L_IndicadoresFinancierosProyectados" runat="server" Font-Bold="True"
                        Text="Indicadores Financieros Proyectados"></asp:Label>
                    <br />
                    <asp:Table ID="T_Indicadores" runat="server" Width="100%">
                    </asp:Table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <asp:Button ID="B_ActualizarIndicador" runat="server" Text="Actualizar Indicador"
                        OnClick="B_ActualizarIndicador_Click" Visible="False" />
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
