using BaseLib.Abstracts;
using Godot;
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

    protected override object InitInternalData() => (object)new Data();

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Science), HoverTipFactory.FromPower<Lexkela>()];

    public override string CustomPackedIconPath => "TurbinePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "TurbinePower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            GetInternalData<Data>().amountsForPlayedCards.Remove(cardPlay.Card, out var value)
            && cardPlay.Card.Owner == Owner.Player
            && cardPlay.Card.Keywords.Contains(NinjaKeyword.Science)
        )
        {
            Flash();
            await CardPileCmd.Draw(context, Amount, Owner.Player);
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        GD.Print("进入函数");
        GetInternalData<Data>().amountsForPlayedCards.Add(cardPlay.Card, base.Amount);
        GD.Print("GetInternalData<Data>() amountsForPlayedCards:" + base.Amount);
        return Task.CompletedTask;
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> amountsForPlayedCards =
            new Dictionary<CardModel, int>();
    }
}
