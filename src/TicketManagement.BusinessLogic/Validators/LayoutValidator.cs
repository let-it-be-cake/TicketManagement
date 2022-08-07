using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic.Proxys
{
    internal class LayoutValidator : IValidate<Layout>
    {
        private readonly string _recordAlreadyContainsMessage = "The Layout record with this VenueId, Description fields already exists in the database.";
        private readonly IEnumerable<Layout> _layouts;

        public LayoutValidator(IEnumerable<Layout> layouts)
        {
            _layouts = layouts;
        }

        /// <inheritdoc cref="IValidate{T}"/>
        public bool IsValid(Layout item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_layouts.Any(o =>
                o.VenueId == item.VenueId &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
