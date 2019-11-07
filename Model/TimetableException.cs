namespace Tellurian.Trains.Models.Planning
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class TimetableException : Exception
    {
        public TimetableException() { }
        public TimetableException(string message) : base(message) { }
        public TimetableException(string message, Exception innerException) : base(message, innerException) { }
        protected TimetableException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
