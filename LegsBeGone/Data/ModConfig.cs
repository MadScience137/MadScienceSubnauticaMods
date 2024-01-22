using Nautilus.Json;
using Nautilus.Options.Attributes;
using UnityEngine;

namespace MadScienceSubnauticaMods.LegsBeGone.Data
{
    [Menu("Legs Be Gone")]
    public class ModConfig : ConfigFile
    {
        [Keybind("Toggle Key")]
        public KeyCode toggleKey = KeyCode.T;
    }
}