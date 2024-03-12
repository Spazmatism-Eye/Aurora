using System;
using System.Management;

namespace AuroraRgb.Modules.HardwareMonitor;

public static class CpuBrandFinder
{
    public static bool IsAmd()
    {
        try
        {
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");

            foreach (var o in searcher.Get())
            {
                var queryObj = (ManagementObject)o;
                var manufacturer = queryObj["Manufacturer"].ToString();
                var caption = queryObj["Caption"].ToString();

                Console.WriteLine($"Manufacturer: {manufacturer}");
                Console.WriteLine($"Caption: {caption}");

                // Check if the manufacturer contains "Intel" or "AMD"
                if (manufacturer.Contains("Intel"))
                {
                    return false;
                }

                if (manufacturer.Contains("AMD"))
                {
                    return true;
                }
            }

            return false;
        }
        catch (ManagementException e)
        {
        }
        return false;
    }
}