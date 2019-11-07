using System.Collections.Generic;

namespace Tellurian.Trains.Models.Planning
{
    public interface IScheduleRepository: IScheduleReadStore, IScheduleWriteStore {  }

    public interface IScheduleReadStore
    {
        (Maybe<Schedule> item, IEnumerable<Message> messages) GetSchedule(string name);
    }

    public interface IScheduleWriteStore
    {
        IEnumerable<Message> Save(Schedule schedule);
    }
}
