<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="~/FONADE/interventoria/Interventor.aspx.cs" Inherits="Fonade.FONADE.interventoria.Interventor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style2
        {
        }
        .auto-style3
        {
            width: 280px;
        }
        .auto-style4
        {
        }
        .auto-style5
        {
            width: 40px;
            height: 40px;
        }
        .auto-style6
        {
            height: 40px;
        }
        .auto-style10
        {
            height: 22px;
        }
        .auto-style14
        {
            width: 280px;
            height: 22px;
        }
        .auto-style15
        {
            width: 82px;
        }
        .auto-style16
        {
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700">Nuevo Usuario Coordinador de Interventoria</asp:Label></h1>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PanelCrear" runat="server" Visible="false">
                <table width="95%" border="0" align="center" cellspacing="1" cellpadding="3">
                    <tbody>
                        <tr valign="top">
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <b>Nombres:</b>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_nombreCrear" runat="server" Width="219px" MaxLength="100" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_nombreCrear"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <b>Apellidos:</b>
                            </td>
                            <%--<td width="167" align="left" colspan="3">--%>
                            <td colspan="3">
                                <asp:TextBox ID="txt_apellidosCrear" runat="server" Width="219px" MaxLength="100"
                                    onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt_apellidosCrear"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lbl_t_identificacion" Text="Identificación:" runat="server" Font-Bold="true" />
                            </td>
                            <td width="167" align="left">
                                <asp:DropDownList ID="ddl_tidentificacionCrear" runat="server" Width="151px" />
                            </td>
                            <td valign="middle">
                                <b>No:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_nidentificacionCrear" runat="server" Width="99px" MaxLength="12"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="filtro1" TargetControlID="txt_nidentificacionCrear"
                                    FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_nidentificacionCrear"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lblPersona" Text="Persona:" runat="server" Font-Bold="true" />
                            </td>
                            <td width="167" align="left" colspan="3">
                                <asp:DropDownList ID="dd_persona" runat="server" Width="68px">
                                    <asp:ListItem Value="J">Jurídica</asp:ListItem>
                                    <asp:ListItem Value="N">Natural</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <b>Email:</b>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txt_emailCrear" runat="server" Width="219px" MaxLength="100" onkeypress="if(!isNS4){if (event.keyCode==34 || event.keyCode==39) event.returnValue = false;}else{if (event.which==34 || event.which==39) return false;}" />
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_emailCrear"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_emailCrear"
                                    Display="Dynamic" ErrorMessage="* Formato de Correo Electrónico incorrecto" ForeColor="Red"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <b>Salario Mensual:</b>
                            </td>
                            <td align="left" colspan="3">
                                <asp:TextBox ID="txt_Salario" runat="server" Width="94px" Text="0" MaxLength="15" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_Salario"
                                    ErrorMessage="* El salario debe ser numérico." ValidationExpression="[0-9]*"
                                    ValidationGroup="GrupoSalario" Display="Dynamic" ForeColor="Red" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_Salario"
                                    ErrorMessage="* El Salario es requerido." ValidationGroup="GrupoSalario" ForeColor="Red" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <asp:Panel ID="pnlSectores" runat="server">
                <table id="tb_cr_int" runat="server" width="95%" border="0" align="center" cellspacing="1"
                    cellpadding="3">
                    <tr valign="top">
                        <td colspan="4">
                            <b>Sectores a los que aplica:</b>
                            <br>
                            Para seleccionar varios sectores mantenga presionada la tecla 'Ctrl' mientras hace
                            click
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="LB_Sectores" runat="server" Height="250px" SelectionMode="Multiple"
                                Width="100%" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                    </asp:Panel>
                <tr valign="top">
                        <td colspan="4" align="right">
                            <asp:Button ID="btn_crear" runat="server" Text="Crear" OnClick="btn_crear_Click" />
                        </td>
                    </tr>
            </asp:Panel>
            <asp:Panel ID="PanelModificar" runat="server" Visible="false">
                <table width="95%" border="0" align="center" cellspacing="1" cellpadding="3">
                    <tbody>
                        <tr>
                            <td colspan="1">
                                <b>Nombres:</b>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txt_nombremod" runat="server" Width="219px" MaxLength="100" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_nombremod"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <b>Apellidos:</b>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txt_apellidosmod" runat="server" Width="219px" MaxLength="100" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_apellidosmod"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td colspan="1">
                                <b>Identificación:</b>
                            </td>
                            <td colspan="2" align="left">
                                <asp:DropDownList ID="dd_TipoIdentidicacionModificar" runat="server" Width="151px">
                                </asp:DropDownList>
                            </td>
                            <td valign="middle">
                                <b>No:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txt_nidentificacionmod" runat="server" MaxLength="12" />
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txt_nidentificacionmod"
                                    FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server" />
                                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_nidentificacionmod"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr valign="top" id="t_mod_persona" runat="server" visible="false">
                            <td colspan="1">
                                <b>Persona:</b>
                            </td>
                            <td align="left" colspan="4">
                                <asp:DropDownList ID="ddl_tidentificacionmod" runat="server" Width="68px">
                                    <asp:ListItem Value="J">Jurídica</asp:ListItem>
                                    <asp:ListItem Value="N">Natural</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="left" style="width: 120px;">
                                <b>Email:</b>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_emailmod" runat="server" Width="219px" MaxLength="100" />
                                <asp:HiddenField ID="hdf_email_mod" runat="server" />
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_emailmod"
                                    Display="Dynamic" ErrorMessage="* Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_emailmod"
                                    Display="Dynamic" ErrorMessage="* Formato de Correo Electrónico incorrecto" ForeColor="Red"
                                    ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:Label ID="lbl_s_salario" Text="Salario: " runat="server" Font-Bold="true" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txt_Salariomod" runat="server" Text="0" Width="94px" MaxLength="15" />
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_Salariomod"
                                    ForeColor="Red" Display="Dynamic" ValidationExpression="[0-9]*" ValidationGroup="GrupoSalario"
                                    ErrorMessage="* El valor debe ser numérico" />
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_Salariomod"
                                    ForeColor="Red" ErrorMessage="* El salario es requerido." ValidationGroup="GrupoSalario" />
                            </td>
                        </tr>
                        <tr id="tr_Cargo" runat="server">
                            <td colspan="1">
                                <asp:Label ID="Label2" runat="server" Text="Cargo:" Font-Bold="true" Visible="false" />
                            </td>
                            <td colspan="3">
                                <asp:Label ID="L_Cargo" runat="server" Text="" visible="false"/>
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr_Telefono" runat="server">
                            <td colspan="1">
                                <asp:Label ID="Label1" runat="server" Text="Teléfono:" Font-Bold="True" />
                            </td>
                            <td colspan="3">
                                <asp:Label ID="L_telefono" runat="server" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr_Fax" runat="server">
                            <td colspan="1">
                                <asp:Label ID="L_Faxe" runat="server" Text="Fax:" Font-Bold="True" />
                            </td>
                            <td colspan="3">
                                <asp:Label ID="L_fax" runat="server" Text="" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <asp:LinkButton ID="lnk_btn_VerContratos" Text="Ver contratos.." runat="server" ForeColor="Red"
                                    Width="120px" Visible="false" OnClick="lnk_btn_VerContratos_Click" Style="text-decoration: none;" />
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="95%" id="tb_sectores_aplica" runat="server" visible="false" border="0"
                    align="center" cellspacing="1" cellpadding="3">
                    <tr valign="top">
                        <td>
                            <b>Sectores a los que aplica:</b>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <b>Principal:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblPrincipal" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                        </td>
                        <td>
                            <b>Experiencia</b>
                            <br />
                            <asp:Label ID="lbl_exp_principal" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <b>Secundario:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblSecundario" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                        </td>
                        <td>
                            <b>Experiencia</b>
                            <br />
                            <asp:Label ID="lbl_exp_secundaria" runat="server" Text="" />
                        </td>
                    </tr>
                </table>
                <table id="t_Otrossectores" runat="server" width="95%" border="0" align="center"
                    cellspacing="1" cellpadding="3">
                    <tr>
                        <td colspan="2">
                            <b>Otros sectores</b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Para seleccionar varios sectores mantenga presionada la tecla &#39;Ctrl&#39; mientras
                            hace click
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="ListBox1" runat="server" Height="150px" SelectionMode="Multiple"
                                Width="100%" AutoPostBack="true" />
                        </td>
                    </tr>
                </table>
                <table width="95%" id="tb_interventor" runat="server" visible="false" border="0"
                    align="center" cellspacing="1" cellpadding="3">
                    <tr valign="top">
                        <td colspan="4">
                            <strong>Experiencia General:</strong>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4">
                            <asp:Label ID="lbl_exp_general" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4">
                            <strong>Intereses:</strong>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4">
                            <asp:Label ID="lbl_intereses" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4">
                            <strong>Hoja de Vida:</strong>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4">
                            <asp:Label ID="lbl_HV" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <b>Banco:</b>&nbsp;
                            <asp:Label ID="lblBanco" Text="" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <b>Cuenta No.</b>&nbsp;<asp:Label ID="lblCuentaNo" Text="" runat="server" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="2">
                            <b>Empresas que puede atender: </b>&nbsp;
                            <asp:Label ID="lblEmpresasAtender" Text="" runat="server" />
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="95%" border="0" align="center" cellspacing="1" cellpadding="3">
                    <tr valign="top">
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td align="right">
                            <asp:Button ID="btn_modificar" runat="server" OnClick="btn_modificar_Click" Text="Modificar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_crear" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_modificar" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnk_btn_VerContratos" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
