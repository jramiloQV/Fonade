<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="NuevoOperador.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Operador.NuevoOperador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .anchoTextBox {
            width: 50%
        }

        .label {
            display: inline-block;
            width: 90px;
        }

       
    </style>

    <script src="../../../Scripts/validaciones.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <div>
        <asp:Label runat="server" ID="lbl_Titulo" Text="NUEVO OPERADOR" Font-Size="Large" />
        <hr />
        <div>
            <asp:Label ID="lblNombre" runat="server" CssClass="label"
                Text="Nombre:"></asp:Label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="anchoTextBox" 
                MaxLength="60"
                pattern="[A-Za-z0-9ÜüñÑ ]+"
                title="Solo se permiten caracteres alfanumericos"
                required></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblNit" runat="server" CssClass="label"
                Text="Nit:"></asp:Label>
            <asp:TextBox ID="txtNit" runat="server" CssClass="anchoTextBox" 
                 pattern="[0-9-]+"
                title="Solo se permiten caracteres numericos y '-'"
                required></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblTelefono" runat="server" CssClass="label"
                Text="Telefono/Celular:"></asp:Label>
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="anchoTextBox" 
                pattern="[0-9]+"
                title="Solo se permiten caracteres numericos"
                MaxLength="10"
                required></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblDireccion" runat="server" CssClass="label"
                Text="Direccion:"></asp:Label>
            <asp:TextBox ID="txtDireccion" runat="server" CssClass="anchoTextBox"
                MaxLength="100"
                required></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblEmail" runat="server" CssClass="label"
                Text="Email:"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="anchoTextBox" required
                TextMode="Email"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblRepresentante" runat="server" CssClass="label"
                Text="Representante:"></asp:Label>
            <asp:TextBox ID="txtRepresentante" runat="server" CssClass="anchoTextBox" 
                 MaxLength="60"
                pattern="[A-Za-z0-9ÜüñÑ ]+"
                title="Solo se permiten caracteres alfanumericos"
                required>
            </asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblTelRepresentante" runat="server" CssClass="label"
                Text="Telefono - Representante:"></asp:Label>
            <asp:TextBox ID="txtTelRepresentante" runat="server" CssClass="anchoTextBox" 
                pattern="[0-9]+"
                title="Solo se permiten caracteres numericos"
                MaxLength="10"
                required></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblEmailRepresentante" runat="server" CssClass="label"
                Text="Email - Representante:"></asp:Label>
            <asp:TextBox ID="txtEmailRepresentante" runat="server" CssClass="anchoTextBox" required
                TextMode="Email"></asp:TextBox>
        </div>
        <div>
            <asp:Label ID="lblLogo" runat="server" CssClass="label"
                Text="Logo-100px*100px"></asp:Label>
            <asp:FileUpload ID="fuArchivo" runat="server" CssClass="anchoTextBox" 
                size="20" accept="image/*"                               
                />
        </div>
        <div>
            <asp:Label ID="Label1" runat="server" CssClass="label"
                Text="Email Observacion Acreditacion:"></asp:Label>
            <asp:TextBox ID="txtEmailObsAcreditacion" runat="server" CssClass="anchoTextBox" required
                TextMode="Email"></asp:TextBox>
        </div>
        <div>
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" OnClick="btnGuardar_Click" />

            <input type="submit" name="btnCancelar" 
                value="Cancelar" id="btnCancelar" onclick="javascript: __doPostBack('ctl00$gv_Menu$ctl05$lnk', '')"                
                >
        </div>
    </div>

    <script>
        function regresar() {
            location.href = "Operadores.aspx";
        }
    </script>

   <script>
       function validarImagen() {
           var fileSize = $('#bodyContentPlace_fuArchivo')[0].files[0].size;
           var siezekiloByte = parseInt(fileSize / 20480);
           if (siezekiloByte > $('#bodyContentPlace_fuArchivo').attr('size')) {
               alert("Imagen muy grande");
               return false;
           }
       }
</script>
</asp:Content>
