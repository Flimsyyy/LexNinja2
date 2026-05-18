using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace LexNinja2.LexNinja2Code.Cards;

public class WildSnakeGod() : LexNinja2Card(2, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new CardsVar(2), new NinjutsuVar(1)];

    // protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromCard<AngrySnakeBite>(),HoverTipFactory.Static(StaticHoverTip.Block)];
    // protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/WildSnakeGod.mp3");
        await PowerCmd.Apply<WildSnakeGodPower>(
            new ThrowingPlayerChoiceContext(),
            Owner.Creature,
            DynamicVars.Cards.BaseValue,
            Owner.Creature,
            this
        );
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
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"WildSnakeGod.png".BigCardImagePath();
    public override string PortraitPath => $"WildSnakeGod.png".CardImagePath();
    // public override string BetaPortraitPath => $"beta/DefendNinja.png".CardImagePath();
    // private Boolean Ninjutsu()
    // {
    //     if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
    //     {
    //         return true;
    //     }
    //     if (Owner.Creature.GetPower<Lexkela>() != null)
    //     {
    //         if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
    //         {
    //             PowerCmd.Apply<Lexkela>(new ThrowingPlayerChoiceContext(), Owner.Creature,-DynamicVars["Renshu"].BaseValue, Owner.Creature, this);
    //             return true;
    //         }
    //     }
    //     return false;
    // }
    //
    // private Boolean CanCastNinjutsu()
    // {
    //     if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
    //     {
    //         return true;
    //     }
    //
    //     if (Owner.Creature.GetPower<Lexkela>() != null)
    //     {
    //         if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
    //         {
    //             return true;
    //         }
    //     }
    //
    //     return false;
    // }
    // protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
