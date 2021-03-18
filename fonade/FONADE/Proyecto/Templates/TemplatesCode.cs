namespace Fonade.FONADE.Proyecto.Templates
{
    public partial class ProyectoTabRealizado {
        private Datos.TabProyecto query;
        private bool nuevo;
        private bool disabled;
        private bool guardar;

        public ProyectoTabRealizado(Datos.TabProyecto query, bool nuevo, bool disabled, bool guardar)
        {            
            this.query = query;
            this.nuevo = nuevo;
            this.disabled = disabled;
            this.guardar = guardar;
        } 
    }
}