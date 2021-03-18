<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoOrganizacionEstrategia.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoOrganizacionEstrategia" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
<%--    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .MsoNormal
        {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }
        .MsoNormalTable
        {
            margin: 6px 0px 4px 8px !important;
        }
        .editorHTMLDisable p
        {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
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
    <div>
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
                        <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" 
                            Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                        &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
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
                <td >
                    <table style="width:52px; border:0px;""  align="right"  >
                        <tr align="center">
                            <td style="width: 50px;">
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server" ToolTip="Nuevo Documento"
                                    OnClick="ImageButton1_Click"/>
                            </td>
                            <td style="width: 138px;">
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../../Images/icoClip2.gif" runat="server" ToolTip="Ver Documentos"
                                    OnClick="ImageButton2_Click"  />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Análisis DOFA', texto: 'AnalisisDOFA'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            Análisis DOFA:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It_1" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="AnalisisDOFA" _txtTab="1" _mostrarPost="false"/>
                    </div>
                </td>
            </tr>
        </table>
        <CKEditor:CKEditorControl ID="txt_analisis" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
        <%--<asp:TextBox ID="txt_analisis" class="editorHTML" runat="server" Width="100%" TextMode="MultiLine"></asp:TextBox>--%>
        <div id="panel_analisis" class="editorHTMLDisable" runat="server">
        </div>
<%--        <ajaxToolkit:HtmlEditorExtender ID="txt_analisis_HtmlEditorExtender" runat="server"
            TargetControlID="txt_analisis" Enabled="True" EnableSanitization="false">
        </ajaxToolkit:HtmlEditorExtender>--%>
        <br />
        <br />
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Organismos de Apoyo', texto: 'OrganismosApoyo'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Justificacion" />
                        </div>
                        <div>
                            Organismos de Apoyo:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It_2" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="OrganismosApoyo" _txtTab="1" _mostrarPost="false"/>
                    </div>
                </td>
            </tr>
        </table>
        <div id="panel_organismos" class="editorHTMLDisable" runat="server">
        </div>
         <CKEditor:CKEditorControl ID="txt_organismos" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--       <asp:TextBox ID="txt_organismos" runat="server" Width="100%" TextMode="MultiLine"
            CssClass="editorHTML"></asp:TextBox>--%>
<%--        <ajaxToolkit:HtmlEditorExtender ID="txt_organismos_HtmlEditorExtender" runat="server"
            TargetControlID="txt_organismos" Enabled="True" EnableSanitization="false">
        </ajaxToolkit:HtmlEditorExtender>--%>
        <br />
        <asp:Button ID="btm_guardarCambios" runat="server" Text="Guardar" OnClick="btm_guardarCambios_Click"
            Visible="false" />
    </div>
    </form>
</body>
</html>
