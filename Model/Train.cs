using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class Train
    {
        private Train() { } // For deserialization.
        private int lastSequenceNumber = 1;

        public Train(string number)
        {
            Number = !string.IsNullOrWhiteSpace(number) ? number : throw new ArgumentNullException(nameof(number));
            Calls = new List<StationCall>();
            ExtenalId = string.Empty;
        }

        public Train(string number, string externalId) : this(number)
        {
            ExtenalId = !string.IsNullOrWhiteSpace(externalId) ? externalId : throw new ArgumentNullException(nameof(externalId));
        }

        public string Category { get; set; } = string.Empty;
        public string Number { get; }
        public string ExtenalId { get; }
        public string Remarks { get; set; }
        public IList<StationCall> Calls { get; }

        public StationCall this[int index] => Calls[index];
        internal IEnumerable<StationTrack> Tracks => Calls.OrderBy(c => c.Arrival).Select(c => c.Track);
        public TrackLayout Layout => Calls[0].Station.Layout;

        public TrainPart AsTrainPart => new TrainPart(this, 0, Calls.Count - 1);

        public void Add(StationCall call)
        {
            if (call == null) throw new ArgumentNullException(nameof(call));
            if (Calls.Contains(call)) return;
            call.Train = this;
            call.SequenceNumber = lastSequenceNumber++;
            Calls.Add(call);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Train other)) return false;
            return Number == other.Number && ExtenalId == other.ExtenalId;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Number.GetHashCode() ^ ExtenalId.GetHashCode();
        }

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, "{0} {1}", Category, Number);
    }
}
