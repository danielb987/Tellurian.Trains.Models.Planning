using System;
using System.Globalization;

namespace Tellurian.Trains.Models.Planning
{
    public class StretchPassing
    {
        private StretchPassing() { } //For deserialzation.

        public StretchPassing(StationCall from, StationCall to, bool isOdd)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (!from.Train.Equals(to.Train)) throw new TimetableException($"From and to station call must be on the same train: from={from}, to={to}");
            Train = from.Train;
            From = from;
            To = to;
            IsOdd = isOdd;
        }

        public StationCall From { get; }
        public StationCall To { get; }
        public bool IsOdd { get; }
        public Train Train { get; }

        public Time Arrival { get { return To.Arrival; } }
        public Time Departure { get { return From.Departure; } }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}: {1} - {2}: {3}", From.Station.Name, Departure, To.Station.Name, Arrival);
        }
    }
}
