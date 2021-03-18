<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="EvaluacionIndicadoresFinancieros.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.EvaluacionFinanciera.EvaluacionIndicadoresFinancieros" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Desea eliminar este indicador?');
        };        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <uc1:EncabezadoEval runat="server" id="EncabezadoEval" /> 
    <br />
    <table border="0" width="100%" style="background-color: White">
        <tr>
            <td class="auto-style5">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <div class="help_container">
                                <div onclick="textoAyuda({titulo: 'Indicadores Financieros', texto: 'Indicadores'});">
                                    <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                    &nbsp; <strong>Indicadores Financieros:</strong>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="div_Post_It1" runat="server" visible="false">
                                <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="FuentesFinanciacion" _txtTab="1" _mostrarPost="false" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" width="100%" style="background-color: White">
        <tr>
            <td>
                <br />
                <asp:ImageButton ID="ImageB" runat="server" PostBackUrl="~/FONADE/evaluacion/CatalogoIndicador.aspx"
                    ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="ImageB_Click" 
                    Visible="False" />&nbsp;
                <asp:LinkButton ID="LB_InsertarIndicadores" runat="server" Text="Agregar Indicador" OnClick="LB_InsertarIndicadores_Click"  Visible="False" />
                <br />
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="1">
                <asp:GridView ID="gv_evaluacionindicadores" runat="server" CssClass="Grilla" OnRowCommand="gv_evaluacionindicadores_RowCommand"
                    AutoGenerateColumns="false" EmptyDataText="No hay información disponible." Width="100%"
                    OnRowDataBound="gv_evaluacionindicadores_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkeliminar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("id_indicador") %>'
                                    CommandName="eliminar" CausesValidation="false" Visible="false">
                                    <asp:Image ID="imgeditar" ImageUrl="../../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                        Visible="false" ToolTip="Eliminar Indicador" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripción" SortExpression="Descripcion">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk_btn_descripcion" Text='<%# Eval("Descripcion") %>' runat="server"
                                    CausesValidation="false" CommandName="Modificar" CommandArgument='<%# Eval("id_Indicador")+ ";" + Eval("Descripcion") %>'
                                    Enabled="false" Style="text-decoration: none;" ForeColor="Black" />
                                <asp:HiddenField ID="oculto" runat="server" Value='<%# Eval("Protegido") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Valor" SortExpression="Valor">
                            <ItemTemplate>
                                <asp:Label ID="lt_valor" runat="server" Text='<%# Eval("Valor1") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Content>
