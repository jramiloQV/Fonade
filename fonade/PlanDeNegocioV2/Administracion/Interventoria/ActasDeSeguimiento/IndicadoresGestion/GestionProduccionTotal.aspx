<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionProduccionTotal.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.GestionProduccionTotal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .auto-style1 {
            width: 326px;
        }

        .auto-style2 {
            width: 177px;
        }

        .auto-style3 {
            width: 97px;
        }

        .auto-style4 {
            width: 133px;
        }
    </style>

    <script type="text/javascript">
        function mostrar(elemento) {
            if (document.getElementById(elemento).style.display == 'inherit') {
                document.getElementById(elemento).style.display = 'none'
            }
            else {
                document.getElementById(elemento).style.display = 'inherit'
            }
        };
    </script>

    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(rb) {
            var isChecked = rb.checked;
            var row = rb.parentNode.parentNode;

            var currentRdbID = rb.id;
            parent = document.getElementById("<%= gvMetasProduccion.ClientID %>");
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

    <script type="text/javascript">
        function alerta() {
            return confirm("Está seguro que desea eliminar el producto seleccionado?");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.5 Gestión en Producción"></asp:Label>
            </h3>
        </div>
        <div>
            <h2>Producto mas representativo:
                <asp:Label ID="lblProductoMasRep" runat="server" Text="Producto"></asp:Label></h2>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">METAS</h1>
        </div>
        <div>
            <asp:GridView ID="gvMetasProduccion" runat="server"
                AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="Id_ProductoInterventoria"
                OnPageIndexChanging="gvMetasProduccion_PageIndexChanging"
                AllowPaging="True" ForeColor="#666666" Width="100%" OnRowCommand="gvMetasProduccion_RowCommand"
                OnRowCancelingEdit="gvMetasProduccion_RowCancelingEdit"
                OnRowEditing="gvMetasProduccion_RowEditing" OnRowUpdating="gvMetasProduccion_RowUpdating"
                EmptyDataText="No hay datos para mostrar.">
                <Columns>
                     <asp:CommandField ShowEditButton="true" ShowCancelButton="true" />
                    <asp:TemplateField HeaderText="Producto representativo">
                        <EditItemTemplate>
                            <div style="word-wrap: break-word;">
                                <asp:RadioButton ID="rdProductoSeleccionado" 
                                    onclick="javascript:CheckOtherIsCheckedByGVID(this);" 
                                    GroupName="checkProductos" Text="" runat="server" Checked='<%# (bool)Eval("productoRepresentativo") %>' />
                                <asp:HiddenField runat="server" ID="hdCodigoProducto" Value='<%# Eval("Id_ProductoInterventoria") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <div style="word-wrap: break-word;">
                                <asp:RadioButton ID="rdProductoSeleccionado" 
                                    onclick="javascript:CheckOtherIsCheckedByGVID(this);" 
                                    GroupName="checkProductos" Text="" runat="server" Checked='<%# (bool)Eval("productoRepresentativo") %>' 
                                    Enabled="false" />
                                <asp:HiddenField runat="server" ID="hdCodigoProducto" Value='<%# Eval("Id_ProductoInterventoria") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>                    
                   
                    <asp:TemplateField HeaderText="Cantidad">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnidadesEditar" runat="server" Text='<%# Bind("Unidades") %>' type="number"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnidades" runat="server" Text='<%# Bind("Unidades") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unidad de Medida">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnidadMedidaEditar" runat="server" Text='<%# Bind("UnidadMedida") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnidadMedida" runat="server" Text='<%# Bind("UnidadMedida") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Producto o Servicio">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtProducto" runat="server" Text='<%# Bind("NomProducto") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblProducto" runat="server" Text='<%# Bind("NomProducto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>

                            <asp:ImageButton ID="btn_Borrar" CommandArgument='<%# Eval("Id_ProductoInterventoria")%>' runat="server"
                                ImageUrl="/Images/icoBorrar.gif" Visible="true" CausesValidation="false"
                                CommandName="Eliminar"
                                OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center;">

                <input type="button" onclick="mostrar('panelAgregarNuevoProducto')"
                    value="Agregar Producto" class="btnCertif" />

            </div>
            <div id="panelAgregarNuevoProducto" style="display: none;">
                <h1 style="text-align: center;">
                    <asp:Label Text="Agregar Producto o Servicio"
                        runat="server" ID="Label2" Visible="true" />
                </h1>
                <hr />
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style1">Nombre Producto:</td>
                        <td>
                            <asp:TextBox ID="txtNomProducto" runat="server"
                                MaxLength="500" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Porcentaje IVA:</td>
                        <td>
                            <asp:TextBox ID="txtPorcentajeIVA" runat="server" autocomplete="off"
                                type="number">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Nombre Comercial:</td>
                        <td>
                            <asp:TextBox ID="txtNomComercial" runat="server" MaxLength="500" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Unidad de Medida:</td>
                        <td>
                            <asp:TextBox ID="txtUnidadMedida" runat="server" MaxLength="500"
                                Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Descripcion General:</td>
                        <td>
                            <asp:TextBox ID="txtDescripcionGeneral" runat="server" MaxLength="500"
                                Width="97%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Condiciones Especiales:</td>
                        <td>
                            <asp:TextBox ID="txtCondicionesEspeciales" runat="server" MaxLength="500"
                                Width="97%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Composicion:</td>
                        <td>
                            <asp:TextBox ID="txtComposicion" runat="server" MaxLength="500"
                                Width="97%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Otros:</td>
                        <td>
                            <asp:TextBox ID="txtOtros" runat="server" MaxLength="500"
                                Width="97%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Forma de Pago:</td>
                        <td>
                            <asp:TextBox ID="txtFormaPago" runat="server" MaxLength="500"
                                Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Justificacion:</td>
                        <td>
                            <asp:TextBox ID="txtJustificacion" runat="server" MaxLength="500"
                                Width="97%" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Meta:</td>
                        <td>
                            <asp:TextBox ID="txtMetaProducto" runat="server" MaxLength="500"
                                Width="97%" type="number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <!--OnClick="btnGuardarProducto_Click"-->
                        <td colspan="2" style="text-align: center" class="auto-style2">
                            <asp:Button ID="Button1" runat="server" Height="29px"
                                Text="Guardar"
                                OnClick="btnGuardarProducto_Click" />

                        </td>
                    </tr>
                </table>
            </div>
            <asp:Label ID="lblUnidadMedida" runat="server" Text="" Visible="false"></asp:Label>
            <%--<div style="text-align: center">*Meta propuesta con el producto más Representativo/Significativo
            </div>--%>
        </div>
        <hr />
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                OnPageIndexChanging="gvIndicador_PageIndexChanging"
                DataKeyNames="id"
                EmptyDataText="No se ha registrado indicador."
                ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Visita" HeaderText="Visita" />
                    <asp:BoundField DataField="NomProducto" HeaderText="Producto o Servicio" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="medida" HeaderText="Unidad de Medida" />
                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                </Columns>
            </asp:GridView>
            <div style="text-align: center">Tabla del indicador de cumplimiento de la meta propuesta</div>
        </div>
        <hr />
        <!--Ingreso de indicadores-->
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <asp:GridView ID="gvAgregarIndicadorProduccion" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="Id_ProductoInterventoria" ForeColor="#666666" Width="100%"
                EmptyDataText="No hay productos para mostrar.">
                <Columns>
                    <asp:TemplateField ShowHeader="False" HeaderText="codProyecto" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblproductoRepresentativo" runat="server"
                                Text='<%# Eval("productoRepresentativo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="codProyecto" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCodProducto" runat="server"
                                Text='<%# Eval("Id_ProductoInterventoria") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="visita" HeaderText="Visita" />
                    <asp:BoundField DataField="NomProducto" HeaderText="Producto o Servicio" />
                    <asp:TemplateField ShowHeader="False" HeaderText="codUnidadMedida" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblNomProducto" runat="server"
                                Text='<%# Eval("NomProducto") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UnidadMedida" HeaderText="Unidad de Medida" />
                    <asp:TemplateField ShowHeader="False" HeaderText="codUnidadMedida" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblUnidadMedida" runat="server"
                                Text='<%# Eval("UnidadMedida") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="Cantidad">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCantidad" runat="server"
                                Style="text-align: center"
                                type="number" MaxLength="4" min="0" pattern="^[0-9]+"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False" HeaderText="Descripcion">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescripcion" runat="server"
                                TextMode="MultiLine" MaxLength="10000"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <div style="text-align: center">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
            </div>
        </asp:Panel>

    </form>
</body>
</html>
