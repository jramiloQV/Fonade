<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CambiarDatosJefe.aspx.cs"
    Inherits="Fonade.FONADE.Administracion.CambiarDatosJefe" MasterPageFile="~/Emergente.Master" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
     <div class="contenedorPadre">
        <%--Información General--%>
        <%--<h1>
            <asp:Label ID="lbl_enunciado" runat="server" Text="EDITAR DATOS" />
        </h1>--%>
            <asp:HiddenField ID="hdf_tel" runat="server" />
            <asp:HiddenField ID="hdf_fax" runat="server" />
            <asp:HiddenField ID="hdf_CodGrupo" runat="server" />
            <asp:HiddenField ID="hdf_EmailOld" runat="server" />
            <table border="0" style="width: 565px; height: 137px">
          <tr>
            <td class="style26"></td>
            <td class="style13"></td>
            <td class="style27"></td>
            <td class="style28" align="right">
                <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
              </td>
          </tr>
          <tr>
            <td colspan="4" class="style24" style="font-weight: 700; font-size: 12pt" align="center">
                EDITAR DATOS
                <%--<asp:Image ID="Image1" runat="server" Height="30px" ImageAlign="AbsBottom" 
                    ImageUrl="~/Images/key.png" Width="30px" />&nbsp; Cambio de Clave, 
                <asp:Label ID="l_usuariolog" runat="server"></asp:Label>--%>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29"valign="baseline">Nombres</td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_Nombres" runat="server" MaxLength="100" Width="215px" />
              </td>
            <td class="style32">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    BackColor="White" ControlToValidate="txt_claveActual" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>--%>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29" valign="baseline">Apellidos</td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_Apellidos" runat="server" MaxLength="100" Width="215px" />
              </td>
            <td class="style32">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    BackColor="White" ControlToValidate="txt_nuevaclave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatora2" runat="server" display="dynamic"
                ControlToValidate="txt_nuevaclave"  
                ErrorMessage="* La clave debe tener mayúsculas, minúsculas, números y caracteres y debe tener minimo 8 digitos!"
                ValidationExpression="(?=.*\d)(?=.*[a-z])(?=.*[\W]).{8,150}" style="font-size: small; color: #FF0000;">
                </asp:RegularExpressionValidator>--%>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29" valign="baseline">Identificacion</td>
            <td valign="baseline" class="style31">
                <asp:DropDownList ID="dd_listado_TipoIdentificacion" runat="server" AutoPostBack="true"
                                Width="151px" />
              </td>
            <td class="style32">
                <%--<asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txt_nuevaclave" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Las claves no coinciden" style="color: #FF0000" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    BackColor="White" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>--%>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29"valign="baseline">No.</td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_num_identificacion" runat="server" MaxLength="8" Width="90px" />
              </td>
            <td class="style32">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    BackColor="White" ControlToValidate="txt_claveActual" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>--%>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29"valign="baseline">Email</td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_Email" runat="server" MaxLength="100" Width="215px" />
              </td>
            <td class="style32">
                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    BackColor="White" ControlToValidate="txt_claveActual" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>--%>
              </td>
          </tr>
          <tr>
            <td class="style20"></td>
            <td colspan="2" align="center" class="style21">
                <asp:Button ID="Btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="return Cerrar();" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Btn_Actualizar" Text="Actualizar" runat="server" OnClick="Btn_Actualizar_Click" />

                <%--<asp:Button ID="Btn_cambiarClave" runat="server" Text="Cambiar Clave" 
                    onclick="Btn_cambiarClave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" 
                    onclick="Btn_Cancelar_Click" CausesValidation="False" />--%>
              </td>
            <td class="style22"></td>
          </tr>
          <tr>
            <td class="style33"></td>
            <td colspan="2" class="style34"></td>
            <td class="style35" align="right">
                &nbsp;</td>
          </tr>
        </table>
                <%--<div class="separadorEncabezado"></div>--%>
            </div>
            <%--<table width="98%" border="0" cellspacing="0" cellpadding="3">
                <tbody>
                    <tr valign="top">
                        <td class="TitDestacado" align="right">
                            <b>Nombres:</b>
                        </td>
                        <td class="TitDestacado" colspan="3">
                            <asp:TextBox ID="txt_Nombres" runat="server" MaxLength="100" Width="215px" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="TitDestacado" align="right">
                            <b>Apellidos:</b>
                        </td>
                        <td width="167" align="left" colspan="3" class="TitDestacado">
                            <asp:TextBox ID="txt_Apellidos" runat="server" MaxLength="100" Width="215px" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="TitDestacado" align="right">
                            <b>Identificación:</b>
                        </td>
                        <td width="167" align="left" class="TitDestacado">
                            <asp:DropDownList ID="dd_listado_TipoIdentificacion" runat="server" AutoPostBack="true"
                                Width="151px" />
                        </td>
                        <td class="TitDestacado" align="right">
                            <b>No:</b>
                        </td>
                        <td width="167" align="left" class="TitDestacado">
                            <asp:TextBox ID="txt_num_identificacion" runat="server" MaxLength="8" Width="90px" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td class="TitDestacado" align="right">
                            <b>Email:</b>
                        </td>
                        <td width="167" align="left" colspan="3" class="TitDestacado">
                            <asp:TextBox ID="txt_Email" runat="server" MaxLength="100" Width="215px" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" align="right" class="TitDestacado">
                            <asp:Button ID="Btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="return Cerrar();" />
                            <asp:Button ID="Btn_Actualizar" Text="Actualizar" runat="server" OnClick="Btn_Actualizar_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>--%>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="~/../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Cerrar() {
            this.window.close();
        }
    </script>
</asp:Content>
   
