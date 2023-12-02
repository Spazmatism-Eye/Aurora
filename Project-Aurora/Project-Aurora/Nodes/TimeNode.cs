using Aurora.Utils;

namespace Aurora.Nodes;

public class TimeNode : Node
{
    public int CurrentMonth => Time.GetMonths();
    public int CurrentDay => Time.GetDays();
    public int CurrentHour => Time.GetHours();
    public int CurrentMinute => Time.GetMinutes();
    public int CurrentSecond => Time.GetSeconds();
    public int CurrentMillisecond => Time.GetMilliSeconds();
    public long MillisecondsSinceEpoch => Time.GetMillisecondsSinceEpoch();
}