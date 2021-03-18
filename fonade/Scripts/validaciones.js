function verificarCaracteres(e) {
    // comprovamos con una expresion regular que el caracter pulsado sea
    // una letra, numero o un espacio
    if (e.key.match(/[a-z0-9ñÑÜüáéíóú\s]/i) === null) {
        // Si la tecla pulsada no es la correcta, eliminado la pulsación
        e.preventDefault();
    }
}