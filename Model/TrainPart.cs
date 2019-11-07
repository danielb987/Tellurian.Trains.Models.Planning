using System;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class TrainPart
    {
        private TrainPart() { } // For deserialization.

        public TrainPart(Train train, int fromCallIndex, int toCallIndex)
        {
            if (train == null) throw new ArgumentNullException(nameof(train));
            if (fromCallIndex < 0 || fromCallIndex > (train.Calls.Count) - 2) throw new ArgumentOutOfRangeException(nameof(fromCallIndex));
            if (toCallIndex <= fromCallIndex || toCallIndex > (train.Calls.Count) - 1) throw new ArgumentOutOfRangeException(nameof(toCallIndex));
            var calls = train.Calls.ToArray();
            From = calls[fromCallIndex];
            To = calls[toCallIndex];
        }

        public StationCall From { get; }
        public StationCall To { get; }

        public Train Train => From.Train;

        public override bool Equals(object obj)
        {
            if (!(obj is TrainPart other)) return false;
            return From == other.From && To == other.To;
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() ^ To.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}:{1}->{2}", Train, From.Departure, To.Arrival);
        }
    }
}
