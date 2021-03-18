﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuSolucion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Solucion.MenuSolucion" MaintainScrollPositionOnPostback="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //Solución
            $('#tc_proyectos_tabSolucion_tab').attr("onclick", "LoadIFrame('holderSolucion','../Solucion/Solucion.aspx')");
            //Ficha tecnica
            $('#tc_proyectos_tabFichaTecnica_tab').attr("onclick", "LoadIFrame('holderFichaTecnica','../Solucion/FichaTecnica.aspx')");

            LoadIFrame('holderSolucion', '../Solucion/Solucion.aspx');
        });
        function Recargar() {
            window.parent.location.reload();
        }
    </script>

</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <div>
            <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            <div>
                <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="1200px">
                    <ajaxToolkit:TabPanel ID="tabSolucion" OnDemandMode="Once" runat="server" Width="100%" Height="1200px">
                        <HeaderTemplate>
                            <div class="tab_header">
                                <span>1 - Solución</span>
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <iframe id="holderSolucion" marginwidth="0" marginheight="0" frameborder="0" 
                                scrolling="yes" width="100%" height="1200px"></iframe>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tabFichaTecnica" OnDemandMode="Once" runat="server" Width="1200px">
                        <HeaderTemplate>
                            <div class="tab_header">
                                <span> 2 - Ficha Técnica</span>
                            </div>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <iframe id="holderFichaTecnica" marginwidth="0" marginheight="0" frameborder="0" 
                                scrolling="yes" width="100%" height="1200px"></iframe>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>

                </ajaxToolkit:TabContainer>
            </div>
        </div>
    </form>
</body>
</html>
