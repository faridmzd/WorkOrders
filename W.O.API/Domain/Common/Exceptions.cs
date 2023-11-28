using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace W.O.API.Domain.Common.Exceptions
{
    public class CustomArgumentException : ArgumentException
    {
        public CustomArgumentException() : base() { }
        public CustomArgumentException(string? message) : base(message) { }
        public CustomArgumentException(string? message, Exception? innerException) : base(message, innerException) { }
        public CustomArgumentException(string? message, string? paramName) : base(message, paramName) { }
        public CustomArgumentException(string? message, string? paramName, Exception? innerException) : base(message, paramName, innerException) { }

        public static void ThrowIfDefault([NotNull] object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);

            var type = argument.GetType();
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;

            if (argument == defaultValue)
            {
                throw new ArgumentException("Default value is not allowed", paramName);
            }
        }
    }
    public class InvalidVisitException : Exception
    {
        public InvalidVisitException(string message) : base(message)
        {

        }

        public static void ThrowIfInvalidNumberOfParts(List<Part> entity, int maxExpectedAmount)
        {
            if (entity is null || entity.Count == 0) throw new InvalidVisitException(message: "At least one part is needed in order to create a visit!");

            else if (entity.Count > maxExpectedAmount) throw new InvalidVisitException(message: $"Max {maxExpectedAmount} part(s) are allowed for each visit!");
        }
    }

    public class InvalidWorkOrderException : Exception
    {
        public InvalidWorkOrderException(string message) : base(message)
        {

        }

        public static void ThrowIfInvalidNumberOfVisits(List<Visit>? entity, int maxExpectedAmount)
        {
            if (entity is null || entity.Count == 0) return;
            
            if (entity.Count > maxExpectedAmount) throw new InvalidWorkOrderException(message: $"Max {maxExpectedAmount} visit(s) are allowed for each work order!");

        }
    }


}
