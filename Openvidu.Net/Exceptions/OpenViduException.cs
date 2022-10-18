using System;

namespace Openvidu.Net.Exceptions
{
    public class OpenViduException : Exception
    {
        public OpenViduException(string message) : base(message)
        {

        }
        public OpenViduException(string message, Exception innerException): base(message,innerException)
        {
            
        }
    }
}