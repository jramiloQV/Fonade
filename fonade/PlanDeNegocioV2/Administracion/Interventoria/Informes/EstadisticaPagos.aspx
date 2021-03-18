<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="EstadisticaPagos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Informes.EstadisticaPagos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .actualizando_principal {
            background-color: #333333;
            filter: alpha(opacity=60);
            opacity: 0.60;
            width: 100%;
            top: 0px;
            left: 0px;
            position: fixed;
            height: 100%;
        }

        .actualizando {
            margin: auto;
            filter: alpha(opacity=100);
            opacity: 1;
            font-size: small;
            vertical-align: middle;
            top: 35%;
            position: fixed;
            right: 45%;
            margin-left: auto;
            margin-right: auto;
            text-align: center;
            background-color: #ffffff;
            height: 128px;
            width: 128px;
            -webkit-border-radius: 10px 10px 10px 10px;
            border-radius: 10px 10px 10px 10px;
        }

            .actualizando img {
                width: 60px;
                height: 64px;
                margin-left: auto;
                margin-right: auto;
                margin-top: 32px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div>
        <asp:Label ID="lbltitulo" runat="server" Text="Estadisticas de Pagos" Font-Size="Large"></asp:Label>
    </div>

    <div>
        <asp:Label ID="lblInterventor" runat="server" Text="Interventor: "></asp:Label>
        <asp:DropDownList ID="ddlInterventores" runat="server"></asp:DropDownList>
    </div>
    <div>
        <asp:Label ID="lblFechaIni" runat="server" Text="Fecha Inicial:"></asp:Label>
        <asp:TextBox ID="txtFechaIni" runat="server" autocomplete="off"
            pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}">
        </asp:TextBox>
        <asp:CalendarExtender ID="calendarInicial" runat="server"
            TargetControlID="txtFechaIni" Format="dd/MM/yyyy">
        </asp:CalendarExtender>


        <asp:Label ID="lblFechaFin" runat="server" Text="Fecha Final:"></asp:Label>
        <asp:TextBox ID="txtFechaFin" runat="server" autocomplete="off"
            pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}">
        </asp:TextBox>
        <asp:CalendarExtender ID="calendarFinal" runat="server"
            TargetControlID="txtFechaFin" Format="dd/MM/yyyy">
        </asp:CalendarExtender>
    </div>
    <asp:UpdatePanel ID="upGrilla" runat="server">
        <ContentTemplate>
            <div>
                <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click" />
                <asp:Button ID="btnExportar" runat="server" Text="Exportar" OnClick="btnExportar_Click" />                
            </div>
            <hr />
            <div>
                <asp:Label ID="lblCantReg" runat="server" Text="Registros Encontrados: 0"></asp:Label>
            </div>

            <div id="resultado" style="overflow: auto;">

                <asp:GridView ID="gvResultado" runat="server"
                    AllowPaging="True"
                    PageSize="<%# PAGE_SIZE %>"
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    DataKeyNames="idPagoActividad" ShowHeaderWhenEmpty="true"
                    EmptyDataText="No hay datos para mostrar."
                    ForeColor="#666666" Width="100%" OnPageIndexChanging="gvResultado_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="idProyecto" HeaderText="Id Proyecto" />
                        <asp:BoundField DataField="nomProyecto" HeaderText="Nombre Proyecto" />
                        <asp:BoundField DataField="nombreInterventor" HeaderText="Nombre Interventor" />
                        <asp:BoundField DataField="fechaAprobInterventor" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Enviado por Interventor" />
                        <asp:BoundField DataField="fechaAprobORechaCoordinador" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Aprobacion o Rechazo Coordinador" />
                        <asp:BoundField DataField="fechaRespuestaFiducia" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Respuesta Fiduciaria" />
                        <asp:BoundField DataField="idPagoActividad" HeaderText="Codigo Solicitud" />
                        <asp:BoundField DataField="nomPagoActividad" HeaderText="Nombre Pago" />
                        <asp:BoundField DataField="cantidadDinero" HeaderText="Cantidad Dinero" />
                        <asp:BoundField DataField="estado" HeaderText="Estado" />
                        <asp:BoundField DataField="observacionFiduciaOCoordinador"
                            HeaderText="Observacion Fiduciaria o Coordinacion" />
                        <asp:BoundField DataField="Operador" HeaderText="Operador" />
                    </Columns>

                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
        <ProgressTemplate>
            <div class="actualizando_principal">
                <div class="actualizando">
                    <asp:Image ID="imgEsperando" ImageUrl="~/images/ajax-loader.gif" runat="server" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>
