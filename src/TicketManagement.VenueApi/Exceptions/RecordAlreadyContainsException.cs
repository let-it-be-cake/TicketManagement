using System;
using System.Runtime.Serialization;

namespace TicketManagement.VenueApi.Exceptions
{
    [Serializable]
    public class RecordAlreadyContainsException : Exception
    {
        public RecordAlreadyContainsException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordAlreadyContainsException"/> class with a specified error parameters message.
        /// </summary>
        /// <param name="exceptionMessage">The name of the class that caused the error.</param>
        public RecordAlreadyContainsException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        protected RecordAlreadyContainsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
