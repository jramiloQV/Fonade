<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoPptalAportes.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.SeguimientoPptalAportes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <!--<script src="../../Scripts/jquery-1.9.1.js"></script>-->
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <style type="text/css">
        html,body{
            background-image: none !important;
        }
        table
        {
            width: 100%;
        }
        .table-Width{
            width:20% !important;
            table-layout: fixed !important;
        }
        table.Grilla th{
            text-align: center;
        }
        table.Grilla th:nth-child(2){
            text-align: center;
            width: 200px;
        }
        table.Grilla th:nth-child(3){
            text-align: center;
            width: 300px;
        }
        table.Grilla tr:nth-child(1n) td:nth-child(4),td:nth-child(5),td:nth-child(6),td:nth-child(7){
            text-align: right !important;
        }
        table#tfinnal tr:nth-child(3){
            display: none;
        }
        table#tfinnal tr:nth-child(4) td:nth-child(2){
            text-align: center;
        }
        .help{
            width: 100%;
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="ContentInfo" style="width: 100%; height: 100%;">
        <div class="help">

        </div>
        <div style="width: 100%;">
            <table>
                <tr>
                    <td colspan="2">
                        <div class="help" onclick="textoAyuda({titulo: 'Aportes', texto: 'Aportes'});">
                            <span class="image_help"><img alt="help_Objetivos" border="0" src="../../Images/imgAyuda.gif" /></span>
                            <span class="text_help">&nbsp;Aportes</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Panel ID="p_aprotes" runat="server" Width="100%">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Table ID="tfinnal" runat="server" class="Grilla">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell RowSpan="2"></asp:TableHeaderCell>
                                <asp:TableHeaderCell RowSpan="2">Integrantes de la iniciativa empresarial</asp:TableHeaderCell>
                                <asp:TableHeaderCell ColumnSpan="2">Tipo</asp:TableHeaderCell>
                                <asp:TableHeaderCell ColumnSpan="4">Valor Aporte (miles de pesos)</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>Emprendedor</asp:TableHeaderCell>
                                <asp:TableHeaderCell>otro</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Aporte Total</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Aporte en Dinero</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Aporte en Especie</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Clase de Especie</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                            <asp:TableRow>
                                <asp:TableCell></asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <br />
                        <table>
                            <tr>
                                <td style="width: 50%;">
                                    Recursos solicitados al fondo emprender en (smlv)
                                </td>
                                <td style="width: 50%;">
                                    <asp:Label ID="lbltxtSolicitado" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50%;">
                                    Valor recomendado (smlv)
                                </td>
                                <td style="width: 50%;">
                                    <asp:Label ID="lblvalorrecomendado" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <br />
            <br />       
        </div>
    </div>
    </form>
</body>
</html>
