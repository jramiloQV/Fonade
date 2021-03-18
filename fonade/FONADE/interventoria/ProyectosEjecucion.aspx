<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="ProyectosEjecucion.aspx.cs" Inherits="Fonade.FONADE.interventoria.ProyectosEjecucion" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="updt_1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="principal" runat="server">
                <h1>
                    <asp:Label ID="L_titulo" runat="server" Text="PROYECTOS EN EJECUCIÓN" />
                </h1>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            Id Proyecto
                        </td>
                        <td>
                            <asp:TextBox ID="txtidproyecto" runat="server" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Proyecto
                        </td>
                        <td>
                            <asp:TextBox ID="txtnomproyecto" runat="server" Width="200px" MaxLength="80" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Empresa
                        </td>
                        <td>
                            <asp:TextBox ID="txtnomempresa" runat="server" Width="200px" MaxLength="255" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="td_a">
                            <asp:Button ID="btnbuscar" runat="server" Text="Buscar..." OnClick="btnbuscar_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <asp:GridView ID="gv_proyectos" runat="server" CssClass="Grilla" AutoGenerateColumns="false"
                    OnRowCommand="gv_proyectos_RowCommand" AllowPaging="True" OnPageIndexChanging="gv_proyectos_PageIndexChanging"
                    DataKeyNames="Id_Proyecto" HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left"
                    PageSize="10" Width="100%">
                    <PagerStyle CssClass="Paginador" />
                    <RowStyle VerticalAlign="Top" />
                    <Columns>
                        <asp:BoundField DataField="Id_Proyecto" HeaderText="Id" />
                        <asp:TemplateField HeaderText="Nombre Proyecto">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnproyecto" runat="server" Text='<%# Eval("NomProyecto") %>'
                                    CommandArgument='<%# Eval("Id_Proyecto") + ";" + Eval("Id_Empresa")  %>' CommandName="ProyetoFrame" Width="180px"
                                    Height="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre Empresa">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnseguimiento" runat="server" Text='<%# Eval("NomEmpresa") %>'
                                    CommandArgument='<%# Eval("Id_Proyecto") + ";" + Eval("Id_Empresa") %>' CommandName="SeguimientoFrame"
                                    Width="160px" Height="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Coordinador">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnmailto1" runat="server" Text='<%# Eval("NomCoordinador") %>'
                                    CommandArgument='<%# Eval("EmailCoordinador") %>' CommandName="mailtoEnviar"
                                    Width="80px" Height="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Interventor Líder">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnmailto2" runat="server" Text='<%# Eval("NomInterventor") %>'
                                    CommandArgument='<%# Eval("EmailInterventor") %>' CommandName="mailtoEnviar"
                                    Width="100px" Height="60px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Convocatoria" HeaderText="Convocatoria" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnbuscar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
