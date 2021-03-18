<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="GestionMercadeo.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.GestionMercadeo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style1 {
            width: 191px;
        }

        .auto-style3 {
            width: 127px;
        }
    </style>

    <script type="text/javascript">
        function alerta() {
            return confirm("Está seguro que desea eliminar la actividad seleccionada?");
        }
    </script>

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

</head>
<body>
    <form id="form1" runat="server">
        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.3 Gestión en Mercadeo"></asp:Label>
            </h3>
        </div>
        <div style="text-align: center;">
            <h1 style="color: #00409b">METAS</h1>
        </div>
        <div>
            <asp:GridView ID="gvMetasMercadeo" runat="server"
                AutoGenerateColumns="False" CssClass="Grilla"
                DataKeyNames="idActividadInterventoria"
                AllowPaging="True" ForeColor="#666666" Width="100%"
                OnPageIndexChanging="gvMetasMercadeo_PageIndexChanging"
                OnRowCommand="gvMetasMercadeo_RowCommand" OnRowCancelingEdit="gvMetasMercadeo_RowCancelingEdit"
                OnRowEditing="gvMetasMercadeo_RowEditing" OnRowUpdating="gvMetasMercadeo_RowUpdating"
                EmptyDataText="No hay datos para mostrar.">
                <Columns>
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" />

                    <asp:TemplateField HeaderText="Cantidad">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnidadesEditar" runat="server" Text='<%# Bind("Unidades") %>' type="number"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblUnidades" runat="server" Text='<%# Bind("Unidades") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Descripción">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtActividadEditar" runat="server" Text='<%# Bind("Actividad") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblActividad" runat="server" Text='<%# Bind("Actividad") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Eliminar">
                        <ItemTemplate>

                            <asp:ImageButton ID="btn_Borrar" CommandArgument='<%# Eval("idActividadInterventoria")%>' runat="server"
                                ImageUrl="/Images/icoBorrar.gif" Visible="true" CausesValidation="false"
                                CommandName="Eliminar"
                                OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div style="text-align: center;">

                <input type="button" onclick="mostrar('panelAgregarNuevoMercadeo')"
                    value="Agregar Mercadeo" class="btnCertif" />

            </div>
            <hr />
            <div id="panelAgregarNuevoMercadeo" style="display: none;">
                <h1 style="text-align: center;">
                    <asp:Label Text="Agregar Mercadeo"
                        runat="server" ID="Label3" Visible="true" />
                </h1>
                <hr />
                <table style="width: 100%;">
                    <tr>
                        <td class="auto-style1">Cantidad:</td>
                        <td>
                            <asp:TextBox ID="txtCantidadMercadeo" runat="server" type="number"
                                MaxLength="500" Width="25%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">Descripción:</td>
                        <td>
                            <asp:TextBox ID="txtDescripcionMercadeo" runat="server" autocomplete="off" >
                            </asp:TextBox>
                        </td>
                    </tr>                   
                    <tr>
                        <!--OnClick="btnGuardarProducto_Click"-->
                        <td colspan="2" style="text-align: center" class="auto-style2">
                            <asp:Button ID="btnGuardarMercadeoAdd" runat="server" Height="29px"
                                Text="Guardar"
                                OnClick="btnGuardarMercadeoAdd_Click" />

                        </td>
                    </tr>
                </table>
            </div>
            <div style="text-align: center">
                *Tabla de meta propuesta en el plan de negocios y ajustada por el Evaluador
            </div>
            <h2 style="color: #00409b; text-align: center">META TOTAL DE EVENTOS: 
                            <asp:Label ID="lblMetaTotalActividad" runat="server" Text="0"></asp:Label>

            </h2>
        </div>
        <hr />
        <div>
            <asp:GridView ID="gvIndicador" runat="server"
                AutoGenerateColumns="False"
                CssClass="Grilla"
                DataKeyNames="id"
                OnPageIndexChanging="gvIndicador_PageIndexChanging"
                EmptyDataText="No se ha registrado indicador."
                AllowPaging="True" ForeColor="#666666" Width="100%">
                <Columns>
                    <asp:BoundField DataField="Visita" HeaderText="Visita" />
                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="DescripcionEvento" HeaderText="Descripcion del Evento" />
                    <asp:BoundField DataField="PublicidadLogos" HeaderText="Publicidad de Logos" />
                </Columns>
            </asp:GridView>
        </div>
        <hr />
        <asp:Panel ID="pnlAddIndicador" runat="server">
            <table style="width: 100%;">
                <tr>
                    <td class="auto-style1">
                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style3">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr style="background-color: #00468f; color: white;">
                    <td class="auto-style1">Visita</td>
                    <td class="auto-style3">Cantidad</td>
                    <td>Descripción del Evento</td>
                    <td>Publicidad del Logos</td>
                </tr>
                <tr>
                    <td class="auto-style1" style="text-align: center">
                        <asp:Label ID="lblnumVisita" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:TextBox ID="txtCantidad" runat="server" Style="text-align: center"
                            type="number" MaxLength="4" min="0" pattern="^[0-9]+"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDescripcionEvento" runat="server" TextMode="MultiLine"
                            MaxLength="10000" Width="97%"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPublicidadLogos" runat="server" MaxLength="10000"
                            TextMode="MultiLine" Width="97%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center">
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </form>
</body>
</html>
