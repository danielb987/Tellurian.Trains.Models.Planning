using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class Station
    {
        public static Station Example { get { return new Station("Copenhagen", "Kh"); } }

        private Station() { } // For deserialization.

        public Station(string name, string signature)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), string.Format(CultureInfo.CurrentCulture, Resources.Strings.NameOfObjectIsRequired, Resources.Strings.Station.ToLowerInvariant()));
            Name = name.Replace("_", " ");
            if (string.IsNullOrWhiteSpace(signature)) throw new ArgumentNullException(nameof(signature), string.Format(CultureInfo.CurrentCulture, Resources.Strings.NameOfObjectIsRequired, Name));
            Signature = signature;
            Exits = new List<StationExit>();
            Tracks = new List<StationTrack>();
        }

        public string Name { get; }
        public string Signature { get; }
        public ICollection<StationExit> Exits { get; }
        public ICollection<StationTrack> Tracks { get; }
        public TrackLayout Layout { get; internal set; }

        public TrackStretch StretchTowards(Station other) => Exits.Where(e => e.Stretch.End.Station.Equals(other)).Select(e => e.Stretch).Single();
        public StationExit Exit(string name) => Exits.Single(e => e.Name == name);
        public IEnumerable<TrackStretch> Stretches => Exits.Select(e => e.Stretch);
        public bool HasTrack(string number) => Tracks.Any(t => t.Number == number);
        public StationTrack this[string number] => Tracks.SingleOrDefault(t => t.Number == number);
        public IEnumerable<StationCall> Calls => Tracks.SelectMany(t => t.Calls);
        public IEnumerable<Train> Trains => Calls.Select(c => c.Train).Distinct();

        public Maybe<StationTrack> Track(string number) =>
                Maybe<StationTrack>.NoneIfNull( Tracks.SingleOrDefault(t => t.Number == number), string.Format(CultureInfo.CurrentCulture, Resources.Strings.StationHasNotTrackNumber, Name, number));

        public void Add(StationExit exit)
        {
            if (exit == null) throw new ArgumentNullException(nameof(exit));
            exit.Station = this;
            Exits.Add(exit);
        }

        public void Add(StationTrack track)
        {
            if (track == null) throw new ArgumentNullException(nameof(track));
            track.Station = this;
            Tracks.Add(track);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Station other)) return false;
            return Signature == other.Signature;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Signature.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
