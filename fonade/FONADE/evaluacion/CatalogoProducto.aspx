<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoProducto.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoProducto" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" />
    <style type="text/css">
        html,body{
            background-color: #fff !important;
            background-image: none !important;
        }
        .inputText 
        {
           text-align:right;
        }
         .StyleColumn
        {
           width:9%;
           max-width:100px;
        } 
        .style10
        {
            width: 7%;
            height: 0px;
        }
        .style11
        {
            width: 299px;
        }
        .titulos
        {
            text-align: center;
            height: 20px;
        }
        .celda
        {
            text-align:center;
         }
                
        .Grilla {
            text-align: left;
        }
         /*#GridView1 tr:nth-child(14) td{
            background-color: #2980B9 !important;
            color: #fff !important;
        }

        #GridView1 tr:nth-child(14) td:nth-child(n+1) input[type="text"]{
            background-color: #2980B9 !important;
            color: #fff !important;
        }*/
    </style>
   <style type="text/css" media="screen">
	div.combobox	{font-family: Tahoma;font-size: 12px}
	div.combobox	{position: relative;zoom: 1}
	div.combobox	div.dropdownlist	{display: none;width: 200px;
		border: solid 1px #000;background-color: #fff;
		height: 200px;overflow: auto;position: absolute;
		top: 18px;left: 0px;}
	div.combobox	.dropdownlist	a	{display: block;text-decoration: none;
		color: #000;padding: 1px;height: 1em;cursor: default}
	div.combobox	.dropdownlist	a.light	{color: #fff;
		background-color: #007}
	div.combobox	.dropdownlist, input {font-family: Tahoma;font-size: 12px;
           height: 17px;
           width: 171px;
       }
	div.combobox	input {float: left;width: 182px;
		border: solid 1px #ccc;height: 16px
       }
	div.combobox	span	{border: solid 1px #ccc;background: #eee;
		width: 16px;height: 17px;
		float: left;text-align: center;border-left: none;cursor: default}
       .auto-style2 {
           width: 235px;
       }
       .auto-style3 {
           width: 11%;
       }
       .auto-style4 {
           width: 50%;
           height: 27px;
       }
       .auto-style5 {
           width: 11%;
           height: 27px;
       }
       .auto-style6 {
           width: 1175px;
       }
       .auto-style7 {
           height: 30px;
           }
       .auto-style8 {
           width: 80%;
           height: 30px;
       }
       #LinkButton1{
           text-decoration: none;
           text-align: center;
           height: 13px !important;
           top:0px;
       }
       input::-webkit-input-placeholder {
        color: red !important;
        font-weight:bold;
        }
 
        input:-moz-placeholder { /* Firefox 18- */
        color: red !important;  
        font-weight:bold;
        }
 
        input::-moz-placeholder {  /* Firefox 19+ */
        color: red !important;  
        font-weight:bold;
        }
 
        input:-ms-input-placeholder {  
        color: red !important;  
        font-weight:bold;
        }
       </style>
     <script type="text/javascript">
        function abrirBuscar() {
            open("ayudaArancel.aspx", "ayuda Arancel", "800px; 500px");
        }
    </script>

    <script type="text/javascript">
        var _lmtNdx = 0;
        var _qtt = 0;
        var _sum = 0;
        var _prc = 0;
        var ishndlr = false;
        var yhn = new Object();
       
        function ClientValidate(Obj)
        {
            var error = document.getElementById('divError')
            var TotalCol = document.getElementById('TotalYear').value;
            var TextName = String(Obj.id).substring(0, 17);
            var ColumnGrid = Obj.id.substring(17).split('_');
            var navigatorName = navigator.userAgent.toLowerCase();
            Obj.value = Obj.value.replace(',', '')
            if (!isNaN(Obj.value))
            {
               yhn = document.getElementById('GridView1');
                if (yhn != null)
                {
                    _qtt = 0;
                    _sum = 0;
                    _prc = 0;
                    var rdx = new String(Obj.id).substring(0, 17);
                    for (cde in yhn.rows){
                        var wsx = new Number(cde).valueOf();
                        switch (true)
                        {
                            case wsx > 0 && wsx < yhn.rows.length-2:
                                var mko = yhn.rows[wsx].cells[ColumnGrid[0]].children[0].value.replace(',', '');
                                _qtt = !isNaN(mko) ? new Number(mko) + _qtt : 0;
                                if (rdx.length > 0){
                                    rdx += ColumnGrid[0] + String.fromCharCode(95) + ++ColumnGrid[1];
                                    document.getElementById(rdx).select();

                                    document.getElementById(rdx).focus();
                                    rdx = new String();
                                }
                                break;
                            case wsx == yhn.rows.length-2:
                                var mko = yhn.rows[wsx].cells[ColumnGrid[0]].children[0].value.replace(',', '');
                                _prc += !isNaN(mko) ? new Number(mko) : 0;
                                break;
                            case wsx == yhn.rows.length-1:
                                var mko = yhn.rows[wsx].cells[ColumnGrid[0]].children[0].value.replace(',', '');;
                                 _sum += !isNaN(mko) ? new Number(mko) : 0;
                                break;
                        }
                        var mko = yhn.rows[14].cells[ColumnGrid[0]].children[0];
                        _sum = _prc * _qtt;
                         mko.value = _sum
                     }
                    if (ColumnGrid[1] == 13 && ColumnGrid[0]<TotalCol)
                    {
                        var IndexCol = ColumnGrid[0]
                        IndexCol =(IndexCol*1+1)*1
                        var lastFoscus = TextName + IndexCol.toString() + '_0'
                            document.getElementById(lastFoscus).focus();
                    }
                 
               }
            }
            else
            {
                error='Debe ingresar un valor numérico con punto Decimal(.)'
                Obj.value = 0.00;
                Obj.focus()
            }
            if (_sum > 0)
                MoneyFormat(mko)
            MoneyFormat(Obj)
        }
        function ValidateCampo(Obj, MaxValue)
        {
            var error = document.getElementById('divError');
            if (Obj.value > MaxValue) {
                error.innerHTML = 'Debe ingresar un valor menor a ' + MaxValue
                Obj.select()
                Obj.focus();
            }
            else
            {
                MoneyFormat(Obj)
            }
        }
        function MoneyFormat(Obj)
        {
            var Textvalue=Obj.value
            var Valortext = Textvalue.toString().split('.').length
            var FraccionInteger = "0"
            var FraccionDecimal = "00"
         if (Textvalue!="")
        {        if (Valortext == 1)
                {
                    if (Textvalue.toString().split('.')[0].substr(0, 1) != "0")
                    {
                        FraccionInteger = Textvalue.toString().split('.')[0]
                    }
                }
            if (Valortext == 2)
            {
                if (Textvalue.toString().split('.')[0].substr(0, 2) != "00" && Textvalue.toString().split('.')[0] != "")
                {
                    FraccionInteger = Textvalue.toString().split('.')[0]
                }
                if (Textvalue.toString().split('.')[1].substr(0, 2) != "00" && Textvalue.toString().split('.')[1] != "")
                {
                    FraccionDecimal = Textvalue.toString().split('.')[1].substr(0, 2)
                }
            }
            var longitud = FraccionInteger % 3
            var Resultado = ""
            var posInicial = 3
            var posFinal = FraccionInteger.length
            if (FraccionInteger.length > 3)
            {
                while (FraccionInteger.length >= 1)
                {
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
        function validarNro(e){
            var error = document.getElementById('divError');
            var  key=e.which
            switch (true)
            {
                case  key <= 44:
                    error.innerHTML = 'Ingrese punto(.) como signo decimal'
                    e.preventDefault();
                    break
                case key==13:
                    error.innerHTML = 'Para pasar al campo siguiente presione la tecla de Tabulación'
                     e.preventDefault();
                case key>57:
                    error.innerHTML ='Ingrese sólo números'
                    e.preventDefault();
                default:
                    error.innerHTML=''
                    break;
            }
        }
        function RangeValidatorValues(Obj, Min, Max)
        {
            var error = document.getElementById('divError');
            if (Obj.value < Min || Obj.value > Max)
            {
                error.innerHTML = 'El valor permitido para este campo debe estar entre:' + Min + ' y ' + Max
                Obj.value = 0.00;
                Obj.focus()
            }
            else
                error.innerHTML = ''
        }
        function ControlaSubmit(e) {
            var error = document.getElementById('divError');
            var key = e.which
            switch (true) {
                 case key == 13:
                    error.innerHTML = 'Para pasar al campo siguiente presione la tecla de Tabulación'
                    e.preventDefault();
                 break;
            }
        }
        function ValidaRequerido()
        {
            var msgError=""
            var ProductoServicio = document.getElementById('NombreProductoServicio').value;
            var PrecioLanzamiento = document.getElementById('PrecioLanzamiento').value;
            var Retencion = document.getElementById('Retencion').value;
            var Iva = document.getElementById('Iva');
            if (ProductoServicio=="")
            {
                msgError="El campo precio de lanzamiento es requerido. </br>"
            }
            if (PrecioLanzamiento == "")
            {
                msgError += "El campo PrecioLanzamiento es requerido. </br> "

            }
            if (Retencion == "")
            {
                msgError += "El campo Retencion es requerido. </br>"

            }
            if (Iva == "")
            {
                msgError += "El campo Iva es requerido. </br>"
            }
             return msgError
             
        }
    </script>
     <script type="text/javascript">
         function Arancelaria() {
             var hid = document.getElementById('hidCombo');
             hid.value = "1"
             document.forms["form1"].submit();
         }
         function GuardarDatos()
         {
             var error = document.getElementById('divError');

             var msgError=ValidaRequerido()
             if (msgError != "")
                 error.innerHTML = msgError
             else
             {
                 var hid = document.getElementById('hidCombo');
                 hid.value = "0"
                 document.forms["form1"].submit();
             }
         }
         function Regresar()
         {
             location.href = '../Proyecto/PProyectoOperacionCompras.aspx';
             location.href = pagina
         }
 </script>
</head>
<body>
    <form id="form1" runat="server"><div>
           <table style="width:100%; height: 169px;">
            <tr>
                <td class="auto-style3" >Nombre del Producto:</td>
                 <td class="auto-style2"  ><input type="text" id="NombreProductoServicio" runat="server"/></td>
            </tr>
            <tr>
                <td class="auto-style3" >Precio de Lanzamiento:</td>
                 <td class="auto-style2"  ><input type="text" id="PrecioLanzamiento" maxlength="9" onkeypress ="javascript:return validarNro(event)" onchange="ValidateCampo(this,999999999)" runat="server"/></td>
            </tr>
             <tr>
                <td class="auto-style5" >Valor Porcentaje(%) del IVA:</td>
                 <td class="auto-style4" ><input type="text" id="Iva" maxlength="5" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server"/></td>
            </tr>
             <tr>
                <td class="auto-style5" >%Retencion en la fuente:</td>
                 <td class="auto-style4" ><input type="text" id="Retencion" maxlength="5"  onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server"/></td>
            </tr>
             <tr>
                <td class="auto-style5" >P% De Ventas a Crédito:</td>
                 <td class="auto-style4" ><input type="text" id="VentasCredito" maxlength="5" onkeypress="javascript:return validarNro(event)" onchange="ValidateCampo(this,100)" runat="server"/></td>
            </tr>
             <tr>
                <td class="auto-style7" style="vertical-align:top">Posición Arancelaria:</td>
                 <td class="auto-style8" ><div class="combobox"><table><tr><td class="auto-style6"><input type="text" name="comboboxfieldname" id="cb_identifier" style="width:97%"  placeholder="Ingrese criterio de busqueda y presione  ENTER" onchange="Arancelaria()" runat="server"/><span>▼</span> 
	<div id="cmbArancelaria" class="dropdownlist" style="width:99%;" runat="server">   </div>  
</td></tr><tr><td class="auto-style6"></td></tr></table></div>
    </td>
            </tr>
             </table>
	   </div>
        <script  type="text/javascript" charset="utf-8"src="../../Scripts/Fonade/combobox-min.js"></script>
        <script  type="text/javascript" charset="utf-8"src="../../Scripts/Fonade/combobox-min.js"></script>
        <script type="text/javascript" charset="utf-8">
            var no = new ComboBox('cb_identifier');
        </script>
   
      <input id="hidCombo" type="hidden"  value="0" runat="server" />
       <div id="divError" style="color:red">
        </div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="odsProductos" EditIndex="0" SelectedIndex="0"  Height="136px" BorderStyle="None" >
        <Columns>
            <asp:TemplateField HeaderText="Periodos" >
                <ItemTemplate>
                    <asp:Label ID="PeriodosLabel" runat="server" Text='<%# Eval("Periodos") %>' ></asp:Label>
                </ItemTemplate>
                <ItemStyle BackColor="#F3F3F3" ForeColor="Black" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 1"  >
                <ItemTemplate>
                    <%if(ndx_>=1){%>
                    <asp:TextBox ID="TextBox1" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=1? Eval(string.Format("Año {0}",1)):string.Empty %>' AutoPostBack="false"  ></asp:TextBox>
                    <%} %>                            
                 </ItemTemplate>


                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 2" >
                <ItemTemplate>
                    <%if(ndx_>=2){%>
                    <asp:TextBox ID="TextBox2" MaxLength="10"  CssClass="inputText" runat="server" Text='<%# ndx_>=2? Eval(string.Format("Año {0}",2)):string.Empty %>' ></asp:TextBox>
                    <%} %>
                </ItemTemplate>

                <ItemStyle BorderStyle="None" />

            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 3" >
                <ItemTemplate>
                    <%if(ndx_>=3){%>
                    <asp:TextBox ID="TextBox3" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=3? Eval(string.Format("Año {0}",3)):string.Empty %>' ></asp:TextBox>
                    <%} %>
                </ItemTemplate>

                <ItemStyle BorderStyle="None" />

            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 4">
                <ItemTemplate>
                    <%if(ndx_>=4){%>
                    <asp:TextBox ID="TextBox4" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=4? Eval(string.Format("Año {0}",4)):string.Empty %>'></asp:TextBox>
                    <%} %>
                 </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 5">
                <ItemTemplate>
                    <%if(ndx_>=5){%>
                        <asp:TextBox ID="TextBox5" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=5? Eval(string.Format("Año {0}",5)):string.Empty %>' ></asp:TextBox>
                    <%} %>
                </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 6">
                <ItemTemplate>
                    <%if(ndx_>=6){%>
                    <asp:TextBox ID="TextBox6" MaxLength="10"  CssClass="inputText" runat="server" Text='<%# ndx_>=6? Eval(string.Format("Año {0}",6)):string.Empty %>' ></asp:TextBox>
                    <%} %>
                 </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 7">
                <ItemTemplate>
                    <%if(ndx_>=7){%>
                    <asp:TextBox ID="TextBox7" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=7 ?Eval(string.Format("Año {0}",7)):string.Empty %>'></asp:TextBox>
                    <%} %>
                </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 8">
                <ItemTemplate>
                    <%if(ndx_>=8){%>
                    <asp:TextBox ID="TextBox8" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=8 ?Eval(string.Format("Año {0}",8)):string.Empty %>'></asp:TextBox>
                    <%} %>
                 </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 9">
                <ItemTemplate>
                    <%if(ndx_>=9){%>
                    <asp:TextBox ID="TextBox9" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=9 ?Eval(string.Format("Año {0}",9)):string.Empty %>'></asp:TextBox>
                    <%} %>
                 </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Año 10">
                <ItemTemplate>
                    <%if(ndx_>=10){%>
                    <asp:TextBox ID="TextBox10" MaxLength="10" CssClass="inputText" runat="server" Text='<%# ndx_>=10 ?Eval(string.Format("Año {0}",10)):string.Empty %>'></asp:TextBox>
                    <%} %>
                </ItemTemplate>
                <ItemStyle BorderStyle="None" />
            </asp:TemplateField>
        </Columns>
        
              <HeaderStyle BackColor="#F3F3F3" ForeColor="Black" />
        
    </asp:GridView>
       <input id="btnGuardar" type="button" onclick="GuardarDatos()" value="Guardar" style="font-family: Arial; border:none; color: #FFFFFF; background-color: #000066; width:50px; height:13px; font-weight: bold; font-size: 9px;" />
       <asp:LinkButton ID="LinkButton1" runat="server" BackColor="#000066"  ForeColor="White" Width="50px" Height="16px" Font-Size="9px" Font-Bold="true" PostBackUrl="~/FONADE/Proyecto/PProyectoMercadoProyecciones.aspx">Regresar</asp:LinkButton>
<asp:ObjectDataSource ID="odsProductos" runat="server" SelectMethod="RegistroProducto" TypeName="Fonade.FONADE.evaluacion.CatalogoProducto" UpdateMethod="ActualizarRegistro"></asp:ObjectDataSource>
       </form>
    <p>
        <input id="TotalYear" type="hidden"  runat="server"/>
    </p>
</body>
</html>
