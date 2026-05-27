using BaseLib.Abstracts;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace LexNinja2.LexNinja2Code.Powers;

public class WildSnakeGodPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    protected override object InitInternalData() => new Data();

    public override string CustomPackedIconPath => "WildSnakeGodPower.png".PowerImagePath();
    public override string? CustomBigIconPath => "WildSnakeGodPower.png".BigPowerImagePath();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (base.Applier?.Player == null)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Owner != base.Applier.Player)
        {
            return Task.CompletedTask;
        }
        GetInternalData<Data>().AmountsForPlayedCards.Add(cardPlay.Card);
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (
            GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card)
            && cardPlay.Card.Owner == Owner.Player
        )
        {
            Flash();
            NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
            foreach (
                var card in PileType
                    .Hand.GetPile(Owner.Player!)
                    .Cards.Where(c => !c.EnergyCost.CostsX)
            )
            {
                if (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0)
                    continue;
                card.EnergyCost.SetThisCombat(
                    Owner.Player!.RunState.Rng.CombatEnergyCosts.NextInt(4)
                );
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
    }

    private class Data
    {
        public readonly List<CardModel> AmountsForPlayedCards = new();
    }
}
