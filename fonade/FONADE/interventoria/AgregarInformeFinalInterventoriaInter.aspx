<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AgregarInformeFinalInterventoriaInter.aspx.cs" Inherits="Fonade.FONADE.interventoria.AgregarInformeFinalInterventoriaInter" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         html,body {
              overflow-y: scroll !important; 
         }
         table {
             width:100%;
         }
         td {
             vertical-align:top;
         }         
     </style>
     <script type="text/javascript">

         function imprimir() {
             document.getElementById("oculto").style.display = "block";
             document.getElementById("antes").style.display = "none";
             document.getElementById("despues").style.display = "block";

             var divToPrint = document.getElementById('contentPrincipal');
             var newWin = window.open('', 'Print-Window', 'width=1000,height=768');
             newWin.document.open();
             newWin.document.write('<html > <head> </head> <style type="text/css"> html { overflow-y: hidden; } html, body { height: 100%; } body { overflow-y: scroll; } </style>  <body class="of_scrollbar" onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
             newWin.document.close();
             setTimeout(function () { newWin.close(); }, 1000);

             document.getElementById("oculto").style.display = "none";
             document.getElementById("antes").style.display = "block";
             document.getElementById("despues").style.display = "none";
         }

    </script>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <div id="contentPrincipal" >
    <asp:Panel ID="principal" runat="server">
        <div id="antes" style="display:block">
            <h1><asp:Label ID="L_titulo" runat="server" Text="INFORMES DE INTERVENTORÍA"></asp:Label></h1>
        </div>
        <div id="despues" style="display:none;">
            <h1><asp:Label ID="Label1" runat="server" Text="INFORME FINAL DE INTERVENTORÍA"></asp:Label></h1>
        </div>
        <br />
        <br />
        <div id="contenido">
            <table>
                <thead>
                    <tr><th colspan="4"><asp:label ID="lblinforme" runat="server" Text="Interventor "></asp:label></th></tr>
                    <tr><th colspan="4"><asp:label ID="L_TituloNombre" runat="server" Text="Interventor "></asp:label></th></tr>
                    <tr><th colspan="4"><asp:label ID="lblnomcoordinador" runat="server" Text="Interventor "></asp:label></th></tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="4"><br /><br /></td>
                    </tr>
                    <tr>
                        <td><b>Número Contrato:</b></td>
                        <td><asp:Label ID="lblnumContrato" runat="server" Text=""></asp:Label></td>
                        <td>
                            Fecha Informe:
                        </td>
                        <td><asp:Label ID="lblfechainforme" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2"><b>Empresa</b></td>
                        <td colspan="2">
                            <asp:Label ID="lblEmpresa" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><b>Teléfono</b></td>
                        <td colspan="2">
                            <asp:Label ID="lblTelefono" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><b>Dirección</b></td>
                        <td colspan="2">
                            <asp:Label ID="lblDireccion" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"><b>Socios</b></td>
                        <td colspan="2">
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
                            <asp:TableHeaderCell>CRITERIO</asp:TableHeaderCell>
                            <asp:TableHeaderCell>CUMPLIMIENTO A VERIFICAR</asp:TableHeaderCell>
                            <asp:TableHeaderCell>OBSERVACIÓN INTERVENTOR</asp:TableHeaderCell>
                    </asp:TableHeaderRow>

                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="3"></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <br />
            <br />
            <br />
            <div id="dvianexos">
                <asp:Panel ID="p_Anexos" runat="server">
                    <asp:Table ID="t_anexos" runat="server" class="Grilla">
                        <asp:TableHeaderRow>
                                <asp:TableHeaderCell ColumnSpan="2">ANEXOS</asp:TableHeaderCell>
                        </asp:TableHeaderRow>

                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="2"></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>

                <br />
                <br />
                
        </div>
            </div>

        <div id="oculto" style="visibility:hidden;">
            <p>Dadas las condiciones en que el Contratista se viene cumpliendo o incumpliendo, con las obligaciones del contrato, el INTERVENTOR recomienda FONADE:</p>
            <br />
            <br />
            <p>Para constancia firman:</p>
            <br />
            <br />
            ________________________________&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;________________________________
            <br />
            Interventor&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Contratista
        </div>
    </asp:Panel>
        </div>

    <div id="imprimir" style="text-align:center; width:100%;">
                    <asp:Button ID="btn_imprimir" runat="server" Text="Imprimir" OnClientClick="imprimir()" OnClick="btn_imprimir_Click" />
                </div>
</asp:Content>