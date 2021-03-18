<%@ Page MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="Consultas.aspx.cs"
    Inherits="Fonade.FONADE.MiPerfil.Consultas" Title="FONADE - " EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <script src="~/../../../Scripts/jquery-1.10.2.min.js"></script>
    <script src="../../Scripts/ScriptsEspecificos.js"></script>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
        function hidePopup() {
            var values = "";
            var listBox = document.getElementById("<%= lb_asesores.ClientID %>");
            for (var i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected) {
                    values += listBox.options[i].innerHTML + " " + listBox.options[i].value + "\n";
                }
            }
            var datos = values.split(' - ');
            var cedula = datos[0].trim();
            var nombre = datos[1].trim().split(' ');
            var modalPopup = $find('ImgBtn_MPE');
            var modalPopup2 = $find('mpeTxtAsesor');
            var txt = document.getElementById('txtBuscarAsesor');
            var codigo = document.getElementById('hdf_CodContacto');
            switch (nombre.length) {
                case 5:
                    txt.value = nombre[0].toUpperCase() + ' ' + nombre[1].toUpperCase() + ' ' + nombre[2].toUpperCase() + ' ' + nombre[3].toUpperCase();
                    codigo.value = nombre[4];
                    break;
                case 4:
                    txt.value = nombre[0].toUpperCase() + ' ' + nombre[1].toUpperCase() + ' ' + nombre[2].toUpperCase();
                    codigo.value = nombre[3];
                    break;
                case 3:
                    txt.value = nombre[0].toUpperCase() + ' ' + nombre[1].toUpperCase();
                    codigo.value = nombre[2];
                    break;
                default:
                    break;
            }
            txt.disabled = true;
            modalPopup.hide();
            modalPopup2.hide();
        }
    </script>
    <script>
        $(function () {
            $(".divA").scroll(function () {
                $(".divB").scrollLeft($(".divA").scrollLeft());
            });
            $(".divB").scroll(function () {
                $(".divA").scrollLeft($(".divB").scrollLeft());
            });
            $('.item-modal').click(function (e) { e.stopPropagation(); });
        });

        var setIdx = function () {
            document.getElementById('bodyContentPlace_hdf_CodContacto').value = document.getElementById('bodyContentPlace_lb_asesores').value;
            document.getElementById('AsesorNmLabel').textContent =
            document.getElementById('bodyContentPlace_lb_asesores').item(document.getElementById('bodyContentPlace_lb_asesores').selectedIndex).textContent;
        }

    </script>
    <style type="text/css">
        .tablaPaginador {
            bottom: 36px !important;
        }

        .PagerControl {
            text-decoration: none;
        }

        #bodyContentPlace_Panel3 {
            overflow-x: auto;
        }

        .auto-style2 {
            height: 27px;
        }

        .auto-style3 {
            width: 100%;
        }

        #AsesoresPanel {
            width: 580px !important;
            height: 250px !important;
        }

        #bodyContentPlace_UpdatePanel1 {
            padding: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:LinqDataSource ID="lds_Consultar" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Consultar_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_Consultarporrol" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Consultarporrol_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_asesores" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_asesores_Selecting">
    </asp:LinqDataSource>
    <table id="table1" class="auto-style3">
        <tr>
            <td></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" /></h1>
    <asp:Panel ID="Panel1" runat="server">
        <table width="98%" border="0" cellspacing="1" cellpadding="4">
            <tbody>
                <tr>
                    <td>
                        <span>Planes de Negocio</span>
                    </td>
                </tr>
                <tr>
                    <td width="40%" align="left" style="color: White;">Búsqueda por palabra
                    </td>
                </tr>
                <tr>
                    <td>
                    <tr valign="top">
                        <td align="left">
                            <asp:Panel ID="buscar_palabra" runat="server" DefaultButton="btn_buscar">
                                <table>
                                    <tbody>
                                        <tr>
                                            <td>
                                                <strong>Por palabra:</strong>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="tb_porPalabra" runat="server" Width="156px" MaxLength="80" />
                                            </td>
                                            <td>
                                                <asp:Button runat="server" ID="btn_buscar" OnClick="btn_buscar_onclick" Text="Buscar" />
                                                <asp:Label ID="lblMensajeBusqPorPalabra" runat="server" ForeColor="Red" Text="*Inicie la busqueda con minimo tres caracteres" Visible="False"></asp:Label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                <tr>
                    <td width="40%" align="left">&nbsp;</td>
                </tr>
                <tr bgcolor="#3D5A87">
                    <td width="40%" align="left" style="color: White;">Búsqueda avanzada
                    </td>
                </tr>
                <tr valign="top">
                    <td align="left">
                        <asp:Panel ID="buscar_codigo" runat="server" DefaultButton="btn_BusquedaAvanzada">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>Código:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tb_codigo" runat="server" Width="106px" MaxLength="80" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Operador:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddloperador" 
                                                runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Departamento:</b>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddl_departamento" OnSelectedIndexChanged="ddl_departamento_OnSelectedIndexChanged"
                                                runat="server" AutoPostBack="true" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Municipio:</b>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="udpDdlMunicipios" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_municipio" AutoPostBack="true" runat="server" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_departamento" EventName="selectedindexchanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Sector:</b>
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lb_sector" runat="server" Width="300px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Estado:</b>
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lb_estados" runat="server" Width="300px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Unidad Emprendimiento:</b>
                                        </td>
                                        <td>
                                            <asp:ListBox ID="lb_unidadEmprendimiento" runat="server" Width="410px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <b>Asesor:</b></td>
                                        <td>
                                            <asp:TextBox ID="txtBuscarAsesor" runat="server" Width="256px" ClientIDMode="Static" />
                                            <asp:ImageButton ID="ImgBtn" runat="server" Height="20px" ImageUrl="~/Images/buscarrr.png" Width="19px" />

                                            <asp:ModalPopupExtender ID="ImgBtn_MPE" runat="server" BackgroundCssClass="modalBackground"
                                                ClientIDMode="Static" Enabled="True" OkControlID="ImageButton2" PopupControlID="AsesoresPanel"
                                                TargetControlID="ImgBtn">
                                            </asp:ModalPopupExtender>
                                            <asp:ModalPopupExtender ID="mpeTxtAsesor" runat="server" BackgroundCssClass="modalBackground"
                                                ClientIDMode="Static" Enabled="True" OkControlID="ImageButton2" PopupControlID="AsesoresPanel"
                                                TargetControlID="txtBuscarAsesor">
                                            </asp:ModalPopupExtender>
                                            <asp:Label ID="AsesorNmLabel" runat="server" ClientIDMode="Static"></asp:Label><br />
                                            <asp:LinkButton ID="lnk_Limpiar" runat="server" OnClick="lnk_Limpiar_Click" Text="Limpiar" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <b>Incluir Descripción:</b>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="CheckBox1" runat="server" Text="Incluir descripción" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td align="right">
                                            <asp:Button ID="btn_BusquedaAvanzada" OnClick="btn_BusquedaAvanzada_onclick" runat="server"
                                                Text="Buscar" />
                                             <asp:Button ID="btn_AbrirEmpresa"  runat="server" Visible="false"
                                                Text="Abrir Empresa" OnClick="btn_AbrirEmpresa_Click" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td width="40%" align="left" class="auto-style2"></td>
                </tr>
                <tr id="tr_part_I" runat="server" visible="false">
                    <td align="left">
                        <span>Emprendedores, asesores</span><br />
                    </td>
                </tr>
                <tr id="tr_part_II" runat="server" visible="false" bgcolor="#3D5A87">
                    <td width="40%" align="left" style="color: White;">Búsqueda por palabra
                    </td>
                </tr>
                <tr id="tr_part_III" runat="server" visible="false" valign="top">
                    <td align="left">
                        <asp:Panel ID="emprendedores_panel" runat="server" DefaultButton="btn_buscartodos">
                            <table width="100%">
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>Por Cédula o Palabra:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tb_cedulaPalabra" runat="server" Width="256px" MaxLength="80" />
                                            <asp:Label ID="lblMensajeBusqEmprendedor" runat="server" ForeColor="Red" Text="*Inicie la busqueda con minimo tres caracteres" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr valign="middle">
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                                            Width="260px">
                                            <asp:ListItem Value="3">Emprendedor</asp:ListItem>
                                            <asp:ListItem Value="1,2">Asesor</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="1,2,3">Todos</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <caption>
                                            <tr>
                                                <td align="right" width="250">
                                                    <asp:Button ID="btn_buscartodos" runat="server" OnClick="tb_cedulaPalabra_onclick"
                                                        Text="Buscar" />
                                                </td>
                                            </tr>
                                        </caption>
                                    </tr>
                                </tbody>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Panel ID="jefes_btn_panel" runat="server" DefaultButton="btnbuscarcedulaopalabra">
            <table id="vercoor1" runat="server" width="98%" border="0" cellspacing="1" cellpadding="4"
                visible="false">
                <tbody>
                    <%--JEFES UNIDAD--%>
                    <tr>
                        <td align="left">
                            <span>Jefes de unidad</span><br />
                        </td>
                    </tr>
                    <tr bgcolor="#3D5A87">
                        <td width="40%" align="left" style="color: White;">Búsqueda por palabra
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="left">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <b>Por Cédula o Palabra:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="tctcedulaopalabra" runat="server" Width="156px" MaxLength="80" />
                                        </td>
                                        <td>
                                            <asp:Button runat="server" OnClick="btnbuscarcedulaopalabra_Click" ID="btnbuscarcedulaopalabra"
                                                Text="Buscar" />
                                            <asp:Label ID="lblMensajeBusqJefeUnidad" runat="server" ForeColor="Red" Text="*Inicie la busqueda con minimo tres caracteres" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <%--JEFES UNIDAD--%>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="emprendimiento_panel" runat="server" DefaultButton="btnbuscarcedulaopalabra0">
            <table id="vercoor2" runat="server" width="98%" border="0" cellspacing="1" cellpadding="4"
                visible="false">
                <tbody>
                    <%--UNIDADES DE EMPRENDIMIENTO--%>
                    <tr>
                        <td align="left">
                            <span>Unidades de Emprendimiento</span><br />
                        </td>
                    </tr>
                    <tr bgcolor="#3D5A87">
                        <td width="100%" align="left" style="color: White;">Búsqueda por palabra
                        </td>
                    </tr>
                    <tr valign="top">
                        <td align="left">
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <strong>Por Palabra:</strong>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtpalabra" runat="server" Width="156px" MaxLength="80" />
                                            <asp:Label ID="lblMensajeBusqUnidadEmp" runat="server" ForeColor="Red" Text="*Inicie la busqueda con minimo tres caracteres" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td valign="middle">
                            <table width="95%" border="0">
                                <tbody>
                                    <tr>
                                        <asp:RadioButtonList ID="radiobutonunidades" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1">Sena</asp:ListItem>
                                            <asp:ListItem Value="3">Externa</asp:ListItem>
                                            <asp:ListItem Selected="True" Value="2">Todos</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <td align="right">
                                            <asp:Button ID="btnbuscarcedulaopalabra0" runat="server" OnClick="btnbuscarcedulaopalabra0_Click"
                                                Text="Buscar" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="40%" align="left">&nbsp;
                        </td>
                    </tr>
                    <%--UNIDADES DE EMPRENDIMIENTO--%>
                </tbody>
            </table>
        </asp:Panel>
    </asp:Panel>
    <asp:Panel ID="pnlInfoResultados" runat="server" Height="50px">
        <table width="95%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" colspan="3">
                    <asp:Label ID="lblResults" Text="Se han encontrado planes de <br/> negocio buscando ''"
                        runat="server" Font-Bold="true" />
                </td>
                <td align="left" colspan="3">
                    <asp:LinkButton runat="server" ID="hpl_nueva" Text="Realizar otra búsqueda..." OnClick="hpl_nueva_onclick"
                        ForeColor="#CC0000" Style="text-decoration: none;" />
                </td>
            </tr>
            <tr>
                <td colspan="7">
                    <table border="0" width="100%">
                        <tbody>
                            <tr>
                                <td align="left" width="120">
                                    <asp:Label ID="lbl_NumeroPaginas" Text="página 1 de 3976" runat="server" />
                                </td>
                                <td align="right" width="200">Planes por página
                                    <asp:DropDownList ID="numRegPorPagina" runat="server" AutoPostBack="True" OnSelectedIndexChanged="numRegPorPagina_SelectedIndexChanged">
                                        <asp:ListItem Value="10" Selected="True">10</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="30">30</asp:ListItem>
                                        <asp:ListItem Value="40">40</asp:ListItem>
                                        <asp:ListItem Value="50">50</asp:ListItem>
                                        <asp:ListItem Value="60">60</asp:ListItem>
                                        <asp:ListItem Value="70">70</asp:ListItem>
                                        <asp:ListItem Value="80">80</asp:ListItem>
                                        <asp:ListItem Value="90">90</asp:ListItem>
                                        <asp:ListItem Value="100">100</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <div style="overflow-x: auto; overflow-y: auto">
        <asp:Panel ID="Panel2" runat="server">
            <asp:GridView ID="grdMain" runat="server" AutoGenerateColumns="false" CssClass="Grilla" CellSpacing="1" GridLines="None"
                AllowPaging="true" OnRowCommand="grdMain_RowCommand" AllowSorting="True" PageSize="20"
                OnPageIndexChanging="grdMain_PageIndexChanging" RowStyle-HorizontalAlign="Center" Width="800"
                OnSorting="grdMain_Sorting" PagerSettings-Position="Top" PagerStyle-HorizontalAlign="Center"
                OnRowDataBound="grdMain_RowDataBound" PagerStyle-ForeColor="#CC0000" PagerSettings-NextPageText=">"
                PagerSettings-LastPageText=">>" PagerSettings-PreviousPageImageUrl="<" PagerSettings-Mode="NumericFirstLast"
                PagerStyle-Font-Underline="false" PagerStyle-CssClass="PagerControl" EnableSortingAndPagingCallbacks="True">
                <Columns>
                    <asp:BoundField DataField="Id_Proyecto" HeaderText="Código" SortExpression="Id_Proyecto"
                        HeaderStyle-Width="2%">
                        <HeaderStyle Width="2%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NomSubSector" HeaderText="Sector" SortExpression="NomSubSector"
                        HeaderStyle-Width="15%">
                        <HeaderStyle Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NomCiudad" HeaderText="Ciudad" SortExpression="NomCiudad"
                        HeaderStyle-Width="8%">
                        <HeaderStyle Width="8%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="NomDepartamento" HeaderText="Departamento" SortExpression="NomDepartamento"
                        HeaderStyle-Width="8%">
                        <HeaderStyle Width="8%" />
                    </asp:BoundField>
                    <asp:TemplateField SortExpression="NomProyecto" HeaderText="Plan de Negocio" HeaderStyle-Width="15%">
                        <ItemTemplate>
                            <asp:LinkButton ID="Lnkbtnplan" runat="server" Text='<%# Eval("NomProyecto") %>'
                                CommandArgument='<%# Eval("Id_Proyecto") %>' CssClass="boton_Link_Grid" CommandName='<%# MostrarPlanOEmpresa %>'
                                ForeColor="#CC0000" />
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Descripcion" HeaderText="Descripción" HeaderStyle-Width="22%">
                        <ItemTemplate>
                            <asp:Label ID="tb_descripcion" runat="server" Text='<%# Eval("Sumario")%>' />
                        </ItemTemplate>
                        <HeaderStyle Width="22%" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="NomEstado" HeaderText="Estado" SortExpression="NomEstado"
                        HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Número Ultima Convocatoria" DataField="CodConvocatoria" />
                    <asp:BoundField DataField="N_NomConvocatoria" HeaderText="Nombre Ultima Convocatoria"
                        HeaderStyle-Width="10%">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="N_Fecha" HeaderText="Fecha de Creación" HeaderStyle-Width="10%"
                        SortExpression="N_Fecha">
                        <HeaderStyle Width="10%" />
                    </asp:BoundField>
                </Columns>
                <PagerSettings LastPageText="&gt;&gt;" Mode="NextPrevious" NextPageText="&gt;" Position="TopAndBottom" PreviousPageImageUrl="&lt;" />
                <PagerStyle CssClass="PagerControl" Font-Underline="False" ForeColor="#CC0000" Height="50px" HorizontalAlign="Center" />
                <RowStyle HorizontalAlign="Center" />
            </asp:GridView>
        </asp:Panel>
    </div>
    <asp:Panel ID="Panel3" runat="server">
        <asp:GridView ID="gv_busquedaporrol" runat="server" Width="100%" AutoGenerateColumns="false" CellSpacing="1" GridLines="None"
            CssClass="Grilla" AllowPaging="true" OnRowCommand="gv_busquedaporrol_RowCommand"
            PageSize="10" AllowSorting="True" OnPageIndexChanging="gv_busquedaporrol_PageIndexChanging"
            PagerSettings-Position="Top" PagerStyle-HorizontalAlign="Center" OnRowDataBound="gv_busquedaporrol_RowDataBound"
            PagerStyle-ForeColor="#CC0000" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>"
            PagerSettings-PreviousPageImageUrl="<" PagerSettings-Mode="NumericFirstLast"
            PagerStyle-Font-Underline="false" PagerStyle-CssClass="PagerControl" OnSorting="gv_busquedaporrol_Sorting">
            <Columns>
                <asp:BoundField DataField="Id_Proyecto" HeaderText="Código Plan" SortExpression="Id_Proyecto"
                    HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField SortExpression="NomProyecto" HeaderText="Plan de Negocio" HeaderStyle-Width="5%"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_planNegocio" Text='<%# Eval("NomProyecto") %>' runat="server"
                            CausesValidation="false" CommandArgument='<%# Eval("Id_Proyecto") %>' CommandName="planes"
                            Style="text-decoration: none;" ForeColor="#CC0000" Width="94px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="NomTipoIdentificacion" HeaderText="Tipo Documento" SortExpression="NomTipoIdentificacion"
                    HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="Identificacion" HeaderText="Número Documento" SortExpression="Identificacion"
                    HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="Nombres" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_nombres" Text='<%# Eval("Nombres") +" "+ Eval("Apellidos")  %>'
                            runat="server" Style="text-decoration: none;" Width="46px" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField SortExpression="Email" HeaderText="Email" HeaderStyle-Width="5%"
                    ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:Button ID="btnemal" runat="server" Text='<%# Eval("Email") %>' CommandArgument='<%# Eval("Id_Proyecto") %>'
                            CommandName="email" CssClass="boton_Link_Grid" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Nombre" HeaderText="Rol" SortExpression="Nombre" HeaderStyle-Width="5%"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="nomInstitucion" HeaderText="Unidad de Emprendimiento"
                    SortExpression="nomInstitucion" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="NomTipoInstitucion" HeaderText="Tipo de Institución" SortExpression="NomTipoInstitucion"
                    HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="CodConvocatoria" HeaderText="Número Ultima Convocatoria"
                    HeaderStyle-Width="10%" SortExpression="CodConvocatoria" />
                <asp:BoundField DataField="N_NomConvocatoria" HeaderText="Nombre Ultima Convocatoria"
                    HeaderStyle-Width="10%" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="Panel4" runat="server">
        <div style="padding: 20px 0px;">
            <asp:GridView ID="gv_busquedaavanzada" runat="server" Width="100%" AutoGenerateColumns="false" CellSpacing="1" GridLines="None"
                DataKeyNames="Id_Proyecto,CodTipoInstitucion" CssClass="Grilla" AllowPaging="true" OnRowCommand="gv_busquedaavanzada_RowCommand"
                FooterStyle-CssClass="PaginadorUno" AllowSorting="True" HeaderStyle-HorizontalAlign="Left"
                RowStyle-HorizontalAlign="Center" OnPageIndexChanging="gv_busquedaavanzada_PageIndexChanging"
                PagerSettings-Position="Top" PagerStyle-HorizontalAlign="Center" OnRowDataBound="gv_busquedaavanzada_RowDataBound"
                PagerStyle-ForeColor="#CC0000" PagerSettings-NextPageText=">" PagerSettings-LastPageText=">>"
                PagerSettings-PreviousPageImageUrl="<" PagerSettings-Mode="NumericFirstLast"
                PagerStyle-CssClass="PagerControl" OnSorting="gv_busquedaavanzada_Sorting">
                <Columns>
                    <asp:BoundField DataField="Id_Proyecto" HeaderText="Código" SortExpression="Id_Proyecto"
                        HeaderStyle-Width="5%" />
                    <asp:BoundField DataField="NomSubSector" HeaderText="Sector" SortExpression="NomSubSector"
                        HeaderStyle-Width="20%" />
                    <asp:BoundField DataField="NomCiudad" HeaderText="Localización Proyecto" SortExpression="NomCiudad"
                        HeaderStyle-Width="20%" />
                    <asp:TemplateField SortExpression="NomProyecto" HeaderText="Plan de Negocio" HeaderStyle-Width="25%">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnidproyecto" runat="server" CommandArgument='<%# Eval("Id_Proyecto") %>'
                                Text='<%# Eval("NomProyecto") %>' CommandName='<%# MostrarPlanOEmpresa %>' ForeColor="#CC0000" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CiudadUnidad" HeaderText="Localización Unidad" SortExpression="CiudadUnidad"
                        HeaderStyle-Width="10%" />
                    <asp:BoundField DataField="Nomunidad" HeaderText="Unidad" SortExpression="Nomunidad"
                        HeaderStyle-Width="20%" />
                    <asp:BoundField DataField="Sumario" HeaderText="Descripción" HeaderStyle-Width="25%" />
                    <asp:BoundField DataField="NomEstado" HeaderText="Estado" SortExpression="NomEstado"
                        HeaderStyle-Width="10%" />
                    <asp:BoundField DataField="Asesor" HeaderText="Asesor" SortExpression="Asesor" HeaderStyle-Width="10%"
                        ConvertEmptyStringToNull="true" />
                    <asp:BoundField DataField="Lider" HeaderText="Lider" SortExpression="Lider" HeaderStyle-Width="10%"
                        ConvertEmptyStringToNull="true" />
                    <asp:BoundField DataField="CodConvocatoria" HeaderText="Número Ultima Convocatoria"
                        HeaderStyle-Width="10%" SortExpression="CodConvocatoria" />
                    <asp:BoundField DataField="N_NomConvocatoria" HeaderText="Nombre Ultima Convocatoria"
                        HeaderStyle-Width="10%" />
                    <asp:BoundField DataField="NombreOperador" HeaderText="Operador" SortExpression="NombreOperador"
                        HeaderStyle-Width="10%" />
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
    <br />
    <asp:Panel ID="pnlpanel5" runat="server" Visible="false">
        <asp:GridView ID="gvcontacto" runat="server" AutoGenerateColumns="false" CssClass="Grilla" CellSpacing="1" GridLines="None"
            Width="100%" OnSorting="gvcontacto_Sorting" AllowSorting="True" OnPageIndexChanging="gvcontacto_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Tipo Documento" DataField="NomTipoIdentificacion" SortExpression="NomTipoIdentificacion" />
                <asp:BoundField HeaderText="Número Documento" DataField="Identificacion" SortExpression="Identificacion" />
                <asp:BoundField HeaderText="Nombres" DataField="Nombre" SortExpression="Nombre" />
                <asp:TemplateField HeaderText="Email" SortExpression="Email">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                            Text='<%# Eval("Email") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Unidad Emprendimiento" DataField="Unidad" SortExpression="Unidad" />
                <asp:BoundField HeaderText="Ciudad" DataField="ciudad" SortExpression="Ciudad" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnlpanel6" runat="server" Visible="false">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" PageSize="10" CssClass="Grilla" CellSpacing="1" GridLines="None"
            Width="100%" OnSorting="GridView1_Sorting" OnPageIndexChanging="GridView1_PageIndexChanging">
            <Columns>
                <asp:BoundField HeaderText="Tipo Institución" DataField="NomTipoInstitucion" SortExpression="NomTipoInstitucion" />
                <asp:BoundField HeaderText="Unidad" DataField="unidad" SortExpression="unidad" />
                <asp:BoundField HeaderText="Ciudad" DataField="ciudad" SortExpression="ciudad" />
                <asp:BoundField HeaderText="Nombre Jefe Unidad" DataField="Nombre" SortExpression="Nombre" />
                <asp:TemplateField HeaderText="Email Jefe Unidad" SortExpression="Email">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                            Text='<%# Eval("Email") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Teléfono" DataField="Telefono" SortExpression="Telefono" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="AsesoresPanel" runat="server" ClientIDMode="Static" BackColor="White" BorderStyle="None" Visible="false" BorderWidth="1px" Height="300px" Width="500px">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <br />
                <asp:TextBox ID="tb_asesor" Visible="false" runat="server" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))" Width="150px" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_buscarAsesor" runat="server" OnClick="btn_buscarAsesor_onclick" Text="Buscar Asesor" Visible="false" />
                <asp:ListBox ID="lb_asesores" runat="server" ClientIDMode="Static" DataSourceID="lds_asesores" DataTextField="Nombre" DataValueField="Id_Contacto" Height="100px" Width="500px" />

                <br>
                <input type="button" value="Aceptar" onclick="hidePopup()" />
                <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/icocanceladas.gif" CausesValidation="False" ClientIDMode="Static" OnClick="ImageButton2_Click" Style="float: right;" />
                <asp:HiddenField ID="hdf_CodContacto" runat="server" ClientIDMode="Static" />

            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tb_asesor" EventName="TextChanged" />
                <asp:AsyncPostBackTrigger ControlID="ImageButton2" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
