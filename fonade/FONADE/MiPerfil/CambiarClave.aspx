<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CambiarClave.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.CambiarClave"
    MasterPageFile="~/Emergente.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelCambioDeClave" runat="server" Visible="true" Width="100%"
        UpdateMode="Conditional">
        <contenttemplate>
        <table border="0" style="width: 565px; height: 137px; background-color: #FFFFFF;">
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
                <asp:Image ID="Image1" runat="server" Height="30px" ImageAlign="AbsBottom" 
                    ImageUrl="~/Images/key.png" Width="30px" />&nbsp; Cambio de Clave, 
                <asp:Label ID="l_usuariolog" runat="server"></asp:Label>
              </td>

            
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29"valign="baseline">Clave Actual:<br /><span style="font-size:smaller;">(Máximo 20 caracteres)</span></td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_claveActual" runat="server" Width="170px" 
                    TextMode="Password" MaxLength="20"></asp:TextBox>
              </td>
            <td class="style32">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    BackColor="White" ControlToValidate="txt_claveActual" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
            

              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29" valign="baseline">Nueva Clave:<br /><span style="font-size:smaller;">(Máximo 20 caracteres)</span></td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_nuevaclave" runat="server" Width="170px" 
                    TextMode="Password" MaxLength="20"></asp:TextBox>
              </td>
            <td class="style32">
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    BackColor="White" ControlToValidate="txt_nuevaclave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatora2" runat="server" display="dynamic"
                ControlToValidate="txt_nuevaclave"  
                ErrorMessage="* La clave debe tener al menos una mayúscula, una minúsculas, un número, un carácter y debe tener al menos 8 digitos, no es permitido ingresar el usuario en el password"
                style="font-size: small; color: #FF0000;"></asp:RegularExpressionValidator>
              </td>
          </tr>
          <tr>
            <td class="style30"></td>
            <td class="style29" valign="baseline">Confirmar Nueva Clave:<span style="font-size:smaller;"><br /> (Máximo 20 caracteres)</span></td>
            <td valign="baseline" class="style31">
                <asp:TextBox ID="txt_confirmaNuevaClave" runat="server" Width="170px" 
                    TextMode="Password" MaxLength="20"></asp:TextBox>
              </td>
            <td class="style32">
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="txt_nuevaclave" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Las claves no coinciden" style="color: #FF0000" Display="Dynamic"></asp:CompareValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    BackColor="White" ControlToValidate="txt_confirmaNuevaClave" 
                    ErrorMessage="* Este campo está vacío" Display="Dynamic"
                    style="font-size: small; color: #FF0000;">
                </asp:RequiredFieldValidator>
              </td>
          </tr>
          <tr>
            <td class="style20"></td>
            <td colspan="2" align="center" class="style21">
                <asp:Button ID="Btn_cambiarClave" runat="server" Text="Cambiar Clave" 
                    onclick="Btn_cambiarClave_Click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Btn_Cancelar" runat="server" Text="Cancelar" 
                    onclick="Btn_Cancelar_Click" CausesValidation="False" />
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


     </contenttemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style11
        {
            height: 65px;
        }
        .style13
        {
            width: 145px;
            height: 27px;
        }
        .style16
        {
            height: 65px;
            width: 104px;
        }
        .style19
        {
            height: 65px;
            width: 160px;
        }
        .style20
        {
            width: 104px;
            height: 63px;
        }
        .style21
        {
            height: 63px;
        }
        .style22
        {
            width: 160px;
            height: 63px;
        }
        .style24
        {
            height: 60px;
        }
        .style26
        {
            width: 104px;
            height: 27px;
        }
        .style27
        {
            height: 27px;
        }
        .style28
        {
            width: 160px;
            height: 27px;
        }
        .style29
        {
            width: 145px;
            font-weight: bold;
            height: 30px;
        }
        .style30
        {
            width: 104px;
            height: 30px;
        }
        .style31
        {
            height: 30px;
        }
        .style32
        {
            width: 160px;
            height: 30px;
        }
        .style33
        {
            width: 104px;
            height: 19px;
        }
        .style34
        {
            height: 19px;
        }
        .style35
        {
            width: 160px;
            height: 19px;
        }
    </style>
</asp:Content>
