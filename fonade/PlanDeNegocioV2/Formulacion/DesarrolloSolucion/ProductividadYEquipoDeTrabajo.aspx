<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProductividadYEquipoDeTrabajo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.ProductividadYEquipoDeTrabajo" EnableEventValidation="false" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="../../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc2" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc3" TagName="Help" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script type="text/javascript" src="../../../Scripts/jquery.number.min.js"></script>
    <script>
        $(function () {
            $('.money').number(true, 2);
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="parentContainer">
            <div class="childContainer">
                <asp:UpdatePanel ID="UpdatePanel" runat="server" Width="100%">
                    <ContentTemplate>
                        <% Page.DataBind(); %>
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <br />
                        <div style="position: relative; left: 705px; width: 160px;">
                            <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="ProductividadYEquipo"/>
                            <br />
                        </div>
                        <div style="text-align: center">
                            <h1>IV. ¿Cómo desarrollo mi solución?</h1>
                            <br />
                            <br />
                        </div>
                        <uc3:Help runat="server" ID="HelpPregunta16" Mensaje="CapacidadProductiva" Titulo="16. ¿Cuál es la capacidad productiva de la empresa? (cantidad de bien o servicio por unidad de tiempo)" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta16" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),Fonade.Negocio.Mensajes.Mensajes.GetMensaje(125)) %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_Pregunta16" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="cke_Pregunta16" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <br />
                        <uc3:Help runat="server" ID="HelpPregunta17" Mensaje="EquipoTrabajo" Titulo="17. Equipo de trabajo" />
                        <br />
                        <uc3:Help runat="server" ID="HelpPregunta171" Mensaje="PerfilEmprendedor" Titulo="17.1. ¿Cuál es el perfil del emprendedor, el rol que tendría dentro de la empresa y su dedicación?" />
                        <br />
                        <asp:GridView ID="gw_pregunta171" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gw_pregunta171_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Nombre Emprendedor">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                        <asp:LinkButton ID="btnEditarEquipo" runat="server" OnClick="btnEditarEquipo_Click" Width="220px"
                                            CommandArgument='<%# string.Format("{0}|{1}|{2}",Eval("IdEmprendedorPerfil"),Eval("NombreEmprendedor"),Eval("IdContacto")) %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("NombreEmprendedor") %></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Perfil">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblPerfil" runat="server" Text='<%# Bind("Perfil") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rol">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblRol" runat="server" Text='<%# Bind("Rol") %>' Width="220"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <uc3:Help runat="server" ID="HelpPregunta172" Mensaje="CargosEmpresa" Titulo="17.2. ¿Qué cargos requiere la empresa para su operación (primer año)?" />
                        <br />
                        <div runat="server" visible='<%# AllowUpdate %>'>
                            <asp:ImageButton ID="imgagregar172" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="btnAddCargo172_Click" />
                            &nbsp;
                        <asp:LinkButton ID="btnAddCargo172" runat="server" Text="Adicionar Cargo" OnClick="btnAddCargo172_Click"></asp:LinkButton>

                        </div>
                        <br />
                        <asp:GridView ID="gwPregunta172" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gwPregunta172_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>' ShowFooter="True">
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField>
                                    <FooterTemplate>
                                        <div style="text-align: left;">
                                            <label>Total Solicitado:</label>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar172" runat="server" ImageUrl="~/Images/icoBorrar.gif" CommandArgument='<%# Bind("Id_Cargo") %>' OnClick="btnEliminar172_Click"
                                            OnClientClick="return confirm('¿Está seguro de eliminar el registro, ya que este puede estar siendo usado en otra sección de su plan de negocio?')" Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    <FooterTemplate>
                                        <div style="text-align: left;">
                                            <label>Total Solicitado:</label>
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ver Detalle">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDetalle172" runat="server" ImageUrl="~/Images/Img/lupa.png" CommandArgument='<%# Bind("Id_Cargo") %>' OnClick="btnDetalle172_Click"
                                           />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEditar172" runat="server" OnClick="btnEditar172_Click"
                                            CommandArgument='<%# Bind("Id_Cargo") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("Cargo") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="ValorRemunCadena" HeaderText="Remuneración Unitario" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="OtrosGastosCadena" HeaderText="Otros Gastos" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ValorPrestacionesCadena" HeaderText="Valor con Prestaciones" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="UnidadTiempo" HeaderText="Unidad" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TiempoVinculacion" HeaderText="Tiempo Vinculación Primer Año" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="ValorRemunPrimerAnioCadena" HeaderText="Remuneración Primer Año" HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Valor Solicitado Fondo" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlrFondoEmprender" runat="server" Text='<%# Eval("ValorFondoEmprenderCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalFondoEmprender" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalFondoEmprender"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Aportes Emprendedores" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlrAportesEmprendedor" runat="server" Text='<%# Eval("AportesEmprendedorCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalAportesEmprendedor" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalAportesEmprendedor"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ingresos por Ventas" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblvlrIngresosVentas" runat="server" Text='<%# Eval("IngresosVentasCadena")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div style="text-align: right;">
                                            <asp:Label ID="lblTotalIngresosVentas" runat="server" Font-Bold="true" Text='<%#(DataBinder.GetPropertyValue(this, "TotalIngresosVentas"))%>' />
                                        </div>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>

                        </asp:GridView>

                        <br />
                        <asp:ValidationSummary ID="vsErrores"
                            runat="server"
                            HeaderText="Advertencia: "
                            ForeColor="Red"
                            Font-Italic="true"
                            ValidationGroup="grupo1" />
                        <br />

                        <div style="text-align: center">
                            <asp:Button ID="btnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="btnLimpiarCampos_Click" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' />&nbsp;
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1" />
                        </div>
                        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
                            <ProgressTemplate>
                                <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <br />
                <br />
                <br />
            </div>
            <asp:HiddenField ID="HiddenWidth" runat="server" />
            <script>
                $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
            </script>
        </div>
    </form>
</body>
</html>
