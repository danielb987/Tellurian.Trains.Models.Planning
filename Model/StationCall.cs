using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Tellurian.Trains.Models.Planning
{
    public class StationCall
    {
        private StationCall() { } // For deserialization.
        public StationCall(StationTrack track, Time arrival, Time departure) : this(track, arrival, departure, true) { }

        public StationCall(StationTrack track, Time arrival, Time departure, bool isStop)
        {
            Track = track ?? throw new ArgumentNullException(nameof(track));
            Track.Add(this);
            Arrival = arrival ?? throw new ArgumentNullException(nameof(arrival), string.Format(CultureInfo.CurrentCulture, "At {0}", track.Station));
            Departure = departure ?? throw new ArgumentNullException(nameof(departure), string.Format(CultureInfo.CurrentCulture, "At {0}", track.Station));
            IsStop = isStop;
            Notes = new List<StationCallNote>();
        }

        public int SequenceNumber { get; internal set; }
        public Train Train { get; internal set; }
        public Time Arrival { get; }
        public Time Departure { get; }
        public StationTrack Track { get; }
        public bool IsArrival { get; private set; }
        public bool IsDeparture { get; private set; }
        public IList<StationCallNote> Notes { get; }

        public Station Station { get { return Track.Station; } }
        public bool IsStop { get { return IsArrival || IsDeparture; } set { IsArrival = value; IsDeparture = value; } }

        public override bool Equals(object obj)
        {
            if (!(obj is StationCall other)) return false;
            return
                Arrival == other.Arrival &&
                Departure == other.Departure &&
                Track == other.Track &&
                Train == other.Train &&
                Station == other.Station;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Train.GetHashCode() ^ Track.GetHashCode() ^ Station.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.Strings.CallAtStationTrackDuringTimes, Station, Track, Arrival, Departure);
        }
    }
}
