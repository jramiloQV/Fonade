<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoOperacionCompras.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoOperacionCompras" %>

<%@ Register src="../../Controles/Post_It.ascx" tagname="Post_It" tagprefix="uc1" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
<%--    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>--%>
    <style type="text/css">
        .style11
        {
            height: 30px;
        }
        .style12
        {
            height: 22px;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelResumen" runat="server" Width="100%" UpdateMode="Conditional">
    <ContentTemplate>

    <table border="0" width="100%" style="background-color:White">

		      <tr>
			    <td>
                    <table style="width:100%">
                    <tr>
                        <td style="width:50%">
                    <div class="help_container">
                    <div onclick="textoAyuda({titulo: 'Resumen Ejecutivo', texto: 'ResumenEjecutivo'});">
                    <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                    </div>
                    <div>
                        &nbsp; <strong>Resumen Ejecutivo:</strong>
                    </div>
                    </div>
                            </td>
                        <td>
                            <div id="div_Post_It_1" runat="server" visible="false">
                                <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Insumos" _txtTab="1" _mostrarPost="false" />
                            </div>
                        </td>
                    </tr>
                </table>
                </td>
		      </tr>
              <tr>
			    <td>
                    
                   <%-- <asp:Button ID="btn_addInsumo" runat="server" 
                        Text="Adicionar Insumo al producto o servicio" CssClass="boton_Link" OnClick="btn_addInsumo_Click" 
                         />--%>
                    
                </td>
		      </tr>
     </table>

    </ContentTemplate>
    </asp:UpdatePanel>

        <div id="divAddInsumo" runat="server">

        </div>
    </form>
</body>
</html>
