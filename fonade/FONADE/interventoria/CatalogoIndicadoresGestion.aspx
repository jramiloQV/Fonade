<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="CatalogoIndicadoresGestion.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoIndicadoresGestion" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        function recargar() {
            window.location = 'CatalogoIndicadoresGestion.aspx';
        }
    </script>
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <label>
                CATALOGO INDICADORES DE GESTION</label>
        </h1>
        <br />
        <br />
        <table class="Grilla">
            <thead>
                <tr>
                    <th style="text-align: center;">
                        INDICADORES DE GESTION
                    </th>
                </tr>
            </thead>
        </table>
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gvindicadoresgestion" runat="server" CssClass="Grilla" Width="100%"
                        AutoGenerateColumns="False" EmptyDataText="No hay indicadores de gestion" OnRowCommand="GridView1_RowCommand"
                        AllowPaging="True" PageSize="150" OnPageIndexChanging="gvindicadoresgestion_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="CodProyecto" HeaderText="Id Proyecto" />
                            <asp:BoundField DataField="Aspecto" HeaderText="Actividad" />
                            <asp:BoundField DataField="Tarea" HeaderText="Tipo de Solicitud" />
                            <asp:BoundField DataField="RazonSocial" HeaderText="Empresa" />
                            <asp:TemplateField HeaderText="Interventor">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Nombres") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("Nombres") + " " + Eval("Apellidos") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btncatalogointer" runat="server" Text="Ir" CommandArgument='<%# "EDITAR" + ";" + Eval("CodProyecto") + ";" + Eval("id_indicadorinter") %>' CssClass="boton_Link_Grid" 
                                        CommandName="EDITAR" />
                                    
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr align="right">
                <td>
                    <asp:Label ID="lbl_pagina" Text="text" runat="server" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
