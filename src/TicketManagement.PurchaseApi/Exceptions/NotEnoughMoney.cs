using System;
using System.Runtime.Serialization;

namespace TicketManagement.PurchaseApi.Exceptions
{
    [Serializable]
    public class NotEnoughMoney : Exception
    {
        public NotEnoughMoney()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotEnoughMoney"/> class with a specified error parameters message.
        /// </summary>
        /// <param name="exceptionMessage">The name of the class that caused the error.</param>
        public NotEnoughMoney(string exceptionMessage)
            : base(exceptionMessage)
        {
        }

        protected NotEnoughMoney(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
