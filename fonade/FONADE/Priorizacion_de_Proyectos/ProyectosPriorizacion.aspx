<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ProyectosPriorizacion.aspx.cs" Inherits="Fonade.FONADE.Priorizacion_deProyectos.ProyectosPriorizacion" Culture="es-CO" UICulture="es-CO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style2 {
            height: 32px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server" EnablePageMethods="true"
        EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </asp:ToolkitScriptManager>

    <table width="98%" border="0">
        <tr>
            <td class="style50">
                <h1>
                    <asp:Label runat="server" ID="lblTitulo" Text="Priorización de proyectos" Style="font-weight: 700"></asp:Label>
                </h1>
            </td>
            <td align="right"></td>
        </tr>
        <tr>
            <td class="style50">
                <asp:Label runat="server" ID="Label1" Text="Operador: "></asp:Label>
                <asp:DropDownList ID="ddlOperador" runat="server" DataValueField="idOperador"
                    DataTextField="NombreOperador" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%" border="0">
                <tr>
                    <td class="style50">
                        <asp:HyperLink runat="server" ID="lnkRutaActa" Text="" Visible="false" />
                    </td>
                </tr>
            </table>

            <asp:GridView ID="gvProyectosAPriorizar" runat="server" Width="98%" 
                BorderWidth="0" CellSpacing="1" CellPadding="4" AllowPaging="false" 
                AutoGenerateColumns="False" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" 
                EmptyDataText="No hay proyectos por priorizar.">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSeleccionarProyecto" runat="server" AutoPostBack="true" OnCheckedChanged="chkSeleccionarProyecto_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Código del proyecto">
                        <ItemTemplate>
                            <asp:Label ID="lblCodigoProyecto" name="codigo" runat="server" Text='<%# Eval("Codigo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Plan de Negocio">
                        <ItemTemplate>
                            <asp:Label ID="lblNombreProyecto" runat="server" Text='<%# Eval("Nombre") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Convocatoria">
                        <ItemTemplate>
                            <asp:HiddenField ID="hiddenCodigoConvocatoria" runat="server" Value='<%# Eval("codigoconvocatoria") %>' />
                            <asp:Label ID="NombreConvocatoria" runat="server" Text='<%# Eval("NombreConvocatoria") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recursos" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="ValorRecomendado" runat="server" Text='<%# Eval("ValorRecomendado") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Operador" ItemStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="Operador" runat="server" Text='<%# Eval("Operador") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
            <table width="98%" border="0">
                <tr>
                    <td class="style50" align="right">
                        <h3>
                            <asp:Label runat="server" ID="lblTituloTotalRecursos" Text="Total asignación de recursos :" Style="font-weight: 700"></asp:Label>
                            <asp:Label runat="server" ID="lblTotalRecursos" Text="$0" Style="font-weight: 700"></asp:Label>
                        </h3>
                    </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td colspan="2" style="font-weight: 700;" class="style24">Detalles del Acta
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Número de Acta:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="txtNumero" runat="server" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;
                    </td>
                    <td class="auto-style2" valign="baseline">Nombre del Acta:
                    </td>
                    <td class="auto-style2" valign="baseline">
                        <asp:TextBox ID="txtNombre" ClientIDMode="Static" runat="server" Width="275px"></asp:TextBox>
                    </td>
                    <td class="auto-style2"></td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Fecha del Acta:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:TextBox ID="txtFecha" runat="server" BackColor="White" Text="" Width="100px" Enabled="False"></asp:TextBox>
                        &nbsp;
                        <asp:Image ID="btnFecha" runat="server" AlternateText="Calendario de acta" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="CalendarioFechaActa" runat="server" Format="dd/MM/yyyy" PopupButtonID="btnFecha"
                            TargetControlID="txtFecha" />
                    </td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td class="style12" valign="baseline">Convocatoria:
                    </td>
                    <td class="style13" valign="baseline">
                        <asp:DropDownList ID="cmbConvocatoria" runat="server" Width="275px" 
                            DataValueField="Codigo" DataTextField="Nombre">
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style16"></td>
                    <td class="style17" valign="bottom">Observaciones:
                    </td>
                    <td class="style18"></td>
                    <td class="style19"></td>
                </tr>
                <tr>
                    <td class="style11">&nbsp;
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtObservaciones" runat="server" Height="140px" TextMode="MultiLine"
                            Width="440px"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td class="style16"></td>
                    <td colspan="3">
                        <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                            <ProgressTemplate>
                                <div>
                                    <div>
                                    </div>
                                    <div>
                                        <label class="control-label"><b>Procesando información, espere un momento.</b> </label>
                                        <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <asp:Label ID="lblError" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="False" Font-Bold="True" Font-Size="Medium" />
                    </td>
                </tr>
                <tr>
                    <td class="style14">&nbsp;</td>
                    <td colspan="2" align="center" class="style15">
                        <asp:Button ID="btnGuardar" runat="server" Text="Asignar Recursos" OnClick="btn_asignarRecursos_Click" Visible="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
            </table>
            <asp:ObjectDataSource ID="dsProyectosPorPriorizar" runat="server" EnablePaging="false" SelectMethod="getProyectosPorPriorizar" TypeName="Fonade.FONADE.Priorizacion_deProyectos.ProyectosPriorizacion"></asp:ObjectDataSource>
            <asp:ObjectDataSource ID="dsConvocatorias" runat="server" EnablePaging="false" SelectMethod="getConvocatorias" TypeName="Fonade.FONADE.Priorizacion_deProyectos.ProyectosPriorizacion"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
