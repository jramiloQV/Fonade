<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AsignarEvaluador.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.AsignarEvaluador" MasterPageFile="~/Master.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <ajax:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajax:ToolkitScriptManager>
    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Text="ASIGNAR EVALUADOR A PROYECTO" Style="font-weight: 700"></asp:Label></h1>                        
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td class="auto-style1">
                    </td>
                    <td class="auto-style3" valign="baseline">
                        <strong>Seleccione un Sector:</strong>
                    </td>
                    <td class="auto-style2" valign="baseline">
                        <asp:DropDownList ID="ddl_evals" runat="server" Width="100%" OnSelectedIndexChanged="ddl_evals_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="auto-style4">
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                    </td>
                    <td class="auto-style3" valign="baseline">
                        <strong>Seleccione un Proyecto:</strong>
                    </td>
                    <td class="auto-style2" valign="baseline">
                        <asp:DropDownList ID="ddl_proyectos" runat="server" Width="100%" OnSelectedIndexChanged="ddl_proyectos_SelectedIndexChanged"  AutoPostBack="True" />
                    </td>
                    <td class="auto-style4">
                    </td>
                </tr>
            </table>
            <table width="100%">
                </tr>
                <tr>
                    <td class="auto-style5">
                        &nbsp;
                    </td>
                    <td class="auto-style6">
                        <asp:Panel ID="Panelproyectos" runat="server" Width="100%" Height="100px" ScrollBars="Vertical">
                            <asp:DataList ID="DltEvaluacion" runat="server" Width="100%" BackColor="White" BorderStyle="None"
                                BorderWidth="0px" CellPadding="4" ForeColor="Black" >
                                <ItemTemplate>
                                    <table id="Table2" width="100%" runat="server">
                                        <tr>
                                            <td style="border-width: thin; border-top-style: none; border-color: #94ADCD; border-bottom-style: solid">
                                                <asp:Label runat="server" ID="lproyecto" Text='<%# "-" + Eval("NomProyecto") %>'
                                                    Font-Size="Small" ForeColor="#174696" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="itemPlaceholderContainer0" width="100%" runat="server">
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="ldescripcion" Text='<%# Eval("Sumario") %>' />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:Panel>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td class="auto-style5">
                        &nbsp;
                    </td>
                    <td class="auto-style6" align="center">
                        <asp:GridView ID="gvcoorAsigando" CssClass="Grilla" runat="server" 
                            AllowPaging="false" AutoGenerateColumns="false" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderText="Evaluador" DataField="nombre" />
                                <asp:TemplateField HeaderText="Correo Electrónico">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hcorreo" runat="server" NavigateUrl='<%#"mailto:"+ Eval("email") %>'
                                            Text='<%# Eval("Email") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="auto-style10">
                    </td>
                    <td class="auto-style11" align="center">
                        <asp:Button ID="btn_asignar" runat="server" Text="Asignación de Evaluador" OnClick="btn_asignar_Click"  />
                    </td>
                    <td class="auto-style12">
                    </td>
                </tr>
            </table>
            <asp:Panel ID="PanellistadoCoordinadores" runat="server" Visible="false">
                <table width="100%">
                    <tr>
                        <td class="auto-style17">
                        </td>
                        <td class="auto-style18" style="text-align: center; background-color: #00468F;">
                            <strong>Evaluadores</strong>
                        </td>
                        <td class="auto-style19">
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="3">
                            <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Auto" Width="100%">
                                <asp:RadioButtonList ID="rbl_coordinEval" runat="server" Height="500px" Width="100%">
                                </asp:RadioButtonList>
                                <a id="linkFInal" runat="server"></a>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style14">
                        </td>
                        <td class="auto-style16" align="center">
                            <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar" OnClick="btn_actualizar_Click"  />
                        </td>
                        <td class="auto-style12">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1
        {
            width: 50px;
            height: 34px;
        }
        .auto-style2
        {
            width: 400px;
            height: 34px;
        }
        .auto-style3
        {
            width: 180px;
            height: 34px;
        }
        .auto-style4
        {
            height: 34px;
        }
        .auto-style5
        {
            width: 50px;
        }
        .auto-style6
        {
            width: 585px;
        }
        .auto-style10
        {
            width: 50px;
            height: 45px;
        }
        .auto-style11
        {
            width: 585px;
            height: 45px;
        }
        .auto-style12
        {
            height: 45px;
        }
        .auto-style13
        {
            width: 25%;
        }
        .auto-style14
        {
            width: 25%;
            height: 45px;
        }
        .auto-style15
        {
            width: 472px;
        }
        .auto-style16
        {
            width: 40%;
            height: 45px;
        }
        .auto-style17
        {
            width: 25%;
            height: 26px;
        }
        .auto-style18
        {
            width: 40%;
            height: 26px;
            color: #FFFFFF;
        }
        .auto-style19
        {
            height: 26px;
        }
    </style>
</asp:Content>
