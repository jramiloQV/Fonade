function imprimir(controlI) {
    var divToPrint = document.getElementById(controlI);
    var newWin = window.open('', 'Print-Window', 'width=1000,height=500');
    newWin.document.open();
    newWin.document.write('<html><body onload="window.print()">' + divToPrint.innerHTML + '</body></html>');
    newWin.document.close();
    setTimeout(function () { newWin.close(); }, 1000);
}