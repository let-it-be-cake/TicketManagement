using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic.Proxys
{
    internal class SeatValidator : IValidate<Seat>
    {
        private readonly string _recordAlreadyContainsMessage = "The Seat record with this AreaId, Row, Number fields already exists in the database.";
        private readonly string _wrongNumberParameter = "The seat number cannot be less than zero";
        private readonly string _wrongRowParameter = "The seat row cannot be less than zero";

        private readonly IEnumerable<Seat> _seats;

        public SeatValidator(IEnumerable<Seat> seats)
        {
            _seats = seats;
        }

        /// <inheritdoc cref="IValidate{T}"/>
        public bool IsValid(Seat item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (item.Number < 0)
            {
                throw new ArgumentOutOfRangeException(_wrongNumberParameter);
            }

            if (item.Row < 0)
            {
                throw new ArgumentOutOfRangeException(_wrongRowParameter);
            }

            if (_seats.Any(o =>
                o.AreaId == item.AreaId &&
                o.Number == item.Number &&
                o.Row == item.Row))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
