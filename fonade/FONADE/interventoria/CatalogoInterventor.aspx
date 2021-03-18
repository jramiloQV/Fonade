<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="CatalogoInterventor.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoInterventor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <%--<asp:LinqDataSource ID="lds_listaInterventors" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="False" OnSelecting="lds_listaInterventors_Selecting" EntityTypeName=""
        TableName="Interventors">
    </asp:LinqDataSource>--%>
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700">USUARIO COORDINADOR DE INTERVENTORIA</asp:Label></h1>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left" class="auto-style3">
                        <asp:ImageButton ID="ibtn_crearGerentInterv" runat="server" ImageAlign="AbsBottom"
                            ImageUrl="../../Images/icoAdicionarUsuario.gif" OnClick="ibtn_crearGerentInterv_Click" />
                        <asp:LinkButton ID="lbtn_crearInterv" runat="server" OnClick="lbtn_crearInterv_Click">Adicionar Usuario Interventor</asp:LinkButton>
                    </td>
                </tr>
                <%--Grilla--%>
                <tr>
                    <td>
                        <asp:GridView ID="gvcGerenteInterventor" CssClass="Grilla" runat="server" AllowSorting="True"
                            AutoGenerateColumns="False" Width="100%" OnRowCommand="gvcGerenteInterventor_RowCommand"
                            OnRowDataBound="gvcGerenteInterventor_RowDataBound" AllowPaging="True" OnPageIndexChanging="gvcGerenteInterventor_PageIndexChanging"
                            OnSorting="gvcGerenteInterventor_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="Hiddeninactivo" runat="server" Value='<%# Eval("Inactivo") %>' />
                                        <asp:HiddenField ID="HiddenNumevals" runat="server" Value='<%# Eval("Cuantos") %>' />
                                        <asp:HiddenField ID="Inhabilitado" runat="server" Value='<%# Eval("Inhabilitado") %>' />

                                        <asp:ImageButton ID="imgActivarOdesactivarinterventor" runat="server" CausesValidation="false"
                                            CssClass="boton_Link_Grid" CommandArgument='<%# Eval("Id_Contacto")%>' />

                                        <%--<asp:ImageButton ID="ibtnreactivar" CausesValidation="false" CommandArgument='<%# Eval("Id_Contacto")%>'
                                            CommandName="reactivarcoorEval" Visible="true" runat="server" ImageUrl="~/Images/icoActivar.gif"
                                            CssClass="boton_Link_Grid" OnClientClick="return confirm('Esta seguro que desea Activar el interventor seleccionado?');" />
                                        
                                        <asp:ImageButton ID="ibtninactivar" CausesValidation="false" CommandArgument='<%# Eval("Id_Contacto")%>'
                                            CommandName="inactivarcoorEval" Visible="true" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                            CssClass="boton_Link_Grid" />--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nombre" SortExpression="nombre" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hlnombre" Text='<%# Eval("nombre") %>' CommandArgument='<%# Eval("Id_Contacto") %>'
                                            CommandName="editacontacto" runat="server" Width="90px" Height="30px" ToolTip='<%# Eval("Inactivo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email" SortExpression="Email" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hcorreo" runat="server" NavigateUrl='<%#"mailto:"+ Eval("Email") %>'
                                            Text='<%# Eval("email") %>' Width="200px" Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hlcuantos" Text='<%# Eval("Cuantos") %>' CommandArgument='<%# Eval("Id_Contacto") %>'
                                            CommandName="vercuantos" CssClass="boton_Link_Grid" runat="server" Width="90px"
                                            Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="hinhabilitado" Text="" CommandArgument='<%# Eval("Id_Contacto")+ ";" + Eval("nombre")  %>'
                                            CommandName="verhabilitado" runat="server" Width="80px" Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtn_Estado" Text='<%# Eval("Inactivo") %>' runat="server"
                                            Enabled="false" CausesValidation="false" CommandName="Estado_Activo_Inactivo"
                                            CommandArgument='<%# Eval("Id_Contacto") %>' ForeColor="Blue" Width="60px" Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                Aún no hay criterios para esta convocatoria.</EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_asignar" runat="server" CssClass="boton_Link_Grid" Text="Asignar Coordinador a Interventores"
                            Width="235px" OnClick="btn_asignar_Click" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnAsignarV2" runat="server" CssClass="boton_Link_Grid" Text="Asignar interventor a Interventores V2" PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Empresa/AsignarEmpresa.aspx" Width="235px" Visible="false" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1
        {
            width: 5%;
        }
        .auto-style2
        {
            width: 90%;
        }
        .auto-style3
        {
            width: 5%;
            height: 30px;
        }
        .auto-style4
        {
            width: 90%;
            height: 30px;
        }
        .auto-style5
        {
            height: 30px;
        }
        .sinlinea
        {
            text-decoration: none;
            color: black;
        }
    </style>
    <script type="text/javascript">
        function AlertaActivar() {
            return confirm('Esta seguro que desea Activar el interventor seleccionado?');
        }
        
    </script>
</asp:Content>
