<%@ Page Language="C#" Title="Fondo Emprender - Administrar Usuario" MasterPageFile="~/Master.master"
    AutoEventWireup="true" CodeBehind="AdministrarUsuarios.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.AdministrarUsuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <script type="text/javascript">
        function alerta() {
            return confirm('Está seguro que desea borrar el usuario seleccionado?');
        }

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
    </script>
    <asp:LinqDataSource ID="lds_Administradores" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        AutoPage="false" OnSelecting="lds_Administradores_Selecting">
    </asp:LinqDataSource>
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <h1>
        <asp:Label runat="server" ID="lbl_Titulo" /></h1>
    <asp:Panel ID="pnl_Administradores" runat="server">
        <asp:HyperLink ID="AgregarUsuario" NavigateUrl="~/FONADE/AdministrarPerfiles/AdministrarUsuarios.aspx?Accion=Crear"
            CssClass="addicionar" runat="server">
        </asp:HyperLink>
        <asp:GridView ID="gv_administradores" runat="server" Width="100%" AutoGenerateColumns="False"
            DataKeyNames="" CssClass="Grilla" AllowPaging="false" DataSourceID="lds_Administradores"
            AllowSorting="True" OnRowDataBound="gv_administradores_RowDataBound"
            OnDataBound="gv_administradores_DataBound"
            OnRowCreated="gv_administradores_RowCreated"
            OnPageIndexChanging="gv_administradores_PageIndexChanged"
            OnRowCommand="gv_administradores_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btn_Inactivar" CommandArgument='<%# Bind("Id_contacto")%>' runat="server"
                            ImageUrl="/Images/icoBorrar.gif" Visible="true" CausesValidation="false" CommandName="eliminar"
                            OnClientClick="return alerta();" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Nombres" SortExpression="Nombres">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_nombre" runat="server" 
                            NavigateUrl='<%# "AdministrarUsuarios.aspx?Accion=Editar&CodContacto="
                                + Eval("Id_contacto") 
                                + "&codGrupo=" + Request.QueryString["codGrupo"].ToString()
                                %>'
                            Text='<%# Eval("Nombres")%>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Email" SortExpression="Email">
                    <ItemTemplate>
                        <asp:HyperLink ID="hl_Email" runat="server" NavigateUrl='<%# "mailto:{"+Eval("Email")+"}"  %> '
                            Text='<%# Eval("Email") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="nomgrupo" HeaderText="Perfil" SortExpression="nomgrupo" />
                <asp:BoundField DataField="operador" HeaderText="Operador" SortExpression="operador" />
                <asp:TemplateField HeaderText="Regional">
                    <ItemTemplate>
                        <asp:Label ID="lblRegional" runat="server" Text='<%# NombreRegional(Eval("regional").ToString()) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel ID="pnl_crearEditar" runat="server">
        <asp:Table ID="tbl_infousuario" runat="server">
            <asp:TableRow>
                <asp:TableCell>Nombres:</asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="tb_nombres" runat="server" />
                    <br />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator1"
                        runat="server" ControlToValidate="tb_nombres" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Apellidos:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_apelidos" runat="server" />
                    <br />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator2"
                        runat="server" ControlToValidate="tb_apelidos" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Identificación:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddl_identificacion" runat="server">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator3"
                        runat="server" ControlToValidate="ddl_identificacion" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell><asp:TableCell>No:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_no" runat="server" MaxLength="15" onkeyup="SoloNumeros(this)" onchange="SoloNumeros(this)" />
                    <br />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator4"
                        runat="server" ControlToValidate="tb_no" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Cargo:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_cargo" runat="server" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Email:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_email" runat="server">
                    </asp:TextBox>
                    <br />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1fax" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ControlToValidate="tb_email" runat="server" ForeColor="Red" Display="Dynamic"
                        ErrorMessage="*Ingrese una dirección de correo valida"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator9"
                        runat="server" ControlToValidate="tb_email" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell><asp:TableCell ID="cellblRegional" runat="server" Visible="false">Regional:</asp:TableCell><asp:TableCell ID="cellddlRegional" runat="server" Visible="false">
                    <asp:DropDownList ID="ddlRegionales" runat="server" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Teléfono:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_telefono" runat="server" />
                </asp:TableCell><asp:TableCell>fax:</asp:TableCell><asp:TableCell>
                    <asp:TextBox ID="tb_fax" runat="server" />
                </asp:TableCell></asp:TableRow><asp:TableRow>
                <asp:TableCell>Perfil:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddl_perfil" runat="server">
                    </asp:DropDownList>
                    <br />
                    <asp:RequiredFieldValidator CssClass="ErrorValidacion" ForeColor="Red" ID="RequiredFieldValidator8"
                        runat="server" ControlToValidate="ddl_perfil" Display="Dynamic" ErrorMessage="* Este campo no puede estar en blanco"></asp:RequiredFieldValidator>
                </asp:TableCell></asp:TableRow><asp:TableRow ID="rowEstado" runat="server" Visible="false">
                <asp:TableCell> Estado Lider:</asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddlEstadoLider" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label ID="lblOperador" runat="server" Text="Operador:"></asp:Label>
                    

                </asp:TableCell><asp:TableCell>
                    <asp:DropDownList ID="ddlOperador" runat="server">
                    </asp:DropDownList>                   
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
    <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server" />
</asp:Content>
