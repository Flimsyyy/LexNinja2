using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class TakeYourHeart()
    : LexNinja2Card(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(10), new DamageVar(0, ValueProp.Move), new("Amount", 2)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [HoverTipFactory.FromPower<ArtifactPower>()];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, NinjaKeyword.Hand];
    public override CardMultiplayerConstraint MultiplayerConstraint =>
        CardMultiplayerConstraint.SingleplayerOnly;
    private int debuffAmount = 0;

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (IsEnoughDebuff(play.Target) && Ninjutsu())
        {
            NinjaAudio.Play("res://LexNinja2/audio/TakeYourHeart.mp3", 0.25f);
            NCombatRoom instance = NCombatRoom.Instance;
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            for (int i = 0; i < 3; i++)
            {
                if (instance != null)
                    instance.CombatVfxContainer.AddChildSafely(
                        (Node)NGroundFireVfx.Create(Owner.Creature, VfxColor.Purple)
                    );
            }
            await MegaCrit.Sts2.Core.Commands.Cmd.Wait(1F);
            if (LocalContext.IsMe(Owner))
                VfxCmd.PlayFullScreenInCombat("vfx/vfx_adrenaline", Owner.Creature);

            await DamageCmd
                .Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(play.Target)
                .WithHitFx("vfx/vfx_molten_fist", tmpSfx: "blunt_attack.mp3")
                .Execute(choiceContext);
            await CreatureCmd.Kill(play.Target, true);
            return;
        }
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3")
            .Execute(choiceContext);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }

    public override string CustomPortraitPath => $"TakeYourHeart_p.png".BigCardImagePath();
    public override string PortraitPath => $"TakeYourHeart.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/TakeYourHeart.png".CardImagePath();

    private bool IsEnoughDebuff(Creature target)
    {
        List<PowerModel> originalDebuffs = (
            from p in target.Powers
            where p.TypeForCurrentAmount == PowerType.Debuff
            select (PowerModel)p.ClonePreservingMutability()
        ).ToList();
        if (originalDebuffs != null)
        {
            foreach (PowerModel p in originalDebuffs)
            {
                debuffAmount += p.Amount;
            }
        }
        if (debuffAmount >= DynamicVars["Amount"].IntValue)
        {
            return true;
        }
        return false;
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

    protected override bool ShouldGlowGoldInternal => CanCastNinjutsu();
}
