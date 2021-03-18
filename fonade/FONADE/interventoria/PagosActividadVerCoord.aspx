<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PagosActividadVerCoord.aspx.cs" Inherits="Fonade.FONADE.interventoria.PagosActividadVerCoord" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width: 100%;
         }
     </style>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <label>SOLICITUDES DE PAGO</label>
        </h1>
        <br />
        <br />
        <table>
            <tr>
                <td>
                    <asp:GridView ID="gv_pagosactividad" runat="server" CssClass="Grilla" Width="100%" AutoGenerateColumns="False" OnRowCommand="gv_pagosactividad_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Número Solicitud">
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Id_Acta") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Button ID="btnveracta" runat="server" Text='<%# Eval("Id_Acta") + "-" + " Ver" %>' CssClass="boton_Link_Grid" CommandArgument='<%# Eval("Id_Acta") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha Envio" />
                            <asp:BoundField DataField="NumSolicitudes" HeaderText="Número Solicitudes" />
                            <asp:BoundField DataField="DatosFirma" HeaderText="Firma" />
                            <asp:BoundField DataField="CodRechazoFirmaDigital" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>