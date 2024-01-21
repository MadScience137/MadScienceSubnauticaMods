using HarmonyLib;
using JetBrains.Annotations;
using MadScienceSubnauticaMods.LegsBeGone.Data;
using MadScienceSubnauticaMods.LegsBeGone.Extensions;
using UnityEngine;

#pragma warning disable CS0618

namespace MadScienceSubnauticaMods.LegsBeGone.patches
{
    public class BuilderToolPatch
    {
        [HarmonyPatch(typeof(BuilderTool))]
        [HarmonyPatch(nameof(BuilderTool.HandleInput))]
        internal class HandleInput
        {
            [HarmonyPostfix]
            public static void Postfix(BuilderTool __instance)
            {
                if (Player.main.IsInBase()) return;

                var targetCell = GetToggleTarget();

                if (targetCell == null) return;

                SetInteractionText();

                if (!IsToggleKeyPressed()) return;

                TogglePiece(targetCell);
            }

            [CanBeNull]
            private static BaseCell GetToggleTarget()
            {
                Targeting.GetTarget(Player.main.gameObject, 30f, out var target, out _);

                if (target == null) return null;

                var cellObject = target.GetComponentInParent<BaseCell>();
                var baseObject = target.GetComponentInParent<Base>();

                if (cellObject == null || baseObject == null) return null;

                var cell = baseObject.WorldToGrid(cellObject.transform.position);
                var cellType = baseObject.GetCell(cell);

                var isToggleable = cellType == Base.CellType.Corridor || cellType == Base.CellType.Room || cellType == Base.CellType.Foundation ||
                                   cellType == Base.CellType.Moonpool;

                return isToggleable ? cellObject : null;
            }

            private static void TogglePiece(Component cellObject)
            {
                var gameObject = cellObject.gameObject;
                var baseObject = gameObject.GetComponentInParent<Base>();
                var position = gameObject.transform.position;
                var gridPosition = baseObject.WorldToGrid(position);

                var active = LegDataProvider.TogglePiece(position);

                var foundations = baseObject.GetCellObject(gridPosition).GetComponentsInChildren<BaseFoundationPiece>();

                foreach (var f in foundations)
                {
                    if (active)
                    {
                        foreach (var pillar in f.pillars)
                        {
                            pillar.root.SetActive(false);
                            var adjustable = pillar.adjustable;

                            if (!adjustable) continue;
                            var floorDistance = BaseUtils.GetFloorDistance(adjustable.position, adjustable.forward, f.maxPillarHeight, baseObject.gameObject);

                            if (floorDistance <= -1f) continue;
                            var num = floorDistance + 0.01f + f.extraHeight;

                            if (num < f.minHeight) continue;
                            adjustable.localScale = new Vector3(1f, 1f, num);

                            if (pillar.bottom)
                            {
                                pillar.bottom.position = adjustable.position + adjustable.forward * num;
                            }

                            pillar.root.SetActive(true);
                        }
                    }
                    else
                    {
                        f.pillars.ForEach(p => p.root.SetActive(false));
                    }
                }
            }

            private static bool IsToggleKeyPressed()
            {
                var pressedInput = GameInput.GetPressedInput(GameInput.Device.Keyboard);
                GameInput.scanningInput = false;

                return string.Equals(pressedInput, KeyCode.T.ToString());
            }

            private static void SetInteractionText()
            {
                var toggleLegsMessage = $"\nToggle compartment legs (<color=#ADF8FFFF>{KeyCode.T.ToString()}</color>)";
                HandReticle.main.AppendTextRaw(HandReticle.TextType.HandSubscript, toggleLegsMessage);
            }
        }
    }
}