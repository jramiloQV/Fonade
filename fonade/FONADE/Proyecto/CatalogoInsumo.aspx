<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoInsumo.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoInsumo" %>

<%@ Register Src="../../Controles/Alert.ascx" TagName="Alert" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html >


<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-x: hidden;">
<head runat="server">
    <title>FONDO EMPRENDER - Costos de Insumos</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../Scripts/ScriptsGenerales.js"></script>
    <script type="text/javascript">
        var ClosingVar = false

        var vldtIntro = function (source, arguments) {
            if (!RegExp("{[a-z,A-Z]}").test(arguments.Value) || arguments.Value.length < 2) {
                arguments.IsValid = false;
                event.target.focus();
            }
            arguments.IsValid = true;
        }
        var vldtNumIntro = function (source, arguments) {
            if (isNaN(arguments.Value) || arguments.Value.length < 1) {
                arguments.IsValid = false;
                event.target.focus();
            }
            arguments.IsValid = true;
        }
        function MoneyFormat(Obj) {
            var Textvalue = Obj.value
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
            Obj.value = Textvalue
        }
        function validarNro(e) {
            var error = document.getElementById('divError');
            var key = e.which
            switch (true) {
                case key <= 44:
                    error.innerHTML = 'Ingrese punto(.) como signo decimal'
                    e.preventDefault();
                    break
                case key == 13:
                    error.innerHTML = 'Para pasar al campo siguiente presione la tecla de Tabulación'
                    e.preventDefault();
                case key > 57:
                    error.innerHTML = 'Ingrese sólo números'
                    e.preventDefault();
            }
        }
        function ValidaRequerido() {
            var msgError = ""
            var nombreinsumo = document.getElementById('txt_nombreinsumo').value;
            var Cantidad = document.getElementById('txtCantidad').value;
            var Iva = document.getElementById('txt_ivainsumo');
            if (nombreinsumo == "") {
                msgError = "El campo precio de lanzamiento es requerido. </br>"
            }
            if (Cantidad == "") {
                msgError += "El campo cantidad es requerido. </br> "

            }
            if (Iva == "") {
                msgError += "El campo Iva es requerido. </br>"
            }
            return msgError
        }
        function CerrarVentana() {
            window.opener.document.getElementById('hidInsumo').value = '1';
            ClosingVar = true
        }
        function SaveButton() {
            ClosingVar = false
        }
        window.onbeforeunload = ExitCheck;
        function ExitCheck() {
            if (ClosingVar == true) {
                ClosingVar = false
                return document.getElementById('ActiveClose').value;
            }
        }
    </script>
</head>
<body onload="CerrarVentana()">


    <form id="form1" runat="server">
        <asp:LinqDataSource ID="lds_cargartxt" runat="server"
            ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true"
            OnSelecting="lds_cargartxt_Selecting">
        </asp:LinqDataSource>
        <input id="ActiveClose" type="hidden" value="Si decide abandonará la página, puede perder los cambios si no ha GRABADO ¡¡¡" runat="server" />


        <asp:ObjectDataSource ID="ProyectoInsumoPrecioODS" runat="server" SelectMethod="ProyectoInsumoPrecio" TypeName="Fonade.FONADE.Convocatoria.CatalogoInsumo" InsertMethod="Insert"></asp:ObjectDataSource>

        <asp:LinqDataSource ID="lds_tipoInsumos" runat="server"
            ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true"
            OnSelecting="lds_tipoInsumos_Selecting">
        </asp:LinqDataSource>
        <table align="center">
            <tr>
                <td class="auto-style4" style="color: #FFFFFF; background-color: #00468F; font-size: 12px; font-family: Arial; text-align: left;" align="center">
                    <asp:Label ID="lblTitulo" runat="server" Text="ADICIONAR INSUMO AL PRODUCTO "></asp:Label>

                </td>
            </tr>
            <tr>
                <td class="auto-style4" style="text-align: center; text-align: center">
                    <table style="height: 201px; width: 340px">

                        <tr>
                            <td class="auto-style3" style="text-align: left">Nombre:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txt_nombreinsumo" runat="server" Width="193px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">Tipo de Insumo:</td>
                            <td class="auto-style5">
                                <asp:DropDownList ID="ddl_insumotipos" runat="server" Height="19px" Width="198px" DataSourceID="lds_tipoInsumos" DataTextField="NomTipoInsumo" DataValueField="Id_TipoInsumo">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">IVA:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txt_ivainsumo" runat="server" Width="191px" FilterType="Numbers"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">Unidad de Medida<br />
                            </td>
                            <td class="auto-style5">
                                <asp:TextBox ID="UnidadMedidaTextBox" runat="server" ToolTip="Tipo de unidad de medida (ej: Centímetros, Gramos, Litros; etc" Width="187px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">Cantidad:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txtCantidad" runat="server" ToolTip="Tipo de unidad de medida (ej: Centímetros, Gramos, Litros; etc" Width="189px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">%Desperdicio:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txtDesperdicio" runat="server" ToolTip="Tipo de unidad de medida (ej: Centímetros, Gramos, Litros; etc" Width="188px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">Presentación:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txt_presentacioninsumo" runat="server" Width="188px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="auto-style3" style="text-align: left">% Compras Crédito:</td>
                            <td class="auto-style5">
                                <asp:TextBox ID="txt_creditoinsumo" runat="server" Width="187px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td class="auto-style4">
                    <div id="divError" style="color: red"></div>
                    <div style="width:900px">
                        <div style="overflow-y:scroll" >
                            <asp:GridView ID="gvProyeccionVentas" runat="server" CssClass="Grilla" DataSourceID="ProyectoInsumoPrecioODS"
                            EmptyDataText="No hay información disponible."  BorderStyle="None" CaptionAlign="Right" Height="16px">
                            <AlternatingRowStyle Width="200px" />
                            <EditRowStyle Width="100%" />
                            <EmptyDataRowStyle Width="200px" />
                            <HeaderStyle Width="20px" />
                            <RowStyle Width="100%" />
                            <SelectedRowStyle Width="100%" />
                        </asp:GridView>
                        </div>
                        
                    </div>
                </td>
            </tr>

            <tr>
                <td class="auto-style4">
                    <asp:GridView ID="grdUnidadesInsumo" runat="server" AutoGenerateColumns="False" CssClass="Grilla" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="Mes" HeaderText="Mes" />
                            <asp:BoundField DataField="Año1" HeaderText="Año1">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año2" HeaderText="Año2">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año3" HeaderText="Año3">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año4" HeaderText="Año4">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año5" HeaderText="Año5">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año6" HeaderText="Año6">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año7" HeaderText="Año7">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año8" HeaderText="Año8">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año9" HeaderText="Año9">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Año10" HeaderText="Año10">
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="auto-style4" style="background-color: #0489B1; color: white">
                    <b>El cálculo de insumos por unidad de producto final que se dificulten asociar su relación por unidad de producto final utilice el sistema<br /> de prorateo.</b>
                </td>
            </tr>

            <tr>
                <td class="auto-style4" style="text-align: right">
                    <div style="width:790px" >
                        <asp:Button ID="GuardarButton" runat="server" Text="Guardar" OnClick="GuardarButton_Click" ValidationGroup="validaGuardar" Height="27px" Width="82px" />
                    </div>
                </td>
            </tr>

        </table>
        <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
        <style type="text/css">
            .style13 {
                width: 193px;
                height: 23px;
            }

            .style15 {
                height: 23px;
            }

            .style16 {
                text-align: right;
                font-weight: bold;
                height: 23px;
            }

            .style17 {
                height: 28px;
                font-size: 13pt;
                color: #FFFFFF;
            }

            .auto-style3 {
                text-align: right;
                font-weight: bold;
                height: 23px;
                width: 141px;
            }

            .auto-style4 {
                width: 461px;
                text-align: left;
            }

            .auto-style5 {
                height: 23px;
                width: 132px;
            }

            .Grilla {
            }
        </style>
    </form>
</body>
</html>
