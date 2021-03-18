<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportePuntajeEval.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.ReportePuntajeEval.ReportePuntajeEval" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Styles/jquery.dataTables.css" rel="stylesheet" />
    <link href="../../Styles/responsive.dataTables.min.css" rel="stylesheet" />
    <style type="text/css">
        a {
            cursor: pointer;
        }

        form {
            background-color: white !important;
            height: 100%;
        }
    </style>
    <script type="text/javascript" src="../../Scripts/jquery-1.11.1.min.js"></script>
    <script type="text/javascript" src="../../Scripts/bootstrap.min.js"></script>

    <script type="text/javascript" src="../../Scripts/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="../../Scripts/dataTables.responsive.min.js"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $('#gwReporte').prepend($("<thead></thead>").append($(this).find("tr:first")))
                .dataTable({
                    "bLengthChange": true,
                    "paging": true,
                    "scrollY": "400px",
                    "pageLength": 50,
                    "scrollCollapse": true,
                    "sPaginationType": "full_numbers",
                    "scrollX": true,
                    "jQueryUI": true,
                    "language": {
                        "lengthMenu": "Presentar _MENU_ registros por página",
                        "zeroRecords": "No existen registros para mostrar",
                        "info": "Presentando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                        "infoEmpty": "No existen coincidencias",
                        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                        "sSearch": "Buscar:",
                        "sInfoThousands": ",",
                        "sLoadingRecords": "Cargando...",
                        "oPaginate": {
                            "sFirst": "Primero ",
                            "sLast": " Último",
                            "sNext": " Siguiente ",
                            "sPrevious": " Anterior "
                        },
                        "oAria": {
                            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                        }
                    }
                });


        });
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <div>
            <br />
            <div style="text-align: center">
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700;" Text="Reporte de Puntaje de Evaluación" />
                </h1>
                <asp:Label ID="lblTituloConvocatoria" runat="server" Style="font-weight: 700;" />
            </div>
            <div>
                <asp:GridView ID="gwReporte" runat="server" Width="100%" AutoGenerateColumns="false" AllowPaging="false" 
                    CssClass="Grilla" AllowSorting="true">
                    <Columns>
                        <asp:BoundField HeaderText="Municipio" DataField="NomCiudad" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Unidad de Emprendimiento" DataField="NomUnidad" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="ID" DataField="codProyecto" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Plan de Negocio" DataField="NomProyecto" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Viable" DataField="Viable" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Valor Solicitado" DataField="MontoSolicitado" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Valor Recomendado" DataField="MontoRecomendado" HtmlEncode="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="A" DataField="A" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="B" DataField="B" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Quién es el protagonista?" DataField="TotalProtagonista" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="C" DataField="C" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="D" DataField="D" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="E" DataField="E" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="F" DataField="F" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="G" DataField="G" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="H" DataField="H" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="I" DataField="I" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="J" DataField="J" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="K" DataField="K" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BA" DataField="BA" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BB" DataField="BB" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Existe oportunidad en el mercado?" DataField="TotalOportunidad" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="L" DataField="L" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="M" DataField="M" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="N" DataField="N" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="O" DataField="O" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="P" DataField="P" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Q" DataField="Q" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="R" DataField="R" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="S" DataField="S" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="T" DataField="T" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="U" DataField="U" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="V" DataField="V" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BC" DataField="BC" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BD" DataField="BD" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BE" DataField="BE" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Cuál es mi solución?" DataField="TotalSolucion" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="W" DataField="W" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="X" DataField="X" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Y" DataField="Y" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Z" DataField="Z" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AA" DataField="AA" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AB" DataField="AB" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AC" DataField="AC" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AD" DataField="AD" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AE" DataField="AE" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AF" DataField="AF" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AG" DataField="AG" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AH" DataField="AH" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AI" DataField="AI" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AJ" DataField="AJ" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AK" DataField="AK" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AL" DataField="AL" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BF" DataField="BF" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BG" DataField="BG" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BH" DataField="BH" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BI" DataField="BI" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BJ" DataField="BJ" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Cómo desarrollo mi solución?" DataField="TotalDesarrollo" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="AM" DataField="AM" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AN" DataField="AN" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AO" DataField="AO" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AP" DataField="AP" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AQ" DataField="AQ" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AR" DataField="AR" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AS" DataField="AS" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AT" DataField="AT" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AU" DataField="AU" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Cuál es el futuro de mi negocio?" DataField="TotalFuturoNegocio" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="AV" DataField="AV" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AW" DataField="AW" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BK" DataField="BK" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BL" DataField="BL" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BM" DataField="BM" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="¿Qué riesgos enfrento?" DataField="TotalRiesgo" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="AX" DataField="AX" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AY" DataField="AY" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="AZ" DataField="AZ" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Resumen ejecutivo V1" DataField="VlrResumenLetras" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="BN" DataField="BN" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BO" DataField="BO" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="BP" DataField="BP" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField HeaderText="Resumen ejecutivo V2" DataField="VlrResumenNumeros" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                        <asp:BoundField HeaderText="Puntaje Total" DataField="PuntajeTotal" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" ItemStyle-Font-Bold="true" />
                    </Columns>
                </asp:GridView>
            </div>
            <br />
            <div style="text-align: right">
                <input type="button" value="Regresar" class="btn btn-success" onclick="history.back(-1)" />
            </div>
        </div>
    </form>
</body>
</html>
