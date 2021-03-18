<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="EjecucionPresupuestal.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.IndicadoresGestion.EjecucionPresupuestal" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style2 {
            width: 407px;
            height: 40px;
        }

        .auto-style4 {
            width: 407px;
        }
        .auto-style10 {
            width: 57px;
        }
        .auto-style13 {
            width: 232px;
        }
        .auto-style15 {
            width: 342px;
        }
        .auto-style16 {
            width: 8px;
        }
    </style>
    <script type="text/javascript">
        function format(input) {
            var num = input.value.replace(/\./g, '');
            if (!isNaN(num) ||
                ((num.indexOf(',') != -1) && comas(num))) {
                num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
                num = num.split('').reverse().join('').replace(/^[\.]/, '');
                input.value = num;
            }

            else {
                alert('Solo se permiten numeros');
                //input.value = input.value.replace(/[^\d\.]*/g, '');
            }
        }

        function comas(entrada) {
            var contador = 0;
            for (var i = 0; i < entrada.length; i++)
                if (entrada[i] == ',') {
                    contador++;
                    if (contador == 2)
                        return false;
                }

            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <div id="titulo">
            <h3>
                <asp:Label ID="Label1" runat="server" Text="3.2 Gestión en Ejecución Presupuestal"></asp:Label>
            </h3>
        </div>
        <div>
            <asp:Panel ID="pnlInfoEjePresupuestal" runat="server">
                <div style="padding-top: 20px;">
                    VISITA N°1: Se le asignaron recursos por 
                    <asp:Label ID="lblValorPesos" runat="server" Text="< $Valor en pesos N SML >"></asp:Label>
                    equivalentes a
                    <asp:Label ID="lblSMLV" runat="server" Text="< N SMLV >"></asp:Label>
                    SMLV, 
                    la interventoría validó el valor asignado con el contrato de cooperación empresarial y plataforma.
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlGestionEjePresupuestal" runat="server">
                <h2>ADICIONAR INFORMACIÓN VISITA
                    <asp:Label ID="lblNumVisita" runat="server" Text="Label"></asp:Label></h2>
                <asp:GridView ID="gvInfoPago" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    DataKeyNames="idPagoActividad"
                    EmptyDataText="NO SE PRESENTARON DESEMBOLSOS"
                    AllowPaging="True" ForeColor="#666666" Width="100%" 
                    OnRowDataBound="gvInfoPago_RowDataBound"
                    OnPageIndexChanging="gvInfoPago_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="idPagoActividad" HeaderText="Id Pago" />
                        <asp:BoundField DataField="Actividad" HeaderText="Actividad" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Concepto" HeaderText="Concepto" />
                        <asp:TemplateField HeaderText="¿Verificó documentos originales?">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDocumentosOrig" runat="server"></asp:DropDownList>
                                <br />
                                <asp:Label ID="lblValDocumentos" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="¿Verficó físicamente Activos y su estado?">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlActivosEstado" runat="server"></asp:DropDownList>
                                <br />
                                <asp:Label ID="lblValEstActivo" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Observación">
                            <ItemTemplate>
                                <asp:TextBox ID="txtObservInfoPago" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblValObservacion" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: right" class="auto-style2">TOTAL DESEMBOLSADO VISITA&nbsp;
                            <asp:Label ID="lblDesVisita" runat="server" Font-Bold="True"
                                Font-Size="Small" ForeColor="#00468F" Text="Label"></asp:Label>
                        </td>
                        <td rowspan="2">*El total desembolsado equivale a la sumatoria de todos los pagos realizados por la Fiduciaria. Este valor será el que afecte el indicador.</td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="auto-style4">TOTAL DESEMBOLSADO&nbsp;
                            <asp:Label ID="lblDesembolsado" runat="server" Font-Bold="True" Font-Size="Small"
                                ForeColor="#00468F" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">

                            <asp:Button ID="btnGuardarInfoPagos" runat="server" Height="28px"
                                Text="Guardar" OnClick="btnGuardarInfoPagos_Click" />

                        </td>
                    </tr>
                </table>
                <h2>HISTORIAL EJECUCIÓN PRESUPUESTAL</h2>
                <asp:GridView ID="gvHistorialEjecucionPresupuesto" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    DataKeyNames="idPagoActividad"
                    EmptyDataText="NO SE PRESENTARON DESEMBOLSOS"
                    OnPageIndexChanging="gvHistorialEjecucionPresupuesto_PageIndexChanging"
                    AllowPaging="True" ForeColor="#666666" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="visita" HeaderText="Visita" />
                        <asp:BoundField DataField="codigoPago" HeaderText="Id Pago" />
                        <asp:BoundField DataField="Actividad" HeaderText="Actividad" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:c}" />
                        <asp:BoundField DataField="Concepto" HeaderText="Concepto" />
                        <asp:BoundField DataField="verificoDocumentos" HeaderText="¿Verificó documentos originales?" />
                        <asp:BoundField DataField="verificoActivosEstado" HeaderText="¿Verficó físicamente Activos y su estado?" />
                        <asp:BoundField DataField="Observacion" HeaderText="Observación" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
        <hr />
        <div>
            <h3>
                <asp:Label ID="Label2" runat="server" Text="3.2.1 Inventarios y Contrato de Garantía"></asp:Label>
            </h3>
        </div>
        <div>
            <asp:Panel ID="pnlInfoInventario" runat="server">
                <div style="padding-top: 20px;">
                    Inventario por Prendar y Verificar
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlGestionInventario" runat="server">
                <asp:GridView ID="gvInventario" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    DataKeyNames="id"
                    EmptyDataText="No se ha registrado indicador."
                    AllowPaging="True" ForeColor="#666666" Width="100%"
                    OnPageIndexChanging="gvInventario_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="visita" HeaderText="Visita" />
                        <asp:BoundField DataField="descripcionRecursos" HeaderText="Descripción Recursos Financiados con Recursos Fondo Emprender" />
                        <asp:BoundField DataField="valorActivos" DataFormatString="{0:c}"
                            HeaderText="Valor Activos Prendables (Incluye Impuestos)" />
                        <asp:BoundField DataField="fechaCargaAnexo" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Carga del Anexo a Plataforma" />
                    </Columns>
                </asp:GridView>
                <div style="text-align:center">
                    <asp:Label ID="lblActivosPrendables" runat="server" Text="Label" 
                        forecolor="#00468F" Font-Size="Small" Font-Bold="True"></asp:Label>
                </div>
                <div>
                    *En cada una de las visitas se debe verificar y plasmar el acumulado 
                    de los activos financiados con recursos del Fondo Emprender. 
                    El anexo de garantía debe quedar cargado en plataforma 
                    al final de cada visita de seguimiento.
                </div>
                <table style="width: 100%;">
                    <tr style="background-color: #00468f;color: white;">
                        <td class="auto-style10">Visita</td>
                        <td class="auto-style15">Descripción Recursos Financiados con Recursos Fondo Emprender</td>
                        <td class="auto-style13">Valor Activos Prendables (Incluye Impuestos)</td>
                        <td>Fecha Carga del Anexo a Plataforma</td>
                    </tr>
                    <tr>
                        <td class="auto-style10" style="text-align:center"> 
                            <asp:Label ID="lblNumVisitaInventario" runat="server" Text=""></asp:Label>

                        </td>
                        <td class="auto-style15">
                            <asp:TextBox ID="txtDescripcionRecursos" runat="server" MaxLength="10000" 
                                TextMode="MultiLine" Width="99%"></asp:TextBox>
                            <br />
                                <asp:Label ID="lblValDescripcionRecursos" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="auto-style13" style="text-align:center">
                            <asp:TextBox ID="txtCantidad" runat="server" MaxLength="12" onkeyup="format(this)" 
                                Style="text-align: center"></asp:TextBox>
                            <br />
                                <asp:Label ID="lblValCantidad" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                        </td>
                        <td style="text-align:center">
                             <asp:TextBox ID="txtFechaCarga" runat="server" autocomplete="off"
                                    pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblValFechaCarga" runat="server" Text=""
                                    Visible="false" ForeColor="Red" Font-Size="Small"></asp:Label>
                                <asp:CalendarExtender ID="calendarFechaCarga" runat="server"
                                    Format="dd/MM/yyyy" TargetControlID="txtFechaCarga">
                                </asp:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="text-align:center">
                            <asp:Button ID="btnGuardarInventario" runat="server" Height="29px" OnClick="btnGuardarInventario_Click" Text="Guardar" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
        <hr />
        <div>
            <h3>
                <asp:Label ID="Label3" runat="server" Text="3.2.2 Aportes del Emprendedor"></asp:Label>
            </h3>
        </div>
        <div>
            <asp:Panel ID="pnlInfoAportes" runat="server">
                <div style="text-align:center">
                    <asp:Label ID="Label4" runat="server" Text="Meta Aportes Emprendedor" 
                        forecolor="#00468F" Font-Size="Small" Font-Bold="True"></asp:Label>
                </div>
                <div style="text-align:center">
                    <asp:TextBox ID="txtMetaAportesEmp" runat="server" TextMode="MultiLine"
                        Style="height: 70px; width: 500px;">
                    </asp:TextBox>
                </div>
                <asp:GridView ID="gvAportesEmp" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    DataKeyNames="id"
                    OnPageIndexChanging="gvAportesEmp_PageIndexChanging"
                    EmptyDataText="No se ha registrado informacion."
                    AllowPaging="True" ForeColor="#666666" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="visita" HeaderText="Visita" />
                        <asp:BoundField DataField="descripcion" HeaderText="Descripción" />                       
                    </Columns>
                </asp:GridView>
                <table style="width: 100%;">
                    <tr style="background-color: #00468f;color: white; text-align:center">
                        <td class="auto-style16">Visita</td>
                        <td>Descripción</td>
                    </tr>
                    <tr>
                        <td style="text-align:center" class="auto-style16">
                            <asp:Label ID="lblNumVisitaAport" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescAporteEmp" runat="server" MaxLength="10000" TextMode="MultiLine" Width="98%"></asp:TextBox>
                            <br />
                            <asp:Label ID="lblValDescripcionAporte" runat="server" Font-Size="Small" 
                                ForeColor="Red" Text="" Visible="false"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align:center">
                            <asp:Button ID="btnGuardarAportEmp" runat="server" Text="Guardar" Height="28px" OnClick="btnGuardarAportEmp_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
