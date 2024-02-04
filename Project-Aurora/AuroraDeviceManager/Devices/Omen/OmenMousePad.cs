using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Common;
using Common.Devices;

namespace AuroraDeviceManager.Devices.Omen
{
    public class OmenMousePad : IOmenDevice
    {
        private const string OmenLightingSdkDll = "x64\\OmenLightingSDK.dll";
        private IntPtr hMousePad = IntPtr.Zero;

        ConcurrentDictionary<int, SimpleColor> cachedColors = new();

        private OmenMousePad(IntPtr hMousePad)
        {
            this.hMousePad = hMousePad;
        }

        public static IEnumerable<OmenMousePad> GetOmenMousePads()
        {
            var ptr = OmenLighting_MousePad_OpenByName("Outpost");
            if (ptr != IntPtr.Zero)
                yield return new OmenMousePad(ptr);
        }

        public void Shutdown()
        {
            try
            {
                Monitor.Enter(this);
                OmenLighting_MousePad_Close(hMousePad);
                hMousePad = IntPtr.Zero;
            }
            catch (Exception exc)
            {
                Global.Logger.Error("OMEN MousePad, Exception during Shutdown. Message: " + exc);
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        private int GetZone(DeviceKeys key)
        {
            return (key == DeviceKeys.MOUSEPADLIGHT15 ? (int)MousePadZone.MOUSE_PAD_ZONE_LOGO : (int)MousePadZone.MOUSE_PAD_ZONE_0 + ((int)key - (int)DeviceKeys.MOUSEPADLIGHT1));
        }

        public void SetLights(Dictionary<DeviceKeys, SimpleColor> keyColors)
        {
            if (hMousePad != IntPtr.Zero)
            {
                foreach (var keyColor in keyColors)
                {
                    if (keyColor.Key >= DeviceKeys.MOUSEPADLIGHT1 && keyColor.Key <= DeviceKeys.MOUSEPADLIGHT15)
                    {
                        SetLight(keyColor.Key, keyColor.Value);
                    }
                }
            }
        }

        private void SetLight(DeviceKeys key, SimpleColor color)
        {
            if (hMousePad != IntPtr.Zero)
            {
                var zone = GetZone(key);
                cachedColors.AddOrUpdate(zone, color, (_, oldValue) => color);

                Task.Run(() =>
                {
                    if (Monitor.TryEnter(this))
                    {
                        try
                        {
                            foreach (var item in cachedColors)
                            {
                                var c = LightingColor.FromColor(item.Value);
                                var res = OmenLighting_MousePad_SetStatic(hMousePad, item.Key, c, IntPtr.Zero);
                                if (res != 0)
                                {
                                    Global.Logger.Error("OMEN MousePad, Set static effect fail: " + res);
                                }

                                cachedColors.TryRemove(item.Key, out _);
                            }
                        }
                        finally
                        {
                            Monitor.Exit(this);
                        }
                    }
                });
            }
        }

        public string GetDeviceName()
        {
            return (hMousePad != IntPtr.Zero ? "Mouse pad Connected" : string.Empty);
        }

        public enum MousePadZone
        {
            MOUSE_PAD_ZONE_ALL = 0,                                 /* All zone */
            MOUSE_PAD_ZONE_LOGO,                                    /* Logo zone */
            MOUSE_PAD_ZONE_PAD,                                     /* Logo zone */
            MOUSE_PAD_ZONE_LEFT,                                    /* Left edge zone */
            MOUSE_PAD_ZONE_BOTTOM,                                  /* Left bottom zone */
            MOUSE_PAD_ZONE_RIGHT,                                   /* Left right zone */
            MOUSE_PAD_ZONE_0,                                       /* Zone 0 */
        }

        [DllImport(OmenLightingSdkDll)]
        static extern IntPtr OmenLighting_MousePad_Open();

        [DllImport(OmenLightingSdkDll)]
        static extern IntPtr OmenLighting_MousePad_OpenByName([MarshalAs(UnmanagedType.LPWStr)] string deviceName);

        [DllImport(OmenLightingSdkDll)]
        static extern void OmenLighting_MousePad_Close(IntPtr hMousePad);

        [DllImport(OmenLightingSdkDll)]
        static extern int OmenLighting_MousePad_SetStatic(IntPtr hMousePad, int zone, LightingColor color, IntPtr property);
    }
}
