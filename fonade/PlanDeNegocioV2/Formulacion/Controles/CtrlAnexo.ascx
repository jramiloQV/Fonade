<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CtrlAnexo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.CtrlAnexo" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        function ValidarTamano() {
            var uploadControl = document.getElementById('<%= Archivo.ClientID %>');
            if (uploadControl.files[0].size > 10485760) {
                alert('El tamaño del archivo debe ser menor a 10 MB');
                uploadControl.value = "";
                return false;
            }
        }
    </script>
    <style>
        .panelPopup {
            display: block;
            background: white;
            height: auto;
            width: auto;
            vertical-align: middle;
        }

        .tamlabel {
            float: left;
            width: 30%;
        }

        .tamlabel1 {
            float: left;
            width: 40%;
        }

        .tamCtrl {
            width: 60%;
        }
    </style>
</head>
<body>
    <% Page.DataBind(); %>
    <asp:Panel ID="pnl_datos" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" Visible='<%#((bool)DataBinder.GetPropertyValue(this, "EsNuevo"))%>'>
        <h3 style="text-align: center">
            <asp:Label ID="lblTitulo" Text="NUEVO DOCUMENTO" runat="server" /></h3>
        <br />
        <br />
        <label for="NomDocumento" class="tamlabel">&nbsp;<b>Nombre:</b></label>
        &nbsp;<asp:TextBox ID="NomDocumento" runat="server" MaxLength="256" />
        <br />
        <br />
        <label for="Archivo" class="tamlabel">&nbsp;<b>Subir Archivo:</b></label>
        &nbsp;<asp:FileUpload ID="Archivo" runat="server" />
        <br />
        <br />
        <label for="txtNomCargo" class="tamlabel">&nbsp;<b>Comentario:</b></label>
        &nbsp;<asp:TextBox ID="Comentario" runat="server" Columns="50" Rows="5" TextMode="MultiLine" />
        <br />
        <br />
        <div style="text-align: center">
            <asp:HiddenField ID="hddIdDocumento" runat="server" />
            <asp:Button ID="btn_Accion" Text="Crear" runat="server" OnClick="btn_Accion_Click" OnClientClick="return ValidarTamano();"/>
            <asp:Button ID="Btn_cerrar" Text="Cerrar" runat="server" OnClick="Btn_cerrar_Click"/>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnl_Grilla" runat="server" CssClass="panelPopup" Width="99%" BorderColor="#00468F" BorderStyle="Solid" BorderWidth="5px" Visible='<%#!((bool)DataBinder.GetPropertyValue(this, "EsNuevo"))%>'>
        <h3 style="text-align: center">
            <asp:Label ID="lblGrilla" Text="DOCUMENTOS" runat="server" /></h3>
        <br />
        <br />
        <asp:GridView ID="gv_Documentos" runat="server" AutoGenerateColumns="false" Width="98%"
            CssClass="Grilla" ShowHeaderWhenEmpty="true"
            RowStyle-HorizontalAlign="Left" OnRowCommand="gv_Documentos_RowCommand" DataKeyNames="url">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_eliminar" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id_Documento") %>'
                            CommandName="Borrar" ToolTip="Eliminar el documento del proyecto" Style="text-decoration: none;" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' OnClientClick="return Confirmacion('¿Está seguro que desea borrar el documento seleccionado?')">
                            <asp:Image ID="img_borrar" ImageUrl="/Images/icoBorrar.gif" runat="server" />
                        </asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <asp:HiddenField ID="hdf_icono" runat="server" Value='<%# Eval("icono") %>' />
                            <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("URL") %>'>
                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# "~/Images/"+ Eval("icono") %>' />
                            </asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <asp:Button ID="btnEditar" runat="server" CommandName="Editar" CommandArgument='<%# Eval("id_Documento") %>'
                                Text='<%# Eval("NombreDocumento") %>' Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' CssClass="boton_Link" />
                            <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("NombreDocumento") %>' Visible='<%# (!(bool)DataBinder.GetPropertyValue(this, "HabilitaBoton"))%>' />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Fecha">
                    <ItemTemplate>
                        <div style="word-wrap: break-word;">
                            <asp:Label ID="lbl_Fecha" Text='<%# Eval("fecha") %>' runat="server" />
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
            <RowStyle HorizontalAlign="Left" />
        </asp:GridView>
        <br />
        <br />
        <asp:GridView ID="gw_DocumentosAcreditacion" runat="server" Width="100%" AutoGenerateColumns="false"
            CssClass="Grilla" DataKeyNames="Id_Documento"
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
                        <asp:LinkButton ID="hlnk_url" runat="server" CausesValidation="false" CommandName="VerDocumento" CommandArgument='<%# Eval("URL") %>'>
                            <asp:Image ID="Image1" runat="server" ImageUrl="/Images/IcoDocNormal.gif" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="tipo" HeaderText="Tipo" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Left" />
            </Columns>
        </asp:GridView>
        <br />
        <br />
        <input id="ActiveClose" type="hidden" value="Si decide abandonará la página, puede perder los cambios si no ha GRABADO ¡¡¡" runat="server" />
    </asp:Panel>
</body>
</html>
