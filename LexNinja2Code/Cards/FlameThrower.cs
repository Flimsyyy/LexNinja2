using BaseLib.Extensions;
using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace LexNinja2.LexNinja2Code.Cards;

public class FlameThrower()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [
            new DamageVar(6, ValueProp.Move),
            new PowerVar<PoisonPower>(5),
            new PowerVar<WeakPower>(1),
            new NinjutsuVar(1),
        ];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
        [
            HoverTipFactory.FromPower<WeakPower>(),
            HoverTipFactory.FromPower<PoisonPower>(),
            HoverTipFactory.FromPower<Lexkela>(),
        ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/FlameThrower.mp3");
        NCombatRoom instance = NCombatRoom.Instance;
        if (instance != null)
            instance.CombatVfxContainer.AddChildSafely((Node)NGroundFireVfx.Create(play.Target));
        await DamageCmd
            .Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(play.Target)
            .Execute(choiceContext);
        await PowerCmd.Apply<WeakPower>(
            new ThrowingPlayerChoiceContext(),
            play.Target,
            DynamicVars.Power<WeakPower>().BaseValue,
            Owner.Creature,
            this
        );
        if (Ninjutsu())
        {
            await PowerCmd.Apply<PoisonPower>(
                new ThrowingPlayerChoiceContext(),
                play.Target,
                DynamicVars.Poison.BaseValue,
                Owner.Creature,
                this
            );
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
        DynamicVars.Damage.UpgradeValueBy(3);
        DynamicVars.Power<WeakPower>().UpgradeValueBy(1);
    }

    public override string CustomPortraitPath => $"FlameThrower_p.png".BigCardImagePath();
    public override string PortraitPath => $"FlameThrower.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/FlameThrower.png".CardImagePath();

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
}
