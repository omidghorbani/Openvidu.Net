namespace Openvidu.Net.Enums
{
    public enum RecordingOutputMode : byte
    {
        /// <summary>
        /// Record all streams in a grid layout in a single archive
        /// </summary>
        COMPOSED,
        /// <summary>
        /// Works the same way as COMPOSED mode, but the necessary recorder service module will start some time in advance and won't be terminated once a specific session recording has ended.
        /// </summary>
        COMPOSED_QUICK_START,
        /// <summary>
        /// Record each stream individually
        /// </summary>
        INDIVIDUAL,
    }
}