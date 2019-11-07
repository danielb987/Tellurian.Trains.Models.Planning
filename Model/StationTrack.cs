using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Tellurian.Trains.Models.Planning
{
    public class StationTrack
    {
        public static StationTrack Example { get { return new StationTrack("1"); } }

        private StationTrack() { } // Only for deserialization.
        public StationTrack(string number) : this(number, true, true) { }        public StationTrack(string number, bool isMain, bool isScheduled)
        {
            Number = number;
            IsMain = isMain;
            IsScheduled = isScheduled;
            Calls = new List<StationCall>();
        }

        public ICollection<StationCall> Calls { get; }
        public string Number { get; }
        public bool IsScheduled { get; }
        public bool IsMain { get; }
        public double Length { get; }
        public Station Station { get; internal set; }

        public override string ToString()
        {
            return Number;
        }

        internal void Add(StationCall call)
        {
            if (call == null) throw new ArgumentNullException(nameof(call));
            if (Calls.Contains(call)) return;
            Calls.Add(call);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StationTrack other)) return false;
            if (Station == null) return false;
            return Number == other.Number && Station.Equals(other.Station);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Number.GetHashCode();
        }
    }
}
