<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CoordinadorEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CoordinadorEvaluador"  MasterPageFile="~/Emergente.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="560px">
              <tr>
                <td class="auto-style19" >
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>

            <table width="560px">
              <tr>
                <td class="auto-style7"></td>
                <td class="auto-style8">Nombres:</td>
                <td class="auto-style17">
                    <asp:TextBox ID="txt_nombre" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nombres está vacío" ForeColor="Red" ControlToValidate="txt_nombre" Display="Dynamic" ValidationGroup="validartotal">*</asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style7"></td>
                <td class="auto-style8">Apellidos:</td>
                <td class="auto-style17">
                    <asp:TextBox ID="txt_apellidos" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Apellidos está vacío" ForeColor="Red" ControlToValidate="txt_apellidos" Display="Dynamic" ValidationGroup="validartotal">*</asp:RequiredFieldValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style11"></td>
                <td class="auto-style12">Tipo de Identificación:</td>
                <td class="auto-style18">
                    <asp:DropDownList ID="ddl_tidentificacion" runat="server" Width="50%">
                    </asp:DropDownList>
                  </td>
                <td class="auto-style14">&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style7"></td>
                <td class="auto-style8">Número de Identificación:</td>
                <td class="auto-style17">
                    <asp:TextBox ID="txt_nidentificación" runat="server" Width="50%"></asp:TextBox>
                  </td>
                <td class="auto-style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Número de Identificación está vacío" ForeColor="Red" ControlToValidate="txt_nidentificación" Display="Dynamic" ValidationGroup="validartotal">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Número de Identificación debe ser numérico" ForeColor="Red" ControlToValidate="txt_nidentificación" Display="Dynamic" ValidationExpression="^[0-9]*" ValidationGroup="validartotal">*</asp:RegularExpressionValidator>
                  </td>
              </tr>
              <tr>
                <td class="auto-style6"></td>
                <td class="auto-style5">Correo Electrónico:</td>
                <td class="auto-style4">
                    <asp:TextBox ID="txt_email" runat="server" Width="100%"></asp:TextBox>
                  </td>
                <td class="auto-style15">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Correo Electrónico está vacío" ForeColor="Red" ControlToValidate="txt_email" Display="Dynamic" ValidationGroup="validartotal">*</asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="El formato del Correo Electrónico es incorrecto" ForeColor="Red" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txt_email" Display="Dynamic" ValidationGroup="validartotal">*</asp:RegularExpressionValidator>
                  </td>
              </tr>
            </table>

            <asp:Panel ID="PanelEdit" runat="server" Visible="false">

                <table width="560px">
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Cargo:</td>
                    <td class="auto-style17">
                        <asp:Label ID="l_cargo" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Teléfono:</td>
                    <td class="auto-style17">
                        <asp:Label ID="l_telefono" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style10"></td>
                  </tr>
                  <tr>
                    <td class="auto-style11"></td>
                    <td class="auto-style12">Fax:</td>
                    <td class="auto-style18">
                        <asp:Label ID="l_fax" runat="server"></asp:Label>
                      </td>
                    <td class="auto-style14"></td>
                  </tr>
                </table>

            </asp:Panel>

            <table width="560px" >
              <tr>
                <td align="center"  class="auto-style16">

                    <asp:Button ID="btn_crear" runat="server" Text="Crear" Visible="False" ValidationGroup="validartotal" OnClick="btn_crear_Click" />

                    <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" Visible="False" ValidationGroup="validartotal" OnClick="btn_actualizar_Click" />

                </td>
                </tr>
                <tr>
                <td align="center"  class="auto-style16">

                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" HeaderText="ERROR DE VALIDACIÓN" ValidationGroup="validartotal" />

                </td>
              </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>



        </asp:Content>



<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style4
        {
            width: 330px;
            height: 30px;
        }
        .auto-style5
        {
            width: 155px;
            font-weight: bold;
            height: 30px;
        }
        .auto-style6
        {
            width: 25px;
            height: 30px;
        }
        .auto-style7
        {
            width: 25px;
            height: 28px;
        }
        .auto-style8
        {
            width: 155px;
            font-weight: bold;
            height: 28px;
        }
        .auto-style10
        {
            height: 28px;
        }
        .auto-style11
        {
            width: 25px;
            height: 19px;
        }
        .auto-style12
        {
            width: 155px;
            font-weight: bold;
            height: 19px;
        }
        .auto-style14
        {
            height: 19px;
        }
        .auto-style15
        {
            height: 30px;
        }
        .auto-style16
        {
            height: 50px;
        }
        .auto-style17
        {
            width: 330px;
            height: 28px;
        }
        .auto-style18
        {
            width: 330px;
            height: 19px;
        }
        .auto-style19
        {
            width: 398px;
        }
    </style>
</asp:Content>




