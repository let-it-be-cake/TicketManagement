using System;
using System.Collections.Generic;
using System.Linq;
using TicketManagement.BusinessLogic.Exceptions;
using TicketManagement.BusinessLogic.Interfaces;
using TicketManagement.Entities.Tables;

namespace TicketManagement.BusinessLogic.Proxys
{
    internal class AreaValidator : IValidate<Area>
    {
        private readonly string _recordAlreadyContainsMessage = "The Area record with this LayoutId, CoordX, CoordY, Description fields already exists in the database.";
        private readonly IEnumerable<Area> _areas;

        public AreaValidator(IEnumerable<Area> areas)
        {
            _areas = areas;
        }

        /// <inheritdoc cref="IValidate{T}"/>
        public bool IsValid(Area item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Cannot be null");
            }

            if (_areas.Any(o =>
                o.LayoutId == item.LayoutId &&
                o.CoordX == item.CoordX &&
                o.CoordY == item.CoordY &&
                o.Description == item.Description))
            {
                throw new RecordAlreadyContainsException(_recordAlreadyContainsMessage);
            }

            return true;
        }
    }
}
