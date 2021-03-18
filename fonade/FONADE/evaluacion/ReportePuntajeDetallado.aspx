<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="ReportePuntajeDetallado.aspx.cs" Inherits="Fonade.ReportePuntajeDetallado" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/bootstrap.min.css" rel="stylesheet" />
    
        <link href="../../Styles/jquery.dataTables.css" rel="stylesheet" />
    <link href="../../Styles/responsive.dataTables.min.css" rel="stylesheet" />

    <style type="text/css">
        .one-line {
            text-align:right left !important;
            white-space:nowrap;
        }
        .Grilla table tbody tr td {
            text-align:right;
            border-left:1px solid white !important;
        }
        .Grilla table tbody tr td.subtotal {
            font-weight:bold;
            border-left:1px solid white !important;
            font-size:16pt;
            text-align:right;
        }

        .paging_full_numbers span.paginate_button {
            background-color: #fff;
        }

            .paging_full_numbers span.paginate_button:hover {
                background-color: #ccc;
            }

        .paging_full_numbers span.paginate_active {
            background-color: #99B3FF;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <script type="text/javascript" src="../../Scripts/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/bootstrap.min.js"></script>
  
    <script type="text/javascript" src="../../Scripts/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../Scripts/dataTables.responsive.min.js"></script>


<script type="text/javascript">
    $(document).ready(function () {
        $('#tblReporte').dataTable({
            "bLengthChange": true,
            "paging": true,
            "scrollY": "400px",
            "pageLength": 50,
            "scrollCollapse": true,
            "sPaginationType": "full_numbers",                    //For Different Paging  Style
            "scrollX": true,                                     // For Scrolling
            "jQueryUI": true                                    //Enabling JQuery UI(User InterFace)
        });
    });           
    </script>

    <br />
    <div style="text-align: center">
        <h1>
            <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700" Text="Reporte de Puntaje de Evaluación" />
        </h1>
        <asp:Label ID="lblTituloConvocatoria" runat="server" Style="font-weight: 700" />
    </div>
    <br />
    <%--<asp:DataList runat="server" ID="DtlReporteDetallado" CssClass="Grilla" OnItemDataBound="DtlReporteDetallado_ItemDataBound">
            <HeaderTemplate>
                <table border='0' width="100%">
                    <tr>
                        <th>Municipio
                        </th>
                        <th>Unidad de Emprendimiento
                        </th>
                        <th>ID
                        </th>
                        <th>Plan de Negocio
                        </th>
                        <th>Viable
                        </th>
                        <th>Valor Solicitado
                        </th>
                        <th>Valor Recomendado
                        </th>
                        <th>A
                        </th>
                        <th>B
                        </th>
                        <th>C
                        </th>
                        <th>D
                        </th>
                        <th>E
                        </th>
                        <th>Generales
                        </th>
                        <th>F
                        </th>
                        <th>G
                        </th>
                        <th>H
                        </th>
                        <th>I
                        </th>
                        <th>J
                        </th>
                        <th>K
                        </th>
                        <th>L
                        </th>
                        <th>M
                        </th>
                        <th>N
                        </th>
                        <th>O
                        </th>
                        <th>P
                        </th>
                        <th>Q
                        </th>
                        <th>R
                        </th>
                        <th>S
                        </th>
                        <th>Comerciales
                        </th>
                        <th>T
                        </th>
                        <th>U
                        </th>
                        <th>V
                        </th>
                        <th>W
                        </th>
                        <th>X
                        </th>
                        <th>Y
                        </th>
                        <th>Técnicos
                        </th>
                        <th>Z
                        </th>
                        <th>AA
                        </th>
                        <th>AB
                        </th>
                        <th>AC
                        </th>
                        <th>AD
                        </th>
                        <th>AE
                        </th>
                        <th>AF
                        </th>
                        <th>Organizacionales
                        </th>
                        <th>AG
                        </th>
                        <th>AH
                        </th>
                        <th>AI
                        </th>
                        <th>AJ
                        </th>
                        <th>AK
                        </th>
                        <th>Financieros
                        </th>
                        <th>AL
                        </th>
                        <th>Medio Ambiente
                        </th>
                        <th>PuntajeTotal
                        </th>
                    </tr>
            </HeaderTemplate>
            <ItemTemplate>

                <tr>
                    <td class="one-line">
                        <asp:Label ID="lblciuadd" runat="server" Text=' <%#Eval("nomciudad")%>' />'
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblinstitucion" runat="server" Text='<%#Eval("nominstitucion")%>' />
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblidproyecto" runat="server" Text=' <%#Eval("Id_Proyecto")%>' />
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblnombreproyecto" runat="server" Text=' <%#Eval("nomproyecto")%>' />
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblviable" runat="server" Text=' <%#Eval("viable")%>' />
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblsolicitado" runat="server" Text=' <%#Eval("montosolicitado")%>' />
                    </td>
                    <td class="one-line">
                        <asp:Label ID="lblrecomendado" runat="server" Text=' <%#Eval("montorecomendado")%>' />
                    </td>
                    <!-- INCIO  DEL TABL GENERALES -->
                    <td>
                        <asp:Label ID="lblga" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblgb" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblgc" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblgd" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblge" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lbltotalG" runat="server" />
                    </td>
                    <!-- FIN GENERALES -->

                    <!-- INCIO COMERCIALES -->
                    <td>
                        <asp:Label ID="lblcc" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcg" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblch" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblci" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcj" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblck" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcl" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcm" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcn" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblco" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcp" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcq" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcr" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblcs" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lbltotalC" runat="server" />
                    </td>
                    <!-- FIN DEL TABL COMERCIALES -->
                    <!-- INCIO DEL TAB TECNICOS -->
                    <td>
                        <asp:Label ID="lbltt" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbltu" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbltv" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbltw" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbltx" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblty" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lblTotalT" runat="server" />
                    </td>
                    <!-- FIN DEL TECNICOS -->
                    <!--  INICIO TAB ORGANIZACIONALES -->
                    <td>
                        <asp:Label ID="lbloz" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbloaa" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbloab" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbloac" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbload" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbloae" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lbloaf" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lblTotalO" runat="server" />
                    </td>
                    <!--  FIN  ORGANIZACIONALES -->
                    <!--  INICIO  FINANCIEROS -->
                    <td>
                        <asp:Label ID="lblfag" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblfah" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblfai" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblfaj" runat="server" />
                    </td>
                    <td>
                        <asp:Label ID="lblfak" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lblTotalF" runat="server" />
                    </td>
                    <!--  FIN  FINANCIEROS -->
                    <td>
                        <asp:Label ID="lblmAL" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lblTotalM" runat="server" />
                    </td>
                    <td class="subtotal">
                        <asp:Label ID="lpuntajetotal" runat="server" />
                    </td>
                </tr>

            </ItemTemplate>
            <FooterTemplate>
                </table>

            </FooterTemplate>
        </asp:DataList>--%>

    <asp:Table ID="tblReporte" runat="server" ClientIDMode="Static" CssClass="Grilla table" >

    </asp:Table>
    <div style="text-align:right">
        <input type="button" value="Regresar" class="btn btn-success" onclick="history.back(-1)" />
    </div>


</asp:Content>
