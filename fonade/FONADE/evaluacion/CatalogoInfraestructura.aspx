<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CatalogoInfraestructura.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoInfraestructura" Culture="es-CO" UICulture="es-CO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <title>Fonade - Catalogo Infraestructura</title>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <style type="text/css">
        #bodyContentPlace_CalendarExtender1_container{
            height: auto !important;
        }
    </style>
    <script type="text/jscript">
        var setFormatFor = function (element){
            element.value = element.value.replace(',', '');
            if (isNaN(element.value.replace(',', ''))) return false;
            element.value = new Number(element.value).toLocaleString("es-CO");
        }
    </script>
   <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True" EnableScriptGlobalization="true" EnableScriptLocalization="true">
    </ajaxToolkit:ToolkitScriptManager>

    

    <table align="center" width="60%">
        <tr>
            <td>
                &nbsp;
            </td>
            <td class="style12">
                &nbsp;
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" colspan="2">
                <h1>
                    <asp:Label runat="server" ID="lbltitle" Style="font-weight: 300">Catalogo Infraestructura</asp:Label>
                </h1>
            </td>
            <td align="center">
                <asp:Label ID="lblfecha" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12" colspan="3" align="center">
                <asp:ValidationSummary ID="ValidationSummary1" HeaderText="Llene los Siguientes Campos"
                    runat="server" ForeColor="Red" ValidationGroup="crear" BackColor="White" />
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                &nbsp;
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label1" runat="server" Text="Nombre"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="requiredFieldValidator">
                <asp:TextBox ID="TxtNombre" runat="server" Width="335px"></asp:TextBox>
                &nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TxtNombre"
                    CssClass="requiredFieldValidator" ErrorMessage="Nombre" ValidationGroup="crear"
                    Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label2" runat="server" Text="Tipo Infraestructura"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td class="requiredFieldValidator">
                <asp:DropDownList ID="DdlTpoInfraestructura" runat="server" Height="25px" Width="335px">
                </asp:DropDownList>
                &nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DdlTpoInfraestructura"
                    CssClass="requiredFieldValidator" ErrorMessage="Tipo Infraestructura" ValidationGroup="crear"
                    Display="Dynamic" InitialValue="0" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label3" runat="server" Text="Unidad Tipo Infraestructura"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtUnidadTipo" runat="server" Width="158px" MaxLength="10" />
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label4" runat="server" Text="Valor Unitario"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtValorU" runat="server" Width="158px" MaxLength="12" />
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label5" runat="server" Text="Cantidad"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtCantidad" runat="server" Width="157px" MaxLength="6" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                    Enabled="True" TargetControlID="TxtCantidad" ValidChars="1234567890" FilterType="Numbers">
                </ajaxToolkit:FilteredTextBoxExtender>
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label6" runat="server" Text="Fecha De Compra"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtFecha" runat="server" Width="157px" Enabled="True" />
                <asp:Image runat="server" ID="btnDate2" AlternateText="cal2" ImageUrl="/images/icoModificar.gif">
                </asp:Image>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" TargetControlID="TxtFecha" Format="dd/MM/yyyy"
                    runat="server" PopupButtonID="btnDate2" />
                &nbsp;
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TxtFecha"
                    CssClass="requiredFieldValidator" ErrorMessage="La Fecha De Compra" ValidationGroup="crear"
                    Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label7" runat="server" Text="Porcentaje de Crédito"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="TxtPorcentaje" runat="server" Width="157px" MaxLength="3" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                <asp:Localize ID="Localize1" runat="server"></asp:Localize>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                    Enabled="True" TargetControlID="TxtPorcentaje" ValidChars="1234567890" FilterType="Numbers">
                </ajaxToolkit:FilteredTextBoxExtender>
                &nbsp;
                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="TxtPorcentaje" ErrorMessage="Valores permitidos de 0 - 100" ForeColor="Red" MaximumValue="100" MinimumValue="0" Type="Integer"></asp:RangeValidator>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label8" runat="server" Text="Periodo de Amoritzación"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:DropDownList ID="DdlPeriodo" runat="server" Height="21px" Width="87px">
                    <asp:ListItem Selected="True">0</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style11">
                &nbsp;
            </td>
            <td class="style12">
                <asp:Label ID="Label9" runat="server" Text="Sistema de Depreciación"></asp:Label>
            </td>
            <td class="style13">
                &nbsp;
            </td>
            <td>
                <asp:TextBox ID="Txtsistema" runat="server" Width="342px"></asp:TextBox>
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="style14">
            </td>
            <td class="style15">
            </td>
            <td class="style16">
            </td>
            <td class="style17">
                <asp:Button ID="BtnCrear" runat="server" Text="Crear" ValidationGroup="crear" OnClick="BtnCrear_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="BtnCancelar" runat="server" Text="Cancelar" OnClick="BtnCancelar_Click" />
            </td>
            <td class="style17">
            </td>
        </tr>
        <tr>
            <td class="style14">
                &nbsp;
            </td>
            <td class="style15">
                &nbsp;
            </td>
            <td class="style16">
                &nbsp;
            </td>
            <td class="style17">
                &nbsp;
            </td>
            <td class="style17">
                &nbsp;
            </td>
        </tr>
    </table>
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
<%--    <script type="text/javascript">setFormatFor(document.getElementById('bodyContentPlace_TxtValorU'));</script>--%>
</asp:Content>