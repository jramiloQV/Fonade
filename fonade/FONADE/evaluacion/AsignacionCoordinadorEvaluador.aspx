<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AsignacionCoordinadorEvaluador.aspx.cs" Inherits="Fonade.FONADE.evaluacion.AsignacionCoordinadorEvaluador" MasterPageFile="~/Emergente.Master" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        #contenido {
            width: 100%;
            display: inline-block;
            position: relative;
            height: 550px;
        }

        #izquierdo {
            width: 30%;
            overflow: scroll;
            float: left;
        }

        #derecho {
            width: 69%;
            float: right;
            overflow-y: scroll;
        }

        #contenidod {
            position:absolute;
            width:100%;
            height:auto;
        }

        #asignar {
            width: 100%;
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function SelectSingleRadiobutton(rdbtnid) {
            var rdBtn = document.getElementById(rdbtnid);
            var rdBtnList = document.getElementsByTagName("input");
            for (i = 0; i < rdBtnList.length; i++) {
                if (rdBtnList[i].type == "radio" && rdBtnList[i].id != rdBtn.id) {
                    rdBtnList[i].checked = false;
                }
            }
        }
    </script>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <asp:LinqDataSource ID="ldsEvaluadores" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsEvaluadores_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsProyectos" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsProyectos_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsCoordinadores" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsCoordinadores_Selecting"></asp:LinqDataSource>
    <asp:LinqDataSource ID="ldsListaCoordinadores" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="ldsListaCoordinadores_Selecting"></asp:LinqDataSource>

    <div id="contenido">
        <div id="izquierdo">
            <h2>Evaluadores</h2>
            <br />
            <asp:GridView ID="gvEvaluadores" runat="server" AutoGenerateColumns="false" DataSourceID="ldsEvaluadores" Width="100%" ShowHeader="false" CssClass="Grilla"
                OnRowDataBound="gvEvaluadores_RowDataBound" OnRowCommand="gvEvaluadores_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkNombreEvaluador" runat="server" Text='<%# Eval("nombre") %>' CommandArgument='<%# Eval("Id_Contacto") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgAdmiracion" runat="server" ImageUrl="~/Images/admiracion.gif" Visible="false" Enabled="false" CommandArgument='<%# Eval("CodCoordinador") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div id="derecho">
            <div id="contenidod">
                <p>
                    <asp:Label ID="lblinfoEvaluador" runat="server" Text=""></asp:Label>
                </p>
                <br />
                <asp:DataList ID="dtlProyectos" runat="server" Width="100%" Visible="false" DataSourceID="ldsProyectos">
                    <ItemTemplate>
                        <h2>
                            <asp:Label ID="lblProyecto" runat="server" Text='<%# Eval("Id_Proyecto") + " - " + Eval("NomProyecto") %>'></asp:Label></h2>
                        <p>
                            <asp:Label ID="lblSumario" runat="server" Text='<%# Eval("Sumario") %>'></asp:Label>
                        </p>
                    </ItemTemplate>
                </asp:DataList>
                <br />
                <asp:GridView ID="gvrCoordinadorDeEvaluador" runat="server" AutoGenerateColumns="false" CssClass="Grilla" DataSourceID="ldsCoordinadores" Visible="false">
                    <Columns>
                        <asp:BoundField HeaderText="Coordinador de Evaluador" DataField="nombre" />
                        <asp:TemplateField HeaderText="Email">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmail" runat="server" Text='<%# Eval("Email") %>' PostBackUrl='<%# "mailto:" + Eval("Email") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <span id="asignar">
                    <asp:LinkButton ID="lnkAsignarCoordinador" runat="server" Text="ASIGNACIÓN DE COORDINADOR A EVALUADOR" OnClick="lnkAsignarCoordinador_Click" Width="100%" Visible="false"></asp:LinkButton>
                </span>

                <asp:GridView ID="gvrListaCoordinadores" runat="server" AutoGenerateColumns="false" CssClass="Grilla" Visible="false" EmptyDataText="No hay coordinadores activos" DataSourceID="ldsListaCoordinadores" DataKeyNames="Id_Contacto" OnRowDataBound="gvrListaCoordinadores_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="rdbCoordinador" runat="server" GroupName="coor" AutoPostBack="false" OnClick="javascript:SelectSingleRadiobutton(this.id)"  />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Coordinador de Evaluadores" DataField="nombre" />
                    </Columns>
                </asp:GridView>
                <br />
                <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" OnClick="btnActualizar_Click" />
            </div>
        </div>
    </div>
    <br />
</asp:Content>
