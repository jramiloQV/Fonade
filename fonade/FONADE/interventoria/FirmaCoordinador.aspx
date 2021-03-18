<%@ Page Title="Aprobacion de Solicitudes de Pago" Language="C#" MasterPageFile="~/Emergente.Master"
    AutoEventWireup="true" CodeBehind="FirmaCoordinador.aspx.cs" Inherits="Fonade.FONADE.interventoria.FirmaCoordinador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="780" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td colspan="2" align="left">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td width="215" align="center" valign="top">
            </td>
            <td width="565" align="left" valign="top">
                <table width="95%" border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td width="165" align="center" valign="baseline" bgcolor="#3D5A87" style="color: White;">
                            SOLICITUDES DE PAGO
                        </td>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="1">
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblNombre" runat="server" Font-Bold="true" />
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblFecha" runat="server" Font-Bold="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr bgcolor="#3D5A87">
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <%--<tr bgcolor="#CC0000">--%>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
                    <tr>
                        <td align="center" valign="top" width="98%">
                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                            <table width="98%" border="0" cellspacing="0" cellpadding="3">
                                <tr align="left" valign="top">
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5" valign="top">
                                        <asp:Literal runat="server" ID="ltMensaje" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td bgcolor="#3D5A87">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="center">
                <table width="588" height="19" border="0" cellpadding="0" cellspacing="0">
                    <tr align="left">
                        <td width="205">
                            <a href='http://www.iconomultimedia.com' target="_blank">
                                <img src="../../Images/ImgLogoIcono.gif" width="205" height="35" border="0"></a>
                        </td>
                        <td width="262">
                            <a href='http://www.fonade.gov.co' target="_blank">
                                <img src="../../Images/ImgLogoFonade.gif" width="238" height="35" border="0"></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <br />
    <div id="div_clear" runat="server">
        <asp:GridView ID="gvsolicitudes" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
            OnRowCommand="gvsolicitudes_RowCommand" OnRowDataBound="gvsolicitudes_RowDataBound"
            AllowPaging="True" OnPageIndexChanging="gvsolicitudes_PageIndexChanging" EmptyDataText="No hay Solicitudes de pago registradas" OnSelectedIndexChanged="gvsolicitudes_SelectedIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Solicitud No.">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_btn_Id_PagoActividad" Text='<%# Eval("Id_PagoActividad") %>'
                            runat="server" CausesValidation="false" CommandName="mostrar_coordinadorPago"
                            CommandArgument='<%# Eval("Id_PagoActividad") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha">
                    <ItemTemplate>
                        <asp:Label ID="lbl_fecha" Text='<%# Eval("Fecha") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Empresa" DataField="RazonSocial" />
                <asp:TemplateField HeaderText="Agendó">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Intervemtor" runat="server" />
                        <asp:HiddenField ID="hdf_RazonSocial" runat="server" Value='<%# Eval("RazonSocial") %>' />
                        <asp:HiddenField ID="hdf_codProyecto" runat="server" Value='<%# Eval("CodProyecto") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Valor" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <asp:Label ID="lbl_valor" Text='<%# Eval("Valor") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Observación Interventor">
                    <ItemTemplate>
                        <asp:Label ID="lbl_observ_interv" Text='<%# Eval("ObservaInterventor") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Aprobado">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdf_codactafonade" runat="server" Value='<%# Eval("codactafonade") %>' />
                        <asp:HiddenField ID="hdf_CodBeneficiario" runat="server" Value='<%# Eval("numIdentificacion") %>' />
                        <asp:HiddenField ID="hdf_empresa" runat="server" Value='<%# Eval("Id_Empresa") %>' />
                        <asp:Label ID="lbl_displayText" Text="" runat="server" Visible="false" />
                        <asp:RadioButtonList ID="rb_lst_aprobado" runat="server" RepeatDirection="Vertical">
                            <asp:ListItem Text="Si" Value="opcion_SI" />
                            <asp:ListItem Text="No" Value="opcion_NO" />
                            <asp:ListItem Text="Pendiente" Value="opcion_Pendiente" />
                        </asp:RadioButtonList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Observaciones">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_observ" runat="server" TextMode="MultiLine" Columns="25" Rows="10" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No hay Solicitudes de pago registradas.</EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
