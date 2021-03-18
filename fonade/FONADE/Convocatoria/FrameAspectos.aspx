<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrameAspectos.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.FrameAspectos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <%--<link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />--%>
    <link href="../../Styles/Site.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../../Styles/FrameApecto.css">
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>

    <script type="text/javascript">
        function closeWindow() {
            window.parent.opener.location.reload();
            window.parent.opener.focus();
            window.parent.close();
        }
    </script>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../../Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">


        $(function () {

            $(".MyTreeView").find(":checkbox").change(function () {
                //check or uncheck childs
                var nextele = $(this).closest("table").next()[0];
                if (nextele && nextele.tagName == "DIV") {
                    $(nextele).find(":checkbox").prop("checked", $(this).prop("checked"));

                }
                //check nodes all with the recursive method
                CheckChildNodes($(".MyTreeView").find(":checkbox").first());

            });
            //method check filial nodes
            function CheckChildNodes(Parentnode) {

                var nextele = $(Parentnode).closest("table").next()[0];

                if (nextele && nextele.tagName == "DIV") {
                    $(nextele).find(":checkbox").each(function () {
                        CheckChildNodes($(this));
                    });

                    if ($(nextele).find("input:checked").length == 0) {
                        $(Parentnode).removeAttr("checked");
                    }
                    if ($(nextele).find("input:checked").length > 0) {
                        $(Parentnode).prop("checked", "checked");
                    }

                }
                else { return; }

            }

        })

    </script>
    <style type="text/css">
        html, body {
            background-image: none !important;
            height: auto !important;
            overflow: scroll;
        }

        .auto-style2 {
        }

        .auto-style3 {
        }

        .auto-style4 {
        }

        .auto-style5 {
            width: 549px;
        }

        .cajasInline {
            display: inline-block;
            width: 48%;
            margin: 5px, 8px;
            /*background-color: #0101DF;*/
            text-align: left;
            color: #2E2E2E;
        }
    </style>
</head>
<body style="background-color: white; background-image: none;">
    <h1>SELECCIONE DE LA LISTA LOS ASPECTOS QUE QUIERE INCLUIR EN LA EVALUACIÓN</h1>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnexpan" runat="server" Text="Expandir todo" OnClick="btnexpan_Click" Height="26px" Width="116px" />
            <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" Height="27px" Width="70px" />
        </div>
        <div class="cajasInline" id="dAspectos" style="vertical-align: top; float:left; width:50%">
            <div style="overflow: auto;">
                <asp:TreeView ID="tv_aspectos" runat="server" OnSelectedNodeChanged="tv_aspectos_SelectedNodeChanged" NodeWrap="true"  
                    ShowCheckBoxes="All" ClientIDMode="Static" CssClass="MyTreeView" >
                    <NodeStyle ChildNodesPadding="1px" VerticalPadding="1px"/>
                    <SelectedNodeStyle ChildNodesPadding="1px" />
                </asp:TreeView>
            </div>
        </div>
        <div class="cajasInline" id="dDetalle" style="vertical-align: top; float:left; width:50%">
            <div id="plnDetallesAspecto" runat="server" visible="false">
                <div>
                    <asp:Label ID="lblEstado" runat="server" Text="Estado:" />
                    <asp:DropDownList ID="ddlestado" runat="server" Height="22px" ValidationGroup="actualizar" Width="155px">
                        <asp:ListItem Value="0">Activo</asp:ListItem>
                        <asp:ListItem Value="1">Inactivo</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtdescripcion" ErrorMessage="Campo requerido" ForeColor="Red" ValidationGroup="actualizar" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlestado" ErrorMessage="Campo requerido" ForeColor="Red" ValidationGroup="actualizar" Display="Dynamic" Visible="False">*</asp:RequiredFieldValidator>
                </div>
                <div>
                    <br />
                    <asp:TextBox ID="txtdescripcion" runat="server" TextMode="MultiLine" Width="376px" Height="120px" ValidationGroup="actualizar" MaxLength="350" ></asp:TextBox>
                    <br />
                    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" ValidationGroup="actualizar" Height="27px" Width="92px"  />
                </div>
            </div>
            <br />
            <div id="pnlVariables" runat="server" visible="false">
                <div>
                    <asp:Image ID="imgaspectoAgr" runat="server" ImageUrl="~/Images/add.png" Visible="false" />
                    <asp:LinkButton ID="lnkadicionar" runat="server" OnClick="lnkadicionar_Click" Text="Nueva variable" Visible="false"></asp:LinkButton>
                </div>
                <div>
                    <asp:GridView ID="gv_campos" runat="server" AutoGenerateColumns="false" CssClass="Grilla" OnRowCommand="gv_campos_RowCommand" Visible="true">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkeliminar" runat="server" CommandArgument='<%# Eval("id_Campo") %>' CommandName="eliminar" CssClass="sinlinea" OnClientClick="return confirm('Al borrar un Aspecto: este desaparecerá de los demás que lo requieren\n¿Esta seguro que desea borrar el aspecto seleccionado?')" Text="">
                                        <asp:Image ID="imgeliminar" runat="server" ImageUrl="~/Images/icoBorrar.gif" CssClass="sinlinea" />
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descripción">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVerIDCampo" runat="server" CommandArgument='<%# Eval("id_Campo") %>' CommandName="ver" Text='<%# Eval("Campo1") %>'>

                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="activo" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id="pnlAgregar" runat="server" visible="false">
                <div>
                    <h1>
                        <asp:Label ID="lbltituloAgregar" runat="server" Text="Nueva variable" /></h1>
                </div>
                <div>
                    <span>Descripción:</span>
                    <asp:TextBox ID="txtnombrecampo" runat="server" TextMode="MultiLine" Height="100px" Width="400px" ValidationGroup="crear" MaxLength="350"></asp:TextBox>
                    <br />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtnombrecampo" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">Campo requerido</asp:RequiredFieldValidator>
                </div>
                <div>
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <span>Estado:</span><br />
                                <asp:DropDownList ID="ddlnuevoactivo" runat="server" Height="16px" Width="150px" ValidationGroup="crear">
                                    <asp:ListItem Value="0">Activo</asp:ListItem>
                                    <asp:ListItem Value="1">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                                <br />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlnuevoactivo" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="crear">Campo requerido</asp:RequiredFieldValidator>
                            </td>
                            <td style="text-align: right">
                                <asp:Button ID="btnAgregarCampo" runat="server" Text="Crear" OnClick="btnAgregarCampo_Click" ValidationGroup="crear" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div style="text-align: right; display: inline-block; width: 96%;">
            <br /><br />
            <asp:Button ID="btnCerrar" runat="server" Text="Cerrar" OnClientClick="javascript:self.close();" />
        </div>
    </form>
</body>
</html>
