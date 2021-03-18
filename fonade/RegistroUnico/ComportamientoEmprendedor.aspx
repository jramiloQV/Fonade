<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComportamientoEmprendedor.aspx.cs"
    Inherits="Fonade.RegistroUnico.ComportamientoEmprendedor" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Registro Comportamiento Emprendedor</title>

    <script>
        (function (i, s, o, g, r, a, m) {
            i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {
                (i[r].q = i[r].q || []).push(arguments)
            }, i[r].l = 1 * new Date(); a = s.createElement(o),
                m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)
        })(window, document, 'script', 'https://www.google-analytics.com/analytics.js', 'ga');

        ga('create', 'UA-85402368-1', 'auto');
        ga('send', 'pageview');
    </script>
    <!-- Facebook Pixel Code -->
    <script>
        !function (f, b, e, v, n, t, s) {
            if (f.fbq) return; n = f.fbq = function () {
                n.callMethod ?
                    n.callMethod.apply(n, arguments) : n.queue.push(arguments)
            }; if (!f._fbq) f._fbq = n;
            n.push = n; n.loaded = !0; n.version = '2.0'; n.queue = []; t = b.createElement(e); t.async = !0;
            t.src = v; s = b.getElementsByTagName(e)[0]; s.parentNode.insertBefore(t, s)
        }(window,
            document, 'script', 'https://connect.facebook.net/en_US/fbevents.js');
        fbq('init', '541324449399250', {
            em: 'insert_email_variable,'
        });
        fbq('track', 'PageView');
    </script>

    <noscript><img height="1" width="1" style="display:none"
        src="https://www.facebook.com/tr?id=541324449399250&ev=PageView&noscript=1"/>
    </noscript>

    <script src="Styles/js/jquery-3.4.1.min.js"></script>
    <link href="Styles/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Styles/js/bootstrap.min.js"></script>
    <script src="Styles/js/funciones.js"></script>

    <style>
        /*Estilo utilizados para ellipse Captcha*/
        .captchaTextBox {
            padding: 12px 20px;
            display: inline-block;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        canvas {
            /*prevent interaction with the canvas*/
            pointer-events: none;
        }
    </style>

    <style>
        .AnchoCompleto {
            width: 100%;
        }

        label {
            width: 100%;
        }

        input {
            width: 100%;
        }

        select {
            width: 100%;
        }

        #cblMercadoOferForta label {
            width: 10%;
        }

        #cblMercadoOferForta input {
            width: 10%;
        }

        #cblMercadoOferForta td {
            padding-bottom: 0px;
            padding-top: 0px;
        }

        #cblMercadoOfer label {
            width: 10%;
        }

        #cblMercadoOfer input {
            width: 10%;
        }

        #cblMercadoOfer td {
            padding-bottom: 0px;
            padding-top: 0px;
        }

        body {
            font-family: "Poppins", sans-serif;
        }
    </style>

    <style>
        .toolTip {
            background: rgba(20,20,20,0.9) url('img/info.gif') center left 5px no-repeat;
            border: 2px solid #87cefa;
            border-radius: 5px;
            box-shadow: 5px 5px 5px #333;
            color: #87cefa;
            font-size: 0.8em;
            padding: 10px 10px 10px 35px;
            max-width: 600px;
            position: absolute;
            z-index: 100;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server" onsubmit="return validarFormulario(this)">
        <div class="container">

            <!--Formulario Principal-->
            <div style="text-align: center">
                <div style="text-align: center">
                    <h3>Comportamiento Emprendedor</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Nombres:</label>
                    <asp:TextBox ID="txtNombres" runat="server"
                        CssClass="form-control-sm" type="text" required></asp:TextBox>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Apellidos:</label>
                    <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control-sm" type="text" required></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Identificacion:</label>
                    <asp:DropDownList runat="server" ID="cmbTipoIdentificacion"
                        CssClass="form-control-sm" nombre="Tipo Identificacion" AppendDataBoundItems="true" DataSourceID="dataTipoDocumento" AutoPostBack="false" DataValueField="Id" DataTextField="Nombre">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>No.</label>
                    <asp:TextBox ID="txtIdentificacion" runat="server" MaxLength="10" CssClass="form-control-sm"
                        type="number" onkeydown="return validNumericos(event)" required></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Telefono :</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control-sm" type="number" required></asp:TextBox>

                </div>
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Correo Electronico :</label>
                    <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control-sm" type="email" required></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Departamento:</label>
                    <asp:DropDownList ID="cmbDepartamentoExpedicion" CssClass="form-control-sm"
                        runat="server" nombre="Departamento" />
                </div>
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Ciudad:</label>
                    <asp:DropDownList ID="cmbCiudadExpedicion" CssClass="form-control-sm" runat="server" nombre="Ciudad" />
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-12 col-md-6 col-lg-6">
                    <label>¿Le gustaría participar de las Charlas "Hablemos de"? :</label>
                    <select name="ParticiparCharlas" id="cmbParticiparCharlas" runat="server" class="form-control-sm">
                        <option value="SI">SI</option>
                        <option value="NO">NO</option>
                    </select>
                </div>

                <div class="col col-sm-12 col-md-6 col-lg-6">
                    <label>¿A cuál(es) de los servicios de emprendimiento del SENA le gustaría acceder?</label>
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" value="" id="chkOrientacion" runat="server"
                            style="width: 7%;">
                        <label class="form-check-label" for="defaultCheck1">
                            i.	Orientación
                        </label>

                        <input class="form-check-input" type="checkbox" value="" id="chkAsesoriaCreacionEmpresa" runat="server"
                            style="width: 7%;">
                        <label class="form-check-label" for="defaultCheck1">
                            ii.	Asesoría para la creación de empresa
                        </label>

                        <input class="form-check-input" type="checkbox" value="" id="chkAsesoriaFormulacionPlan" runat="server"
                            style="width: 7%;">
                        <label class="form-check-label" for="defaultCheck1">
                            iii.	Asesoría para la formulación del plan de negocios a Convocatorias Fondo Emprender
                        </label>

                        <input class="form-check-input" type="checkbox" value="" id="chkFortalecimientoEmpresarial" runat="server"
                            style="width: 7%;">
                        <label class="form-check-label" for="defaultCheck1">
                            iv.	Fortalecimiento empresarial 
                        </label>
                    </div>
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-3 col-lg-3" style="text-align: center">
                </div>
                <div class="col-md-6 col-lg-6" style="text-align: center">
                    <asp:Button ID="btnRegistrar" runat="server" Text="Registrar y Descargar"
                        OnClick="btnRegistrar_Click"
                        CssClass="btn btn-primary"></asp:Button>

                </div>
                <div class="col-md-3 col-lg-3" style="text-align: center">
                </div>
            </div>

        </div>

        <asp:TextBox ID="CodCiudadExpedicion" runat="server" name="Cod Ciudad Expedicion" type="hidden" value="xm234jq"></asp:TextBox>

    </form>

    <asp:ObjectDataSource
        ID="dataTipoDocumento"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getTipoIdentificacion"></asp:ObjectDataSource>



</body>
</html>

<%--Carga Departamento y Ciudad--%>
<script type="text/javascript">

    $().ready(function () {
        cargarCiudades();
        document.getElementById("CodCiudadExpedicion").value = $("#<%=cmbCiudadExpedicion.ClientID%> :selected").val();
    });

    $("#<%=cmbDepartamentoExpedicion.ClientID%>").change(function () {
        cargarCiudades();
    });
    $("#<%=cmbCiudadExpedicion.ClientID%>").change(function () {
        // alert("Ha seleccionado: " + $("#<%=cmbCiudadExpedicion.ClientID%> :selected").val());
        document.getElementById("CodCiudadExpedicion").value = $(this).val();
    });

    function cargarCiudades() {
        var params = new Object();
        params.CodDepartamento = $("#<%=cmbDepartamentoExpedicion.ClientID%>").val();
        params = JSON.stringify(params);
        $.ajax({
            type: "POST",
            url: "http://www.fondoemprender.com:8080/RegistroUnico/ComportamientoEmprendedor.aspx/llenarciudad",
            data: params,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: LoadCiudadesExpedicion,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(textStatus + ": " + XMLHttpRequest.responseText);
            }
        });
    }

    function LoadCiudadesExpedicion(result) {

        $("#<%=cmbCiudadExpedicion.ClientID%>").html("");
        //$("#<%=cmbCiudadExpedicion.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbCiudadExpedicion.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }

    function validarFormulario(f) {
        var validado = true;
               
        emailRegex = /^[-\w.%+]{1,64}@(?:[A-Z0-9-]{1,63}\.){1,125}[A-Z]{2,63}$/i;

        numeros = /^([0-9])*$/;


        if (!(emailRegex.test(f.txtCorreo.value))) {
            f.txtCorreo.focus();
            mostrarTooltip(f.txtCorreo, 'Escriba una dirección de correo electronico válida.');
            validado = false;
        }

        if (!(numeros.test(f.txtIdentificacion.value))) {
            f.txtIdentificacion.focus();
            mostrarTooltip(f.txtIdentificacion, 'Escriba un numero de identificación válido.');
            validado = false;
        }

        if (!(numeros.test(f.txtTelefono.value))) {
            f.txtTelefono.focus();
            mostrarTooltip(f.txtTelefono, 'Escriba un numero de telefono válido.');
            validado = false;
        }

        if (f.txtTelefono.value.length > 10 || f.txtTelefono.value.length < 7) {
            f.txtTelefono.focus();
            mostrarTooltip(f.txtTelefono, 'Escriba un numero de telefono válido.');
            validado = false;
        }

        if (f.txtNombres.value.length < 1) {
            f.txtNombres.focus();
            mostrarTooltip(f.txtNombres, 'Escriba un nombre válido.');
            validado = false;
        }

        if (f.txtApellidos.value.length < 1) {
            f.txtApellidos.focus();
            mostrarTooltip(f.txtApellidos, 'Escriba un nombre válido.');
            validado = false;
        }

        if (f.txtNombres.value == "" || f.txtApellidos.value == "" || f.txtIdentificacion.value == ""
            || f.txtTelefono.value == "" || f.txtCorreo.value == "") {
            validado = false;
        }
        else {
            $('#myModal').modal('hide');
        }

        return validado;
    }

    function mostrarTooltip(elemento, mensaje) {

        // Si no existe aun el tooltip se crea
        if (!document.getElementById(elemento.id + "tp")) {
            // Dimensiones del elemento al que se quiere añadir el tooltip
            anchoElemento = $('#' + elemento.id).width();
            altoElemento = $('#' + elemento.id).height();

            // Coordenadas del elemento al que se quiere añadir el tooltip
            coordenadaXElemento = $('#' + elemento.id).position().left;
            coordenadaYElemento = $('#' + elemento.id).position().top;

            // Coordenadas en las que se colocara el tooltip
            x = coordenadaXElemento + anchoElemento / 2 + 20;
            y = coordenadaYElemento + altoElemento / 2 + 10;

            // Crea el tooltip con sus atributos
            var tooltip = document.createElement('div');
            tooltip.id = elemento.id + "tp";
            tooltip.className = 'toolTip';
            tooltip.innerHTML = mensaje;
            tooltip.style.left = x + "px";
            tooltip.style.top = y + "px";

            // Añade el tooltip
            document.body.appendChild(tooltip);
        }

        // Cambia la opacidad del tooltip y lo muestra o lo oculta (Si el raton esta encima del elemento se muestra el tooltip y sino se oculta)
        $('#' + elemento.id).hover(
            function () {
                tooltip.style.display = "block";
                $('#' + tooltip.id).animate({ "opacity": 1 });

            },
            function () {
                $('#' + tooltip.id).animate({ "opacity": 0 });
                setTimeout(
                    function () {
                        tooltip.style.display = "none";
                    },
                    400
                );

            }
        );
    }
</script>

