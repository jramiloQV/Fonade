<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AdministrarEvaluador.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluadores.AdministrarEvaluador" %>

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
        function alerta(){
            return confirm(" ¿ Desea asociar este sector al evaluador. ?");
        };        
    </script>
    <script type="text/javascript" >                
        function alertaAsignarPlanes(){
            return confirm(" ¿ Desea asignar los planes de negocio seleccionados a este evaluador. ?");
        };
    </script>        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
      <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                
                <table class="auto-style2">
                    <tr>
                        <td style="width:30%; vertical-align: top;">
                            <h3> Asignar evaluadores a planes de negocio </h3> 
                            <asp:GridView ID="gvEvaluadores" runat="server" AllowPaging="false" DataSourceID="dataEvaluadores"   AutoGenerateColumns="False"  EmptyDataText="No hay actividades." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvEvaluadores_RowCommand" >
                                <Columns>            
                                    <asp:TemplateField HeaderText="Evaluadores">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="verEvaluador" CommandArgument='<%# Eval("Id") %>'
                                                CommandName="mostrar" CausesValidation="False" Text='<%# Eval("NombreCompleto") %>' runat="server"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:GridView>                                                                 
                        </td>
                        <td style="width:70%; vertical-align:top; padding-top:0px;">
                            <h3> Planes de negocio </h3> 
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
                            <asp:GridView ID="gvProyectos" runat="server" AllowPaging="false"   AutoGenerateColumns="False" EmptyDataText="No hay datos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" Visible="true" OnRowCommand="gvProyectos_RowCommand" EnableViewState="true">
                                <Columns>   
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkProyecto" Checked='<%# (bool)Eval("IsOwner") %>' Enabled='<%# (bool)Eval("IsValidSector") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>         
                                    <asp:TemplateField HeaderText="Proyectos" >
                                        <ItemTemplate>
                                            <asp:Label Text='<%# Eval("Id") + " - " + Eval("Nombre") %>' runat="server" Font-Bold="true" />
                                            <br />
                                            <asp:Label Text="Evaluador:" runat="server" />
                                            <asp:Label Text='<%# Eval("Evaluador") %>' runat="server" ForeColor='<%# Eval("currentColor") %>' />
                                            <br />
                                            <asp:Label Text='<%# "Sector: " + Eval("Sector") %>' runat="server" />                                                                                                
                                            <br />
                                            <asp:Label id="lblSectorDiferente" Text="Sector diferente al evaluador" Font-Bold="true" ForeColor="Red" runat="server" Visible='<%# !(bool)Eval("IsValidSector") %>' /> 
                                            <asp:LinkButton ID="lnkAsociarSector" CommandArgument='<%# Eval("IdSector") %>' CommandName="asociarSector" CausesValidation="False" Text='Asociar sector' runat="server"  Font-Bold="true" Visible='<%# !(bool)Eval("IsValidSector") %>' OnClientClick=" if (!alerta()) return false;"/>
                                            <asp:HiddenField runat="server" ID="hdCodigoProyecto" Value='<%# Eval("Id") %>' />                                            
                                            <asp:CheckBox runat="server" ID="chkIsCurrentOwner" Checked='<%# (bool)Eval("IsOwner") %>'  Visible="false"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                                            
                                </Columns>
                                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            </asp:GridView>
                            
                            <asp:Button ID="btnAsignarEvaluador" Text="Asignar planes a evaluador" runat="server" Visible="false" OnClientClick="if (!alertaAsignarPlanes()) return false;" OnClick="btnAsignarEvaluador_Click" />
                        </td>
                    </tr>        
                </table>
            <asp:HiddenField ID="hdCodigoEvaluador" runat="server" Visible="false" Value="0" />
            <asp:ObjectDataSource
                    ID="dataEvaluadores"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Evaluacion.AdministrarEvaluacion.AdministrarEvaluadores.AdministrarEvaluador"
                    SelectMethod="GetEvaluadores"                                                            
                    EnablePaging="false">                    
            </asp:ObjectDataSource>            
            </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
