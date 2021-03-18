<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="PlanOperativo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo.PlanOperativo" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
        function CalcularTotal(nombreCampo, anio) {
            var totalRegistro = 0;

            for (var i = 1; i <= 12; i++) {
                var valor = document.getElementById(nombreCampo + i).value;
                if (isNaN(parseFloat(valor))) {
                    valor = 0
                    document.getElementById(nombreCampo + i).value = "";
                } else {
                    valor = parseFloat(valor)
                }
                totalRegistro = totalRegistro + valor;
            }
            document.getElementById(nombreCampo + "Total").innerHTML = "<b>" + formatCurrency(totalRegistro) + "</b>";
            //Total Columna
            var valorAporte = document.getElementById('txtAporte' + anio).value;
            var valorFondo = document.getElementById('txtFondo' + anio).value;

            valorAporte = isNaN(parseFloat(valorAporte)) ? 0 : parseFloat(valorAporte);
            valorFondo = isNaN(parseFloat(valorFondo)) ? 0 : parseFloat(valorFondo);

            document.getElementById('TotalMes' + anio).innerHTML = "<b>" + formatCurrency(parseFloat(valorAporte) + parseFloat(valorFondo)) + "</b>";

        }

        function formatCurrency(strValue) {
            strValue = strValue.toString().replace(/\$|\,/g, '');
            dblValue = parseFloat(strValue);

            blnSign = (dblValue == (dblValue = Math.abs(dblValue)));
            dblValue = Math.floor(dblValue * 100 + 0.50000000001);
            intCents = dblValue % 100;
            strCents = intCents.toString();
            dblValue = Math.floor(dblValue / 100).toString();
            if (intCents < 10)
                strCents = "0" + strCents;
            for (var i = 0; i < Math.floor((dblValue.length - (1 + i)) / 3); i++)
                dblValue = dblValue.substring(0, dblValue.length - (4 * i + 3)) + ',' +
                dblValue.substring(dblValue.length - (4 * i + 3));
            return (((blnSign) ? '' : '-') + '$' + dblValue + '.' + strCents);
        }

    </script>
    <style type="text/css">
        #pnlPrincipal{
            background-color: #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
   <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />  
        <br />
        <table width='100%' border='0' align="center" cellspacing='0' cellpadding='0'>            
            <tr>
                <td align="left">
                    <div class="help_container">
                        <div onclick="textoAyuda({titulo: 'Plan Operativo', texto: 'PlanOperativo'});">
                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                        </div>
                        <div>
                            &nbsp; <strong>Plan Operativo:</strong>
                        </div>
                    </div>
                </td>
                <td>
                    <div id="div_Post_It1" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="PlanOperativo" _txtTab="1" _mostrarPost="false" />
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table width='100%' align="Center" border='0' cellpadding='0' cellspacing='0'>
            <tr>
                <td align='left' valign='top' width='98%'>
                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                    </table>
                    <table cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td valign="Top">
                                <div style="width: 300px; overflow: auto; border-right: silver 1px solid">
                                    <table class="Grilla" cellpadding="0" cellspacing="0" width="300px" border="0">
                                        <tr>
                                            <th style="width: 260px; text-align: center">
                                                &nbsp;
                                            </th>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_Anexos" runat="server" Width="300px" AutoGenerateColumns="false"
                                        CssClass="Grilla" RowStyle-Height="35px" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Item" HeaderText="Item" />
                                            <asp:TemplateField HeaderText="Actividad">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEditar" runat="server" Text='<%# Eval("Actividad") %>' Visible="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                            <td valign="Top">
                                <div style="width: 560px; overflow: auto">
                                    <table class="Grilla" cellpadding="4" cellspacing="1" width="3380px" border="0">
                                        <tr>
                                            <th style="width: 260px; text-align: center">
                                                Mes 1
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 2
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 3
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 4
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 5
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 6
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 7
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 8
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 9
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 10
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 11
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Mes 12
                                            </th>
                                            <th style="width: 260px; text-align: center">
                                                Costo Total
                                            </th>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_AnexosActividad" runat="server" Width="3380px" AutoGenerateColumns="false"
                                        CssClass="Grilla" RowStyle-Height="35px" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="fondo1" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor1" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo2" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor2" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo3" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor3" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo4" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor4" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo5" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor5" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo6" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor6" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo7" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor7" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo8" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor8" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo9" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor9" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo10" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor10" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo11" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor11" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondo12" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedor12" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="fondoTotal" HeaderText="Fondo" ItemStyle-Width="130px" />
                                            <asp:BoundField DataField="emprendedorTotal" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
