using HarmonyLib;
using Assets.Scripts.UI;
using Assets.Scripts;

namespace DataExtractor
{
    [HarmonyPatch]
    public static class Patch_StationpediaExporter
    {
        [HarmonyPatch(typeof(NetworkServer), nameof(NetworkServer.Host)), HarmonyPostfix]
        public static void Postfix()
        {
            Stationpedia.Regenerate();
            StationpediaExporter.Execute();
            XMLSchemaExporter.ExportSchema();
            NotepadPlusLanguageExporter.Export();
            StationpediaExporter.Logger.LogInfo($"Data export completed, quitting now.");
            UnityEngine.Application.Quit();
        }
    }
}
