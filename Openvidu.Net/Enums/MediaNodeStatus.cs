namespace Openvidu.Net.Enums;

public enum MediaNodeStatus : byte
{

    // the Media Node is launching. This is the entry status and can also be reached from canceled status.
    launching,
    // the Media Node will immediately enter terminating status after the launching process succeeds. This status can be reached from launching status.
    canceled,
    // the Media Node failed to launch. This status can be reached from launching status.
    failed,
    // the Media Node is up and running. New sessions can now be established in this Media Node. This status can be reached from launching and waiting-idle-to-terminate statuses.
    running,
    // the Media Node is waiting until the last of its sessions is closed. Once this happens, it will automatically enter terminating status. The Media Node won't accept new sessions during this status. This status can be reached from running status.
    waiting_idle_to_terminate,
    // the Media Node is shutting down. This status can be reached from running, waiting-idle-to-terminate and canceled statuses.
    terminating,
    // the Media Node is shut down. This status can be reached from terminating status. For On Premises OpenVidu Pro clusters, this status means that you can safely shut down the Media Node instance.
    terminated
}