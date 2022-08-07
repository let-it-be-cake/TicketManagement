using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic.Proxys
{
    internal class VenueValidator : IValidate<Venue>
    {
        private readonly string _recordAlreadyContainsMessage = "The Venue record with this Address, Phone, Description fields already exists in the database.";
        private readonly IEnumerable<Venue> _venues;

        public VenueValidator(IEnumerable<Venue> venues)
        {
            _venues = venues;
        }

        /// <inheritdoc cref="IValidate{T}"/>
        public bool IsValid(Venue item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_venues.Any(o =>
                o.Address == item.Address &&
                o.Phone == item.Phone &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
