<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="AdministrarAsesores.aspx.cs"
    Inherits="Fonade.FONADE.AdministrarPerfiles.AdministrarAsesores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        function alerta() {
            return confirm('Esta seguro que desea borrar el asesor seleccionado?');
        }
        function AbrirPlanes(id) {
            window.open("VerProyectosAsesor.aspx?CodContacto=" + id, "_blank", "height=500, width=800, left=150, top=150, " +
            "location=no, menubar=no, resizable=no, " +
            "scrollbars=no, titlebar=no, toolbar=no", true);
        }

    </script>
    <asp:LinqDataSource ID="lds_Asesores" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Asesores_Selecting">
        <WhereParameters>
            <asp:Parameter Name="Fecha" />
        </WhereParameters>
    </asp:LinqDataSource>
    <asp:HyperLink ID="AgregarAsesor" NavigateUrl="~/FONADE/AdministrarPerfiles/CrearAsesores.aspx"
        runat="server">
    <img alt="" src="../../../Images/icoAdicionarUsuario.gif" />
    Agregar Asesor</asp:HyperLink>
    <div style="padding: 20px 0px;">
        <asp:Panel ID="pnl_asesores" runat="server">
            <asp:GridView ID="gw_Asesores" runat="server" Width="100%" AutoGenerateColumns="False"
                DataKeyNames="" CssClass="Grilla" AllowPaging="True" DataSourceID="lds_Asesores"
                PageSize="<%# PAGE_SIZE %>" AllowSorting="True" OnRowDataBound="gw_Asesores_RowDataBound"
                OnRowCreated="gw_Asesores_RowCreated" OnPageIndexChanging="gw_Asesores_PageIndexChanged"
                OnRowCommand="gw_Asesores_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Inactivar" CausesValidation="false" CommandName="eliminar"
                                CommandArgument='<%# Bind("Id_contacto")%>' runat="server" ImageUrl="/Images/icoBorrar.gif"
                                Visible="true" OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombres" SortExpression="Nombres">
                        <ItemTemplate>
                            <asp:HyperLink ID="hl_nombre" runat="server" NavigateUrl='<%# "Editar.aspx?CodContacto="+ Eval("Id_contacto") %>'
                                Text='<%# Eval("Nombres") + "  " + Eval("Apellidos") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Detalle Datos" SortExpression="Nombres">
                        <ItemTemplate>
                            <asp:HyperLink ID="hl_detalle" runat="server" NavigateUrl='<%# "VerDetalle.aspx?CodContacto="+ Eval("Id_contacto") %>'
                                Text="Ver detalle"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" SortExpression="Email">
                        <ItemTemplate>
                            <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                                Text='<%# Eval("Email") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="# Planes de Negocio" SortExpression="Conteo">
                        <ItemTemplate>
                                <asp:Button ID="hl_conteo" runat="server" OnClientClick='<%# "AbrirPlanes("+Eval("Id_contacto")+")" %>' Text='<%# Eval("Conteo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <div style="width: 100%; text-align: center;">
                <asp:LinkButton ID="lnkasesores" runat="server" Text="Asignar Asesores a un Plan de Negocio"
                    OnClick="lnkasesores_Click"></asp:LinkButton>
            </div>
            <br />
            <br />
        </asp:Panel>
        <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server"></asp:Label>
    </div>
</asp:Content>
