<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="Conclusion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Conclusion" ValidateRequest="false" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #div_observaciones
        {
            cursor: pointer;
        }
        .auto-style1 {
            width: 775px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />     
        <br />
        <div align="center" style="margin: 15px;">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="L_Viable" runat="server" Text="Viable:"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                            <asp:ListItem Value="0">No</asp:ListItem>
                            <asp:ListItem Value="1">Si</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        &nbsp;&nbsp;&nbsp;
        </div>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td colspan="2">
                    <div id="div_observaciones">                        
                        <asp:LinkButton ID="lnkObservaciones" ForeColor="Blue" runat="server" Text="Ver observaciones" OnClick="lnkObservaciones_Click" ></asp:LinkButton>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    &nbsp;&nbsp;&nbsp;
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Conceptos de Justificación', texto: 'ConceptosJustificacion'});">
                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            &nbsp; <strong>Conceptos de Justificación:</strong>
                        </div>
                    </div>
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:DropDownList ID="DDL_Conceptos" runat="server" Width="50%">
                    </asp:DropDownList>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td class="auto-style1">
                    &nbsp;&nbsp;&nbsp;
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Justificación', texto: 'JustificacionEval'});">
                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            &nbsp; <strong>Justificación:</strong>
                        </div>
                    </div>
                    <br />
                </td>
                <td>
                    <div id="div_Post_It1" runat="server" visible="false">
                        <uc1:post_it ID="Post_It1" runat="server" _txtCampo="JustificacionEval" _txtTab="1" _mostrarPost="false" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="__TB_Justificacion" runat="server" Height="100px" TextMode="MultiLine"
                        Width="99%" ValidationGroup="guardar" />
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="__TB_Justificacion"
                        ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="guardar">Este Campo Es Requerido</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="text-align: Center;">
                    &nbsp;
                </td>
            </tr>
            <td class="auto-style1" runat="server" visible="false">
                <tr>
                    <td class="auto-style1">Empleos detectados en evaluación: 
                        <asp:TextBox ID="txtEmpleosDetectados" runat="server" Text="" EnableViewState="true"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </td>
            <tr>
                <td class="auto-style1">
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="B_Guardar" runat="server" OnClick="B_Guardar_Click" Text="Guardar"
                        ValidationGroup="guardar" CausesValidation="True" CommandName="Update" Visible="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
