using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace LexNinja2.LexNinja2Code.Cards;

public class DarknessSnakeHand()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    private int _testEnergyCostOverride = -1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(5), new LexKelaVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DarknessSnakeHand.mp3");
        await PowerCmd.Apply<Lexkela>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars["Kela"].BaseValue,
            Owner.Creature,
            this
        );
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        foreach (
            CardModel card in PileType
                .Hand.GetPile(Owner)
                .Cards.Where<CardModel>((Func<CardModel, bool>)(c => !c.EnergyCost.CostsX))
        )
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) >= 0)
            {
                card.EnergyCost.SetThisCombat(NextEnergyCost());
                NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
            }
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Kela"].UpgradeValueBy(1);
    }

    public int TestEnergyCostOverride
    {
        get => this._testEnergyCostOverride;
        set
        {
            TestMode.AssertOn();
            this.AssertMutable();
            this._testEnergyCostOverride = value;
        }
    }

    private int NextEnergyCost()
    {
        return this.TestEnergyCostOverride >= 0
            ? this.TestEnergyCostOverride
            : this.Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }

    public override string CustomPortraitPath => $"DarknessSnakeHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarknessSnakeHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarknessSnakeHand.png".CardImagePath();
}
