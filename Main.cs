using System;
using BepInEx;
using HarmonyLib;


namespace DataExtractor
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class DataExtractorPlugin : BaseUnityPlugin
    {
        public const string pluginGuid = "aproposmath-stationeers-data-extractor"; // Change this to your own unique Mod ID
        public const string pluginName = "DataExtractor";
        public const string pluginVersion = VersionInfo.Version;

        private void Awake()
        {
            try
            {
                Logger.LogInfo(
                    $"Awake ${pluginName} {VersionInfo.VersionGit}, build time {VersionInfo.BuildTime}"
                );

                StationpediaExporter.Logger = Logger;
                var harmony = new Harmony(pluginGuid);
                harmony.PatchAll();
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error during ${pluginName} {VersionInfo.VersionGit} init: {ex}");
            }
        }
    }
}
