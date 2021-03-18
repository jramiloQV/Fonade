<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AsignarContrato.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.AsignarContrato" %>

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
    <script type="text/javascript">                
        function alertaAsignarPlanes() {
            return confirm(" ¿ Desea asignar los interventores a este contrato. ?");
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
    <label>Operador</label>
    <asp:DropDownList ID="ddlOperador" runat="server" DataValueField="idOperador"
        DataTextField="NombreOperador" AutoPostBack="true" OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged">
    </asp:DropDownList>
    <hr />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <table class="auto-style2">
                <tr>
                    <td style="width: 30%; vertical-align: top;">
                        <label>Seleccione la entidad para ver sus contratos</label>
                        <br />
                        <asp:DropDownList ID="cmbEntidades" runat="server" 
                            DataValueField="Id" DataTextField="Nombre" 
                             AutoPostBack="true" OnSelectedIndexChanged="cmbEntidades_SelectedIndexChanged"
                            >
                        </asp:DropDownList>
                        <br />

                        <h3>Asignar interventores a contratos </h3>
                        <asp:GridView ID="gvMain" runat="server" AllowPaging="false" 
                            AutoGenerateColumns="False" 
                            EmptyDataText="No hay actividades." Width="98%" BorderWidth="0" 
                            CellSpacing="1" CellPadding="4" CssClass="Grilla" 
                            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvMain_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Contratos">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="verMain" CommandArgument='<%# Eval("Id") %>'
                                            CommandName="mostrar" CausesValidation="False" 
                                            Text='<%# Eval("NumeroContrato") %>' runat="server" />
                                        <asp:Label Text="Contrato vencido" runat="server" ForeColor="Red" 
                                            Visible='<%# (bool)Eval("ContratoVencido") %>' />
                                        <br />
                                        <asp:Label Text='<%# "Inicio " + Eval("FechaInicioWithFormat") + " - " %>' runat="server" ForeColor="Gray" />
                                        <asp:Label Text='<%# "Fin " + Eval("FechaFinWithFormat") %>' runat="server" ForeColor="Gray" />
                                        <br />

                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:GridView>
                    </td>
                    <td style="width: 70%; vertical-align: top; padding-top: 0px;">
                        <h3>Interventores </h3>
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
                        <asp:GridView ID="gvContainer" runat="server" AllowPaging="false" 
                            AutoGenerateColumns="False" EmptyDataText="No hay datos que mostrar." 
                            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" 
                            CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" 
                            ShowHeaderWhenEmpty="true" Visible="true" EnableViewState="true">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkContainer" Checked='<%# (bool)Eval("IsOwner") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Interventores">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("NombreCompleto") %>' runat="server" Font-Bold="true" />
                                        <br />
                                        <asp:Label Text="Interventor sin contrato" runat="server" ForeColor="Red" Visible='<%# !(bool)Eval("HasContract") %>' />
                                        <br />
                                        <asp:HiddenField runat="server" ID="hdCodigoContainer" Value='<%# Eval("IdContacto") %>' />
                                        <asp:CheckBox runat="server" ID="chkIsCurrentOwner" Checked='<%# (bool)Eval("IsOwner") %>' Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:GridView>

                        <asp:Button ID="btnSave" Text="Asignar" runat="server" Visible="false" OnClientClick="if (!alertaAsignarPlanes()) return false;" OnClick="btnAsignarEvaluador_Click" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hdMainValue" runat="server" Visible="false" Value="0" />

            <asp:ObjectDataSource
                ID="dataMain"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.AsignarContrato"
                SelectMethod="GetContratos"
                EnablePaging="false">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbEntidades" Name="idEntidad" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataEntidades"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Contrato.AsignarContrato"
                SelectMethod="GetEntidades"
                EnablePaging="false"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
