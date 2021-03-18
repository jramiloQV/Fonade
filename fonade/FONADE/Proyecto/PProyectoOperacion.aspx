<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoOperacion.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoOperacion" Debug="true" Strict="false" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .editorHTMLDisable {
            padding:20px;
        }
        /*.MsoNormal
        {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px !important;
        }
        .MsoNormalTable
        {
            margin: 6px 0px 4px 8px !important;
        }
        .MsoNormal + ol
        {
            padding: 5px 15px 0px 44px !important;
        }*/
        .parentContainer
        {
            width: 100%;
            height: 650px;
            overflow-x: hidden;
            overflow-y: visible;
        }
        .childContainer
        {
            width: 100%;
            height: auto;
        }
        html, body, div, iframe
        {
            /*height: 13% !important;*/
        }
    </style>
    <script type="text/javascript">
        window.onload = function () {
            Realizado();
        };

        function Realizado() {
            var chk = document.getElementById('chk_realizado')
            var rol = document.getElementById('txtIdGrupoUser').value;
            if (rol != '5') {
                if (chk.checked) {
                    chk.disabled = true;
                    document.getElementById('btn_guardar_ultima_actualizacion').setAttribute("hidden", 'true');
                }
            }
        }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
<%--    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>--%>
    <%--<div>
        <%= obtenerUltimaActualizacion(txtTab, codProyecto) %>
    </div>--%>
    <table>
        <tbody>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    ULTIMA ACTUALIZACIÓN:&nbsp;
                </td>
                <td>
                    <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                </td>
                <td style="width: 100px;">
                </td>
                <td>
                    <asp:CheckBox ID="chk_realizado" runat="server" Text="MARCAR COMO REALIZADO"  TextAlign="Left" 
                    Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                    <asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                        ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click"
                        Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <table id="tabla_docs" runat="server" visible="false" width="780" border="0" cellspacing="0"
        cellpadding="0">
        <tr>
            <td style="text-align:right">
                <table style="width:52px; border:0" align="right" >
                    <tr style="text-align:center;">
                        <td style="width: 50px;">
                            <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server"
                                ToolTip="Nuevo Documento" OnClick="ImageButton1_Click" />
                        </td>
                        <td style="width: 138px;">
                            <asp:ImageButton ID="ImageButton2" ImageUrl="../../Images/icoClip2.gif" runat="server"
                                ToolTip="Ver Documentos" OnClick="ImageButton2_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div>
        <table style="width: 100%">
            <tr >
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Ficha Técnica del producto o servicio',texto: 'FichaProducto'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_FichaProducto" />
                        </div>
                        <div>
                            Ficha Técnica del producto o servicio:
                        </div>
                    </div>
                </td>
                <td style="width: 50%">
                    <div id="div_post_it_1" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="FichaProducto" _txtTab="1" _mostrarPost="false"/>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <div id="panel_fichaProducto" class="editorHTMLDisable" runat="server">

            </div>
             <CKEditor:CKEditorControl  ID ="txt_fichaProducto" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" ></CKEditor:CKEditorControl>
          <%-- <asp:TextBox ID="txt_fichaProducto" class="editorHTML" runat="server" Width="100%"
                TextMode="MultiLine"></asp:TextBox>--%>
            <%--<ajaxToolkit:HtmlEditorExtender ID="txt_fichaProducto_HtmlEditorExtender" runat="server"
                TargetControlID="txt_fichaProducto" Enabled="true">
            </ajaxToolkit:HtmlEditorExtender>--%>
        </div>
        <table style="width: 100%">
            <tr >
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Estado de Desarrollo',texto: 'EstadoDesarrollo'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstadoDesarrollo" />
                        </div>
                        <div>
                            Estado de Desarrollo:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_post_it_2" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="EstadoDesarrollo" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <div id="panel_estadoDesarrollo" class="editorHTMLDisable" runat="server">
            </div>
             <CKEditor:CKEditorControl ID="txt_estadoDesarrollo" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--            <asp:TextBox ID="txt_estadoDesarrollo" class="editorHTML" runat="server" Width="100%"
                TextMode="MultiLine"></asp:TextBox>--%>
<%--            <ajaxToolkit:HtmlEditorExtender ID="txt_estadoDesarrollo_HtmlEditorExtender" runat="server"
                TargetControlID="txt_estadoDesarrollo" Enabled="true">
            </ajaxToolkit:HtmlEditorExtender>--%>
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Descripci&oacute;n Del Proceso',texto: 'DescripcionProceso'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_DescripcionProceso" />
                        </div>
                        <div>
                            Descripci&oacute;n Del Proceso:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_post_it_3" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="DescripcionProceso" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <div id="panel_descripcionProceso" class="editorHTMLDisable" runat="server">
            </div>
             <CKEditor:CKEditorControl ID="txt_descripcionProceso" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--            <asp:TextBox ID="txt_descripcionProceso" class="editorHTML" runat="server" Width="100%"
                TextMode="MultiLine"></asp:TextBox>--%>
<%--            <ajaxToolkit:HtmlEditorExtender ID="txt_descripcionProceso_HtmlEditorExtender" runat="server"
                TargetControlID="txt_descripcionProceso" Enabled="true">
            </ajaxToolkit:HtmlEditorExtender>--%>
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Necesidades y Requerimientos',texto: 'Necesidades'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Necesidades" />
                        </div>
                        <div>
                            Necesidades y Requerimientos:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_post_it_4" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It4" runat="server" _txtCampo="Necesidades" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <div id="panel_necesidades" class="editorHTMLDisable" runat="server">
            </div>
             <CKEditor:CKEditorControl ID="txt_necesidades" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--            <asp:TextBox ID="txt_necesidades" class="editorHTML" runat="server" Width="100%"
                TextMode="MultiLine"></asp:TextBox>--%>
<%--            <ajaxToolkit:HtmlEditorExtender ID="txt_necesidades_HtmlEditorExtender" runat="server"
                TargetControlID="txt_necesidades" Enabled="true">
            </ajaxToolkit:HtmlEditorExtender>--%>
        </div>
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: ' Plan De Producci&oacute;n',texto: 'PlanProduccion'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_PlanProduccion" />
                        </div>
                        <div>
                            Plan De Producci&oacute;n:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_post_it_5" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It5" runat="server" _txtCampo="PlanProduccion" _txtTab="1"/>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <div id="panel_planProduccion" class="editorHTMLDisable" runat="server">
            </div>
             <CKEditor:CKEditorControl ID="txt_planProduccion" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--            <asp:TextBox ID="txt_planProduccion" class="editorHTML" runat="server" Width="100%"
                TextMode="MultiLine"></asp:TextBox>--%>
<%--            <ajaxToolkit:HtmlEditorExtender ID="txt_planProduccion_HtmlEditorExtender" runat="server"
                TargetControlID="txt_planProduccion" Enabled="true">
            </ajaxToolkit:HtmlEditorExtender>--%>
        </div>
        <div>
            <asp:Button ID="btn_limpiarCampos" runat="server" Text="Limpiar Campos" OnClick="btn_limpiarCampos_Click" />
            <asp:Button ID="btm_guardarCambios" runat="server" Text="Guardar" OnClick="btm_guardarCambios_Click" />
        </div>
    </div>
    </form>
</body>
</html>
