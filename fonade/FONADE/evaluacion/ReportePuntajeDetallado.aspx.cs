#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>16 - 03 - 2014</Fecha>
// <Archivo>ReportePuntajeDetallado.aspx.cs</Archivo>

#endregion

#region

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI.WebControls;
using Fonade.Negocio;
using System.Web;
using System.IO;
using System.Web.UI;
using Datos;

#endregion

namespace Fonade
{
    public partial class ReportePuntajeDetallado : Base_Page
    {
        Consultas consultas = new Consultas();
        int codConvocatoria;
        int puntajeMinimo;
        string txtSql, NombreDpto;
        #region Variables
        private double _subTotalSolicitado, _subTotalRecomendado, _subTotalRecomendadoAprobados, _subTotalRecomendadoViables
                       , _subTotalAprobados, _subTotalViables, _subTotalAvalados, _subTotalProyectos, _totalProyectos
                       , _totalSolicitado, _totalRecomendado, _totalRecomendadoAprobados, _totalRecomendadoViables,
                       _totalAprobados, _totalViables, _totalAvaslados;

        private int _numAspectos = 5, PuntajeAspecto, PuntajeTotal, PuntajeComercial, PuntajeTecnico, PuntajeOrganizacional, PuntajeFinanciero, PuntajeMedioAmbiente;

        #endregion
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["codConvocatoria"] != null)
            {
                codConvocatoria = int.Parse(Session["codConvocatoria"].ToString());
                CargarGrid();
                Session["codConvocatoria"] = null;
            }
            else
            {
                 codConvocatoria = int.Parse(Session["codExcel"].ToString());
                //DtlReporteDetallado.Visible = false;
                CargarGrid();
                Session["codExcel"] = null;
                exportar();
            }
        }
        #endregion

        #region Metodos
        private void CargarGrid()
        {
            txtSql = "SELECT SUM(puntaje) FROM convocatoriacampo cc, campo c WHERE id_campo=cc.codcampo AND c.codcampo IS NULL ";
            txtSql += "AND codconvocatoria = " + codConvocatoria;
            if (codConvocatoria > 1)
                txtSql += " AND id_Campo<>6";
            var dt = consultas.ObtenerDataTable(txtSql, "text");

            if (dt.Rows.Count > 0)
                puntajeMinimo = int.Parse((dt.Rows[0].ItemArray[0].ToString() != "" ? dt.Rows[0].ItemArray[0].ToString() : "0"));

            var convocatoria = (from c in consultas.Db.Convocatoria
                                where c.Id_Convocatoria == codConvocatoria
                                select c).FirstOrDefault();
            var anioConvoca = convocatoria.FechaFin.Year;
            lblTituloConvocatoria.Text = convocatoria.NomConvocatoria;
            string viable;
            if (Request["viable"] != null)
            {
                viable = Request["viable"].ToString() + " And ";
            }
            else
            {
                viable = string.Empty;
            }

            txtSql = "select id_proyecto, nomproyecto, nomciudad, d.NomDepartamento, nominstitucion, nomunidad, isnull(recursos,0) as montosolicitado, ";
            txtSql += " round (isnull (valorrecomendado,0),0) as montorecomendado, case when isnull(viable, 0)=1 then 'SI' else 'NO' end as viable from proyecto ";
            txtSql += "inner join convocatoriaproyecto cp on id_proyecto=cp.codproyecto and " + viable + " codconvocatoria= " + codConvocatoria;
            txtSql += " inner join ciudad on id_ciudad=codciudad inner join departamento d on id_departamento=coddepartamento ";
            txtSql += "inner join subsector on id_subsector = codsubsector inner join sector on id_sector = codsector ";
            txtSql += "inner join institucion on id_institucion = codinstitucion left join proyectofinanzasingresos pfi on id_proyecto=pfi.codproyecto ";
            txtSql += "left join evaluacionobservacion ev on id_proyecto=ev.codproyecto and ev.codconvocatoria = cp.codconvocatoria ";
            txtSql += "order by nomdepartamento ";

            dt = consultas.ObtenerDataTable(txtSql, "text");

            if (dt.Rows.Count > 0)
            {
                string[] encabezados = new string[51];
                encabezados[0] = "Municipio"; encabezados[1] = "Unidad de Emprendimiento"; encabezados[2] = "ID"; encabezados[3] = "Plan de Negocio";
                encabezados[4] = "Viable"; encabezados[5] = "Valor Solicitado"; encabezados[6] = "Valor Recomendado"; encabezados[7] = "A";
                encabezados[8] = "B"; encabezados[9] = "C"; encabezados[10] = "D"; encabezados[11] = "E"; encabezados[12] = "GENERALES";
                encabezados[13] = "F"; encabezados[14] = "G"; encabezados[15] = "H"; encabezados[16] = "I"; encabezados[17] = "J";
                encabezados[18] = "K"; encabezados[19] = "L"; encabezados[20] = "M"; encabezados[21] = "N"; encabezados[22] = "O";
                encabezados[23] = "P"; encabezados[24] = "Q"; encabezados[25] = "R"; encabezados[26] = "S"; encabezados[27] = "COMERCIALES";
                encabezados[28] = "T"; encabezados[29] = "U"; encabezados[30] = "V"; encabezados[31] = "W"; encabezados[32] = "X";
                encabezados[33] = "Y"; encabezados[34] = "Técnicos"; encabezados[35] = "Z"; encabezados[36] = "AA"; encabezados[37] = "AB";
                encabezados[38] = "AC"; encabezados[39] = "AD"; encabezados[40] = "AE"; encabezados[41] = "AF"; encabezados[42] = "ORGANIZACIONALES";
                encabezados[43] = "AG"; encabezados[44] = "AH"; encabezados[45] = "AI"; encabezados[46] = "AJ"; encabezados[47] = "AK";
                encabezados[48] = "FINANCIEROS"; encabezados[49] = "Medio Ambiente"; encabezados[50] = "Puntaje Total";

                var rowHead = new TableRow();
                for(var i=0; i<= 50; i++)
                {
                    var cell = new TableCell
                    {
                        Text = encabezados[i]
                    };
                    cell.Font.Bold = true;
                    cell.ForeColor = System.Drawing.Color.DarkBlue;
                    rowHead.Cells.Add(cell);
                }
                rowHead.TableSection = TableRowSection.TableHeader;
                tblReporte.Rows.Add(rowHead);

                int numAspectos = 6;

                foreach(DataRow fila in dt.Rows)
                {
                    var row = new TableRow();
                    PuntajeTotal = 0;
                    if(NombreDpto != fila["NomDepartamento"].ToString())
                    {
                        NombreDpto = fila["NomDepartamento"].ToString();
                        _totalSolicitado = 0;
                        _subTotalRecomendado = 0;
                        _subTotalRecomendadoAprobados = 0;
                        _subTotalRecomendadoViables = 0;
                        _subTotalProyectos = 0;
                        _subTotalAprobados = 0;
                        _subTotalViables = 0;
                        _subTotalAvalados = 0;
                    }

                    var cell = new TableCell();
                    cell.Text = fila["NomCiudad"].ToString() + " (" + fila["NomDepartamento"].ToString() + ")";
                    row.Cells.Add(cell);

                    var cell2 = new TableCell();
                    cell2.Text = fila["NomUnidad"].ToString() + " (" + fila["NomInstitucion"].ToString() + ")";
                    row.Cells.Add(cell2);

                    var cell3 = new TableCell();
                    cell3.Text = fila["Id_Proyecto"].ToString();
                    row.Cells.Add(cell3);

                    var cell4 = new TableCell();
                    cell4.Text = fila["NomProyecto"].ToString();
                    row.Cells.Add(cell4);

                    var cell5 = new TableCell();
                    cell5.Text = fila["viable"].ToString();
                    row.Cells.Add(cell5);

                    var cell6 = new TableCell();
                    cell6.Text = fila["montosolicitado"].ToString();
                    row.Cells.Add(cell6);

                    var cell7 = new TableCell();
                    cell7.Text = fila["montorecomendado"].ToString();
                    row.Cells.Add(cell7);

                    for(var j = 1; j< numAspectos; j++)
                    {
                        txtSql = "select puntaje from evaluacioncampo ec ";
                        txtSql += "inner join campo c on c.id_campo = ec.codcampo ";
                        txtSql += "inner join campo v on v.id_campo = c.codcampo ";
                        txtSql += "inner join campo a on a.id_campo = v.codcampo ";
                        txtSql += "where codproyecto=" + fila["id_proyecto"].ToString() + " and codconvocatoria=" + codConvocatoria + " and a.id_campo=" + j;
                        txtSql += " order by a.id_campo,v.campo Asc";
                        
                        var dtAspepcto = consultas.ObtenerDataTable(txtSql, "text");

                        PuntajeAspecto = 0;

                        foreach(DataRow f in dtAspepcto.Rows)
                        {
                            if(j  < 6)
                            {
                                var cellA = new TableCell();
                                cellA.Text = f["Puntaje"].ToString();
                                row.Cells.Add(cellA);
                            }
                            if(!string.IsNullOrEmpty(f["Puntaje"].ToString()))
                            {
                                PuntajeAspecto += int.Parse(f["Puntaje"].ToString());
                                if(j < 6 )
                                {
                                    PuntajeTotal += int.Parse(f["Puntaje"].ToString());
                                }
                            }
                        }
                        dtAspepcto = null;

                        var cellb= new TableCell();
                        cellb.Text = PuntajeAspecto.ToString();
                        row.Cells.Add(cellb);
                    }
                    var cellViable = new TableCell();
                    if (fila["viable"].ToString() == "SI")
                    {
                        cellViable.Text = "1";
                    }
                    else
                    {
                        cellViable.Text = "0";
                    }


                    row.Cells.Add(cellViable);
                    var cell8 = new TableCell();
                    cell8.Text = PuntajeTotal.ToString();
                    row.Cells.Add(cell8);

                    if(fila["viable"].ToString() == "SI")
                    {
                        _subTotalViables += 1;
                        _totalViables += 1;

                        _subTotalRecomendadoViables += int.Parse(fila["montorecomendado"].ToString());
                        _totalRecomendadoViables += int.Parse(fila["montorecomendado"].ToString());
                    }

                    txtSql = "SELECT isnull(COUNT(0),0) as Total FROM TabEvaluacion WHERE codTabEvaluacion is NULL";
                    var dtAux = consultas.ObtenerDataTable(txtSql, "text");
                    var numTbas = int.Parse(dtAux.Rows[0].ItemArray[0].ToString()) - 2;

                    txtSql = "select codproyecto from tabevaluacionproyecto tep, tabevaluacion te ";
                    txtSql += "where id_tabevaluacion=tep.codtabevaluacion  and realizado=1 ";
                    txtSql += "and te.codtabevaluacion is null and codproyecto=" + fila["id_Proyecto"].ToString() + " group by codproyecto ";
                    txtSql += "HAVING count(tep.codtabevaluacion)= " + numTbas;

                    var dtAux2 = consultas.ObtenerDataTable(txtSql, "text");
                    if(dtAux2.Rows.Count > 0)
                    {
                        _subTotalAvalados += 1;
                        _totalAvaslados += 1;
                    }
                    tblReporte.Rows.Add(row);
                }



                //DtlReporteDetallado.DataSource = dt;
                //DtlReporteDetallado.DataBind();
            }
        }

        protected void DtlReporteDetallado_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region Variables Generales

                // GENERALES
                var puntajea = e.Item.FindControl("lblga") as Label;
                var puntajeb = e.Item.FindControl("lblgb") as Label;
                var puntajec = e.Item.FindControl("lblgc") as Label;
                var puntajed = e.Item.FindControl("lblgd") as Label;
                var puntajeE = e.Item.FindControl("lblge") as Label;
                var lpuntajetotal = e.Item.FindControl("lpuntajetotal") as Label;
                var puntajetotalG = e.Item.FindControl("lbltotalG") as Label;
                // FIN //

                #endregion

                #region Variables Comerciales

                // COMERCIALES ///

                var lblcc = e.Item.FindControl("lblcc") as Label;
                var lblcg = e.Item.FindControl("lblcg") as Label;
                var lblch = e.Item.FindControl("lblch") as Label;
                var lblci = e.Item.FindControl("lblci") as Label;
                var lblcj = e.Item.FindControl("lblcj") as Label;
                var lblck = e.Item.FindControl("lblck") as Label;
                var lblcl = e.Item.FindControl("lblcl") as Label;
                var lblcm = e.Item.FindControl("lblcm") as Label;
                var lblcn = e.Item.FindControl("lblcn") as Label;
                var lblco = e.Item.FindControl("lblco") as Label;
                var lblcp = e.Item.FindControl("lblcp") as Label;
                var lblcq = e.Item.FindControl("lblcq") as Label;
                var lblcr = e.Item.FindControl("lblcr") as Label;
                var lblcs = e.Item.FindControl("lblcs") as Label;
                var lbltotalC = e.Item.FindControl("lbltotalC") as Label;

                //*****************************////

                #endregion

                #region Variables Tecnicos

                var lbltt = e.Item.FindControl("lbltt") as Label;
                var lbltu = e.Item.FindControl("lbltu") as Label;
                var lbltv = e.Item.FindControl("lbltv") as Label;
                var lbltw = e.Item.FindControl("lbltw") as Label;
                var lbltx = e.Item.FindControl("lbltx") as Label;
                var lblty = e.Item.FindControl("lblty") as Label;
                var lblTotalT = e.Item.FindControl("lblTotalT") as Label;

                #endregion

                #region Variables Organizacionales

                var lbloz = e.Item.FindControl("lbloz") as Label;
                var lbloaa = e.Item.FindControl("lbloaa") as Label;
                var lbloab = e.Item.FindControl("lbloab") as Label;
                var lbloac = e.Item.FindControl("lbloac") as Label;
                var lbload = e.Item.FindControl("lbload") as Label;
                var lbloae = e.Item.FindControl("lbloae") as Label;
                var lbloaf = e.Item.FindControl("lbloaf") as Label;
                var lblTotalO = e.Item.FindControl("lblTotalO") as Label;

                #endregion

                #region Variables Financieros

                var lblfag = e.Item.FindControl("lblfag") as Label;
                var lblfah = e.Item.FindControl("lblfah") as Label;
                var lblfai = e.Item.FindControl("lblfai") as Label;
                var lblfaj = e.Item.FindControl("lblfaj") as Label;
                var lblfak = e.Item.FindControl("lblfak") as Label;
                var lblTotalF = e.Item.FindControl("lblTotalF") as Label;

                #endregion

                #region Variable Medio Ambiente
                var lblmAL = e.Item.FindControl("lblmAL") as Label;
                var lblTotalM = e.Item.FindControl("lblTotalM") as Label;

                #endregion

                var idproyecto = e.Item.FindControl("lblidproyecto") as Label;
                var viable = e.Item.FindControl("lblviable") as Label;
                var solicitado = e.Item.FindControl("lblsolicitado") as Label;
                var recomendado = e.Item.FindControl("lblrecomendado") as Label;

                if (puntajea != null)
                {
                    if (idproyecto != null)
                    {
                        PuntajeTotal = 0;
                        for (int i = 1; i < _numAspectos; i++)
                        {
                            PuntajeAspecto = 0;
                            PuntajeComercial = 0;
                            PuntajeTecnico = 0;
                            PuntajeOrganizacional = 0;
                            PuntajeFinanciero = 0;

                            PuntajeMedioAmbiente = 0;

                            List<string> mipuntaje = Obtenerpuntaje(Convert.ToInt32(idproyecto.Text), i);

                            foreach (string mpunt in mipuntaje)
                            {
                                #region Generales

                                if (i < 6)
                                {
                                    /* *********************************** INCIO GENERALES *******************************/
                                    if (i == 1)
                                    {
                                        if (string.IsNullOrEmpty(puntajea.Text))
                                        {
                                            puntajea.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(puntajeb.Text))
                                        {
                                            puntajeb.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(puntajec.Text))
                                        {
                                            puntajec.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(puntajed.Text))
                                        {
                                            puntajed.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(puntajeE.Text))
                                        {
                                            puntajeE.Text = mpunt;
                                        }
                                    }
                                    /* *********************************** FIN GENERALES *******************************/

                                #endregion

                                    #region Comerciales

                                    /***********************************    INICIO COMERCIALES   *******************************/
                                    if (i == 2)
                                    {
                                        if (string.IsNullOrEmpty(lblcc.Text))
                                        {
                                            lblcc.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcg.Text))
                                        {
                                            lblcg.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblch.Text))
                                        {
                                            lblch.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblci.Text))
                                        {
                                            lblci.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcj.Text))
                                        {
                                            lblcj.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblck.Text))
                                        {
                                            lblck.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcl.Text))
                                        {
                                            lblcl.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcm.Text))
                                        {
                                            lblcm.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcn.Text))
                                        {
                                            lblcn.Text = mpunt;
                                        }

                                        else if (string.IsNullOrEmpty(lblco.Text))
                                        {
                                            lblco.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcp.Text))
                                        {
                                            lblcp.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcq.Text))
                                        {
                                            lblcq.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcr.Text))
                                        {
                                            lblcr.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblcs.Text))
                                        {
                                            lblcs.Text = mpunt;
                                        }

                                    }
                                    /* *********************************** FIN COMERCIALES *******************************/

                                    #endregion

                                    #region Tecnicos

                                    /***********************************    INICIO TECNICOS   *******************************/
                                    if (i == 3)
                                    {
                                        if (string.IsNullOrEmpty(lbltt.Text))
                                        {
                                            lbltt.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbltu.Text))
                                        {
                                            lbltu.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbltv.Text))
                                        {
                                            lbltv.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbltw.Text))
                                        {
                                            lbltw.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbltx.Text))
                                        {
                                            lbltx.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblty.Text))
                                        {
                                            lblty.Text = mpunt;
                                        }

                                    }

                                    /* *********************************** FIN TECNICOS *******************************/

                                    #endregion

                                    #region Organizacionales

                                    /***********************************    INICIO ORGANIZACIONALES   *******************************/
                                    if (i == 4)
                                    {
                                        if (string.IsNullOrEmpty(lbloz.Text))
                                        {
                                            lbloz.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbloaa.Text))
                                        {
                                            lbloaa.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbloab.Text))
                                        {
                                            lbloab.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbloac.Text))
                                        {
                                            lbloac.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lbload.Text))
                                        {
                                            lbload.Text = mpunt;
                                        }


                                        else if (string.IsNullOrEmpty(lbloae.Text))
                                        {
                                            lbloae.Text = mpunt;
                                        }

                                        else if (string.IsNullOrEmpty(lbloaf.Text))
                                        {
                                            lbloaf.Text = mpunt;
                                        }

                                    }

                                    /* *********************************** FIN ORGANIZACIONALES *******************************/

                                    #endregion

                                    #region Financieros

                                    /***********************************    INICIO FINANCIEROS   *******************************/
                                    if (i == 5)
                                    {
                                        if (string.IsNullOrEmpty(lblfag.Text))
                                        {
                                            lblfag.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblfah.Text))
                                        {
                                            lblfah.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblfai.Text))
                                        {
                                            lblfai.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblfaj.Text))
                                        {
                                            lblfaj.Text = mpunt;
                                        }
                                        else if (string.IsNullOrEmpty(lblfak.Text))
                                        {
                                            lblfak.Text = mpunt;
                                        }

                                    }

                                    /* *********************************** FIN FINANCIEROS *******************************/

                                    #endregion
                                    if (i == 6)
                                    {
                                        //medio ambiente
                                        if (string.IsNullOrEmpty(lblmAL.Text))
                                        {
                                            lblmAL.Text = mpunt;
                                        }
                                        //fin medio ambiente.
                                    }

                                    if (!string.IsNullOrEmpty(mpunt))
                                    {
                                        PuntajeAspecto += Convert.ToInt16(mpunt);

                                        if (i == 2)
                                        {
                                            PuntajeComercial += Convert.ToInt16(mpunt);
                                        }
                                        if (i == 3)
                                        {
                                            PuntajeTecnico += Convert.ToInt16(mpunt);
                                        }
                                        if (i == 4)
                                        {
                                            PuntajeOrganizacional += Convert.ToInt16(mpunt);
                                        }

                                        if (i == 5)
                                        {
                                            PuntajeFinanciero += Convert.ToInt16(mpunt);
                                        }
                                        if (i == 6)
                                        {
                                            PuntajeMedioAmbiente += Convert.ToInt16(mpunt);
                                        }


                                    }
                                    //PuntajeTotal = PuntajeAspecto + PuntajeComercial + PuntajeTecnico + PuntajeOrganizacional + PuntajeFinanciero + PuntajeMedioAmbiente;
                                    PuntajeTotal += Convert.ToInt16(mpunt);
                                    lpuntajetotal.Text = PuntajeTotal.ToString();


                                    #region Totales

                                    if (i == 1)
                                    {
                                        puntajetotalG.Text = PuntajeAspecto.ToString();
                                    }

                                    if (i == 2)
                                    {
                                        lbltotalC.Text = PuntajeComercial.ToString();
                                    }

                                    if (i == 3)
                                    {
                                        lblTotalT.Text = PuntajeTecnico.ToString();
                                    }
                                    if (i == 4)
                                    {
                                        lblTotalO.Text = PuntajeOrganizacional.ToString();
                                    }

                                    if (i == 5)
                                    {
                                        lblTotalF.Text = PuntajeFinanciero.ToString();
                                    }
                                    if (i == 6)
                                    {
                                        lblTotalM.Text = PuntajeMedioAmbiente.ToString();
                                    }

                                    #endregion

                                }
                            }

                            if (viable != null)
                            {
                                if (viable.Text == "SI")
                                {
                                    _subTotalViables += 1;
                                    _totalViables += 1;
                                    _subTotalRecomendadoViables += Convert.ToInt32(recomendado.Text);
                                    _totalRecomendado += Convert.ToInt32(solicitado.Text);
                                }

                            }
                            consultas.Parameters = null;
                            consultas.Parameters = new[]
                                                       {
                                                           new SqlParameter
                                                               {
                                                                   ParameterName = "@idproyecto",
                                                                   Value = Convert.ToInt32(idproyecto.Text)
                                                               }
                                                       };
                            DataTable dtProyecto = consultas.ObtenerDataTable("MD_obtenerTabs");

                            if (dtProyecto.Rows.Count != 0)
                            {
                                _subTotalAvalados += 1;
                                _totalAvaslados += 1;
                            }

                            _subTotalSolicitado += Convert.ToInt32(solicitado.Text);
                            _totalSolicitado += Convert.ToInt32(solicitado.Text);
                            _subTotalRecomendado += Convert.ToInt32(recomendado.Text);
                            _totalRecomendado += Convert.ToInt32(recomendado.Text);
                            _subTotalProyectos += 1;
                            _totalProyectos += 1;

                            if (PuntajeTotal >= puntajeMinimo)
                            {
                                _subTotalAprobados += 1;
                                _totalAprobados += 1;
                                _subTotalRecomendadoAprobados += Convert.ToInt32(recomendado.Text);
                                _subTotalAprobados += _subTotalRecomendadoAprobados + Convert.ToInt32(recomendado.Text);
                                _totalRecomendadoAprobados += Convert.ToInt32(recomendado.Text);
                            }



                        }
                    }
                }

            }
        }

        public List<string> Obtenerpuntaje(int idproyecto, int campo)
        {

            var lstpuntaje = new List<string>();

            string query = "select puntaje from evaluacioncampo ec  inner join campo c on c.id_campo = ec.codcampo  "
                            + "inner join campo v on v.id_campo = c.codcampo  inner join campo a on a.id_campo = v.codcampo where codproyecto= " + idproyecto
                            + " and codconvocatoria=" + codConvocatoria + " and a.id_campo=" + campo
                            + " order by a.id_campo,v.campo Asc";

            consultas.Parameters = null;

            DataTable puntaje = consultas.ObtenerDataTable(query, "text");

            if (puntaje != null && puntaje.Rows.Count != 0)
            {
                lstpuntaje.AddRange(from DataRow row in puntaje.Rows select row["puntaje"].ToString());
            }

            return lstpuntaje;
        }

        private void exportar()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "ReporteEvaluacion" + DateTime.Now.Date + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            tblReporte.GridLines = GridLines.Both;
            tblReporte.RenderControl(htmltextwrtter);
            //DtlReporteDetallado.GridLines = GridLines.Both;
            //DtlReporteDetallado.HeaderStyle.Font.Bold = true;
            //DtlReporteDetallado.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();
        }
        #endregion
    }
}