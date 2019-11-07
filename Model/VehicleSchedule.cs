using System;
using System.Collections.Generic;

namespace Tellurian.Trains.Models.Planning
{
    public sealed class VehicleSchedule
    {
        public static VehicleSchedule Loco(string identity) => new VehicleSchedule(identity);
        public static VehicleSchedule Trainset(string identity) => new VehicleSchedule(identity) { IsTrainset = true };

        private VehicleSchedule(string identity)
        {
            Identity = identity;
            Parts = new List<TrainPart>();
        }

        public string Identity { get; }
        public bool IsTrainset { get; private set; }
        public bool IsLoco => !IsTrainset;

        public ICollection<TrainPart> Parts { get; }

        public void Add(TrainPart part)
        {
            if (part == null) throw new ArgumentNullException(nameof(part));
            if (Parts.Contains(part)) return;
            Parts.Add(part);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is VehicleSchedule other)) return false;
            return Identity == other.Identity;
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public override string ToString()
        {
            return Identity;
        }
    }
}
