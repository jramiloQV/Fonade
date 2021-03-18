<%@ Page Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true" CodeBehind="MisTareas.aspx.cs" Inherits="Fonade.FONADE.Tareas.MisTareas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <h1><asp:Label runat="server" ID="lbl_Titulo"></asp:Label></h1>
<asp:Panel ID="Panel1" runat="server"    Width="100%">  
        <div style="padding:20px 0px;">
            <asp:GridView ID="GridView1" CssClass="Grilla" OnPageIndexChanging="GridView1_PageIndexChanging"  runat="server"  AutoGenerateColumns="False" AllowPaging="True" AllowSorting="True" EnableTheming="True" OnSelectedIndexChanged="GridView1_SelectedIndexChanged"> 
                <columns>                
                     
                     <asp:BoundField DataField="nomproyecto" HeaderText="Plan de Negocio" />
                     <asp:BoundField DataField="fecha" HeaderText="Fecha" />
                     <asp:BoundField DataField="nomtareaprograma" HeaderText="Tipo" />
                     <asp:BoundField DataField="nomtareausuario" HeaderText="Tarea"  InsertVisible="False" ReadOnly="True" />  

                     <asp:BoundField DataField="descripcion" HeaderText="Descripcion"  InsertVisible="False" ReadOnly="True" />  
                     <asp:BoundField DataField="agendo" HeaderText="Tarea"  InsertVisible="False" ReadOnly="True" />  
                     <asp:BoundField DataField="fechacierre" HeaderText="Fecha Cierre"  InsertVisible="False" ReadOnly="True" />  
                     <asp:BoundField DataField="nomtareausuario" HeaderText="Agendó"  InsertVisible="False" ReadOnly="True" />  
                     <asp:BoundField DataField="fechacierre" HeaderText="Fecha Cierre"  InsertVisible="False" ReadOnly="True" />  
                     <asp:BoundField DataField="respuesta" HeaderText="Respuesta"  InsertVisible="False" ReadOnly="True" />  
                </columns> 
            </asp:GridView>
            <asp:Label ID="Lbl_Resultados" CssClass="Indicador" runat="server"></asp:Label>
        </div>
    </asp:Panel>


    </asp:Content>
