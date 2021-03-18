<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" 
    AutoEventWireup="true" CodeBehind="EvaluacionAportes.aspx.cs" Inherits="Fonade.FONADE.evaluacion.EvaluacionAportes" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <div style="overflow: hidden;">
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td>
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                    ULTIMA ACTUALIZACIÓN:&nbsp;
                                </td>
                                <td>
                                    <asp:Label ID="lbl_nombre_user_ult_act" Text="" runat="server" ForeColor="#CC0000" />&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbl_fecha_formateada" Text="" runat="server" ForeColor="#CC0000" />
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <td>
                                    <asp:CheckBox ID="chk_realizado" Text="MARCAR COMO REALIZADO:&nbsp;&nbsp;&nbsp;&nbsp;"
                                        runat="server" TextAlign="Left" />
                                    &nbsp;<asp:Button ID="btn_guardar_ultima_actualizacion" Text="Guardar" runat="server"
                                        ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="False" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <table style="width: 100%">
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" width="100%" style="background-color: White">
            <tr>
                <td class="auto-style5">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 50%">
                                <div class="help_container">
                                    <div onclick="textoAyuda({titulo: 'Aportes', texto: 'Aportes'});">
                                        <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" />
                                        &nbsp; <strong>Aportes:</strong>
                                    </div>
                                </div>
                            </td>
                            <td>
                                <div id="div_Post_It1" runat="server" visible="false">
                                    <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Aportes" _txtTab="1" _mostrarPost = "false" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table style="width: 100%">
            <tr>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <asp:Panel ID="pnlagregar" runat="server">
                        <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                        <img id="Adicionar" runat="server" src="../../Images/icoAdicionarUsuario.gif" style="cursor: pointer;"
                            alt="Adicionar" />
                        <a id="Hadicionar" style="cursor: pointer;" href="" runat="server">Adicionar aporte</a>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:Panel ID="PnlInversiones" runat="server" Visible="True">
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Medium" Text="Inversiones fijas" />
            <br />
            <br />
            <asp:GridView ID="GrAportes" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                OnRowDataBound="GrAportes_RowDataBound" GridLines="None" CellSpacing="1" CellPadding="4"
                BorderColor="#ffffff">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgeditar" src="../../Images/editar.png" style="cursor: pointer;" tipo="<%# Eval("CodTipoIndicador") %>"
                                aporte='<%# Eval("Id_Aporte") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgborrar" src="../../Images/icoBorrar.gif" style="cursor: pointer;" aporte="<%# Eval("Id_Aporte") %>"
                                tipo="<%# Eval("CodTipoIndicador") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Left">
                        <ItemStyle HorizontalAlign="Justify" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" HeaderStyle-Width="25%"
                        HeaderStyle-HorizontalAlign="Left" />
                    <asp:TemplateField HeaderText="Total Solicitado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalSolicitado" runat="server" Text='<%# Bind("Solicitado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeSolicitado" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Recomendado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="18%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalRecomendado" runat="server" Text='<%# Bind("Recomendado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeRecomendado" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="pnlTotales_Inversiones" runat="server" HorizontalAlign="Left">
                <table style="margin: 0px auto; float: right;">
                    <thead>
                        <tr>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Totales Solicitado
                            </th>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                %
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Total Recomendado
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                <strong>%</strong>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="LblTotal" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="ldif1A" runat="server" BorderStyle="None" Font-Bold="True" /></td>
                                <td style="text-align: center; width: 25%;">
                                    <asp:Label ID="ldif2A" runat="server" BorderStyle="None" Font-Bold="True" />
                                </td>
                                <td style="width: 25%;">
                                    <asp:Label ID="ldif3A" runat="server" BorderStyle="None" Font-Bold="True" />
                                </td>
                            <%--</td>--%>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="panelcapital" runat="server" Width="100%">
            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Capital de trabajo Primer Año de operación" />
            <br />
            <asp:GridView ID="GrvCapital" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                OnRowDataBound="GrvCapital_RowDataBound" OnPageIndexChanging="GrvCapital_PageIndexChanging"
                GridLines="None" CellSpacing="1" CellPadding="4" BorderColor="#ffffff">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgeditarC" src="../../Images/editar.png" style="cursor: pointer;" tipo="<%# Eval("CodTipoIndicador") %>"
                                aporte='<%# Eval("Id_Aporte") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgborrarC" src="../../Images/icoBorrar.gif" style="cursor: pointer;" aporte="<%# Eval("Id_Aporte") %>"
                                tipo="<%# Eval("CodTipoIndicador") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="30%">
                        <ItemStyle HorizontalAlign="Justify" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="25%" />
                    <asp:TemplateField HeaderText="Total Solicitado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalSolicitadoC" runat="server" Text='<%# Bind("Solicitado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeSolicitadoC" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Recomendado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="18%">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalRecomendadoC" runat="server" Text='<%# Bind("Recomendado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeRecomendadoC" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="pnlTotalesCapital" runat="server" HorizontalAlign="Right">
                <table style="margin: 0px auto; float: right;">
                    <thead>
                        <tr>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Totales Solicitado
                            </th>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                %
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Total Recomendado
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                <strong>%</strong>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="LblTotalCapital" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="ldif1c" runat="server" BorderStyle="None" Font-Bold="True" /></td>
                                <td style="text-align: center; width: 25%;">
                                    <asp:Label ID="ldif2c" runat="server" BorderStyle="None" Font-Bold="True" />
                                </td>
                                <td align="justify" style="width: 25%;">
                                    <asp:Label ID="ldif3c" runat="server" BorderStyle="None" Font-Bold="True" />
                                </td>
                            
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="PnlInversionsDiferidas" runat="server" Width="100%">
            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="Medium" Text="Inversiones Diferidas" />
            <br />
            <asp:GridView ID="GrvDifereridas" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                OnRowDataBound="GrvDiferidas_RowDataBound" OnPageIndexChanging="GrvDiferidas_PageIndexChanging"
                GridLines="None" CellSpacing="1" CellPadding="4" BorderColor="#ffffff">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgeditarD" src="../../Images/editar.png" style="cursor: pointer;" tipo="<%# Eval("CodTipoIndicador") %>"
                                aporte='<%# Eval("Id_Aporte") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgborrarD" src="../../Images/icoBorrar.gif" style="cursor: pointer;" aporte="<%# Eval("Id_Aporte") %>"
                                tipo="<%# Eval("CodTipoIndicador") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="30%">
                        <ItemStyle HorizontalAlign="Justify" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Detalle" HeaderText="Detalle" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="25%" />
                    <asp:TemplateField HeaderText="Total Solicitado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="15%" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalSolicitadoD" runat="server" Text='<%# Bind("Solicitado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeSolicitadoD" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Recomendado" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="18%" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblTotalRecomendadoD" runat="server" Text='<%# Bind("Recomendado") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="%" ItemStyle-Width="12.5%" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblPorcentajeRecomendadoD" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <asp:Panel ID="pnl_TotalesDiferidas" runat="server" HorizontalAlign="Right">
                <table style="margin: 0px auto; float: right;">
                    <thead>
                        <tr>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Totales Solicitado
                            </th>
                            <th bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                %
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                Total Recomendado
                            </th>
                            <th class="style13" bgcolor="#00468F" style="color: #FFFFFF; width: 25%;">
                                <strong>%</strong>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="LblTotalDiferida" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="ldif1" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                            <td style="text-align: center; width: 25%;">
                                <asp:Label ID="ldif2" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                            <td style="width: 25%;">
                                <asp:Label ID="ldif3" runat="server" BorderStyle="None" Font-Bold="True" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <br />
            <br />
        </asp:Panel>
        <br />
        <asp:Panel ID="PnIntegrantes" runat="server">
            <table width="100%">
                <tr>
                    <td class="style11" align="center" bgcolor="#3D5A87" style="color: #FFFFFF">
                        Integrantes de la
                        <br />
                        Iniciativa Empresarial
                    </td>
                    <td bgcolor="#3D5A87" align="center" bgcolor="#3D5A87" style="color: #FFFFFF">
                        Tipo
                    </td>
                    <td bgcolor="#3D5A87" align="center" bgcolor="#3D5A87" style="color: #FFFFFF">
                        Valores Aporte a Peso
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="GrvIntegrantes" runat="server" CssClass="Grilla" AutoGenerateColumns="False"
                OnPageIndexChanging="GrvIntegrantes_PageIndexChanging" OnRowDataBound="GrvIntegrantes_RowDataBound"
                GridLines="None" CellSpacing="1" CellPadding="4" BorderColor="#ffffff">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img id="imgeditarI" runat="server" src="../../Images/editar.png" style="cursor: pointer;"
                                contacto='<%# Eval("id_contacto")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="NomCompleto">
                        <ItemStyle HorizontalAlign="Justify" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Emprendedor" HeaderStyle-Width="1%">
                        <ItemTemplate>
                            <asp:Label ID="lblEmprendedor" runat="server" Text='<%# Bind("Beneficiario") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Otro" HeaderStyle-Width="1%">
                        <ItemTemplate>
                            <asp:Label ID="lblotro" runat="server" Text='<%# Bind("Beneficiario") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aporte Total" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblAporteTotal" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aporte en Dinero" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblAporteDinero" runat="server" Text='<%# Bind("AporteDinero") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Aporte En Especie" HeaderStyle-HorizontalAlign="Center"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label ID="lblAporteEspecie" runat="server" Text='<%# Bind("AporteEspecie") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Clase de Especie" DataField="DetalleEspecie" HeaderStyle-HorizontalAlign="Left"
                        HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Right" />
                </Columns>
            </asp:GridView>
            <br />
        </asp:Panel>
        <br />
        <table border="0" width="100%">
            <tr>
                <td class='Titulo'>
                    Observaciones composición grupo de socios - Equipo de trabajo
                </td>
                <td class='Titulo'>
                    &nbsp;
                </td>
                <td class='Titulo'>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;<asp:TextBox ID="txtobservaciones" runat="server" Height="90px" TextMode="MultiLine"
                        Width="110%" />
                </td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtobservaciones"
                        ErrorMessage="Obligatorio" ForeColor="Red" ValidationGroup="crear" />
                </td>
            </tr>
            <tr>
                <td>
                    <table border='0' width='100%'>
                        <tr>
                            <td style="width:50%;">
                                Recursos solicitados al fondo emprender en (smlv)
                            </td>
                            <td colspan='2' class='Titulo' align='left'>
                                <asp:TextBox ID="txtsolicitado" runat="server" Enabled="False" />
                                <asp:Label ID="lbl_solicitado" Text="" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class='Titulo'>
                                Valor recomendado (smlv)
                            </td>
                            <td colspan='2' class='Titulo' align='left'>
                                <asp:TextBox ID="txtrecomendado" runat="server" Enabled="false" />
                                <asp:Label ID="lbl_recomendado" Text="" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td align="left">
                                <asp:Button ID="Btnupdate" runat="server" OnClick="Btnupdate_Click" Text="Actualizar"
                                    ValidationGroup="crear" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <script type="text/javascript">

                $(document).ready(function () {

                    var url = "EvaluacionAportes.aspx/Eliminar";
                    $("#<%=Adicionar.ClientID %>").click(function (event) {
                        event.preventDefault();
                        $.fn.windowopen('../evaluacion/CatalogoAporteEvaluacion.aspx?Accion=Nuevo&Aporte=0', 670, 400, 'no', 1, 'no');
                    });
                    $("#<%=Hadicionar.ClientID %>").click(function (event) {
                        event.preventDefault();
                        $.fn.windowopen('../evaluacion/CatalogoAporteEvaluacion.aspx?Accion=Nuevo&Aporte=0', 670, 400, 'no', 1, 'no');
                    });


                    $("#<%=GrAportes.ClientID %> [id*='imgeditar']").click(function (event) {
                        event.preventDefault();
                        $.fn.windowopen('../evaluacion/CatalogoAporteEvaluacion.aspx?Aporte=' + $(this).attr('aporte') + "&tipo=" + +$(this).attr('tipo') + "&Accion=Editar", 670, 400, 'no', 1, 'no');
                    });


                    $("#<%=GrvCapital.ClientID %> [id*='imgeditarC']").click(function (event) {
                        event.preventDefault();
                        $.fn.windowopen('../evaluacion/CatalogoAporteEvaluacion.aspx?Aporte=' + $(this).attr('aporte') + "&tipo=" + +$(this).attr('tipo') + "&Accion=Editar", 670, 400, 'no', 1, 'no');
                    });

                    $("#<%=GrvDifereridas.ClientID %> [id*='imgeditarD']").click(function (event) {
                        event.preventDefault();
                        $.fn.windowopen('../evaluacion/CatalogoAporteEvaluacion.aspx?Aporte=' + $(this).attr('aporte') + "&tipo=" + +$(this).attr('tipo') + "&Accion=Editar", 670, 400, 'no', 1, 'no');
                    });
                    $("#<%=GrvIntegrantes.ClientID %> [id*='imgeditarI']").click(function (event) {
                        event.preventDefault();

                        $.fn.windowopen('../evaluacion/CatalogoEvaluacionContacto.aspx?codContacto=' + $(this).attr('contacto') + "&Accion=Editar", 570, 430, 'no', 1, 'no');

                    });

                    $("#<%=GrAportes.ClientID %> [id*='imgborrar']").click(function (event) {
                        event.preventDefault();
                        var variables = new Object();
                        variables.codigo = $(this).attr('aporte');


                        var bandera = false;
                        bandera = confirm('¿ Esta Seguro que Desea Eliminar el Registro ?');
                        if (bandera) {
                            ajaxEliminar(variables);
                        }


                    });
                    // meodos para borrar un aporte 
                    $("#<%=GrvCapital.ClientID %> [id*='imgborrarC']").click(function (event) {
                        event.preventDefault();
                        var variables = new Object();
                        variables.codigo = $(this).attr('aporte');
                        var bandera = false;
                        bandera = confirm('¿ Esta Seguro que Desea Eliminar el Registro ?');
                        if (bandera) {
                            ajaxEliminar(variables);
                        }

                    });

                    $("#<%=GrvDifereridas.ClientID %> [id*='imgborrarD']").click(function (event) {
                        event.preventDefault();
                        var variables = new Object();
                        variables.codigo = $(this).attr('aporte');
                        var bandera = false;
                        bandera = confirm('¿ Esta Seguro que Desea Eliminar el Registro ?');
                        if (bandera) {
                            ajaxEliminar(variables);
                        }

                    });


                    function ajaxEliminar(variables) {

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: url,
                            data: JSON.stringify(variables),
                            dataType: 'Json',
                            async: false,
                            success: function (data) {
                                var mensaje = JSON.parse(data.d);

                                if (mensaje.mensaje == "ok") {
                                    alert('Registro Eliminado Exitosamente!');
                                    location.reload();
                                } else alert(mensaje.mensaje);

                            },
                            error: function (request) {
                                alert(JSON.parse(request.responseText).Message);
                            }
                        });

                    }



                });
            </script>
        </table>
        <br />
        <br />
        <br />
    </div>
    <br />
    <br />
    <br />
    <br />
</asp:Content>
