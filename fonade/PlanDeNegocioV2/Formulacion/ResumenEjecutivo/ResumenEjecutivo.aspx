<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResumenEjecutivo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.ResumenEjecutivo" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagPrefix="uc1" TagName="Encabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="uc1" TagName="Help" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Resumen Ejecutivo</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(rb) {
            var isChecked = rb.checked;
            var row = rb.parentNode.parentNode;

            var currentRdbID = rb.id;
            parent = document.getElementById("<%= gvProductos.ClientID %>");
            var items = parent.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].id != currentRdbID && items[i].type == "radio") {
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel" runat="server">
            <ContentTemplate>
                <% Page.DataBind(); %>
                <div id="divParentContainer" runat="server">
                    <div class="childContainer">
                        <uc1:Encabezado runat="server" ID="Encabezado" />
                        <div style="position: relative; left: 720px; width: 160px;">
                            <uc1:Post_It runat="server" ID="Post_It" Visible='<%# PostitVisible %>' _txtCampo="ResumenEjecutivo"
                                _mostrarPost='<%# PostitVisible %>' />
                        </div>
                        <div style="text-align: center">
                            <h1>VII. Resumen Ejecutivo</h1>
                        </div>
                        <br />
                        Emprendedor(es) y equipo de trabajo:
                <asp:GridView ID="gwEmprendedores" runat="server" AutoGenerateColumns="False" CssClass="Grilla" PageSize="5" Width="100%" EmptyDataText="No existen registros para mostrar." AllowPaging="True" AllowSorting="True" ShowHeaderWhenEmpty="True" OnPageIndexChanging="gwEmprendedores_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <div style="word-wrap: break-word;">
                                    <asp:LinkButton ID="BtnVerDetalle" runat="server" CommandArgument='<%# Bind("IdContacto") %>' Width="300px" OnClick="BtnVerDetalle_Click"><%# Eval("Nombre") %></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 300px;"><%# Eval("Email") %>  </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rol">
                            <ItemTemplate>
                                <div style="word-wrap: break-word; width: 300px;"><%# Eval("Rol") %></div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#00468F" ForeColor="White" />
                </asp:GridView>
                        <br />
                        <uc1:Help runat="server" ID="Help1" Titulo="Concepto del Negocio:" Mensaje="ConceptoNegocioResumen" />
                        <CKEditor:CKEditorControl ID="CKConcepto" runat="server" Enabled='<%# AllowUpdate %>'></CKEditor:CKEditorControl>
                        <asp:RequiredFieldValidator ID="RequiredCKConcepto" runat="server" ControlToValidate="CKConcepto" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Concepto del negocio") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                        <fieldset>
                            <legend>
                                <uc1:Help runat="server" ID="Help2" Titulo="Indicadores de gestión" Mensaje="ResumenMetas" />
                            </legend>
                            <blockquote>
                                <asp:Panel runat="server" Enabled='<%# AllowUpdate %>' Visible="false">
                                    Indicador Empleos:<br />
                                    <p style="display: inline; padding-left: 20px;">Meta para el primer año:</p>
                                    <asp:TextBox ID="txtEmpleo" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmpleo" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Indicador Empleos") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    Indicador Ventas:<br />
                                    <p style="display: inline; padding-left: 20px;">Meta para el primer año:</p>
                                    <asp:TextBox ID="txtVentas" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtVentas" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Indicador Ventas") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    Indicador Mercadeo (eventos):<br />
                                    <p style="display: inline; padding-left: 20px;">Meta para el primer año:</p>
                                    <asp:TextBox ID="txtMercadeo" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtMercadeo" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Indicador Mercadeo...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    Indicador Contrapartida SENA:<br />
                                    <p style="display: inline; padding-left: 20px;">Meta para el primer año:</p>
                                    <asp:TextBox ID="txtSena" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtSena" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Indicador Contrapartida...") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    Indicador Empleos Indirectos:<br />
                                    <p style="display: inline; padding-left: 20px;">Meta para el primer año:</p>
                                    <asp:TextBox ID="txtIndirectos" runat="server" Width="600px" MaxLength="100"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtIndirectos" ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"Indicador Empleos Indirectos") %>' SetFocusOnError="True" ValidationGroup="Requerido" Display="None"></asp:RequiredFieldValidator>
                                </asp:Panel>

                                <table id="gvMain" class="auto-style2 Grilla" runat="server" style="width: 100%;">
                                    <tr>
                                        <th colspan="2">Indicadores</th>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">
                                            <asp:Label ID="lblTitle1" Font-Bold="true" runat="server" Text="Empleos:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtEmpleosDetectados" Enabled="false" runat="server" Width="300px"></asp:TextBox>
                                        </td>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="Contrapartidas:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtContrapartidas" Enabled="false" runat="server" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Ejecución presupuestal:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtEjecucionPresupuestal" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Ventas:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtVentasProductos" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Mercadeo:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtMercadeoTotal" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Periodo improductivo:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtPeriodoImproductivo" runat="server" Width="300px" Enabled='<%# AllowUpdate %>'></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label6" Font-Bold="true" runat="server" Text="IDH:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtIDH" runat="server" Width="300px" Enabled="false"></asp:TextBox>
                                        </td>
                                        <td class="auto-style3">
                                            <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="Recursos aportados emprendedor:"></asp:Label>
                                            <br />
                                            <asp:TextBox ID="txtRecursosAportados" runat="server" Enabled='<%# AllowUpdate %>' TextMode="MultiLine" Height="50px" Width="300px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:GridView ID="gvProductos" Visible="true" runat="server" DataSourceID="data"
                                    AutoGenerateColumns="False" CssClass="Grilla" Width="100%"
                                    EmptyDataText="No existen productos para mostrar." AllowPaging="false"
                                    AllowSorting="false" ShowHeaderWhenEmpty="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Producto representativo">
                                            <ItemTemplate>
                                                <div style="word-wrap: break-word;">
                                                    <asp:RadioButton ID="rdProductoSeleccionado" onclick="javascript:CheckOtherIsCheckedByGVID(this);" GroupName="checkProductos" Text="" runat="server" Checked='<%# (bool)Eval("Selected") %>' Enabled='<%# AllowUpdate %>' />
                                                    <asp:HiddenField runat="server" ID="hdCodigoProducto" Value='<%# Eval("IdProducto") %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Producto" DataField="NombreProducto" HtmlEncode="false" />
                                        <asp:TemplateField HeaderText="Unidades">
                                            <ItemTemplate>
                                                <asp:Label runat="server" Text='<%# Eval("Unidades") + " " + Eval("unidadDeMedida") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#00468F" ForeColor="White" />
                                </asp:GridView>
                            </blockquote>
                        </fieldset>
                        <!--Sector y subsector-->
                        <fieldset>
                            <legend>
                                <uc1:Help runat="server" ID="Help3"
                                    Titulo="¿En que sector se encuentra clasificado el proyecto a desarrollar?" Mensaje="" />
                            </legend>
                            <blockquote>
                                <div>


                                    <asp:Label ID="LabelSector" runat="server"
                                        Text="Sector: " Width="150"></asp:Label>
                                    <asp:DropDownList ID="cmbSector" runat="server" Width="400px" 
                                        Enabled='<%# AllowUpdate %>'
                                        DataValueField="idSector" DataTextField="nomSector" AutoPostBack="true"
                                        OnSelectedIndexChanged="cmbSector_SelectedIndexChanged" />
                                    <hr />
                                    <asp:Label ID="Label8" runat="server"
                                        Text="SubSector: " Width="150"></asp:Label>
                                    <asp:DropDownList ID="cmbSubSector" runat="server" Width="400px"
                                        Enabled='<%# AllowUpdate %>'
                                        DataTextField="nomSector" DataValueField="idSector" />


                                    <br />
                                </div>
                            </blockquote>
                        </fieldset>
                        <!--Video Emprendedor-->
                        <fieldset>
                            <legend>
                                <uc1:Help runat="server" ID="idPnlVideo" Titulo="Video del Emprendedor" Mensaje="" />
                            </legend>
                            <blockquote>
                                <p style="display: inline; padding-left: 20px;">URL Video del Emprendedor (Enlace de Youtube):</p>
                                <asp:TextBox ID="txtEnlaceVideoEmprendedor" runat="server" Enabled='<%# AllowUpdate %>'
                                    placeholder="Ejemplo https://www.youtube.com/watch?v=xxxxxxxxxx"
                                    Width="600px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                    ControlToValidate="txtEnlaceVideoEmprendedor"
                                    ErrorMessage='<%# string.Format(Fonade.Negocio.Mensajes.Mensajes.GetMensaje(104),"URL Video del Emprendedor (Enlace de Youtube)") %>'
                                    SetFocusOnError="True" ValidationGroup="Requerido" Display="None">
                                </asp:RequiredFieldValidator>
                                <hr />
                                <asp:Label ID="lblErrorVideo" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                                <br />

                                <div id="FrmVideoEmprendedor" runat="server">
                                </div>



                            </blockquote>
                        </fieldset>

                        <blockquote>
                            <asp:ValidationSummary ID="ValidationSummary" runat="server" Font-Italic="True" ForeColor="Red" HeaderText="Advertencia:" ValidationGroup="Requerido" />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        </blockquote>



                        <div id="divBotones" runat="server" visible='<%# AllowUpdate %>' style="text-align: center">
                            <asp:Button ID="BtnLimpiarCampos" runat="server" Text="Limpiar Campos" OnClick="BtnLimpiarCampos_Click" />&nbsp;<asp:Button ID="BtnGuardar" runat="server" Text="Guardar" ValidationGroup="Requerido" OnClick="BtnGuardar_Click" />
                        </div>
                        <asp:HiddenField ID="HiddenWidth" runat="server" />
                        <script>
                            $(document).ready(function () { $('input[name="HiddenWidth"]').val(screen.width); });
                        </script>
                    </div>
                </div>

                <asp:ObjectDataSource
                    ID="data"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Formulacion.ResumenEjecutivo.ResumenEjecutivo"
                    SelectMethod="GetProductos"
                    EnablePaging="false">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codproyecto" />
                    </SelectParameters>
                </asp:ObjectDataSource>



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
