using System;
using System.Collections.Generic;
using System.Linq;

namespace Tellurian.Trains
{
    public struct Maybe<T> where T : class
    {
        public static Maybe<T> Item(T value) => new Maybe<T>(value);
        public static Maybe<T> None(string message) => new Maybe<T>(message);

        public static Maybe<T> NoneIfNull(T value, string errorMessage) =>
            value is null ? None(errorMessage) : Item(value);

        public static Maybe<T> NoneIfError(T value, bool hasErrors) =>
            hasErrors ?
            None("One or several errors occured that preventet the operations to complete.") :
            NoneIfNull(value, "One or several errors occured that preventet the operations to complete.");

        public static Maybe<T> ItemIfOne(IEnumerable<T> values, string noneMessage) =>
            values.Count() == 1 ?
            Maybe<T>.Item(values.First()) :
            Maybe<T>.None(noneMessage);

        private Maybe(T value) { _Value = value; Message = string.Empty; }
        private Maybe(string message) { _Value = null; Message = message; }

        private readonly T _Value;

        public T Value
        {
            get
            {
                if (_Value is null) throw new InvalidOperationException("Value is null");
                return _Value;
            }
        }

        public bool HasValue => _Value != null;
        public bool IsNone => !HasValue;
        public string Message { get; }
    }
}