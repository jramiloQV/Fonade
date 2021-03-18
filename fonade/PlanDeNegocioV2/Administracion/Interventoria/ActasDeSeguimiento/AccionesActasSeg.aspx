<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="AccionesActasSeg.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.AccionesActasSeg" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>

    <div style="text-align: right; height: 0%;">
        <asp:Button ID="btnVolver" runat="server" Text="← volver"
            PostBackUrl="~/PlanDeNegocioV2/Administracion/Interventoria/ActasDeSeguimiento/AdministrarActasSeguimiento.aspx" />
    </div>


    <asp:Label ID="lblTitulo" runat="server"
        Text="Administrar Actas de Seguimiento Proyecto: " Font-Size="Large"></asp:Label>
    <hr />
    <div>
        <asp:GridView ID="gvMain" runat="server" AllowPaging="false" AutoGenerateColumns="False"
            EmptyDataText="No existen actas de seguimiento." Width="98%" BorderWidth="0"
            CellSpacing="1" CellPadding="4" CssClass="Grilla" HeaderStyle-HorizontalAlign="Left"
            DataKeyNames="idActa" OnRowCommand="gvMain_RowCommand"
            ShowHeaderWhenEmpty="true">
            <Columns>
                <asp:BoundField HeaderText="Interventor" DataField="nombreInterventor" HtmlEncode="false" />
                <asp:BoundField HeaderText="Acta" DataField="Acta" HtmlEncode="false" />
                <asp:BoundField HeaderText="Fecha Creacion" DataField="FechaCreacion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Ultima Actualizacion" DataField="FechaActualizacion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Fecha Publicacion" DataField="FechaPublicacion" HtmlEncode="false" />
                <asp:BoundField HeaderText="Publicada" DataField="Publicado" HtmlEncode="false" />
                <asp:TemplateField HeaderText="Accion">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk" CommandArgument='<%# Eval("idActa") %>'
                            CommandName="Despublicar" CausesValidation="False"
                            Text='Despublicar' runat="server"
                            Visible='<%# (bool)Eval("actaPublicada") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
        </asp:GridView>
    </div>

    <!--Modal-->

    <asp:Label ID="lblModalJusticicacion" runat="server" Text=""></asp:Label>
    <!--MODAL EDITAR COMPROMISO-->

    <asp:ModalPopupExtender ID="ModalJustificacion" runat="server"
        CancelControlID="btnCerrarModal"
        TargetControlID="lblModalJusticicacion" PopupControlID="pnlJustificacion"
        PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
    <!--Style="display: none"-->
    <asp:Panel ID="pnlJustificacion" runat="server"
        Width="550px" Height="330px" BackColor="White">
        <div class="EliminarArchivoPopup" style="overflow: auto;">
            <%--Popup--%>
            <div class="Controls" style="text-align: right; height: 0%;">
                <input type="submit" value="X" id="btnCerrarModal" style="margin: 0" />
            </div>
            <div class="PopupBody" style="max-height: 500px;">
                <input id="hdIdActa" type="hidden" runat="server" />
                <div id="cuerpoEliminarArchivo" style="height: 0%; padding-left: 20px;">

                    <h2>Despublicar Acta:
                        <asp:Label ID="lblNombreActa" runat="server" Text="Nombre Acta"></asp:Label></h2>
                    <hr />
                    <h3>Justificacion:</h3>

                    <asp:TextBox ID="txtJustificacion" runat="server"
                        TextMode="MultiLine" Style="width: 500px; margin: 0px; height: 120px;"></asp:TextBox>

                    <div style="text-align: center; height: 0%;">
                        <asp:Button ID="btnGuardarJustificacion" runat="server"
                            Text="Guardar Justificacion" OnClick="btnGuardarJustificacion_Click" />

                    </div>

                </div>
            </div>
        </div>
    </asp:Panel>

    <!--FIN MODAL-->

</asp:Content>
