(function ($) {

    $.fn.validarCheckbox = function(nombre) {

        var lMensaje;
        var lResult;

        var lPendiente = $(".chkPendiente").is(":checked");
        var lSubSanado = $(".chkSubsanado").is(":checked");
        var lAcreditado = $(".chkAcreditado").is(":checked");
        var lNoAcreditado = $(".chkNoAcreditado").is(":checked");
        var lAnexo1 = document.$(".chkAnexo1_").is(":checked");
        var lAnexo2 = document.$(".chkAnexo2_").is(":checked");
        var lAnexo3 = document.$(".chkAnexo3_").is(":checked");
        var lDI = $(".chkDI_").is(":checked");


        switch (nombre) {
        case "Subsanado":
            if (lNoAcreditado) {
                lMensaje = "No puede activar el estado 'Subsanado' si el estado del emprendedor es  'No Acreditado'";
                lResult = false;
            }
            break;
        case "Acreditado":
            if (lNoAcreditado) {
                lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'No Acreditado'";
                lResult = false;
            } else if (lPendiente && !lSubSanado) {
                lMensaje = "No puede activar el estado 'Acreditado' si el estado del emprendedor es 'Pendiente'";
                lResult = false;
            } else if (!lAnexo1 || !lAnexo2 || !lAnexo3 || !lDI) {
                lMensaje = "No puede activar el estado 'Acreditado' si los anexos no están completos (1,2,3 y DI)";
                lResult = false;
            }
            break;
        case "NoAcreditado":
            if (lAcreditado) {
                lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Acreditado'";
                lResult = false;
            } else if (lSubSanado) {
                lMensaje = "No puede activar el estado 'No Acreditado' si el estado del emprendedor es 'Subsanado'";
                lResult = false;
            }
            break;
        case "Pendiente":
            if (lAcreditado) {
                lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'Acreditado'";
                lResult = false;
            } else if (lNoAcreditado) {
                lMensaje = "No puede activar el estado 'Pendiente' si el estado del emprendedor es 'No Acreditado'";
                lResult = false;
            }
            break;
        }

        if (!lResult && lMensaje != "")
            alert(lMensaje);


        return lResult;
    };
})(jQuery);


