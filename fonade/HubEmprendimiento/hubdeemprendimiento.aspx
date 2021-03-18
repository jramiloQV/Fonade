<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="hubdeemprendimiento.aspx.cs" Inherits="Fonade.HubEmprendimiento.hubdeemprendimiento" %>

<!doctype html>
<html lang="es">

<head>
    <!-- Required meta tags -->
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="shortcut icon" href="IMAGES/Group 84.png" type="image/png">

    <!-- Bootstrap CSS -->
    <link href="https://fonts.googleapis.com/css2?family=Josefin+Sans&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous">
    <link rel="stylesheet" href="Style/CSS/style.css">
    <script src="Style/js/jquery-3.4.1.min.js"></script>   
    <script src="Style/js/Validaciones.js"></script>


    <title>Hub de Emprendimiento SENA | Formulario de Regístro</title>
</head>

<body>

    <div class="form">

        <div class="container-fluid">
            <div class="row pt-5">
                <div class="col-12 col-md-2 text-black-50 text-center text-md-right">
                    <!-- <h2>Regístrate</h2> -->
                    <img src="IMAGES/Elementoformulario1.png" alt="logo" width="200" height="200">
                </div>
                <div class="col-md-3 offset-md-7 text-center mt-md-0 mt-3">
                    <img src="IMAGES/Group 84.png" alt="logo">
                </div>
            </div>
        </div>

        <div class="container">
            <div class="row justify-content-center">
                <form runat="server" class="needs-validation col-12 col-lg-8  m-3 p-3 rounded " onsubmit="return validateForm()" novalidate>
                    <h2 class="mb-5 text-primary font-weight-bold text-center">Formulario de registro</h2>

                    <div class="form-group row mt-3">
                        <label for="name" class="col-md-3 col-form-label text-md-right">Nombre</label>
                        <div class="input-group col-md-8">
                            <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control bg-inputs"  type="text" required></asp:TextBox>
                        <div class="invalid-feedback">
                                Por favor ingrese su Nombre.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="apellido" class="col-md-3 col-form-label text-md-right">Apellido</label>
                        <div class="input-group col-md-8">
                             <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control bg-inputs" type="text" required></asp:TextBox>
                               <div class="invalid-feedback">
                                Por favor ingrese sus apellidos.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="documento" class=" col-md-3 col-form-label text-md-right">Documento</label>
                        <div class="input-group col-md-4">
                    <asp:DropDownList runat="server" ID="cmbTipoDocumento" CssClass="form-control bg-inputs" nombre="Tipo Identificacion" AppendDataBoundItems="true"  AutoPostBack="false">
                        <asp:ListItem Text="Cédula" Value="cedula" />
                         <asp:ListItem Text="Tarjeta de identidad" Value="TI" />
                         <asp:ListItem Text="Pasaporte" Value="pasaporte" />
                        <asp:ListItem Text="NIT" Value="NIT" />
                    </asp:DropDownList>
                            
                        </div>
                        <div class="input-group col-md-4 mt-2 mt-md-0">
                            <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="form-control bg-inputs" type="number" placeholder="No. de Documento" required></asp:TextBox>
                            <div class="invalid-feedback">
                                Por favor ingrese su numero de identificacion.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="fecha-nacimiento" class="col-md-3 col-form-label text-md-right">Fecha de nacimiento</label>
                        <div class="input-group col-md-8">
                    <asp:TextBox ID="txtfechanacimiento" CssClass="form-control bg-inputs" type="date" runat="server"  placeholder="dd/mm/yyyy" autocomplete="off"
                       pattern="(^(((0[1-9]|1[0-9]|2[0-8])[\/](0[1-9]|1[012]))|((29|30|31)[\/](0[13578]|1[02]))|((29|30)[\/](0[4,6,9]|11)))[\/](19|[2-9][0-9])\d\d$)|(^29[\/]02[\/](19|[2-9][0-9])(00|04|08|12|16|20|24|28|32|36|40|44|48|52|56|60|64|68|72|76|80|84|88|92|96)$)" required>
                    </asp:TextBox>
                            <div class="invalid-feedback">
                                Por favor ingrese su fecha de nacimiento.
                            </div>
                        </div>

                    </div>

                    <div class="form-group row">
                        <label for="email" class="col-md-3 col-form-label text-md-right">Email</label>
                        <div class="input-group col-md-8">
                            <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control bg-inputs" type="email" placeholder="" required></asp:TextBox>
                            <div class="invalid-feedback">
                                Por favor ingrese su correo.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="telefono" class="col-md-3 col-form-label text-md-right">Número de teléfono</label>
                        <div class="input-group col-md-8">
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control bg-inputs" type="number" placeholder="" required></asp:TextBox>
                            <div class="invalid-feedback">
                                Por favor ingrese su Teléfono.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row">
                        <label for="departamento" class=" col-md-3 col-form-label text-md-right">Ubicación</label>
                        <div class="input-group col-md-4">                          
                            <asp:DropDownList runat="server" ID="cmbDepartamento" CssClass="form-control bg-inputs" nombre="cmbDepartamento" AppendDataBoundItems="true"  AutoPostBack="false">
                     	        <asp:ListItem Text ="Amazonas" Value="Amazonas" />
                                <asp:ListItem Text ="Antioquia" Value="Antioquia" />
                                <asp:ListItem Text ="Arauca" Value="Arauca" />
                                <asp:ListItem Text ="Atlántico" Value="Atlántico" />
                                <asp:ListItem Text ="Bolívar" Value="Bolívar" />
                                <asp:ListItem Text ="Boyacá" Value="Boyacá" />
                                <asp:ListItem Text ="Caldas" Value="Caldas" />
                                <asp:ListItem Text ="Caquetá" Value="Caquetá" />
                                <asp:ListItem Text ="Casanare" Value="Casanare" />
                                <asp:ListItem Text ="Cauca" Value="Cauca" />
                                <asp:ListItem Text ="Cesar" Value="Cesar" />
                                <asp:ListItem Text ="Chocó" Value="Chocó" />
                                <asp:ListItem Text ="Cundinamarca" Value="Cundinamarca" />
                                <asp:ListItem Text ="Córdoba" Value="Córdoba" />
                                <asp:ListItem Text ="Distrito Capital" Value="Distrito Capital" />
                                <asp:ListItem Text ="Guainía" Value="Guainía" />
                                <asp:ListItem Text ="Guajira" Value="Guajira" />
                                <asp:ListItem Text ="Guaviare" Value="Guaviare" />
                                <asp:ListItem Text ="Huila" Value="Huila" />
                                <asp:ListItem Text ="Magdalena" Value="Magdalena" />
                                <asp:ListItem Text ="Meta" Value="Meta" />
                                <asp:ListItem Text ="Nariño" Value="Nariño" />
                                <asp:ListItem Text ="Norte de Santander" Value="Norte de Santander" />
                                <asp:ListItem Text ="Putumayo" Value="Putumayo" />
                                <asp:ListItem Text ="Quindío" Value="Quindío" />
                                <asp:ListItem Text ="Risaralda" Value="Risaralda" />
                                <asp:ListItem Text ="San Andrés" Value="San Andrés" />
                                <asp:ListItem Text ="Santander" Value="Santander" />
                                <asp:ListItem Text ="Sucre" Value="Sucre" />
                                <asp:ListItem Text ="Tolima" Value="Tolima" />
                                <asp:ListItem Text ="Valle" Value="Valle" />
                                <asp:ListItem Text ="Vaupés" Value="Vaupés" />
                                <asp:ListItem Text ="Vichada" Value="Vichada" />
                        </asp:DropDownList>                           

                        </div>
                        <div class="input-group col-md-4 mt-2 mt-md-0">
                            <asp:TextBox ID="txtCiudad" runat="server" CssClass="form-control bg-inputs" type="text" placeholder="Ciudad" required></asp:TextBox>
                            <div class="invalid-feedback">
                                Por favor ingrese la ciudad.
                            </div>
                        </div>
                    </div>

                    <div class="form-group row align-items-center">
                        <label for="si" class="col-md-3 col-form-label text-md-right">¿Haces parte de la Comunidad SENA?</label>
                        <div class="col-md-8">
                            <div class="input-group" id="ComunidadSena">

                                
                                <label class="radio-custom">
                                  Sí
                                  <input  type="radio" runat="server" name="RadioComunidad" id="Radiosi" value="Si">

                                 <span></span>
                                </label>
                                <label class="radio-custom">
                                  <input type="radio" runat="server" name="RadioComunidad"   id="Radiono" value="No">
                                  No
                                  <span></span>
                                </label>
                            </div>
                        </div>
                    </div>


                    <div class="form-group row">
                        <label for="sede" class=" col-md-3 col-form-label text-md-right">Regional</label>
                        <div class="input-group col-md-5">
                            
                           <asp:DropDownList runat="server" ID="cbmRegional" CssClass="form-control bg-inputs" nombre="cmbRegional" AppendDataBoundItems="true"  AutoPostBack="false">
                     	        <asp:ListItem Text ="Ninguno" Value= "Ninguno" />
                                <asp:ListItem Text ="Amazonas" Value= "Amazonas" />
                                <asp:ListItem Text ="Antioquia" Value= "Antioquia" />
                                <asp:ListItem Text ="Arauca" Value= "	Arauca" />
                                <asp:ListItem Text ="Atlántico" Value= "Atlántico" />
                                <asp:ListItem Text ="Bolívar" Value= "Bolívar" />
                                <asp:ListItem Text ="Boyacá" Value= "	Boyacá" />
                                <asp:ListItem Text ="Caldas" Value= "	Caldas" />
                                <asp:ListItem Text ="Caquetá" Value= "Caquetá" />
                                <asp:ListItem Text ="Casanare" Value= "Casanare" />
                                <asp:ListItem Text ="Cauca" Value= "Cauca" />
                                <asp:ListItem Text ="Cesar" Value= "Cesar" />
                                <asp:ListItem Text ="Chocó" Value= "Chocó" />
                                <asp:ListItem Text ="Cundinamarca" Value= "Cundinamarca" />
                                <asp:ListItem Text ="Córdoba" Value= "Córdoba" />
                                <asp:ListItem Text ="Dirección General" Value= "Dirección General" />
                                <asp:ListItem Text ="Distrito Capital" Value= "Distrito Capital" />
                                <asp:ListItem Text ="Guainía" Value= "Guainía" />
                                <asp:ListItem Text ="Guajira" Value= "Guajira" />
                                <asp:ListItem Text ="Guaviare" Value= "Guaviare" />
                                <asp:ListItem Text ="Huila" Value= "Huila" />
                                <asp:ListItem Text ="Magdalena" Value= "Magdalena" />
                                <asp:ListItem Text ="Meta" Value= "Meta" />
                                <asp:ListItem Text ="Nariño" Value= "Nariño" />
                                <asp:ListItem Text ="Norte de Santander" Value= "Norte de Santander" />
                                <asp:ListItem Text ="Putumayo" Value= "Putumayo" />
                                <asp:ListItem Text ="Quindío" Value= "Quindío" />
                                <asp:ListItem Text ="Risaralda" Value= "Risaralda" />
                                <asp:ListItem Text ="San Andrés" Value= "San Andrés" />
                                <asp:ListItem Text ="Santander" Value= "Santander" />
                                <asp:ListItem Text ="Sucre" Value= "Sucre" />
                                <asp:ListItem Text ="Tolima" Value= "Tolima" />
                                <asp:ListItem Text ="Valle" Value= "Valle" />
                                <asp:ListItem Text ="Vaupés" Value= "Vaupés" />
                                <asp:ListItem Text ="Vichada" Value= "Vichada" />

                        </asp:DropDownList>  

                        </div>
                    </div>

                    <div class="form-group row perfiles">
                        <label for="sede" class=" col-md-3 col-form-label text-md-right">Selecciona tu perfil</label>
                        <div class="col-9"></div>

                        <div class="col-12 text-center ">
                            <input type="radio" runat="server" name="public"  id="RadioAprendiz"  value="APRENDIZ" class="visible px-5 mx-3" checked>
                            <label for="RadioAprendiz">APRENDIZ</label>
                            <input type="radio" runat="server"   name="public"  id="RadioEmprendedor"  value="EMPRENDEDOR" class="novisible px-5 mx-3">
                            <label for="RadioEmprendedor">EMPRENDEDOR</label>
                            <input type="radio" runat="server"  name="public"  id="RadioEmpresario"   value="EMPRESARIO" class="novisible px-5 mx-3">
                            <label for="RadioEmpresario">EMPRESARIO</label>
                        </div>
                        <div class="col-12 text-center pt-3">
                            <input type="radio" runat="server"  name="public"   id="RadioComprador"  value="COMPRADOR" class="visible px-5 mx-3">
                            <label for="RadioComprador">COMPRADOR</label>
                            <input type="radio" runat="server"  name="public"  id="RadioInnovador"  value="INNOVADOR" class="novisible px-5 mx-3">
                            <label for="RadioInnovador">INNOVADOR</label>
                            <input type="radio" runat="server"  name="public"  id="RadioInversionista"   value="INVERSIONISTA" class="novisible px-5 mx-3">
                            <label for="RadioInversionista">INVERSIONISTA</label>
                        </div>
                    </div>

                    <div class="form-group row">

                        <div class="col-md-12 text-center mt-5">
                            <div class="form-check form-check-inline ">
                                <label class="radio-custom">
                                  Recibir notificaciones en mi correo
                               <input type="checkbox" runat="server" name="notificaciones" id="notificaciones" value="si" >
                      
                  <span></span>
                </label>

                            </div>
                        </div>
                    </div>


                    <div class="form-group row">

                        <div class="col-md-12 text-center">
                            <div class="form-check form-check-inline">
                                <label class="radio-custom">
                                      He leído y acepto <a href="#" class="text-decoration-none color-secundario">política de tratamiento y
                                        protección de datos</a>
                                      <input type="checkbox" runat="server" name="politica" id="politica" value="1" >
                               
                                 <span>  </span>

                                </label>
                         
                               
                            </div>
                        </div>
                    </div>

                    <div class="col-12 text-center">

                         <asp:Button ID="btnEnviar" Text="Enviar" CssClass="btn btn-primary px-5 btn-submit" runat="server" OnClick="GuardarRegistro" />
                        
                        
                    </div>

                </form>


                <script>
                    // Example starter JavaScript for disabling form submissions if there are invalid fields
                    (function() {
                        'use strict';
                        window.addEventListener('load', function() {
                            // Fetch all the forms we want to apply custom Bootstrap validation styles to
                            var forms = document.getElementsByClassName('needs-validation');
                            // Loop over them and prevent submission
                            var validation = Array.prototype.filter.call(forms, function(form) {
                                form.addEventListener('submit', function(event) {
                                    if (form.checkValidity() === false) {
                                        event.preventDefault();
                                        event.stopPropagation();
                                    }
                                    form.classList.add('was-validated');
                                }, false);
                            });
                        }, false);
                    })();
                </script>
            </div>
        </div>
        <div class="container-fluid">
            <div class="row pt-5">
                <div class="col-12 col-md-2 text-black-50 text-center text-md-right">
                   
                </div>
                <div class="col-md-3 offset-md-7 text-center mt-md-0 mt-3">
                    <img src="IMAGES/Elementoformulario2.png" alt="logo" width="200" height="200">
                </div>
            </div>
        </div>
        
    </div>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut" crossorigin="anonymous">
    </script>

    <!--font awesome-->
    <script src="https://kit.fontawesome.com/07109de310.js" crossorigin="anonymous"></script>

    <!-- carga de Jquery -->
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous">
    </script>

    <!-- carga de popper -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossorigin="anonymous">
    </script>

    <!-- carga de Bootstrap -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossorigin="anonymous">
    </script>
</body>

</html>


    <script type="text/javascript">
$(document).ready(function() {
    $('input[type=checkbox]').live('click', function(){
        var parent = $(this).parent().attr('ComunidadSena');
        $('#'+parent+' input[type=checkbox]').removeAttr('checked');
        $(this).attr('checked', 'checked');
    });
});
</script>

