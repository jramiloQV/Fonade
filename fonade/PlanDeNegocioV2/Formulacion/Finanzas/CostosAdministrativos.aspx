<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CostosAdministrativos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Finanzas.CostosAdministrativos" %>

<%@ Register Src="~/Controles/CargarArchivos.ascx" TagName="CargarArchivos" TagPrefix="uc1" %>
<%@ Register Src="~/Controles/CatalogoCargo.ascx" TagName="CatalogoCargo" TagPrefix="uc2" %>
<%@ Register Src="~/Controles/Alert.ascx" TagName="Alert" TagPrefix="uc3" %>
<%@ Register Src="~/Controles/CatalogoGastoV2.ascx" TagName="CatalogoGasto" TagPrefix="uc4" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>

<%@ Register Src="~/PlanDeNegocioV2/Formulacion/Controles/Encabezado.ascx" TagName="Encabezado" TagPrefix="controlEncabezado" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagName="Post_It" TagPrefix="controlPostit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FONADE - Costos de Producción</title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <link href="~/Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../../Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
    <script src="../../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <link href="../../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" />
    <script src="../../../Scripts/common.js" type="text/javascript"></script>        
    <script type="text/javascript" src="~/Scripts/jquery.number.min.js"></script>
    <script src="../../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>    
    <style type="text/css">
        .MsoNormal {
            margin: 0cm 0cm 0pt 0cm !important;
            padding: 5px 15px 0px 15px;
        }

        .MsoNormalTable {
            margin: 6px 0px 4px 8px !important;
        }

        #btnAdicionarCargo {
            background-color: #fff !important;
        }

            #btnAdicionarCargo:hover {
                cursor: pointer;
                background-image: none !important;
                background-color: #fff !important;
                outline: 0 !important;
            }

        #btnAdicionarGasto {
            background-color: #fff !important;
        }

            #btnAdicionarGasto:hover {
                cursor: pointer;
                background-image: none !important;
                background-color: #fff !important;
                outline: 0 !important;
            }

        #btnAdicionarGastoAnual {
            background-color: #fff !important;
        }

            #btnAdicionarGastoAnual:hover {
                cursor: pointer;
                background-image: none !important;
                background-color: #fff !important;
                outline: 0 !important;
            }
        body, html {
            background-image: none !important;
        }
    </style>
    <script type="text/javascript">
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
    </script>
</head>
<body style="background-color: White; background-image: none">
    <% Page.DataBind(); %>
    <form id="form1" runat="server" style="background-color: White; background-image: none">
        <uc3:Alert ID="Alert1" runat="server" />
        <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
            <table runat="server" visible="false">
                <tbody>
                    <tr>
                        <td width="19">&nbsp;
                        </td>
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
                                ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="true" />
                            <%--Visible='<%# Convert.ToBoolean(DataBinder.GetPropertyValue(this, "visibleGuardar")??false) %>' />--%>
                        </td>
                    </tr>
                </tbody>
            </table>
            <controlEncabezado:Encabezado ID="Encabezado" runat="server" />
            <div style="position: relative; left: 705px; width: 160px;">
                <controlPostit:Post_It ID="Post_It" runat="server" Visible='<%# PostitVisible %>' _txtCampo="CostosAdministrativos" />
            </div>
            <br />
            <div style="text-align: center">
                <h1>VIII - Estructura financiera </h1>
            </div>
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="19">&nbsp;
                    </td>
                    <td>&nbsp;<table width='100%' align="Center" border='0' cellpadding='0' cellspacing='0'>
                        <tr>
                            <td align='left' valign='top' width='98%'>
                                <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                    <tr>
                                        <td align="left">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <div class="help" onclick="textoAyuda({titulo: 'Gastos de Puesta en Marcha', texto: 'GastosArranque'});">
                                                            <span class="image_help">
                                                                <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></span>
                                                            <span class="text_help">&nbsp;Gastos de Puesta en Marcha:</span>
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left'>
                                            <asp:Panel ID="pnlAdicionarGasto" runat="server" Visible="false">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                <asp:Button ID="btnAdicionarGasto" runat="server" Text="Adicionar Gasto de Puesta en Marcha"
                                                    CssClass='boton_Link' BorderStyle="None" OnClick="btnAdicionarGasto_Click" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gw_GastosPuestaMarca" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="Grilla" OnRowCommand="gw_GastosPuestaMarca_RowCommand" DataKeyNames="protegido" ShowHeaderWhenEmpty="true"
                                    OnSelectedIndexChanged="gw_GastosPuestaMarca_SelectedIndexChanged" OnRowDataBound="gw_GastosPuestaMarca_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdf_gastoMarcha" runat="server" Value='<%# Eval("Protegido") %>' />
                                                <asp:ImageButton ID="btn_BorrarMarcha" CommandName="BorrarMarcha" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id_Gasto") %>' runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                    OnClientClick="return Confirmacion('Esta seguro que desea borrar el gasto seleccionado?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditarMarcha" runat="server" CommandName="EditarMarcha" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id_Gasto") %>' Text='<%# Eval("Descripcion") %>' />
                                                <%--<asp:Label ID="lblEditarMarcha" runat="server" Text='<%# Eval("Descripcion") %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                                <table width="100%">
                                    <tr style="background: #00468f; color: White">
                                        <td style="width: 700px; font-weight: bold;">Total
                                        </td>
                                        <td style="width: 100px; text-align: right; font-weight: bold;">
                                            <asp:Label ID="lblTotalGastosPuestaMarca" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                                    <tr>
                                        <td align="left">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 50%">
                                                        <div class="help" onclick="textoAyuda({titulo: 'Gastos Anuales de Administracion', texto: 'GastosAnuales'});">
                                                            <span class="image_help">
                                                                <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" /></span>
                                                            <span class="text_help">&nbsp;Gastos Anuales de Administracion:</span>
                                                        </div>
                                                    </td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='left'>
                                            <asp:Panel ID="pnlAdicionarGastoAnual" runat="server" Visible="false">
                                                <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif" />
                                                <asp:Button ID="btnAdicionarGastoAnual" runat="server" Text="Adicionar Gasto Anual de Administración"
                                                    CssClass='boton_Link' BorderStyle="None" OnClick="btnAdicionarGastoAnual_Click" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gw_GastosAnuales" runat="server" Width="100%" AutoGenerateColumns="False"
                                    CssClass="Grilla" OnRowCommand="gw_GastosAnuales_RowCommand" DataKeyNames="protegido" ShowHeaderWhenEmpty="true"
                                    OnRowDataBound="gw_GastosAnuales_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="30px">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdf_gastoAnual" runat="server" Value='<%# Eval("Protegido") %>' />
                                                <asp:ImageButton ID="btn_BorrarAnual" CommandName="BorrarAnual" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id_Gasto") %>' runat="server" ImageUrl="/Images/icoBorrar.gif"
                                                    OnClientClick="return Confirmacion('Esta seguro que desea borrar el gasto seleccionado?')" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripción">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnEditarAnual" runat="server" CommandName="EditarAnual" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id_Gasto") %>' Text='<%# Eval("Descripcion") %>' />
                                                <%--<asp:Label ID="lblEditarAnual" runat="server" Text='<%# Eval("Descripcion") %>' />--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right" />
                                    </Columns>
                                </asp:GridView>
                                <table width="100%">
                                    <tr style="background: #00468f; color: White">
                                        <td style="width: 700px; font-weight: bold;">Total
                                        </td>
                                        <td style="width: 100px; text-align: right; font-weight: bold;">
                                            <asp:Label ID="lblTotalGastosAnuales" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    </td>
                </tr>
            </table>
            <div style="width: 100%; text-align: right;">
                <asp:Button ID="btm_guardarCambios" runat="server" Text="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="false" />
            </div>
        </asp:Panel>
        <!--  Nuevo Panel -->
        <asp:Panel ID="pnlCargo" Visible="false" runat="server">
            <uc2:CatalogoCargo ID="CatalogoCargo1" runat="server" />
        </asp:Panel>
        <!-- Nuevo Panel -->
        <asp:Panel ID="pnlGastos" runat="server" Visible="false">
            <uc4:CatalogoGasto ID="CatalogoGasto1" runat="server" />
        </asp:Panel>
    </form>
</body>
</html>
