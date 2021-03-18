<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ReportesBI.aspx.cs" Inherits="Fonade.FONADE.Reportes.ReportesBI" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width:100%;
             text-align:left;
             height: 59px;
         }

         td {
             width:50%;
         }

         .calass {
             text-decoration:none;
             cursor:pointer;
             color:black;
         }
     </style>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="principal" runat="server" Height="375px">
        
        <h1>
            <asp:Label ID="L_titulo" runat="server" Text="">
            </asp:Label>
        </h1>
        <br />
        <br />
        INFORMES DE INTELIGENCIA DE NEGOCIOS<br /> 
        <br />
        <table>
            <tr>
                <td>
                    <asp:HyperLink id="id_Cierre_Convocatoria" NavigateUrl="http://190.131.233.21:90/Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fcierresConvocatoria" 
                        Text="1. Cierre de Convocatoria" Target="_new" runat="server"/>  
                 <!--   <asp:HyperLink id="id_Cierre_Convocatoria2" NavigateUrl="http://10.3.3.111:90//Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fcierresConvocatoria" 
                        Text="1. Cierre de Convocatoria" Target="_new" runat="server"/>-->
                    <br />
                    <br />
                </td>
                <td>
                   <asp:HyperLink id="id_Evaluacion" NavigateUrl="http://190.131.233.21:90/Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fObservacionesEvaluacion"
                        Text="2. Observación Evaluación" Target="_new" runat="server"/>  
                    <!-- <asp:HyperLink id="id_Evaluacion2" NavigateUrl="http://10.3.3.111:90//Reports/Pages/Report.aspx?ItemPath=%2fmesa%2fObservacionesEvaluacion"
                        Text="2. Observación Evaluación" Target="_new" runat="server"/> -->
                    <br />
                    <br />
                    <!--<asp:HyperLink id="id_Acreditacion" NavigateUrl="" Text="2. Acreditación" Target="_new" runat="server"/>
                    <br />
                    <br />-->
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HyperLink id="id_Tarea_Usuario" NavigateUrl="http://190.131.233.21:90/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f4+Tarea+Usuario"
                         Text="3. Tarea Usuario" Target="_new" runat="server"/>  
                   <!--  <asp:HyperLink id="id_Tarea_Usuario2" NavigateUrl="http://10.3.3.111:90//Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f4+Tarea+Usuario"
                         Text="3. Tarea Usuario" Target="_new" runat="server"/> -->
                    <br />
                    <br />
                </td>
                <td>
                    <asp:HyperLink id="id_Respuesta_Evaluacion_Emprendedor" NavigateUrl="http://190.131.233.21:90/Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f5+Respuesta+Evaluacion+Emprendedores"
                       Text="4. Respuesta Evaluación Emprendedores" Target="_new" runat="server"/>  
                  <!--  <asp:HyperLink id="HyperLink12" NavigateUrl="http://10.3.3.111:90//Reports/Pages/Report.aspx?ItemPath=%2fFonadeDWH%2f5+Respuesta+Evaluacion+Emprendedores"
                       Text="4. Respuesta Evaluación Emprendedores" Target="_new" runat="server"/> -->
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>
                    <!--<asp:HyperLink id="id_Viabilidad" NavigateUrl="" Text="6. Viabilidad" Target="_new" runat="server"/>  
                    <br />
                    <br />-->
                </td>
            </tr>
            <tr>
                <td>
                    <!--<asp:HyperLink id="id_Empleo_Generado" NavigateUrl="" Text="7. Empleos Generados" Target="_new" runat="server"/>  
                    <br />
                    <br />-->
                </td>
                <td>
                    <!--<asp:HyperLink id="ID_Indicador_Gestion" NavigateUrl="" Text="8. Indicadores de Gestion" Target="_new" runat="server"/>   
                    <br />
                    <br />-->
                </td>
            </tr>
            <tr>
                <td>
                    <!--<asp:HyperLink id="Id_Pagos_Detallados" NavigateUrl="" Text="9. Pagos Detallados" Target="_new" runat="server"/> 
                    <br />
                    <br />-->
                </td>
                <td>
                    <!--<asp:HyperLink id="id_Reporte_Contratos" NavigateUrl="" Text="10. Reporte Contratos" Target="_new" runat="server"/>   
                    <br />
                    <br />-->
                </td>
            </tr>
            <tr>
                <td>
                    <!--<asp:HyperLink id="id_Pagos_Detalle_Proyecto" NavigateUrl="" Text="11. Pagos Detallados Proyecto" Target="_new" runat="server"/> 
                    <br />
                    <br />-->
                </td>
                <td>
                    <!--<asp:HyperLink id="id_Asesores_Regional" NavigateUrl="" Text="12. Asesores Regional" Target="_new" runat="server"/>   
                    <br />
                    <br />-->
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>