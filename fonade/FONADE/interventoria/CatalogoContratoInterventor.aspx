<%@ Page Title="FONDO EMPRENDER" Language="C#" AutoEventWireup="true" CodeBehind="CatalogoContratoInterventor.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoContratoInterventor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head2" runat="server">
    <title>FONDO EMPRENDER - ADMINISTRAR INTERVENTORES</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function Cerrar() {
            this.window.close();
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td style="width: 50%; background-color:#09305b; border-color:#09305b; text-align:center">
                        <asp:Label ID="lblContratoTitulo" runat="server" Text="Contratos" BackColor="#09305b" ForeColor="White" Font-Bold="true" />
                    </td>
                    <td></td>
                </tr>
            </table>
            <div style="background-color:#09305b; height:2px"></div><br />
            <table id="tb_ver" runat="server">
                <tbody>
                    <tr>
                        <td>
                            <b>Coordinador de Interventoria:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </b>
                            <b><asp:Label ID="lbl_nmb_coord_interv" Text="" runat="server" /></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table>
                                <tbody>
                                    <tr>
                                        <td class="auto-style1">
                                            <b>Número de Contrato:</b>
                                        </td>
                                        <td class="auto-style1">
                                            <asp:TextBox ID="txt_NumContrato" runat="server" MaxLength="7" Width="68px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Fecha de Inicio:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="FechaInicioDD" runat="server" AutoPostBack="true" />
                                            /
                                        <asp:DropDownList ID="FechaInicioMM" runat="server" AutoPostBack="true">
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
                                            /
                                        <asp:DropDownList ID="FechaInicioYYYY" runat="server" AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>
                                                <asp:Label ID="lblMeses" runat="server" Text="Meses:" /></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Meses" runat="server" MaxLength="2" Width="55px" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="panel_expriracion">
                                        <td>
                                            <b>
                                                <asp:Label ID="lbl_fecha_expiracion" Text="Fecha Expiracion:" runat="server" Visible="false" /></b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="dd_days_expiracion" runat="server" AutoPostBack="true" Visible="false" />
                                            <asp:Label ID="lbl_exp_1" runat="server" Text="/" Visible="false" />
                                            <asp:DropDownList ID="dd_months_expiracion" runat="server" AutoPostBack="true" Visible="false">
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
                                            <asp:Label ID="lbl_exp_2" runat="server" Text="/" Visible="false" />
                                            <asp:DropDownList ID="dd_years_expiracion" runat="server" AutoPostBack="true" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr runat="server" id="panel_motivo">
                                        <td>
                                            <b>
                                                <asp:Label ID="lbl_motivo_modificacion" Text="Motivo Modificación:" runat="server"
                                                    Visible="false" /></b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_Motivo" runat="server" Visible="false" TextMode="MultiLine"
                                                Columns="40" Rows="5" OnTextChanged="txt_Motivo_TextChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        <asp:HiddenField ID="hdf_FechaInicio" runat="server" Visible="false" />
                                            <asp:HiddenField ID="hdf_FechaExpiracion" runat="server" Visible="false" />
                                            <asp:HiddenField ID="hdf_numContrato" runat="server" Visible="false" />
                                        </td>
                                        <td>
                                            <asp:Button ID="btn_Crear" Text="Crear" runat="server" OnClick="btn_Crear_Click" />
                                            <asp:Button ID="btn_Cerrar" Text="Cerrar" runat="server" OnClientClick="return Cerrar();" />
                                            <asp:Button ID="btn_Lista" Text="Lista" runat="server" OnClick="btn_Lista_Click" />
                                            <%--<input type="button" value="Lista" class="Boton" onclick="document.location.href='CatalogoContratoInterventor.asp?Accion=Lista&amp;codContacto=33803'">--%>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table id="tb_crear" runat="server" visible="false">
                <tbody>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <b>Coordinador de Interventoria:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </b>
                            <b><asp:Label ID="lbl_nmb_coord_interv_mod" Text="" runat="server" /></b>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td align="left">
                            <asp:ImageButton ID="img_btn_Add" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                                runat="server" OnClick="img_btn_Add_Click" />
                            &nbsp;<asp:LinkButton ID="lnk_btn_Add" Text="Nuevo Contrato" runat="server" OnClick="lnk_btn_Add_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:GridView ID="gv_intv" runat="server" AutoGenerateColumns="false" CssClass="Grilla"
                                EmptyDataText="No hay datos." OnRowCommand="gv_intv_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Número">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnk_btn_Numero" Text='<%# Eval("numContrato") %>' runat="server"
                                                CausesValidation="false" CommandName="VerNumero" CommandArgument='<%# Eval("Id_InterventorContrato") + ";" + Eval("CodContacto") + ";" + Eval("FechaInicio") %>'
                                                ForeColor="Black" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Fecha Inicio" DataField="P_FechaInicio" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="Fecha Expiración" DataField="P_FechaExpiracion" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
