<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AprobacionMasivaDeActividades.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Ejecucion.Actividades.AprobacionMasivaDeActividades" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Está seguro procesar las actividades disponibles ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate> 
            
            <asp:Label ID="lblTitle" runat="server" Font-Bold="True" Text="Aprobación masiva de actividades"></asp:Label>
            <br />
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
            <br />
            <asp:Label ID="lblActividadesPlanOperativo" runat="server" Font-Bold="false" Text="0 actividades pendientes por aprobar."></asp:Label>
            <br />
            <asp:Button ID="btnAprobar" Text="Aprobar todas las actividades de plan operativo" runat="server" OnClientClick="return alerta();" OnClick="btnAprobar_Click" />
            
            <br />
            <asp:Label ID="lblSubTitle" runat="server" Font-Bold="True" Text="Resultados" Visible="false"></asp:Label>          
            <br />
            <asp:GridView ID="gvMain" runat="server" AllowPaging="false" AutoGenerateColumns="False" EmptyDataText="No se encontro información." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" Visible="false">
                <Columns>                                                                   
                    <asp:TemplateField HeaderText="Resultado">
                            <ItemTemplate>
                                <asp:Label ID="lblResultado" runat="server" Text='<%# Container.DataItem %>' />
                            </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
