<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeBehind="InterventorAgenda.aspx.cs" Inherits="Fonade.FONADE.interventoria.InterventorAgenda" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table
        {
            width: 100%;
        }
        td
        {
            vertical-align: top;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <h1>
        <asp:Label ID="L_titulo" runat="server" Text="AGENDAR VISITA" />
    </h1>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gv_agenda" runat="server" CssClass="Grilla" Width="100%" EmptyDataText="Usted no tiene Visitas Programadas."
                    AutoGenerateColumns="False" OnRowCommand="gv_agenda_RowCommand" ShowHeaderWhenEmpty="true">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="imgEstado" runat="server" ImageUrl='<%# "~/Images/ico" + Eval("Estado").ToString().Trim() + "s.gif" %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Nit" DataField="Nit" />
                        <asp:TemplateField HeaderText="Nombre">
                            <ItemTemplate>
                                <asp:Button ID="btnNombre" runat="server" Text='<%# Eval("razonsocial") %>' CommandArgument='<%# Eval("Id_Visita") %>'
                                    CssClass="boton_Link_Grid" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha o Período">
                            <ItemTemplate>
                                <%-- 
                                    f3l 2014
                                    Se agrega formato de fecha para NO mostrar fecha y hora segun solicitud:ERROR INT-81
                                --%>
                                <%--<asp:Label ID="Label1" runat="server" Text='<%# Eval("FechaInicio") + " a " + Eval("FechaFin") %>' />--%>
                               <asp:Label ID="Label1" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}",Eval("FechaInicio")) + " al " + String.Format("{0:dd/MM/yyyy}",Eval("FechaFin")) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnNuevaVisita" runat="server" Text="Agendar nueva visita" Visible="false"
                    Enabled="false" OnClick="btnNuevaVisita_Click" />
            <!-- Pedro V. Carreño  Falta el filtro por letra ERROR INT-82 13/1/2014 -INICIO -->
                <div  style = "float:right"">    
                 <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="Z" runat="server" OnClick="Z_Click">Z </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID ="Y" runat="server" OnClick="Y_Click">Y </asp:LinkButton>
                </div> 
               <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="X" runat="server" OnClick="X_Click">X </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="W" runat="server" OnClick="W_Click">W </asp:LinkButton>
                </div> 
                 <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="V" runat="server" OnClick="V_Click">V </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="U" runat="server" OnClick="U_Click">U </asp:LinkButton>
                </div> 
               <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="T" runat="server" OnClick="T_Click">T </asp:LinkButton>
                </div> 
                  <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="S" runat="server" OnClick="S_Click">S </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="R" runat="server" OnClick="R_Click">R </asp:LinkButton>
                </div> 
                  <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="Q" runat="server" OnClick="Q_Click">Q </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="P" runat="server" OnClick="P_Click">P </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="O" runat="server" OnClick="O_Click">O </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="N" runat="server" OnClick="N_Click">N </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="M" runat="server" OnClick="M_Click">M </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="L" runat="server" OnClick="L_Click">L </asp:LinkButton>
                </div> 
                 <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="K" runat="server" OnClick="K_Click">K </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="J" runat="server" OnClick="J_Click">J </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="I" runat="server" OnClick="I_Click">I </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="H" runat="server" OnClick="H_Click">H </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="G" runat="server" OnClick="G_Click">G </asp:LinkButton>
                </div> 
                 <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="F" runat="server" OnClick="F_Click">F </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="E" runat="server" OnClick="E_Click">E </asp:LinkButton>
                </div>
                <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="D" runat="server" OnClick="D_Click">D </asp:LinkButton>
                </div> 
                 <div style = "float:right; padding-right: 0.5em" > 
                    <asp:LinkButton ID="C" runat="server" OnClick="C_Click">C </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                <asp:LinkButton ID="B" runat="server" OnClick="B_Click">B </asp:LinkButton>
                </div> 
                <div style = "float:right; padding-right: 0.5em" > 
                 <asp:LinkButton ID="A" runat="server" OnClick="A_Click">A </asp:LinkButton>
                 </div> 
            </div> 
            <!-- Pedro V. Carreño  Falta el filtro por letra ERROR INT-82 13/1/2014 - FIN -->
            </td>
            
        </tr>
    </table>
</asp:Content>

