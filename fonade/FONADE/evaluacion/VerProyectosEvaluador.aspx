<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerProyectosEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.VerProyectosEvaluador"  MasterPageFile="~/Emergente.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_eval" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true" 
        onselecting="lds_eval_Selecting">
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="600px">
              <tr>
                <td class="auto-style1" >
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>

            <table width="600">
              <tr>
                <td class="style10">&nbsp;</td>
                <td class="style11">                   
                <asp:Panel ID="Panel1" runat="server" Width="100%" Height="250px" ScrollBars="Vertical">
                <asp:GridView ID="gvevaluadores"  CssClass="Grilla" runat="server" DataSourceID="lds_eval" 
                   AllowPaging="false" AutoGenerateColumns="false" Width="100%" >
                        <Columns>
                           <asp:TemplateField HeaderText="Planes de Negocio">
                                <ItemTemplate> 
                                    <asp:Label ID="lnombre" runat="server" Text='<%# Eval("proyecto") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <asp:Label ID="lplanes" runat="server" Text='<%# Eval("fecha") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                    </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style12"></td>
                <td class="style13" align="center">
                    <asp:Button ID="btn_cerrar" runat="server" onclick="btn_cerrar_Click" 
                        Text="Cerrar" />
                  </td>
                <td class="style14"></td>
              </tr>

            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

 </asp:Content>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style10
        {
            width: 35px;
        }
        .style11
        {
            width: 523px;
        }
        .style12
        {
            width: 35px;
            height: 47px;
        }
        .style13
        {
            width: 523px;
            height: 47px;
        }
        .style14
        {
            height: 47px;
        }
        .auto-style1
        {
            width: 390px;
        }
    </style>
</asp:Content>