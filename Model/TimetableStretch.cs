using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class TimetableStretch
    {
        private TimetableStretch() { } // For deserialization.

        public TimetableStretch(string number)
        {
            Number = !string.IsNullOrWhiteSpace(number) ?  number : throw new ArgumentNullException(nameof(number), string.Format(CultureInfo.CurrentCulture, Resources.Strings.NumberOfObjectIsRequired, Resources.Strings.TimetableStretch));
            Stretches = new List<TrackStretch>();
        }

        public TimetableStretch(string number, string description) : this(number)
        {
            Description = description;
        }

        public string Number { get; }
        public string Description { get; }
        public IList<TrackStretch> Stretches { get; }

        public bool HasStation(Station station) => Stretches.Any(s => s.Start.Station.Equals(station) || s.End.Station.Equals(station));

        public IEnumerable<Station> Stations
        {
            get
            {
                var result = Stretches.Select(s => s.Start.Station).ToList();
                result.Add(Stretches.Last().End.Station);
                return result;
            }
        }

        public double DistanceToStation(Station station)
        {
            if (station == null) throw new ArgumentNullException(nameof(station));
            if (station.Equals(Stretches[0].Start.Station)) return 0;
            var stretch = Stretches.FirstOrDefault(s => s.End.Station.Equals(station));
            if (stretch == null) throw new ArgumentOutOfRangeException(nameof(station), string.Format(CultureInfo.CurrentCulture, Resources.Strings.TheStationNameIsNotPartOfTheStretch, station.Name));
            return Stretches.Take(Stretches.IndexOf(stretch) + 1).Sum(s => s.Distance);
        }

        public void Add(TrackStretch stretch)
        {
            if (stretch == null) throw new ArgumentNullException(nameof(stretch));
            if (Stretches.Count > 0 && stretch.End.Station.Equals(Stretches.Last().Start.Station))
            {
                Stretches.Insert(0, stretch);
            }
            else
            {
                Stretches.Add(stretch);
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TimetableStretch other)) return false;
            return Number == other.Number;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture,"{0}: {1} - {2}", Number, Stretches[0].Start.Station, Stretches.Last().End.Station);
        }
    }
}
