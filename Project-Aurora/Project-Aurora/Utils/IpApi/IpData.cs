using Newtonsoft.Json;

namespace AuroraRgb.Utils.IpApi;

public class IpData
{
    public double Lat { get; }
    public double Lon { get; }

    public string Status { get; }
    public string? Message { get; }

    [JsonConstructor]
    public IpData(double lat, double lon, string status, string? message)
    {
        Lat = lat;
        Lon = lon;
        Status = status;
        Message = message;
    }
}