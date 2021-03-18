<%@ Page Title="" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CargarActa.aspx.cs" Inherits="Fonade.PlanDeNegocioV2.Administracion.Interventoria.ActasDeSeguimiento.CargarActa" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">        
        function alerta() {
            return confirm('¿ Esta seguro de cargar este archivo, solo es permitido una sola vez ?');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">    
    <h1>
        <asp:Label ID="titleMain" runat="server" Text ="Cargar acta" ></asp:Label>            
    </h1>
    <br />
    <br />
    <asp:FileUpload ID="fuArchivo" runat="server" Width="422px" Visible="true" />  
    <br />
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Sucedio un error." Visible="False"></asp:Label>
    <br />
    <br />
    <asp:Button ID="btnAdicionar" runat="server" Text="Cargar acta" OnClientClick="return alerta();" OnClick="btnAdicionar_Click"></asp:Button>
</asp:Content>