<%@ Page Title="FONDO EMPRENDER - Crear Documento" Language="C#" MasterPageFile="~/Emergente.Master"
    AutoEventWireup="true" CodeBehind="CatalogoAnexarInterventor.aspx.cs" Inherits="Fonade.FONADE.MiPerfil.CatalogoAnexarInterventor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm('Esta seguro que desea borrar el documento seleccionado?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="pnl_Grilla" runat="server" BackColor="White">
        <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
            <tr>
                <td>
                    <h3>
                        <asp:Label ID="lblGrilla" Text="DOCUMENTOS" runat="server" /></h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gv_Documentos" runat="server" AutoGenerateColumns="false" Width="98%"
                        CssClass="Grilla" OnRowDataBound="gv_Documentos_RowDataBound" ShowHeaderWhenEmpty="true"
                        RowStyle-HorizontalAlign="Left" OnRowCommand="gv_Documentos_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_eliminar" runat="server" CausesValidation="false" CommandArgument='<%# Eval("NomDocumento") %>'
                                        CommandName="eliminar" ToolTip="Eliminar el documento del proyecto" Style="text-decoration: none;"
                                        OnClientClick="return alerta();">
                                        <asp:Image ID="img_borrar" ImageUrl="../../Images/icoBorrar.gif" runat="server" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdf_icono" runat="server" Value='<%# Eval("icono") %>' />
                                    <asp:ImageButton ID="imgDoc" ImageUrl='<%# Eval("URL") %>' runat="server" CausesValidation="false"
                                        CommandArgument='<%# Eval("CodDocumentoFormato") %>' CommandName="Descargar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_NomDoc" Text='<%# Eval("NomDocumento") %>' runat="server"
                                        CausesValidation="false" CommandName="editar" CommandArgument='<%# Eval("id_Documento") %>'
                                        ForeColor="Black" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" HeaderStyle-Width="30%">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Fecha" Text='<%# Eval("fecha") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnl_datos" runat="server" Visible="false" BackColor="White">
        <table width="95%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
            <tbody>
                <tr>
                    <td align="left" valign="top">
                        <h3>
                            <asp:Label ID="lblTitulo" Text="NUEVO DOCUMENTO" runat="server" /></h3>
                    </td>
                </tr>
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
                                        <b>Nombre:</b>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="NomDocumento" runat="server" MaxLength="256" Width="269" />
                                        <asp:HiddenField ID="NomDocumentoAnt" runat="server" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Subir Archivo:</b>
                                    </td>
                                    <td colspan="3">
                                        <asp:FileUpload ID="Archivo" runat="server" />
                                        <asp:Button runat="server" ID="btnUpload" Text="Subir Archivo" OnClick="btnUpload_Click"
                                            ToolTip="Presione este botón una vez haya seleccionado el archivo." />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Comentario:</b><br />
                                        (Max. 250 Caracteres)
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="Comentario" runat="server" Columns="50" Rows="5" TextMode="MultiLine"
                                            MaxLength="250" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan="4" align="right">
                                        <asp:Button ID="btn_Accion" Text="Crear" runat="server" OnClick="btn_Accion_Click" />
                                        <asp:Button ID="Btn_cerrar" Text="Cerrar" runat="server" OnClick="Btn_cerrar_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
