<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="IndicadorGestion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.IndicadoresDeGestion.IndicadorGestion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
    <script type="text/javascript">
        function CheckOtherIsCheckedByGVID(rb) {
            var isChecked = rb.checked;
            var row = rb.parentNode.parentNode;
            debugger;
            var currentRdbID = rb.id;
            parent = document.getElementById("<%= gvProductos.ClientID %>");
            var items = parent.getElementsByTagName('input');

            for (i = 0; i < items.length; i++) {
                if (items[i].id != currentRdbID && items[i].type == "radio" && items[i].value != "rdProductoSeleccionado") {
                    if (items[i].checked) {
                        items[i].checked = false;
                    }
                }
            }
        }
    </script>
   
    <script type="text/javascript">
        function format(input) {
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>

            <% Page.DataBind(); %>
            <uc1:EncabezadoEval runat="server" ID="EncabezadoEval" />
            <br />
            <div style="width: 99%">
                <table id="gvMain" class="auto-style2 Grilla" runat="server" style="width: 100%;">
                    <tr>
                        <th colspan="3">Indicadores</th>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label8" Font-Bold="true" runat="server" Text="Indicador"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="Label9" Font-Bold="true" runat="server" Text="Datos del emprendedor"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="Label10" Font-Bold="true" runat="server" Text="Datos evaluador"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label11" Font-Bold="true" runat="server" Text="Ejecución presupuestal"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblEjecucionPresupuestal" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblEjecucionPresupuestalEvaluador" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label12" Font-Bold="true" runat="server" Text="IDH"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblIdh" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblIdhEvaluador" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label13" Font-Bold="true" runat="server" Text="Contrapartidas"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblContrapartidas" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblContrapartidasEvaluador" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label16" Font-Bold="true" runat="server" Text="Ventas"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblVentasProductos" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtVentasProductosEvaluador" 
                                onkeyup="format(this)" onchange="format(this)" MaxLength="11"                                 
                                runat="server" Width="300px" Enabled='<%# AllowUpdate %>'></asp:TextBox>
                            <!--<input type="text" onkeyup="format(this)" onchange="format(this)">-->
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label14" Font-Bold="true" runat="server" Text="Periodo Improductivo"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblPeriodoImproductivo" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtPeriodoImproductivoEvaluador" onkeyup="format(this)" onchange="format(this)"
                                MaxLength="2" runat="server" Width="300px" Enabled='<%# AllowUpdate %>'></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Label ID="Label15" Font-Bold="true" runat="server" Text="Recursos aportados emprendedor"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:Label ID="lblRecursosAportados" Font-Bold="false" runat="server" Text="N/A"></asp:Label>
                        </td>
                        <td class="auto-style3">
                            <asp:TextBox ID="txtRecursosAportadosEvaluador" runat="server" Enabled='<%# AllowUpdate %>' 
                                  onkeyup="format(this)" onchange="format(this)" MaxLength="11"
                               Width="300px" ></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />

                <h3 style="color: white;text-align: center;background-color: #00468f;margin-bottom: 0px;">PRODUCCIÓN</h3>
                <asp:GridView ID="gvProductos" Visible="true" runat="server" DataSourceID="data" 
                    AutoGenerateColumns="False" CssClass="Grilla" Width="100%" 
                    EmptyDataText="No existen productos para mostrar." AllowPaging="false" AllowSorting="false" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Producto representativo">
                            <ItemTemplate>
                                <div style="word-wrap: break-word;">
                                    <asp:RadioButton ID="rdProductoSeleccionado" CssClass="ignoreRadioButton" Text="" runat="server" Checked='<%# (bool)Eval("Selected") %>' Enabled="false" />
                                    <asp:HiddenField runat="server" ID="hdCodigoProducto" Value='<%# Eval("IdProducto") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto representativo Evaluación">
                            <ItemTemplate>
                                <div style="word-wrap: break-word;">
                                    <asp:RadioButton ID="rdProductoSeleccionadoEvaluacion" onclick="javascript:CheckOtherIsCheckedByGVID(this);" GroupName="checkProductosEvaluacion" Text="" runat="server" Checked='<%# (bool)Eval("SelectedEvaluacion") %>' Enabled='<%# AllowUpdate %>' />
                                    <asp:HiddenField runat="server" ID="hdCodigoProductoEvaluacion" Value='<%# Eval("IdProducto") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto">
                            <ItemTemplate>
                                <asp:Label ID="lblNombreProducto" runat="server" Text='<%# Eval("NombreProducto") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unidades">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# Eval("Unidades") + " " + Eval("unidadDeMedida") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datos Evaluador">
                            <ItemTemplate>
                                <asp:TextBox ID="txtProduccionEvaluador" runat="server" 
                                     onkeyup="format(this)" onchange="format(this)" MaxLength="11"
                                    Text='<%# ((int)Eval("UnidadesEvaluador")).ToString("N0") %>' Enabled='<%# AllowUpdate %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#00468F" ForeColor="White" />
                </asp:GridView>
                <br />
                <h3 style="color: white;text-align: center;background-color: #00468f;margin-bottom: 0px;">MERCADEO</h3>
                <asp:GridView ID="gvMercadeo" Visible="true" runat="server" DataSourceID="dataMercadeo"
                    AutoGenerateColumns="False" CssClass="Grilla" Width="100%" EmptyDataText="No existen cargos para mostrar."
                    AllowPaging="false" AllowSorting="false" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <div style="word-wrap: break-word;">
                                    <asp:Label ID="lblCantidad" runat="server" Text="1" />
                                    <asp:HiddenField runat="server" ID="hdCodigo" Value='<%# Eval("IdActividad") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actividad">
                            <ItemTemplate>
                                <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datos Evaluador">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnidadesEvaluador" 
                                    onkeyup="format(this)" onchange="format(this)" MaxLength="3"
                                    runat="server" Text='<%# Eval("UnidadesEvaluador") %>' Enabled='<%# AllowUpdate %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#00468F" ForeColor="White" />
                </asp:GridView>
                <br />
                <h3 style="color: white;text-align: center;background-color: #00468f;margin-bottom: 0px;">EMPLEOS</h3>
                <asp:GridView ID="gvCargos" Visible="true" runat="server" DataSourceID="dataCargos" AutoGenerateColumns="False" CssClass="Grilla" Width="100%" EmptyDataText="No existen cargos para mostrar." AllowPaging="false" AllowSorting="false" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <div style="word-wrap: break-word;">
                                    <asp:Label ID="lblCantidad" runat="server" Text="1" />
                                    <asp:HiddenField runat="server" ID="hdCodigo" Value='<%# Eval("IdCargo") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo">
                            <ItemTemplate>
                                <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("Nombre") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Datos Evaluador">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnidadesEvaluador" 
                                     onkeyup="format(this)" onchange="format(this)" MaxLength="3"
                                    runat="server" Text='<%# Eval("UnidadesEvaluador") %>' Enabled='<%# AllowUpdate %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#00468F" ForeColor="White" />
                </asp:GridView>
            </div>
            <div align="center">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
                <br />
                <asp:Button ID="btnGuardar" runat="server" Text="Actualizar" class="Boton" OnClick="btnGuardar_Click" Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "AllowUpdate")) %>'></asp:Button>
            </div>
            <br />
            <asp:ObjectDataSource
                ID="data"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Evaluacion.IndicadoresDeGestion.IndicadorGestion"
                SelectMethod="GetProductos"
                EnablePaging="false">
                <SelectParameters>
                    <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codproyecto" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource
                ID="dataCargos"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Evaluacion.IndicadoresDeGestion.IndicadorGestion"
                SelectMethod="GetCargos"
                EnablePaging="false">
                <SelectParameters>
                    <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codproyecto" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource
                ID="dataMercadeo"
                runat="server"
                TypeName="Fonade.PlanDeNegocioV2.Evaluacion.IndicadoresDeGestion.IndicadorGestion"
                SelectMethod="GetMercadeo"
                EnablePaging="false">
                <SelectParameters>
                    <asp:QueryStringParameter Name="codigoProyecto" Type="String" DefaultValue="0" QueryStringField="codproyecto" />
                </SelectParameters>
            </asp:ObjectDataSource>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
