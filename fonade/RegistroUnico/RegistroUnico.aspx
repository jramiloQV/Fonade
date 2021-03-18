<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroUnico.aspx.cs"
    Inherits="Fonade.RegistroUnico.RegistroUnico" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">


<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Registro Fondo Emprender</title>

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
    src="https://www.facebook.com/tr?id=541324449399250&ev=PageView&noscript=1"
    /></noscript>
    <!-- DO NOT MODIFY -->
    <!-- End Facebook Pixel Code -->

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
</head>


<body>



    <div style="text-align: center">
        <h3>SOLICITUD DE INFORMACIÓN SERVICIOS EMPRENDIMIENTO SENA</h3>
    </div>
    <hr />
    <form id="form1" runat="server">

        <div class="container">


            <!--Formulario Principal-->
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Nombres:</label>
                    <asp:TextBox ID="txtNombres" runat="server"
                        CssClass="form-control-sm" type="text"></asp:TextBox>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Apellidos:</label>
                    <asp:TextBox ID="TextApellidos" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>Identificacion:</label>
                    <asp:DropDownList runat="server" ID="cmbTipoIdentificacion" CssClass="form-control-sm" nombre="Tipo Identificacion" AppendDataBoundItems="true" DataSourceID="dataTipoDocumento" AutoPostBack="false" DataValueField="Id" DataTextField="Nombre">
                        <asp:ListItem Text="Seleccione.." Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-12 col-md-6 col-lg-6">
                    <label>No.</label>
                    <asp:TextBox ID="TextNumIdentificacion" runat="server" MaxLength="10" CssClass="form-control-sm" type="number" onkeydown="return validNumericos(event)"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Departamento Expedición:</label>
                    <asp:DropDownList ID="cmbDepartamentoExpedicion" CssClass="form-control-sm"
                        runat="server" nombre="Departamento Expedicion" />
                </div>
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Ciudad Expedición:</label>
                    <asp:DropDownList ID="cmbCiudadExpedicion" CssClass="form-control-sm" runat="server" nombre="Ciudad expedición" />
                </div>

            </div>
            <div class="row">
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Fecha Nacimiento:</label>
                    <asp:TextBox ID="Textfechanac" CssClass="form-control-sm" type="date" runat="server" autocomplete="off"
                        pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"> 
                    </asp:TextBox>
                </div>
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Departamento Nacimiento:</label>
                    <asp:DropDownList ID="cmbDepartamentoNacimiento" CssClass="form-control-sm"
                        runat="server" nombre="Departamento nacimiento" />
                </div>
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Ciudad Nacimiento:</label>
                    <asp:DropDownList ID="cmbCiudadNacimiento" CssClass="form-control-sm" runat="server" nombre="Ciudad nacimiento" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Género:</label>
                    <asp:DropDownList runat="server" ID="cmbGenero" nombre="Genero" CssClass="form-control-sm">
                        <asp:ListItem Text="Seleccione" Value="" />
                        <asp:ListItem Text="Masculino" Value="M" />
                        <asp:ListItem Text="Femenino" Value="F" />
                    </asp:DropDownList>

                </div>
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Estado civil:</label>
                    <asp:DropDownList ID="cmbEstadoCivil" CssClass="form-control-sm" runat="server" AppendDataBoundItems="true" nombre="Estado Civil" DataSourceID="dataEstadoCivil" AutoPostBack="false" DataValueField="Id_EstadoCivil" DataTextField="NomEstadoCivil">
                        <asp:ListItem Text="Seleccione.." Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Ocupación :</label>
                    <asp:DropDownList ID="cmbOcupacion" CssClass="form-control-sm" runat="server"
                        nombre="Ocupación" AppendDataBoundItems="true" DataSourceID="dataOcupacion"
                        AutoPostBack="false" DataValueField="Id_Ocupacion" DataTextField="NombreOcupacion">
                        <asp:ListItem Text="Seleccione.." Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Telefono :</label>
                    <asp:TextBox ID="TextTelefono" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>

                </div>
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Correo Electronico :</label>
                    <asp:TextBox ID="TextCorreo" runat="server" CssClass="form-control-sm" type="email"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Departamento de Residencia:</label>
                    <asp:DropDownList ID="cmbDepartamentoReside" CssClass="form-control-sm" runat="server"
                        nombre="Departamento Reside" />
                </div>
                <div class="col col-sm-6 col-md-6 col-lg-6">
                    <label>Municipio de Residencia :</label>
                    <asp:DropDownList ID="cmbMunicipioReside" CssClass="form-control-sm" runat="server" nombre="Ciudad Reside" />
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-6 col-md-6 col-lg-4">
                    <label>Direccion de Residencia:</label>
                    <asp:TextBox ID="TextDireccionReside" runat="server" CssClass="form-control-sm" type="text"> </asp:TextBox>
                </div>
                <div class="col col-sm-6 col-md-6 col-lg-4">
                    <label>Estrato :</label>
                    <asp:TextBox ID="TextEstrato" runat="server" CssClass="form-control-sm" type="number" onkeydown="return validNumericos(event)"> </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-5 col-md-4 col-lg-4">
                    <label>Nivel Estudio:</label>
                    <asp:DropDownList ID="cmbNivelEstudio" CssClass="form-control-sm" runat="server" nombre="Nivel Estudio" />
                </div>
                <div class="col-sm-5 col-md-4 col-lg-4">
                    <label>Filtrar Programa:</label>
                    <asp:TextBox ID="txtSearch" runat="server" onkeyup="FilterItems(this.value)" CssClass="form-control-sm"></asp:TextBox>
                </div>
                <div class="col-sm-2 col-md-4 col-lg-4">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </div>
            </div>

            <div class="row">
                <div class="col col-sm-12 col-md-12 col-lg-12">
                    <label>Programa:</label>
                    <asp:DropDownList ID="cmbProgramaAcademico" CssClass="form-control-sm"
                        AppendDataBoundItems="true" runat="server" nombre="Programa Academico" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Institucion </label>
                    <asp:DropDownList ID="cmbInstitucion" CssClass="form-control-sm" runat="server" AppendDataBoundItems="true" nombre="Institucion Academica" />
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Ciudad Institucion </label>
                    <asp:DropDownList ID="cmbInstitucionCiudad" CssClass="form-control-sm" runat="server" nombre="Institucion Ciudad" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Estado </label>
                    <asp:DropDownList runat="server" ID="cmbEstadoEstudio" nombre="Estudio" CssClass="form-control-sm" AutoPostBack="false">
                        <asp:ListItem Text="Seleccione" Value="0" />
                        <asp:ListItem Text="Actualmente cursando" Value="Actualmente cursando" />
                        <asp:ListItem Text="Finalizado" Value="Finalizado" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-12 col-md-4 col-lg-4">
                    <label>Fecha Inicio </label>
                    <asp:TextBox ID="txtfechaini" CssClass="form-control-sm" type="date" runat="server" autocomplete="off"
                        pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"> 
                    </asp:TextBox>
                </div>
                <div id="DivSemestreActual" style="visibility: hidden" class="col-sm-12 col-md-4 col-lg-4">
                    <label>Semestre actual u horas dedicadas </label>
                    <div id="TxtSemestreActual">
                        <asp:TextBox ID="TextSemestreActual" runat="server" CssClass="form-control-sm" type="number" onkeydown="return validNumericos(event)"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="DivFinalizacion" style="display: none;" class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Fecha Finalización </label>
                    <asp:TextBox ID="Textfechafin" CssClass="form-control-sm" type="date" runat="server" autocomplete="off"
                        pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"> 
                    </asp:TextBox>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Fecha Graduacion </label>
                    <asp:TextBox ID="Textfechagraducacion" CssClass="form-control-sm" type="date" runat="server" autocomplete="off"
                        pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"> 
                    </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col col-sm-12 col-md-12 col-lg-12">
                    <label>Tipo Aprendiz:</label>
                    <asp:DropDownList ID="cmbTipoAprendiz" CssClass="form-control-sm" runat="server" nombre="Tipo Aprendiz"
                        AppendDataBoundItems="true" DataSourceID="DataTipoAprendiz" AutoPostBack="false" DataValueField="Id_TipoAprendiz"
                        DataTextField="NomTipoAprendiz">
                        <asp:ListItem Text="Seleccione" Value="0" />
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Departamento Centro Desarrollo Empresarial </label>
                    <asp:DropDownList ID="cmbDepartamentoEmpresarial" CssClass="form-control-sm" runat="server" nombre="Departamento Centro" />
                </div>
                <div class="col-sm-6 col-md-6 col-lg-6">
                    <label>Ciudad Centro Desarrollo Empresarial </label>
                    <asp:DropDownList ID="cmbCiudadEmpresarial" CssClass="form-control-sm" runat="server" nombre="CIudad Centro" />
                </div>
                <div class="col-sm-12 col-md-12 col-lg-12">
                    <label>Centro Desarrollo Empresarial </label>
                    <asp:DropDownList ID="cmbCentroEmpresarial" CssClass="form-control-sm" runat="server" nombre="Centro Empresarial" />
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12 col-md-12 col-lg-2">
                </div>
                <div class="col-sm-12 col-md-12 col-lg-8">
                    <label class="align-content-center">Servicio acerca del cual desea mayor información</label>
                    <asp:DropDownList ID="cmbServicioRequerido" CssClass="form-control-sm" runat="server" nombre="Servicio Requerido" AppendDataBoundItems="true" DataSourceID="DataServicioRequerido" AutoPostBack="false" DataValueField="Id_Servicio" DataTextField="NombreServicio">
                        <asp:ListItem Text="Seleccione" Value="0" />
                    </asp:DropDownList>
                </div>
                <div class="col-sm-12 col-md-12 col-lg-2">
                </div>

            </div>

            <!--FIN Formulario Principal-->

            <!--Formulario Sena Rural-->

            <div id="SenaRural" style="display: none">

                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>Formacion en la que esta interesado </label>
                        <asp:DropDownList ID="cmbFormacion" CssClass="form-control-sm" runat="server" nombre="Formacion" />
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Departamento de Formacion </label>
                        <asp:DropDownList ID="cmbDepartamentoFormacion" CssClass="form-control-sm" runat="server" nombre="Departamento De Formacion" />
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Ciudad de Formacion </label>
                        <asp:DropDownList ID="cmbCiudadFormacion" CssClass="form-control-sm" runat="server" nombre="Ciudad De Formacion" AutoPostBack="false" DataValueField="Id_CiudadFormacion" DataTextField="NomCiudadFormacion" />
                    </div>
                </div>


            </div>

            <!--FIN Formulario Sena Rural-->

            <!--Formulario EMPRENDIMIENTO y FONDO EMPRENDER-->

            <div id="EmprendimientoFondoEmprender" style="display: none">

                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Departamento donde va desarrollar el Proyecto</label>
                        <asp:DropDownList ID="cmbDepartamentoDesarrollarProyecto" CssClass="form-control-sm" runat="server" nombre="Departamento a Desarrollar Proyecto" />
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Ciudad donde va desarrollar el Proyecto </label>
                        <asp:DropDownList ID="cmbCiudadDesarrollarProyecto" CssClass="form-control-sm" runat="server" nombre="CIudad a Desarrollar Proyecto" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Sector </label>
                        <asp:DropDownList ID="cmbSector" CssClass="form-control-sm" runat="server"
                            nombre="Sector" />
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>SubSector </label>
                        <asp:DropDownList ID="cmbSubSector" CssClass="form-control-sm"
                            runat="server" nombre="SubSector" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Nombre Del Proyecto </label>
                        <asp:TextBox ID="TextNomProyecto" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                    </div>
                    <div class="col-sm-6 col-md-6 col-lg-6">
                        <label>Mercado en el que vende o proyecta vender su producto o servicio:</label>
                        <asp:CheckBoxList ID="cblMercadoOfer"
                            runat="server">
                        </asp:CheckBoxList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>Descripcion del Proyecto (Max. 5000 Caracteres)</label>
                        <asp:TextBox ID="TextDescPro" TextMode="multiline" runat="server" CssClass="form-control" type="text" Columns="50" Rows="5"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>¿Producto o servicio que oferta o proyecta ofertar? (Max. 5000 Caracteres)</label>
                        <asp:TextBox ID="TextProductoOferta" TextMode="multiline" runat="server" CssClass="form-control" type="text" Columns="50" Rows="5"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>¿Actualmente comercializa su producto en el mercado? (Max. 5000 Caracteres)</label>
                        <asp:TextBox ID="TextProductoMercado" TextMode="multiline" runat="server" CssClass="form-control" type="text" Columns="50" Rows="5"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-md-4 col-lg-4">
                        <label>¿Tiene empleados a su cargo?</label>
                        <asp:DropDownList runat="server" ID="cmbEmpleados" nombre="Empleados a cargo" CssClass="form-control-sm">
                            <asp:ListItem Text="Seleccione" Value="0" />
                            <asp:ListItem Text="Si" Value="1" />
                            <asp:ListItem Text="No" Value="2" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-6 col-md-4 col-lg-4">
                        <div id="divempleadosacargo" style="visibility: hidden">
                            <label>Cuantos </label>
                        </div>
                        <div id="divempleadosacargotxt" style="visibility: hidden">
                            <asp:TextBox ID="TextCuantosEmpleados" runat="server" CssClass="form-control-sm" type="number" onkeydown="return validNumericos(event)"></asp:TextBox>
                        </div>
                    </div>

                </div>


            </div>

            <!--FIN Formulario EMPRENDIMIENTO y FONDO EMPRENDER-->

            <!--Formulario FORTALECIMIENTO-->

            <div id="fortalecimiento" style="display: none">
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Nombre de la Empresa </label>
                        <asp:TextBox ID="TextNomEmpresa" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Departamento donde se encuentra su empresa </label>
                        <asp:DropDownList ID="cmbUbicacionEmpresaD" CssClass="form-control-sm" runat="server" nombre="Departamento Ubicacion Empresa" />
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Ciudad donde se encuentra su empresa </label>
                        <asp:DropDownList ID="cmbUbicacionEmpresaC" CssClass="form-control-sm" runat="server" nombre="CIudad Ubicacion Empresa" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>Sector </label>
                        <asp:DropDownList ID="cmbSectorEmpresa" CssClass="form-control-sm"
                            runat="server" nombre="Sector Empresa" />
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>SubSector </label>
                        <asp:DropDownList ID="cmbSubSectorEmpresa"
                            CssClass="form-control-sm" runat="server" nombre="SubSector Empresa" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Direccion de la Empresa </label>
                        <asp:TextBox ID="TextDireccionEmpresa" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Correo de la Empresa </label>
                        <asp:TextBox ID="TextCorreoEmpresa" runat="server" CssClass="form-control-sm" type="email"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Teléfono de la Empresa </label>
                        <asp:TextBox ID="TextTelefonoEmpresa" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Fecha Constitucion:</label>
                        <asp:TextBox ID="TextFechaEmpresa" CssClass="form-control-sm" type="date" runat="server" autocomplete="off"
                            pattern="(0[1-9]|1[0-9]|2[0-9]|3[01])/(0[1-9]|1[012])/[0-9]{4}"> 
                        </asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Tamaño de la empresa</label>
                        <asp:DropDownList runat="server" ID="cmbTamaEmpresa" nombre="Tamaño de la empresa" CssClass="form-control-sm">
                            <asp:ListItem Text="Seleccione" Value="0" />
                            <asp:ListItem Text="Microempresa " Value="Microempresa" />
                            <asp:ListItem Text="Pequeña empresa" Value="Pequeña empresa" />
                            <asp:ListItem Text="Mediana empresa" Value="Mediana empresa" />
                            <asp:ListItem Text="Gran empresa" Value="Gran empresa" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-12 col-md-4 col-lg-4">
                        <label>Mercado en el que vende su producto o servicio</label>
                        <asp:CheckBoxList ID="cblMercadoOferForta" runat="server">
                        </asp:CheckBoxList>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>Producto o servicio que oferta (Max. 5000 Caracteres)</label>
                        <asp:TextBox ID="TextProductoOfertaE" TextMode="multiline" runat="server" CssClass="form-control" type="text" Columns="50" Rows="5"></asp:TextBox>
                    </div>
                    <div class="col-sm-12 col-md-12 col-lg-12">
                        <label>Descripción de la actividad económica que desarrolla la empresa(Max. 5000 Caracteres)</label>
                        <asp:TextBox ID="TextActividadEconomica" TextMode="multiline" runat="server" CssClass="form-control" type="text" Columns="50" Rows="5"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-6 col-md-3 col-lg-3">
                        <label>Valor de las ventas anuales ($):</label>
                        <asp:TextBox ID="TextVentasAnuales" runat="server" CssClass="form-control-sm" type="number"></asp:TextBox>
                    </div>
                    <div class="col-sm-6 col-md-3 col-lg-3">
                        <label>Número de empleados </label>
                        <asp:TextBox ID="TextNumEmpleados" runat="server" CssClass="form-control-sm"
                            type="number" onkeydown="return validNumericos(event)"></asp:TextBox>
                    </div>
                    <div class="col-sm-6 col-md-3 col-lg-3">
                        <label>Es usted el propietario</label>
                        <asp:DropDownList runat="server" ID="cmbPropietario" nombre="Propietario"
                            CssClass="form-control-sm">
                            <asp:ListItem Text="Seleccione" Value="0" />
                            <asp:ListItem Text="Si " Value="1" />
                            <asp:ListItem Text="No" Value="2" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-6 col-md-3 col-lg-3">
                        <div id="divCargoOcupa" style="display: none">
                            <label>Cargo que ocupa </label>
                        </div>
                        <div id="TextCargoOcupaa" style="display: none">
                            <asp:TextBox ID="TextCargoOcupa" runat="server" CssClass="form-control-sm" type="text"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <!--FIN tablaResponsive FORTALECIMIENTO-->


            <input id="CodPrograma" name="CodPrograma" type="hidden" value="xm234jq" />
            <asp:TextBox ID="CodCiudadExpedicion" runat="server" name="Cod Ciudad Expedicion" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudadNacimiento" runat="server" name="Cod Ciudad Nacimiento" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudadReside" runat="server" name="Cod Ciudad Reside" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCentroDesarrollo" runat="server" name="Cod Centro Desarrollo" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudadCurso" runat="server" name="Cod Ciudad Curso" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudadDesaProyec" runat="server" name="Cod Ciudad Desarolla Proyecto" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodSubsectorEmprendimiento" runat="server" name="Cod Sub Sector Emprendimiento" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudadEmpresa" runat="server" name="Cod Ciudad Empresa" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodSubSectEmpresa" runat="server" name="Cod Sub Sector Empresa" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodProAca" runat="server" name="Codigo programa academico" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodInstituAca" runat="server" name="Codigo Institucion Academica" type="hidden" value="xm234jq"></asp:TextBox>
            <asp:TextBox ID="CodCiudInst" runat="server" name="Codigo Ciudad Institucion" type="hidden" value="xm234jq"></asp:TextBox>


        </div>

        <br />
       
            
       
        <div id="crearSol" class="row" style="display: none;">
            <div class="col-lg-2"></div>
            <div class="col-lg-8">
                <p>
                    Al presionar el botón “Registrar solicitud de información” usted 
                acepta los términos y condiciones establecidos en las normas que regulan al SENA/Fondo Emprender, 
                así como el uso de esta información para los fines que se requieran dentro de los 
                servicio de emprendimiento del SENA/Fondo Emprender .
                </p>
            </div>
            <div class="col-lg-2"></div>

            <div class="col-lg-2"></div>
            <div class="col-lg-8 my-2">
                <input type="button" onclick="return ValidarFormulario(this)" class="btn btn-primary"
                    value="Registrar solicitud de información" />
            </div>
            <div class="col-lg-2"></div>
        </div>


        <!-- Button trigger modal -->


        <!-- Modal -->
        <div class="modal fade" id="MostrarRegistro" tabindex="-1" role="dialog" aria-labelledby="MostrarRegistroModal" aria-hidden="true">
            <div class="modal-dialog modal-xl modal-dialog-scrollable" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="MostrarRegistroModal">Por favor validar la información antes de enviar el formulario.</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">

                        <div class="container">
                            <div class="row row-cols-3">
                                <div class="col">
                                    <label class="">Nombres:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNombres" />
                                </div>
                                <div class="col">
                                    <label class="">Apellidos:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblApellidos" />
                                </div>
                                <div class="col">
                                    <label class="">Identificacion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblIdentificacion" />
                                </div>


                                <div class="col">
                                    <label class="">No:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNumeroIden" />
                                </div>
                                <div class="col">
                                    <label class="">Departamento Expedición:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDeparExp" />
                                </div>
                                <div class="col">
                                    <label class="">Ciudad Expedición:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuExp" />
                                </div>

                                <div class="col">
                                    <label class="">Correo:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCorreoElec" />
                                </div>
                                <div class="col">
                                    <label class="">Genero:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblGenero" />
                                </div>
                                <div class="col">
                                    <label class="">Fecha Nacimiento:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFechaNac" />
                                </div>

                                <div class="col">
                                    <label class="">Departamento de nacimiento:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDepNac" />
                                </div>
                                <div class="col">
                                    <label class="">Ciudad de nacimiento:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuNac" />
                                </div>
                                <div class="col">
                                    <label class="">Telefono:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblTelefono" />
                                </div>

                                <div class="col">
                                    <label class="">Estado Civil:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblEstaCiv" />
                                </div>
                                <div class="col">
                                    <label class="">Ocupacion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblOcupacion" />
                                </div>
                                <div class="col">
                                    <label class="">Departamento de Residencia:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDepResi" />
                                </div>


                                <div class="col">
                                    <label class="">Municipio de residencia:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblMunires" />
                                </div>
                                <div class="col">
                                    <label class="">Direccion de residencia:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDirRes" />
                                </div>
                                <div class="col">
                                    <label class="">Estrato:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblEstrato" />
                                </div>

                            </div>

                            <div class="row">
                                <div class="col col-md-4">
                                    <label class="">Nivel Estudio:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNivelEst" />
                                </div>

                                <div class="col col-md-8">
                                    <label class="">Programa Academico:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblProgramaAca" />
                                </div>

                                <div class="col col-md-8">
                                    <label class="">Institucion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblInstitucion" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Ciudad Institucion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuInstitucion" />
                                </div>

                                <div class="col col-md-4">
                                    <label class="">Estado Estudio:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblEstadoEstudio" />
                                </div>

                                <div class="col col-md-4">
                                    <label class="">Fecha Inicio:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFechaInicio" />
                                </div>
                                <div id="DivSemesAct" class="col col-md-4">
                                    <label class="">Semestre Actual:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblSemestreActual" />
                                </div>

                                <div id="DivFechaFin" class="col col-md-4">
                                    <label class="">Fecha Fin:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFechaFin" />
                                </div>
                                <div id="DivFechaGradua" class="col col-md-4">
                                    <label class="">Fecha Graduacion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFechaGraduacion" />
                                </div>

                                <div class="col col-md-8">
                                    <label class="">Tipo Aprendiz:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblTipoAprendiz" />
                                </div>

                                <div class="col col-md-4">
                                    <label class="">Departamento Centro Desarrollo Empresarial:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDepCentroEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Ciudad Centro Desarrollo Empresarial:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuEmpre" />
                                </div>
                                <div class="col col-md-8">
                                    <label class="">Centro de Desarrollo Empresarial:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCentroEmpre" />
                                </div>

                                <div class="col col-md-4">
                                    <label class="">Servicio Requerido:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblServicioRequerido" />
                                </div>
                            </div>

                            <div id="DivSenaEmprenderRural" class="row">

                                <div class="col col-md-12">
                                    <label class="">Formacion en la que esta interesado:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFormacion" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Departamento de la formacion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDeparForma" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Ciudad de la formacion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuForma" />
                                </div>
                            </div>

                            <div id="DivEmprenFondo" class="row">

                                <div class="col col-md-6">
                                    <label class="">Departamento donde va a desarrollar su proyecto:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDepDesaPro" />
                                </div>

                                <div class="col col-md-6">
                                    <label class="">Ciudad donde va a desarrollar su proyecto:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuDesaPro" />
                                </div>

                                <div class="col col-md-6">
                                    <label class="">Sector de su proyecto:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblSectorPro" />
                                </div>

                                <div class="col col-md-6">
                                    <label class="">SubSector de su proyecto:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblSubSectorPro" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Nombre del proyecto:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNomPro" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Mercado en el que vende o proyecta vender su producto o servicio:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblMercadoOfer" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Descripcion del Proyecto:</label>
                                    <textarea rows="4" disabled="disabled" class="form-control" id="lblDesPro"></textarea>
                                </div>
                                <div class="col col-md-6">
                                    <label class="">¿Producto o servicio que oferta o proyecta ofertar? :</label>
                                    <textarea rows="4" disabled="disabled" class="form-control" id="lblProServOferE"></textarea>
                                </div>
                                <div class="col col-md-6">
                                    <label class="">¿Actualmente comercializa su producto en el mercado? :</label>
                                    <textarea rows="4" disabled="disabled" class="form-control" id="lblActualComerMer"></textarea>
                                </div>
                                <div class="col col-md-3">
                                    <label class="">¿Tiene empleados a su cargo?:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblEmpleCarg" />
                                </div>
                                <div id="DivCuantosEmple" class="col col-md-3">
                                    <label class="">Cuantos:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCantEmple" />
                                </div>

                            </div>
                            <div id="DivFortalecimiento" class="row">

                                <div class="col col-md-4">
                                    <label class="">Nombre de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNomEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Departamento de ubicacion de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDepEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Ciudad de ubicacion de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCiuEmpresa" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Sector de su empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblSectorEmpre" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">SubSector de su empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblSubSectorEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Direccion de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblDirEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Fecha de constitucion:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblFechaConsEmpre" />
                                </div>
                                <div class="col col-md-4">
                                    <label class="">Telefono de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblTelEmpre" />
                                </div>

                                <div class="col col-md-4 my-4">
                                    <label class="">Correo de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCorreoEmpre" />
                                </div>
                                <div class="col col-md-4 my-4">
                                    <label class="">Tamaño de la empresa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblTamEmpre" />
                                </div>
                                <div class="col col-md-4 my-2">
                                    <label class="">Mercado en el que vende su producto o servicio:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblMercadoOferEmpre" />
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Mercado en el que vende su producto o servicio:</label>
                                    <textarea rows="4" disabled="disabled" class="form-control" id="lblProServOferEmpre"></textarea>
                                </div>
                                <div class="col col-md-6">
                                    <label class="">Descripción de la actividad económica que desarrolla la empresa:</label>
                                    <textarea rows="4" disabled="disabled" class="form-control" id="lblDesActEcoEmpre"></textarea>
                                </div>

                                <div class="col col-md-3">
                                    <label class="">Valor de las ventas anuales:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblValVentas" />
                                </div>
                                <div class="col col-md-3">
                                    <label class="">Numero de empleados:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblNumEmpleados" />
                                </div>
                                <div class="col col-md-3">
                                    <label class="">Es usted el propietario:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblEsPropietario" />
                                </div>
                                <div id="DivCargoOcupaEmpresa" class="col col-md-3">
                                    <label class="">Cargo que ocupa:</label>
                                    <input type="text" disabled="disabled" class="form-control" id="lblCargoOcupa" />
                                </div>


                            </div>
                            <div class="row">
                                <div class="col col-md-4 my-3"></div>
                                <div class="col col-md-4">
                                    <div id="captcha">
                                    </div>
                                    <input type="text" placeholder="Ingresar Captcha" id="captchaTextBox" class="form-control" autocomplete="off" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <div class="row">
                            <div class="col-md-6 col-lg-6">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">Volver</button>
                            </div>
                            <div class="col-md-6 col-lg-6">
                                <asp:Button ID="btnInsertarRegistro" runat="server" Text="Confirmar" OnClientClick="validateCaptcha()" CssClass="btn btn-primary"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </form>


    <asp:ObjectDataSource
        ID="dataTipoDocumento"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getTipoIdentificacion"></asp:ObjectDataSource>

    <asp:ObjectDataSource
        ID="dataEstadoCivil"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getEstadoCivil"></asp:ObjectDataSource>


    <asp:ObjectDataSource
        ID="dataOcupacion"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getOcupacion"></asp:ObjectDataSource>

    <asp:ObjectDataSource
        ID="DataTipoAprendiz"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getTipoAprendiz"></asp:ObjectDataSource>

    <asp:ObjectDataSource
        ID="DataServicioRequerido"
        runat="server"
        TypeName="Fonade.RegistroUnico.RegistroUnico"
        SelectMethod="getServicioRequerido"></asp:ObjectDataSource>


</body>



</html>


<%--Carga Departamento y Ciudad De Expedicion--%>
<script type="text/javascript">

    $().ready(function () {
        $("#<%=cmbDepartamentoExpedicion.ClientID%>").change(function () {

            var params = new Object();
            params.CodDepartamento = $("#<%=cmbDepartamentoExpedicion.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesExpedicion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        });
        $("#<%=cmbCiudadExpedicion.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCiudadExpedicion.ClientID%> :selected").val());
            document.getElementById("CodCiudadExpedicion").value = $(this).val();
        });
    });

    function LoadCiudadesExpedicion(result) {

        $("#<%=cmbCiudadExpedicion.ClientID%>").html("");
        $("#<%=cmbCiudadExpedicion.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {

            $("#<%=cmbCiudadExpedicion.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>

<%--Carga Departamento y Ciudad De Nacimiento--%>
<script type="text/javascript">

    $().ready(function () {
        $("#<%=cmbDepartamentoNacimiento.ClientID%>").change(function () {
            var params = new Object();
            params.CodDepartamento = $("#<%=cmbDepartamentoNacimiento.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesNacimiento,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        });
        $("#<%=cmbCiudadNacimiento.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCiudadNacimiento.ClientID%> :selected").val());
            document.getElementById("CodCiudadNacimiento").value = $(this).val();
        });
    });

    function LoadCiudadesNacimiento(result) {

        $("#<%=cmbCiudadNacimiento.ClientID%>").html("");
        $("#<%=cmbCiudadNacimiento.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {

            $("#<%=cmbCiudadNacimiento.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>

<%--Carga Departamento y Ciudad De Residencia--%>
<script type="text/javascript">

    $().ready(function () {
        $("#<%=cmbDepartamentoReside.ClientID%>").change(function () {
            var params = new Object();
            params.CodDepartamento = $("#<%=cmbDepartamentoReside.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesResidencia,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        });
        $("#<%=cmbMunicipioReside.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbMunicipioReside.ClientID%> :selected").val());
            document.getElementById("CodCiudadReside").value = $(this).val();
        });
    });

    function LoadCiudadesResidencia(result) {

        $("#<%=cmbMunicipioReside.ClientID%>").html("");
        $("#<%=cmbMunicipioReside.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbMunicipioReside.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>


<%--Carga Programas--%>
<script type="text/javascript">

    $().ready(function () {

        $("#<%=cmbNivelEstudio.ClientID%>").change(function () {
            var params = new Object();
            params.CodNivelEstudio = $("#<%=cmbNivelEstudio.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarProgramaAcademico",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadProgramas,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });
        $("#<%=cmbProgramaAcademico.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbProgramaAcademico.ClientID%> :selected").val());
            document.getElementById("CodProAca").value = $(this).val();
        });
    });

    function LoadProgramas(result) {

        $("#<%=cmbProgramaAcademico.ClientID%>").html("");
        $("#<%=cmbProgramaAcademico.ClientID%>").append($("<option></option>").attr("value", "1111111").text("Seleccione.."));
        $("#<%=cmbProgramaAcademico.ClientID%>").append($("<option></option>").attr("value", "0").text("Otro.."));
        $.each(result.d, function () {
            $("#<%=cmbProgramaAcademico.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
        CacheItems();
    }



</script>

<%--Carga Instituciones--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbProgramaAcademico.ClientID%>").change(function () {
            var params = new Object();
            params.CodPrograma = $("#<%=cmbProgramaAcademico.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/LlenarInstitucionEducativa",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadInstituciones,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbInstitucion.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbInstitucion.ClientID%> :selected").val());
            document.getElementById("CodInstituAca").value = $(this).val();

        });
    });

    function LoadInstituciones(result) {
        $("#<%=cmbInstitucion.ClientID%>").html("");
        $("#<%=cmbInstitucion.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbInstitucion.ClientID%>").append($("<option></option>").attr("value", this.Id_InstitucionEducativa).text(this.NomInstitucionEducativa))
        });
    }
</script>

<%--Cargar Ciudad Institucion--%>

<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbInstitucion.ClientID%>").change(function () {
            var Valor = new Object();
            Valor.CodInstitucion = $("#<%=cmbInstitucion.ClientID%>").val();
            Valor.CodPrograma = document.getElementById("CodPrograma").value;
            Valor = JSON.stringify(Valor);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarCiudadInstitucion",
                data: Valor,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCIudadesInstituciones,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        });
        $("#<%=cmbInstitucionCiudad.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbInstitucionCiudad.ClientID%> :selected").val());
            document.getElementById("CodCiudInst").value = $(this).val();

        });
    });

    function LoadCIudadesInstituciones(result) {
        $("#<%=cmbInstitucionCiudad.ClientID%>").html("");
        $("#<%=cmbInstitucionCiudad.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbInstitucionCiudad.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>

<%--llenar ciudades de los centros Empresariales--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbDepartamentoEmpresarial.ClientID%>").change(function () {
            var params = new Object();
            params.CodDepartamento = $("#<%=cmbDepartamentoEmpresarial.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesEmpresariales,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbCiudadEmpresarial.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCiudadEmpresarial.ClientID%> :selected").val());
        });
    });

    function LoadCiudadesEmpresariales(result) {
        $("#<%=cmbCiudadEmpresarial.ClientID%>").html("");
        $("#<%=cmbCiudadEmpresarial.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbCiudadEmpresarial.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>


<%--llenar Centros Empresariales--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbCiudadEmpresarial.ClientID%>").change(function () {
            var params = new Object();
            params.CodCiudad = $("#<%=cmbCiudadEmpresarial.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarCentroEmpresarial",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCentrosEmpresariales,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbCentroEmpresarial.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCentroEmpresarial.ClientID%> :selected").val());
            document.getElementById("CodCentroDesarrollo").value = $(this).val();
        });
    });

    function LoadCentrosEmpresariales(result) {
        $("#<%=cmbCentroEmpresarial.ClientID%>").html("");
        $("#<%=cmbCentroEmpresarial.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbCentroEmpresarial.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>

<%--llenar Departamentos y ciudades De Centros De Formacion--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbFormacion.ClientID%>").change(function () {
            var params = new Object();
            params.CodCurso = $("#<%=cmbFormacion.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenardepartamentoformacion",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadDepartamentoCentro,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbDepartamentoFormacion.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbDepartamentoFormacion.ClientID%> :selected").val());
        });
    });

    function LoadDepartamentoCentro(result) {
        $("#<%=cmbDepartamentoFormacion.ClientID%>").html("");
        $("#<%=cmbDepartamentoFormacion.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbDepartamentoFormacion.ClientID%>").append($("<option></option>").attr("value", this.Id_DepartamentoFormacion).text(this.NomDepartamentoFormacion))
        });
    }
</script>
<%--llenar Departamentos y ciudades Donde va a desarrollar el proyecto--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbDepartamentoDesarrollarProyecto.ClientID%>").change(function () {
            var params = new Object();
            params.CodDepartamento = $("#<%=cmbDepartamentoDesarrollarProyecto.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesDesarrollarProyecto,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbCiudadDesarrollarProyecto.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCiudadDesarrollarProyecto.ClientID%> :selected").val());  
            document.getElementById("CodCiudadDesaProyec").value = $(this).val();
        });
    });

    function LoadCiudadesDesarrollarProyecto(result) {
        $("#<%=cmbCiudadDesarrollarProyecto.ClientID%>").html("");
        $("#<%=cmbCiudadDesarrollarProyecto.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbCiudadDesarrollarProyecto.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>

<%--llenar subsectores donde Emprendedimiento y fondo emprender--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbSector.ClientID%>").change(function () {
            var params = new Object();
            params.CodSector = $("#<%=cmbSector.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarSubSectores",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadSubSectores,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbSubSector.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbSubSector.ClientID%> :selected").val());
            document.getElementById("CodSubsectorEmprendimiento").value = $(this).val();

        });
    });

    function LoadSubSectores(result) {
        $("#<%=cmbSubSector.ClientID%>").html("");
        $("#<%=cmbSubSector.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbSubSector.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>


<%--llenar departamento y ciudad ubicacion empresa--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbUbicacionEmpresaD.ClientID%>").change(function () {
            var params = new Object();
            params.CodDepartamento = $("#<%=cmbUbicacionEmpresaD.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudad",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadUbicacion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbUbicacionEmpresaC.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbUbicacionEmpresaC.ClientID%> :selected").val());
            document.getElementById("CodCiudadEmpresa").value = $(this).val();
        });
    });

    function LoadCiudadUbicacion(result) {
        $("#<%=cmbUbicacionEmpresaC.ClientID%>").html("");
        $("#<%=cmbUbicacionEmpresaC.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbUbicacionEmpresaC.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>


<%--llenar sectores y subsectores empresa--%>
<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbSectorEmpresa.ClientID%>").change(function () {
            var params = new Object();
            params.CodSector = $("#<%=cmbSectorEmpresa.ClientID%>").val();
            params = JSON.stringify(params);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarSubSectores",
                data: params,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadSubSectoresEmpresa,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });

        });

        $("#<%=cmbSubSectorEmpresa.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbSubSectorEmpresa.ClientID%> :selected").val());
            document.getElementById("CodSubSectEmpresa").value = $(this).val();
        });
    });

    function LoadSubSectoresEmpresa(result) {
        $("#<%=cmbSubSectorEmpresa.ClientID%>").html("");
        $("#<%=cmbSubSectorEmpresa.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbSubSectorEmpresa.ClientID%>").append($("<option></option>").attr("value", this.Id).text(this.Nombre))
        });
    }
</script>


<%--Cargar Ciudad Formacion--%>

<script type="text/javascript">
    $().ready(function () {

        $("#<%=cmbDepartamentoFormacion.ClientID%>").change(function () {
            var Valor = new Object();
            Valor.CodDepartamento = $("#<%=cmbDepartamentoFormacion.ClientID%>").val();
            Valor.CodCurso = $("#<%=cmbFormacion.ClientID%>").val();
            Valor = JSON.stringify(Valor);
            $.ajax({
                type: "POST",
                url: "RegistroUnico.aspx/llenarciudadesformacion",
                data: Valor,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: LoadCiudadesFormacion,
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ": " + XMLHttpRequest.responseText);
                }
            });
        });
        $("#<%=cmbCiudadFormacion.ClientID%>").change(function () {
           // alert("Ha seleccionado: " + $("#<%=cmbCiudadFormacion.ClientID%> :selected").val());
            document.getElementById("CodCiudadCurso").value = $(this).val();
        });
    });

    function LoadCiudadesFormacion(result) {
        $("#<%=cmbCiudadFormacion.ClientID%>").html("");
        $("#<%=cmbCiudadFormacion.ClientID%>").append($("<option></option>").attr("value", "0").text("Seleccione.."));
        $.each(result.d, function () {
            $("#<%=cmbCiudadFormacion.ClientID%>").append($("<option></option>").attr("value", this.Id_CiudadFormacion).text(this.NomCiudadFormacion))
        });
    }
</script>

<script>
    function EnviarDatos() {

        var EstadoEstudio, ServicioRequerido, EmpleCargo;
        EstadoEstudio = document.getElementById("cmbEstadoEstudio").value;
        ServicioRequerido = document.getElementById("cmbServicioRequerido").value;
        EmpleCargo = document.getElementById("cmbEmpleados").value;

        document.getElementById("lblNombres").value = document.getElementById("txtNombres").value;
        document.getElementById("lblApellidos").value = document.getElementById("TextApellidos").value;
        document.getElementById("lblIdentificacion").value = $("#cmbTipoIdentificacion option:selected").text();
        document.getElementById("lblNumeroIden").value = document.getElementById("TextNumIdentificacion").value;
        document.getElementById("lblDeparExp").value = $("#cmbDepartamentoExpedicion option:selected").text();
        document.getElementById("lblCiuExp").value = $("#cmbCiudadExpedicion option:selected").text();
        document.getElementById("lblCorreoElec").value = document.getElementById("TextCorreo").value;
        document.getElementById("lblGenero").value = $("#cmbGenero option:selected").text();
        document.getElementById("lblFechaNac").value = document.getElementById("Textfechanac").value;
        document.getElementById("lblDepNac").value = $("#cmbDepartamentoNacimiento option:selected").text();
        document.getElementById("lblCiuNac").value = $("#cmbCiudadNacimiento option:selected").text();
        document.getElementById("lblTelefono").value = document.getElementById("TextTelefono").value;
        document.getElementById("lblEstaCiv").value = $("#cmbEstadoCivil option:selected").text();
        document.getElementById("lblOcupacion").value = $("#cmbOcupacion option:selected").text();
        document.getElementById("lblDepResi").value = $("#cmbDepartamentoReside option:selected").text();
        document.getElementById("lblMunires").value = $("#cmbMunicipioReside option:selected").text();
        document.getElementById("lblDirRes").value = document.getElementById("TextDireccionReside").value;
        document.getElementById("lblEstrato").value = document.getElementById("TextEstrato").value;
        document.getElementById("lblNivelEst").value = $("#cmbNivelEstudio option:selected").text();
        document.getElementById("lblProgramaAca").value = $("#cmbProgramaAcademico option:selected").text();
        document.getElementById("lblInstitucion").value = $("#cmbInstitucion option:selected").text();
        document.getElementById("lblCiuInstitucion").value = $("#cmbInstitucionCiudad option:selected").text();
        document.getElementById("lblEstadoEstudio").value = $("#cmbEstadoEstudio option:selected").text();
        document.getElementById("lblFechaInicio").value = document.getElementById("txtfechaini").value;

        if (EstadoEstudio == 'Finalizado') {
            document.getElementById("lblFechaFin").value = document.getElementById("Textfechafin").value;
            document.getElementById("lblFechaGraduacion").value = document.getElementById("Textfechagraducacion").value;
            $("#DivSemesAct").hide();
            $("#DivFechaFin").show();
            $("#DivFechaGradua").show();
        } else {
            document.getElementById("lblSemestreActual").value = document.getElementById("TextSemestreActual").value;
            $("#DivFechaFin").hide();
            $("#DivFechaGradua").hide();
            $("#DivSemesAct").show();
        }

        document.getElementById("lblTipoAprendiz").value = $("#cmbTipoAprendiz option:selected").text();
        document.getElementById("lblDepCentroEmpre").value = $("#cmbDepartamentoEmpresarial option:selected").text();
        document.getElementById("lblCiuEmpre").value = $("#cmbCiudadEmpresarial option:selected").text();
        document.getElementById("lblCentroEmpre").value = $("#cmbCentroEmpresarial option:selected").text();
        document.getElementById("lblServicioRequerido").value = $("#cmbServicioRequerido option:selected").text();

        if (ServicioRequerido == "2") {

            document.getElementById("lblFormacion").value = $("#cmbFormacion option:selected").text();
            document.getElementById("lblDeparForma").value = $("#cmbDepartamentoFormacion option:selected").text();
            document.getElementById("lblCiuForma").value = $("#cmbCiudadFormacion option:selected").text();
            $("#DivSenaEmprenderRural").show();
            $("#DivEmprenFondo").hide();
            $("#DivFortalecimiento").hide();

        } else if (ServicioRequerido == "3" || ServicioRequerido == "4") {

            document.getElementById("lblDepDesaPro").value = $("#cmbDepartamentoDesarrollarProyecto option:selected").text();
            document.getElementById("lblCiuDesaPro").value = $("#cmbCiudadDesarrollarProyecto option:selected").text();
            document.getElementById("lblSectorPro").value = $("#cmbSector option:selected").text();
            document.getElementById("lblSubSectorPro").value = $("#cmbSubSector option:selected").text();
            document.getElementById("lblNomPro").value = document.getElementById("TextNomProyecto").value;
            var chkBoxList = document.getElementById("cblMercadoOfer");
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var selecionmercadoofer = "";
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked) {
                    selecionmercadoofer = selecionmercadoofer + chkBoxCount[i].labels[0].innerText + ", ";
                }
            }
            if (selecionmercadoofer.length > 2) {
                selecionmercadoofer = selecionmercadoofer.substring(0, selecionmercadoofer.length - 2);

            }
            document.getElementById("lblMercadoOfer").value = selecionmercadoofer;
            document.getElementById("lblDesPro").value = document.getElementById("TextDescPro").value;
            document.getElementById("lblProServOferE").value = document.getElementById("TextProductoOferta").value;
            document.getElementById("lblActualComerMer").value = document.getElementById("TextProductoMercado").value;
            document.getElementById("lblEmpleCarg").value = $("#cmbEmpleados option:selected").text();

            if (EmpleCargo == 1) {
                document.getElementById("lblCantEmple").value = document.getElementById("TextCuantosEmpleados").value;
                $("#DivCuantosEmple").show();
            } else {
                $("#DivCuantosEmple").hide();
            }

            $("#DivEmprenFondo").show();
            $("#DivSenaEmprenderRural").hide();
            $("#DivFortalecimiento").hide();
        } else if (ServicioRequerido == "5") {
            var CarOcu = document.getElementById("cmbPropietario").value;
            document.getElementById("lblNomEmpre").value = document.getElementById("TextNomEmpresa").value;
            document.getElementById("lblDepEmpre").value = $("#cmbUbicacionEmpresaD option:selected").text();
            document.getElementById("lblCiuEmpresa").value = $("#cmbUbicacionEmpresaC option:selected").text();
            document.getElementById("lblSectorEmpre").value = $("#cmbSectorEmpresa option:selected").text();
            document.getElementById("lblSubSectorEmpre").value = $("#cmbSubSectorEmpresa option:selected").text();
            document.getElementById("lblDirEmpre").value = document.getElementById("TextDireccionEmpresa").value;
            document.getElementById("lblFechaConsEmpre").value = document.getElementById("TextFechaEmpresa").value;
            document.getElementById("lblTelEmpre").value = document.getElementById("TextTelefonoEmpresa").value;
            document.getElementById("lblCorreoEmpre").value = document.getElementById("TextCorreoEmpresa").value;
            document.getElementById("lblTamEmpre").value = $("#cmbTamaEmpresa option:selected").text();
            var chkBoxList = document.getElementById("cblMercadoOferForta");
            var chkBoxCount = chkBoxList.getElementsByTagName("input");
            var selecionmercadoofer = "";
            for (var i = 0; i < chkBoxCount.length; i++) {
                if (chkBoxCount[i].checked) {
                    selecionmercadoofer = selecionmercadoofer + chkBoxCount[i].labels[0].innerText + ", ";
                }
            }
            if (selecionmercadoofer.length > 2) {
                selecionmercadoofer = selecionmercadoofer.substring(0, selecionmercadoofer.length - 2);

            }
            document.getElementById("lblMercadoOferEmpre").value = selecionmercadoofer;
            document.getElementById("lblProServOferEmpre").value = document.getElementById("TextProductoOfertaE").value;
            document.getElementById("lblDesActEcoEmpre").value = document.getElementById("TextActividadEconomica").value;
            document.getElementById("lblValVentas").value = document.getElementById("TextVentasAnuales").value;
            document.getElementById("lblNumEmpleados").value = document.getElementById("TextNumEmpleados").value;
            document.getElementById("lblEsPropietario").value = $("#cmbPropietario option:selected").text();
            document.getElementById("lblCargoOcupa").value = document.getElementById("TextCargoOcupa").value;


            if (CarOcu == 2) {
                $("#DivCargoOcupaEmpresa").show();
            } else {
                $("#DivCargoOcupaEmpresa").hide();
            }

            $("#DivFortalecimiento").show();
            $("#DivEmprenFondo").hide();
            $("#DivSenaEmprenderRural").hide();
        }


    }
</script>

<script type="text/javascript"> 
    var ddlText, ddlValue, ddl, lblMesg;
    function CacheItems() {
        ddlText = new Array();
        ddlValue = new Array();
        ddl = document.getElementById("<%=cmbProgramaAcademico.ClientID %>");
        lblMesg = document.getElementById("<%=lblMessage.ClientID%>");
        for (var i = 0; i < ddl.options.length; i++) {
            ddlText[ddlText.length] = ddl.options[i].text;
            ddlValue[ddlValue.length] = ddl.options[i].value;
        }
    }
    window.onload = CacheItems;
    createCaptcha();
    function FilterItems(value) {
        ddl.options.length = 0;
        AddItem("Seleccione", "1111111");
        for (var i = 0; i < ddlText.length; i++) {
            if (ddlText[i].toLowerCase().indexOf(value) != -1) {
                AddItem(ddlText[i], ddlValue[i]);
            }
        }
        if (ddl.options.CodPrograma == 1111111) {
            AddItem("No se encontró el item.", "");
        }
        lblMesg.innerHTML = (ddl.options.length - 1) + " item(s) encontrado(s).";
        if (ddl.options.length == 0) {
            AddItem("No se encontró el item.", "");
        }

    }
    function AddItem(text, value) {
        var opt = document.createElement("option");
        opt.text = text;
        opt.value = value;
        ddl.options.add(opt);
    }

</script>
