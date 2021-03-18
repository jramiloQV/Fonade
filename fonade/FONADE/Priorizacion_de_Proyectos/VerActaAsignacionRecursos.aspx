<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerActaAsignacionRecursos.aspx.cs" Inherits="Fonade.FONADE.Priorizacion_de_Proyectos.VerActaAsignacionRecursos"  MasterPageFile="~/Master.Master" %>
<asp:Content ID="BodyContent"  runat="server" ContentPlaceHolderID="bodyContentPlace">
        <table width="100%">
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
        <table width="100%">
          <tr>
            <td class="style30">&nbsp;</td>
            <td class="style31">
            <asp:GridView ID="gvActaDeAsignacion" DataSourceID="dsActaDeAsignacion" CssClass="Grilla" runat="server" AllowPaging="false" AutoGenerateColumns="false" EmptyDataText="No hay información disponible." Width="100%" >
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
            <td class="style30">&nbsp;</td>
            <td class="style31" align="right">                
                <strong> Total (SMMLV): </strong>
                    &nbsp;
                <asp:Label ID="lblTotalSalariosMinimos" runat="server" Text="0" Visible="true" />
              </td>
            <td>
                &nbsp;
            </td>
          </tr>
          <tr>
            <td class="style30">&nbsp;</td>
            <td class="style31" align="right">
                <strong>Total:</strong>
                &nbsp;
                <asp:Label ID="lblTotalRecursos" runat="server" Text="$0.00"></asp:Label></td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td class="style30">&nbsp;</td>
            <td class="style31" align="center">
                <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" onclick="btn_Imprimir_Click" />
              </td>
            <td>
                &nbsp;
            </td>
          </tr>
        </table>
        <asp:ObjectDataSource ID="dsActaDeAsignacion" runat="server" EnablePaging="false" SelectMethod="getProyectosActa" TypeName="Fonade.FONADE.Priorizacion_de_Proyectos.VerActaAsignacionRecursos" >          
            <SelectParameters>
                <asp:SessionParameter Name="IdActa" SessionField="Id_Acta" />         
            </SelectParameters>
        </asp:ObjectDataSource>
</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .style19
        {
            width: 131px;
        }
        .style25
        {
            width: 152px;
            font-weight: bold;
        }
        .style27
        {
            width: 37px;
        }
        .style28
        {
            width: 721px;
        }
        .style30
        {
            width: 22px;
        }
        .style31
        {
            width: 693px;
        }
    </style>
</asp:Content>
