#region Diego Quiñonez

// <Author>Diego Quiñonez</Author>
// <Fecha>08 - 07 - 2014</Fecha>
// <Archivo>FrameAsesorProyecto.cs</Archivo>

#endregion

using Datos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fonade.Clases;

namespace Fonade
{
    /// <summary>
    /// FrameAsesorProyecto
    /// </summary>    
    public partial class FrameAsesorProyecto : Negocio.Base_Page
    {
        #region variables globales

        int CodInstitucion;
        int CodProyecto = 0;

        #endregion

        int IdProyecto { get; set; }
        int IdContacto { get; set; }

        int IdRol { get; set; }

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// metodo de carga
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //recoge el codigo de la institucion relaciona con el usuario de Session
            CodInstitucion = usuario.CodInstitucion;

            if (CodProyecto == 0)
            {
                gvrasesorlider.Visible = false;
                gvrasesores.Visible = false;
                lnkasignacionasesores.Visible = false;
                btnactualizar.Visible = false;
                lbltitulo.Text = "Para ver los Asesores de un plan de negocio, seleccione uno a la izquierda.";
            }
        }

        #region cargue datos

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// lista todos los proyectos relacionados con la institucion
        /// y los dibuja en la grilla gv_proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ldsproyectos_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var proyecto = (from p in consultas.Db.Proyecto
                            orderby p.Id_Proyecto
                            where p.Inactivo == false
                            && p.CodInstitucion == CodInstitucion
                            select new
                            {
                                p.Id_Proyecto,
                                NomProyecto = p.NomProyecto.htmlDecode()
                            });

            e.Result = proyecto.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// carga el asesor lider en la grilla gvrasesorlider
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ldsasesorlider_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var asesor = (from c in consultas.Db.Contacto
                          join pc in consultas.Db.ProyectoContactos 
                          on c.Id_Contacto equals pc.CodContacto
                          join p in consultas.Db.Proyecto
                          on pc.CodProyecto equals p.Id_Proyecto
                          where pc.FechaFin == null
                          && pc.CodRol == 1
                          && pc.CodProyecto == CodProyecto
                          && p.CodInstitucion == CodInstitucion

                          select new
                          {
                              Nombre = c.Nombres + " " + c.Apellidos,
                              c.Email
                          }).Distinct();

            e.Result = asesor.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// carga los asesores relacionados al proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ldsasesores_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var asesor = (from c in consultas.Db.Contacto
                          join pc in consultas.Db.ProyectoContactos
                          on c.Id_Contacto equals pc.CodContacto
                          join p in consultas.Db.Proyecto
                          on pc.CodProyecto equals p.Id_Proyecto
                          where pc.FechaFin == null
                          && pc.CodRol == 2
                          && pc.CodProyecto == CodProyecto
                          && p.CodInstitucion == CodInstitucion
                          select new
                          {
                              Nombre = c.Nombres + " " + c.Apellidos,
                              c.Email
                          }).Distinct();

            e.Result = asesor.ToList();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// carga los asesores disponibles para asignar a un proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ldsasesoresasignar_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            var asesores = (from c in consultas.Db.Contacto
                            from gc in consultas.Db.GrupoContactos
                            where gc.CodGrupo == 5
                            && c.Id_Contacto == gc.CodContacto
                            && c.CodInstitucion == CodInstitucion
                            select new
                            {
                                c.Id_Contacto,
                                Nombre = c.Nombres + " " + c.Apellidos,
                            }).Distinct(); ;

            e.Result = asesores.ToList();
        }

        #endregion

        #region eventos grilla proyectos

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// genera el paginado de la grilla gv_proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_proyectos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //obtiene el indice de la nueva pagina
            gv_proyectos.PageIndex = e.NewPageIndex;
        }

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// agrega informacion en la creacion de la grilla proyectos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_proyectos_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //recoge el id del proyecto de cada fila de la grilla proyectos
                int idproyecto = Convert.ToInt32(gv_proyectos.DataKeys[e.Row.RowIndex].Value.ToString());

                var contactoLider = (from pc in consultas.Db.ProyectoContactos
                                     where pc.CodProyecto == idproyecto
                                     && pc.FechaFin == null
                                     && (pc.CodRol == Constantes.CONST_RolAsesorLider)
                                     select pc.CodContacto).FirstOrDefault();

                //si no hay un asesor lider asignado muestra un icono de admiracion que de aviso al usuario
                if (!(contactoLider > 0))
                {
                    e.Row.FindControl("imgadmiracion").Visible = true;
                }

                var contactos = (from pc in consultas.Db.ProyectoContactos
                                 where pc.CodProyecto == idproyecto
                                 && pc.FechaFin == null
                                 && (pc.CodRol == Constantes.CONST_RolAsesorLider || pc.CodRol == Constantes.CONST_RolAsesor)
                                 select pc.CodContacto).Count();

                //muestra cuantos asesores estan asignados a cada proyecto
                ((Label)e.Row.FindControl("lblcontactos")).Text = contactos.ToString() + " Asesores";
            }
            //siempre inicia en -1 un argumento fura de rango
            catch (ArgumentOutOfRangeException) { }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 08 - 07 - 2014
        /// recoge el id del proyecto
        /// para cargar los asesores asociados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gv_proyectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Ver"))
            {
                int parametro = Convert.ToInt32(e.CommandArgument.ToString());

                cargueAsesores(parametro);
            }
        }

        #endregion

        #region eventos grilla gvrasignarasesores

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// al momento de crear la grilla
        /// selecciona los asesores asignados
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvrasignarasesores_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                //recoge el id del proyecto de la grilla
                var id = Convert.ToInt32(gvrasignarasesores.DataKeys[e.Row.RowIndex].Value.ToString());

                //trae los asesores relacionados al proyecto 
                var rol = (from pc in consultas.Db.ProyectoContactos
                           where pc.CodContacto == id
                           && pc.CodProyecto == Convert.ToInt32(hdproyecto.Value)
                           && pc.FechaFin == null
                           select pc.CodRol).FirstOrDefault();

                //si es asesor el checked pasa a true
                ((CheckBox)e.Row.FindControl("cbxasesor")).Checked = (rol == 1 || rol == 2);

                //si es asesor lider el checked pasa a true
                ((RadioButton)e.Row.FindControl("rbasesorlider")).Checked = rol == 1?true:false;
            }
            //siempre inicia en -1 un argumento fura de rango
            catch (ArgumentOutOfRangeException) { }
            //se dispara al cambiar de proyecto
            catch (FormatException) { }
        }

        #endregion

        #region metodos generales

        private void cargueAsesores(int id_proyecto)
        {
            hdproyecto.Value = id_proyecto.ToString();

            CodProyecto = id_proyecto;
            infoProyecto(id_proyecto);
            gvrasesorlider.DataBind();
            gvrasesores.DataBind();

            gvrasesorlider.Visible = true;
            gvrasesores.Visible = true;
            lnkasignacionasesores.Visible = true;
            gvrasignarasesores.Visible = false;
            btnactualizar.Visible = false;
        }

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// establece el titulo del proyecto
        /// </summary>
        /// <param name="id_proyecto"></param>
        private void infoProyecto(int id_proyecto)
        {
            //devuelve el estado y el nombre del proyecto de la BD de acuerdo al id del proyecto
            var proyecto = (from p in consultas.Db.Proyecto
                            where p.Id_Proyecto == id_proyecto 
                            && p.CodInstitucion == usuario.CodInstitucion
                            select new
                            {
                                p.NomProyecto,
                                p.CodEstado
                            }).FirstOrDefault(); 
            lbltitulo.Text = string.Format("Asesores para el Plan de Negocio {0}", proyecto!=null?proyecto.NomProyecto:string.Empty);
        }

        protected void lnkasignacionasesores_Click(object sender, EventArgs e)
        {
            gvrasesores.Visible = false;
            gvrasesorlider.Visible = false;
            gvrasignarasesores.Visible = true;
            btnactualizar.Visible = true;
            infoProyecto(Convert.ToInt32(hdproyecto.Value));
            gvrasignarasesores.DataBind();
        }

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// solo es un lider pro proyecto
        /// selecciona solo el que a asignado el usuario
        /// y el resto le asigna como false
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rbasesorlider_CheckedChanged(object sender, EventArgs e)
        {
            //recorre las filas de la grilla
            foreach (GridViewRow gvr in gvrasignarasesores.Rows)
            {
                //busca los radiobutton para pasar el checked a flase
                ((RadioButton)gvr.FindControl("rbasesorlider")).Checked = false;
            }

            //el que selecciono el usuario lo asigna a true
            ((RadioButton)sender).Checked = true;
            //coloca informacion del proyecto
            //infoProyecto(Convert.ToInt32(hdproyecto.Value));
            btnactualizar.Visible = true;
        }

        #endregion

        /// <summary>
        /// Diego Quiñonez
        /// 09 - 07 - 2014
        /// actualiza la lista de  asesores
        /// que se asignan a un proyecto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnactualizar_Click(object sender, EventArgs e){
            GridViewRow[] asesores = new GridViewRow[gvrasignarasesores.Rows.Count];
            gvrasignarasesores.Rows.CopyTo(asesores,0);
            Array.ForEach<GridViewRow>(asesores, asignarAsesor);
            cargueAsesores(Convert.ToInt32(hdproyecto.Value));
            if (ScriptManager.GetCurrent(this).GetRegisteredClientScriptBlocks().Where(s => Equals(s.Key, "updated")).Count() == 0){
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "updated", "document.execCommand('refresh')", true);
            }
        }

        /// <summary>
        /// Diego Quiñonez
        /// 18 - 07 - 2014
        /// guarda la relacion del usuario y proyecto en BD
        /// </summary>
        /// <param name="id"></param>
        private void insertar(int id)
        {

            Datos.ProyectoContacto pc = new Datos.ProyectoContacto();


            pc.CodProyecto = Convert.ToInt32(hdproyecto.Value);
            pc.CodContacto = id;
            pc.CodRol = Convert.ToByte(IdRol);
            pc.FechaInicio = DateTime.Now;
            pc.Inactivo = false;
            pc.Id_ProyectoContacto = 0;
            var yhn = consultas.Db.ProyectoContactos.Where(c => c.Id_ProyectoContacto == pc.Id_ProyectoContacto).ToList();
            if (yhn.Count!=0) return;
            consultas.Db.ProyectoContactos.InsertOnSubmit(pc);
            try
            {
                consultas.Db.SubmitChanges();
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Asignars the asesor.
        /// </summary>
        /// <param name="row_">The row.</param>
        protected void asignarAsesor(GridViewRow row_){
            var ckAsesor = ((CheckBox)row_.FindControl("cbxasesor")).Checked;
            var rbAsesorLider = ((RadioButton)row_.FindControl("rbasesorlider")).Checked;
            var idAsesor = (int)gvrasignarasesores.DataKeys[row_.RowIndex].Value;
            IdRol = ckAsesor && rbAsesorLider ? 1 : 2;
            var queryProyectoContacto = consultas.Db.ProyectoContactos.Where(
                p => (p.CodRol == 1 &&
                p.CodContacto == idAsesor &&
                p.CodProyecto.ToString() == hdproyecto.Value) ||
                (p.CodRol == 2 && p.CodProyecto == int.Parse(hdproyecto.Value) &&
                p.CodContacto == idAsesor)).Select(s => s).ToList();
            if (queryProyectoContacto.Count == 0 && (ckAsesor)) { insertar(idAsesor); }
            else{
                queryProyectoContacto = 
                consultas.Db.ProyectoContactos.Where(
                p => (p.CodRol == 1 &&
                p.CodContacto == idAsesor &&
                p.CodProyecto == int.Parse(hdproyecto.Value)) ||
                (p.CodRol == 2 && p.CodProyecto == int.Parse(hdproyecto.Value) &&
                p.CodContacto == idAsesor)).Select(s => s).ToList();
                if (queryProyectoContacto.Count != 0){ 
                    var proyectoContacto = queryProyectoContacto.Count==0? 
                        new Datos.ProyectoContacto(): queryProyectoContacto.FirstOrDefault();
                    proyectoContacto.CodRol = Convert.ToByte(IdRol);
                    proyectoContacto.Inactivo = !ckAsesor;
                    proyectoContacto.FechaFin = !ckAsesor ? DateTime.Today : new Nullable<DateTime>();
                    consultas.Db.SubmitChanges();                
                }
            }
        }


    }
}