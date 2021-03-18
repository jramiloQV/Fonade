<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Evaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.Evaluador"  MasterPageFile="~/Master.Master"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="100%">
              <tr>
                <td>
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
              </tr>
            </table>
            <asp:Panel ID="PanelCrear" runat="server" Visible="false">

            <table width="100%">
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Nombres:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_nombreCrear" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_nombreCrear" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Apellidos:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_apellidosCrear" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_apellidosCrear" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Tipo de Identificación:</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddl_tidentificacionCrear" runat="server" Width="50%">
                    </asp:DropDownList>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Número de Identificación:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_nidentificacionCrear" runat="server" Width="50%"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="filtro1" TargetControlID="txt_nidentificacionCrear" FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server"/>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_nidentificacionCrear" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Persona:</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddl_personaCrear" runat="server" Width="100px">
                        <asp:ListItem Value="N">Natural</asp:ListItem>
                        <asp:ListItem Value="J">Jurídica</asp:ListItem>
                    </asp:DropDownList>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Correo Electrónico:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_emailCrear" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style15">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*Este campo está vacío" ForeColor="Red" ControlToValidate="txt_emailCrear" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="*Formato de Correo Electrónico incorrecto" ForeColor="Red" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txt_emailCrear" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Sectores a los que aplica:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2">
                    <asp:Panel ID="PanelListaCrear" runat="server" Height="170px" ScrollBars="Vertical" Width="100%">
                        <asp:CheckBoxList ID="cbl_listaSectorCrear" CssClass="Grilla" runat="server" Width="100%" Height="26px" style="font-size: 11px">
                        </asp:CheckBoxList>
                        
                    </asp:Panel>
                  </td>
                <td class="auto-style15">
                    &nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style5"></td>
                <td colspan="2" align="center" class="auto-style6">
                    <asp:Button ID="btn_crear" runat="server" Text="Crear" OnClick="btn_crear_Click" />
                </td>
                <td class="auto-style6">
                </td>
              </tr>
            </table>

            </asp:Panel>

            <asp:Panel ID="PanelModificar" runat="server" Visible="false">

            <table width="100%">
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Nombres:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_nombremod" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_nombremod" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Apellidos:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_apellidosmod" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_apellidosmod" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Tipo de Identificación:</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddl_tidentificacionmod" runat="server" Width="50%">
                    </asp:DropDownList>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Número de Identificación:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_nidentificacionmod" runat="server" Width="50%"></asp:TextBox>
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txt_nidentificacionmod" FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server"/>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_nidentificacionmod" Display="Dynamic" ErrorMessage="*Este campo está vacío" ForeColor="Red"></asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Persona:</td>
                <td class="auto-style3">
                    <asp:DropDownList ID="ddl_personaMod" runat="server" Width="100px">
                        <asp:ListItem Value="N">Natural</asp:ListItem>
                        <asp:ListItem Value="J">Jurídica</asp:ListItem>
                    </asp:DropDownList>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Correo Electrónico:</td>
                <td class="auto-style3">
                    <asp:TextBox ID="txt_emailmod" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style15">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*Este campo está vacío" ForeColor="Red" ControlToValidate="txt_emailmod" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*Formato de Correo Electrónico incorrecto" ForeColor="Red" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txt_emailmod" Display="Dynamic"></asp:RegularExpressionValidator>
                </td>
              </tr>

            <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Cargo:</td>
                <td class="auto-style3">
                    
                    <asp:Label ID="lcargo" runat="server"></asp:Label>
                    
                  </td>
                <td class="auto-style15">
                </td>
              </tr>
                <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Teléfono:</td>
                <td class="auto-style3">
                    
                    <asp:Label ID="ltelefono" runat="server"></asp:Label>
                    
                  </td>
                <td class="auto-style15">
                </td>
              </tr>
                <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">Fax:</td>
                <td class="auto-style3">
                    
                    <asp:Label ID="lfax" runat="server"></asp:Label>
                    
                  </td>
                <td class="auto-style15">
                </td>
              </tr>
                <tr>
                <td class="auto-style12"></td>
                <td colspan="2" align="center" class="auto-style10">
                    
                    <asp:Button ID="btn_vercontrator" runat="server" CssClass="boton_Link" Text="Ver Contratos" OnClick="btn_vercontrator_Click" />
                    
                  </td>
                <td class="auto-style10">
                </td>
              </tr>

              <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Sectores a los que aplica:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4"><strong>Sector Principal:</strong></td>
                <td class="auto-style3">
                    <asp:Label ID="lsectorprincipalmod" runat="server"></asp:Label>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style3">
                    <strong>Experiencia</strong></td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style7"></td>
                <td class="auto-style8"></td>
                <td class="auto-style9">
                    <asp:Label ID="lexperienciaprincipal" runat="server"></asp:Label>
                  </td>
                <td class="auto-style11"></td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4"><strong>Sector Secundario:</strong></td>
                <td class="auto-style3">
                    <asp:Label ID="lsectorsecundariomod" runat="server"></asp:Label>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style3">
                    <strong>Experiencia</strong></td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4">&nbsp;</td>
                <td class="auto-style3">
                    <asp:Label ID="lexperienciasecundario" runat="server"></asp:Label>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Otros Sectores:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2">
                    <asp:Panel ID="Panel2" runat="server" Height="170px" ScrollBars="Vertical" Width="100%">
                        <asp:CheckBoxList ID="cbl_listaSectormod" CssClass="Grilla" runat="server" Width="100%" Height="26px" style="font-size: 11px">
                        </asp:CheckBoxList>
                        
                    </asp:Panel>
                  </td>
                <td class="auto-style15">
                    &nbsp;</td>
              </tr>
            <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Experiencia General:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2">
                    <asp:Label ID="lexperienciageneral" runat="server"></asp:Label>
                  </td>
                <td class="auto-style15">
                    &nbsp;</td>
              </tr>
                <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Intereses:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2">
                    <asp:Label ID="lintereses" runat="server"></asp:Label>
                  </td>
                <td class="auto-style15">
                    &nbsp;</td>
              </tr>
                <tr>
                <td class="auto-style2"></td>
                <td colspan="2"><strong>Hoja de Vida:</strong></td>
                <td class="auto-style15">
                </td>
              </tr>
              <tr>
                <td class="auto-style2"></td>
                <td colspan="2">
                    <asp:Label ID="lHojaDeVida" runat="server"></asp:Label>
                  </td>
                <td class="auto-style15">
                    &nbsp;</td>
              </tr>
                            <tr>
                <td class="auto-style2"></td>
                <td class="auto-style4"><strong>Banco:</strong></td>
                <td class="auto-style3">
                    <asp:Label ID="l_banco" runat="server"></asp:Label>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style12"></td>
                <td class="auto-style13"><strong>Cuenta:</strong></td>
                <td class="auto-style14">
                    <asp:Label ID="l_cuenta" runat="server"></asp:Label>
                    </td>
                <td class="auto-style14"></td>
              </tr>
              <tr>
                <td class="auto-style7"></td>
                <td colspan="2"><strong>Planes de Negocio a los que puede pertenecer:</strong>
                    <asp:Label ID="l_numPlanes" runat="server"></asp:Label>
                  </td>
                <td class="auto-style11"></td>
              </tr>
              <tr>
                <td class="auto-style5"></td>
                <td colspan="2" align="center" class="auto-style6">
                    <asp:Button ID="btn_modificar" runat="server" Text="Modificar" OnClick="btn_modificar_Click" />
                </td>
                <td class="auto-style6">
                </td>
              </tr>
            </table>

            </asp:Panel>

            

        </ContentTemplate>
    </asp:UpdatePanel>

    </asp:Content>

<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2
        {
            width: 40px;
        }
        .auto-style3
        {
            width: 280px;
        }
        .auto-style4
        {
            width: 172px;
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
        .auto-style7
        {
            width: 40px;
            height: 23px;
        }
        .auto-style8
        {
            width: 172px;
            height: 23px;
        }
        .auto-style9
        {
            width: 280px;
            height: 23px;
        }
        .auto-style10
        {
            height: 22px;
        }
        .auto-style11
        {
            height: 23px;
        }
        .auto-style12
        {
            width: 40px;
            height: 22px;
        }
        .auto-style13
        {
            width: 172px;
            height: 22px;
        }
        .auto-style14
        {
            width: 280px;
            height: 22px;
        }
    </style>
</asp:Content>


