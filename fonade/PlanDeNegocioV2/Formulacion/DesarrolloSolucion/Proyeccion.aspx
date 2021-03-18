<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Proyeccion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Help.ascx" TagPrefix="controlHelp" TagName="Help" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>

<!DOCTYPE html >
<html style="overflow-x: hidden;">
<head runat="server">
        <title>Proyecciones de ventas </title>
        <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
        <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
        <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
        <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
        <script src="../../../Scripts/common.js"></script>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px !important;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .parentContainer {
            width: 100%;
            height: 650px;
            overflow-x: hidden;
            overflow-y: visible;
        }

        .childContainer {
            width: 100%;
            height: auto;
        }

        html, body, div, iframe {
        }
    </style>
    <script type="text/javascript">
        function alertTiempo() {
            var tiempo = $('#<%=cmbTiempoProyeccion.ClientID %> option:selected').text();
            if (tiempo !== '')
                return confirm('¿ Está seguro de cambiar el tiempo de proyección ya que se borrará la información antes ingresada ?');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <% Page.DataBind(); %>
                <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
                <div style="position: relative; left: 705px; width: 160px;">
                    <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Proyeccion" />
                </div>
                <div style="text-align: center">
                    <h1>IV - ¿ Como desarrollo mi solución ?</h1>
                </div>
                <br />
                <controlHelp:Help runat="server" ID="helpProyeccion" Mensaje="ProyeccionVentasV2" Titulo="11. Realice la proyección de cantidades y precios de venta (mensual). Justifique los resultados y señala la forma de pago:" />
                <br />
                <asp:Label ID="lblTiempoProyeccion" runat="server" Text="Tiempo de Proyección de ventas" />
                <asp:DropDownList ID="cmbTiempoProyeccion" runat="server" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' CssClass="tiempo">
                    <asp:ListItem Text="" Value="" />
                    <asp:ListItem Value="3">3 años</asp:ListItem>
                    <asp:ListItem Value="4">4 años</asp:ListItem>
                    <asp:ListItem Value="5">5 años</asp:ListItem>
                    <asp:ListItem Value="6">6 años</asp:ListItem>
                    <asp:ListItem Value="7">7 años</asp:ListItem>
                    <asp:ListItem Value="8">8 años</asp:ListItem>
                    <asp:ListItem Value="9">9 años</asp:ListItem>
                    <asp:ListItem Value="10">10 años</asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnSave" runat="server" Text="Guardar" align="center" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClick="btnSave_Click" />
                <br />
                <br />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Medium" Visible="false">Sucedio un error inesperado, intentalo de nuevo.</asp:Label>
                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                    <ProgressTemplate>
                        <div class="form-group center-block">
                            <div class="col-xs-4">
                            </div>
                            <div class="col-xs-4">
                                <label class="control-label"><b>Actualizando información </b></label>
                                <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <br />
                <br />
                <div style="background-color: #00468F; text-align: center; color: White;">
                    <b>Listado de productos
                    </b>
                </div>
                <asp:GridView ID="gvProductos" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla" AllowPaging="false" AllowSorting="false" EmptyDataText="No existen productos para este plan de negocio." DataSourceID="dataProductos" OnRowCommand="gvProductos_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre de Producto" DataField="NomProducto" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Unidad de medida" DataField="UnidadMedida" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Forma de pago" DataField="FormaDePago" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Justificación" DataField="Justificacion" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Iva" DataField="Iva" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="Proyección de ventas">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Producto")%>' CommandName="actualizar" Text="Editar Proyección" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdateProyeccion")) %>' Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></asp:LinkButton>
                                <asp:Label ID="lblErrorProyeccion" runat="server" ForeColor="Red" Font-Bold="True" Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "AllowUpdateProyeccion")) %>'>Debe seleccionar un tiempo de proyección</asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>


                <br />
                <br />
                <br />
                <div runat="server" id="tituloIngresosPorVentas" style="background-color: #00468F; text-align: center; color: White;">
                    <b>Ingresos por ventas
                    </b>
                </div>
                <asp:GridView ID="gvIngresosPorVentas" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grilla" AllowPaging="false" AllowSorting="false" ShowHeaderWhenEmpty="true" EmptyDataText="No hay datos que mostrar" DataSourceID="dataVentas">
                    <Columns>
                        <asp:TemplateField HeaderText="Periodo">
                            <ItemTemplate>
                                <asp:Label ID="lblPeriodo" runat="server" Text='<%# Eval("Periodo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 1">
                            <ItemTemplate>
                                <asp:Label ID="lblYear1" runat="server" Text='<%# Eval("Year1Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 2">
                            <ItemTemplate>
                                <asp:Label ID="lblYear2" runat="server" Text='<%# Eval("Year2Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 3">
                            <ItemTemplate>
                                <asp:Label ID="lblYear3" runat="server" Text='<%# Eval("Year3Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 4" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear4" runat="server" Text='<%# Eval("Year4Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 5" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear5" runat="server" Text='<%# Eval("Year5Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 6" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear6" runat="server" Text='<%# Eval("Year6Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 7" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear7" runat="server" Text='<%# Eval("Year7Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 8" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear8" runat="server" Text='<%# Eval("Year8Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 9" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear9" runat="server" Text='<%# Eval("Year9Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Año 10" Visible="true">
                            <ItemTemplate>
                                <asp:Label ID="lblYear10" runat="server" Text='<%# Eval("Year10Formatted") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:ObjectDataSource
                    ID="dataProductos"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Formulacion.Solucion.FichaTecnica"
                    SelectMethod="Get">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="codigoProyecto" QueryStringField="codproyecto" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>

                <asp:ObjectDataSource
                    ID="dataVentas"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.Proyeccion"
                    SelectMethod="GetIngresosPorVentas">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="codigoProyecto" QueryStringField="codproyecto" DefaultValue="0" />
                    </SelectParameters>
                </asp:ObjectDataSource>

            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
