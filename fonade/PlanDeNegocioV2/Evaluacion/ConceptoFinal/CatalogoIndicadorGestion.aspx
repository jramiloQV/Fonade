<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="CatalogoIndicadorGestion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.CatalogoIndicadorGestion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 70%;
            text-align: center;
            margin: 0px auto;
        }

        .auto-style2 {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </asp:ToolkitScriptManager>
            <div align="center">
                <br />
                <table style="border: 1px solid black;">
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td colspan="3" style="text-align: center">
                                        <asp:Label ID="L_NUEVOINDICADOR" runat="server" Text="NUEVO INDICADOR" Font-Bold="true" /><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="L_TipoIndicador" runat="server" Text="Tipo Indicador:" Font-Bold="True" />
                                    </td>
                                    <td style="width: 4px"></td>
                                    <td>
                                        <asp:DropDownList ID="DD_TipoIndicador" runat="server" ClientIDMode="Static" OnSelectedIndexChanged="DD_TipoIndicador_SelectedIndexChanged" AutoPostBack="true" />
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="udpIndicador" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="L_Indicador" runat="server" Text="Indicador:" Font-Bold="True" Visible="false" ClientIDMode="Static" />
                                            </td>
                                            <td style="width: 4px"></td>
                                            <td>
                                                <input type="text" runat="server" id="txtNumerador" name="txtNumerador" placeholder="Numerador" visible="false" /><br />
                                                <input type="text" runat="server" id="txtDenominador" name="txtDenominador" placeholder="Denominador" visible="false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="L_Aspecto" runat="server" Text="Aspecto:" Font-Bold="True" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="TB_Aspecto" runat="server" ClientIDMode="Static" TextMode="MultiLine" Height="80px" Width="167" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="L_FechaSeguimiento" runat="server" Text="Fecha de Seguimiento:" Font-Bold="True" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="TB_fechaSeguimiento" runat="server" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="L_Descripcionindicador" runat="server" Text="Descripción del indicador:" Font-Bold="True" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="TB_Descripcion" runat="server" ClientIDMode="Static" Height="80px" TextMode="MultiLine" Width="167" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="L_RangoAceptable" runat="server" Text="Rango Aceptable:" Font-Bold="True" />
                                            </td>
                                            <td></td>
                                            <td>
                                                <asp:TextBox ID="TB_rango" runat="server" ClientIDMode="Static" Width="20%" MaxLength="3" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                                                <asp:Label ID="L_RangoAceptablePorcentaje" runat="server" Text="%" Font-Bold="True" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                
                                            </td>
                                            <td></td>
                                            <td style="text-align: right">
                                                <asp:Button ID="B_Crear" runat="server" Text="Crear" OnClick="B_Crear_Click" OnClientClick="return ValidateForm()" />&nbsp;&nbsp;&nbsp;
                                                <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClick="B_Cancelar_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="DD_TipoIndicador" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    <script type="text/javaascript">
        function SoloNumeros(input) {
            var num = input.value.replace(/\./g, '');
            if (!isNaN(num)) {
                num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
                num = num.split('').reverse().join('').replace(/^[\.]/, '');
                input.value = num;
            }

            else {
                alert('Solo se permiten numeros');
                input.value = input.value.replace(/[^\d\.]*/g, '');
            }
        }

        function ValidateForm() {
            if (document.getElementById('DD_TipoIndicador').value == '0') {
                document.getElementById('DD_TipoIndicador').focus;
                alert('Debe seleccionar un tipo de indicador');
                return false;
            }

            if (document.getElementById('TB_Numerador').value == "") {
                document.getElementById('TB_Numerador').focus;
                alert('Debe ingresar el indicador NUMERADOR');
                return false;
            }

            if (document.getElementById('DD_TipoIndicador').value == '1') {
                if (document.getElementById('TB_Denominador').value == "") {
                    document.getElementById('TB_Denominador').focus;
                    alert('Debe ingresar el indicador DENOMINADOR');
                    return false;
                }
            }

            if (document.getElementById('TB_Aspecto').value == "") {
                document.getElementById('TB_Aspecto').focus;
                alert('Debe ingresar el aspecto');
                return false;
            }

            if (document.getElementById('TB_fechaSeguimiento').value == "") {
                document.getElementById('TB_fechaSeguimiento').focus;
                alert('Debe ingresar un periodo de seguimiento');
                return false;
            }

            if (document.getElementById('TB_Descripcion').value == "") {
                document.getElementById('TB_Descripcion').focus;
                alert('Debe ingresar la descripción del indicador');
                return false;
            }

            if (document.getElementById('TB_rango').value == "") {
                document.getElementById('TB_rango').focus;
                alert('Debe ingresar el rango aceptable');
                return false;
            }
        }
    </script>
</asp:Content>
