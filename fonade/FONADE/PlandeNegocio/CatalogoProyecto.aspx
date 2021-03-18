<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="CatalogoProyecto.aspx.cs" Inherits="Fonade.FONADE.PlandeNegocio.CatalogoProyecto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <%--<script type="text/javascript">
        function SetActiveTab(tabControl, tabNumber) {
            var ctrl = $find(tabControl);
            ctrl.set_activeTab(ctrl.get_tabs()[tabNumber]);
        }
        function cambiartab() {
            SetActiveTab('<%=tc_Emprendedor.ClientID%>', 1);
            return false;
        }
    </script>--%>
    <style>
        .tablaEstilo { 
            padding: 0px;
            margin: 0px;
}
    </style>
    <script type="text/javascript">
        var tb_fechaInicioperfil = null;
        var id_fechafinalizacion = null;
        var tb_fechagraduacionperfil = null;

        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }

        function alerta() {
            return confirm('¿Está seguro que desea eliminar la actividad seleccionada?');
        }

        var txtItm = function (eventSource) {
            var itm = eventSource.id;
            switch (itm) {
                case "bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechaInicioperfil": {
                    if (tb_fechaInicioperfil == null) tb_fechaInicioperfil = eventSource.value;
                    if (eventSource.value == null && tb_fechaInicioperfil != null) eventSource.value = tb_fechaInicioperfil;
                    break;
                }
                case "bodyContentPlace_tc_Emprendedor_tp_perfil_id_fechafinalizacion": {
                    if (id_fechafinalizacion == null) id_fechafinalizacion = eventSource.value;
                    if (eventSource.value == null && id_fechafinalizacion != null) eventSource.value = id_fechafinalizacion;
                    break;
                }
                case "bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechagraduacionperfil": {
                    if (tb_fechagraduacionperfil == null) tb_fechagraduacionperfil = eventSource.value;
                    if (eventSource.value == null && tb_fechagraduacionperfil != null) eventSource.value = tb_fechagraduacionperfil;
                    break;
                }
                default: { break; }
            }
        }
    </script>
    <script type="text/javascript">
        var controlFechas = function () {
            //var brfi = document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').selectedIndex > 0;
            var brfi = document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').value;

            //Diego Quiñonez
            //11 de Diciembre de 2014
            if (brfi == "0") {
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_id_fechafinalizacion').style.display = "none";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechagraduacionperfil').style.display = "none";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_Image3').style.display = "none";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_lblfecfaFinalizacion').style.display = "none";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_lblfecfaGraduacion').style.display = "none";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_Image2').style.display = "none";
            } else {
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_id_fechafinalizacion').style.display = "inline-block";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechagraduacionperfil').style.display = "inline-block";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_Image3').style.display = "inline-block";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_lblfecfaFinalizacion').style.display = "inline-block";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_lblfecfaGraduacion').style.display = "inline-block";
                document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_Image2').style.display = "inline-block";
            }
        }
    </script>
    <style type="text/css">
        .boton_Link001 {
            background: none;
            border: none;
            color: white;
            border-collapse: collapse;
            text-decoration: underline;
            color: blue;
        }

        #bodyContentPlace_tc_Emprendedor_tp_perfil_Calendarextender2_container {
            position: none !important;
            height: auto !important;
        }

        #bodyContentPlace_tc_Emprendedor_tp_perfil_Calendarextender1_container {
            position: none !important;
            height: auto !important;
        }

        #bodyContentPlace_tc_Emprendedor_tp_perfil_Calendarextender3_container {
            position: none !important;
            height: auto !important;
        }

        #bodyContentPlace_tc_Emprendedor_tp_perfil_Calendarextender4_container {
            position: none !important;
            height: auto !important;
        }

        .zoom{
             -moz-transform:    scale(0.9);
             -o-transform:      scale(0.9);
             -webkit-transform: scale(0.9);
             transform:         scale(0.9);

             /* IE8+ - must be on one line, unfortunately */ 
             -ms-filter: "progid:DXImageTransform.Microsoft.Matrix(M11=0.9, M12=0, M21=0, M22=0.9, SizingMethod='auto expand')";
   
            /* IE6 and 7 */ 
            filter: progid:DXImageTransform.Microsoft.Matrix(
                        M11=0.9,
                        M12=0,
                        M21=0,
                        M22=0.9,
                        SizingMethod='auto expand');
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:LinqDataSource ID="lds_Proyectos" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Proyectos_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_Emprendedor" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Emprendedor_Selecting">
    </asp:LinqDataSource>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" /></h1>
    <asp:Panel ID="pnl_Proyectos" runat="server">
        <asp:ImageButton ID="imgBtnAdd_Emrpendedor" ImageUrl="../../../Images/icoAdicionarUsuario.gif"
            runat="server" AlternateText="image" />
        <asp:HyperLink ID="AgregarProyecto" NavigateUrl="~/FONADE/PlandeNegocio/CatalogoProyecto.aspx?Accion=Crear"
            runat="server" Text="Agregar Plan de Negocio" />
        <asp:ImageButton ID="ImageButton1" ImageUrl="../../../Images/icoAdicionarUsuario.gif" runat="server" PostBackUrl="~/FONADE/PlandeNegocio/PlanDeNegocio.aspx" AlternateText="image" />
        <asp:GridView ID="gv_Proyectos" runat="server" Width="100%" AutoGenerateColumns="False"
            DataKeyNames="ID_Proyecto" CssClass="Grilla" AllowPaging="false" DataSourceID="lds_Proyectos"
            AllowSorting="True" OnRowDataBound="gv_Proyectos_RowDataBound" OnDataBound="gv_Proyectos_DataBound"
            OnRowCreated="gv_Proyectos_RowCreated" OnPageIndexChanging="gv_Proyectos_PageIndexChanged">
            <Columns>
                <asp:TemplateField HeaderText="Nombre" SortExpression="NomProyecto">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Proyecto" runat="server" NavigateUrl='<%# "CatalogoProyecto.aspx?Accion=Editar&CodProyecto="+ Eval("ID_Proyecto") %>'
                            Text='<%# Eval("Proyecto")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Ciudad" DataField="Ciudad" SortExpression="Ciudad" />
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_crearEditar" runat="server">
        <asp:Table ID="tbl_Proyecto" runat="server">
            <asp:TableRow>
                <asp:TableCell>Nombre:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_Nombre" runat="server" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Descripción:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_Descripcion" TextMode="MultiLine" runat="server" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Lugar de Ejecución:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddl_depto1" runat="server" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                        OnSelectedIndexChanged="ddl_depto1_SelectedIndexChanged" Width="400px" />
                    <br />
                    <asp:UpdatePanel ID="panelDropDowList" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_ciudad1" runat="server" Width="400px" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_depto1" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:TableCell></asp:TableRow><asp:TableRow>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Sector:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddl_depto2" runat="server" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                        OnSelectedIndexChanged="ddl_depto2_SelectedIndexChanged" Width="400px" />
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddl_ciudad2" runat="server" Width="400px" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="ddl_depto2" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </asp:TableCell></asp:TableRow></asp:Table><asp:Button ID="btn_crearActualizar" OnClick="btn_crearActualizar_onclick" runat="server"
            Text="Actualizar" />
        <ajaxToolkit:ConfirmButtonExtender Enabled="false" ID="cbe1" runat="server" DisplayModalPopupID="mpe1"
            TargetControlID="btn_crearActualizar">
        </ajaxToolkit:ConfirmButtonExtender>
        <ajaxToolkit:ModalPopupExtender ID="mpe1" runat="server" PopupControlID="pnlPopup1"
            TargetControlID="btn_crearActualizar" OkControlID="btnYes" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="pnlPopup1" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header">
                Confirmación </div><div class="body">
                <asp:Label ID="lbl_popup" runat="server" />
            </div>
            <div class="footer" align="right">
                <asp:Button ID="btnYes" runat="server" Text="Aceptar" />
            </div>
        </asp:Panel>
    </asp:Panel>
    <br />
    <br />
    <asp:UpdatePanel runat="server" ID="pnl_Emprendedor">
        <ContentTemplate>
            <asp:ImageButton ID="image_i" runat="server" ImageUrl="~/Images/icoAdicionarUsuario.gif"
                OnClick="image_i_Click" />
            <asp:LinkButton ID="sd" runat="server" Text="Agregar Emprendedor" OnClientClick="ClearAllControls" OnClick="btn_CrearEmprendedor_Click" />
            <br />
            <br />
            <asp:GridView ID="gv_emprendedor" runat="server" Width="100%" AutoGenerateColumns="False"
                DataKeyNames="" CssClass="Grilla" AllowPaging="false" DataSourceID="lds_Emprendedor"
                AllowSorting="True" OnRowCommand="gv_emprendedor_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtn_EliminarEmprendedor" ImageUrl="/Images/icoBorrar.gif"
                                runat="server" CausesValidation="false" CommandName="Borrar" CommandArgument='<%# Eval("CodProyecto") +";"+ Eval("Id_contacto") %>'
                                OnClientClick="return alerta();" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nombre" SortExpression="NomProyecto">
                        <ItemTemplate>
                            <asp:LinkButton ID="hl_Emprendedor" CommandArgument='<%# Eval("CodProyecto") +";"+ Eval("Id_contacto") %>'
                                Text='<%# Eval("Nombre")%>' runat="server" CommandName="Editar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email" SortExpression="Email">
                        <ItemTemplate>
                            <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                                Text='<%# Eval("Email") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>


            <asp:Panel ID="pnl_infoAcademica" runat="server">
                <ajaxToolkit:TabContainer ID="tc_Emprendedor" runat="server" ActiveTabIndex="0" Width="100%"
                    Height="80%">
                    <ajaxToolkit:TabPanel ID="tp_perfil" runat="server" HeaderText="Asignar Perfil" Width="100%"
                        Height="100%">
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server">
                                <asp:Table ID="tbl_Convenio" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell>Nombres:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_nombreperfil" runat="server" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Apellidos:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_apellidoperfil" runat="server" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Identificación:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList runat="server" ID="ddl_Tipodocumentoperfil" nombre="Tipo documento" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>No:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_nodocperfil" runat="server" MaxLength="10" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Departamento expedición:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList ID="ddl_deptoexpedicionperfil" runat="server" nombre="Dpto. expedición" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                                                OnSelectedIndexChanged="ddl_deptoexpedicionperfil_SelectedIndexChanged" Width="400px" />
                                            <br />
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddl_ciudadexpedicionperfil" nombre="Ciudad expedición" runat="server" Width="400px" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddl_deptoexpedicionperfil" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Correo electrónico:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_correoperfil" runat="server" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Género:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList runat="server" ID="ddl_generoperfil" nombre="Género">
                                                <asp:ListItem Text="Seleccione" Value="0" />
                                                <asp:ListItem Text="Masculino" Value="M" />
                                                <asp:ListItem Text="Femenino" Value="F" />
                                            </asp:DropDownList>
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Fecha Nacimiento:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="tb_fechanacimiento" placeholder="dd/mm/aaaa" />
                                            <asp:Image runat="server" ID="Image1" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender2" PopupButtonID="Image1"
                                                CssClass="ajax__calendar" TargetControlID="tb_fechanacimiento" Format="dd/MM/yyyy" ClearTime="True" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Departamento nacimiento:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList ID="ddl_deptonacimientoperfil" runat="server" nombre="Dpto. nacimiento" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                                                OnSelectedIndexChanged="ddl_deptonacimientoperfil_SelectedIndexChanged" Width="400px" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Ciudad nacimiento:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList ID="ddl_ciudadonacimientoperfil" nombre="Ciudad nacimiento" runat="server" Width="400px" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Teléfono:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="tb_telefonoperfil" Text="" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Nivel de estudio:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList ID="ddl_nivelestudioperfil" runat="server" nombre="Nivel estudio" AutoPostBack="true" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Programa Realizado:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_Programarealizadoperfil" runat="server" Enabled="false" />
                                            <asp:ImageButton ID="imbtn_institucion" ImageUrl="~/Images/icoComentario.gif" runat="server"
                                                OnClick="imbtn_institucion_Click" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Institución:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox ID="tb_Institucionperfil" runat="server" Enabled="false" />
                                            <asp:ImageButton ID="imbtn_nivel" ImageUrl="~/Images/icoComentario.gif" runat="server"
                                                OnClick="imbtn_nivel_Click" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Ciudad Institución:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="tb_ciudadinstitucionperfil" Enabled="false" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Estado:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList runat="server" ID="ddl_estadoperfil" nombre="Estado estudio">
                                                <asp:ListItem Text="Seleccione" Value="-" />
                                                <asp:ListItem Text="Finalizado" Value="1" />
                                                <asp:ListItem Text="Actualmente cursando" Value="0" />
                                            </asp:DropDownList>
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Fecha inicio:</asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="tb_fechaInicioperfil" placeholder="dd/mm/aaaa" />
                                            <asp:Image runat="server" ID="img_dateInicio" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender1" PopupButtonID="img_dateInicio"
                                                CssClass="ajax__calendar" TargetControlID="tb_fechaInicioperfil" Format="dd/MM/yyyy" ClearTime="True" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow ID="filafecha" runat="server">
                                        <asp:TableCell>
                                            <asp:Label ID="lblfecfaFinalizacion" runat="server" Text="Fecha Finalización de Materias:"></asp:Label>
                                        </asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="id_fechafinalizacion" placeholder="dd/mm/aaaa" />
                                            <asp:Image runat="server" ID="Image3" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender4" PopupButtonID="Image3"
                                                CssClass="ajax__calendar" TargetControlID="id_fechafinalizacion" Format="dd/MM/yyyy" ClearTime="True" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow ID="filafecha2" runat="server">
                                        <asp:TableCell>
                                            <asp:Label ID="lblfecfaGraduacion" runat="server" Text="Fecha Graduación:"></asp:Label>
                                        </asp:TableCell><asp:TableCell>
                                            <asp:TextBox runat="server" ID="tb_fechagraduacionperfil" placeholder="dd/mm/aaaa" />
                                            <asp:Image runat="server" ID="Image2" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender3" PopupButtonID="Image2"
                                                CssClass="ajax__calendar" TargetControlID="tb_fechagraduacionperfil" Format="dd/MM/yyyy" DefaultView="Days" ClearTime="True" />
                                        </asp:TableCell></asp:TableRow><asp:TableRow Visible="false">
                                        <asp:TableCell>¿Usted tiene alguna condición especial?:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList runat="server" ID="ddl_condicionespecialperfil">
                                                <asp:ListItem Text="Sí" Value="1" />
                                                <asp:ListItem Text="No" Value="0" Selected="True" />
                                            </asp:DropDownList>
                                        </asp:TableCell></asp:TableRow><asp:TableRow>
                                        <asp:TableCell>Tipo de Aprendiz:</asp:TableCell><asp:TableCell>
                                            <asp:DropDownList runat="server" ID="ddl_tipoaprendizperfil" nombre="Tipo aprendiz" />
                                        </asp:TableCell></asp:TableRow></asp:Table></asp:Panel></ContentTemplate></ajaxToolkit:TabPanel><ajaxToolkit:TabPanel ID="tp_infoacademica" runat="server" HeaderText="Agregar información Académica">
                        <ContentTemplate>
                            <!--Seleccionar Información Académica.-->
                            <asp:Panel ID="pnl_Convenios" runat="server">
                                <table width="100%" border="0">
                                    <tbody>
                                        <tr>
                                            <td>Buscar </td><td colspan="28">
                                                <asp:TextBox ID="txtBusqueda" runat="server" />
                                            </td>
                                            <td>Ciudad </td><td>
                                                <asp:DropDownList ID="SelCiudad" runat="server" />
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_Buscar_Programa" Text="Buscar" runat="server" OnClick="btn_Buscar_Programa_Click" />
                                            </td>
                                            <td>&nbsp; </td><td align="right">
                                                <asp:LinkButton ID="lnkBtn_CrearPrograma" Text="Crear <br /> programa <br /> académico"
                                                    runat="server" ForeColor="Black" Style="text-decoration: none;" OnClick="lnkBtn_CrearPrograma_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Pagina" Text="" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="6">
                                                <asp:LinkButton ID="lnkbtn_opcion_A" Text="A" CssClass="opcionUno" runat="server"
                                                    Style="font: bold; color: Blue; text-decoration: none;" OnClick="lnkbtn_opcion_A_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_B" Text="B" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_B_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_C" Text="C" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_C_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_D" Text="D" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_D_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_E" Text="E" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_E_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_F" Text="F" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_F_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_G" Text="G" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_G_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_H" Text="H" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_H_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_I" Text="I" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_I_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_J" Text="J" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_J_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_K" Text="K" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_K_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_L" Text="L" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_L_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_M" Text="M" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_M_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_N" Text="N" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_N_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_O" Text="O" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_O_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_P" Text="P" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_P_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_Q" Text="Q" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_Q_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_R" Text="R" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_R_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_S" Text="S" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_S_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_T" Text="T" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_T_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_U" Text="U" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_U_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_V" Text="V" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_V_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_W" Text="W" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_W_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_X" Text="X" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_X_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_Y" Text="Y" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_Y_Click" />
                                                <asp:LinkButton ID="lnkbtn_opcion_Z" Text="Z" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                                    OnClick="lnkbtn_opcion_Z_Click" />
                                                <strong style="font: bold; color: Blue; text-decoration: none;">|</strong> <asp:LinkButton ID="lnkbtn_opcion_todos" Text="Todos" runat="server" OnClick="lnkbtn_opcion_todos_Click"
                                                    Style="font: bold; color: Blue; text-decoration: none;" />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <asp:GridView ID="gv_institucion" runat="server" AutoGenerateColumns="false" CssClass="Grilla"
                                    HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" Width="100%"
                                    PageSize="20" ShowHeaderWhenEmpty="true" EmptyDataText="No hay datos." AllowPaging="true"
                                    OnPageIndexChanging="gv_institucion_PageIndexChanging" OnRowDataBound="gv_institucion_RowDataBound"
                                    OnRowCommand="gv_institucion_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Programa Académico">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk_nom_programa_ac" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# Eval("ID_PROGRAMAACADEMICO") +";"+ Eval("NOMPROGRAMAACADEMICO") +";"+ Eval("NOMCIUDAD")+";"+ Eval("NOMINSTITUCIONEDUCATIVA") %>'
                                                    CommandName="seleccionar_1" Text='<%# Eval("NOMPROGRAMAACADEMICO")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Institución Educativa">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk_inst_educativa" runat="server" CausesValidation="false" CommandArgument='<%# Eval("ID_PROGRAMAACADEMICO") +";"+ Eval("NOMPROGRAMAACADEMICO") +";"+ Eval("NOMCIUDAD")+";"+ Eval("NOMINSTITUCIONEDUCATIVA") %>'
                                                    CommandName="seleccionar_2" Text='<%# Eval("NOMINSTITUCIONEDUCATIVA")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ciudad">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk_ciudad" runat="server" CausesValidation="false" CommandArgument='<%# Eval("ID_PROGRAMAACADEMICO") +";"+ Eval("NOMPROGRAMAACADEMICO") +";"+ Eval("NOMCIUDAD")+";"+ Eval("NOMINSTITUCIONEDUCATIVA") %>'
                                                    CommandName="seleccionar_3" Text='<%# Eval("NOMCIUDAD")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <div style="float: right; height: 50px;">
                                    <asp:Button ID="btn_volver" Text="cerrar" runat="server" OnClick="btn_volver_Click" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="Panel2" runat="server">
                                <!--Crear Información Académica.-->
                                <table>
                                    <tbody>
                                        <tr>
                                            <td colspan="2">Institución: </td></tr><tr>
                                            <td colspan="2">
                                                <asp:DropDownList ID="SelInstitucion" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">&nbsp; </td></tr><tr id="trOtraInstitucion" style="display: none">
                                            <td nowrap="">Otra Institución </td><td>
                                                <asp:TextBox ID="txtOtraInstitucion" runat="server" MaxLength="200" size="70" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td nowrap="">Nombre del Programa </td><td>
                                                <asp:TextBox ID="txtNomPrograma" runat="server" MaxLength="200" size="70" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Departamento de institución </td><td>
                                                <asp:DropDownList ID="SelDptoExpedicion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="SelDptoExpedicion_OnSelectedIndexChanged" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Ciudad de institución </td><td>
                                                <asp:UpdatePanel ID="updt_depto" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                                    <ContentTemplate>
                                                        <asp:DropDownList ID="SelMunExpedicion" runat="server" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="SelDptoExpedicion" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <center>
                                    <asp:Button ID="btn_CrearPrograma" Text="Crear Programa" runat="server" OnClick="btn_CrearPrograma_Click" />
                                    <asp:Button ID="btn_Ocultar" Text="Ocultar" runat="server" OnClick="btn_Ocultar_Click" />
                                </center>
                            </asp:Panel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
                <asp:Button ID="VistaPreviaButton" runat="server" Text="Vista Previa" OnClientClick="return  PerformValidations();" />
                <asp:ModalPopupExtender ID="VistaPreviaButton_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" CancelControlID="cnclBtn"
                    DynamicServicePath="" Enabled="True" OkControlID="tstBtn" PopupControlID="VistapreviaPanel" TargetControlID="VistaPreviaButton" BehaviorID="VistaPreviaButton_ModalPopupExtender">
                </asp:ModalPopupExtender>
                <%Page.DataBind();%>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="VistapreviaPanel" runat="server">
                            <div id="cntnt" runat="server" class="TitulosRegistrosGrilla zoom" visible="true" style="text-align: left; vertical-align: top; justify-content: flex-start; align-content: flex-start; background-color: rgb(240, 240, 240);">
                                <fieldset title="Validar datos de emprendedor" style="align-content: center;">
                                    <legend>Confirmar datos del emprendedor a registrar</legend><table>
                                        <tr>
                                            <td><b>Nombres:</b></td><td>
                                                <asp:TextBox runat="server" ID="nombre_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Apellidos:</b></td><td>
                                                <asp:TextBox runat="server" ID="apellido_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Identificación:</b></td><td>
                                                <asp:TextBox runat="server" ID="Tipodocumento_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>No:</b></td><td>
                                                <asp:TextBox runat="server" ID="nodoc_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Departamento expedición:</b></td><td>
                                                <asp:TextBox runat="server" ID="deptoexpedicion_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Ciudad expedición:</b></td><td>
                                                <asp:TextBox runat="server" ID="ciudadexpedicion_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Correo electrónico:</b></td><td>
                                                <asp:TextBox runat="server" ID="correo_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Género:</b></td><td>
                                                <asp:TextBox runat="server" ID="genero_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Fecha Nacimiento:</b></td><td>
                                                <asp:TextBox runat="server" ID="fecha_nacimiento" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Departamento nacimiento:</b></td><td>
                                                <asp:TextBox runat="server" ID="deptonacimiento_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Ciudad nacimiento:</b></td><td>
                                                <asp:TextBox runat="server" ID="ciudadonacimiento_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Teléfono:</b></td><td>
                                                <asp:TextBox runat="server" ID="telefono_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Nivel de estudio:</b></td><td>
                                                <asp:TextBox runat="server" ID="nivelestudio_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Programa Realizado:</b></td><td>
                                                <asp:TextBox runat="server" ID="Programarealizado_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Institución:</b></td><td>
                                                <asp:TextBox runat="server" ID="Institucion_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Ciudad Institución:</b></td><td>
                                                <asp:TextBox runat="server" ID="ciudadinstitucion_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Estado:</b></td><td>
                                                <asp:TextBox runat="server" ID="estado_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Fecha inicio:</b></td><td>
                                                <asp:TextBox runat="server" ID="fecha_Inicioperfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Fecha Finalización de Materias:</b></td><td>
                                                <asp:TextBox runat="server" ID="fecha_finalizacion" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Fecha Graduación:</b></td><td>
                                                <asp:TextBox runat="server" ID="fechagraduacion_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td><b>Tipo de Aprendiz:</b></td><td>
                                                <asp:TextBox runat="server" ID="tipoaprendiz_perfil" class="aspNetDisabled"></asp:TextBox></td></tr><tr>
                                            <td colspan="2">
                                                <div style="text-align: right">
                                                    <asp:Button ID="cnclBtn" runat="server" Text="Editar" Visible="true" />
                                                    <asp:Button ID="tstBtn" runat="server" Text="Guardar" Visible="true"
                                                         />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                            </div>

                            </fieldset> 

                            
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="gv_emprendedor" EventName="RowCommand" />
                        <asp:AsyncPostBackTrigger ControlID="sd" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddl_depto1" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="VistaPreviaButton" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server" />
    <script type="text/javascript">
        var PerformValidations = function () {
            //var yhn = new Array('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechanacimiento', 'bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechaInicioperfil', 'bodyContentPlace_tc_Emprendedor_tp_perfil_id_fechafinalizacion', 'bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechagraduacionperfil');
            var yhn = new Array('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechanacimiento', 'bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechaInicioperfil');
            //var brfi = document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').selectedIndex > 0;
            var brfi = document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').value;
            var dttCompNac, dttCompIni, dttCompFini, dttCompGrd = new Date();
            for (d in yhn) {
                var isObj = $get(yhn[d]);
                if (isObj != null) {
                    with (isObj) {
                        if (!isNaN(new Date(value.split('/').reverse().join('/').toString()))) {
                            var evlIdx = d;
                            switch (evlIdx) {
                                case "0": {
                                    dttCompNac = new Date(value.split('/').reverse().join('/').toString());
                                    if (dttCompNac.getFullYear() > new Date().getFullYear() - 10) {
                                        yhn[d] = null;
                                    }
                                    break;
                                }
                                case "1": {
                                    dttCompIni = new Date(value.split('/').reverse().join('/').toString());
                                    if (dttCompIni.getFullYear() > new Date().getFullYear() - 1) {
                                        yhn[d] = null;
                                    }
                                    break;
                                }
                                case "2": {
                                    dttCompFini = new Date(value.split('/').reverse().join('/').toString());
                                    if (brfi == "0") {
                                        if (dttCompFini <= dttCompIni || dttCompFini >= new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())) { yhn[d] = null; }
                                    }
                                    break;
                                }
                                case "3": {
                                    dttCompGrd = new Date(value.split('/').reverse().join('/').toString());
                                    if (brfi == "0") {
                                        if (dttCompGrd <= dttCompFini || dttCompGrd >= new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate())) { yhn[d] = null; }
                                    }
                                    break;
                                }
                                default: {
                                    break;
                                }
                            }
                        }
                        if (value == '' || value == null) { yhn[d] = null; }
                    }
                }
            }
            var idxCndt = yhn.indexOf(null);
            if (idxCndt > -1) {
                var rfv = new String('Error /%d');
                switch (idxCndt) {
                    case 0: { rfv = rfv.replace('/%d', 'fecha de nacimiento'); break; } case 1: { rfv = rfv.replace('/%d', 'fecha inicio'); break; }
                    case 2: { rfv = rfv.replace('/%d', 'fecha finalizacion'); break; } case 3: { rfv = rfv.replace('/%d', 'fecha graduación'); break; }
                }
                alert("Existen errores en los datos: \n" + rfv);
                window.event.stopImmediatePropagation();
                event.cancelBubble = true;
                event.stopPropagation();
                event.preventDefault();
                document.execCommand('Stop');
                window.event.stopPropagation();
                window.event.preventDefault();
                window.event.cancelBubble = true;
                window.event.stopImmediatePropagation = true;
                return false;
            }
            else {
                if (document.getElementById('bodyContentPlace_cntnt') != null) {
                    document.getElementById('bodyContentPlace_nombre_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_nombreperfil').value;
                    document.getElementById('bodyContentPlace_apellido_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_apellidoperfil').value;
                    document.getElementById('bodyContentPlace_Tipodocumento_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_Tipodocumentoperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_Tipodocumentoperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_Tipodocumentoperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_nodoc_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_nodocperfil').value;
                    document.getElementById('bodyContentPlace_deptoexpedicion_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_deptoexpedicionperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_deptoexpedicionperfil').selectedIndex].innerText;
                    document.getElementById('bodyContentPlace_ciudadexpedicion_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadexpedicionperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadexpedicionperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadexpedicionperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_correo_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_correoperfil').value;
                    document.getElementById('bodyContentPlace_genero_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_generoperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_generoperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_generoperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_fecha_nacimiento').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechanacimiento').value;
                    document.getElementById('bodyContentPlace_deptonacimiento_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_deptonacimientoperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_deptonacimientoperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_deptonacimientoperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_ciudadonacimiento_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadonacimientoperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadonacimientoperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_ciudadonacimientoperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_telefono_perfil').value = document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_telefonoperfil').value;
                    document.getElementById('bodyContentPlace_nivelestudio_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_nivelestudioperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_nivelestudioperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_nivelestudioperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_Programarealizado_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_Programarealizadoperfil').value;
                    document.getElementById('bodyContentPlace_Institucion_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_Institucionperfil').value;
                    document.getElementById('bodyContentPlace_ciudadinstitucion_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_ciudadinstitucionperfil').value;
                    document.getElementById('bodyContentPlace_estado_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').selectedIndex].innerText : "";
                    document.getElementById('bodyContentPlace_fecha_Inicioperfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechaInicioperfil').value;
                    if (document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_estadoperfil').value == "1") {
                        document.getElementById('bodyContentPlace_fecha_finalizacion').value =
                        document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_id_fechafinalizacion').value;
                        document.getElementById('bodyContentPlace_fechagraduacion_perfil').value =
                        document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_tb_fechagraduacionperfil').value;
                    }
                    else {
                        document.getElementById('bodyContentPlace_fecha_finalizacion').value = "";
                        document.getElementById('bodyContentPlace_fechagraduacion_perfil').value = "";
                    }
                    document.getElementById('bodyContentPlace_tipoaprendiz_perfil').value =
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_tipoaprendizperfil').options.length > 0 ?
                    document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_tipoaprendizperfil').options[document.getElementById('bodyContentPlace_tc_Emprendedor_tp_perfil_ddl_tipoaprendizperfil').selectedIndex].innerText : "";
                }
            }
            ValidarCombos();
        }

        function ClearAllControls() {
            for (i = 0; i < document.forms[0].length; i++) {
                doc = document.forms[0].elements[i];
                switch (doc.type) {
                    case "text":
                        doc.value = "";
                        break;
                    case "checkbox":
                        doc.checked = false;
                        break;
                    case "radio":
                        doc.checked = false;
                        break;
                    case "select-one":
                        doc.options[doc.selectedIndex].selected = false;
                        break;
                    case "select-multiple":
                        while (doc.selectedIndex != -1) {
                            indx = doc.selectedIndex;
                            doc.options[indx].selected = false;
                        }
                        doc.selected = false;
                        break;

                    default:
                        break;
                }
            }
        }

        function ValidarCombos() {
            var objSelec = document.getElementsByTagName('select');
            for (var i in objSelec) {
                var atributo = objSelec[i].getAttribute('nombre');
                if(atributo == 'Tipo documento' || atributo == 'Dpto. expedición' || atributo == 'Ciudad expedición' || 
                    atributo == 'Género' || atributo == 'Dpto. nacimiento' || atributo == 'Ciudad nacimiento' || 
                    atributo == 'Nivel estudio' || atributo == 'Estado estudio' || atributo == 'Tipo aprendiz') {
                    if (objSelec[i].selectedIndex == 0) {
                        objSelec[i].focus();
                        document.getElementById('bodyContentPlace_tstBtn').disabled = true;
                        alert('Debe seleccionar una opción de ' + atributo);
                        return false;
                    } else {
                        document.getElementById('bodyContentPlace_tstBtn').disabled = false;
                    }
                }
            }
        }
    </script>
    <br />
    <br />
</asp:Content>
