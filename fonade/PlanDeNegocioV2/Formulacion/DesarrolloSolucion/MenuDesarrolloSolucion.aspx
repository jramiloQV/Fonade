<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuDesarrolloSolucion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.MenuDesarrolloSolucion" %>

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
            //Ingreso
            $('#tc_proyectos_tabIngreso_tab').attr("onclick", "LoadIFrame('holderIngreso','../DesarrolloSolucion/IngresosYCondicionesComerciales.aspx')");
            //Proyección
            $('#tc_proyectos_tabProyeccion_tab').attr("onclick", "LoadIFrame('holderProyeccion','../DesarrolloSolucion/Proyeccion.aspx')");
            //Normatividad
            $('#tc_proyectos_tabNormatividad_tab').attr("onclick", "LoadIFrame('holderNormatividad','../DesarrolloSolucion/NormatividadYCondicionesTecnicas.aspx')");
            //Requerimientos
            $('#tc_proyectos_tabRequerimientos_tab').attr("onclick", "LoadIFrame('holderRequerimientos','../DesarrolloSolucion/Requerimientos.aspx')");
            //Producción
            $('#tc_proyectos_tabProduccion_tab').attr("onclick", "LoadIFrame('holderProduccion','../DesarrolloSolucion/Produccion.aspx')");            
            //Productividad
            $('#tc_proyectos_tabProductividad_tab').attr("onclick", "LoadIFrame('holderProductividad','../DesarrolloSolucion/ProductividadYEquipoDeTrabajo.aspx')");

            LoadIFrame('holderIngreso', '../DesarrolloSolucion/IngresosYCondicionesComerciales.aspx');
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div>
            <ajaxToolkit:TabContainer ID="tc_proyectos" runat="server" ActiveTabIndex="0" Width="100%" Height="1200px">

                <ajaxToolkit:TabPanel ID="tabIngreso" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">  
                            <span>1 - Ingresos y Condiciones Comerciales</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderIngreso" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="yes" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabProyeccion" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">                        
                            <span>2 - Proyección</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderProyeccion" marginwidth="0" marginheight="0" 
                            frameborder="0" scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabNormatividad" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">                        
                            <span>3 - Normatividad y Condiciones Técnicas</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderNormatividad" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabRequerimientos" OnDemandMode="Once" runat="server" 
                    Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">                        
                            <span>4 - Requerimientos</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderRequerimientos" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>

                <ajaxToolkit:TabPanel ID="tabProduccion" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">                        
                            <span>5 - Producción</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderProduccion" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                
                <ajaxToolkit:TabPanel ID="tabProductividad" OnDemandMode="Once" runat="server" Width="100%" Height="1200px" >
                    <HeaderTemplate>
                        <div class="tab_header">                        
                            <span>6 - Productividad y Equipo de Trabajo</span>
                        </div>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="holderProductividad" marginwidth="0" marginheight="0" frameborder="0" 
                            scrolling="auto" width="100%" height="1200px"></iframe>
                    </ContentTemplate>
                </ajaxToolkit:TabPanel>
                                              
            </ajaxToolkit:TabContainer>
        </div>
    </div>
    </form>
</body>
</html>
