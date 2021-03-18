<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VentasPorMes.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.VentasPorMes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Fondo emprender - Proyecciones de ventas </title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>   
    <script src="../../../Scripts/jquery-ui-1.8.21.custom.min.js" type="text/javascript"></script>
    <script src="http://code.jquery.com/jquery-migrate-1.2.1.js"></script>
    <script src="../../../Scripts/common.js" type="text/javascript"></script>    
    <script type="text/javascript" src="../../../Scripts/jquery.number.min.js"></script>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px !important;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .parentContainer {
            width: 100%;
            height: 650px;
            overflow-x: hidden;
            overflow-y: visible;
        }

        .childContainer {
            width: 100%;
            height: auto;
        }

        html, body, div, iframe {            
        }
    </style>      

     <script type="text/javascript">     
         var acciones = {

             init: function () {                 
                 acciones.clicks();
                 $('.money').number(true, 0);
             },

             clicks: function () {                                               
                 debugger;
                 for (var i = 1; i <= 10; i++) {
                     var ua = window.navigator.userAgent;
                     var msie = ua.indexOf("MSIE ");
                     if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {                                                  

                         $('.Year' + i + ',' + '.YearPrice' + i).keyup(function () {                             
                             $(this).change();
                         });

                         $('.Year' + i + ',' + '.YearPrice' + i).change(
                            (function (yearCounter) {
                                return function () {
                                    acciones.calcular(".Year" + yearCounter, ".YearPrice" + yearCounter, ".YearTotal" + yearCounter);
                                };
                            })(i)
                         );
                     } else {
                         $('.Year' + i + ',' + '.YearPrice' + i).change(
                            (function (yearCounter) {
                                return function () {
                                    acciones.calcular(".Year" + yearCounter, ".YearPrice" + yearCounter, ".YearTotal" + yearCounter);
                                };
                            })(i)
                         );
                     }                                                              
                 }
             },
            
             calcular: function (txtCantidades,txtPrecio,txtTotal) {                                  
                 var sum = 0;
                 $(txtCantidades).each(function () {
                     var value = $(this).val();
                     if (!isNaN(value) && value.length != 0) {
                         sum += parseFloat(value);
                     }
                 });
                 var priceText = $(txtPrecio).val();

                 if (!isNaN(priceText) && priceText.length != 0) {
                     var price = parseFloat(priceText);
                     var total = sum * price;
                     $(txtTotal).val(total);
                 } else {
                     $(txtTotal).val(0);
                 }
             }
         }

         $(document).on('ready', acciones.init);

         function alerta() {
             var totalYear1 = parseFloat($(".YearTotal1").val());
             var totalText2 = parseFloat($(".YearTotal2").val());
             var totalText3 = parseFloat($(".YearTotal3").val());
             var totalText4 = parseFloat($(".YearTotal4").val());
             var totalText5 = parseFloat($(".YearTotal5").val());
             var totalText6 = parseFloat($(".YearTotal6").val());
             var totalText7 = parseFloat($(".YearTotal7").val());
             var totalText8 = parseFloat($(".YearTotal8").val());
             var totalText9 = parseFloat($(".YearTotal9").val());
             var totalText10 = parseFloat($(".YearTotal10").val());
             
             if (!(totalYear1 > 0 || totalText2 > 0 || totalText3 > 0 || totalText4 > 0 || totalText5 > 0
                   || totalText6 > 0 || totalText7 > 0 || totalText8 > 0 || totalText9 > 0 || totalText10 > 0)) {

                 alert("Debe ingresar información de proyección de cantidades a vender para al menos un año.");
                 return false;
             } else {                 
                 return true;
             }
         }
    </script>
</head>
<body>
    <% Page.DataBind(); %>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>                                                      
            <table style="width:1000px; border=0;" cellspacing='0' cellpadding='3'>
                <tr style="vertical-align:top">
                    <td width="350px;" align="right">
                    </td>
                    <td >
                        <h3>
                            <asp:Label Text="Proyección de ventas" runat="server" ID="lblCreate" />
                        </h3>
                    </td>               
                </tr>   
                                             
                <tr style="vertical-align:top">
                    <td align="right">
                        <b>Producto:</b>                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreProducto" Enabled="false" runat="server" Width="300" />
                    </td>
                </tr>
                       
                <tr style="vertical-align:top">
                    <td align="right">
                        <b>Unidad de medida :</b>                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtUnidadMedida" runat="server" Enabled="false" Height="20" Width="300" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>

                <tr style="vertical-align:top">
                    <td align="right">
                        <b>Forma de pago (Contado / Credito) :</b>
                        <br />
                        <label> Nota : Si la forma de pago es crédito por favor  señale los días de plazo. </label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFormaDePago" runat="server" Height="20" Width="300" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>

                <tr style="vertical-align:top">
                    <td align="right">
                        <b> Justificación :</b>                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtJustificacion" runat="server" Height="20" Width="300" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>

                <tr style="vertical-align:top">
                    <td align="right">
                        <b>IVA :</b>                    
                    </td>
                    <td>
                        <asp:TextBox ID="txtIva" runat="server" CssClass="money" Width="300" MaxLength="2" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td>
                    </td>
                    <td >                    
                        <asp:Button ID="btnUpdate" runat="server" Text="Guardar" align="center" OnClick="btnUpdate_Click" OnClientClick="return alerta();" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" align="center" OnClick="btnCancel_Click" Visible="false" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td colspan="2">
                        <asp:GridView ID="gvVentasPorMes" runat="server"  Width="100%" AutoGenerateColumns="False" CssClass="Grilla" AllowPaging="false" AllowSorting="false" DataSourceID="dataVentasPorMes" >
                            <Columns>                                                     
                                <asp:TemplateField HeaderText="Periodo" >
                                    <ItemTemplate>                                    
                                        <asp:Label ID="lblPeriodo" runat="server" Text='<%# Eval("Periodo") %>' ></asp:Label>                                    
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />                                
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 1"  >
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear1" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year1.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year1.Unidades") %>'  > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 2" >
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear2" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year2.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year2.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 3" >
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear3" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year3.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year3.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 4" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear4" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year4.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year4.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 5" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear5" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year5.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year5.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 6" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear6" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year6.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year6.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 7" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear7" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year7.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year7.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 8" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear8" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year8.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year8.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 9" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear9" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year9.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year9.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Año 10" Visible="true">
                                    <ItemTemplate>                                    
                                        <asp:TextBox ID="txtYear10" Enabled='<%# Eval("UnLock") %>' CssClass='<%# "money " + Eval("CssClass").ToString() + Eval("Year10.Ano").ToString() %>' runat="server" Width="90px" CausesValidation="False" Text='<%# Eval("Year10.Unidades") %>' > </asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="90px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>                                        
                    </td>
                </tr>

                <tr style="vertical-align:top">
                    <td colspan="2">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False" Font-Bold="True" Font-Size="Large" >Sucedio un error inesperado, intentalo de nuevo.</asp:Label>
                        <asp:updateprogress id="UpdateProgress4" runat="server" associatedupdatepanelid="UpdatePanel1" dynamiclayout="true" >
                        <progresstemplate>
                            <div class="form-group center-block">                                                                 
                                <div class="col-xs-4">
                                </div>
                                <div class="col-xs-4">
                                    <label class="control-label"> <b> Actualizando información </b> </label>
                                    <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />                                  
                                </div>
                            </div>                                                                
                        </progresstemplate>
                </asp:updateprogress>
                    </td>
                </tr>
                
            </table>

            <asp:ObjectDataSource
                    ID="dataVentasPorMes"
                    runat="server"
                    TypeName="Fonade.PlanDeNegocioV2.Formulacion.DesarrolloSolucion.VentasPorMes"
                    SelectMethod="Get"
                    EnableCaching="false"
                >
                <SelectParameters>
                    <asp:querystringparameter name="CodigoProducto" querystringfield="codproducto" defaultvalue="0" />
                </SelectParameters>  
            </asp:ObjectDataSource>   
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
