///////////////////////////////////////////////////////////////////////
//                         EmptyMembership                           //
//             Written by: Miron Abramson. Date: 04-10-07            //
//  An empty membership provider to use its 'DecryptPassword' method //
// when using of reflection is not allowded (as happen in            //
// share hostings such GoDaddy)                                      //
//                    Last updated: 05-05-2008                       //
///////////////////////////////////////////////////////////////////////

#region Using

using System;
using System.Web.Security;
using System.Web;
using System.Text;
using System.Security.Cryptography;

#endregion

namespace Miron.Web.MbCompression
{
    /// <summary>
    /// Membership provider without any supported method or properties.
    /// The only prupose of this class is to decrypt a string using the machine key, without using a reflection
    /// in our code.
    /// </summary>
    public class EmptyMembership : MembershipProvider
    {
        private static EmptyMembership emptyMembership;

        // Create singleton object of the settings
        internal static EmptyMembership Instance
        {
            get
            {
                if (emptyMembership == null)
                {
                    emptyMembership = new EmptyMembership();
                }
                return emptyMembership;
            }
        }

        /// <summary>
        /// Decript a string using the machine key
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string DecryptString(string input)
        {
            byte[] buf = HttpServerUtility.UrlTokenDecode(input);
            buf = DecryptPassword(buf);
            return Encoding.UTF8.GetString(buf);
        }

        #region Properties - Not supported


        public override string ApplicationName
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotSupportedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotSupportedException(); }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotSupportedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotSupportedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotSupportedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotSupportedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Clear; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotSupportedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotSupportedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region Methods - Not supported

        public override bool ValidateUser(string username, string password)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotSupportedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotSupportedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        #endregion

    }
}