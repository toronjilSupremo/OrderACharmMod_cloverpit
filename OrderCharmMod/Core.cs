using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Panik;
using TMPro;
using UnityEngine;

namespace OrderCharmMod;

[BepInPlugin(pluginGuid, pluginName, pluginVersion)]
[BepInProcess("CloverPit.exe")]
public class Core : BaseUnityPlugin
{
    private const string pluginGuid = "com.zix.ordercarmmod";
    private const string pluginName = "OrderCharmMod";
    private const string pluginVersion = "1.0.0";

    public static new ManualLogSource Logger { get; private set; }

    internal const int MaxOrders = 4;

    internal static System.Collections.Generic.List<int> OrderedCharms = new System.Collections.Generic.List<int>();

    internal static GameObject OrderButton;

    internal static global::TerminalButton OrderTerminalButton;

    public static GameObject OrderTextObject;

    public static TextMeshProUGUI OrderCostText;

    private void Awake()
    {
        Logger = base.Logger;

        try
        {
            var terminalScriptType = typeof(TerminalScript);
            Harmony harmony = new Harmony(pluginGuid);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            Logger.LogInfo("=== INITIALIZATION COMPLETE OderACharmMod ===");
        }
        catch (Exception ex)
        {
            Logger.LogError($"✗ FATAL ERROR: {ex.Message}");
            Logger.LogError($"StackTrace: {ex.StackTrace}");
        }
    }
}
