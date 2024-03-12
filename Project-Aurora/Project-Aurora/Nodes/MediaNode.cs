using AuroraRgb.Modules.Media;

namespace AuroraRgb.Nodes;

public class MediaNode : Node
{
    public bool MediaPlaying => MediaMonitor.MediaPlaying;
    public bool HasMedia => MediaMonitor.HasMedia;
    public bool HasNextMedia => MediaMonitor.HasNextMedia;
    public bool HasPreviousMedia => MediaMonitor.HasPreviousMedia;
}