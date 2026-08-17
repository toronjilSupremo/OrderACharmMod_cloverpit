using System;
using BepInEx.Logging;
using HarmonyLib;
using Panik;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace OrderCharmMod;

public class Patches
{
    [HarmonyPatch(typeof(global::TerminalScript), "Update")]
    public static class TerminalScript_Update_Patch
    {
        private static void Postfix(global::TerminalScript __instance)
        {
            try
            {
                if (__instance == null)
                {
                    Core.Logger.LogError("[PATCH] __instance is null!");
                    return;
                }


                if (__instance.state != global::TerminalScript.State.navigation)
                {
                    if ((Object)(object)Core.OrderButton != (Object)null && Core.OrderButton.activeSelf)
                    {
                        Core.OrderButton.SetActive(false);
                        if ((Object)(object)Core.OrderTextObject != (Object)null)
                        {
                            Core.OrderTextObject.SetActive(false);
                        }
                    }
                    return;
                }

                if ((Object)(object)Core.OrderButton == (Object)null && (Object)(object)__instance.navigationButton_Buy != (Object)null)
                {
                    Core.OrderButton = new GameObject("OrderCharmButton");
                    Core.OrderButton.transform.SetParent(((Component)__instance.navigationButton_Buy).transform.parent, false);
                    RectTransform val = Core.OrderButton.AddComponent<RectTransform>();
                    RectTransform component = ((Component)__instance.navigationButton_Buy).GetComponent<RectTransform>();
                    val.anchorMin = component.anchorMin;
                    val.anchorMax = component.anchorMax;
                    val.pivot = component.pivot;
                    val.sizeDelta = component.sizeDelta;
                    ((Transform)val).localPosition = ((Component)__instance.navigationButton_Buy).transform.localPosition;
                    Image val2 = Core.OrderButton.AddComponent<Image>();
                    val2.sprite = __instance.navigationButton_Buy.ImageRenderer.sprite;
                    ((Graphic)val2).material = ((Graphic)__instance.navigationButton_Buy.ImageRenderer).material;
                    val2.type = (Image.Type)1;
                    Core.OrderTerminalButton = Core.OrderButton.AddComponent<global::TerminalButton>();
                    Core.OrderTerminalButton.ImageRenderer = val2;
                    Core.OrderButton.AddComponent<BoxCollider>().size = new Vector3(val.sizeDelta.x, val.sizeDelta.y, 0.01f);
                    Core.OrderTextObject = new GameObject("OrderButtonText");
                    Core.OrderTextObject.transform.SetParent(((Component)__instance.navigationButton_Buy).transform.parent, false);
                    Core.OrderCostText = Core.OrderTextObject.AddComponent<TextMeshProUGUI>();
                    ((TMP_Text)Core.OrderCostText).font = ((TMP_Text)__instance.inspector_TitleText).font;
                    ((TMP_Text)Core.OrderCostText).fontSize = 0.04f;
                    ((TMP_Text)Core.OrderCostText).alignment = (TextAlignmentOptions)513;
                    ((Graphic)Core.OrderCostText).color = new Color(1f, 0.5f, 0f, 1f);
                    ((TMP_Text)Core.OrderCostText).text = "";
                    RectTransform component2 = Core.OrderTextObject.GetComponent<RectTransform>();
                    component2.anchorMin = val.anchorMin;
                    component2.anchorMax = val.anchorMax;
                    component2.pivot = val.pivot;
                    component2.sizeDelta = new Vector2(1f, component.sizeDelta.y);
                    ((Transform)component2).localPosition = ((Transform)val).localPosition + new Vector3(0.4f, 0f, 0f);
                }
                global::PowerupScript val3 = global::TerminalScript.HoveredPowerupGet();
                if ((Object)(object)val3 == (Object)null)
                {
                    if ((Object)(object)Core.OrderButton != (Object)null)
                    {
                        Core.OrderButton.SetActive(false);
                    }
                    if ((Object)(object)Core.OrderTextObject != (Object)null)
                    {
                        Core.OrderTextObject.SetActive(false);
                    }
                    return;
                }

                bool queueFull = Core.OrderedCharms.Count >= Core.MaxOrders;
                bool alreadyOrdered = Core.OrderedCharms.Contains((int)val3.identifier);
                bool flag = true; // Se ignora IsPowerupBuyable: cualquier charm se puede pedir (incluidos especiales)
                bool flag2 = !global::PowerupScript.IsEquipped_Quick(val3.identifier) && !global::PowerupScript.IsInDrawer_Quick(val3.identifier) && !queueFull && flag;
                if ((Object)(object)Core.OrderButton != (Object)null && (Object)(object)Core.OrderTextObject != (Object)null)
                {
                    Core.OrderButton.SetActive(flag2);
                    Core.OrderTextObject.SetActive(flag2);
                }
                if (!flag2)
                {
                    return;
                }
                if (Core.OrderTerminalButton.IsMouseOnMe())
                {
                    Core.OrderTerminalButton.HoverColor();
                }
                ((TMP_Text)Core.OrderCostText).text = $"{Core.OrderedCharms.Count}/{Core.MaxOrders}";

                if (Core.OrderTerminalButton.IsMouseOnMe() && UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (Core.OrderedCharms.Count < Core.MaxOrders && !Core.OrderedCharms.Contains((int)val3.identifier))
                    {
                        Core.OrderedCharms.Add((int)val3.identifier);
                        Sound.Play("SoundStoreBuy", 1f, 1f);
                        Core.Logger.LogInfo($"[PATCH] Charm '{val3.identifier}' ordered for free. Queue now {Core.OrderedCharms.Count}/{Core.MaxOrders}.");
                    }
                    else
                    {
                        Sound.Play("SoundMenuError", 1f, 1f);
                    }
                }
            }
            catch (Exception ex)
            {
                Core.Logger.LogError($"Error in TerminalScript_Update_Patch: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }

    [HarmonyPatch(typeof(global::StoreCapsuleScript), "Restock")]
    public static class StoreCapsuleScript_Restock_Patch
    {
        private static bool Prefix(ref global::PowerupScript[] predeterminedPowerups, bool isFirstRestockOfDeadline)
        {
            if (Core.OrderedCharms.Count > 0)
            {
                if (predeterminedPowerups == null)
                {
                    predeterminedPowerups = new global::PowerupScript[4];
                }

                int slot = 0;
                foreach (var identifierInt in Core.OrderedCharms)
                {
                    while (slot < predeterminedPowerups.Length && (Object)(object)predeterminedPowerups[slot] != (Object)null)
                    {
                        slot++;
                    }
                    if (slot >= predeterminedPowerups.Length)
                    {
                        break;
                    }

                    var identifier = (global::PowerupScript.Identifier)identifierInt;
                    global::PowerupScript powerup_Quick = global::PowerupScript.GetPowerup_Quick(identifier);
                    if ((Object)(object)powerup_Quick != (Object)null && !global::PowerupScript.IsEquipped_Quick(identifier) && !global::PowerupScript.IsInDrawer_Quick(identifier))
                    {
                        predeterminedPowerups[slot] = powerup_Quick;
                        slot++;
                    }
                }
                Core.OrderedCharms.Clear();
            }
            return true;
        }
    }
}