namespace Openvidu.Net.Enums
{
    public enum OpenViduRole : byte
    {
        /// <summary>
        /// SUBSCRIBER + PUBLISHER permissions + can force the unpublishing or disconnection over a third-party Stream or Connection (call Session.forceUnpublish() and Session.forceDisconnect())
        /// </summary>
        MODERATOR,
        /// <summary>
        /// SUBSCRIBER permissions + can publish their own Streams (call Session.publish())
        /// </summary>
        PUBLISHER,
        /// <summary>
        /// Can subscribe to published Streams of other users
        /// </summary>
        SUBSCRIBER
    }
}