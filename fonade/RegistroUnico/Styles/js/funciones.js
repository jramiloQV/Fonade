$(document).ready(function () {

    $("#cmbPropietario").change(function () {

        if ($(this).val() == 2) {
            $("#TextCargoOcupaa").show();
            $("#divCargoOcupa").show();
        }
        else {
            $("#TextCargoOcupaa").hide();
            $("#divCargoOcupa").hide();
        }
    });
});

$(document).ready(function () {
    $("#cmbProgramaAcademico").change(function () {

        document.getElementById("CodPrograma").value = $(this).val();


    });
});

function calcularEdad(fecha) {
    var hoy = new Date();
    var cumpleanos = new Date(fecha);
    var edad = hoy.getFullYear() - cumpleanos.getFullYear();
    var m = hoy.getMonth() - cumpleanos.getMonth();

    if (m < 0 || (m === 0 && hoy.getDate() < cumpleanos.getDate())) {
        edad--;
    }

    return edad;
}

$(document).ready(function () {
    $('#cmbEmpleados').change(function (e) {
        if ($(this).val() == 1) {
            if ($('#divempleadosacargo').css('visibility') == 'hidden' && $('#divempleadosacargotxt').css('visibility') == 'hidden') {
                $('#divempleadosacargo').css('visibility', 'visible');
                $('#divempleadosacargotxt').css('visibility', 'visible');
            }
        } else {
            $('#divempleadosacargo').css('visibility', 'hidden');
            $('#divempleadosacargotxt').css('visibility', 'hidden');
        }
    });
});

$(document).ready(function () {
    $('#cmbEstadoEstudio').change(function () {
        if ($(this).val() == 'Finalizado') {          
          
            $("#DivFinalizacion").show();
            $("#DivSemestreActual").css('visibility', 'hidden');
            
        }
        else if ($(this).val() == 0)
        {
            $("#DivFinalizacion").hide();
            $("#DivSemestreActual").css('visibility', 'hidden');
        }
        else {
            $("#DivFinalizacion").hide();
            $("#DivSemestreActual").css('visibility', 'visible');
        }
    });
});


$(document).ready(function () {

    $("#cmbServicioRequerido").change(function () {

        if ($(this).val() == 0) {
            $("#SenaRural").hide();
            $("#EmprendimientoFondoEmprender").hide();
            $("#fortalecimiento").hide();
            $("#crearSol").hide();
            $("#btnRegistrarSolicitud").show();


        }
        if ($(this).val() == 2) {
            $("#SenaRural").show();
            $("#EmprendimientoFondoEmprender").hide();
            $("#fortalecimiento").hide();
            $("#crearSol").show();
            $("#btnRegistrarSolicitud").show();
        }
        if ($(this).val() == 3 || $(this).val() == 4) {
            $("#EmprendimientoFondoEmprender").show();
            $("#SenaRural").hide();
            $("#fortalecimiento").hide();
            $("#crearSol").show();
            $("#btnRegistrarSolicitud").show();
        }
        if ($(this).val() == 5) {
            $("#fortalecimiento").show();
            $("#SenaRural").hide();
            $("#EmprendimientoFondoEmprender").hide();
            $("#crearSol").show();
            $("#btnRegistrarSolicitud").show();
        }

    });
});


function ValidateCheckBoxList() {

    var listItems = document.getElementById("cblMercadoOfer").getElementsByTagName("input");
    var itemcount = listItems.length;
    var iCount = 0;
    var isItemSelected = false;
    var ok = true;
    for (iCount = 0; iCount < itemcount; iCount++) {
        if (listItems[iCount].checked) {
            isItemSelected = true;
            break;
        }
    }
    if (!isItemSelected) {
        alert("Por favor seleccione una opcion de Mercado en el que vende o proyecta vender su producto o servicio:");
        document.getElementById("cblMercadoOfer").focus();
        return ok = false;
    }
    else {
        ValidarFormularioMaxCaracteres();

    }
    return false;
}
function ValidateCheckBoxEmpresa() {

    var listItems = document.getElementById("cblMercadoOferForta").getElementsByTagName("input");
    var itemcount = listItems.length;
    var iCount = 0;
    var isItemSelected = false;
    var ok = true;
    for (iCount = 0; iCount < itemcount; iCount++) {
        if (listItems[iCount].checked) {
            isItemSelected = true;
            break;
        }
    }
    if (!isItemSelected) {
        alert("Por favor seleccione una opcion de Mercado en  el que vende su producto o servicio");
        document.getElementById("cblMercadoOferForta").focus();
        ok = false;
    }
    else {

        ValidarFormularioMaxCaracteres();
       
    }
    return false;
}


function ValidarFormulario(e) {

    var Noms, Apell, Identi, NumIden, DepExp, CiuExp, correo, genero, fechanac, DepNac,
        CiuNac, Telefono, EstadoCivil, Ocupacion, DepResi, MunResi, DirecionRe, NivelEst,
        programa, InstiAca, InstiAcaCiud, EstadoEst, fechaini, SemestreActual, fechafin,
        fechagradu, Estrato, TipoApr, DepCentroEmp, CiuCentroEmp, CentroEmpresarial, ServRequ,
        formacionIntere, DepFor, CiuFor, DepDesaProyecto, CiudDesaProyecto, sectoremprend, subsectorempren,
        NomProyecto, txtProductoOferta, txtProductoMercado, cmbemple, cuantosemple, NomEmpresa, DepEmpresa, CiuEmpresa,
        SectorEmpresa, SubSecEmpresa, DirEmpresa, FechaConsEmpre, TeleEmpresa, CorreoEmpresa, TamaEmpre, ProduOferEmp,
        DescripActiEcoEmpre, ValVenAnua, NumEmpleEmp, UstedEsPropie, CargoOcupa, DescProyecto, ok = true;

    Noms = document.getElementById("txtNombres").value;
    Apell = document.getElementById("TextApellidos").value;
    Identi = document.getElementById("cmbTipoIdentificacion").value;
    NumIden = document.getElementById("TextNumIdentificacion").value
    DepExp = document.getElementById("cmbDepartamentoExpedicion").value;
    CiuExp = document.getElementById("cmbCiudadExpedicion").value;
    correo = document.getElementById("TextCorreo").value;
    genero = document.getElementById("cmbGenero").value;
    fechanac = document.getElementById("Textfechanac").value;
    DepNac = document.getElementById("cmbDepartamentoNacimiento").value;
    CiuNac = document.getElementById("cmbCiudadNacimiento").value;
    Telefono = document.getElementById("TextTelefono").value;
    EstadoCivil = document.getElementById("cmbEstadoCivil").value;
    Ocupacion = document.getElementById("cmbOcupacion").value;
    DepResi = document.getElementById("cmbDepartamentoReside").value;
    MunResi = document.getElementById("cmbMunicipioReside").value;
    DirecionRe = document.getElementById("TextDireccionReside").value;
    Estrato = document.getElementById("TextEstrato").value;
    NivelEst = document.getElementById("cmbNivelEstudio").value;
    programa = document.getElementById("cmbProgramaAcademico").value;
    InstiAca = document.getElementById("cmbInstitucion").value;
    InstiAcaCiud = document.getElementById("cmbInstitucionCiudad").value;
    EstadoEst = document.getElementById("cmbEstadoEstudio").value;
    fechaini = document.getElementById("txtfechaini").value;
    SemestreActual = document.getElementById("TextSemestreActual").value;
    fechafin = document.getElementById("Textfechafin").value;
    fechagradu = document.getElementById("Textfechagraducacion").value;
    TipoApr = document.getElementById("cmbTipoAprendiz").value;
    DepCentroEmp = document.getElementById("cmbDepartamentoEmpresarial").value;
    CiuCentroEmp = document.getElementById("cmbCiudadEmpresarial").value;
    CentroEmpresarial = document.getElementById("cmbCentroEmpresarial").value;
    ServRequ = document.getElementById("cmbServicioRequerido").value;
    formacionIntere = document.getElementById("cmbFormacion").value;
    DepFor = document.getElementById("cmbDepartamentoFormacion").value;
    CiuFor = document.getElementById("cmbCiudadFormacion").value;
    DepDesaProyecto = document.getElementById("cmbDepartamentoDesarrollarProyecto").value;
    CiudDesaProyecto = document.getElementById("cmbCiudadDesarrollarProyecto").value;
    sectoremprend = document.getElementById("cmbSector").value;
    subsectorempren = document.getElementById("cmbSubSector").value;
    NomProyecto = document.getElementById("TextNomProyecto").value;
    DescProyecto = document.getElementById("TextDescPro").value;
    txtProductoOferta = document.getElementById("TextProductoOferta").value;
    txtProductoMercado = document.getElementById("TextProductoMercado").value;
    cmbemple = document.getElementById("cmbEmpleados").value;
    cuantosemple = document.getElementById("TextCuantosEmpleados").value;
    NomEmpresa = document.getElementById("TextNomEmpresa").value;
    DepEmpresa = document.getElementById("cmbUbicacionEmpresaD").value;
    CiuEmpresa = document.getElementById("cmbUbicacionEmpresaC").value;
    SectorEmpresa = document.getElementById("cmbSectorEmpresa").value;
    SubSecEmpresa = document.getElementById("cmbSubSectorEmpresa").value;
    DirEmpresa = document.getElementById("TextDireccionEmpresa").value;
    FechaConsEmpre = document.getElementById("TextFechaEmpresa").value;
    TeleEmpresa = document.getElementById("TextTelefonoEmpresa").value;
    CorreoEmpresa = document.getElementById("TextCorreoEmpresa").value;
    TamaEmpre = document.getElementById("cmbTamaEmpresa").value;
    ProduOferEmp = document.getElementById("TextProductoOfertaE").value;
    DescripActiEcoEmpre = document.getElementById("TextActividadEconomica").value;
    ValVenAnua = document.getElementById("TextVentasAnuales").value;
    NumEmpleEmp = document.getElementById("TextNumEmpleados").value;
    UstedEsPropie = document.getElementById("cmbPropietario").value;
    CargoOcupa = document.getElementById("TextCargoOcupa").value;

    var f = new Date();
    var hoy = (f.getFullYear() + "/" + (f.getMonth() + 1) + "/" + f.getDate());

    var Edad = calcularEdad(fechanac);
    var expr = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (Noms == "") {
        alert("El campo Nombres no puede estar vacio");
        document.getElementById("txtNombres").focus();
        return ok = false;

    } else if (Apell == "") {
        alert("El campo Apellidos no puede estar vacio");
        document.getElementById("TextApellidos").focus();
        return ok = false;
    } else if (Identi == 0) {
        alert("Debe seleccionar un tipo de identificacion");
        document.getElementById("cmbTipoIdentificacion").focus();
        return ok = false;
    } else if (NumIden == "") {
        alert("El campo No. no puede esta vacio");
        document.getElementById("TextNumIdentificacion").focus();
        return ok = false;
    } else if (DepExp == 0) {
        alert("Debe seleccionar sl Departamento se Expediccion");
        document.getElementById("cmbDepartamentoExpedicion").focus();
        return ok = false;
    } else if (CiuExp == 0) {
        alert("Debe seleccionar la Ciudad de Expediccion");
        document.getElementById("cmbCiudadExpedicion").focus();
        return ok = false;
    } else if (correo == "") {
        alert("El campor Correo no puede estar vacio");
        document.getElementById("TextCorreo").focus();
        return ok = false;
    } else if (!expr.test(correo)) {
        alert("Error: La dirección de correo " + correo + " es incorrecta.");
        document.getElementById("TextCorreo").focus();
        return ok = false;
    } else if (genero == "") {
        alert("Debe Seleccionar un Genero");
        document.getElementById("cmbGenero").focus();
        return ok = false;
    } else if (fechanac == "") {
        alert("Debe Seleccionar Fecha de nacimiento");
        document.getElementById("Textfechanac").focus();
        return ok = false;
    } else if (Date.parse(fechanac) >= Date.parse(hoy)) {
        alert("La fecha nacimiento no puede ser mayor o igual a la actual");
        document.getElementById("Textfechanac").focus();
        return ok = false;
    } else if (Edad <= 14) {
        alert("Debe registrar una fecha valida");
        document.getElementById("Textfechanac").focus();
        return ok = false;
    } else if (DepNac == 0) {
        alert("Debe Seleccionar el departamento de nacimiento");
        document.getElementById("cmbDepartamentoNacimiento").focus();
        return ok = false;
    } else if (CiuNac == 0) {
        alert("Debe Seleccionar la ciudad de nacimiento");
        document.getElementById("cmbCiudadNacimiento").focus();
        return ok = false;
    } else if (Telefono == "") {
        alert("El campo Telefono no debe estar vacio");
        document.getElementById("TextTelefono").focus();
        return ok = false;
    } else if (EstadoCivil == 0) {
        alert("Debe Seleccionar un estado civil");
        document.getElementById("cmbEstadoCivil").focus();
        return ok = false;
    } else if (Ocupacion == 0) {
        alert("Debe Seleccionar una ocupacion");
        document.getElementById("cmbOcupacion").focus();
        return ok = false;
    } else if (DepResi == 0) {
        alert("Debe Seleccionar un departamento de residencia");
        document.getElementById("cmbDepartamentoReside").focus();
        return ok = false;
    } else if (MunResi == 0) {
        alert("Debe Seleccionar una ciudad de residencia");
        document.getElementById("cmbMunicipioReside").focus();
        return ok = false;
    } else if (DirecionRe == "") {
        alert("El campo direccion residencia no puede estar vacio ");
        document.getElementById("TextDireccionReside").focus();
        return ok = false;
    } else if (Estrato == "") {
        alert("El campo Estrato no puede estar vacio ");
        document.getElementById("TextEstrato").focus();
        return ok = false;
    } else if (NivelEst == 0) {
        alert("Debe Seleccionar un nivel de estudio ");
        document.getElementById("cmbNivelEstudio").focus();
        return ok = false;
    } else if (programa == 1111111) {
        alert("Debe Seleccionar un programa academico");
        document.getElementById("cmbProgramaAcademico").focus();
        return ok = false;
    } else if (InstiAca == 0) {
        alert("Debe Seleccionar una institucion");
        document.getElementById("cmbInstitucion").focus();
        return ok = false;
    } else if (InstiAcaCiud == 0) {
        alert("Debe Seleccionar una Ciudad");
        document.getElementById("cmbInstitucionCiudad").focus();
        return ok = false;
    } else if (EstadoEst == 0) {
        alert("Debe Seleccionar el estado del estudio");
        document.getElementById("cmbEstadoEstudio").focus();
        return ok = false;
    } else if (fechaini == "") {
        alert("Debe Seleccionar fecha de inicio del estudio");
        document.getElementById("txtfechaini").focus();
        return ok = false;
    } else if (Date.parse(fechaini) >= Date.parse(hoy)) {
        alert("La fecha inicio de estudio no puede ser mayor o igual a la actual");
        document.getElementById("txtfechaini").focus();
        return ok = false;
    } else if (EstadoEst == 'Actualmente cursando') {
        if (SemestreActual == "") {
            alert("El Campo Semestre actual u horas dedicadas no debe estar vacio");
            document.getElementById("TextSemestreActual").focus();
            return ok = false;
        }
    } else if (EstadoEst == 'Finalizado') {
        if (fechafin == "") {
            alert("El campo fecha finalizacion de  materias no debe estar vacio");
            document.getElementById("Textfechafin").focus();
            return ok = false;
        } else if (Date.parse(fechafin) >= Date.parse(hoy)) {
            alert("La fecha Finalizacion de materias no puede ser mayor a la fecha actual");
            document.getElementById("Textfechafin").focus();
            return ok = false;
        } else if (Date.parse(fechafin) <= Date.parse(fechaini)) {
            alert("La fecha Finalizacion de materias no puede ser menor a la fecha inicio");
            document.getElementById("Textfechafin").focus();
            return ok = false;
        } else if (fechagradu == "") {
            alert("El campo fecha graduacion no debe estar vacio");
            document.getElementById("Textfechagraducacion").focus();
            return ok = false;
        } else if (Date.parse(fechagradu) >= Date.parse(hoy)) {
            alert("La fecha de graduacion no puede ser mayor o igual a la fecha actual");
            document.getElementById("Textfechagraducacion").focus();
            return ok = false;
        } else if (Date.parse(fechagradu) <= Date.parse(fechaini)) {
            alert("La fecha graduacion no puede ser menor a la fecha inicio");
            document.getElementById("Textfechagraducacion").focus();
            return ok = false;
        } else if (Date.parse(fechagradu) <= Date.parse(fechafin)) {
            alert("La fecha graduacion no puede ser menor a la fecha finalizacion de materias");
            document.getElementById("Textfechagraducacion").focus();
            return ok = false;
        }

    }
    if (TipoApr == 0) {
        alert("Seleccione una opcion de tipo de aprendiz");
        document.getElementById("cmbTipoAprendiz").focus();
        return ok = false;
    } else if (DepCentroEmp == 0) {
        alert("Seleccione un departamento para filtrar el centro de desarrollo empresarial");
        document.getElementById("cmbDepartamentoEmpresarial").focus();
        return ok = false;
    } else if (CiuCentroEmp == 0) {
        alert("Seleccione una Ciudad para filtrar el centro de desarrollo empresarial");
        document.getElementById("cmbCiudadEmpresarial").focus();
        return ok = false;
    } else if (CentroEmpresarial == 0) {
        alert("Seleccione un centro de desarrollo empresarial, en caso de que no se muestre ningun centro, debe validar en otro municipio");
        document.getElementById("cmbCentroEmpresarial").focus();
        return ok = false;
    } else if (ServRequ == 0) {
        alert("Seleccione el servicio que requiere");
        document.getElementById("cmbServicioRequerido").focus();
        return ok = false;
    } else if (ServRequ == 2) {

        if (formacionIntere == 0) {
            alert("Seleccione una formacion en la que este interesado");
            document.getElementById("cmbFormacion").focus();
            return ok = false;
        } else if (DepFor == 0) {
            alert("Seleccione un departamento para la formacion");
            document.getElementById("cmbDepartamentoFormacion").focus();
            return ok = false;
        } else if (CiuFor == 0) {
            alert("Seleccione una Ciudad para la formacion");
            document.getElementById("cmbCiudadFormacion").focus();
            return ok = false;
        }
    } else if (ServRequ == 3 || ServRequ == 4) {

        if (DepDesaProyecto == 0) {
            alert("Seleccione un departamento donde va a desarrollar su proyecto");
            document.getElementById("cmbDepartamentoDesarrollarProyecto").focus();
            return ok = false;
        } else if (CiudDesaProyecto == 0) {
            alert("Seleccione una ciudad donde va a desarrollar su proyecto");
            document.getElementById("cmbCiudadDesarrollarProyecto").focus();
            return ok = false;
        } else if (sectoremprend == 0) {
            alert("Seleccione el sector de su proyecto");
            document.getElementById("cmbSector").focus();
            return ok = false;
        } else if (subsectorempren == 0) {
            alert("Seleccione el subsector de su proyecto");
            document.getElementById("cmbSubSector").focus();
            return ok = false;
        } else if (NomProyecto == "") {
            alert("El campo Nombre del proyecto No puede estar vacio");
            document.getElementById("TextNomProyecto").focus();
            return ok = false;
        } else if (DescProyecto == "") {
            alert("El campo Descripcion del Proyecto No puede estar vacio");
            document.getElementById("TextDescPro").focus();
            return ok = false;
        } else if (txtProductoOferta == "") {
            alert("El campo Producto o servicio que oferta o proyecta ofertar no puede estar vacio");
            document.getElementById("TextProductoOferta").focus();
            return ok = false;
        } else if (txtProductoMercado == "") {
            alert("El campo Actualmente comercializa su producto en el mercado no puede estar vacio");
            document.getElementById("TextProductoMercado").focus();
            return ok = false;
        } else if (cmbemple == 0) {
            alert("Debe seleccionar una opcion de la lista");
            document.getElementById("cmbEmpleados").focus();
            return ok = false;
        } else if (cmbemple == 1) {
            if (cuantosemple == "") {
                alert("Debe indicar la cantidad de empleado");
                document.getElementById("TextCuantosEmpleados").focus();
                return ok = false;
            }
        }

        return ValidateCheckBoxList(this);
    } else if (ServRequ == 5) {

        if (NomEmpresa == "") {
            alert("El campo Nombre Empresa no puede estar vacio");
            document.getElementById("TextNomEmpresa").focus();
            return ok = false;
        } else if (DepEmpresa == 0) {
            alert("Seleccione el Departamento donde se encuentra su empresa");
            document.getElementById("cmbUbicacionEmpresaD").focus();
            return ok = false;
        } else if (CiuEmpresa == 0) {
            alert("Seleccione la Ciudad donde se encuentra su empresa");
            document.getElementById("cmbUbicacionEmpresaC").focus();
            return ok = false;
        } else if (SectorEmpresa == 0) {
            alert("Seleccione el sector");
            document.getElementById("cmbSectorEmpresa").focus();
            return ok = false;
        } else if (SubSecEmpresa == 0) {
            alert("Seleccione el subsector");
            document.getElementById("cmbSubSectorEmpresa").focus();
            return ok = false;
        } else if (DirEmpresa == "") {
            alert("El campo Direccion de la Empresa no puede estar vacio");
            document.getElementById("TextDireccionEmpresa").focus();
            return ok = false;
        } else if (FechaConsEmpre == "") {
            alert("El campo Fecha Constitucion no puede estar vacio");
            document.getElementById("TextFechaEmpresa").focus();
            return ok = false;
        } else if (TeleEmpresa == "") {
            alert("El campo Teléfono de la Empresa no puede estar vacio");
            document.getElementById("TextTelefonoEmpresa").focus();
            return ok = false;
        } else if (CorreoEmpresa == "") {
            alert("El campo Correo de la Empresa no puede estar vacio");
            document.getElementById("TextCorreoEmpresa").focus();
            return ok = false;
        } else if (!expr.test(CorreoEmpresa)) {
            alert("Error: La dirección de correo " + CorreoEmpresa + " es incorrecta.");
            document.getElementById("TextCorreoEmpresa").focus();
            return ok = false;
        } else if (TamaEmpre == 0) {
            alert("Seleccione el tamaño de la empresa");
            document.getElementById("cmbTamaEmpresa").focus();
            return ok = false;
        } else if (ProduOferEmp == "") {
            alert("El campo Producto o servicio que oferta  no puede estar vacio");
            document.getElementById("TextProductoOfertaE").focus();
            return ok = false;
        } else if (DescripActiEcoEmpre == "") {
            alert("El campo Descripción de la actividad económica  no puede estar vacio");
            document.getElementById("TextActividadEconomica").focus();
            return ok = false;
        } else if (ValVenAnua == "") {
            alert("El campo Ventas Anuales  no puede estar vacio");
            document.getElementById("TextVentasAnuales").focus();
            return ok = false;
        } else if (NumEmpleEmp == "") {
            alert("El campo Número de empleados no puede estar vacio");
            document.getElementById("TextNumEmpleados").focus();
            return ok = false;
        } else if (UstedEsPropie == 0) {
            alert("debe seleccionar una opcion");
            document.getElementById("cmbPropietario").focus();
            return ok = false;
        } else if (UstedEsPropie == 2) {
            if (CargoOcupa == "") {
                alert("el campo Cargo que ocupa no puede estar vacio");
                document.getElementById("TextCargoOcupa").focus();
                return ok = false;
            }
        }
        return ValidateCheckBoxEmpresa(this);

    }

    return ValidarFormularioMaxCaracteres(this);



    return ok;
}

function ValidarFormularioMaxCaracteres() {

    var Max_Length = 50, Max_Length2 = 100, Max_Length3 = 200, Max_Length4 = 1, Max_Length5 = 300, Max_Length6 = 5000, Max_Length7 = 10;
    var Nombres = $("#txtNombres").val().length;
    var Apellidos = $("#TextApellidos").val().length;
    var telefono = $("#TextTelefono").val().length;
    var DireccionReside = $("#TextDireccionReside").val().length;
    var Estrato = $("#TextEstrato").val().length;
    var Estrato2 = $("#TextEstrato").val();
    var NomProyecto = $("#TextNomProyecto").val().length;
    var ProduOfer = $("#TextProductoOferta").val().length;
    var MercadoProud = $("#TextProductoMercado").val().length;
    var NomEmpresa = $("#TextNomEmpresa").val().length;
    var DirEmpre = $("#TextDireccionEmpresa").val().length;
    var TelEmpre = $("#TextTelefonoEmpresa").val().length;
    var ProduOSerEmpre = $("#TextProductoOfertaE").val().length;
    var ActEcoEmpre = $("#TextActividadEconomica").val().length;
    var CargoOcu = $("#TextCargoOcupa").val().length;
    var ServRequ = document.getElementById("cmbServicioRequerido").value;
    var descpro = document.getElementById("TextDescPro").value;
    var numident = document.getElementById("TextNumIdentificacion").value.length;
    var numident2 = document.getElementById("TextNumIdentificacion").value;
    var semestreActua = document.getElementById("TextSemestreActual").value;
    var EstaEstudio = document.getElementById("cmbEstadoEstudio").value;
    var NumEmple = document.getElementById("TextNumEmpleados").value;
    var CantEmple = document.getElementById("TextCuantosEmpleados").value;
    var cmbemple = document.getElementById("cmbEmpleados").value;
     
    
    var ok = true;
    if (Nombres > Max_Length) {
        alert("El valor maximo de caracteres para los Nombres es de 50");
        document.getElementById("txtNombres").focus();
        return ok = false;
    } else if (Apellidos > Max_Length) {
        alert("El valor maximo de caracteres para los Apellidos es de 50");
        document.getElementById("TextApellidos").focus();
        return ok = false;
    } else if (telefono > Max_Length2) {
        alert("El valor maximo de caracteres para el telefono es de 100");
        document.getElementById("TextTelefono").focus();
        return ok = false;
    } else if (DireccionReside > Max_Length3) {
        alert("El valor maximo de caracteres para la direccion es de 200");
        document.getElementById("TextDireccionReside").focus();
        return ok = false;
    } else if (Estrato > Max_Length4) {
        alert("Debe ingresar un solo valor");
        document.getElementById("TextEstrato").focus();
        return ok = false;
    } else if (Estrato2 <= 0) {
        alert("El estrato no puede ser menor o igual a cero");
        document.getElementById("TextEstrato").focus();
        return ok = false;
    } else if (numident > Max_Length7) {
        alert("El documento de identidad no puede superar los 10 digitos");
        document.getElementById("TextNumIdentificacion").focus();
        return ok = false;
    } else if (numident2 <= 0) {
        alert("El numero de documento de identidad no puede ser menor o igual a cero");
        document.getElementById("TextNumIdentificacion").focus();
        return ok = false;
    }  else if (EstaEstudio == 'Actualmente cursando') {

        if (semestreActua <= 0) {
            alert("El Semestre actual u horas dedicadas no puede ser menor o igual a cero");
            document.getElementById("TextSemestreActual").focus();
            return ok = false;
        }

    } if (ServRequ == 3 || ServRequ == 4) {

        if (NomProyecto > Max_Length5) {
            alert("El valor maximo de caracteres para el Nombre del Proyecto es de 300");
            document.getElementById("TextNomProyecto").focus();
            return ok = false;
        } else if (descpro > Max_Length6) {
            alert("El valor maximo de caracteres para la Descripcion del Proyecto es de 5000");
            document.getElementById("TextDescPro").focus();
            return ok = false;
        } else if (ProduOfer > Max_Length6) {
            alert("El valor maximo de caracteres para el ¿Producto o servicio que oferta o proyecta ofertar?  es de 5000");
            document.getElementById("TextProductoOferta").focus();
            return ok = false;
        } else if (MercadoProud > Max_Length6) {
            alert("El valor maximo de caracteres para el campo ¿Actualmente comercializa su producto en el mercado? es de 5000");
            document.getElementById("TextProductoMercado").focus();
            return ok = false;
        } else if (cmbemple == 1) {
            if (CantEmple <= 0) {
            alert("La Cantidad de empleados no puede ser menor o igual a cero");
            document.getElementById("TextCuantosEmpleados").focus();
                return ok = false;
            }
        } 
        
    } if (ServRequ == 5) {

        if (NomEmpresa > Max_Length3) {
            alert("El valor maximo de caracteres para el nombre de la Empresa es de 200");
            document.getElementById("TextNomEmpresa").focus();
            return ok = false;
        } else if (DirEmpre > Max_Length3) {
            alert("El valor maximo de caracteres para  la Direccion de la Empresa es de 200");
            document.getElementById("TextDireccionEmpresa").focus();
            return ok = false;
        } else if (TelEmpre > Max_Length2) {
            alert("El valor maximo de caracteres para el  Teléfono de la Empresa es de 100");
            document.getElementById("TextTelefonoEmpresa").focus();
            return ok = false;
        } else if (ProduOSerEmpre > Max_Length6) {
            alert("El valor maximo de caracteres para el campo Producto o servicio que oferta es de 5000");
            document.getElementById("TextProductoOfertaE").focus();
            return ok = false;
        } else if (ActEcoEmpre > Max_Length6) {
            alert("El valor maximo de caracteres para el campo Descripción de la actividad económica que desarrolla la empresa es de 5000");
            document.getElementById("TextActividadEconomica").focus();
            return ok = false;
        } else if (CargoOcu > Max_Length2) {
            alert("El valor maximo de caracteres para el cargo que ocupa es de 100");
            document.getElementById("TextCargoOcupa").focus();
            return ok = false;
        } else if (NumEmple <= 0) {
            alert("La Cantidad de empleados no puede ser menor o igual a cero");
            document.getElementById("TextNumEmpleados").focus();
            return ok = false;
        } 
        

    }


    EnviarDatos();
    MostrarModal();
    return ok;
}


function validNumericos(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (((charCode == 8) || (charCode == 46)
        || (charCode >= 35 && charCode <= 40)
        || (charCode >= 48 && charCode <= 57)
        || (charCode >= 96 && charCode <= 105))) {
        return true;
    }
    else {
        return false;
    }
}

function MostrarModal() {

    $("#MostrarRegistro").modal("show");
};


//Funcion para el captcha
var code;
function createCaptcha() {
    //clear the contents of captcha div first 
    document.getElementById('captcha').innerHTML = "";
    var charsArray =
        "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ@!#$%^&*";
    var lengthOtp = 6;
    var captcha = [];
    for (var i = 0; i < lengthOtp; i++) {
        //below code will not allow Repetition of Characters
        var index = Math.floor(Math.random() * charsArray.length + 1); //get the next character from the array
        if (captcha.indexOf(charsArray[index]) == -1)
            captcha.push(charsArray[index]);
        else i--;
    }
    var canv = document.createElement("canvas");
    canv.id = "captcha";
    canv.width = 100;
    canv.height = 50;
    var ctx = canv.getContext("2d");
    ctx.font = "25px Georgia";
    ctx.strokeText(captcha.join(""), 0, 30);
    //storing captcha so that can validate you can save it somewhere else according to your specific requirements
    code = captcha.join("");
    document.getElementById("captcha").appendChild(canv); // adds the canvas to the body element
}
function validateCaptcha() {
    event.preventDefault();
    debugger
    if (document.getElementById("captchaTextBox").value == code) {
        procesarDatos();
           
    } else {
        alert("Por favor ingrese el Captcha Correctamente!!");
        document.getElementById("captchaTextBox").focus();
        createCaptcha();
    }
}
function procesarDatos() {
    __doPostBack('GuardarDatos');
}


