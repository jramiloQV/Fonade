<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImpresionPlanOperativo.ascx.cs" Inherits="Fonade.PlanDeNegocioV2.Formulacion.Controles.ImpresionPlan.ImpresionPlanOperativo" %>
<asp:Panel ID="pnltabResumen" runat="server">
    <div>
        <asp:Panel ID="pnlPrincipal" Visible="true" runat="server">
            <br />
            <table style="width: 100%">
                <tr>
                    <td style="width: 50%">
                        <label>Cronograma de Actividades:</label>
                    </td>
                </tr>
            </table>
            <br />
            <table style="width: 100%; align-items: center; border: 0;">
                <tr>
                    <td style="align-content: flex-end; vertical-align: top;">
                        <table>
                            <tr>
                                <td style="vertical-align: Top">
                                    <div style="overflow: auto;">
                                        <asp:GridView ID="gw_Anexos" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grilla" RowStyle-Height="35px"
                                            CellPadding="2" CellSpacing="2">
                                            <Columns>
                                                <asp:BoundField DataField="Item" HeaderText="Item" />
                                                <asp:TemplateField HeaderText="Actividad">
                                                    <ItemTemplate>
                                                        <label><%# Eval("Actividad") %>'</label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                        <br />
                                    </div>
                                </td>
                                
                            </tr>
                            <tr>
                                <td style="vertical-align: Top;">
                                    <div style="overflow: auto">
                                        <table runat="server" class="Grilla" cellspacing="2">
                                            <tr>
                                                <th style="width: 272px; text-align: center">Mes 1
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 2
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 3
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gw_AnexosActividadA" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grilla" RowStyle-Height="35px"
                                            CellPadding="2" CellSpacing="2">
                                            <Columns>
                                                <asp:BoundField DataField="fondo1" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor1" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo2" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor2" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo3" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor3" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </div>
                                    <div style="overflow: auto">
                                        <table runat="server" class="Grilla" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <th style="width: 272px; text-align: center">Mes 4
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 5
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 6
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gw_AnexosActividadB" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grilla" RowStyle-Height="35px"
                                            CellPadding="2" CellSpacing="2">
                                            <Columns>
                                                <asp:BoundField DataField="fondo4" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor4" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo5" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor5" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo6" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor6" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </div>
                                    <div style="overflow: auto">
                                        <table runat="server" class="Grilla" cellpadding="2" cellspacing="2">
                                            <tr>
                                                <th style="width: 272px; text-align: center">Mes 7
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 8
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 9
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gw_AnexosActividadC" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grilla" RowStyle-Height="35px"
                                            CellPadding="2" CellSpacing="2">
                                            <Columns>
                                                <asp:BoundField DataField="fondo7" HeaderText="Fondo" ItemStyle-Width="130px" />
                                               <asp:BoundField DataField="emprendedor7" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo8" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor8" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo9" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor9" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </div>
                                    <div style="overflow: auto">
                                        <table runat="server" class="Grilla" cellpadding="2" cellspacing="2">
                                            <tr>
                                                
                                                <th style="width: 272px; text-align: center">Mes 10
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 11
                                                </th>
                                                <th style="width: 272px; text-align: center">Mes 12
                                                </th>
                                                <th style="width: 272px; text-align: center">Costo Total
                                                </th>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gw_AnexosActividadD" runat="server" AutoGenerateColumns="false"
                                            CssClass="Grilla" RowStyle-Height="35px"
                                            CellPadding="2" CellSpacing="2">
                                            <Columns>
                                                <asp:BoundField DataField="fondo10" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor10" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo11" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor11" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondo12" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedor12" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="fondoTotal" HeaderText="Fondo" ItemStyle-Width="130px" />
                                                <asp:BoundField DataField="emprendedorTotal" HeaderText="Emprendedor" ItemStyle-Width="130px" />
                                            </Columns>
                                        </asp:GridView>
                                        <br />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

        </asp:Panel>
    </div>
    <br />
    <br />
</asp:Panel>
