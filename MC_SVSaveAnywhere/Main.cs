using BepInEx;
using HarmonyLib;

namespace MC_SVSaveAnywhere
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string pluginGuid = "mc.starvalor.saveanywhere";
        public const string pluginName = "SV Save Anywhere";
        public const string pluginVersion = "1.0.0";

        private static bool manualSave = false;
        private static bool saveGameEntry = false;

        private void Awake()
        {
            Harmony.CreateAndPatchAll(typeof(Main));
        }

        [HarmonyPatch(typeof(MenuControl), nameof(MenuControl.PrepareSaveGame))]
        [HarmonyPrefix]
        private static void MenuControlPrepareSaveGame_Pre(bool manualSave)
        {
            Main.manualSave = manualSave;
        }

        [HarmonyPatch(typeof(MenuControl), nameof(MenuControl.SaveGame))]
        [HarmonyPrefix]
        private static void MenuControlSaveGame_Pre()
        {
            if (manualSave)
                saveGameEntry = true;
        }

        [HarmonyPatch(typeof(GameManager), nameof(GameManager.CheckPlayerSafe))]
        [HarmonyPostfix]
        private static void GameManagerCheckPlayerSafe_Post(ref bool __result)
        {
            if (manualSave)
            {
                if (saveGameEntry)
                {
                    manualSave = false;
                    saveGameEntry = false;
                }

                __result = true;
            }
        }
    }
}
