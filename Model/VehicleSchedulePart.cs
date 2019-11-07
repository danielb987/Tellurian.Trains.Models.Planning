namespace Tellurian.Trains.Models.Planning
{
    using System;
    using System.Globalization;
    using System.Linq;

    public class VehicleSchedulePart
    {
        public VehicleSchedulePart(VehicleSchedule locoSchedule, int fromPartIndex, int toPartIndex)
        {
            if (locoSchedule == null) throw new ArgumentNullException(nameof(locoSchedule));
            if (fromPartIndex < 0 || fromPartIndex > (locoSchedule.Parts.Count) - 1) throw new ArgumentOutOfRangeException(nameof(fromPartIndex));
            if (toPartIndex < fromPartIndex || toPartIndex > (locoSchedule.Parts.Count) - 1) throw new ArgumentOutOfRangeException(nameof(toPartIndex));
            var parts = locoSchedule.Parts.ToArray();
            From = (parts[fromPartIndex], fromPartIndex);
            To = (parts[toPartIndex], toPartIndex);
        }

        public (TrainPart Part, int Index) From { get; private set; }
        public (TrainPart Part, int Index) To { get; private set; }

        public Train FirstTrain => From.Part.Train;
        public StationCall Departure => From.Part.From;
        public Train LastTrain => To.Part.Train;
        public StationCall Arrival => To.Part.To;

        public override bool Equals(object obj)
        {
            if (!(obj is VehicleSchedulePart other)) return false;
            return From == other.From && To == other.To;
        }

        public override int GetHashCode()
        {
            return From.GetHashCode() ^ To.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}:{1} - {2}:{3}", FirstTrain, Departure, LastTrain, Arrival);
        }
    }
}
