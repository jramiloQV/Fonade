<%@ Page Language="C#" MasterPageFile="~/Master.master" Title="Fonade-Administrar Criterios de priorización"
    AutoEventWireup="true" CodeBehind="CatalogoCriterioPriorizacion.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.Convenios.CatalogoCriterioPriorizacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        function alerta() {
            return confirm('Esta seguro que desea borrar el Criterio seleccionado?');
        }
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" /></h1>
    <asp:Panel ID="pnl_Criterios" runat="server">
        <asp:HyperLink ID="AgregarCriterio" NavigateUrl="~/FONADE/AdministrarPerfiles/Convenios/CatalogoCriterioPriorizacion.aspx?Accion=Crear"
            runat="server">
 <img alt="" src="../../../Images/icoAdicionarUsuario.gif" />
 Agregar Criterio de Priorización</asp:HyperLink>
        <asp:GridView ID="gv_Criterios" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="false" AllowSorting="True" OnRowDataBound="gv_Criterios_RowDataBound"
            OnDataBound="gv_Criterios_DataBound" OnRowCreated="gv_Criterios_RowCreated" OnPageIndexChanging="gv_Criterios_PageIndexChanged"
            OnRowCommand="gv_Criterios_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btn_Inactivar" CommandArgument='<%# Bind("Id_criterio")%>' runat="server"
                            OnClientClick="return alerta();" ImageUrl="/Images/icoBorrar.gif" Visible="true"
                            CausesValidation="false" CommandName="eliminar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Criterio" SortExpression="Criterio">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Criterio" runat="server" NavigateUrl='<%# "CatalogoCriterioPriorizacion.aspx?Accion=Editar&CodCriterio="+ Eval("Id_criterio") %>'
                            Text='<%# Eval("nomcriterio")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Cuantos" DataField="cuantos" SortExpression="cuantos" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_crearEditar" runat="server">
        <asp:Table ID="tbl_Criterio" runat="server">
            <asp:TableRow>
                <asp:TableCell>Factor:</asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="ddl_Factores" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator5"
                        runat="server" ControlToValidate="ddl_Factores" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Componente:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_componente" runat="server" Style="width:600px;" MaxLength="50"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1"
                        runat="server" ControlToValidate="tb_componente" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Criterio:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_criterio" runat="server" Style="width:600px;" MaxLength="50"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2"
                        runat="server" ControlToValidate="tb_criterio" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Sigla:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_sigla" runat="server" Style="width:600px;" MaxLength="10"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3"
                        runat="server" ControlToValidate="tb_sigla" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Indicador:</asp:TableCell><asp:TableCell>
                    <asp:TextBox TextMode="MultiLine" ID="tb_indicador" runat="server" Style="width:600px;height:100px;" MaxLength="255"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4"
                        runat="server" ControlToValidate="tb_indicador" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Valor de Base:</asp:TableCell><asp:TableCell>
                    <asp:TextBox TextMode="MultiLine" ID="tb_valor_base" runat="server" Style="width:600px;height:100px;" MaxLength="255"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator6"
                        runat="server" ControlToValidate="tb_valor_base" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Formulación:</asp:TableCell><asp:TableCell>
                    <asp:TextBox TextMode="MultiLine" ID="tb_formulacion" runat="server" Style="width:600px;height:100px;" MaxLength="255"/>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator7"
                        runat="server" ControlToValidate="tb_formulacion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Query:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_query" runat="server" MaxLength="50" />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator10"
                        runat="server" ControlToValidate="tb_query" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco" />
                </asp:TableCell></asp:TableRow></asp:Table><asp:Button ID="btn_crearActualizar" OnClick="btn_crearActualizar_onclick" runat="server"
            Text="Actualizar" />
        <ajaxToolkit:ConfirmButtonExtender Enabled="false" ID="cbe1" runat="server" DisplayModalPopupID="mpe1"
            TargetControlID="btn_crearActualizar">
        </ajaxToolkit:ConfirmButtonExtender>
        <ajaxToolkit:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlPopup1"
            TargetControlID="btn_crearActualizar" OkControlID="btnYes" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlPopup1" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header">
                Confirmación </div><div class="body">
                <asp:Label ID="lbl_popup" runat="server" />
            </div>
            <div class="footer" align="right">
                <asp:Button ID="btnYes" runat="server" Text="Aceptar" />
            </div>
        </asp:Panel>
    </asp:Panel>
    <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server" />
</asp:Content>
