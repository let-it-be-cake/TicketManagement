using System;
using System.Runtime.Serialization;

namespace TicketManagement.UserApi.Exceptions
{
    [Serializable]
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class with a specified error parameters message.
        /// </summary>
        /// <param name="exceptionMessage">The name of the class that caused the error.</param>
        public UserNotFoundException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        protected UserNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
