<%@ Page Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="ReporteSectores.aspx.cs" Inherits="Fonade.FONADE.evaluacion.ReporteSectores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .auto-style1 {
        width: 100%;
    }
        .cclasecelad {
            text-align:center;
        }

        .mover {
            overflow-x:scroll;
        }

        .sdP_principal {
            background-color:#D2D4D6;
        }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="bodyContentPlace" runat="server">

    <table class="auto-style1">
        <thead>
            <tr>
                <th colspan="2" style="background-color:#00468f; text-align:left; padding-left:50px">
                    <asp:Label ID="L_ReportesEvaluacion" runat="server" ForeColor="White" Text="" Width="260px"></asp:Label>
                </th>
            </tr>
        </thead>
        
        <tr>
            <td>

                <br />
                <br />
                <asp:Panel ID="P_principal" runat="server" CssClass="mover" Width="735px">
                    
                </asp:Panel>
                <br />
                <br />
                <br />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>