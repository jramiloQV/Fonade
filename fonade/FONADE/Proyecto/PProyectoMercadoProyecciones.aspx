<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoMercadoProyecciones.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoMercadoProyecciones" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>    
    <link href="../../Styles/EstilosEspecificos.css" rel="stylesheet" />
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        body {
            background-color: white;
        }

        .sinlinea {
            border: none;
            border-collapse: collapse;
        }

        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        .childContainer {
            width: 100%;
            height: auto;
        }

        html, body, div, iframe {
            /*height: 13% !important;*/
        }
    </style>
    <script type="text/ecmascript">
        function url() {
            open("../Ayuda/Mensaje.aspx", "Proyección de ventas", "width=500,height=200");
        }
        function OpenPage(strPage)
        {
            var ActivarVentana = document.getElementById("hidInsumo")
            if (ActivarVentana.value == "1") {
                window.open(strPage, 'Insumo', 'width=800px,height=500px')
                ActivarVentana.value = 0
            }
        }

        window.onload = function () {
            Realizado();
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
</head>
<body>
    <% Page.DataBind(); %>	
    <form id="form1" runat="server">    
        <table>
            <tbody>
                <tr>
                    <td> &nbsp; </td>
                    <td>ULTIMA ACTUALIZACIÓN: &nbsp; </td>
                    <td>
                        <asp:Label ID="lblUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" /> &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblFechaUltimaActualizacion" Text="" runat="server" ForeColor="#CC0000" />
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEsRealizado" Text="MARCAR COMO REALIZADO: &nbsp;&nbsp;&nbsp;&nbsp;" runat="server" TextAlign="Left" /> &nbsp;
                        <asp:Button ID="btnUpdateTab" Text="Guardar" runat="server" ToolTip="Guardar" OnClick="btnUpdateTab_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowCheckTab")) %>' />
                    </td>
                </tr>
            </tbody>
        </table>
        <div style="text-align:right; width:90%">
            <asp:ImageButton ID="ImageButton11" ImageUrl="../../Images/icoClip.gif" runat="server" Visible ='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'
                                    OnClick="ImageButton11_Click" />&nbsp;
            <asp:ImageButton ID="ImageButton22" ImageUrl="../../Images/icoClip2.gif" runat="server"
                                    OnClick="ImageButton22_Click" />
        </div>
        <table id="tabla_docs" runat="server" style="width:780px; text-align:right" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td style="width:80%">
                    <table style="width: 52px; border: 0" aling="right">
                        <tr>
                            <td style="width: 50px;">
                                
                            </td>
                            <td style="width: 138px;">
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <div>
            <table style="width: 100%;">
                <tr>
                    <td style="width: 100%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Proyección de ventas', texto: 'ProyeccionVentas'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento" />
                            </div>
                            &nbsp;<div>
                                Proyección de ventas:
                            </div>
                        </div>
                    </td>
                    <td style="width: 100%">
                        <div id="div_post_it_1" runat="server" visible="false">
                            <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="ProyeccionVentas" _txtTab="1" 
                                visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>' 
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                    </td>
                </tr>
            </table>
            <table style="width: 98%; border: 0; border-collapse: separate; padding: 4px;">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="L_FechIniPro" runat="server" Text="Fecha de Inicio del Proyecto" />
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="DDL_Dia" runat="server" ValidationGroup="grabar" AppendDataBoundItems="True" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                                <asp:ListItem Value="1">1</asp:ListItem>
                                <asp:ListItem Value="2">2</asp:ListItem>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                                <asp:ListItem Value="13">13</asp:ListItem>
                                <asp:ListItem Value="14">14</asp:ListItem>
                                <asp:ListItem Value="15">15</asp:ListItem>
                                <asp:ListItem Value="16">16</asp:ListItem>
                                <asp:ListItem Value="17">17</asp:ListItem>
                                <asp:ListItem Value="18">18</asp:ListItem>
                                <asp:ListItem Value="19">19</asp:ListItem>
                                <asp:ListItem Value="20">20</asp:ListItem>
                                <asp:ListItem Value="21">21</asp:ListItem>
                                <asp:ListItem Value="22">22</asp:ListItem>
                                <asp:ListItem Value="23">23</asp:ListItem>
                                <asp:ListItem Value="24">24</asp:ListItem>
                                <asp:ListItem Value="25">25</asp:ListItem>
                                <asp:ListItem Value="26">26</asp:ListItem>
                                <asp:ListItem Value="27">27</asp:ListItem>
                                <asp:ListItem Value="28">28</asp:ListItem>
                                <asp:ListItem Value="29">29</asp:ListItem>
                                <asp:ListItem Value="30">30</asp:ListItem>
                                <asp:ListItem Value="31">31</asp:ListItem>
                                <asp:ListItem Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                        <asp:DropDownList ID="DDL_Mes" runat="server" ValidationGroup="grabar" AppendDataBoundItems="True" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                            <asp:ListItem Value="01">Ene</asp:ListItem>
                            <asp:ListItem Value="02">Feb</asp:ListItem>
                            <asp:ListItem Value="03">Mar</asp:ListItem>
                            <asp:ListItem Value="04">Abr</asp:ListItem>
                            <asp:ListItem Value="05">May</asp:ListItem>
                            <asp:ListItem Value="06">Jun</asp:ListItem>
                            <asp:ListItem Value="07">Jul</asp:ListItem>
                            <asp:ListItem Value="08">Ago</asp:ListItem>
                            <asp:ListItem Value="09">Sep</asp:ListItem>
                            <asp:ListItem Value="10">Oct</asp:ListItem>
                            <asp:ListItem Value="11">Nov</asp:ListItem>
                            <asp:ListItem Value="12">Dic</asp:ListItem>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                            &nbsp;
                        <asp:DropDownList ID="DD_Anio" runat="server" ValidationGroup="grabar" AppendDataBoundItems="True" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                            <asp:ListItem Value="2004">2004</asp:ListItem>
                            <asp:ListItem Value="2005">2005</asp:ListItem>
                            <asp:ListItem Value="2006">2006</asp:ListItem>
                            <asp:ListItem Value="2007">2007</asp:ListItem>
                            <asp:ListItem Value="2008">2008</asp:ListItem>
                            <asp:ListItem Value="2009">2009</asp:ListItem>
                            <asp:ListItem Value="2010">2010</asp:ListItem>
                            <asp:ListItem Value="2011">2011</asp:ListItem>
                            <asp:ListItem Value="2012">2012</asp:ListItem>
                            <asp:ListItem Value="2013">2013</asp:ListItem>
                            <asp:ListItem Value="2014">2014</asp:ListItem>
                            <asp:ListItem Value="2015">2015</asp:ListItem>
                            <asp:ListItem Value="2016">2016</asp:ListItem>
                            <asp:ListItem Value="2017">2017</asp:ListItem>
                            <asp:ListItem Value="2018">2018</asp:ListItem>
                            <asp:ListItem Value="2019">2019</asp:ListItem>
                            <asp:ListItem Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="L_Tamperi" runat="server" Text="Tamaño del Período" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DD_Periodo" runat="server" ValidationGroup="grabar" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                                <asp:ListItem Value="1">Mes</asp:ListItem>
                                <asp:ListItem Value="2">Bimestre</asp:ListItem>
                                <asp:ListItem Value="3">Trimestre</asp:ListItem>
                                <asp:ListItem Value="4">Semestre</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="L_TiemPro" runat="server" Text="Tiempo de Proyección" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server" ValidationGroup="grabar" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                                <asp:ListItem Value="3">3</asp:ListItem>
                                <asp:ListItem Value="4">4</asp:ListItem>
                                <asp:ListItem Value="5">5</asp:ListItem>
                                <asp:ListItem Value="6">6</asp:ListItem>
                                <asp:ListItem Value="7">7</asp:ListItem>
                                <asp:ListItem Value="8">8</asp:ListItem>
                                <asp:ListItem Value="9">9</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="L_MetPro" runat="server" Text="Método de Proyección" />
                        </td>
                        <td>
                            <asp:DropDownList ID="DD_MetProy" runat="server" ValidationGroup="grabar" OnSelectedIndexChanged="DD_MetProy_SelectedIndexChanged" AppendDataBoundItems="false" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'>
                                <asp:ListItem Value="Lineal">Lineal</asp:ListItem>
                                <asp:ListItem Value="Exponencial">Exponencial</asp:ListItem>
                                <asp:ListItem Value="Logarítmico">Logarítmico</asp:ListItem>
                                <asp:ListItem Value="Promedios Móviles">Promedios Móviles</asp:ListItem>
                                <asp:ListItem Value="Promedios Móviles Suavizados">Promedios Móviles Suavizados</asp:ListItem>
                                <asp:ListItem Value="Sistema Winters">Sistema Winters</asp:ListItem>
                                <asp:ListItem Value="Otro">Otro</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2" id="td_otroMedio" runat="server" visible="false">
                            <asp:TextBox ID="OtroMetodo" runat="server" size="30" MaxLength="30" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="L_CostoVenta" runat="server" Text="Costo de Venta" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="TB_CostoVenta" runat="server" Width="368px" MaxLength="100" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />&nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Justificación de Proyección de Ventas', texto: 'JustificaProyeccion'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento" />&nbsp;Justificación
                            de Proyección de Ventas:
                            </div>
                        </div>
                    </td>
                    <td style="width: 100%">
                        <div id="div_post_it_2" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="JustificaProyeccion" _txtTab="1"
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                    </td>
                </tr>
            </table>
            <CKEditor:CKEditorControl ID="TB_JusProVen" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>        
            <table style="width: 100%">
                <tr>
                    <td style="width: 100%">
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Política de Cartera', texto: 'PoliticaCartera'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_EstrategiasAprovisionamiento" />&nbsp;Política
                            de Cartera:
                            </div>
                        </div>
                    </td>
                    <td style="width:100%;">
                        <div id="div_post_it_3" runat="server" visible='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'>
                            <uc1:Post_It ID="Post_It3" runat="server" _txtCampo="PoliticaCartera" _txtTab="1"
                                _mostrarPost='<%# ((bool)DataBinder.GetPropertyValue(this, "PostitVisible")) %>'/>
                        </div>
                    </td>
                </tr>
            </table>
            <CKEditor:CKEditorControl ID="TB_PoliCarte" runat="server" EnableTabKeyTools="true" ForcePasteAsPlainText="false" BasePath="~/ckeditor" Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></CKEditor:CKEditorControl>            
            <br />
            <asp:Button ID="B_Guardar" runat="server" Text="Guardar" OnClick="B_Guardar_Click" 
                ValidationGroup="grabar" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
            <br />
            <br />            
            <asp:ImageButton ID="IB_AgregarProductoServicio" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                OnClick="IB_AgregarProductoServicio_Click" Visible="true"/>&nbsp;
            <asp:LinkButton ID="B_AgregarProductoServicio" runat="server" Text="Adicionar Producto o Servicio"
                    Font-Bold="true" OnClick="B_AgregarProductoServicio_Click" Visible="true"/>
            <asp:Panel ID="pnl_Datos" runat="server" Width="100%">
                <asp:GridView ID="GV_productoServicio" runat="server" AllowPaging="True" AutoGenerateColumns="False" 
                    CellSpacing="1" CssClass="Grilla"  EmptyDataText="No hay datos." Enabled='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'
                    Width="100%" DataKeyNames="Id_Producto,NomProducto" OnPageIndexChanging="GV_productoServicio_PageIndexChanging">
                    <HeaderStyle HorizontalAlign="Left" />
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="3%" ShowHeader="False" >
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" OnClientClick="return confirm('Esta seguro que desea borrar el producto seleccionado?');"
                                    Text="">
                                    <asp:Image ID="I_imagen" runat="server" ImageUrl="~/Images/icoBorrar.gif" CssClass="sinlinea" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'/>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto o Servicio" ItemStyle-HorizontalAlign="Left"
                            ItemStyle-Width="38%">
                            <ItemTemplate>
                                <asp:Label ID="LB_Producto" runat="server"  Text='<%#Eval("NomProducto")%>' />
                         </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="12%" Visible="false" />
                        <asp:BoundField DataField="PosicionArancelaria" HeaderText="Posición Arancelaria"
                            ItemStyle-HorizontalAlign="Right" ItemStyle-Width="12%" />
                        <asp:BoundField DataField="PorcentajeRetencion" HeaderText="RTF" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="5%" />
                        <asp:BoundField DataField="PorcentajeIva" HeaderText="IVA" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="5%" />
                        <asp:TemplateField HeaderText="Precio Inicial" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="10%">
                            <ItemTemplate>
                                <asp:Label ID="lbl_PrecioInicial" runat="server" Text='<%#String.Format("{0:N2}", Eval("PrecioLanzamiento") ) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PorcentajeVentasContado" HeaderText="%Contado" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="10%" />
                        <asp:BoundField DataField="PorcentajeVentasPlazo" HeaderText="%Crédito" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Width="10%" />
                        <asp:TemplateField HeaderText="Insumo" ItemStyle-HorizontalAlign="Right" >
                            <ItemTemplate>
                                <asp:Label ID="LB_Insumo" runat="server"  Text='<%#Eval("AdicionarInsumo")%>'  Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <br />
            </asp:Panel>
        </div>
       <input id="hidInsumo" name="hidInsumo" type="hidden" value="1" />
       </form>
</body>
</html>
