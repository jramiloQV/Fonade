<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngresosYCondicionesComerciales.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.IngresosYCondicionesComerciales" %>

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
                            <uc2:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="IngresosYCondiciones" />
                            <br />
                        </div>
                        <div style="text-align: center">
                            <h1>IV. ¿Cómo desarrollo mi solución?</h1>
                            <br />
                            <br />
                        </div>
                        <uc3:Help runat="server" ID="HelpPregunta9" Mensaje="ComoObtendraIngreso" Titulo="9. ¿Cómo obtendrá ingresos? Describa la estrategia de generación de ingresos para su proyecto" />
                        &nbsp;<asp:RequiredFieldValidator ID="rvPregunta9" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Pregunta 9") %>' Text="" Display="None" Font-Bold="True"
                            Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_Pregunta9" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                        <br />
                        <CKEditor:CKEditorControl ID="cke_Pregunta9" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                            Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                        <br />
                        <br />
                        <uc3:Help runat="server" ID="HelpPregunta10" Mensaje="DescribaCondicion" Titulo="10. Describa las condiciones comerciales que aplican para el portafolio de sus productos:" />
                        <br />
                        <asp:GridView ID="gw_pregunta10" runat="server" Width="100%" AutoGenerateColumns="False" OnPageIndexChanging="gw_pregunta10_PageIndexChanging"
                            CssClass="Grilla" AllowPaging="true" PageSize="5" AllowSorting="true"
                            ShowHeaderWhenEmpty="True" EmptyDataText='<%# Fonade.Negocio.Mensajes.Mensajes.GetMensaje(101)%>'>
                            <RowStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField HeaderText="Cliente">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:LinkButton ID="btnEditarCliente" runat="server" OnClick="btnEditarCliente_Click" Width="80px"
                                            CommandArgument='<%# Bind("IdCliente") %>' Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>'>
                                                                <%# Eval("Cliente") %></asp:LinkButton>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Volúmenes y Frecuencia">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblVolumen" runat="server" Text='<%# Bind("FrecuenciaCompra") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Características Compra">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblCaracteristica" runat="server" Text='<%# Bind("CaracteristicasCompra") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sitio de Compra">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblSitio" runat="server" Text='<%# Bind("SitioCompra") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Forma de Pago">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblFormaPago" runat="server" Text='<%# Bind("FormaPago") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Precio">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblPrecio" runat="server" Text='<%# Bind("PrecioCadena") %>' Width="100"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Requisitos Post-Venta">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblRecTecnico" runat="server" Text='<%# Bind("RequisitosPostVenta") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Garantías">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblGarantia" runat="server" Text='<%# Bind("Garantias") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margen de Comercialización">
                                    <ItemTemplate>
                                        <div style="word-wrap: break-word;">
                                            <asp:Label ID="lblMargen" runat="server" Text='<%# Bind("Margen") %>' Width="80"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <br />

                        <asp:Panel runat="server" ID="pnConsumidor" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "EsClienteConsumidor"))%>'>
                            <div>
                                <h1>Consumidor</h1>
                                <br />
                            </div>
                            <uc3:Help runat="server" ID="HelpPtaConsumidor1" Mensaje="DondeCompra" Titulo="¿Dónde Compra?" />
                            &nbsp;<asp:RequiredFieldValidator ID="rvPtaConsumidor1" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"¿Dónde Compra?") %>' Text="" Display="None" Font-Bold="True"
                                Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_PtaConsumidor1" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                            <br />
                            <CKEditor:CKEditorControl ID="cke_PtaConsumidor1" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                                Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                            <br />

                            <uc3:Help runat="server" ID="HelpPtaConsumidor2" Mensaje="CaracteristicasExigen" Titulo="¿Qué características se exigen para la compra (Ej: calidades, presentación - empaque)?" />
                            &nbsp;<asp:RequiredFieldValidator ID="rvPtaConsumidor2" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Características Compra") %>' Text="" Display="None" Font-Bold="True"
                                Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_PtaConsumidor2" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                            <br />
                            <CKEditor:CKEditorControl ID="cke_PtaConsumidor2" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                                Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                            <br />

                            <uc3:Help runat="server" ID="HelpPtaConsumidor3" Mensaje="CualFrecuencia" Titulo="¿Cuál es la frecuencia de compra?"/>
                            &nbsp;<asp:RequiredFieldValidator ID="rvPtaConsumidor3" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Frecuencia Compra") %>' Text="" Display="None" Font-Bold="True"
                                Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_PtaConsumidor3" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                            <br />
                            <CKEditor:CKEditorControl ID="cke_PtaConsumidor3" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                                Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                            <br />

                            <uc3:Help runat="server" ID="HelpPtaConsumidor4" Mensaje="Precio" Titulo="Precio"/>
                            &nbsp;<asp:RequiredFieldValidator ID="rvPtaConsumidor4" runat="server" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Precio") %>' Text="" Display="None" Font-Bold="True"
                                Font-Size="X-Large" ForeColor="Red" ControlToValidate="cke_PtaConsumidor4" SetFocusOnError="True" ToolTip="Requerido" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                            <br />
                            <CKEditor:CKEditorControl ID="cke_PtaConsumidor4" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"
                                Enabled='<%#((bool)DataBinder.GetPropertyValue(this, "AllowUpdate"))%>' ValidationGroup="grupo1"></CKEditor:CKEditorControl>
                            <br />
                        </asp:Panel>

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
