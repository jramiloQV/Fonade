<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProyectoEmpresa.aspx.cs"
    Inherits="Fonade.FONADE.interventoria.ProyectoEmpresa" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
    <link href="../../Styles/jquery-ui-1.10.3.min.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-ui-1.10.3.min.js" type="text/javascript"></script>
    <script src="../../Scripts/common.js" type="text/javascript"></script>
    <style type="text/css">
        /*table {
            width: 100%;
        }*/
        .do-not-break 
        {
            white-space:nowrap;
        }
        .gvsocios  tr  ,.Grilla tr
        {
            vertical-align:top;    
        }
        .sinlinea
        {
            border: none;
            border-collapse: collapse;
            border-bottom-color: none;
        }
        .contenedor
        {
            width: 100%;
            height: auto;
        }
        .contenedor_titulo
        {
            padding: 0px 0px 0px 66px;
        }
        .Grilla
        {
            width: 500px !important;
        }
        #ce_refecha_container, #CalendarExtender1_container, #CalendarExtender2_container, #CalendarExtender3_container, #CalendarExtender4_container, #CalendarExtender5_container{
            height: auto !important;
        }
        .ajax__calendar_header{
            height:0px !important;
        }
    </style>
    <script type="text/javascript">
        function ValidNum(e) {
            var tecla = document.all ? tecla = e.keyCode : tecla = e.which;
            return (tecla > 47 && tecla < 58);
        }
    </script>
</head>
<body style="overflow-x: hidden;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="tsm_formulario" runat="server">
        
 </asp:ToolkitScriptManager>
    <asp:LinqDataSource ID="lds_ciudad" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        OnSelecting="lds_ciudad_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_tiposociedad" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        OnSelecting="lds_tiposociedad_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource ID="lds_departamento" runat="server" ContextTypeName="Datos.FonadeDBDataContext"
        OnSelecting="lds_departamento_Selecting">
    </asp:LinqDataSource>
    <div class="contenedor">
        <div class="contenedor">
            <h2>
                Nombre del Plan de negocio:</h2>
        </div>
        <div class="contenedor">
            <br />
            <span class="contenedor_titulo">
                <asp:Label ID="lblplannegocio" runat="server" Width="400px" />
            </span>
        </div>
        <div class="contenedor">
            <h2>
                Razón Social:</h2>
        </div>
        <div class="contenedor">
            <br />
            <span class="contenedor_titulo">
                <asp:TextBox ID="txtrazonsocial" runat="server" Width="400px" MaxLength="250" ValidationGroup="guardar" />
            </span>
            <br />
            <span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtrazonsocial"
                    ErrorMessage="Debe digitar la razón social" ForeColor="Red" ValidationGroup="guardar" />
            </span>
        </div>
        <div class="contenedor">
            <h2>
                Socios:</h2>
        </div>
        <div class="contenedor" style="overflow-x: visible; overflow-y: hidden; width: 100% !important;
            height: auto;">
            <asp:GridView ID="gvsocios" runat="server" AutoGenerateColumns="false" CssClass="Grilla"
                DataKeyNames="codContacto"  HeaderStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:BoundField HeaderText="Nombre del Emprendedor" DataField="Nombre" ItemStyle-CssClass="do-not-break" />
                    <asp:BoundField HeaderText="Número de Cédula" DataField="Identificacion" />
                    <asp:TemplateField HeaderText="Representante Legal" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:RadioButton ID="rb_representante" runat="server" Checked='<%# Eval("RepresentanteLegal")  %>' GroupName="Representante" ValidationGroup="guardar" />
                            <asp:HiddenField ID="hdf_rb_codigocontacto" runat="server" Value='<%# Eval("codContacto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Suplente" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:RadioButton ID="rb_suplente" runat="server" Checked='<%# Eval("Suplente") %>' GroupName="Suplente" ValidationGroup="guardar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dirección">
                        <ItemTemplate>
                            <asp:TextBox ID="txtdireccion" runat="server" Text='<%# Eval("Direccion") %>'
                                ValidationGroup="guardar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Correo Electrónico" DataField="Email" />
                    <asp:TemplateField HeaderText="Participación Accionaria %">
                        <ItemTemplate>
                            <asp:TextBox ID="txtParticipacion" runat="server" Text='<%# Eval("participacion") %>' MaxLength="15" ValidationGroup="guardar"
                                Width="80" />
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtParticipacion"
                                ErrorMessage="Debe llenar la participación por cada emprendedor" ForeColor="Red"
                                ValidationGroup="guardar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Teléfono" ItemStyle-CssClass="do-not-break">
                        <ItemTemplate>
                            <asp:Label ID="lbl_telefono_registro" Text='<%# Eval("Telefono") %>' runat="server" />
                            <asp:HiddenField ID="hdf_codigocontacto" runat="server" Value='<%# Eval("CodContacto") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ciudad">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlciudad" runat="server" DataTextField="NomCiudad" Enabled="false"
                                DataValueField="Id_Ciudad" DataSourceID="lds_ciudad" SelectedValue='<%# Eval("CodCiudad") %>'
                                ValidationGroup="guardar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <table class="tabla_padre">
        <tr>
            <td>
                <h2>
                    Datos de la Empresa:</h2>
                <br />
                <table>
                    <tr>
                        <td rowspan="2">
                            <h3>
                                RUT:                               
                                                       
                            <asp:Label ID="lblAvisoCargaArchivo" runat="server" style="color: #00468f" Text="Ya cargó un archivo para el RUT" Visible="False"></asp:Label>
                           
                            </h3>
                            
                        </td>
                        <td>
                           
                            <asp:FileUpload ID="fu_archivo" runat="server" Width="399px" />

                        </td>
                        <td rowspan="2">
                           
                            <asp:Label ID="lblMensajeRut" runat="server" style="color: #FF0000" Text="*Cargar el RUT es obligatorio" Visible="False"></asp:Label>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                           
                             <asp:Button ID="btn_Cargar"  runat="server" 
                                  Text="Cargar Archivo" Height="29px" Width="111px" OnClick="btn_Cargar_Click" />

                             <asp:Button ID="btnVerArchivo"  runat="server" 
                                  Text="Descargar Archivo" Height="29px" Width="156px" OnClick="btnVerArchivo_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Objeto Social:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtobjetosocial" runat="server" MaxLength="1000" Width="400px" ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtobjetosocial"
                                ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="guardar">Debe digitar el objeto social</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Capital Social:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcapitalsocial" runat="server" Width="400px" ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtcapitalsocial"
                                ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="guardar">Debe digitar el capital social</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Tipo de Sociedad:</h3>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddltiposociedad" runat="server"  Width="400px"
                                ValidationGroup="guardar">
                                <%--<asp:ListItem Value="-1">Seleccione el tipo de sociedad</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddltiposociedad"
                                ErrorMessage="Debe seleccionar el tipo de sociedad" ForeColor="Red" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                               Código CIIU:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCodigoCIIU" runat="server" Width="400px"  MaxLength="4"
                                ValidationGroup="guardar" />
                        </td>
                        <td>
                             <asp:Label ID="lblCodigoCIIU" runat="server" style="color: #FF0000" Text="*Código CIIU es obligatorio" Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Numero de Escritura Pública:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtescriturapublica" runat="server" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Domicilio de la Empresa:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdomicilioempresa" runat="server" Width="400px" MaxLength="250"
                                ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtdomicilioempresa"
                                ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="guardar">Debe digitar el domicilio de la empresa</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Departamento:</h3>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddldepartamneto" runat="server" DataValueField="Id_Departamento"
                                DataTextField="NomDepartamento" DataSourceID="lds_departamento" Width="400px"
                                ValidationGroup="guardar" OnChange="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(this.name, '', true, '', '', false, true))"
                                OnSelectedIndexChanged="ddldepartamneto_SelectedIndexChanged">
                                <asp:ListItem Value="-1">Seleccione Departamento</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Ciudad:</h3>
                        </td>
                        <td>
                            <asp:UpdatePanel ID="panelDropDowList" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlciudades" runat="server" DataValueField="Id_Ciudad" DataTextField="NomCiudad"
                                        Width="400px" ValidationGroup="guardar">
                                        <asp:ListItem Value="-1">Seleccione Ciudad</asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddldepartamneto" EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Teléfono:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txttelefono" runat="server" Width="400px" ValidationGroup="guardar" MaxLength="10" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txttelefono"
                                ErrorMessage="Debe digitar el teléfono" ForeColor="Red" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Correo Electrónico:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcorreo" runat="server" Width="400px" ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtcorreo"
                                ErrorMessage="Debe digitar el correo" ForeColor="Red" ValidationGroup="guardar" />
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtcorreo"
                                ErrorMessage="Correo no válido" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ValidationGroup="guardar" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <h2>
                    Información Tributaria:</h2>
                <br />
                <table>
                    <tr>
                        <td>
                            <h3>
                                Nit:</h3>
                        </td>
                        <td>
                            <asp:Label ID="lblnitverificacion" runat="server" Text="Omita dígito de verificación, guiones y puntos"
                                ForeColor="#ff0000" />
                            <br />
                            <asp:TextBox ID="txtnit" runat="server" Width="400px" type="number" ValidationGroup="guardar" MaxLength="30" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Régimen Especial? (S/N):</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblregimenespecial" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtrenorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtrenormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image1" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="ce_refecha" runat="server" TargetControlID="txtrenormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image1" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Contribuyente? (S/N):</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_contribuyente" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcenorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtcenormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image2" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtcenormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Autoretenedor? (S/N):</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_Autoretenedor" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtarnorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtarnormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image3" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtarnormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image3" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Declarante? (S/N):</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_Declarante" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdnorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtdnormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image4" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtdnormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image4" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Exento de Retención en la Fuente? (S/N):</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_Retencion" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txterfnorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txterfnormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image5" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txterfnormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image5" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Tipo Régimen:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txttiporegimen" runat="server" MaxLength="50" Width="400px" Text="C"
                                Enabled="false" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Es Gran Contribuyente? (S/N)</h3>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_GranContribuyente" runat="server" ValidationGroup="guardar">
                                <asp:ListItem Value="1">SI</asp:ListItem>
                                <asp:ListItem Value="0">NO</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtgcnorma" runat="server" MaxLength="50" Width="400px" ValidationGroup="guardar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <h3>
                                Fecha de Norma:</h3>
                        </td>
                        <td>
                            <asp:TextBox ID="txtgcnormafecha" runat="server" Width="400px" ValidationGroup="guardar"
                                Enabled="False" />
                            <asp:Image runat="server" ID="Image6" AlternateText="cal2" ImageUrl="~/images/icomodificar.gif" />
                            <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtgcnormafecha"
                                Format="dd/MM/yyyy" PopupButtonID="Image6" />
                        </td>
                    </tr>
                     
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Button ID="btnguardar" runat="server" Text="Guardar" OnClick="btnguardar_Click"
                    Visible="false" Enabled="false" ValidationGroup="guardar" />
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    </form>
</body>
</html>
