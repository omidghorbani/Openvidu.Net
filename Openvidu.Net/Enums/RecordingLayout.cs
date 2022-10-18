namespace Openvidu.Net.Enums
{
    public enum RecordingLayout : byte
    {
        /// <summary>
        /// All the videos are evenly distributed, taking up as much space as possible
        /// </summary>
        BEST_FIT,
        /// <summary>
        /// Use your own custom recording layout.
        /// </summary>
        CUSTOM,
        /// <summary>
        /// (not available yet)
        /// </summary>
        HORIZONTAL_PRESENTATION,
        /// <summary>
        /// (not available yet)
        /// </summary>
        PICTURE_IN_PICTURE,
        /// <summary>
        /// (not available yet)
        /// </summary>
        VERTICAL_PRESENTATION	
    }
}