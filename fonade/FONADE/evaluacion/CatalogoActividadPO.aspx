<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoActividadPO.aspx.cs" EnableViewState="true" ViewStateMode="Enabled"
    Inherits="Fonade.FONADE.evaluacion.CatalogoActividadPO" UICulture="es" Culture="es-CO" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONDO EMPRENDER - Actividad</title>
    <style type="text/css">
        html, body {
            background-color: #fff !important;
            background-image: none !important;
        }

        .auto-style1 {
            width: 80%;
            margin: 0px auto;
            text-align: center;
        }

        .panelmeses {
            margin: 0px auto;
            text-align: center;
        }

        .button-align {
            text-align: right;
            width: auto;
            height: auto;
        }

        .auto-style2 {
            height: 23px;
        }
    </style>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/common.js"></script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
        var resume = function () { window.opener.document.location = window.opener.document.location.href; }
    </script>
</head>
<body style="overflow-x: auto; overflow-y: auto; width: 100%;" onbeforeunload="resume()">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scripManaer" runat="server" />
        <div style="width: 100%">
            <table width="98%" border="0">
                <tr>
                    <td class="style50">
                        <h1 style="text-align: center;">
                            <asp:Label ID="lbl_titulo_PO" runat="server" Text="MODIFICAR ACTIVIDAD" Width="50%" Style="text-align: center;" />
                        </h1>
                    </td>
                </tr>
            </table>
            <%Page.DataBind(); %>
            <table>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="L_Item" runat="server" Text="Item:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TB_item" runat="server" ValidationGroup="accionar" Width="50px"
                            MaxLength="3" EnableViewState="true" ViewStateMode="Enabled" Text='<%# Convert.ToInt32(DataBinder.GetPropertyValue(this,"_Item")??"0") %>' ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TB_item"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="L_Actividad" runat="server" Text="Actividad:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TB_Actividad" runat="server" ValidationGroup="accionar" Width="250px"
                            MaxLength="150" Text='<%# Convert.ToString(DataBinder.GetPropertyValue(this, "_NomActividad")??string.Empty) %>' ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TB_Actividad"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="L_Metas" runat="server" Text="Metas:"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="TB_metas" runat="server" TextMode="MultiLine" required
                            Columns="60" Rows="7" Text='<%# Convert.ToString(DataBinder.GetPropertyValue(this,"_Metas")??string.Empty) %>' ></asp:TextBox>
                        <br />
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TB_metas"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lbl_inv_aprobar" Text="Aprobar:" runat="server" Visible="false" />
                    </td>
                    <td>
                        <asp:DropDownList ID="dd_inv_aprobar" runat="server" Visible="false">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <asp:Label ID="lbl_inv_obvservaciones" Text="Observaciones:" runat="server" Visible="false" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_inv_observaciones" runat="server" TextMode="MultiLine" Columns="25"
                            Rows="5" Visible="false" />
                    </td>
                </tr>
                <table width="98%" border="0">
                    <tr>
                        <td class="style50">
                            <h1>
                                <asp:Label runat="server" ID="lbl_Titulo" Style="text-align: center;"></asp:Label>
                            </h1>
                        </td>
                        <td align="right"></td>
                    </tr>
                </table>

                <tr>
                    <td class="style50">
                        <h1 style="text-align: center;">
                            <asp:Label ID="Label1" runat="server" Text="REQUERIMIENTOS DE RECURSOS POR MES" Width="100%" Style="text-align: center;"></asp:Label>
                        </h1>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="udpPrincipal" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel ID="P_Meses" runat="server" Width="100%">
                                    <asp:Table ID="T_Meses" runat="server" CssClass="panelmeses">
                                    </asp:Table>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div class="button-align">
                <br />
                <asp:Button ID="B_Acion" runat="server" ValidationGroup="accionar" OnClick="B_Acion_Click"
                    Text="Crear" />
                <asp:Button ID="B_Cancelar" runat="server" Text="Cancelar" OnClientClick="window.close();window.opener.location.reload();" />
            </div>
        </div>
    </form>
</body>
</html>
