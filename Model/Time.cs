namespace Tellurian.Trains.Models.Planning
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    public sealed class Time : IComparable<Time>
    {
        private Time() { }  // For deserialization.

        private Time(TimeSpan value)
        {
            Value = value;
        }

        public Time(int hour, int minute)
        {
            Value = new TimeSpan(hour, minute, 0);
        }

        public Time(int day, int hour, int minute)
        {
            Value = new TimeSpan(day, hour, minute, 0);
        }

        private Time(double value)
        {
            var temp = TimeSpan.FromDays(value);
            Value = new TimeSpan(temp.Hours, temp.Minutes, 0);
        }

        public TimeSpan Value { get; private set; }

        public Time HourAndMinute => new Time(new TimeSpan(Value.Hours, Value.Minutes, 0));

        public static Time Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            var parts = value.Split(':');
            if (parts.Length == 1)
            {
                return new Time(double.Parse(value.Replace(',', '.'), CultureInfo.InvariantCulture) + 0.0001);
            }
            else
            {
                return new Time(int.Parse(parts[0], CultureInfo.InvariantCulture), int.Parse(parts[1], CultureInfo.InvariantCulture));
            }
        }

        public Time Add(int minutes)
        {
            var t = Value.Add(TimeSpan.FromMinutes(minutes));
            return AsTime(t);
        }

        public Time AddDays(int days)
        {
            var t = Value.Add(TimeSpan.FromDays(days));
            return AsTime(t);
        }

        public Time Diff(Time other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return new Time(Value - other.Value);
        }

        private static Time AsTime(TimeSpan value) => new Time(value.Days, value.Hours, value.Minutes);
        int IComparable<Time>.CompareTo(Time other) => CompareTo(other);
        public int CompareTo(Time other) => (int)(other.Value.TotalMinutes - Value.TotalMinutes);

        public static bool operator !=(Time time1, Time time2) => (IsNull(time1) || IsNull(time2)) ? false : time1.Value != time2.Value;
        public static bool operator ==(Time time1, Time time2) => (IsNull(time1) && IsNull(time2)) || ((IsNull(time1) || IsNull(time2)) ? false : time1.Value == time2.Value);
        public static bool operator <=(Time time1, Time time2) => (IsNull(time1) || IsNull(time2)) ? false : time1.Value <= time2.Value;
        public static bool operator >=(Time time1, Time time2) => (IsNull(time1) || IsNull(time2)) ? false : time1.Value >= time2.Value;
        public static bool operator <(Time time1, Time time2) => (IsNull(time1) || IsNull(time2)) ? false : time1.Value < time2.Value;
        public static bool operator >(Time time1, Time time2) => (IsNull(time1) || IsNull(time2)) ? false : time1.Value > time2.Value;

        public override bool Equals(object obj)
        {
            var other = obj as Time;
            if (IsNull(other)) return false;
            return Value == other.Value;
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:hh\\:mm}", Value);
        }

        private static bool IsNull(object value) { return value == null; }
    }
}
