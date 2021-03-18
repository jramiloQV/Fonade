<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="GestionarContratos.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Abogado.GestionarContratos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
     <h1>
        <asp:Label Text="Gestión de contratos de empresas" runat="server" ID="lblTitulo" />
    </h1>
    <br />
    <h3>
        <asp:Label Text="Cargue masivo de contratos de empresas" runat="server" ID="Label4" />
    </h3>   
    <asp:LinkButton ID="linkCargueArchivo" runat="server" Text="Cargue masivo de información de contratos" OnClick="linkCargueArchivo_Click"/>
    <br />
    <h3>
        <asp:Label ID="linkVisorContratos" Text="Visor de contratos" runat="server" />
    </h3>
    <asp:HyperLink ID="HyperLink1" NavigateUrl="~/PlanDeNegocioV2/Administracion/Abogado/VisorContrato/VisorDeContratos.aspx" runat="server" Text="Visor de contratos" />
    <br />
</asp:Content>
