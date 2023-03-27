using FluentValidation.Results;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ordering.Application.Exceptions
{
    [Serializable]
    public class ValidationException : ApplicationException
    {
        public ValidationException() : base("One or more validation errors ocurred")
        {
        }

        public ValidationException(IEnumerable<ValidationFailure> failures) : this()
        {
            Errors = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(g => g.Key, g => g.ToArray());
        }

        public ValidationException(string? message) : base(message)
        {
        }

        public ValidationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Dictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();
    }
}
