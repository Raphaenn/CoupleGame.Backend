// Application/Exceptions/NotFoundException.cs
public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

// Application/Exceptions/ValidationException.cs
public sealed class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(string message, IDictionary<string, string[]>? errors = null)
        : base(message)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }
}