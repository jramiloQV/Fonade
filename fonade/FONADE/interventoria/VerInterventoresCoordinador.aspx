<%@ Page Title="FONDO EMPRENDER" Language="C#" AutoEventWireup="true" CodeBehind="VerInterventoresCoordinador.aspx.cs" Inherits="Fonade.FONADE.interventoria.VerInterventoresCoordinador" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <style type="text/css">
         table {
             width: 100%;
         }
        .celdaest {
            text-align:center;
        }
     </style>
    <link href="../../Styles/siteProyecto.css" rel="stylesheet" />
    <script language="javascript">
        window.onload = zoom;
        function zoom()
        {
          self.moveTo(50,50);
          self.resizeTo(650, 500);
        }
    </script>
</head>
<body>
       <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"> </asp:ToolkitScriptManager>
    <div style="width:600px; height:auto;">
            <table width="500px">
              <tr>
                <td class="auto-style1" >
                    <h1><asp:Label runat="server" ID="lbl_Titulo" style="font-weight: 700"></asp:Label></h1>
                </td>
                <td align="right">
                    <asp:Label ID="l_fechaActual" runat="server" style="font-weight: 700"></asp:Label>
                </td>
              </tr>
            </table>

            <table width="600">
              <tr>
                <td class="style10">&nbsp;</td>
                <td class="style11">                   
                <asp:Panel ID="Panel1" runat="server" Width="100%">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="Grilla" Visible="false">
                        <Columns>
                            <asp:BoundField HeaderText="Empresa" DataField="razonsocial" />
                            <asp:BoundField HeaderText="Rol" DataField="rol" />
                            <asp:BoundField HeaderText="Fecha" DataField="FechaInicio" DataFormatString="{0:MMM-dd-yyyy}" />
                            <%--<asp:TemplateField HeaderText="Nombre Interventor">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreIner" runat="server" Text='<%# Eval("Nombres") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Empresa">
                                <ItemTemplate> 
                                    <asp:Label ID="lempresa" runat="server" Text='<%# EmpresasByInterventor(Eval("ID").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <%--<asp:TemplateField HeaderText="Rol">
                                <ItemTemplate> 
                                    <asp:Label ID="lrol" runat="server" Text='Interventor Lider'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate> 
                                    <asp:Label ID="lfecha" runat="server" Text='<%# Eval("FechaInicio", "{0:dd/MM/yyyy}")%>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>

                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="Grilla" Visible="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Nombre Interventor">
                                <ItemTemplate>
                                    <asp:Label ID="lblNombreIner" runat="server" Text='<%# Eval("Nombres") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Empresa">
                                <ItemTemplate> 
                                    <asp:Label ID="lempresa" runat="server" Text='<%# EmpresasByInterventor(Eval("ID").ToString()) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    </asp:Panel>
                    </td>
                <td>&nbsp;</td>
              </tr>
              <tr>
                <td class="style12"></td>
                <td class="style13" align="center">
                    <asp:Button ID="btn_cerrar" runat="server" onclick="btn_cerrar_Click" 
                        Text="Cerrar" />
                  </td>
                <td class="style14"></td>
              </tr>

            </table>

    </div>
    </form>

   </body>
</html>

