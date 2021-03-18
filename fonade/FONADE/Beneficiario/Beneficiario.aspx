<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Beneficiario.aspx.cs" Inherits="Fonade.FONADE.Beneficiario.Beneficiario"
    MasterPageFile="~/Master.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">

    <script type="text/javascript">
        function length(id) {
            var tipoId = id;
        }
    </script>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upbeneficiario" runat="server" UpdateMode="Conditional" Width="98%">
        <ContentTemplate>
            <asp:Panel ID="pnl_Creacion" runat="server">
                <table border="1" cellpadding="0" cellspacing="0" bordercolor="#4e77af">
                    <tbody>
                        <tr>
                            <td align="center" valign="top" width="98%">
                                LOS DATOS QUE REGISTRE DEBEN SER PRECISOS, DE LO CONTRARIO EL PAGO NO SERÁ EFECTUADO.
                                POR FAVOR NO INGRESE PUNTOS NI COMAS NI GUIONES.
                                <br>
                                <br>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table width="98%" border="0">
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td colspan="3">
                            <h1>
                                <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700" /></h1>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Tipo de identificación:
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_tipoid" runat="server" Height="19px" Width="215px" AutoPostBack="true" OnSelectedIndexChanged="ddl_tipoid_SelectedIndexChanged"  />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Número de identificación
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:UpdatePanel ID="udpNumDoc" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txt_numdocumento" runat="server" Width="198px" MaxLength="14" /><br />
                                    <b><small style="color: #CC0000;">(Ingresar sin digito de verificación!)</small></b>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddl_tipoid" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator987" ValidationExpression="^[0-9]*"
                                ControlToValidate="txt_numdocumento" runat="server" ForeColor="Red" Display="Dynamic"
                                ErrorMessage="Llene el campo de Numero de Identificacion con solo numeros, sin digito de verificacion, ni ceros al inicio." />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Nombres
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_nombres" runat="server" Width="198px" MaxLength="100" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3256" runat="server" Display="Dynamic"
                                ControlToValidate="txt_nombres" ErrorMessage="Llene el campo de Nombres." Style="font-size: small;
                                color: #FF3300;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Apellidos
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_apellidos" runat="server" Width="198px" MaxLength="100" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator769" runat="server" Display="Dynamic"
                                ControlToValidate="txt_apellidos" ErrorMessage="Llene el campo de Apellidos."
                                Style="font-size: small; color: #FF3300;" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Razón Social
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_rsocial" runat="server" Width="198px" MaxLength="100" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Tipo Sociedad
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_tsociedad" runat="server" Height="19px" Width="215px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Tipo de retención
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_tretencion" runat="server" Height="19px" Width="303px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Ciudad de Pago
                            <br />
                            <small style="font-weight: normal;">(Si no encuentra su ciudad, seleccione la más cercana)</small>
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_ciudadpago" runat="server" Height="19px" Width="303px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Teléfono
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_telefono" runat="server" Width="198px" MaxLength="20" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Dirección
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_direccion" runat="server" Width="198px" MaxLength="40" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Fax
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_fax" runat="server" Width="198px" MaxLength="20" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Email
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_email" runat="server" Width="198px" MaxLength="60" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator981" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ControlToValidate="txt_email" runat="server" ForeColor="Red" Display="Dynamic"
                                ErrorMessage="* Ingrese una dirección de correo válida." />
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Banco
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_banco" runat="server" Height="19px" Width="215px" OnSelectedIndexChanged="ddl_banco_SelectedIndexChanged1"
                                AutoPostBack="True" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Sucursal
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_sucursal" runat="server" Height="19px" Width="215px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Tipo de Cuenta
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:DropDownList ID="ddl_tcuenta" runat="server" Height="19px" Width="215px">
                                <asp:ListItem Value="">Seleccione el tipo de cuenta</asp:ListItem>
                                <asp:ListItem Value="1">Ahorros</asp:ListItem>
                                <asp:ListItem Value="2">Corriente</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td class="style16" valign="baseline">
                            Número de Cuenta
                        </td>
                        <td class="style17" valign="baseline">
                            <asp:TextBox ID="txt_ncuenta" runat="server" Width="198px" MaxLength="20" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="style10">
                            &nbsp;
                        </td>
                        <td class="style11">
                            &nbsp;
                        </td>
                        <td colspan="2" align="center">
                            <asp:Button ID="btn_crear" runat="server" Enabled="False" Text="Crear" Visible="False"
                                OnClick="btn_crear_Click" />
                            <asp:Button ID="btn_Actualizar" runat="server" Enabled="False" Text="Actualizar"
                                Visible="False" OnClick="btn_Actualizar_Click" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnl_Validacion" runat="server" Visible="false">
                <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4e77af">
                    <tr>
                        <td align="center" valign="top" width="98%">
                            <br>
                            <br>
                            SU PROYECTO DEBE ESTAR EN EJECUCIÓN PARA PODER REALIZAR REGISTRO DE BENEFICIARIOS.
                            <br>
                            <br>
                            <br>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style10
        {
            width: 9px;
        }
        .style11
        {
            width: 20px;
        }
        .style16
        {
            width: 201px;
            font-weight: bold;
        }
        .style17
        {
            width: 308px;
        }
    </style>
</asp:Content>
