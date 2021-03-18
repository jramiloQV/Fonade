<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="LegalizacionEmpresasActa.aspx.cs" Inherits="Fonade.FONADE.Administracion.LegalizacionEmpresasActa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function imprimir() {
            document.getElementById("oculto").style.display = "block";

            var divToPrint = document.getElementById('oculto');
            var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
            newWin.document.open();
            newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
            document.getElementById("oculto").style.display = "none";
            newWin.document.close();
            setTimeout(function () { newWin.close(); }, 1000);

            document.getElementById("oculto").style.display = "none";
        }

    </script>
    <%--<link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label ID="lbl_enunciado" runat="server" />
    </h1>
    <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
        <table class="auto-style1">
            <tr>
                <td style="text-align: left" class="auto-style3">
                    <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                    <asp:HiddenField ID="hdf_id" runat="server" Visible="false" />
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:LinqDataSource ID="ldsmemorandoempresas" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
                        AutoPage="true" OnSelecting="ldsmemorandoempresas_Selecting">
                    </asp:LinqDataSource>
                    <asp:GridView ID="gv_MemorandoEmpresas" runat="server" Width="400px" AutoGenerateColumns="false"
                        DataSourceID="ldsmemorandoempresas" CssClass="Grilla" AllowPaging="True" AllowSorting="true"
                        OnRowCommand="gv_MemorandoEmpresas_RowCommand" PageSize="30" OnPageIndexChanging="gv_MemorandoEmpresas_PageIndexChanging"
                        OnRowDataBound="gv_MemorandoEmpresas_RowDataBound">
                        <PagerStyle CssClass="Paginador" />
                        <RowStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="No Memorando" SortExpression="NumActa">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_id_memorando" runat="server" ForeColor="Black" CausesValidation="False"
                                        CommandArgument='<%# Eval("Id_Acta") %>' CommandName="mostrar" Text='<%#Eval("NumActa")%>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Nombre" DataField="NomActa" SortExpression="NomActa" />
                            <asp:TemplateField HeaderText="Archivo" SortExpression="NumActa">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkmostrar" runat="server" ForeColor="Black" NavigateUrl='<%#Eval("NumActa")%>'
                                        Text="Descargar" Style="text-decoration: underline;" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnl_detalles" runat="server" Visible="false">
        <table class="auto-style1">
            <tbody>
                <tr valign="top">
                    <td class="TitDestacado">
                        <b>No Memorando:</b>
                    </td>
                    <td class="TitDestacado">
                        <asp:TextBox ID="txt_noMemorando" runat="server" Enabled="false" MaxLength="10" Width="501px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td class="TitDestacado">
                        <b>Nombre:</b>
                    </td>
                    <td class="TitDestacado">
                        <asp:TextBox ID="txtNombreMemorando" runat="server" Enabled="false" MaxLength="80"
                            Width="501px" />
                    </td>
                </tr>
                <tr valign="top">
                    <td class="TitDestacado">
                        <b>Fecha:</b>
                    </td>
                    <td class="TitDestacado">
                        <asp:DropDownList ID="dd_fecha_dias_Memorando" runat="server">
                            <asp:ListItem Text="1" Value="1" />
                            <asp:ListItem Text="2" Value="2" />
                            <asp:ListItem Text="3" Value="3" />
                            <asp:ListItem Text="4" Value="4" />
                            <asp:ListItem Text="5" Value="5" />
                            <asp:ListItem Text="6" Value="6" />
                            <asp:ListItem Text="7" Value="7" />
                            <asp:ListItem Text="8" Value="8" />
                            <asp:ListItem Text="9" Value="9" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="11" Value="11" />
                            <asp:ListItem Text="12" Value="12" />
                            <asp:ListItem Text="13" Value="13" />
                            <asp:ListItem Text="14" Value="14" />
                            <asp:ListItem Text="15" Value="15" />
                            <asp:ListItem Text="16" Value="16" />
                            <asp:ListItem Text="17" Value="17" />
                            <asp:ListItem Text="18" Value="18" />
                            <asp:ListItem Text="19" Value="19" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="21" Value="21" />
                            <asp:ListItem Text="22" Value="22" />
                            <asp:ListItem Text="23" Value="23" />
                            <asp:ListItem Text="24" Value="24" />
                            <asp:ListItem Text="25" Value="25" />
                            <asp:ListItem Text="26" Value="26" />
                            <asp:ListItem Text="27" Value="27" />
                            <asp:ListItem Text="28" Value="28" />
                            <asp:ListItem Text="29" Value="29" />
                            <asp:ListItem Text="30" Value="30" />
                            <asp:ListItem Text="31" Value="31" />
                        </asp:DropDownList>
                        <asp:DropDownList ID="dd_fecha_mes_Memorando" runat="server">
                            <asp:ListItem Text="Ene" Value="1" />
                            <asp:ListItem Text="Feb" Value="2" />
                            <asp:ListItem Text="Mar" Value="3" />
                            <asp:ListItem Text="Abr" Value="4" />
                            <asp:ListItem Text="May" Value="5" />
                            <asp:ListItem Text="Jun" Value="6" />
                            <asp:ListItem Text="Jul" Value="7" />
                            <asp:ListItem Text="Ago" Value="8" />
                            <asp:ListItem Text="Sep" Value="9" />
                            <asp:ListItem Text="Oct" Value="10" />
                            <asp:ListItem Text="Nov" Value="11" />
                            <asp:ListItem Text="Dic" Value="12" />
                        </asp:DropDownList>
                        <asp:DropDownList ID="dd_fecha_year_Memorando" runat="server" />
                    </td>
                </tr>
                <tr valign="top">
                    <td class="TitDestacado">
                        <b>Observaciones:</b>
                    </td>
                    <td class="TitDestacado">
                        <asp:TextBox ID="txt_observaciones" runat="server" Enabled="false" TextMode="MultiLine"
                            Rows="8" Columns="60" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table>
            <tbody>
                <tr>
                    <td style="font-size: 10px; font-family:Tahoma">
                        <asp:GridView ID="gv_detallesMemorando" runat="server" AutoGenerateColumns="false"
                            CssClass="Grilla" Enabled="false" OnRowDataBound="gv_detallesMemorando_RowDataBound">
                            <PagerStyle CssClass="Paginador" />
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:BoundField HeaderText="Id" DataField="Id_Proyecto" />
                                <asp:BoundField HeaderText="Plan de Negocio" DataField="NomProyecto" ControlStyle-Width="" />
                                <asp:TemplateField HeaderText="Documentación">
                                    <ItemStyle Width="300px" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_garantia" Text="Garantía" runat="server" Checked='<%# Eval("Garantia") %>' />
                                        <asp:CheckBox ID="chk_pagare" Text="Pagaré" runat="server" Checked='<%# Eval("Pagare") %>' />
                                        <asp:CheckBox ID="chk_contrato" Text="Contrato" runat="server" Checked='<%# Eval("Contrato") %>' />
                                        <asp:CheckBox ID="chk_planOperativo" Text="Plan Operativo" runat="server" Checked='<%# Eval("PlanOperativo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Empresa" DataField="razonsocial" />
                                <asp:TemplateField HeaderText="Legalizado">
                                    <ItemTemplate>
                                        <asp:RadioButtonList ID="rb_EstaLegalizado" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Text="SI" Value="1" />
                                            <asp:ListItem Text="NO" Value="0" />
                                        </asp:RadioButtonList>
                                        <asp:Label ID="lbl_legal" Text='<%# Eval("Legalizado") %>' runat="server" Style="display: none;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr align="right">
                    <td>
                        <asp:Button ID="btn_imprimirMemorando" Text="Imprimir" runat="server" OnClientClick="imprimir()" />
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
    <%--Impresión.--%>
    <div id="oculto" style="display: none;">
        <table width="95%" border="0" cellspacing="0" cellpadding="2">
            <tbody>
                <tr>
                    <td width="50%" align="center" valign="baseline" bgcolor="#000000" class="Blanca">
                        <b>
                            <asp:Label ID="lbl_actaTitulo" Text="ACTA DE LEGALIZACION DE EMPRESAS" runat="server"
                                ForeColor="White" /></b>
                    </td>
                    <td width="30%" align="right" class="titulo">
                        &nbsp;
                    </td>
                    <td width="20%" align="right" class="titulo">
                        &nbsp;
                    </td>
                </tr>
            </tbody>
        </table>
        <table width="100%" border="0" cellpadding="4" cellspacing="1">
            <tbody>
                <tr>
                    <td width="30%">
                        <b>No Acta:</b>
                    </td>
                    <td>
                        <span id="sp_noActa" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Nombre:</b>
                    </td>
                    <td>
                        <span id="sp_Nombre" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Fecha:</b>
                    </td>
                    <td>
                        <span id="sp_FechaFormateada" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>Observaciones:</b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <span id="sp_observaciones" runat="server" />
                    </td>
                </tr>
            </tbody>
        </table>
        <hr />
        <p>
            Planes de Negocio Incluidos
            <asp:GridView ID="gv_imprimir_planesNegocio" runat="server" AutoGenerateColumns="false"
                CssClass="Grilla" Enabled="false" OnRowDataBound="gv_detallesMemorando_RowDataBound">
                <PagerStyle CssClass="Paginador" />
                <RowStyle HorizontalAlign="Left" />
                <HeaderStyle BackColor="Gray" ForeColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Id" DataField="Id_Proyecto" />
                    <asp:BoundField HeaderText="Plan de Negocio" DataField="NomProyecto" />
                    <asp:TemplateField HeaderText="Documentación">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_garantia" Text="Garantía" runat="server" Checked='<%# Eval("Garantia") %>' />
                            <asp:CheckBox ID="chk_pagare" Text="Pagaré" runat="server" Checked='<%# Eval("Pagare") %>' />
                            <asp:CheckBox ID="chk_contrato" Text="Contrato" runat="server" Checked='<%# Eval("Contrato") %>' />
                            <asp:CheckBox ID="chk_planOperativo" Text="Plan Operativo" runat="server" Checked='<%# Eval("PlanOperativo") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Empresa" DataField="razonsocial" />
                    <asp:TemplateField HeaderText="Legalizado">
                        <ItemTemplate>
                            <asp:RadioButtonList ID="rb_EstaLegalizado" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Text="SI" Value="1" />
                                <asp:ListItem Text="NO" Value="0" />
                            </asp:RadioButtonList>
                            <asp:Label ID="lbl_legal" Text='<%# Eval("Legalizado") %>' runat="server" Style="display: none;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </p>
        <br />
        <hr />
        <p>
            Aprobó:
        </p>
        <br />
        <br />
        ______________________________________ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        ________________________________
        <br />
        Subgerente Financiero&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Subgerente Técnico
        <br />
        <br />
        <br />
        <br />
        ______________________________________&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        ________________________________
        <br />
        Coordinador Grupo de Ejecución y Liquidación de&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        Gerente Unidad Crédito y Cartera&nbsp;
        <br />
        Convenios
        <br />
        <br />
        <br />
        <br />
        ______________________________________<br />
        Gerente de Convenio Fondo Emprender<br />
        <br />
        <br />
        <br />
    </div>
</asp:Content>
