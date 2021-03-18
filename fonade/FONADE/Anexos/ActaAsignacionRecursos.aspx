<%@ Page   MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="ActaAsignacionRecursos.aspx.cs" Inherits="Fonade.FONADE.Anexos.ActaAsignacionRecursos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <div >  
        <div>       
                <div class="div_Encabezado">
                    <table class="tbl_Encabezado">
                    <tr>
                        <td>ACTA DE ASIGNACION DE RECURSOS</td>                        
                    </tr>
                   
                    </table>
                </div>
        </div>  
          <asp:Panel ID="Panel2" Visible="true" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    
                   
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
                            
                        <asp:TextBox runat="server" ID="txtDate2"  Text="11/01/2006" />
                        <asp:Image runat="server" ID="btnDate2" AlternateText="cal2" ImageUrl="~/images/calendaricon.jpg" />
                        <ajaxtoolkit:calendarextender runat="server" ID="calExtender2" PopupButtonID="btnDate2" CssClass="AjaxCalendar" TargetControlID="txtDate2" Format="MMMM d, yy" />
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


