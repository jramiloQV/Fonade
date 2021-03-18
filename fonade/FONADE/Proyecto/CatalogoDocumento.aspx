<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente2.master" AutoEventWireup="true"
    CodeBehind="CatalogoDocumento.aspx.cs" Inherits="Fonade.FONADE.Proyecto.CatalogoDocumento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        window.onunload = refreshParentPage();

        function alerta() {
            return confirm('Esta seguro que desea borrar el documento seleccionado?');
        }
        function CerrarVentana() {
            ClosingVar == false
            document.getElementById('ActiveClose').value = "Se ha grabado la informacion de manera correcta!"
        }

        function refreshParentPage() {
            window.opener.location.href = window.opener.location.href;
            if (window.opener.progressWindow) {
                window.opener.progressWindow.close();
            }
            //window.close();
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <center>
    <asp:Panel ID="pnl_Grilla" runat="server" BackColor="White" Width="95%">
        <input id="ActiveClose" type="hidden"  value="Si decide abandonará la página, puede perder los cambios si no ha GRABADO ¡¡¡" runat="server"/>
  
        <table width="100%" border="1" cellpadding="0" cellspacing="0" bordercolor="#4E77AF">
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
                        RowStyle-HorizontalAlign="Left" OnRowCommand="gv_Documentos_RowCommand" DataKeyNames="url">
                        <Columns>
                            <asp:TemplateField HeaderStyle-Width="3%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnk_eliminar" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id_Documento") %>'
                                        CommandName="eliminar" ToolTip="Eliminar el documento del proyecto" Style="text-decoration: none;">
                                        <asp:Image ID="img_borrar" ImageUrl="../../Images/icoBorrar.gif" runat="server" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <HeaderStyle Width="3%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:HiddenField ID="hdf_icono" runat="server" Value='<%# Eval("icono") %>' />                                  
                                    <asp:HyperLink ID="hlnk_url" runat="server" Target="_blank" NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") + "/" + Eval("URL") %>'>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# "~/Images/"+ Eval("icono") %>' />
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle Width="5%" />
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
                                <HeaderStyle Width="30%" />
                            </asp:TemplateField>
                        </Columns>
                        <RowStyle HorizontalAlign="Left" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gw_DocumentosAcreditacion" runat="server" Width="100%" AutoGenerateColumns="false"
                                        CssClass="Grilla"  DataKeyNames="Id_Documento"
                                        ShowHeaderWhenEmpty="True">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                        Visible="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Archivo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                        <asp:HyperLink ID="hlnk_url" runat="server" Target="_blank" NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") + "//" + ConfigurationManager.AppSettings.Get("DirVirtual2") + Eval("URL") %>'>
                                                        <asp:Image ID="Image1" runat="server" ImageUrl="/Images/IcoDocNormal.gif" />
                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="tipo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pnl_datos" runat="server" Visible="false" BackColor="White" Width="95%">
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
                                    <td colspan="3" style="text-align: left">
                                        <asp:TextBox ID="NomDocumento" runat="server" MaxLength="256" Width="269" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Subir Archivo:</b>
                                    </td>
                                    <td colspan="3" style="text-align: left">
                                        <asp:FileUpload ID="Archivo" runat="server" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>o Crear Link:</b>
                                    </td>
                                    <td colspan="3" style="text-align: left">
                                        <asp:TextBox ID="Link" runat="server" MaxLength="256" Width="269" Visible="true" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Comentario:</b>
                                    </td>
                                    <td colspan="3" style="text-align: left">
                                        <asp:TextBox ID="Comentario" runat="server" Columns="50" Rows="5" TextMode="MultiLine" />
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
        </center>
</asp:Content>
