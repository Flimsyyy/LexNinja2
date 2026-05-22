using System.Reflection.Emit;
using HarmonyLib;
using LexNinja2.LexNinja2Code.Api.Cards;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Api.Patch;

[HarmonyPatch(typeof(CardModel))]
public static class CardModelPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(CardModel.SetToFreeThisTurn))]
    public static void SetToFreeThisTurnPostfix(CardModel __instance)
    {
        if (__instance is LexNinja2Card ninjaCard)
        {
            ninjaCard.SetLexKelaToFreeThisTurn();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CardModel.SetToFreeThisCombat))]
    public static void SetToFreeThisCombatPostfix(CardModel __instance)
    {
        if (__instance is LexNinja2Card ninjaCard)
        {
            ninjaCard.SetLexKelaToFreeThisCombat();
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CardModel.EndOfTurnCleanup))]
    public static void EndOfTurnCleanupPostfix(CardModel __instance)
    {
        if (__instance is not LexNinja2Card ninjaCard)
            return;
        if (ninjaCard.TemporaryLexKelaCost == null)
        {
            return;
        }
        ninjaCard.ClearsLexKelaWhenTurnEnds();
    }
}
