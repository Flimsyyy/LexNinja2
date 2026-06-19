using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace LexNinja2.LexNinja2Code.Api.Hooks;

public class NinjaHooks
{
    public static decimal ModifyLexKelaCost(
        IRunState runState,
        ICombatState combatState,
        CardModel card,
        decimal originalCost
    )
    {
        if (originalCost < 0M)
            return originalCost;
        var modifiedCost = originalCost;
        foreach (
            var item in BetaMainCompatibility.RunState.IterateHookListeners.Invoke<
                IEnumerable<AbstractModel>
            >(runState, combatState) ?? []
        )
        {
            var mod = item as ITryModifyLexKelaCost;
            mod?.TryModifyLexKeLaCost(card, originalCost, out modifiedCost);
        }
        return modifiedCost;
    }

    public static async Task AfterLexKelaSpent(
        PlayerChoiceContext choiceContext,
        IRunState runState,
        ICombatState combatState,
        int amount,
        Player spender
    )
    {
        foreach (
            var item in BetaMainCompatibility.RunState.IterateHookListeners.Invoke<
                IEnumerable<AbstractModel>
            >(runState, combatState) ?? []
        )
        {
            if (item is IAfterLexKelaSpent mod)
            {
                await mod.AfterLexKelaSpent(choiceContext, amount, spender);
            }
            item.InvokeExecutionFinished();
        }
    }
}
