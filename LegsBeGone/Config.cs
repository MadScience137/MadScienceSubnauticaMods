using SMLHelper.V2.Json;
using SMLHelper.V2.Options.Attributes;
using UnityEngine;

namespace MadScienceSubnauticaMods.LegsBeGone
{
    [Menu("Legs Be Gone")]
    public class Config : ConfigFile
    {
        [Keybind("Legs toggle key")]
        public KeyCode ToggleKey = KeyCode.T;
    }
}