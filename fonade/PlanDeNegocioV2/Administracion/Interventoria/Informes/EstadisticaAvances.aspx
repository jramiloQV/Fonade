<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" 
    CodeBehind="EstadisticaAvances.aspx.cs" 
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Informes.EstadisticaAvances" %>

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
        <asp:Label ID="lbltitulo" runat="server" Text="Estadisticas de Avances - Interventor" Font-Size="Large"></asp:Label>
    </div>

    <div>
        <asp:Label ID="lblInterventor" runat="server" Text="Interventor: "></asp:Label>
        <asp:DropDownList ID="ddlInterventores" runat="server"></asp:DropDownList>
    </div>
    
   <%----%> <asp:UpdatePanel ID="upGrilla" runat="server">
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
                    PageSize = 10
                    AutoGenerateColumns="False"
                    CssClass="Grilla"
                    ShowHeaderWhenEmpty="true"
                    EmptyDataText="No hay datos para mostrar."
                    ForeColor="#666666" Width="100%" OnPageIndexChanging="gvResultado_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="idProyecto" HeaderText="Id Proyecto" />
                        <asp:BoundField DataField="nomActividad" HeaderText="Nombre Actividad" />
                        <asp:BoundField DataField="item" HeaderText="Item" />
                        <asp:BoundField DataField="mes" HeaderText="Mes" />
                        <asp:BoundField DataField="fechaAvanceEmprendedor" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Avance" />
                        <asp:BoundField DataField="observacionesEmprendedor" HeaderText="Observaciones Emprendedor" />                        
                        <asp:BoundField DataField="fechaAprobacionInterventor" DataFormatString="{0:dd/MM/yyyy}"
                            HeaderText="Fecha Aprobacion" />
                        <asp:BoundField DataField="observacionesInterventor" HeaderText="Observaciones Interventor" />
                        <asp:BoundField DataField="Aprobada" HeaderText="Aprobada" />                        
                        <asp:BoundField DataField="nomInterventor" HeaderText="Interventor" />
                        <asp:BoundField DataField="nomEntidad" HeaderText="Entidad" />
                        <asp:BoundField DataField="nomOperador" HeaderText="Operador" />
                    </Columns>

                </asp:GridView>

            </div>
        <%----%></ContentTemplate>
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
