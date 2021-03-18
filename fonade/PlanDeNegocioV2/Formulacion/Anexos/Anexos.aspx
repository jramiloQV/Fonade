<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Anexos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Anexos.Anexos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html, body {
            background-color: #fff !important;
            background-image: none !important;
        }
    </style>
    <script type="text/javascript">
        function ValidarTamano() {
            var uploadControl = document.getElementById('<%= Archivo.ClientID %>');
            if (uploadControl.files[0].size > 10485760) {
                alert('El tamaño del archivo debe ser menor a 10 MB');
                uploadControl.value = "";
                return false;
            }
        }
        function ValidarTamanoDocumentoAcreditacion() {
            var uploadControl = document.getElementById('<%= flDocumentoAcreditacion.ClientID %>');
            if (uploadControl.files[0].size > 10485760) {
                alert('El tamaño del archivo debe ser menor a 10 MB');
                uploadControl.value = "";
                return false;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="overflow: auto; width: 100%; height: 680px;">

            <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width='95%' border='0' align="center" cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td width='18' align='left'>
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Anexos', texto: 'Anexos'});">
                                                <img src="../../../Images/ImgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                        </div>
                                    </td>
                                    <td width='350'>
                                        <b>Anexos:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                    </td>
                                    <td align='right'>&nbsp;
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table width='95%' align="center" border='0' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlCargarCertificadoAutenticado" runat="server" BorderStyle="Solid">
                                            <asp:Label ID="lblInfoCarga" runat="server"
                                                Text="Cargar Certificado Digital Autenticado"></asp:Label>
                                            <br />
                                            <br />
                                            <asp:FileUpload ID="fuCargaCertificado" runat="server" accept=".pdf"/>
                                            <asp:Button ID="btnCargarCertificado" runat="server" Text="Subir" OnClick="btnCargarCertificado_Click" />
                                            <br />       
                                             <asp:Label ID="lblError" runat="server" Visible="false"
                                                Text="No ha seleccionado ningun archivo" ForeColor="Maroon"></asp:Label>
                                            <asp:Label ID="lblInfoAdvertencia" runat="server"
                                                Text="Por favor verifique el archivo antes de subirlo, 
                                                ya que esta acción solo se puede realizar una sola vez." ForeColor="Maroon"></asp:Label>
                                           
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='left' valign='top' width='98%'>
                                        <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                            <tr>
                                                <td align='left'>
                                                    <asp:Panel ID="pnlAdicionarAnexos" runat="server" Visible="false">
                                                        <asp:ImageButton ID="Image2" runat="server"
                                                            ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="Image2_Click" />
                                                        <asp:Button ID="btnAdicionarInversion" runat="server" Text="Adicionar Documento"
                                                            BorderStyle="None" OnClick="btnAdicionarInversion_Click" CssClass="boton_Link" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="errorProyectoCompleta" runat="server" Text="El proyecto se encuentra marcado como realizado de forma completa y no se pueden anexar más archivos anexos." Font-Bold="false" ForeColor="Red" Font-Size="Medium" Visible="false"></asp:Label>
                                        <asp:GridView ID="gw_Anexos" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="Grilla" OnRowCommand="gw_Anexos_RowCommand" DataKeyNames="Id_Documento"
                                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
                                            <RowStyle HorizontalAlign="Left" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                            runat="server" ImageUrl="/Images/icoBorrar.gif" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' OnClientClick="return Confirmacion('¿Está seguro que desea borrar el documento seleccionado?')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("URL") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("URL") %>'>
                                                            <asp:Image ID="img_Url" runat="server" ImageUrl='<%# "~/Images/"+ Eval("Icono") %>' />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre">
                                                    <ItemTemplate>
                                                        <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                            Text='<%# Eval("NombreDocumento") %>' Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' CssClass="boton_Link" />
                                                        <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("NombreDocumento") %>' Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Tab" HeaderText="Tab" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#174680" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button ID="btnDescargarAnexos" runat="server" Text="Descargar Anexos" OnClick="btnDescargarAnexos_Click" />
                                        <hr />
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
                                                    <td align="left" style="font-weight: bold;">Documentos de Evaluaci&oacute;n
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:GridView ID="gw_DocumentosEvaluacion" runat="server" Width="100%" AutoGenerateColumns="False"
                                                CssClass="Grilla" OnRowCommand="gw_DocumentosEvaluacion_RowCommand" DataKeyNames="Id_Documento"
                                                ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101) %>' Visible="true">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                                runat="server" ImageUrl="/Images/icoBorrar.gif" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBotonEvaluacion"))%>' OnClientClick="return Confirmacion('Esta seguro que desea borrar el documento seleccionado?')" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                            <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("URL") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Tipo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("URL") %>'>
                                                                <asp:Image ID="img_Url" runat="server" ImageUrl='<%# "~/Images/"+ Eval("icono") %>' />
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Nombre">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEditar" runat="server" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBotonEvaluacion"))%>' CommandName="Editar" CommandArgument='<%# Eval("Id_Documento") %>' Text='<%# Eval("NombreDocumento") %>' ForeColor="#696969" Style="text-decoration: none;" />
                                                            <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("NombreDocumento") %>' Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "HabilitaBotonEvaluacion"))%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="fecha" HeaderText="Fecha" HeaderStyle-Width="25%" ItemStyle-HorizontalAlign="Left"
                                                        ItemStyle-Font-Bold="true" ItemStyle-ForeColor="#174680" />
                                                </Columns>
                                            </asp:GridView>
                                            <asp:Button ID="btnDescargarEvaluacion" runat="server" Text="Descargar Anexos Evaluacion" OnClick="btnDescargarEvaluacion_Click" />
                                        </asp:Panel>
                                        <hr />
                                        <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                            <tr>
                                                <td align="left" style="font-weight: bold;">Documentos de contrato
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvContratos" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                            EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                                            CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="Id"
                                            OnRowCommand="gvFormulacion_RowCommand">
                                            <Columns>

                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("URL") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Nombre" DataField="Nombre" HtmlEncode="false" />
                                                <asp:TemplateField HeaderText="Ver archivo">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="VerArchivo" CommandArgument='<%#  Eval("Nombre") + ";" + Eval("Url")+ ";" + Eval("Id") %>'
                                                            CommandName="VerArchivo" CausesValidation="False" Text='Ver archivo' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                                        </asp:GridView>
                                        <asp:Button ID="btnDescargarContratos" runat="server" Text="Descargar Anexos Contratos" OnClick="btnDescargarContratos_Click" />
                                        <hr />
                                        <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                            <tr>
                                                <td align="left" style="font-weight: bold;">Documentos de Acreditaci&oacute;n
                                                </td>
                                            </tr>
                                        </table>
                                        <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                            <tr>
                                                <td align='left'>
                                                    <asp:Panel ID="pnlAdicionalDocAcreditacion" runat="server" Visible="false">
                                                        <asp:ImageButton ID="imgAddDocAcreditacion" runat="server"
                                                            ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="imgAddDocAcreditacion_Click" />
                                                        <asp:Button ID="btnAddDocAcreditacion" runat="server" Text="Adicionar Documento de acreditación"
                                                            BorderStyle="None" OnClick="btnAddDocAcreditacion_Click" CssClass="boton_Link" />
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="errorProyectoCompletaAcreditacion" runat="server" Text="El proyecto se encuentra marcado como realizado de forma completa y no se pueden anexar más archivos anexos." Font-Bold="false" ForeColor="Red" Font-Size="Medium" Visible="false"></asp:Label>
                                        <asp:GridView ID="gw_DocumentosAcreditacion" Visible="true" runat="server" Width="100%" AutoGenerateColumns="false"
                                            CssClass="Grilla" OnRowCommand="gvDocAcreditacion_RowCommand" DataKeyNames="Id_Documento"
                                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Documento") %>'
                                                            runat="server" ImageUrl="/Images/icoBorrar.gif" Visible='<%# ( (bool)DataBinder.GetPropertyValue(this, "HabilitaBoton") && !(bool)Eval("Bloqueado") )%>' OnClientClick="return Confirmacion('¿Está seguro que desea borrar el documento seleccionado?')" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Seleccionar" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="5%">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblFilePath" runat="server" Text='<%# Eval("Ruta") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Archivo" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# ConfigurationManager.AppSettings.Get("DirVirtual") + Eval("Ruta") %>'>
                                                            <asp:Image ID="img_Url" runat="server" ImageUrl="~/Images/IcoDocNormal.gif" />
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nombre" Visible="true">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnEditar" runat="server" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' CommandName="Editar" Enabled='<%# !(bool)Eval("Bloqueado") %>' CommandArgument='<%# Eval("Id_Documento") %>' Text='<%# Eval("Descripcion") %>' />
                                                        <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("Descripcion") %>' Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TipoArchivo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" />
                                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="20%" />
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Button ID="btnDescargarAcreditacion" runat="server" Text="Descargar Anexos Acreditacion" OnClick="btnDescargarAcreditacion_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
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
                        <asp:TextBox ID="txtLink" runat="server" MaxLength="256" Width="300px" Visible="false"></asp:TextBox>
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
                        <asp:Button ID="btnCrearAnexo" runat="server" Text="Crear" OnClick="btnCrearAnexo_Click" OnClientClick="return ValidarTamano();" />
                        <asp:Button ID="btnCerrarAnexo" runat="server" Text="Cerrar" OnClick="btnCerrarAnexo_Click" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:Panel ID="pnlDocumentosAcreditacion" Visible="false" runat="server">
                NUEVO DOCUMENTO
            <table width='1000px' border='0' cellspacing='0' cellpadding='3'>
                <tr valign="top">
                    <td colspan="2">
                        <asp:Label ID="lblMensajeErrorDocAcreditacion" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <b>Nombre:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreDocAcreditacion" runat="server" MaxLength="256" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top" id="tdSubir2" runat="server">
                    <td align="right">
                        <b>Subir Archivo:</b>
                    </td>
                    <td>
                        <asp:FileUpload ID="flDocumentoAcreditacion" runat="server" Width="500px" />
                    </td>
                </tr>
                <tr valign="top" id="tdLink2" runat="server">
                    <td align="right">
                        <!--<b>o Crear Link:</b>-->
                    </td>
                    <td>
                        <asp:TextBox ID="txtLinkDocAcreditacion" runat="server" MaxLength="256" Width="300px" Visible="false"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td align="right">
                        <b>Comentario:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservacionDocAcreditacion" runat="server" MaxLength="256" Width="400px" Height="100px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2" align="center">
                        <asp:HiddenField ID="hdIdDocAcreditacion" runat="server" />
                        <asp:Button ID="btnCrearDocumentoAcreditacion" runat="server" Text="Crear" OnClick="btnCrearDocumentoAcreditacion_Click" OnClientClick="return ValidarTamanoDocumentoAcreditacion();" />
                        <asp:Button ID="btnCancelarDocumentoAcreditacion" runat="server" Text="Cerrar" OnClick="btnCancelarDocumentoAcreditacion_Click" />
                    </td>
                </tr>
            </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
