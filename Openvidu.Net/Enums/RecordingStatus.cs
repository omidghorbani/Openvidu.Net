namespace Openvidu.Net.Enums
{
    public enum RecordingStatus
    {
        /// <summary>
        /// The recording has failed.
        /// </summary>
        failed,
        /// <summary>
        /// The recording has finished being processed and is available for download through property Recording.getUrl()
        /// </summary>
        ready,
        /// <summary>
        /// The recording has started and is going on
        /// </summary>
        started,
        /// <summary>
        /// The recording is starting (cannot be stopped).
        /// </summary>
        starting,
        /// <summary>
        /// The recording has stopped and is being processed.
        /// </summary>
        stopped
    }
}