<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoPptal.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.SeguimientoPptal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <!--<script src="../../Scripts/jquery-1.9.1.js"></script>-->
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        html,body{
            background-image: none !important;
        }
        table
        {
            width: 100%;
        }
        table table td
        {
            width: 50%;
        }
        table.Grilla th{
            text-align: center;
        }
        .valor{
            text-align: right;
        }
        tr:first-child
        {
            text-align: left;
        }
        .help{
            width: 100%;
            height: 30px;
        }
    </style>
</head>
<body style="overflow: auto;">
    <form id="form1" runat="server">
    <div style="width: 100%; height: auto;">
        <div class="help">

        </div>
        <table>
            <tr>
                <td colspan="2">
                    <div class="help_container">
                        
                        <div>
                            <strong>Seguimiento Presupuestal</strong>
                        </div>
                        <div onclick="textoAyuda({titulo: 'Presupuesto', texto: 'Presupuesto'});">
                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;" colspan="2">
                    <table>                        
                        <tr>
                            <td>
                                Presupuesto Recomendado por Fondo Emprender:
                            </td>
                            <td style="text-align: left;">
                                &nbsp;
                                <asp:Label ID="lblemprender" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Presupuesto Aprobado por Interventoria:
                            </td>
                            <td style="text-align: left;">
                                &nbsp;
                                <asp:Label ID="lblinterventoria" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Presupuesto Disponible: 
                            </td>
                            <td style="text-align: left;">
                                &nbsp;
                                <asp:Label ID="lbldisponible" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1">
                                <asp:LinkButton ID="lnkPagos" Text="Descargar pagos en excel" runat="server" OnClick="lnkPagos_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    <div style=" overflow-y: auto; width: 100%; height: 530px;">
                        <asp:GridView ID="gvpresupuesto" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                            OnRowCommand="gvpresupuesto_RowCommand">
                            <Columns>
                                <asp:BoundField HeaderText="Id" DataField="Id_PagoActividad" ></asp:BoundField>
                                <asp:BoundField HeaderText="Nombre" DataField="NomPagoActividad" />
                                <asp:BoundField HeaderText="Fecha creación pago" DataField="FechaIngresoPago" DataFormatString="{0:g}"/>
                                <asp:BoundField HeaderText="Fecha envió a interventor" DataField="FechaIngresoInterventor" DataFormatString="{0:g}"/>
                                <asp:BoundField HeaderText="Fecha aprobación Interventor" DataField="FechaAprobacionInterventor" DataFormatString="{0:g}"/>                                
                                <asp:BoundField HeaderText="Fecha Aprobación o rechazo coordinador" DataField="FechaAprobacionORechazoCoordinador" DataFormatString="{0:g}"/>                                
                                <asp:BoundField HeaderText="Valor" DataField="CantidadDinero" DataFormatString="{0:C}"/>
                                <asp:BoundField HeaderText="Estado" DataField="Estado" />
                                <asp:TemplateField HeaderText="Beneficiario">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("NomTipoIdentificacion") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("NomTipoIdentificacion") + " No. " + Eval("NumIdentificacion") + "<br>" + Eval("Nombre") + " " + Eval("Apellido") + " - " + Eval("RazonSocial") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                                
                                <asp:BoundField HeaderText="Fecha Respuesta fiduciaria" DataField="FechaRtaFA" DataFormatString="{0:d}"
                                    HtmlEncode="false" />
                                <asp:BoundField HeaderText="Valor ReteFuente" DataField="ValorReteFuente" DataFormatString="{0:C}">
                                    <ItemStyle CssClass="valor"/>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Valor ReteIVA" DataField="ValorReteIVA" DataFormatString="{0:C}"><ItemStyle CssClass="valor"/></asp:BoundField>
                                <asp:BoundField HeaderText="Valor ReteICA" DataField="ValorReteICA" DataFormatString="{0:C}"><ItemStyle CssClass="valor"/></asp:BoundField>
                                <asp:BoundField HeaderText="Otros Descuentos" DataField="OtrosDescuentos" DataFormatString="{0:C}"><ItemStyle CssClass="valor"/></asp:BoundField>
                                <asp:BoundField HeaderText="Valor Pagado" DataField="ValorPagado" DataFormatString="{0:C}"><ItemStyle CssClass="valor"/></asp:BoundField>
                                <asp:BoundField HeaderText="Valor ultimo reintegro" DataField="UltimoReintegro" DataFormatString="{0:C}"><ItemStyle CssClass="valor"/></asp:BoundField>
                                <asp:BoundField HeaderText="Codigo Pago" DataField="CodigoPago" DataFormatString="{0}"></asp:BoundField>
                                <asp:TemplateField HeaderText="Anexos del Pago">
                                    <ItemTemplate>
                                        <asp:Button ID="btnverdocumentsopagos" runat="server" Text="Ver" CommandName="Lista" CommandArgument='<%# Eval("Id_PagoActividad") %>'
                                            CssClass="boton_Link_Grid" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Observaciones Fiduciaria o coordinación" DataField="ObservacionesFA" />
                                <asp:TemplateField HeaderText="Ver reintegros">
                                    <ItemTemplate>
                                        <asp:Button ID="btnverReintegros" runat="server" Text="Ver" CommandName="Reintegros" CommandArgument='<%# Eval("Id_PagoActividad") %>'
                                            CssClass="boton_Link_Grid" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                        
                    </div>
                    <br />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
