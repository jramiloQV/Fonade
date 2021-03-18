
function borrarInsumo(codProyecto, idProducto, idInsumo) {
    var view = { codProyecto: codProyecto, idProducto: idProducto, idInsumo: idInsumo };
    $.ajax({

        url: "PProyectoOperacionCompras.aspx/borrarFila",
        data: JSON.stringify(view),
        type: "POST",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (mydata) {
            alert(mydata.d);
        }
    });

}

