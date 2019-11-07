namespace Tellurian.Trains.Models.Planning
{
    using System.Collections.Generic;

    public interface ITimetableReadStore
    {
        (Maybe<Timetable> item, IEnumerable<Message> messages) GetTimetable(string name);
    }

    public interface ITimetableSaveStore
    {
        IEnumerable<Message> Save(Timetable timetable);
    }
}
