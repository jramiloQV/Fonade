<%@ Page Language="C#" Title="FONDO EMPRENDER - Administrar Unidades de Emprendimiento"
    MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="~/FONADE/Administracion/CatalogoUnidadEmprende.aspx.cs"
    Inherits="Fonade.FONADE.Administracion.CatalogoUnidadEmprende" EnableEventValidation="false" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="udpPrincipal" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                function SoloNumeros(input) {
                    var num = input.value.replace(/\./g, '');
                    if (!isNaN(num)) {
                        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
                        num = num.split('').reverse().join('').replace(/^[\.]/, '');
                        input.value = num;
                    }

                    else {
                        alert('Solo se permiten numeros');
                        input.value = input.value.replace(/[^\d\.]*/g, '');
                    }
                }

                function OpenPopup(id) {
                    window.open('DesactivarUnidadEmprende.aspx?idUnidad=' + id, "List", "toolbar=no, location=no,status=yes,menubar=no,scrollbars=yes,resizable=no, width=360,height=200,left=430,top=100");
                    return false;
                }

            </script>
            <asp:Panel ID="pnlPrincipaal" runat="server">
                <h1>
                    <asp:Label ID="lbl_enunciado" runat="server" Text="" />
                </h1>
                <asp:Label ID="lblError" runat="server" Text="" Style="color: red;" />
                <table border="0" style="width: 100%;">
                    <tr>
                        <td style="text-align: left">
                            <asp:Label ID="lblvalidador" runat="server" Style="display: none" />
                            <asp:ImageButton ID="ImgBtnAdicionar" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif"
                                Style="cursor: pointer;" OnClick="ImgBtnAdicionar_Click" />
                            &nbsp;
                            <asp:LinkButton ID="lnkAddUnidad" runat="server" Text=" Adicionar Unidad" OnClick="lnkAddUnidad_Click" />
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="../../Images/icoAdicionarUsuario.gif" Style="cursor: pointer;" PostBackUrl="~/PlanDeNegocioV2/Administracion/UnidadEmprendimiento/UnidadesDeEmprendimiento.aspx" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;" colspan="4">
                            <asp:LinkButton ID="lnkbtn_opcion_A" Text="A" CssClass="opcionUno" runat="server" Style="font: bold; color: Blue; text-decoration: none;"
                                OnClick="lnkbtn_opcion_A_Click" />
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
                            <strong style="font: bold; color: Blue; text-decoration: none;">|</strong>
                            <asp:LinkButton ID="lnkOpcionTodos" Text="Todos" runat="server"
                                Style="font: bold; color: Blue; text-decoration: none;" OnClick="lnkOpcionTodos_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <asp:GridView ID="grvUnidadesEmprendimiento" runat="server" Width="98%" AutoGenerateColumns="false"
                                CssClass="Grilla" AllowSorting="true" ShowHeaderWhenEmpty="true" BorderWidth="1" CellSpacing="1" CellPadding="4"
                                PageSize="20" OnRowDataBound="grvUnidadesEmprendimiento_RowDataBound" OnRowCommand="grvUnidadesEmprendimiento_RowCommand" >
                                <PagerStyle CssClass="Paginador" />
                                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                <Columns>
                                    <asp:BoundField HeaderText="Id_Institucion" DataField="Id_Institucion" Visible="false" />
                                    <asp:TemplateField HeaderStyle-Width="3%">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="img_btn" ImageUrl="../../Images/icoBorrar.gif" runat="server" CausesValidation="false" CommandName="eliminar" 
                                                CommandArgument='<%# Eval("Id_Institucion") + ";"+Eval("NomUnidad") %>' />
                                            
                                        </ItemTemplate>
                                        <HeaderStyle Width="3%" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre Unidad" ControlStyle-CssClass="miClase" SortExpression="NomUnidad" HeaderStyle-Width="40%">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkeditar" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Id_Institucion")+ ";" + Eval("NomUnidad") %>'
                                                CommandName="editar" Text='<%# Eval("NomUnidad")+ " "  +"("+Eval("NomInstitucion")+")" %>' />
                                        </ItemTemplate>
                                        <ControlStyle CssClass="miClase" />
                                        <HeaderStyle Width="40%" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Tipo" DataField="NomTipoInstitucion" SortExpression="NomTipoInstitucion"
                                        HeaderStyle-Width="30%">
                                        <HeaderStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Estado" ItemStyle-ForeColor="Blue" HeaderStyle-Width="27%">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_id_inst" Text='<%# Eval("Id_Institucion") %>' runat="server" Visible="false" />
                                            <asp:Label ID="lbl_estado" Text='<%# Eval("Inactivo") %>' runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="27%" />
                                        <ItemStyle ForeColor="Blue" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="CodCiudad" DataField="CodCiudad" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlDetalles" runat="server" Visible="false">
                <!-- Nueva Unidad -->
                <div id="dNuevaUnidad" runat="server" aling="left">
                    <h1>
                        <asp:Label ID="lblNuevaUnidad" runat="server" Text="DATOS NUEVA UNIDAD" />
                    </h1>
                    <table style="border-color: #4E77AF; ">
                        <tr>
                            <td style="text-align: right; width:38%" class="auto-style3" >
                                <b>Tipo de Unidad</b>
                            </td>
                            <td style="width: 2%"></td>
                            <td style="text-align: left; width:60%">
                                <asp:DropDownList ID="ddlTipoInstitucion" runat="server" Width="179px" Style="height: 18px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Nombre Unidad</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNombreUnidad" runat="server" MaxLength="80" Width="219px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Nombre Centro o Institución</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNombreCentroInstitucion" runat="server" MaxLength="80" Width="219px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>NIT Centro o Institución</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNit" runat="server" MaxLength="80" Width="219px" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Departamento</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlDpto" runat="server" Width="179px" Style="height: 18px" OnSelectedIndexChanged="ddlDpto_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Ciudad</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlCiudades" runat="server" Width="179px" Style="height: 18px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Dirección de Correpondencia</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtDireccion" runat="server" MaxLength="100" Width="219px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <b>Criterios de Selección</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtCriterios" runat="server" TextMode="MultiLine" Rows="5" Columns="35" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; color: #000000; background-color: #E6E6E6;" class="auto-style5">
                                <b>Jefe de Unidad</b>
                            </td>
                            <td style="width: 5px"></td>
                            <td valign="middle"  style="text-align: left; color: #000000; background-color: #E6E6E6;" class="auto-style6">
                                <b><asp:Label ID="lblJefeUnidad" runat="server" Enabled ="false" Width="219px" Text="" ForeColor="Blue" /></b>
                                <asp:Button ID="btn_buscarJefeUnidad" Text="Buscar Jefe de Unidad" runat="server" BackColor="#6699FF" OnClick="btn_buscarJefeUnidad_Click" /><br />
                                <asp:Button ID="btn_cambiarDatosJefe" Text="Cambiar Datos Jefe" runat="server" Visible="false" OnClick="btn_cambiarDatosJefe_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">
                                <asp:Label ID="lbl_razonCambio" Text="Razón de cambio de Jefe de Unidad:" runat="server" />
                            </td>
                            <td></td>
                            <td style="width: 5px">
                                <asp:TextBox ID="txtCambioJefe" runat="server" Columns="35" Rows="5" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: right">
                                <asp:Button ID="btn_crearUnidad" CssClass="botonParaClick" Text="Crear" runat="server" OnClick="btn_crearUnidad_Click" />
                                <asp:Button ID="btnAtrasPrincipal" CssClass="openCargando" Text="Atras" runat="server" Visible="false" OnClick="btnAtrasPrincipal_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- Buscar Jefe unidad -->
                <div id="dBuscarJefe" runat="server" visible="false">
                    <h1>
                        <asp:Label ID="lblBuscarJefe" runat="server" Text="JEFE UNIDAD EMPRENDIMIENTO" />
                    </h1>
                    <table style="border-color: #4E77AF;">
                        <tr>
                            <td colspan="3" class="TituloDestacados">BUSCAR JEFE DE LA UNIDAD
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Tipo Documento
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlTipoIdentificacion" runat="server" Width="151px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Número Documento
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNumIdentificacion" runat="server" MaxLength="15" Width="215px" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3"></td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                                <asp:Button ID="btnAtras" runat="server" Text="Atras" OnClick="btnAtras_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- Datos Jefe -->
                <div id="dDatosJefe" runat="server" visible="false">
                    <h1>
                        <asp:Label ID="lblDatosJefe" runat="server" Text="DATOS JEFE UNIDAD" />
                    </h1>
                    <table>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Nombres
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtNombres" runat="server" MaxLength="50" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Apellidos
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtApellidos" runat="server" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">E-Mail
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" type="email" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tituloDestacados" colspan="3" style="text-align: center">
                                <b>Información de la Unidad</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Departamento
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlDptoJefe" runat="server" Width="172px" Style="height: 18px" OnSelectedIndexChanged="ddlDptoJefe_SelectedIndexChanged" AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Ciudad
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:DropDownList ID="ddlCiudadJefe" runat="server" Width="172px" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Telefono
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTelefonoUnidad" runat="server" MaxLength="15" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Fax
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtFax" runat="server" MaxLength="15" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Sitio Web
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtWebsite" runat="server" MaxLength="100" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td class="tituloDestacados" colspan="3" style="text-align: center">
                                <b>Datos Personales</b>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Cargo
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtCargo" runat="server" MaxLength="80" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Telefono
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTelefono" runat="server" MaxLength="15" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="auto-style3">Fax
                            </td>
                            <td style="width: 5px"></td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtFaxJefe" runat="server" MaxLength="15" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align: right">
                                <asp:Button ID="btnCrerJefe" runat="server" Text="Crear" OnClick="btnCrerJefe_Click"/>&nbsp;
                                <asp:Button ID="btnAtras2" runat="server" Text="Atras" OnClick="btnAtras2_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <!-- atos edit jefe unidad -->
                <div id="deditDatosFeje" runat="server" visible="false">
                    <table>
                        <tr>
                            <td colspan="2" style="text-align:left">
                                <b><h2>EDITAR DATOS</h2></b>
                            </td>
                        </tr>
                        <tr>
                            <td>Nombres</td>
                            <td>
                                <asp:TextBox ID="txtEditJefeNombres" runat="server" Width="200" /> 
                                <asp:TextBox ID="txtEditIdJefe" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>Apellidos</td>
                            <td>
                                <asp:TextBox ID="txtEditJefeApellidos" runat="server" Width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td>Identificación</td>
                            <td>
                                <asp:DropDownList ID="ddlEditTipoDocJefe" runat="server" Width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td>Número</td>
                            <td>
                                <asp:TextBox ID="txteditNumeroId" runat="server" Width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <asp:TextBox ID="txtEditEmailJefe" runat="server" Width="200" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnClosedEdit" runat="server" Text="Cerrar" OnClick="btnClosedEdit_Click" />
                                <asp:Button ID="btnUpdateJefe" runat="server" Text="Actualizar" OnClick="btnUpdateJefe_Click" />
                            </td>
                        </tr>
                    </table>
                </div>

                <!-- Lista jefes encontrados -->
                <div id="dListaJefes" runat="server" visible="false" >
                    <h1>
                        <asp:Label ID="Label1" runat="server" Text="SELECCIONAR EL JEFE DE LA UNIDAD" /><br />
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkUnidadJefe" runat="server" Text="" Font-Size="Medium" OnClick="lnkUnidadJefe_Click" />
                                </td>
                                <td style="width: 2px"></td>
                                <td>
                                    <span id="lblrol" runat="server" style="font-size:medium">Usuario sin rol</span>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <span id="lblTextoNoAsigna" runat="server" visible="false" style="font-size:medium; color:black" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="text-align:right">
                                    <asp:Button ID="btnAtras3" runat="server" Text="Realizar otra busqueda" OnClick="btnAtras2_Click" />
                                </td>
                            </tr>
                        </table>
                    </h1>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
