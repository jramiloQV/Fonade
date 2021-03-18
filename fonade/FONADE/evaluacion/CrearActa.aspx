<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true"
    CodeBehind="CrearActa.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CrearActa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function alerta() {
            return confirm(' ¿ Esta seguro de eliminar este proyecto del Acta ?');
        }
    </script>

    <script type = "text/javascript">
        var ddlText, ddlValue, ddl, lblMesg;
        function CacheItems() {
            ddlText = new Array();
            ddlValue = new Array();
            ddl = document.getElementById("<%=DdlCodConvocatoria.ClientID %>");
            lblMesg = document.getElementById("<%=lblMessage.ClientID%>");
            for (var i = 0; i < ddl.options.length; i++) {
                ddlText[ddlText.length] = ddl.options[i].text;
                ddlValue[ddlValue.length] = ddl.options[i].value;
            }
        }
        window.onload = CacheItems;

        function FilterItems(value) {
            ddl.options.length = 0;
            for (var i = 0; i < ddlText.length; i++) {
                if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                    AddItem(ddlText[i], ddlValue[i]);
                }
            }
            lblMesg.innerHTML = ddl.options.length + " item(s) encontrado(s).";
            if (ddl.options.length == 0) {
                AddItem("No se encontró el item.", "");
            }
        }

        function AddItem(text, value) {
            var opt = document.createElement("option");
            opt.text = text;
            opt.value = value;
            ddl.options.add(opt);
        }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <script src="../../Scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../../Scripts/ScriptsGenerales.js" type="text/javascript"></script>
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="tolscripmanager">
    </ajaxToolkit:ToolkitScriptManager>
    <table class="style10">
        <tr>
            <td>
                <h1>
                    <asp:Label ID="lbltitulo" runat="server" Style="font-weight: 700" />
                    <asp:Label runat="server" Style="display: none" ID="lidacta" CssClass="imprimir" />
                </h1>
            </td>
        </tr>
    </table>
    <asp:Panel ID="PanelAgregar" runat="server">
        <table align="left" width="70%">
            <tr>
                <td>&nbsp;
                </td>
                <td align="left" colspan="4">
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" BackColor="White" Font-Bold="True"
                        Font-Italic="False" ForeColor="Red" HeaderText="Complete los Siguientes Campos"
                        ValidationGroup="crear" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Text="Nro Acta" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtNroActa" runat="server" MaxLength="10" Width="100px" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtNroActa"
                        CssClass="requiredFieldValidator" Display="Dynamic" ErrorMessage="Nro Acta" ForeColor="Red"
                        ValidationGroup="crear">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Nombre" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtnomActa" runat="server" Width="401px" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtnomActa"
                        CssClass="requiredFieldValidator" Display="Dynamic" ErrorMessage="Nombre" ForeColor="Red"
                        ValidationGroup="crear">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td align="left">
                    <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="Fecha" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtfecha" runat="server" Width="108px" />
                    <asp:Image ID="btnDate2" runat="server" AlternateText="cal2" ImageUrl="/images/icoModificar.gif" />
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format='dd/MM/yyyy'
                        PopupButtonID="btnDate2" TargetControlID="txtfecha" />
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtfecha"
                        CssClass="requiredFieldValidator" Display="Dynamic" ErrorMessage="Fecha" ForeColor="Red"
                        ValidationGroup="crear">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td valign="top">
                    <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Observaciones:" />
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtObservaciones" runat="server" class="Boxes" cols="50" Height="113px"
                        name="Detalle" Rows="6" TextMode="MultiLine" Width="499px" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td align="center">
                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Convocatoria" />
                </td>
                <td colspan="2">
                    <asp:Label ID="Label13" runat="server" Font-Bold="True" Text="filtrar: " />
                    <asp:TextBox ID="txtSearch" runat="server" onkeyup="FilterItems(this.value)"></asp:TextBox>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    <br />
                    <asp:DropDownList ID="DdlCodConvocatoria" runat="server" BackColor="White" DataSourceID="lds_convocatoria" DataTextField="Nombre" DataValueField="Id" Width="503px">
                    </asp:DropDownList>
                    <asp:LinqDataSource ID="lds_convocatoria" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_convocatoria_Selecting">
                    </asp:LinqDataSource>
                </td>
                <td class="style10">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DdlCodConvocatoria" Display="Dynamic" ErrorMessage="Convocatoria" ForeColor="Red" InitialValue="0" ValidationGroup="crear">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>&nbsp;
                </td>
                <td colspan="4">
                    <asp:Panel ID="pnlNegocioPublico" runat="server" Visible="False">
                        <table class="style10">
                            <tr>
                                <td width="15%">
                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Publicar" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkpublico" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                    <asp:ImageButton ID="imgadicionarplan" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                        OnClick="imgadicionarplan_Click" Visible="False" />
                    &nbsp;<asp:LinkButton ID="lnkadcionarplan" runat="server" Text="Adicionar Planes de Negocio al Acta"
                        OnClick="lnkadcionarplan_Click" Visible="False"></asp:LinkButton>
                    <br />
                    <br />
                    <asp:LinqDataSource ID="lds_planesnegocio" runat="server" OnSelecting="lds_planesnegocio_Selecting"
                        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false">
                    </asp:LinqDataSource>
                    <asp:Panel ID="panelNegocioGrid" runat="server">
                        <asp:GridView ID="GrvPlanesNegocio" runat="server" Width="100%" AutoGenerateColumns="False"
                            ShowHeaderWhenEmpty="true" CssClass="Grilla" OnRowDataBound="GrvPlanesNegocio_RowDataBound"
                            OnRowCommand="GrvPlanesNegocio_RowCommand" AllowPaging="false" DataSourceID="lds_planesnegocio"
                            OnPageIndexChanging="GrvPlanesNegocio_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnk_del" runat="server" CausesValidation="false" CommandName="eliminar"
                                            CommandArgument='<%# Eval("id_Proyecto") %>' OnClientClick="return alerta();">
                                            <asp:Image ID="imgborrar" runat="server" ImageUrl="~/Images/icoBorrar.gif" idacta='<%# Eval("id_Proyecto") %>'
                                                AlternateText="Eliminar emprendedor del Plan de Negocio" />
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="id_proyecto" HeaderText="Id" />
                                <asp:TemplateField HeaderText="Plan de Negocio">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkProyecto" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_Proyecto") %>'
                                            CommandName="proyecto" Text='<%# Eval("nomproyecto") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="coordinador" HeaderText="Coordinador" />
                                <asp:TemplateField HeaderText="Evaluador">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkevaluador" runat="server" CausesValidation="False" idevaluador='<%# Eval("idevaluador") %>'
                                            CommandName="evaluador" Text='<%# Eval("evaluador") %>' CommandArgument='<%# Eval("idevaluador") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="valorrecomendado" HeaderText="Valor SMLV" />
                                <asp:TemplateField HeaderText="Viable Evaluador">
                                    <ItemTemplate>
                                        <asp:Label ID="lblviableevaluador" runat="server" Text='<%# Bind("viableEvaluador") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Priorizado">
                                    <ItemTemplate>
                                        <asp:Label ID="lblviable" runat="server" Text='<%# Eval("viable") %>' Visible="False" />
                                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("id_proyecto") %>' Visible="False" />
                                        <asp:RadioButtonList ID="rdbViable" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">SI</asp:ListItem>
                                            <asp:ListItem Value="0">NO</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="Paginador" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td class="style14"></td>
                <td class="style15"></td>
                <td class="style15">&nbsp;
                </td>
                <td class="style16" align="right">
                    <asp:Button ID="btnimprimir" runat="server" Text="Imprimir" ValidationGroup="crear"
                        Visible="False" />
                    <asp:Button ID="btnupdate" runat="server" OnClick="btnupdate_Click" Text="Actualizar"
                        ValidationGroup="crear" Visible="False" />
                    <asp:Button ID="btnCrearActa" runat="server" OnClick="btnCrearActa_Click" Text="Crear"
                        ValidationGroup="crear" Visible="true" />
                </td>
                <td align="right">&nbsp;
                </td>
            </tr>
        </table>
    </asp:Panel>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=btnimprimir.ClientID%>").click(function (event) {
                event.preventDefault();
                var id = $(".imprimir").html();
                $.fn.windowopen('../evaluacion/ImprimirActa.aspx?CodActa=' + id, 670, 530, 'no', 1, 'no'); /*, 670, 530, 'no', 1, 'no'*/
            });
            $("#<%=GrvPlanesNegocio.ClientID%>[id*='lnkevaluador']").click(function (event) {
                event.preventDefault();
                $.fn.windowopen('../MiPerfil/VerPerfilContacto.aspx?LoadCode=' + $(this).attr('idevaluador') + '&CodRol=4', 770, 530, 'no', 1, 'no');
            });
            $("#<%=GrvPlanesNegocio.ClientID%>[id*='imgborrar']").click(function (event) {
                e.preventDefault();
                var dir = "CrearActa.apsx/EliminarProyecto";
                var variables = new Object();
                variables.idacta = $(this).attr('adacta');
                variables.codproyecto = $(this).attr('proyecto');
                ajaxEliminar(variables, dir);
            });

            function ajaxEliminar(variables, dir) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: url,
                    data: JSON.stringify(variables),
                    dataType: 'Json',
                    async: false,
                    success: function (data) {
                        var mensaje = JSON.parse(data.d);
                        if (mensaje.mensaje == "ok") {
                            alert('Registro Eliminado Exitosamente!!!');
                            location.reload();
                        }
                    },
                    error: function (request) {
                        alert(JSON.parse(request.responseText).Message);
                    }
                });
            }
        });
    </script>
</asp:Content>
