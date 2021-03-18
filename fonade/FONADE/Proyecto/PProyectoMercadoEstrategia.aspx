<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoMercadoEstrategia.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoMercadoEstrategia" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html >
<html style="overflow-x: hidden;">
<head runat="server">
    <title> Estrategia de mercado </title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
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
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
       <table>
            <tbody>
                <tr>
                    <td> &nbsp; </td>
                    <td>ULTIMA ACTUALIZACIÓN: &nbsp; </td>
                    <td>
                        <asp:Label ID="lblUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" /> &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblFechaUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEsRealizado" Text="MARCAR COMO REALIZADO: &nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" /> &nbsp;
                        <asp:Button ID="btnUpdateTab" Text="Guardar" runat="server" ToolTip="Guardar" OnClick="btnUpdateTab_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowCheckTab")) %>' />
                    </td>
                </tr>
            </tbody>
        </table>
        <table id="tabla_docs" runat="server" width="780" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td style="text-align: right;">     
                    <table style="width: 52px; border-collapse: separate; border-spacing: 0px; border-collapse: collapse; border-spacing: 0;" align="right">
                        <tr style="text-align: center;">
                            <td style="width: 50px;">
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server" ToolTip="Nuevo Documento" Visible ='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'
                                    OnClick="ImageButton1_Click" />
                            </td>
                            <td style="width: 138px;">
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../../Images/icoClip2.gif" runat="server" ToolTip="Ver Documentos" OnClick="ImageButton2_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Concepto del Producto o Servicio', texto: 'ConceptoProducto'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_ConceptoProducto">
                            </div>
                            <div>
                                Concepto del Producto o Servicio:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_1" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="ConceptoProducto" _txtTab="1" _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                    </td>
                </tr>
            </table>
            <CKEditor:CKEditorControl ID="txtConcepto" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' ></CKEditor:CKEditorControl>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Distribución', texto: 'EstrategiasDistribucion'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasDistribucion">
                            </div>
                            <div>
                                Estrategias de Distribución:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_2" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="EstrategiasDistribucion" _txtTab="1" 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                          
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiasDistribucion" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl> 
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Precio', texto: 'EstrategiasPrecio'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasPrecio">
                            </div>
                            <div>
                                Estrategias de Precio:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_3" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="EstrategiasPrecio" _txtTab="1" 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                            
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiasPrecio" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' ></CKEditor:CKEditorControl>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Promoción', texto: 'EstrategiasPromocion'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasPromocion">
                            </div>
                            <div>
                                Estrategias de Promoción:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_4" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It4" runat="server" _txtCampo="EstrategiasPromocion" _txtTab="1" 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                            
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiaPromocion" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>
            
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Comunicación', texto: 'EstrategiasComunicacion'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasComunicacion">
                            </div>
                            <div>
                                Estrategias de Comunicación:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_5" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It5" runat="server" _txtCampo="EstrategiasComunicacion" _txtTab="1"
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                            
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiaComunicacion" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>            
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Servicio', texto: 'EstrategiasServicio'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasServicio">
                            </div>
                            <div>
                                Estrategias de Servicio:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_6" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It6" runat="server" _txtCampo="EstrategiasServicio" _txtTab="1"
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                          
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiaServicio" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Presupuesto de la Mezcla de Mercadeo', texto: 'PresupuestoMercado'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_PresupuestoMercado">
                            </div>
                            <div>
                                Presupuesto de la Mezcla de Mercadeo:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_7" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It7" runat="server" _txtCampo="PresupuestoMercado" _txtTab="1" 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                            
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtPresupuestoMercadeo" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>
                       
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Estrategias de Aprovisionamiento', texto: 'EstrategiasAprovisionamiento'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento">
                            </div>
                            <div>
                                Estrategias de Aprovisionamiento:
                            </div>
                        </div>
                    </td>
                    <td>
                        <div id="div_post_it_8" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It8" runat="server" _txtCampo="EstrategiasAprovisionamiento" _txtTab="1" 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>                            
                        </div>
                    </td>
                </tr>
            </table>            
            <CKEditor:CKEditorControl ID="txtEstrategiaAprovisionamiento" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>            

            <asp:Button ID="btnLimpiarCampos" runat="server" Text="Limpiar Campos" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClick="btnLimpiarCampos_Click" />
            <asp:Button ID="btnSave" runat="server" Text="Guardar" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' OnClick="btm_guardarCambios_Click" />
        </div>
    </form>
</body>
</html>
