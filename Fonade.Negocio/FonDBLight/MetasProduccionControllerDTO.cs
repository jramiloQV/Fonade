using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class MetasProduccionControllerDTO
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<MetasProduccionModelDTO> ListMetasProduccion(int _codProyecto, int _codConvocatoria)
        {
            List<MetasProduccionModelDTO> listProduccion = new List<MetasProduccionModelDTO>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listProduccion = (from r in db.ActaSeguimGestionProduccionEvaluacion
                                  where r.codProyecto == _codProyecto
                                  select new MetasProduccionModelDTO
                                  {
                                      UnidadMedida = r.unidadMedida,
                                      Id_Producto = r.id_Producto,
                                      Cantidad = r.unidades.ToString(),
                                      NomProducto = r.nomProducto,
                                      productoRepresentativo = r.productoRepresentativo
                                  }).ToList();
            }

            //using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            //{
            //    listProduccion = (from r in db.IndicadorGestionEvaluacions
            //                      join p in db.ProyectoProductos
            //                      on r.ProductoMasRepresentativo equals p.Id_Producto
            //                      join v in db.IndicadorProductoEvaluacions
            //                      on p.Id_Producto equals v.IdProducto
            //                        where r.IdProyecto == _codProyecto
            //                        && v.IdConvocatoria == _codConvocatoria
            //                        select new MetasProduccionModelDTO
            //                        {
            //                           UnidadMedida = p.UnidadMedida,
            //                           Id_Producto = p.Id_Producto,
            //                           Cantidad = v.Unidades.ToString(),
            //                           NomProducto = p.NomProducto
            //                        }).ToList();               
            //}

            return listProduccion;
        }

        public List<MetasProduccionModelDTO> ListAllMetasProduccion(int _codProyecto, int _codConvocatoria)
        {
            List<MetasProduccionModelDTO> listProduccion = new List<MetasProduccionModelDTO>();
            
            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var indicadoresEvaluacion = db.IndicadorGestionEvaluacions.FirstOrDefault(filter => filter.IdProyecto == _codProyecto && filter.IdConvocatoria == _codConvocatoria);

                int codigoProductoSeleccionadoEvaluacion = indicadoresEvaluacion.ProductoMasRepresentativo.GetValueOrDefault(0);

                listProduccion = (from  p in db.ProyectoProductos                                  
                                  join v in db.IndicadorProductoEvaluacions
                                  on p.Id_Producto equals v.IdProducto
                                  where p.CodProyecto == _codProyecto
                                  && v.IdConvocatoria == _codConvocatoria
                                  select new MetasProduccionModelDTO
                                  {
                                      UnidadMedida = p.UnidadMedida,
                                      Id_Producto = p.Id_Producto,
                                      Cantidad = v.Unidades + " " + p.UnidadMedida,
                                      NomProducto = p.NomProducto,
                                      productoRepresentativo = (p.Id_Producto == codigoProductoSeleccionadoEvaluacion)
                                  }).ToList();
            }

            return listProduccion;
        }

        public List<MetasProduccionModelDTO> ListAllMetasProduccionInterventoria(int _codProyecto, int _codConvocatoria)
        {
            List<MetasProduccionModelDTO> listProduccion = new List<MetasProduccionModelDTO>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                
                listProduccion = (from p in db.ActaSeguimGestionProduccionEvaluacion
                                  where p.codProyecto == _codProyecto && p.ocultar == false
                                  select new MetasProduccionModelDTO
                                  {
                                      UnidadMedida = p.unidadMedida,
                                      Id_Producto = p.id_Producto,
                                      Cantidad = p.unidades + " " + p.unidadMedida,
                                      Unidades = p.unidades,
                                      NomProducto = p.nomProducto,
                                      productoRepresentativo = p.productoRepresentativo,
                                      Id_ProductoInterventoria = p.idProduccionInterventoria
                                  }).ToList();
            }

            return listProduccion;
        }

        public string productoMasRepresentativo(int _codProyecto, int _codConvocatoria)
        {
            string producto = "No se seleccióno el produco más representativo.";

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                int codProducto = (from r in db.IndicadorGestionEvaluacions                                  
                                  where r.IdProyecto == _codProyecto
                                  && r.IdConvocatoria == _codConvocatoria
                                  select r.ProductoMasRepresentativo
                                  ).FirstOrDefault()??0;

                if (codProducto!=0)
                {
                    producto = (from p in db.ProyectoProductos
                                where p.Id_Producto == codProducto
                                select p.NomProducto).FirstOrDefault();
                }
            }


            return producto;
        }

        public string productoMasRepresentativoInterventoria(int _codProyecto, int _codConvocatoria)
        {
            string producto = "No se seleccióno el produco más representativo.";

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                producto = (from pe in db.ActaSeguimGestionProduccionEvaluacion
                            where pe.codProyecto == _codProyecto && pe.productoRepresentativo == true
                            select pe.nomProducto).FirstOrDefault();
            }


            return producto;
        }
    }
}
