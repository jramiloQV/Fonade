<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="PagosActividadRechazo.aspx.cs" Inherits="Fonade.FONADE.interventoria.PagosActividadRechazo" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla != 13);
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <label>
                SOLICITUDES DE PAGO ENVIADAS A FIDUCIARIA</label>
        </h1>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    No Solicitud
                </td>
                <td>
                    <asp:TextBox ID="txtidproyecto" runat="server" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Empresa
                </td>
                <td>
                    <asp:TextBox ID="txtnomproyecto" runat="server" Width="200px" MaxLength="80"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnbuscar" runat="server" Text="Buscar..." OnClick="btnbuscar_Click" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gv_pagosactividad" runat="server" CssClass="Grilla" Width="100%"
                        AutoGenerateColumns="False" OnRowCommand="gv_pagosactividad_RowCommand" AllowPaging="True"
                        OnPageIndexChanging="gv_pagosactividad_PageIndexChanging" OnRowDataBound="gv_pagosactividad_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Número Solicitud">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnveracta" runat="server" Text='<%# Eval("Id_PagoActividad") %>'
                                        CssClass="boton_Link_Grid" CausesValidation="false" CommandName="PagosActividad"
                                        CommandArgument='<%# Eval("Id_PagoActividad") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha Envio">
                                <ItemTemplate>
                                    <asp:Label ID="lblfecha" Text='<%# Eval("FechaCoordinador") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="FechaCoordinador" HeaderText="Fecha Envio" />--%>
                            <asp:BoundField DataField="razonsocial" HeaderText="Empresa" />
                            <%--<asp:BoundField DataField="CantidadDinero" HeaderText="Valor" />--%>
                            <asp:TemplateField HeaderText="Valor">
                                <ItemTemplate>
                                    <asp:Label ID="lblvalor" Text='<%# Eval("CantidadDinero") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    <asp:LinkButton ID="lnk_excel" runat="server" OnClick="lnk_excel_Click" Text="Cargar Archivo de Excel" Visible="false" Enabled="false">
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
