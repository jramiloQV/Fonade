<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionFlujoCaja.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.EvaluacionFlujoCaja" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <style type="text/css">
        .auto-style1
        {
            width: 100%;
        }
        #Label1{
            font-weight:bold;
            font-size:22px;
        }
    </style> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />  
        <br />
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td class="auto-style5">
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td style="width: 50%">
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Flujo de Caja', texto: 'FlujoCaja'});">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            &nbsp; <strong>Flujo de Caja:</strong>
                                        </div>
                                    </div>
                                </td>
                                <td>
                                    <div id="div_Post_It1" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="FlujoCaja" _txtTab="1" _mostrarPost="false" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="L_des" runat="server" Text="Flujo de Caja Proyectado. Cifras en Miles de Pesos"></asp:Label>
                                    <br />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    &nbsp;&nbsp;&nbsp;
                                    <asp:Panel ID="P_FlujoCaja" runat="server">
                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table width="100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td>
                                    <br />
                                    <asp:Label ID="Label1" runat="server" Text="Conclusiones" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 99%">
            <tbody>
                <tr>
                    <td>
                        <asp:TextBox ID="TB_Conclusiones" runat="server" TextMode="MultiLine" Width="99%"
                            Height="150px" Visible="false" Style="margin: 5px;" MaxLength="1000" />
                        <div id="div_conclusiones" runat="server" style="width: 99%; height: 150px;" visible="false">
                        </div>
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="99%">
            <tr>
                <td>
                    <table style="width: 99%">
                        <tbody>
                            <tr>
                                <td>
                                    <br />
                                    <br />
                                    <asp:Button ID="B_Registar" runat="server" Text="Actualizar" OnClick="B_Registar_Click"
                                        Visible="false" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
