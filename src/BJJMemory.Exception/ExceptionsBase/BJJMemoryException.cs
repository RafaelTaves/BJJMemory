namespace BJJMemory.Exception.ExceptionsBase;

public abstract class BJJMemoryException : SystemException
{
    protected BJJMemoryException(string message) : base(message)
    {

    }

    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
