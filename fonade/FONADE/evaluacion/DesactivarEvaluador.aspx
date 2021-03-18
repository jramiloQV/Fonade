<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DesactivarEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.DesactivarEvaluador" MasterPageFile="~/Emergente.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

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
            
            
            <asp:Panel ID="PanelDesactivar" runat="server" Visible="false">

                <table width="500px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Desactivar:</td>
                    <td class="auto-style9">
                        <asp:Label ID="l_nombre" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Cedula de Ciudadanía:</td>
                    <td class="auto-style9">
                        <asp:Label ID="l_cedula" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style11"></td>
                    <td class="auto-style6">Fecha final:</td>
                    <td class="auto-style12">

                        <asp:TextBox runat="server" ID="txt_fechFin"  Text=""
                        Enabled="False" BackColor="White"></asp:TextBox>&nbsp;
                        <asp:Image runat="server" ID="btnDateInicio" AlternateText="cal2" 
                            ImageUrl="/images/icoModificar.gif" ImageAlign="AbsBottom"></asp:Image>
                        <asp:CalendarExtender ID="CalendarExtender1" TargetControlID="txt_fechFin" Format="dd/MM/yyyy" runat="server" PopupButtonID="btnDateInicio"/>

                    </td>
                    <td class="auto-style13"></td>
                  </tr>
                  <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td class="auto-style2">&nbsp;</td>
                    <td class="auto-style5">
                        <asp:CheckBox ID="ch_indefinidamente" runat="server" Text="Desactivar Indefinidamente" />
                      </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>

            </asp:Panel>

            <table width="500px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Motivo:</td>
                    <td class="auto-style9">
                        &nbsp;</td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style7"></td>
                    <td colspan="2">
                        <asp:TextBox ID="txt_motivo" runat="server" Height="60px" TextMode="MultiLine" Width="100%" BackColor="White" Enabled="False"></asp:TextBox>
                      </td>
                    
                    <td class="auto-style10"></td>
                  </tr>

                  <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_desactivar" runat="server" Text="Desactivar" Visible="False" OnClick="btn_desactivar_Click" />

                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server"
                        TargetControlID="btn_desactivar"
                        ConfirmText="¿Desea desactivar este evaluador?"
                        />

                        <asp:Button ID="btn_cerrar" runat="server" Text="Cerrar" Visible="False" OnClick="btn_cerrar_Click" />
                      </td>
                    <td>&nbsp;</td>
                  </tr>
                </table>


        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style2
        {
            width: 149px;
        }
        .auto-style4
        {
            width: 20px;
        }
        .auto-style5
        {
            width: 290px;
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

