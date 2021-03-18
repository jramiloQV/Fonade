<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoFinanzasCapitalTrabajo.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoFinanzasCapitalTrabajo" EnableEventValidation="true" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        .MsoNormal
        {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }
        .MsoNormalTable
        {
            margin: 6px 0px 4px 8px !important;
        }
        #gw_CapitalTrabajo tr:nth-child(n+2) td:nth-child(3){
            text-align: right;
        }
    </style>

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
<body style="background-color:white;background-image:none;">
    <% Page.DataBind(); %>
    <form id="form1" runat="server" style="background-color:white;background-image:none;">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:Panel ID="pnlCapitalTrabajo" Visible="true" runat="server">
        <table>
            <tbody>
                <tr>
                    <td width="19">
                    </td>
                    <td>
                        ULTIMA ACTUALIZACIÓN:&nbsp;
                    </td>
                    <td>
                        <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                    </td>
                    <td style="width: 100px;">
                    </td>
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
        <table id="tabla_docs" runat="server" visible="true" width="780" border="0" cellspacing="0"
            cellpadding="0">
            <tr>
                <td align="right">
                    <table width="52" border="0" cellspacing="0" cellpadding="0">
                        <tr align="center">
                            <td style="width: 50;">
                                <asp:ImageButton ID="ImageButton1" ImageUrl="../../Images/icoClip.gif" runat="server" ToolTip="Nuevo Documento"
                                    OnClick="ImageButton1_Click" />
                            </td>
                            <td style="width: 138;">
                                <asp:ImageButton ID="ImageButton2" ImageUrl="../../Images/icoClip2.gif" runat="server" ToolTip="Ver Documentos"
                                    OnClick="ImageButton2_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="19">
                    &nbsp;
                </td>
                <td>
                    <table width='95%' border='0' align="center" cellspacing='0' cellpadding='0'>
                        <tr>
                            <td width='18' align='left'>
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Capital de Trabajo', texto: 'CapitalTrabajo'});">
                                        <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                                    </div>
                                </div>
                            </td>
                            <td width='350'>
                                <b>Capital de Trabajo:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                            </td>
                            <td align='right'>
                                <div id="div_Post_It_2" runat="server" visible="false">
                                    <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="CapitalTrabajo" _txtTab="1" Visible="true" _mostrarPost="false" />
                                    <%--Visible='<%# ((bool)DataBinder.GetPropertyValue(this, "ejecucion")) %>' />--%>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                        <tr>
                            <td align='left'>
                                <asp:Panel ID="pnlAdicionar" runat="server" Visible="false">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                    <asp:Button ID="btnAdicionarCapitalTrabajo" runat="server" Text="Adicionar Capital de Trabajo"
                                        CssClass='boton_Link' BorderStyle="None" OnClick="btnAdicionarCapitalTrabajo_Click" />
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gw_CapitalTrabajo" runat="server" Width="95%" AutoGenerateColumns="False"
                        CssClass="Grilla" OnRowCommand="gw_CapitalTrabajo_RowCommand" DataKeyNames="id_Capital" OnRowDataBound="gw_CapitalTrabajo_RowDataBound"
                        >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("id_Capital") %>'
                                        runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="true" OnClientClick="return Confirmacion('Esta seguro que desea borrar el concepto seleccionado?')" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Componente">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkComponente" runat="server" CommandArgument='<%# Eval("id_Capital") %>' CommandName="Editar" Text='<%# Eval("componente") %>' Font-Bold='<%# Equals("Total", DataBinder.GetPropertyValue(GetDataItem(), "componente")) %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="componente" HeaderText="Componente" />--%>
                            <asp:BoundField DataField="valor" HeaderText="Valor" />
                            <asp:BoundField DataField="observacion" HeaderText="Observacion" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <!--  Nuevo Panel -->
    <asp:Panel ID="pnlCrearCapitalTrabajo" Visible="false" runat="server">
        <table width='600px' border='0' cellspacing='0' cellpadding='2'>
            <tr>
                <td align='center'>
                    NUEVO CAPITAL DE TRABAJO
                </td>
            </tr>
        </table>
        <table width='600px' border='1' cellpadding='0' cellspacing='0' bordercolor='#4E77AF'>
            <tr>
                <td align='center' valign='top' width='98%'>
                    <table width='98%' border='0' cellspacing='0' cellpadding='3'>
                        <tr valign="top">
                            <td align="Right" width="110">
                                <b>Componente:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtComponente" runat="server" MaxLength="100" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="Right">
                                <b>Valor:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtValor" runat="server" MaxLength="20" Width="90px" Text="0"></asp:TextBox>                             
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="Right">
                                <b>Observación:</b>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtObservacioin" runat="server" TextMode="MultiLine" Width="400px"
                                    Height="100px"></asp:TextBox><br />
                            </td>
                        </tr>
                        <tr valign="top">
                            <td align="right" class="TitDestacado" colspan="2">
                                <asp:Button ID="btnCrearCapitalTrabajo" runat="server" Text="Crear" OnClick="btnCrearCapitalTrabajo_Click"
                                    ValidationGroup="GrupoCrearCapital" />
                                <asp:Button ID="btnCancelarNuevoCapital" runat="server" Text="Cancelar" OnClick="btnCancelarNuevoCapital_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
