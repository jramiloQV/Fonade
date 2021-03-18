<%@ Page Language="C#" MasterPageFile="~/Master.master"  EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ReportesBI.aspx.cs" Inherits="Fonade.FONADE.interventoria.ReportesBI" %>

 <asp:Content  ID="head1"   ContentPlaceHolderID="head" runat="server">
     <style type="text/css">
         table {
             width:100%;
             text-align:left;
         }

         td {
             width:50%;
         }

         .calass {
             text-decoration:none;
             cursor:pointer;
             color:black;
         }
     </style>
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:Panel ID="principal" runat="server">
        <h1>
            <asp:Label ID="L_titulo" runat="server" Text="">

            </asp:Label>
        </h1>

        <br />
        <br />

        <table>
            <tr>
                <td>
                    <asp:LinkButton ID="LB_InformeVisitaInter" runat="server" Text="Visita de Interventoría" CssClass="calass" PostBackUrl="~/FONADE/interventoria/InformeVisitaInter.aspx"></asp:LinkButton>
                    <br />
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="LB_InformeBimensualInter" runat="server" Text="Avance Bimensual" CssClass="calass" PostBackUrl="~/FONADE/interventoria/InformeBimensualInter.aspx"></asp:LinkButton>
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="LB_InformeEjecucionInter" runat="server" Text="Ejecución Presupuestal" CssClass="calass" PostBackUrl="~/FONADE/interventoria/InformeEjecucionInter.aspx"></asp:LinkButton>
                    <br />
                    <br />
                </td>
                <td>
                    <asp:LinkButton ID="LB_InformeConsolidadoInter" runat="server" Text="Consolidado Interventoría" CssClass="calass" PostBackUrl="~/FONADE/interventoria/InformeConsolidadoInter.aspx"></asp:LinkButton>
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>