using System;
using System.Runtime.Serialization;

namespace ThirdPartyEventEditor.Exceptions
{
    [Serializable]
    public class FileNullException : Exception
    {
        public FileNullException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileNullException"/> class with a specified error parameters message.
        /// </summary>
        /// <param name="exceptionMessage">The name of the class that caused the error.</param>
        public FileNullException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        protected FileNullException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
