using System;

namespace Openvidu.Net.Exceptions
{
    public class OpenViduHttpException : OpenViduException
    {
        public OpenViduHttpException(string message) : base(message)
        { }
        public OpenViduHttpException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}