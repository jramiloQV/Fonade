<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true"
    CodeBehind="ImprimirActa.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ImprimirActa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery.printPage.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).ready(function () {
            $(".imprimir").printPage({
                message: "Su documento esta cargando espere!!!..",
                url: "../evaluacion/imprimirActa.aspx",
                title: "Acta Del Comite Evaluador"
            });
        });
    </script>
    <style type="text/css">
        .style10 {
            width: 80%;
        }

        .style11 {
            width: 10%;
        }

        .blanco {
            background-color: white;
        }

        /*::-webkit-scrollbar{
            display:none;
        }*/
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div class="blanco">
        <br />
        <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
        <title>Acta Del Comite Evaluador</title>
        <table class="style10 " style="display: none;" align="center">
            <tr>
                <td align="center">
                    <asp:Button ID="Button1" CssClass="imprimir" Text="Imprimir" runat="server" />
                </td>
            </tr>
        </table>
        <br />
        <br />
        <table class="style10" align="center">

            <tr>
                <td>
                    <h4>
                       <%--<asp:Label ID="Label2" runat="server" Style="font-weight: 700" Text="Acta Del Comite Evaluador" />--%>
                        <asp:Label ID="lblTituloActa" runat="server" Style="font-weight: 700" 
                            Text="DESFORMALIZACIÓN DE PLANES DE NEGOCIO PRESENTADOS A LA CONVOCATORIA XXX, 
                            SEGÚN ACTA DEL CONSEJO DIRECTIVO NACIONAL DEL SENA N° XXX, 
                            CORRESPONDIENTE A LA SESIÓN REALIZADA EL XXDIAXX DE XXMESXXX DE XXAÑOXX" />
                    </h4>
                </td>
            </tr>
        </table>
        <br />
        <div align="center" width='80%'>
            <br />
            <table width='80%' border='0' cellpadding='4' cellspacing='1' class="Grilla" frame="box"
                style="border-style: outset">
                <tbody>
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" />
                            No Acta  </asp:Label>
                        </td>
                        <td align="justify">
                            <asp:Label ID="nroconvocatoria" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" />
                            Nombre  </asp:Label>
                        </td>
                        <td align="justify">
                            <asp:Label ID="nomconvocatoria" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" />
                            Fecha  </asp:Label>
                        </td>
                        <td align="justify">
                            <asp:Label ID="fecha" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" />
                            Observaciones  </asp:Label>
                        </td>
                        <td align="justify">
                            <asp:Label ID="observaciones" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label7" runat="server" />
                            Convocatoria  </asp:Label>
                        </td>
                        <td align="justify">
                            <asp:Label ID="convocatoria" runat="server" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <br />
        </div>
        <hr />
        <div align="center">
            <table class="style10" align="center">
                <tr>
                    <td>
                        <h1>
                            <asp:Label ID="Label1" runat="server" Style="font-weight: 700" Text="Planes de Negocio Incluidos" />
                        </h1>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="GrvPlanesNegocio" runat="server" Width="80%" AutoGenerateColumns="False"
                CssClass="Grilla" OnRowDataBound="GrvPlanesNegocioRowDataBound"
                EmptyDataText="No se encontraron Datos">
                <Columns>
                    <asp:BoundField DataField="nomproyecto" HeaderText="Plan de Negocio">
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="coordinador" HeaderText="Coordinador" />
                    <asp:TemplateField HeaderText="Evaluador">
                        <ItemTemplate>
                            <asp:Label ID="evaluador" runat="server" Text='<%# Eval("evaluador") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valor">

                        <ItemTemplate>
                            <asp:Label ID="lvalor" runat="server" Text='<%# Bind("valorrecomendado") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Priorizado">
                        <ItemTemplate>
                            <asp:Label ID="lblPriorizado" runat="server"
                                Text='<%# ((bool)Eval("viable") == true) ? "SI" : "NO" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Viable" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblviable" runat="server" Text='<%# Eval("viableEvaluador")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass="Paginador" />
            </asp:GridView>
            <table width='80%' border='0' cellpadding='4' cellspacing='1' frame="box"
                style="border-style: outset">
                <thead>
                    <th colspan="3" align="right">
                        <b style="color: #000000">Total SMLV:</b></th>
                    <th align="left">
                        <asp:Label ID="ltsalario" runat="server"></asp:Label>
                    </th>
                    <th colspan="2">&nbsp;</th>

                </thead>
                <tbody>
                    <tr style="width: 20%">
                        <td align="justify">&nbsp;</td>
                        <td align="justify">&nbsp;</td>
                        <td align="right">
                            <b style="color: #000000">Total :</b></td>
                        <td align="justify">
                            <asp:Label ID="ltotal" runat="server" />
                        </td>
                        <td align="justify">&nbsp;</td>
                    </tr>
                </tbody>
            </table>
            <br />
            <br />
            <br />
            <table width='100%' border='0' cellspacing='2' cellpadding='0'>


                <%--<tr>
                    <td class="style11">&nbsp;</td>
                    <td colspan="2">Aprobó:<br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                    </td>
                </tr>--%>

                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>Realizó el acta:</td>
                    <td>Aprobó:</td>
                </tr>

                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>

                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>_____________________________________</td>
                    <td>_____________________________________</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>Nombre: </td>
                    <td>Nombre: </td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>Cargo: </td>
                    <td>Cargo:</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                    </td>
                </tr>

               <%-- <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>_____________________________________</td>
                    <td>_____________________________________</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>Subgerente Financiero</td>
                    <td>Subgerente Técnico</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                    </td>
                </tr>--%>
                <%--<tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>_____________________________________</td>
                    <td>_____________________________________</td>
                </tr>
                <tr class='Titulo'>
                    <td valign="top" class="style11">&nbsp;</td>
                    <td valign="top">Coordinador Grupo de<br />
                        &nbsp;Ejecución y Liquidación de Convenios</td>
                    <td valign="top">Gerente Unidad Crédito y Cartera</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                        <br>
                    </td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td width='50%'>_____________________________________</td>
                    <td>&nbsp;</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td>Gerente de Convenio Fondo Emprender</td>
                    <td>&nbsp;</td>
                </tr>
                <tr class='Titulo'>
                    <td class="style11">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>--%>
            </table>
        </div>
    </div>
</asp:Content>
