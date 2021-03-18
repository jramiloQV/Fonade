<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Estrategias.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.FuturoDelNegocio.Estrategias" EnableEventValidation="false" MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Estrategias</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery.number.min.js"></script>
     <style>
        #tc_proyectos_body {
            overflow: auto;            
            height : 100% !important;
        }
     </style>
    <script> function SetScroll(v) {
     var div = document.getElementById('divParentContainer');
     div.scrollTop = v;
 }</script>
</head>
<body>
    <form id="FormEstrategia" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <% Page.DataBind(); %>
                <div id="divParentContainer" runat="server">
                    <div class="childContainer">
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <div style="position: relative; left: 720px; width: 160px;">
                            <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="Estrategias" _mostrarPost='<%# PostitVisible %>' />
                        </div>
                        <div style="text-align: center">
                            <h1>V. ¿Cuál es el Futuro de mi Negocio?</h1>
                        </div>
                        <uc1:Help runat="server" ID="Help" Titulo="18. ¿Qué estrategias utilizará para lograr la meta de ventas y cuál es su presupuesto?" Mensaje="EstrategiaVentas" />
                        <fieldset id="fsPromocion">
                            <asp:Panel ID="PanelPro" runat="server" Enabled='<%# AllowUpdate %>'>
                                <br />
                                <asp:Label ID="LabelPromocion" runat="server" Text="Estrategia de promoción (nombre):" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtPromocion" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtPromocion" runat="server" ControlToValidate="txtPromocion" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Estrategia de promoción") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                                <asp:Label ID="Label2" runat="server" Text="Propósito:" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtProposito" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtProposito" runat="server" ControlToValidate="txtProposito" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Proposito/Promoción") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                            </asp:Panel>
                            <div id="divAgregar" runat="server" visible='<%# AllowUpdate %>'>
                                <asp:ImageButton ID="imgAgregar" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                &nbsp;
                        <asp:LinkButton ID="btnAgregarPromocion" runat="server" Text="Adicionar Actividad" OnClick="btnAgregarPromocion_Click"></asp:LinkButton>
                                <br />
                            </div>
                            <asp:GridView ID="gwPromocion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" PageSize="5" Width="100%" EmptyDataText="No existen registros para mostrar." AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gwPromocion_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" IdTipoEstrategia='<%# Eval("IdTipoEstrategia") %>' CommandArgument='<%# Bind("IdActividad") %>' OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" OnClick="btnEliminar_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actividad">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:LinkButton ID="BtnEditarPromocion" runat="server" CommandArgument='<%# Bind("IdActividad") %>' Width="100" Enabled='<%# AllowUpdate %>' OnClick="BtnEditarPromocion_Click"><%# Eval("Actividad") %></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recurso Requerido">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("RecursosRequeridos") %>  </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mes de Ejecución">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("MesEjecucion") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Costo">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 86px;">
                                                <%# decimal.Parse(Eval("Costo").ToString()).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<div style='word-wrap: break-word; width: 210px;'>Responsable<br/>(nombre del cargo lider del proceso)</div>">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 130px;"><%# Eval("Responsable") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#00468F" ForeColor="White" />
                            </asp:GridView>
                            <div id="divFooterGrid" style="background-color: #00468F; color: #FFFFFF; font-weight: bold; text-align: center; height: 17px; vertical-align: middle;">
                                <asp:Label ID="Label" runat="server" Text="Costo Total" Width="49%" BackColor="#00468F"></asp:Label>│
                        <asp:Label ID="LabelCosto" runat="server" Text="000,000.00" Width="49%" BackColor="#00468F"></asp:Label>
                            </div>
                        </fieldset>
                        <fieldset id="fsComunicacion">
                            <asp:Panel ID="PanelCom" runat="server" Enabled='<%# AllowUpdate %>'>
                                <br />
                                <asp:Label ID="LabelComunicacion" runat="server" Text="Estrategia de Comunicación (nombre):" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtComunicacion" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtComunicacion" runat="server" ControlToValidate="txtComunicacion" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Estrategia de comunicación") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                                <asp:Label ID="Label1" runat="server" Text="Propósito:" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtPropositoCom" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtPropositoCom" runat="server" ControlToValidate="txtPropositoCom" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Proposito/Comunicación") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                            </asp:Panel>
                            <div id="divAgregarCom" runat="server" visible='<%# AllowUpdate %>'>
                                <asp:ImageButton ID="ImgAgregarCom" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="ImgAgregarCom_Click" />
                                &nbsp;
                        <asp:LinkButton ID="btnAgregarComunicacion" runat="server" Text="Adicionar Actividad" OnClick="btnAgregarComunicacion_Click"></asp:LinkButton>
                                <br />
                            </div>
                            <asp:GridView ID="gwComunicacion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" PageSize="5" Width="100%" EmptyDataText="No existen registros para mostrar." AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gwComunicacion_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminarCom" runat="server" ImageUrl="~/Images/icoBorrar.gif" IdTipoEstrategia='<%# Eval("IdTipoEstrategia") %>' CommandArgument='<%# Bind("IdActividad") %>' OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" OnClick="btnEliminarCom_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actividad">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:LinkButton ID="BtnEditarComunicacion" runat="server" CommandArgument='<%# Bind("IdActividad") %>' Width="100" Enabled='<%# AllowUpdate %>' OnClick="BtnEditarComunicacion_Click"><%# Eval("Actividad") %></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recurso Requerido">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("RecursosRequeridos") %>  </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mes de Ejecución">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("MesEjecucion") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Costo">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 86px;">
                                                <%# decimal.Parse(Eval("Costo").ToString()).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<div style='word-wrap: break-word; width: 210px;'>Responsable<br/>(nombre del cargo lider del proceso)</div>">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 130px;"><%# Eval("Responsable") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#00468F" ForeColor="White" />
                            </asp:GridView>
                            <div id="divFooterGridCom" style="background-color: #00468F; color: #FFFFFF; font-weight: bold; text-align: center; height: 17px; vertical-align: middle;">
                                <asp:Label ID="Label3" runat="server" Text="Costo Total" Width="49%" BackColor="#00468F"></asp:Label>│
                        <asp:Label ID="LabelCostoCom" runat="server" Text="000,000.00" Width="49%" BackColor="#00468F"></asp:Label>
                            </div>
                        </fieldset>
                        <fieldset id="fsDistribucion">
                            <asp:Panel ID="PanelDis" runat="server" Enabled='<%# AllowUpdate %>'>
                                <br />
                                <asp:Label ID="LabelDistribucion" runat="server" Text="Estrategia de Distribución (nombre):" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtDistribucion" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtDistribucion" runat="server" ControlToValidate="txtDistribucion" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Estrategia de distribución") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                                <asp:Label ID="Label4" runat="server" Text="Propósito:" Width="215px"></asp:Label>
                                <asp:TextBox ID="txtPropositoDis" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredtxtPropositoDis" runat="server" ControlToValidate="txtPropositoDis" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Proposito/Distribución") %>' ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                <br />
                                <br />
                            </asp:Panel>
                            <div id="divAgregarDis" runat="server" visible='<%# AllowUpdate %>'>
                                <asp:ImageButton ID="imgAgregarDis" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" OnClick="imgAgregarDis_Click" />
                                &nbsp;
                        <asp:LinkButton ID="btnAgregarDistribucion" runat="server" Text="Adicionar Actividad" OnClick="btnAgregarDistribucion_Click"></asp:LinkButton>
                                <br />
                            </div>
                            <asp:GridView ID="gwDistribucion" runat="server" AutoGenerateColumns="False" CssClass="Grilla" PageSize="5" Width="100%" EmptyDataText="No existen registros para mostrar." AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gwDistribucion_PageIndexChanging" ShowHeaderWhenEmpty="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEliminarDis" runat="server" ImageUrl="~/Images/icoBorrar.gif" IdTipoEstrategia='<%# Eval("IdTipoEstrategia") %>' CommandArgument='<%# Bind("IdActividad") %>' OnClientClick="return confirm('¿Está seguro de eliminar el registro seleccionado?')" OnClick="btnEliminarDis_Click" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="50" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Actividad">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:LinkButton ID="BtnEditarDistribucion" runat="server" CommandArgument='<%# Bind("IdActividad") %>' Width="100" Enabled='<%# AllowUpdate %>' OnClick="BtnEditarDistribucion_Click"><%# Eval("Actividad") %></asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Recurso Requerido">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("RecursosRequeridos") %>  </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mes de Ejecución">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 180px;"><%# Eval("MesEjecucion") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Costo">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 86px;">
                                                <%# decimal.Parse(Eval("Costo").ToString()).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture) %>
                                            </div>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<div style='word-wrap: break-word; width: 210px;'>Responsable<br/>(nombre del cargo lider del proceso)</div>">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word; width: 130px;"><%# Eval("Responsable") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#00468F" ForeColor="White" />
                            </asp:GridView>
                            <div id="divFooterGridDis" style="background-color: #00468F; color: #FFFFFF; font-weight: bold; text-align: center; height: 17px; vertical-align: middle;">
                                <asp:Label ID="Label5" runat="server" Text="Costo Total" Width="49%" BackColor="#00468F"></asp:Label>│
                        <asp:Label ID="LabelCostoDis" runat="server" Text="000,000.00" Width="49%" BackColor="#00468F"></asp:Label>
                            </div>
                        </fieldset>
                        <blockquote>
                            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" ValidationGroup="Requerido" />
                        </blockquote>
                        <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                            <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="Requerido" OnClick="BtnGuardar_Click" OnClientClick="SummaryFocus(1400);" />
                        </div>
                        <asp:HiddenField ID="HiddenWidth" runat="server" />
                        <script>
                            $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
                        </script>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
            <ProgressTemplate>
                <div style="text-align: right"><b>Procesando información</b>&nbsp;&nbsp;<img src="../../../Images/fbloader.gif" />&nbsp;&nbsp;</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
