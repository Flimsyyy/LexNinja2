using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class LanBladePower : CustomPowerModel
{
    protected override object InitInternalData() => (object)new Data();

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Blade), HoverTipFactory.FromCard<LanBlade>()];

    public override string CustomPackedIconPath => "LanBladePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "LanBladePower84.png".BigPowerImagePath();

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var value)
            && (
                (
                    cardPlay.Card.Keywords.Contains(NinjaKeyword.Blade)
                    && !cardPlay.Card.Tags.Contains(NinjaTags.LanBlade)
                ) || cardPlay.Card.Tags.Contains(CardTag.Shiv)
            )
            && cardPlay.Card.Owner == Owner.Player
        )
        {
            NinjaAudio.Play("res://LexNinja2/audio/LanBlade.mp3");
            for (var i = 0; i < Amount; i++)
            {
                CardModel card = CombatState.CreateCard<LanBlade>(Owner.Player);
                await CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner.Player);
            }
        }
    }

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, base.Amount);
        return Task.CompletedTask;
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
