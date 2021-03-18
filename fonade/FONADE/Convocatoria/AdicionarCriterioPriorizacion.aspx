<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdicionarCriterioPriorizacion.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.AdicionarCriterioPriorizacion" MasterPageFile="~/Emergente.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_criterioPriorizacion" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true" 
        onselecting="lds_criterioPriorizacion_Selecting">
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="600px">
              <tr>
                <td >
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
                
                <asp:GridView ID="gvcriteriosPriorizacion"  CssClass="Grilla" runat="server" DataSourceID="lds_criterioPriorizacion" 
                   AllowPaging="false" AutoGenerateColumns="false" Width="100%" >
                        <Columns>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>                    
                                    <asp:CheckBox ID="ch_criterio" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Criterio">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("id_criterio") %>' />
                                    <asp:Label ID="l_criterio" runat="server" Text='<%# Eval("nomcriterio") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView></td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style12"></td>
                <td class="style13" align="center">
                    <asp:Button ID="btn_adicionar" runat="server" onclick="btn_adicionar_Click" 
                        Text="Adicionar" />
                  </td>
                <td class="style14"></td>
              </tr>

            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

 </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
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
    </style>
</asp:Content>

