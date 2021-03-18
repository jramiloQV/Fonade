var textosAyuda = {};

function textoAyuda(obj) {
    obj = obj || {};

    if (typeof obj.texto == "undefined") {
        alert("Error llamando la funcion de ayuda");
        return;
    }
    if (typeof textosAyuda[obj.texto] != "undefined") {
        showTooltip({titulo: obj.titulo, texto: textosAyuda[obj.texto]});
        return;
    }
    var view = { texto: "TXT_" + obj.texto };

    if (!window.location.origin) {
        window.location.origin = window.location.protocol + "//" + window.location.hostname + (window.location.port ? ':' + window.location.port : '');
    }
    var url = window.location.origin + "/Default.aspx/textoAyuda"
    $.ajax({
        url: url,
        data: JSON.stringify(view),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8"
    }).done(function (mydata) {
        textosAyuda[obj.texto] = mydata.d;
        showTooltip({titulo: obj.titulo, texto:textosAyuda[obj.texto]});
    }).fail(function (error) {
        alert("Error: " + error.message);
    });

    function showTooltip(obj) {
        var canvas = $(document.createElement('div'));
        $.get((window.location.origin + "/Scripts/Templates/ayuda.tmpl.htm"),
        function (data) {
            $(data).tmpl(obj).appendTo(canvas);
            canvas.dialog({
                title: obj.titulo,
                modal: true,
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                },
                width: 500
            });
        });

       
    }
}

function SummaryFocus(top) {
    //Page_ClientValidate();
    var i;
    for (i = 0; i < Page_ValidationSummaries.length; i++) {
        if (!Page_ValidationSummaries[i].isvalid) {
            var div = document.getElementById('divParentContainer');
            div.scrollTop = top;
            break;
        }
    }
}