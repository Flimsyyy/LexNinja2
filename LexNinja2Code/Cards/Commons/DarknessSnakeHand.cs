using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Cards;

namespace LexNinja2.LexNinja2Code.Cards.Commons;

public class DarknessSnakeHand()
    : LexNinja2Card(0, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    // private int _testEnergyCostOverride = -1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(5), new LexKelaVar(1)];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, NinjaKeyword.Hand];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/DarknessSnakeHand.mp3");
        await LexKela.Gain(this);
        await CommonActions.Draw(this, choiceContext);
        foreach (var card in PileType.Hand.GetPile(Owner).Cards.Where(c => !c.EnergyCost.CostsX))
        {
            if (card.EnergyCost.GetWithModifiers(CostModifiers.None) < 0)
                continue;
            card.EnergyCost.SetThisCombat(NextEnergyCost());
            NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.LexKela().UpgradeValueBy(1);
    }

    private int NextEnergyCost()
    {
        return Owner.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }

    public override string CustomPortraitPath => $"DarknessSnakeHand_p.png".BigCardImagePath();
    public override string PortraitPath => $"DarknessSnakeHand.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/DarknessSnakeHand.png".CardImagePath();
}
