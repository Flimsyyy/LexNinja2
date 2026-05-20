using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Hooks;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace LexNinja2.LexNinja2Code.Api;

public class NinjaColor
{
    public static CardCostColor GetLexKelaCostColor(LexNinja2Card card)
    {
        if (!card.IsMutable)
        {
            return CardCostColor.Unmodified;
        }
        var combatState = card.CombatState;
        if (card.Owner == null)
        {
            return CardCostColor.Unmodified;
        }
        var runState = card.Owner.RunState;
        if (runState == null || combatState == null)
        {
            return CardCostColor.Unmodified;
        }
        if (card.GetLexKelaCostWithModifiers() > card.GetLexKelaAmount())
        {
            return CardCostColor.InsufficientResources;
        }
        if (card.HasStarCostX)
            return CardCostColor.Unmodified;
        if (TryModifyLexNinjaCostWithHooks(card, runState, combatState, out var hookModifiedCost))
        {
            return GetColorForHookModifiedCost(hookModifiedCost, card.GetBaseLexKelaCost());
        }
        return card.TemporaryLexKelaCost != null
            ? GetColorForLocalCost(card.TemporaryLexKelaCost.Cost, card.GetBaseLexKelaCost())
            : CardCostColor.Unmodified;
    }

    private static bool TryModifyLexNinjaCostWithHooks(
        LexNinja2Card card,
        IRunState runState,
        ICombatState combatState,
        out decimal hookModifiedCost
    )
    {
        hookModifiedCost = card.GetBaseLexKelaCost();
        var flag = false;
        foreach (
            var item in BetaMainCompatibility.RunState.IterateHookListeners.Invoke<
                IEnumerable<AbstractModel>
            >(runState, combatState) ?? []
        )
        {
            if (item is ITryModifyLexKelaCost mod)
            {
                flag |= mod.TryModifyLexKeLaCost(card, hookModifiedCost, out hookModifiedCost);
            }
        }
        return flag;
    }

    private static CardCostColor GetColorForHookModifiedCost(decimal hookModifiedCost, int baseCost)
    {
        if (hookModifiedCost > baseCost)
            return CardCostColor.Increased;
        return hookModifiedCost < baseCost ? CardCostColor.Decreased : CardCostColor.Unmodified;
    }

    private static CardCostColor GetColorForLocalCost(int localCost, int baseCost)
    {
        if (localCost > baseCost)
            return CardCostColor.Increased;
        return localCost < baseCost ? CardCostColor.Decreased : CardCostColor.Unmodified;
    }
}
