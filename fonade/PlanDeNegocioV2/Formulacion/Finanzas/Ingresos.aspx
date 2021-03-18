<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ingresos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.Ingresos" %>

<%@ Register Src="~/Controles/CargarArchivos.ascx" TagName="CargarArchivos" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />           
    <script type="text/javascript" src="~/Scripts/jquery.number.min.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>    
    <script src="../../../Scripts/jquery-1.11.1.min.js"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js"></script>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        #gw_AporteEmprendedores tr:nth-last-child(1) {
            font-weight: bold;
        }

        #gw_RecursosCapital tr:nth-child(n+2) td:nth-child(3), td:nth-child(5) {
            text-align: right;
        }
        /*#gw_RecursosCapital tr:nth-child(n+3) td:nth-child(2){
            text-align:right;
        }*/
        #gw_RecursosCapital tr:nth-last-child(1) {
            font-weight: bold;
        }

            #gw_RecursosCapital tr:nth-last-child(1) td:nth-child(2) {
                text-align: left;
            }

        #gw_IngresosVentas tr:nth-last-child(1) {
            font-weight: bold;
        }
         body, html {
            background-image: none !important;
        }
    </style>
    <script>
        $(document).ready(function () {
            var url = window.location.href;
            $("#CargarArchivos1_btnCancelar").click(function (event) {
                event.preventDefault();
                $('#CargarArchivos1_btnCancelar').attr('onclick', '').unbind('click');
                $("#pnlCargueDocumento").css("visibility", "hidden");
                $("#pnlPrincipal").css("display", "block");
                window.location = url;
            });
        });

        window.onload = function () {
           // Realizado();
        };

        function Realizado() {
            var chk = document.getElementById('chk_realizado')
            var rol = document.getElementById('txtIdGrupoUser').value;
            if (rol != '5') {
                if (chk.checked) {
                    chk.disabled = true;
                    document.getElementById('btn_guardar_ultima_actualizacion').setAttribute("hidden", 'true');
                }
                else {
                    chk.disabled = false;
                    document.getElementById('btn_guardar_ultima_actualizacion').setAttribute("hidden", 'false');
                }
            }
        }
        $(function () {
            $('.money').number(true, 2);
        });
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server" style="background-color: white; background-image: none;">
        <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
            <table runat="server" visible="false">
                <tbody>
                    <tr>
                        <td width="19px;"></td>
                        <td>ULTIMA ACTUALIZACIÓN:&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                        </td>
                        <td></td>
                        <td>
                            <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left"
                                Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                            &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                                ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click"
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
            <div style="position: relative; left: 705px; width: 160px;">
                <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Ingresos" />
            </div>
            <br />
            <div style="text-align: center">
                <h1>VIII - Estructura financiera </h1>
            </div>
            <table runat="server" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td style="width: 19px;">&nbsp;
                    </td>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 50%">
                                    <div class="help" onclick="textoAyuda({titulo: 'Fuentes de Financiación', texto: 'FuentesFinanciacion'});">
                                        <span class="image_help">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></span>
                                        <span class="text_help">&nbsp;&nbsp;Fuentes de Financiaci&oacute;n: &nbsp;&nbsp;&nbsp;</span>
                                    </div>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                        <br />
                        Recursos solicitados al fondo emprender en (smlv)
                    <asp:TextBox ID="txtRecursosSolicitados" runat="server" MaxLengt="3" Width="30px" MaxLength="180"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="failureNotification"
                            ControlToValidate="txtRecursosSolicitados" ErrorMessage="*" ValidationExpression="[0-9]*"
                            ValidationGroup="validaGuardar" Display="Dynamic">El valor debe ser numérico sin decimales</asp:RegularExpressionValidator>
                        <asp:Label ID="lblErrorRecursos" runat="server" Text="" CssClass="failureNotification"></asp:Label>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass='Boton' OnClick="btnGuardar_Click"
                            ValidationGroup="validaGuardar" Visible="False" />
                        <br />
                        <table width="100%">
                            <tr>
                                <td align='left' valign='top' width='98%'>
                                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                        <tr>
                                            <td align='left'>
                                                <asp:Panel ID="pnlBotonAdicionarAporte" runat="server" Visible="false">
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                                                        Visible="False" />
                                                    <asp:Button ID="btnAdicionarAporte" runat="server" Text="Adicionar Aporte" CssClass='boton_Link'
                                                        BorderStyle="None" OnClick="btnAdicionarAporte_Click" Visible="false" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #00468F;">
                                            <td align="left" style="text-align: center; color: White;">Aporte de los Emprendedores
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_AporteEmprendedores" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="Grilla" OnRowCommand="gw_AporteEmprendedores_RowCommand" DataKeyNames="Id_Aporte"
                                        OnRowDataBound="gw_AporteEmprendedores_RowDataBound" CellSpacing="1" GridLines="None">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="3%" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_BorrarEmprendedor" CommandName="Borrar" CommandArgument='<%# Eval("Id_Aporte") %>'
                                                        runat="server" ImageUrl="/Images/icoBorrar.gif" OnClientClick="return Confirmacion('Esta seguro que desea borrar el aporte seleccionado?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nombre">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNombre" runat="server" Text='<%# Eval("nombre") %>' CommandArgument='<%# Eval("Id_Aporte") %>' CommandName="Editar"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="nombre" HeaderText="Nombre" ItemStyle-Width="47%" ItemStyle-HorizontalAlign="Left" />--%>
                                            <asp:BoundField DataField="valor" HeaderText="Valor" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="detalle" HeaderText="Detalle" ItemStyle-Width="35%" />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                        <tr>
                                            <td align='left'>
                                                <asp:Panel ID="pnlBotonAdicionarRecurso" runat="server" Visible="false">
                                                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                    <asp:Button ID="btnAdicionarRecurso" runat="server" Text="Adicionar Recurso" CssClass='boton_Link'
                                                        BorderStyle="None" OnClick="btnAdicionarRecurso_Click" />
                                                </asp:Panel>
                                            </td>
                                        </tr>
                                        <tr style="background-color: #00468F;">
                                            <td align="left" style="text-align: center; color: White;">Recursos de Capital
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_RecursosCapital" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="Grilla" OnRowCommand="gw_RecursosCapital_RowCommand" DataKeyNames="Id_Recurso"
                                        OnRowDataBound="gw_RecursosCapital_RowDataBound" CellSpacing="1" GridLines="None">
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField ItemStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_BorrarRecursosCapital" CommandName="Borrar" CommandArgument='<%# Eval("Id_Recurso") %>'
                                                        runat="server" ImageUrl="../../../Images/icoBorrar.gif" Visible="true" OnClientClick="return Confirmacion('Esta seguro que desea borrar el Recurso de Capital seleccionado?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cuantía">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCuantia" runat="server" CommandArgument='<%# Eval("Id_Recurso") %>' CommandName="Editar" Text='<%# Eval("cuantia") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="cuantia" HeaderText="Cuantía" ItemStyle-Width="10%" />--%>
                                            <asp:BoundField DataField="plazo" HeaderText="Plazo" ItemStyle-Width="18%" />
                                            <asp:BoundField DataField="formaPago" HeaderText="Forma de Pago" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="intereses" HeaderText="Interes (Nominal/Anual)" ItemStyle-Width="10%" />
                                            <asp:BoundField DataField="destinacion" HeaderText="Destinación" ItemStyle-Width="31%" />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%" style="background-color: #00468F;">
                                        <tr>
                                            <td style="text-align: center; color: White;">Proyeccion de Ingresos por Ventas
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_IngresosVentas" runat="server" CssClass="Grilla" Width="100%"
                                        OnRowDataBound="gw_IngresosVentas_RowDataBound"
                                        CellSpacing="1" GridLines="None">
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width="100%" border='0' align="center" cellspacing='0' cellpadding='0'>
                            <tr>
                                <td width="50%" align="left">
                                    <div class="help" onclick="textoAyuda({titulo: 'Modelo Financiero', texto: 'FormatosFinancieros'});">
                                        <span class="image_help">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos"></span>
                                        <span class="text_help">&nbsp;&nbsp;Modelo Financiero&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                    </div>
                                </td>
                                <td align="left"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <table width="95%" border="0" align="center" cellspacing='0' cellpadding='0'>
                <tr>
                    <td> 
                        <asp:Button ID="bntModeloFinancieroComercial" runat="server" Text="Modelo Financiero Comercial y Servicios" OnClick="bntModeloFinancieroComercial_Click" CssClass="boton_Link" Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "vislzItem")??false) %>' />
                        <br />
                        <asp:Button ID="bntModeloFinancieroIndustrial" runat="server" Text="Modelo Financiero Industrial" OnClick="bntModeloFinancieroIndustrial_Click" CssClass="boton_Link" Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "vislzItem")??false) %>' />
                        <br />
                        <asp:Button ID="bntModeloFinancieroAgropecuario" runat="server" Text="Modelo Financiero Agropecuario" OnClick="bntModeloFinancieroAgropecuario_Click" CssClass="boton_Link" Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "vislzItem")??false) %>' />
                    </td>
                    <td>
                        <asp:Button ID="btnLinkSubirModeloFinanciero" runat="server" Text="Subir Modelo Financiero" OnClick="btnLinkSubirModeloFinanciero_Click" CssClass="boton_Link" Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "vislzItem")??false) %>' />
                    </td>
                    <td>
                        <div id="pnlVerPlan" runat="server">
                            <asp:LinkButton ID="btnLinkVerModeloFinanciero" runat="server" OnClick="btnLinkVerModeloFinanciero_Click" Visible="false">Ver Modelo Financiero</asp:LinkButton>
                        </div>
                    </td>
                </tr>                
            </table>
            <br />
        </asp:Panel>
        <!--  Nuevo Panel -->
        <asp:Panel ID="pnlAporte" Visible="false" runat="server">
            <table runat="server" width='600' border='0' cellspacing='0' cellpadding='2'>
                <tr>
                    <td align='center' valign='baseline'>NUEVO APORTE
                    </td>
                </tr>
            </table>
            <table style="width: 1000px; border: 0px;" cellspacing='0' cellpadding='3'>
                <tr style="vertical-align: top">
                    <td align="right" width="167px">
                        <b>Nombre:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreAporte" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombreAporte"
                            ErrorMessage="El nombre es requerido" ValidationGroup="ValidacionNuevoAporte">El nombre es requerido</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td align="Right">
                        <b>Valor:</b>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtValorAportes" runat="server" CssClass="money" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <%--<tr style="vertical-align:top">--%>
                <td align="right">
                    <b>Tipo de Aporte:</b>
                </td>
                <td align="left">
                    <asp:DropDownList ID="ddlTipoAporte" runat="server">
                    </asp:DropDownList>
                </td>
                </tr>
            <tr style="vertical-align: top">
                <td align="right">
                    <b>Detalle:</b>
                </td>
                <td>
                    <asp:TextBox ID="txtDetalleAporte" runat="server" MaxLength="20" Width="410px" TextMode="MultiLine"
                        Height="100px"></asp:TextBox>
                </td>
            </tr>
                <tr style="vertical-align: top">
                    <td colspan="2" align="center">
                        <asp:Button ID="btnCrearAporte" runat="server" Text="Guardar" OnClick="btnCrearAporte_Click" />
                        <asp:Button ID="btnCancelarAporte" runat="server" Text="Cerrar" OnClick="btnCancelarAporte_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <!-- Nuevo Panel -->
        <asp:Panel ID="pnlRecurso" runat="server" Visible="false">
            <table width='600' border='0' cellspacing='0' cellpadding='2'>
                <tr>
                    <td align='center' style="vertical-align: baseline;">NUEVO RECURSO
                    </td>
                </tr>
            </table>
            <table style="width: 1000px; border=0;" cellspacing='0' cellpadding='3'>
                <tr style="vertical-align: top">
                    <td width="167px;" align="right">
                        <b>Tipo:</b>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlTipoRecurso">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="right">
                        <b>Cuantía:</b>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtCuantiaRecurso" runat="server" CssClass="money" MaxLength="20" Width="100" />
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCuantiaRecurso"
                        ErrorMessage="* El valor debe ser numérico" ValidationGroup="ValidaNuevoRecurso"
                        ValidationExpression="\d+" />--%>
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="right">
                        <b>Plazo (Meses):</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPlazoRecurso" runat="server" Width="220" align="left" />
                    </td>
                </tr>
                <tr style="vertical-align: top;">
                    <td align="right">
                        <b>Forma de Pago:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormaPagoRecurso" runat="server" MaxLength="50" Width="240px"
                            Height="22px" align="left" />
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td align="right">
                        <b>Interes (Nominal/Anual):</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtInteresRecurso" runat="server" MaxLength="4" Width="40" align="left" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td align="right">
                        <b>Destinación:</b>
                    </td>
                    <td>
                        <asp:TextBox ID="txtDestinacionRecurso" runat="server" Width="410" Height="100" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr style="vertical-align: top">
                    <td colspan="2">
                        <asp:Button ID="btnCrearRecurso" runat="server" Text="Guardar" OnClick="btnCrearRecurso_Click"
                            ValidationGroup="ValidaNuevoRecurso" align="center" />
                        <asp:Button ID="btnCancelarRecurso" runat="server" Text="Cerrar" OnClick="btnCancelarRecurso_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlCargueDocumento" runat="server" Visible="false">
            <!-- Este archivo funciona correctamente -->
            <uc1:CargarArchivos ID="CargarArchivos1" runat="server" />
        </asp:Panel>
        <br />
        <br />
        <br />
        <br />
    </form>
</body>
<script type="text/javascript">
    function SoloNumeros(input) {
        var num = input.value.replace(/\./g, '');
        if (!isNaN(num)) {
            num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
            num = num.split('').reverse().join('').replace(/^[\.]/, '');
            input.value = num;
        }

        else {
            alert('Solo se permiten numeros');
            input.value = input.value.replace(/[^\d\.]*/g, '');
        }
    }

    function MoneyFormat(obj) {
        var Textvalue = obj.value
        var Valortext = Textvalue.toString().split('.').length
        var FraccionInteger = "0"
        var FraccionDecimal = "00"
        if (Textvalue != "") {
            if (Valortext == 1) {
                if (Textvalue.toString().split('.')[0].substr(0, 1) != "0") {
                    FraccionInteger = Textvalue.toString().split('.')[0]
                }
            }
            if (Valortext == 2) {
                if (Textvalue.toString().split('.')[0].substr(0, 2) != "00" && Textvalue.toString().split('.')[0] != "") {
                    FraccionInteger = Textvalue.toString().split('.')[0]
                }
                if (Textvalue.toString().split('.')[1].substr(0, 2) != "00" && Textvalue.toString().split('.')[1] != "") {
                    FraccionDecimal = Textvalue.toString().split('.')[1].substr(0, 2)
                }
            }
            var longitud = FraccionInteger % 3
            var Resultado = ""
            var posInicial = 3
            var posFinal = FraccionInteger.length
            if (FraccionInteger.length > 3) {
                while (FraccionInteger.length >= 1) {
                    if (posFinal < posInicial) {
                        posInicial = posFinal
                    }
                    Resultado += FraccionInteger.substr(posFinal - posInicial, posInicial) + ','
                    FraccionInteger = FraccionInteger.substring(0, posFinal - posInicial)
                    posFinal = posFinal - 3
                }
                Resultado = Resultado.substr(0, Resultado.length - 1)
                var ArrInteger = Resultado.split(',')
                for (j = ArrInteger.length - 1; j >= 0; j--) {
                    FraccionInteger += ArrInteger[j] + ','
                }
                FraccionInteger = FraccionInteger.substring(0, Resultado.length)
            }
        }
        Textvalue = FraccionInteger + '.' + FraccionDecimal
        obj.value = Textvalue
    }
    function validarNro(e) {
        var key = e.which
        switch (true) {
            case key <= 44:
                e.preventDefault();
                break
            case key == 13:
                e.preventDefault();
            case key > 57:
                e.preventDefault();
        }
    }
</script>
</html>
