<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="CatalogoAporteEvaluacion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.ConceptoFinal.CatalogoAporteEvaluacion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style10
        {
            width: 493px;
        }
    </style>
     <script type="text/javascript">
        $(function () {
            // Set up the number formatting.
            $('.money').number(true, 2);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">   
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </ajaxToolkit:ToolkitScriptManager>
    <br/>
    <table align="center" width="60%">
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" colspan="4" align="left" valign="bottom" >
                <asp:Label ID="title" runat="server" Text="Label" Font-Size="Large"/> 
            </td>
            <td valign="bottom">
                &nbsp;&nbsp;<asp:Label ID="lblfecha" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" colspan="4" align="center">
                <asp:ValidationSummary ID="ValidationSummary1" HeaderText="Llene los Siguientes Campos"
                    runat="server" ForeColor="Red" ValidationGroup="crear" BackColor="White" />
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                &nbsp;
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="style10">
                &nbsp;
                </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" align="center">
                <asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="style10">
                <asp:TextBox ID="TxtNombre" runat="server" Width="335px"></asp:TextBox>
                &nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtNombre"
                    CssClass="requiredFieldValidator" ErrorMessage="Nombre" ValidationGroup="crear"
                    Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;</td>
            <td class="style12" valign="top" align="center">
                <asp:Label ID="Label10" runat="server" Text="Detalle"></asp:Label>
            </td>
            <td class="style12" valign="top">
                &nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="style10">
               
               <asp:TextBox class='Boxes' name='Detalle' rows='6' cols='50' runat="server" 
                                            id="txt_detalle" TextMode="MultiLine" Width="408px"/>
               </td>
            <td>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                            Display="Dynamic" ErrorMessage="Detalle" ForeColor="Red" 
                                            ValidationGroup="crear" ControlToValidate="txt_detalle">*</asp:RequiredFieldValidator>
                
                </td>
            <td>
                 &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" align="center">
                <asp:Label ID="Label4" runat="server" Text="Solicitado"></asp:Label>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="style10">
                 <asp:TextBox ID="txtsolicitado" name="number" runat="server" Width="158px" class="money" MaxLength="16" />
                 &nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtsolicitado"
                    CssClass="requiredFieldValidator" ErrorMessage="Nombre" ValidationGroup="crear"
                    Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" align="center">
                <asp:Label ID="Label5" runat="server" Text="Recomendado"></asp:Label>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="style10">
                <asp:TextBox ID="txtRecomendado" runat="server" Width="157px" MaxLength="15" >0</asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                    Enabled="True" TargetControlID="txtRecomendado" ValidChars="1234567890" 
                    FilterType="Numbers">
                </ajaxToolkit:FilteredTextBoxExtender>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" align="center">
                <asp:Label ID="Label8" runat="server" Text="Tipo de Aporte"></asp:Label>
            </td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="style10">
                 <asp:DropDownList ID="dpl_tipo" runat="server">
                                        </asp:DropDownList>
            </td>
            <td>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                            Display="Dynamic" ErrorMessage="Tipo Aporte" ForeColor="Red" 
                                            ValidationGroup="crear" ControlToValidate="dpl_tipo">*</asp:RequiredFieldValidator>
            </td>
            <td>
                 &nbsp;</td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
            <td class="style12">
                &nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="style10">
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style14">
            </td>
            <td class="style15">
            </td>
            <td class="style15">
                &nbsp;</td>
            <td class="style16">
            </td>
            <td class="style10">
                  <asp:Button runat="server" ID="btn_crearaporte" OnClick="btn_crearaporte_Click" 
                                            ValidationGroup="crear" />
                &nbsp;&nbsp;
                 <asp:Button  runat="server" ID="BtnCerrar" Text="Cerrar" 
                                            onclick="BtnCerrar_Click"  />
            </td>
            <td class="style17">
            </td>
            <td class="style17">
                &nbsp;</td>
        </tr>
        <tr>
            <td class="style14">
                &nbsp;
            </td>
            <td class="style15">
                &nbsp;
            </td>
            <td class="style15">
                &nbsp;</td>
            <td class="style16">
                &nbsp;
            </td>
            <td class="style10">
                &nbsp;
            </td>
            <td class="style17">
                &nbsp;
            </td>
            <td class="style17">
                &nbsp;</td>
        </tr>
    </table>
     
</asp:Content>
