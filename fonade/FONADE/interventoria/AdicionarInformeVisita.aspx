<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="AdicionarInformeVisita.aspx.cs" Inherits="Fonade.FONADE.interventoria.AdicionarInformeVisita" UICulture="es" Culture="es-CO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2
        {
            width: 100%;
        }
        .Grilla
        {
        }
        .auto-style3
        {
            height: 168px;
        }
        #bodyContentPlace_c_fecha_s_today{
            height:auto !important;
        }
        /*.ajax__calendar .ajax__calendar_container {
            height: 200px;
        }
        .ajax__calendar_header div:nth-child(1){
            float: left;
        }
        .ajax__calendar_header div:nth-child(2){
            float: right;
        }*/
    </style>
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            //alert($("#ajax__calendar_header div:nth-child(2)").html());
            //$("#ajax__calendar_header div:nth-child(1)").css("float", "left");
            //$("#ajax__calendar_header div:nth-child(2)").css("float", "right");
        });
    </script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="principal" runat="server">
        <h1>
            <asp:Label ID="L_titulo" runat="server" Text="Informe de Visita de Interventoría">

            </asp:Label>
        </h1>
        <br />
        <br />
        <div id="contenido">
            <table class="auto-style2">
                <tr>
                    <td colspan="2">
                        Nombre Informe
                    </td>
                    <td colspan="2">
                        Nombre Empresa
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txtinforme" runat="server" Width="250px"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtinforme"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;El Nombre del Informe es Requerido!!!&quot;</asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtempresa" runat="server" Width="250px" Enabled="false"></asp:TextBox>
                        <asp:Label ID="lblnit" runat="server" Enabled="false" Visible="false"></asp:Label>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtempresa"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;Es necesario seleccionar una empresa!!!&quot;</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        <b>Origen</b>
                    </td>
                    <td colspan="2">
                        <br />
                        <b>Destino</b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Departamento:
                    </td>
                    <td colspan="2">
                        Departamento:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_dedorigen" runat="server" Width="200px" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                            OnSelectedIndexChanged="ddl_dedorigen_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddl_dedorigen"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;Es necesario seleccionar departamento de origen!!!&quot;</asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_deddestino" runat="server" Width="200px" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                            OnSelectedIndexChanged="ddl_deddestino_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddl_deddestino"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;Es necesario seleccionar departamento de destino!!!&quot;</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Ciudad:
                    </td>
                    <td colspan="2">
                        Ciudad:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="panelDropDowList" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_ciuorigen" runat="server" Width="200px">
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddl_ciuorigen"
                                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;Es necesario seleccionar ciudad de origen!!!&quot;</asp:RequiredFieldValidator>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_dedorigen" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddl_ciudestino" runat="server" Width="200px">
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddl_ciudestino"
                                    ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">&quot;Es necesario seleccionar ciudad de destino!!!&quot;</asp:RequiredFieldValidator>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddl_deddestino" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        Costos Medios de Transporte:
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Otro
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox ID="medio1" runat="server" Width="150px" Text="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Avión
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox ID="medio2" runat="server" Width="150px" Text="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Bus
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox ID="medio3" runat="server" Width="150px" Text="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        Barco
                    </td>
                    <td style="text-align: center;">
                        <asp:TextBox ID="medio4" runat="server" Width="150px" Text="0" ClientIDMode="Static"></asp:TextBox>
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        Ingrese el costo del medio de transporte empleado en la visita.
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        Fecha Salida:
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="txtDate" runat="server"  Text=""></asp:TextBox>
                        <asp:ImageButton ID="imgPopup" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                            runat="server" />
                        <asp:CalendarExtender ID="c_fecha_s" PopupButtonID="imgPopup" runat="server" TargetControlID="txtDate" 
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                        <%--<asp:Calendar ID="c_fecha_s" runat="server" CssClass="Grilla" Height="16px" Width="44px">
                        </asp:Calendar>  images/calendar.png--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        Fecha Regreso:
                    </td>
                    <td colspan="2">
                        <%--<asp:Calendar ID="c_fecha_r" runat="server" CssClass="Grilla" Height="153px" Width="82px">
                        </asp:Calendar>--%>
                        <asp:TextBox ID="txtDate2" runat="server"  Text=""></asp:TextBox>
                        <asp:ImageButton ID="imgPopup2" ImageUrl="../../Images/icoModificar.gif" ImageAlign="Bottom"
                            runat="server" />
                        <asp:CalendarExtender ID="c_fecha_r" PopupButtonID="imgPopup2" runat="server" TargetControlID="txtDate2"
                            Format="dd/MM/yyyy">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="4">
                        <br />
                        <br />
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="2">
                        Contenido Informe Visita
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <br />
                        Información Técnica:
                    </td>
                    <td colspan="2">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:TextBox ID="tb_info_tecnica" runat="server" Width="500px" TextMode="MultiLine" ClientIDMode="Static"
                            Height="150px" MaxLength="8000" onkeypress="onTestChange();"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="tb_info_tecnica"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">Los campos Información Técnica son requeridos!!!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Información Financiera:
                    </td>
                    <td colspan="2">
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="auto-style3">
                        <asp:TextBox ID="tb_info_financiera" runat="server" Width="500px" TextMode="MultiLine" ClientIDMode="Static"
                          onkeypress="onTestChange();"  Height="150px" MaxLength="8000"></asp:TextBox>
                        <br />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="tb_info_financiera"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">Los campos Información Financiera son requeridos!!!</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                    <td style="text-align:left">
                        <asp:Button ID="btn_creaar" runat="server" Text="" ValidationGroup="crear" OnClick="btn_creaar_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btn_eliminar" runat="server" Text="" OnClientClick="return confirm('Está seguro de ELIMINAR este informe?')"
                            OnClick="btn_eliminar_Click" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                    <td>
                        
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        function onTestChange() {
            var key = window.event.keyCode;

            // If the user has pressed enter
            if (key == 13) {
                document.getElementById("txtArea").value = document.getElementById("txtArea").value + "\n*";
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
