using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class BurningBlade()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DamageVar(6, ValueProp.Move), new NinjutsuVar(1)];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [NinjaKeyword.Blade];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        // bool shouldTriggerFatal = play.Target.Powers.All<PowerModel>((Func<PowerModel, bool>) (p => p.ShouldOwnerDeathTriggerFatal()));
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely((Node)NGroundFireVfx.Create(play.Target));

        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .Execute(choiceContext);
        // if (!shouldTriggerFatal || !attackCommand.Results.Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))

        if (Ninjutsu())
        {
            if (Owner.PlayerCombatState.AllCards.OfType<BurningBlade>() == null)
            {
                return;
            }
            foreach (CardModel card in this.Owner.PlayerCombatState.AllCards.OfType<BurningBlade>())
            {
                ++card.BaseReplayCount;
            }
        }
    }

    private Boolean Ninjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }
        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                PowerCmd.Apply<Lexkela>(
                    new ThrowingPlayerChoiceContext(),
                    Owner.Creature,
                    -DynamicVars["Renshu"].BaseValue,
                    Owner.Creature,
                    this
                );
                return true;
            }
        }
        return false;
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2);
    }

    public override string CustomPortraitPath => "OverBurningBlade_p.png".BigCardImagePath();
    public override string PortraitPath => "OverBurningBlade.png".CardImagePath();
    public override string BetaPortraitPath => "beta/OverBurningBlade.png".CardImagePath();
    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();

    private Boolean CanCastNinjutsu()
    {
        if (Owner.Creature.GetPower<FreeNinjutsuPower>() != null)
        {
            return true;
        }

        if (Owner.Creature.GetPower<Lexkela>() != null)
        {
            if (Owner.Creature.GetPower<Lexkela>().Amount >= DynamicVars["Renshu"].BaseValue)
            {
                return true;
            }
        }

        return false;
    }

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource
    )
    {
        if (dealer != Owner.Creature || cardSource == null)
        {
            return;
        }
        if (cardSource == this)
        {
            NinjaAudio.Play("res://LexNinja2/audio/OverBurningBlade.mp3");
        }
        else
        {
            return;
        }
    }
}
