<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContratoEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ContratoEvaluador"  MasterPageFile="~/Emergente.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <script language="javascript">
        window.onload = zoom;
        function zoom()
        {
          self.moveTo(0,0);
          self.resizeTo(600, 400);
        }
    </script>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="500px">
              <tr>
                <td class="auto-style19" >
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>
            
            
            <asp:Panel ID="PanelCrear" runat="server" Visible="false">

                <table width="500px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Número de Contrato:</td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txt_numero" runat="server" ValidationGroup="accionncrear"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="filtro1" TargetControlID="txt_numero" FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server"/>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style11">&nbsp;</td>
                    <td class="auto-style6">Fecha Inicial:</td>
                    <td class="auto-style12">
                        <asp:TextBox ID="txt_fechainicio" runat="server" BackColor="White" Enabled="False" Text="" ValidationGroup="accionncrear"></asp:TextBox>
                        &nbsp;
                        <asp:Image ID="btnDateInicio" runat="server" AlternateText="cal2" ImageAlign="AbsBottom" ImageUrl="/images/icoModificar.gif" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnDateInicio" TargetControlID="txt_fechainicio" />
                    </td>
                    <td class="auto-style13"></td>
                    <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Meses:</td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txt_meses" runat="server" Width="40px" ValidationGroup="accionncrear"></asp:TextBox>
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" TargetControlID="txt_meses" FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server"/>
                    </td>
                    <td class="auto-style10">
                      </td>
                  </tr>
                    <tr>
                        <td class="auto-style4">&nbsp;</td>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Crear" runat="server" Text="Crear" OnClick="btn_Crear_Click" ValidationGroup="accionncrear" />
                            
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

            </asp:Panel>

            <asp:Panel ID="PanelModificar" runat="server" Visible="false">

                <table width="500px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Número de Contrato:</td>
                    <td class="auto-style9">
                        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" TargetControlID="txt_numero" FilterMode="ValidChars" FilterType="Custom" ValidChars="1234567890" runat="server"/>
                        <asp:Label ID="lnumero" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style11">&nbsp;</td>
                    <td class="auto-style6">Fecha Inicial:</td>
                    <td class="auto-style12">
                        
                        <asp:Label ID="lfechainicio" runat="server"></asp:Label>
                        
                    </td>
                    <td class="auto-style13"></td>
                    <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Fecha de Expiración:</td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txt_fechafin" runat="server" BackColor="White" Enabled="False" Text="" ValidationGroup="accionupdate"></asp:TextBox>
                        &nbsp;
                        <asp:Image ID="btnDateFin" runat="server" AlternateText="cal2" ImageAlign="AbsBottom" ImageUrl="/images/icoModificar.gif" />
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnDateFin" TargetControlID="txt_fechafin" />
                    </td>
                    <td class="auto-style10">
                      </td>
                  </tr>
                    
                </table>

                <table width="500px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Motivo de Modificación:</td>
                    <td class="auto-style9">
                        &nbsp;</td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style7"></td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_motivo" runat="server" Height="60px" TextMode="MultiLine" Width="100%" BackColor="White" ValidationGroup="accionupdate"></asp:TextBox>
                      </td>
                    
                    <td class="auto-style10"></td>
                  </tr>

                  <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" OnClick="btn_actualizar_Click" ValidationGroup="accionupdate" />
                      </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>


            </asp:Panel>

           

        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style4
        {
            width: 20px;
        }
        .auto-style6
        {
            width: 149px;
            font-weight: bold;
            height: 31px;
        }
        .auto-style7
        {
            width: 20px;
            height: 24px;
        }
        .auto-style8
        {
            width: 149px;
            font-weight: bold;
            height: 24px;
        }
        .auto-style9
        {
            width: 290px;
            height: 24px;
        }
        .auto-style10
        {
            height: 24px;
        }
        .auto-style11
        {
            width: 20px;
            height: 31px;
        }
        .auto-style12
        {
            width: 290px;
            height: 31px;
        }
        .auto-style13
        {
            height: 31px;
        }
    </style>
</asp:Content>

