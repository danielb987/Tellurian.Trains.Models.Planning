using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tellurian.Trains.Models.Planning
{
    public sealed class Message : IEquatable<Message>
    {
        public static Message Information(string text) => new Message(text, Severity.Information);
        public static Message Warning(string text) => new Message(text, Severity.Warning);
        public static Message Warning(CultureInfo culture, string format, params object[] args) => new Message(string.Format(culture, format, args), Severity.Warning);
        public static Message Error(string text) => new Message(text, Severity.Error);
        public static Message Error(CultureInfo culture, string format, params object[] args) => new Message(string.Format(culture, format, args), Severity.Error);
        public static Message System(string text) => new Message(text, Severity.System);

        private Message(string text, Severity severity) { Text = text; Severity = severity; }

        public string Text { get; }
        public Severity Severity { get; }

        public override string ToString()
        {
            return $"{Severity.ToLanguageString(CultureInfo.CurrentCulture)}: {Text}";
        }

        public bool Equals(Message other) => other.Severity == Severity && other.Text == Text;
        public override bool Equals(object obj) => (obj is Message message) && Equals(message);
        public override int GetHashCode() => Text.GetHashCode();
    }

    public enum Severity
    {
        Information = 0,
        Warning = 1,
        Error = 2,
        System = 3
    }

    public static class ErrorMessageExtensions
    {
        public static bool CanContinue(this IEnumerable<Message> me) => !me.Any() || me.Any(m => m.Severity < Severity.Error);
        public static bool HasStoppingErrors(this IEnumerable<Message> me) => me.Any(m => m.Severity >= Severity.Error);
        public static bool Contains(this IEnumerable<Message> me, string text) => me.Any(m => m.Text.Contains(text));
    }

    internal static class SeverityExtensions
    {
        public static string ToLanguageString(this Severity me, CultureInfo culture)
        {
            return Resources.Strings.ResourceManager.GetString(me.ToString(), culture);
        }
    }
}
