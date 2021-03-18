<%@ Page  MasterPageFile="~/Master.master" Language="C#" AutoEventWireup="true" CodeBehind="CatalogoConvocatoria.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.CatalogoConvocatoria" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
    <div >  
        <div>       
                <div class="div_Encabezado">
                    <table class="tbl_Encabezado">
                    <tr>
                        <td>Nueva Convocatoria</td>                        
                    </tr>
                   
                    </table>
                </div>
        </div>  
          <asp:Panel ID="Panel2" Visible="true" runat="server">
        <div>        
                  <div class="div_Contenido1">
                    <table class="tbl_Contenido1">
                    
                    <tr>
                        <td><asp:Image ID="Image1" runat="server" /><asp:Label ID="Label1" Text="Adicionar Usuario" runat="server"></asp:Label></td>                        
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
                        <td>Nombre:
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                     <tr>
                        <td>Descripción:
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
                        <td>Fecha de inicio:</td>
                        <td>
                        </td>
                    </tr>
                     <tr>
                        <td>Fecha Fin:</td>
                        <td>
                        </td>
                    </tr>
                     <tr>
                        <td>Presupuesto:</td>                        
                    </tr>
                    <tr>
                        <td>Valor mínimo para aprobar el plan de negocio:</td>
                    </tr>
                    <tr>
                     <td>Encargado Fiduciario:</td>
                    </tr>
                    <tr>
                        <td>Convenio:</td>
                    </tr>
                    <tr>
                    <td>
                        <asp:Button ID="Button1" runat="server" Text="Crear Convocatoria" />
                    </td>
                    </tr>

                    
                    </table>
                </div>
               
                
            </div>

    </asp:Panel>
    
    </div>
    </asp:Content>