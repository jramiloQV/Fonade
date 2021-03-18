<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ListadoRegistroUnico.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.LiderRegional.ListadoRegistroUnico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <asp:Label ID="lblTitulo" runat="server"
        Text="Registro Unico" Font-Size="Large"></asp:Label>
    <hr />
    <asp:Label ID="lblCentroDesarrollo" runat="server" Text="Centro de Desarrollo:"></asp:Label>
    <asp:DropDownList ID="ddlCentroDesarrollo" runat="server">
        </asp:DropDownList>
    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" OnClick="btnFiltrar_Click"/>
    <hr />
    <div id="divGrilla" style="overflow-y:auto">
        <asp:GridView ID="gvRegistros" runat="server" Width="98%" AutoGenerateColumns="False"
            CssClass="Grilla"
            AllowPaging="true" RowStyle-VerticalAlign="Top"
            EmptyDataText="No hay usuarios registrados"
            OnPageIndexChanging="gvRegistros_PageIndexChanging"
            PageSize="30">
            <Columns>
                <asp:BoundField DataField="Id_ContactoRegistro" HeaderText="ID"
                    SortExpression="Id_ContactoRegistro" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro"
                    SortExpression="FechaRegistro" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Nombres" HeaderText="Nombres"
                    SortExpression="Nombres" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Apellidos" HeaderText="Apellidos"
                    SortExpression="Apellidos" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomTipoIdentificacion" HeaderText="Tipo Identificacion"
                    SortExpression="NomTipoIdentificacion" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion"
                    SortExpression="Identificacion" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadExpedicion" HeaderText="Ciudad Expedicion"
                    SortExpression="CiudadExpedicion" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoExpedicion" HeaderText="Departamento Expedicion"
                    SortExpression="DeptoExpedicion" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Fecha_Nacimiento" HeaderText="Fecha Nacimiento"
                    SortExpression="Fecha_Nacimiento" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadNacimiento" HeaderText="Ciudad Nacimiento"
                    SortExpression="CiudadNacimiento" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoNacimiento" HeaderText="Departamento Nacimiento"
                    SortExpression="DeptoNacimiento" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Genero" HeaderText="Genero"
                    SortExpression="Genero" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomEstadoCivil" HeaderText="Estado Civil"
                    SortExpression="NomEstadoCivil" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomOcupacion" HeaderText="Ocupacion"
                    SortExpression="NomOcupacion" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Telefono" HeaderText="Telefono"
                    SortExpression="Telefono" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Correo" HeaderText="Correo"
                    SortExpression="Correo" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadResidencia" HeaderText="Ciudad Residencia"
                    SortExpression="CiudadResidencia" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoResidencia" HeaderText="Departamento Residencia"
                    SortExpression="DeptoResidencia" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DireccionDondeReside" HeaderText="Direccion Residencia"
                    SortExpression="DireccionDondeReside" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Estrato" HeaderText="Estrato"
                    SortExpression="Estrato" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomNivelEstudio" HeaderText="Nivel Estudio"
                    SortExpression="NomNivelEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProgramaAcademico" HeaderText="Programa Academico"
                    SortExpression="ProgramaAcademico" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomInstitucionEducativa" HeaderText="Institucion Educativa"
                    SortExpression="NomInstitucionEducativa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadEstudio" HeaderText="Ciudad Estudio"
                    SortExpression="CiudadEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoEstudio" HeaderText="Departamento Estudio"
                    SortExpression="DeptoEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EstadoEstudio" HeaderText="Estado Estudio"
                    SortExpression="EstadoEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaINIEstudio" HeaderText="Fecha Inicio Estudio"
                    SortExpression="FechaINIEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="SemestresCursados" HeaderText="Semestres Cursados"
                    SortExpression="SemestresCursados" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaFinMaterias" HeaderText="Fecha Fin Materias"
                    SortExpression="FechaFinMaterias" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaGradoEstudio" HeaderText="Fecha Grado"
                    SortExpression="FechaGradoEstudio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomTipoAprendiz" HeaderText="Tipo Aprendiz"
                    SortExpression="NomTipoAprendiz" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CentroDesaEmpresarial" HeaderText="Centro Desarrollo Empresarial"
                    SortExpression="CentroDesaEmpresarial" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadCentEmpresarial" HeaderText="Ciudad Centro Desarrollo Empresarial"
                    SortExpression="CiudadCentEmpresarial" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoCentEmpresarial" HeaderText="Departamento Centro Desarrollo Empresarial"
                    SortExpression="DeptoCentEmpresarial" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomServicio" HeaderText="Servicio"
                    SortExpression="NomServicio" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FormacionInteres" HeaderText="Formacion en la que esta interesado"
                    SortExpression="FormacionInteres" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudEmprendeRural" HeaderText="Ciudad de Formacion"
                    SortExpression="CiudEmprendeRural" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="dptoEmprendeRural" HeaderText="Departamento de Formacion"
                    SortExpression="dptoEmprendeRural" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomProyecto" HeaderText="Nombre Del Proyecto"
                    SortExpression="NomProyecto" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescripcionProyecto" HeaderText="Descripcion del Proyecto"
                    SortExpression="DescripcionProyecto" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadFondoEmprender" HeaderText="Ciudad donde va desarrollar el Proyecto"
                    SortExpression="CiudadFondoEmprender" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DeptoFondoEmprender" HeaderText="Departamento donde va desarrollar el Proyecto"
                    SortExpression="DeptoFondoEmprender" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomSubSector" HeaderText="SubSector del Proyecto"
                    SortExpression="NomSubSector" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomSector" HeaderText="Sector del Proyecto"
                    SortExpression="NomSector" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="local" HeaderText="(local) Proyecto"
                    SortExpression="local" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Regional" HeaderText="(Regional) Proyecto"
                    SortExpression="Regional" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Nacional" HeaderText="(Nacional) Proyecto"
                    SortExpression="Nacional" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Internacional" HeaderText="(Internacional) Proyecto"
                    SortExpression="Internacional" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProductoServicioOferta" HeaderText="Producto o servicio que oferta o proyecta ofertar"
                    SortExpression="ProductoServicioOferta" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProductoEnElMercado" HeaderText="Actualmente comercializa su producto en el mercado"
                    SortExpression="ProductoEnElMercado" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EmpleadosACargo" HeaderText="Tiene empleados a su cargo"
                    SortExpression="EmpleadosACargo" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CantidadEmpleados" HeaderText="Cantidad Empleados"
                    SortExpression="CantidadEmpleados" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EmpresaFortalecimiento" HeaderText="Nombre de la Empresa"
                    SortExpression="EmpresaFortalecimiento" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CorreoEmpresa" HeaderText="Correo de la Empresa"
                    SortExpression="CorreoEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="TelefonoEmpresa" HeaderText="Teléfono de la Empresa"
                    SortExpression="TelefonoEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DireccionEmpresa" HeaderText="Direccion de la Empresa"
                    SortExpression="DireccionEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CiudadEmpresa" HeaderText="Ciudad de la Empresa"
                    SortExpression="CiudadEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DepartamentoEmpresa" HeaderText="Departamento de la Empresa"
                    SortExpression="DepartamentoEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomSubSectorEmpresa" HeaderText="SubSector Empresa"
                    SortExpression="NomSubSectorEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NomSectorEmpresa" HeaderText="Sector Empresa"
                    SortExpression="NomSectorEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="FechaConstitucionEmpresa" HeaderText="Fecha Constitucion Empresa"
                    SortExpression="FechaConstitucionEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="TamaEmpresa" HeaderText="Tamaño de la empresa"
                    SortExpression="TamaEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="localEmpresa" HeaderText="local (Empresa)"
                    SortExpression="localEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="RegionalEmpresa" HeaderText="Regional (Empresa)"
                    SortExpression="RegionalEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="NacionalEmpresa" HeaderText="Nacional (Empresa)"
                    SortExpression="NacionalEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="InternacionalEmpresa" HeaderText="Internacional (Empresa)"
                    SortExpression="InternacionalEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ProductoServicioOfertaEmpresa" HeaderText="Producto o servicio que oferta (Empresa)"
                    SortExpression="ProductoServicioOfertaEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="DescripcionActividadEmpresa" HeaderText="Descripción de la actividad económica que desarrolla (Empresa)"
                    SortExpression="DescripcionActividadEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ValorVentasAnuales" HeaderText="Valor de las ventas anuales (Empresa)"
                    SortExpression="ValorVentasAnuales" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EmpleadosEmpresa" HeaderText="Número de empleados (Empresa)"
                    SortExpression="EmpleadosEmpresa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="EsElPropietario" HeaderText="Es usted el propietario (Empresa)"
                    SortExpression="EsElPropietario" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="CargoQueOcupa" HeaderText="Cargo que ocupa (Empresa)"
                    SortExpression="CargoQueOcupa" HeaderStyle-HorizontalAlign="Center">
                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                </asp:BoundField>
                
            </Columns>
            <RowStyle VerticalAlign="Top"></RowStyle>
        </asp:GridView>
    </div>
</asp:Content>
