<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AdicionarInformePresupuestalInter.aspx.cs" Inherits="Fonade.FONADE.interventoria.AdicionarInformePresupuestalInter" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width:100%;
         }
         td {
             vertical-align:top;
         }
     </style>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="principal" runat="server">
        <h1>
            <asp:Label ID="L_titulo" runat="server" Text="ADICIONAR INFORME PRESUPUESTAL">

            </asp:Label>
        </h1>
        <br />
        <br />
        <div id="contenido">
            <table>
                <thead>
                    <tr><th colspan="2">FORMATO 01A</th></tr>
                    <tr><th colspan="2">INFORME DE SEGUIMIENTO PRESUPUESTAL DE LA INTERVENTORIA</th></tr>
                    <tr><th colspan="2"><asp:label ID="L_TituloNombre" runat="server" Text="Interventor "></asp:label></th></tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="2"><br /><br /></td>
                    </tr>
                    <tr>
                        <td>Coordinador</td>
                        <td>
                            <asp:Label ID="lblCoordinador" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Periodo</td>
                        <td>
                            <asp:Label ID="lblPeriodo" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Contrato</td>
                        <td>
                            <asp:Label ID="lblContrato" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Fecha</td>
                        <td>
                            <asp:Label ID="lblFecha" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Empresa</td>
                        <td>
                            <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Teléfono</td>
                        <td>
                            <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Dirección</td>
                        <td>
                            <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Ciudad</td>
                        <td>
                            <asp:Label ID="lblCiudad" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Socios</td>
                        <td>
                            <asp:Panel ID="pSocios" runat="server">
                                <asp:Table ID="t_table" runat="server">

                                </asp:Table>
                            </asp:Panel>
                        </td>
                    </tr>
                </tbody>
            </table>

            <br />
            <br />
            <asp:Panel ID="p_iB" runat="server" Width="100%">
                <asp:Table ID="t_variable" runat="server" class="Grilla">
                    <asp:TableHeaderRow>
                            <asp:TableHeaderCell>Código</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Ámbito</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cumplimiento a verificar</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Observación Interventor</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Cumple</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Hacer Seguimiento</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Eliminar</asp:TableHeaderCell>
                            <asp:TableHeaderCell>Modificar</asp:TableHeaderCell>
                    </asp:TableHeaderRow>

                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="8"></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </div>
    </asp:Panel>
</asp:Content>