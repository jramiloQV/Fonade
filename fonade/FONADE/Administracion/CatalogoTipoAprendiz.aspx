<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="CatalogoTipoAprendiz.aspx.cs" Inherits="Fonade.FONADE.Administracion.CatalogoTipoAprendiz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Desea borrar el tipo de aprendiz?');
        }
    </script>
    <style>
        #MainMenu{
            margin-top: 0 !important;
        }
        .Login{
            padding: 13px 10px 15px !important;
        }
        #HeadLoginView_HeadLoginName{
            color: #fff;
        }
        .ContentInfo{
            margin-top: -15px !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <%--Información General--%>
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" />
    </h1>
    <%--<p style="text-align: left;">
        <asp:Label ID="lbl_enunciado" runat="server" BackColor="#000066" ForeColor="White"
            Width="35%" />
        <asp:Label ID="lbl_Interventor" runat="server" Width="40%" />
        <asp:Label ID="lbl_tiempo" runat="server" ForeColor="Red" />
    </p>--%>
    <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
        <table class="auto-style1">
            <tr>
                <td style="text-align: left" class="auto-style3">
                    <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                    <asp:ImageButton ID="Adicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                        Style="cursor: pointer;" OnClick="Adicionar_Click" />
                    &nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click"> Adicionar Tipo de Aprendiz</asp:LinkButton>
                    <asp:HiddenField ID="hdf_id" runat="server" Visible="false" />
                </td>
            </tr>
            <%--Grilla--%>
            <tr>
                <td style="text-align: center">
                    <asp:GridView ID="gv_tiposDeAprendices" runat="server" Width="400px" AutoGenerateColumns="false"
                        CssClass="Grilla" AllowPaging="True" OnRowCommand="gv_tiposDeAprendices_RowCommand"
                        PageSize="30" OnPageIndexChanging="gv_tiposDeAprendices_PageIndexChanging" ShowHeaderWhenEmpty="true"
                        EmptyDataText="No hay Tipos de Aprendices.">
                        <PagerStyle CssClass="Paginador" />
                        <RowStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField HeaderText="ID" DataField="Id_TipoAprendiz" />
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_TipoAprendiz")+ ";" + Eval("NomTipoAprendiz") %>'
                                        CommandName="mostrar" Text='<%#Eval("NomTipoAprendiz")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle CssClass="Paginador" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnl_detalles" runat="server" Visible="false">
        <table class="auto-style1">
            <%--Campos editables--%>
            <tr>
                <td class="auto-style3">
                    <asp:Label ID="lbl_idSelected" runat="server" />
                    <br />
                    <asp:TextBox ID="txt_nmb_tipoAprendiz" runat="server" Width="350px" />
                </td>
            </tr>
            <%--Botones--%>
            <tr align="right">
                <td>
                    <asp:Button ID="B_Adicionar" Text="Adicionar" runat="server" ValidationGroup="accionar"
                        OnClick="B_Adicionar_Click" />
                    <asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClick="B_Acion_Click"
                        Width="100px" Text="Actualizar" />
                    <asp:Button ID="B_Borrar" runat="server" Text="Borrar" ValidationGroup="accionar"
                        OnClientClick="return alerta()" OnClick="B_Borrar_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
