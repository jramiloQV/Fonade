<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="CrearActaSeguimiento.aspx.cs"
    Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.CrearActaSeguimiento" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            width: 100%;
        }

        /*.auto-style3 {
            width: 197px;
        }*/

        .auto-style4 {
            width: 197px;
            height: 22px;
        }
    </style>
    <script type="text/javascript">        
        function alerta() {
            return confirm('¿ Esta seguro de crear esta acta de seguimiento ?');
        }
    </script>

    <script  type="text/javascript">
        //this script will get the date selected from the given calendarextender (ie: "sender") and append the
        //current time to it.
        function AppendTime(sender, args) {
            var selectedDate = new Date();
            selectedDate = sender.get_selectedDate();
            var now = new Date();
            sender.get_element().value = selectedDate.format("dd/MM/yyyy") + " " + now.format("hh:mm t.t.");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <h1>
                <asp:Label Text="Crear Acta de Seguimiento" runat="server" ID="lblMainTitle" Visible="true" />
            </h1>
            <br />
            <table id="gvMain" class="auto-style2" runat="server">
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblActa" Font-Bold="True" runat="server" Text="Acta N°"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblNumActa" Font-Bold="True" runat="server" Text="N/A" ForeColor="Maroon"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="lblContrato" Font-Bold="true" runat="server" Text="Contrato N°"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblNumContrato" Font-Bold="True" runat="server" Text="N/A" ForeColor="Maroon"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label16" Font-Bold="true" runat="server" Text="Fecha de Visita"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtFechaVisita" runat="server" autocomplete="off"  required="true"
                            pattern="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"></asp:TextBox>
                        <asp:CalendarExtender ID="calendarFechaVisita" runat="server"
                            Format="dd/MM/yyyy hh:mm tt"                            
                            TargetControlID="txtFechaVisita" ClearTime="True" 
                            EnabledOnClient="True" TodaysDateFormat=""
                            
                            >
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label18" Font-Bold="true" runat="server" Text="Fecha Final de Visita"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtFechaFinalVisita" runat="server" autocomplete="off"  required="true"
                            pattern="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"></asp:TextBox>
                        <asp:CalendarExtender ID="CalendarFechaFinalVisita" runat="server"
                            Format="dd/MM/yyyy hh:mm tt"                            
                            TargetControlID="txtFechaFinalVisita" ClearTime="True" 
                            EnabledOnClient="True" TodaysDateFormat="" >
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="lblInfFecActaInicio" Font-Bold="true" runat="server" Text="Fecha de Acta de Inicio"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:TextBox ID="txtFechaInicio" runat="server" autocomplete="off"  required="true"
                            pattern="^([0]\d|[1][0-2])\/([0-2]\d|[3][0-1])\/([2][01]|[1][6-9])\d{2}(\s([0-1]\d|[2][0-3])(\:[0-5]\d){1,2})?$"></asp:TextBox>
                        <asp:CalendarExtender ID="calendarFechaInicio" runat="server"
                            Format="dd/MM/yyyy hh:mm tt"                            
                            TargetControlID="txtFechaInicio" ClearTime="True" 
                            EnabledOnClient="True" TodaysDateFormat="" >
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="lblInfProrroga" Font-Bold="true" runat="server" Text="Prórroga (Meses)"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblProrroga" Font-Bold="True" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label1" Font-Bold="true" runat="server" Text="ID - Nombre del Plan de Negocio"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblNombreProyecto" Font-Bold="True" runat="server" Text="N/A" ForeColor="Maroon"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label2" Font-Bold="true" runat="server" Text="Nombre de la Empresa"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblNombreEmpresa" Font-Bold="True" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label7" Font-Bold="true" runat="server" Text="Nit de la Empresa"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblNitEmpresa" Font-Bold="True" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="Contrato Marco Interadministrativo"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblContratoMarcoInter" Font-Bold="True" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style4">
                        <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Contrato de Interventoria"></asp:Label>
                    </td>
                    <td class="auto-style4">
                        <asp:Label ID="lblContratoInterventoria" Font-Bold="True" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label5" Font-Bold="true" runat="server" Text="Contratista (s)"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblContratistas" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Valor Aprobado"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblValorAprobado" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label12" Font-Bold="true" runat="server" Text="Domicilio Principal"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblDomicilioEmpresa" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label13" Font-Bold="true" runat="server" Text="Convocatoria/Corte"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblConvocatoriaCorte" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label14" Font-Bold="true" runat="server" Text="Sector Económico"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblSectorEconomico" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label17" Font-Bold="true" runat="server" Text="SubSector Económico"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblSubSector" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label3" Font-Bold="true" runat="server" Text="Objeto del Contrato"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblObjeto" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td class="auto-style3">
                        <asp:Label ID="Label4" Font-Bold="true" runat="server" Text="Objetivo Visita"></asp:Label>
                    </td>
                    <td class="auto-style3">
                        <asp:Label ID="lblObjetivoVisita" Font-Bold="true" runat="server" Text="N/A"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <fieldset style="width: initial;">
                            <legend>Gestor Operativo - Administrativo SENA</legend>
                            <div>

                                <asp:Label ID="Label6" runat="server"
                                    Font-Bold="True">Nombre Completo:</asp:Label>
                                <asp:TextBox ID="txtNombreGestorOperativo" runat="server" Style="width: 100%"
                                    required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                    ControlToValidate="txtNombreGestorOperativo"
                                    CssClass="failureNotification" ErrorMessage="Nombre del Gestor es Obligatorio."
                                    ToolTip="Nombre del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>

                            </div>
                            <div>

                                <asp:Label ID="Label10" runat="server"
                                    Font-Bold="True">Correo Electrónico:</asp:Label>
                                <asp:TextBox ID="txtCorreoGestorOperativo" runat="server"
                                    Style="width: 100%" type="email" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4"
                                    runat="server" ControlToValidate="txtCorreoGestorOperativo"
                                    CssClass="failureNotification" ErrorMessage="Correo del Gestor es Obligatorio."
                                    ToolTip="Correo del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label ID="Label15" runat="server"
                                    Font-Bold="True">Teléfono:</asp:Label>
                                <asp:TextBox ID="txtTelefonoGestorOperativo" runat="server"
                                    Style="width: 100%" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5"
                                    runat="server" ControlToValidate="txtTelefonoGestorOperativo"
                                    CssClass="failureNotification" ErrorMessage="Telefono del Gestor es Obligatorio."
                                    ToolTip="Telefono del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>

                            </div>
                        </fieldset>
                    </td>
                </tr>

                <tr>
                    <td colspan="2">
                        <fieldset style="width: initial;">
                            <legend>Datos Gestor Técnico SENA</legend>
                            <div>

                                <asp:Label ID="lblNombreGestor" runat="server"
                                    Font-Bold="True">Nombre Completo:</asp:Label>
                                <asp:TextBox ID="txtNombreGestorTecnico" runat="server" Style="width: 100%"
                                    required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvNombreGestor" runat="server" 
                                    ControlToValidate="txtNombreGestorTecnico"
                                    CssClass="failureNotification" ErrorMessage="Nombre del Gestor es Obligatorio."
                                    ToolTip="Nombre del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>

                            </div>
                            <div>

                                <asp:Label ID="lblCorreoGestor" runat="server"
                                    Font-Bold="True">Correo Electrónico:</asp:Label>
                                <asp:TextBox ID="txtCorreoGestorTecnico" runat="server"
                                    Style="width: 100%" type="email" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                    runat="server" ControlToValidate="txtCorreoGestorTecnico"
                                    CssClass="failureNotification" ErrorMessage="Correo del Gestor es Obligatorio."
                                    ToolTip="Correo del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>
                            </div>
                            <div>
                                <asp:Label ID="lblTelefonoGestor" runat="server"
                                    Font-Bold="True">Teléfono:</asp:Label>
                                <asp:TextBox ID="txtTelefonoGestorTecnico" runat="server"
                                    Style="width: 100%" required></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                    runat="server" ControlToValidate="txtTelefonoGestorTecnico"
                                    CssClass="failureNotification" ErrorMessage="Telefono del Gestor es Obligatorio."
                                    ToolTip="Telefono del Gestor es Obligatorio."
                                    ValidationGroup="vgDatosGestor">*</asp:RequiredFieldValidator>

                            </div>
                        </fieldset>
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" ></asp:Button>
                        <asp:Button ID="btnAdicionar" runat="server" Text="Crear Acta" 
                            OnClientClick="return alerta();" OnClick="btnAdd_Click"
                            ValidationGroup="vgDatosGestor"></asp:Button>
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
