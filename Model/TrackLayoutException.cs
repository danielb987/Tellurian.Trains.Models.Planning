namespace Tellurian.Trains.Models.Planning
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [ExcludeFromCodeCoverage]
    [Serializable]
    public class TrackLayoutException : Exception
    {
        public TrackLayoutException() { }
        public TrackLayoutException(string message) : base(message) { }
        public TrackLayoutException(string message, Exception innerException) : base(message, innerException) { }
        protected TrackLayoutException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
