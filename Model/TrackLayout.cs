using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class TrackLayout
    {
        private TrackLayout() { } // For deserialization.

        public TrackLayout(string name)
        {
            Name = name;
            Stations = new List<Station>();
            TrackStretches = new List<TrackStretch>();
            TimetableStretches = new List<TimetableStretch>();
        }

        public string Name { get; }
        public ICollection<Station> Stations { get; }
        public ICollection<TrackStretch> TrackStretches { get; }
        public ICollection<TimetableStretch> TimetableStretches { get; }

        public bool HasExit(StationExit exit) => StationExits.Any(e => e.Equals(exit));
        public bool HasStation(Station station) => Stations.Any(s => s.Equals(station));
        public bool HasTimetableStretch(string number) => TimetableStretches.Any(t => t.Number == number);
        public bool HasTrack(StationTrack track) => StationTracks.Any(t => t.Equals(track));

        public IEnumerable<StationExit> StationExits => Stations.SelectMany(s => s.Exits);
        public IEnumerable<StationTrack> StationTracks => Stations.SelectMany(s => s.Tracks);

        public Maybe<Station> Station(string nameOrSignature) => Maybe<Station>.NoneIfNull(
            Stations.SingleOrDefault(s => s.Name == nameOrSignature || s.Signature == nameOrSignature),
            $"There is no station with signature or name '{nameOrSignature}'");

        public StationTrack StationTrack(StationTrack value) => StationTracks.Single(t => t.Equals(value));
        public TimetableStretch TimetableStretch(string number) => TimetableStretches.Single(t => t.Number == number);

        public Maybe<TrackStretch> TrackStretch(Station from, Station to)
        {
            var stretches = TrackStretches.Where(ts => (ts.Start.Station.Equals(from) && ts.End.Station.Equals(to)) ||
                (ts.Start.Station.Equals(to) && ts.End.Station.Equals(from)));
            if (stretches.Count() == 1) return Maybe<TrackStretch>.Item(stretches.First());
            if (!stretches.Any()) return Maybe<TrackStretch>.None(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ThereIsNoStretchBetweenStation1AndStation2, from, to));
            return Maybe<TrackStretch>.None(string.Format(CultureInfo.CurrentCulture, Resources.Strings.MoreThanOneStretchBetweenStations, from, to));
        }

        public Maybe<TrackStretch> TrackStretch(string fromStationNameOrSignature, string toStationNameOrSignature)
        {
            var from = Station(fromStationNameOrSignature);
            if (from.IsNone) return Maybe<TrackStretch>.None($"There is no from-station with signature or name '{fromStationNameOrSignature}'.");
            var to = Station(toStationNameOrSignature);
            if (to.IsNone) return Maybe<TrackStretch>.None($"There is no to-station with signature or name '{fromStationNameOrSignature}'.");
            return TrackStretch(from.Value, to.Value);
        }

        public void Add(Station station)
        {
            if (HasStation(station))
                throw new TrackLayoutException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.DuplicateAddOfStation, station));
            station.Layout = this;
                Stations.Add(station);
            }

        public TrackStretch Add(StationExit from, StationExit to, double distance)
        {
            return Add(from, to, distance, 1);
        }

        public TrackStretch Add(StationExit from, StationExit to, double distance, int tracksCount)
        {
            if (from is null) throw new ArgumentNullException(nameof(from));
            if (to is null) throw new ArgumentNullException(nameof(to));
            if (!HasExit(from)) throw new TrackLayoutException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.StationExitIsNotInLayout, from));
            if (!HasExit(to)) throw new TrackLayoutException(string.Format(CultureInfo.CurrentCulture, Resources.Strings.StationExitIsNotInLayout, to));
            return Add(new TrackStretch(from, to, distance, tracksCount));
        }

        public TrackStretch Add(TrackStretch stretch)
        {
            if (stretch == null) throw new ArgumentNullException(nameof(stretch));
            if (TrackStretches.Contains(stretch)) return stretch;
            TrackStretches.Add(stretch);
            return TrackStretches.Last();
        }

        public void Add(TimetableStretch timetableStretch)
        {
            if (timetableStretch == null) throw new ArgumentNullException(nameof(timetableStretch));
            if (TimetableStretches.Contains(timetableStretch)) return;
            TimetableStretches.Add(timetableStretch);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
