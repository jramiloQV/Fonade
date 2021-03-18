<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionObservaciones.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionObservaciones" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsEspecificos.js"></script>
    <title>FONDO EMPRENDER</title>
</head>
<body>
    <form id="form1" runat="server">    
    <table style="width: 99%;">
        <tbody>
            <tr>
                <td>
                    ULTIMA ACTUALIZACIÓN:&nbsp;
                </td>
                <td>
                    <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                </td>
                <td style="width: 20px;">
                </td>
                <td>
                    <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                        runat="server" TextAlign="Left" />
                    &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                        ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="false" />
                </td>
            </tr>
        </tbody>
    </table>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" onclick="textoAyuda({titulo: 'Actividades a las que se dedicara la empresa', texto: 'Actividades'});">
                                Actividades a las que se dedicará la empresa: </img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It" runat="server">
                        <uc1:Post_It runat="server" ID="Post_It" _txtCampo="Actividades" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>
        <asp:TextBox ID="Actividades" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Productos y servicios que ofrecerá', texto: 'ProductosServicios'});">
                                Productos y servicios que ofrecerá: </img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It0" runat="server">
                        <uc1:Post_It ID="Post_It0" runat="server" _txtCampo="ProductosServicios" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>       
        <asp:TextBox ID="ProductosServicios" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Canales de distribución, estrategias de mercado', texto: 'EstrategiaMercado'});">
                                Canales de distribución, estrategias de mercado: </img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It1" runat="server">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="EstrategiaMercado" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>       
        <asp:TextBox ID="EstrategiaMercado" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Proceso de Producción', texto: 'ProcesoProduccion'});">
                                Proceso de Producción:</img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It2" runat="server">
                        <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="ProcesoProduccion" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>        
        <asp:TextBox ID="ProcesoProduccion" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Análisis estructura organizacional', texto: 'EstructuraOrganizacionalEval'});">
                                Análisis estructura organizacional:</img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It3" runat="server">
                        <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="EstructuraOrganizacionalEval"
                            _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>        
        <asp:TextBox ID="EstructuraOrganizacionalEval" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Análisis Tamaño propuesto y localización', texto: 'TamanioLocalizacion'});">
                                Análisis Tamaño propuesto y localización:</img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It4" runat="server">
                        <uc1:Post_It ID="Post_It4" runat="server" _txtCampo="TamanioLocalizacion" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>        
        <asp:TextBox ID="TamanioLocalizacion" runat="server" CssClass="actividades"
            TextMode="MultiLine" Height="146px" Width="100%" />
    </div>
    <br />
    <div style="width: 99%">
        <table style="width: 100%">
            <tr>
                <td style="width: 70%">
                    <div class="help_container">
                        <div>
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" onclick="textoAyuda({titulo: 'Resumen concepto General - Compromisos y Condiciones', texto: 'Generales'});">
                                Resumen concepto General - Compromisos y Condiciones:</img>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It5" runat="server">
                        <uc1:Post_It ID="Post_It5" runat="server" _txtCampo="Generales" _txtTab="1" />
                    </div>
                </td>
            </tr>
        </table>        
        <asp:TextBox ID="Generales" runat="server" CssClass="actividades" TextMode="MultiLine"
            Height="146px" Width="100%" />
    </div>
    <br />
    <div align="center">
        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="Boton" OnClick="btnGuardar_Click"
            Visible="False"></asp:Button>
    </div>
    <br />
    </form>    
</body>
</html>
