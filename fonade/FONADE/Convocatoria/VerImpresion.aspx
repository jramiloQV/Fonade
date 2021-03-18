<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerImpresion.aspx.cs" Inherits="Fonade.FONADE.Convocatoria.VerImpresion" MasterPageFile="~/MasterImpr.Master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

        <asp:LinqDataSource ID="lds_equipo" runat="server" 
        ContextTypeName="Datos.FonadeDBDataContext" AutoPage="true" 
        onselecting="lds_equipo_Selecting" >
        </asp:LinqDataSource>

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>

    <asp:UpdatePanel ID="P_infoProyecto" runat="server" Visible="true" 
        Width="660px" UpdateMode="Conditional" RenderMode="Inline">
    <ContentTemplate>
          <table width="650px">
              <tr>
                <td class="style37">
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>
        <table width="650px">
           <tr>
            <td class="style25"></td>
            <td colspan="2" class="style27">
                <hr />
              </td>

            <td class="style27"></td>
          </tr>
          <tr>
            <td class="style29"></td>
            <td class="style30" valign="top">Nombre:</td>
            <td class="style31" valign="top">
                <asp:Label ID="l_nombre" runat="server"></asp:Label>
              </td>
            <td class="style32"></td>
          </tr>
          <tr>
            <td class="style29"></td>
            <td class="style30" valign="top">Institución:</td>
            <td class="style31" valign="top">
                <asp:Label ID="l_institucion" runat="server"></asp:Label>
              </td>
            <td class="style32"></td>
          </tr>
          <tr>
            <td class="style29"></td>
            <td class="style30" valign="top">Subsector:</td>
            <td class="style31" valign="top">
                <asp:Label ID="l_subsector" runat="server"></asp:Label>
              </td>
            <td class="style32"></td>
          </tr>
          <tr>
            <td class="style29"></td>
            <td class="style30" valign="top">Ciudad:</td>
            <td class="style31" valign="top">
                <asp:Label ID="l_ciudad" runat="server"></asp:Label>
              </td>
            <td class="style32"></td>
          </tr>
          <tr>
            <td class="style29"></td>
            <td class="style30" valign="top">Recursos solicitados al fondo:</td>
            <td class="style31" valign="top">
                <asp:Label ID="l_recursos" runat="server"></asp:Label>
              </td>
            <td class="style32"></td>
          </tr>
          <tr>
            <td class="style20">&nbsp;</td>
            <td class="style23" valign="top">Fecha de creación:</td>
            <td class="style24" valign="top">
                <asp:Label ID="l_fecha" runat="server"></asp:Label>
              </td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style25"></td>
            <td colspan="2" class="style27">
                <hr />
              </td>

            <td class="style27"></td>
          </tr>
          <tr>
            <td class="style20">&nbsp;</td>
            <td class="style23" valign="top">Sumario:</td>
            <td class="style24" valign="top">
                <asp:Label ID="l_sumario" runat="server"></asp:Label>
              </td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style25"></td>
            <td colspan="2" class="style27">
                <hr />
              </td>

            <td class="style27"></td>
          </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server"  width="650px">
            <table width="100%">
              <tr>
                <td colspan="3" style="color: #FFFFFF; background-color: #666666" class="style28"><strong>Resumen Ejecutivo</strong></td>

              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Concepto del Negocio</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><asp:Literal ID="lit_conceptoNegocios" runat="server"></asp:Literal></td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Potencial del Mercado en Cifras</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                    <asp:Literal ID="lit_potencial" runat="server"></asp:Literal>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Ventajas Competitivas y Propuesta de Valor</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                    <asp:Literal ID="lit_ventajas" runat="server"></asp:Literal>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Resumen de las Inversiones Requeridas</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                    <asp:Literal ID="lit_inversiones" runat="server"></asp:Literal>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Proyecciones de Ventas y Rentabilidad</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                    <asp:Literal ID="lit_proyecciones" runat="server"></asp:Literal>
                  </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33"><strong>Conclusiones Financieras y Evaluación de Viabilidad</strong><hr /> </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                    <asp:Literal ID="lit_conclusiones" runat="server"></asp:Literal>
                  </td>
                <td>&nbsp;</td>
              </tr>
               <tr>
                <td class="style20">&nbsp;</td>
                <td class="style33">
                  </td>
                <td>&nbsp;</td>
              </tr>
            </table>
            
        </asp:Panel>

        <table width="650px">
          <tr>
            <td class="style28" style="color: #FFFFFF; background-color: #666666"><strong>Equipo 
                de Trabajo</strong></td>
          </tr>
          <tr>
            <td align="center">&nbsp;<asp:GridView ID="gv_equipotrabajo"  CssClass="Grilla2" runat="server" Width="600px"
                    AllowPaging="false" AutoGenerateColumns="false" DataSourceID="lds_equipo"
                    EmptyDataText="No hay información disponible."  >
                        <Columns>
                            <asp:TemplateField HeaderText="Nombre">
                                <ItemTemplate>
                                    <asp:Label ID="lnombres" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="e-mail">
                                <ItemTemplate>
                                    <asp:Label ID="lemail" runat="server" Text='<%# Eval("email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rol">
                                <ItemTemplate>
                                    <asp:Label ID="lrol" runat="server" Text='<%# Eval("rol") %>'></asp:Label>
                                </ItemTemplate> 
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView></td>
          </tr>
          <tr>
            <td>&nbsp;</td>
          </tr>
        </table>

    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style20
        {
            width: 35px;
        }
        .style23
        {
            width: 174px;
            font-weight: bold;
        }
        .style24
        {
            width: 395px;
        }
        .style25
        {
            width: 35px;
            height: 13px;
        }
        .style27
        {
            height: 13px;
        }
        .style28
        {
            font-size: medium;
            height: 29px;
        }
        .style29
        {
            width: 35px;
            height: 23px;
        }
        .style30
        {
            width: 174px;
            font-weight: bold;
            height: 23px;
        }
        .style31
        {
            width: 395px;
            height: 23px;
        }
        .style32
        {
            height: 23px;
        }
        .style33
        {
            width: 574px;
        }
    </style>
</asp:Content>
