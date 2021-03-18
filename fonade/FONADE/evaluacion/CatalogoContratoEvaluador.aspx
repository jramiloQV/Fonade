<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoContratoEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoContratoEvaluador"  MasterPageFile="~/Master.Master"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_listaCoordEval" runat="server" 
    ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
    onselecting="lds_listaCoordEval_Selecting" >
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="100%">
              <tr>
                <td>
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
              </tr>
            </table>

            <table width="100%">
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style6">
                    <asp:ImageButton ID="ibtn_crearCntrato" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/add.png" OnClick="ibtn_crearCntrato_Click" />
                    <asp:LinkButton ID="lbtn_crearContrato" runat="server" OnClick="lbtn_crearContrato_Click" >Crear un nuevo contrato</asp:LinkButton>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style6">

                    <asp:GridView ID="gvcCoordinadoresEval"  CssClass="Grilla" runat="server"
                        DataSourceID="lds_listaCoordEval"
                        AllowPaging="false" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="No hay aún contratos para este evaluador" OnRowCommand="gvcCoordinadoresEval_RowCommand">
                        <Columns>

                            <asp:TemplateField HeaderText="Número de Contrato">
                                <ItemTemplate>
                                    <asp:Button ID="hContrato" Text='<%# Eval("numContrato") %>' CommandArgument='<%# Eval("Id_EvaluadorContrato") %>' CommandName="editarContrato" CssClass="boton_Link_Grid" runat="server"  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fecha de Inicio" >
                                <ItemTemplate>
                                    
                                    <asp:Label ID="finicio" runat="server" Text='<%# Eval("FechaInicio") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Fecha de Finalización">
                                <ItemTemplate>
                                    
                                    <asp:Label ID="ffin" runat="server" Text='<%# Eval("FechaExpiracion") %>'></asp:Label>

                                </ItemTemplate>
                            </asp:TemplateField>


                            </Columns>
                        </asp:GridView>

                </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style3"></td>
                <td class="auto-style4" align="center">
                    &nbsp;</td>
                <td class="auto-style5"></td>
              </tr>
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style6">&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            width: 5%;
        }
        .auto-style2
        {
            width: 90%;
        }
        .auto-style3
        {
            width: 5%;
            height: 30px;
        }
        .auto-style4
        {
            width: 60%;
            height: 30px;
        }
        .auto-style5
        {
            height: 30px;
        }
        .auto-style6
        {
            width: 60%;
        }
    </style>
</asp:Content>


