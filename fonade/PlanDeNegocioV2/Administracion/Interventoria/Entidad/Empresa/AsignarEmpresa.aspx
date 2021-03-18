<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AsignarEmpresa.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Empresa.AsignarEmpresa" %>

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
            return confirm(" ¿ Desea asignar estas empresas a este interventor ?");
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
                        <label>Seleccione la entidad para ver sus interventores</label>
                        <br />
                        <asp:DropDownList ID="cmbEntidades" runat="server" 
                            DataValueField="Id" 
                            DataTextField="Nombre" 
                            AutoPostBack="true" OnSelectedIndexChanged="cmbEntidades_SelectedIndexChanged"
                            >
                        </asp:DropDownList>
                        <br />

                        <h3>Asignar interventores a empresas </h3>
                        <asp:GridView ID="gvMain" runat="server" AllowPaging="false" 
                            AutoGenerateColumns="False" 
                            EmptyDataText="No hay actividades." Width="98%" BorderWidth="0" 
                            CellSpacing="1" CellPadding="4" CssClass="Grilla" 
                            HeaderStyle-HorizontalAlign="Left" OnRowCommand="gvMain_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Interventores">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="verMain" CommandArgument='<%# Eval("IdContacto") %>'
                                            CommandName="mostrar" CausesValidation="False" Text='<%# Eval("NombreCompleto") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                        </asp:GridView>
                    </td>
                    <td style="width: 70%; vertical-align: top; padding-top: 0px;">
                        <h3>Empresas </h3>
                        <asp:TextBox ID="txtCodigo" runat="server"></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar proyecto" OnClick="btnBuscar_Click" />
                        <br />
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
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
                        <asp:GridView ID="gvContainer" runat="server" AllowPaging="true" PageSize="30"
                            AutoGenerateColumns="False" EmptyDataText="No hay datos que mostrar."
                            Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla"
                            HeaderStyle-HorizontalAlign="Left" ShowHeaderWhenEmpty="true" Visible="true" 
                            EnableViewState="true"
                            DataSourceID="dataContainer" OnRowDataBound="gvProyectos_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkContainer" Checked='<%# (bool)Eval("IsOwner") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Proyectos">
                                    <ItemTemplate>
                                        <asp:Label Text='<%# Eval("Id") + " - " + Eval("Nombre") %>' runat="server" Font-Bold="true" />
                                        <br />
                                        <asp:Label Text="Contrato:" runat="server" />
                                        <asp:DropDownList ID="DropDownList1" runat="server" DataValueField="Id" DataTextField="NumeroContratoWithFormat">
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label Text="Interventor:" runat="server" />
                                        <asp:Label Text='<%# Eval("Interventor") %>' runat="server" ForeColor='<%# Eval("currentColor") %>' />
                                        <br />
                                        <asp:Label Text='<%# "Sector: " + Eval("Sector") %>' runat="server" />
                                        <br />
                                        <asp:Label Text='<%# "Razón social: " + Eval("RazonSocial") %>' runat="server" />
                                        <br />
                                        <asp:HiddenField runat="server" ID="hdCodigoProyecto" Value='<%# Eval("Id") %>' />
                                        <asp:HiddenField runat="server" ID="hdIdEmpresa" Value='<%# Eval("IdEmpresa") %>' />
                                        <asp:HiddenField runat="server" ID="hdCodigoContrato" Value='<%# Eval("CodigoContrato") %>' />
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
                TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Empresa.AsignarEmpresa"
                SelectMethod="Get"
                EnablePaging="false">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbEntidades" Name="idEntidad" PropertyName="SelectedValue" Type="Int32" DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataEntidades"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Empresa.AsignarEmpresa"
                SelectMethod="GetEntidades"
                EnablePaging="false"></asp:ObjectDataSource>

            <asp:ObjectDataSource
                ID="dataContainer"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.Empresa.AsignarEmpresa"
                SelectMethod="GetProyectos"
                EnablePaging="true"
                SelectCountMethod="CountProyectos"
                MaximumRowsParameterName="maxRows"
                StartRowIndexParameterName="startIndex">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hdMainValue" Name="idInterventor" PropertyName="Value" Type="String" DefaultValue="0" ConvertEmptyStringToNull="false" />
                    <asp:ControlParameter ControlID="txtCodigo" Name="codigoProyecto" PropertyName="Text" Type="String" DefaultValue="0" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
