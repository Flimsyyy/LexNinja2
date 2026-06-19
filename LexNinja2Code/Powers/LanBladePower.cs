using BaseLib.Abstracts;
using BaseLib.Extensions;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Cards.Tokens;
using LexNinja2.LexNinja2Code.Cards.Uncommons;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace LexNinja2.LexNinja2Code.Powers;

public class LanBladePower : CustomPowerModel, IHasSecondAmount
{
    private const string Base = "LanBlade";
    private const string Upgraded = "LanBladeUpgraded";

    protected override object InitInternalData() => new Data();

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromKeyword(NinjaKeyword.Blade), HoverTipFactory.FromCard<LanBlade>()];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new(Base, 0), new(Upgraded, 0)];
    public override int DisplayAmount => DynamicVars[Base].IntValue;

    public override string CustomPackedIconPath => "LanBladePower32.png".PowerImagePath();
    public override string? CustomBigIconPath => "LanBladePower84.png".BigPowerImagePath();

    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        var cardPlayed = cardPlay.Card;
        if (!IsTargetCard(cardPlayed))
            return Task.CompletedTask;

        GetInternalData<Data>()
            .AmountsForPlayedCards.Add(
                cardPlay.Card,
                (DynamicVars[Base].IntValue, DynamicVars[Upgraded].IntValue)
            );
        return Task.CompletedTask;
    }

    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        var cardPlayed = cardPlay.Card;
        if (!IsTargetCard(cardPlayed))
            return;
        if (!GetInternalData<Data>().AmountsForPlayedCards.Remove(cardPlay.Card, out var amounts))
        {
            return;
        }
        var baseAmount = amounts.Item1;
        var upgradedAmount = amounts.Item2;
        NinjaAudio.Play("res://LexNinja2/audio/LanBlade.mp3");
        List<CardModel> cards = [];
        for (var i = 0; i < upgradedAmount; i++)
        {
            var card = CombatState.CreateCard<LanBlade>(Owner.Player);
            CardCmd.Upgrade(card);
            cards.Add(card);
        }
        for (var i = 0; i < baseAmount; i++)
        {
            var card = CombatState.CreateCard<LanBlade>(Owner.Player);
            cards.Add(card);
        }
        await CardPileCmd.AddGeneratedCardsToCombat(cards, PileType.Hand, Owner.Player);
    }

    public void UpgradeBaseLanBladeValue(decimal addend)
    {
        NinjaHelper.UpgradeDynamicVarValue(DynamicVars[Base], addend);
        InvokeDisplayAmountChanged();
    }

    public void UpgradeUpgradedLanBladeValue(decimal addend)
    {
        NinjaHelper.UpgradeDynamicVarValue(DynamicVars[Upgraded], addend);
        this.InvokeSecondAmountChanged();
    }

    public override Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource
    )
    {
        if (power != this)
        {
            return Task.CompletedTask;
        }
        if (cardSource is LanBladeCutting)
        {
            if (cardSource.IsUpgraded)
            {
                UpgradeUpgradedLanBladeValue(1);
            }
            else
            {
                UpgradeBaseLanBladeValue(1);
            }
            return Task.CompletedTask;
        }

        var ratio = amount / (Amount - amount);
        UpgradeBaseLanBladeValue(ratio * DynamicVars[Base].IntValue);
        UpgradeUpgradedLanBladeValue(ratio * DynamicVars[Upgraded].IntValue);
        return Task.CompletedTask;
    }

    private bool IsTargetCard(CardModel card)
    {
        return card.Owner == Owner.Player
            && card.Owner == Owner.Player
            && NinjaHelper.IsBladeRenShu(card)
            && card is not LanBlade;
    }

    private class Data
    {
        public readonly Dictionary<CardModel, (int, int)> AmountsForPlayedCards = new();
    }

    public string GetSecondAmount()
    {
        return $"{DynamicVars[Upgraded].IntValue}";
    }
}
