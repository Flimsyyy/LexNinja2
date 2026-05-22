using BaseLib.Utils;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.Cards;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.TestSupport;

namespace LexNinja2.LexNinja2Code.Cards.Uncommons;

public class WildSnakeGod() : LexNinja2Card(0, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(1), new PowerVar<WildSnakeGodPower>(2)];

    // protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<AngrySnakeBite>(),HoverTipFactory.Static(StaticHoverTip.Block)];
    // protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
        await CommonActions.ApplySelf<WildSnakeGodPower>(choiceContext, this);
        // if (Ninjutsu())
        // {
        //     for (int i = 0; i < DynamicVars.Cards.BaseValue; i++)
        //     {
        //         CardModel card = CombatState.CreateCard<AngrySnakeBite>(Owner);
        //         await CardPileCmd.AddGeneratedCardToCombat(card,PileType.Hand,true);
        //     }
        //
        // }
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }

    public override string CustomPortraitPath => $"WildSnakeGod.png".BigCardImagePath();
    public override string PortraitPath => $"WildSnakeGod.png".CardImagePath();

    public override Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw
    )
    {
        if (!fromHandDraw)
        {
            return Task.CompletedTask;
        }
        EnergyCost.SetThisCombat(NextEnergyCost());
        NCard.FindOnTable(card)?.PlayRandomizeCostAnim();
        return Task.CompletedTask;
    }

    private int _testEnergyCostOverride = -1;

    public int TestEnergyCostOverride
    {
        get => _testEnergyCostOverride;
        set
        {
            TestMode.AssertOn();
            AssertMutable();
            _testEnergyCostOverride = value;
        }
    }

    private int NextEnergyCost()
    {
        return TestEnergyCostOverride >= 0
            ? TestEnergyCostOverride
            : Owner!.RunState.Rng.CombatEnergyCosts.NextInt(4);
    }
}
