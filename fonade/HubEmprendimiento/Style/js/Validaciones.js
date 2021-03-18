function validateForm() {


    return ValidateCheckBoxList1(this);
    
   
}

function ValidateCheckBoxList() {

    var acepto = document.getElementById("politica").checked;
           
    if (acepto == false) {
        alert("Debe Aceptar terminos y condiciones, para poder continuar");
        document.getElementById("politica").focus();
        return ok = false;
    }
    else {
        return ok = true;
    }
    return false;
}

function ValidateCheckBoxList1() {

    var listItems = document.getElementById("ComunidadSena").getElementsByTagName("input");
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
        alert("Por favor seleccione una opcion de la comunidad SENA");
        document.getElementById("ComunidadSena").focus();
        return ok = false;
    }
    else {
        return ValidateCheckBoxList(this);
    }
    return false;
}