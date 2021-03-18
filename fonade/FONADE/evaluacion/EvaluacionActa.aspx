<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="EvaluacionActa.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionActa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <style type="text/css">
        .style10
        {
            width: 100%;
        }
        
        #imgeditar
        {
            border-collapse: collapse;
            border: none;
        }
    </style>
    <script type="text/javascript">
        function alerta2() {
            return confirm('Esta seguro de eliminar este proyecto del Acta?');
        }
        function alerta() {
            return confirm('Esta seguro de eliminar esta Acta?');
        }
    </script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700">Planes De Negocio</asp:Label>
                </h1>
            </td>
        </tr>
    </table>
    <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
        <asp:Panel ID="p_apregar" runat="server">
            <asp:ImageButton ID="ImgBtn_Adicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                Style="cursor: pointer;" OnClick="ImgBtn_Adicionar_Click" />
            &nbsp;<asp:LinkButton ID="lnkBtn_Adicionar" Text="Adicionar Acta de Validación" runat="server"
                OnClick="lnkBtn_Adicionar_Click" />
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                Style="cursor: pointer;" PostBackUrl="~/FONADE/evaluacion/ActasDeValidacionEvaluacion.aspx" />
        </asp:Panel>
        <asp:GridView ID="GrvActas" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
            OnSorting="GrvActasSorting" OnRowCommand="GrvActasRowCommand" OnPageIndexChanging="GrvActasPageIndexChanging"
            OnRowDataBound="GrvActas_RowDataBound">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="imgborrar" runat="server" CommandArgument='<%# Eval("Id_Acta") %>'
                            CommandName="eliminar" CausesValidation="false">
                            <asp:Label runat="server" ID="publicado" Visible="false" Text='<%# Eval("publicado") %>'></asp:Label>
                            <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                ToolTip="Eliminar acta" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="No Acta" SortExpression="NumActa">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Acta") + ";" + Eval("publicado") %>'
                            CommandName="NombreActa" Text='<%# Eval("NumActa") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NomActa" HeaderText="Nombre" SortExpression="NomActa" />
                <asp:BoundField DataField="NomConvocatoria" HeaderText="Convocatoria" SortExpression="NomConvocatoria" />
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
    </asp:Panel>
</asp:Content>
