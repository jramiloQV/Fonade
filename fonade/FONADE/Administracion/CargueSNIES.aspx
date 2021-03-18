<%@ Page Language="C#" MasterPageFile="~/Master.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CargueSNIES.aspx.cs" Inherits="Fonade.FONADE.Administracion.CargueSNIES" %>

<asp:Content ID="head1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        table {
            width: 100%;
        }

        td {
            vertical-align: top;
        }

        .formatoTabla {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="bodyContentPlace">
    <h1>
        <label>CARGUE SNIES</label>
    </h1>
    <br />
    <br />
    <asp:Panel ID="pnlCargar" runat="server">
        <table>
            <tr>
                <td style="text-align: center;">
                    <asp:LinqDataSource ID="lds_nivelestduio" runat="server" ContextTypeName="Datos.FonadeDBDataContext" OnSelecting="lds_nivelestduio_Selecting"></asp:LinqDataSource>
                    <asp:DropDownList ID="ddl_nivelestudio" runat="server" DataSourceID="lds_nivelestduio" DataValueField="Id_NivelEstudio" DataTextField="NomNivelEstudio"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <br />
                    Seleccione el archivo:
                </td>
            </tr>
            <tr>
                <td>
                    <asp:FileUpload ID="fld_cargar" runat="server" Width="400px" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btncargar" runat="server" Text="Subir Archivo" OnClick="btncargar_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <p>
                        Solo se debe subir el archivo de SNIES. Con el siguiente formato (colocar solo las filas de valores,
                        incluir una sola fila de titulos, el nombre de la hoja electrónica debe ser
                        cargue.):
                    </p>
                </td>
            </tr>
        </table>

        <br />
        <br />

        <table class="Grilla formatoTabla">
            <thead>
                <tr>
                    <th>Numero Campo</th>
                    <th>Nombre Campo</th>
                    <th>Ejemplo</th>
                </tr>
            </thead>

            <tbody>

                <tr>
                    <td>1</td>
                    <td>IES_CODE</td>
                    <td>1117</td>
                </tr>
                <tr>
                    <td>2</td>
                    <td>IES_NOMBRE</td>
                    <td>UNIVERSIDAD MILITAR-NUEVA GRANADA</td>
                </tr>
                <tr>
                    <td>3</td>
                    <td>NOMBRE</td>
                    <td>Registro Calificado</td>
                </tr>
                <tr>
                    <td>4</td>
                    <td>PRO_CONSECUTIVO</td>
                    <td>52793</td>
                </tr>
                <tr>
                    <td>5</td>
                    <td>PROG_NOMBRE
                    </td>
                    <td>ESPECIALIZACION EN NEUROLOGIA PEDIATRICA PARA ESPECIALISTAS EN PEDIATRIA</td>
                </tr>
                <tr>
                    <td>6</td>
                    <td>ESTADO_PROG_DESCR
                    </td>
                    <td>ACTIVO</td>
                </tr>
                <tr>
                    <td>7</td>
                    <td>METODOLOGIA_DESCR</td>
                    <td>Presencial</td>
                </tr>
                <tr>
                    <td>8</td>
                    <td>MUNIC_DESCR</td>
                    <td>SANTAFE DE BOGOTA</td>
                </tr>
                <tr>
                    <td>9</td>
                    <td>DEPTO_DESCR</td>
                    <td>BOGOTA D.C</td>
                </tr>
                <tr>
                    <td>10</td>
                    <td>NIVEL_DESCR (Opcional, ya que se toma el nivel seleccionado en la lista desplegable)</td>
                    <td>POSGRADO</td>
                </tr>
            </tbody>
        </table>

    </asp:Panel>
</asp:Content>
