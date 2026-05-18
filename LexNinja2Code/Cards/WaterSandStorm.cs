using Godot;
using LexNinja2.LexNinja2Code.Api;
using LexNinja2.LexNinja2Code.Api.DynamicVars;
using LexNinja2.LexNinja2Code.Api.Extensions;
using LexNinja2.LexNinja2Code.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;

namespace LexNinja2.LexNinja2Code.Cards;

public class WaterSandStorm()
    : LexNinja2Card(1, CardType.Attack, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new NinjutsuVar(2), new PowerVar<SandWall>(10)];
    protected override HashSet<CardTag> CanonicalTags => [NinjaTags.Ninjutsu];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        NinjaAudio.Play("res://LexNinja2/audio/WaterSandStorm.mp3");
        if (Ninjutsu())
        {
            await PowerCmd.Apply<SandWall>(
                new ThrowingPlayerChoiceContext(),
                Owner.Creature,
                DynamicVars["SandWall"].BaseValue,
                Owner.Creature,
                this
            );
        }

        if (Owner.Creature.GetPower<SandWall>() != null)
        {
            NRollingBoulderVfx nRollingBoulderVfx = NRollingBoulderVfx.Create(
                base.CombatState.HittableEnemies,
                Owner.Creature.GetPower<SandWall>().Amount
            );
            nRollingBoulderVfx.Connect(
                NRollingBoulderVfx.SignalName.HitCreature,
                Callable.From(
                    delegate(NCreature c)
                    {
                        DamageCmd
                            .Attack(Owner.Creature.GetPower<SandWall>().Amount)
                            .FromCard(this)
                            .TargetingAllOpponents(CombatState)
                            .WithHitFx(tmpSfx: "blunt_attack.mp3")
                            .Execute(choiceContext);
                    }
                )
            );
            SignalAwaiter signalAwaiter = nRollingBoulderVfx.ToSignal(
                nRollingBoulderVfx,
                NRollingBoulderVfx.SignalName.Finished
            );
            NCombatRoom.Instance?.CombatVfxContainer.CallDeferred(
                Node.MethodName.AddChild,
                nRollingBoulderVfx
            );
            await signalAwaiter;
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["SandWall"].UpgradeValueBy(3);
    }

    public override string CustomPortraitPath => $"WaterSandStorm_p.png".BigCardImagePath();
    public override string PortraitPath => $"WaterSandStorm.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/WaterSandStorm.png".CardImagePath();

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
