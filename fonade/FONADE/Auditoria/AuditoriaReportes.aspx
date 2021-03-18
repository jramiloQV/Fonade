<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuditoriaReportes.aspx.cs"
    Inherits="Fonade.FONADE.Auditoria.AuditoriaReportes" MasterPageFile="~/Master.Master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="panelReportes" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td>
                        <h1>
                            <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700" /></h1>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr >
                    <td align="left" class="style17">
                        * seleccione la tabla para generar el reporte de Auditoría:  
                        <asp:DropDownList ID="ddlTablas" runat="server" OnSelectedIndexChanged="ddlTablas_SelectedIndexChanged" AutoPostBack="true" />
                    </td>
                    <td class="style18">
                    </td>
                </tr>
            </table>
            <table width="100%">
            </table>
            <table width="100%">
                <tr>
                    <td></td>
                </tr>
                <caption>
                    <br />
                    <tr>
                        <td style="text-align: left"></td>
                        <td><span id="lblNotas" runat="server" visible="false">
                            <ul>
                                <li>- Ingrese la(s) condición(es) de busqueda(s) por el/los campo(s) a consultar.</li>
                                <li>- En caso de ser mas de una condición, seleccione la union &quot;Y&quot; u &quot;O&quot;.</li>
                                <li>- En la ultmia condicion ingresada, NO debe seleccionar ninguna union.</li>
                                <li>- En caso de campos de texto puede utilizar el comodin % al principio y al final de la palabra a buscar, o una de las dos.</li>
                            </ul>                            
                            </span></td>
                    </tr>
                    <tr>
                        <td class="style16">&nbsp; </td>
                        <td class="style21">
                            <asp:GridView ID="grvCriterios" runat="server" AutoGenerateColumns="false" CssClass="Grilla" OnRowDataBound="grvCriterios_RowDataBound"
                                horizontalalign="Center">
                                <Columns>
                                    <asp:TemplateField HeaderText="Campo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCampo" runat="server" Text='<%# Eval("Campo") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="true" HeaderText="Tipo dato">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoDato" runat="server" Text='<%# Eval("TipoDato") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Condición" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Panel ID="pnlTexto" runat="server" Visible="false">
                                                <asp:CheckBox ID="chkPorcenInico" Text="%" runat="server" Visible="false" />
                                                <asp:TextBox ID="txtCondicion" runat="server" Visible="false" />
                                                <asp:CheckBox ID="chkPorcenFin" Text="%" runat="server" Visible="false" />
                                            </asp:Panel>
                                            <asp:Panel ID="pnlFechas" runat="server" Visible="false">
                                                Desde: <asp:TextBox ID="dteFechaInicio" runat="server" type="date" /><br />
                                                Hasta: <asp:TextBox ID="dteFechaFin" runat="server" type="date" /><br />
                                            </asp:Panel>
                                            <asp:CheckBox ID="chkCondicion" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Union">
                                        <ItemTemplate>
                                            <asp:RadioButtonList ID="rbnAndOr" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table">
                                                <asp:ListItem Text="Y" Value="And" />
                                                <asp:ListItem Text="O" Value="Or" />
                                            </asp:RadioButtonList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>&nbsp; </td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td class="style23" style="text-align: right">
                            <asp:Button ID="btn_generareporte" runat="server" OnClick="btn_generareporte_Click" Text="Generar Reporte" Visible="false"/>
                            <asp:HyperLink ID="lnkDescargar" runat="server" Text="Descargar reporte" Visible="false"  /><br />
                            <asp:LinkButton ID="lnkNuevaConsulta" runat="server" PostBackUrl="AuditoriaReportes.aspx" Visible="false" Text="Nueva Consulta" />
                        </td>
                        <td class="style24"></td>
                    </tr>
                    <tr>
                        <td class="style22"></td>
                        <td align="center" class="style23">&nbsp; </td>
                        <td class="style24"></td>
                    </tr>
                </caption>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .style11
        {
            width: 90px;
        }
        .style15
        {
            width: 150px;
        }
        .style16
        {
            width: 30px;
        }
        .style17
        {
            width: 580px;
            height: 30px;
        }
        .style18
        {
            height: 30px;
        }
        .style21
        {
            width: 492px;
        }
        .style22
        {
            width: 30px;
            height: 53px;
        }
        .style23
        {
            width: 492px;
            height: 53px;
        }
        .style24
        {
            height: 53px;
        }
    </style>
</asp:Content>
