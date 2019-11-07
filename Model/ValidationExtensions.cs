using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public static class ValidationExtensions
    {
        public static IEnumerable<Message> GetValidationErrors(this Schedule me, ValidationOptions options)
        {
            var result = new List<Message>();
            result.AddRange(me.Timetable.GetValidationErrors(me, options));
            if (options.ValidateLocoSchedules) result.AddRange(me.LocoSchedules.SelectMany(l => l.ValidateOverlappingParts()));
            return result;
        }

        public static IEnumerable<Message> GetValidationErrors(this Timetable me, Schedule schedule, ValidationOptions options)
        {
            var result = new List<Message>();
            result.AddRange(me.EnsureStationHasTrack());
            result.AddRange(me.Trains.SelectMany(t => t.CheckTrainTimeSequence()));
            if (options.ValidateStationTracks) result.AddRange(me.Stations.SelectMany(s => s.Tracks).SelectMany(t => t.GetValidationErrors(schedule.LocoSchedules)));
            if (options.ValidateStationCalls) result.AddRange(me.Stations.SelectMany(s => s.Calls).SelectMany(c => c.GetValidationErrors()));
            if (options.ValidateStretches) result.AddRange(me.Stations.SelectMany(s => s.Stretches).SelectMany(ss => ss.GetValidationErrors()).Distinct());
            if (options.ValidateTrainSpeed) result.AddRange(me.CheckTrainSpeed(options.MinTrainSpeedMetersPerClockMinute, options.MaxTrainSpeedMetersPerClockMinute));
            return result;
        }

        #region Station
        internal static IEnumerable<Message> EnsureStationHasTrack(this Timetable me)
        {
            var result = new List<Message>();
            foreach (var train in me.Trains)
            {
                foreach (var track in train.Tracks)
                {
                    var t = track;
                    if (!me.Layout.HasTrack(t))
                        result.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TrackInStationReferredInTrainIsNotInLayout, track, track.Station, train)));
                }
            }
            return result;
        }

        #endregion

        #region StationTrack
        internal static IEnumerable<Message> GetValidationErrors(this StationTrack me, IEnumerable<VehicleSchedule> locos)
        {
            return me.GetConflicts(locos).Select(c => Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.CallAtStationHasConflictsWithOtherCall, c.one.Train, c.one, c.another.Train, c.another)));
        }

        private static IEnumerable<(StationCall one, StationCall another)> GetConflicts(this StationTrack me, IEnumerable<VehicleSchedule> locos)
        {
            if (me.Calls.Count < 2) return Array.Empty<(StationCall, StationCall)>();
            var result = GetConflicts(me.Calls.First(), me.Calls.Skip(1), locos);
            return result.Distinct();
        }

        private static IEnumerable<(StationCall one, StationCall other)> GetConflicts(this StationCall me, IEnumerable<StationCall> remaining, IEnumerable<VehicleSchedule> locos)
        {
            var result = new List<(StationCall, StationCall)>();
            var conflictingWithMe = remaining.Where(r => r.Track.Equals(me.Track) && !r.Train.Equals(me.Train) && r.Arrival > me.Departure && r.Departure < me.Arrival && !locos.HasSameLoco(r, me)).ToList();
            result.AddRange(conflictingWithMe.Select(c => (me, c)));
            if (remaining.Count() > 1) result.AddRange(GetConflicts(remaining.First(), remaining.Skip(1), locos));
            return result;
        }

        internal static (bool, IEnumerable<StationCall>) GetConflicts(this StationTrack me, StationCall call, IEnumerable<StationCall> withCalls, IEnumerable<VehicleSchedule> locos)
        {
            if (me.Calls.Count == 0) return (false, null);
            var conflictingCalls = withCalls
                .Where(c => !locos.HasSameLoco(call, c) && (
                    (call.Departure > c.Arrival && call.Departure <= c.Departure) ||
                    (call.Arrival >= c.Arrival && call.Arrival < c.Departure)));
            if (conflictingCalls.Any())
                return (true, conflictingCalls);
            return (false, null);
        }
        #endregion

        #region StationCall
        public static IEnumerable<Message> GetValidationErrors(this StationCall me)
        {
            var result = new List<Message>();
            if (me.Arrival > me.Departure)
                result.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.ArrivalIsAfterDeparture, me.Track.Station.Name, me.Arrival, me.Departure)));
            return result;
        }
        #endregion

        #region TrackStretch
        internal static IEnumerable<Message> GetValidationErrors(this TrackStretch me)
        {
            var result = new List<Message>();
            var passings = me.Passings.OrderBy(p => p.Departure).ToArray();
            for (var i = 0; i < passings.Length - me.TracksCount; i++)
            {
                var first = passings[i];
                var second = passings[i + me.TracksCount];
                if (second.To.Arrival > first.From.Departure)
                    result.Add(Message.Warning(CultureInfo.CurrentCulture, $"Train {first.Train} between {first} is in conflict with {second.Train} between {second}."));
            }
            return result;
        }

        #endregion

        #region Train
        public static IEnumerable<Message> GetValidationErrors(this Train me, ValidationOptions options)
        {
            var result = new List<Message>();
            result.AddRange(me.CheckTrainSpeed(options.MinTrainSpeedMetersPerClockMinute, options.MaxTrainSpeedMetersPerClockMinute));
            result.AddRange(me.CheckTrainTimeSequence());
            return result;
        }

        internal static IEnumerable<Message> CheckTrainSpeed(this Timetable me, double minTrainSpeedMetersPerClockMinute, double maxTrainSpeedMetersPerClockMinute)
        {
            var result = new List<Message>();
            foreach (var train in me.Trains)
            {
                result.AddRange(train.CheckTrainSpeed(minTrainSpeedMetersPerClockMinute, maxTrainSpeedMetersPerClockMinute));
            }
            return result;
        }

        private static IEnumerable<Message> CheckTrainSpeed(this Train me, double minTrainSpeedMetersPerClockMinute, double maxTrainSpeedMetersPerClockMinute)
        {
            var result = new List<Message>();
            var calls = me.Calls.ToArray();
            for (var i = 0; i < calls.Length - 2; i++)
            {
                var c1 = calls[i];
                var c2 = calls[i + 1];
                var maybeStretch = me.Layout.TrackStretch(c1.Station, c2.Station);
                if (maybeStretch.HasValue)
                {
                    var time = c2.Arrival.Value - c1.Departure.Value;
                    var length = maybeStretch.Value.Distance;
                    var speed = time.TotalMinutes == 0 ? 0 : length / time.TotalMinutes;
                    if (speed == 0) continue;
                    if (speed < minTrainSpeedMetersPerClockMinute)
                        result.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TrainSpeedBetweenCallsIsTooSlow, c1.Train,c1.Station, c1.Departure, c2.Station, c2.Arrival, length)));
                    if (speed > maxTrainSpeedMetersPerClockMinute)
                        result.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TrainSpeedBetweenCallsIsTooFast, c1.Train,c1.Station, c1.Departure, c2.Station, c2.Arrival, length)));
                }
            }
            return result;
        }

        internal static IEnumerable<string> CheckTrainNumbers(this Timetable me)
        {
            var result = new List<string>();
            foreach (var train in me.Trains)
            {
                var calls = train.Calls.ToArray();
                for (var i = 0; i < calls.Length - 2; i++)
                {
                    var c1 = calls[i];
                    var c2 = calls[i + 1];
                }
            }
            return result;
        }

        internal static IEnumerable<Message> CheckTrainTimeSequence(this Train me)
        {
            var result = new List<Message>();
            if (me.Calls.Count < 1)
            {
                result.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TrainMustHaveMinimumTwoCalls, me)));
            }
            else
            {
                var conflicts = me.GetConflicts();
                result.AddRange(conflicts.Select(c => Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.TrainHasConflictingCalls, me, c.one, c.another))));
            }
            return result;
        }

        private static IEnumerable<(StationCall one, StationCall another)> GetConflicts(this Train me)
        {
            var result = new List<(StationCall, StationCall)>();
            var calls = me.Calls.ToArray();

            for (var i = 0; i < calls.Length - 1; i++)
            {
                var c1 = calls[i];
                var c2 = calls[i + 1];
                if (c2 != null)
                {
                    if (c1.Arrival > c2.Departure) result.Add((c1, c2));
                    //if (c1.Station.Equals(c2.Station)) result.Add((c1, c2));
                }
            }
            return result;
        }
        #endregion

        #region VehicleSchedule

        private static IEnumerable<Message> ValidateOverlappingParts(this VehicleSchedule me)
        {
            var messages = new List<Message>();
            var parts = me.Parts.ToArray();
            for (var i = 0; i < parts.Length - 1; i++)
            {
                for (var j = i + 1; j < parts.Length; j++)
                {
                    var p1 = parts[i];
                    var p2 = parts[j];
                    if (p1.To.Arrival > p2.From.Departure && p1.From.Departure < p2.To.Arrival) messages.Add(Message.Warning(string.Format(CultureInfo.CurrentCulture, Resources.Strings.VehicleScheduleContainsOverlappingTrainParts, me.Identity, p1, p2)));
                }
            }
            return messages;
        }
        #endregion

        internal static bool HasSameLoco(this IEnumerable<VehicleSchedule> me, StationCall one, StationCall another)
        {
            var foundOne = me.FindVehicleSchedule(one);
            var foundOther = me.FindVehicleSchedule(another);
            return !(foundOne is null) && !(foundOther is null) && foundOne == foundOther;
        }

        internal static VehicleSchedule FindVehicleSchedule(this IEnumerable<VehicleSchedule> me, StationCall call)
        {
            if (me == null) return null;
            foreach (var schedule in me.Where(s => s.IsLoco))
            {
                foreach (var part in schedule.Parts)
                {
                    if (part.ContainsCall(call)) return schedule;
                }
            }
            return null;
        }

        internal static bool ContainsCall(this TrainPart me, StationCall call)
        {
            return me.Train == call.Train && call.SequenceNumber >= me.From.SequenceNumber && call.SequenceNumber <= me.To.SequenceNumber;
        }
    }
}
