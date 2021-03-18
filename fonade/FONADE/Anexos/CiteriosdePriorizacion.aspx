<%@ Page MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="CiteriosdePriorizacion.aspx.cs" Inherits="Fonade.FONADE.Anexos.CiteriosdePriorizacion" %>
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
                        <td>Factor:
                        </td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td>Componente:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    </table>
                </div>

                 <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    <tr>
                        <td>Citerio:</td>
                        <td>
                            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Sigla:</td>
                        <td>
                            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Indicador:</td>    
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="TextBox5" runat="server"></asp:TextBox>
                         </td>                    
                    </tr>
                    <tr>
                        <td>Valor de Base:</td>
                        <td>
                            <asp:TextBox ID="TextBox6" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                     <td>Formulación:</td>
                     <td>
                            <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Query:</td>
                        <td>
                            <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Crear" />
                    </td>
                    </tr>

                    
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>

   
    
    </div>
    </asp:Content>
