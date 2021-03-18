<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoActa.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoActa"  EnableEventValidation= "false"  %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        table
        {
            width: 100%;
        }
        .celdaest
        {
            text-align: center;
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="style10">
            <tr>
                <td>
                    <h1>
                        <asp:Label ID="lbltitulo" CssClass="titulo" runat="server" Style="font-weight: 700">Nueva Acta</asp:Label>
                    </h1>
                </td>
            </tr>
        </table>
        <br />
        <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
            <asp:GridView ID="GrvDocumentos" runat="server" Width="100%" AutoGenerateColumns="False"
                CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
                OnSorting="GrvDocumentosSorting" OnPageIndexChanging="GrvDocumentosPageIndexChanging"
                ShowHeader="true" ShowHeaderWhenEmpty="true" OnRowCommand="GrvDocumentos_RowCommand" >
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="3%">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Acta") %>'
                                                    runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="true"  OnClientClick="return Confirmacion('Esta seguro que desea borrar el documento seleccionado?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo" SortExpression="NomTipo">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkDoc" runat="server" ImageUrl="../../Images/IcoDocNormal.gif" Style="cursor: pointer;" 
                                NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") + "/" +  Eval("URL") %>' Target="_blank" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText ="Nombre" DataField="NomActa" />
                    <asp:BoundField HeaderText="Fecha Acta" DataField="FechaActa" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField HeaderText="Fecha Cargue" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
                <PagerStyle CssClass="Paginador" />
            </asp:GridView>
            <br />
            <asp:Button ID="btnRegresar" runat="server" OnClientClick="javascript:self.close();" Text="Cerrar" />
        </asp:Panel>
        <asp:Panel ID="pnl_NuevoDoc" runat="server" Visible="false">
            <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
                <tbody>
                    <tr>
                        <td align="center" valign="top" width="98%">
                            <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table width="98%" border="0" cellspacing="0" cellpadding="3">
                                <tbody>
                                    <tr valign="top">
                                        <td colspan="2">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            <b>Numero:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNumeroActa" runat="server" MaxLength="10" Width="115" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            <b>Fecha Acta:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFecha" runat="server" type="date" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            <b>Nombre</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNombre" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            <b>Subir Archivo:</b>
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="Archivo" runat="server" size="50" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td align="right">
                                            <b>Comentario:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="Comentario" runat="server" TextMode="MultiLine" Columns="50" Rows="5" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnCrear" Text="Crear" runat="server" OnClick="btnCrear_Click" Visible="true" />&nbsp;
                                            <asp:Button Text="Cerrar" runat="server" OnClientClick="javascript:self.close();" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <br />
    </div>
    </form>
</body>
</html>
