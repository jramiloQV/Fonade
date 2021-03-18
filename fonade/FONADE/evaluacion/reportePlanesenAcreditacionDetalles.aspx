<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="reportePlanesenAcreditacionDetalles.aspx.cs" Inherits="Fonade.FONADE.evaluacion.reportePlanesenAcreditacionDetalles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }

        .claseTitulos {
            background-color: #D1D8E2;
            text-align:center;
            color:black;
        }

        .clasesubtitulos {
            background-color:#EDEFF3;
            text-align:center;
            color:black;
        }

</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">
    <asp:ObjectDataSource ID="ODS_Convocatoria" runat="server" SelectMethod="Convocatoria" TypeName="Fonade.FONADE.evaluacion.reportePlanesenAcreditacion"></asp:ObjectDataSource>
    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="Reporte Planes de acreditación" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr>
            <td style="text-align:center;">
                <table style="width:100%">
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td style="width:50%">
                            <asp:Label ID="Label1" runat="server" Text="Nombre de la convocatoria : "></asp:Label>
                        </td>
                        <td style="width:50%">
                            <asp:Label ID="L_NomConvocatoria" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:50%">
                            <asp:Label ID="Label2" runat="server" Text="Recursos : "></asp:Label>
                        </td>
                        <td style="width:50%">
                            <asp:Label ID="L_Recursos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width:100%;">
                            <br />
                            <br />
                            <div style="overflow:scroll">
                                <asp:Panel ID="P_Tabla" runat="server" Width="735px">

                                </asp:Panel>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="2" style="width:100%;">
                            <br />
                            <br />
                            <div style="overflow:scroll">
                                <asp:Button ID="B_Excel" runat="server" Text="Exportar excel" OnClick="B_Excel_Click" />
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

</asp:Content>