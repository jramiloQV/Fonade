<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MiPerfil.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.MiPerfil"
    MasterPageFile="~/Master.Master" Culture="es-CO" UICulture="es-CO" %>

<%--<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
</asp:Content>--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <%--<link href="../../Styles/Site.css" rel="stylesheet" />--%>
    <meta content="IE=edge,chrome=1" http-equiv="X-UA-Compatible">
    <asp:LinqDataSource ID="lds_estudiosAsesor" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_estudios_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_estudiosEmprendedor" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_estudios_Selecting">
    </asp:LinqDataSource>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ToolkitScriptManager>
    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700" />
                </h1>
            </td>
            <td align="right"></td>
        </tr>
    </table>
    <asp:UpdatePanel ID="PanelGeneral" runat="server" Visible="false" Width="98%" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%" border="0">
                <tr>
                    <td colspan="4" style="font-weight: 700; font-size: 10pt">Datos Personales
                    </td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Nombres:
                    </td>
                    <td class="style24">
                        <asp:Label ID="l_nombre1" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Apellidos:
                    </td>
                    <td class="style24">
                        <asp:Label ID="l_apellido1" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr id="tr_cedula" runat="server" visible="false">
                    <td class="style22"></td>
                    <td class="style23">Cédula de Ciudadanía:
                    </td>
                    <td class="style24">
                        <asp:Label ID="lbl_Cedula" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr id="tr_numIdentificacion" runat="server">
                    <td class="style22"></td>
                    <td class="style23">Número de Identificación:
                    </td>
                    <td class="style24">
                        <asp:Label ID="l_identificacion1" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr id="tr_cargo" runat="server">
                    <td class="style22"></td>
                    <td class="style23">
                        <asp:Label ID="lbl_Email" Text="Correo Electrónico:" runat="server" />
                    </td>
                    <td class="style24">
                        <asp:Label ID="l_email1" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr id="tr_direccion" runat="server" visible="false">
                    <td class="style22"></td>
                    <td class="style23">Dirección:
                    </td>
                    <td class="style24">
                        <asp:TextBox ID="txtDireccion" runat="server" Width="235px" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">
                        <asp:Label ID="lblCargo" Text="Cargo" runat="server" />
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="tx_cargo1" runat="server" Width="235px" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Teléfono:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="tx_telefono1" runat="server" Width="235px" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" BackColor="White"
                            ControlToValidate="tx_telefono1" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                            Style="font-size: small; color: #FF0000;">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr id="tr_Celular" runat="server" visible="false">
                    <td class="style22"></td>
                    <td class="style23">Celular:
                    </td>
                    <td class="style24">
                        <asp:TextBox ID="txtCelular" runat="server" Width="235px" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Fax:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="tx_fax1" runat="server" Width="235px" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr id="tr_Departamento" runat="server" visible="false">
                    <td class="style22"></td>
                    <td class="style39" valign="baseline">Departamento:
                    </td>
                    <td class="style40" valign="baseline">
                        <asp:DropDownList ID="dd_deptos" runat="server" Height="16px" Width="116px" AutoPostBack="True"
                            OnSelectedIndexChanged="dd_deptos_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr id="tr_Ciudad" runat="server" visible="false">
                    <td class="style22"></td>
                    <td class="style39" valign="baseline">Ciudad:
                    </td>
                    <td class="style40" valign="baseline">
                        <asp:DropDownList ID="dd_ciudad" runat="server" Height="16px" Width="116px">
                        </asp:DropDownList>
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style27"></td>
                    <td class="style28" valign="baseline">Clave:
                    </td>
                    <td class="style29" valign="baseline">••••••••&nbsp;
                        <asp:HyperLink ID="h_clave1" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                    </td>
                    <td class="style30"></td>
                </tr>
                <asp:Panel ID="pnl_exp_interventor" runat="server" Visible="false">
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Anexo de documentos básicos:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:ImageButton ID="Img_Btn_Nuevo_Doc_interventor" ImageUrl="../../Images/icoClip.gif"
                                runat="server" OnClick="Img_Btn_Nuevo_Doc_interventor_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="Img_Btn_Ver_Doc_interventor" ImageUrl="../../Images/icoClip2.gif"
                                runat="server" OnClick="Img_Btn_Ver_Doc_interventor_Click" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia Profesional:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_exp_int_profesional" runat="server" Height="107px" Width="525px"
                                TextMode="MultiLine" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Resumen Hoja de Vida:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_int_res_HV" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia e Intereses:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_exp_int_experi_intere" runat="server" Height="107px" Width="525px"
                                TextMode="MultiLine" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Sector Principal de Interventoría:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:DropDownList ID="dd_sector_princ_int" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia en el Sector Principal:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_exp_sector_principal" runat="server" Height="107px" Width="525px"
                                TextMode="MultiLine" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Sector Secundario de Interventoría:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:DropDownList ID="dd_sector_second_int" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia en el Sector Secundario:
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_exp_sector_secundario" runat="server" Height="107px" Width="525px"
                                TextMode="MultiLine" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:ImageButton ID="img_add_interv_infoAcademic" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/Images/icoAdicionarUsuario.gif" Width="19px" OnClientClick='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));' />
                            &nbsp;<asp:HyperLink ID="hlnk_add_interv_infoAcademic" runat="server" NavigateUrl='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));'>Adicionar Información Académica</asp:HyperLink>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:GridView ID="gv_infoAcademic_Interventor" runat="server" AutoGenerateColumns="false"
                                CssClass="Grilla" EmptyDataText="Aún no ha adicionado información académica."
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btn_del_infoAcademic_interventor" CommandArgument='<%# Bind("Id_ContactoEstudio")%>'
                                                OnCommand="Eliminar_Estudios_Realizados" runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                Visible="true" />
                                            <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btn_del_infoAcademic_interventor"
                                                ConfirmText="Desea eliminar esta información académica?" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nivel de Estudio">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Nmb_infoAcademic_interventor" runat="server" Text='<%# Eval("NomNivelEstudio") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Título Obtenido">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lbl_Titulo_infoAcademic_interventor" runat="server" Text='<%# Eval("TituloObtenido") %>' />
                                            <%--NavigateUrl='<%# Eval("URL") %>'></asp:HyperLink>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Institución">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Institucion_infoAcademic_interventor" runat="server" Text='<%# Eval("Institucion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Año del Título">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_year_infoAcademic_interventor" runat="server" Text='<%# Eval("AnoTitulo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=""></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ciudad_infoAcademic_interventor" runat="server" Text='<%# "" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td class="style25"></td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td colspan="3" align="center">
                        <asp:Button ID="btn_actualizar1" runat="server" Text="Actualizar" OnClick="btn_actualizar1_Click" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelAsesor" runat="server" Visible="false" Width="98%" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="tb_PanelAsesor" runat="server">
                <table width="98%" border="0">
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt">Datos Personales
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33">Nombres:
                        </td>
                        <td class="style24">
                            <asp:Label ID="l_nombre2" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33">Apellidos:
                        </td>
                        <td class="style24">
                            <asp:Label ID="l_apellido2" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33">Número de Identificación:
                        </td>
                        <td class="style24" id="2">
                            <asp:Label ID="l_identificacion2" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33" valign="baseline">Departamento expedición:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddl_departamento2" runat="server" Height="16px" Width="132px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_departamento2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33" valign="baseline">Ciudad de expedición:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddl_ciudad2" runat="server" Height="16px" Width="132px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr id="rowRegional" runat="server">
                        <td class="style22"></td>
                        <td class="style33" valign="baseline">Regional Lider:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddlRegioanles" runat="server" Height="16px" Width="132px" Enabled="false">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33" valign="baseline">Correo Electrónico:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:Label ID="l_email2" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia Docente
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="tx_experiencia2" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" BackColor="White"
                                ControlToValidate="tx_experiencia2" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style33" valign="baseline">Dedicación a la Unidad:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddl_decalracion2" runat="server" Height="16px" Width="132px" />
                        </td>
                        <td class="style25">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Resumen Hoja de Vida
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="tx_hojadevida2" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" BackColor="White"
                                ControlToValidate="tx_hojadevida2" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Experiencia e Interes
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="tx_interes2" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" BackColor="White"
                                ControlToValidate="tx_interes2" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btn_actualizar2" runat="server" Text="Actualizar" OnClick="btn_actualizar2_Click" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="style34">Información Académica
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="style36">
                            <asp:ImageButton ID="Ibtn_AdicionarAcad2" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/Images/add.png" Width="19px" OnClientClick='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));' />
                            <asp:HyperLink ID="h_addInfoacademica2" runat="server" NavigateUrl='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));'>Adicionar Información Académica</asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="style25" style="overflow: auto; height: 100%; width: 80%">
                            <asp:GridView ID="gvestudiosrealizadosasesor" CssClass="Grilla" runat="server" DataSourceID="lds_estudiosAsesor"
                                AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="Aún no ha adicionado información académica."
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminarEstudioAS" CommandArgument='<%# Bind("Id_ContactoEstudio")%>'
                                                OnCommand="Eliminar_Estudios_Realizados" runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                Visible="true" />
                                            <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnEliminarEstudioAS"
                                                ConfirmText="Desea eliminar esta información académica?" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nivel de Estudio">
                                        <ItemTemplate>
                                            <asp:Label ID="lnivelestudioAS" runat="server" Text='<%# Eval("NomNivelEstudio") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Título Obtenido">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="ltituloobtenidoAS" runat="server" Text='<%# Eval("TituloObtenido") %>'
                                                NavigateUrl='<%# Eval("URL") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Institución">
                                        <ItemTemplate>
                                            <asp:Label ID="linstitucionAS" runat="server" Text='<%# Eval("Institucion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Año del Título">
                                        <ItemTemplate>
                                            <asp:Label ID="lañotituloAS" runat="server" Text='<%# Eval("AnoTitulo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=""></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad">
                                        <ItemTemplate>
                                            <asp:Label ID="lciudadAS" runat="server" Text='<%# "" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: auto;">Clave:
                        </td>
                        <td class="style24">••••••••&nbsp;
                            <asp:HyperLink ID="h_clave2" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelJefeUnidad" runat="server" Visible="false" Width="98%"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="tb_PanelJefeUnidad" runat="server">
                <table width="98%" border="0">
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt">Información de la Unidad
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Nombre Centro o Institución:
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_nomcentro3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">NIT. Centro o Institución:
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_nit3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr id="tr_ICFES" runat="server" visible="false">
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Registro ICFES:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_icfes3" runat="server" Width="219px" MaxLength="25" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" BackColor="White"
                                ControlToValidate="tx_icfes3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3568" ValidationExpression="^[0-9]*"
                                ControlToValidate="tx_icfes3" runat="server" ForeColor="Red" Display="Dynamic"
                                ErrorMessage="* Este campo debe ser numérico." />
                        </td>
                    </tr>
                    <tr id="tr_FECHA_REGISTRO" runat="server" visible="false">
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Fecha de Registro:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox runat="server" ID="tx_fregistro2" Text="" Enabled="False" BackColor="White" />&nbsp;
                            <asp:Image runat="server" ID="btnDate2" AlternateText="cal2" ImageUrl="/images/icoModificar.gif"></asp:Image>
                            <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="tx_fregistro2" Format="dd/MM/yyyy"
                                runat="server" PopupButtonID="btnDate2" />
                            &nbsp;
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" BackColor="White"
                                ControlToValidate="tx_fregistro2" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Departamento:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:DropDownList ID="ddl_deparunidad3" runat="server" Height="16px" Width="116px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_deparunidad3_SelectedIndexChanged"
                                Enabled="False" EnableTheming="False" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Ciudad:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:DropDownList ID="ddl_ciudadunidad3" runat="server" Height="16px" Width="116px" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Dirección Correspondencia
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_correspondencia3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Teléfono:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_telunidad3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" BackColor="White"
                                ControlToValidate="tx_telunidad3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Fax:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_faxunidad3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" BackColor="White"
                                ControlToValidate="tx_faxunidad3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Sitio Web:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_sitioweb3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" BackColor="White"
                                ControlToValidate="tx_sitioweb3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" class="style35" style="font-weight: 700; font-size: 10pt">Información del Contacto
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Nombres:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:Label ID="l_nombre3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Apellidos:
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_apellido3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Número de Identificación:
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_identificacion3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Departamento de Expedición:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:DropDownList ID="ddl_departamento3" runat="server" Height="16px" Width="116px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_departamento3_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Ciudad de Expedición:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:DropDownList ID="ddl_ciudad3" runat="server" Height="16px" Width="116px" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Cargo:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_cargo3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" BackColor="White"
                                ControlToValidate="tx_cargo3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Correo Electrónico:
                        </td>
                        <td class="style40">
                            <asp:Label ID="l_email3" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Teléfono:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_telefono3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" BackColor="White"
                                ControlToValidate="tx_telefono3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Fax:
                        </td>
                        <td class="style40" valign="baseline">
                            <asp:TextBox ID="tx_fax3" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" BackColor="White"
                                ControlToValidate="tx_fax3" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">&nbsp;
                        </td>
                        <td class="style38">Clave:
                        </td>
                        <td class="style41">••••••••&nbsp;
                            <asp:HyperLink ID="h_clave3" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btn_actualizar3" runat="server" Text="Actualizar" OnClick="btn_actualizar3_Click" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelEmprendedor" runat="server" Visible="false" Width="98%"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="tb_PanelEmprendedor" runat="server">
                <table width="98%" border="0">
                    <tr>
                        <td colspan="4" class="style47" style="font-weight: 700; font-size: 10pt">Datos Personales
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Nombres:
                        </td>
                        <td class="style49">
                            <asp:Label ID="l_nombre4" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Apellidos:
                        </td>
                        <td class="style49">
                            <asp:Label ID="l_apellidos4" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Número de Identificación:
                        </td>
                        <td class="style49">
                            <asp:Label ID="l_identificacion4" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25" valign="baseline">
                            <asp:ImageButton ID="Image1" runat="server" ImageUrl="~/Images/icoClick.gif" ImageAlign="AbsBottom" OnClick="Image1_Click" CommandName="NuevoDocumento"/>
                            <asp:ImageButton ID="verDocAdjunto" runat="server" ImageUrl="~/Images/icoClick2.gif" ImageAlign="AbsBottom" Visible="false" OnClick="verDocAdjunto_Click" />
                            <span>Adjuntar Documento de identificacion</span>
                            <%--<asp:HyperLink ID="h_adjuntarDocumentoIdent4" runat="server">Ver y adjuntar documento de identificación</asp:HyperLink>--%>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Departamento de Expedición:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_depexped4" runat="server" Height="16px" Width="116px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_depexped4_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Ciudad de Expedición:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="dd_ciuexp4" runat="server" Height="16px" Width="116px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Correo Electrónico:
                        </td>
                        <td class="style49">
                            <asp:Label ID="l_email4" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Genero:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_genero4" runat="server" Height="16px" Width="116px">
                                <asp:ListItem Value="F">Femenino</asp:ListItem>
                                <asp:ListItem Value="M">Masculino</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Fecha de Nacimiento:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:TextBox runat="server" ID="tx_fechanacimiento4" Text="" Enabled="false" BackColor="White" />&nbsp;
                            <asp:Image runat="server" ID="btnimgcalend4" AlternateText="cal2" ImageUrl="/images/icoModificar.gif"
                                ImageAlign="AbsMiddle"></asp:Image>
                            <asp:CalendarExtender ID="CalendarExtender4" TargetControlID="tx_fechanacimiento4"
                                Format="dd/MM/yyyy" runat="server" PopupButtonID="btnimgcalend4" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" BackColor="White"
                                ControlToValidate="tx_fechanacimiento4" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Departamento de Nacimiento:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_departamento4" runat="server" Height="16px" Width="116px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_departamento4_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Ciudad de Nacimiento:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_ciudad4" runat="server" Height="16px" Width="116px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Teléfono:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:TextBox ID="tx_telefono4" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" BackColor="White"
                                ControlToValidate="tx_telefono4" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Direccion de Domicilio:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:TextBox ID="txtDireccionEmprendedor" runat="server" Width="219px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" 
                                BackColor="White"
                                ControlToValidate="txtDireccionEmprendedor" Display="Dynamic"
                                ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <!--Ciudad y departamento de residencia-->
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Departamento de Domicilio:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_DepartamentoDomicilio" runat="server" Height="16px" Width="116px"
                                AutoPostBack="True" OnSelectedIndexChanged="ddl_DepartamentoDomicilio_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39" valign="baseline">Ciudad de Domicilio:
                        </td>
                        <td class="style49" valign="baseline">
                            <asp:DropDownList ID="ddl_CiudadDomicilio" runat="server" Height="16px" Width="116px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <!--FIN Ciudad y departamento de residencia-->
                    <tr>
                        <td colspan="4" class="style34">Información Académica
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">&nbsp;
                        </td>
                        <td colspan="2">
                            <asp:GridView ID="gvestudiosemprendedor" CssClass="Grilla" runat="server" DataSourceID="lds_estudiosEmprendedor"
                                AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="Aún no ha adicionado información académica."
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminarEstudioEmp" CommandArgument='<%# Bind("Id_ContactoEstudio")%>'
                                                OnCommand="Eliminar_Estudios_Emprendedor" Visible='<%# !((int)Eval("FlagIngresadoAsesor") == 0) %>'
                                                runat="server" ImageUrl="/Images/icoBorrar.gif" />
                                            <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnEliminarEstudioEmp"
                                                ConfirmText="Desea eliminar esta información académica?" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nivel de Estudio">
                                        <ItemTemplate>
                                            <asp:Label ID="lnivelestudioEMP" runat="server" Text='<%# Eval("NomNivelEstudio") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Título Obtenido">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="ltituloobtenidoEMP" runat="server" Text='<%# Eval("TituloObtenido") %>'
                                                NavigateUrl='<%#  Eval("URL") %>' Enabled='<%#  Eval("Habilitado") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Institución">
                                        <ItemTemplate>
                                            <asp:Label ID="linstitucionEMP" runat="server" Text='<%# Eval("Institucion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Año del Título">
                                        <ItemTemplate>
                                            <asp:Label ID="lañotituloEMP" runat="server" Text='<%# Eval("AnoTitulo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad">
                                        <ItemTemplate>
                                            <asp:Label ID="lciudadEMP" runat="server" Text='<%# "" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Certificaciones">
                                        <ItemTemplate>
                                            <asp:Label ID="lcertificacionesEMP" runat="server" />
                                            <asp:ImageButton ID="ClipCarga" CommandArgument='<%# Bind("Id_ContactoEstudio")%>' OnCommand="AgregaDocumento" runat="server" ImageUrl="~/Images/icoClick.gif" ImageAlign="AbsBottom" />
                                            <asp:ImageButton ID="verCertificado" CommandArgument='<%# Bind("Id_ContactoEstudio")%>' OnCommand="VerCertificado" runat="server" ImageUrl="~/Images/icoClick2.gif" ImageAlign="AbsBottom" />   
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="style11">&nbsp;
                        </td>
                        <td colspan="2">
                            <asp:ImageButton ID="Ibtn_adicionarIA4" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/Images/add.png" Width="19px" OnClientClick='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));' />
                            <asp:HyperLink ID="h_addInfoacademica4" runat="server" NavigateUrl='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));'>Adicionar Información Académica</asp:HyperLink>
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style45">Clave:
                        </td>
                        <td class="style49">••••••••&nbsp;
                            <asp:HyperLink ID="h_clave4" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btn_actualizar4" runat="server" Text="Actualizar" OnClick="btn_actualizar4_Click" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelGerenteAdmin" runat="server" Visible="false" Width="98%"
        UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="tb_PanelGerenteAdmin" runat="server">
                <table width="98%" border="0">
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt">Datos Personales
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Nombres:
                        </td>
                        <td class="style53" valign="baseline">
                            <asp:Label ID="l_nombres5" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Apellidos:
                        </td>
                        <td class="style53">
                            <asp:Label ID="l_apellidos5" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Número de Identificación:
                        </td>
                        <td class="style53">
                            <asp:Label ID="l_identificacion5" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style39">Correo Electrónico:
                        </td>
                        <td class="style53">
                            <asp:Label ID="l_email5" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style54"></td>
                        <td class="style55" valign="baseline">Clave:
                        </td>
                        <td class="style53" valign="baseline">••••••••&nbsp;
                            <asp:HyperLink ID="h_clave5" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                        </td>
                        <td class="style56"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">&nbsp;
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelEvaluador" runat="server" Visible="false" Width="98%" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="tb_PanelEvaluador" runat="server">
                <table width="98%" border="0">
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt">Datos Personales
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1">Nombres:
                        </td>
                        <td class="style24">
                            <asp:Label ID="l_nombre6" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1">Apellidos:
                        </td>
                        <td class="style24">
                            <asp:Label ID="l_apellidos6" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1">Número de Identificación:
                        </td>
                        <td class="style24" id="Td1">
                            <asp:Label ID="l_identificacion6" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Persona:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:Label ID="l_persona6" runat="server" Style="font-weight: 700" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Correo Electrónico:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:Label ID="l_email6" runat="server" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Dirección:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:TextBox ID="txt_direccion6" runat="server" Width="300px" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Teléfono:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:TextBox ID="txt_telefono6" runat="server" Width="300px" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Fax:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:TextBox ID="txt_fax6" runat="server" Width="300px" />
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Número de cuenta bancaria:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:TextBox ID="txt_numcuenta6" runat="server" Width="200px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" BackColor="White"
                                ControlToValidate="txt_numcuenta6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Tipo de Cuenta:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddl_tipocuenta6" runat="server" Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Banco:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:DropDownList ID="ddl_banco6" runat="server" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1" valign="baseline">Planes de negocio que puede atender:
                        </td>
                        <td class="style24" valign="baseline">
                            <asp:TextBox ID="txt_planes6" runat="server" Width="50px" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" BackColor="White"
                                ControlToValidate="txt_planes6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator134" ValidationExpression="^[0-9]*"
                                ControlToValidate="txt_planes6" runat="server" ForeColor="Red" Display="Dynamic"
                                ErrorMessage="* Este campo debe ser numérico.">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Experiencia General
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_expgeneral6" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" BackColor="White"
                                ControlToValidate="txt_expgeneral6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style34">Resumen Hoja de Vida
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_hojadevida6" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" BackColor="White"
                                ControlToValidate="txt_hojadevida6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Experiencia e Interes
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_expintereses6" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" BackColor="White"
                                ControlToValidate="txt_expintereses6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Sector Principal de Evaluación
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="3" class="style25">
                            <asp:DropDownList ID="ddl_secprincipal6" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Experiencia en el Sector Principal
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_secprincipal6" runat="server" Height="107px" Width="525px" TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" BackColor="White"
                                ControlToValidate="txt_secprincipal6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Sector Secundario de Evaluación
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="3" class="style25">
                            <asp:DropDownList ID="ddl_secsecundario6" runat="server" Width="100%">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="font-weight: 700; font-size: 10pt" class="style35">Experiencia en el Sector Secundario
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:TextBox ID="txt_secsecundario6" runat="server" Height="107px" Width="525px"
                                TextMode="MultiLine" />
                        </td>
                        <td class="style25">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" BackColor="White"
                                ControlToValidate="txt_secsecundario6" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                                Style="font-size: small; color: #FF0000;">
                            </asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <%--<tr>
                        <td colspan="4" class="style34">
                            Información Académica
                        </td>
                    </tr>--%>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="16px" ImageAlign="AbsBottom"
                                ImageUrl="~/Images/icoAdicionarUsuario.gif" Width="19px" OnClientClick='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));' />
                            &nbsp;<asp:HyperLink ID="h_addInfoacademica6" runat="server" NavigateUrl='javascript:void(window.open("IngresarInformacionAcademica.aspx?LoadCode=0","_blank","width=580,height=580,toolbar=no, scrollbars=no, resizable=no"));'>Adicionar Información Académica</asp:HyperLink>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td colspan="2" class="style25">
                            <asp:GridView ID="gvestudiosevaluador" CssClass="Grilla" runat="server" DataSourceID="lds_estudiosAsesor"
                                AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="Aún no ha adicionado información académica."
                                ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminarEstudioAS" CommandArgument='<%# Bind("Id_ContactoEstudio")%>'
                                                OnCommand="Eliminar_Estudios_Realizados" runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                Visible="true" />
                                            <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server" TargetControlID="btnEliminarEstudioAS"
                                                ConfirmText="Desea eliminar esta información académica?" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nivel de Estudio">
                                        <ItemTemplate>
                                            <asp:Label ID="lnivelestudioAS" runat="server" Text='<%# Eval("NomNivelEstudio") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Título Obtenido">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="ltituloobtenidoAS" runat="server" Text='<%# Eval("TituloObtenido") %>'
                                                NavigateUrl='<%# Eval("URL") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Institución">
                                        <ItemTemplate>
                                            <asp:Label ID="linstitucionAS" runat="server" Text='<%# Eval("Institucion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Año del Título">
                                        <ItemTemplate>
                                            <asp:Label ID="lañotituloAS" runat="server" Text='<%# Eval("AnoTitulo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText=""></asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ciudad">
                                        <ItemTemplate>
                                            <asp:Label ID="lciudadAS" runat="server" Text='<%# "" + Eval("NomCiudad") + " (" + Eval("NomDepartamento") + ")" %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="auto-style1">Clave:
                        </td>
                        <td class="style24">••••••••&nbsp;
                            <asp:HyperLink ID="h_clave6" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                        </td>
                        <td class="style25"></td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btn_actualizar6" runat="server" Text="Actualizar" OnClick="btn_actualizar6_Click" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="PanelFiduciaria" runat="server" Visible="false" Width="98%"
        UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%" border="0">
                <tr>
                    <td colspan="4" style="font-weight: 700; font-size: 10pt">Datos Personales
                    </td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Nombres:
                    </td>
                    <td class="style24">
                        <asp:Label ID="Label1" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Apellidos:
                    </td>
                    <td class="style24">
                        <asp:Label ID="Label2" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Número de Identificación:
                    </td>
                    <td class="style24">
                        <asp:Label ID="Label3" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Correo Electrónico:
                    </td>
                    <td class="style24">
                        <asp:Label ID="Label4" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Cargo:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="TextBox1" runat="server" Width="235px" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Teléfono:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="TextBox2" runat="server" Width="235px" />
                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" BackColor="White"
                            ControlToValidate="tx_telefono1" Display="Dynamic" ErrorMessage="* Este campo está vacío"
                            Style="font-size: small; color: #FF0000;">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Fax:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="TextBox3" runat="server" Width="235px" />
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style27"></td>
                    <td class="style28" valign="baseline">Clave:
                    </td>
                    <td class="style29" valign="baseline">••••••••&nbsp;
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                    </td>
                    <td class="style30"></td>
                </tr>
                <tr>
                    <td colspan="3" align="center" class="auto-style2">
                        <asp:Button ID="Button1" runat="server" Text="Actualizar" OnClick="btn_actualizar1_Click" />
                    </td>
                    <td class="auto-style2">&nbsp;
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:UpdatePanel ID="pnlLiiderRegional" runat="server" Visible="false" Width="98%"
        UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tr>
                    <td colspan="4" style="font-weight: 700; font-size: 10pt">
                        Datos Personales
                    </td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Nombres:</td>
                    <td class="style24">
                        <asp:Label ID="lblNombreLider" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Apellidos:</td>
                    <td class="style24">
                        <asp:Label ID="lblApellidosLider" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
                <tr>
                    <td class="style22"></td>
                    <td class="style23">Cédula de Ciudadanía:</td>
                    <td class="style24">
                        <asp:Label ID="lblCedulaLider" runat="server" />
                    </td>
                    <td class="style25"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- ------------------------------------------------------------------------------------------------- --%>
    <asp:Panel ID="Admonfon" runat="server" Visible="false">
        <table>
            <tr>
                <td class="style28" valign="baseline">Clave:
                </td>
                <td class="style29" valign="baseline">••••••••&nbsp;
                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:void(window.open('CambiarClave.aspx','_blank','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'));">Cambiar Clave</asp:HyperLink>
                </td>
                <td class="style30"></td>
            </tr>
        </table>
    </asp:Panel>

    <asp:Panel ID="pnlCallCenter" runat="server" Visible="false">
        <table style="width:100%">
            <tr>
                <td>&nbsp</td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp</td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <a href='CambiarClave.aspx' target="popup" onclick="window.open('CambiarClave.aspx','popup','width=580,height=300,toolbar=no, scrollbars=no, resizable=no'); return false;" >Cambio de clave</a>
                </td>
                <%--<td style="text-align:right">
                    <asp:Button ID="btnActualizaCall" Text="Actualizar datos" runat="server" OnClientClick="javascript:alert('Los datos han sido actualizado con exito!')" />
                </td>--%>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        /*html, body, div, iframe{
            height:auto !important;
        }*/
        .style11
        {
            width: 26px;
        }
        .style12
        {
            width: 157px;
            font-weight: bold;
        }
        .style13
        {
            width: 373px;
        }
        .style22
        {
            width: 26px;
            height: 24px;
        }
        .style23
        {
            width: 157px;
            font-weight: bold;
            height: 24px;
        }
        .style24
        {
            width: 373px;
            height: 24px;
        }
        .style25
        {
            height: 24px;
            font-weight: 700;
            height:100%;
        }
        .style27
        {
            width: 26px;
            height: 29px;
        }
        .style28
        {
            width: 157px;
            font-weight: bold;
            height: 29px;
        }
        .style29
        {
            width: 373px;
            height: 29px;
        }
        .style30
        {
            height: 29px;
        }
        .style33
        {
            width: 157px;
            font-weight: 700;
            height: 24px;
        }
        .style34
        {
            height: 26px;
            font-size: 10pt;
            font-weight: 700;
        }
        .style35
        {
            height: 27px;
        }
        .style36
        {
            height: 24px;
            font-weight: bold;
        }
        .style38
        {
            width: 172px;
            font-weight: bold;
        }
        .style39
        {
            width: 172px;
            font-weight: bold;
            height: 24px;
        }
        .style40
        {
            width: 313px;
            height: 24px;
        }
        .style41
        {
            width: 313px;
        }
        .style45
        {
            width: 172px;
            height: 24px;
            font-weight: 700;
        }
        .style47
        {
            height: 22px;
        }
        .style49
        {
            width: 330px;
            height: 24px;
        }
        .style50
        {
            width: 391px;
        }
        .style53
        {
            width: 332px;
            height: 24px;
        }
        .style54
        {
            width: 26px;
            height: 42px;
        }
        .style55
        {
            width: 157px;
            font-weight: bold;
            height: 42px;
        }
        .style56
        {
            height: 42px;
        }
        .auto-style1
        {
            width: 164px;
            font-weight: 700;
            height: 24px;
        }
        .auto-style2 {
            height: 48px;
        }
    </style>
</asp:Content>
