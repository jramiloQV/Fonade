<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoProducto2.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoProducto2"
    ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-1.7.2.min.js"></script>
    <style type="text/css">
        
         .AlineadoDerecha {
             text-align: right;
         }
        .auto-style1 {
            width: 346px;
        }
        #txtPosicionAlancelaria {
            width: 502px;
        }

        .ui-autocomplete {
            position: absolute;
            cursor: default;
            z-index: 99999999999999 !important;
            /*max-height: 100px;*/
            height: 200px; 
            overflow-y: scroll;
            width: 504px;
            overflow-x: hidden;
        }
    </style>

    <script type="text/javascript">
        var inputs = $("#tblProyecionVentas :input");
        inputs.on("keypress keyup", function (e) {
            if (e.keyCode === 13) {
                var $this = $(this),
                    index = $this.closest('td').index();

                $this.closest('tr').next().find('td').eq(index).find('input').focus();
                e.preventDefault();
                e.stopPropagation();
            }
        });
        $(document).ready(function () {
            //iterate through each textboxes and add keyup
            //handler to trigger sum event
            $(".txt").each(function () {
                $(this).keyup(function () {
                    calculateSum();
                });
            });
        });

        $(document).ready(function () {
            //iterate through each textboxes and add keyup
            //handler to trigger sum event
            $(".txtTol").each(function () {
                $(this).keyup(function () {
                    calculateSum();
                });
            });
        });

        function calculateSum() {
            var sumQ = [];
            var col = (document.getElementById('tblProyecionVentas').rows[0].cells.length - 1);
            for (var i = 1; i <= col; i++) {
                sumQ[i] = 0;
                $('td:nth-child(' + (i + 1) + ')').find(".txt").each(function () {
                    if (!isNaN(this.value) && this.value.length !== 0) {
                        sumQ[i] += parseFloat(this.value);
                    }
                });
                var nombre = 'txtPrecioAnio' + i;
                var precio = document.getElementById(nombre).value;
                var total = Number(sumQ[i]) * Number(precio);
                var nomTotal = 'lblAnio' + i;
                document.getElementById(nomTotal).innerText = total.toFixed(2);
            }
        }

        $(document).ready(function () {
            $("#txtArancelPosicion").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: '<%=ResolveUrl("CatalogoProducto2.aspx/BuscarPosicionAlancelaria") %>',
                        data: "{'criterio':'" + $("#txtArancelPosicion").val() + "'}",
                        dataType: "json",
                        delay: 1000,
                        minLength: 4,
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (response) {
                            alert("Error" + res.responseText);
                        }
                    });
                }
            });
        });

        function validarCampos() {
            var flag = true;
            var mensaje = 'Debe ingresar ';
            if ($('#NombreProductoServicio').val().trim() === '') {
                mensaje += 'eL nombre del producto/servicio!';
                flag = false;
            }
            if ($('#PrecioLanzamiento').val().trim() === '') {
                mensaje += 'el precio de lanzamiento!';
                flag = false;
            }
            if ($('#Iva').val().trim() === '') {
                mensaje += 'el precio del IVA!';
                flag = false;
            }
            if ($('#Retencion').val().trim() === '') {
                mensaje += 'el precio de la reteción en la fuente!';
                flag = false;
            }
            if ($('#VentasCredito').val().trim() === '') {
                mensaje += 'el % de ventas a credito!'; 
                flag = false;
            }
            if ($('#txtArancelPosicion').val().trim() === '') {
                mensaje += 'la posición alancelaria!'; 
                flag = false;
            }

            if (!flag) {
                alert(mensaje);
                return flag;
            }
        }
    </script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width: 100%; height: 169px;">
                <tr>
                    <td class="auto-style1">Nombre del Producto:</td>
                    <td class="auto-style2">
                        <input type="text" id="NombreProductoServicio" runat="server"  /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Precio de Lanzamiento:</td>
                    <td class="auto-style2">
                        <input type="text" id="PrecioLanzamiento" maxlength="9" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,999999999)" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">Valor Porcentaje(%) del IVA:</td>
                    <td class="auto-style4">
                        <input type="text" id="Iva" maxlength="5" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">%Retencion en la fuente:</td>
                    <td class="auto-style4">
                        <input type="text" id="Retencion" maxlength="5" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1">% De Ventas a Crédito:</td>
                    <td class="auto-style4">
                        <input type="text" id="VentasCredito" maxlength="5" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server" /></td>
                </tr>
                <tr>
                    <td class="auto-style1" style="vertical-align: top">Posición Arancelaria:</td>
                    <td class="auto-style8">
                        <asp:TextBox runat="server" ID="txtArancelPosicion" Width="500" style="z-index:999 !important;" placeholder="Ingrese el criterio de busqueda y seleccionar una opción"/>
                        <%--<input type="text" id="txtPosicionAlancelaria" runat="server" />--%>
                        <%--<div class="combobox">
                            <table>
                                <tr>
                                    <td class="auto-style6">
                                        <input type="text" name="comboboxfieldname" id="cb_identifier" style="width: 97%" placeholder="Ingrese criterio de busqueda y presione  ENTER" onchange="Arancelaria()" runat="server" /><span>▼</span>
                                        <div id="cmbArancelaria" class="dropdownlist" style="width: 99%;" runat="server"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style6"></td>
                                </tr>
                            </table>
                        </div>--%>
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript" charset="utf-8" src="../../Scripts/Fonade/combobox-min.js"></script>
        <script type="text/javascript" charset="utf-8" src="../../Scripts/Fonade/combobox-min.js"></script>
        <script type="text/javascript" charset="utf-8">
            var no = new ComboBox('cb_identifier');
        </script>

        <input id="hidCombo" type="hidden" value="0" runat="server" />
        <div id="divError" style="color: red">
        </div>
        <div style="overflow-x: auto; text-align: right">
            <asp:Table ID="tblProyecionVentas" runat="server" Width="100%" CssClass="Grilla" ClientIDMode="Static">
            </asp:Table>
        </div>
        <div style="text-align: right">
            <asp:Button runat="server" ID="btnGrabar" Text="Crear" OnClick="btnGrabar_Click" OnClientClick="return validarCampos()"  />
            <asp:Button runat="server" ID="btnRgresa" OnClientClick="javascript:history.go(-1);" Text="Regresar" />
        </div>
        <%--<input id="btnGuardar" type="button" onclick="GuardarDatos()" value="Guardar" style="font-family: Arial; border: none; color: #FFFFFF; background-color: #000066; width: 50px; height: 13px; font-weight: bold; font-size: 9px;" />
        <asp:LinkButton ID="LinkButton1" runat="server" BackColor="#000066" ForeColor="White" Width="50px" Height="16px" Font-Size="9px" Font-Bold="true" PostBackUrl="~/FONADE/Proyecto/PProyectoMercadoProyecciones.aspx">Regresar</asp:LinkButton>
        <asp:ObjectDataSource ID="odsProductos" runat="server" SelectMethod="RegistroProducto" TypeName="Fonade.FONADE.evaluacion.CatalogoProducto" UpdateMethod="ActualizarRegistro"></asp:ObjectDataSource>--%>
    </form>
    <p>
        <input id="TotalYear" type="hidden" runat="server" />
    </p>
</body>
</html>
