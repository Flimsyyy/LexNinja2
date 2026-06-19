using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class PlasmaArrowPower : CustomPowerModel, ITryModifyLexKelaCost, IAfterLexKelaSpent
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "PlasmaArrowPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "PlasmaArrowPower.png".BigPowerImagePath();

    public override bool TryModifyEnergyCostInCombat(
        CardModel card,
        decimal originalCost,
        out decimal modifiedCost
    )
    {
        modifiedCost = originalCost;
        if (card.Owner.Creature != Owner)
        {
            return false;
        }
        if (!card.Tags.Contains(NinjaTags.Ninjutsu))
        {
            return false;
        }
        var pileType = card.Pile?.Type;
        if (pileType is not (PileType.Hand or PileType.Play))
            return false;
        modifiedCost = 0;
        return true;
    }

    public bool TryModifyLexKeLaCost(CardModel card, decimal originalCost, out decimal modifiedCost)
    {
        if (card.Owner.Creature != Owner)
        {
            modifiedCost = originalCost;
            return false;
        }
        modifiedCost = 0;
        return true;
    }

    public async Task AfterLexKelaSpent(
        PlayerChoiceContext choiceContext,
        int amount,
        Player spender
    )
    {
        if (spender.Creature != Owner)
        {
            return;
        }
        if (amount > 0)
        {
            return;
        }
        await PowerCmd.Decrement(this);
    }
}
