using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nautilus.Utility;
using Newtonsoft.Json;
using UnityEngine;

namespace MadScienceSubnauticaMods.LegsBeGone.Data
{
    public static class LegDataProvider
    {
        private static List<Vector3> disabledLegs = new List<Vector3>();
        private const float Slack = 0.1f;

        public static bool TogglePiece(Vector3 cell)
        {
            if (HasLegs(cell))
            {
                AddPiece(cell);
                return false;
            }

            RemovePiece(cell);
            return true;
        }

        private static void AddPiece(Vector3 cell)
        {
            disabledLegs.Add(new Vector3(cell.x, cell.y, cell.z));
        }

        private static void RemovePiece(Vector3 cell)
        {
            disabledLegs.RemoveAll(disabledCell => VectorSloppyEquals(disabledCell, cell));
        }

        public static bool HasLegs(Vector3 cell)
        {
            return disabledLegs.All(disabledCell => !VectorSloppyEquals(disabledCell, cell));
        }

        private static bool VectorSloppyEquals(Vector3 v1, Vector3 v2)
        {
            return Vector3.Distance(v1, v2) < Slack;
        }

        public static void Load()
        {
            var path = GetFilePath();

            if (File.Exists(path))
            {
                var data = JsonConvert.DeserializeObject<Vector3[]>(File.ReadAllText(path));
                disabledLegs = data == null ? disabledLegs : data.ToList();
            }

            LegsBeGonePluginSN.logger.LogInfo($"Loaded {disabledLegs.Count} pieces of leg data.");
        }

        public static void Save()
        {
            var json = JsonConvert.SerializeObject(disabledLegs.ToArray(),
                Formatting.None,
                new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }
            );
            File.WriteAllText(GetFilePath(), json);
            LegsBeGonePluginSN.logger.LogInfo($"Saved {disabledLegs.Count} pieces of leg data.");
        }

        private static string GetFilePath()
        {
            return $"{SaveUtils.GetCurrentSaveDataDir()}/LegsBeGoneData.json";
        }
    }
}