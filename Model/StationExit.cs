using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Tellurian.Trains.Models.Planning
{
    public class StationExit
    {
        public static StationExit Example => new StationExit("Uppspår Ytterby");

        private StationExit() { } // For deserialization.

        public StationExit(string name)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public StationExit(string name, Station station) : this(name)
        {
            Station = station ?? throw new ArgumentNullException(nameof(station));
        }

        public StationExit(string name, TrackStretch stretch) : this (name)
        {
            Stretch = stretch ?? throw new ArgumentNullException(nameof(stretch));
        }

        public string Name { get; }
        public Station Station { get; internal set; }
        public TrackStretch Stretch { get; internal set; }

        public override bool Equals(object obj)
        {
            if (!(obj is StationExit other)) return false;
            if (Station == null) return false;
            return Name == other.Name && Station == other.Station;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}->{1}", Station, Name);
        }
    }
}
