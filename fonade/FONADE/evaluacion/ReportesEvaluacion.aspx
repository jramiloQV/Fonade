<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReportesEvaluacion.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReportesEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
        .enlace {
            text-decoration: none;
            cursor:pointer
        }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <asp:ObjectDataSource ID="ODS_Convocatoria" runat="server" SelectMethod="Convocatoria" TypeName="Fonade.FONADE.evaluacion.ReportesEvaluacion"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ODS_Departamento" runat="server" SelectMethod="Departamento" TypeName="Fonade.FONADE.evaluacion.ReportesEvaluacion"></asp:ObjectDataSource>

    <asp:ObjectDataSource ID="ODS_Sector" runat="server" SelectMethod="Sector" TypeName="Fonade.FONADE.evaluacion.ReportesEvaluacion"></asp:ObjectDataSource>

    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">

                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="REPORTES DE EVALUACIÓN"></asp:Label>

                </th>
            </tr>
        </thead>
        
        <tr>
            <td>
                <br />
                <table>
                    <thead>
                        <tr>
                            <th colspan="2" style="background-color:#00468f;">

                                <asp:Label ID="Label1" runat="server" ForeColor="White" Text="Evaluadores Asignados por Departamento"></asp:Label>

                            </th>
                        </tr>
                    </thead>
                    <tr style="width:150px;">
                        <td style="text-align:right; padding-right:15px;">
                            <asp:Label ID="L_Convocatoria" runat="server" Text="Convocatoria"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_Convocatoria" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="item1">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DDL_Convocatoria" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item1">Campo Requerido</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr style="width:150px;">
                        <td style="text-align:right; padding-right:15px; vertical-align:top;">
                            <asp:Label ID="L_Departamento" runat="server" Text="Departamento"></asp:Label>
                        </td>
                        <td>
                            <asp:ListBox ID="LB_Departamento" runat="server" Height="100px" Width="200px" DataSourceID="ODS_Departamento" DataTextField="NomDepartamento" DataValueField="Id_Departamento" SelectionMode="Multiple" ValidationGroup="item1"></asp:ListBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="LB_Departamento" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item1">Campo Requerido</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="B_Buscar1" runat="server" Text="Buscar" OnClick="B_Buscar1_Click" ValidationGroup="item1" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <table style="width:100%;">
                    <thead>
                        <tr>
                            <th colspan="2" style="background-color:#00468f;">

                                <asp:Label ID="Label2" runat="server" ForeColor="White" Text="Consolidado por Sector"></asp:Label>

                            </th>
                        </tr>
                    </thead>
                    <tr style="width:100%;">
                        <td style="text-align:right; padding-right:15px; width:25%;">
                            <asp:Label ID="Label3" runat="server" Text="Convocatoria"></asp:Label>
                            <br />
                        </td>
                        <td style=" width:75%;">
                            <asp:DropDownList ID="DDL_ConvocatoriaSector" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="item2">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DDL_ConvocatoriaSector" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item2">Campo Requerido</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr style="width:100%;">
                        <td style="text-align:left; padding-left:100px; width:100%" colspan="2">
                            <asp:RadioButtonList ID="RB_Viavilidad" runat="server" RepeatDirection="Horizontal" Width="300px" ValidationGroup="item2">
                                <asp:ListItem Value="1">Viables</asp:ListItem>
                                <asp:ListItem Value="2">No Viables</asp:ListItem>
                                <asp:ListItem Selected="True" Value="3">Todos</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="RB_Viavilidad" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item2">Campo Requerido</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr style="width:100%;">
                        <td style="text-align:right; padding-right:15px; vertical-align:top; width:25%;">
                            <asp:Label ID="Label4" runat="server" Text="Departamento"></asp:Label>
                        </td>
                        <td style=" width:75%;">
                            <asp:ListBox ID="LB_CIIU" runat="server" Height="100px" Width="95%" DataSourceID="ODS_Sector" DataTextField="NomSector" DataValueField="Id_Sector" SelectionMode="Multiple" ValidationGroup="item2"></asp:ListBox>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LB_CIIU" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item2">Campo Requerido</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="B_Buscar2" runat="server" Text="Buscar" OnClick="B_Buscar2_Click" ValidationGroup="item2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <table>
                    <thead>
                        <tr>
                            <th colspan="2" style="background-color:#00468f;">

                                <asp:Label ID="Label5" runat="server" ForeColor="White" Text="# Evaluadores Asignados por Sector"></asp:Label>

                            </th>
                        </tr>
                    </thead>
                    <tr style="width:150px;">
                        <td style="text-align:right; padding-right:15px;">
                            <asp:Label ID="Label6" runat="server" Text="Convocatoria"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_ConvocatoriaEvaluadoresSector" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="item4">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="DDL_ConvocatoriaEvaluadoresSector" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item4">Campo Requerido</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="B_Buscar3" runat="server" Text="Buscar" OnClick="B_Buscar3_Click" ValidationGroup="item4" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <table>
                    <thead>
                        <tr>
                            <th colspan="2" style="background-color:#00468f;">

                                <asp:Label ID="Label7" runat="server" ForeColor="White" Text="Fechas de Asignación"></asp:Label>

                            </th>
                        </tr>
                    </thead>
                    <tr style="width:150px;">
                        <td style="text-align:right; padding-right:15px;">
                            <asp:Label ID="Label8" runat="server" Text="Convocatoria"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_ConvocatoriaFechas" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="DDL_ConvocatoriaFechas" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item3">Campo Requerido</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="B_Buscar4" runat="server" Text="Buscar" OnClick="B_Buscar4_Click" ValidationGroup="item3" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <table>
                    <thead>
                        <tr>
                            <th colspan="2" style="background-color:#00468f;">

                                <asp:Label ID="Label9" runat="server" ForeColor="White" Text="Empleos Proyectos Viables por Evaluador"></asp:Label>

                            </th>
                        </tr>
                    </thead>
                    <tr style="width:150px;">
                        <td style="text-align:right; padding-right:15px;">
                            <asp:Label ID="Label10" runat="server" Text="Convocatoria"></asp:Label>
                            <br />
                        </td>
                        <td>
                            <asp:DropDownList ID="DDL_Empleos" runat="server" Width="400px" DataSourceID="ODS_Convocatoria" DataTextField="NomConvocatoria" DataValueField="Id_Convocatoria" ValidationGroup="item5">
                            </asp:DropDownList>
                            <br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="DDL_ConvocatoriaFechas" ErrorMessage="RequiredFieldValidator" ForeColor="Red" ValidationGroup="item5">Campo Requerido</asp:RequiredFieldValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LB_DepartamentoSector" runat="server" CssClass="enlace" ForeColor="Gray" Font-Bold="true" OnClick="LB_DepartamentoSector_Click" ValidationGroup="item5">- Departamento y Sector</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LB_Consolidado" runat="server" CssClass="enlace" ForeColor="Gray" Font-Bold="true" OnClick="LB_Consolidado_Click" ValidationGroup="item5">- Consolidado Nacional</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="LB_ConsolidadoNacionalSector" runat="server" CssClass="enlace" ForeColor="Gray" Font-Bold="true" OnClick="LB_ConsolidadoNacionalSector_Click" ValidationGroup="item5">- Consolidado Nacional Sector</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>