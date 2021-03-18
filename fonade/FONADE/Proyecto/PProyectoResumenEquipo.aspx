<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PProyectoResumenEquipo.aspx.cs"
    Inherits="Fonade.FONADE.Proyecto.PProyectoResumenEquipo"  Async="true" EnableSessionState="True" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
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
    </style>
    <script type="text/javascript">
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
<body style="background-color:white;background-image:none"> 
    <% Page.DataBind(); %>

    <form id="form1" runat="server" style="background-color:white;background-image:none">
    <asp:LinqDataSource ID="lds_equipo" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="true" OnSelecting="lds_equipo_Selecting">
    </asp:LinqDataSource>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel ID="PanelResumen" runat="server" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table>
                <tbody>
                    <tr>
                        <td>
                            &nbsp;
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
            <table id="tabla_docs" runat="server" visible="false" width="780" border="0" cellspacing="0"
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
            <table border="0" width="100%" style="background-color: White">
                <tr>
                    <td>
                        <div class="help_container">
                            <div onclick="textoAyuda({titulo: 'Resumen Ejecutivo', texto: 'ResumenEjecutivo'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos">
                            </div>
                            <div>
                                &nbsp; <strong>Resumen Ejecutivo:</strong>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="l_info1" runat="server" Style="font-size: 13px; font-weight: 700" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="l_info2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="l_info3" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_sumario" Text="" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel_recursos" runat="server" Visible="false">
                            <table border="0">
                                <tr>
                                    <td>
                                        Recursos solicitados al fondo:
                                        <asp:Label ID="l_recursos" runat="server" Style="font-weight: 700" Font-Bold="True" />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Equipo de Trabajo</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gv_equipotrabajo" CssClass="Grilla" runat="server" AllowPaging="false"
                            AutoGenerateColumns="false" DataSourceID="lds_equipo" EmptyDataText="No hay información disponible."
                            OnDataBound="gv_equipotrabajo_DataBound" HeaderStyle-HorizontalAlign="Left" RowStyle-VerticalAlign="Top"
                            RowStyle-HorizontalAlign="Left">
                            <Columns>
                                <asp:TemplateField HeaderText="Nombre" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnombres" runat="server" Text='<%# Eval("nombre") %>' Style="text-decoration: none;" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lemail" runat="server" Text='<%# Eval("email") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rol" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="lrol" runat="server" Text='<%# Eval("rol") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Número de horas diarias dedicadas al proyecto en la etapa de ejecución"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" AccessibleHeaderText="columna4"
                                    FooterText="columna4" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hiddenID" runat="server" Value='<%# Eval("id_contacto") %>' />
                                        <asp:TextBox ID="txt_horasDedicadas" runat="server" Width="35px" Text='<%# Eval("horas") %>'
                                            Enabled="False" MaxLength="2"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btn_Guardar" runat="server" Text="Guardar" Height="28px" Enabled="False"
                                            Visible="False" Width="80px" OnClick="Guardar_Horas" />
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1" ControlToValidate="txt_horasDedicadas"
                                            ValidationExpression="^[0-9]*" ErrorMessage="*Este campo es numérico." Display="dynamic"
                                            ForeColor="Red" />
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ValueToCompare="24" ControlToValidate="txt_horasDedicadas"
                                            Type="Integer" Display="Dynamic" Operator="LessThanEqual" ErrorMessage="*El número de horas no pueden ser superiores a 24."
                                            ForeColor="Red"></asp:CompareValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
