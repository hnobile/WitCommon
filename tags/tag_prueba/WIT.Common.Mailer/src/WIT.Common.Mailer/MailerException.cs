using System.Runtime.Serialization;
using System;

namespace WIT.Common.Mailer
{
    /// <summary>
    /// Exception generated due to a mailing error.
    /// </summary>
    public class MailerException : Exception
    {
        #region Constructors

        /// <summary>
        /// Non-parameterized constructor.
        /// </summary>
        public MailerException()
            : base()
        {

        }

        /// <summary>
        /// Constructor parameterized with a message.
        /// </summary>
        /// <param name="message">Message for creating the exception.</param>
        public MailerException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor parameterized with the serialization information and the streaming context.
        /// </summary>
        /// <param name="info">Serialization information for creating the exception.</param>
        /// <param name="context">Streaming context for creating the exception.</param>
        public MailerException(SerializationInfo info, StreamingContext context)
            : base(info,context)
        {

        }

        /// <summary>
        /// Constructor parameterized with a message.
        /// </summary>
        /// <param name="message">Message for creating the exception.</param>
        /// <param name="innerException">Inner Exception for creating the exception.</param>
        public MailerException(string message, Exception innerException)
            : base(message,innerException)
        {

        }

        #endregion Constructors
    }
}
