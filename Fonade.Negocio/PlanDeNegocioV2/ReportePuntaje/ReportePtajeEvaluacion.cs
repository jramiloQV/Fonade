using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.PlanDeNegocioV2.ReportePuntaje
{
    public static class ReportePtajeEvaluacion
    {
        /// <summary>
        /// Cadena de conexión a la base de datos
        /// </summary>
        static string cadenaConexion
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            }
        }

        /// <summary>
        /// Obtiene la información de una convocatoria
        /// </summary>
        /// <param name="codConvocatoria">Identificador primario de la convocatoria</param>
        /// <returns></returns>
        public static Convocatoria getConvocatoria(int codConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var consulta = (from c in db.Convocatoria
                                where c.Id_Convocatoria == codConvocatoria
                                select c).FirstOrDefault();

                return consulta;
            }
        }

        /// <summary>
        /// Obtiene los datos consultados del puntaje de evaluación de los planes asociados a una convocatoria
        /// </summary>
        /// <param name="codConvocatoria">Código de la convocatoria</param>
        /// <returns>Información consultada</returns>
        public static List<Datos.DataType.ReportePuntaje> getReporte(int codConvocatoria)
        {
            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var consulta = db.REP_ConsultarReportePuntaje(codConvocatoria).Select(x => new 
                    
                Datos.DataType.ReportePuntaje()
                {
                    codProyecto = x.Id_Proyecto,
                    MontoRecomendado = x.MontoRecomendado,
                    MontoSolicitado = x.MontoSolicitado,
                    NomCiudad = string.Format("{0} ({1})", x.NomCiudad, x.NomDepartamento),
                    NomProyecto = x.NomProyecto,
                    NomUnidad = string.Format("{0} ({1})", x.NomUnidad, x.NomInstitucion),
                    Viable = x.Viable,

                    A = ConvertirNulo(x.A, codConvocatoria, Constantes.Const_RepA),
                    B = ConvertirNulo(x.B, codConvocatoria, Constantes.Const_RepB),
                    C = ConvertirNulo(x.C, codConvocatoria, Constantes.Const_RepC),
                    D = ConvertirNulo(x.D, codConvocatoria, Constantes.Const_RepD),
                    E = ConvertirNulo(x.E, codConvocatoria, Constantes.Const_RepE),
                    F = ConvertirNulo(x.F, codConvocatoria, Constantes.Const_RepF),
                    G = ConvertirNulo(x.G, codConvocatoria, Constantes.Const_RepG),
                    H = ConvertirNulo(x.H, codConvocatoria, Constantes.Const_RepH),
                    I = ConvertirNulo(x.I, codConvocatoria, Constantes.Const_RepI),
                    J = ConvertirNulo(x.J, codConvocatoria, Constantes.Const_RepJ),
                    K = ConvertirNulo(x.K, codConvocatoria, Constantes.Const_RepK),
                    L = ConvertirNulo(x.L, codConvocatoria, Constantes.Const_RepL),
                    M = ConvertirNulo(x.M, codConvocatoria, Constantes.Const_RepM),
                    N = ConvertirNulo(x.N, codConvocatoria, Constantes.Const_RepN),
                    O = ConvertirNulo(x.O, codConvocatoria, Constantes.Const_RepO),
                    P = ConvertirNulo(x.P, codConvocatoria, Constantes.Const_RepP),
                    Q = ConvertirNulo(x.Q, codConvocatoria, Constantes.Const_RepQ),
                    R = ConvertirNulo(x.R, codConvocatoria, Constantes.Const_RepR),
                    S = ConvertirNulo(x.S, codConvocatoria, Constantes.Const_RepS),
                    T = ConvertirNulo(x.T, codConvocatoria, Constantes.Const_RepT),
                    U = ConvertirNulo(x.U, codConvocatoria, Constantes.Const_RepU),
                    V = ConvertirNulo(x.V, codConvocatoria, Constantes.Const_RepV),
                    W = ConvertirNulo(x.W, codConvocatoria, Constantes.Const_RepW),
                    X = ConvertirNulo(x.X, codConvocatoria, Constantes.Const_RepX),
                    Y = ConvertirNulo(x.Y, codConvocatoria, Constantes.Const_RepY),
                    Z = ConvertirNulo(x.Z, codConvocatoria, Constantes.Const_RepZ),
                    AA = ConvertirNulo(x.AA, codConvocatoria, Constantes.Const_RepAA),
                    AB = ConvertirNulo(x.AB, codConvocatoria, Constantes.Const_RepAB),
                    AC = ConvertirNulo(x.AC, codConvocatoria, Constantes.Const_RepAC),
                    AD = ConvertirNulo(x.AD, codConvocatoria, Constantes.Const_RepAD),
                    AE = ConvertirNulo(x.AE, codConvocatoria, Constantes.Const_RepAE),
                    AF = ConvertirNulo(x.AF, codConvocatoria, Constantes.Const_RepAF),
                    AG = ConvertirNulo(x.AG, codConvocatoria, Constantes.Const_RepAG),
                    AH = ConvertirNulo(x.AH, codConvocatoria, Constantes.Const_RepAH),
                    AI = ConvertirNulo(x.AI, codConvocatoria, Constantes.Const_RepAI),
                    AJ = ConvertirNulo(x.AJ, codConvocatoria, Constantes.Const_RepAJ),
                    AK = ConvertirNulo(x.AK, codConvocatoria, Constantes.Const_RepAK),
                    AL = ConvertirNulo(x.AL, codConvocatoria, Constantes.Const_RepAL),
                    AM = ConvertirNulo(x.AM, codConvocatoria, Constantes.Const_RepAM),
                    AN = ConvertirNulo(x.AN, codConvocatoria, Constantes.Const_RepAN),
                    AO = ConvertirNulo(x.AO, codConvocatoria, Constantes.Const_RepAO),
                    AP = ConvertirNulo(x.AP, codConvocatoria, Constantes.Const_RepAP),
                    AQ = ConvertirNulo(x.AQ, codConvocatoria, Constantes.Const_RepAQ),
                    AR = ConvertirNulo(x.AR, codConvocatoria, Constantes.Const_RepAR),
                    AS = ConvertirNulo(x.AS, codConvocatoria, Constantes.Const_RepAS),
                    AT = ConvertirNulo(x.AT, codConvocatoria, Constantes.Const_RepAT),
                    AU = ConvertirNulo(x.AU, codConvocatoria, Constantes.Const_RepAU),
                    AV = ConvertirNulo(x.AV, codConvocatoria, Constantes.Const_RepAV),
                    AW = ConvertirNulo(x.AW, codConvocatoria, Constantes.Const_RepAW),
                    AX = ConvertirNulo(x.AX, codConvocatoria, Constantes.Const_RepAX),
                    AY = ConvertirNulo(x.AY, codConvocatoria, Constantes.Const_RepAY),
                    AZ = ConvertirNulo(x.AZ, codConvocatoria, Constantes.Const_RepAZ),

                    BA = ConvertirNulo(x.BA, codConvocatoria, Constantes.Const_RepBA),
                    BB = ConvertirNulo(x.BB, codConvocatoria, Constantes.Const_RepBB),
                    BC = ConvertirNulo(x.BC, codConvocatoria, Constantes.Const_RepBC),
                    BD = ConvertirNulo(x.BD, codConvocatoria, Constantes.Const_RepBD),
                    BE = ConvertirNulo(x.BE, codConvocatoria, Constantes.Const_RepBE),
                    BF = ConvertirNulo(x.BF, codConvocatoria, Constantes.Const_RepBF),
                    BG = ConvertirNulo(x.BG, codConvocatoria, Constantes.Const_RepBG),
                    BH = ConvertirNulo(x.BH, codConvocatoria, Constantes.Const_RepBH),
                    BI = ConvertirNulo(x.BI, codConvocatoria, Constantes.Const_RepBI),
                    BJ = ConvertirNulo(x.BJ, codConvocatoria, Constantes.Const_RepBJ),
                    BK = ConvertirNulo(x.BK, codConvocatoria, Constantes.Const_RepBK),
                    BL = ConvertirNulo(x.BL, codConvocatoria, Constantes.Const_RepBL),
                    BM = ConvertirNulo(x.BM, codConvocatoria, Constantes.Const_RepBM),
                    BN = ConvertirNulo(x.BN, codConvocatoria, Constantes.Const_RepBN),
                    BO = ConvertirNulo(x.BO, codConvocatoria, Constantes.Const_RepBO),
                    BP = ConvertirNulo(x.BP, codConvocatoria, Constantes.Const_RepBP),


                    //Se suma el total de los valores de los códigos tab 71(A), 73(B)
                    TotalProtagonista = calcularTotales(new string[]{x.A,x.B},true),

                    //Se suma el total de los valores de los códigos tab 76(C),77(D),79(E),80(F),81(G),82(H),84(I),85(J),86(K)
                    //, 146(BA), 147(BB)
                    TotalOportunidad = calcularTotales(new string[] { x.C, x.D, x.E, x.F, x.G, x.H, x.I, x.J, x.K
                                                        , x.BA, x.BB}, true),

                    //Se suma el total de los valores de los códigos tab 89(L),90(M),91(N),92(O),93(P),95(Q),96(R),97(S),99(T),100(U),101(V)
                    //, 148(BC), 149(BD), 150(BE)
                    TotalSolucion = calcularTotales(new string[] { x.L, x.M, x.N, x.O, x.P, x.Q, x.R, x.S, x.T, x.U, x.V
                                                        , x.BC, x.BD, x.BE }, true),

                    //Se suma el total de los valores de los códigos tab 104(W), 105(X), 106(Y), 107(Z), 108(AA), 110(AB), 111(AC), 113(AD), 114(AE), 115(AF), 116(AG), 117(AH), 118(AI), 119(AJ), 121(AK), 122(AL)
                    //, 151(BF), 152(BG), 153(BH), 154(BI), 155(BJ)
                    TotalDesarrollo = calcularTotales(new string[] {x.W, x.X, x.Y, x.Z, x.AA, x.AB, x.AC, x.AD, x.AE, x.AF, x.AG, x.AH, x.AI, x.AJ, x.AK, x.AL
                                                        , x.BF, x.BG, x.BH, x.BI, x.BJ }, true),

                    //Se suma el total de los valores de los códigos tab 125(AM), 126(AN), 128(AO), 129(AP), 130(AQ), 132(AR), 133(AS), 134(AT), 135(AU)
                    TotalFuturoNegocio = calcularTotales(new string[] { x.AM, x.AN, x.AO, x.AP, x.AQ, x.AR, x.AS, x.AT, x.AU }, true),

                    //Se suma el total de los valores de los códigos tab 138(AV), 139(AW), 156(BK), 157(BL), 158(BM)
                    TotalRiesgo = calcularTotales(new string[] { x.AV, x.AW, x.BK, x.BL, x.BM }, true),

                    //Se busca la condición AND de los códigos 142(AX), 143(AY), 145(AZ)
                    VlrResumenLetras = calcularTotales(new string[] {x.AX, x.AY, x.AZ }, false) != 0 ? "SI" : "NO",

                    //Se ResumenEjecutivo los códigos 162(BN), 163(BO), 164(BP) 
                    VlrResumenNumeros = calcularTotales(new string[] { x.BN, x.BO, x.BP }, true),

                    //Se calcula el puntaje total exceptuando las preguntas del valor resumen en letras
                    PuntajeTotal = calcularTotales(new string[] { x.A, x.B, x.C, x.D, x.E, x.F, x.G, x.H, x.I, x.J, x.K,
                                                                  x.L, x.M, x.N, x.O, x.P, x.Q, x.R, x.S, x.T, x.U, x.V,
                                                                  x.W, x.X, x.Y, x.Z, x.AA, x.AB, x.AC, x.AD, x.AE, x.AF, x.AG, 
                                                                  x.AH, x.AI, x.AJ, x.AK, x.AL,x.AM, x.AN, x.AO, x.AP, x.AQ,
                                                                  x.AR, x.AS, x.AT, x.AU, x.AV, x.AW, x.BA, x.BB, x.BC, x.BD,
                                                                  x.BE, x.BF, x.BG, x.BH, x.BI, x.BJ, x.BK, x.BL, x.BM, x.BN,
                                                                  x.BO, x.BP  }, true),

                }).ToList();

                return consulta;
            }
        }

        /// <summary>
        /// Calcula los totales de cada sección de evaluación realizada
        /// </summary>
        /// <param name="datos">Conjunto de datos a calcular</param>
        /// <param name="esnumerico">Determina si el valor a calcular es numérico o booleano</param>
        /// <returns>Total calculado. Cero o uno para un valor booleano</returns>
        private static int calcularTotales(string[] datos, bool esnumerico)
        {
            int total = 0;


            foreach (var item in datos)
            {
                if (item != null)
                {
                    if (esnumerico)
                    {
                        total += int.Parse(item);
                    }
                    else
                    {
                        if (item.Equals("NO"))
                        {
                            total = 0;
                            break;
                        }
                        else
                        {
                            total = 1;
                        }

                    }
                }

            }

            return total;
        }

        /// <summary>
        /// Convierte el valor nulo con la expresión por defecto
        /// </summary>
        /// <param name="cad">Cadena a procesar</param>
        /// <returns>Cadena procesada</returns>
        private static string ConvertirNulo(string cad, int codconvocatoria, int codCampo)
        {
            string cadFinal = cad;

            using (Datos.FonadeDBDataContext db = new Datos.FonadeDBDataContext(cadenaConexion))
            {
                var consulta = (from c in db.ConvocatoriaCampos
                                where c.codConvocatoria == codconvocatoria
                                && c.codCampo == codCampo
                                select c).Count();

                if(consulta > 0)
                {
                    if(cad == null)
                    {
                        cadFinal = "0";
                    }
                }
                else
                {
                    cadFinal = "N.A";
                }
            }

            return cadFinal;
        }
    }
}
