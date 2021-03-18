<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Egresos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.Egresos" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .Grilla tr td:first-child, #gw_InversionesFijas > tbody > tr:nth-child(n) > td:nth-child(2) {
            text-align: left;
        }

        .Grilla tr td {
            text-align: right;
        }

        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        #gw_InversionesFijas tr:nth-last-child(1) {
            font-weight: bold;
        }

        #gw_CostosPuestaMarcha tr:nth-last-child(1) {
            font-weight: bold;
        }

        #gw_CostosAnualizados tr:nth-last-child(1) {
            font-weight: bold;
        }

        #gw_GastosPersonales tr:nth-last-child(1) {
            font-weight: bold;
        }

        table#gw_CostosPuestaMarcha tr:nth-child(1n) td:nth-child(2) {
            text-align: right;
        }

        table#gw_CostosAnualizados tr:nth-child(1n) td:nth-child(n+2) {
            text-align: right;
        }

        table#gw_GastosPersonales tr:nth-child(1n) td:nth-child(n+2) {
            text-align: right;
        }
    </style>
</head>
<script type="text/javascript">

    function remove_Nan(txtbox) {
        txtbox.value = txtbox.value.replace(".", ",")
        var Valor = txtbox.value
        if (isNaN(Number(Valor))) {
            txtbox.value = Valor.replace(Valor, "");
        }
    }

    window.onload = function () {
        //Realizado();
    };

    function Realizado() {
        var chk = document.getElementById('chk_realizado')
        var rol = document.getElementById('txtIdGrupoUser').value;
        if (rol != '5') {
            if (chk.checked) {
                chk.disabled = true;
                document.getElementById('btn_guardar_ultima_actualizacion').setAttribute("hidden", 'true');
            }
        }
    }
</script>
<body>
    <% Page.DataBind(); %>
    <div style="overflow:scroll">
    <form id="form1" runat="server" style="background-color: white; background-image: none;">
        <asp:Panel ID="pnlEgresos" Visible="true" runat="server">
            <table runat="server" visible="false">
                <tbody>
                    <tr>
                        <td width="19"></td>
                        <td>ULTIMA ACTUALIZACIÓN:&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                        </td>
                        <td style="width: 100px;"></td>
                        <td>
                            <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left"
                                Enabled='<%# (bool)DataBinder.GetPropertyValue(this,"vldt")?true:false %>' />
                            &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                                ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click"
                                Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />
                        </td>
                    </tr>
                </tbody>
            </table>
            <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
            <div style="position: relative; left: 705px; width: 160px;">
                <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="Egresos" />
            </div>
            <br />
            <div style="text-align: center">
                <h1>VIII - Estructura financiera </h1>
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="19">&nbsp;
                    </td>
                    <td>
                        <table width='95%' border='0' align="center" cellspacing='0' cellpadding='0'>
                            <tr>
                                <td width='50%' align='left'>
                                    <div class="help" onclick="textoAyuda({titulo: 'Egresos', texto: 'Egresos'});">
                                        <span class="image_help">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos"></span>
                                        <span class="text_help">&nbsp;Egresos:</span>
                                    </div>
                                </td>
                                <td align='right'></td>
                            </tr>
                        </table>
                        <br />
                        <table border='0' cellspacing='1' cellpadding='4'>
                            <tr>
                                <td width='50%'>Índice de Actualización monetaria
                                </td>
                                <td>
                                    <!--Validar formato Moneda-->
                                    <asp:TextBox ID="txtActualizacionMonetaria" runat="server" MaxLength="5" Width="30px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="failureNotification"
                                        ControlToValidate="txtActualizacionMonetaria" ErrorMessage="0 a 9 con punto (.) decimal"
                                        ValidationExpression="[0-9]*\.?[0-9]*" ValidationGroup="validaGuardar">0 a 9 con punto (.) decimal</asp:RegularExpressionValidator>
                                </td>
                                <td>
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass='Boton' OnClick="btnGuardar_Click"
                                        ValidationGroup="validaGuardar" Visible="true" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table width='95%' align="Center" border='0' cellpadding='0' cellspacing='0'>
                            <tr>
                                <td align='left' valign='top' width='98%'>
                                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                        <tr bgcolor="#00468f" align="center">
                                            <td>
                                                <span style="color: #ffffff;">Inversiones Fijas y Diferidas</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_InversionesFijas" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="Grilla" OnRowCommand="gw_InversionesFijas_RowCommand"
                                        GridLines="None" CellSpacing="1" CellPadding="4" OnRowDataBound="gw_InversionesFijas_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="3%">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("Id_Inversion") %>'
                                                        runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="false" OnClientClick="return Confirmacion('Esta seguro que desea borrar el concepto seleccionado?')" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Concepto" HeaderStyle-Width="47%" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkConcepto" runat="server" CommandArgument='<%# Eval("Id_Inversion") %>' CommandName="Editar" Text='<%# Eval("concepto") %>' Enabled="false"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField DataField="concepto" HeaderText="Concepto" HeaderStyle-Width="47%"
                                            ItemStyle-HorizontalAlign="Left" />--%>
                                            <asp:BoundField DataField="valor" HeaderText="Valor" HeaderStyle-Width="12%" DataFormatString="{0:#,##0.00}" />
                                            <asp:BoundField DataField="mes" HeaderText="Mes" HeaderStyle-Width="8%" />
                                            <asp:BoundField DataField="tipoFuente" HeaderText="Fuente de financiación" HeaderStyle-Width="30%"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                        <tr bgcolor="#00468f" align="center">
                                            <td>
                                                <span style="color: #ffffff;">Costos de Puesta en Marcha</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_CostosPuestaMarcha" runat="server" AutoGenerateColumns="true"
                                        CssClass="Grilla" Width="100%" GridLines="None" CellSpacing="1" CellPadding="4"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                        <tr bgcolor="#00468f" align="center">
                                            <td>
                                                <span style="color: #ffffff;">Costos Anualizados Administrativos</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_CostosAnualizados" runat="server" AutoGenerateColumns="True"
                                        CssClass="Grilla" Width="100%" GridLines="None" CellSpacing="1" CellPadding="4"
                                        HeaderStyle-HorizontalAlign="Left" />
                                    <br />
                                    <table border="0" cellpadding="4" cellspacing="1" width="100%">
                                        <tr bgcolor="#00468f" align="center">
                                            <td>
                                                <span style="color: #ffffff;">Gastos de Personal</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="gw_GastosPersonales" runat="server" AutoGenerateColumns="True"
                                        CssClass="Grilla" Width="100%" GridLines="None" CellSpacing="1" CellPadding="4" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </asp:Panel>
        <!--  Nuevo Panel -->
        <asp:Panel ID="pnlCrearInversion" Visible="false" runat="server">
            <table width='600px' border='0' cellspacing='0' cellpadding='2'>
                <tr>
                    <td align='center'>NUEVA INVERSIÓN
                    </td>
                </tr>
            </table>
            <table width='600px' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF'>
                <tr>
                    <td align='center' valign='top' width='98%'>
                        <table width='98%' border='0' cellspacing='0' cellpadding='3'>
                            <tr valign="top">
                                <td align="Right" width="110">
                                    <b>Concepto:</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtConcepto" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtConcepto"
                                        ErrorMessage="Ingrese el campo Concepto" ValidationGroup="GrupoCrearInversion">Ingrese el campo Concepto</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="Right">
                                    <b>Valor:</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtValor" runat="server" MaxLength="12" Width="90px"></asp:TextBox>
                                    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtValor"
                                    ErrorMessage="Ingrese el campo Valor" ValidationExpression="[0-9]*\,?[0-9]*"
                                    ValidationGroup="GrupoCrearInversion">Ingrese el campo Valor</asp:RegularExpressionValidator>--%>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="Right">
                                    <b>Meses:</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSemana" runat="server" MaxLength="3" Width="30px" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtSemana"
                                        ErrorMessage="Ingrese el campo Semana" ValidationExpression="[0-9]*" ValidationGroup="GrupoCrearInversion">Ingrese el numero de meses, debe ser un entero.</asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="Right">
                                    <b>Tipo de Fuente:</b>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTipoFuente" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="right" class="TitDestacado" colspan="2">
                                    <asp:Button ID="btnCrearInversion" runat="server" Text="Guardar" OnClick="btnCrearInversion_Click"
                                        ValidationGroup="GrupoCrearInversion" />
                                    <asp:Button ID="btnCancelarNuevaInversion" runat="server" Text="Cancelar" OnClick="btnCancelarNuevaInversion_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            
        </asp:Panel>
        
        <% 
            int i;
            for (i = 0; i < 15; i++)
            {
                Response.Write("<br />");
            }
        %> 
         
    </form>
       
    </div>
</body>
<script type="text/javascript">

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
