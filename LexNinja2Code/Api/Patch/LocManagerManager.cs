using HarmonyLib;
using LexNinja2.LexNinja2Code.Api.Formatters;
using MegaCrit.Sts2.Core.Localization;
using SmartFormat;

namespace LexNinja2.LexNinja2Code.Api.Patch;

[HarmonyPatch(typeof(LocManager), "LoadLocFormatters")]
public static class LocManagerManager
{
    public static void Postfix()
    {
        var formatter =
            AccessTools.Field(typeof(LocManager), "_smartFormatter").GetValue(null)
            as SmartFormatter;
        formatter?.AddExtensions(new RenShuFormatter());
    }
}
