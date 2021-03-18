<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConvocatoriaReglaSalarios.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.ConvocatoriaReglaSalarios" MasterPageFile="~/Emergente.Master" EnableSessionState="True" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Width="70%" UpdateMode="Conditional">
        <ContentTemplate>            
            <table width="70%" >
              <tr>
                <td class="style28">
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>

            <table width="70%">
              <tr>
                <td class="style24"></td>
                <td class="style11"><b>Condición
                    <asp:Label ID="l_Numcondicion" runat="server">0</asp:Label>
                    . si los empleos generados son o están</b></td>
                <td class="style15">
                    <asp:DropDownList ID="ddl_condicion" runat="server" Width="60px" 
                        CssClass="bold" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged">
                        <asp:ListItem>=</asp:ListItem>
                        <asp:ListItem>&lt;</asp:ListItem>
                        <asp:ListItem>&gt;</asp:ListItem>
                        <asp:ListItem>&lt;=</asp:ListItem>
                        <asp:ListItem>&gt;=</asp:ListItem>
                        <asp:ListItem>Entre</asp:ListItem>
                    </asp:DropDownList>
                  </td>
                <td class="style27" align="center">
                    <asp:TextBox ID="txt_numero1" runat="server" Width="30px" CssClass="bold"></asp:TextBox>
                    <b>
                    <asp:Label ID="l_y" runat="server" Text=" y " Visible="False"></asp:Label>
                    </b>
                    <asp:TextBox ID="txt_numero2" runat="server" CssClass="bold" Width="30px" 
                        Visible="False"></asp:TextBox>
                  </td>
                <td class="style23">, se prestarán</td>
                <td class="style25">
                    <asp:TextBox ID="txt_salarios" runat="server" CssClass="bold" Width="30px"></asp:TextBox>
                    <b>&nbsp;(SMMLV)</b></td>
                <td class="style22"></td>
              </tr>
              <tr>
                <td class="style10"></td>
                <td colspan="5" align="center" class="style26">
                    <asp:Button ID="btn_crear" runat="server" Text="Crear" Visible="False" 
                        onclick="btn_crear_Click" />
                    <asp:Button ID="btn_modificar" runat="server" Text="Modificar" 
                        Visible="False" onclick="btn_modificar_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_cerrar" runat="server" Text="Cerrar" 
                        onclick="btn_cerrar_Click" CausesValidation="False" Visible="False" ClientIDMode="Static" />
                  </td>
                <td class="style26">
<%--                    <img alt="" src="../../Images/bgrIcoPos.gif" onclick="alert(document.getElementById('Button1').value" />--%>
                  </td>
              </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_cerrar" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    
 </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
<style type="text/css">
        .style10 {
            width: 10px;
            height: 60px;
        }

        .style11 {
            width: 333px;
            height: 26px;
        }

        .style15 {
            width: 15px;
            height: 26px;
        }

        .style22 {
            height: 26px;
        }

        .style23 {
            height: 26px;
            width: 84px;
            font-weight: bold;
        }

        .style24 {
            width: 10px;
            height: 26px;
        }

        .style25 {
            height: 26px;
            width: 99px;
        }

        .style26 {
            height: 60px;
        }

        .style27 {
            height: 26px;
            width: 108px;
        }

        .style28 {
            width: 484px;
        }
         body, html {
            background-image: none !important;
        }
</style>
</asp:Content>

