using System;
using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public class Timetable
    {
        private Timetable() { } // For deserialization;

        public Timetable(string name, TrackLayout layout)
        {
            Name = name;
            Layout = layout;
            Trains = new List<Train>();
        }

        public string Name { get; }
        public TrackLayout Layout { get; }
        public ICollection<Train> Trains { get; }

        public IEnumerable<Station> Stations => Layout.Stations;
        public Maybe<Train> Train(string name) => Maybe<Train>.ItemIfOne(Trains.Where(t => t.ExtenalId == name), $"Train {name} not found.");
        public int StartHour => Trains.Select(t => t.Calls.Min(c => c.Arrival.Value)).Min(tt => tt).Hours;
        public int EndHour => Trains.Select(t => t.Calls.Max(c => c.Arrival.Value)).Max(tt => tt).Hours + 1;

        public void AddTrain(Train train)
        {
            if (train == null) throw new ArgumentNullException(nameof(train));
            if (Trains.Contains(train)) return;
            Trains.Add(train);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
