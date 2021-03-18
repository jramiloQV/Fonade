<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoOperacionInfraestructura.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoOperacionInfraestructura" ValidateRequest="false" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .editorHTMLDisable p {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
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
 <%--   <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </ajaxToolkit:ToolkitScriptManager>--%>
    <%-- Inicio ayuda infraestructura--%>
    <br />
    <%--<div>
        <%= obtenerUltimaActualizacion(txtTab, codProyecto) %>
    </div>--%>
    <table>
        <tbody>
            <tr style="text-align:center">
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
                    <%--<asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" 
                        Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                    &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                        ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" 
                        Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />--%>
                    <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" 
                            Enabled='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this,"vldt")??false) %>' />
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
    <%--inicio WAFS 12-OCT-2014 --%>
    <table id="tabla_docs" runat="server" visible="false" width="780" border="0" cellspacing="0"
        cellpadding="0">
        <tr>
            <td style="text-align:right">
                <table style=" width:52px; border:0;" align="right" >
                    <tr style="text-align:center">
                        <td style="width: 50px;">
                            <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server"
                                ToolTip="Nuevo Documento" OnClick="ImageButton1_Click" style="height: 23px" />
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
    <%--FIN WAFS 12-OCT-2014 --%>

    <br />
    <table style="width: 100%">
        <tr>
            <td style="width: 50%">
                <div class="help_container">
                    <div>
                        <a onclick="textoAyuda({titulo: 'Infraestructura', texto: 'Infraestructura'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura"></img>
                        </a>
                    </div>
                    &nbsp;<div>
                        Infraestructura
                    </div>
                </div>
            </td>
            <td>
                <div id="div_Post_It1" runat="server" visible="false">
                    <%--<uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Infraestructura" _txtTab="1" 
                        Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "ejecucion")) %>' />--%>
                    <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Infraestructura" _txtTab="1" _mostrarPost="false"/>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <div runat="server" id="divcrear" visible="false">
        <%--<img id="Adicionar" src="../../Images/icoAdicionarUsuario.gif" style="cursor: pointer;"
            codigo='<%= codProyecto %>' alt="Adicionar" alternatetext="Adicionar" />--%>
        <asp:ImageButton ID="imgBtn_Adicionar" ImageUrl="../../Images/icoAdicionarUsuario.gif"
            runat="server" AlternateText="Adicionar" OnClick="imgBtn_Adicionar_Click"
            Visible='false' />
        &nbsp;<asp:LinkButton ID="lnk_Adicionar" Text="Adicionar Infraestructura" runat="server"
            OnClick="lnk_Adicionar_Click" 
            Visible='false' />
    </div>
    <br />
    <div>
        <asp:Panel ID="pnl_tabla_infraestructura" runat="server" />
    </div>
    <br />
    <div>
        <%-- Inicio Parámetros Técnicos Especiales--%>
        <table style="width: 100%">
            <tr>
                <td style="width: 50%">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Parámetros Técnicos Especiales',texto: 'ParametrosTecnicos'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_ParametrosTecnicos" />&nbsp;Parámetros
                            Técnicos Especiales:
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It2" runat="server" visible="false">
                        <%--<uc1:Post_It ID="Post_It2" runat="server" _txtCampo="ParametrosTecnicos" _txtTab="1"
                            Visible='<%# (bool)DataBinder.GetPropertyValue(this, "ejecucion") %>' />--%>
                        <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="ParametrosTecnicos" _txtTab="1" _mostrarPost="false"/>
                    </div>
                </td>
            </tr>
        </table>
        <CKEditor:CKEditorControl ID="txt_parametrosT" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
<%--        <asp:TextBox ID="txt_parametrosT" class="editorHTML" runat="server" Width="95%" Height="250px"
            TextMode="MultiLine"></asp:TextBox>--%>
<%--        <ajaxToolkit:HtmlEditorExtender ID="txt_parametrosT_HtmlEditorExtender" runat="server"
            TargetControlID="txt_parametrosT" Enabled="True">
        </ajaxToolkit:HtmlEditorExtender>--%>
        <div id="div_param" runat="server" visible="false" style="width: 95%; height: 250px;
            overflow: scroll; border-color: Black; border-style: solid; border-width: 2px;">
        </div>
        <%-- Fin ayuda Parámetros Técnicos Especiales--%>
        <%-- Fin ayuda infraestructura--%>
        <br />
        <asp:Button ID="btm_guardarCambios" runat="server" Text="Guardar" OnClick="btm_guardarCambios_Click"
            Visible="False" />
        <br />
        <br />
    </div>
    </form>
</body>
</html>
