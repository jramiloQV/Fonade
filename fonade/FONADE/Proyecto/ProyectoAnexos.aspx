<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoAnexos.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.ProyectoAnexos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html,body{
            background-color: #fff !important;
            background-image: none !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="overflow:auto; width: 100%; height: 680px;">
        <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <%--<td width="19">
                        &nbsp;
                    </td>--%>
                    <td>
                        <table width='95%' border='0' align="center" cellspacing='0' cellpadding='0'>
                            <tr>
                                <td width='18' align='left'>
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Anexos', texto: 'Anexos'});">
                                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                        </div>
                                    </div>
                                </td>
                                <td width='350'>
                                    <b>Anexos:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                </td>
                                <td align='right'>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width='95%' align="center" border='0' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td align='left' valign='top' width='98%'>
                                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                        <tr>
                                            <td align='left'>
                                                <asp:Panel ID="pnlAdicionarAnexos" runat="server" Visible="false">
                                                    <asp:ImageButton ID="Image2" runat="server" 
                                                        ImageUrl="~/Images/icoAdicionarUsuario.gif" onclick="Image2_Click" />
                                                    <asp:Button ID="btnAdicionarInversion" runat="server" Text="Adicionar Documento"
                                                        BorderStyle="None" OnClick="btnAdicionarInversion_Click" CssClass="boton_Link" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_Anexos" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="Grilla" OnRowCommand="gw_Anexos_RowCommand" DataKeyNames="Id_Documento"
                                        ShowHeaderWhenEmpty="True">
                                        <RowStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                        runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="false" OnClientClick="return Confirmacion('Esta seguro que desea borrar el documento seleccionado?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>                                                    
                                                    <asp:HyperLink ID="hlnk_url" runat="server" Target="_blank" NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") + Eval("URL") %>'>
                                                        <asp:Image ID="img_Url" runat="server" ImageUrl='<%# "~/Images/"+ Eval("icono") %>' />
                                                    </asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nombre">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                        Text='<%# Eval("nombre") %>' Visible="false" CssClass="boton_Link" />
                                                    <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("nombre") %>' Visible="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="tab" HeaderText="Tab" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left"
                                                ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#174680" />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <asp:Panel ID="pnlDocumentosDeEvaluacion" runat="server">
                                        <table id="tb_eval" runat="server" width='100%' border='0' cellspacing='1' cellpadding='4'
                                            visible="false">
                                            <tr>
                                                <td align='left'>
                                                    <asp:Panel ID="pnlAdicionarDocumentoEvaluacion" runat="server" Visible="false">
                                                        <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                        <asp:Button ID="Button1" runat="server" Text="Adicionar Documento de Evaluación"
                                                            CssClass='boton_Link' BorderStyle="None" OnClick="btnAdicionarInversion_Click" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" runat="server" visible="true" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" style="font-weight: bold;">
                                                    Documentos de Evaluaci&oacute;n
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gw_DocumentosEvaluacion" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="Grilla" OnRowCommand="gw_DocumentosEvaluacion_RowCommand" DataKeyNames="Id_Documento"
                                            ShowHeaderWhenEmpty="True" Visible="true">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                            runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="false" OnClientClick="return Confirmacion('Esta seguro que desea borrar el documento seleccionado?')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlnk_url" runat="server" Target="_blank" NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") + Eval("URL") %>'>
                                                        <asp:Image ID="img_Url" runat="server" ImageUrl='<%# "~/Images/"+ Eval("icono") %>' />
                                                    </asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                            Text='<%# Eval("nombre") %>' Visible="true" ForeColor="#696969" Style="text-decoration: none;" />
                                                        <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("nombre") %>' Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#174680" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                        <tr>
                                            <td align="left" style="font-weight: bold;">
                                                Documentos de contrato
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gvContratos" runat="server" AllowPaging="false"   AutoGenerateColumns="False" EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"  OnRowCommand="gvFormulacion_RowCommand">
                                        <Columns>                                            
                                            <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />                                              
                                            <asp:TemplateField HeaderText="Ver archivo">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="VerArchivo" CommandArgument='<%#  Eval("Nombre") + ";" + Eval("Url") %>'
                                                        CommandName="VerArchivo" CausesValidation="False" Text='Ver archivo' runat="server"                                                
                                                        />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                    </asp:GridView>
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                        <tr>
                                            <td align="left" style="font-weight: bold;">
                                                Documentos de Acreditaci&oacute;n
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_DocumentosAcreditacion" runat="server" Width="100%" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnRowCommand="gw_DocumentosAcreditacion_RowCommand" DataKeyNames="Id_Documento"
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
                                                    <asp:HyperLink ID="hlnk_url" runat="server" Target="_blank" NavigateUrl='<%# ConfigurationManager.AppSettings.Get("RutaWebSite") +"Documentos/"+ Eval("URL") %>'>
                                                        <asp:Image ID="img_Url" runat="server" ImageUrl="~/Images/IcoDocNormal.gif" />
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
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />
        </asp:Panel>
        <!--  Nuevo Panel -->
        <asp:Panel ID="pnlCrearDocumento" Visible="false" runat="server">
            NUEVO DOCUMENTO
            <table width='1000px' border='0' cellspacing='0' cellpadding='3'>
                <tr valign="top">
                    <td colspan="2">
                        <asp:Label ID="lblMensajeError" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <b>Nombre:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreDocumento" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top" id="tdSubir" runat="server">
                    <td align="right">
                        <b>Subir Archivo:</b>
                    </td>
                    <td>
                        <asp:FileUpload ID="Archivo" runat="server" Width="500px" />
                    </td>
                </tr>
                <tr valign="top" id="tdLink" runat="server">
                    <td align="right">
                        <!--<b>o Crear Link:</b>-->
                    </td>
                    <td>
                        <asp:TextBox ID="txtLink" runat="server" MaxLength="256" Width="300px" Visible ="false"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <b>Comentario:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtComentario" runat="server" MaxLength="256" Width="400px" Height="100px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2" align="center">
                        <asp:HiddenField ID="hddIdDocumento" runat="server" />
                        <asp:Button ID="btnCrearAnexo" runat="server" Text="Crear" OnClick="btnCrearAnexo_Click" />
                        <asp:Button ID="btnCerrarAnexo" runat="server" Text="Cerrar" OnClick="btnCerrarAnexo_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
