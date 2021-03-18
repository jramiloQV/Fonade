<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CapitalDeTrabajo.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.CapitalDeTrabajo" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../../Scripts/jquery.number.min.js"></script>
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        #gw_CapitalTrabajo tr:nth-child(n+2) td:nth-child(3) {
            text-align: right;
        }
    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            $('.money').number(true, 2);
        });

        window.onload = function () {
            //Realizado();
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

        function alerta() {
            return confirm('¿ Está seguro de eliminar el registro ?');
        }
    </script>
</head>
<body style="background-color: white; background-image: none;">
    <% Page.DataBind(); %>
    <form id="form1" runat="server" style="background-color: white; background-image: none;">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:Panel ID="pnlCapitalTrabajo" Visible="true" runat="server">
            <table runat="server" visible="false">
                <tbody>
                    <tr>
                        <td width="19"></td>
                        <td>ULTIMA ACTUALIZACIÓN:&nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                        </td>
                        <td style="width: 100px;"></td>
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
            <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
            <div style="position: relative; left: 720px; width: 160px;">
                <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="CapitalTrabajo" />
            </div>
            <br />
            <div style="text-align: center">
                <h1>VIII - Estructura financiera </h1>
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="19">&nbsp;
                    </td>
                    <td>
                        <table width='95%' border='0' align="center" cellspacing='0' cellpadding='0'>
                            <tr>
                                <td width='18' align='left'>
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Capital de Trabajo', texto: 'CapitalTrabajo'});">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                                        </div>
                                    </div>
                                </td>
                                <td width='350'>
                                    <b>Capital de Trabajo:&nbsp;&nbsp;&nbsp;&nbsp;</b>
                                </td>
                                <td align='right'></td>
                            </tr>
                        </table>
                        <br />
                        <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                            <tr>
                                <td align='left'>
                                    <asp:Panel ID="pnlAdicionar" runat="server" Visible="false">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                        <asp:LinkButton ID="lnkAdicionarCapitalTrabajo" runat="server" Text="Adicionar Capital de Trabajo" OnClick="btnAdicionarCapitalTrabajo_Click"></asp:LinkButton>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gw_CapitalTrabajo" runat="server" Width="95%" AutoGenerateColumns="False"
                            CssClass="Grilla" OnRowCommand="gw_CapitalTrabajo_RowCommand" DataKeyNames="id_Capital" OnRowDataBound="gw_CapitalTrabajo_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btn_Borrar" CommandName="Borrar" CommandArgument='<%# Eval("id_Capital") %>'
                                            runat="server" ImageUrl="/Images/icoBorrar.gif" Visible="true" OnClientClick="alerta();" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Componente">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkComponente" runat="server" CommandArgument='<%# Eval("id_Capital") %>' CommandName="Editar" Text='<%# Eval("componente") %>' Font-Bold='<%# Equals("Total", DataBinder.GetPropertyValue(GetDataItem(), "componente")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="valor" HeaderText="Valor" />
                                <asp:BoundField DataField="FuenteFinanciacion" HeaderText="Fuente de financiación" />
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
                    <td align='center'>NUEVO CAPITAL DE TRABAJO
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
                                    <asp:TextBox ID="txtValor" runat="server" CssClass="money" MaxLength="20" Width="150px" Text="0"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td align="Right">
                                    <b>Fuente de financiación:</b>
                                </td>
                                <td align="left">
                                    <asp:DropDownList runat="server" ID="cmbFuenteFinanciacion" Width="150px" nombre="Fuente de financiacion" DataSourceID="dataFuenteFinanciacion" AutoPostBack="false" DataValueField="IdFuente" DataTextField="DescFuente" />
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

        <asp:ObjectDataSource
            ID="dataFuenteFinanciacion"
            runat="server"
            TypeName="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.CapitalDeTrabajo"
            SelectMethod="GetFuenteFinanciacion"></asp:ObjectDataSource>
    </form>
</body>
</html>
