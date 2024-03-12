using System.Threading.Tasks;
using AuroraRgb.Utils;
using Lombok.NET;

namespace AuroraRgb.Modules;

public sealed partial class PointerUpdateModule : AuroraModule
{
    protected override async Task Initialize()
    {
        if (!Global.Configuration.GetPointerUpdates) return;
        Global.logger.Information("Fetching latest pointers");
        await PointerUpdateUtils.FetchDevPointers("master");
    }

    [Async]
    public override void Dispose()
    {
        //noop
    }
}