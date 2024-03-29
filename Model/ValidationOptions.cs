﻿namespace Tellurian.Trains.Models.Planning
{
    public class ValidationOptions
    {
        public bool ValidateStationCalls { get; set; } = true;
        public bool ValidateStationTracks { get; set; } = true;
        public bool ValidateStretches { get; set; } = true;
        public bool ValidateTrainSpeed { get; set; } = true;
        public bool ValitdateTrainNumbers { get; set; } = true;
        public bool ValidateLocoSchedules { get; set; } = true;
        public bool ValidateTrainsetSchedules { get; set; } = true;
        public bool ValidateDriverDuties { get; set; } = true;
        public double MinTrainSpeedMetersPerClockMinute { get; set; } = 0.5;
        public double MaxTrainSpeedMetersPerClockMinute { get; set; } = 10;
        public int MinMinutesBetweenTrackUsage { get; set; } = 0;
    }
}
