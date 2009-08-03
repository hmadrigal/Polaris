﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using Polaris.Bal;
using Polaris.Bal.Extensions;
using Polaris.Bal.DataRepositories;

namespace Polaris.Pal.Code
{
    /// <summary>
    /// Internal Authentication provider 
    /// </summary>
    /// <remarks>
    /// See override methods at http://msdn.microsoft.com/en-us/library/f1kyba5e.aspx
    /// </remarks>
    public class PolarisMembershipProvider : MembershipProvider
    {
        private ISiteRepository SiteRepository { get; set; }

        #region Required ProviderBase Methods
        /// <summary>
        /// Takes, as input, the name of the provider and a  NameValueCollection  of configuration settings. Used to set property values for the provider instance including implementation-specific values and options specified in the configuration file (Machine.config or Web.config) supplied in the configuration
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            // Default Values
            this.applicationName = String.Empty;
            this.enablePasswordReset = true;
            this.enablePasswordRetrieval = false;
            this.maxInvalidPasswordAttempts = 3;
            this.minRequiredNonAlphanumericCharacters = 0;
            this.minRequiredPasswordLength = 8;
            this.passwordAttemptWindow = 0;
            this.passwordFormat = MembershipPasswordFormat.Hashed;
            this.passwordStrengthRegularExpression = String.Empty;
            this.requiresQuestionAndAnswer = false;
            this.requiresUniqueEmail = true;

            // Tries to override the default values if the settings is provided thru the web.config
            foreach (string key in config.Keys)
            {
                var value = config[key];
                switch (key)
                {
                    case @"applicationName":
                        this.applicationName = value;
                        break;
                    case @"enablePasswordReset":
                        this.enablePasswordReset = value.ToBoolean(this.enablePasswordReset);
                        break;
                    case @"enablePasswordRetrieval":
                        this.enablePasswordRetrieval = value.ToBoolean(this.enablePasswordRetrieval);
                        break;
                    case @"maxInvalidPasswordAttempts":
                        this.maxInvalidPasswordAttempts = value.ToInt32(this.maxInvalidPasswordAttempts);
                        break;
                    case @"minRequiredNonAlphanumericCharacters":
                        this.minRequiredNonAlphanumericCharacters = value.ToInt32(this.minRequiredNonAlphanumericCharacters);
                        break;
                    case @"minRequiredPasswordLength":
                        this.minRequiredPasswordLength = value.ToInt32(this.minRequiredPasswordLength);
                        break;
                    case @"passwordAttemptWindow":
                        this.passwordAttemptWindow = value.ToInt32(this.passwordAttemptWindow);
                        break;
                    case @"passwordFormat":
                        this.passwordFormat = value.ToEnum(this.passwordFormat);
                        break;
                    case @"passwordStrengthRegularExpression":
                        this.passwordStrengthRegularExpression = value;
                        break;
                    case @"requiresQuestionAndAnswer":
                        this.requiresQuestionAndAnswer = value.ToBoolean(this.requiresQuestionAndAnswer);
                        break;
                    case @"requiresUniqueEmail":
                        this.requiresUniqueEmail = value.ToBoolean(this.requiresUniqueEmail);
                        break;
                }
            }

            this.SiteRepository = RepositoryFactory.GetNewRepository<ISiteRepository>();

            base.Initialize(name, config);
        }
        #endregion

        #region Required MembershipProvider Properties
        /// <summary>
        /// The name of the application using the membership information specified in the configuration file (Web.config). The  ApplicationName   is stored in the data source with related user information and used when querying for that information. See the section on the  ApplicationName   later in this topic for more information.
        /// This property is read/write and defaults to the ApplicationPath if not specified explicitly.
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                return applicationName;
            }
            set
            {
                applicationName = value;
            }
        }
        private string applicationName;
        /// <summary>
        /// A Boolean value specified in the configuration file (Web.config).
        /// The EnablePasswordReset property indicates whether users can use the ResetPassword method to overwrite their current password with a new, randomly generated password.
        /// This property is read-only.
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { return enablePasswordReset; }
        }
        private bool enablePasswordReset;

        /// <summary>
        /// A Boolean value specified in the configuration file (Web.config).
        /// The EnablePasswordRetrieval property indicates whether users can retrieve their password using the GetPassword method.
        /// This property is read-only.
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { return enablePasswordRetrieval; }
        }
        private bool enablePasswordRetrieval;

        /// <summary>
        /// An Integer value specified in the configuration file (Web.config).
        /// The MaxInvalidPasswordAttempts works in conjunction with the PasswordAttemptWindow to guard against an unwanted source guessing the password or password answer of a membership user through repeated attempts. If the number of invalid passwords or password questions supplied for a membership user exceeds the MaxInvalidPasswordAttempts within the number of minutes identified by the PasswordAttemptWindow , then the membership user is locked out by setting the IsLockedOut property to true until the user is unlocked using the UnlockUser method. If a valid password or password answer is supplied before the MaxInvalidPasswordAttempts is reached, the counter that tracks the number of invalid attempts is reset to zero.
        /// If the RequiresQuestionAndAnswer property is set to false, invalid password answer attempts are not tracked.
        /// Invalid password and password answer attempts are tracked in the ValidateUser , ChangePassword , ChangePasswordQuestionAndAnswer , GetPassword , and ResetPassword methods.
        /// This property is read-only.
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return maxInvalidPasswordAttempts; }
        }
        private int maxInvalidPasswordAttempts;

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return minRequiredNonAlphanumericCharacters; }
        }
        private int minRequiredNonAlphanumericCharacters;

        public override int MinRequiredPasswordLength
        {
            get { return minRequiredPasswordLength; }
        }
        private int minRequiredPasswordLength;

        /// <summary>
        /// An Integer value specified in the configuration file (Web.config).
        /// For a description, see the description of the MaxInvalidPasswordAttempts property.
        /// This property is read-only.
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get { return passwordAttemptWindow; }
        }
        private int passwordAttemptWindow;

        /// <summary>
        /// A  MembershipPasswordFormat   value specified in the configuration file (Web.config).
        /// The  PasswordFormat   property indicates the format that passwords are stored in. Passwords can be stored in Clear, Encrypted, and Hashed password formats. Clear passwords are stored in plain text, which improves the performance of password storage and retrieval but is less secure, as passwords are easily read if your data source is compromised. Encrypted passwords are encrypted when stored and can be decrypted for password comparison or password retrieval. This requires additional processing for password storage and retrieval but is more secure, as passwords are not easily determined if the data source is compromised. Hashed passwords are hashed using a one-way hash algorithm and a randomly generated salt value when stored in the database. When a password is validated, it is hashed with the salt value in the database for verification. Hashed passwords cannot be retrieved.
        /// You can use the EncryptPassword and DecryptPassword virtual methods of the MembershipProvider class to encrypt and decrypt password values, or you can supply your own encryption code. If you use the EncryptPassword and DecryptPassword virtual methods of the MembershipProvider class, Encrypted passwords are encrypted using the key information supplied in the machineKey element in the configuration file.
        /// This property is read-only.
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return passwordFormat; }
        }
        private MembershipPasswordFormat passwordFormat;

        public override string PasswordStrengthRegularExpression
        {
            get { return passwordStrengthRegularExpression; }
        }
        private string passwordStrengthRegularExpression;

        /// <summary>
        /// A Boolean value specified in the configuration file (Web.config).
        /// The RequiresQuestionAndAnswer property indicates whether users must supply a password answer in order to retrieve their password using the GetPassword method, or reset their password using the ResetPassword method.
        /// This property is read-only.
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return requiresQuestionAndAnswer; }
        }
        private bool requiresQuestionAndAnswer;

        /// <summary>
        /// A Boolean value specified in the configuration file (Web.config).
        /// The RequiresUniqueEmail property indicates whether users must supply a unique e-mail address value when creating a user. If a user already exists in the data source for the current ApplicationName , the MembershipProvider..::.CreateUser method returns null and a status value of DuplicateEmail .
        /// This property is read-only.
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return requiresUniqueEmail; }
        }
        private bool requiresUniqueEmail;
        #endregion

        #region Required MembershipProvider Methods
        /// <summary>
        /// Takes, as input, a user name, a current password, and a new password, and updates the password in the data source if the supplied user name and current password are valid. The  ChangePassword   method returns true if the password was updated successfully; otherwise, false.
        /// The ChangePassword method raises the ValidatingPassword event, if a MembershipValidatePasswordEventHandler has been specified, and continues or cancels the change-password action based on the results of the event. You can use the OnValidatingPassword virtual method to execute the specified MembershipValidatePasswordEventHandler .
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Argument cannot be null or empty", "usernameToMatch");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Argument cannot be null or empty", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Argument cannot be null or empty", "newPassword");

            // NOTE: How to change the password http://www.qualitydata.com/products/aspnet-membership/help/configuration/asp-net-sql-membership-password-administration.aspx
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, a user name, a password, a password question, and a password answer, and updates the password question and answer in the data source if the supplied user name and password are valid. The  ChangePasswordQuestionAndAnswer   method returns true if the password question and answer are updated successfully; otherwise, false.
        /// If the supplied user name and password are not valid, false is returned.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Argument cannot be null or empty", "usernameToMatch");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Argument cannot be null or empty", "password");
            if (String.IsNullOrEmpty(newPasswordQuestion)) throw new ArgumentException("Argument cannot be null or empty", "newPasswordQuestion");
            if (String.IsNullOrEmpty(newPasswordAnswer)) throw new ArgumentException("Argument cannot be null or empty", "newPasswordAnswer");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, the name of a new user, a password, and an e-mail address and inserts a new user for the application into the data source. The MembershipProvider..::.CreateUser   method returns a  MembershipUser   object that is populated with the information for the newly created user. The  CreateUser   method also defines an out parameter (in Visual Basic, you can use ByRef) that returns a  MembershipCreateStatus   value that indicates whether the user was successfully created, or a reason that the user was not successfully created.
        /// The CreateUser method raises the ValidatingPassword event if a MembershipValidatePasswordEventHandler has been specified, and continues or cancels the create-user action based on the results of the event. You can use the OnValidatingPassword virtual method to execute the specified MembershipValidatePasswordEventHandler .
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Argument cannot be null or empty", "usernameToMatch");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Argument cannot be null or empty", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Argument cannot be null or empty", "email");
            if (String.IsNullOrEmpty(passwordQuestion) && this.enablePasswordRetrieval) throw new ArgumentException("Argument cannot be null or empty", "passwordQuestion");
            if (String.IsNullOrEmpty(passwordAnswer) && this.enablePasswordRetrieval) throw new ArgumentException("Argument cannot be null or empty", "passwordAnswer");
            //TODO: hmadrigal. Ask where are we going to apply the validations for password, and related tasks.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, the name of a user and deletes that user's information from the data source. The  DeleteUser   method returns true if the user was successfully deleted; otherwise, false. An additional Boolean parameter is included to indicate whether related information for the user, such as role or profile information is also deleted.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Argument cannot be null or empty", "usernameToMatch");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of membership users where the user name contains a match of the supplied emailToMatch for the configured  ApplicationName  . For example, if the emailToMatch parameter is set to "address@example.com," then users with the e-mail addresses "address1@example.com," "address2@example.com," and so on are returned. Wildcard support is included based on the data source. Users are returned in alphabetical order by user name.
        /// The results returned by FindUsersByEmail are constrained by the pageIndex and pageSize parameters. The pageSize parameter identifies the number of MembershipUser objects to return in the MembershipUserCollection collection. The pageIndex parameter identifies which page of results to return, where 1 identifies the first page. The totalRecords parameter is an out parameter that is set to the total number of membership users that matched the emailToMatch value. For example, if 13 users were found where emailToMatch matched part of or the entire user name, and the pageIndex value was 2 with a pageSize of 5, then the MembershipUserCollection would contain the sixth through the tenth users returned. totalRecords would be set to 13.
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            if (String.IsNullOrEmpty(emailToMatch)) throw new ArgumentException("Argument cannot be null or empty", "emailToMatch");
            if (pageIndex < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageIndex");
            if (pageSize < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageSize");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a list of membership users where the user name contains a match of the supplied usernameToMatch for the configured  ApplicationName  . For example, if the usernameToMatch parameter is set to "user," then the users "user1," "user2," "user3," and so on are returned. Wildcard support is included based on the data source. Users are returned in alphabetical order by user name.
        /// The results returned by FindUsersByName are constrained by the pageIndex and pageSize parameters. The pageSize parameter identifies the number of MembershipUser objects to return in the MembershipUserCollection . The pageIndex parameter identifies which page of results to return, where 1 identifies the first page. The totalRecords parameter is an out parameter that is set to the total number of membership users that matched the usernameToMatch value. For example, if 13 users were found where usernameToMatch matched part of or the entire user name, and the pageIndex value was 2 with a pageSize of 5, then the MembershipUserCollection would contain the sixth through the tenth users returned. totalRecords would be set to 13.
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            if (String.IsNullOrEmpty(usernameToMatch)) throw new ArgumentException("Argument cannot be null or empty", "usernameToMatch");
            if (pageIndex < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageIndex");
            if (pageSize < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageSize");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a  MembershipUserCollection   populated with  MembershipUser   objects for all of the users in the data source.
        /// The results returned by GetAllUsers are constrained by the pageIndex and pageSize parameters. The pageSize parameter identifies the maximum number of MembershipUser objects to return in the MembershipUserCollection . The pageIndex parameter identifies which page of results to return, where 0 identifies the first page. The totalRecords parameter is an out parameter that is set to the total number of membership users. For example, if 13 users were in the database for the application, and the pageIndex value was 1 with a pageSize of 5, the MembershipUserCollection returned would contain the sixth through the tenth users returned. totalRecords would be set to 13.
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            if (pageIndex < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageIndex");
            if (pageSize < 0) throw new ArgumentException("Argument cannot be lower than 0", "pageSize");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an integer value that is the count of all the users in the data source where the LastActivityDate is greater than the current date and time minus the UserIsOnlineTimeWindow property. The UserIsOnlineTimeWindow property is an integer value specifying the number of minutes to use when determining whether a user is online.
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            //TODO: hmadrigal. If we want to implement this we might need a new column
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, a user name and a password answer and retrieves the password for that user from the data source and returns the password as a string.
        /// GetPassword ensures that the EnablePasswordRetrieval property is set to true before performing any action. If the EnablePasswordRetrieval property is false, an ProviderException is thrown.
        /// The GetPassword method also checks the value of the RequiresQuestionAndAnswer property. If the RequiresQuestionAndAnswer property is true, the GetPassword method checks the value of the supplied answer parameter against the stored password answer in the data source. If they do not match, a MembershipPasswordException is thrown.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            if (String.IsNullOrEmpty(username)) throw new ArgumentException("Argument cannot be null or empty", "username");
            if (String.IsNullOrEmpty(answer)) throw new ArgumentException("Argument cannot be null or empty", "answer");
            if (this.passwordFormat == MembershipPasswordFormat.Hashed) throw new InvalidOperationException("The Password format is set to Hashed. See PasswordFormat for mode information.");

            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, a unique user identifier and a Boolean value indicating whether to update the  LastActivityDate   value for the user to show that the user is currently online. The  GetUser   method returns a  MembershipUser  object populated with current values from the data source for the specified user. If the user name is not found in the data source, the  GetUser   method returns null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, a user name and a Boolean value indicating whether to update the  LastActivityDate   value for the user to show that the user is currently online. The  GetUser   method returns a  MembershipUser  object populated with current values from the data source for the specified user. If the user name is not found in the data source, the  GetUser   method returns null (Nothing in Visual Basic).
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, an e-mail address and returns the first user name from the data source where the e-mail address matches the supplied email parameter value.
        /// If no user name is found with a matching e-mail address, an empty string is returned.
        /// If multiple user names are found that match a particular e-mail address, only the first user name found is returned. 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes, as input, a user name and a password answer and generates a new, random password for the specified user. The  ResetPassword   method updates the user information in the data source with the new password value and returns the new password as a string. A convenient mechanism for generating a random password is the  GeneratePassword   method of the  Membership   class.
        /// The ResetPassword method ensures that the EnablePasswordReset property is set to true before performing any action. If the EnablePasswordReset property is false, a NotSupportedException is thrown. The ResetPassword method also checks the value of the RequiresQuestionAndAnswer property. If the RequiresQuestionAndAnswer property is true, the ResetPassword method checks the value of the supplied answer parameter against the stored password answer in the data source. If they do not match, a MembershipPasswordException is thrown.
        /// The ResetPassword method raises the ValidatingPassword event, if a MembershipValidatePasswordEventHandler has been specified, to validate the newly generated password and continues or cancels the reset-password action based on the results of the event. You can use the OnValidatingPassword virtual method to execute the specified MembershipValidatePasswordEventHandler .
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            //NOTE: See how to reset password based on encription http://www.qualitydata.com/products/aspnet-membership/help/configuration/asp-net-sql-membership-password-administration.aspx
            
        }

        /// <summary>
        /// Takes, as input, a user name, and updates the field in the data source that stores the  IsLockedOut   property to false. The  UnlockUser   method returns true if the record for the membership user is updated successfully; otherwise false.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            if (String.IsNullOrEmpty(userName)) return false;
            throw new NotImplementedException(String.Format("{0} is not going to be implemented at this time.", "UnlockUser"));
        }

        /// <summary>
        /// Takes, as input, a  MembershipUser   object populated with user information and updates the data source with the supplied values.
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(MembershipUser user)
        {
            if (user == null) throw new ArgumentException(@"Argument cannot be null", "user");
            throw new NotImplementedException(String.Format("{0} is not going to be implemented at this time.", "UpdateUser"));
        }

        /// <summary>
        /// Takes, as input, a user name and a password and verifies that the values match those in the data source. The  ValidateUser   method returns true for a successful user name and password match; otherwise, false.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return false;
            var isValidUser = this.SiteRepository.ValidateUser(username, password);
            return isValidUser;
        }
        #endregion
    }
}