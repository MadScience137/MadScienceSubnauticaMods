using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MadScienceSubnauticaMods.LegsBeGone.Data
{
    public static class LegDataProvider
    {
        private const float Slack = 0.1f;

        private static List<Vector3> disabledLegs = new List<Vector3>();
        public static int Count => disabledLegs.Count;

        public static void LoadFromArray(Vector3[] a)
        {
            if (a == null) return;
            disabledLegs = a.ToList();
        }

        public static Vector3[] ToArray() => disabledLegs.ToArray();

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
    }
}