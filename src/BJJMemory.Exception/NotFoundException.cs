using BJJMemory.Exception.ExceptionsBase;
using System.Net;

namespace BJJMemory.Exception.ExceptionsBase;

public class NotFoundException : BJJMemoryException
{
    public NotFoundException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
