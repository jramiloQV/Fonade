<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoResumen.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoResumen" Async="true" EnableSessionState="True" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
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
<body style="overflow-x: hidden;">
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="PanelResumen" runat="server" Width="100%" UpdateMode="Conditional">
            <ContentTemplate>
                <table>
                    <tbody>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>ULTIMA ACTUALIZACIÓN:&nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                            </td>
                            <td style="width: 100px;"></td>
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
                        <td align="right">
                            <table width="52" border="0" cellspacing="0" cellpadding="0">
                                <tr align="center">
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
                <br />
                <table width="100%" border="0" bgcolor="White">
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Concepto del Negocio', texto: 'ConceptoNegocio'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Concepto del Negocio:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It1" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="ConceptoNegocio" _txtTab="1" _mostrarPost="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_concepto" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_concepto" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                        <asp:TextBox ID="txt_concepto" runat="server" Enabled="true" Width="100%" TextMode="MultiLine"
                            Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                        <ajaxToolkit:HtmlEditorExtender ID="txt_concepto_HtmlEditorExtender" runat="server"
                            TargetControlID="txt_concepto" Enabled="true" EnableSanitization="false">
                        </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" BackColor="White"
                                ControlToValidate="txt_concepto" ErrorMessage=" * Este campo está vacío" Display="Dynamic"
                                Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_concepto_vi" runat="server" visible="false">
                        <td>
                            <div id="div_concepto" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; overflow-x: hidden !important; padding: 15px;">
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Potencial del Mercado en Cifras', texto: 'PotencialMercados'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Potencial del Mercado en Cifras:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It2" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="PotencialMercados" _txtTab="1" _mostrarPost="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_potencial" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_potencial" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                           <asp:TextBox ID="txt_potencial" runat="server" Enabled="true" Width="100%" TextMode="MultiLine"
                                Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                            <ajaxToolkit:HtmlEditorExtender ID="txt_potencial_HtmlEditorExtender" runat="server"
                                TargetControlID="txt_potencial" Enabled="true" EnableSanitization="false">
                            </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" BackColor="White"
                                ControlToValidate="txt_potencial" ErrorMessage=" * Este campo está vacío" Display="Dynamic"
                                Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_potencial_vi" runat="server" visible="false">
                        <td>
                            <div id="div_potencial" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; overflow-x: hidden !important; padding: 15px;">
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Ventajas Competitivas y Propuesta de Valor', texto: 'VentajasCompetitivas'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Ventajas Competitivas y Propuesta de Valor:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It3" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="VentajasCompetitivas" _txtTab="1" _mostrarPost="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_ventajas" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_Ventajas" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                            <asp:TextBox ID="txt_Ventajas" runat="server" Enabled="true" Width="100%" TextMode="MultiLine"
                                Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                            <ajaxToolkit:HtmlEditorExtender ID="txt_Ventajas_HtmlEditorExtender" runat="server"
                                TargetControlID="txt_Ventajas" Enabled="true" EnableSanitization="false">
                            </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" BackColor="White"
                                ControlToValidate="txt_Ventajas" ErrorMessage=" * Este campo está vacío" Display="Dynamic"
                                Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_ventajas_vi" runat="server" visible="false">
                        <td>
                            <div id="div_ventajas" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; overflow-x: hidden !important; padding: 15px;">
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Resumen de las Inversiones Requeridas', texto: 'ResumenInversiones'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Resumen de las Inversiones Requeridas:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It4" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It4" runat="server" _txtCampo="ResumenInversiones" _txtTab="1" _mostrarPost="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_resumen" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_ResumenInversiones" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                            <asp:TextBox ID="txt_ResumenInversiones" runat="server" Enabled="true" Width="100%"
                                TextMode="MultiLine" Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                            <ajaxToolkit:HtmlEditorExtender ID="txt_ResumenInversiones_HtmlEditorExtender" runat="server"
                                TargetControlID="txt_ResumenInversiones" Enabled="true" EnableSanitization="false">
                            </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" BackColor="White"
                                ControlToValidate="txt_ResumenInversiones" ErrorMessage=" * Este campo está vacío"
                                Display="Dynamic" Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_resumen_vi" runat="server" visible="false">
                        <td>
                            <div id="div_resumen" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; overflow-x: hidden !important; padding: 15px;">
                                <!-- overflow-x: hidden !important; -->
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Proyecciones de Ventas y Rentabilidad', texto: 'Proyecciones'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Proyecciones de Ventas y Rentabilidad:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It5" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It5" runat="server" _txtCampo="Proyecciones" _txtTab="1" _mostrarPost="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_proyecciones" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_Proyecciones" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                            <asp:TextBox ID="txt_Proyecciones" runat="server" Enabled="true" Width="100%" TextMode="MultiLine"
                                Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                            <ajaxToolkit:HtmlEditorExtender ID="txt_Proyecciones_HtmlEditorExtender" runat="server"
                                TargetControlID="txt_Proyecciones" Enabled="true" EnableSanitization="false">
                            </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" BackColor="White"
                                ControlToValidate="txt_Proyecciones" ErrorMessage=" * Este campo está vacío"
                                Display="Dynamic" Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_proyecciones_vi" runat="server" visible="false">
                        <td>
                            <div id="div_proyecciones" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; padding: 15px;">
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 50%">
                                        <div class="help_container">
                                            <div onclick="textoAyuda({titulo: 'Conclusiones Financieras y Evaluación de Viabilidad', texto: 'ConclusionesFinancieras'});">
                                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                            </div>
                                            <div>
                                                &nbsp; Conclusiones Financieras y Evaluación de Viabilidad:
                                            </div>
                                        </div>
                                    </td>
                                    <td id="td_Post_It6" runat="server" visible="false">
                                        <uc1:Post_It ID="Post_It6" runat="server" _txtCampo="ConclusionesFinancieras" _txtTab="1" _mostrarPost="false"/>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="tr_conclusiones" runat="server">
                        <td>
                            <CKEditor:CKEditorControl ID="txt_Conclusiones" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor"></CKEditor:CKEditorControl>
                            <%--                            <asp:TextBox ID="txt_Conclusiones" runat="server" Enabled="true" Width="100%" TextMode="MultiLine"
                                Height="184px"></asp:TextBox>--%>
                            <br />
                            <%--                            <ajaxToolkit:HtmlEditorExtender ID="txt_Conclusiones_HtmlEditorExtender" runat="server"
                                TargetControlID="txt_Conclusiones" Enabled="true" EnableSanitization="false">
                            </ajaxToolkit:HtmlEditorExtender>--%>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" BackColor="White"
                                ControlToValidate="txt_Conclusiones" ErrorMessage=" * Este campo está vacío"
                                Display="Dynamic" Style="font-size: small; color: #FF0000;"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="tr_conclusiones_vi" runat="server" visible="false">
                        <td>
                            <div id="div_conclusiones" runat="server" visible="false" style="width: 840px; height: 200px; border-color: Black; border-style: solid; border-width: 2px; overflow: scroll; overflow-x: hidden !important; padding: 15px;">
                            </div>
                        </td>
                    </tr>
                    <%-- ------------------------------------------------------------------------------------------------- --%>
                    <tr>
                        <td>
                            <asp:Panel ID="PanelGuardar" runat="server" Visible="false" Width="95%" Height="55px" >
                                <table width="100%" style="height: 56px">
                                    <tr>
                                        <td class="style12">
                                            <asp:Button ID="btn_guardar" runat="server" OnClick="btn_guardar_Click" Text="Guardar"
                                                CausesValidation="true" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
