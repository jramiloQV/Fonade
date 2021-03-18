<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActaPDF.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Templates.ActaPDF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="http://www.fondoemprender.com:8080/styles/estilos_generales.css" rel="stylesheet" type="text/css" />
    <style>
        table.specialTable, table.specialTable th, table.specialTable td {
            border: 1px solid gray;
            border-collapse: collapse;
        }

            table.specialTable th, table.specialTable td {
                padding: 5px;
                text-align: left;
                font-size: 12px;
            }

        .centered {
            text-align: center !important;
        }

        .gray {
            color: gray;
        }

        .bold {
            font-weight: bold;
        }

        .verticalSpace {
            margin-top: 5px;
            margin-bottom: 5px;
        }

        .tabla_anexo_2 {
            padding-left: 50px;
            padding-right: 50px;
        }

        page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
        }

            page[size="A4"] {
                width: 21cm;
                height: 29.7cm; /*29.7cm;*/
            }

                page[size="A4"][layout="portrait"] {
                    width: 29.7cm;
                    height: 21cm;
                }

        @media print {
            body, page {
                margin: 0;
                box-shadow: 0;
            }

            /*@page {
                margin: 0;
                size: auto;
            }*/
        }

        div.footer {
            display: block;
            text-align: center;
        }
    </style>
    <style type="text/css">
        .auto-style4 {
            width: 577px;
            text-align: center;
        }

        .borderTabla {
            border: 1px solid gray;
            border-collapse: collapse;
        }

        .tamanImagen {
            width: 80px;
            height: 90px;
        }

        .centrar {
            text-align: center;
        }

        .colorgris {
            background-color: lightgray;
        }

        .auto-style5 {
            width: 107px;
        }

        .auto-style6 {
            width: 226px;
            border: 1px solid gray;
            border-collapse: collapse;
            background-color: lightgray;
        }
        .auto-style7 {
            width: 89px;
        }
        .auto-style8 {
            width: 420px;
        }
        .auto-style9 {
            width: 422px;
        }
        .auto-style10 {
            width: 421px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <page size="A4" style="page-break-after: always;">
        <!--Encabezado-->
         <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgInterventorPAG1" runat="server" 
                                ImageUrl="" 
                               class="tamanImagen"></asp:Image>                                
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
        <!--Fin Encabezado-->
        <!--Informacion Actas Pagina #1-->
            <hr />
            <table style="width:100%;" class="borderTabla" id="InfoActa">
                <tr>
                    <td class="auto-style6" ><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">ACTA No:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblNumActa" runat="server" Text="NumActa"></asp:Label>
                    </td>

                </tr>
                <tr>
                    <td class="auto-style6 "><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">FECHA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblFechaActa" runat="server" Text="FechaActa"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6 "><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">CONTRATO No:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblNumContrato" runat="server" Text="NumContrato"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6 "><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">FECHA DE ACTA DE INICIO:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblFechaActaInicio" runat="server" Text="FechaActaInicio"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6 "><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">PRÓRROGA (MESES):</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblProrroga" runat="server" Text="Prorroga"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"> <b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">ID Y NOMBRE DEL PLAN DE NEGOCIOS:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblNomPlanNegocio" runat="server" Text="NomPlanNegocio"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">NOMBRE DE LA EMPRESA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblNomEmpresa" runat="server" Text="NomEmpresa"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                                mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                                ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">NIT DE LA EMPRESA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblNitEmpresa" runat="server" Text="NitEmpresa"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                    mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                    ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">CONTRATO MARCO INTERADMINISTRATIVO:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblContratoMarcoAdmin" runat="server" Text="ContratoAdmin"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">CONTRATO DE INTERVENTORIA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblContratoInterventoria" runat="server" Text="ContratoInterventoria"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                            mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                            ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">CONTRATISTA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblContratista" runat="server" Text="Contratista"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">VALOR APROBADO:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblValorAprobado" runat="server" Text="ValorAprobado"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                    mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                    ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">DOMICILIO PRINCIPAL:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblDomPrincipal" runat="server" Text="DomicilioPrincipal"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                    mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                    ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">CONVOCATORIA / CORTE:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblConvocatoria" runat="server" Text="Convocatoria"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">SECTOR ECONOMICO:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblSector" runat="server" Text="Sector"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">OBJETO DEL PROYECTO:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblObjetoProyecto" runat="server" Text="ObjetoProyecto"></asp:Label>
                    </td>

                </tr>

                <tr>
                    <td class="auto-style6"><b style="mso-bidi-font-weight:normal">
                        <span style="font-size:10.0pt;line-height:107%;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;;
                        mso-fareast-font-family:Calibri;mso-fareast-theme-font:minor-latin;mso-ansi-language:
                        ES-CO;mso-fareast-language:EN-US;mso-bidi-language:AR-SA">OBJETIVO VISITA:</span></b>

                    </td>
                    <td class="borderTabla">
                        <asp:Label ID="lblObjetoVisita" runat="server" Text="ObjetoVisita"></asp:Label>
                    </td>

                </tr>

            </table>

        <h5>1.	VERIFICACIÓN DE INDICADORES Y METAS</h5>
        <div style="text-align:center">
        <asp:Table ID="tblMetas" runat="server" Width="100%"
            CssClass="borderTabla">
            <asp:TableRow ID="trTitulos" runat="server" BackColor="Silver" 
                BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell ID="tdVisita" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                <asp:TableCell ID="tdEmpleo" runat="server" BorderStyle="Solid" BorderWidth="1px">EMPLEO</asp:TableCell>
                <asp:TableCell ID="tdEjecucion" runat="server" BorderStyle="Solid" BorderWidth="1px">EJECUCION PRESUPUESTAL</asp:TableCell>
                <asp:TableCell ID="tdMercadeo" runat="server" BorderStyle="Solid" BorderWidth="1px">MERCADEO</asp:TableCell>
                <asp:TableCell ID="tdIDH" runat="server" BorderStyle="Solid" BorderWidth="1px">IDH</asp:TableCell>
                <asp:TableCell ID="tdContrapartidas" runat="server" BorderStyle="Solid" BorderWidth="1px">CONTRAPARTIDAS</asp:TableCell>
                <asp:TableCell ID="tdProduccion" runat="server" BorderStyle="Solid" BorderWidth="1px">PRODUCCION</asp:TableCell>
                <asp:TableCell ID="tdVenta" runat="server" BorderStyle="Solid" BorderWidth="1px">VENTA</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </div>
        
    </page>

        <!--FIN Pagina 1-->
        <!--Pagina 2-->

        <page size="A4" style="page-break-after: always;">
        <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgInterventorPAG2" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
        <!--Fin Encabezado-->
        <!--Cuerpo Pagina-->
         <h5>2.	RIESGOS IDENTIFICADOS EN EVALUACIÓN</h5>
        <div style="text-align:center">
        <asp:Table ID="tbRiegosIdentificados" runat="server"  Width="100%"
            CssClass="borderTabla">
            <asp:TableRow ID="trRiesgosIdentificados" runat="server" BackColor="Silver" 
                BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell ID="tcNo" runat="server" BorderStyle="Solid" BorderWidth="1px">No</asp:TableCell>
                <asp:TableCell ID="tcDescripcion" runat="server" BorderStyle="Solid" BorderWidth="1px">Descripción del Riesgo</asp:TableCell>
                <asp:TableCell ID="tcMitigacion" runat="server" BorderStyle="Solid" BorderWidth="1px">Mitigación</asp:TableCell>
                <asp:TableCell ID="tcGestion" runat="server" BorderStyle="Solid" BorderWidth="1px">Gestión del Emprendedor</asp:TableCell>               
            </asp:TableRow>
        </asp:Table>
        </div>

        <h5>3.	CUMPLIMIENTO DE INDICADORES DE GESTIÓN</h5>
        <h6>3.1	Gestión en la Generación de Empleo </h6>
        <h6 style ="text-align:center">METAS</h6>
        <div style="text-align:center">
        <asp:Table ID="tbMetaEmpleos" runat="server" Width="100%"
            CssClass="borderTabla">
            <asp:TableRow ID="trMetaEmpleos" runat="server" BackColor="Silver" 
                BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell ID="tdCantidad" runat="server" BorderStyle="Solid" BorderWidth="1px">Cantidad</asp:TableCell>
                <asp:TableCell ID="tdCargo" runat="server" BorderStyle="Solid" BorderWidth="1px">Cargo</asp:TableCell>
                <asp:TableCell ID="tdCondicion" runat="server" BorderStyle="Solid" BorderWidth="1px">Condición</asp:TableCell>                
            </asp:TableRow>
        </asp:Table>
            <h6 style ="text-align:center">
                <asp:Label ID="lblMetasEmpleos" runat="server" Text="">
                </asp:Label></h6>
            <p style="text-align:justify">Los empleos serán homologados de conformidad con la normatividad vigente del Fondo Emprender. 
                La empresa contrató las siguientes personas:</p>

            <asp:Table ID="tbIndicadorEmpleo" runat="server" Width="100%"
            CssClass="borderTabla">
            <asp:TableRow ID="TableRow1" runat="server" BackColor="Silver" 
                BorderStyle="Solid" BorderWidth="1px">
                <asp:TableCell ID="tdVisitaEmpleo" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                <asp:TableCell ID="tdIndicador" runat="server" BorderStyle="Solid" BorderWidth="1px">VERIFICACION DEL INDICADOR</asp:TableCell>
                <asp:TableCell ID="tdDesarrollo" runat="server" BorderStyle="Solid" BorderWidth="1px">DESARROLLO DEL INDICADOR</asp:TableCell>                
            </asp:TableRow>
       </asp:Table>     

        </div><!--Cuerpo Pagina-->
    </page>

        <!--FIN Pagina 2-->
        <!--Pagina 3-->

        <page size="A4" style="page-break-after: always;">
    <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPagina3" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
      <!--Cuerpo Pagina-->
        <h6>3.2	Gestión en la Ejecución Presupuestal</h6>
        
        <div>
            VISITA N°1: Se le asignaron recursos por 
            <asp:Label ID="lblValorPesos" runat="server" Text="< $Valor en pesos N SML >"></asp:Label>
            equivalentes a
            <asp:Label ID="lblSMLV" runat="server" Text="< N SMLV >"></asp:Label>
            SMLV año base <asp:Label ID="lblAnoSLMV" runat="server" Text="< N SMLV >"></asp:Label>, 
            la interventoría validó el valor asignado con el contrato de cooperación empresarial y plataforma.
        </div>
            
            <asp:Panel ID="pnlGestionEjePresupuestal" runat="server">
                <div style="text-align:center">
                <asp:Table ID="tbEjecuPresupuestal" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow2" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell1" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell2" runat="server" BorderStyle="Solid" BorderWidth="1px">Id.</asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server" BorderStyle="Solid" BorderWidth="1px">Actividad</asp:TableCell>                
                        <asp:TableCell ID="TableCell4" runat="server" BorderStyle="Solid" BorderWidth="1px">Valor</asp:TableCell>                
                        <asp:TableCell ID="TableCell5" runat="server" BorderStyle="Solid" BorderWidth="1px">Concepto</asp:TableCell>                
                        <asp:TableCell ID="TableCell6" runat="server" BorderStyle="Solid" BorderWidth="1px">Indicar si verificó documentos originales</asp:TableCell>                
                        <asp:TableCell ID="TableCell7" runat="server" BorderStyle="Solid" BorderWidth="1px">Indicar si verificó físicamente los activos y en qué estado</asp:TableCell>                
                        <asp:TableCell ID="TableCell8" runat="server" BorderStyle="Solid" BorderWidth="1px">Observación</asp:TableCell>                
                    </asp:TableRow>
               </asp:Table>  
                    </div>
                <p>TOTAL DESEMBOLSADO <asp:Label ID="lblTotalDesembolsado" runat="server" Text=""></asp:Label></p>
            </asp:Panel>
      <!--FIN Cuerpo Pagina-->

    </page>

        <!--FIN Pagina 3-->
        <!--Pagina 4-->

        <page size="A4" style="page-break-after: always;">
         <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag4" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
     <!--Cuerpo Pagina-->
        <h6>3.2.1 Inventarios y Contrato de Garantía.</h6>
         <div>
            <asp:Panel ID="pnlInfoInventario" runat="server">
                <div style="padding-top: 20px;">
                   VISITA N°1: Inventario por Prendar y Verificar
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlGestionInventario" runat="server">
               <p>La emprendedora cumplió con la firma del contrato de garantía mobiliaria, 
                   el cual fue enviado a FONADE con los respectivos anexos a favor del 
                   SENA de los siguientes bienes adquiridos con los recursos del Fondo Emprender:</p>
                <div style="text-align:center">
                 <asp:Table ID="tbInventario" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow3" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell9" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell10" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN RECURSOS FINANCIADOS CON RECURSOS FONDO EMPRENDER</asp:TableCell>
                        <asp:TableCell ID="TableCell11" runat="server" BorderStyle="Solid" BorderWidth="1px">VALOR ACTIVOS PRENDABLES (INCLUYE IMPUESTOS)</asp:TableCell>                
                        <asp:TableCell ID="TableCell12" runat="server" BorderStyle="Solid" BorderWidth="1px">FECHA DE CARGA DEL ANEXO A PLATAFORMA</asp:TableCell>                                        
                    </asp:TableRow>                     
                 </asp:Table>  
                </div>
                <p><asp:Label ID="lblActivosPrendables" runat="server" Text=""></asp:Label></p>

            </asp:Panel>
        </div>
      <!--Cuerpo Pagina-->
    </page>

        <!--FIN Pagina 4-->
        <!--Pagina 5-->

        <page size="A4" style="page-break-after: always;">
        <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag5" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
         <!--Cuerpo Pagina-->
        <h6>3.2.2 Aportes del emprendedor.</h6>
           <p>META: <asp:Label ID="lblMetaAporteEmp" runat="server" Text=""></asp:Label></p>

         <div style="text-align:center">
                 <asp:Table ID="tbAporteEmp" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow4" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell13" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell14" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
            </div>

         <h6>3.3	Gestión en Mercadeo</h6>
         
         <div style="text-align:center">
                 <asp:Table ID="tbMetaGestMercadeo" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow5" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell15" runat="server" BorderStyle="Solid" BorderWidth="1px">Cantidad</asp:TableCell>
                        <asp:TableCell ID="TableCell16" runat="server" BorderStyle="Solid" BorderWidth="1px">Descripción</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             <p><asp:Label ID="lblMetaMercadeo" runat="server" Text=""></asp:Label></p>
             <asp:Table ID="tbGestMercadeo" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow6" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell17" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell18" runat="server" BorderStyle="Solid" BorderWidth="1px">CANTIDAD</asp:TableCell>                        
                        <asp:TableCell ID="TableCell19" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN DEL EVENTO</asp:TableCell>
                        <asp:TableCell ID="TableCell20" runat="server" BorderStyle="Solid" BorderWidth="1px">PUBLICIDAD DE LOGOS</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
            </div>

          <h6>3.4	Contrapartidas</h6>
         <div style="text-align:center">
         <asp:Table ID="tbContrapartidas" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow7" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell21" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell22" runat="server" BorderStyle="Solid" BorderWidth="1px">CANTIDAD CONTRAPARTIDAS</asp:TableCell>                        
                        <asp:TableCell ID="TableCell23" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <!--Cuerpo Pagina-->
      </page>

        <!--FIN Pagina 5-->
        <!--Pagina 6-->

        <page size="A4" style="page-break-after: always;">
          <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag6" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
        <h6>3.5	Gestión en Producción.</h6>
         <div style="text-align:center">
         <asp:Table ID="tbMetaProduccion" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow8" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell24" runat="server" BorderStyle="Solid" BorderWidth="1px">META</asp:TableCell>
                        <asp:TableCell ID="TableCell25" runat="server" BorderStyle="Solid" BorderWidth="1px">CANTIDAD</asp:TableCell>                        
                        <asp:TableCell ID="TableCell26" runat="server" BorderStyle="Solid" BorderWidth="1px">PRODUCTO O SERVICIO</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             <br />
           <asp:Table ID="tbGestionProduccion" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow9" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell30" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell27" runat="server" BorderStyle="Solid" BorderWidth="1px">CANTIDAD</asp:TableCell>
                        <asp:TableCell ID="TableCell28" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                                                
                    </asp:TableRow>                     
                 </asp:Table>    

             </div>

         <h6>3.6	Gestión en Ventas</h6>
         <p>META VENTAS: <asp:Label ID="lblMetaVentas" runat="server" Text="0"></asp:Label></p>
         <div style="text-align:center">
         <asp:Table ID="tbGestionVentas" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow10" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell29" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell31" runat="server" BorderStyle="Solid" BorderWidth="1px">VALOR</asp:TableCell>                        
                        <asp:TableCell ID="TableCell32" runat="server" BorderStyle="Solid" BorderWidth="1px">PRODUCTO O SERVICIO</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>


         <!--Cuerpo Pagina-->
    </page>

        <!--FIN Pagina 6-->
        <!--Pagina 7-->

        <page size="A4" style="page-break-after: always;">

         <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag7" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
        <h5>4.	VERIFICAR OBLIGACIONES TIPICAS PARA LOS COMERCIANTES</h5>
         <h6>4.1	VERIFICAR EL CUMPLIMIENTO DE LAS OBLIGACIONES CONTABLES.</h6>

         <div style="text-align:center">
         <asp:Table ID="tbcumplObligContable" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow11" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell33" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell34" runat="server" BorderStyle="Solid" BorderWidth="1px">Estados Financieros</asp:TableCell>                        
                        <asp:TableCell ID="TableCell35" runat="server" BorderStyle="Solid" BorderWidth="1px">Libros oficiales</asp:TableCell>
                        <asp:TableCell ID="TableCell36" runat="server" BorderStyle="Solid" BorderWidth="1px">Libros de Contabilidad</asp:TableCell>
                        <asp:TableCell ID="TableCell37" runat="server" BorderStyle="Solid" BorderWidth="1px">Conciliaciones bancarias</asp:TableCell>
                        <asp:TableCell ID="TableCell38" runat="server" BorderStyle="Solid" BorderWidth="1px">Cuenta Bancaria</asp:TableCell>
                        <asp:TableCell ID="TableCell39" runat="server" BorderStyle="Solid" BorderWidth="1px">OBSERVACIONES</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>

         <h6>4.2	VERIFICAR EL CUMPLIMIENTO DE LAS OBLIGACIONES TRIBUTARIAS.</h6>
         <div style="text-align:center">
         <asp:Table ID="tbCumpObligTributaria" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow12" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell40" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell41" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Retención en la Fuente</asp:TableCell>                        
                        <asp:TableCell ID="TableCell42" runat="server" BorderStyle="Solid" BorderWidth="1px">Autorrete. Renta</asp:TableCell>
                        <asp:TableCell ID="TableCell43" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de IVA</asp:TableCell>
                        <asp:TableCell ID="TableCell44" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Impuesto Nacional al Consumo</asp:TableCell>
                        <asp:TableCell ID="TableCell45" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Renta</asp:TableCell>
                        <asp:TableCell ID="TableCell46" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Información Exógena</asp:TableCell>
                        <asp:TableCell ID="TableCell47" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Industria y Comercio</asp:TableCell>
                        <asp:TableCell ID="TableCell48" runat="server" BorderStyle="Solid" BorderWidth="1px">Declar. de Retención impuesto de Industria y Comercio</asp:TableCell>
                        <asp:TableCell ID="TableCell49" runat="server" BorderStyle="Solid" BorderWidth="1px">OBSERV.</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>


         <h6>4.3	VERIFICAR EL CUMPLIMIENTO DE LAS OBLIGACIONES LABORALES.</h6>
         <div style="text-align:center">
         <asp:Table ID="tbCumpObligLaborales" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow13" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell50" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell51" runat="server" BorderStyle="Solid" BorderWidth="1px">Contratos laborales</asp:TableCell>                        
                        <asp:TableCell ID="TableCell52" runat="server" BorderStyle="Solid" BorderWidth="1px">Pagos de nómina</asp:TableCell>
                        <asp:TableCell ID="TableCell53" runat="server" BorderStyle="Solid" BorderWidth="1px">Afiliación la seguridad social</asp:TableCell>
                        <asp:TableCell ID="TableCell54" runat="server" BorderStyle="Solid" BorderWidth="1px">Pagos a la seguridad social</asp:TableCell>
                        <asp:TableCell ID="TableCell55" runat="server" BorderStyle="Solid" BorderWidth="1px">Certificado de paz y salvo de parafiscales y seguridad social</asp:TableCell>
                        <asp:TableCell ID="TableCell56" runat="server" BorderStyle="Solid" BorderWidth="1px">Reglamento interno de trabajo</asp:TableCell>
                        <asp:TableCell ID="TableCell57" runat="server" BorderStyle="Solid" BorderWidth="1px">Sistema de Gestión de la Seguridad y Salud en el Trabajo</asp:TableCell>                        
                        <asp:TableCell ID="TableCell59" runat="server" BorderStyle="Solid" BorderWidth="1px">OBSERVACIONES</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
          <!--Cuerpo Pagina-->
    </page>

        <!--FIN Pagina 7-->
        <!--Pagina 8-->

        <page size="A4" style="page-break-after: always;">
           <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag8" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
         <h6>4.4	VERIFICAR REGISTROS, TRÁMITES Y LICENCIAS</h6>
         <div style="text-align:center">
         <asp:Table ID="tbVerificaRegTramites" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow14" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell58" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell60" runat="server" BorderStyle="Solid" BorderWidth="1px">Inscrip. Cámara de C/cio</asp:TableCell>                        
                        <asp:TableCell ID="TableCell61" runat="server" BorderStyle="Solid" BorderWidth="1px">Renovación de reg. mercantil</asp:TableCell>
                        <asp:TableCell ID="TableCell62" runat="server" BorderStyle="Solid" BorderWidth="1px">RUT</asp:TableCell>
                        <asp:TableCell ID="TableCell63" runat="server" BorderStyle="Solid" BorderWidth="1px">Resol. Facturación</asp:TableCell>
                        <asp:TableCell ID="TableCell64" runat="server" BorderStyle="Solid" BorderWidth="1px">Cert. de libertad y tradición</asp:TableCell>
                        <asp:TableCell ID="TableCell65" runat="server" BorderStyle="Solid" BorderWidth="1px">Aval Unidad de Emprendi. del predio</asp:TableCell>
                        <asp:TableCell ID="TableCell66" runat="server" BorderStyle="Solid" BorderWidth="1px">Permiso uso de suelo</asp:TableCell>                        
                        <asp:TableCell ID="TableCell68" runat="server" BorderStyle="Solid" BorderWidth="1px">Cert. de Bomberos</asp:TableCell>  
                        <asp:TableCell ID="TableCell69" runat="server" BorderStyle="Solid" BorderWidth="1px">Reg. de marca</asp:TableCell>  
                        <asp:TableCell ID="TableCell70" runat="server" BorderStyle="Solid" BorderWidth="1px">Otros Permisos</asp:TableCell>  
                        <asp:TableCell ID="TableCell67" runat="server" BorderStyle="Solid" BorderWidth="1px">OBSERV.</asp:TableCell>
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <h5>5.	OTROS ASPECTOS DEL PLAN DE NEGOCIOS.</h5>
         <h6>5.1 Componente Innovador.</h6>
         <p style="text-align:justify"><asp:Label ID="lblCompInnovador" runat="server" Text=""></asp:Label></p>
          <div style="text-align:center">
         <asp:Table ID="tbCompInnovador" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow15" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell71" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell72" runat="server" BorderStyle="Solid" BorderWidth="1px">SI/NO/PARCIAL</asp:TableCell>                        
                        <asp:TableCell ID="TableCell73" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>

          <h6>5.2 Componente Ambiental.</h6>
         <p style="text-align:justify"><asp:Label ID="lblCompAmbiental" runat="server" Text=""></asp:Label></p>
          <div style="text-align:center">
         <asp:Table ID="tbCompAmbiental" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow16" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell74" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell75" runat="server" BorderStyle="Solid" BorderWidth="1px">SI/NO/PARCIAL</asp:TableCell>                        
                        <asp:TableCell ID="TableCell76" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
          <!--Cuerpo Pagina-->
    </page>

        <!--FIN Pagina 8-->
        <!--Pagina 9-->

        <page size="A4" style="page-break-after: always;">
           <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag9" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
         <h5>6.	OTRAS OBLIGACIONES CONTRACTUALES.</h5>
         <h6>6.1 Reporte de Información en Plataforma.</h6>         
          <div style="text-align:center">
         <asp:Table ID="tbRepInfoPlataforma" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow17" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell77" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell78" runat="server" BorderStyle="Solid" BorderWidth="1px">SI/NO/PARCIAL</asp:TableCell>                        
                        <asp:TableCell ID="TableCell79" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <h6>6.2 Tiempo de Dedicación del emprendedor.</h6>         
          <div style="text-align:center">
         <asp:Table ID="tbTiempoDedicaEmp" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow18" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell80" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell81" runat="server" BorderStyle="Solid" BorderWidth="1px">SI/NO</asp:TableCell>                        
                        <asp:TableCell ID="TableCell82" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
          <h6>6.3 Acompañamiento y Asesoría de la Unidad de Emprendimiento.</h6>         
          <div style="text-align:center">
         <asp:Table ID="tbAcomUnidadEmp" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow19" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell83" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell84" runat="server" BorderStyle="Solid" BorderWidth="1px">SI/NO</asp:TableCell>                        
                        <asp:TableCell ID="TableCell85" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <h5>7.	ESTADO DE LA EMPRESA</h5>         
          <div style="text-align:center">
         <asp:Table ID="tbEstadoEmpresa" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow20" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell86" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell87" runat="server" BorderStyle="Solid" BorderWidth="1px">DESCRIPCIÓN</asp:TableCell>                                                
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <!--Cuerpo Pagina-->         
    </page>

        <!--FIN Pagina 9-->
        <!--Pagina 10-->

        <page size="A4" style="page-break-after: always;">
           <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag10" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
         <h5>8.	COMPROMISOS Y REQUERIMIENTOS DE LA INTERVENTORIA</h5>         
          <div style="text-align:center">
         <asp:Table ID="tbCompromisos" runat="server" Width="100%" CssClass="borderTabla">
                    <asp:TableRow ID="TableRow21" runat="server" BackColor="Silver" 
                        BorderStyle="Solid" BorderWidth="1px">
                        <asp:TableCell ID="TableCell88" runat="server" BorderStyle="Solid" BorderWidth="1px">VISITA</asp:TableCell>
                        <asp:TableCell ID="TableCell89" runat="server" BorderStyle="Solid" BorderWidth="1px">COMPROMISOS</asp:TableCell>
                        <asp:TableCell ID="TableCell90" runat="server" BorderStyle="Solid" BorderWidth="1px">FECHA PROPUESTA PARA LA EJECUCIÓN DEL COMPROMISO</asp:TableCell>
                        <asp:TableCell ID="TableCell93" runat="server" BorderStyle="Solid" BorderWidth="1px">ESTADO</asp:TableCell>
                        <asp:TableCell ID="TableCell91" runat="server" BorderStyle="Solid" BorderWidth="1px">FECHA DE CUMPLIMIENTO DE COMPROMISO</asp:TableCell>
                        <asp:TableCell ID="TableCell92" runat="server" BorderStyle="Solid" BorderWidth="1px">OBSERVACION</asp:TableCell>                        
                    </asp:TableRow>                     
                 </asp:Table>  
             </div>
         <!--Cuerpo Pagina-->
         </page>

        <!--FIN Pagina 10-->
        <!--Pagina 11-->

        <page size="A4" style="page-break-after: always;">
           <!--Encabezado-->
        <table style="width: 100%;" class="borderTabla" id="Encabezado">
                    <tr class="borderTabla">
                        <td class="auto-style5 centrar borderTabla" rowspan="2">
                            <img alt="" class="tamanImagen"
                                src="../../../../../Images/Img/logoFonade.png" /></td>
                        <td class="auto-style4 borderTabla" >
                            <strong>ACTA DE SEGUIMIENTO DE CONTRATO</strong></td>
                        <td rowspan="2" class="borderTabla centrar auto-style5">
                            <asp:Image ID="imgPag11" runat="server" 
                                ImageUrl="../../../../../Images/Img/logoFonade.png" 
                               class="tamanImagen"></asp:Image>
                            </td>
                    </tr>
                    <tr class="borderTabla">
                        <td class="auto-style4 borderTabla">
                            <strong>DOCUMENTO DE INTERVENTORIA FONDO EMPRENDER</strong></td>
                    </tr>
             </table>
      <!--Fin Encabezado-->
          <!--Cuerpo Pagina-->
         <h5>9.	RESOLVER DUDAS DEL EMPRENDEDOR.</h5>  
         <p style="text-align:justify">Se resolvieron las dudas al contratista y se le recordó nuevamente 
             que siempre deberá estar al día con las obligaciones contractuales y empresariales. 
             Debe reportar oportunamente la información solicitada por la interventoría
             , de acuerdo a los plazos informados en el Acta de Agenda de Reunión de la 
             primera visita de interventoría.</p>
         <p style="text-align:justify; border:solid; border-width:1px">Para efectos legales y en constancia de lo tratado 
             en la reunión se suscribe por quienes en ella participaron aceptando que el 
             contenido de la misma es claro y preciso y corresponde con lo evidenciado en la visita.</p>

         <div id="datosContratista" runat="server"></div>
            <table style="width:100%;" class="borderTabla">
                <tr>
                    <td class="auto-style7"><strong>NOMBRE:</strong></td>
                    <td class="auto-style9">
                        <asp:Label ID="lblNombreGestorOperativo" runat="server" Text="GestorOperativo"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Rol:</td>
                    <td class="auto-style9">Gestor operativo-administrativo SENA</td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Correo:</td>
                    <td class="auto-style9">
                        <asp:Label ID="lblCorreoGesOperativo" runat="server" Text="CorreoGesOpera"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>

                <tr>
                    <td class="auto-style7">Teléfono:</td>
                    <td class="auto-style9">
                        <asp:Label ID="lblTelefonoGesOpera" runat="server" Text="telefonoGesOpera"></asp:Label>
                    </td>
                    <td class="centered">Firma</td>

                </tr>

            </table>
            <table style="width:100%;" class="borderTabla">
                <tr>
                    <td class="auto-style7"><strong>NOMBRE:</strong></td>
                    <td class="auto-style10">
                        <asp:Label ID="lblNombreGestorTecnico" runat="server" Text="GestorTecnico"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Rol:</td>
                    <td class="auto-style10">Gestor técnico SENA</td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Correo:</td>
                    <td class="auto-style10">
                        <asp:Label ID="lblCorreoGesTecnico" runat="server" Text="CorreoGesTecnico"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>

                <tr>
                    <td class="auto-style7">Teléfono:</td>
                    <td class="auto-style10">
                        <asp:Label ID="lblTelefonoGesTecnico" runat="server" Text="telefonoGesTecnico"></asp:Label>
                    </td>
                    <td class="centered">Firma</td>

                </tr>

            </table>
            <table style="width:100%;" class="borderTabla">
                <tr>
                    <td class="auto-style7"><strong>NOMBRE:</strong></td>
                    <td class="auto-style8">
                        <asp:Label ID="lblNombreInterventor" runat="server" Text="Interventor"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Rol:</td>
                    <td class="auto-style8">
                        Interventor Universidad de <asp:Label ID="lblInterventorUniversidad" runat="server" Text="Universidad"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>
                <tr>
                    <td class="auto-style7">Correo:</td>
                    <td class="auto-style8">
                        <asp:Label ID="lblCorreoInterventor" runat="server" Text="CorreoInterventor"></asp:Label>
                    </td>
                    <td>&nbsp;</td>

                </tr>

                <tr>
                    <td class="auto-style7">Teléfono:</td>
                    <td class="auto-style8">
                        <asp:Label ID="lblTelefonoInterventor" runat="server" Text="telefonoInterventor"></asp:Label>
                    </td>
                    <td class="centered">Firma</td>

                </tr>

            </table>

          <p style="text-align:justify">“De acuerdo con la Interventoría realizada, 
              las partes firman la presente acta dejando constancia que en ella reposan 
              las evidencias encontradas por la Interventoría durante la visita, 
              la cual termina a las 
              <asp:Label ID="lblHoraPublicacionActa" runat="server" Text="xx:xx (am/pm)"></asp:Label> 
              del <asp:Label ID="lblFechaPublicacionActa" runat="server" Text="DD/MM/AAAA"></asp:Label>”</p>

         <p style="text-align:right">Original: FONADE</p>
         <p style="text-align:right">Copia: SENA</p>
         <p style="text-align:right">Copia: contratista</p>

         <!--Cuerpo Pagina-->
         </page>

        <!--FIN Pagina 11-->
    </form>

</body>
</html>
<script language="javascript">
    window.print();
</script>
