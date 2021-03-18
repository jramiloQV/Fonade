<%@ Page Title="FONDO EMPRENDER" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="CatalogoTipoAmbito.aspx.cs" Inherits="Fonade.FONADE.interventoria.CatalogoAmbito" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .auto-style2 {
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    
     <table class="style10">
        <tr>
            <td>
            <h1>
                <asp:Label ID="lbltitulo" runat="server"  style="font-weight: 700">Ambito</asp:Label>
                </h1>
            </td>
        </tr>
    </table>
    <br />
    <asp:Panel ID="pnlPrincipal" runat="server" Style="margin-left: 10px;">
               <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">
                    <asp:ImageButton ID="ibtn_crearAmbito" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/add.png" OnClick="ibtn_crearAmbito_Click" />
                    <asp:LinkButton ID="lbtn_crearAmbito" runat="server" OnClick="lbtn_crearAmbito_Click" >Adicionar Ambito</asp:LinkButton>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
        <asp:GridView ID="gvcAmbito" runat="server" Width="100%" AutoGenerateColumns="False"
            CssClass="Grilla" AllowPaging="True" AllowSorting="True" PagerStyle-CssClass="Paginador"
           OnRowCommand="gvcAmbitoRowCommand" 
            OnPageIndexChanging="gvcAmbitoPageIndexChanging" >
            <Columns>
               
                <asp:TemplateField HeaderText="" >
                    <ItemTemplate>
                        <asp:Label ID="lblidproyecto" runat="server" Text='<%# Eval("id_TipoAmbito") %>'  CommandArgument='<%# Eval("id_TipoAmbito")  %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="Ambito" SortExpression="Tipo Ambito" >
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEmpresa" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id_TipoAmbito") %>' 
                            CommandName="editacontacto" Text='<%# Eval("nomTipoAmbito") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
            </Columns>
            <PagerStyle CssClass="Paginador" />
        </asp:GridView>
        <br/>
     
        <br/>
     
     </asp:Panel>
    <br />
    <asp:Panel ID="PanelModificar" runat="server" Visible="false">

                <table width="500px">
          
              
                    
                    <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Ingrese Nombre:</td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txt_Nombre" runat="server" BackColor="White" Enabled="true" Text="" Width="350px"></asp:TextBox>
                        &nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_Nombre" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator>  
                    </td>
                    <td class="auto-style10">
                      </td>
                  </tr>
                 <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">&nbsp;</td>
                    <td class="auto-style9">
                        &nbsp;
                        
                    </td>
                    <td class="auto-style10">
                      </td>
                  </tr>
                    
                </table>

                <table width="500px">

                  <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_actualizar" runat="server" Text="Crear" OnClick="btn_actualizar_Click"  />
                      </td>
                    <td>&nbsp;</td>
                  </tr>

                </table>


            </asp:Panel>
<asp:Panel ID="PnlActualizar" runat="server" Visible="false">

       <table width="500px">
          
              
                    
                    <tr>
                    <td class="auto-style7"></td>
                    <td class="auto-style8">Ingrese Nombre:</td>
                    <td class="auto-style9">
                        <asp:TextBox ID="txtNomUpd" runat="server" BackColor="White" Enabled="true" Text="" Width="350px"></asp:TextBox>
                        &nbsp;
                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtNomUpd" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="accionar">Campo Requerido</asp:RequiredFieldValidator> 
                    </td>
                    <td class="auto-style10">
                      </td>
                  </tr>
               <tr>
                    <td class="auto-style4">&nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Button ID="Btnupdate" runat="server" Text="Actualizar" OnClick="Btnupdate_Click" />
                        <asp:Button ID="BtnBorrar" runat="server" Text="Borrar" OnClick="BtnBorrar_Click" />
                      </td>
                    <td>&nbsp;</td>
                  </tr>
                    
                </table>

            </asp:Panel>

</asp:Content>
