namespace Openvidu.Net.Enums
{
    public enum RecordingMode : byte
    {
        /// <summary>
        /// The session is recorded automatically as soon as the first client publishes a stream to the session.
        /// </summary>
        ALWAYS,
        /// <summary>
        /// The session is not recorded automatically.
        /// </summary>
        MANUAL
    }
}