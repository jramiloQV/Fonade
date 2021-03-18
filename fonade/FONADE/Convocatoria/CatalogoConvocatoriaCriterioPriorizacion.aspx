<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoConvocatoriaCriterioPriorizacion.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoConvocatoriaCriterioPriorizacion" MasterPageFile="~/Master.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_criterioPriorizacion" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true" 
        onselecting="lds_criterioPriorizacion_Selecting">
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>


        <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="98%">
              <tr>
                <td class="style10">
                    
                </td>
                <td class="style11">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/add.png" />
                    <asp:LinkButton ID="lnkadicionar" runat="server" OnClick="lnkadicionar_Click" Text="Adicionar criterio de priorización a la convocatoria"></asp:LinkButton>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style10">&nbsp;</td>
                <td class="style11">
                
                   <asp:GridView ID="gvcriteriosPriorizacion"  CssClass="Grilla" runat="server" DataSourceID="lds_criterioPriorizacion" 
                   AllowPaging="false" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="Aún no hay criterios para esta convocatoria." 
                        onload="gvcriteriosPriorizacion_Load" 
                        onrowcommand="gvcriteriosPriorizacion_RowCommand"  >
                        <Columns>
                           <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>                    
                                    <asp:ImageButton ID="btnEliminarcp" Visible="true" runat="server" ImageUrl="/Images/icoBorrar.gif" CommandArgument='<%# Eval("id_criterio") %>' CommandName="EliminarCriterio"/>
                                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server"
                                            TargetControlID="btnEliminarcp"
                                            ConfirmText="¿Desea eliminar este criterio?"
                                        />
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="Criterio">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("id_criterio") %>' />
                                    <asp:Label ID="l_criterio" runat="server" Text='<%# Eval("nomcriterio") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Parametros"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_parametros" runat="server" Text='<%# Eval("parametros") %>' Enabled="true"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField  HeaderText="% de incidencia" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_incidencia" runat="server" Width="35px" Text='<%# Eval("incidencia") %>' Enabled="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator runat="server" id="RegularExpressionValidator1" controltovalidate="txt_incidencia" validationexpression="^[0-9]*" errormessage="*Este campo es numérico."  display="dynamic" ForeColor="Red" />
                                </ItemTemplate> 
                            </asp:TemplateField>
  

                        </Columns>
                    </asp:GridView>

                </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btn_volver" runat="server" Text="Volver a la Convocatoria" 
                        onclick="btn_volver_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" 
                        onclick="btn_actualizar_Click" />
                  </td>
                <td>&nbsp;</td>
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
            width: 630px;
        }
    </style>
</asp:Content>

