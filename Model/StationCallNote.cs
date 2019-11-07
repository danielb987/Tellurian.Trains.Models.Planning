using System;

namespace Tellurian.Trains.Models.Planning
{
    public class StationCallNote
    {
        public StationCallNote(string text)
        {
            Text = !string.IsNullOrWhiteSpace(text) ? text : throw new ArgumentNullException(nameof(text));
        }

        public string Text { get; }

        public bool IsDriverNote { get; set; }
        public bool IsStationNote { get; set; }
        public bool IsShuntingNote { get; set; }
    }
}
