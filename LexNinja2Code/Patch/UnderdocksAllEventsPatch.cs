using HarmonyLib;
using LexNinja2.LexNinja2Code.Event;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Acts;

namespace LexNinja2.LexNinja2Code.Patch;

[HarmonyPatch(typeof(Underdocks), nameof(Underdocks.AllEvents), MethodType.Getter)]
public static class UnderdocksAllEventsPatch
{
    static void Postfix(ref IEnumerable<EventModel> result)
    {
        result = result.Concat([ModelDb.Event<TheSpectre>()]).Distinct();
    }
}
