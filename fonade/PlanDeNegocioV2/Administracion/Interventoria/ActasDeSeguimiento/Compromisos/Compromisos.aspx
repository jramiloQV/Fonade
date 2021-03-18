<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="Compromisos.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.Compromisos.Compromisos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Compromisos</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../../../../Scripts/ScriptsGenerales.js"></script>
    <script src="../../../../../Scripts/jquery-ui-1.8.21.custom.min.js"></script>
    <script src="../../../../../Scripts/jquery-1.10.2.min.js"></script>

    <style type="text/css">
        .auto-style1 {
            width: 203px;
        }

        .auto-style2 {
            height: 33px;
        }
    </style>
    <script>
        function validarDDL() {
            var e = document.getElementById("ddlEditEstado");
            if (e.options[e.selectedIndex].value == '0') {
                alert("Debe Seleccionar un Estado");
                return false;
            }
        }


    </script>
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Está seguro que desea eliminar el compromiso seleccionado?');
        }        
    </script>


</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>

        <div id="titulo">
            <h1>
                <asp:Label ID="Label1" runat="server"
                    Text="8. COMPROMISOS Y REQUERIMIENTOS DE LA INTERVENTORÍA"></asp:Label>
            </h1>
        </div>
        <h2>Seguimiento de compromisos pendientes / Nuevos Compromisos</h2>
        <asp:GridView ID="gvCompromisos" runat="server"
            AutoGenerateColumns="False"
            CssClass="Grilla"
            DataKeyNames="id" ShowHeaderWhenEmpty="true"
            EmptyDataText="No se ha registrado indicador."
            AllowPaging="True" ForeColor="#666666" Width="100%"
            OnRowCommand="gvCompromisos_RowCommand"
            OnPageIndexChanging="gvCompromisos_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="visita" HeaderText="Visita" />
                <asp:BoundField DataField="compromiso" HeaderText="Compromisos" />
                <asp:BoundField DataField="fechaPropuestaEjec" DataFormatString="{0:dd/MM/yyyy}"
                    HeaderText="Fecha Propuesta para la ejecucion" />
                <asp:BoundField DataField="estado" HeaderText="Estado" />
                <asp:BoundField DataField="fechaCumpliCompromiso" DataFormatString="{0:dd/MM/yyyy}"
                    HeaderText="Fecha Cumplimiento Compromiso" />
                <asp:BoundField DataField="observacion" HeaderText="Observación" />
                <asp:BoundField DataField="fechaModificado" HeaderText="Fecha Registro" />
                <asp:TemplateField HeaderText="Editar">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditarCompromiso" runat="server"
                            CausesValidation="false" CommandName="EditarCompromiso"
                            CommandArgument='<%# Eval("id") %>'>
                            <asp:Image runat="server" ID="imgEditarCompromiso" Height="16" Width="13"
                                ImageUrl="~/Images/editar.png" />
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Eliminar">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkeliminar" runat="server" OnClientClick="return alerta()" 
                            CommandArgument='<%# Eval("id")%>'
                            CommandName="ELIMINAR" CausesValidation="false">                                                        
                            <asp:Image runat="server" ID="imgEliminarCompromiso" Height="16" Width="13"
                                ImageUrl="~/Images/icoBorrar.gif" />
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView><div style="text-align: center">
            *Los compromisos de visitas anteriores deben verificarse durante esta visita, verificar su Estado y Fecha de Cumplimiento. </div><div style="text-align: center">
            <asp:Button ID="btnAddCompromiso" runat="server" Text="Agregar Nuevo Compromiso" />
        </div>
        <hr />       
        <!--Histortial de compromisos AllowPaging="True"-->
        <h2>Historial de Compromisos</h2>
        <asp:GridView ID="gvHistorialCompromisos" runat="server"
            AutoGenerateColumns="False"
            CssClass="Grilla"
            DataKeyNames="id" ShowHeaderWhenEmpty="true"
            EmptyDataText="No se ha registrado indicador."
             ForeColor="#666666" Width="100%"
            OnPageIndexChanging="gvHistorialCompromisos_PageIndexChanging">
            <Columns>
                <asp:BoundField DataField="visita" HeaderText="Visita" />
                <asp:BoundField DataField="compromiso" HeaderText="Compromisos" />
                <asp:BoundField DataField="fechaPropuestaEjec" DataFormatString="{0:dd/MM/yyyy}"
                    HeaderText="Fecha Propuesta para la ejecucion" />
                <asp:BoundField DataField="estado" HeaderText="Estado" />
                <asp:BoundField DataField="fechaCumpliCompromiso" DataFormatString="{0:dd/MM/yyyy}"
                    HeaderText="Fecha Cumplimiento Compromiso" />
                <asp:BoundField DataField="observacion" HeaderText="Observación" />
                 <asp:BoundField DataField="fechaModificado" HeaderText="Fecha Registro" />
            </Columns>
        </asp:GridView>



        <!--MODAL NUEVO COMPROMISO-->
        <%--OkControlID="btnGuardar"--%>
        <asp:ModalPopupExtender ID="ModalPopupCompromisos" runat="server"
            CancelControlID="btnCerrarModal"
            TargetControlID="btnAddCompromiso" PopupControlID="pnlAddCompromiso"
            PopupDragHandleControlID="PopupHeader" Drag="true"
            BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <!--Style="display: none"-->
        <asp:Panel ID="pnlAddCompromiso" Style="display: none" runat="server"
            Width="600px" Height="500px" BackColor="White">
            <div class="HellowWorldPopup">
                <%--Popup--%>
                <div class="Controls" style="text-align: right">
                    <input type="submit" value="Cerrar" id="btnCerrarModal" />
                </div>
                <div class="PopupBody" style="max-height: 500px; overflow: auto;">
                    <h1>
                        <asp:Label Text="Adicionar Compromiso/Requerimiento"
                            runat="server" ID="lblMainTitle" Visible="true" />
                    </h1>
                    <table style="width: 100%;">
                        <tr>
                            <td class="auto-style1">Compromiso</td>
                            <td>
                                <asp:TextBox ID="txtCompromiso" runat="server"
                                    MaxLength="500" TextMode="MultiLine" Width="97%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Fecha propuesta para la Ejecución:</td>
                            <td>
                                <asp:TextBox ID="txtFecha" runat="server" autocomplete="off"
                                    pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}">

                                </asp:TextBox>
                                <asp:CalendarExtender ID="calendar" runat="server"
                                    TargetControlID="txtFecha" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Observación:</td>
                            <td>
                                <asp:TextBox ID="txtObservacion" runat="server" MaxLength="10000"
                                    TextMode="MultiLine" Width="97%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center" class="auto-style2">
                                <asp:Button ID="btnGuardar" runat="server" Height="29px"
                                    Text="Guardar" OnClick="btnGuardar_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </asp:Panel>


        <asp:Label ID="lblModalEditar" runat="server" Text=""></asp:Label>
        <!--MODAL EDITAR COMPROMISO-->
        <%--OkControlID="btnGuardar"--%>
        <asp:ModalPopupExtender ID="ModalEditarCompromiso" runat="server"
            CancelControlID="btnCerrarModalEdit"
            TargetControlID="lblModalEditar" PopupControlID="pnlEditCompromiso"
            PopupDragHandleControlID="PopupHeader" Drag="true"
            BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <!--Style="display: none"-->
        <asp:Panel ID="pnlEditCompromiso" runat="server" Style="display: none"
            Width="600px" Height="500px" BackColor="White">
            <div class="HellowWorldPopup">
                <%--Popup--%>
                <div class="Controls" style="text-align: right">
                    <input type="submit" value="Cerrar" id="btnCerrarModalEdit" />
                </div>
                <div class="PopupBody" style="max-height: 500px; overflow: auto;">
                    <h1>
                        <asp:Label Text="Editar Compromiso"
                            runat="server" ID="Label2" Visible="true" />
                    </h1>
                    <table style="width: 100%;">
                        <tr>
                            <td class="auto-style1">Compromiso</td>
                            <td>
                                <asp:Label ID="lblIDCompromiso" runat="server" Text="" Visible="false"></asp:Label>
                                <asp:TextBox ID="txtEditCompromiso" runat="server"
                                    MaxLength="500" TextMode="MultiLine" Width="97%"
                                    Enabled="False"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Fecha propuesta para la Ejecución:</td>
                            <td>
                                <asp:TextBox ID="txtEditFechaEjecucion" runat="server" autocomplete="off"
                                    pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"
                                    disabled="False"></asp:TextBox>
                                <asp:CalendarExtender ID="calendarEditEjecucion" runat="server"
                                    TargetControlID="txtEditFechaEjecucion" Format="dd/MM/yyyy">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Estado:</td>
                            <td>

                                <select name="ddlEditEstado" id="ddlEditEstado" runat="server"
                                    onchange="
                                    if(this.value=='1') 
                                    {
                                        document.getElementById('txtEditFechaCumplimiento').disabled = false;
                                        document.getElementById('txtEditFechaEjecucion').disabled = true;
                                    } 
                                    else if(this.value=='2') {
                                        document.getElementById('txtEditFechaCumplimiento').disabled = true;
                                        document.getElementById('txtEditFechaEjecucion').disabled = false;
                                    }
                                    else {
                                        document.getElementById('txtEditFechaCumplimiento').disabled = true;
                                        document.getElementById('txtEditFechaEjecucion').disabled = true;
                                    }">
                                    <option value="0">Seleccione</option>
                                    <option value="1">CERRADO</option>
                                    <option value="2">REPROGRAMADO POR INCUMPLIMIENTO</option>

                                </select>


                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Fecha Cumplimiento Compromiso:</td>
                            <td>
                                <asp:TextBox ID="txtEditFechaCumplimiento" runat="server" autocomplete="off"
                                    disabled="False"
                                    pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"></asp:TextBox>
                                <asp:CalendarExtender ID="calendarCumplimiento" runat="server"
                                    Format="dd/MM/yyyy" TargetControlID="txtEditFechaCumplimiento">
                                </asp:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style1">Observación:</td>
                            <td>
                                <asp:TextBox ID="txtEditObservacion" runat="server" MaxLength="10000"
                                    TextMode="MultiLine" Width="97%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center" class="auto-style2">
                                <asp:Button ID="btnEditGuardar" OnClientClick="validarDDL()"
                                    runat="server" Height="29px"
                                    Text="Guardar" OnClick="btnEditGuardar_Click" />
                            </td>
                        </tr>
                    </table>

                </div>
            </div>
        </asp:Panel>

    </form>
</body>
</html>
