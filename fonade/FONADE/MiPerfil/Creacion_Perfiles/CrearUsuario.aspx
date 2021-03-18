<%@ Page MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="CrearUsuario.aspx.cs" Inherits="Fonade.FONADE.Creacion_Perfiles.CrearUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <div >  
        <div>       
                <div class="div_Encabezado">
                    <table class="tbl_Encabezado">
                    <tr>
                        <td>Usuario Fiduciaria</td>                        
                    </tr>
                   
                    </table>
                </div>
        </div>  

        <asp:Panel ID="Panel1" Visible="false" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    
                    <tr>
                        <td><asp:Image runat="server" /><asp:Label Text="Adicionar Usuario" runat="server"></asp:Label></td>                        
                    </tr>
                    <tr>
                        <td>
                        <asp:GridView runat="server">
                        
                        </asp:GridView>
                        </td>  
                                               
                    </tr>
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>
    <asp:Panel ID="Panel2"   Visible="false" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">                    
                    <tr>    
                        <td>Nombres:</td>                    
                        <td></td>                    
                    </tr>
                    <tr>  
                    <td>Apellidos:</td>                    
                    <td></td>              
                                               
                    </tr>
                    <tr>  
                    <td>Identificación:</td>                    
                    <td>
                        <asp:DropDownList   ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                    </td> 
                    <td>No:</td> 
                    <td>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td>              
                                               
                    </tr>
                    <tr>  
                    <td>Email:</td>                    
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td>              
                                               
                    </tr>
                    <tr> 
                    <td></td> 
                    <td></td>                    
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Crear" /></td>              
                                               
                    </tr>
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>
    </div>
    </asp:Content>