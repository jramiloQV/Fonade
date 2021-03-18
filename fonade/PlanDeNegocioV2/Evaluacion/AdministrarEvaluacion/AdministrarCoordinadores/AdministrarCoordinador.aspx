<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarCoordinador.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinadores.AdministrarCoordinador" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }
        .Grilla {
            margin-top: 0px;
        }
    </style>
    
    <script type="text/javascript" >                
        function alertaAsignarCoordinador(){
            return confirm(" ¿ Desea asignar los evaluadores seleccionados a este coordinador. ?");
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>

                <table class="auto-style2">
                    <tr>
                        <td style="width:40%;vertical-align: top;">
                            <h3> Asignar Coordinador a evaluadores </h3> 
                            <asp:GridView ID="gvCoordinadores" runat="server" AllowPaging="false" DataSourceID="dataCoordinadores"   AutoGenerateColumns="False"  EmptyDataText="No hay actividades." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvCoordinadores_RowCommand" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Coordinadores">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="verEvaluadores" CommandArgument='<%# Eval("Id") %>'
                                                CommandName="mostrar" CausesValidation="False" Text='<%# Eval("NombreCompleto") %>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:GridView>                                                                 
                        </td>
                        <td style="width:60%; vertical-align:top; padding-top:0px;">
                            <h3> Evaluadores </h3> 
                            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="form-group center-block">
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <label class="control-label"><b>Actualizando información </b></label>
                                            <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                        </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:GridView ID="gvEvaluadores" runat="server" AllowPaging="false"   AutoGenerateColumns="False" EmptyDataText="No hay datos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" Visible="true"  >
                                <Columns>   
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkEvaluador" Checked='<%# (bool)Eval("IsOwner") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                                    <asp:TemplateField HeaderText="Evaluadores" >
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("NombreCompleto") %>' runat="server" Font-Bold="true" />
                                            <br />
                                            <asp:Label Text="Coordinador:" runat="server" />
                                            <asp:Label Text='<%# Eval("NombreCoordinador") %>' runat="server" ForeColor='<%# Eval("currentColor") %>' />
                                            <br />                                            
                                            <asp:HiddenField runat="server" ID="hdIdEvaluador" Value='<%# Eval("IdEvaluador") %>' />                                
                                            <asp:CheckBox runat="server" ID="chkIsCurrentOwner" Checked='<%# (bool)Eval("IsOwner") %>'  Visible="false"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                            
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:GridView>                            
                            <asp:Button ID="btnAsignar" Text="Asignar" runat="server" Visible="false" OnClientClick="if (!alertaAsignarCoordinador()) return false;" OnClick="btnAsignar_Click" />
                        </td>
                    </tr>        
                </table>
            <asp:HiddenField ID="hdCodigoCoordinador" runat="server" Visible="false" Value="0" />
            <asp:ObjectDataSource
                    ID="dataCoordinadores"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarCoordinadores.AdministrarCoordinador"
                    SelectMethod="GetCoordinadores"                                                            
                    EnablePaging="false">                    
            </asp:ObjectDataSource>            
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>