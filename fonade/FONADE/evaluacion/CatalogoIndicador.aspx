<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master"
    AutoEventWireup="true" CodeBehind="CatalogoIndicador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoIndicador" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style1 {
            height: 28px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <table width="565" border="0" align="center" cellspacing="0" cellpadding="0">
        <tr>
            <td width="100%" align="center" valign="top">
                <table width='95%' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF'>
                    <tr>
                        <td align='center' valign='top' width='98%'>
                            <table width='98%' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td>
                                        <h2>
                                            <asp:Label runat="server" ID="lbltitle" />
                                        </h2>
                                    </td>
                                    <td align="right">
                                        <asp:Label runat="server" ID="lbltitle0" Style="font-weight: 700" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        &nbsp;</td>
                                </tr>
                            </table>
                            <table width='98%' border='0' cellspacing='0' cellpadding='3'>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Detalle:</b>
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox name='Detalle' Rows='6' cols='50' runat="server" ID="txtDetalle" TextMode="MultiLine"
                                            Width="408px" MaxLength="255" />
                                    </td>
                                    <td align="left">
                                        &nbsp;</td>
                                </tr>
                                <tr valign="top">
                                    <td align="right">
                                        <b>Valor:</b>
                                    </td>
                                    <td align="right">
                                        &nbsp;
                                    </td>
                                    <td width='167' align="left" colspan="3" class="TitDestacado">
                                        <asp:TextBox runat="server" class='soloLectura money' ID="txtValor" size='15' MaxLength='20'
                                            runat="server" type="number" step="any" min="-999999999" max="999999999" 
                                            ToolTip="Ingrese el valor entero sin separadores de miles. Para indicar la parte decimal coloque un . o una ," /> &nbsp; &nbsp;
                                        (Ej: 12345,34 o -12345.34)
                                        <%--<ajaxToolkit:FilteredTextBoxExtender runat="server" TargetControlID="txtvalor" FilterMode="ValidChars"
                                            ValidChars="1234567890" Enabled="True" ID="FilteredTextBoxExtender1" FilterType="Numbers" />--%>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td align="right" class="auto-style1">
                                        <b>Tipo :</b>
                                    </td>
                                    <td align="right" class="auto-style1">
                                        &nbsp;
                                    </td>
                                    <td width='167' align='left' colspan='3' class='auto-style1'>
                                        <asp:DropDownList ID="cmbTipo" runat="server">
                                            <asp:ListItem Value="$">Dinero</asp:ListItem>
                                            <asp:ListItem Value="%">Porcentaje</asp:ListItem>
                                            <asp:ListItem Value="#">Numérico</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td width='167' align='left' class='auto-style1'>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td colspan='4' align='center' class='TitDestacado'>
                                        &nbsp;
                                    </td>
                                    <td align='center' class='TitDestacado'>
                                        <asp:Button runat="server" ID="btnCrearOrUpdate" OnClick="EventoClickCreateOrUpdate" ValidationGroup="crear"
                                            Height="28px" />
                                        &nbsp;&nbsp;
                                        <asp:Button runat="server" ID="btnCancelar" Text="Cerrar"
                                            OnClick="EventCancelar" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="95%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td bgcolor="#3D5A87">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <script>

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
                        FraccionDecimal = Textvalue.toString().split('.')[1].substr(0, 4)
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
</asp:Content>
