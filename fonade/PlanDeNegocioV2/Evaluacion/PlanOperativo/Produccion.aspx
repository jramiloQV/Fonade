<%@ Page Title="" Language="C#" MasterPageFile="~/PlanDeNegocioV2/Evaluacion/Master/EvaluacionSite.Master" AutoEventWireup="true" CodeBehind="Produccion.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Evaluacion.PlanOperativo.Produccion" %>
<%@ Register Src="~/Controles/Post_It.ascx" TagPrefix="uc1" TagName="Post_It" %>
<%@ Register Src="~/PlanDeNegocioV2/Evaluacion/Controles/EncabezadoEval.ascx" TagPrefix="uc1" TagName="EncabezadoEval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyHolder" runat="server">
    <div>
        <uc1:EncabezadoEval runat="server" id="EncabezadoEval" />  
        <br />
        <table class="auto-style1">
            <tr>
                <td colspan="2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td style="text-align: center">
                    <div id="div_Post_It1" runat="server" visible="false">
                        <uc1:Post_It ID="Post_It1" runat="server" _txtCampo="Produccion" _txtTab="1" _mostrarPost="false" />
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
                                        <div onclick="textoAyuda({titulo: 'Producción', texto: 'Produccion'});">
                                            <img src="../../../Images/imgAyuda.gif" border="0" alt="help_Objetivos" />
                                        </div>
                                        <div>
                                            &nbsp; <strong>Producción:</strong>
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
                                    <asp:GridView ID="GV_ProyectoProducto" runat="server" AutoGenerateColumns="False" OnRowCommand="GV_ProyectoProducto_RowCommand"
                                        Width="100%" CssClass="Grilla" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="id_producto" Visible="false" />
                                            <asp:BoundField DataField="NomProducto" HeaderText="Producto o Servicio" />
                                            <asp:TemplateField HeaderText="Insumos">
                                                <ItemTemplate>
                                                    <%--Cambio realizado por Diego Quiñonez el 19 de Enero de 2015--%>
                                                    <asp:LinkButton ID="lnkInsumos" runat="server" Text="Insumos" CommandArgument='<%# Eval("Id_Producto") %>' CommandName="insumos"></asp:LinkButton>
                                                    <%--<a id="btnRedirect" idproducto='<%# Eval("Id_Producto") %>' style="text-decoration: underline;
                                                        color: blue;">Insumos</a>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO"  DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes2" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes3" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes4" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes5" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes6" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes7" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes8" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes9" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes10" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes11" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_Mes12" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}" />
                                        </Columns>
                                    </asp:GridView>
                                </td>
                                <td>
                                    <asp:GridView ID="GV_costoTotal" runat="server" AutoGenerateColumns="False" CssClass="Grilla"
                                        Width="100%" GridLines="None" CellSpacing="1" CellPadding="4">
                                        <RowStyle HorizontalAlign="Right" />
                                        <Columns>
                                            <asp:BoundField DataField="Id_Producto" HeaderText="Id_Producto" Visible="False" />
                                            <asp:BoundField DataField="Unidades" HeaderText="Unidades" />
                                            <asp:BoundField DataField="PRECIO" HeaderText="PRECIO" DataFormatString="${0:C}"/>
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
</asp:Content>
