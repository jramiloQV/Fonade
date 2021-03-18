<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EvaluacionVentas.aspx.cs"
    Inherits="Fonade.FONADE.evaluacion.EvaluacionVentas" %>

<%@ Register Src="../../Controles/Post_It.ascx" TagName="Post_It" TagPrefix="uc1" %>
<%--<%@ Register Src="../../Controles/CtrlCheckedProyecto.ascx" TagName="CtrlCheckedProyecto"
    TagPrefix="uc2" %>--%>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1
        {
            width: 100%;
            vertical-align: bottom;
        }
        .auto-style2
        {
            width: 100%;
            text-align: center;
        }
        .Grilla
        {
        }
    </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
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
                            ToolTip="Guardar" OnClick="btn_guardar_ultima_actualizacion_Click" Visible="false" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table class="auto-style1">
            <%--<tr>
                <td colspan="2">
                    <uc2:CtrlCheckedProyecto ID="CtrlCheckedProyecto1" runat="server" />
                </td>
            </tr>--%>
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center">
                    <div id="div_Post_It2" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Ventas" _txtTab="1" _mostrarPost="false" />
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 40%">
                    <div style="overflow: auto; width: 200px;">
                        <table class="auto-style1">
                            <tr>
                                <td>
                                    <div class="help_container">
                                        <div onclick="textoAyuda({titulo: 'Ventas', texto: 'Ventas'});">
                                            <img src="../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                        </div>
                                        <div>
                                            &nbsp; <strong>Ventas:</strong>
                                        </div>
                                    </div>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="GV_ProyectoProducto" runat="server" AutoGenerateColumns="False"
                                        DataKeyNames="Id_Producto" Width="100%" CssClass="Grilla" GridLines="None" CellSpacing="1"
                                        CellPadding="4">
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="id_producto" Visible="false" />
                                            <asp:BoundField DataField="NomProducto" HeaderText="Producto o Servicio" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td style="width: 60%;">
                    <div style="width: 670px; text-align: right; overflow: auto;">
                        <table style="width: 2550px; text-align: center;" cellpadding="4" cellspacing="1">
                            <tr>
                                <td colspan="13">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="L_Mes1" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 1" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes2" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 2" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes3" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 3" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes4" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 4" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes5" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 5" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes6" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 6" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes7" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 7" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes8" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 8" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes9" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 9" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes10" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 10" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes11" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 11" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_Mes12" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Mes 12" Width="100%"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="L_CostoTotal" runat="server" BackColor="#00468f" Font-Strikeout="False"
                                        ForeColor="White" Height="30px" Text="Costo Total" Width="100%"></asp:Label>
                                </td>
                            </tr>
                            <%--<tr>
                                <td colspan="13">
                                </td>
                            </tr>--%>
                            <tr>
                                <td>
                                    <asp:GridView ID="GV_Mes1" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes2" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes3" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes4" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes5" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes6" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes7" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes8" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes9" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes10" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes11" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes12" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_costoTotal" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Ventas" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="Ingreso" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
