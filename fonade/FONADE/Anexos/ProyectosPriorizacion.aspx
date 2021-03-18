<%@ Page MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="ProyectosPriorizacion.aspx.cs" Inherits="Fonade.FONADE.Anexos.ProyectosPriorizacion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <div >  
        <div>       
                <div class="div_Encabezado">
                    <table class="tbl_Encabezado">
                    <tr>
                        <td>CRITERIOS DE PRIORIZACIÓN</td>                        
                    </tr>
                   
                    </table>
                </div>
        </div>  
          <asp:Panel ID="Panel2" Visible="true" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    
                    <tr>
                        <td><asp:Image ID="Image1" runat="server" /><asp:Label ID="Label1" Text="Adicionar Criterio de priorización" runat="server"></asp:Label></td>                        
                    </tr>
                    <tr>
                        <td>
                        <asp:GridView ID="GridView1" runat="server">
                        
                        </asp:GridView>
                        </td>  
                                               
                    </tr>
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>
       <asp:Panel ID="Panel1" Visible="false" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">                   
                    <tr>
                        <td>Número Acta:</td>
                        <td>
                            <asp:TextBox ID="TextBox8" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Nombre de Acta::
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </div>

                 <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    <tr>
                        <td>Fecha Acta:</td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Convocatoria:</td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>Observaciones:</td>    
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="TextBox5" runat="server"></asp:TextBox>
                         </td>                    
                    </tr>
                    
                   
                  
                    <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Asignar Recursos" />
                    </td>
                    </tr>

                    
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>

   
    
    </div>
    </asp:Content>
