function MoneyFormat(Textvalue) {
    var Valortext = Textvalue.toString().split('.').length
    var FraccionInteger = "0"
    var FraccionDecimal = "00"
    if (Valortext == 1) {
        if (Textvalue.toString().split('.')[0].substr(0, 1) != "0") {
            FraccionInteger = Textvalue.toString().split('.')[0]
        }
    }
    if (Valortext == 2) {
        if (Textvalue.toString().split('.')[0].substr(0, 2) != "00" && Textvalue.toString().split('.')[0] != "") {
            FraccionInteger = Textvalue.toString().split('.')[0]
        }
        if (Textvalue.toString().split('.')[1].substr(0, 2) != "00" && Textvalue.toString().split('.')[1] != "") {
            FraccionDecimal = Textvalue.toString().split('.')[1].substr(0, 2)
        }
    }
    var longitud = FraccionInteger % 3
    var Resultado = ""
    var posInicial = 3
    var posFinal = FraccionInteger.length
    if (FraccionInteger.length > 3) {
        while (FraccionInteger.length >= 1) {
            if (posFinal < posInicial) {
                posInicial = posFinal
            }
            Resultado += FraccionInteger.substr(posFinal - posInicial, posInicial) + ','
            FraccionInteger = FraccionInteger.substring(0, posFinal - posInicial)
            posFinal = posFinal - 3
        }
        Resultado = Resultado.substr(0, Resultado.length - 1)
        var ArrInteger = Resultado.split(',')
        for (j = ArrInteger.length - 1; j >= 0; j--) {
            FraccionInteger += ArrInteger[j] + ','
        }
        FraccionInteger = FraccionInteger.substring(0, Resultado.length)
    }
    Textvalue = FraccionInteger + '.' + FraccionDecimal
    return Textvalue
}
function validarNro(e) {
    var error = document.getElementById('divError');
    var key = e.which
    switch (true) {
        case key == 44:
            error.innerHTML = 'Ingrese punto(.) como signo decimal'
            e.preventDefault();
            break
        case key == 13:
            error.innerHTML = 'Para pasar al campo siguiente presione la tecla de Tabulación'
            e.preventDefault();
            break;

        default:
            error.innerHTML = ''
            break;
    }
}
function ControlaSubmit(e) {
    var error = document.getElementById('divError');
    var key = e.which
    switch (true) {
        case key == 13:
            error.innerHTML = 'Para pasar al campo siguiente presione la tecla de Tabulación'
            e.preventDefault();
            break;
        default:
            error.innerHTML = ''
            break;
    }
}
function OpenPage(strPage) {
    var ActivarVentana = document.getElementById("hidInsumo")
    if (ActivarVentana.value == "1") {
        window.open(strPage, 'Insumo', 'status:false;dialogWidth:900px;dialogHeight:700px')
        ActivarVentana.value = 0
    }
}
