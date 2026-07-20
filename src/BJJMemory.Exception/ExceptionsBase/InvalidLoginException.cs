using BJJMemory.Exception;
using System.Net;

namespace BJJMemory.Exception.ExceptionsBase;

public class InvalidLoginException : BJJMemoryException
{
    public InvalidLoginException() : base(ResourceErrorMessages.INVALID_LOGIN)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return new List<string> { Message };
    }
}
