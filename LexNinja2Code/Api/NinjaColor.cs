using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using STS2RitsuLib.Cards.FreePlay;
using STS2RitsuLib.Combat.SecondaryResources;

namespace LexNinja2.LexNinja2Code.Api;

public class NinjaColor
{
    public static CardCostColor GetLexKelaCostColor(NinjutsuCard card)
    {
        if (!card.IsMutable || card.HasLexKelaCostX)
        {
            return CardCostColor.Unmodified;
        }

        var combatState = card.CombatState;
        var ninjutsu = card.DynamicVars.Ninjutsu();
        if (ninjutsu.WasJustUpgraded)
        {
            return CardCostColor.Decreased;
        }
        if (card.Owner == null)
        {
            return CardCostColor.Unmodified;
        }
        var runState = card.Owner.RunState;
        if (runState == null || combatState == null)
        {
            return CardCostColor.Unmodified;
        }
        var plan = SecondaryResourcePaymentResolver.Plan(
            card,
            FreePlayBindingRegistry.IsCardFreeForUpcomingPlay(card)
        );
        var line = plan.Lines.FirstOrDefault(line => line.Definition.Id == LexKela.Id);
        if (line == null)
        {
            return CardCostColor.Unmodified;
        }
        if (!line.IsAffordable)
        {
            return CardCostColor.InsufficientResources;
        }
        var baseSpend = card.DynamicVars.Ninjutsu().BaseValue;
        var nowSpend = line.Cost;
        if (nowSpend > baseSpend)
        {
            return CardCostColor.Increased;
        }
        return nowSpend == baseSpend ? CardCostColor.Unmodified : CardCostColor.Decreased;
    }
}
