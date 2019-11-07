using System;
using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Trains.Models.Planning.Tests
{
    public sealed class TestRepository : ILayoutRepository, ITimetableReadStore, IScheduleRepository
    {
        public (Maybe<TrackLayout> item, IEnumerable<Message> messages) GetLayout(string name)
        {
            return (Maybe<TrackLayout>.Item(GetTestLayout(name)), Array.Empty<Message>());
        }

        public IEnumerable<Message> Save(TrackLayout layout)
        {
            throw new NotSupportedException();
        }

        private static TrackLayout GetTestLayout(string name)
        {
            var layout = new TrackLayout(name);
            var stations = TestDataFactory.Stations.ToArray();
            foreach (var station in stations)
            {
                layout.Add(station);
            }
            layout.Add(stations[0].Exit("Vänster"), stations[1].Exit("Höger"), 10);
            layout.Add(stations[2].Exit("Höger"), stations[1].Exit("Vänster"), 10);
            return layout;
        }

        (Maybe<Timetable> item, IEnumerable<Message> messages) ITimetableReadStore.GetTimetable(string name)
        {
            return GetTestTimetable(name);
        }

        private (Maybe<Timetable> item, IEnumerable<Message> messages) GetTestTimetable(string name)
        {
            var (item, messages) = GetLayout(name);
            if (item.IsNone) return (Maybe<Timetable>.None("Layout does not exist."), messages);
            var result = new Timetable(name, item.Value);
            result.AddTrain(TestDataFactory.CreateTrain1());
            result.AddTrain(TestDataFactory.CreateTrain2());
            return (Maybe<Timetable>.Item(result), messages);
        }

        public (Maybe<Schedule> item, IEnumerable<Message> messages) GetSchedule(string name)
        {
            throw new NotSupportedException();
        }

        public IEnumerable<Message> Save(Schedule schedule)
        {
            throw new NotSupportedException();
        }
    }
}
