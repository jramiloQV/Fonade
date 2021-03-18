using Datos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Fonade.Negocio.PlanDeNegocioV2.Interventoria
{
    public class SeguimientoInterventoriaBLL
    {
        string conexion = System.Configuration.ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
        public bool agregarHabilitarActa(int _codProyecto, int _codUsuario, int _numActa, ref string errorMensaje)
        {
            bool insertado = false;

            try
            {
                if (!existeHabilitaActa(_numActa, _codProyecto))
                {
                    using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                    {
                        HabilitarSeguimientoInterventoria habilitar = new HabilitarSeguimientoInterventoria
                        {
                            codProyecto = _codProyecto,
                            codUsuarioCrea = _codUsuario,
                            fechaCreacion = DateTime.Now,
                            habilitado = true,
                            numActa = _numActa
                        };

                        var habActaSeguimiento = (from h in db.HabilitarSeguimientoInterventoria
                                                  where h.codProyecto == _codProyecto && h.numActa == _numActa
                                                  select h).FirstOrDefault();

                        if (habActaSeguimiento == null)
                        {
                            db.HabilitarSeguimientoInterventoria.InsertOnSubmit(habilitar);
                        }
                        else
                        {
                            habActaSeguimiento.habilitado = true;
                        }
                        db.SubmitChanges();
                    }

                    insertado = true;
                }
                else
                {
                    insertado = false;
                    errorMensaje = "Ya se habilitó el acta No. " + _numActa;
                }
            }
            catch (Exception ex)
            {
                errorMensaje = ex.Message;
                insertado = false;
            }

            return insertado;
        }

        public bool deshabilitarHabilitarActa(int _codProyecto, int _numActa, ref string error)
        {
            bool deshabilitar = false;
            try
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    var habilitaActa = (from h in db.HabilitarSeguimientoInterventoria
                                        where h.codProyecto == _codProyecto && h.numActa == _numActa
                                        select h
                                        ).FirstOrDefault();

                    if (habilitaActa != null)
                    {
                        habilitaActa.habilitado = false;
                        db.SubmitChanges();
                    }

                    deshabilitar = true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return deshabilitar;
        }

        public bool existeHabilitaActa(int _numActa, int _codProyecto)
        {
            bool existe = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var habilitaActa = (from h in db.HabilitarSeguimientoInterventoria
                                    where h.codProyecto == _codProyecto && h.numActa == _numActa
                                    && h.habilitado == true
                                    select h
                                    ).Count();

                if (habilitaActa > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        public List<ActasIdNomModel> getListActasHabilitadas(int _codProyecto)
        {
            List<ActasIdNomModel> listActas = new List<ActasIdNomModel>();
            ActasIdNomModel actaSel = new ActasIdNomModel
            {
                idActa = "S",
                NomActa = "Seleccione..."
            };

            listActas.Add(actaSel);

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var listActasBD = (from h in db.HabilitarSeguimientoInterventoria
                                   where h.codProyecto == _codProyecto && h.habilitado == true
                                   orderby h.numActa
                                   select new ActasIdNomModel
                                   {
                                       idActa = h.numActa.ToString(),
                                       NomActa = "Acta " + h.numActa
                                   }).ToList();

                listActas.AddRange(listActasBD);
            }

            return listActas;
        }

        public bool UploadFile(FileUpload archivo, string _rutaArchivo, int _codProyecto, int _codUsuario
                                    , int _numActa, string nombreArchivo, ref string Error)
        {
            bool cargado = false;

            try
            {
                string rutaProyecto = _rutaArchivo + _codProyecto + "\\";
                string rutaFinal = rutaProyecto + nombreArchivo;

                if (!Directory.Exists(rutaProyecto))
                    Directory.CreateDirectory(rutaProyecto);
                if (File.Exists(rutaFinal))
                    File.Delete(rutaFinal);
                //if (Directory.Exists(OutPutDirectory))
                //Directory.Delete(OutPutDirectory);

                archivo.SaveAs(rutaFinal);

                cargado = true;
            }
            catch (Exception ex)
            {
                cargado = false;
                Error = ex.Message;
            }
            return cargado;
        }

        public bool ingresarRegistroArchivo(int _codproyecto, int _codContacto, string _nomArchivo
                                            , int _numActa, ref string error)
        {
            bool ingresado = false;

            try
            {
                using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
                {
                    string rutaDef = "Documentos/SeguiVirtualInterventoria/" + _codproyecto + "/" + _nomArchivo;

                    var habActaSeguimiento = (from h in db.ArchivosSeguimInterventoria
                                              where h.codProyecto == _codproyecto && h.numActa == _numActa
                                              && h.ruta == rutaDef && h.Borrado == false
                                              select h).FirstOrDefault();
                    if (habActaSeguimiento == null)
                    {
                        ArchivosSeguimInterventoria archivo = new ArchivosSeguimInterventoria
                        {
                            codProyecto = _codproyecto,
                            Borrado = false,
                            codContactoCarga = _codContacto,
                            fechaCarga = DateTime.Now,
                            nomArchivo = _nomArchivo,
                            numActa = _numActa,
                            ruta = rutaDef
                        };

                        db.ArchivosSeguimInterventoria.InsertOnSubmit(archivo);
                    }
                    else
                    {
                        habActaSeguimiento.fechaCarga = DateTime.Now;
                    }
                    db.SubmitChanges();
                    ingresado = true;
                }
            }
            catch (Exception ex)
            {
                ingresado = false;
                error = ex.Message;
            }

            return ingresado;
        }

        public bool existeArchivoActa(int _codProyecto, int _numActa)
        {
            bool existe = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var mostrarGrilla = (from h in db.ArchivosSeguimInterventoria
                                     where h.codProyecto == _codProyecto && h.numActa == _numActa
                                     && h.Borrado == false
                                     select h
                                    ).Count();

                if (mostrarGrilla > 0)
                {
                    existe = true;
                }
            }

            return existe;
        }

        public List<ArchivosActas> archivosActas(int _codProyecto, int _numActa)
        {
            List<ArchivosActas> archivos = new List<ArchivosActas>();

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                archivos = (from h in db.ArchivosSeguimInterventoria
                            where h.codProyecto == _codProyecto && h.numActa == _numActa
                            && h.Borrado == false
                            select new ArchivosActas
                            {
                                Borrado = h.Borrado,
                                codContactoCarga = h.codContactoCarga,
                                codProyecto = h.codProyecto,
                                fechaCarga = h.fechaCarga,
                                idArchivoSeguimInterventoria = h.idArchivoSeguimInterventoria,
                                nomArchivo = h.nomArchivo,
                                numActa = h.numActa,
                                ruta = h.ruta
                            }).ToList();

                //buscar NombresContacto
                foreach (var i in archivos)
                {
                    i.contacto = nombreContacto(i.codContactoCarga);
                }
            }

            return archivos;
        }

        private string nombreContacto(int _codContacto)
        {
            string nombre = "";

            using (FonadeDBDataContext db = new FonadeDBDataContext(conexion))
            {
                nombre = (from h in db.Contacto
                          where h.Id_Contacto == _codContacto
                          select h.Nombres + " " + h.Apellidos
                            ).FirstOrDefault();
            }

            return nombre;
        }

        public bool archivoActaHabilitado(int _codProyecto, int _numActa)
        {
            bool habilitado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var hab = (from h in db.HabilitarSeguimientoInterventoria
                                    where h.codProyecto == _codProyecto
                                    && h.numActa == _numActa && h.habilitado == true
                                    select h
                               ).Count();

                if (hab > 0)
                {
                    habilitado = true;
                }
            }

            return habilitado;
        }
                
        public bool eliminarArchivo(int _idArchivoActa, int _codContacto)
        {
            bool borrado = false;

            using (FonadeDBLightDataContext db = new FonadeDBLightDataContext(conexion))
            {
                var borrarArchivo = (from a in db.ArchivosSeguimInterventoria
                                        where a.idArchivoSeguimInterventoria == _idArchivoActa
                                     select a
                                    ).FirstOrDefault();

                if (borrarArchivo!=null)
                {
                    borrarArchivo.Borrado = true;
                    borrarArchivo.eliminadoPor = _codContacto;
                    db.SubmitChanges();

                    borrado = true;
                }
            }

            return borrado;
        }
    }

    public class ArchivosActas
    {
        public int idArchivoSeguimInterventoria { get; set; }
        public int codProyecto { get; set; }
        public int numActa { get; set; }
        public string ruta { get; set; }
        public string nomArchivo { get; set; }
        public DateTime fechaCarga { get; set; }
        public int codContactoCarga { get; set; }
        public string contacto { get; set; }
        public bool Borrado { get; set; }
    }

    public class ActasIdNomModel
    {
        public string idActa { get; set; }
        public string NomActa { get; set; }
    }
}
