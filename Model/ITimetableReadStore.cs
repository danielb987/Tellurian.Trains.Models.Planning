using System.Collections.Generic;

namespace Tellurian.Trains.Models.Planning
{
    public interface ITimetableRepository: ITimetableReadStore, ITimetableWriteStore { }

    public interface ITimetableReadStore
    {
        (Maybe<Timetable> item, IEnumerable<Message> messages) GetTimetable(string name);
    }

    public interface ITimetableWriteStore
    {
        IEnumerable<Message> Save(Timetable timetable);
    }
}
