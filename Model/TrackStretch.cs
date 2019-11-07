using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class TrackStretch
    {
        private TrackStretch() { } // For deserialization.

        public TrackStretch(StationExit start, StationExit end, double distance) : this(start, end, distance, 1) { }

        public TrackStretch(StationExit start, StationExit end, double distance, int tracksCount)
        {
            Start = start;
            End = end;
            Start.Stretch = this;
            End.Stretch = this;
            Distance = distance;
            TracksCount = tracksCount;
        }

        public double Distance { get; }
        public int TracksCount { get; }
        public StationExit Start { get; }
        public StationExit End { get; }

        public IEnumerable<StretchPassing> Passings
        {
            get
            {
                var result = new List<StretchPassing>();

                foreach (var from in Start.Station.Calls)
                {
                    var to = End.Station.Calls.FirstOrDefault(c => c.Train == from.Train && c.Departure > from.Departure);
                    if (to != null) result.Add(new StretchPassing(from, to, true));
                }
                foreach (var from in End.Station.Calls)
                {
                    var to = Start.Station.Calls.FirstOrDefault(c => c.Train == from.Train && c.Departure > from.Departure);
                    if (to != null) result.Add(new StretchPassing(from, to, false));
                }
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.StretchToString, Start, End);
        }
    }
}
