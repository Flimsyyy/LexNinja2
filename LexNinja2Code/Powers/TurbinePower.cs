using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class TurbinePower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Science), HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "TurbinePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "TurbinePower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (!IsTargetCard(cardPlay.Card))
        {
            return;
        }
        if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var amount))
        {
            return;
        }

        Flash();
        await CardPileCmd.Draw(context, amount, Owner.Player!);
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (!IsTargetCard(cardPlay.Card))
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    private bool IsTargetCard(CardModel card)
    {
        return card.Owner == Owner.Player && card.Keywords.Contains(NinjaKeyword.Science);
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
