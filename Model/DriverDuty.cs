using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class DriverDuty
    {
        private DriverDuty() { } // For deserialization.

        public DriverDuty(string identity)
        {
            Identity = identity;
            Parts = new List<VehicleSchedulePart>();
        }

        public string Identity { get; }
        public string Remarks { get; set; }
        public ICollection<VehicleSchedulePart> Parts { get; }

        public void Add(VehicleSchedulePart part)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            if (Parts.Contains(part)) return;
            Parts.Add(part);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DriverDuty other)) return false;
            return Identity == other.Identity;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}: {1} - {2}", Identity, Parts.Count > 0 ? Parts.First().Departure : null, Parts.Count > 0 ?  Parts.Last().Arrival : null);
        }
    }
}
