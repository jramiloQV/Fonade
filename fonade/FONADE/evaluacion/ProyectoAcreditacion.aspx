<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="ProyectoAcreditacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ProyectoAcreditacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/validacionProyectoAcreditacion.js" type="text/javascript"></script>
    <style type="text/css">
        th
        {
            text-align: center;
            background: #3D5A87;
            color: white;
            padding: 0 5px;
        }
        .td
        {
            background: #3D5A87;
            color: white;
            padding: 0 6px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        Acreditar Plan De Negocio</h1>
    <br />
    <table cellspacing="0" cellpadding="0">
        <tr>
            <td valign="top" align="lef">
                <table cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblnomconvocatoria" runat="server" Font-Size="Small" ForeColor="#174696"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblunidad" runat="server" Font-Size="Small" ForeColor="#174696"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:Label ID="lblnomproyecto2" runat="server" Font-Bold="True" ForeColor="Black">Asesor Lider</asp:Label>
                            :&nbsp;
                            <asp:LinkButton ID="hplAsesorLider" runat="server" OnClick="hplAsesorLider_Click"></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblnomproyecto3" runat="server" Font-Bold="True" ForeColor="Black">Asesor</asp:Label>
                            :&nbsp;&nbsp;
                            <asp:LinkButton ID="hplAsesor" runat="server" OnClick="hplAsesor_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table bgcolor="#f5f5f8">
                                <tr align="left">
                                    <td colspan="3">
                                        <asp:Label ID="lblnomproyecto0" runat="server" Font-Bold="True" ForeColor="Black">Datos Del Proyecto</asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr align="left">
                                    <td>
                                        <table bgcolor="#f5f5f8">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblnomproyecto1" runat="server" Font-Bold="True" ForeColor="Black"
                                                        Text="Plan De Negocio:" />
                                                </td>
                                                <td align="justify">
                                                    <asp:Label ID="lblnomproyecto" runat="server" ForeColor="Black" />
                                                </td>
                                                <td>
                                                </td>
                                                <td style="color: #000000; font-style: normal;">
                                                    Lugar ejecución:
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblnomciudad" runat="server" Font-Bold="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="color: #000000">
                                                    Fecha de aval:
                                                    <br>
                                                    (dd/mm/aaaa)
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:Label ID="lblfechaEval" runat="server" ForeColor="Black"></asp:Label>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table align="left">
                                <tr align="left">
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdb" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                            <asp:ListItem Value="10">Asignado</asp:ListItem>
                                            <asp:ListItem Value="15">Pendiente</asp:ListItem>
                                            <asp:ListItem Value="16">Subsanado </asp:ListItem>
                                            <asp:ListItem Value="13">Acreditado</asp:ListItem>
                                            <asp:ListItem Value="14">No Acreditado</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <asp:DataList ID="DtlProyectoConvocatoria" runat="server" BorderWidth="0px" OnItemCommand="DtlProyectoRowcommand"
                                CellPadding="4" ForeColor="Black" OnItemDataBound="DtlProyectoConvocatoriaItemDataBound">
                                <HeaderTemplate>
                                    <table border="0" cellspacing="1" cellpadding="0" style="width: 726px;height: 44px;">
                                        <tr>
                                            <th align="left" rowspan="2">
                                                Nombre Emprendedores
                                            </th>
                                            <th align="left" rowspan="2">
                                                Pendiente
                                            </th>
                                            <th align="left" rowspan="2">
                                                Subsanado
                                            </th>
                                            <th align="left" rowspan="2">
                                                Acreditado
                                            </th>
                                            <th align="left" rowspan="2">
                                                No Acreditado
                                            </th>
                                            <th align="left">
                                                Documentos Anexos
                                            </th>
                                        </tr>
                                        <tr class="Titulo">
                                            <td align="left">
                                                <table border="0" cellpadding="0" cellspacing="0" id="Table16">
                                                    <tr>
                                                        <th align="center">
                                                            Anexos
                                                        </th>
                                                        <th rowspan="2" style="padding-left: 3px">
                                                            Certificaciones
                                                        </th>
                                                        <th rowspan="2">
                                                            Diplomas
                                                        </th>
                                                        <th rowspan="2">
                                                            Actas
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td class="td">
                                                                        1
                                                                    </td>
                                                                    <td class="td">
                                                                        2
                                                                    </td>
                                                                    <td class="td">
                                                                        3
                                                                    </td>
                                                                    <td class="td">
                                                                        DI
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellspacing="1" cellpadding="0" style="width: 726px;height: 44px;">
                                        <tr>
                                            <td rowspan="2" style="width: 20%">
                                                <asp:Label ID="lblidproyecto" runat="server" Style="display: none" Text='<%# Eval("ID_PROYECTOACREDITACIONDOCUMENTO") %>'></asp:Label>
                                                <asp:LinkButton ID="lnbEmprendedor" runat="server" CommandArgument='<%# Eval("CODCONTACTO") %>'
                                                    CommandName="emprendedor" Text='<%# Eval("Nombre") %>'></asp:LinkButton>
                                            </td>
                                            <td rowspan="2" align="center" style="width: 10%">
                                                <asp:Label ID="chPendiente" runat="server" Text='<%# Eval("PENDIENTE") %>' />
                                                <asp:Label ID="lchPendiente" runat="server" Text='<%# Eval("PENDIENTE") %>' Style="display: none" />
                                            </td>
                                            <td rowspan="2" align="center" style="width: 9%">
                                                <asp:Label ID="chsubsanado" runat="server" Text='<%# Eval("SUBSANADO") %>' />
                                                <asp:Label ID="lchsubsanado" runat="server" Text='<%# Eval("SUBSANADO") %>' Style="display: none" />
                                            </td>
                                            <td rowspan="2" align="center" style="width: 11%">
                                                <asp:Label ID="chAcreditado" runat="server" Text='<%# Eval("ACREDITADO") %>' />
                                                <asp:Label ID="lchAcreditado" runat="server" Text='<%# Eval("ACREDITADO") %>' Style="display: none" />
                                            </td>
                                            <td rowspan="2" align="center" style="width: 12%">
                                                <asp:Label ID="chNoAcreditado" runat="server" Text='<%# Eval("NOACREDITADO") %>'
                                                    check='<%# Eval("NOACREDITADO") %>' />
                                                <asp:Label ID="lchNoAcreditado" runat="server" Text='<%# Eval("NOACREDITADO") %>'
                                                    Style="display: none" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td align="center">
                                                        </td>
                                                        <td align="center" style="padding-left: 30px;padding-right: 30px;text-align: center;"
                                                            rowspan="2">
                                                            <asp:CheckBox ID="chCertificaion" runat="server" CssClass="chCertificaion" Checked='<%# Eval("FLAGCERTIFICACIONES") %>' />
                                                        </td>
                                                        <td style="padding-right: 30px;text-align: right;padding-left: 30px;" rowspan="2">
                                                            <asp:CheckBox ID="chDiplomas" runat="server" CssClass="chDiplomas" Checked='<%# Eval("FLAGDIPLOMA") %>' />
                                                        </td>
                                                        <td align="right" rowspan="2" style="padding-left: 10px;">
                                                            <asp:CheckBox ID="chActas" runat="server" CssClass="chActas" Checked='<%# Eval("FLAGACTA") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table border="0" cellspacing="0" cellpadding="0" id="Table17">
                                                                <tr>
                                                                    <td style="padding: 0 0px">
                                                                        <asp:CheckBox ID="ch1" runat="server" CssClass="ch1" Checked='<%# Eval("FLAGANEXO1") %>' />
                                                                    </td>
                                                                    <td style="padding: 0 0px">
                                                                        <asp:CheckBox ID="ch2" runat="server" CssClass="ch2" Checked='<%# Eval("FLAGANEXO2") %>' />
                                                                    </td>
                                                                    <td style="padding: 0 0px">
                                                                        <asp:CheckBox ID="ch3" runat="server" CssClass="ch3" Checked='<%# Eval("FLAGANEXO3") %>' />
                                                                    </td>
                                                                    <td style="padding: 0 0px">
                                                                        <asp:CheckBox ID="chdi" runat="server" CssClass="chdi" Checked='<%# Eval("FLAGDI") %>' />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            &nbsp;
                            <asp:LinkButton ID="hplnotificaciones" runat="server" Text="Ver Notificaciones Enviadas"
                                ToolTip="Ver Notificaciones Enviadas" OnClick="hplnotificaciones_Click"></asp:LinkButton>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;#Radicaci&oacute;n
                            CRIF &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtcrif" runat="server" Height="19px"
                                Width="146px" />
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton
                                ID="lbvertodos" runat="server" Text="Ver Todos" ToolTip="Ver todos los CRIF"
                                OnClick="lbvertodos_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="Btnguardar" runat="server" Text="Guardar" Visible="False" OnClick="Btnguardar_Click" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            &nbsp;Observaciones:
                        </td>
                    </tr>
                    <br />
                    <tr>
                        <td align="center">
                            <asp:TextBox ID="txtObservaciones" runat="server" Height="100px" Width="698px" TextMode="MultiLine"
                                MaxLength="1000" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnfinalizar" runat="server" Text="Finalizar Acreditación" Visible="False"
                                            OnClick="btnfinalizar_Click" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        $(document).on("ready", LoadPage);

        function LoadPage() {
            var lchPendiente = $("#<%=DtlProyectoConvocatoria.ClientID %> [id*='lchPendiente']").html();
            var lchsubsanado = $("#<%=DtlProyectoConvocatoria.ClientID %> [id*='lchsubsanado']").html();
            var lchAcreditado = $("#<%=DtlProyectoConvocatoria.ClientID %> [id*='lchAcreditado']").html();
            var lchNoAcreditado = $("#<%=DtlProyectoConvocatoria.ClientID %> [id*='lchNoAcreditado']").html();
            var lAnexo1 = $(".ch1").is(":checked");
            var lAnexo2 = $(".chd2").is(":checked");
            var lAnexo3 = $(".ch3").is(":checked");
            var lDI = $(".chdi").is(":checked");


            $("#<%=DtlProyectoConvocatoria.ClientID%> [id*='chPendiente']").click(function () {
                if (lchPendiente != "True") {
                    mostrarVentana("Pendiente", lchNoAcreditado, lchPendiente, lchsubsanado, lchAcreditado, lAnexo1, lAnexo2, lAnexo3, lDI);
                } else {
                    error();
                }

            });
            $("#<%=DtlProyectoConvocatoria.ClientID%> [id*='chsubsanado']").click(function () {
                if (lchsubsanado != "True") {
                    mostrarVentana("Subsanado", lchNoAcreditado, lchPendiente, lchsubsanado, lchAcreditado, lAnexo1, lAnexo2, lAnexo3, lDI);
                } else {
                    error();
                }

            });
            $("#<%=DtlProyectoConvocatoria.ClientID%> [id*='chAcreditado']").click(function () {
                if (lchAcreditado != "True") {
                    mostrarVentana("Acreditado", lchNoAcreditado, lchPendiente, lchsubsanado, lchAcreditado, lAnexo1, lAnexo2, lAnexo3, lDI);
                } else {
                    error();
                }

            });
            $("#<%=DtlProyectoConvocatoria.ClientID%> [id*='chNoAcreditado']").click(function () {
                if (lchNoAcreditado != "True") {
                    mostrarVentana("NoAcreditado", lchNoAcreditado, lchPendiente, lchsubsanado, lchAcreditado, lAnexo1, lAnexo2, lAnexo3, lDI);
                } else {
                    error();
                }

            });
        }

        function error() {
            alert("Error en [mostrarContenido] Exception:" + "undefined");
        }

        function mostrarVentana(nombre, lNoAcreditado, lPendiente, lSubSanado, lAcreditado, lAnexo1, lAnexo2, lAnexo3, lDI) {

            var lMensaje;
            var lResult;
            switch (nombre) {
                case "Subsanado":
                    if (lNoAcreditado) {
                        lMensaje = "No puede activar el estado 'Subsanado' si el estado del emprendedor es  'No Acreditado'";
                        lResult = false;
                    }
                    break;
                case "Acreditado":
                    if (lNoAcreditado) {
                        lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'No Acreditado'";
                        lResult = false;
                    } else if (lPendiente && !lSubSanado) {
                        lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'Pendiente'";
                        lResult = false;
                    } else if (!lAnexo1 || !lAnexo2 || !lAnexo3 || !lDI) {
                        lMensaje = "No puede activar el estado 'Acreditado' si los anexos no están completos (1,2,3 y DI)";
                        lResult = false;
                    }
                    break;
                case "NoAcreditado":
                    if (lAcreditado) {
                        lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Acreditado'";
                        lResult = false;
                    } else if (lSubSanado) {
                        lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Subsanado'";
                        lResult = false;
                    }
                    break;
                case "Pendiente":
                    if (lAcreditado) {
                        lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'Acreditado'";
                        lResult = false;
                    } else if (lNoAcreditado) {
                        lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'No Acreditado'";
                        lResult = false;
                    }
                    break;
            }

            if (!lResult && lMensaje != "")
                alert(lMensaje);


            return lResult;
        }
       

    </script>
</asp:Content>
