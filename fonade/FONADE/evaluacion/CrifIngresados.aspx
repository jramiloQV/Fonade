<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Emergente.Master" AutoEventWireup="true" CodeBehind="CrifIngresados.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CrifIngresados" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <br/>
      <div  align="center" style="margin-top:100; overflow: auto;" >

                    <table border=0 cellSpacing=0 cellPadding=2 width="450px" ID="Table10">
                        <tr>
                           <td style="background-color:#00468f;" width=175 align=middle>
                               <label style="color: #ffffff">CRIF INGRESADOS</label></td>
                                <td style="background-color:#00468f;" width=175 align=middle>
                                    <asp:Label style="color: #ffffff" ID="lfecha" runat="server" />
                                    </td>
                           
                        </tr>
                       

                    </table>


                    <table width="450px"  cellpadding="0" cellspacing="0" ID="Table12">
                    <tr><td>&nbsp;</td></tr>
                        <tr>
                            <td valign="top">
                                <asp:GridView ID="GrvCrif" Width="100%" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="True" CssClass="Grilla" 
                                    EmptyDataText="NO SE ENCONTRARON DATOS" 
                                    OnPageIndexChanging="GrvNotificaciones_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        <asp:BoundField DataField="CRIF" HeaderText="# Radicaci&oacute;n CRIF" />
                                       
                                    </Columns>
                                </asp:GridView>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                               
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                               
                                <asp:Button ID="btncerrar" runat="server" Text="Cerrar" 
                                    onclick="btncerrar_Click" />
                               
                            </td>
                        </tr>
                    </table>
</div>
</asp:Content>
