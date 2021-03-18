<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="Riesgos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.Riesgos" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/ecmascript">
        function url() {
            open("agregarRiesgo.aspx", "Agregar Riesgo", "width=800,height=600");
        }
    </script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla != 13);
        }
    </script>
    <style type="text/css">
        .sinlinea
        {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }
    </style>
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
                                <div onclick="textoAyuda({titulo: 'Riesgos Identificados y Mitigación', texto: 'RiesgoMitigacion'});">
                                    <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                    &nbsp; <strong>Riesgos Identificados y Mitigación</strong>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div id="div_Post_It1" runat="server" visible="false">
                                <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="RiesgoMitigacion" _txtTab="1" _mostrarPost="false"/>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                &nbsp;&nbsp;
                <asp:ImageButton ID="IB_AgregarIndicador" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                    OnClick="IB_AgregarIndicador_Click" />
                &nbsp;&nbsp;
                <asp:LinkButton ID="btn_agregar" runat="server" Text="Agregar Riesgo" OnClick="btn_agregar_Click1"
                     />
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                    DataSourceID="ObjectDataSource1" DataKeyNames="Id_Riesgo" OnRowDataBound="GridView1_RowDataBound"
                    AllowPaging="True" ForeColor="#666666" Width="100%" OnRowCommand="GridView1_RowCommand" EmptyDataText="No hay datos para mostrar.">
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkBLButton2" runat="server" CausesValidation="False" CommandName="Delete"
                                    Text="" OnClientClick="return confirm('desea eliminar el riesgo de la lista?')"
                                    CssClass="sinlinea">
                                    <asp:Image ID="LB_eliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CssClass="sinlinea" />
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="False" HeaderText="Riesgo">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkeditarRiesgo" runat="server" CausesValidation="False" CommandName="editar"
                                    Text='<%# Eval("Riesgo") %>' CommandArgument='<%# Eval("Id_Riesgo") %>'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Mitigacion" HeaderText="Mitigación" />
                    </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="resultado"
                    TypeName="Fonade.FONADE.evaluacion.EvaluacionRiesgos" DeleteMethod="eliminar">
                    <DeleteParameters>
                        <asp:Parameter Name="Id_Riesgo" Type="Int32" />
                    </DeleteParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
    </table>    
</asp:Content>
