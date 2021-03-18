<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proyectos.aspx.cs" Inherits="Fonade.Fonade.Proyecto.Proyectos"
    MasterPageFile="~/Master.Master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        function alerta() {
            return confirm('Esta seguro que desea activar el proyecto seleccionado?');
        }
    </script>
    <asp:LinqDataSource ID="lds_proyectos" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_proyectos_Selecting">
    </asp:LinqDataSource>
    <asp:Panel ID="pnlPrincipal" runat="server">
        <label>
            Buscar por letra:</label>
        <asp:DropDownList ID="ddlbuscar" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbuscar_SelectedIndexChanged">
            <asp:ListItem Value="" Text="Todo" Selected="True" />
            <asp:ListItem Value="a" Text="A" />
            <asp:ListItem Value="b" Text="B" />
            <asp:ListItem Value="c" Text="C" />
            <asp:ListItem Value="d" Text="D" />
            <asp:ListItem Value="e" Text="E" />
            <asp:ListItem Value="f" Text="F" />
            <asp:ListItem Value="g" Text="G" />
            <asp:ListItem Value="h" Text="H" />
            <asp:ListItem Value="i" Text="I" />
            <asp:ListItem Value="j" Text="J" />
            <asp:ListItem Value="k" Text="K" />
            <asp:ListItem Value="l" Text="L" />
            <asp:ListItem Value="m" Text="M" />
            <asp:ListItem Value="n" Text="N" />
            <asp:ListItem Value="o" Text="O" />
            <asp:ListItem Value="p" Text="P" />
            <asp:ListItem Value="q" Text="Q" />
            <asp:ListItem Value="r" Text="R" />
            <asp:ListItem Value="s" Text="S" />
            <asp:ListItem Value="t" Text="T" />
            <asp:ListItem Value="u" Text="U" />
            <asp:ListItem Value="v" Text="V" />
            <asp:ListItem Value="w" Text="W" />
            <asp:ListItem Value="x" Text="X" />
            <asp:ListItem Value="y" Text="Y" />
            <asp:ListItem Value="z" Text="Z" />
        </asp:DropDownList>
        <br />
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <br />
        <asp:GridView ID="gw_proyectos" runat="server" Width="98%"
            AutoGenerateColumns="False"
            DataKeyNames="IdProyecto,idTipoInstitucion" CssClass="Grilla"
            DataSourceID="lds_proyectos"
            OnSorting="gw_proyectos_Sorting"
            AllowSorting="True" OnRowDataBound="gw_proyectos_RowDataBound"
            OnDataBound="gw_proyectos_DataBound" PagerStyle-CssClass="Paginador"
            OnRowCommand="gw_proyectos_RowCommand"
            AllowPaging="true"
            PageSize="50"
            OnPageIndexChanging="gw_proyectos_PageIndexChanging"
            RowStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="Top">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                    <ItemTemplate>
                        <asp:ImageButton ID="ibtn_Inactivar" runat="server" CommandArgument='<%# Bind("IdProyecto") %>' CommandName="InActivar" ImageUrl="~/Images/icoBorrar.gif" Visible="false" />
                        <asp:ImageButton ID="ibtn_Activar" runat="server" CommandArgument='<%# Bind("IdProyecto") %>' CommandName="Activar" ImageUrl="~/Images/icoActivar.gif" Visible="false" OnClientClick="return alerta();" OnClick="ibtn_Activar_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre" SortExpression="NombreProyecto">
                    <ItemTemplate>
                        <asp:LinkButton ID="hl_proyecto" runat="server" CommandName="Frameset" CommandArgument='<%# Eval("IdProyecto") %>'
                            Text='<%# Eval("IdProyecto") + " - " + Eval("NombreProyecto") %>' ForeColor="Black" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Evaluacion" SortExpression="Evaluacion">
                    <ItemTemplate>
                        <asp:LinkButton ID="hl_evaluacion" runat="server" CommandArgument='<%# Eval("IdProyecto") %>'
                            CommandName="Evaluacions" Text="Evaluar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ciudad" SortExpression="NombreCiudad">
                    <ItemTemplate>
                        <asp:Label ID="lbl_ciudad" runat="server" Text='<%# Eval("NombreCiudad") + " (" + Eval("NombreDepartamento") + ")" %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Unidad" SortExpression="NombreUnidad">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtn_reasignar" runat="server" CommandArgument='<%# Eval("IdProyecto") %>'
                            Text='<%# Eval("NombreUnidad") + " (" + Eval("NombreInstitucion") + ")" %>' ForeColor="#174680" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtn_inactivo" runat="server" CommandArgument='<%# Eval("IdProyecto") %>'
                            Text='<%# Eval("Estado") %>' CausesValidation="false" CommandName="ventanaInactivo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Evaluador">
                    <ItemTemplate>
                        <asp:Label ID="levaluador_" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Avalado">
                    <ItemTemplate>
                        <asp:Label ID="lblavalado" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Convocatoria">
                    <ItemTemplate>
                        <asp:Label ID="lbconvocatoria" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Evaluar">
                    <ItemTemplate>
                        <asp:LinkButton ID="hl_evaluacion_hp" runat="server" CommandName="Evaluacion" Text="Evaluar"
                            CommandArgument='<%# Eval("IdProyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Operador" SortExpression="NombreOperador">
                    <ItemTemplate>
                        <asp:Label ID="lblOperador" runat="server" Text='<%# Eval("NombreOperador") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
    </asp:Panel>
    <div style="text-align: center">
        <asp:Panel ID="pnlInActivar" runat="server" Visible="false">
            <asp:Label ID="lblTitulo" runat="server" />
            Motivo de Inactivación:<br />
            <asp:TextBox ID="txtMotivoInactivacion" runat="server" Width="500px" Height="80px"
                TextMode="MultiLine"></asp:TextBox>
            <br />
            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClick="btnCerrar_Click" />
            <asp:Button ID="btnInActivar" runat="server" Text="Inactivar" OnClick="btnInActivar_Click" />
            <asp:HiddenField ID="hddIdProyecto" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
