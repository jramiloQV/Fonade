using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Datos;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Runtime.Caching;
using Datos.DataType;


namespace Fonade.Account
{
    /// <summary>
    /// FonadeMembershipProvider : MembershipProvider
    /// </summary>
    /// <seealso cref="System.Web.Security.MembershipProvider" />
    public class FonadeMembershipProvider : MembershipProvider
    {
        Consultas db;
        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        //private MembershipPasswordFormat pPasswordFormat;

        /// <summary>
        /// inicializa valores del web.config
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {


            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "FonadeMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Fonade membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);
            db = new Consultas();

            pApplicationName = GetConfigValue(config["applicationName"],
                                     System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));
        }

        /// <summary>
        /// Nombre de la aplicación que utiliza el proveedor de membresía personalizado.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        //
        // Used when determining encryption key values.
        //

        //private MachineKeySection machineKey;

        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog;

        /// <summary>
        /// Obtiene o establece un valor que indica si [escribe excepciones en el registro de eventos].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [write exceptions to event log]; otherwise, <c>false</c>.
        /// </value>
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }
        //
        // A helper function to retrieve config values from the configuration file.
        //

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        /// <summary>
        /// Procesa una solicitud para actualizar la contraseña de un usuario.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Procesa una solicitud para actualizar la pregunta de la contraseña y la respuesta para un usuario
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///Agrega un nuevo usuario al origen de datos.
        /// </summary>
        /// <param name="username">Nick del nuevo usuario.</param>
        /// <param name="password">Contraseña del nuevo usuario.</param>
        /// <param name="email">Email del usuario nuevo.</param>
        /// <param name="passwordQuestion">Pregunta de contraseña del nuevo usuario.</param>
        /// <param name="passwordAnswer">Respuesta de la pregunta del nuevo usuario</param>
        /// <param name="isApproved">Si el nuevo usuario está o no aprobado para ser valido</param>
        /// <param name="providerUserKey">El identificador único del usuario.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus" /> valor de enumeración que indica si el usuario se creó correctamente.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> objeto con la información para el usuario recién creado.
        /// </returns>
        public override MembershipUser CreateUser(string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            FonadeUser u = (FonadeUser)GetUser(username, false);
            if (u == null)
            {
                try
                {
                    db.CrearContacto(username, password, email);
                    status = MembershipCreateStatus.Success;
                }
                catch (Exception)
                {
                    status = MembershipCreateStatus.UserRejected;
                }

                return GetUser(username, true);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }

        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene información del origen de datos para un usuario. Proporciona una opción para actualizar la fecha / hora de última actividad para el usuario.
        /// </summary>
        /// <param name="username">El nombre del usuario para obtener información .</param>
        /// <param name="userIsOnline">true para actualizar la fecha / hora de última actividad para el usuario; false para devolver la información del usuario sin actualizar la fecha / hora de última actividad para el usuario.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> objeto con la información del usuario especificado del origen de datos.
        /// </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            ObjectCache cache = MemoryCache.Default;
            FonadeUser usuario = (FonadeUser)cache[string.Format("FonadeUser-{0}", username)];

            if (usuario == null || !userIsOnline)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5.0);

                UsuarioFonade contacto = db.GetContacto(username);
                if (contacto == null) return null;
                usuario = new FonadeUser("FonadeMembershipProvider",
                                            contacto.Email,
                                            null,
                                            contacto.Email,
                                            null,
                                            "",
                                            true,
                                            false,
                                            contacto.FechaCreacion,
                                            DateTime.Now,
                                            DateTime.Now,
                                            contacto.FechaCambioClave,
                                            DateTime.Now,
                                            contacto.Nombres,
                                            contacto.Apellidos,
                                            contacto.CodGrupo,
                                            contacto.IdContacto,
                                            contacto.CodInstitucion,
                                            contacto.Identificacion,
                                            contacto.Clave,
                                            contacto.AceptoTerminosYCondiciones,
                                            contacto.CodOperador,
                                            null
                                        );
                cache.Set(string.Format("FonadeUser-{0}", username), usuario, policy);
            }

            return usuario;
        }

        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///Obtiene la longitud mínima requerida para una contraseña.
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { return 3; }
        }

        /// <summary>
        /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Obtiene un valor que indica si el proveedor de membresía está configurado para requerir que el usuario responda una pregunta de contraseña para restablecer y recuperar la contraseña.
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// validacion de usuario y password
        /// </summary>
        /// <param name="username">Usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                return db.ValidarContacto(username, password);
            else
                return false;
        }
    }

    #region OldCode
    /// <summary>
    /// FonadeMembershipProvider : MembershipProvider
    /// </summary>
    /// <seealso cref="System.Web.Security.MembershipProvider" />
    //public class FonadeMembershipProvider : MembershipProvider
    //{
    //    Consultas db;
    //    private string pApplicationName;
    //    private bool pEnablePasswordReset;
    //    private bool pEnablePasswordRetrieval;
    //    private bool pRequiresQuestionAndAnswer;
    //    private bool pRequiresUniqueEmail;
    //    private int pMaxInvalidPasswordAttempts;
    //    private int pPasswordAttemptWindow;
    //    //private MembershipPasswordFormat pPasswordFormat;

    //    /// <summary>
    //    /// inicializa valores del web.config
    //    /// </summary>
    //    /// <param name="name"></param>
    //    /// <param name="config"></param>
    //    public override void Initialize(string name, NameValueCollection config)
    //    {


    //        if (config == null)
    //            throw new ArgumentNullException("config");

    //        if (name == null || name.Length == 0)
    //            name = "FonadeMembershipProvider";

    //        if (String.IsNullOrEmpty(config["description"]))
    //        {
    //            config.Remove("description");
    //            config.Add("description", "Custom Fonade membership provider");
    //        }

    //        // Initialize the abstract base class.
    //        base.Initialize(name, config);
    //        db = new Consultas();

    //        pApplicationName = GetConfigValue(config["applicationName"],
    //                                 System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
    //        pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
    //        pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
    //        pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
    //        pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
    //        pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
    //        pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
    //        pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));
    //    }

    //    /// <summary>
    //    /// Nombre de la aplicación que utiliza el proveedor de membresía personalizado.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException">
    //    /// </exception>
    //    public override string ApplicationName
    //    {
    //        get
    //        {
    //            throw new NotImplementedException();
    //        }
    //        set
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }

    //    //
    //    // Used when determining encryption key values.
    //    //

    //    //private MachineKeySection machineKey;

    //    //
    //    // If false, exceptions are thrown to the caller. If true,
    //    // exceptions are written to the event log.
    //    //

    //    private bool pWriteExceptionsToEventLog;

    //    /// <summary>
    //    /// Obtiene o establece un valor que indica si [escribe excepciones en el registro de eventos].
    //    /// </summary>
    //    /// <value>
    //    ///   <c>true</c> if [write exceptions to event log]; otherwise, <c>false</c>.
    //    /// </value>
    //    public bool WriteExceptionsToEventLog
    //    {
    //        get { return pWriteExceptionsToEventLog; }
    //        set { pWriteExceptionsToEventLog = value; }
    //    }
    //    //
    //    // A helper function to retrieve config values from the configuration file.
    //    //

    //    private string GetConfigValue(string configValue, string defaultValue)
    //    {
    //        if (String.IsNullOrEmpty(configValue))
    //            return defaultValue;

    //        return configValue;
    //    }

    //    /// <summary>
    //    /// Procesa una solicitud para actualizar la contraseña de un usuario.
    //    /// </summary>
    //    /// <param name="username">The user to update the password for.</param>
    //    /// <param name="oldPassword">The current password for the specified user.</param>
    //    /// <param name="newPassword">The new password for the specified user.</param>
    //    /// <returns>
    //    /// true if the password was updated successfully; otherwise, false.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool ChangePassword(string username, string oldPassword, string newPassword)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Procesa una solicitud para actualizar la pregunta de la contraseña y la respuesta para un usuario
    //    /// </summary>
    //    /// <param name="username">The user to change the password question and answer for.</param>
    //    /// <param name="password">The password for the specified user.</param>
    //    /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
    //    /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
    //    /// <returns>
    //    /// true if the password question and answer are updated successfully; otherwise, false.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    ///Agrega un nuevo usuario al origen de datos.
    //    /// </summary>
    //    /// <param name="username">Nick del nuevo usuario.</param>
    //    /// <param name="password">Contraseña del nuevo usuario.</param>
    //    /// <param name="email">Email del usuario nuevo.</param>
    //    /// <param name="passwordQuestion">Pregunta de contraseña del nuevo usuario.</param>
    //    /// <param name="passwordAnswer">Respuesta de la pregunta del nuevo usuario</param>
    //    /// <param name="isApproved">Si el nuevo usuario está o no aprobado para ser valido</param>
    //    /// <param name="providerUserKey">El identificador único del usuario.</param>
    //    /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus" /> valor de enumeración que indica si el usuario se creó correctamente.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUser" /> objeto con la información para el usuario recién creado.
    //    /// </returns>
    //    public override MembershipUser CreateUser(string username,
    //        string password,
    //        string email,
    //        string passwordQuestion,
    //        string passwordAnswer,
    //        bool isApproved,
    //        object providerUserKey,
    //        out MembershipCreateStatus status)
    //    {
    //        FonadeUser u = (FonadeUser)GetUser(username, false);
    //        if (u == null)
    //        {
    //            try
    //            {
    //                db.CrearContacto(username, password, email);
    //                status = MembershipCreateStatus.Success;
    //            }
    //            catch (Exception)
    //            {
    //                status = MembershipCreateStatus.UserRejected;
    //            }

    //            return GetUser(username, true);
    //        }
    //        else
    //        {
    //            status = MembershipCreateStatus.DuplicateUserName;
    //        }
    //        return null;
    //    }

    //    /// <summary>
    //    /// Removes a user from the membership data source.
    //    /// </summary>
    //    /// <param name="username">The name of the user to delete.</param>
    //    /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
    //    /// <returns>
    //    /// true if the user was successfully deleted; otherwise, false.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool DeleteUser(string username, bool deleteAllRelatedData)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Indicates whether the membership provider is configured to allow users to reset their passwords.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool EnablePasswordReset
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool EnablePasswordRetrieval
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
    //    /// </summary>
    //    /// <param name="emailToMatch">The e-mail address to search for.</param>
    //    /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
    //    /// <param name="pageSize">The size of the page of results to return.</param>
    //    /// <param name="totalRecords">The total number of matched users.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets a collection of membership users where the user name contains the specified user name to match.
    //    /// </summary>
    //    /// <param name="usernameToMatch">The user name to search for.</param>
    //    /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
    //    /// <param name="pageSize">The size of the page of results to return.</param>
    //    /// <param name="totalRecords">The total number of matched users.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets a collection of all the users in the data source in pages of data.
    //    /// </summary>
    //    /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.</param>
    //    /// <param name="pageSize">The size of the page of results to return.</param>
    //    /// <param name="totalRecords">The total number of matched users.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets the number of users currently accessing the application.
    //    /// </summary>
    //    /// <returns>
    //    /// The number of users currently accessing the application.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override int GetNumberOfUsersOnline()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets the password for the specified user name from the data source.
    //    /// </summary>
    //    /// <param name="username">The user to retrieve the password for.</param>
    //    /// <param name="answer">The password answer for the user.</param>
    //    /// <returns>
    //    /// The password for the specified user name.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override string GetPassword(string username, string answer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Obtiene información del origen de datos para un usuario. Proporciona una opción para actualizar la fecha / hora de última actividad para el usuario.
    //    /// </summary>
    //    /// <param name="username">El nombre del usuario para obtener información .</param>
    //    /// <param name="userIsOnline">true para actualizar la fecha / hora de última actividad para el usuario; false para devolver la información del usuario sin actualizar la fecha / hora de última actividad para el usuario.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUser" /> objeto con la información del usuario especificado del origen de datos.
    //    /// </returns>
    //    public override MembershipUser GetUser(string username, bool userIsOnline)
    //    {
    //        ObjectCache cache = MemoryCache.Default;
    //        FonadeUser usuario = (FonadeUser)cache[string.Format("FonadeUser-{0}", username)];

    //        if (usuario == null || !userIsOnline)
    //        {
    //            CacheItemPolicy policy = new CacheItemPolicy();
    //            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(5.0);

    //            UsuarioFonade contacto = db.GetContacto(username);
    //            if (contacto == null) return null;
    //            usuario = new FonadeUser(   "FonadeMembershipProvider",
    //                                        contacto.Email,
    //                                        null,
    //                                        contacto.Email,
    //                                        null,
    //                                        "",
    //                                        true,
    //                                        false,
    //                                        contacto.FechaCreacion,
    //                                        DateTime.Now,
    //                                        DateTime.Now,
    //                                        DateTime.Now,
    //                                        DateTime.Now,
    //                                        contacto.Nombres,
    //                                        contacto.Apellidos,
    //                                        contacto.CodGrupo,
    //                                        contacto.IdContacto,
    //                                        contacto.CodInstitucion,
    //                                        contacto.Identificacion,
    //                                        contacto.Clave,
    //                                        contacto.AceptoTerminosYCondiciones,
    //                                        contacto.CodOperador
    //                                    );
    //            cache.Set(string.Format("FonadeUser-{0}", username), usuario, policy);
    //        }

    //        return usuario;
    //    }

    //    /// <summary>
    //    /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
    //    /// </summary>
    //    /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
    //    /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
    //    /// <returns>
    //    /// A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets the user name associated with the specified e-mail address.
    //    /// </summary>
    //    /// <param name="email">The e-mail address to search for.</param>
    //    /// <returns>
    //    /// The user name associated with the specified e-mail address. If no match is found, return null.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override string GetUserNameByEmail(string email)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override int MaxInvalidPasswordAttempts
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Gets the minimum number of special characters that must be present in a valid password.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override int MinRequiredNonAlphanumericCharacters
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    ///Obtiene la longitud mínima requerida para una contraseña.
    //    /// </summary>
    //    public override int MinRequiredPasswordLength
    //    {
    //        get { return 3; }
    //    }

    //    /// <summary>
    //    /// Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override int PasswordAttemptWindow
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Gets a value indicating the format for storing passwords in the membership data store.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override MembershipPasswordFormat PasswordFormat
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Gets the regular expression used to evaluate a password.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override string PasswordStrengthRegularExpression
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Obtiene un valor que indica si el proveedor de membresía está configurado para requerir que el usuario responda una pregunta de contraseña para restablecer y recuperar la contraseña.
    //    /// </summary>
    //    public override bool RequiresQuestionAndAnswer
    //    {
    //        get { return false; }
    //    }

    //    /// <summary>
    //    /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
    //    /// </summary>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool RequiresUniqueEmail
    //    {
    //        get { throw new NotImplementedException(); }
    //    }

    //    /// <summary>
    //    /// Resets a user's password to a new, automatically generated password.
    //    /// </summary>
    //    /// <param name="username">The user to reset the password for.</param>
    //    /// <param name="answer">The password answer for the specified user.</param>
    //    /// <returns>
    //    /// The new password for the specified user.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override string ResetPassword(string username, string answer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Clears a lock so that the membership user can be validated.
    //    /// </summary>
    //    /// <param name="userName">The membership user whose lock status you want to clear.</param>
    //    /// <returns>
    //    /// true if the membership user was successfully unlocked; otherwise, false.
    //    /// </returns>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override bool UnlockUser(string userName)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// Updates information about a user in the data source.
    //    /// </summary>
    //    /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user.</param>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public override void UpdateUser(MembershipUser user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //   /// <summary>
    //   /// validacion de usuario y password
    //   /// </summary>
    //   /// <param name="username">Usuario</param>
    //   /// <param name="password">Contraseña</param>
    //   /// <returns></returns>
    //    public override bool ValidateUser(string username, string password)
    //    {
    //        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
    //            return db.ValidarContacto(username, password);
    //        else
    //            return false;            
    //    }
    //}
    #endregion
}