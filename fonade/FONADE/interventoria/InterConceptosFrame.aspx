<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InterConceptosFrame.aspx.cs" Inherits="Fonade.FONADE.interventoria.InterConceptosFrame" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>

    <script src="../../Scripts/jquery-1.9.1.js"></script>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        table {
            width: 100%;
        }

        h1 {
            text-align: center;
        }

        #neConte {
            margin-left: 10%;
            margin-right: 10%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

         <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div class="ContentInfo" style="width: 900px; height: auto;">
            <div id="neConte">
                <br />
                <br />
                <asp:Panel ID="pnlPlanOperativo" runat="server">
                    <h1>PLAN OPERATIVO</h1>
                    <asp:GridView ID="gvPlanOperativo" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Nombre Actividad" DataField="NomActividad" />
                            <asp:BoundField HeaderText="# Mes" DataField="Mes" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlNomina" runat="server">
                    <h1>NÓMINA</h1>
                    <asp:GridView ID="gvNomina" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Nombre Cargo" DataField="NomActividad" />
                            <asp:BoundField HeaderText="# Mes" DataField="Mes" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlProduccion" runat="server">
                    <h1>PRODUCCIÓN</h1>
                    <asp:GridView ID="gvProduccion" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Nombre Producto" DataField="NomActividad" />
                            <asp:BoundField HeaderText="# Mes" DataField="Mes" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlVentas" runat="server">
                    <h1>VENTAS</h1>
                    <asp:GridView ID="gvVentas" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Nombre Producto" DataField="NomActividad" />
                            <asp:BoundField HeaderText="# Mes" DataField="Mes" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlIndicadoresGenericos" runat="server">
                    <h1>INDICADORES GENERICOS</h1>
                    <asp:GridView ID="gvIndicadoresGenericos" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Indicador" DataField="NomActividad" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlindicadoresEspecificos" runat="server">
                    <h1>INDICADORES ESPECIFICOS</h1>
                    <asp:GridView ID="gvindicadoresEspecificos" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Indicador" DataField="NomActividad" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlRiesgos" runat="server">
                    <h1>RIESGOS</h1>
                    <asp:GridView ID="gvRiesgos" runat="server" CssClass="Grilla" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField HeaderText="Riesgo" DataField="NomActividad" />
                            <asp:BoundField HeaderText="Observaciones" DataField="ObservacionesInterventor" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <br />

                <asp:Panel ID="pnlEmpresa" runat="server">
                    <h1>EMPRESA</h1>
                    <br />
                    <table>
                        <tr>
                            <td>Dificultad Central</td>
                            <td>
                                <asp:DropDownList ID="ddlDificultadCentral" runat="server" Height="16px" Width="400px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>Observaciones</td>
                            <td>
                                <asp:TextBox ID="txtObesrvaciones" runat="server" TextMode="MultiLine"
                                    MaxLength="70000" Width="400px" Height="150px" ValidationGroup="guardar">
                                </asp:TextBox>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                    ErrorMessage="RequiredFieldValidator" Text="*" ForeColor="#ff0000"
                                    ControlToValidate="txtObesrvaciones" ValidationGroup="guardar">
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align: center;">
                                <asp:Button ID="btnGrabar" runat="server" Text="Grabar" 
                                    Visible="false" Enabled="false" ValidationGroup="guardar" 
                                    OnClick="btnGrabar_Click" />
                                 <asp:Button ID="btn_modificar" runat="server" Text="Modificar" 
                                    Visible="false" OnClick="btn_modificar_Click" 
                                    />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <br />
            </div>
        </div>


        <!--Modal-->

          <asp:Label ID="lblModalEliminar" runat="server" Text=""></asp:Label>
        <!--MODAL EDITAR COMPROMISO-->

        <asp:ModalPopupExtender ID="ModalModificarObservacion" runat="server"
            CancelControlID="btnCerrarModalEdit"
            TargetControlID="lblModalEliminar" PopupControlID="pnlEliminarArchivo"
            PopupDragHandleControlID="PopupHeader" Drag="true" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <!--Style="display: none"-->
        <asp:Panel ID="pnlEliminarArchivo" runat="server" Style="display: none"
            Width="550px" Height="330px" BackColor="White">
            <div class="EliminarArchivoPopup" style="overflow: auto;">
                <%--Popup--%>
                <div class="Controls" style="text-align: right; height: 0%">
                    <input type="submit" value="X" id="btnCerrarModalEdit" />
                </div>
                <div class="PopupBody" style="max-height: 500px;">

                    <asp:Label ID="lblIdIndicador" runat="server" Text="Label" ForeColor="White"></asp:Label>
                     <asp:Label ID="lblIndice" runat="server" Text="Label" ForeColor="White"></asp:Label>
                    <div id="cuerpoEliminarArchivo" style="height: 0%; padding-left: 20px;">

                        <h1>Modificar Observaciones</h1>
                        <hr />
                        <h3>Observaciones: </h3>                        

                        <asp:TextBox ID="txtObservacion_New" runat="server"
                            TextMode="MultiLine" Style="width: 500px; margin: 0px; height: 120px;"></asp:TextBox>

                        <h3>Motivo de cambio de la observacion (Obligatorio): </h3>
                        <asp:TextBox ID="txtMotivoCambio" runat="server"
                            TextMode="MultiLine" Style="width: 500px; margin: 0px; height: 120px;"></asp:TextBox>
                        <div style="text-align: center; height: 0%;">
                            <asp:Button ID="btnModificarIndicador" runat="server"
                                Text="Modificar Observaciones" OnClick="btnModificarObservacion_Click" />                            
                            <!---->
                        </div>

                    </div>
                </div>
            </div>
        </asp:Panel>


    </form>
</body>
