using Datos;
using Datos.Modelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fonade.Negocio.FonDBLight
{
    public class ActaSeguimGestionProduccionController
    {
        private string _cadena = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;

        public List<ActaSeguimGestionProduccionModel> GetGestionProduccion(int _codProyecto, int _codConvocatoria)
        {
            List<ActaSeguimGestionProduccionModel> listGestionProduccion = new List<ActaSeguimGestionProduccionModel>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                listGestionProduccion = (from e in db.ActaSeguimGestionProduccion
                                         where e.codProyecto == _codProyecto && e.codConvocatoria == _codConvocatoria
                                         orderby e.numActa
                                         select new ActaSeguimGestionProduccionModel
                                         {
                                             id = e.idActaSegProduccion,
                                             codProyecto = e.codProyecto,
                                             codConvocatoria = e.codConvocatoria,
                                             numActa = e.numActa,
                                             visita = e.visita,
                                             cantidad = e.Cantidad,
                                             Descripcion = e.Descripcion,
                                             FechaIngreso = e.FechaIngreso,
                                             medida = e.Medida,
                                             cantidadMedida = e.Cantidad + " " + e.Medida,
                                             codProducto = e.codProducto,
                                             NomProducto = e.NomProducto,
                                             productoRepresentativo = e.productoRepresentativo ?? false
                                         }).ToList();
            }

            return listGestionProduccion;

        }

        public bool InsertOrUpdateGestionProduccion(ActaSeguimGestionProduccionModel produccion, ref string mensaje)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                int CantidadAnt = (from g in db.ActaSeguimGestionProduccion
                                   where g.codConvocatoria == produccion.codConvocatoria
                                      && g.codProyecto == produccion.codProyecto
                                      && g.numActa == (produccion.numActa - 1)
                                   select g.Cantidad).FirstOrDefault();


                if (produccion.cantidad >= CantidadAnt)
                {
                    var actaProduccion = (from g in db.ActaSeguimGestionProduccion
                                          where g.codConvocatoria == produccion.codConvocatoria
                                          && g.codProyecto == produccion.codProyecto
                                          && g.numActa == produccion.numActa
                                          select g).FirstOrDefault();

                    if (actaProduccion != null)//Actualizar
                    {
                        actaProduccion.Descripcion = produccion.Descripcion;
                        actaProduccion.Cantidad = produccion.cantidad;
                        actaProduccion.FechaIngreso = DateTime.Now;
                        actaProduccion.NomProducto = produccion.NomProducto;
                    }
                    else//Insertar
                    {
                        ActaSeguimGestionProduccion gesProduccion = new ActaSeguimGestionProduccion
                        {
                            Cantidad = produccion.cantidad,
                            codConvocatoria = produccion.codConvocatoria,
                            codProyecto = produccion.codProyecto,
                            Descripcion = produccion.Descripcion,
                            numActa = produccion.numActa,
                            visita = produccion.visita,
                            FechaIngreso = DateTime.Now,
                            Medida = produccion.medida,

                        };

                        db.ActaSeguimGestionProduccion.InsertOnSubmit(gesProduccion);
                    }

                    db.SubmitChanges();

                    insertado = true;
                }
                else
                {
                    mensaje = "La cantidad ingresada de producción acumulada no puede ser menor al de la última visita.";
                }

            }

            return insertado;
        }

        public void copiarInformacionProduccion(List<MetasProduccionModelDTO> productos, int _codContacto)
        {
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                List<ActaSeguimGestionProduccionEvaluacion> List = new List<ActaSeguimGestionProduccionEvaluacion>();

                foreach (var p in productos)
                {
                    var cant = (from a in db.ActaSeguimGestionProduccionEvaluacion
                                where a.id_Producto == p.Id_Producto
                                select a).Count();

                    if (cant == 0)
                    {
                        using (FonadeDBDataContext db2 = new FonadeDBDataContext(_cadena))
                        {
                            var producto = (from pp in db2.ProyectoProductos
                                            where pp.Id_Producto == p.Id_Producto
                                            select pp).FirstOrDefault();

                            ActaSeguimGestionProduccionEvaluacion produccionEvaluacion = new ActaSeguimGestionProduccionEvaluacion
                            {
                                codContacto = _codContacto,
                                codProyecto = p.codProyecto,
                                fechaUltimaActualizacion = DateTime.Now,
                                id_Producto = p.Id_Producto,
                                nomProducto = p.NomProducto,
                                ocultar = false,
                                productoRepresentativo = p.productoRepresentativo,
                                unidades = Convert.ToInt32(p.Cantidad),
                                unidadMedida = p.UnidadMedida,
                                composicion = producto.Composicion,
                                condicionesEspeciales = producto.CondicionesEspeciales,
                                descripcionGeneral = producto.DescripcionGeneral,
                                formaDePago = producto.FormaDePago,
                                iva = producto.Iva,
                                justificacion = producto.Justificacion,
                                nombreComercial = producto.NombreComercial,
                                otros = producto.Otros,
                                porcentajeIva = producto.PorcentajeIva

                            };

                            List.Add(produccionEvaluacion);
                        }

                    }
                }

                db.ActaSeguimGestionProduccionEvaluacion.InsertAllOnSubmit(List);
                db.SubmitChanges();
            }
        }

        public List<MetasProduccionModelDTO> getProduccionEvaluacion(int _codProyecto)
        {
            List<MetasProduccionModelDTO> listProductos = new List<MetasProduccionModelDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                listProductos = (from pp in db.ProyectoProductos
                                 join ipe in db.IndicadorProductoEvaluacions on pp.Id_Producto equals ipe.IdProducto
                                 where pp.CodProyecto == _codProyecto
                                 select new MetasProduccionModelDTO
                                 {
                                     Id_Producto = pp.Id_Producto,
                                     codProyecto = pp.CodProyecto,
                                     Cantidad = ipe.Unidades.ToString(),
                                     NomProducto = pp.NomProducto,
                                     UnidadMedida = pp.UnidadMedida,
                                     productoRepresentativo = false
                                 }).ToList();

                int productoRepresentativo = (from i in db.IndicadorGestionEvaluacions
                                              where i.IdProyecto == _codProyecto
                                              select i.ProductoMasRepresentativo).FirstOrDefault() ?? 0;

                foreach (MetasProduccionModelDTO dTO in listProductos)
                {
                    if (dTO.Id_Producto == productoRepresentativo)
                        dTO.productoRepresentativo = true;
                }
            }

            return listProductos;
        }

        public List<MetasProduccionModelDTO> ListAllProductos(int _codProyecto, int _codConvocatoria, int _visita)
        {
            List<MetasProduccionModelDTO> listProduccion = new List<MetasProduccionModelDTO>();

            using (FonadeDBDataContext db = new FonadeDBDataContext(_cadena))
            {
                var productoMasREP = (from p in db.IndicadorGestionEvaluacions
                                      where p.IdProyecto == _codProyecto
                                      && p.IdConvocatoria == _codConvocatoria
                                      select p.ProductoMasRepresentativo).FirstOrDefault();

                listProduccion = (from p in db.ProyectoProductos
                                  join v in db.IndicadorProductoEvaluacions
                                  on p.Id_Producto equals v.IdProducto
                                  where p.CodProyecto == _codProyecto
                                  && v.IdConvocatoria == _codConvocatoria
                                  select new MetasProduccionModelDTO
                                  {
                                      codConvocatoria = v.IdConvocatoria,
                                      codProyecto = p.CodProyecto,
                                      visita = _visita,
                                      UnidadMedida = p.UnidadMedida,
                                      Id_Producto = p.Id_Producto,
                                      NomProducto = p.NomProducto,
                                      productoRepresentativo = (p.Id_Producto == productoMasREP)
                                  }).ToList();
            }

            return listProduccion;
        }

        public List<MetasProduccionModelDTO> ListAllProductosInterventoria(int _codProyecto, int _codConvocatoria, int _visita)
        {
            List<MetasProduccionModelDTO> listProduccion = new List<MetasProduccionModelDTO>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {

                listProduccion = (from p in db.ActaSeguimGestionProduccionEvaluacion
                                  where p.codProyecto == _codProyecto && p.ocultar == false
                                  select new MetasProduccionModelDTO
                                  {
                                      codConvocatoria = _codConvocatoria,
                                      codProyecto = p.codProyecto,
                                      visita = _visita,
                                      UnidadMedida = p.unidadMedida,
                                      Id_ProductoInterventoria = p.idProduccionInterventoria,
                                      NomProducto = p.nomProducto,
                                      productoRepresentativo = p.productoRepresentativo
                                  }).ToList();
            }

            return listProduccion;
        }

        public bool InsertOrUpdateListGestionProduccion(List<ActaSeguimGestionProduccionModel> gestionProduccion, ref string mensaje)
        {
            bool insertado = false;

            mensaje = "";
            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                foreach (var r in gestionProduccion)
                {
                    int CantidadAnt = (from g in db.ActaSeguimGestionProduccion
                                       where g.codConvocatoria == r.codConvocatoria
                                          && g.codProyecto == r.codProyecto
                                          && g.numActa == (r.numActa - 1)
                                       select g.Cantidad).FirstOrDefault();

                    if (r.cantidad >= CantidadAnt)
                    {
                        var actaProduccion = (from g in db.ActaSeguimGestionProduccion
                                              where g.codProducto == r.codProducto
                                              && g.numActa == r.numActa
                                              select g).FirstOrDefault();

                        if (actaProduccion != null)//Actualizar
                        {
                            actaProduccion.Cantidad = r.cantidad;
                            actaProduccion.FechaIngreso = DateTime.Now;
                            actaProduccion.Descripcion = r.Descripcion;
                            actaProduccion.NomProducto = r.NomProducto;
                            actaProduccion.productoRepresentativo = r.productoRepresentativo;
                        }
                        else//Insertar
                        {
                            ActaSeguimGestionProduccion gesProducto = new ActaSeguimGestionProduccion
                            {
                                numActa = r.numActa,
                                codConvocatoria = r.codConvocatoria,
                                codProyecto = r.codProyecto,
                                codProducto = r.codProducto,
                                Descripcion = r.Descripcion,
                                Cantidad = r.cantidad,
                                FechaIngreso = DateTime.Now,
                                Medida = r.medida,
                                visita = r.visita,
                                NomProducto = r.NomProducto,
                                productoRepresentativo = r.productoRepresentativo
                            };

                            db.ActaSeguimGestionProduccion.InsertOnSubmit(gesProducto);
                        }

                        db.SubmitChanges();
                        insertado = true;
                    }
                    else
                    {
                        mensaje = mensaje + "La cantidad del producto: " +
                                    r.NomProducto + " NO puede ser menor al de la visita anterior; ";
                    }

                }

            }

            return insertado;
        }

        public bool insertarProductoInterventoria(ActaSeguimGestionProduccionEvaluacion proyectoProducto)
        {
            bool insertado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                db.ActaSeguimGestionProduccionEvaluacion.InsertOnSubmit(proyectoProducto);
                db.SubmitChanges();
                insertado = true;
            }

            return insertado;
        }

        public bool ocultarProducto(int _idProducto, int _codContacto, int _codProyecto)
        {
            bool ocultado = false;

            if (cantProductosXProyecto(_codProyecto)>1)
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
                {
                    var query = (from r in db.ActaSeguimGestionProduccionEvaluacion
                                 where r.idProduccionInterventoria == _idProducto
                                 select r).FirstOrDefault();

                    query.ocultar = true;
                    query.fechaUltimaActualizacion = DateTime.Now;
                    query.codContacto = _codContacto;

                    db.SubmitChanges();

                    ocultado = true;
                }
            }
            

            return ocultado;
        }

        private int cantProductosXProyecto(int _codProyecto)
        {
            int cant = 0;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                cant = (from r in db.ActaSeguimGestionProduccionEvaluacion
                        where r.codProyecto == _codProyecto && r.ocultar == false
                        select r).Count();
            }

            return cant;
        }

        public bool actualizarProducto(int _idProducto, int _codContacto, int _cantidad, string _unidadMedida
                            , string _nomProducto, bool _productoRepresentativo)
        {
            bool actualizado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(_cadena))
            {
                var query = (from r in db.ActaSeguimGestionProduccionEvaluacion
                             where r.idProduccionInterventoria == _idProducto
                             select r).FirstOrDefault();

                query.fechaUltimaActualizacion = DateTime.Now;
                query.codContacto = _codContacto;
                query.unidades = _cantidad;
                query.unidadMedida = _unidadMedida;
                query.nomProducto = _nomProducto;

                if (_productoRepresentativo)
                {
                    //Actulizar mas representativo para evaluacion

                    (from p in db.ActaSeguimGestionProduccionEvaluacion
                     where p.codProyecto == query.codProyecto
                     select p).ToList().ForEach(x => x.productoRepresentativo = false);

                    var listProductos = (from q in db.ActaSeguimGestionProduccionEvaluacion
                                         where q.codProyecto == query.codProyecto 
                                         && q.idProduccionInterventoria == query.idProduccionInterventoria
                                         select q).FirstOrDefault();

                    listProductos.productoRepresentativo = _productoRepresentativo;

                    //Actulizar mas representativo para las actas

                    (from p in db.ActaSeguimGestionProduccion
                     where p.codProyecto == query.codProyecto
                     select p).ToList().ForEach(x => x.productoRepresentativo = false);

                    (from p in db.ActaSeguimGestionProduccion
                     where p.codProyecto == query.codProyecto
                      && p.codProducto == query.idProduccionInterventoria
                     select p).ToList().ForEach(x => x.productoRepresentativo = true);
                }

                db.SubmitChanges();

                actualizado = true;
            }

            return actualizado;
        }
    }
}
