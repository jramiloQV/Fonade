<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="VerDetalle.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.VerDetalle" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Table ID="Table1" runat="server" Width="696px">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label7" runat="server" Text="Email" />
            </asp:TableCell><asp:TableCell>
            <asp:TextBox ID="tb_email" runat="server" Width="250px" Wrap="true" Enabled="false" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
            <asp:TableCell>
            <asp:Label ID="Label2" runat="server" Text="Experiencia Docente" />
            </asp:TableCell><asp:TableCell>
                <asp:TextBox ID="tb_ExperienciaDocente" runat="server" Width="500px" Wrap="true" TextMode="MultiLine" Rows="4" Enabled="false" />
            </asp:TableCell></asp:TableRow><asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label3" runat="server" Text="Dedicación a la Unidad	" />
         </asp:TableCell><asp:TableCell>  
            <asp:TextBox ID="tb_Dedicacion" runat="server" Width="500px" Wrap="true" Enabled="false" />
          </asp:TableCell></asp:TableRow><asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label5" runat="server" Text="Resumen hoja de Vida	" />
         </asp:TableCell><asp:TableCell>  
           <asp:Textbox ID="tb_Resumenhv" runat="server" Width="500px" Wrap="true" TextMode="MultiLine" Rows="4" Enabled="false" />
        </asp:TableCell></asp:TableRow><asp:TableRow>
         <asp:TableCell>  
            <asp:Label ID="Label6" runat="server" Text="Experiencia e Intereses	" />

         </asp:TableCell><asp:TableCell>  
            <asp:Textbox ID="tb_experienciaIntereses" runat="server" Width="500px" Wrap="true" TextMode="MultiLine" Rows="4" Enabled="false" />
         </asp:TableCell></asp:TableRow></asp:Table><asp:Label ID="Label1" runat="server"  Text="Información Academica" />

        <asp:GridView ID="gw_InformacionAcademica" runat="server" Width="100%" AutoGenerateColumns="False"
        DataKeyNames="" CssClass="Grilla" AllowPaging="false" AllowSorting="True">
        <Columns>          
            <asp:BoundField DataField="NivelEstudio"  HeaderText="Nivel Estudio" />
            <asp:BoundField DataField="TituloObtenido" HeaderText="Titulo Obtenido"/>
            <asp:BoundField DataField="Institucion" HeaderText="Institución"/>
            <asp:BoundField DataField="FechaTitulo" HeaderText="Año Titulo"/>
            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />            
        </Columns>
    </asp:GridView>

        <div style="text-align:right" >
            <asp:Button ID="btnRegresar" runat="server" Text="Volver" OnClientClick="javascript:window.history.back();" />
        </div>
         
    </asp:Panel>
</asp:Content>