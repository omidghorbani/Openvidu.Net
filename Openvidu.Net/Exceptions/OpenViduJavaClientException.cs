using System;

namespace Openvidu.Net.Exceptions
{
    public class OpenViduClientException : OpenViduException
    {
        public OpenViduClientException(string message) : base(message)
        {
            
        }
        public OpenViduClientException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}