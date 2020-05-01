using Aurora.EffectsEngine;
using Aurora.EffectsEngine.Animations;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace Aurora.Profiles.Desktop
{
    public class Event_Desktop : LightEvent
    {
        public new bool IsEnabled => true;
    }
}
