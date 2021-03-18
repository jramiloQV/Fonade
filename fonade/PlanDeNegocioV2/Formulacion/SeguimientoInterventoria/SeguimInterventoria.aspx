<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimInterventoria.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.SeguimientoInterventoria.SeguimInterventoria" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Seguimiento Interventoria</title>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" />
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <script src="../../../Scripts/common.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />

    <style>
        .borderDiv {
            border-style: solid;
            border-width: 1px;
            padding: 5px;
        }
    </style>

    <script type="text/javascript">
        function alerta() {
            return confirm('¿Está seguro de eliminar este archivo?');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblTitulo" runat="server" Text="Seguimiento Interventoria" Font-Size="Medium"></asp:Label>
            <hr />
        </div>

        <fieldset id="panelInterventor" runat="server">
            <legend>Habilitar cargue de archivos del acta</legend>
            <div>
                <asp:Label ID="lblListACta" runat="server" Text="Habilitar Acta:"></asp:Label>
                <asp:DropDownList ID="ddlHabilitarActa" runat="server">
                    <asp:ListItem Selected="True" Value="S"> Seleccione... </asp:ListItem>
                    <asp:ListItem Value="1"> Acta No. 1 </asp:ListItem>
                    <asp:ListItem Value="2"> Acta No. 2 </asp:ListItem>
                    <asp:ListItem Value="3"> Acta No. 3 </asp:ListItem>
                    <asp:ListItem Value="4"> Acta No. 4 </asp:ListItem>
                    <asp:ListItem Value="5"> Acta No. 5 </asp:ListItem>
                </asp:DropDownList>
                <asp:Button ID="btnHabilitar" runat="server" Text="Habilitar" OnClick="btnHabilitar_Click" />
            
                 <asp:Label ID="lblDeshabilitar" runat="server" Text="Deshabilitar Acta:"></asp:Label>
                <asp:DropDownList ID="ddlDeshabilitarActa" runat="server">                    
                </asp:DropDownList>
                <asp:Button ID="btnDeshabilitar" runat="server" Text="Deshabilitar" OnClick="btnDeshabilitar_Click" />
            </div>
        </fieldset>

        <fieldset id="panelCargaArchivos" runat="server">
            <legend>Cargar Archivo</legend>
            <asp:FileUpload ID="Archivo" runat="server" />
            <div>
                <asp:Label ID="lblACtaSe" runat="server" Text="Seleccionar Acta:"></asp:Label>
                <asp:DropDownList ID="ddlActaACargar" runat="server"
                    DataValueField="idActa"
                    DataTextField="NomActa">
                </asp:DropDownList>
                <asp:Button ID="btnSubirArchivo" runat="server" Text="Cargar Archivo" OnClick="btnSubirArchivo_Click" />
                <br />
                <asp:Label ID="lblMensajeArchivo" runat="server" Text="Verifique que el nombre del archivo no lleve caracteres especiales, ni espacios, en caso de tenerlos reemplacelos por guion bajo (_)." ForeColor="Maroon"></asp:Label>
            </div>
        </fieldset>

        <fieldset id="panelActa1" runat="server">
            <legend>Archivos Acta 1</legend>
            <asp:GridView ID="gvArchivosActa1" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="idArchivoSeguimInterventoria"
                OnRowCommand="gvArchivosActa1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                Width="20px"
                                CommandName="Borrar" CommandArgument='<%# Eval("idArchivoSeguimInterventoria") %>'
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                OnClientClick="return alerta()"
                                ImageAlign="Left" />

                            <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                CommandName="VerArchivo" CausesValidation="False"
                                CommandArgument='<%#  Eval("nomArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivoSeguimInterventoria") %>'
                                ImageAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="nomArchivo" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha Carga" DataField="fechaCarga" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="contacto" HtmlEncode="false" />
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
        </fieldset>

        <fieldset id="panelActa2" runat="server">
            <legend>Archivos Acta 2</legend>
            <asp:GridView ID="gvArchivosActa2" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="idArchivoSeguimInterventoria"
                OnRowCommand="gvArchivosActa1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                Width="20px"
                                CommandName="Borrar" CommandArgument='<%# Eval("idArchivoSeguimInterventoria") %>'
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                OnClientClick="return alerta()"
                                ImageAlign="Left" />

                            <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                CommandName="VerArchivo" CausesValidation="False"
                                CommandArgument='<%#  Eval("nomArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivoSeguimInterventoria") %>'
                                ImageAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="nomArchivo" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha Carga" DataField="fechaCarga" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="contacto" HtmlEncode="false" />
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
        </fieldset>

        <fieldset id="panelActa3" runat="server">
            <legend>Archivos Acta 3</legend>
            <asp:GridView ID="gvArchivosActa3" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="idArchivoSeguimInterventoria"
                OnRowCommand="gvArchivosActa1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                Width="20px"
                                CommandName="Borrar" CommandArgument='<%# Eval("idArchivoSeguimInterventoria") %>'
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                OnClientClick="return alerta()"
                                ImageAlign="Left" />

                            <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                CommandName="VerArchivo" CausesValidation="False"
                                CommandArgument='<%#  Eval("nomArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivoSeguimInterventoria") %>'
                                ImageAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="nomArchivo" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha Carga" DataField="fechaCarga" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="contacto" HtmlEncode="false" />
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
        </fieldset>

        <fieldset id="panelActa4" runat="server">
            <legend>Archivos Acta 4</legend>
            <asp:GridView ID="gvArchivosActa4" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="idArchivoSeguimInterventoria"
                OnRowCommand="gvArchivosActa1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                Width="20px"
                                CommandName="Borrar" CommandArgument='<%# Eval("idArchivoSeguimInterventoria") %>'
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                OnClientClick="return alerta()"
                                ImageAlign="Left" />

                            <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                CommandName="VerArchivo" CausesValidation="False"
                                CommandArgument='<%#  Eval("nomArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivoSeguimInterventoria") %>'
                                ImageAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="nomArchivo" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha Carga" DataField="fechaCarga" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="contacto" HtmlEncode="false" />
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
        </fieldset>

        <fieldset id="panelActa5" runat="server">
            <legend>Archivos Acta 5</legend>
            <asp:GridView ID="gvArchivosActa5" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                EmptyDataText="No hay archivos que mostrar." Width="98%" BorderWidth="0" CellSpacing="1"
                CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left" DataKeyNames="idArchivoSeguimInterventoria"
                OnRowCommand="gvArchivosActa1_RowCommand">
                <Columns>
                    <asp:TemplateField HeaderText="Accion">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif"
                                Width="20px"
                                CommandName="Borrar" CommandArgument='<%# Eval("idArchivoSeguimInterventoria") %>'
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "mostrarEliminar")) %>'
                                OnClientClick="return alerta()"
                                ImageAlign="Left" />

                            <asp:ImageButton ID="imgBtnDescargar" runat="server" ImageUrl="~/Images/buscarrr.png" Width="20px"
                                CommandName="VerArchivo" CausesValidation="False"
                                CommandArgument='<%#  Eval("nomArchivo") + ";" + Eval("ruta")+ ";" + Eval("idArchivoSeguimInterventoria") %>'
                                ImageAlign="Right" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Nombre" DataField="nomArchivo" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Fecha Carga" DataField="fechaCarga" HtmlEncode="false" />
                    <asp:BoundField HeaderText="Cargado por" DataField="contacto" HtmlEncode="false" />
                </Columns>
                <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
            </asp:GridView>
        </fieldset>

    </form>
</body>
</html>
