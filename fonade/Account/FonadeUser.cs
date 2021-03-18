using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace Fonade.Account
{
    /// <summary>
    /// Estructura para cargar los datos del usuario logueado
    /// </summary>
    /// <seealso cref="System.Web.Security.MembershipUser" />
    //[Serializable]
    public class FonadeUser : MembershipUser
    {
        private string nombres;

        /// <summary>
        /// Obtiene o establece los nombres.
        /// </summary>
        /// <value>
        /// Nombres.
        /// </value>
        public string Nombres
        {
            get { return nombres; }
            set { nombres = value; }
        }

        private string apellidos;

        /// <summary>
        /// Obtiene o establece los apellidos.
        /// </summary>
        /// <value>
        /// Apellidos
        /// </value>
        public string Apellidos
        {
            get { return apellidos; }
            set { apellidos = value; }
        }

        private string Password;

        /// <summary>
        /// Obtiene o establece el password1.
        /// </summary>
        /// <value>
        /// Password1.
        /// </value>
        public string Password1
        {
            get { return Password; }
            set { Password = value; }
        }

        private int codGrupo;

        /// <summary>
        /// Obtiene o establece el codigo del grupo.
        /// </summary>
        /// <value>
        /// Id del grupo.
        /// </value>
        public int CodGrupo
        {
            get { return codGrupo; }
            set { codGrupo = value; }
        }

        private int idContacto;

        /// <summary>
        /// Obtiene o establece el id del contacto.
        /// </summary>
        /// <value>
        /// id del contacto.
        /// </value>
        public int IdContacto
        {
            get { return idContacto; }
            set { idContacto = value; }
        }

        private int codInstitucion;

        /// <summary>
        /// Obtiene o establece el codigo de la institucion.
        /// </summary>
        /// <value>
        /// Codigo de institucion
        /// </value>
        public int CodInstitucion
        {
            get { return codInstitucion; }
            set { codInstitucion = value; }
        }

        private double identificacion;

        /// <summary>
        /// Obtiene o establece la identificacion.
        /// </summary>
        /// <value>
        /// Identificacion.
        /// </value>
        public double Identificacion
        {
            get { return identificacion; }
            set { identificacion = value; }
        }

        private Boolean aceptoTerminosYCondiciones;

        /// <summary>
        /// Obtiene o establece un valor que indica si[acepto terminos y condiciones].
        /// </summary>
        /// <value>
        ///   <c>true</c> Si [acepto terminos y condiciones]; de otra manera, <c>false</c>.
        /// </value>
        public Boolean AceptoTerminosYCondiciones
        {
            get
            {
                return aceptoTerminosYCondiciones;
            }
            set
            {
                aceptoTerminosYCondiciones = value;
            }
        }

        private int? codOperador;

        public int? CodOperador
        {
            get { return codOperador; }
            set { codOperador = value; }
        }

        private DateTime? fechaTemporal;


        public DateTime? FechaTemporal
        {
            get { return fechaTemporal; }
            set { fechaTemporal = value; }
        }

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="FonadeUser"/> class.
        /// </summary>
        /// <param name="providername">The providername.</param>
        /// <param name="username">The username.</param>
        /// <param name="providerUserKey">The provider user key.</param>
        /// <param name="email">The email.</param>
        /// <param name="passwordQuestion">The password question.</param>
        /// <param name="comment">The comment.</param>
        /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
        /// <param name="isLockedOut">if set to <c>true</c> [is locked out].</param>
        /// <param name="creationDate">The creation date.</param>
        /// <param name="lastLoginDate">The last login date.</param>
        /// <param name="lastActivityDate">The last activity date.</param>
        /// <param name="lastPasswordChangedDate">The last password changed date.</param>
        /// <param name="lastLockedOutDate">The last locked out date.</param>
        /// <param name="nombres">The nombres.</param>
        /// <param name="apellidos">The apellidos.</param>
        /// <param name="codGrupo">The cod grupo.</param>
        /// <param name="idContacto">The identifier contacto.</param>
        /// <param name="codInstitucion">The cod institucion.</param>
        /// <param name="identificacion">The identificacion.</param>
        /// <param name="Password1">The password1.</param>
        /// <param name="aceptoTerminosYCondiciones">if set to <c>true</c> [acepto terminos y condiciones].</param>
        public FonadeUser(string providername, string username, object providerUserKey, string email, string passwordQuestion,
                          string comment, bool isApproved, bool isLockedOut, DateTime creationDate, DateTime lastLoginDate,
                          DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockedOutDate,
                          string nombres, string apellidos, int codGrupo, int idContacto, int codInstitucion
                        , double identificacion, string Password1, Boolean aceptoTerminosYCondiciones, int? _codOperador, DateTime? _fechaTemporal)
                         : base(providername, username, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut,
                                creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockedOutDate)
        {
            this.nombres = nombres;
            this.apellidos = apellidos;
            this.codGrupo = codGrupo;
            this.idContacto = idContacto;
            this.codInstitucion = codInstitucion;
            this.identificacion = identificacion;
            this.Password = Password1;
            this.AceptoTerminosYCondiciones = aceptoTerminosYCondiciones;
            this.CodOperador = _codOperador;
            this.fechaTemporal = _fechaTemporal;
        }

    }

    #region OldCode
    /// <summary>
    /// Estructura para cargar los datos del usuario logueado
    /// </summary>
    /// <seealso cref="System.Web.Security.MembershipUser" />
    //[Serializable]
    //public class FonadeUser : MembershipUser
    //{
    //    private string nombres;

    //    /// <summary>
    //    /// Obtiene o establece los nombres.
    //    /// </summary>
    //    /// <value>
    //    /// Nombres.
    //    /// </value>
    //    public string Nombres
    //    {
    //        get { return nombres; }
    //        set { nombres = value; }
    //    }

    //    private string apellidos;

    //    /// <summary>
    //    /// Obtiene o establece los apellidos.
    //    /// </summary>
    //    /// <value>
    //    /// Apellidos
    //    /// </value>
    //    public string Apellidos
    //    {
    //        get { return apellidos; }
    //        set { apellidos = value; }
    //    }

    //    private string Password;

    //    /// <summary>
    //    /// Obtiene o establece el password1.
    //    /// </summary>
    //    /// <value>
    //    /// Password1.
    //    /// </value>
    //    public string Password1
    //    {
    //        get { return Password; }
    //        set { Password = value; }
    //    }

    //    private int codGrupo;

    //    /// <summary>
    //    /// Obtiene o establece el codigo del grupo.
    //    /// </summary>
    //    /// <value>
    //    /// Id del grupo.
    //    /// </value>
    //    public int CodGrupo
    //    {
    //        get { return codGrupo; }
    //        set { codGrupo = value; }
    //    }

    //    private int idContacto;

    //    /// <summary>
    //    /// Obtiene o establece el id del contacto.
    //    /// </summary>
    //    /// <value>
    //    /// id del contacto.
    //    /// </value>
    //    public int IdContacto
    //    {
    //        get { return idContacto; }
    //        set { idContacto = value; }
    //    }

    //    private int codInstitucion;

    //    /// <summary>
    //    /// Obtiene o establece el codigo de la institucion.
    //    /// </summary>
    //    /// <value>
    //    /// Codigo de institucion
    //    /// </value>
    //    public int CodInstitucion
    //    {
    //        get { return codInstitucion; }
    //        set { codInstitucion = value; }
    //    }

    //    private double identificacion;

    //    /// <summary>
    //    /// Obtiene o establece la identificacion.
    //    /// </summary>
    //    /// <value>
    //    /// Identificacion.
    //    /// </value>
    //    public double Identificacion
    //    {
    //        get { return identificacion; }
    //        set { identificacion = value; }
    //    }

    //    private Boolean aceptoTerminosYCondiciones;

    //    /// <summary>
    //    /// Obtiene o establece un valor que indica si[acepto terminos y condiciones].
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> Si [acepto terminos y condiciones]; de otra manera, <c>false</c>.
    //    /// </value>
    //    public Boolean AceptoTerminosYCondiciones {
    //        get {
    //            return aceptoTerminosYCondiciones;
    //        }
    //        set {
    //            aceptoTerminosYCondiciones = value;
    //        }
    //    }

    //    private int? codOperador;

    //    public int ? CodOperador
    //    {
    //        get { return codOperador; }
    //        set { codOperador = value; }
    //    }

    //    /// <summary>
    //    /// Inicializa una nueva instancia de <see cref="FonadeUser"/> class.
    //    /// </summary>
    //    /// <param name="providername">The providername.</param>
    //    /// <param name="username">The username.</param>
    //    /// <param name="providerUserKey">The provider user key.</param>
    //    /// <param name="email">The email.</param>
    //    /// <param name="passwordQuestion">The password question.</param>
    //    /// <param name="comment">The comment.</param>
    //    /// <param name="isApproved">if set to <c>true</c> [is approved].</param>
    //    /// <param name="isLockedOut">if set to <c>true</c> [is locked out].</param>
    //    /// <param name="creationDate">The creation date.</param>
    //    /// <param name="lastLoginDate">The last login date.</param>
    //    /// <param name="lastActivityDate">The last activity date.</param>
    //    /// <param name="lastPasswordChangedDate">The last password changed date.</param>
    //    /// <param name="lastLockedOutDate">The last locked out date.</param>
    //    /// <param name="nombres">The nombres.</param>
    //    /// <param name="apellidos">The apellidos.</param>
    //    /// <param name="codGrupo">The cod grupo.</param>
    //    /// <param name="idContacto">The identifier contacto.</param>
    //    /// <param name="codInstitucion">The cod institucion.</param>
    //    /// <param name="identificacion">The identificacion.</param>
    //    /// <param name="Password1">The password1.</param>
    //    /// <param name="aceptoTerminosYCondiciones">if set to <c>true</c> [acepto terminos y condiciones].</param>
    //    public FonadeUser(string providername, string username, object providerUserKey, string email, string passwordQuestion,
    //                      string comment,bool isApproved,bool isLockedOut,DateTime creationDate,DateTime lastLoginDate,
    //                      DateTime lastActivityDate,DateTime lastPasswordChangedDate,DateTime lastLockedOutDate,
    //                      string nombres,string apellidos,int codGrupo,int idContacto,int codInstitucion
    //                    ,double identificacion,string Password1, Boolean aceptoTerminosYCondiciones, int? _codOperador) 
    //                     : base(providername,username, providerUserKey,email,passwordQuestion,comment,isApproved,isLockedOut,
    //                            creationDate,lastLoginDate,lastActivityDate,lastPasswordChangedDate,lastLockedOutDate)
    //    {
    //        this.nombres = nombres;
    //        this.apellidos = apellidos;
    //        this.codGrupo = codGrupo;
    //        this.idContacto = idContacto;
    //        this.codInstitucion = codInstitucion;
    //        this.identificacion = identificacion;
    //        this.Password = Password1;
    //        this.AceptoTerminosYCondiciones = aceptoTerminosYCondiciones;
    //        this.CodOperador = _codOperador;
    //    }

    //}
    #endregion
}