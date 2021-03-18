$(document).ready(function () {
    
    //
    // Estilos para paginador absoluto
    //

    $("<style type='text/css'> .tablaPaginador{ position: absolute; bottom: 26px; } .contenedorTablaPaginador{ height: 14px; }</style>").appendTo("head");
    $(".Paginador td table").addClass("tablaPaginador");
    $(".Paginador td").addClass("contenedorTablaPaginador");

    //
    // Eventos de validación de longitud de textarea e inputs
    //

    //Maneja el evento de presiona sobre el input o textarea que contenga la clase validarLargo
    $('.validarLargo').keypress(function (e) {
        var tval = $(this).val(),
            tlength = tval.length,
            set = 1000,
            remain = parseInt(set - tlength);
        if (remain <= 0 && e.which !== 0 && e.charCode !== 0) {
            alert("No puede ingresar más de 1.000 de caracteres");
            $(this).val((tval).substring(0, tlength - 1));
        }
    });

    //Maneja el evento de copiar texto sobre el input o textarea que contenga la clase validarLargo
    $('.validarLargo').bind('paste', function () {
        var base = $(this);
        var baseTexto = $(this).val();
        var baseLargo = $(this).val().length;

        //alert('Largo de base ' + baseLargo);

        var nuevo;
        var nueveLargo;

        function textoCopiadoMuyLargo()
        {
            base.val(baseTexto);
            alert("No puede ingresar más de 1.000 de caracteres");
        }

        setTimeout(function () { 
            var nuevoLargo = base.val().length;
            //alert('el nuevo largo es ' + nuevoLargo);
            if (nuevoLargo > 1000)
            {
                textoCopiadoMuyLargo()

            }
            var diferencia = nuevoLargo - baseLargo;
            //alert('la diferencia de largo es' + diferencia);

            if (diferencia > 1000)
            {
                textoCopiadoMuyLargo()
            }

        }, 0);
    });
});
