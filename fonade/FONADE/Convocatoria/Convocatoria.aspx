<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Convocatoria.aspx.cs" 
    Inherits="Fonade.FONADE.Convocatoria.Convocatoria" MasterPageFile="~/Master.Master" 
    ValidateRequest="true" Async="true" EnableSessionState="True"
  Title="Crear Convocatoria"   %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">   
    <asp:LinqDataSource ID="lds_regla" runat="server"
        ContextTypeName="Datos.FonadeDBDataContext"
        OnSelecting="lds_regla_Selecting" EnableDelete="True" EnableObjectTracking="False" EntityTypeName="" TableName="ConvocatoriaReglaSalarios">
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <table width="98%">
        <tr>
            <td>
                <h1>
                    <asp:Label runat="server" ID="lbl_Titulo" Style="font-weight: 700"></asp:Label></h1>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="false" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="98%">
                <tr>
                    <td class="style24"></td>
                    <td class="style25" valign="middle">Apertura Número:</td>
                    <td class="style56" valign="middle">
                        <asp:Label ID="l_numeroapertura" runat="server" Text="" Style="color: #FF3300"></asp:Label>
                        &nbsp;</td>
                    <td class="style27"></td>
                </tr>

            </table>

        </ContentTemplate>
    </asp:UpdatePanel>
        
    <asp:UpdatePanel ID="PanelInsert" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="98%">
                <tr>
                    <td class="style24"></td>
                    <td class="style25" valign="middle">Nombre:</td>
                    <td class="style56" valign="middle">
                        <asp:TextBox ID="txt_nombre" runat="server" Width="99%"></asp:TextBox>
                    </td>
                    <td class="style27">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                            ControlToValidate="txt_nombre" Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red">* Este campo es requerido</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style31"></td>
                    <td class="style32" valign="top">Descripción:</td>
                    <td class="style33">
                        <asp:TextBox ID="txt_descripcion" runat="server" Height="120px" TextMode="MultiLine"
                            Width="99%" MaxLength="500"></asp:TextBox>
                    </td>
                    <td class="style34">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                            ControlToValidate="txt_descripcion" Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red">* Este campo es requerido</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style35"></td>
                    <td class="style36">Fecha de Inicio:</td>
                    <td class="style37">





                        <asp:TextBox ID="txt_frechaInicio" runat="server" BackColor="White" />
                        &nbsp;
                    <asp:Image ID="btnDate1" runat="server" AlternateText="cal1"
                        ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px"
                        Width="20px" />
                        <asp:CalendarExtender ID="CalendarfechaI" runat="server" Format="dd/MM/yyyy"
                            OnClientDateSelectionChanged="fechaIniMenor"
                            CssClass="ajax__calendar"
                            PopupButtonID="btnDate1" TargetControlID="txt_frechaInicio" />

                    </td>
                    <td class="style38">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                            ControlToValidate="txt_frechaInicio" Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red">* Este campo es requerido</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="style39"></td>                    
                    <td class="style40">Fecha de Finalización:</td>
                    <td class="style41">

                        <asp:TextBox ID="txt_fechaFin" runat="server" BackColor="White" />&nbsp; 
                        <asp:Image ID="btnDate2" runat="server" AlternateText="cal1"
                            ImageAlign="AbsBottom" ImageUrl="~/Images/calendar.png" Height="21px"
                            Width="20px" />
                        <asp:CalendarExtender ID="CalendarFechaF" runat="server" Format="dd/MM/yyyy"
                            OnClientDateSelectionChanged="fechaFinMayor"
                            CssClass="ajax__calendar"
                            PopupButtonID="btnDate2"
                            TargetControlID="txt_fechaFin" />
                    </td>
                    <td class="style42">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                            ControlToValidate="txt_fechaFin" Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red">* Este campo es requerido</asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td class="style24"></td>
                    <td class="style25">hora de Finalización:</td>
                    <td class="style56">
                        <asp:DropDownList ID="ddlhora" runat="server">
                            <asp:ListItem Value="0" Text="0"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2"></asp:ListItem>
                            <asp:ListItem Value="3" Text="3"></asp:ListItem>
                            <asp:ListItem Value="4" Text="4"></asp:ListItem>
                            <asp:ListItem Value="5" Text="5"></asp:ListItem>
                            <asp:ListItem Value="6" Text="6"></asp:ListItem>
                            <asp:ListItem Value="7" Text="7"></asp:ListItem>
                            <asp:ListItem Value="8" Text="8"></asp:ListItem>
                            <asp:ListItem Value="9" Text="9"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                            <asp:ListItem Value="13" Text="13"></asp:ListItem>
                            <asp:ListItem Value="14" Text="14"></asp:ListItem>
                            <asp:ListItem Value="15" Text="15"></asp:ListItem>
                            <asp:ListItem Value="16" Text="16"></asp:ListItem>
                            <asp:ListItem Value="17" Text="17"></asp:ListItem>
                            <asp:ListItem Value="18" Text="18"></asp:ListItem>
                            <asp:ListItem Value="19" Text="19"></asp:ListItem>
                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                            <asp:ListItem Value="21" Text="21"></asp:ListItem>
                            <asp:ListItem Value="22" Text="22"></asp:ListItem>
                            <asp:ListItem Value="23" Text="23"></asp:ListItem>
                        </asp:DropDownList>:
                        <asp:DropDownList ID="ddlminuto" runat="server">
                            <asp:ListItem Value="0" Text="00"></asp:ListItem>
                            <asp:ListItem Value="1" Text="01"></asp:ListItem>
                            <asp:ListItem Value="2" Text="02"></asp:ListItem>
                            <asp:ListItem Value="3" Text="03"></asp:ListItem>
                            <asp:ListItem Value="4" Text="04"></asp:ListItem>
                            <asp:ListItem Value="5" Text="05"></asp:ListItem>
                            <asp:ListItem Value="6" Text="06"></asp:ListItem>
                            <asp:ListItem Value="7" Text="07"></asp:ListItem>
                            <asp:ListItem Value="8" Text="08"></asp:ListItem>
                            <asp:ListItem Value="9" Text="09"></asp:ListItem>
                            <asp:ListItem Value="10" Text="10"></asp:ListItem>
                            <asp:ListItem Value="11" Text="11"></asp:ListItem>
                            <asp:ListItem Value="12" Text="12"></asp:ListItem>
                            <asp:ListItem Value="13" Text="13"></asp:ListItem>
                            <asp:ListItem Value="14" Text="14"></asp:ListItem>
                            <asp:ListItem Value="15" Text="15"></asp:ListItem>
                            <asp:ListItem Value="16" Text="16"></asp:ListItem>
                            <asp:ListItem Value="17" Text="17"></asp:ListItem>
                            <asp:ListItem Value="18" Text="18"></asp:ListItem>
                            <asp:ListItem Value="19" Text="19"></asp:ListItem>
                            <asp:ListItem Value="20" Text="20"></asp:ListItem>
                            <asp:ListItem Value="21" Text="21"></asp:ListItem>
                            <asp:ListItem Value="22" Text="22"></asp:ListItem>
                            <asp:ListItem Value="23" Text="23"></asp:ListItem>
                            <asp:ListItem Value="24" Text="24"></asp:ListItem>
                            <asp:ListItem Value="25" Text="25"></asp:ListItem>
                            <asp:ListItem Value="26" Text="26"></asp:ListItem>
                            <asp:ListItem Value="27" Text="27"></asp:ListItem>
                            <asp:ListItem Value="28" Text="28"></asp:ListItem>
                            <asp:ListItem Value="29" Text="29"></asp:ListItem>
                            <asp:ListItem Value="30" Text="30"></asp:ListItem>
                            <asp:ListItem Value="31" Text="31"></asp:ListItem>
                            <asp:ListItem Value="32" Text="32"></asp:ListItem>
                            <asp:ListItem Value="33" Text="33"></asp:ListItem>
                            <asp:ListItem Value="34" Text="34"></asp:ListItem>
                            <asp:ListItem Value="35" Text="35"></asp:ListItem>
                            <asp:ListItem Value="36" Text="36"></asp:ListItem>
                            <asp:ListItem Value="37" Text="37"></asp:ListItem>
                            <asp:ListItem Value="38" Text="38"></asp:ListItem>
                            <asp:ListItem Value="39" Text="39"></asp:ListItem>
                            <asp:ListItem Value="40" Text="40"></asp:ListItem>
                            <asp:ListItem Value="41" Text="41"></asp:ListItem>
                            <asp:ListItem Value="42" Text="42"></asp:ListItem>
                            <asp:ListItem Value="43" Text="43"></asp:ListItem>
                            <asp:ListItem Value="44" Text="44"></asp:ListItem>
                            <asp:ListItem Value="45" Text="45"></asp:ListItem>
                            <asp:ListItem Value="46" Text="46"></asp:ListItem>
                            <asp:ListItem Value="47" Text="47"></asp:ListItem>
                            <asp:ListItem Value="48" Text="48"></asp:ListItem>
                            <asp:ListItem Value="49" Text="49"></asp:ListItem>
                            <asp:ListItem Value="50" Text="50"></asp:ListItem>
                            <asp:ListItem Value="51" Text="51"></asp:ListItem>
                            <asp:ListItem Value="52" Text="52"></asp:ListItem>
                            <asp:ListItem Value="53" Text="53"></asp:ListItem>
                            <asp:ListItem Value="54" Text="54"></asp:ListItem>
                            <asp:ListItem Value="55" Text="55"></asp:ListItem>
                            <asp:ListItem Value="56" Text="56"></asp:ListItem>
                            <asp:ListItem Value="57" Text="57"></asp:ListItem>
                            <asp:ListItem Value="58" Text="58"></asp:ListItem>
                            <asp:ListItem Value="59" Text="59"></asp:ListItem>
                        </asp:DropDownList>



                    </td>
                    <td class="style27"></td>
                </tr>
                <tr>
                    <td class="style24"></td>
                    <td class="style25">Presupuesto:</td>
                    <td class="style56">
                        <asp:TextBox ID="txt_Presupuesto" runat="server" TextMode="Number"></asp:TextBox></td>
                    <td class="style27">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                            ControlToValidate="txt_Presupuesto" Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator" ForeColor="Red">* Este campo es requerido
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3568"
                                runat="server" ControlToValidate="txt_Presupuesto" Display="Dynamic"
                                ErrorMessage="* Este campo es numérico." ForeColor="Red"
                                ValidationExpression="^[0-9]*">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style24"></td>
                    <td class="style25">Tope de convocatoria % (establezca 0 para tope sin limites):</td>
                    <td class="style56">
                        <asp:TextBox ID="txtTopeConvocatoria" runat="server" TextMode="Number"></asp:TextBox></td>
                    <td class="style27">
                        <asp:RequiredFieldValidator 
                                ID="RequiredFieldValidator7" 
                                runat="server"
                                ControlToValidate="txtTopeConvocatoria" 
                                Display="Dynamic"
                                ErrorMessage="RequiredFieldValidator" 
                                ForeColor="Red">* Este campo es requerido
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator 
                                ID="RegularExpressionValidator1"
                                runat="server" 
                                ControlToValidate="txtTopeConvocatoria" 
                                Display="Dynamic"
                                ErrorMessage="* Este campo es numérico." 
                                ForeColor="Red"
                                ValidationExpression="^[0-9]*">
                        </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style19">&nbsp;</td>
                    <td class="style29">Valor mínimo para aprobar plan de negocio:</td>
                    <td class="style55">
                        <asp:TextBox ID="txt_valorminimo" runat="server" TextMode="Number"></asp:TextBox>

                    </td>
                    <td>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                            ControlToValidate="txt_valorminimo"
                            Display="Dynamic"
                            ErrorMessage="RequiredFieldValidator"
                            ForeColor="Red">* Este campo es requerido
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3569"
                            runat="server" ControlToValidate="txt_valorminimo" Display="Dynamic"
                            ErrorMessage="* Este campo es numérico." ForeColor="Red"
                            ValidationExpression="^[0-9]*"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style43"></td>
                    <td class="style44">Encargo Fiduciario:</td>
                    <td class="style45">
                        <asp:TextBox ID="txt_encargo" runat="server"></asp:TextBox></td>
                    <td class="style46"></td>
                </tr>
                 <tr>
                    <td class="style51"></td>
                    <td class="style52" valign="baseline">Operador:</td>
                    <td class="style53" valign="baseline">
                        <asp:DropDownList ID="ddlOperador" runat="server" OnSelectedIndexChanged="ddlOperador_SelectedIndexChanged" AutoPostBack="True">
                        </asp:DropDownList>
                    </td>
                    <td class="style54"></td>
                </tr>
                <tr>
                    <td class="style51"></td>
                    <td class="style52" valign="baseline">Convenio:</td>
                    <td class="style53" valign="baseline">
                        <asp:DropDownList ID="ddl_convenios" runat="server" Height="19px" Width="106px">
                        </asp:DropDownList>
                    </td>
                    <td class="style54"></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
   

    <asp:UpdatePanel ID="PanelUpdate" runat="server" Visible="false" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="98%">
                <tr>
                    <td class="style24"></td>
                    <td class="style25" valign="baseline">Publicar Convocatoria:</td><td class="style56" valign="baseline">
                        <asp:CheckBox ID="Ch_publicado" runat="server" />
                    </td>
                    <td class="style27"></td>
                </tr>
                <tr>
                    <td class="style24"></td>
                    <td colspan="2"></td>
                    <td class="style27"></td>
                </tr>
                <tr>
                    <td class="style24">
                        Actas </td><td class="style24">
                    </td>
                    <td colspan="1">
                        <asp:ImageButton ID="ImageButton2" ImageUrl="~/Images/icoClip.gif" 
                        runat="server" ToolTip="Nuevo Documento" OnClick="ImageButton2_Click" OnClientClick="form1.target ='_blank';"  />
                        <asp:ImageButton ID="ImageButton1" ImageUrl="~/Images/icoClick2.gif" 
                        runat="server" ToolTip="Lista documentos" OnClientClick="form1.target ='_blank';" OnClick="ImageButton1_Click"  />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="panelBotonesCrear" runat="server" Visible="false" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="98%">
                <tr>
                    <td class="style19">&nbsp;</td><td colspan="2" align="center" class="style57">
                        <asp:Button ID="btn_CrearConv" runat="server" Text="Crear Convocatoria"
                            OnClick="btn_CrearConv_Click" />
                    </td>
                    <td>&nbsp;</td></tr></table></ContentTemplate></asp:UpdatePanel>
    <asp:UpdatePanel ID="PanelBotonesactualizar" runat="server" Width="100%" UpdateMode="Conditional" Visible="false">
        <ContentTemplate>
            <table width="98%">
                <tr>
                    <td class="style19">&nbsp;</td><td colspan="2" align="center">
                        <table width="100%">
                            <tr>
                                <td class="auto-style2">
                                    <asp:Button ID="btn_Criterios" runat="server" Text="Criterios de Priorización"
                                        OnClick="btn_Criterios_Click" CausesValidation="False" />
                                </td>
                                <td class="auto-style3">
                                    <asp:Button ID="btn_Proyectos" runat="server"
                                        OnClick="btn_Proyectos_Click" Text="Ver Proyectos"
                                        CausesValidation="False" />
                                </td>
                                <td class="auto-style3">
                                    <asp:Button ID="btn_actualizar" runat="server" OnClick="btn_actualizar_Click"
                                        Text="Actualizar Convocatoria" CausesValidation="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td></tr></table><table width="98%">
                <tr class="style61">
                    <td class="style65"></td>
                    <td class="style62" style="background-color: #00468F; text-align: center;">Regla de Salarios.</td><td class="style66"></td>
                </tr>
                <tr>
                    <td class="style19">&nbsp;</td><td align="center" class="style60">
                        <asp:GridView ID="gvreglasalarios" CssClass="Grilla" runat="server" DataSourceID="lds_regla" AutoGenerateColumns="False" Width="100%"
                            EmptyDataText="Aún no hay concidiones para esta regla, haca clic en 'Crear Condición'." OnRowCommand="gvreglasalarios_RowCommand" ClientIDMode="Static" DataKeyNames="NoRegla" EnableModelValidation="False" EnablePersistedSelection="True" OnRowDeleting="gvreglasalarios_RowDeleting">
                            <Columns>
                                <asp:CommandField ButtonType="Image" DeleteImageUrl="~/Images/icoBorrar.gif" DeleteText="" ShowDeleteButton="True" />
                                
                                <asp:TemplateField HeaderText="Condición">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hiddenNumero" runat="server" Value='<%# Eval("NoRegla") %>' />
                                        <asp:Label ID="l_condicion" runat="server" Text='<%# Eval("condicion") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/add.png" ShowSelectButton="True" HeaderText="Modificar" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>&nbsp;</td></tr><tr>
                    <td class="style19">&nbsp;</td><td class="style60" valign="baseline">
                        <asp:ImageButton ID="ImageButton5" runat="server" CommandArgument="0" 
                            ImageUrl="~/Images/add.png" OnClick="ImageButton5_Click" 
                            OnClientClick="uhb(0,0)" />
                        <asp:ModalPopupExtender ID="ImageButton5_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" BehaviorID="lbtn_adicionarRegla2" CancelControlID="ImageButton4" ClientIDMode="Static" DynamicServicePath="" Enabled="True" OkControlID="ImageButton4" PopupControlID="ConvocatoriaReglaSalariosPanel" TargetControlID="ImageButton5" >
                        </asp:ModalPopupExtender>
                        <asp:Label ID="Label1" runat="server" Text="Crear Regla"></asp:Label></td><td>&nbsp;</td></tr></table></ContentTemplate><Triggers>
            <asp:AsyncPostBackTrigger ControlID="gvreglasalarios" EventName="RowCommand" />
            <asp:AsyncPostBackTrigger ControlID="gvreglasalarios" EventName="DataBound" />
            <asp:AsyncPostBackTrigger ControlID="gvreglasalarios" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:HiddenField ID="IdConvocatoriaHiddenField" ClientIDMode="Static" runat="server" />
    <br />
    <br />
    <asp:Panel ID="pnlcriteriosselecion" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <h1>
                        <label>CRITERIOS DE SELECCIÓN</label> </h1></td></tr><tr>
                <td>
                    <asp:LinkButton ID="lnkcriteriosseleccion" runat="server" Text="Adicionar Criterio" OnClick="lnkcriteriosseleccion_Click" CausesValidation="false"></asp:LinkButton></td></tr><tr>
                <td>
                    <asp:LinqDataSource ID="lds_criterioseleccion" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_criterioseleccion_Selecting"></asp:LinqDataSource>
                    <asp:GridView ID="gvr_criteriosseleccion" runat="server" AutoGenerateColumns="false" Width="100%" 
                        DataSourceID="lds_criterioseleccion" OnRowCreated="gvr_criteriosseleccion_RowCreated" 
                        DataKeyNames="Id_Criterio" ShowHeader="false" OnRowCommand="gvr_criteriosseleccion_RowCommand">

                        <Columns>
                            <asp:TemplateField ShowHeader="false">
                                <ItemTemplate>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td style="width:15px;">
                                                <asp:LinkButton ID="lnkeliminar" runat="server" CommandArgument='<%# Eval("Id_Criterio") %>' CommandName="Borrar">
                                                    <asp:Image ID="img_borrar" runat="server" ImageUrl="~/Images/icoBorrar.gif" />
                                                </asp:LinkButton></td><td>
                                                <asp:LinkButton ID="lnkeditar" runat="server" Text='<%# Eval("NomCriterio") %>' CommandName="Editar" CommandArgument='<%# Eval("Id_Criterio") %>'></asp:LinkButton></td></tr><tr>
                                            <td colspan="2">
                                                <table style="width:100%;">
                                                    <tr>
                                                        <td style="width:100%;float:left;">
                                                            <asp:GridView ID="gvr_ambitos" runat="server" AutoGenerateColumns="false" CssClass="Grilla" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Ámbito Geográfico" DataField="Ciudad" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                        <td style="width:50%;">
                                                            <asp:GridView ID="gvr_ambitos1" runat="server" AutoGenerateColumns="false" CssClass="Grilla" Width="100%">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Ámbito Económico" DataField="nombreSector" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
    <br />
    <asp:Panel ID="pnl_confirmacion" runat="server">
        <table style="width: 100%;">
            <tr>
                <td>
                    <h1>
                        <label>COFINANCIACIÓN</label> </h1></td></tr><tr>
                <td>
                    <asp:LinkButton ID="lnk_adicionarconfirmacion" runat="server" Text="Adicionar Cofinanciación" OnClick="lnk_adicionarconfirmacion_Click" CausesValidation="false"></asp:LinkButton></td></tr><tr>
                <td>
                    <asp:LinqDataSource ID="ldsconfirmacion" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsconfirmacion_Selecting"></asp:LinqDataSource>
                    <asp:GridView ID="gvr_confirmacion" runat="server" CssClass="Grilla" AutoGenerateColumns="false" AllowSorting="true" DataSourceID="ldsconfirmacion" Width="100%" OnRowCommand="gvr_confirmacion_RowCommand">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkeliminar" runat="server" CommandArgument='<%# Eval("CodCiudad") %>' CommandName="Borrar">
                                        <asp:Image ID="img_borrar" runat="server" ImageUrl="~/Images/icoBorrar.gif" />
                                    </asp:LinkButton></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Ciudad" SortExpression="nomCiudad">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkeditar" runat="server" Text='<%# Eval("nomCiudad") %>' CommandName="Editar" CommandArgument='<%# Eval("CodCiudad") %>'></asp:LinkButton></ItemTemplate></asp:TemplateField><asp:BoundField HeaderText="Cofinanciacion" DataField="Cofinanciacion" SortExpression="Cofinanciacion" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </asp:Panel>

<asp:Panel ID="ConvocatoriaReglaSalariosPanel" runat="server" Height="100%" Width="70%" BackColor="White">
    <asp:HiddenField ID="condicionReglaHiddenField" runat="server" ClientIDMode="Static" />
    <asp:ImageButton ID="ImageButton4" runat="server" ImageUrl="~/Images/icoBorrar.gif" OnClientClick="rfv()" />    
    <object type="text/html" id="htmlContainer" data="??" style="width: 100%; height: 70%" onchange="__doPostback('ImageButton4','')" itemid="htmlContainer"></object></asp:Panel></asp:Content><asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">        
        #bodyContentPlace_ListActasPanel{
            left:0px !important;
        }
        #bodyContentPlace_UpdatePanel1{
            width:90%;
            position:none;
            margin:0 auto;
        }
        .style19 {
            width: 10px;
        }

        .style24 {
            width: 10px;
            height: 27px;
        }

        .style25 {
            width: 159px;
            font-weight: bold;
            height: 27px;
        }

        .style27 {
            height: 27px;
        }

        .style29 {
            width: 159px;
            font-weight: bold;
        }

        .style31 {
            width: 10px;
            height: 126px;
        }

        .style32 {
            width: 159px;
            font-weight: bold;
            height: 126px;
        }

        .style33 {
            width: 386px;
            height: 126px;
        }

        .style34 {
            height: 126px;
        }

        .style35 {
            width: 10px;
            height: 32px;
        }

        .style36 {
            width: 159px;
            font-weight: bold;
            height: 32px;
        }

        .style37 {
            width: 386px;
            height: 32px;
        }

        .style38 {
            height: 32px;
        }

        .style39 {
            width: 10px;
            height: 31px;
        }

        .style40 {
            width: 159px;
            font-weight: bold;
            height: 31px;
        }

        .style41 {
            width: 386px;
            height: 31px;
        }

        .style42 {
            height: 31px;
        }

        .style43 {
            width: 10px;
            height: 26px;
        }

        .style44 {
            width: 159px;
            font-weight: bold;
            height: 26px;
        }

        .style45 {
            width: 386px;
            height: 26px;
        }

        .style46 {
            height: 26px;
        }

        .style51 {
            width: 10px;
            height: 29px;
        }

        .style52 {
            width: 159px;
            font-weight: bold;
            height: 29px;
        }

        .style53 {
            width: 386px;
            height: 29px;
        }

        .style54 {
            height: 29px;
        }

        .style55 {
            width: 386px;
        }

        .style56 {
            width: 386px;
            height: 27px;
        }

        .style57 {
            width: 29px;
        }

        .style58 {
            width: 223px;
        }

        .style60 {
            width: 600px;
            font-weight: 700;
        }

        .style61 {
            font-size: 13px;
            color: #FFFFFF;
        }

        .style62 {
            width: 100%;
            font-weight: 700;
            color: #F4F4F4;
            height: 24px;
        }

        .style65 {
            width: 10px;
            height: 24px;
        }

        .style66 {
            height: 24px;
        }

        .style67 {
            width: 10px;
            height: 25px;
        }

        .style68 {
            height: 25px;
        }
        .auto-style2 {
            width: 223px;
            height: 50px;
        }
        .auto-style3 {
            height: 50px;
        } 
        #bodyContentPlace_GridView1 tr:nth-child(2) td:nth-child(1){
            width:4% !important;
        }
        #bodyContentPlace_GridView1 tr:nth-child(2) td:nth-child(2){
            width:18% !important;
        }
        #bodyContentPlace_GridView1 tr:nth-child(2) td:nth-child(n+3){
            width:10% !important;
        }
        #bodyContentPlace_UpdatePanel1 td:nth-child(7) textarea{
            width:90% !important;
            height:70% !important;
        }         
         
       
   
    </style>
    <script type="text/javascript">
        var ujh = function (val){
            if (val == '') return;
            window.open(window.decodeURI(val));
        }
        var uhb = function (g){
            var e = null;
            var esz = document.getElementById('IdConvocatoriaHiddenField');
            e = esz != null ? esz.value : "0";
            var wdc = $get('htmlContainer');
            if (wdc != null){
                wdc.data = 'ConvocatoriaReglaSalarios.aspx?CodConvocatoria='+e.toString()+'&NoRegla='+g;
            }
            if($find('lbtn_adicionarRegla')!=null)$find('lbtn_adicionarRegla').show();
        }
        var rfv = function (){
            var plk = $find('lbtn_adicionarRegla');
            if (plk != null) plk.hide();
            location.reload();
        }
    </script>

        <script type="text/javascript">

            function fechaIniMenor(sender, args) {
                var startDate = document.getElementById("<%=txt_frechaInicio.ClientID%>").value.split('/');
                var endDate = document.getElementById("<%=txt_fechaFin.ClientID%>").value.split('/');

                var FechaInicial = new Date(startDate[2] + '-' + startDate[0] + '-' + startDate[1]);
                var FechaFin = new Date(endDate[2] + '-' + endDate[0] + '-' + endDate[1]);

                if (FechaInicial > FechaFin) {
                    alert("La fecha inicial no puede ser mayor que la final.");
                    document.getElementById("<%=txt_frechaInicio.ClientID%>").value = document.getElementById("<%=txt_fechaFin.ClientID%>").value;
                    return false;
                }
            }


            function fechaFinMayor(sender, args) {
                var startDate = document.getElementById("<%=txt_frechaInicio.ClientID%>").value.split('/');
                var endDate = document.getElementById("<%=txt_fechaFin.ClientID%>").value.split('/');
                var FechaInicial = new Date(startDate[2] + '-' + startDate[0] + '-' + startDate[1]);
                var FechaFin = new Date(endDate[2] + '-' + endDate[0] + '-' + endDate[1]);

                if (FechaInicial > FechaFin) {
                    alert("La fecha Final no puede ser menor que la Inicial.");
                    document.getElementById("<%=txt_fechaFin.ClientID%>").value = document.getElementById("<%=txt_frechaInicio.ClientID%>").value;
                    return false;
                }
            }
        </script>

</asp:Content>