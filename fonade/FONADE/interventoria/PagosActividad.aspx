<%@ Page Title="FONDO EMPRENDER - Actividad" Language="C#" MasterPageFile="~/Emergente.Master" 
    AutoEventWireup="true" CodeBehind="~/FONADE/interventoria/PagosActividad.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.PagosActividad" EnableSessionState="True" ViewStateMode="Enabled"
    Async="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .aRemplazar span{
            line-height:80px;
        }
    </style>
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Esta seguro que desea borrar el pago seleccionado?');
        }

        function Desactivar(valor) {
            if (valor == "1" || valor == "2") {
                document.getElementById('btnEnviar').style.visibility = 'hidden';
            }

            function DisableButton() {
                var txt = document.getElementById('lnkBtn_Add_PagosActividadSinConvenio');
                if(txt != null)
                {
                    if (txt.innerText != '') {
                        document.getElementById('btn_accion').style.visibility = 'hidden';
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:Panel ID="pnl_PagosActividad" runat="server" Width="95%" border="1" cellpadding="0"
        cellspacing="0" BorderColor="#4E77AF">
        <h1>
            <asp:Label ID="lbl_titutlo" runat="server" />
        </h1>
        <br />
        <% Page.DataBind(); %>
        <asp:ImageButton ID="imgBtn_Add_PagosActividadSinConvenio" ImageUrl="../../Images/icoAdicionarUsuario.gif"
            runat="server" OnClick="imgBtn_Add_PagosActividadSinConvenio_Click"  />
        <%--Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "usrVldn")??false) %>'--%>
        &nbsp;<asp:LinkButton ID="lnkBtn_Add_PagosActividadSinConvenio" Text="Adicionar Pago por Actividad" ClientIDMode="Static" 
            runat="server" OnClick="lnkBtn_Add_PagosActividadSinConvenio_Click"  />
        <%--Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "usrVldn")??false) %>'--%>
        <br />
        <br />
        <asp:GridView ID="gv_PagosActividades" runat="server" AutoGenerateColumns="False"
            ShowHeaderWhenEmpty="True" CssClass="Grilla" Width="95%" OnPageIndexChanging="gv_PagosActividades_PageIndexChanging"
            PageSize="10"
            OnRowDataBound="gv_PagosActividades_RowDataBound" AllowPaging="True"
            OnRowCommand="gv_PagosActividades_RowCommand" HeaderStyle-HorizontalAlign="Left">
            <Columns>
                <asp:TemplateField HeaderStyle-Width="3%">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_eliminar" runat="server"
                            CommandArgument='<%# Eval("Id_PagoActividad") %>' CommandName="eliminar" CausesValidation="false">
                            <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                Visible="false" />
                        </asp:LinkButton></ItemTemplate>
                    <HeaderStyle Width="3%" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID">
                    <ItemTemplate>
                        <asp:Label ID="lbl_id" Text='<%# Eval("Id_PagoActividad") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombre">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_nombre" Text='<%# Eval("NomPagoActividad") %>' CommandArgument='<%# Eval("Id_PagoActividad")  + ";" + Eval("NomPagoActividad") + ";" +  Eval("Estado") %>'
                            runat="server" CausesValidation="false" CommandName="editar" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Estado">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Estado" Text='<%# Eval("Estado") %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle HorizontalAlign="Left" />
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_Datos" runat="server" Width="95%" border="1" cellpadding="0" cellspacing="0"
        BorderColor="#4E77AF" Visible="false">
        <table width="95%" border="0" align="center" cellspacing="0" cellpadding="3">
            <tbody>
                <tr valign="top">
                    <td colspan="2">
                        <%--&nbsp;--%>
                        <h1>
                            <asp:Label ID="lbl_newOrEdit" runat="server" /></h1>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <div style="text-align: right">
                            <p id="pAviso" runat="server">
                                <b><span style="color: red">ATENCION:</span> Para crear el pago de click en el boton "Adicionar". Una vez tenga listo toda la informacion <br />del pago (datos y adjuntos), deberá dar click al boton &quot;Enviar&quot; para generarle la tarea al Interventor </b>
                            </p>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="3">
                        Tipo:
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="ddl_Tipo" runat="server" onChange="Desactivar(this.value)" >
                            <asp:ListItem Value="0" Text="Seleccione" />
                            <asp:ListItem Value="1" Text="Nueva" />
                            <asp:ListItem Value="2" Text="Rechazada" />
                        </asp:DropDownList>
                        <asp:Label ID="lbl_tipo_seleccionado" runat="server" Visible="true" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdf_tipo" runat="server" />
                    </td>
                    <td id="td_archivosAdjuntos" runat="server"  align="center">
                        ARCHIVOS ADJUNTOS<br />
                        <asp:ImageButton ID="imgBtn_addDocumentoPago" ImageUrl="../../Images/icoClip.gif"
                            runat="server" OnClick="imgBtn_addDocumentoPago_Click" Visible ="false" />
                        <asp:ImageButton ID="imgBtnListaDocs" ImageUrl="../../Images/icoClip2.gif"
                            runat="server" OnClick="imgBtn_addDocumentoPago_Click2" Width="16px" />
                    </td>
                </tr>
                <tr valign="top" bgcolor="#D1D8E2">
                    <td colspan="3">
                        <span>Número de la solicitud de orden que se va a reemplazar con la nueva:</span>
                    </td>
                    <%--<td colspan="3" class="aRemplazar">
                        <asp:Label ID="Label_a_remplazar" runat="server" />
                    </td>--%>
                    <td colspan="3">
                        <asp:Label ID="lbl_NumSolic" runat="server" />
                        <asp:DropDownList ID="ddl_NumSolicitudRechazada" runat="server" />
                        <asp:HiddenField ID="hdf_numsolicitud" runat="server" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="3">
                        Mes:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_meses" runat="server" />
                        <asp:Label ID="lbl_mes_seleccionado" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr valign="top" bgcolor="#D1D8E2">
                    <td colspan="3">
                        <asp:Label ID="lbl_Actividad_Cargo" Text="Actividad:" runat="server" />
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_actividad_cargo" runat="server"  />
                        <asp:Label ID="lbl_loaded_actividad_cargo" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="3">
                        Concepto:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_Concepto" runat="server" Width="448px" />
                    </td>
                </tr>
                <tr valign="top" bgcolor="#D1D8E2">
                    <td colspan="3">
                        Nombre del Beneficiario:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_CodPagoBeneficiario" runat="server" Width="505px" />
                        <asp:Label ID="lblNombreBeneficiario" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="3">
                        Forma de Pago:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddl_CodPagoForma" runat="server" Width="186px" />
                        <asp:Label ID="lbl_FormaDePago" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr valign="top" bgcolor="#D1D8E2">
                    <td colspan="3">
                        Observaciones:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="Observaciones" runat="server" TextMode="MultiLine" Height="28px"
                            Width="99%" />
                    </td>
                </tr>
                <tr valign="top" >
                    <td colspan="3">
                        Observaciones Interventor:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtObservaInterventor" runat="server" TextMode="MultiLine" Height="28px"
                            Width="99%" />
                    </td>
                </tr>
                <tr valign="top" bgcolor="#D1D8E2">
                    <td colspan="3">
                        Aprobado:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlAprobado" runat="server" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="3">
                        Cantidad de dinero solicitado al fondo emprender:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="CantidadDinero" runat="server" MaxLength="20" Height="18px" Width="119px" />
                        <asp:HiddenField ID="hdf_estado" runat="server" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="6" align="right">
                        <asp:Button ID="btn_accion" Text="Adicionar" runat="server" Visible="false" OnClick="btn_accion_Click" ClientIDMode="Static" />
                        <asp:Button ID="btnEnviar" Text="Enviar" runat="server" Visible="false" OnClick="btnEnviar_Click" ClientIDMode="Static" />
                        <asp:Button ID="btnRegresar" runat="server" OnClick="btnRegresar_Click" Text="Regresar" />

                        <asp:HiddenField ID="hdCodigoPago" runat="server" Visible="false" />                        
                    </td>
                </tr>
            </tbody>
        </table>
    </asp:Panel>
</asp:Content>
