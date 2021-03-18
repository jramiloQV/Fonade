<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConfiguracionAuditoria.aspx.cs" Inherits="Fonade.FONADE.Auditoria.ConfiguracionAuditoria"  MasterPageFile="~/Master.Master" %>
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
     <table width="100%">
      <tr>
        <td class="style10">&nbsp;</td>
        <td class="style11">&nbsp;</td>
        <td>&nbsp;</td>
      </tr>
      <tr>
        <td class="style10">&nbsp;</td>
        <td class="style11" align="center">
            <asp:CheckBox ID="Ch_activar_auditoria" runat="server" AutoPostBack="true" 
                style="font-size: 14pt; text-align: left" 
                Text="Activar o Inactivar Módulo de Auditoría" 
                oncheckedchanged="Ch_activar_auditoria_CheckedChanged" />
          </td>
        <td>&nbsp;</td>
      </tr>
    </table>

    <asp:Panel ID="panelEventos" runat="server"  width="100%" Visible= "false">
        
        <table width="100%">
          <tr>
            <td class="style10">&nbsp;</td>
            <td class="style11" align="center">
                &nbsp;&nbsp;
                <asp:Label ID="l_informativo" runat="server" 
                    
                    
                    Text=" Seleccione los eventos que desea auditar en la base de datos de Fonade"></asp:Label>
              </td>
              <td>&nbsp;</td>
          </tr>
         </table>
         <table width="100%">
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style24">
                <asp:CheckBox ID="ch_insercion" runat="server" 
                    style="font-size: 13px; font-weight: 700" Text="Inserción" />
              </td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style24">
                <asp:CheckBox ID="ch_actualizacion" runat="server" 
                    style="font-size: 13px; font-weight: 700" Text="Actualización" />
              </td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style24">
                <asp:CheckBox ID="ch_eliminacion" runat="server" 
                    style="font-size: 13px; font-weight: 700" Text="Eliminación" />
              </td>
            <td>&nbsp;</td>
          </tr>
        </table>

    </asp:Panel>
        
                <table width="100%">
                  <tr>
                    <td class="style10">&nbsp;</td>
                    <td class="style11" align="center">
                        
                        
                        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" 
                            onclick="btn_Guardar_Click" />
                        
                        
                      </td>
                      <td>&nbsp;</td>
                  </tr>
                                    <tr>
                    <td colspan="2" align="right">
                        
                        <asp:Label ID="l_ultima_actualizacion" runat="server" 
                            style="color: #990000; font-size: 11px"></asp:Label>                      
                        
                      </td>
                      <td class="style30"></td>
                  </tr>
                </table>

    </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style10
        {
            width: 30px;
        }
        .style11
        {
            width: 550px;
        }
        .style24
        {
            width: 139px;
        }
        .style27
        {
            width: 237px;
        }
        .style28
        {
            width: 10px;
            height: 40px;
        }
        .style29
        {
            width: 550px;
            height: 40px;
        }
        .style30
        {
            height: 40px;
        }
    </style>
</asp:Content>
