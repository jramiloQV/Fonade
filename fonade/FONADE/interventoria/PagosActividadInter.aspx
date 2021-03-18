<%@ Page Title="Actividad" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="PagosActividadInter.aspx.cs" Inherits="Fonade.FONADE.interventoria.PagosActividadInter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <table width="95%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblTitulo" runat="server" />
            </td>
            <td style="text-align: center;">
                <asp:Label ID="lblNombre" runat="server" />
            </td>
            <td style="text-align: center;">
                <asp:Label ID="lbl_tiempo" runat="server" />
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnl_PagosActividad" runat="server" Width="95%" border="1" cellpadding="0"
        cellspacing="0" BorderColor="#4E77AF">
        <asp:GridView ID="gv_pagosactividad" runat="server" CssClass="Grilla" Width="100%"
            ShowHeaderWhenEmpty="true" AutoGenerateColumns="False" OnRowCommand="gv_pagosactividad_RowCommand"
            OnRowDataBound="gv_pagosactividad_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="ID" DataField="Id_PagoActividad" />
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_nombre" Text='<%# Eval("NomPagoActividad") %>' CommandArgument='<%# Eval("Id_PagoActividad")  + ";" + Eval("NomPagoActividad") %>'
                            runat="server" CausesValidation="false" CommandName="editar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Estado" Text='<%# Eval("Estado") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_Datos" runat="server" Width="95%" border="1" cellpadding="0" cellspacing="0"
        BorderColor="#4E77AF" Visible="false">
        <table width="95%" border="0" align="center" cellspacing="0" cellpadding="3">
            <tr valign="top">
                <td colspan="2">
                    <h1>
                        <asp:Label ID="lbl_newOrEdit" runat="server" /></h1>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    Tipo:
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddl_Tipo" runat="server">
                        <asp:ListItem Value="0" Text="Seleccione" />
                        <asp:ListItem Value="1" Text="Nueva" />
                        <asp:ListItem Value="2" Text="Rechazada" />
                    </asp:DropDownList>
                    <asp:Label ID="lbl_tipo_seleccionado" runat="server" Visible="false" />
                    <asp:HiddenField ID="hdf_tipo" runat="server" />
                </td>
                <td id="td_archivosAdjuntos" runat="server"  align="center">
                    ARCHIVOS ADJUNTOS<br />
                    <asp:ImageButton ID="imgBtn_addDocumentoPago" ImageUrl="../../Images/icoClip.gif"
                        runat="server" OnClick="imgBtn_addDocumentoPago_Click" />
                </td>
            </tr>
            <tr valign="top" bgcolor="#D1D8E2">
                <td>
                    <span>Número de la solicitud de orden que se va a reemplazar con la nueva:</span>
                </td>
                <td colspan="2">
                    <asp:DropDownList ID="ddl_NumSolicitudRechazada" runat="server" />
                    <asp:Label ID="lbl_numsolicitudRechazada" runat="server" Visible="false" />
                    <asp:HiddenField ID="hdf_numsolicitud" runat="server" />
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    Mes:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_meses" runat="server" />
                    <asp:Label ID="lbl_mes_seleccionado" runat="server" Visible="false" />
                </td>
            </tr>
            <tr valign="top" bgcolor="#D1D8E2">
                <td colspan="2">
                    <asp:Label ID="lbl_Actividad_Cargo" Text="Actividad:" runat="server" />
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_actividad_cargo" runat="server" />
                    <asp:Label ID="lbl_loaded_actividad_cargo" runat="server" Visible="false" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    Concepto:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_Concepto" runat="server" Width="448px" />
                </td>
            </tr>
            <tr valign="top" bgcolor="#D1D8E2">
                <td colspan="2">
                    Nombre del Beneficiario:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CodPagoBeneficiario" runat="server" Width="505px" />
                    <asp:Label ID="lblNombreBeneficiario" runat="server" Visible="false" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    Forma de Pago:
                </td>
                <td colspan="3">
                    <asp:DropDownList ID="ddl_CodPagoForma" runat="server" Width="186px" />
                    <asp:Label ID="lbl_FormaDePago" runat="server" Visible="false" />
                </td>
            </tr>
            <tr valign="top" bgcolor="#D1D8E2">
                <td colspan="2">
                    Observaciones:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="Observaciones" runat="server" TextMode="MultiLine" Height="28px"
                        Width="121px" />
                </td>
            </tr>
            <tr valign="top">
                <td colspan="2">
                    Cantidad de dinero solicitado al fondo emprender:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="CantidadDinero" runat="server" MaxLength="20" Height="18px" Width="119px" />
                    <asp:HiddenField ID="hdf_estado" runat="server" />
                </td>
            </tr>
            <tr id="tr_1" runat="server" visible="false" valign="top">
                <td colspan="2" bgcolor="#D1D8E2">
                    Aprobado:
                </td>
                <td colspan="3" bgcolor="#D1D8E2">
                    <asp:DropDownList ID="Aprobado" runat="server">
                        <asp:ListItem Value="Si" Text="Si" />
                        <asp:ListItem Value="No" Text="No" />
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="tr_2" runat="server" visible="false" valign="top">
                <td colspan="2">
                    Observaciones:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="ObservacionesInter" runat="server" TextMode="MultiLine" Height="28px"
                        Width="121px" />
                </td>
            </tr>
            <tr valign="top">
                <asp:HiddenField ID="hdCodigoPago" runat="server" Visible="false" />                        
                <td colspan="4" align="right">
                    <asp:Button ID="btnEnviar" Text="Enviar" runat="server" OnClick="btnEnviar_Click"
                        Visible="false" />
                    <asp:Button ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" Text="Regresar"
                        Visible="false" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
