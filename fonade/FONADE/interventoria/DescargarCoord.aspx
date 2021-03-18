<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="DescargarCoord.aspx.cs" Inherits="Fonade.FONADE.interventoria.DescargarCoord" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width: 100%;
         }
     </style>
     <script type="text/javascript">
         function imprimir() {

             document.getElementById('oculton1').style.display = "block";
             document.getElementById('oculton2').style.display = "block";
             document.getElementById('oculton3').style.display = "block";

             var divToPrint = document.getElementById('impresion');
             var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
             newWin.document.open();
             newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
             newWin.document.close();
             setTimeout(function () { newWin.close(); }, 1000);

             document.getElementById('oculton1').style.display = "none";
             document.getElementById('oculton2').style.display = "none";
             document.getElementById('oculton3').style.display = "none";
         }
     </script>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    
    <asp:Panel ID="pnlprincipal" runat="server">
        <h1>
            <label>DETALLE SOLICITUDES DE PAGO</label>
        </h1>
        <br />
        <br />
        <div id="impresion">
            <div id="oculton1" style="display:none;">
                <table>
                    <tr>
                        <td>
                            <h1>SOLICITUDES DE PAGO - MEMO No. <asp:label id="lblidacta001" runat="server" Text=""></asp:label></h1>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td>No Memo:</td>
                                    <td>
                                        <asp:Label ID="lblnomemo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Número Solicitudes:</td>
                                    <td>
                                        <asp:Label ID="lblnumsolici" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Fecha:</td>
                                    <td>
                                        <asp:Label ID="lblfecha" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Datos de Firma Digital:</td>
                                    <td>
                                        <asp:Label ID="lbldatosfirmdigita" runat="server" Text="" Width="400px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Solicitudes de Pago Incluidas
                        </td>
                    </tr>
                </table>
            </div>
            <table>
                <tr>
                    <td><div id="oculton3">
                        <asp:Label ID="lblseract" runat="server" Text=""></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gv_pagosactividad" runat="server" CssClass="Grilla" Width="100%" AutoGenerateColumns="False" DataKeyNames="Aprobado" OnRowCommand="gv_pagosactividad_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Número Solicitud">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Id_PagoActividad") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Button ID="btncodpago" runat="server" Text='<%# Bind("Id_PagoActividad") %>' CssClass="boton_Link_Grid" CommandArgument='<%# Bind("Id_PagoActividad") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FechaCoordinador" HeaderText="Fecha Envio" />
                                <asp:BoundField DataField="razonsocial" HeaderText="Empresa" />
                                <asp:BoundField DataField="CantidadDinero" HeaderText="Valor" />
                                <asp:TemplateField HeaderText="Aprobado">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Aprobado") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblaprobado" runat="server" Text='<%# Eval("Aprobado") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right;">
                        <asp:Label ID="lblcantipagos" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
            </table>
            <div id="oculton2" style="display:none;">
                <table>
                    <tr>
                        <td>Aprobó
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <br />
                            _____________________________________
                            <br />
                            Coordinador de Interventoria
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <br />
                            <br />
                            _____________________________________
                            <br />
                            Gerente de Convenio Fondo Emprender
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </asp:Panel>
    

    <asp:Panel ID="pnlimprimir" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnimprimir" runat="server" Text="Imprimir" OnClientClick="imprimir()" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>