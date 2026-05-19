using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class PlasmaArrowPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override string CustomPackedIconPath => "power.png".PowerImagePath();
    public override string? CustomBigIconPath => "power.png".BigPowerImagePath();

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

    public override async Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Keywords.Contains(NinjaKeyword.FreeNinjutsu))
        {
            return;
        }
        if (
            cardPlay.Card.Owner.Creature != Owner
            || !cardPlay.Card.Tags.Contains(NinjaTags.Ninjutsu)
        )
        {
            return;
        }

        var pileType = cardPlay.Card.Pile?.Type;
        if (pileType is not (PileType.Hand or PileType.Play))
            return;
        await PowerCmd.Apply<FreeNinjutsuPower>(
            new ThrowingPlayerChoiceContext(),
            Owner,
            1,
            Owner,
            null
        );
        await PowerCmd.Decrement(this);
    }
}
