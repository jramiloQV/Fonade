<%@ Page Title="Catalogo Contacto" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="CatalogoEvaluacionContacto.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoEvaluacionContacto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style10
        {
            width: 351px;
        }
        .style11
        {
            color: red;
            width: 351px;
        }
        .style13
        {
            width: 77px;
        }
        .style14
        {
            width: 78px;
        }
        .style15
        {
        }
        .style16
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
   
   <table >
        <tr>
            <td >
                &nbsp;</td>
            <td class="style15">
                &nbsp;</td>
            <td class="style13">
                &nbsp;</td>
            <td class="style10">
                <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
                </ajaxToolkit:ToolkitScriptManager>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td align="center" colspan="3">
                <asp:Label ID="lbltitulo" runat="server" Font-Size="Large">APORTES Y CENTRALES DE RIESGO</asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td class="style15" align="center" colspan="3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="Small" 
                    ForeColor="Red" HeaderText="Llene  los siguientes Campos" 
                    ValidationGroup="crear" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td class="style15">
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
            <td >
                &nbsp;</td>
            <td class="style15">
                <asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;</td>
            <td >
                <asp:TextBox ID="TxtNombre" runat="server"  Width="335px" Enabled="False"/>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td >
                &nbsp;</td>
            <td class="style15">
                <asp:Label ID="lbl" runat="server" Text="Emprendedor"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;</td>
            <td >
                <asp:Label ID="lblemprendedor" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td class="style15" align="center" colspan="4">
            <fieldset><legend align="left"> Aportes</legend>
                <table class="style16">
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:Label ID="Label2" runat="server" Text="Dinero"/>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td align="left">
                <asp:TextBox ID="txtdinero" runat="server"  Width="156px"/>
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtdinero_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtdinero" 
                                ValidChars="1234567890">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:Label ID="Label3" runat="server" Text="Valor Especie"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td align="left">
                <asp:TextBox ID="txtespecie" runat="server" Width="158px" MaxLength="100"/>
                         
                            <ajaxToolkit:FilteredTextBoxExtender ID="txtespecie_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" FilterType="Numbers" ScriptPath="" 
                                TargetControlID="txtespecie" ValidChars="1234567890">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:Label ID="Label4" runat="server" Text="Detalle Especie"/>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                <asp:TextBox ID="txtDetalleespecie" runat="server" Width="239px" Height="108px" 
                    TextMode="MultiLine"/>
               
                        </td>
                        <td>

                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                &nbsp;&nbsp;
                <asp:Button ID="BtnCrear" runat="server" Text="Actualizar" ValidationGroup="crear" 
                    onclick="BtnCrear_Click" />    
                &nbsp;    
                <asp:Button ID="BtnCancelar" runat="server" Text="Cerrar" 
                    onclick="BtnCancelar_Click" />
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                        <td>
                            &nbsp;</td>
                    </tr>
                </table>
                </fieldset>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        
    </table>

</asp:Content>
