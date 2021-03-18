<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GestionEntidades.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.Entidad.GestionEntidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
      <h1>
        <asp:Label Text="Gestión de entidades de interventoria" runat="server" ID="lblTitulo" />
    </h1>
    <br />
    <h3>
        <asp:Label Text="Gestión de entidades" runat="server" ID="Label4" />
    </h3>
    <br />
    <asp:HyperLink ID="linkCargueMasivo" NavigateUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Entidades.aspx" runat="server" Text="Administrar entidades" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink2" NavigateUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Empresa/AsignarEmpresa.aspx" runat="server" Text="Asignar interventores a empresas" />
    <br />
    <br />
    <asp:HyperLink ID="HyperLink3" NavigateUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Interventor/Interventores.aspx" runat="server" Text="Asignar interventores a entidades" />
    <br />
    <br />   
    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/PlanDeNegocioV2/Administracion/Interventoria/Entidad/Contrato/AsignarContrato.aspx" runat="server" Text="Asignar contratos a interventores"/>
</asp:Content>