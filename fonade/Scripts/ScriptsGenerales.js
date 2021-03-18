
//Control de accion en las pestañas de la opcion Ver Proyecto
var pestanaActual = "";
function CargarPestana(nombreFrame, pagina) {
    if (pestanaActual != "") {
        document.getElementById(pestanaActual).src = "";
    }

    document.getElementById(nombreFrame).src = pagina;
    pestanaActual = nombreFrame;
}

function LoadIFrame(nombreFrame, pagina) {    
    if (pestanaActual != "") {
        document.getElementById(pestanaActual).src = "";
    }

    document.getElementById(nombreFrame).src = pagina + "?codproyecto=" + getUrlVars()["codproyecto"];
    pestanaActual = nombreFrame;
}

function LoadIFrameAspectos(nombreFrame, pagina, codigoAspecto) {
    if (pestanaActual != "") {
        document.getElementById(pestanaActual).src = "";
    }

    document.getElementById(nombreFrame).src = pagina + "?codproyecto=" + getUrlVars()["codproyecto"] + "&codaspecto=" + codigoAspecto;
    pestanaActual = nombreFrame;
}

function Confirmacion(texto) {
    if (!confirm(texto))
        return false;
}

function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

/// Datapicker que nos sirve para mostrar un calendar 
// Creado por Diego Quiñonez
// 22-02-2014

function load_datepicker() { // f3l added this so scripts load in the correct order.

    (function ($) {

        $.fn.Datepicker = function (id, formato) {

            $(id).datepicker({
                dateFormat: formato,
                showOn: "button",
                buttonImage: "../../Images/calendar.png",
                changeYear: true,
                changeMonth: true,
                buttonImageOnly: true
            });
        };

    })(jQuery);

    (function ($) {

        $.fn.showModalDialog = function (url, width, height, resizable, scroll, center) {

            showModalDialog(url, "dialogWidth:" + width + "px;dialogHeight:" + height
              + "px;resizable:" + resizable + ";scroll:" + scroll + ";center:" + center);
        };

    })(jQuery);

    (function ($) {

        $.fn.windowopen = function (url, width, height, resizable, scroll, center) {

            window.open(url, '_blank', 'width=' + width + ',height=' + height + ',toolbar=no, scrollbars=' + scroll + ',resizable=no,center=yes;');
        };

    })(jQuery);


    /// Validamos los pendientes que tiene el proyecto para acreditar //
    (function ($) {

        $.fn.windowopen = function (url, width, height, resizable, scroll, center) {

            window.open(url, '_blank', 'width=' + width + ',height=' + height + ',toolbar=no, scrollbars=' + scroll + ',resizable=no,center=yes;');
        };

    })(jQuery);

    (function ($) {

        $.fn.validarCheckbox = function (nombre) {

            var lMensaje;
            var lResult;

            var lPendiente = $(".chkPendiente").is(":checked");
            var lSubSanado = $(".chkSubsanado").is(":checked");
            var lAcreditado = $(".chkAcreditado").is(":checked");
            var lNoAcreditado = $(".chkNoAcreditado").is(":checked");
            var lAnexo1 = $(".ch1").is(":checked");
            var lAnexo2 = $(".chd2").is(":checked");
            var lAnexo3 = $(".ch3").is(":checked");
            var lDI = $(".chdi").is(":checked");


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

}

function bindReady(handler) {
    var called = false
    function ready() {
        if (called) return
        called = true
        handler()
    }
    if (document.addEventListener) {
        document.addEventListener("DOMContentLoaded", function () {
            ready()
        }, false)
    } else if (document.attachEvent) {
        if (document.documentElement.doScroll && window == window.top) {
            function tryScroll() {
                if (called) return
                if (!document.body) return
                try {
                    document.documentElement.doScroll("left")
                    ready()
                } catch (e) {
                    setTimeout(tryScroll, 0)
                }
            }
            tryScroll()
        }
        document.attachEvent("onreadystatechange", function () {
            if (document.readyState === "complete") {
                ready()
            }
        })
    }
    if (window.addEventListener)
        window.addEventListener('load', ready, false)
    else if (window.attachEvent)
        window.attachEvent('onload', ready)
    /*  else  // use this 'else' statement for very old browsers :)
        window.onload=ready
    */
}
readyList = []
function onReady(handler) {
    if (!readyList.length) {
        bindReady(function () {
            for (var i = 0; i < readyList.length; i++) {
                readyList[i]()
            }
        })
    }
    readyList.push(handler)
}


onReady(load_datepicker); //f3l load in order (after jquery loads)

//Establece la posición del scroll
function SetScroll(h, v) {
    window.scrollTo(h, v);
}


//  /*/






