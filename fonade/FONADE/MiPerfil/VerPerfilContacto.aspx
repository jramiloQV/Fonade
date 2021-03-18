<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerPerfilContacto.aspx.cs"
    Inherits="Fonade.FONADE.MiPerfil.VerPerfilContacto" MasterPageFile="~/Emergente.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
<div style="background-color: #FFFFFF">
    <script type="text/javascript">
        function Cerrar() {
            this.window.close();
        }
        
    </script>
    <asp:LinqDataSource ID="lds_estudios" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_estudios_Selecting">
    </asp:LinqDataSource>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelTitilo" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="570" border="0">
                <tr>
                    <td colspan="2">
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700"></asp:Label></h1>
                    </td>
                    <td colspan="2" align="right">
                        <asp:Label ID="l_fechaActual" runat="server" Style="font-weight: 700"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------------------------------------------%>
    <asp:UpdatePanel ID="PanelAsesores" runat="server" Visible="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="P_PanelAsesores" runat="server">
                <table width="570" border="0">
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32">
                            Nombres:
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_nombresAs" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32">
                            Correo Electrónico:
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_emailAs" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32">
                            Dedicación a la Unidad:
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_dedicacionAs" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style16">
                        </td>
                        <td class="style33">
                            Experiencia Docente:
                        </td>
                        <td class="style39">
                        </td>
                        <td class="style19">
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                        </td>
                        <td colspan="2" class="style14">
                            <asp:TextBox ID="txt_Sectores" runat="server" Height="97px" ReadOnly="True" TextMode="MultiLine"
                                Width="100%"></asp:TextBox>
                        </td>
                        <td class="style14">
                        </td>
                    </tr>
                    <tr>
                        <td class="style16">
                        </td>
                        <td class="style33">
                            <b>Resumen Hoja de Vida: </b>
                        </td>
                        <td class="style39">
                        </td>
                        <td class="style19">
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                        </td>
                        <td colspan="2" class="style14">
                            <asp:TextBox ID="txt_hojavidaAs" runat="server" Height="97px" ReadOnly="True" TextMode="MultiLine"
                                Width="100%"></asp:TextBox>
                        </td>
                        <td class="style14">
                        </td>
                    </tr>
                    <tr>
                        <td class="style16">
                        </td>
                        <td class="style33">
                            <b>Experiencia e Intereses:</b>
                        </td>
                        <td class="style39">
                        </td>
                        <td class="style19">
                        </td>
                    </tr>
                    <tr>
                        <td class="style13">
                        </td>
                        <td colspan="2" class="style14">
                            <asp:TextBox ID="txt_expintAs" runat="server" Height="97px" ReadOnly="True" TextMode="MultiLine"
                                Width="100%"></asp:TextBox>
                        </td>
                        <td class="style14">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------------------------------------------%>
    <asp:UpdatePanel ID="PanelDemasRoles" runat="server" Visible="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="P_PanelDemasRoles" runat="server">
                <table width="570" border="0">
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32">
                            Nombres:
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_nombre" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32">
                            Correo Electrónico
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_email" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32" >
                            <asp:Label ID="lbl_FechaDeNacimiento" runat="server">Fecha de Nacimiento:</asp:Label>
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_fechanac" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td class="style32" >
                            <asp:Label ID="lbl_LugarDeNacimiento" runat="server">Lugar de Nacimiento:</asp:Label>
                        </td>
                        <td class="style38">
                            <asp:Label ID="l_lugarnac" runat="server"></asp:Label>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style31">
                            &nbsp;
                        </td>
                        <td class="style37">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------------------------------------------%>
    <asp:UpdatePanel ID="PanelEstudios" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="P_PanelEstudios" runat="server">
                <table width="570" border="0">
                    <tr>
                        <td class="style41">
                        </td>
                        <td class="style40">
                            <strong>I</strong><span class="footG"><strong>nformación Académica</strong></span>
                        </td>
                        <td class="style42">
                        </td>
                        <td class="style43">
                        </td>
                    </tr>
                    <tr>
                        <td class="style24">
                        </td>
                        <td colspan="2">
                            <asp:GridView ID="gvestudiosrealizadosasesor" CssClass="Grilla2" runat="server" DataSourceID="lds_estudios"
                                AllowPaging="false" AutoGenerateColumns="false" Width="100%" EmptyDataText="No hay información académica para este usuario.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Nivel de Estudio" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lnivelestudioAS" runat="server" Text='<%# Eval("NomNivelEstudio") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Título Obtenido" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="ltituloobtenidoAS" runat="server" Text='<%# Eval("TituloObtenido") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Año" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lañotituloAS" runat="server" Text='<%# Eval("AnoTitulo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Institución">
                                        <ItemTemplate>
                                            <asp:Label ID="linstitucionAS" runat="server" Text='<%# Eval("Institucion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad" HeaderStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:Label ID="lciudadAS" runat="server" Text='<%# "" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style31">
                            &nbsp;
                        </td>
                        <td class="style37">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------------------------------------------%>
    <asp:UpdatePanel ID="PanelCerrar" runat="server" Visible="true" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="P_PanelCerrar" runat="server">
                <table width="570" border="0">
                    <tr>
                        <td class="style24">
                        </td>
                        <td colspan="2" align="center">
                            <asp:Button ID="btn_Cerrar" runat="server" Text="Cerrar" OnClick="btn_Cerrar_Click" />
                        </td>
                        <td class="style27">
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style31">
                            &nbsp;
                        </td>
                        <td class="style37">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--------------------------------------------------------------------------%>
    <%------------------------INFO Ver Perfil del Contacto seleccionado.-------- class="Grilla"--------%>
    <asp:UpdatePanel ID="updt_especial" runat="server" Visible="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnl_especial" runat="server" Visible="false">
                <table width="95%" border="0" cellspacing="0" cellpadding="2">
                    <tbody>
                        <asp:Label ID="lbl_tabla_dibujada" Text="" runat="server" />
                        <tr valign="top" align="right">
                            <td colspan="4" class="TitDestacado">
                                <asp:Button ID="btn_cerrarVentana" Text="Cerrar" runat="server" OnClientClick="return Cerrar();" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%------------------------FIN Ver Perfil del Contacto seleccionado.---------------------------------%>
</div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style10
        {
            width: 25px;
        }
        .style13
        {
            width: 25px;
            height: 22px;
        }
        .style14
        {
            height: 22px;
        }
        .style16
        {
            width: 25px;
            height: 32px;
        }
        .style19
        {
            height: 32px;
        }
        .style24
        {
            width: 25px;
            height: 24px;
        }
        .style27
        {
            height: 24px;
        }
        .style31
        {
            width: 159px;
        }
        .style32
        {
            width: 159px;
            font-weight: bold;
            height: 24px;
        }
        .style33
        {
            width: 159px;
            font-weight: bold;
            height: 32px;
        }
        .style37
        {
            width: 349px;
        }
        .style38
        {
            width: 349px;
            height: 24px;
        }
        .style39
        {
            width: 349px;
            height: 32px;
        }
        .style40
        {
            width: 159px;
            height: 25px;
            color: #666666;
        }
        .style41
        {
            width: 25px;
            height: 25px;
        }
        .style42
        {
            width: 349px;
            height: 25px;
        }
        .style43
        {
            height: 25px;
        }
    </style>
</asp:Content>
