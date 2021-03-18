<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameNominaInter.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.FrameNominaInter" %>

<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Nómina</title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function alerta() {
            return confirm('¿Está seguro que desea eliminar la actividad seleccionada?');
        }
        function borrar() {
            return confirm('¿Esta seguro que desea borrar el avance seleccionado?');
        }
    </script>
    <style type="text/css">
        th
        {
            border: 1px solid black;
        }
        html, body, div, iframe
        {
            height: auto !important;
        }

        table#t_anexos tr:nth-child(3)
         {
           display: none !important;
         }
        #t_anexos tbody tr 
        {
            white-space:nowrap;
        }
    </style>
</head>
<body style="overflow-x: hidden;">
    <form id="form1" runat="server">
    <asp:Panel ID="pnlPrincipal" runat="server" Visible="true">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left">
                    <div class="help_container">
                        <div>
                            <a onclick="textoAyuda({titulo: 'Nómina', texto: 'Nomina'});">
                                <img src="../../Images/imgAyuda.gif" border="0" alt="help_infraestructura" />
                                Nómina: </a>
                        </div>
                    </div>
                </td>
                <td align="left">
                    &nbsp;</td>
                <td align="right">
                    &nbsp;</td>
                <td align="left" colspan="3">
                    <asp:LinkButton ID="lnkImprimir" runat="server" OnClick="lnkImprimir_Click">Imprimir</asp:LinkButton>
                </td>
                <td align="left" colspan="3" style="height: 26px; width: 100px">
                    &nbsp;</td>
                <td align="left" colspan="3">
                    <uc1:Post_It ID="Post_It2" runat="server" _txtCampo="Nomina" 
                        _txtTab="1" _mostrarPost="true"/>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblpuestosPendientesConteo" runat="server" 
            Text="Puestos Pendientes de Aprobar: 0" />
        <br />
        <table style="width: 50%">
            <tr>
                <td style="width: 50%">
                    <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                    <asp:ImageButton ID="Adicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                        Style="cursor: pointer;" OnClick="Adicionar_Click" />
                    &nbsp;
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">Adicionar Cargo al Plan Operativo</asp:LinkButton>
                </td>
                <td>
                     <asp:LinkButton ID="Pagos_nomina" runat="server" OnClick="lnkPagos_Click">Pagos</asp:LinkButton>
                </td>
            </tr>
        </table>
        <table width='100%' style="text-align: center;" border='0' cellpadding='0' cellspacing='0'>
            <tr>
                <td align='left' valign='top' width='98%'>
                    <table width='100%' border='0' cellspacing='1' cellpadding='4'>
                    </table>
                    <table cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td valign="top" style="width: 70% !important;">
                                <div style="width: 100%; overflow: scroll; border-right: silver 1px solid">
                                    <asp:Label ID="Label1" Text="Puesto de trabajo" Width="100%" runat="server" BackColor="#00468f"
                                        ForeColor="White" />
                                    <asp:Label ID="lbl_personalCalificado" Text="Personal calificado" runat="server"
                                        Style="background-color: #FFFFFF; font-weight: bold; color: #00468f;" Width="150px" />
                                    <asp:GridView ID="gv_personalCalificado" runat="server" Width="400px" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnPageIndexChanging="gv_personalCalificado_PageIndexChanging"
                                        OnRowCommand="gv_personalCalificado_RowCommand" OnRowDataBound="gv_personalCalificado_RowDataBound"
                                        ShowHeader="false">
                                        <PagerStyle CssClass="Paginador" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkeliminar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("Id_Nomina") %>'
                                                        CommandName="eliminar" CausesValidation="false">
                                                        <asp:Label runat="server" ID="lblactividaPOI" Visible="False" Text='<%# Eval("Id_Nomina") %>' />
                                                        <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                                            actividad='<%# Eval("Id_Nomina") %>' proyecto='<%= CodProyecto %>'></asp:Image>
                                                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina")+ ";" + Eval("Cargo") %>'
                                                        CommandName="mostrar" Text="Mostrar" ForeColor="Black" Font-Bold="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Id_Nomina" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Cargo" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Tipo" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="NominaID" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbl_nombreCargo" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina") %>'
                                                        CommandName="editar" Text='<%# Eval("Cargo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="Paginador" />
                                    </asp:GridView>
                                    <table id="tb_add" runat="server" style="width: 80%; margin-top: 20px;">
                                        <tr>
                                            <td style="width: 80%">
                                                <asp:ImageButton ID="img_help_add_manoDeObra" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                                                    Style="cursor: pointer;" OnClick="Adicionar2_Click" />
                                                &nbsp; <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton2_Click">Adicionar Mano de Obra al Plan Operativo</asp:LinkButton></td></tr></table><%--Cargos en Aprobación "PageIndexChanging" y "RowCommand"--%><asp:Panel ID="pnl_Actividades"
                                        runat="server">
                                                    <asp:Label ID="Label2" Text="Mano de Obra Directa" runat="server" Visible="true"
                                        Style="background-color: #FFFFFF; font-weight: bold; color: #00468f;" Width="150px" />
                                                    <asp:GridView ID="grvManoObraDirecta" runat="server" Width="400px" AutoGenerateColumns="false"
                                        CssClass="Grilla" OnPageIndexChanging="gv_personalCalificado_PageIndexChanging" Visible ="false"
                                        OnRowCommand="grvManoObra_RowCommand" OnRowDataBound="gv_personalCalificado_RowDataBound"
                                        ShowHeader="false">
                                        <PagerStyle CssClass="Paginador" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkeliminar" runat="server" OnClientClick="return alerta()" CommandArgument='<%# Eval("Id_Nomina") %>'
                                                        CommandName="eliminar" CausesValidation="false">
                                                        <asp:Label runat="server" ID="lblactividaPOI" Visible="False" Text='<%# Eval("Id_Nomina") %>' />
                                                        <asp:Image ID="imgeditar" ImageUrl="../../Images/icoBorrar.gif" runat="server" Style="cursor: pointer;"
                                                            actividad='<%# Eval("Id_Nomina") %>' proyecto='<%= CodProyecto %>'></asp:Image>
                                                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina")+ ";" + Eval("Cargo") %>'
                                                        CommandName="mostrar" Text="Mostrar" ForeColor="Black" Font-Bold="true" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Id_Nomina" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Cargo" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="Tipo" />
                                            <asp:BoundField HeaderText="" Visible="false" DataField="NominaID" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbl_nombreCargo" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina") %>'
                                                        CommandName="editar" Text='<%# Eval("Cargo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="Paginador" />
                                    </asp:GridView>
                                                    <br /><br />
                                        <asp:Label ID="lbl_cargosApr" Text="Cargos en Aprobación" runat="server" Style="background-color: #FFFFFF;
                                            font-weight: bold; color: #00468f;" Width="100%" Visible="false" />
                                                    <asp:Label ID="lblPorAprobar" Text="Cargos en Aprobación" runat="server" Style="background-color: #FFFFFF;
                                            font-weight: bold; color: #00468f;" Width="100%" Visible="false" />
                                                    <asp:Label ID="lblCargosPorAprobar" Text="Cargos en Aprobación" runat="server" Style="background-color: #FFFFFF;
                                            font-weight: bold; color: #00468f;" Width="100%" Visible="false" />
                                        <asp:GridView ID="GrvActividadesNoAprobadas" runat="server" Width="400px" AutoGenerateColumns="False"
                                            CssClass="Grilla" AllowPaging="false" OnRowCommand="GrvActividadesNoAprobadas_RowCommand"
                                            OnRowDataBound="GrvActividadesNoAprobadas_RowDataBound" ShowHeader="false">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkmostrar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina") + ";" + Eval("Cargo") %>'
                                                            CommandName="mostrar" Text="Mostrar" Font-Bold="true" ForeColor="Black" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_editarValores" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Nomina") %>'
                                                            CommandName="editar" Text='<%# Eval("Cargo") %>' ForeColor="Black" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="" Visible="false" DataField="Id_Nomina" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="CodProyecto" />
                                                <%--<asp:BoundField HeaderText="" Visible="false" DataField="Cargo" />--%>
                                                <asp:BoundField HeaderText="" Visible="false" DataField="Tipo" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="ChequeoCoordinador" ConvertEmptyStringToNull="true" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="Tarea" />
                                                <asp:BoundField HeaderText="" Visible="false" DataField="ChequeoUrgente" ConvertEmptyStringToNull="true" />
                                            </Columns>
                                            <PagerStyle CssClass="Paginador" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </td>
                            <td valign="top" style="width: 70% !important;">
                                <div style="width: 100%; width: 439px; overflow-x: scroll !important; overflow-y: hidden !important;">
                                    <asp:Table ID="t_anexos" runat="server" Width="100%" BorderWidth="0" CellPadding="4"
                                        CellSpacing="1" CssClass="Grilla">
                                    </asp:Table>
                                </div>
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
