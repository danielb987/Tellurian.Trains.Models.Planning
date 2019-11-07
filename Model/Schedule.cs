using System;
using System.Collections.Generic;

namespace Tellurian.Trains.Models.Planning
{
    public class Schedule
    {
        private Schedule() { } // For deserialization.

        public Schedule(string name, Timetable timetable)
        {
            Name = name;
            Timetable = timetable;
            LocoSchedules = new List<VehicleSchedule>();
            TrainsetScedules = new List<VehicleSchedule>();
            DriverDuties = new List<DriverDuty>();
        }

        public string Name { get; }
        public Timetable Timetable { get; }
        public ICollection<VehicleSchedule> LocoSchedules { get; }
        public ICollection<VehicleSchedule> TrainsetScedules { get; }
        public ICollection<DriverDuty> DriverDuties { get; }

        public void AddLocoSchedule(VehicleSchedule schedule)
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));
            if (LocoSchedules.Contains(schedule)) throw new ArgumentOutOfRangeException(nameof(schedule), "Loco schedule alreade added.");
            LocoSchedules.Add(schedule);
        }

        public void AddTrainsetSchedule(VehicleSchedule schedule)
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));
            if (TrainsetScedules.Contains(schedule)) throw new ArgumentOutOfRangeException(nameof(schedule), "Trainset schedule alreade added.");
            TrainsetScedules.Add(schedule);
        }

        public void AddDriverDuty(DriverDuty duty)
        {
            if (duty == null) throw new ArgumentNullException(nameof(duty));
            if (DriverDuties.Contains(duty)) throw new ArgumentOutOfRangeException(nameof(duty), "Driver duty alreade added.");
            DriverDuties.Add(duty);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
