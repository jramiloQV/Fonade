<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="SoporteTecnico.aspx.cs" Inherits="Fonade.SoporteHelper.SoporteTecnico" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <h1>
        <asp:Label Text="Soporte tecnico fondo emprender" runat="server" ID="lblTitulo" />
    </h1>
    <br />



    <h3>
        <asp:Label Text="Actividades de ejecución" runat="server" ID="Label4" />
    </h3>
    <asp:HyperLink ID="linkActividadesDuplicadas" NavigateUrl="~/SoporteHelper/ActividadesDuplicadas/ActividadesDuplicadas.aspx" runat="server" Text="Administrar Actividades duplicadas de plan operativo" />
    <br />
    <br />
    <asp:HyperLink ID="linkActividades" NavigateUrl="~/SoporteHelper/ActividadesDuplicadas/Actividades.aspx" runat="server" Text="Administrar Actividades de plan operativo" />
    <br />
    <br />
    <asp:HyperLink ID="linkActividadesNomina" NavigateUrl="~/SoporteHelper/ActividadesDuplicadas/ActividadesNomina.aspx" runat="server" Text="Administrar Actividades de nomina" />
    <br />
    <br />
    <asp:HyperLink ID="linkActividadesProduccion" NavigateUrl="~/SoporteHelper/ActividadesDuplicadas/ActividadesProduccion.aspx" runat="server" Text="Administrar Actividades de producción" />
    <br />
    <br />
    <asp:HyperLink ID="linkActividadesVentas" NavigateUrl="~/SoporteHelper/ActividadesDuplicadas/ActividadesVentas.aspx" runat="server" Text="Administrar Actividades de Ventas" />
    <br />
    <br />

    <h3>
        <asp:Label Text="General" runat="server" ID="Label3" />
    </h3>
    <asp:HyperLink ID="linkSucursal" NavigateUrl="~/SoporteHelper/Sucursal/Sucursal.aspx" runat="server" Text="Administrar sucursales" />
    <br />
    <br />
    <asp:HyperLink ID="linkObservacionesDeEvaluacion" NavigateUrl="~/SoporteHelper/ObservacionesDeEvaluacion/ObservacionesDeEvaluacion.aspx" runat="server" Text="Administrar observaciones de evaluación" />
    <br />
    <br />
    <asp:HyperLink ID="linkObservacionesDeAcreditacion" NavigateUrl="~/SoporteHelper/ObservacionesDeAcreditacion/ObservacionesDeAcreditacion.aspx" runat="server" Text="Administrar observaciones de acreditación" />
    <br />
    <br />
    <h3>
        <asp:Label Text="Usuarios" runat="server" ID="Label2" />
    </h3>
    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/SoporteHelper/Usuarios/UsuariosPorProyecto.aspx" runat="server" Text="Administrar Usuarios por proyecto" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink3" NavigateUrl="~/SoporteHelper/Usuarios/InactivarEmprendedor.aspx" runat="server" Text="Inactivar emprendedores" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink9" NavigateUrl="~/SoporteHelper/Usuarios/ReiniciarAceptarTerminosyCondiciones.aspx" runat="server" Text="Reiniciar Terminos y Condiciones" />
    <br />
    <br />

    <h3>
        <asp:Label Text="Archivos" runat="server" ID="Label1" />
    </h3>
    <asp:HyperLink ID="HyperLink4" NavigateUrl="~/SoporteHelper/Archivos/Contrato.aspx" runat="server" Text="Administrar archivos de contrato" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/SoporteHelper/CargueMasivoContratos/CargueContratos.aspx" runat="server" Text="Cargue masivo de contratos" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink10" NavigateUrl="~/SoporteHelper/Archivos/EliminarAnexosAcreditacion.aspx" runat="server" Text="Eliminar archivos de acreditación" />

    <h3>
        <asp:Label Text="Interventoria" runat="server" ID="Label5" />
    </h3>
    <br />
    <asp:HyperLink ID="HyperLink6" NavigateUrl="~/SoporteHelper/Interventoria/Presupuesto/Presupuesto.aspx" runat="server" Text="Modificar presupuesto" />
    <h3>
        <asp:Label Text="Proyecto" runat="server" ID="Label6" />
    </h3>
    <br />
    <asp:HyperLink ID="HyperLink5" Enabled="false" NavigateUrl="~/SoporteHelper/Interventoria/Presupuesto/Presupuesto.aspx" runat="server" Text="Cambiar estado de planes de negocio. (Desactivado temporalmente hasta hacer cambios con proyecto de pruebas)" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink7" Enabled="true" Target="_blank" NavigateUrl="http://www.fondoemprender.com:8081/" runat="server" Text="Ver historia del proyecto. (Modulo de soporte tecnico V2.)" />
    <h3>
        <asp:Label Text="Convocatorias" runat="server" ID="Label7" />
    </h3>
    <br />
    <asp:HyperLink ID="HyperLink8" Enabled="true" Target="_blank" NavigateUrl="~/SoporteHelper/PresupuestoConvocatorias/PresupuestoConvocatorias.aspx" runat="server" Text="Ver presupuesto disponible por convocatorias" />
    <br />
</asp:Content>
