using System;
using System.Runtime.Serialization;

namespace ThirdPartyEventEditor.Exceptions
{
    [Serializable]
    public class ExtentionFileException : Exception
    {
        public ExtentionFileException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtentionFileException"/> class with a specified error parameters message.
        /// </summary>
        /// <param name="exceptionMessage">The name of the class that caused the error.</param>
        public ExtentionFileException(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        protected ExtentionFileException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
