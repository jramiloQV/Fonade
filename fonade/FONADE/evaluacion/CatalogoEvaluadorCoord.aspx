<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CatalogoEvaluadorCoord.aspx.cs" Inherits="Fonade.FONADE.evaluacion.CatalogoEvaluadorCoord"  MasterPageFile="~/Master.Master"  %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">

    <asp:LinqDataSource ID="lds_listaCoordEval" runat="server" 
    ContextTypeName="Datos.FonadeDBDataContext" AutoPage="false" 
    onselecting="lds_listaCoordEval_Selecting" >
    </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="panelApertura" runat="server" Visible="true" Width="100%" UpdateMode="Conditional">
        <ContentTemplate>

            <table width="100%">
              <tr>
                <td>
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
              </tr>
            </table>

            <table width="100%">
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">
                    <asp:ImageButton ID="ibtn_crearCoorE" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/add.png" />
                    <asp:LinkButton ID="lbtn_crearCoorE" runat="server" OnClick="lbtn_crearCoorE_Click" >Crear un nuevo coordinador de evaluadores</asp:LinkButton>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">

                    <asp:GridView ID="gvcCoordinadoresEval"  CssClass="Grilla" runat="server"
                        DataSourceID="lds_listaCoordEval" AllowSorting="True"
                        AllowPaging="false" AutoGenerateColumns="false" Width="100%"
                        EmptyDataText="Aún no hay criterios para esta convocatoria." OnLoad="gvcCoordinadoresEval_Load" OnRowCommand="gvcCoordinadoresEval_RowCommand"  >
                        <Columns>

                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>

                                    <asp:HiddenField ID="Hiddeninactivo" runat="server" Value='<%# Eval("inactivo") %>' />
                                    <asp:HiddenField ID="HiddenNumevals" runat="server" Value='<%# Eval("Cuantos") %>' />

                                        <asp:ImageButton ID="ibtninactivar" CommandArgument='<%# Bind("id_contacto")%>'  CommandName="inactivarcoorEval" Visible="false" runat="server" ImageUrl="/Images/icoBorrar.gif" />    

                                        <asp:ImageButton ID="ibtnreactivar" CommandArgument='<%# Bind("id_contacto")%>'  CommandName="reactivarcoorEval" Visible="false" runat="server" ImageUrl="/Images/icoActivar.gif" />
                                        <ajaxToolkit:ConfirmButtonExtender ID="cbe" runat="server"
                                        TargetControlID="ibtnreactivar"
                                        ConfirmText="¿Desea activar este evaluador?"
                                        />

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Nombre"  SortExpression="nombre">
                                <ItemTemplate>
                                    <asp:Button ID="hlnombre" Text='<%# Eval("nombre") %>' CommandArgument='<%# Eval("id_contacto") %>' CommandName="editacontacto" CssClass="boton_Link_Grid" runat="server"  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Correo Electrónico" SortExpression="email">
                                <ItemTemplate>
                                    
                                    <asp:HyperLink ID="hcorreo" runat="server" NavigateUrl = '<%#"mailto:"+ Eval("email") %>' Text = '<%# Eval("email") %>'></asp:HyperLink>

                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Evaluadores" SortExpression="Cuantos">
                                <ItemTemplate>
                                    <asp:Button ID="hlcuantos" Text='<%# Eval("Cuantos") %>' CommandArgument='<%# Eval("id_contacto") %>' CommandName="vercuantos" CssClass="boton_Link_Grid" runat="server"  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Estado" SortExpression="inactividad">
                                <ItemTemplate>
                                     <asp:Button ID="hlestador" Text='<%# Eval("inactividad") %>' CommandArgument='<%# Eval("id_contacto") %>' CommandName="verestador" Enabled='<%# !((int)Eval("inactivo") == 0) %>' CssClass="boton_Link_Grid" runat="server"  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            </Columns>
                        </asp:GridView>

                </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="auto-style3"></td>
                <td class="auto-style4" align="center">
                    <asp:Button ID="btn_asignar" runat="server" CssClass="boton_Link" Text="Asignar Coordinador a Evaluadores" Width="224px" OnClick="btn_asignar_Click" />
                  </td>
                <td class="auto-style5"></td>
              </tr>
              <tr>
                <td class="auto-style1">&nbsp;</td>
                <td class="auto-style2">&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
            </table>

        </ContentTemplate>
    </asp:UpdatePanel>

    </asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1
        {
            width: 5%;
        }
        .auto-style2
        {
            width: 90%;
        }
        .auto-style3
        {
            width: 5%;
            height: 30px;
        }
        .auto-style4
        {
            width: 90%;
            height: 30px;
        }
        .auto-style5
        {
            height: 30px;
        }
    </style>
</asp:Content>

