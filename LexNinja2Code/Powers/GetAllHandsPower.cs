using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class GetAllHandsPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override object InitInternalData() => (object)new Data();

    public override string CustomPackedIconPath => "GetAllHandsPower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "GetAllHandsPower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var value)
            && cardPlay.Card.Owner == Owner.Player
        )
        {
            if (!cardPlay.Card.Keywords.Contains(NinjaKeyword.Hand))
            {
                await PowerCmd.Remove(this);
                return;
            }
            await CardPileCmd.Draw(context, 1, Owner.Player);
            await PlayerCmd.GainEnergy(1, Owner.Player);
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
        return Task.CompletedTask;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side != this.Owner.Side)
        {
            return;
        }

        await PowerCmd.Remove(this);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards =
            new Dictionary<CardModel, int>();
    }
}
