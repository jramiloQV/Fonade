<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CrearEmprendedor.aspx.cs" Inherits="Fonade.FONADE.PlandeNegocio.CrearEmprendedor" Culture="es-CO" UICulture="es-CO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 1000px; margin: 0 auto; background-color: #FFFFFF;">
                <div class="form-horizontal no-margin center-block">
                    <div class="form-group center-block" style="background-color: #00468F; color: #FFFFFF">
                        <div class="col-xs-4">
                        </div>
                        <div class="col-xs-4">
                            <h3>
                                <asp:Label Text="Crear Emprendedor" runat="server" ID="lblTituloCrearEmprendedor" Visible="false" />
                                <asp:Label Text="Actualizar Emprendedor" runat="server" ID="lblTituloActualizarEmprendedor" Visible="False" />
                            </h3>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfCodigoProyecto" runat="server" Value="" Visible="false" />
                    <asp:HiddenField ID="hfCodigoEmprendedor" runat="server" Value="" Visible="false" />
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Nombres : </label>
                        <div class="col-xs-4">
                            <asp:TextBox ID="txtNombres" runat="server" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Apellidos : </label>
                        <div class="col-xs-4">
                            <asp:TextBox ID="txtApellidos" runat="server" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Identificación : </label>
                        <div class="col-xs-4">
                            <asp:DropDownList runat="server" ID="cmbTipoDocumento" nombre="Tipo documento" DataSourceID="dataTiposIdentificacion" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">No : </label>
                        <div class="col-xs-4">
                            <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="10" TextMode="Number" />
                        </div>
                    </div>
                     <div class="form-group center-block">
                        <label class="control-label col-xs-4">Digite nuevamente el número de identificación: </label>
                        <div class="col-xs-4">
                            <asp:TextBox ID="txtIdentificacionConfirm" runat="server" 
                                MaxLength="10" TextMode="Number" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Departamento expedición: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbDepartamentoExpedicion" runat="server" nombre="Departamento expedición" Width="400px" DataSourceID="dataDepartamentoExpedicion" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Ciudad expedición: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbCiudadExpedicion" nombre="Ciudad expedición" runat="server" Width="400px" DataSourceID="dataCiudadExpedicion" DataTextField="Nombre" DataValueField="Id" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Correo electrónico: </label>
                        <div class="col-xs-4">
                            <asp:TextBox ID="txtEmail" runat="server" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Género: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList runat="server" ID="cmbGenero" nombre="Género">
                                <asp:ListItem Text="Seleccione" Value="" />
                                <asp:ListItem Text="Masculino" Value="M" />
                                <asp:ListItem Text="Femenino" Value="F" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Fecha Nacimiento: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtFechaNacimiento" Enabled="false" placeholder="dd/mm/aaaa" />
                            <asp:Image runat="server" ID="Image1" AlternateText="Calendario de fecha de nacimiento" ImageUrl="~/images/icomodificar.gif" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender2" PopupButtonID="Image1" CssClass="ajax__calendar" TargetControlID="txtFechaNacimiento" Format="dd/MM/yyyy" ClearTime="True" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Departamento nacimiento: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbDepartamentoNacimiento" runat="server" nombre="Departamento nacimiento" Width="400px" DataSourceID="dataDepartamentoNacimiento" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Ciudad nacimiento: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbCiudadNacimiento" nombre="Ciudad nacimiento" runat="server" Width="400px" DataSourceID="dataCiudadNacimiento" DataTextField="Nombre" DataValueField="Id" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Teléfono: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtTelefonoFijo" Text="" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Direccion de Residencia: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtDireccionEmprendedor" Text="" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Departamento Residencia: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbDepartamentoResidencia" runat="server" nombre="Departamento Residencia" Width="400px" DataSourceID="dataDepartamentoNacimiento" AutoPostBack="true" DataValueField="Id" DataTextField="Nombre" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Ciudad Residencia: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbCiudadResidencia" nombre="Ciudad Residencia" runat="server" Width="400px" DataSourceID="dataCiudadResidencia" DataTextField="Nombre" DataValueField="Id" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Nivel de estudio : </label>
                        <div class="col-xs-4">
                            <asp:DropDownList ID="cmbNivelEstudio" runat="server" nombre="Nivel estudio" DataSourceID="dataNivelEstudio" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Programa Realizado: </label>
                        <div class="col-xs-6">
                            <asp:HiddenField ID="hfcodigoProgramaRealizado" runat="server" Value="" Visible="false" />
                            <asp:TextBox ID="txtProgramaRealizado" runat="server" Enabled="false" Width="400px" />
                            <asp:ImageButton ID="imbtn_institucion" ImageUrl="~/Images/icoComentario.gif" runat="server" OnClick="imbtn_institucion_Click" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Institución: </label>
                        <div class="col-xs-6">
                            <asp:HiddenField ID="hfCodigoInstitucionEducativa" runat="server" Value="" Visible="false" />
                            <asp:TextBox ID="txtInstitucionEducativa" runat="server" Enabled="false" Width="400px" />
                            <asp:ImageButton ID="imbtn_nivel" ImageUrl="~/Images/icoComentario.gif" runat="server" OnClick="imbtn_nivel_Click" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Ciudad Institución: </label>
                        <div class="col-xs-6">
                            <asp:HiddenField ID="hfCodigoCiudadInstitucionEducativa" runat="server" Value="" Visible="false" />
                            <asp:TextBox runat="server" ID="txtCiudadInstitucion" Enabled="false" Width="400px" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Estado: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList runat="server" ID="cmbEstadoEstudio" nombre="Estado" AutoPostBack="true" OnTextChanged="cmbEstadoEstudio_TextChanged" OnLoad="cmbEstadoEstudio_Load">
                                <asp:ListItem Text="Actualmente cursando" Value="0" />
                                <asp:ListItem Text="Finalizado" Value="1" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Fecha inicio: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtFechaInicioEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                            <asp:Image runat="server" ID="img_dateInicio" AlternateText="Calendario fecha de inicio estudio" ImageUrl="~/images/icomodificar.gif" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender1" PopupButtonID="img_dateInicio"
                                CssClass="ajax__calendar" TargetControlID="txtFechaInicioEstudio" Format="dd/MM/yyyy" ClearTime="True" />
                        </div>
                    </div>
                    <div id="formFechaFinalizacion" runat="server" visible="false" class="form-group center-block">
                        <label class="control-label col-xs-4">Fecha finalización: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtFechaFinalizacionEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                            <asp:Image runat="server" ID="Image3" AlternateText="Calendario de fecha de finalizacion de estudio" ImageUrl="~/images/icomodificar.gif" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender4" PopupButtonID="Image3"
                                CssClass="ajax__calendar" TargetControlID="txtFechaFinalizacionEstudio" Format="dd/MM/yyyy" ClearTime="True" />
                        </div>
                    </div>
                    <div id="formFechaGraduacion" runat="server" visible="false" class="form-group center-block">
                        <label class="control-label col-xs-4">Fecha graduación: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtFechaGraduacionEstudio" Enabled="false" placeholder="dd/mm/aaaa" />
                            <asp:Image runat="server" ID="Image2" AlternateText="Calendario de fecha de graduación" ImageUrl="~/images/icomodificar.gif" />
                            <ajaxToolkit:CalendarExtender runat="server" ID="Calendarextender3" PopupButtonID="Image2"
                                CssClass="ajax__calendar" TargetControlID="txtFechaGraduacionEstudio" Format="dd/MM/yyyy" DefaultView="Days" ClearTime="True" />
                        </div>
                    </div>
                    <div id="formHorasDedicadas" runat="server" visible="false" class="form-group center-block">
                        <label class="control-label col-xs-4">Semestre actual u horas dedicadas: </label>
                        <div class="col-xs-4">
                            <asp:TextBox runat="server" ID="txtHorasDedicadas" Enabled="true" Visible="true" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <label class="control-label col-xs-4">Tipo de Aprendiz: </label>
                        <div class="col-xs-4">
                            <asp:DropDownList runat="server" ID="cmbTipoAprendiz" CssClass="selectpicker" nombre="Tipo aprendiz" Width="400px" DataSourceID="dataTipoAprendiz" DataTextField="Nombre" DataValueField="Id" />
                        </div>
                    </div>
                    <div class="form-group center-block">
                        <asp:Label ID="lblErrorCrearEmprendedor" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="False" Font-Bold="False" Style="text-align: center" Width="100%" />
                    </div>
                    <div class="form-group center-block">
                        <div class="col-xs-4">
                        </div>
                        <div class="col-xs-4">
                            <asp:Button ID="btnCrearEmprendedor" runat="server" Text="Crear Emprendedor" OnClick="btnCrearEmprendedor_Click" Visible="false" />
                            <asp:Button ID="btnActualizarEmprendedor" runat="server" Text="Actualizar Emprendedor" OnClick="btnActualizarEmprendedor_Click" Visible="False" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="true" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:ObjectDataSource
                ID="dataCiudadExpedicion"
                runat="server"
                SelectMethod="getCiudades"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbDepartamentoExpedicion" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataDepartamentoExpedicion"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getDepartamentos"></asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataCiudadNacimiento"
                runat="server"
                SelectMethod="getCiudades"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbDepartamentoNacimiento" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataCiudadResidencia"
                runat="server"
                SelectMethod="getCiudades"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
                <SelectParameters>
                    <asp:ControlParameter ControlID="cmbDepartamentoResidencia" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataDepartamentoNacimiento"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getDepartamentos"></asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataTiposIdentificacion"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getTiposIdentificacion"></asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataNivelEstudio"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getNivelesEstudio"></asp:ObjectDataSource>
            <asp:ObjectDataSource
                ID="dataTipoAprendiz"
                runat="server"
                TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                SelectMethod="getTiposDeAprendiz"></asp:ObjectDataSource>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Panel ID="ModalEstudios" CssClass="modal fade" runat="server" role="dialog">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Programas academicos</h4>
                        </div>

                        <div class="modal-body">
                            <div id="buscador" runat="server" visible="true">
                                <div class="form-group center-block">
                                    <label id="lblTituloBusqueda">Buscar: </label>
                                    <asp:TextBox runat="server" ID="txtBusquedaPorNombre" Enabled="true" AutoPostBack="false" />
                                    <label id="lblTituloSeleccionarCiudad">Ciudad: </label>
                                    <asp:DropDownList ID="cmbAllCiudades" runat="server" nombre="Ciudades" DataSourceID="dataAllCiudades" AutoPostBack="false" DataValueField="Id" DataTextField="Nombre" />
                                    <asp:Button ID="btnBuscarEstudio" runat="server" Text="Buscar" Visible="true" OnClick="btnBuscarEstudio_Click" />
                                    <asp:Button ID="btnCrearPrograma" runat="server" Text="Crear programa academico" Visible="true" OnClick="btnCrearPrograma_Click" />
                                </div>
                            </div>
                            <div id="nuevoProgramaAcademico" runat="server" visible="false">
                                <div class="form-horizontal no-margin center-block">
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Institución educativa: </label>
                                        <div class="col-xs-4">
                                            <asp:DropDownList runat="server" ID="cmbInstitucionEducativa" nombre="Institucion educativa" DataSourceID="dataInstitucionesEducativas" DataTextField="NombreYLocacion" DataValueField="Id" Width="200px" OnTextChanged="cmbInstitucionEducativa_TextChanged" OnDataBound="cmbInstitucionEducativa_DataBound" AutoPostBack="true" />
                                        </div>
                                    </div>
                                    <div id="formNuevaInstitucion" class="form-group center-block" runat="server" visible="false">
                                        <label class="control-label col-xs-4">Nueva institución educativa: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtNuevaInstitucion" Text="" Width="200px" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Nombre del nuevo programa academico: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtNuevoPrograma" Text="" Width="200px" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Departamento de institución: </label>
                                        <div class="col-xs-4">
                                            <asp:DropDownList runat="server" ID="cmbDepartamentoInstitucion" nombre="Departamento institución" DataSourceID="dataDepartamentoInstituciones" DataTextField="Nombre" DataValueField="Id" AutoPostBack="true" Width="200px" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Ciudad de institución: </label>
                                        <div class="col-xs-4">
                                            <asp:DropDownList runat="server" ID="cmbCiudadInstitucion" nombre="Ciudad institución" DataSourceID="dataCiudadInstituciones" DataTextField="Nombre" DataValueField="Id" Width="200px" />
                                        </div>
                                    </div>
                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="form-group center-block">
                                                <div class="col-xs-4">
                                                </div>
                                                <div class="col-xs-4">
                                                    <label class="control-label"><b>Procesando información</b> </label>
                                                    <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                                </div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <div class="form-group center-block">
                                        <asp:Label ID="lblErrorProgramaAcademico" CssClass="control-label col-xs-8" ForeColor="Red" Text="Sucedio un error" runat="server" Visible="false" />
                                    </div>
                                    <div class="form-group center-block">
                                        <div class="col-xs-8">
                                            <asp:Button ID="btnNuevoProgramaAcademico" runat="server" Text="Crear programa academico" Visible="true" OnClick="btnNuevoProgramaAcademico_Click" />
                                            <asp:Button ID="btnCancelarNuevoProgramaAcademico" runat="server" Text="Cancelar" OnClick="btnCancelarNuevoProgramaAcademico_Click" Visible="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="listadoProgramas" runat="server" visible="true">
                                <asp:GridView ID="gvProgramaAcademico" runat="server" AutoGenerateColumns="false" CssClass="Grilla" OnRowCommand="gvProgramaAcademico_RowCommand"
                                    HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" Width="100%"
                                    PageSize="10" AllowPaging="true" DataSourceID="dataProgramasAcademicos">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Programa Académico">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkPrograma" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("Nombre")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Institución Educativa">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkInstitucion" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("InstitucionEducativa")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ciudad">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="linkCiudad" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Id") +";"+ Eval("Nombre") +";"+ Eval("Ciudad")+";"+ Eval("CodigoInstitucionEducativa")+";"+ Eval("InstitucionEducativa") + ";" + Eval("CodigoCiudad") %>'
                                                    CommandName="seleccionarPrograma" Text='<%# Eval("Ciudad")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="true">
                                    <ProgressTemplate>
                                        <div class="form-group center-block">
                                            <div class="col-xs-4">
                                            </div>
                                            <div class="col-xs-4">
                                                <label class="control-label"><b>Procesando información</b> </label>
                                                <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                            </div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                        <asp:ObjectDataSource
                            ID="dataAllCiudades"
                            runat="server"
                            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                            SelectMethod="getAllCiudades"></asp:ObjectDataSource>

                        <asp:ObjectDataSource
                            ID="dataProgramasAcademicos"
                            runat="server"
                            EnablePaging="true"
                            SelectMethod="getProgramasAcademicos"
                            SelectCountMethod="getProgramasAcademicosCount"
                            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                            MaximumRowsParameterName="maxRows"
                            StartRowIndexParameterName="startIndex">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="cmbNivelEstudio" DefaultValue="0" Name="codigoNivelEstudio" PropertyName="SelectedValue" Type="Int32" />
                                <asp:ControlParameter ControlID="cmbAllCiudades" DefaultValue="0" Name="codigoCiudad" PropertyName="SelectedValue" Type="Int32" />
                                <asp:ControlParameter ControlID="txtBusquedaPorNombre" DefaultValue="" Name="nombrePrograma" PropertyName="Text" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:ObjectDataSource
                            ID="dataInstitucionesEducativas"
                            runat="server"
                            SelectMethod="getInstitucionesAcademicas"
                            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="cmbNivelEstudio" DefaultValue="0" Name="codigoNivelEstudio" PropertyName="SelectedValue" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>

                        <asp:ObjectDataSource
                            ID="dataCiudadInstituciones"
                            runat="server"
                            SelectMethod="getCiudades"
                            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="cmbDepartamentoInstitucion" DefaultValue="0" Name="codigoDepartamento" PropertyName="SelectedValue" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:ObjectDataSource
                            ID="dataDepartamentoInstituciones"
                            runat="server"
                            TypeName="Fonade.FONADE.PlandeNegocio.CrearEmprendedor"
                            SelectMethod="getDepartamentos"></asp:ObjectDataSource>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrarModalProgramaAcademico" runat="server" Text="Cerrar" OnClick="btnCerrarModalProgramaAcademico_Click" Visible="true" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel ID="ModalVistaPrevia" CssClass="modal fade" runat="server" role="dialog">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title"><b>Verifique la información de emprendedor antes de guardarlo !</b></h4>
                        </div>
                        <div class="modal-body">
                            <div style="width: 1000px; margin: 0 auto;">
                                <div class="form-horizontal no-margin center-block">
                                    <div class="form-group center-block">
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <h3>
                                                <asp:Label Text="Datos del emprendedor" runat="server" ID="lblTituloCrearEmprendedorVistaPrevia" Visible="true" />
                                            </h3>
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Nombres : </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtNombresVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Apellidos : </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtApellidosVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Identificación : </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtTipoDocumentoVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">No : </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtIdentificacionVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Departamento expedición: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtDepartamentoExpedicionVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Ciudad expedición: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtCiudadExpedicionVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Correo electrónico: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtEmailVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Género: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtGeneroVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Fecha Nacimiento: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtFechaNacimientoVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Departamento nacimiento: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtDepartamentoNacimientoVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Ciudad nacimiento: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtCiudadNacimientoVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Teléfono: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtTelefonoFijoVistaPrevia" Text="" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Direccion de Residencia: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtDireccionEmpVistaPrevia" Text="" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Departamento Residencia: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtDepartamentoResidencia" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Ciudad Residencia: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtCiudadResidencia" Enabled="false"/>
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Nivel de estudio : </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtNivelEstudioVistaPrevia" Text="" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Programa Realizado: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtProgramaRealizadoVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Institución: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox ID="txtInstitucionEducativaVistaPrevia" runat="server" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Ciudad Institución: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtCiudadInstitucionVistaPrevia" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Estado: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtEstadoEstudioVistaPrevia" Enabled="false" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Fecha inicio: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtFechaInicioEstudioVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div id="formFechaFinalizacionVistaPrevia" runat="server" visible="false" class="form-group center-block">
                                        <label class="control-label col-xs-4">Fecha finalización: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtFechaFinalizacionEstudioVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div id="formFechaGraduacionVistaPrevia" runat="server" visible="false" class="form-group center-block">
                                        <label class="control-label col-xs-4">Fecha graduación: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtFechaGraduacionEstudioVistaPrevia" Enabled="false" placeholder="dd/mm/aaaa" />
                                        </div>
                                    </div>
                                    <div id="formHorasDedicadasVistaPrevia" runat="server" visible="false" class="form-group center-block">
                                        <label class="control-label col-xs-4">Semestre actual u horas dedicadas: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtHorasDedicadasVistaPrevia" Enabled="false" Visible="true" />
                                        </div>
                                    </div>
                                    <div class="form-group center-block">
                                        <label class="control-label col-xs-4">Tipo de Aprendiz: </label>
                                        <div class="col-xs-4">
                                            <asp:TextBox runat="server" ID="txtTipoAprendizVistaPrevia" Enabled="false" Visible="true" />
                                        </div>
                                    </div>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3" DynamicLayout="true">
                                        <ProgressTemplate>
                                            <div class="form-group center-block">
                                                <div class="col-xs-4">
                                                </div>
                                                <div class="col-xs-4">
                                                    <label class="control-label"><b>Procesando información</b> </label>
                                                    <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                                </div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnActualizarEmprendedorVistaPrevia" runat="server" Text="Actualizar emprendedor" Visible="false" OnClick="btnActualizarEmprendedorVistaPrevia_Click" />
                            <asp:Button ID="btnGuardarEmprendedorVistaPrevia" runat="server" Text="Guardar emprendedor" Visible="false" OnClick="btnVerCertificado_Click" />
                            <asp:Button ID="btnModificarVistaPrevia" runat="server" Text="Modificar información" Visible="true" OnClick="btnModificarVistaPrevia_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <asp:Panel ID="ModalCertificadoAsesor" CssClass="modal fade" runat="server" role="dialog">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="modal-dialog modal-lg">
                    <!-- Modal content-->
                    <div class="modal-content" style="width: 600px; margin: 0 auto;">
                        <div class="modal-body">
                            <h4 class="modal-title"><b>Certificación de registro a emprendedores </b></h4>
                            <br />
                            <p>
                                Yo,
                    <b>
                        <asp:Label ID="lblNombreAsesor" Text="Nombre de asesor" runat="server" Visible="true" /></b>, certifico que 
                    <b>
                        <asp:Label ID="lblNombreEmprendedor" Text="Nombre de emprendedor" runat="server" Visible="true" />
                    </b>identificado(a) con 
                    <b>C.C.
                        <asp:Label ID="lblCedulaEmprendedor" Text="123456788" runat="server" Visible="true" />
                    </b>
                                del plan de negocios <b>
                                    <asp:Label ID="lblNombreProyecto" Text="123456788" runat="server" Visible="true" />
                                </b>cumple con todos los requisitos establecidos en el reglamento interno vigente del Fondo Emprender.
                            </p>
                            <p>
                                Certifico además que, ninguno de los miembros del equipo de trabajo inscritos en
                    el plan de negocio anteriormente relacionado por la Unidad de Emprendimiento de: 
                    <b>
                        <asp:Label ID="lblUnidadEmpredimientoTexto" Text="Unidad de emprendimiento" runat="server" Visible="true" />
                    </b>
                                tiene vínculos con Directores Regionales, Subdirectores de Centro, asesores y/o instructores 
                    de emprendimiento y Empresarismo de esta Unidad de Emprendimiento, como cónyuge, compañera o
                    compañero permanente o vínculos de parentesco hasta el segundo grado de consanguinidad, 
                    segundo de afinidad o primero civil.
                            </p>
                            <p>
                                <br />
                                <br />
                                <span>Firma
                <br />
                                    <br />
                                    <br />
                                    ______________________________________________________
                <br />
                                    <asp:Label ID="lblUnidadEmpredimiento" Text="Unidad de emprendimiento" runat="server" Visible="true" />
                                    <br />
                                    CARGO :
                                    <asp:Label ID="lblNombrePerfil" Text="Emprendedor" runat="server" Visible="true" />
                                    <br />
                                    C.C.
                                    <asp:Label ID="lblCedulaAsesor" Text="123456789" runat="server" Visible="true" />
                                    <br />
                                </span>
                            </p>
                        </div>
                        <div class="modal-footer">
                            <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanel4" DynamicLayout="true">
                                <ProgressTemplate>
                                    <div class="form-group center-block">
                                        <div class="col-xs-4">
                                        </div>
                                        <div class="col-xs-4">
                                            <label class="control-label"><b>Procesando información</b> </label>
                                            <img class="control-label" src="http://www.bba-reman.com/images/fbloader.gif" />
                                        </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:Button ID="btnCertificarRegistro" CssClass="btn btn-success" runat="server" Text="Certifico" Visible="true" OnClick="btnGuardarEmprendedorVistaPrevia_Click" />
                            <asp:Button ID="btnNoCertificarRegistro" CssClass="btn btn-danger" runat="server" Text="No Certifico" Visible="true" OnClick="btnNoCertificarRegistro_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>


    <script src="https://code.jquery.com/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js" type="text/javascript"></script>
</asp:Content>
