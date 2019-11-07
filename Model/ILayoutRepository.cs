using System.Collections.Generic;

namespace Tellurian.Trains.Models.Planning
{
    public interface ILayoutRepository : ILayoutReadStore, ILayoutWriteStore { }

    public interface ILayoutReadStore
    {
        (Maybe<TrackLayout> item, IEnumerable<Message> messages) GetLayout(string name);
    }

    public interface ILayoutWriteStore
    {
        IEnumerable<Message> Save(TrackLayout layout);
    }
}
