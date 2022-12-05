using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MadScienceSubnauticaMods.LegsBeGone.Data;
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

                var target = GetToggleTarget();

                if (target == null) return;

                SetInteractionText(__instance, target);

                if (!IsToggleKeyPressed()) return;

                TogglePiece(target);
            }

            private static GameObject GetToggleTarget()
            {
                bool Filter(RaycastHit hit)
                {
                    if (hit.collider == null) return false;

                    var cellObject = hit.collider.GetComponentInParent<BaseCell>();
                    var baseObject = hit.collider.GetComponentInParent<Base>();

                    if (cellObject == null || baseObject == null) return false;

                    var cell = baseObject.WorldToGrid(cellObject.transform.position);
                    var cellType = baseObject.GetCell(cell);

                    return cellType == Base.CellType.Corridor || cellType == Base.CellType.Room || cellType == Base.CellType.Foundation || cellType == Base.CellType.Moonpool;
                }

                Targeting.AddToIgnoreList(Player.main.gameObject);
                Targeting.GetTarget(30f, out var gameObject, out _, Filter);
                return gameObject;
            }

            private static void TogglePiece(GameObject gameObject)
            {
                var cell = gameObject.GetComponentInParent<BaseCell>();
                var baseObject = gameObject.GetComponentInParent<Base>();
                var position = cell.transform.position;
                var gridPosition = baseObject.WorldToGrid(position);

                var active = LegDataProvider.TogglePiece(position);

                var foundations = baseObject.GetCellObject(gridPosition).GetComponentsInChildren<BaseFoundationPiece>();

                foreach (var f in foundations)
                {
                    if (active)
                    {
                        f.OnGenerate();
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

                return string.Equals(pressedInput, QMod.Config.ToggleKey.ToString());
            }

            private static void SetInteractionText(BuilderTool __instance, GameObject gameObject)
            {
                var toggleLegsMessage = $"Toggle compartment legs (<color=#ADF8FFFF>{QMod.Config.ToggleKey.ToString()}</color>)";

                var deconstructText = new StringBuilder();
                var deconstructable = gameObject.GetComponentInParent<BaseDeconstructable>();

                if (deconstructable != null && deconstructable.DeconstructionAllowed(out _))
                {
                    var primaryText = deconstructable.Name == "None" ? Language.main.Get(TechType.BaseRoom) : deconstructable.Name;
                    var secondaryText = $"{__instance.deconstructText}\n{toggleLegsMessage}";
                    HandReticle.main.SetInteractInfo(primaryText, secondaryText);
                    return;
                }

                var constructable = gameObject.GetComponentInParent<Constructable>();

                if (constructable == null)
                {
                    var primaryText = deconstructable == null || deconstructable.Name == "None" ? Language.main.Get(TechType.BaseRoom) : deconstructable.Name;
                    HandReticle.main.SetInteractInfo(primaryText, toggleLegsMessage);
                    return;
                }

                if (constructable.constructed)
                {
                    deconstructText.AppendLine(__instance.deconstructText);
                    deconstructText.Append(toggleLegsMessage);
                    HandReticle.main.SetInteractText(Language.main.Get(constructable.techType), deconstructText.ToString(), false, false, false);
                }
                else
                {
                    deconstructText.AppendLine(__instance.constructText);

                    foreach (KeyValuePair<TechType, int> keyValuePair in constructable.GetRemainingResources())
                    {
                        var key = keyValuePair.Key;
                        var text = Language.main.Get(key);
                        var value = keyValuePair.Value;
                        deconstructText.AppendLine(value > 1 ? Language.main.GetFormat("RequireMultipleFormat", text, value) : text);
                    }

                    deconstructText.Append(toggleLegsMessage);
                    var techType = constructable.techType == TechType.None ? TechType.BaseRoom : constructable.techType;
                    HandReticle.main.SetInteractText(Language.main.Get(techType), deconstructText.ToString(), false, false, false);
                }
            }
        }
    }
}