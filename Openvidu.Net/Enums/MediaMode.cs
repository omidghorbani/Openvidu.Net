namespace Openvidu.Net.Enums
{
    public enum MediaMode : byte
    {
        /// <summary>
        /// (not available yet) The session will attempt to transmit streams directly between clients
        /// </summary>
        RELAYED,

        /// <summary>
        /// The session will transmit streams using OpenVidu Media Node
        /// </summary>
        ROUTED
    }
}