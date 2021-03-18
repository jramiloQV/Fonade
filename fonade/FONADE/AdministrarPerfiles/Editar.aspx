<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="Editar.aspx.cs" Inherits="Fonade.FONADE.AdministrarPerfiles.Editar" %>


<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Table ID="Table1" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label1" runat="server" Text="Tipo de Identificación:"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
            <asp:dropdownlist ID="ddl_TipoIdentificacion" runat="server"></asp:dropdownlist>
                
            </asp:TableCell>            
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
            <asp:Label ID="Label2" runat="server" Text="Número de Identificación:"></asp:Label>
            </asp:TableCell> 
            <asp:TableCell>
                <asp:TextBox ID="tb_NumeroIdentificacion" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label3" runat="server" Text="Nombre Asesor:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
            <asp:TextBox ID="tb_NombreAsesor" runat="server"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label5" runat="server" Text="Apellidos del  Asesor:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
           <asp:Textbox ID="tb_ApellidosAsesor" runat="server"></asp:Textbox>
        </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
         <asp:TableCell>  
            <asp:Label ID="Label6" runat="server" Text="Experiencia Docente:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
            <asp:TextBox ID="tb_ExperienciaDocente" TextMode="MultiLine" runat="server"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label7" runat="server" Text="Dedicación a la unidad:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
              <asp:DropDownList ID="ddl_DedicacionUnidad" runat="server">
                  <asp:ListItem Value="1">Parcial</asp:ListItem>
                  <asp:ListItem Value="0">Completa</asp:ListItem>
                  
              </asp:DropDownList>
          </asp:TableCell>
        </asp:TableRow>
       
        <asp:TableRow>
         <asp:TableCell>
            <asp:Label ID="Label4" runat="server" Text="EMail Asesor:	"></asp:Label>
          </asp:TableCell>
           <asp:TableCell>   
            <asp:TextBox ID="tb_EmailAsesor" runat="server"></asp:TextBox>
             </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label8" runat="server"  Text="Resumen Hoja de vida:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
            <asp:TextBox ID="tb_ResumenHojaVida" TextMode="MultiLine" runat="server"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label9" runat="server" Text="Experiencia e intereses:	"></asp:Label>
         </asp:TableCell> 
          <asp:TableCell>  
            <asp:TextBox ID="tb_ExperienciaIntereses" TextMode="MultiLine" runat="server"></asp:TextBox>
          </asp:TableCell>
        </asp:TableRow>
         <asp:TableRow>
          <asp:TableCell>
            <asp:Button ID="btn_modificar"  OnClick="btn_modificar_onclick" runat="server" Text="Modificar Asesor" />
          </asp:TableCell>
        </asp:TableRow>
        </asp:Table>

        <asp:Label runat="server"  Text="Información Academica"></asp:Label>

        <asp:GridView ID="gw_InformacionAcademica" runat="server" Width="100%" AutoGenerateColumns="False"
        DataKeyNames="" CssClass="Grilla" AllowPaging="false" 
        
        AllowSorting="True">
        <Columns>            
          
            <asp:BoundField DataField="NivelEstudio"  HeaderText="Nivel Estudio" />
            <asp:BoundField DataField="TituloObtenido" HeaderText="Titulo Obtenido"/>
            <asp:BoundField DataField="Institucion" HeaderText="Institución"/>
            <asp:BoundField DataField="FechaTitulo" HeaderText="Año Titulo"/>
            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />            
        </Columns>
    </asp:GridView>
         
    </asp:Panel>
</asp:Content>