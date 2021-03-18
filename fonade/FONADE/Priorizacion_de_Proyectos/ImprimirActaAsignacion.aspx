<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImprimirActaAsignacion.aspx.cs" Inherits="Fonade.FONADE.Priorizacion_de_Proyectos.ImprimirActaAsignacion"  MasterPageFile="~/MasterImpr.Master"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <table width="650px">
          <tr>
            <td colspan="4">&nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td colspan="2" >
                <h1>
                    <asp:Label runat="server" ID="lblTitulo" Text="Ver acta de asignación de recursos" style="font-weight: 700"></asp:Label>
                </h1></td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style25" valign="baseline">Número de Acta:</td>
            <td class="style28" valign="baseline">
                <asp:Label ID="lblNumero" runat="server"></asp:Label>
              </td>
            <td>
                &nbsp;
            </td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style25" valign="baseline">Nombre del Acta:</td>
            <td class="style28" valign="baseline">
                <asp:Label ID="lblNombre" runat="server"></asp:Label>
              </td>
            <td>
                &nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style25" valign="baseline">Fecha del Acta:</td>
            <td class="style28" valign="baseline">
                <asp:Label ID="lblFecha" runat="server"></asp:Label>
            </td>
            <td>
                &nbsp;</td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td class="style25" valign="baseline">Convocatoria:</td>
            <td class="style28" valign="baseline">
                <asp:Label ID="lblConvocatoria" runat="server"></asp:Label>
              </td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style27"></td>
            <td class="style25" valign="bottom">Observaciones:</td>
            <td class="style28"></td>
            <td class="style19"></td>
          </tr>
          <tr>
            <td class="style27">&nbsp;</td>
            <td colspan="2">
                <asp:TextBox ID="txtObservaciones" runat="server" Height="140px" TextMode="MultiLine" Width="440px" BackColor="White" ReadOnly="True"></asp:TextBox>
              </td>
            <td>
                &nbsp;
            </td>
          </tr>
          <tr>
            <td class="style27"></td>
            <td colspan="4">
                <asp:Label ID="lblError" Text="Sucedio un error." runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Red" Visible="false"></asp:Label>
            </td>
          </tr>
        </table>
        <table width="650px">
          <tr>
            <td class="style30">&nbsp;</td>
            <td class="style31">
            <asp:GridView ID="gvActaDeAsignacion" DataSourceID="dsActaDeAsignacion" CssClass="Grilla" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="No hay información disponible." width="650px">
                <Columns>
                    <asp:TemplateField HeaderText="Código del proyecto">
                        <ItemTemplate>
                            <asp:Label ID="lblCodigoProyecto" runat="server" Text='<%# Eval("Codigo") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Plan de Negocio">
                        <ItemTemplate>
                            <asp:Label ID="lblNombreProyecto" runat="server" Text='<%# Eval("Nombre") %>'></asp:Label>
                        </ItemTemplate> 
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recursos (SMMLV)" >
                        <ItemTemplate>                            
                            <asp:Label ID="lblSalarios" runat="server" Text='<%# Eval("ValorRecomendado") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Legalizado" >
                        <ItemTemplate>
                            <asp:Label ID="lblAsignado" runat="server" Text='<%# Eval("Recursos") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </td>
            <td> &nbsp; </td>
          </tr>
          <tr>
            <td >&nbsp;</td>
            <td align="right">                
                <strong> Total (SMMLV): </strong>
                    &nbsp;
                <asp:Label ID="lblTotalSalariosMinimos" runat="server" Text="0" Visible="true" />
              </td>
            <td>
                &nbsp;
            </td>
          </tr>
          <tr>
            <td >&nbsp;</td>
            <td  align="right">
                <strong>Total:</strong>
                &nbsp;
                <asp:Label ID="lblTotalRecursos" runat="server" Text="$0.00"></asp:Label></td>
            <td>&nbsp;</td>
          </tr>          
        </table>
        <asp:ObjectDataSource ID="dsActaDeAsignacion" runat="server" EnablePaging="false" SelectMethod="getProyectosActa" TypeName="Fonade.FONADE.Priorizacion_de_Proyectos.VerActaAsignacionRecursos" >          
            <SelectParameters>
                <asp:SessionParameter Name="IdActa" SessionField="Id_Acta" />         
            </SelectParameters>
        </asp:ObjectDataSource>
                
            <table width="650px">
              <tr>
                <td class="style11">&nbsp;</td>
                <td class="style18"><strong>Aprobó:</strong></td>
                <td class="style18">&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style21"></td>
                <td class="style22" valign="bottom">___________________________________</td>
                <td class="style22" valign="bottom">___________________________________</td>
                <td class="style23"></td>
              </tr>
              <tr>
                <td class="style11">&nbsp;</td>
                <td class="style20">Subgerente Financiero</td>
                <td class="style20">Subgerente Técnico</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style21"></td>
                <td class="style29" valign="bottom">______________________________</td>
                <td class="style29" valign="bottom">______________________________</td>
                <td class="style23"></td>
              </tr>
              <tr>
                <td class="style11">&nbsp;</td>
                <td class="style20">Coordinador Grupo de Ejecución y Liquidación de Convenios</td>
                <td class="style20" valign="top">Gerente Unidad Crédito y Cartera</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style21"></td>
                <td class="style29" valign="bottom">______________________________</td>
                <td class="style29"></td>
                <td class="style23"></td>
              </tr>
              <tr>
                <td class="style11">&nbsp;</td>
                <td class="style20">Gerente de Convenio Fondo Emprender</td>
                <td class="style20">&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style11">&nbsp;</td>
                <td class="style18">&nbsp;</td>
                <td class="style18">&nbsp;</td>
                <td>&nbsp;</td>
              </tr>
            </table>    
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style11
        {
            width: 16px;
        }
        .style18
        {
            width: 395px;
        }
        .style20
        {
            width: 395px;
            font-weight: bold;
        }
        .style21
        {
            width: 16px;
            height: 80px;
        }
        .style22
        {
            width: 395px;
            height: 80px;
        }
        .style23
        {
            height: 80px;
        }
        .style28
        {
            width: 395px;
            height: 27px;
        }
        .style29
        {
            width: 395px;
            font-weight: bold;
            height: 80px;
        }
        .style30
        {
            height: 32px;
        }
        .style31
        {
            height: 35px;
        }
        .style33
        {
            width: 141px;
            font-weight: bold;
            height: 24px;
        }
        .style35
        {
            width: 12px;
            height: 27px;
        }
        .style36
        {
            width: 20px;
            font-weight: bold;
            height: 26px;
        }
        .style37
        {
            width: 364px;
        }
    </style>
</asp:Content>
