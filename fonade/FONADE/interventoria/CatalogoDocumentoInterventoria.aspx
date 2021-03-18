<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoDocumentoInterventoria.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.CatalogoDocumentoInterventoria" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }

        .celdaest {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        function openDoc(dir) {
            var dominio = document.domain;
            var url = dominio + '/' + dir;
            window.open(url, '_blank');
        }

        function alerta() {
            alert("hello world");
        }

        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval(& quot; page & quot; + id + & quot; = window.open(URL, &#39;& quot; + id + & quot;&#39;, &#39; toolbar = 0, scrollbars = 0, location = 0, statusbar = 0, menubar = 0, resizable = 0, width = 600, height = 400, left = 212, top = 184 &#39;);& quot;);
        }
    </script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table class="style10">
                <tr>
                    <td>
                        <h1>
                            <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700">Documentos</asp:Label>
                        </h1>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
                <asp:GridView ID="GrvDocumentos" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
                    OnSorting="GrvDocumentosSorting" OnPageIndexChanging="GrvDocumentosPageIndexChanging"
                    ShowHeader="true" ShowHeaderWhenEmpty="true" OnRowCommand="GrvDocumentos_RowCommand" OnRowDataBound="GrvDocumentos_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id") %>'
                                    runat="server" ImageUrl="/Images/icoBorrar.gif" Visible='<%# (bool)DataBinder.GetPropertyValue(this,"AllowDelete") ? true : false %>' OnClientClick="return confirm('Esta seguro que desea borrar el documento seleccionado?, en caso de eliminarlo este no se podrá recuperar.')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo" SortExpression="NomTipo">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkDoc" runat="server" ImageUrl="../../Images/IcoDocPDF.gif" Style="cursor: pointer;"
                                    NavigateUrl='<%# Eval("URL") %>' Target="_blank" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ver archivo">
                            <ItemTemplate>
                                <asp:LinkButton ID="VerArchivo" CommandArgument='<%#  Eval("NomDocumento") + ".PDF" + ";" + ConfigurationManager.AppSettings.Get("RutaIP") + Eval("URL") %>'
                                    CommandName="VerArchivo" CausesValidation="False" Text='Ver archivo' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" Text='<%# Eval("NomDocumento") %>' NavigateUrl='<%# Eval("Id","CatalogoDocumentoInterventoria.aspx?idFile={0}") %>' Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"AllowDelete") ? true : false %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Fecha" DataField="Fecha" />
                    </Columns>
                    <PagerStyle CssClass="Paginador" />
                </asp:GridView>
                <br />
                <asp:Button ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" Text="Regresar" />
            </asp:Panel>
            <asp:Panel ID="pnl_NuevoDoc" runat="server" Visible="false">
                <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
                    <tbody>
                        <tr>
                            <td align="center" valign="top" width="98%">
                                <table width="98%" border="0" cellspacing="0" cellpadding="0">
                                    <tbody>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <table width="98%" border="0" cellspacing="0" cellpadding="3">
                                    <tbody>
                                        <tr valign="top">
                                            <td colspan="2">&nbsp;
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="right">
                                                <b>Nombre:</b>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="NomDocumento" runat="server" MaxLength="80" size="50" />
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
                                                <b>Tipo de Documento:</b>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="dd_TipoInterventor" runat="server" />
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
                                            <asp:Button ID="btnActualizar" Text="Actualizar" runat="server" Visible="false" OnClick="btnActualizar_Click" />&nbsp;
                                            <asp:Button Text="Cerrar" runat="server" OnClientClick="javascript:history.back(-1)" />
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
