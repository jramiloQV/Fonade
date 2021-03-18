<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ProyectoAcreditacion.aspx.cs" Inherits="Fonade.FONADE.interventoria.ProyectoAcreditacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/validacionProyectoAcreditacion.js" type="text/javascript"></script>
    <style type="text/css">
        th {
            text-align: center;
            background: #3D5A87;
            color: white;
            padding: 0 5px;
        }

        .td {
            background: #3D5A87;
            color: white;
            padding: 0 6px;
        }

        /*Diego Quiñonez - 12 de Didiembre de 2014*/

        .Control {
            display: inline-block;
            margin: 0px auto;
            float: right;
        }

        .contenido {
            width: 650px;
            height: auto;
            text-align: left;
        }

        .popup {
            background-color: #FFFFFF;
            border: 3px solid #00468f;
            position: absolute;
            width: 650px;
            height: auto;
            visibility: visible;
            top: 54px;
            left: 162px;
        }

        .popup2 {
            top: 122px;
            left: 237px;
        }

        #dtxtVistaEmail {
            border: medium solid #000000;
            height: 200px;
            width:620px;
            overflow: scroll;
            background-color: White;
            overflow-x: hidden !important;
            margin:10px;
        }

        table {
            border-spacing:0px;
            padding:0px;
        }

        th {
            border:none;
            /*border-collapse:collapse;*/
            /*padding:0px;*/
            /*border-spacing:0px;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <h1>Acreditar Plan De Negocio</h1>
    <br />
    <table>
        <tr>
            <td style="text-align: left; vertical-align: top;">
                <table border="0">
                    <tr>
                        <td style="text-align: left;" colspan="2">
                            <asp:Label ID="lblnomconvocatoria" runat="server" Font-Size="Small" ForeColor="#174696"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblunidad" runat="server" Font-Size="Small" ForeColor="#174696"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;" colspan="2">
                            <asp:Label ID="lblnomproyecto2" runat="server" Font-Bold="True" ForeColor="Black">Asesor Lider</asp:Label>
                            :&nbsp;&nbsp;
                            <asp:LinkButton ID="hplAsesorLider" runat="server" OnClick="hplAsesorLider_Click"></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:Label ID="lblnomproyecto3" runat="server" Font-Bold="True" ForeColor="Black">Asesor</asp:Label>
                            :&nbsp;&nbsp;
                            <asp:LinkButton ID="hplAsesor" runat="server" OnClick="hplAsesor_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table style="background-color: #f5f5f8;">
                                <tr style="text-align: left;">
                                    <td colspan="3">
                                        <asp:Label ID="lblnomproyecto0" runat="server" Font-Bold="True" ForeColor="Black">Datos Del Proyecto</asp:Label>
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr style="text-align: left;">
                                    <td>
                                        <table style="text-align: left; vertical-align: top; background-color: #f5f5f8;">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblnomproyecto1" runat="server" Font-Bold="True" ForeColor="Black"
                                                        Text="Plan De Negocio:" />
                                                </td>
                                                <td style="text-align: justify;">
                                                    <asp:Label ID="lblnomproyecto" runat="server" ForeColor="Black" />
                                                </td>
                                                <td></td>
                                                <td style="color: #000000; font-style: normal;">Lugar ejecución:
                                                </td>
                                                <td style="text-align: left;">
                                                    <asp:Label ID="lblnomciudad" runat="server" Font-Bold="True" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="color: #000000">Fecha de aval:
                                                    <br>
                                                    (dd/mm/aaaa)
                                                </td>
                                                <td style="text-align: left;" colspan="2">
                                                    <asp:Label ID="lblfechaEval" runat="server" ForeColor="Black"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table style="text-align: left;">
                                <tr style="text-align: left;">
                                    <td style="text-align: left;">
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
                <table>
                    <tr>
                        <td>
                            <asp:DataList ID="DtlProyectoConvocatoria" runat="server" BorderWidth="0px" OnItemCommand="DtlProyectoRowcommand"
                                CellPadding="4" ForeColor="Black" OnItemDataBound="DtlProyectoConvocatoriaItemDataBound">
                                <HeaderTemplate>
                                    <table>
                                        <tr>
                                            <th style="text-align: left;" rowspan="2">Nombre Emprendedores
                                            </th>
                                            <th style="text-align: left;" rowspan="2">Pendiente
                                            </th>
                                            <th style="text-align: left;" rowspan="2">Subsanado
                                            </th>
                                            <th style="text-align: left;" rowspan="2">Acreditado
                                            </th>
                                            <th style="text-align: left;" rowspan="2">No Acreditado
                                            </th>
                                            <th style="text-align: left;">Documentos Anexos
                                            </th>
                                        </tr>
                                        <tr class="Titulo">
                                            <td style="text-align: left;">
                                                <table border="0" id="Table16">
                                                    <tr>
                                                        <th style="text-align: center;">Anexos
                                                        </th>
                                                        <th rowspan="2" style="padding-left: 3px">Certificaciones
                                                        </th>
                                                        <th rowspan="2">Diplomas
                                                        </th>
                                                        <th rowspan="2">Actas
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td class="td">1
                                                                    </td>
                                                                    <td class="td">2
                                                                    </td>
                                                                    <td class="td">3
                                                                    </td>
                                                                    <td class="td">DI
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
                                    <table style="width: 100%;">
                                        <tr>
                                            <td rowspan="2" style="width: 20%">
                                                <asp:Label ID="lblidproyecto" runat="server" Style="display: none" Text='<%# Eval("ID_PROYECTOACREDITACIONDOCUMENTO") %>'></asp:Label>
                                                <asp:LinkButton ID="lnbEmprendedor" runat="server" CommandArgument='<%# Eval("CODCONTACTO") %>'
                                                    CommandName="emprendedor" Text='<%# Eval("Nombre") %>'></asp:LinkButton>
                                            </td>
                                            <td rowspan="2" style="width: 10%; text-align: center;">
                                                <asp:ImageButton ID="imgPendiente" runat="server" CommandArgument='<%# Eval("PENDIENTE") %>' CommandName="Pendiente" />

                                                <asp:TextBox ID="txtParametrosPendiente" runat="server" Visible="false" Text='<%# Eval("OBSERVACIONPENDIENTE") + ";" + Eval("ASUNTOPENDIENTE") + ";" + Eval("FECHAPENDIENTE") %>'></asp:TextBox>

                                                <%--<asp:Label ID="chPendiente" runat="server" Text='<%# Eval("PENDIENTE") %>' />
                                                <asp:Label ID="lchPendiente" runat="server" Text='<%# Eval("PENDIENTE") %>' Visible="false" />--%>

                                                <%--<asp:ModalPopupExtender ID="mpe_pendiente" runat="server" Enabled="True" TargetControlID="imgPendiente"
                                                    CancelControlID="btnCancelar" OkControlID="btnCancelar" PopupControlID="contenido"
                                                    BackgroundCssClass="modalBackground" Drag="True">
                                                </asp:ModalPopupExtender>--%>
                                            </td>
                                            <td rowspan="2" style="width: 9%; text-align: center;">
                                                <asp:ImageButton ID="imgsubsanado" runat="server" CommandArgument='<%# Eval("SUBSANADO") %>' CommandName="Subsanado" />

                                                <asp:TextBox ID="txtParametrossubsanado" runat="server" Visible="false" Text='<%# Eval("OBSERVACIONSUBSANADO") + ";" + Eval("ASUNTOSUBSANADO") + ";" + Eval("FECHASUBSANADO") %>'></asp:TextBox>
                                                <%--<asp:Label ID="chsubsanado" runat="server" Text='<%# Eval("SUBSANADO") %>' />--%>
                                                <%--<asp:Label ID="lchsubsanado" runat="server" Text='<%# Eval("SUBSANADO") %>' Visible="false" />--%>
                                            </td>
                                            <td rowspan="2" style="width: 11%; text-align: center;">
                                                <asp:ImageButton ID="imgAcreditado" runat="server" CommandArgument='<%# Eval("ACREDITADO") %>' CommandName="Acreditado" />

                                                <asp:TextBox ID="txtParametrosAcreditado" runat="server" Visible="false" Text='<%# Eval("OBSERVACIONACREDITADO") + ";" + Eval("ASUNTOACREDITADO") + ";" + Eval("FECHAACREDITADO") %>'></asp:TextBox>
                                                <%--<asp:Label ID="chAcreditado" runat="server" Text='<%# Eval("ACREDITADO") %>' />
                                                <asp:Label ID="lchAcreditado" runat="server" Text='<%# Eval("ACREDITADO") %>' Visible="false" />--%>
                                            </td>
                                            <td rowspan="2" style="width: 12%; text-align: center;">
                                                <asp:ImageButton ID="imgNoAcreditado" runat="server" CommandArgument='<%# Eval("NOACREDITADO") %>' CommandName="NoAcreditado" />

                                                <asp:TextBox ID="txtParametrosNoAcreditado" runat="server" Visible="false" Text='<%# Eval("OBSERVACIONNOACREDITADO") + ";" + Eval("ASUNTONOACREDITADO") + ";" + Eval("FECHANOACREDITADO") %>'></asp:TextBox>
                                                <%--<asp:Label ID="chNoAcreditado" runat="server" Text='<%# Eval("NOACREDITADO") %>' />
                                                <asp:Label ID="lchNoAcreditado" runat="server" Text='<%# Eval("NOACREDITADO") %>' Visible="false" />--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="text-align: center;"></td>
                                                        <td style="text-align: center; padding-left: 20px; padding-right: 50px; text-align: center"
                                                            rowspan="2">
                                                            <asp:CheckBox ID="chCertificaion" runat="server" CssClass="chCertificaion" Checked='<%# Eval("FLAGCERTIFICACIONES") %>' />
                                                        </td>
                                                        <td style="padding-right: 30px; text-align: right" rowspan="2">
                                                            <asp:CheckBox ID="chDiplomas" runat="server" CssClass="chDiplomas" Checked='<%# Eval("FLAGDIPLOMA") %>' />
                                                        </td>
                                                        <td style="text-align: right;" rowspan="2">
                                                            <asp:CheckBox ID="chActas" runat="server" CssClass="chActas" Checked='<%# Eval("FLAGACTA") %>' />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table id="Table17">
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
                        <td colspan="5">&nbsp;
                            <asp:LinkButton ID="hplnotificaciones" runat="server" Text="Ver Notificaciones Enviadas" ToolTip="Ver Notificaciones Enviadas" OnClick="hplnotificaciones_Click"></asp:LinkButton>
                            &nbsp;&nbsp;#Radicación
                            CRIF &nbsp;&nbsp;
                            <asp:TextBox ID="txtcrif" runat="server" Height="19px" Width="146px" />
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lbvertodos" runat="server" Text="Ver Todos" ToolTip="Ver todos los CRIF" OnClick="lbvertodos_Click"></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkDocumentosAcreditacion" runat="server" Text="Ver documentos de acreditación" ToolTip="Ver documentos de acreditación" OnClick="lnkDocumentosAcreditacion_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <table>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="Btnguardar" runat="server" Text="Guardar" Visible="true" OnClick="Btnguardar_Click" 
                                            OnClientClick="return confirm('¿Desea guardar la información?')" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="lblObservaciones" runat="server" Text="Observaciones:"></asp:Label>
                        </td>
                    </tr>
                    <br />
                    <tr>
                        <td style="text-align: center;">
                            <asp:TextBox ID="txtObservaciones" runat="server" Height="100px" Width="698px" TextMode="MultiLine"
                                MaxLength="1000" />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <table>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td style="text-align: right;">
                                        <asp:Button ID="btnfinalizar" runat="server" Text="Finalizar Acreditación" Visible="true"
                                            OnClick="btnfinalizar_Click" OnClientClick="return confirm('¿Desea finalizar la acreditación?');" />
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <%-- <div id="divLayerObservacion">
        <div style="margin-top:100px; text-align:center;">
            <h1>ACREDITAR PLAN DE NEGOCIO</h1>
        </div>
    </div>--%>

    <%-- <script type="text/javascript">

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
                //alert(lchPendiente);
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
                alert("El Valor es: " + $('#ch1').attr('checked'));
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
            var lResult = true;
            switch (nombre) {
                case "Subsanado":
                    if (lNoAcreditado == "True") {
                        lMensaje = "No puede activar el estado 'Subsanado' si el estado del emprendedor es  'No Acreditado'";
                        lResult = false;
                    }
                    break;
                case "Acreditado":
                    if (lNoAcreditado == "True") {
                        lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'No Acreditado'";
                        lResult = false;
                    } else if ((lPendiente == "True" && lSubSanado == "False")) {
                        lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'Pendiente'";
                        lResult = false;
                    } else if (!(lAnexo1 && lAnexo2 && lAnexo3 && lDI)) {
                        alert(lAnexo1 + " , " + lAnexo2 + " , " + lAnexo3 + " , " + lDI);
                        lMensaje = "No puede activar el estado 'Acreditado' si los anexos no están completos (1,2,3 y DI)";
                        lResult = false;
                    }
                    break;
                case "NoAcreditado":
                    if (lAcreditado == "true" || lAcreditado == "True") {
                        lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Acreditado'";
                        lResult = false;
                    } else if (lSubSanado == "true" || lSubSanado == "True") {
                        lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Subsanado'";
                        lResult = false;
                    }
                    break;
                case "Pendiente":
                    //alert(lAcreditado + " ; " + lNoAcreditado);
                    if (lAcreditado == "true" || lAcreditado == "True") {
                        lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'Acreditado'";
                        lResult = false;
                    } else if (lNoAcreditado == "true" || lNoAcreditado == "True") {
                        lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'No Acreditado'";
                        lResult = false;
                    }
                    break;
            }

            if (!lResult && lMensaje != "")
                alert(lMensaje);


            return lResult;
        }


    </script>--%>

    <%--Diego Quiñonez - 12 de Diciembre de 2014--%>

    <asp:Panel ID="pnlContenidoAcreditacion" runat="server" Width="650px" CssClass="popup" Visible="false" Enabled="false">

        <div class="contenido">
            <div id="titulo">
                <h1>ACREDITAR PLAN DE NEGOCIO</h1>
            </div>
            <table>
                <tr>
                    <td>Emprendedor:</td>
                    <td>
                        <asp:Label ID="lblEmprendedor" runat="server" Text="" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Estado de Acreditación:</td>
                    <td>
                        <asp:Label ID="lblEstado" runat="server" Text="" Width="300px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Fecha compromiso:</td>
                    <td>
                        <asp:TextBox ID="txtFecha" runat="server" BackColor="White" Text="" Width="300px"></asp:TextBox>
                        <asp:Image ID="imgFecha" runat="server" AlternateText="cal1" ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px" Width="20px" />
                        <asp:CalendarExtender ID="cldFecha" runat="server" Format="dd/MM/yyyy" PopupButtonID="imgFecha" TargetControlID="txtFecha" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblEstadoR" runat="server" Text="" Width="300px"></asp:Label></td>
                    <td>
                        <asp:CheckBox ID="ckkEstado" runat="server" Checked="true" />
                    </td>
                </tr>
                <tr>
                    <td>Asunto:</td>
                    <td>
                        <asp:TextBox ID="txtAsunto" runat="server" Text="" Width="300px" MaxLength="150"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Observación:</td>
                    <td>
                        <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Width="300px" Height="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <div class="Control">
                <asp:Button ID="btnGuardarEnviar" runat="server" Text="Guardar y Enviar" OnClick="btnGuardarEnviar_Click" />
                <asp:Button ID="Button1" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />
                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" OnClick="btnCancelar_Click" />
            </div>
        </div>

    </asp:Panel>


    <asp:Panel ID="pnlCOntenidoEmail" runat="server" Width="650px" Visible="false" Enabled="false" CssClass="popup popup2" Style="padding:10px;">

        <div class="contenido">
            <div id="tituloemail">
                <h1>VISTA PREVIA EMAIL</h1>
            </div>
            <table>
                <tr>
                    <td>
                        <div id="dtxtVistaEmail" runat="server">
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div class="Control">
                <asp:Button ID="EnviarEmail" runat="server" Text="Enviar e-mail" OnClick="EnviarEmail_Click" />
                <asp:Button ID="btnCancelarEmail" runat="server" Text="Cancelar" OnClick="btnCancelarEmail_Click" />
            </div>
        </div>

    </asp:Panel>
</asp:Content>
