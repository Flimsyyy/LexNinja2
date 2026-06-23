using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Api.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace LexNinja2.LexNinja2Code.Powers;

public class WildSnakeGodPower : LexNinja2Power
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override object InitInternalData() => new Data();

    public override string CustomIconPath => "WildSnakeGodPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "WildSnakeGodPower.png".BigPowerImagePath();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner.Player)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card, Amount);
        return Task.CompletedTask;
    }

    public override Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            cardPlay.Card.Owner != Owner.Player
            || !GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var amount)
        )
        {
            return Task.CompletedTask;
        }
        Flash();
        NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
        var cards = PileType
            .Hand.GetPile(Owner.Player!)
            .Cards.Where(c => !c.EnergyCost.CostsX)
            .Where(card => card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            .ToList();
        for (var i = 0; i < amount; i++)
        {
            foreach (var card in cards)
            {
                card.EnergyCost.SetThisCombat(
                    Owner.Player!.RunState.Rng.CombatEnergyCosts.NextInt(4)
                );
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
        return Task.CompletedTask;
    }

    private class Data
    {
        public readonly Dictionary<CardModel, int> AmountsForPlayedCards = new();
    }
}
