using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class GetAllHandsPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override string CustomPackedIconPath => "GetAllHandsPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "GetAllHandsPower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Owner != Owner.Player
            || !GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var amount)
        )
        {
            return;
        }
        if (!NinjaHelper.IsHandRenShu(cardPlay.Card))
        {
            await PowerCmd.Remove(this);
            return;
        }
        for (var i = 0; i < amount; i++)
        {
            await CardPileCmd.Draw(context, 1, Owner.Player);
            await PlayerCmd.GainEnergy(1, Owner.Player);
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> creatures
    )
    {
        if (side != Owner.Side)
        {
            return;
        }
        await PowerCmd.Remove(this);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
